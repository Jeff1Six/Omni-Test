[Back to README](../README.md)

### Sales

#### GET /sales
- Description: Retrieve a list of all sales
- Query Parameters:
  - `_page` (optional): Page number for pagination (default: 1)
  - `_size` (optional): Number of items per page (default: 10)
  - `_order` (optional): Ordering of results (e.g., "id desc, saleNumber asc")
  - `_cancelled` (optional): Filter cancelled sales (`true` or `false`)
- Response: 
  ```json
  {
    "data": [
      {
        "id": "string (guid)",
        "saleNumber": "integer",
        "date": "string (date)",
        "customerId": "string (guid)",
        "branchId": "string (guid)",
        "totalAmount": "number",
        "cancelled": "boolean",
        "items": [
          {
            "productId": "string (guid)",
            "quantity": "integer",
            "unitPrice": "number",
            "discount": "number",
            "totalItemAmount": "number",
            "cancelled": "boolean"
          }
        ]
      }
    ],
    "totalItems": "integer",
    "currentPage": "integer",
    "totalPages": "integer"
  }
#### POST /sales
- Description: Add a new sale

- Request Body:
```json
{
  "date": "string (date)",
  "customerId": "string (guid)",
  "branchId": "string (guid)",
  "items": [
    {
      "productId": "string (guid)",
      "quantity": "integer",
      "unitPrice": "number"
    }
  ]
}
Response:

{
  "id": "string (guid)",
  "saleNumber": "integer",
  "date": "string (date)",
  "customerId": "string (guid)",
  "branchId": "string (guid)",
  "totalAmount": "number",
  "cancelled": "boolean",
  "items": [
    {
      "productId": "string (guid)",
      "quantity": "integer",
      "unitPrice": "number",
      "discount": "number",
      "totalItemAmount": "number",
      "cancelled": "boolean"
    }
  ]
}
```
#### GET /sales/{id}
- Description: Retrieve a specific sale by ID

- Path Parameters:

- id: Sale ID
```json
Response:

{
  "id": "string (guid)",
  "saleNumber": "integer",
  "date": "string (date)",
  "customerId": "string (guid)",
  "branchId": "string (guid)",
  "totalAmount": "number",
  "cancelled": "boolean",
  "items": [
    {
      "productId": "string (guid)",
      "quantity": "integer",
      "unitPrice": "number",
      "discount": "number",
      "totalItemAmount": "number",
      "cancelled": "boolean"
    }
  ]
}
```
#### PUT /sales/{id}
- Description: Update a specific sale

- Path Parameters:

- id: Sale ID
```json
Request Body:

{
  "date": "string (date)",
  "customerId": "string (guid)",
  "branchId": "string (guid)",
  "items": [
    {
      "productId": "string (guid)",
      "quantity": "integer",
      "unitPrice": "number"
    }
  ]
}
Response:

{
  "id": "string (guid)",
  "saleNumber": "integer",
  "date": "string (date)",
  "customerId": "string (guid)",
  "branchId": "string (guid)",
  "totalAmount": "number",
  "cancelled": "boolean",
  "items": [
    {
      "productId": "string (guid)",
      "quantity": "integer",
      "unitPrice": "number",
      "discount": "number",
      "totalItemAmount": "number",
      "cancelled": "boolean"
    }
  ]
}
```
#### DELETE /sales/{id}
- Description: Cancel a specific sale (soft delete)

- Path Parameters:

- id: Sale ID
```json
Response:

{
  "message": "string"
}
```
####DELETE /sales/{id}/items/{itemId}
- Description: Cancel a specific sale item (soft delete)

- Path Parameters:

- id: Sale ID

- itemId: Sale Item ID

```json
Response:

{
  "message": "string"
}
```

- Business Rules (Discounts)
- Purchases above 4 identical items have a 10% discount

- Purchases between 10 and 20 identical items have a 20% discount

- It's not possible to sell above 20 identical items

- Purchases below 4 items cannot have a discount

<br> <div style="display: flex; justify-content: space-between;"> <a href="./users-api.md">Previous: Users API</a> <a href="./products-api.md">Next: Products API</a> </div> ```