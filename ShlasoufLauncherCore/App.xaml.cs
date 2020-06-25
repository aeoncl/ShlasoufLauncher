using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShlasoufLauncherCore.Models;
using ShlasoufLauncherCore.Models.Services;
using ShlasoufLauncherCore.Services;
using ShlasoufLauncherCore.ViewModels;
using ShlasoufLauncherCore.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ShlasoufLauncherCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            host = Host.CreateDefaultBuilder() 
            .ConfigureAppConfiguration((context, builder) =>
            {
                // Add other configuration files...
            }).ConfigureServices((context, services) =>
            {
                ConfigureServices(context.Configuration, services);
            })
            .ConfigureLogging(logging =>
            {
                // Add other loggers...
            })
            .Build();
            ServiceProvider = host.Services;

        }

        private void ConfigureServices(IConfiguration config, IServiceCollection services)
        {


            // Register all ViewModels.
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<SetupViewModel>();
            services.AddSingleton<StatusBarViewModel>();

            //Services
            services.AddScoped<IStatusBarUpdateService, StatusBarUpdateService>();
            services.AddScoped<IConsoleParamService, ConsoleParamService>();
            services.AddScoped<ILauncherSettingsService, LauncherSettingsService>();
            services.AddScoped<IFileDialogHelperService, FileDialogHelperService>();
            services.AddScoped<IGameDownloaderService, GameDownloaderService>();
            services.AddScoped<IMD5ComputeService, MD5ComputeService>();
            services.AddScoped<IShlasoufAPI, ShlasoufAPI>();
            services.AddScoped<IUnzipService, UnzipService>();
            services.AddScoped<IRegistryWriter, RegistryWriter>();
            services.AddScoped<IUnrealConfigManager, UnrealConfigManager>();
            services.AddScoped<IScreenResolutionService, ScreenResolutionService>();


            // Register all the Windows of the applications.
            services.AddTransient<MainWindow>();
            services.AddTransient<GameSettingsWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();
            var window = ServiceProvider.GetRequiredService<MainWindow>();
            window.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (host)
            {
                await host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }

    }
}
