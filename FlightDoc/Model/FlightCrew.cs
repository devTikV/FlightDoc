using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class FlightCrew
    {
        [Key]
        public int FlightCrewId { get; set; }

        [ForeignKey("Flight")]
        public int FlightId { get; set; }
        public virtual Flight Flight { get; set; }

        [ForeignKey("Pilot")]
        public int PilotId { get; set; }
        public Pilot Pilot { get; set; }

        [ForeignKey("FlightAttendant")]
        public int FlightAttendantId { get; set; }
        public FlightAttendant FlightAttendant { get; set; }

        // Thêm các trường thông tin khác liên quan đến phi công và tiếp viên chuyến bay

    }
}