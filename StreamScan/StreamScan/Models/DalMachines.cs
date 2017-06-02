using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QueriesManager;
using StreamScanCommon.Infos;
using QueriesManager.Bean;
using StreamScan.Constants;
using StreamScan.Constants.Sql;
using StreamScan.Helpers;

namespace StreamScan.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DalMachines
    {

        private SqlManager db;

        public DalMachines(SqlManager db)
        {
            this.db = db;
        }
        /// <summary>
        /// R�cup�re toutes les machines d'une entreprise
        /// </summary>
        /// <param name="enterprise">L'ID de l'entreprise</param>
        /// <returns></returns>
        public List<Info> GetEnterpriseMachines(int enterprise)
        {

            return null;
        }

        /// <summary>
        /// R�cup�re toutes les machines d'un ouvrage
        /// </summary>
        /// <param name="facility">L'ID de l'ouvrage</param>
        /// <returns>La liste des machines de l'ouvrage</returns>
        public List<Info> GetFacilityMachines(int facility)
        {
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@facility", facility);
            MySqlReturn sqlR = db.ExecuteQuery(CMachines.GET_FACILITY_MACHINES, parameters);
            if (sqlR.ErrorMessage != "")
                throw new Exception(sqlR.ErrorMessage);
            if (!sqlR.IsOk)
                return new List<Info>();

            //Dictionary ayant pour cl� la PK_System et comme valeur un objet Info 
            //repr�sentant les propri�t�s de la machine
            Dictionary<int, Info> machines = new Dictionary<int, Info>();
            //On parcours les infos retourn�es afin de construire une liste d'Info
            foreach (List<string> line in sqlR.Data)
            {
                Info machine = new Info();
                machine.InfosMachine = new InfosMachine();
                machine.InfosReseau = new InfosReseau();
                machine.InfosStreamX = new InfosStx();
                int pk_system;
                int sys_property;
                //On v�fifie que les PK soient des INT
                if (!Int32.TryParse(line[0], out pk_system))
                    throw new Exception($"A database property type is not correct. " +
                        $"Attempted type : INT32 but got {line[0].GetType()}(Value:{line[0]}). " +
                        $"Please contact the administrator.");
                if (!Int32.TryParse(line[1], out sys_property))
                    throw new Exception($"A database property type is not correct. " +
                        $"Attempted type : INT32 but got {line[1].GetType()}(Value:{line[1]}). " +
                        $"Please contact the administrator.");

                //Si le dictionary contient d�j� la machine, alors on la r�cup�re
                if (machines.ContainsKey(pk_system))
                {
                    machine = machines[pk_system];
                }
                //On applique la propri�t� � l'objet Info
                machine = HMachines.SetMachineProperty(machine, sys_property, line[2]);
                machines[pk_system] = machine;
            }
            List<Info> r = new List<Info>(machines.Values);
            return r;
        }

        /// <summary>
        /// Insert la machine donn�e dans la base de donn�es
        /// </summary>
        /// <param name="facility">L'ID de l'ouvrage dans lequel on insert la machine</param>
        /// <param name="machine">la machine</param>
        /// <returns>Le retour SQL (bool�en d'�tat + [si erreur]message d'erreur)</returns>
        public MySqlReturn InsertMachine(int facility, Info machine)
        {
            //On d�marre la transaction
            db.ExecuteQuery("SET autocommit = 0");
            db.ExecuteQuery("START TRANSACTION");

            //On verrouille en �criture la table "T_System" afin de pouvoir 
            //r�cup�rer plus tard la PK de l'entr�e ins�r�e
            db.ExecuteQuery(CMachines.LOCK_SYSTEM);

            //On insert la machine dans la table "T_System"
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@facility", facility);
            MySqlReturn sqlR = db.ExecuteQuery(CMachines.INSERT_MACHINE, parameters);
            if (!sqlR.IsOk)
            {
                //On lib�re la table "T_System"
                db.ExecuteQuery("UNLOCK TABLES");
                db.ExecuteQuery("ROLLBACK");
                if (sqlR.ErrorMessage == "")
                    sqlR.ErrorMessage = "Can't insert the machine into the table \"T_System\"";
                return sqlR;
            }

            //On r�cup�re la derni�re PK ins�r�e
            sqlR = db.ExecuteQuery(CMachines.GET_LAST_INSERT_ID);
            if (!sqlR.IsOk)
            {
                //On lib�re la table "T_System"
                db.ExecuteQuery("UNLOCK TABLES");
                db.ExecuteQuery("ROLLBACK");
                if (sqlR.ErrorMessage == "")
                    sqlR.ErrorMessage = "Can't get the last inserted ID";
                return sqlR;
            }
            string systemId = sqlR.Data[0][0];

            //On lib�re la table "T_System"
            db.ExecuteQuery("UNLOCK TABLES");

            //On insert chaque propri�t� de la machine dans la table "T_System_Property"
            Dictionary<int, Object> properties = HMachines.GetMachineProperties(machine);
            foreach (int key in properties.Keys)
            {
                string property = ""+properties[key];
                parameters = new Dictionary<string, object>();
                parameters.Add("@systemId", systemId);
                parameters.Add("@propertyId", key);
                parameters.Add("@value", property);
                sqlR = db.ExecuteQuery(CMachines.INSERT_MACHINE_PROPERTIES, parameters);
                if (!sqlR.IsOk)
                {
                    db.ExecuteQuery("ROLLBACK");
                    if (sqlR.ErrorMessage == "")
                        sqlR.ErrorMessage = "An error occured during adding the properties";
                    return sqlR;
                }
            }
            sqlR = db.ExecuteQuery("COMMIT");
            return new MySqlReturn { IsOk = true};
        }

        /// <summary>
        /// Met � jour la machine dans la base de donn�es
        /// </summary>
        /// <param name="id">l'ID de la machine � mettre � jour</param>
        /// <param name="machine">Les infos de la machine</param>
        /// <returns>Le retour SQL (bool�en d'�tat + [si erreur]message d'erreur)</returns>
        public MySqlReturn UpdateMachine(int id, Info machine)
        {
            
            return null;
        }
    }
}