using PracticeProject_Dotnet.Services.Interfaces;

namespace PracticeProject_Dotnet.Services;

public class EmailNotification : INotificationProvider
{
    public string SendNotification(string message)
    {
        return $"The email triggered with this message - {message}";
    }
}