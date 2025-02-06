namespace DemoDotNetCoreApplication.Dtos
{
    public class RegisterModelDto
    {
        public string? userName { get; set; }

        public string? email { get; set; }

        public List<string>? roles { get; set; }

        public List<ClaimDto>? claims { get; set; }

        public string? phoneNumber { get; set; }

        public string? practice { get; set; }

        public string? password { get; set; }

        public string? displayName { get; set; }

        public string? question1 { get; set; }

        public string? answer1 { get; set; }

        public string? question2 { get; set; }

        public string? answer2 { get; set; }

        public string? question3 { get; set; }

        public string? answer3 { get; set; }

    }
}
