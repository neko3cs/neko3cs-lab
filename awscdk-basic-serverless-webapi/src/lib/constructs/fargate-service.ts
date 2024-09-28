import * as ec2 from 'aws-cdk-lib/aws-ec2';
import * as ecs from 'aws-cdk-lib/aws-ecs';
import { ApplicationLoadBalancedFargateService } from "aws-cdk-lib/aws-ecs-patterns";
import * as elbv2 from 'aws-cdk-lib/aws-elasticloadbalancingv2';
import * as secretsmanager from 'aws-cdk-lib/aws-secretsmanager';
import { Construct } from "constructs"
import { CfnOutput } from 'aws-cdk-lib';

interface Props {
  vpc: ec2.Vpc;
  vpcSubnets: ec2.SubnetSelection;
  securityGroup: ec2.SecurityGroup;
  dbCredential: secretsmanager.ISecret;
  databaseHost: string;
  databasePort: number;
  containerPort: number;
  apiTargetGroup: elbv2.ApplicationTargetGroup;
  pageTargetGroup: elbv2.ApplicationTargetGroup;
}

export class FargateService extends Construct {
  constructor(
    scope: Construct,
    id: string,
    {
      vpc,
      vpcSubnets,
      securityGroup,
      dbCredential,
      databaseHost,
      databasePort,
      containerPort,
    }: Props
  ) {
    super(scope, id);

    const cluster = new ecs.Cluster(this, 'Cluster', {
      vpc,
    });

    const fargateService = new ApplicationLoadBalancedFargateService(this, 'FargateService', {
      cluster,
      taskImageOptions: {
        image: ecs.ContainerImage.fromRegistry('httpd:latest'),
        containerPort: containerPort,
      },
      publicLoadBalancer: true,
      taskSubnets: vpcSubnets,
      securityGroups: [securityGroup]
    });

    new CfnOutput(this, 'LoadBalancerDNS', {
      value: fargateService.loadBalancer.loadBalancerDnsName
    })
  }
}
