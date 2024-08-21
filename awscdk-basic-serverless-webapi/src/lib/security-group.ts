import * as ec2 from 'aws-cdk-lib/aws-ec2';
import { Construct } from 'constructs';

const APP_PORT = 80;

interface Props {
  vpc: ec2.Vpc;
}

export class SecurityGroups extends Construct {
  public readonly appSecurityGroup: ec2.SecurityGroup;
  public readonly dbSecurityGroup: ec2.SecurityGroup;

  constructor(scope: Construct, id: string, { vpc }: Props) {
    super(scope, id);

    this.appSecurityGroup = new ec2.SecurityGroup(this, 'AppSecurityGroup', {
      vpc,
      description: 'App security group',
      allowAllOutbound: true,
    });

    this.appSecurityGroup.addIngressRule(
      this.appSecurityGroup,
      ec2.Port.tcp(APP_PORT),
    );

    this.dbSecurityGroup = new ec2.SecurityGroup(this, 'DnSecurityGroup', {
      vpc,
      description: 'Database security group',
    });
  }
}
