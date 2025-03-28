using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Duo.Core.Data;
using Duo.Core.Models;
using Duo.Core.Repositories;

namespace Duo.ConsoleTest;

class Program
{
    private static IServiceProvider _serviceProvider = null!;

    static async Task Main(string[] args)
    {
        ConfigureServices();
        await RunApplicationLoop();
    }

    private static void ConfigureServices()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<DatabaseConnection>();
        services.AddSingleton<UserRepository>();
        
        _serviceProvider = services.BuildServiceProvider();
    }

    private static async Task RunApplicationLoop()
    {
        var db = _serviceProvider.GetRequiredService<DatabaseConnection>();

        while (true)
        {
            Console.WriteLine("\n=== Database Testing Menu ===");
            Console.WriteLine("1. Test Connection");
            Console.WriteLine("2. Run Test Query");
            Console.WriteLine("3. Test User Repository");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();
            
            try
            {
                switch (choice)
                {
                    case "1":
                        await TestConnection(db);
                        break;
                    case "2":
                        await RunTestQuery(db);
                        break;
                    case "3":
                        var userRepository = _serviceProvider.GetRequiredService<UserRepository>();
                        await TestUserRepository(userRepository);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static async Task TestConnection(DatabaseConnection db)
    {
        // INFO: using the "using" keyword to ensure the connection is disposed of after use
        using var connection = db.CreateConnection();
        await connection.OpenAsync();
        Console.WriteLine("Connection successful!");
    }

    private static async Task RunTestQuery(DatabaseConnection db)
    {
        using var connection = db.CreateConnection();
        await connection.OpenAsync();
        
        // Example query - modify as needed
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM YourTable";
        
        var result = await command.ExecuteScalarAsync();
        Console.WriteLine($"Query result: {result}");
    }

    private static async Task TestUserRepository(UserRepository userRepository)
    {
        // get the user by username
        var user = await userRepository.GetByUsernameAsync("testuser");
        Console.WriteLine($"User: {user.Username}, LastCompletedSectionId: {user.LastCompletedSectionId}");

        // update the user
        user.LastCompletedSectionId = 1;
        await userRepository.UpdateUserSectionProgressAsync(user, user.LastCompletedSectionId);

        // get the user by username again
        var user_get = await userRepository.GetByUsernameAsync("testuser");
        Console.WriteLine($"User: {user_get.Username}, LastCompletedSectionId: {user_get.LastCompletedSectionId}");
    }
}