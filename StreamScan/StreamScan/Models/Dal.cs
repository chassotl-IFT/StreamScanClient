using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QueriesManager;
using QueriesManager.Bean;
using StreamScanCommon.Infos;
using StreamScan.Constants.Sql;

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
        
        /// <summary>
        /// R�cup�re le nom d'utilisateur � l'aide des identifiants donn�s
        /// </summary>
        /// <param name="login">Les identifiants</param>
        /// <returns>Le nom d'utilisateur si OK sinon NULL</returns>
        public string Login(Login login)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, object>();
            parameters.Add("@username", login.Username);
            parameters.Add("@password", login.Password);
            MySqlReturn sqlR = db.ExecuteQuery(CUser.LOGIN, parameters);
            if (sqlR.ErrorMessage != "")
                throw new Exception(sqlR.ErrorMessage);
            return sqlR.IsOk ? sqlR.Data[0][0] : null;
        }

        //----------------------------------- ENTERPRISES -----------------------------------

        public List<Enterprise> GetEnterprises()
        {
            return dalEnterprises.GetEnterprises();
        }

        public Enterprise GetEnterprise(int enterprise)
        {
            return dalEnterprises.GetEnterprise(enterprise);
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

        public Facility GetFacility(int facility)
        {
            return dalFacilities.GetFacility(facility);
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

        public Dictionary<int, List<Machine>> GetEnterpriseMachines(int enterprise)
        {
            return dalMachines.GetEnterpriseMachines(enterprise);
        }

        public List<Machine> GetFacilityMachines(int facility)
        {
            return dalMachines.GetFacilityMachines(facility);
        }

        public MySqlReturn InsertMachine(int facility, Info machine)
        {
            return dalMachines.InsertMachine(facility, machine);
        }

        public MySqlReturn UpdateMachine(int version, int systemId, Info machine)
        {
            return dalMachines.UpdateMachine(version, systemId, machine);
        }
    }

}