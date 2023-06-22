using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDoc.Model
{
    public class Passenger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PassengerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(12)]
        public string CCCD { get; set; }

        [MaxLength(20)]
        public string Passport { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        // Thêm các trường thông tin khác về hành khách
        public bool active { get; set; } = false;
        public ICollection<FlightPassenger> FlightPassengers { get; set; }
    }

}