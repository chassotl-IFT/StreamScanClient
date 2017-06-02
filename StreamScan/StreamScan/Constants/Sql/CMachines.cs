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

        public const string LOCK_SYSTEM = "LOCK TABLES T_System WRITE";

        public const string INSERT_MACHINE = "INSERT INTO T_System (FK_Facility) VALUES (@facility)";

        public const string GET_LAST_INSERT_ID = "SELECT LAST_INSERT_ID() FROM T_System";

        public const string INSERT_MACHINE_PROPERTIES = "INSERT INTO T_System_Property(FK_System, FK_Property_Label, value) " +
                                                        "VALUES (@systemId, @propertyId, @value)";

        public const string UPDATE_MACHINE = "UPDATE T_System_Property SET FK_Property_Label=@propertyId, value=@value WHERE FK_System=@systemId";
    }
}