using StreamScanConnect;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StreamScan.ViewModels
{
    public class ScanViewModel
    {
        [Required]
        public Dictionary<string, string> Enterprises { set; get; }
        [Required]
        public Dictionary<string, string> Facilities { set; get; }
        [Required]
        public string MachineAddress { set; get; }

        public ClientWCF Wcf { get; set; }
    }
}