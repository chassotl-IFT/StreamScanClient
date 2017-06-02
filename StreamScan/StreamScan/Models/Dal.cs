using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QueriesManager;
using QueriesManager.Bean;
using StreamScanCommon.Infos;

namespace StreamScan.Models
{
    public class Dal : IDal
    {

        private DalEnterprises dalEnterprises;
        private DalFacilities dalFacilities;
        private DalMachines dalMachines;
        private SqlManager db;

        public Dal()
        {
            db = new SqlManager("localhost", "streamscan", "root", "");
            dalEnterprises = new DalEnterprises(db);
            dalFacilities = new DalFacilities(db);
            dalMachines = new DalMachines(db);
        }

        //------------------------------------- ACCOUNT -------------------------------------
        
        public string Login(Login login)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, object>();
            parameters.Add("@username", login.Username);
            parameters.Add("@password", login.Password);
            string sql = "SELECT username FROM T_User WHERE username = @username AND password = @password";
            MySqlReturn sqlR = db.ExecuteQuery(sql, parameters);
            if (sqlR.ErrorMessage != "")
                throw new Exception(sqlR.ErrorMessage);
            return sqlR.IsOk ? sqlR.Data[0][0] : null;
        }

        //----------------------------------- ENTERPRISES -----------------------------------

        public List<Enterprise> GetEnterprises()
        {
            return dalEnterprises.GetEnterprises();
        }

        public MySqlReturn InsertEnterprise(Enterprise enterprise)
        {
            return dalEnterprises.InsertEnterprise(enterprise);
        }

        public MySqlReturn UpdateEnterprise(Enterprise enterprise)
        {
            return dalEnterprises.UpdateEnterprise(enterprise);
        }

        public MySqlReturn DeleteEnterprise(int id)
        {
            return dalEnterprises.DeleteEnterprise(id);
        }

        //----------------------------------- FACILITIES ------------------------------------

        public List<Facility> GetFacilities(int enterprise)
        {
            return dalFacilities.GetFacilities(enterprise);
        }

        public MySqlReturn InsertFacility(Facility facility)
        {
            return dalFacilities.InsertFacility(facility);
        }

        public MySqlReturn UpdateFacility(Facility facility)
        {
            return dalFacilities.UpdateFacility(facility);
        }

        public MySqlReturn DeleteFacility(int id)
        {
            return dalFacilities.DeleteFacility(id);
        }

        //------------------------------------ MACHINES -------------------------------------

        public List<Info> GetEnterpriseMachines(int enterprise)
        {
            return dalMachines.GetEnterpriseMachines(enterprise);
        }

        public List<Info> GetFacilityMachines(int facility)
        {
            return dalMachines.GetFacilityMachines(facility);
        }

        public MySqlReturn InsertMachine(int facility, Info machine)
        {
            return dalMachines.InsertMachine(facility, machine);
        }

        public MySqlReturn UpdateMachine(int id, Info machine)
        {
            return dalMachines.UpdateMachine(id, machine);
        }

    }

}