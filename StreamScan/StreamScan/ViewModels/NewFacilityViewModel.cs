using StreamScan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StreamScan.ViewModels
{
    public class NewFacilityViewModel
    {

        public int Enterprise { get; set; }

        public Facility Facility { get; set; }
    }
}