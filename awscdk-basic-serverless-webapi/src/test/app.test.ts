import * as cdk from 'aws-cdk-lib';
import { Template } from 'aws-cdk-lib/assertions';
import { AppStack } from '../lib/app-stack';

test('Infrastructure Resource Validation', () => {
  const app = new cdk.App();
  const stack = new AppStack(app, 'MyTestStack');
  const template = Template.fromStack(stack);

  // 1. VPC Validation
  template.hasResourceProperties('AWS::EC2::VPC', {
    CidrBlock: '10.0.0.0/16',
  });
  // Check Subnets (2 AZs * 3 types = 6 subnets)
  template.resourceCountIs('AWS::EC2::Subnet', 6);

  // 2. RDS Validation (PostgreSQL)
  template.hasResourceProperties('AWS::RDS::DBCluster', {
    Engine: 'aurora-postgresql',
    EngineVersion: '17.7',
    DatabaseName: 'app',
  });

  // 3. ECS/Fargate Validation (httpd:latest)
  template.hasResourceProperties('AWS::ECS::TaskDefinition', {
    ContainerDefinitions: [
      {
        Image: 'httpd:latest',
        Name: 'AppContainer',
        PortMappings: [
          {
            ContainerPort: 80,
            Protocol: 'tcp',
          },
        ],
      },
    ],
  });

  // 4. Security Group Validation
  // App SG should allow traffic on port 80 (for httpd)
  template.hasResourceProperties('AWS::EC2::SecurityGroup', {
    GroupDescription: 'App security group',
  });

  // DB SG should allow traffic on port 5432
  template.hasResourceProperties('AWS::EC2::SecurityGroupIngress', {
    FromPort: 5432,
    ToPort: 5432,
    IpProtocol: 'tcp',
  });
});
