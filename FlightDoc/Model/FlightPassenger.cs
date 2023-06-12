using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class FlightPassenger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightPassengerId { get; set; }

        [ForeignKey("Flight")]
        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        [ForeignKey("Passenger")]
        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; }

        // Thêm các trường thông tin khác liên quan đến hành khách trong chuyến bay
    }
}