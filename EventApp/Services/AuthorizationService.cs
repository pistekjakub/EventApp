using System;

namespace EventApp.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public bool IsEventCreator(string login, string password)
        {
            if (string.IsNullOrEmpty(login)) 
            {
                throw new ArgumentException(nameof(login));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(nameof(password));
            }

            return login == "admin" && password == "niceday";
        }
    }
}
