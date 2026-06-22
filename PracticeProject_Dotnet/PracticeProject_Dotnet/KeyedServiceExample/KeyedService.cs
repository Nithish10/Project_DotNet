using PracticeProject_Dotnet.Services.Interfaces;

namespace PracticeProject_Dotnet.KeyedServiceExample
{
    public interface IKeyedService
    {
        string KeyedServiceExample(string notificationType);
    }

    public class KeyedService : IKeyedService
    {
        private readonly IKeyedServiceProvider serviceProvider;

        public KeyedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider as IKeyedServiceProvider;
        }

        public string KeyedServiceExample(string notificationType)
        {
            var service = serviceProvider.GetRequiredKeyedService<INotificationProvider>(notificationType);

            return service.SendNotification("Hello");
        }
    }
}