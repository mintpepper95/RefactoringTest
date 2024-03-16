using System.Configuration;

namespace Refactoring.LegacyService {
    public class ConfigurationManagerWrapper : IConfigurationManagerWrapper {
        public string GetConnectionString() {
            return ConfigurationManager.ConnectionStrings["applicationDatabase"].ConnectionString;
        }
    }
}
