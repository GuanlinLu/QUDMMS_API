using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QUDMMSAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace QUDMMSAPI.Common
{
    public class EmailCheckHelper
    {
        public static bool CheckEmailaddress(string emailAdd_front)
        {
            Regex r = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            bool result = false;
            result = r.IsMatch(emailAdd_front);
            if (result) { return true; } else { return false; }
        }
    }
}
