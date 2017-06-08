using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StreamScan.Models
{
    public class Facility
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address{ get; set; }

        [DisplayName("NPA")]
        [Required]
        [Range(1000, 9999, ErrorMessage = "The field NPA must have a length of 4.")]
        public int Npa { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int Fk_Enterprise { get; set; }
    }
}