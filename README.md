
# Virtual Vending Machine API

A RESTful backend system for managing virtual vending machines, shelves, inventory, and product purchases.

## Technologies

- C# / ASP.NET Core Web API (.NET 10)
- In-Memory Storage
- xUnit for unit testing
- OpenAPI / Swagger UI

## Getting Started

Requirements:
- .NET 10 SDK

Run the API from the solution directory:

```bash
dotnet run --project VendingMachine.Api --launch-profile https
```

Open Swagger UI:

https://localhost:7203/swagger

## Running Tests

```bash
dotnet test
```

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | /api/machines | Create a machine |
| GET | /api/machines | Get all machines |
| GET | /api/machines/{id} | Get a machine |
| POST | /api/machines/{machineId}/shelves | Add a shelf |
| GET | /api/machines/{machineId}/shelves/{shelfId} | Get a shelf |
| POST | /api/machines/{machineId}/shelves/{shelfId}/inventory | Load inventory |
| POST | /api/machines/{machineId}/shelves/{shelfId}/purchase | Purchase products (bonus) |

## Business Rules

- Each machine has a maximum number of shelves.
- Machines are created without shelves.
- Each shelf supports one product category.
- Only products matching the shelf category can be loaded.
- Total inventory cannot exceed shelf capacity.
- Inventory quantities must be positive.
- Purchases decrease inventory and fail when stock is insufficient.

## Architecture

- Controllers: HTTP endpoints and responses.
- Services: Application operations and orchestration.
- Domain: Entities and business rules.
- Repositories: Data access abstraction and in-memory storage.
- DTOs: API request and response contracts.
- Tests: Unit tests for business rules.

## Design Decisions

- Business rules are enforced inside domain entities.
- Repository abstraction allows future database integration.
- A global exception handler provides consistent HTTP error responses.
- Product categories are represented by strings, allowing new categories without changing business logic.
- Swagger UI is provided for API testing without a separate frontend.

## Assumptions

- A shelf can contain different products belonging to the same category.
- Product codes identify products within a shelf.
- Shelf capacity represents the total number of product units.
- Product information is supplied when loading inventory; no separate product catalog is implemented.
- Authentication and payment processing are outside the assignment scope.
- Data is stored in memory and is lost when the application restarts.
- Single-process inventory and shelf updates are synchronized with locks. Multi-instance deployments and database consistency require additional concurrency controls.

## Open Questions

1. Can a shelf contain multiple products of the same category?
2. Should product categories be predefined or managed dynamically?
3. Should administrators be able to update or remove shelves?
4. Should inventory operations support concurrent requests?
5. Should product details be managed in a central catalog?
6. Are stock transfers between machines required?
7. What authentication and authorization rules are required for administrators?

## Concurrency

- A machine-level lock makes adding shelves atomic, including the maximum-shelves check.
- A shelf-level lock makes inventory loading and purchasing atomic, including capacity and stock checks.
- Read operations return snapshots, preventing enumeration of mutable collections during concurrent changes.
- The in-memory locks apply only within a single server process; a distributed deployment would need database-level concurrency control.

## Future Improvements

- Database persistence with Entity Framework Core.
- Authentication and authorization.
- Database transactions / optimistic concurrency for multi-instance deployments.
- Centralized product catalog.
- Integration tests and structured logging.
