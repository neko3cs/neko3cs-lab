import * as cdk from 'aws-cdk-lib';
import * as ecs from 'aws-cdk-lib/aws-ecs';
import { Construct } from 'constructs';

import { NetworkConstruct } from './network-construct';
import { DatabaseConstruct } from './database-construct';
import { ApplicationConstruct } from './application-construct';

export interface AppStackProps extends cdk.StackProps {
  readonly appPort: number;
  readonly useNatGateway?: boolean;
  readonly acmArn?: string;
}

export class AppStack extends cdk.Stack {
  constructor(scope: Construct, id: string, props: AppStackProps) {
    super(scope, id, props);

    cdk.Tags.of(this).add('CdkStackName', 'Neko3csAppStack');

    const network = new NetworkConstruct(this, 'NetworkConstruct', {
      useNatGateway: props.useNatGateway,
    });

    const database = new DatabaseConstruct(this, 'DatabaseConstruct', {
      vpc: network.vpc,
      subnets: network.dbSubnets,
      securityGroup: network.dbSecurityGroup,
      databaseName: 'app',
    });

    const application = new ApplicationConstruct(this, 'ApplicationConstruct', {
      vpc: network.vpc,
      appSubnets: network.appSubnets,
      publicSubnets: network.publicSubnets,
      albSecurityGroup: network.albSecurityGroup,
      appSecurityGroup: network.appSecurityGroup,
      appPort: props.appPort,
      acmArn: props.acmArn,
    });

    const container = application.taskDefinition.defaultContainer!;

    container.addEnvironment('DB_HOST', database.cluster.clusterEndpoint.hostname);
    container.addEnvironment('DB_PORT', '5432');
    container.addEnvironment('DB_NAME', 'app');

    container.addSecret('DB_PASSWORD', ecs.Secret.fromSecretsManager(database.cluster.secret!, 'password'));
    container.addSecret('DB_USER', ecs.Secret.fromSecretsManager(database.cluster.secret!, 'username'));

    // ネットワーク層で定義したメソッドを使い、各レイヤー間の通信を許可
    network.addConnectivityRules(props.appPort);

    // データベースの準備が整ってからアプリケーションを起動
    application.service.node.addDependency(database.cluster);
  }
}
