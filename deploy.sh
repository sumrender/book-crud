#!/bin/bash

# Books CRUD API Deployment Script
# This script builds, publishes, and deploys the application to Azure App Service

set -e  # Exit on any error

# Configuration
RESOURCE_GROUP="api-load-testing"
APP_NAME="books-crud-api-app"
APP_SERVICE_PLAN="books-crud-api"
PUBLISH_DIR="./publish"
ZIP_FILE="books-crud-api-deploy.zip"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Function to check if Azure CLI is installed and logged in
check_azure_cli() {
    print_status "Checking Azure CLI installation..."
    
    if ! command -v az &> /dev/null; then
        print_error "Azure CLI is not installed. Please install it first."
        exit 1
    fi
    
    print_success "Azure CLI is installed"
    
    # Check if logged in
    if ! az account show &> /dev/null; then
        print_error "Not logged into Azure. Please run 'az login' first."
        exit 1
    fi
    
    print_success "Logged into Azure"
}

# Function to check if .NET SDK is installed
check_dotnet() {
    print_status "Checking .NET SDK installation..."
    
    if ! command -v dotnet &> /dev/null; then
        print_error ".NET SDK is not installed. Please install it first."
        exit 1
    fi
    
    print_success ".NET SDK is installed"
}

# Function to clean previous build artifacts
clean_build() {
    print_status "Cleaning previous build artifacts..."
    
    if [ -d "$PUBLISH_DIR" ]; then
        rm -rf "$PUBLISH_DIR"
        print_success "Cleaned publish directory"
    fi
    
    if [ -f "$ZIP_FILE" ]; then
        rm "$ZIP_FILE"
        print_success "Cleaned previous zip file"
    fi
}

# Function to build and publish the application
build_and_publish() {
    print_status "Building and publishing the application..."
    
    # Restore packages
    print_status "Restoring NuGet packages..."
    dotnet restore
    
    # Build in Release mode
    print_status "Building in Release mode..."
    dotnet build -c Release --no-restore
    
    # Publish to directory
    print_status "Publishing to $PUBLISH_DIR..."
    dotnet publish -c Release -o "$PUBLISH_DIR" --no-build
    
    print_success "Application built and published successfully"
}

# Function to create deployment zip
create_deployment_zip() {
    print_status "Creating deployment zip file..."
    
    cd "$PUBLISH_DIR"
    zip -r "../$ZIP_FILE" . > /dev/null
    cd ..
    
    print_success "Deployment zip created: $ZIP_FILE"
}

# Function to deploy to Azure App Service
deploy_to_azure() {
    print_status "Deploying to Azure App Service..."
    
    # Check if app service exists
    if ! az webapp show --name "$APP_NAME" --resource-group "$RESOURCE_GROUP" &> /dev/null; then
        print_error "App Service '$APP_NAME' not found in resource group '$RESOURCE_GROUP'"
        print_status "Creating App Service..."
        az webapp create --name "$APP_NAME" --resource-group "$RESOURCE_GROUP" --plan "$APP_SERVICE_PLAN" --runtime "DOTNETCORE:8.0"
        print_success "App Service created"
    fi
    
    # Deploy the application
    print_status "Uploading and deploying application..."
    az webapp deploy --resource-group "$RESOURCE_GROUP" --name "$APP_NAME" --src-path "$ZIP_FILE"
    
    print_success "Application deployed successfully"
}

# Function to configure app settings
configure_app_settings() {
    print_status "Configuring application settings..."
    
    # Set environment to Production
    az webapp config appsettings set --name "$APP_NAME" --resource-group "$RESOURCE_GROUP" --settings ASPNETCORE_ENVIRONMENT=Production
    
    print_success "Application settings configured"
}

# Function to configure connection string
configure_connection_string() {
    print_status "Configuring database connection string..."
    
    # Set the connection string for Azure SQL Database
    az webapp config connection-string set \
        --name "$APP_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --settings DefaultConnection="Server=tcp:books-crud-db.database.windows.net,1433;Initial Catalog=BooksCrudApi.Database;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";" \
        --connection-string-type SQLAzure
    
    print_success "Database connection string configured"
}

# Function to ensure managed identity is configured
ensure_managed_identity() {
    print_status "Checking managed identity configuration..."
    
    # Check if managed identity exists
    IDENTITY_INFO=$(az webapp identity show --name "$APP_NAME" --resource-group "$RESOURCE_GROUP" --query "principalId" -o tsv 2>/dev/null || echo "")
    
    if [ -z "$IDENTITY_INFO" ]; then
        print_status "Enabling managed identity..."
        az webapp identity assign --name "$APP_NAME" --resource-group "$RESOURCE_GROUP"
        print_success "Managed identity enabled"
    else
        print_success "Managed identity already configured"
    fi
}

# Function to test the deployed application
test_deployment() {
    print_status "Testing deployed application..."
    
    # Wait for application to start
    print_status "Waiting for application to start..."
    sleep 30
    
    # Test API endpoint
    print_status "Testing API endpoint..."
    if curl -f -s "https://$APP_NAME.azurewebsites.net/api/books" > /dev/null; then
        print_success "API endpoint is working"
    else
        print_warning "API endpoint test failed - application may still be starting"
    fi
    
    # Test Swagger endpoint
    print_status "Testing Swagger endpoint..."
    if curl -f -s "https://$APP_NAME.azurewebsites.net/swagger/v1/swagger.json" > /dev/null; then
        print_success "Swagger endpoint is working"
    else
        print_warning "Swagger endpoint test failed - application may still be starting"
    fi
}

# Function to display deployment information
show_deployment_info() {
    echo
    print_success "🎉 Deployment completed successfully!"
    echo
    echo -e "${GREEN}Application URLs:${NC}"
    echo -e "  API Base: ${BLUE}https://$APP_NAME.azurewebsites.net${NC}"
    echo -e "  Swagger UI: ${BLUE}https://$APP_NAME.azurewebsites.net/swagger${NC}"
    echo
    echo -e "${GREEN}API Endpoints:${NC}"
    echo -e "  GET    /api/books          - Get all books"
    echo -e "  GET    /api/books/{id}     - Get specific book"
    echo -e "  POST   /api/books          - Create new book"
    echo -e "  PUT    /api/books/{id}     - Update book"
    echo -e "  DELETE /api/books/{id}     - Delete book"
    echo
    print_status "You can now test your API using the Swagger UI or curl commands"
}

# Main deployment function
main() {
    echo -e "${BLUE}================================${NC}"
    echo -e "${BLUE}  Books CRUD API Deployment${NC}"
    echo -e "${BLUE}================================${NC}"
    echo
    
    # Check prerequisites
    check_azure_cli
    check_dotnet
    
    # Clean previous build
    clean_build
    
    # Build and publish
    build_and_publish
    
    # Create deployment package
    create_deployment_zip
    
    # Deploy to Azure
    deploy_to_azure
    
    # Configure settings
    configure_app_settings
    configure_connection_string
    
    # Ensure managed identity
    ensure_managed_identity
    
    # Test deployment
    test_deployment
    
    # Show deployment information
    show_deployment_info
}

# Run the main function
main "$@" 