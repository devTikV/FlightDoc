using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class Passenger
    {
        [Key]
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

        public ICollection<FlightPassenger> FlightPassengers { get; set; }
    }

}