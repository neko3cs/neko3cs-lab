import * as cdk from 'aws-cdk-lib';
import { Construct } from 'constructs';
import * as ec2 from 'aws-cdk-lib/aws-ec2';
import * as rds from 'aws-cdk-lib/aws-rds';
import * as ecs from 'aws-cdk-lib/aws-ecs';
import * as ecsPatterns from 'aws-cdk-lib/aws-ecs-patterns';

export interface AppStackProps extends cdk.StackProps {
  ServicePrefix: string,
  MyIPAddress: string
}
export class AppStack extends cdk.Stack {
  constructor(scope: Construct, id: string, props?: AppStackProps) {
    super(scope, id, props);
    // CONFIG
    const SERVICE_PREFIX = props?.ServicePrefix;
    const ALLOWED_IPADDRESS = props?.MyIPAddress;

    // NETWORK
    const vpc = new ec2.Vpc(this, `${SERVICE_PREFIX}Vpc`, {
      maxAzs: 1,
      subnetConfiguration: [
        {
          cidrMask: 24,
          name: 'ingress',
          subnetType: ec2.SubnetType.PUBLIC
        },
        {
          cidrMask: 24,
          name: 'app',
          subnetType: ec2.SubnetType.PRIVATE_WITH_EGRESS
        },
        {
          cidrMask: 28,
          name: 'db',
          subnetType: ec2.SubnetType.PRIVATE_ISOLATED
        },
      ]
    });
    const applicationSecurityGroup = new ec2.SecurityGroup(this, `${SERVICE_PREFIX}AppSecurityGroup`, { vpc });
    applicationSecurityGroup.addIngressRule(
      ec2.Peer.ipv4(`${ALLOWED_IPADDRESS}/32`),
      ec2.Port.tcp(80),
      'Allow HTTP traffic from the IP of executing machine.',
    );
    const databaseSecurityGroup = new ec2.SecurityGroup(this, `${SERVICE_PREFIX}DbSecurityGroup`, {
      vpc,
      allowAllOutbound: true
    });
    databaseSecurityGroup.connections.allowFrom(applicationSecurityGroup, ec2.Port.tcp(3306), 'Ingress 3306 from App.');

    // DATABASE
    const dbUserName = 'db_admin';
    const dbSecret = new cdk.aws_secretsmanager.Secret(this, 'DBSecret', {
      secretName: 'db-credentials',
      generateSecretString: {
        secretStringTemplate: JSON.stringify({ username: dbUserName }),
        generateStringKey: 'P@ssword!',
        excludeCharacters: '"@/\\ '
      }
    });
    const db = new rds.ServerlessCluster(this, `${SERVICE_PREFIX}AuroraCluster`, {
      defaultDatabaseName: 'neko3csdb',
      engine: rds.DatabaseClusterEngine.AURORA_MYSQL,
      credentials: rds.Credentials.fromSecret(dbSecret),
      vpc,
      vpcSubnets: {
        subnetType: ec2.SubnetType.PRIVATE_ISOLATED
      },
      securityGroups: [databaseSecurityGroup],
      scaling: {
        autoPause: cdk.Duration.minutes(5),
        minCapacity: rds.AuroraCapacityUnit.ACU_1,
        maxCapacity: rds.AuroraCapacityUnit.ACU_2
      },
      deletionProtection: false,
    });
    // APPLICATION
    const cluster = new ecs.Cluster(this, `${SERVICE_PREFIX}Cluster`, { vpc });
    const fargateService = new ecsPatterns.ApplicationLoadBalancedFargateService(this, `${SERVICE_PREFIX}FargateService`, {
      cluster,
      taskImageOptions: {
        image: ecs.ContainerImage.fromRegistry('nginx'),
        containerPort: 80
      },
      desiredCount: 1,
      memoryLimitMiB: 512,
      publicLoadBalancer: true,
      securityGroups: [applicationSecurityGroup],
    });

    // OUTPUT
    new cdk.CfnOutput(this, 'RDSEndpointAddress', { value: db.clusterEndpoint.socketAddress });
    new cdk.CfnOutput(this, 'LoadBalancerDNS', { value: fargateService.loadBalancer.loadBalancerDnsName });
  }
}
