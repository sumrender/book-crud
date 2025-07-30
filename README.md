# Books CRUD API

A simple .NET Web API for managing books with in-memory storage.

## Features

- Full CRUD operations for books
- In-memory data storage
- RESTful API design
- Swagger/OpenAPI documentation
- Data validation
- Sample data included

## Book Properties

The Book model includes 15 relevant properties:

- **Id** (Guid): Unique identifier
- **Title** (string): Book title (required, max 200 chars)
- **Description** (string): Book description (max 1000 chars)
- **Author** (string): Book author (required, max 100 chars)
- **ISBN** (string): International Standard Book Number (max 50 chars)
- **Publisher** (string): Book publisher (max 50 chars)
- **PublicationYear** (int): Year of publication
- **PageCount** (int): Number of pages
- **Genre** (string): Book genre (max 50 chars)
- **Language** (string): Book language (max 20 chars)
- **Price** (decimal): Book price
- **IsAvailable** (bool): Availability status
- **CreatedOn** (DateTime): Creation timestamp
- **UpdatedOn** (DateTime?): Last update timestamp
- **CoverImageUrl** (string): Cover image URL (max 500 chars)

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later

### Running the Application

1. Navigate to the project directory:
   ```bash
   cd BooksCrudApi
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Open your browser and navigate to:
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

## Sample Data

The API comes pre-loaded with 3 sample books:
- The Great Gatsby by F. Scott Fitzgerald
- To Kill a Mockingbird by Harper Lee
- 1984 by George Orwell

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

- Data is stored in memory and will be reset when the application restarts
- The API uses GUIDs for book IDs
- All timestamps are in UTC
- The service is thread-safe with proper locking mechanisms 