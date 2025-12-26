# InfoWise UserManagementService

A C# .NET 8 Web API responsible for managing user identities, authentication via JWT, and user preferences regarding news categories. This service persists data using PostgreSQL (via Entity Framework Core) and publishes integration events to RabbitMQ whenever user preferences are updated, ensuring decoupled communication with other microservices.

This project is part of InfoWise.

## Project Structure

-   `UserManagementService.sln`: Solution file.
-   `UserManagementService/`: Main project directory.
    -   `Models/`: Entity definitions including `User` and `UserPreference`.
    -   `Controllers/UsersController.cs`: API endpoint to manage user resources.
    -   `Application/Services/UserAppService.cs`: Encapsulates business logic and orchestrates event publishing to RabbitMQ.
    -   `Infrastructure/Data/ApplicationDbContext.cs`: EF Core context configuration for PostgreSQL.
    -   `Infrastructure/Repositories/UserRepository.cs`: Handles data access operations for users and preferences.
    -   `Infrastructure/RabbitMQ/`: Contains `Producer` logic and connection configuration for the message bus.
    -   `Infrastructure/JWT/`: Logic for generating and handling JSON Web Tokens.
    -   `Program.cs`: Configures the application, registers services (DI), and defines the HTTP request pipeline.

## Getting Started

### Prerequisites

-   .NET SDK 8.0
-   PostgreSQL Database
-   RabbitMQ Server (local or containerized)

### Setup

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/arieloou/infowise-usermanagementservice.git](https://github.com/arieloou/infowise-usermanagementservice.git)
    cd UserManagementService
    ```

2.  **Configure Environment:**
    Update `appsettings.json` with your specific configuration:
    -   **ConnectionStrings**: Update `DefaultConnection` with your PostgreSQL credentials.
    -   **RabbitMQConfiguration**: Set your HostName, Username, and Password.
    -   **JwtSettings**: Ensure your issuer and audience settings are correct (and ensure `private.pem` exists for RSA signing).

3.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

4.  **Apply Migrations:**
    Create the database schema in PostgreSQL:
    ```bash
    dotnet ef database update
    ```

### Running the Application

1.  **Build the project:**
    ```bash
    dotnet build
    ```

2.  **Run the application:**
    ```bash
    dotnet run --project UserManagementService
    ```
    The application will start, typically listening on `http://localhost:5067` (or ports configured in `launchSettings.json`).

### API Endpoints

Once the application is running, you can interact with the API via Swagger or HTTP requests.

-   **POST /users/{userId}/preferences**
    Updates the list of news categories a user is subscribed to. This action triggers a `UserPreferencesUpdatedEvent` to RabbitMQ.

    **Body:** `[int]` (Array of Category IDs)

    Example using `curl`:
    ```bash
    curl -X POST http://localhost:5067/users/GUID-GOES-HERE/preferences \
    -H "Content-Type: application/json" \
    -d "[1, 5, 12]"
    ```

### Event Driven Architecture

This service acts as a **Producer** in the InfoWise architecture.

1.  **Event:** `UserPreferencesUpdatedEvent`
2.  **Trigger:** When `SetUserPreferences` is called.
3.  **Exchange:** `user.exchange`
4.  **Routing Key:** `preferences.configured`

Services like `NewsProcessorService` can subscribe to this exchange to update their local registry of interested users.

## License
This project is distributed under the MIT License, as specified in the [LICENSE](LICENSE) file.
