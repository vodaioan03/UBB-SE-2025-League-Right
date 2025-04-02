# Duo - Interactive Language Learning Platform

Duo is a modern Windows application designed to make language learning engaging and effective. Built with WinUI 3, it offers a variety of interactive exercises and a structured learning roadmap.

## Features

### Exercise Types

- **Multiple Choice**: Test your knowledge with carefully crafted multiple-choice questions
- **Fill in the Blanks**: Practice sentence completion with contextual clues
- **Association Exercises**: Match related items to reinforce connections
- **Flashcards**: Quick review with timed flashcards featuring:
  - Difficulty-based timer (Easy: 15s, Normal: 30s, Hard: 45s)
  - Visual progress indicator
  - Immediate feedback
  - Answer validation

### Learning Roadmap

- Structured learning paths
- Progress tracking
- Difficulty levels (Easy, Normal, Hard)
- Section-based organization
- Quiz previews and completion tracking

### User Interface

- Modern WinUI 3 design
- Responsive layout
- Visual feedback for answers
- Progress indicators
- Difficulty-based styling
- Accessibility support

## Technical Details

### Architecture

- MVVM pattern implementation
- Dependency injection
- Service-based architecture
- Event-driven communication

### Key Components

- **Views**: User interface components
- **ViewModels**: Business logic and state management
- **Models**: Data structures and exercise definitions
- **Services**: Data access and business operations

### Exercise System

- Modular exercise types
- Extensible validation system
- Real-time feedback
- Progress tracking
- Difficulty scaling

## Code Structure

The project is organized into the following folders:

- **Assets**: Contains assets, though it is lightly used.
- **Commands**: Houses command classes used throughout the application.
- **Converter**: Contains converter classes to transform data between different formats.
- **Data**: Stores all required SQL scripts. Inside the `Schema` subfolder, all necessary procedures are merged into three main files:
  - `database_schema.sql`
  - `all_procedures.sql`
  - `dummy_data.sql`
- **Helpers**: Includes static classes providing utility methods for various parts of the application.
- **Models**: Defines the base models related to exercises, quizzes, exams, sections, and the learning roadmap.
- **Repositories**: Contains classes that bridge business logic and database interactions.
- **Services**: Implements logic that connects `ViewModels` to `Repositories`.
- **ViewModels**: Manages the logic behind the user interface, handling user input, state management, and business logic.
- **Views**: Defines the UI components and layouts users interact with.

## Getting Started

### Prerequisites

- Windows 10 or later
- Visual Studio 2022 or later
- .NET 6.0 or later
- Windows App SDK
- SQL Server (or compatible database)

### Database Setup

1. Create a database in SQL Server (we recommend naming it `duolingo_test`, but any name is fine).
2. Run the following SQL scripts in order:
   - `Duo/Data/Schema/database_schema.sql`
   - `Duo/Data/Schema/all_procedures.sql`
   - `Duo/Data/Schema/dummy_data.sql`
3. Create a connection string.

#### Example Connection Strings

For Windows authentication:

```plaintext
Server=localhost;Database=duolingo_test;Trusted_Connection=True;TrustServerCertificate=True;
```

For SQL Server authentication with a dummy user:

```plaintext
Server=localhost;Database=duolingo_test;User Id=dummy_user;Password=dummy_password;TrustServerCertificate=True;
```

4. Add the connection string to an `appsettings.json` file in the root of the project:

```json
{
    "DbConnection": "your_connection_string_here"
}
```

5. Replace `your_connection_string_here` with your actual connection string.
6. Ensure the database is running and accessible.

### Installation

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Restore NuGet packages.
4. Set up the database connection string in `appsettings.json`.
5. Build and run the application.

### Development

- Follow the MVVM pattern.
- Use dependency injection for services.
- Implement new exercises by extending the base `Exercise` class.
- Add new features through the existing service architecture.

## Contributing

1. Fork the repository.
2. Create a feature branch.
3. Commit your changes.
4. Push to the branch.
5. Create a Pull Request.

