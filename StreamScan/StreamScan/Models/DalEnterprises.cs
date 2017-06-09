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
        /// R�cup�re la liste des entreprises
        /// </summary>
        /// <returns>La liste des entreprises</returns>
        public List<Enterprise> GetEnterprises()
        {
            MySqlReturn sqlR = db.ExecuteQuery(CEnterprises.GET_ENTERPRISES);
            if (sqlR.ErrorMessage != "")
                throw new Exception(sqlR.ErrorMessage);

            List<Enterprise> enterprises = new List<Enterprise>();
            //On parcours les lignes retourn�es et on construit une liste de Enterprise
            foreach (List<string> line in sqlR.Data)
            {
                enterprises.Add(new Enterprise
                {
                    Id = Int32.Parse(line[0]),
                    Name = line[1],
                    Address = line[2],
                    Npa = Int32.Parse(line[3]),
                    City = line[4]
                });
            }
            return enterprises;
        }

        /// <summary>
        /// R�cup�re l'entreprise � l'aide de l'ID sp�cifi�
        /// </summary>
        /// <returns>La liste des entreprises</returns>
        public Enterprise GetEnterprise(int enterprise)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@enterprise", enterprise);
            MySqlReturn sqlR = db.ExecuteQuery(CEnterprises.GET_ENTERPRISE, parameters);
            if (sqlR.ErrorMessage != "")
                throw new Exception(sqlR.ErrorMessage);
            Enterprise enterpriseObj = new Enterprise
            {
                Id = Int32.Parse(sqlR.Data[0][0]),
                Name = sqlR.Data[0][1],
                Address = sqlR.Data[0][2],
                Npa = Int32.Parse(sqlR.Data[0][3]),
                City = sqlR.Data[0][4],
                Version = Int32.Parse(sqlR.Data[0][5])
            };
            return enterpriseObj;
        }

        /// <summary>
        /// Insert l'entreptise dans la base de donn�es
        /// </summary>
        /// <param name="enterprise">L'entreprise � ins�rer</param>
        /// <returns>Le retour SQL (bool�en d'�tat + [si erreur]message d'erreur)</returns>
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
        /// Met � jour l'entreprise dans la base de donn�es
        /// </summary>
        /// <param name="enterprise">L'entreprise � mettre � jour</param>
        /// <returns>Le retour SQL (bool�en d'�tat + [si erreur]message d'erreur)</returns>
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
        /// Supprime l'entreprise dont l'ID est sp�cifi�
        /// </summary>
        /// <param name="id">L'ID de l'entreprise</param>
        /// <returns>Le retour SQL (bool�en d'�tat + [si erreur]message d'erreur)</returns>
        public MySqlReturn DeleteEnterprise(int enterprise)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@enterprise", enterprise);
            MySqlReturn sqlR = db.ExecuteQuery(CEnterprises.DELETE_ENTERPRISE, parameters);
            return sqlR;
        }
    }
}