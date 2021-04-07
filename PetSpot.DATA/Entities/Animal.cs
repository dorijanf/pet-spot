using System;

namespace PetSpot.DATA.Entities
{
    public class Animal : IDeletable, ITimestamps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public byte Age { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public int SpeciesId { get; set; }
        public User User { get; set; }
        public Location Location { get; set; }
        public Species Species { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
