#!/bin/bash
set -e

PROJECT_ID="gcloud-serverless-web-app"
REGION="asia-northeast1"
FUNCTION_NAME="app-function"

echo "Deploying to Cloud Functions (building remotely)..."
gcloud functions deploy $FUNCTION_NAME \
  --project=$PROJECT_ID \
  --region=$REGION \
  --gen2 \
  --runtime=nodejs20 \
  --entry-point=app \
  --source=. \
  --trigger-http \
  --ingress-settings=internal-and-gclb \
  --vpc-connector=vpc-con \
  --egress-settings=all \
  --service-account=function-sa@$PROJECT_ID.iam.gserviceaccount.com \
  --quiet

echo "Deployment complete!"
