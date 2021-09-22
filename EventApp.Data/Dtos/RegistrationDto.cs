namespace EventApp.Data.Dtos
{
    public class RegistrationDto
    {
        public long Id { get; set; } = (long)decimal.MinusOne;

        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
