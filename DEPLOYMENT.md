# Books CRUD API Deployment Guide

This guide explains how to deploy the Books CRUD API to Azure App Service using the provided deployment scripts.

## Prerequisites

Before running the deployment scripts, ensure you have:

1. **Azure CLI** installed and configured
2. **.NET 8.0 SDK** installed
3. **Azure subscription** with access to the required resources
4. **Azure resources** already created:
   - Resource Group: `api-load-testing`
   - App Service Plan: `books-crud-api`
   - SQL Server: `books-crud-db`
   - Database: `BooksCrudApi.Database`

## Deployment Scripts

### 1. Full Deployment Script (`deploy.sh`)

This is a comprehensive deployment script that handles the entire deployment process:

```bash
./deploy.sh
```

**What it does:**
- ✅ Checks prerequisites (Azure CLI, .NET SDK)
- ✅ Verifies Azure login status
- ✅ Cleans previous build artifacts
- ✅ Builds and publishes the application
- ✅ Creates deployment package
- ✅ Deploys to Azure App Service
- ✅ Configures application settings
- ✅ Sets up database connection string
- ✅ Ensures managed identity is configured
- ✅ Tests the deployed application
- ✅ Displays deployment information

**Use this script when:**
- First-time deployment
- Infrastructure changes
- Need full validation and testing

### 2. Quick Deploy Script (`quick-deploy.sh`)

This is a simplified script for fast deployments:

```bash
./quick-deploy.sh
```

**What it does:**
- ✅ Builds and publishes the application
- ✅ Creates deployment package
- ✅ Deploys to Azure App Service

**Use this script when:**
- Code changes only
- Infrastructure is already configured
- Need fast deployment

## Manual Deployment Steps

If you prefer to deploy manually, follow these steps:

### Step 1: Build and Publish

```bash
# Clean previous builds
rm -rf ./publish

# Build and publish
dotnet publish -c Release -o ./publish
```

### Step 2: Create Deployment Package

```bash
# Create zip file
cd ./publish
zip -r ../books-crud-api-deploy.zip .
cd ..
```

### Step 3: Deploy to Azure

```bash
# Deploy to App Service
az webapp deploy \
  --resource-group api-load-testing \
  --name books-crud-api-app \
  --src-path books-crud-api-deploy.zip
```

### Step 4: Configure Settings (if needed)

```bash
# Set environment
az webapp config appsettings set \
  --name books-crud-api-app \
  --resource-group api-load-testing \
  --settings ASPNETCORE_ENVIRONMENT=Production

# Set connection string
az webapp config connection-string set \
  --name books-crud-api-app \
  --resource-group api-load-testing \
  --settings DefaultConnection="Server=tcp:books-crud-db.database.windows.net,1433;Initial Catalog=BooksCrudApi.Database;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";" \
  --connection-string-type SQLAzure
```

## Configuration

### Environment Variables

The application uses these environment variables:

- `ASPNETCORE_ENVIRONMENT`: Set to `Production` for production deployment
- `DefaultConnection`: Azure SQL Database connection string

### Database Configuration

The application connects to:
- **Server**: `books-crud-db.database.windows.net`
- **Database**: `BooksCrudApi.Database`
- **Authentication**: Azure Active Directory (Managed Identity)

## Testing the Deployment

After deployment, test your application:

### API Endpoints

```bash
# Get all books
curl -X GET "https://books-crud-api-app.azurewebsites.net/api/books"

# Create a book
curl -X POST "https://books-crud-api-app.azurewebsites.net/api/books" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test Book",
    "author": "Test Author",
    "description": "A test book",
    "isbn": "978-1234567890",
    "publisher": "Test Publisher",
    "publicationYear": 2024,
    "pageCount": 300,
    "genre": "Fiction",
    "language": "English",
    "price": 19.99,
    "isAvailable": true,
    "coverImageUrl": "https://example.com/cover.jpg"
  }'
```

### Swagger UI

Access the interactive API documentation at:
```
https://books-crud-api-app.azurewebsites.net/swagger
```

## Troubleshooting

### Common Issues

1. **Authentication Errors**
   - Ensure managed identity is enabled on the App Service
   - Verify the managed identity has access to the SQL Database

2. **Connection String Issues**
   - Check if the connection string is properly configured
   - Verify the SQL Server and database exist

3. **Deployment Failures**
   - Check Azure CLI login status
   - Verify resource group and app service exist
   - Ensure sufficient permissions

### Logs

View application logs:

```bash
# Stream logs
az webapp log tail --name books-crud-api-app --resource-group api-load-testing

# Download logs
az webapp log download --name books-crud-api-app --resource-group api-load-testing
```

## Security Considerations

- The application uses Azure AD authentication for database access
- Managed identity provides secure authentication without storing credentials
- HTTPS is enforced for all connections
- Connection strings are encrypted in Azure App Service configuration

## Cost Optimization

- The App Service Plan is set to B1 tier (Basic)
- Consider scaling down during development/testing
- Monitor usage in Azure Portal

## Support

For issues with:
- **Azure Resources**: Check Azure Portal and Azure CLI
- **Application Code**: Review application logs and error messages
- **Database**: Verify SQL Server configuration and permissions 