using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class FlightAttendant
    {
        [Key]
        public int FlightAttendantId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }


        // Thêm các thuộc tính thông tin khác về tiếp viên chuyến bay

        public virtual ICollection<FlightCrew> FlightCrew { get; set; }
    }
}