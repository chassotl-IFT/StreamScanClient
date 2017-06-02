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
            MySqlReturn sqlR = null;
            string sql = CEnterprises.GET_ENTERPRISES;
            sqlR = db.ExecuteQuery(sql);
            if (!sqlR.IsOk)
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
        /// Supprime l'entreprise dont l'ID est sp�cifi�
        /// </summary>
        /// <param name="id">L'ID de l'entreprise</param>
        /// <returns>Le retour SQL (bool�en d'�tat + [si erreur]message d'erreur)</returns>
        public MySqlReturn DeleteEnterprise(int id)
        {

            return null;
        }

        /// <summary>
        /// Insert l'entreptise dans la base de donn�es
        /// </summary>
        /// <param name="enterprise">L'entreprise � ins�rer</param>
        /// <returns>Le retour SQL (bool�en d'�tat + [si erreur]message d'erreur)</returns>
        public MySqlReturn InsertEnterprise(Enterprise enterprise)
        {

            return null;
        }

        /// <summary>
        /// Met � jour l'entreprise dans la base de donn�es
        /// </summary>
        /// <param name="enterprise">L'entreprise � mettre � jour</param>
        /// <returns>Le retour SQL (bool�en d'�tat + [si erreur]message d'erreur)</returns>
        public MySqlReturn UpdateEnterprise(Enterprise enterprise)
        {

            return null;
        }
    }
}