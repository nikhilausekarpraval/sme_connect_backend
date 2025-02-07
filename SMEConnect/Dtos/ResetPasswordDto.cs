namespace SMEConnect.Dtos
{
    public class ResetPasswordDto 
    {
        public string userName { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public string displayName { get; set; }

        public string question1 { get; set; }

        public string answer1 { get; set; }
        public string newPassword { get; set; }

        public string confirmPassword { get; set; }
    }
}
