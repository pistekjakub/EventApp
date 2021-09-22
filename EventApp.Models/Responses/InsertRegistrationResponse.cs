namespace EventApp.Models.Responses
{
    public class InsertRegistrationResponse
    {
        public bool Inserted { get; set; } = false;

        public string Error { get; set; } = string.Empty;
    }
}
