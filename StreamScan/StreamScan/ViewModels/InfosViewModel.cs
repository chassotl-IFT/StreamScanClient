using StreamScan.Models;
using StreamScanCommon.Infos;
using System.Collections.Generic;

namespace StreamScan.ViewModels
{
    public class InfosViewModel
    {
        public Info Infos { get; set; }
        public List<Machine> Machines { get; set; }
        public string Error { get; set; }
    }
}