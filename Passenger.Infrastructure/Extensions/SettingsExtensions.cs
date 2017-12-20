using Microsoft.Extensions.Configuration;

namespace Passenger.Infrastructure.Extensions
{
    public static class SettingsExtensions
    {
        public static T GetSettings<T>(this IConfiguration configuration) where T: new()//t musi być klasa z konstruktorem domyslnym
        {
            var section  = typeof(T).Name.Replace("Settings",string.Empty);
            var configurationValue = new T();
            configuration.GetSection(section).Bind(configurationValue);

            return configurationValue;
        }
    }
}