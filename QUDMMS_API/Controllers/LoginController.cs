using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QUDMMSAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace theLWebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region 【theL】 - Login - LoginAuthentication

        [HttpPost]
        public async Task<ActionResult> LoginAuthentication(JObject Parameter)
        {
            try
            {
                object Param_6YJAbXWFxC = new

                {
                    userNetId = Convert.ToString(Parameter["userNetId"]),
                    login_pw = Convert.ToString(Parameter["login_pw"])

                };

                DataTable DT_6YJAbXWFxC = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_6YJAbXWFxC"), Param_6YJAbXWFxC);

                if (DT_6YJAbXWFxC.Rows.Count == 0) { return BadRequest("Account does not exist or Password does not match."); }

                string userNetId = DT_6YJAbXWFxC.Rows[0]["userNetId"].ToString();

                string Token = Guid.NewGuid().ToString();

                object Param_LHAC81aJ46 = new
                {
                    Token = Convert.ToString(Token),
                    userNetId = Convert.ToString(userNetId)
                };

                await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_LHAC81aJ46"), Param_LHAC81aJ46);

                JObject Json_Result = new JObject();

                Json_Result.Add("userNetId", userNetId);

                Json_Result.Add("first_name", Convert.ToString(DT_6YJAbXWFxC.Rows[0]["first_name"]));

                Json_Result.Add("last_name", Convert.ToString(DT_6YJAbXWFxC.Rows[0]["last_name"]));

                Json_Result.Add("token", Token);

                return Ok(Json_Result);
            }
            catch (Exception ex)
            {
                return BadRequest("Unknow error occurs when login authentication, please contact administrator.");
            }
        }

        #endregion
    }
}
