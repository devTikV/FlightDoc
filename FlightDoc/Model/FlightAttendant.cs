using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDoc.Model
{
    public class FlightAttendant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightAttendantId { get; set; }

       
        [MaxLength(50)]
        public string Name { get; set; }


        // Thêm các thuộc tính thông tin khác về tiếp viên chuyến bay

        public virtual ICollection<FlightCrew> FlightCrew { get; set; }
    }
}