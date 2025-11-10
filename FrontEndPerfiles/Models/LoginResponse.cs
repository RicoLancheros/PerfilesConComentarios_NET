namespace FrontEndPerfiles.Models
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public Perfil? Perfil { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
