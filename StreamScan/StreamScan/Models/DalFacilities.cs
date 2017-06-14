using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QueriesManager;
using QueriesManager.Bean;
using StreamScan.Constants.Sql;

namespace StreamScan.Models
{
    public class DalFacilities
    {

        private SqlManager db;

        public DalFacilities(SqlManager db)
        {
            this.db = db;
        }

        /// <summary>
        /// Récupère les ouvrages d'une entreprise
        /// </summary>
        /// <param name="enterprise">L'ID de l'entreprise</param>
        /// <returns></returns>
        public List<Facility> GetFacilities(int enterprise)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@enterprise", enterprise);
            MySqlReturn sqlR = db.ExecuteQuery(CFacilities.GET_ENTERPRISE_FACILITIES, parameters);
            if (sqlR.ErrorMessage != "")
                throw new Exception(sqlR.ErrorMessage);

            List<Facility> facilities = new List<Facility>();
            //On parcours les lignes retournées et on construit une liste de Facility
            foreach (List<string> line in sqlR.Data)
            {
                int id;
                if (!Int32.TryParse(line[0], out id))
                    throw new Exception($"A database property type is not correct. " +
                        $"Attempted type : Int32 but got {line[0].GetType()}(Value:{line[0]}). " +
                        $"Please contact the administrator.");
                int npa;
                if (!Int32.TryParse(line[3], out npa))
                    throw new Exception($"A database property type is not correct. " +
                        $"Attempted type : Int32 but got {line[3].GetType()}(Value:{line[3]}). " +
                        $"Please contact the administrator.");
                facilities.Add(new Facility
                {
                    Id = id,
                    Name = line[1],
                    Address = line[2],
                    Npa = npa,
                    City = line[4]
                });
            }
            return facilities;
        }

        /// <summary>
        /// Récupère l'ouvrage à l'aide de l'ID spécifié
        /// </summary>
        /// <param name="enterprise">L'ID de l'ouvrage</param>
        /// <returns></returns>
        public Facility GetFacility(int facility)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@facility", facility);
            MySqlReturn sqlR = db.ExecuteQuery(CFacilities.GET_FACILITY, parameters);
            if (sqlR.ErrorMessage != "")
                throw new Exception(sqlR.ErrorMessage);

            int id;
            if (!Int32.TryParse(sqlR.Data[0][0], out id))
                throw new Exception($"A database property type is not correct. " +
                    $"Attempted type : Int32 but got {sqlR.Data[0][0].GetType()}(Value:{sqlR.Data[0][0]}). " +
                    $"Please contact the administrator.");
            int npa;
            if (!Int32.TryParse(sqlR.Data[0][3], out npa))
                throw new Exception($"A database property type is not correct. " +
                    $"Attempted type : Int32 but got {sqlR.Data[0][3].GetType()}(Value:{sqlR.Data[0][3]}). " +
                    $"Please contact the administrator.");
            int version;
            if (!Int32.TryParse(sqlR.Data[0][5], out version))
                throw new Exception($"A database property type is not correct. " +
                    $"Attempted type : Int32 but got {sqlR.Data[0][5].GetType()}(Value:{sqlR.Data[0][5]}). " +
                    $"Please contact the administrator.");
            int fk_enterprise;
            if (!Int32.TryParse(sqlR.Data[0][6], out fk_enterprise))
                throw new Exception($"A database property type is not correct. " +
                    $"Attempted type : Int32 but got {sqlR.Data[0][6].GetType()}(Value:{sqlR.Data[0][6]}). " +
                    $"Please contact the administrator.");
            Facility facilityObj = new Facility
            {
                Id = id,
                Name = sqlR.Data[0][1],
                Address = sqlR.Data[0][2],
                Npa = npa,
                City = sqlR.Data[0][4],
                Version = version,
                Fk_Enterprise = fk_enterprise
            };
            return facilityObj;
        }

        /// <summary>
        /// Insert l'ouvrage dans la base de données
        /// </summary>
        /// <param name="facility">L'ouvrage à insérer</param>
        /// <returns>Le retour SQL (booléen d'état + [si erreur]message d'erreur)</returns>
        public MySqlReturn InsertFacility(Facility facility)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@name", facility.Name);
            parameters.Add("@address", facility.Address);
            parameters.Add("@npa", facility.Npa);
            parameters.Add("@city", facility.City);
            parameters.Add("@enterpriseId", facility.Fk_Enterprise);
            MySqlReturn sqlR = db.ExecuteQuery(CFacilities.INSERT_FACILITY, parameters);
            return sqlR;
        }

        /// <summary>
        /// Met à jour l'ouvrage dans la base de données
        /// </summary>
        /// <param name="facility">L'ouvrage à mettre à jour</param>
        /// <returns>Le retour SQL (booléen d'état + [si erreur]message d'erreur)</returns>
        public MySqlReturn UpdateFacility(Facility facility)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@facilityId", facility.Id);
            parameters.Add("@name", facility.Name);
            parameters.Add("@address", facility.Address);
            parameters.Add("@npa", facility.Npa);
            parameters.Add("@city", facility.City);
            parameters.Add("@version", facility.Version);
            MySqlReturn sqlR = db.ExecuteQuery(CFacilities.UPDATE_FACILITY, parameters);
            return sqlR;
        }

        /// <summary>
        /// Supprime l'ouvrage de la base de données
        /// </summary>
        /// <param name="id">L'ID de l'ouvrage</param>
        /// <returns>Le retour SQL (booléen d'état + [si erreur]message d'erreur)</returns>
        public MySqlReturn DeleteFacility(int facility)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@facility", facility);
            MySqlReturn sqlR = db.ExecuteQuery(CFacilities.DELETE_FACILITY, parameters);
            return sqlR;
        }
    }
}