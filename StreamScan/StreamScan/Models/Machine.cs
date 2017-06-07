using StreamScanCommon.Infos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StreamScan.Models
{
    public class Machine
    {
        public int Id { get; set; }

        public Info Properties { get; set; }
    }
}