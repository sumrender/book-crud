import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
  stages: [
    { duration: '2m', target: 1000 }, 
    { duration: '2m', target: 5000 },   
    { duration: '2m', target: 10000 },
    { duration: '2m', target: 15000 }, 
    { duration: '2m', target: 20000 }, 
    { duration: '2m', target: 0 }, 
  ],
  thresholds: {
    
  },
};

const BASE_URL = 'https://books-crud-api-app.azurewebsites.net';
const HEADERS_COMMON = {
  'accept': 'application/json',
};

export default function () {
  // 1. POST new book first
  const currentYear = new Date().getFullYear();
  const postPayload = JSON.stringify({
    title: `Test Book ${Date.now()}`,
    description: 'A test book created during load testing',
    author: 'Test Author',
    isbn: `978-${Math.floor(Math.random() * 999999999)}`,
    publisher: 'Test Publisher',
    publicationYear: currentYear - Math.floor(Math.random() * 10), // Random year in last 10 years
    pageCount: Math.floor(Math.random() * 500) + 100, // 100-600 pages
    genre: 'Fiction',
    language: 'English',
    price: Math.floor(Math.random() * 50) + 10, // $10-$60
    isAvailable: true,
    coverImageUrl: 'https://via.placeholder.com/300x400',
  });

  const postBookResponse = http.post(`${BASE_URL}/api/Books`, postPayload, {
    headers: {
      ...HEADERS_COMMON,
      'Content-Type': 'application/json',
    },
  });

  check(postBookResponse, {
    'POST book status is 201': (r) => r.status === 201,
    'POST book response time < 2000ms': (r) => r.timings.duration < 2000,
  });

  // 2. GET 50 books
  const getBooksPaginatedResponse = http.get(`${BASE_URL}/api/Books?skip=0&take=50`, {
    headers: HEADERS_COMMON,
  });

//   check(getBooksPaginatedResponse, {
//     'GET 50 books status is 200': (r) => r.status === 200,
//     'GET 50 books response time < 2000ms': (r) => r.timings.duration < 2000,
//   });

  // 3. Choose a random ID from the 50 books and get by ID
  let bookId = 'e1a73988-bba4-4cdb-b6fc-036092e00b8f'; // fallback ID
  if (getBooksPaginatedResponse.status === 200) {
    try {
      const responseBody = getBooksPaginatedResponse.json();
      // The API returns PaginatedResponseDto with a 'data' property containing the books
      const books = responseBody.data || [];
      if (books.length > 0) {
        const randomIndex = Math.floor(Math.random() * books.length);
        bookId = books[randomIndex].Id || bookId; // Note: C# property is 'Id' (capital I)
      }
    } catch (e) {
      console.log('Error parsing books response:', e);
    }
  }

  const getBookResponse = http.get(`${BASE_URL}/api/Books/${bookId}`, {
    headers: HEADERS_COMMON,
  });

  check(getBookResponse, {
    'GET book by ID status is 200': (r) => r.status === 200,
    'GET book by ID response time < 2000ms': (r) => r.timings.duration < 2000,
  });

  // 4. GET 10 books
  const getBooksResponse = http.get(`${BASE_URL}/api/Books?skip=0&take=10`, {
    headers: HEADERS_COMMON,
  });

//   check(getBooksResponse, {
//     'GET 10 books status is 200': (r) => r.status === 200,
//     'GET 10 books response time < 2000ms': (r) => r.timings.duration < 2000,
//   });
} 