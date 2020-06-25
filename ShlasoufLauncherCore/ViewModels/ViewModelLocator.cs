using Microsoft.Extensions.DependencyInjection;
using ShlasoufLauncherCore.Models.Services;
using ShlasoufLauncherCore.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => App.ServiceProvider.GetRequiredService<MainViewModel>();

        public SetupViewModel SetupViewModel => App.ServiceProvider.GetRequiredService<SetupViewModel>();
        public StatusBarViewModel StatusBarViewModel => App.ServiceProvider.GetRequiredService<StatusBarViewModel>();

        public GameSettingsViewModel GameSettingsViewModel => new GameSettingsViewModel(App.ServiceProvider.GetService<IUnrealConfigManager>(), App.ServiceProvider.GetService<IScreenResolutionService>());

    }
}
