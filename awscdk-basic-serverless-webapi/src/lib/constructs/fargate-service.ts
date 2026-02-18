import * as path from 'path';
import * as ec2 from 'aws-cdk-lib/aws-ec2';
import * as ecs from 'aws-cdk-lib/aws-ecs';
import * as elbv2 from 'aws-cdk-lib/aws-elasticloadbalancingv2';
import * as secretsmanager from 'aws-cdk-lib/aws-secretsmanager';
import * as logs from 'aws-cdk-lib/aws-logs';
import { Construct } from "constructs"

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
  public readonly service: ecs.FargateService;

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
      apiTargetGroup,
      pageTargetGroup,
    }: Props
  ) {
    super(scope, id);

    const cluster = new ecs.Cluster(this, 'Cluster', {
      vpc,
    });

    const taskDefinition = new ecs.FargateTaskDefinition(this, 'TaskDef', {
      memoryLimitMiB: 512,
      cpu: 256,
    });

    const container = taskDefinition.addContainer('AppContainer', {
      image: ecs.ContainerImage.fromAsset(path.join(__dirname, '../../app')),
      logging: ecs.LogDrivers.awsLogs({
        streamPrefix: 'App',
        logRetention: logs.RetentionDays.ONE_WEEK,
      }),
      environment: {
        DB_HOST: databaseHost,
        DB_PORT: databasePort.toString(),
        DB_NAME: 'app',
      },
      secrets: {
        DB_PASSWORD: ecs.Secret.fromSecretsManager(dbCredential, 'password'),
        DB_USER: ecs.Secret.fromSecretsManager(dbCredential, 'username'),
      },
    });

    container.addPortMappings({
      containerPort: containerPort,
      protocol: ecs.Protocol.TCP,
    });

    this.service = new ecs.FargateService(this, 'Service', {
      cluster,
      taskDefinition,
      vpcSubnets,
      securityGroups: [securityGroup],
      desiredCount: 1,
    });

    // 既存のターゲットグループにサービスを登録
    apiTargetGroup.addTarget(this.service);
    pageTargetGroup.addTarget(this.service);
  }
}
