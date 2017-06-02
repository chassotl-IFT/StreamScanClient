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
        /// R�cup�re les ouvrages d'une entreprise
        /// </summary>
        /// <param name="enterprise">L'ID de l'entreprise</param>
        /// <returns></returns>
        public List<Facility> GetFacilities(int enterprise)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@enterprise", enterprise);
            MySqlReturn sqlR = null;
            sqlR = db.ExecuteQuery(CFacilities.GET_ENTERPRISE_FACILITIES, parameters);
            if (!sqlR.IsOk && sqlR.ErrorMessage != "")
                throw new Exception(sqlR.ErrorMessage);

            List<Facility> facilities = new List<Facility>();
            //On parcours les lignes retourn�es et on construit une liste de Facility
            foreach (List<string> line in sqlR.Data)
            {
                facilities.Add(new Facility {
                    Id = Int32.Parse(line[0]),
                    Name = line[1],
                    Address = line[2],
                    Npa = Int32.Parse(line[3]),
                    City = line[4]
                });
            }
            return facilities;
        }

        /// <summary>
        /// Insert l'ouvrage dans la base de donn�es
        /// </summary>
        /// <param name="facility">L'ouvrage � ins�rer</param>
        /// <returns>Le retour SQL (bool�en d'�tat + [si erreur]message d'erreur)</returns>
        public MySqlReturn InsertFacility(Facility facility)
        {

            return null;
        }

        /// <summary>
        /// Met � jour l'ouvrage dans la base de donn�es
        /// </summary>
        /// <param name="facility">L'ouvrage � mettre � jour</param>
        /// <returns>Le retour SQL (bool�en d'�tat + [si erreur]message d'erreur)</returns>
        public MySqlReturn UpdateFacility(Facility facility)
        {

            return null;
        }

        /// <summary>
        /// Supprime l'ouvrage de la base de donn�es
        /// </summary>
        /// <param name="id">L'ID de l'ouvrage</param>
        /// <returns>Le retour SQL (bool�en d'�tat + [si erreur]message d'erreur)</returns>
        public MySqlReturn DeleteFacility(int id)
        {

            return null;
        }
    }
}