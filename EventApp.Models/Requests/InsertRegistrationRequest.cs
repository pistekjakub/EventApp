namespace EventApp.Models.Requests
{
    public class InsertRegistrationRequest
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string EventName { get; set; }
    }
}
