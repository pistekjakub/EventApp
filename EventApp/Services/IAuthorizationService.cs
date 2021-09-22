namespace EventApp.Services
{
    public interface IAuthorizationService
    {
        bool IsEventCreator(string login, string password);
    }
}
