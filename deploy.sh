#!/bin/bash

# Books CRUD API Deployment Script

set -e  # Exit on any error

# Load environment variables from .env file
if [ -f ".env" ]; then
    export $(cat .env | grep -v '^#' | xargs)
fi

# Configuration
RESOURCE_GROUP="api-load-testing"
APP_NAME="books-crud-api-app"
APP_SERVICE_PLAN="books-crud-api"
PUBLISH_DIR="./publish"
ZIP_FILE="books-crud-api-deploy.zip"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

print_status() { echo -e "${BLUE}[INFO]${NC} $1"; }
print_success() { echo -e "${GREEN}[SUCCESS]${NC} $1"; }
print_warning() { echo -e "${YELLOW}[WARNING]${NC} $1"; }
print_error() { echo -e "${RED}[ERROR]${NC} $1"; }

check_azure_cli() {
    print_status "Checking Azure CLI..."
    if ! command -v az &> /dev/null; then
        print_error "Azure CLI not installed."
        exit 1
    fi
    if ! az account show &> /dev/null; then
        print_error "Not logged into Azure. Run 'az login'."
        exit 1
    fi
    print_success "Azure CLI is installed and logged in."
}

check_dotnet() {
    print_status "Checking .NET SDK..."
    if ! command -v dotnet &> /dev/null; then
        print_error ".NET SDK not found."
        exit 1
    fi
    print_success ".NET SDK is installed."
}

clean_build() {
    print_status "Cleaning previous builds..."
    [ -d "$PUBLISH_DIR" ] && rm -rf "$PUBLISH_DIR" && print_success "Removed $PUBLISH_DIR"
    [ -f "$ZIP_FILE" ] && rm "$ZIP_FILE" && print_success "Removed $ZIP_FILE"
}

build_and_publish() {
    print_status "Building project..."
    dotnet restore
    dotnet build -c Release --no-restore
    dotnet publish -c Release -o "$PUBLISH_DIR" --no-build
    print_success "Build and publish complete."
}

create_deployment_zip() {
    print_status "Packaging publish folder into zip..."
    zip -r "$ZIP_FILE" "$PUBLISH_DIR" > /dev/null
    print_success "Created $ZIP_FILE"
}

create_or_prepare_app_service() {
    print_status "Checking if App Service exists..."
    if ! az webapp show --name "$APP_NAME" --resource-group "$RESOURCE_GROUP" &> /dev/null; then
        print_warning "App Service not found. Creating..."
        az webapp create \
            --name "$APP_NAME" \
            --resource-group "$RESOURCE_GROUP" \
            --plan "$APP_SERVICE_PLAN" \
            --runtime "DOTNETCORE:8.0"
        print_success "App Service created."
    else
        print_success "App Service exists."
    fi

    # Always apply configuration
    configure_app_settings
    configure_connection_string
    ensure_managed_identity
}

configure_app_settings() {
    print_status "Configuring app settings..."
    az webapp config appsettings set \
        --name "$APP_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --settings ASPNETCORE_ENVIRONMENT=Production

    if [ -n "$APPLICATIONINSIGHTS_CONNECTION_STRING" ]; then
        az webapp config appsettings set \
            --name "$APP_NAME" \
            --resource-group "$RESOURCE_GROUP" \
            --settings APPLICATIONINSIGHTS_CONNECTION_STRING="$APPLICATIONINSIGHTS_CONNECTION_STRING"
        print_success "Application Insights configured."
    else
        print_warning "APPLICATIONINSIGHTS_CONNECTION_STRING missing. Telemetry won't work."
    fi
}

configure_connection_string() {
    print_status "Configuring database connection string..."
    az webapp config connection-string set \
        --name "$APP_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --settings DefaultConnection="Server=tcp:books-crud-db.database.windows.net,1433;Initial Catalog=BooksCrudApi.Database;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";" \
        --connection-string-type SQLAzure
    print_success "Database connection string set."
}

ensure_managed_identity() {
    print_status "Checking managed identity..."
    if ! az webapp identity show --name "$APP_NAME" --resource-group "$RESOURCE_GROUP" --query "principalId" -o tsv &> /dev/null; then
        az webapp identity assign --name "$APP_NAME" --resource-group "$RESOURCE_GROUP"
        print_success "Managed identity enabled."
    else
        print_success "Managed identity already active."
    fi
}

deploy_zip_package() {
    print_status "Deploying zip package..."
    az webapp deploy --resource-group "$RESOURCE_GROUP" --name "$APP_NAME" --src-path "$ZIP_FILE"
    print_success "App deployed to Azure App Service."
}

main() {
    echo -e "${BLUE}================================${NC}"
    echo -e "${BLUE}  📦 Books CRUD API Deployment${NC}"
    echo -e "${BLUE}================================${NC}"
    echo

    check_azure_cli
    check_dotnet
    clean_build
    build_and_publish
    create_deployment_zip
    create_or_prepare_app_service
    deploy_zip_package
}

main "$@"
