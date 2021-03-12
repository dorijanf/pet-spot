namespace PetSpot.API.Configuration
{
    /// <summary>
    /// Class that defines neccessary jwt bearer properties
    /// </summary>
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
