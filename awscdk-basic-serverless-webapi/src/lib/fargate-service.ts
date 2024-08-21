import * as ec2 from 'aws-cdk-lib/aws-ec2';
import * as ecs from 'aws-cdk-lib/aws-ecs';
import * as ecr from 'aws-cdk-lib/aws-ecr';
import * as logs from 'aws-cdk-lib/aws-logs';
import * as elbv2 from 'aws-cdk-lib/aws-elasticloadbalancingv2';
import * as secretsmanager from 'aws-cdk-lib/aws-secretsmanager';
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
  }
}
