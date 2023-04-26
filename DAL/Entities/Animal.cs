namespace DAL.Entities
{
    public class Animal : AquariumItem
    {
        public DateTime DeathDate { get; set; } = DateTime.MinValue;

        public Boolean IsAlive { get; set; } = true;

    }
}
