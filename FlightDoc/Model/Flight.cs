using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Required]
        [MaxLength(10)]
        public string FlightNumber { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [MaxLength(50)]
        public string DepartureAirport { get; set; }

        [MaxLength(50)]
        public string ArrivalAirport { get; set; }

        // Thêm các trường thông tin khác về chuyến bay

        public ICollection<FlightPassenger> FlightPassengers { get; set; }
        public virtual ICollection<FlightCrew> FlightCrew { get; set; }
    }
}