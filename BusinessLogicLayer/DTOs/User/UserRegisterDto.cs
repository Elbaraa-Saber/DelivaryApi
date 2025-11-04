namespace BusinessLogicLayer.DTOs.User
{
    public class UserRegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
