import * as cdk from 'aws-cdk-lib';
import * as ec2 from 'aws-cdk-lib/aws-ec2';
import { Construct } from 'constructs';
import { VpcSubnets } from './vpc-subnets';
import { VpcEndpoints } from './vpc-endpoints';
import { LoadBalancer } from './load-balancer';
import { SecurityGroups } from './security-group';
import { RelationalDatabaseService } from './relational-database-service';
import { FargateService } from './fargate-service';

const APP_PORT = 80;

export class AppStack extends cdk.Stack {
  constructor(scope: Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

    const { vpc, publicSubnets, appSubnets, dbSubnets } = new VpcSubnets(
      this,
      'VpcSubnets',
      {},
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

    const { apiTargetGroup, pageTargetGroup } = new LoadBalancer(
      this,
      'LoadBalancer',
      {
        vpc,
        vpcSubnets: publicSubnets,
      },
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
      ec2.Port.tcp(dbCluster.clusterEndpoint.port),
    );

    new FargateService(this, 'FargateService', {
      vpc,
      vpcSubnets: appSubnets,
      securityGroup: appSecurityGroup,
      dbCredential: dbCluster.secret!,
      databaseHost: dbCluster.clusterEndpoint.hostname,
      databasePort: dbCluster.clusterEndpoint.port,
      containerPort: APP_PORT,
      apiTargetGroup,
      pageTargetGroup,
    }).node.addDependency(dbCluster);

    // const vpc = new ec2.Vpc(this, 'Vpc', {
    //   maxAzs: 2,
    // });
    // const cluster = new ecs.Cluster(this, 'Cluster', {
    //   vpc,
    // });

    // const fargateService = new ecs_patterns.ApplicationLoadBalancedFargateService(this, 'FargateService', {
    //   cluster,
    //   taskImageOptions: {
    //     image: ecs.ContainerImage.fromRegistry('httpd:latest'),
    //     containerPort: 80,
    //   },
    //   publicLoadBalancer: true,
    // });

    // fargateService.service.connections.securityGroups[0].addIngressRule(
    //   ec2.Peer.anyIpv4(),
    //   ec2.Port.tcp(80),
    //   'Allow HTTP traffic from any access.',
    // );

    // new cdk.CfnOutput(this, 'LoadBalancerDNS', {
    //   value: fargateService.loadBalancer.loadBalancerDnsName
    // })
  }
}
