namespace InTimePro.Api.Dtos
{
    public class AuthResultDto
    {
        public UserDto User { get; set; }

        public TokenPairDto Tokens { get; set; }
    }
}
