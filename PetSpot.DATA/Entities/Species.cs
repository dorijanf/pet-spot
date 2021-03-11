using System.Collections.Generic;

namespace PetSpot.DATA.Entities
{
    public class Species
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Animal> Animals { get; set; }
    }
}
