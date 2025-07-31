#!/bin/bash

# Quick Deploy Script for Books CRUD API
# Use this for fast deployments when infrastructure is already configured

set -e

# Configuration
RESOURCE_GROUP="api-load-testing"
APP_NAME="books-crud-api-app"
PUBLISH_DIR="./publish"
ZIP_FILE="books-crud-api-quick.zip"

# Colors
GREEN='\033[0;32m'
BLUE='\033[0;34m'
NC='\033[0m'

echo -e "${BLUE}🚀 Quick Deploy - Books CRUD API${NC}"
echo

# Clean and build
echo "📦 Building application..."
rm -rf "$PUBLISH_DIR" "$ZIP_FILE" 2>/dev/null || true
dotnet publish -c Release -o "$PUBLISH_DIR"

# Create zip
echo "📦 Creating deployment package..."
cd "$PUBLISH_DIR"
zip -r "../$ZIP_FILE" . > /dev/null
cd ..

# Deploy
echo "🚀 Deploying to Azure..."
az webapp deploy --resource-group "$RESOURCE_GROUP" --name "$APP_NAME" --src-path "$ZIP_FILE"

echo
echo -e "${GREEN}✅ Deployment completed!${NC}"
echo -e "🌐 API: https://$APP_NAME.azurewebsites.net"
echo -e "📚 Swagger: https://$APP_NAME.azurewebsites.net/swagger" 