#!/usr/bin/env node
import 'source-map-support/register';
import * as cdk from 'aws-cdk-lib';
import * as dotenv from 'dotenv';
import { AppStack } from '../lib/app-stack';

dotenv.config();
const app = new cdk.App();
new AppStack(app, 'AppStack', {
  ServicePrefix: process.env.SERVICE_PREFIX ?? 'App',
  MyIPAddress: process.env.MY_IP_ADDRESS ?? '*',
});
