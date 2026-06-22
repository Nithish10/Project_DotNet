using PracticeProject_Dotnet.Services.Interfaces;

namespace PracticeProject_Dotnet.Services;

public class SMSNotification : INotificationProvider
{
    public string SendNotification(string message)
    {
        return $"The SMS triggered with this message - {message}";
    }
}