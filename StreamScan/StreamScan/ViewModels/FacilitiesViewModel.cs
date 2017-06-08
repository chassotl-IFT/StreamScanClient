using StreamScan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StreamScan.ViewModels
{
    public class FacilitiesViewModel
    {
        public Enterprise Enterprise { get; set; }

        public List<Facility> Facilities { get; set; }
    }
}