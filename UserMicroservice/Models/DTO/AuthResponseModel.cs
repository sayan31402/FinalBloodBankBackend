namespace UserMicroservice.Models.DTO
{
    public class AuthResponseModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
