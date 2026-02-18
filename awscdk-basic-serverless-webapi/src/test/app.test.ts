import * as cdk from 'aws-cdk-lib';
import { Template } from 'aws-cdk-lib/assertions';
import { AppStack } from '../lib/app-stack';

test('Infrastructure Resource Validation', () => {
  const app = new cdk.App();
  const stack = new AppStack(app, 'MyTestStack', {
    appPort: 3000,
  });
  const template = Template.fromStack(stack);

  template.hasResourceProperties('AWS::EC2::VPC', {
    CidrBlock: '10.0.0.0/16',
  });
  template.resourceCountIs('AWS::EC2::Subnet', 6);

  template.hasResourceProperties('AWS::RDS::DBCluster', {
    Engine: 'aurora-postgresql',
    EngineVersion: '17.7',
    DatabaseName: 'app',
  });

  template.hasResourceProperties('AWS::ECS::TaskDefinition', {
    ContainerDefinitions: [
      {
        Name: 'AppContainer',
        PortMappings: [
          {
            ContainerPort: 3000,
            Protocol: 'tcp',
          },
        ],
      },
    ],
  });

  template.hasResourceProperties('AWS::EC2::SecurityGroup', {
    GroupDescription: 'Security group for Application',
    SecurityGroupIngress: [
      {
        FromPort: 443,
        ToPort: 443,
        IpProtocol: 'tcp',
      },
      {
        Description: 'Allow ALB to access Application',
        FromPort: 3000,
        ToPort: 3000,
        IpProtocol: 'tcp',
      }
    ]
  });

  template.hasResourceProperties('AWS::EC2::SecurityGroup', {
    GroupDescription: 'Security group for Database',
    SecurityGroupIngress: [
      {
        Description: 'Allow Application to access Database',
        FromPort: 5432,
        ToPort: 5432,
        IpProtocol: 'tcp',
      }
    ]
  });
});
