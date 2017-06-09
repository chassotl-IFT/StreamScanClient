using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StreamScan.Models
{
    public class Enterprise
    {
        public int Id { get; set; }

        public int Version { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [DisplayName("NPA")]
        [Required]
        [Range(1000, 9999, ErrorMessage = "The field NPA must have a length of 4.")]
        public int Npa { get; set; }

        [Required]
        public string City { get; set; }
    }
}