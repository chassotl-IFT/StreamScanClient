using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StreamScan.Constants.Sql
{
    public class CFacilities
    {
        public const string GET_FACILITIES = "SELECT PK_Facility, name, address, npa, city, FK_Enterprise FROM T_Facility";

        public const string GET_FACILITY = "SELECT PK_Facility, name, address, npa, city, version, FK_Enterprise FROM T_Facility WHERE PK_Facility = @facility";

        public const string GET_ENTERPRISE_FACILITIES = "SELECT PK_Facility, name, address, npa, city FROM T_Facility WHERE FK_Enterprise = @enterprise";

        public const string INSERT_FACILITY = "INSERT INTO T_Facility (name, address, npa, city, version, FK_Enterprise) " +
                                              "VALUES (@name, @address, @npa, @city, 1, @enterpriseId)";

        public const string UPDATE_FACILITY = "UPDATE T_Facility SET name=@name, address=@address, npa=@npa, city=@city, version=version+1 " +
                                              "WHERE PK_Facility=@facilityId AND version=@version";

        public const string DELETE_FACILITY = "DELETE FROM T_Facility WHERE PK_Facility = @facility";
    }
}