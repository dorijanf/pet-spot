using PetSpot.DATA.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetSpot.DATA.Models
{
    public class SyncDto
    {
        public List<Animal> Animals { get; set; }
        public List<Species> Species { get; set; }
        public List<Location> Locations { get; set; }
    }
}
