using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDoc.Model
{
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightId { get; set; }

        
        [MaxLength(10)]
        public string FlightNumber { get; set; } 

       
        public DateTime DepartureTime { get; set; }

       
        public DateTime ArrivalTime { get; set; }

        [MaxLength(50)]
        public string DepartureAirport { get; set; } = "sân bay hcm";

        [MaxLength(50)]
        public string ArrivalAirport { get; set; } = "sân bay hn";

        // Thêm các trường thông tin khác về chuyến bay

        public ICollection<FlightPassenger> FlightPassengers { get; set; }
        public virtual ICollection<FlightCrew> FlightCrew { get; set; }
    }
}