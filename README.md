# Books CRUD API

A simple .NET Web API for managing books with SQL Server database.

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Docker and Docker Compose (for database)

### Running the Application

1. Start the SQL Server database:
   ```bash
   docker-compose up -d
   ```

2. The solution includes a SQL Server Database Project (`BooksCrudApi.Database`) that can be used to:
- Publish the database schema
- Deploy initial data
- Manage database changes

Use the Database Projects extension in Visual Studio Code to publish the database.


3. Run the application:
   ```bash
   dotnet run
   ```

4. Open your browser and navigate to:
   - API: http://localhost:5051/api/books
   - Swagger UI: http://localhost:5051/swagger

## API Endpoints

### GET /api/books
Get all books

### GET /api/books/{id}
Get a specific book by ID

### POST /api/books
Create a new book

**Example request body:**
```json
{
  "title": "The Hobbit",
  "description": "A fantasy novel about a hobbit's journey",
  "author": "J.R.R. Tolkien",
  "isbn": "978-0547928241",
  "publisher": "Houghton Mifflin Harcourt",
  "publicationYear": 1937,
  "pageCount": 366,
  "genre": "Fantasy",
  "language": "English",
  "price": 15.99,
  "isAvailable": true,
  "coverImageUrl": "https://example.com/hobbit-cover.jpg"
}
```

### PUT /api/books/{id}
Update an existing book

### DELETE /api/books/{id}
Delete a book

## Testing the API

You can test the API using:
- Swagger UI (available at /swagger)
- curl commands
- Postman or any HTTP client

### Example curl commands:

```bash
# Get all books
curl -X GET "http://localhost:5051/api/books"

# Get a specific book (replace {id} with actual GUID)
curl -X GET "http://localhost:5051/api/books/{id}"

# Create a new book
curl -X POST "http://localhost:5051/api/books" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "New Book",
    "description": "A new book description",
    "author": "New Author",
    "isbn": "978-1234567890",
    "publisher": "New Publisher",
    "publicationYear": 2024,
    "pageCount": 300,
    "genre": "Fiction",
    "language": "English",
    "price": 19.99,
    "isAvailable": true,
    "coverImageUrl": "https://example.com/new-book-cover.jpg"
  }'
```

## Notes

- Data is stored in SQL Server database
- The API uses GUIDs for book IDs
- All timestamps are in UTC
- The service is thread-safe with proper locking mechanisms
- Database schema and data can be managed through the Database Project 