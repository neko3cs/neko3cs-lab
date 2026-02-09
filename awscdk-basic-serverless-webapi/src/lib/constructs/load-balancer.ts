import * as cdk from 'aws-cdk-lib';
import * as ec2 from 'aws-cdk-lib/aws-ec2';
import * as elbv2 from 'aws-cdk-lib/aws-elasticloadbalancingv2';

import { Construct } from 'constructs';

interface Props {
  vpc: ec2.Vpc;
  vpcSubnets: ec2.SubnetSelection;
  appPort: number;
  acmArn?: string;
}

export class LoadBalancer extends Construct {
  public readonly loadBalancer: elbv2.ApplicationLoadBalancer;
  public readonly apiTargetGroup: elbv2.ApplicationTargetGroup;
  public readonly pageTargetGroup: elbv2.ApplicationTargetGroup;

  constructor(scope: Construct, id: string, { vpc, vpcSubnets, appPort, acmArn }: Props) {
    super(scope, id);
    this.loadBalancer = new elbv2.ApplicationLoadBalancer(this, 'LoadBalancer', {
      vpc,
      internetFacing: true,
      vpcSubnets,
    });

    const appListener = this.loadBalancer.addListener(
      acmArn ? 'HttpsListener' : 'HttpListener',
      {
        port: acmArn ? 443 : 80,
        protocol: acmArn
          ? elbv2.ApplicationProtocol.HTTPS
          : elbv2.ApplicationProtocol.HTTP,
        certificates: acmArn
          ? [elbv2.ListenerCertificate.fromArn(acmArn)]
          : undefined,
        defaultAction: elbv2.ListenerAction.fixedResponse(404, {
          contentType: 'text/plain',
          messageBody: 'Not Found',
        }),
      },
    );

    if (acmArn) {
      // httpsにリダイレクトする設定
      this.loadBalancer.addListener('HttpListener', {
        port: 80,
        protocol: elbv2.ApplicationProtocol.HTTP,
        defaultAction: elbv2.ListenerAction.redirect({
          protocol: 'HTTPS',
          port: '443',
          permanent: true,
        }),
      });
    }


    this.apiTargetGroup = new elbv2.ApplicationTargetGroup(
      this,
      'ApiTargetGroup',
      {
        vpc,
        port: appPort,
        protocol: elbv2.ApplicationProtocol.HTTP,
        targetType: elbv2.TargetType.IP,
        healthCheck: {
          path: '/healthCheck',
        },
      },
    );

    this.pageTargetGroup = new elbv2.ApplicationTargetGroup(
      this,
      'PageTargetGroup',
      {
        vpc,
        port: appPort,
        protocol: elbv2.ApplicationProtocol.HTTP,
        targetType: elbv2.TargetType.IP,
        healthCheck: {
          path: '/healthCheck',
        },
      },
    );

    appListener.addTargetGroups('ApiTargetGroup', {
      priority: 100,
      conditions: [elbv2.ListenerCondition.pathPatterns(['/api/*'])],
      targetGroups: [this.apiTargetGroup],
    });
    appListener.addTargetGroups('PageTargetGroup', {
      priority: 200,
      conditions: [elbv2.ListenerCondition.pathPatterns(['/*'])],
      targetGroups: [this.pageTargetGroup],
    });

    const protocal = acmArn ? 'https' : 'http';

    new cdk.CfnOutput(this, 'OutputApplicationUrl', {
      exportName: 'ApplicationUrl',
      value: `${protocal}://${this.loadBalancer.loadBalancerDnsName}`,
    });

  }
}
