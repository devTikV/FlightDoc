using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDoc.Model
{
    public class Pilot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PilotId { get; set; }

       
        [MaxLength(50)]
        public string Name { get; set; }

       
        [MaxLength(50)]
        public string LicenseNumber { get; set; }
        // Thêm các thuộc tính thông tin khác về phi công
        public bool active { get; set; } = false;

        public virtual ICollection<FlightCrew> FlightCrew { get; set; }
    }
}