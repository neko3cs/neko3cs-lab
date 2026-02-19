# Agent Instructions for `awscdk-basic-serverless-webapi`

This document provides essential guidelines, commands, and conventions for AI agents working on this AWS CDK project.

## Project Overview

- **Technology Stack**: AWS CDK v2, TypeScript, Node.js.
- **Purpose**: A serverless Web API infrastructure including VPC, Fargate, RDS, and Load Balancer.
- **Package Manager**: `pnpm` (Workspace root has no `package.json`, use `src/` directory for development).

## 1. Development Commands

**Note: Always run these commands from the `src/` directory.**

### Build & Compilation

- **Compile TypeScript**: `pnpm build`
- **Watch mode**: `pnpm watch`

### Testing (Jest)

- **Run all tests**: `pnpm test`
- **Run a single test file**: `pnpm exec jest test/app.test.ts`
- **Run with coverage**: `pnpm exec jest --coverage`
- **Update snapshots**: `pnpm exec jest -u`

### AWS CDK Commands

- **Synthesize CloudFormation**: `pnpm exec cdk synth`
- **Check differences**: `pnpm exec cdk diff`
- **Deploy stack**: `pnpm exec cdk deploy`
- **List stacks**: `pnpm exec cdk ls`

## 2. Directory Structure

- `src/bin/`: Application entry point (`app.ts`). Environment variables are handled here.
- `src/lib/`: Stack and Construct definitions.
  - `app-stack.ts`: The main stack orchestrating all constructs.
  - `*-construct.ts`: Modularized L2/L3 constructs (Network, Database, Application).
- `src/test/`: Infrastructure tests using `@aws-cdk/assertions`.
- `src/app/`: (If exists) Application source code running on Fargate.

## 3. Code Style & Conventions

### Imports

- **Grouping**: Group imports in the following order:
  1. AWS CDK libraries (`import * as cdk from 'aws-cdk-lib'`)
  2. Third-party libraries (`import { Construct } from 'constructs'`)
  3. Local constructs and modules (`import { X } from './x'`)
- **Namespace Imports**: Prefer `import * as [service] from 'aws-cdk-lib/aws-[service]'` (e.g., `aws-ec2`, `aws-rds`) to maintain consistency.

### Naming Conventions

- **Classes/Constructs**: `PascalCase` (e.g., `NetworkConstruct`).
- **Variables/Instances**: `camelCase` (e.g., `vpc`, `dbCluster`).
- **Interfaces/Props**: `PascalCase` named as `[ClassName]Props` (e.g., `AppStackProps`).
- **Constants**: `SCREAMING_SNAKE_CASE` (e.g., `APP_PORT`).

### TypeScript Usage

- **Strict Typing**: Define interfaces for all construct props.
- **Public/Private**: Mark construct members as `public readonly` if required by other constructs (e.g., `vpc` in `NetworkConstruct`).

## 4. Resource Specific Guidelines

### Networking

- Subnets: Explicitly use `Public`, `App` (Private with Egress), and `Db` (Isolated) subnet groups.
- Security Groups: Define rules within the construct or via dedicated methods like `addConnectivityRules()`.

### RDS & ECS

- **RDS**: Use Aurora PostgreSQL or RDS. Credentials are automatically managed via AWS Secrets Manager.
- **ECS/Fargate**: Use `ApplicationLoadBalancedFargateService` for standard web APIs. Use `container.addEnvironment()` and `container.addSecret()` for configuration.

## 5. Gemini-Specific Workflow

### Research & Strategy

- **File Discovery**: Always check the `src/lib/` directory first for infrastructure logic.
- **Context**: Read `src/bin/app.ts` to understand how environment variables map to stack properties.

### Execution

- **Atomic Changes**: Modify one construct at a time.
- **Validation**: After modifying a construct, run `pnpm exec cdk synth` in the `src/` directory to ensure no synthesis errors.
- **Test-Driven**: Update tests in `src/test/` concurrently with infrastructure changes.

### Security

- **No Secrets**: Never hardcode credentials. Use environment variables in `src/bin/app.ts` or CDK Secrets.

---

_Note: This file is optimized for AI agents. Update it when project patterns evolve._
