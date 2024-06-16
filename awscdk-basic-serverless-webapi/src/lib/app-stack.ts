import * as cdk from 'aws-cdk-lib';
import { Construct } from 'constructs';
import * as ecs from 'aws-cdk-lib/aws-ecs';
import * as ec2 from 'aws-cdk-lib/aws-ec2';

export interface AppStackProps extends cdk.StackProps {
  AllowedIPAddress: string
}
export class AppStack extends cdk.Stack {
  constructor(scope: Construct, id: string, props?: AppStackProps) {
    super(scope, id, props);

    const vpc = new ec2.Vpc(this, 'Vpc', {
      maxAzs: 2,
    });
    const cluster = new ecs.Cluster(this, 'Cluster', {
      vpc,
    });
    const taskDefinition = new ecs.FargateTaskDefinition(this, 'TaskDefinition');
    const container = taskDefinition.addContainer('ApacheContainer', {
      image: ecs.ContainerImage.fromRegistry('httpd:latest'),
      memoryLimitMiB: 512,
      cpu: 2,
    });
    container.addPortMappings({
      containerPort: 80,
    });
    const securityGroup = new ec2.SecurityGroup(this, 'FargateSecurityGroup', {
      vpc,
      allowAllOutbound: true,
    });
    securityGroup.addIngressRule(
      ec2.Peer.ipv4(`${props?.AllowedIPAddress}/32`),
      ec2.Port.tcp(80),
      'Allow HTTP traffic from developer machine.'
    );
    new ecs.FargateService(this, 'FargateService', {
      cluster,
      taskDefinition,
      desiredCount: 1,
      securityGroups: [securityGroup],
      vpcSubnets: { subnetType: ec2.SubnetType.PUBLIC },
      assignPublicIp: true,
    });
  }
}
