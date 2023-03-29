using DAL.Entities;

namespace Services.Models.Response
{
    public class AquariumUserResponse
    {
        public Aquarium Aquarium { get; set; }

        public UserRole Role { get; set; }
    }
}
