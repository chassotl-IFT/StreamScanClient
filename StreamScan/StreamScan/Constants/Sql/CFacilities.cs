using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StreamScan.Constants.Sql
{
    public class CFacilities
    {
        public const string GET_FACILITIES = "SELECT PK_Facility, name, address, npa, city, FK_Enterprise FROM T_Facility";

        public const string GET_ENTERPRISE_FACILITIES = "SELECT PK_Facility, name, address, npa, city FROM T_Facility WHERE FK_Enterprise = @enterprise";

        public const string INSERT_FACILITY = "INSERT INTO T_Facility (name, address, npa, city, FK_Enterprise) " +
                                              "VALUES (@name, @address, @npa, @city, @enterpriseId)";

        public const string UPDATE_FACILITY = "UPDATE T_Enterprise SET name=@name, address=@address, npa=@npa, city=@city, FK_Enterprise=@enterpriseId " +
                                              "WHERE PK_Facility=@facilityId";

        public const string DELETE_FACILITY = "DELETE FROM T_Facility WHERE PK_Facility = @facility";
    }
}