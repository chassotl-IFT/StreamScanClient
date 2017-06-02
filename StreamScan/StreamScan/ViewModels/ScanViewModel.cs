using StreamScanConnect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StreamScan.ViewModels
{
    public class ScanViewModel
    {
        [Required]
        public Dictionary<string, string> Facilities { set; get; }

        [DisplayName("IP Address/Hostname")]
        [Required]
        public string MachineAddress { set; get; }
    }
}