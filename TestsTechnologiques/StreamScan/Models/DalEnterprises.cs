///////////////////////////////////////////////////////////
//  DalEnterprises.cs
//  Implementation of the Class DalEnterprises
//  Generated by Enterprise Architect
//  Created on:      29-mai-2017 15:58:21
//  Original author: Laurent
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using QueriesManager;
using QueriesManager.Bean;

namespace StreamScan.Models
{
    public class DalEnterprises
    {

        private SqlManager db;

        public DalEnterprises(SqlManager db)
        {
            this.db = db;
        }

        public List<Enterprise> GetEnterprises()
        {
            MySqlReturn sqlR = null;
            string sql = "SELECT PK_Enterprise, name, address, npa, city FROM T_Enterprise";
            sqlR = db.ExecuteQuery(sql);
            if (!sqlR.IsOk)
                throw new Exception(sqlR.ErrorMessage);

            List<Enterprise> enterprises = new List<Enterprise>();
            foreach (List<string> line in sqlR.Data)
            {
                enterprises.Add(new Enterprise
                {
                    id = Int32.Parse(line[0]),
                    name = line[1],
                    address = line[2],
                    npa = Int32.Parse(line[3]),
                    city = line[4]
                });
            }
            return enterprises;
        }
    }
}