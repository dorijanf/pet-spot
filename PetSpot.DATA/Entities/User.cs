using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace PetSpot.DATA.Entities
{
    public class User : IdentityUser
    {
        public ICollection<Animal> Animals { get; set; }
    }
}
