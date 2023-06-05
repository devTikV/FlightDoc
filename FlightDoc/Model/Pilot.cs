using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class Pilot
    {
        [Key]
        public int PilotId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string LicenseNumber { get; set; }
        // Thêm các thuộc tính thông tin khác về phi công

        public virtual ICollection<FlightCrew> FlightCrew { get; set; }
    }
}