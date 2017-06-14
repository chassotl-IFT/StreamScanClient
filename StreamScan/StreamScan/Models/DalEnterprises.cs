using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QueriesManager;
using QueriesManager.Bean;
using StreamScan.Constants.Sql;

namespace StreamScan.Models
{
    public class DalEnterprises
    {

        private SqlManager db;

        public DalEnterprises(SqlManager db)
        {
            this.db = db;
        }

        /// <summary>
        /// Récupère la liste des entreprises
        /// </summary>
        /// <returns>La liste des entreprises</returns>
        public List<Enterprise> GetEnterprises()
        {
            MySqlReturn sqlR = db.ExecuteQuery(CEnterprises.GET_ENTERPRISES);
            if (sqlR.ErrorMessage != "")
                throw new Exception(sqlR.ErrorMessage);

            List<Enterprise> enterprises = new List<Enterprise>();
            //On parcours les lignes retournées et on construit une liste de Enterprise
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
                enterprises.Add(new Enterprise
                {
                    Id = id,
                    Name = line[1],
                    Address = line[2],
                    Npa = npa,
                    City = line[4]
                });
            }
            return enterprises;
        }

        /// <summary>
        /// Récupère l'entreprise à l'aide de l'ID spécifié
        /// </summary>
        /// <returns>La liste des entreprises</returns>
        public Enterprise GetEnterprise(int enterprise)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@enterprise", enterprise);
            MySqlReturn sqlR = db.ExecuteQuery(CEnterprises.GET_ENTERPRISE, parameters);
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
            Enterprise enterpriseObj = new Enterprise
            {
                Id = id,
                Name = sqlR.Data[0][1],
                Address = sqlR.Data[0][2],
                Npa = npa,
                City = sqlR.Data[0][4],
                Version = version
            };
            return enterpriseObj;
        }

        /// <summary>
        /// Insert l'entreptise dans la base de données
        /// </summary>
        /// <param name="enterprise">L'entreprise à insérer</param>
        /// <returns>Le retour SQL (booléen d'état + [si erreur]message d'erreur)</returns>
        public MySqlReturn InsertEnterprise(Enterprise enterprise)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@name", enterprise.Name);
            parameters.Add("@address", enterprise.Address);
            parameters.Add("@npa", enterprise.Npa);
            parameters.Add("@city", enterprise.City);
            MySqlReturn sqlR = db.ExecuteQuery(CEnterprises.INSERT_ENTERPRISE, parameters);
            return sqlR;
        }

        /// <summary>
        /// Met à jour l'entreprise dans la base de données
        /// </summary>
        /// <param name="enterprise">L'entreprise à mettre à jour</param>
        /// <returns>Le retour SQL (booléen d'état + [si erreur]message d'erreur)</returns>
        public MySqlReturn UpdateEnterprise(Enterprise enterprise)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@enterpriseId", enterprise.Id);
            parameters.Add("@name", enterprise.Name);
            parameters.Add("@address", enterprise.Address);
            parameters.Add("@npa", enterprise.Npa);
            parameters.Add("@city", enterprise.City);
            parameters.Add("@version", enterprise.Version);
            MySqlReturn sqlR = db.ExecuteQuery(CEnterprises.UPDATE_ENTERPRISE, parameters);
            return sqlR;
        }

        /// <summary>
        /// Supprime l'entreprise dont l'ID est spécifié
        /// </summary>
        /// <param name="id">L'ID de l'entreprise</param>
        /// <returns>Le retour SQL (booléen d'état + [si erreur]message d'erreur)</returns>
        public MySqlReturn DeleteEnterprise(int enterprise)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@enterprise", enterprise);
            MySqlReturn sqlR = db.ExecuteQuery(CEnterprises.DELETE_ENTERPRISE, parameters);
            return sqlR;
        }
    }
}