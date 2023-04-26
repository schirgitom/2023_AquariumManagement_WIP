namespace DAL.Entities
{
    public class Picture : Entity
    {
        public DateTime Uploaded { get; set; }
        public String Aquarium { get; set; }
        public String Description { get; set; }
        public String ContentType { get; set; }

        public String PictureID { get; set; }
    }
}
