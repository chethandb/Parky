using System.ComponentModel.DataAnnotations;
using static ParkyAPI.Models.Trail;

namespace ParkyAPI.Models.DTO
{
    public class TrailDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public double Distance { get; set; }

        public DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public NationalPark NationalPark { get; set; }
    }
}
