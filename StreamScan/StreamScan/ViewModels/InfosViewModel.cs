﻿using StreamScanCommon.Infos;
using System.Collections.Generic;

namespace StreamScan.ViewModels
{
    public class InfosViewModel
    {
        public Info Infos { get; set; }
        public List<Info> Machines { get; set; }
        public string Error { get; set; }
    }
}