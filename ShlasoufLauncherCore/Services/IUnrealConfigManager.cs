using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ShlasoufLauncherCore.Models.Services
{
    public interface IUnrealConfigManager
    {
        public Task setPropertiesForConfigAsync(List<UnrealConfigProperty> properties, string configFilePath);

        public Task<List<UnrealConfigProperty>> getPropertiesFromConfigAsync(List<UnrealConfigProperty> properties, string configFilePath);

        public Task SetOptions(UnrealShlasoufOptions options);
        public Task<UnrealShlasoufOptions> GetOptions();
        public Task SetDefaultOptions();
    }
}
