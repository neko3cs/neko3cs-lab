import * as ec2 from 'aws-cdk-lib/aws-ec2';
import { Construct } from 'constructs';

export interface NetworkConstructProps {
  readonly cidr?: string;
  readonly useNatGateway?: boolean;
}

export class NetworkConstruct extends Construct {
  public readonly vpc: ec2.Vpc;
  public readonly publicSubnets: ec2.SelectedSubnets;
  public readonly appSubnets: ec2.SelectedSubnets;
  public readonly dbSubnets: ec2.SelectedSubnets;
  public readonly albSecurityGroup: ec2.SecurityGroup;
  public readonly appSecurityGroup: ec2.SecurityGroup;
  public readonly dbSecurityGroup: ec2.SecurityGroup;
  public readonly eicSecurityGroup: ec2.SecurityGroup;

  constructor(scope: Construct, id: string, props: NetworkConstructProps) {
    super(scope, id);

    this.vpc = new ec2.Vpc(this, 'Vpc', {
      ipAddresses: ec2.IpAddresses.cidr(props.cidr ?? '10.0.0.0/16'),
      maxAzs: 2,
      subnetConfiguration: [
        {
          name: 'Public',
          subnetType: ec2.SubnetType.PUBLIC,
        },
        {
          name: 'App',
          subnetType: props.useNatGateway
            ? ec2.SubnetType.PRIVATE_WITH_EGRESS
            : ec2.SubnetType.PUBLIC,
        },
        {
          name: 'Db',
          subnetType: ec2.SubnetType.PRIVATE_ISOLATED,
        },
      ],
    });

    this.publicSubnets = this.vpc.selectSubnets({ subnetGroupName: 'Public' });
    this.appSubnets = this.vpc.selectSubnets({ subnetGroupName: 'App' });
    this.dbSubnets = this.vpc.selectSubnets({ subnetGroupName: 'Db' });

    this.albSecurityGroup = new ec2.SecurityGroup(this, 'AlbSG', {
      vpc: this.vpc,
      allowAllOutbound: true,
      description: 'Security group for ALB',
    });

    this.appSecurityGroup = new ec2.SecurityGroup(this, 'AppSG', {
      vpc: this.vpc,
      allowAllOutbound: true,
      description: 'Security group for Application',
    });

    this.dbSecurityGroup = new ec2.SecurityGroup(this, 'DbSG', {
      vpc: this.vpc,
      allowAllOutbound: false,
      description: 'Security group for Database',
    });

    this.eicSecurityGroup = new ec2.SecurityGroup(this, 'EicSG', {
      vpc: this.vpc,
      allowAllOutbound: false,
      description: 'Security group for EC2 Instance Connect Endpoint',
    });

    this.addEndpoints();
  }

  // 循環参照を回避するため、Security Group IDを使用して相互の接続許可を定義
  public addConnectivityRules(appPort: number): void {
    this.appSecurityGroup.addIngressRule(
      ec2.Peer.securityGroupId(this.albSecurityGroup.securityGroupId),
      ec2.Port.tcp(appPort),
      'Allow ALB to access Application'
    );

    this.dbSecurityGroup.addIngressRule(
      ec2.Peer.securityGroupId(this.appSecurityGroup.securityGroupId),
      ec2.Port.tcp(5432),
      'Allow Application to access Database'
    );

    this.dbSecurityGroup.addIngressRule(
      ec2.Peer.securityGroupId(this.eicSecurityGroup.securityGroupId),
      ec2.Port.tcp(5432),
      'Allow EIC Endpoint to access Database'
    );

    // EIC Endpoint -> DB (Egress)
    // NOTE: CloudFormationレベルの循環参照（Circular Dependency）を回避するため、
    // 相手側のSecurity Group IDではなく、VPC CIDR（IP範囲）をターゲットとして指定。
    // これにより、EicSGとDbSGの相互依存を断ち切り、リソースの作成順序問題を解消している。
    this.eicSecurityGroup.addEgressRule(
      ec2.Peer.ipv4(this.vpc.vpcCidrBlock),
      ec2.Port.tcp(5432),
      'Allow EIC Endpoint to access VPC Database ports'
    );
  }

  private addEndpoints(): void {
    this.vpc.addGatewayEndpoint('S3Endpoint', {
      service: ec2.GatewayVpcEndpointAwsService.S3,
      subnets: [this.appSubnets],
    });

    const services = [
      ec2.InterfaceVpcEndpointAwsService.ECR,
      ec2.InterfaceVpcEndpointAwsService.ECR_DOCKER,
      ec2.InterfaceVpcEndpointAwsService.CLOUDWATCH_LOGS,
      ec2.InterfaceVpcEndpointAwsService.SECRETS_MANAGER,
      ec2.InterfaceVpcEndpointAwsService.SSM,
    ];

    services.forEach((service, index) => {
      this.vpc.addInterfaceEndpoint(`Endpoint${index}`, {
        service,
        subnets: this.appSubnets,
        securityGroups: [this.appSecurityGroup],
      });
    });

    new ec2.CfnInstanceConnectEndpoint(this, 'EicEndpoint', {
      subnetId: this.dbSubnets.subnetIds[0],
      securityGroupIds: [this.eicSecurityGroup.securityGroupId],
    });
  }
}
