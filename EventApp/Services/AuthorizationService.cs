namespace EventApp.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public bool IsEventCreator(string login, string password)
        {
            return login == "admin" && password == "niceday";
        }
    }
}
