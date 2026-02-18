import * as path from 'path';
import * as cdk from 'aws-cdk-lib';
import * as ec2 from 'aws-cdk-lib/aws-ec2';
import * as ecs from 'aws-cdk-lib/aws-ecs';
import * as elbv2 from 'aws-cdk-lib/aws-elasticloadbalancingv2';
import * as logs from 'aws-cdk-lib/aws-logs';
import { Construct } from 'constructs';

export interface ApplicationConstructProps {
  readonly vpc: ec2.IVpc;
  readonly appSubnets: ec2.SubnetSelection;
  readonly publicSubnets: ec2.SubnetSelection;
  readonly albSecurityGroup: ec2.ISecurityGroup;
  readonly appSecurityGroup: ec2.ISecurityGroup;
  readonly appPort: number;
  readonly acmArn?: string;
}

export class ApplicationConstruct extends Construct {
  public readonly loadBalancer: elbv2.ApplicationLoadBalancer;
  public readonly service: ecs.FargateService;
  public readonly taskDefinition: ecs.FargateTaskDefinition;

  constructor(scope: Construct, id: string, props: ApplicationConstructProps) {
    super(scope, id);

    const cluster = new ecs.Cluster(this, 'Cluster', {
      vpc: props.vpc,
    });

    this.loadBalancer = new elbv2.ApplicationLoadBalancer(this, 'ALB', {
      vpc: props.vpc,
      internetFacing: true,
      vpcSubnets: props.publicSubnets,
      securityGroup: props.albSecurityGroup,
    });

    this.taskDefinition = new ecs.FargateTaskDefinition(this, 'TaskDef', {
      memoryLimitMiB: 512,
      cpu: 256,
    });

    const container = this.taskDefinition.addContainer('AppContainer', {
      image: ecs.ContainerImage.fromAsset(path.join(__dirname, '../app')),
      logging: ecs.LogDrivers.awsLogs({
        streamPrefix: 'App',
        logRetention: logs.RetentionDays.ONE_WEEK,
      }),
    });

    container.addPortMappings({
      containerPort: props.appPort,
      protocol: ecs.Protocol.TCP,
    });

    this.service = new ecs.FargateService(this, 'Service', {
      cluster,
      taskDefinition: this.taskDefinition,
      vpcSubnets: props.appSubnets,
      securityGroups: [props.appSecurityGroup],
      desiredCount: 1,
    });

    const listener = this.loadBalancer.addListener('Listener', {
      port: 80,
      open: true,
    });

    listener.addTargets('TargetGroup', {
      port: props.appPort,
      protocol: elbv2.ApplicationProtocol.HTTP,
      targets: [this.service],
      healthCheck: {
        path: '/healthCheck',
      },
    });

    new cdk.CfnOutput(this, 'ApplicationUrl', {
      value: `http://${this.loadBalancer.loadBalancerDnsName}`,
    });
  }
}
