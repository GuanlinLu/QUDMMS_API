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

namespace QUDMMSAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CommitteeController : ControllerBase
    {
        #region[QUDMMS] - Admin - Committees
        
               
        [HttpPost]
        public async Task<ActionResult> SearchCommittee(JObject Parameter)
        {
            try
            {
                Object Param_comms_search = new
                {
                    cttee_title = Convert.ToString(Parameter["cttee_title"])
                };

                DataTable DT_Param_comms_search = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_comms_details"), Param_comms_details);
                JObject Json_Param_comms_search = (JObject)JArray.Parse(JsonConvert.SerializeObject(Param_comms_details))[0];
                return Ok(Json_Param_comms_search);
            }
            catch (Exception ex) { return BadRequest("Load Committee Detail Failed."); }
           
        }

        [HttpPost]
        public async Task<ActionResult> CreateCommittee(JObject Parameter)
        {

            Object Param_comms_create = new
            {
                cttee_tittle = Convert.ToString(Parameter["cttee_tittle"]),
                chair_admin_weight = Convert.ToString(Parameter["chair_admin_weight"]),
                co_chair_admin_weight = Convert.ToString(Parameter["co_chair_admin_weight"]),
                secretary_admin_weight = Convert.ToString(Parameter["secretary_admin_weight"]),
                memeber_admin_weight = Convert.ToString(Parameter["memeber_admin_weight"]),
                cttee_year = Convert.ToString(Parameter["ctee_year"]),
                cttee_notes = Convert.ToString(Parameter["cttee_notes"]),
            };
            await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_comms"), Param_comms_create);

            return Ok("Successfully created committee.");
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCommittee(JObject Parameter)
        {
            Object Param_comms_update = new
            {
                cttee_id = Convert.ToString(Parameter["cttee_id"]),
                cttee_tittle = Convert.ToString(Parameter["cttee_tittle"]),
                chair_admin_weight = Convert.ToString(Parameter["chair_admin_weight"]),
                co_chair_admin_weight = Convert.ToString(Parameter["co_chair_admin_weight"]),
                secretary_admin_weight = Convert.ToString(Parameter["secretary_admin_weight"]),
                memeber_admin_weight = Convert.ToString(Parameter["memeber_admin_weight"]),
                cttee_year = Convert.ToString(Parameter["ctee_year"]),
                cttee_notes = Convert.ToString(Parameter["cttee_notes"]),
            };

            await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql(XMLHelper.GetSql("SQL_comms_update")));
            return Ok("Update successful.");
        }

            /*
            Object Param_members = (JArray)Parameter["comm_members"];
           
            foreach (var ms in Param_members)
            {
                Object Param_cttee_member_rel = new
                {
                    cttee_title = Convert.ToString(Parameter["cttee_title"]),
                    belong_cttee = Convert.ToString(Parameter["belong_cttee"]),
                    instructor_id = Convert.ToString(Parameter["instructor_id"]),
                    cttee_role = Convert.ToString(Parameter["rel_id"]),
                    cttee_year = Convert.ToString(Parameter["cttee_year"])
                };

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_cttee_member_rel"));
            };
            */

        [HttpPost]
        public async Task<ActionResult> CreateCommitteeRel(JObject Parameter)
        {
            Object Param_comms_rel_create = new
            {
                    belong_cttee = Convert.ToString(Parameter["belong_cttee"]),
                    cttee_id = Convert.ToString(Parameter["cttee_id"]),
                    instructor_id = Convert.ToString(Parameter["instructor_id"]),
                    cttee_role = Convert.ToString(Parameter["rel_id"]),
                    cttee_year = Convert.ToString(Parameter["cttee_year"])
            };

            await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql(XMLHelper.GetSql("SQL_comms_rel_create")));
            return Ok("Committee role creation was successful.");
        }
    
        [HttpPost]
        public async Task<ActionResult> UpdateCommitteeRel(JObject Parameter)
        {
            Object Param_comms_rel_update = new
            {
                    cttee_title = Convert.ToString(Parameter["cttee_title"]),
                    cttee_role = Convert.ToString(Parameter["cttee_role"]),
                    instructor_id = Convert.ToString(Parameter["instructor_id"]),
                    rel_id = Convert.ToString(Parameter["rel_id"]),
                    cttee_year = Convert.ToString(Parameter["cttee_year"])
            };

            await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql(XMLHelper.GetSql("SQL_comms_rel_update")));
            return Ok("Committee role update was successful.");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCommitteeRel(JObject Parameter)
        {
            Object Param_comms_rel_delete = new
            {
                    cttee_id = Convert.ToString(Parameter["cttee_id"]),
                    instructor_id = Convert.ToString(Parameter["instructor_id"]),
                    cttee_role = Convert.ToString(Parameter["rel_id"]),
                    cttee_year = Convert.ToString(Parameter["cttee_year"])
            };

            await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql(XMLHelper.GetSql("SQL_comms_rel_delete")));
            return Ok("Committee role removal was successful.");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCommittee(JObject Parameter)
        {
            try
            {
                Object Param_comms_del = new
                {
                    cttee_id = Convert.ToString(Parameter["cttee_id"])
                };

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_comms_del"), Param_comms_del);
                return Ok("Committee was successfully removed.");
            }
            catch (Exception ex) { return BadRequest("Committee removal was unsuccessful."); }
        }
        
    }
}
