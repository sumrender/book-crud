#!/bin/bash

# Books CRUD API Health Check Script

set -e  # Exit on any error

# Load environment variables from .env file
if [ -f ".env" ]; then
    export $(cat .env | grep -v '^#' | xargs)
fi

# Configuration
RESOURCE_GROUP="api-load-testing"
APP_NAME="books-crud-api-app"
APP_SERVICE_PLAN="books-crud-api"

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

check_webapp_status() {
    print_status "Checking webapp status..."
    
    local state=$(az webapp show --name "$APP_NAME" --resource-group "$RESOURCE_GROUP" --query "state" -o tsv 2>/dev/null)
    
    if [ "$state" = "Running" ]; then
        print_success "Webapp is running."
        return 0
    else
        print_error "Webapp is not running. Current state: $state"
        return 1
    fi
}

check_aspnetcore_environment() {
    print_status "Checking ASP.NET Core environment..."
    
    local env_value=$(az webapp config appsettings list \
        --name "$APP_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --query "[?name=='ASPNETCORE_ENVIRONMENT'].value" \
        -o tsv 2>/dev/null)
    
    if [ "$env_value" = "Production" ]; then
        print_success "ASPNETCORE_ENVIRONMENT is set to Production."
        return 0
    else
        print_error "ASPNETCORE_ENVIRONMENT is not set correctly. Current value: $env_value"
        return 1
    fi
}

check_database_connection_string() {
    print_status "Checking database connection string..."
    
    local conn_string=$(az webapp config connection-string list \
        --name "$APP_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --query "[?name=='DefaultConnection'].value" \
        -o tsv 2>/dev/null)
    
    if [[ "$conn_string" == *"books-crud-db.database.windows.net"* ]] && [[ "$conn_string" == *"Active Directory Default"* ]]; then
        print_success "Database connection string is configured correctly."
        return 0
    else
        print_error "Database connection string is not configured correctly."
        return 1
    fi
}

check_app_insights_connection_string() {
    print_status "Checking Application Insights connection string..."
    
    local app_insights_conn=$(az webapp config appsettings list \
        --name "$APP_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --query "[?name=='APPLICATIONINSIGHTS_CONNECTION_STRING'].value" \
        -o tsv 2>/dev/null)
    
    if [ -n "$app_insights_conn" ] && [[ "$app_insights_conn" == *"InstrumentationKey="* ]]; then
        print_success "Application Insights connection string is configured."
        return 0
    else
        print_warning "Application Insights connection string is not configured."
        return 1
    fi
}

check_managed_identity() {
    print_status "Checking managed identity..."
    
    local principal_id=$(az webapp identity show \
        --name "$APP_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --query "principalId" \
        -o tsv 2>/dev/null)
    
    if [ -n "$principal_id" ] && [ "$principal_id" != "null" ]; then
        print_success "Managed identity is enabled."
        return 0
    else
        print_error "Managed identity is not enabled."
        return 1
    fi
}

test_api_endpoints() {
    print_status "Testing API endpoints..."
    
    local base_url="https://$APP_NAME.azurewebsites.net"
    local api_test=$(curl -fs "$base_url/api/books" 2>/dev/null)
    local swagger_test=$(curl -fs "$base_url/swagger/v1/swagger.json" 2>/dev/null)
    
    if [ -n "$api_test" ]; then
        print_success "API endpoint /api/books is responding."
    else
        print_error "API endpoint /api/books is not responding."
        return 1
    fi
    
    if [ -n "$swagger_test" ]; then
        print_success "Swagger endpoint is accessible."
    else
        print_warning "Swagger endpoint is not accessible."
    fi
    
    return 0
}

show_health_summary() {
    echo
    echo -e "${BLUE}================================${NC}"
    echo -e "${BLUE}  📊 Health Check Summary${NC}"
    echo -e "${BLUE}================================${NC}"
    echo
    
    if [ $1 -eq 0 ]; then
        print_success "🎉 All health checks passed!"
        echo -e "${GREEN}Your Books CRUD API is healthy and ready to use.${NC}"
    else
        print_error "❌ Some health checks failed."
        echo -e "${YELLOW}Please review the failed checks above.${NC}"
    fi
    
    echo
    echo -e "${BLUE}Access your app:${NC}"
    echo -e "  🌐 API Base   : ${BLUE}https://$APP_NAME.azurewebsites.net${NC}"
    echo -e "  📚 Swagger UI : ${BLUE}https://$APP_NAME.azurewebsites.net/swagger${NC}"
    echo
}

main() {
    echo -e "${BLUE}================================${NC}"
    echo -e "${BLUE}  🔍 Books CRUD API Health Check${NC}"
    echo -e "${BLUE}================================${NC}"
    echo
    
    local exit_code=0
    
    check_azure_cli
    
    # Run all health checks
    check_webapp_status || exit_code=1
    check_aspnetcore_environment || exit_code=1
    check_database_connection_string || exit_code=1
    check_app_insights_connection_string || exit_code=1
    check_managed_identity || exit_code=1
    test_api_endpoints || exit_code=1
    
    show_health_summary $exit_code
    
    exit $exit_code
}

main "$@" 