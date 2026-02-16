#!/usr/bin/env node
import 'source-map-support/register';
import * as cdk from 'aws-cdk-lib';
import { AppStack } from '../lib/app-stack';

const app = new cdk.App();

const appPort = process.env.APP_PORT ? parseInt(process.env.APP_PORT) : 3000;
const useNatGateway = process.env.USE_NAT_GATEWAY === 'true' || true;
const acmArn = process.env.ACM_ARN;

new AppStack(app, 'AppStack', {
  appPort,
  useNatGateway,
  acmArn,
});
