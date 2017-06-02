using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StreamScan.Constants.Sql
{
    public class CEnterprises
    {
        public const string GET_ENTERPRISES = "SELECT PK_Enterprise, name, address, npa, city FROM T_Enterprise";

        public const string INSERT_ENTERPRISE = "INSERT INTO T_Enterprise (name, address, npa, city) VALUES (@name, @address, @npa, @city)";

        public const string UPDATE_ENTERPRISE = "UPDATE T_Enterprise SET name=@name, address=@address, npa=@npa, city=@city WHERE PK_Enterprise=@enterpriseId";

        public const string DELETE_ENTERPRISE = "DELETE FROM T_Enterprise WHERE PK_Enterprise = @enterprise";
    }
}