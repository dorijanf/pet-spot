using System;

namespace PetSpot.DATA.Entities
{
    interface ITimestamps
    {
        DateTime UpdatedAt { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
