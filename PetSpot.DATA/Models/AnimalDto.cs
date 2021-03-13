using PetSpot.DATA.Entities;

namespace PetSpot.DATA.Models
{
    public class AnimalDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public byte Age { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public int SpeciesId { get; set; }
        public LocationBm Location { get; set; }
    }
}
