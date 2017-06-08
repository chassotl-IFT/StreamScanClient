using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StreamScan.Constants.Sql
{
    public class CMachines
    {
        public const string GET_FACILITY_MACHINES = "SELECT FK_System, FK_Property_Label, value FROM T_System_Property " +
                                                    "INNER JOIN T_System ON T_System.PK_system = FK_System " +
                                                    "WHERE T_System.Fk_Facility = @facility";

        public const string GET_ENTERPRISE_MACHINES = "SELECT T_Facility.name, FK_System, FK_Property_Label, value FROM T_System_Property " +
                                                      "INNER JOIN T_System ON T_System.PK_system = FK_System " +
                                                      "INNER JOIN T_Facility ON T_Facility.PK_Facility = T_System.FK_Facility " +
                                                      "WHERE T_Facility.FK_Enterprise = @enterprise";

        public const string GET_MACHINE_VERSION = "SELECT version FROM T_System WHERE PK_System = @systemId";

        public const string LOCK_SYSTEM = "LOCK TABLES T_System WRITE";

        public const string GET_LAST_INSERT_ID = "SELECT LAST_INSERT_ID() FROM T_System";

        public const string INSERT_MACHINE = "INSERT INTO T_System (FK_Facility, version) " +
                                             "VALUES (@facility, 1)";

        public const string INSERT_MACHINE_PROPERTIES = "INSERT INTO T_System_Property (FK_System, FK_Property_Label, value) " +
                                                        "VALUES (@systemId, @propertyId, @value)";

        public const string UPDATE_MACHINE = "UPDATE T_System_Property SET value=@value " +
                                             "WHERE FK_System=@systemId AND FK_Property_Label=@propertyId";

        public const string GET_COMPONENTS = "SELECT PK_Component FROM T_Component WHERE FK_System=@systemId";

        public const string DELETE_COMPONENTS = "DELETE FROM T_Component WHERE FK_System=@systemId";

        public const string INSERT_COMPONENTS = "INSERT INTO T_Component (version, name, log, FK_System) VALUES(@version, @name, @log, @systemId)";

        public const string UPDATE_MACHINE_VERSION = "UPDATE T_System SET version=version+1 WHERE PK_System=@systemId AND version=@version";
    }
}