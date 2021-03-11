namespace PetSpot.DATA.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public double CoordX { get; set; }
        public double CoordY { get; set; }
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }
    }
}
