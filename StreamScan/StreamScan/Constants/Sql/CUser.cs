using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StreamScan.Constants.Sql
{
    public class CUser
    {
        public const string LOGIN = "SELECT username FROM T_User WHERE username = @username AND password = @password";
    }
}