using System;
using System.Collections.Generic;

namespace PetSpot.DATA.Entities
{
    public class Species: ITimestamps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Animal> Animals { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
