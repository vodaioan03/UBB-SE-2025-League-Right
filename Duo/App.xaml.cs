using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Duo.Data;
using Duo.ViewModels.ExerciseViewModels;
using Duo.Services;
using Duo.Repositories;
using Duo.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? serviceProvider;
        private Window? m_window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<DatabaseConnection>();

            services.AddSingleton<UserRepository>();
            services.AddSingleton<UserService>();

            services.AddSingleton<ExerciseRepository>();
            services.AddSingleton<ExerciseService>();

            services.AddSingleton<ExamRepository>();
            services.AddSingleton<QuizRepository>();
            services.AddSingleton<QuizService>();

            services.AddSingleton<SectionRepository>();
            services.AddSingleton<SectionService>();

            services.AddSingleton<RoadmapRepository>();
            services.AddSingleton<RoadmapService>();

            services.AddTransient<FillInTheBlankExerciseViewModel>();
            services.AddTransient<MultipleChoiceExerciseViewModel>();
            services.AddTransient<AssociationExerciseViewModel>();
            services.AddTransient<ExerciseCreationViewModel>();
            services.AddTransient<QuizExamViewModel>();
            services.AddTransient<CreateQuizViewModel>();


            serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }
    }
}
