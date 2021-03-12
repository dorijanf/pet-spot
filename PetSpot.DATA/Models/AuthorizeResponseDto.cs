using System;

namespace PetSpot.DATA.Models
{
    public class AuthorizeResponseDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
