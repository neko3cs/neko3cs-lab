import * as cdk from 'aws-cdk-lib';
import * as ec2 from 'aws-cdk-lib/aws-ec2';

import { Construct } from 'constructs';

import { VpcSubnets } from './constructs/vpc-subnets';
import { VpcEndpoints } from './constructs/vpc-endpoints';
import { LoadBalancer } from './constructs/load-balancer';
import { SecurityGroups } from './constructs/security-groups';
import { RelationalDatabaseService } from './constructs/relational-database-service';
import { FargateService } from './constructs/fargate-service';

interface AppStackProps extends cdk.StackProps {
  appPort: number;
  useNatGateway?: boolean;
  acmArn?: string;
  apiAppContainerTag?: string;
  pageAppContainerTag?: string;
  apiAppRepositoryName?: string;
  pageAppRepositoryName?: string;
}

export class AppStack extends cdk.Stack {
  constructor(
    scope: Construct,
    id: string,
    {
      appPort,
      useNatGateway = true,
      acmArn,
      apiAppContainerTag,
      pageAppContainerTag,
      apiAppRepositoryName,
      pageAppRepositoryName,
      ...props
    }: AppStackProps
  ) {
    super(scope, id, props);

    cdk.Tags.of(this).add('CdkStackName', 'Neko3csAppStack');

    const { vpc, publicSubnets, appSubnets, dbSubnets } = new VpcSubnets(
      this,
      'VpcSubnets',
      {
        useNatGateway,
      },
    );

    const { appSecurityGroup, dbSecurityGroup } = new SecurityGroups(
      this,
      'SecurityGroups',
      {
        vpc,
      },
    );

    new VpcEndpoints(this, 'VpcEndpoints', {
      vpc,
      appSubnets,
      appSecurityGroup,
    });

    const { apiTargetGroup, pageTargetGroup, loadBalancer } = new LoadBalancer(
      this,
      'LoadBalancer',
      {
        vpc,
        vpcSubnets: publicSubnets,
        appPort,
        acmArn,
      },
    );

    appSecurityGroup.addIngressRule(
      loadBalancer.connections.securityGroups[0],
      ec2.Port.tcp(appPort),
      'Allow ALB to App',
    );

    const { dbCluster } = new RelationalDatabaseService(
      this,
      'RelationalDatabaseService',
      {
        vpc,
        vpcSubnets: dbSubnets,
        securityGroup: dbSecurityGroup,
      },
    );

    dbSecurityGroup.addIngressRule(
      appSecurityGroup,
      ec2.Port.tcp(5432),
    );

    new FargateService(this, 'FargateService', {
      vpc,
      vpcSubnets: appSubnets,
      securityGroup: appSecurityGroup,
      dbCredential: dbCluster.secret!,
      databaseHost: dbCluster.clusterEndpoint.hostname,
      databasePort: dbCluster.clusterEndpoint.port,
      containerPort: appPort,
      apiTargetGroup,
      pageTargetGroup,
    }).node.addDependency(dbCluster);
  }
}
