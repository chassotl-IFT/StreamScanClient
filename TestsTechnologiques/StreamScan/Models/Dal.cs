using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QueriesManager;

namespace StreamScan.Models
{
    public class Dal : IDal
    {

        private DalEnterprises dalEnterprises;
        private DalFacilities dalFacilities;
        private SqlManager db;

        public Dal()
        {
            db = new SqlManager("localhost", "streamscan", "root", "");
            dalEnterprises = new DalEnterprises(db);
            dalFacilities = new DalFacilities(db);
        }

        public List<Enterprise> GetEnterprises()
        {
            return dalEnterprises.GetEnterprises();
        }

        public List<Facility> GetFacilities(int enterprise)
        {
            return dalFacilities.GetFacilities(enterprise);
        }
    }
}