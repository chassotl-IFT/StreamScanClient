using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QueriesManager;
using QueriesManager.Bean;

namespace StreamScan.Models
{
    public class DalFacilities
    {

        private SqlManager db;

        public DalFacilities(SqlManager db)
        {
            this.db = db;
        }

        public List<Facility> GetFacilities(int enterprise)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@enterprise", enterprise);
            MySqlReturn sqlR = null;
            sqlR = db.ExecuteQuery("SELECT PK_Facility, name, address, npa, city FROM T_Facility WHERE FK_Enterprise = @enterprise", parameters);
            if (!sqlR.IsOk && sqlR.ErrorMessage != "")
                throw new Exception(sqlR.ErrorMessage);

            List<Facility> facilities = new List<Facility>();
            foreach (List<string> line in sqlR.Data)
            {
                facilities.Add(new Facility { id = Int32.Parse(line[0]), name = line[1], address = line[2], npa = Int32.Parse(line[3]), city = line[4] });
            }
            return facilities;
        }
    }
}