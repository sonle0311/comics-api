# Comics API

A comprehensive .NET Core Web API for managing comics, manga, categories, and chapters with advanced crawler functionality.

## 🚀 Features

- **Clean Architecture**: Follows Clean Architecture principles with clear separation of concerns
- **CQRS Pattern**: Implements Command Query Responsibility Segregation using MediatR
- **Entity Framework Core**: Database operations with Code First approach
- **AutoMapper**: Object-to-object mapping
- **Serilog**: Structured logging
- **Swagger/OpenAPI**: API documentation
- **CORS Support**: Cross-origin resource sharing
- **Crawler Service**: Automated content crawling functionality

## 🏗️ Architecture

```
src/
├── ComicsApi.Domain/          # Domain entities and interfaces
├── ComicsApi.Application/     # Application logic, DTOs, and handlers
├── ComicsApi.Infrastructure/  # Data access, repositories, and external services
└── ComicsApi.WebAPI/         # API controllers and configuration
```

## 📦 Tech Stack

- **.NET 9.0**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **MediatR**
- **AutoMapper**
- **Serilog**
- **Swagger/OpenAPI**

## 🛠️ Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository:
```bash
git clone https://github.com/sonle0311/comics-api.git
cd comics-api
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Update database connection string in `appsettings.json`

4. Run database migrations:
```bash
dotnet ef database update --project src/ComicsApi.Infrastructure --startup-project src/ComicsApi.WebAPI
```

5. Build and run the application:
```bash
dotnet build
dotnet run --project src/ComicsApi.WebAPI
```

6. Navigate to `http://localhost:5000` to access the API
7. Visit `http://localhost:5000/swagger` for API documentation

## 📚 API Endpoints

### Categories
- `GET /api/categories` - Get all categories
- `GET /api/categories/{slug}` - Get category by slug
- `POST /api/categories` - Create new category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category

### Manga
- `GET /api/manga` - Get all manga with pagination
- `GET /api/manga/{slug}` - Get manga by slug
- `POST /api/manga` - Create new manga
- `PUT /api/manga/{id}` - Update manga
- `DELETE /api/manga/{id}` - Delete manga

### Chapters
- `GET /api/chapters` - Get all chapters
- `GET /api/chapters/{id}` - Get chapter by ID
- `GET /api/manga/{mangaSlug}/chapters` - Get chapters by manga
- `POST /api/chapters` - Create new chapter
- `PUT /api/chapters/{id}` - Update chapter
- `DELETE /api/chapters/{id}` - Delete chapter

### Crawler
- `POST /api/crawler/crawl-manga` - Crawl manga data
- `GET /api/crawler/logs` - Get crawl logs

## 🔧 Configuration

### Database Connection
Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ComicsApiDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Logging
Logs are configured with Serilog and written to:
- Console (Development)
- File: `Logs/log-.txt` (Production)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Son Le** - [sonle0311](https://github.com/sonle0311)

## 🙏 Acknowledgments

- Clean Architecture by Robert C. Martin
- CQRS pattern implementation
- Entity Framework Core team
- MediatR library