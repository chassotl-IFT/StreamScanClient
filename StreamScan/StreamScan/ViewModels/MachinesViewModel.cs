using StreamScan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StreamScan.ViewModels
{
    public class MachinesViewModel
    {
        [DisplayName("Enterprise")]
        public int enterprise { get; set; }
        public List<Enterprise> Enterprises { get; set; }
        public IEnumerable<SelectListItem> CmxEnterprises { get { return new SelectList(Enterprises, "Id", "Name",this.enterprise); } }

        [DisplayName("Facility")]
        public int facility { get; set; }
        public List<Facility> Facilities { get; set; }
        public IEnumerable<SelectListItem> CmxFacilities { get { return new SelectList((Facilities!=null)?Facilities:new List<Facility>(), "Id", "Name", this.facility); } }

        public Dictionary<int, List<Machine>> Machines { get; set; }
    }
}