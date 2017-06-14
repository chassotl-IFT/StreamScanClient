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
        /// Récupère toutes les machines d'une entreprise
        /// </summary>
        /// <param name="enterprise">L'ID de l'entreprise</param>
        /// <returns>Un Dictionary ayant comme clé l'ID de l'ouvrage et comme valeur la liste des machines</returns>
        public Dictionary<int, List<Machine>> GetEnterpriseMachines(int enterprise)
        {
            Dictionary<int, List<Machine>> machines = new Dictionary<int, List<Machine>>();
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@enterprise", enterprise);
            MySqlReturn sqlR = db.ExecuteQuery(CFacilities.GET_ENTERPRISE_FACILITIES, parameters);
            if (sqlR.ErrorMessage != "")
                throw new Exception(sqlR.ErrorMessage);
            if (!sqlR.IsOk)
                return new Dictionary<int, List<Machine>>();
            foreach (List<string> line in sqlR.Data)
            {
                int pk_facility;
                if (!Int32.TryParse(line[0], out pk_facility))
                    throw new Exception($"A database property type is not correct. " +
                        $"Attempted type : INT32 but got {line[0].GetType()}(Value:{line[0]}). " +
                        $"Please contact the administrator.");
                machines[pk_facility] = GetFacilityMachines(pk_facility);
            }
            return machines;
        }

        /// <summary>
        /// Récupère toutes les machines d'un ouvrage
        /// </summary>
        /// <param name="facility">L'ID de l'ouvrage</param>
        /// <returns>La liste des machines de l'ouvrage</returns>
        public List<Machine> GetFacilityMachines(int facility)
        {
            //On récupère les propriétés de la machine
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@facility", facility);
            MySqlReturn sqlR = db.ExecuteQuery(CMachines.GET_FACILITY_MACHINES, parameters);
            if (sqlR.ErrorMessage != "")
                throw new Exception(sqlR.ErrorMessage);
            if (!sqlR.IsOk)
                return new List<Machine>();

            //Dictionary ayant pour clé la PK_System et comme valeur un objet Info 
            //représentant les propriétés de la machine
            Dictionary<int, Info> machines = new Dictionary<int, Info>();
            //On parcours les infos retournées afin de construire une liste d'Info
            foreach (List<string> line in sqlR.Data)
            {
                Info machine = new Info();
                machine.InfosMachine = new InfosMachine();
                machine.InfosReseau = new InfosReseau();
                machine.InfosStreamX = new InfosStx();
                int pk_system;
                int sys_property;
                //On véfifie que les PK soient des INT
                if (!Int32.TryParse(line[0], out pk_system))
                    throw new Exception($"A database property type is not correct. " +
                        $"Attempted type : INT32 but got {line[0].GetType()}(Value:{line[0]}). " +
                        $"Please contact the administrator.");
                if (!Int32.TryParse(line[1], out sys_property))
                    throw new Exception($"A database property type is not correct. " +
                        $"Attempted type : INT32 but got {line[1].GetType()}(Value:{line[1]}). " +
                        $"Please contact the administrator.");

                //Si le dictionary contient déjà la machine, alors on la récupère
                if (machines.ContainsKey(pk_system))
                {
                    machine = machines[pk_system];
                }
                //On applique la propriété à l'objet Info
                machine = HMachines.SetMachineProperty(machine, sys_property, line[2]);
                machines[pk_system] = machine;
            }
            Dictionary<int, int> versions = new Dictionary<int, int>();
            foreach (int key in machines.Keys)
            {
                //On récupère la version de la machine
                parameters = new Dictionary<string, Object>();
                parameters.Add("@systemId", key);
                sqlR = db.ExecuteQuery(CMachines.GET_MACHINE_VERSION, parameters);

                int version;
                if (!Int32.TryParse(sqlR.Data[0][0], out version))
                    throw new Exception($"A database property type is not correct. " +
                        $"Attempted type : INT32 but got {sqlR.Data[0][0].GetType()}(Value:{sqlR.Data[0][0]}). " +
                        $"Please contact the administrator.");
                versions[key] = version;

                //On récupère les composants de la machine
                parameters = new Dictionary<string, Object>();
                parameters.Add("@systemId", key);
                sqlR = db.ExecuteQuery(CMachines.GET_COMPONENTS, parameters);
                if (sqlR.ErrorMessage != "")
                    throw new Exception(sqlR.ErrorMessage);
                List<StreamXComponent> components = new List<StreamXComponent>();
                foreach (List<string> line in sqlR.Data)
                {
                    StreamXComponent component = new StreamXComponent();
                    component.Version = line[0];
                    component.Name = line[1];
                    component.Log = (line[2] == "False") ? false : true;
                    components.Add(component);
                }
                machines[key].InfosStreamX.StreamXComponents = components;

                //On récupère les disques durs de la machine
                parameters = new Dictionary<string, Object>();
                parameters.Add("@systemId", key);
                sqlR = db.ExecuteQuery(CMachines.GET_HARDDRIVES, parameters);
                if (sqlR.ErrorMessage != "")
                    throw new Exception(sqlR.ErrorMessage);
                List<CustomDrive> drives = new List<CustomDrive>();
                foreach (List<string> line in sqlR.Data)
                {
                    CustomDrive drive = new CustomDrive();
                    drive.Name = line[0];
                    drive.VolumeLabel = line[1];
                    double totalSize;
                    if (!Double.TryParse(line[2], out totalSize))
                        throw new Exception($"A database property type is not correct. " +
                            $"Attempted type : Double but got {line[2].GetType()}(Value:{line[2]}). " +
                            $"Please contact the administrator.");
                    drive.TotalSize = totalSize;
                    double usedSpace;
                    if (!Double.TryParse(line[2], out usedSpace))
                        throw new Exception($"A database property type is not correct. " +
                            $"Attempted type : Double but got {line[3].GetType()}(Value:{line[3]}). " +
                            $"Please contact the administrator.");
                    drive.UsedSpace = usedSpace;
                    drives.Add(drive);
                }
                machines[key].InfosStreamX.StreamXComponents = components;
                machines[key].InfosMachine.CustomDrives = drives;
            }

            //On construit une liste de Machine à partir du Dictionary
            List<Machine> machinesList = new List<Machine>();
            foreach (int key in machines.Keys)
            {
                machinesList.Add(new Machine { Id = key, Properties = machines[key], Version = versions[key] });
            }
            return machinesList;
        }

        /// <summary>
        /// Insert la machine donnée dans la base de données
        /// </summary>
        /// <param name="facility">L'ID de l'ouvrage dans lequel on insert la machine</param>
        /// <param name="machine">la machine</param>
        /// <returns>Le retour SQL (booléen d'état + [si erreur]message d'erreur)</returns>
        public MySqlReturn InsertMachine(int facility, Info machine)
        {
            //On démarre la transaction
            Transaction transaction = db.BeginTransaction();

            #region  [obsolete] Table lock of "T_System"
            //On verrouille en écriture la table "T_System" afin de pouvoir 
            //récupérer plus tard la PK de l'entrée insérée
            //db.ExecuteQuery(CMachines.LOCK_SYSTEM);
            #endregion

            //On insert la machine dans la table "T_System"
            Dictionary<string, Object> parameters = new Dictionary<string, Object>();
            parameters.Add("@facility", facility);
            MySqlReturn sqlR = transaction.ExecuteQuery(CMachines.INSERT_MACHINE, parameters);
            if (!sqlR.IsOk)
            {
                //On annule la transaction
                transaction.Rollback();
                if (sqlR.ErrorMessage == "")
                    sqlR.ErrorMessage = "Can't insert the machine into the table \"T_System\"";
                return sqlR;
            }

            //On récupère la dernière PK insérée
            #region [obsolete] Get last inserted ID by SQL query
            /*sqlR = db.ExecuteQuery(CMachines.GET_LAST_INSERT_ID);
            if (!sqlR.IsOk)
            {
                //On annule la transaction
                db.ExecuteQuery("ROLLBACK");
                if (sqlR.ErrorMessage == "")
                    sqlR.ErrorMessage = "Can't get the last inserted ID";
                return sqlR;
            }
            */
            #endregion
            int systemId = sqlR.LastInsertedId;

            //On insert chaque propriété de la machine dans la table "T_System_Property"
            Dictionary<int, Object> properties = HMachines.GetMachineProperties(machine);
            foreach (int key in properties.Keys)
            {
                string property = "" + properties[key];
                parameters = new Dictionary<string, object>();
                parameters.Add("@systemId", systemId);
                parameters.Add("@propertyId", key);
                parameters.Add("@value", property);
                sqlR = transaction.ExecuteQuery(CMachines.INSERT_MACHINE_PROPERTIES, parameters);
                if (!sqlR.IsOk)
                {
                    transaction.Rollback();
                    if (sqlR.ErrorMessage == "")
                        sqlR.ErrorMessage = "An error occured during adding the properties";
                    return sqlR;
                }
            }

            //On insert les composents StreamX
            foreach (StreamXComponent component in machine.InfosStreamX.StreamXComponents)
            {
                parameters = new Dictionary<string, Object>();
                parameters.Add("@version", component.Version);
                parameters.Add("@name", component.Name);
                parameters.Add("@log", component.Log);
                parameters.Add("@systemId", systemId);
                sqlR = transaction.ExecuteQuery(CMachines.INSERT_COMPONENT, parameters);
                if (!sqlR.IsOk)
                {
                    transaction.Rollback();
                    if (sqlR.ErrorMessage == "")
                        sqlR.ErrorMessage = "An error occured during adding the components";
                    return sqlR;
                }
            }

            //On insert les disques durs
            foreach (CustomDrive drive in machine.InfosMachine.CustomDrives)
            {
                parameters = new Dictionary<string, Object>();
                parameters.Add("@name", drive.Name);
                parameters.Add("@label", drive.VolumeLabel);
                parameters.Add("@capacity", drive.TotalSize);
                parameters.Add("@usedSpace", drive.TotalSize - drive.TotalFreeSpace);
                parameters.Add("@systemId", systemId);
                sqlR = transaction.ExecuteQuery(CMachines.INSERT_HARDDRIVE, parameters);
                if (!sqlR.IsOk)
                {
                    transaction.Rollback();
                    if (sqlR.ErrorMessage == "")
                        sqlR.ErrorMessage = "An error occured during adding the hard drives";
                    return sqlR;
                }
            }

            transaction.Commit();
            transaction.EndTransaction();
            return new MySqlReturn { IsOk = true };
        }

        /// <summary>
        /// Met à jour la machine dans la base de données
        /// </summary>
        /// <param name="systemId">l'ID de la machine à mettre à jour</param>
        /// <param name="machine">Les infos de la machine</param>
        /// <returns>Le retour SQL (booléen d'état + [si erreur]message d'erreur)</returns>
        public MySqlReturn UpdateMachine(int version, int systemId, Info machine)
        {

            //On démarre la transaction
            Transaction transaction = db.BeginTransaction();

            Dictionary<string, Object> parameters = new Dictionary<string, object>();
            MySqlReturn sqlR = null;
            Dictionary<int, Object> properties = HMachines.GetMachineProperties(machine);
            foreach (int key in properties.Keys)
            {
                string property = "" + properties[key];
                parameters = new Dictionary<string, object>();
                parameters.Add("@systemId", systemId);
                parameters.Add("@propertyId", key);
                parameters.Add("@value", property);
                sqlR = transaction.ExecuteQuery(CMachines.UPDATE_MACHINE, parameters);
                if (!sqlR.IsOk)
                {
                    transaction.Rollback();
                    if (sqlR.ErrorMessage == "")
                        sqlR.ErrorMessage = "An error occured during updating the properties";
                    return sqlR;
                }
            }

            //On supprime les composants StreamX
            parameters = new Dictionary<string, Object>();
            parameters.Add("@systemId", systemId);
            sqlR = transaction.ExecuteQuery(CMachines.DELETE_COMPONENTS, parameters);
            if (sqlR.ErrorMessage != "")
                return sqlR;

            //On insert les composents StreamX
            foreach (StreamXComponent component in machine.InfosStreamX.StreamXComponents)
            {
                parameters = new Dictionary<string, Object>();
                parameters.Add("@version", component.Version);
                parameters.Add("@name", component.Name);
                parameters.Add("@log", component.Log);
                parameters.Add("@systemId", systemId);
                sqlR = transaction.ExecuteQuery(CMachines.INSERT_COMPONENT, parameters);
                if (!sqlR.IsOk)
                {
                    transaction.Rollback();
                    if (sqlR.ErrorMessage == "")
                        sqlR.ErrorMessage = "An error occured during adding the components";
                    return sqlR;
                }
            }

            //On supprime les disques durs
            parameters = new Dictionary<string, Object>();
            parameters.Add("@systemId", systemId);
            sqlR = transaction.ExecuteQuery(CMachines.DELETE_HARDDRIVES, parameters);
            if (sqlR.ErrorMessage != "")
                return sqlR;

            //On insert les disques durs
            foreach (CustomDrive drive in machine.InfosMachine.CustomDrives)
            {
                parameters = new Dictionary<string, Object>();
                parameters.Add("@name", drive.Name);
                parameters.Add("@label", drive.VolumeLabel);
                parameters.Add("@capacity", drive.TotalSize);
                parameters.Add("@usedSpace", drive.UsedSpace);
                parameters.Add("@systemId", systemId);
                sqlR = transaction.ExecuteQuery(CMachines.INSERT_HARDDRIVE, parameters);
                if (!sqlR.IsOk)
                {
                    transaction.Rollback();
                    if (sqlR.ErrorMessage == "")
                        sqlR.ErrorMessage = "An error occured during adding the hard drives";
                    return sqlR;
                }
            }

            //Mise à jour de la version de la machine
            parameters = new Dictionary<string, Object>();
            parameters.Add("@systemId", systemId);
            parameters.Add("@version", version);
            sqlR = transaction.ExecuteQuery(CMachines.UPDATE_MACHINE_VERSION, parameters);
            if (!sqlR.IsOk)
            {
                transaction.Rollback();
                if (sqlR.ErrorMessage == "")
                    sqlR.ErrorMessage = "The machine you tried to update has been updated by another user since you loaded the page. Please repeat the operation.";
                return sqlR;
            }

            transaction.Commit();
            transaction.EndTransaction();
            return new MySqlReturn { IsOk = true };
        }
    }
}