import * as cdk from 'aws-cdk-lib';
import * as ec2 from 'aws-cdk-lib/aws-ec2';
import * as rds from 'aws-cdk-lib/aws-rds';
import * as logs from 'aws-cdk-lib/aws-logs';
import { Construct } from 'constructs';

export interface DatabaseConstructProps {
  readonly vpc: ec2.IVpc;
  readonly subnets: ec2.SubnetSelection;
  readonly securityGroup: ec2.ISecurityGroup;
  readonly databaseName?: string;
}

export class DatabaseConstruct extends Construct {
  public readonly cluster: rds.DatabaseCluster;

  constructor(scope: Construct, id: string, props: DatabaseConstructProps) {
    super(scope, id);

    const engine = rds.DatabaseClusterEngine.auroraPostgres({
      version: rds.AuroraPostgresEngineVersion.of('17.7', '17'),
    });

    const parameterGroup = new rds.ParameterGroup(this, 'ParameterGroup', {
      engine,
      parameters: {
        'search_path': 'public',
      },
    });

    const instanceProps: rds.ClusterInstanceProps = {
      instanceType: ec2.InstanceType.of(ec2.InstanceClass.T3, ec2.InstanceSize.MEDIUM),
    };

    this.cluster = new rds.DatabaseCluster(this, 'Cluster', {
      vpc: props.vpc,
      subnetGroup: new rds.SubnetGroup(this, 'SubnetGroup', {
        vpc: props.vpc,
        vpcSubnets: props.subnets,
        description: 'Subnets for Cluster database',
      }),
      engine,
      port: 5432,
      credentials: rds.Credentials.fromGeneratedSecret('app_user'),
      parameterGroup,
      defaultDatabaseName: props.databaseName ?? 'app',
      writer: rds.ClusterInstance.serverlessV2('Instance1'),
      readers: [
        rds.ClusterInstance.serverlessV2('Instance2'),
      ],
      serverlessV2MinCapacity: 0.5,
      serverlessV2MaxCapacity: 1,
      securityGroups: [props.securityGroup],
      monitoringInterval: cdk.Duration.minutes(1),
      cloudwatchLogsRetention: logs.RetentionDays.ONE_YEAR,
      cloudwatchLogsExports: ['postgresql'],
      backup: {
        retention: cdk.Duration.days(7),
        preferredWindow: '19:00-20:00',
      },
      removalPolicy: cdk.RemovalPolicy.DESTROY,
      enableDataApi: true, // Enable Data API for RDS Query Editor
    });
  }
}
