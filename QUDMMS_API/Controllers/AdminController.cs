﻿using Dapper;
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
    public class AdminController : ControllerBase
    {
        #region[QUDMMS] - Admin - TAMS - InstructorMS
        [HttpPost]
        public async Task<ActionResult> BrowseInstructorList(JObject Parameter)
        {
            try
            {/*Validation Required parameter */
                if (string.IsNullOrEmpty(Convert.ToString(Parameter["page_number"])))
                {
                    return BadRequest("Bad request, page number is required");
                }

                string Sql = "";
                DynamicParameters DyParam_InstructorList = new DynamicParameters();

                /*Dynamic Parameter Alter*/

                if (!string.IsNullOrEmpty(Convert.ToString(Parameter["name"])))
                {
                    string Full_name = Convert.ToString(Parameter["name"]);
                    var Names = Full_name.Split(' ', '.', ',');
                    string First_name = Names[0];
                    string Last_name = Names[1];

                    Sql += "AND `first_name` = @first_name AND `last_name` = @last_name" + " ";

                    DyParam_InstructorList.Add("first_name", First_name);
                    DyParam_InstructorList.Add("last_name", Last_name);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(Parameter["instructor_id"])))
                {
                    string instructor_id = Convert.ToString(Parameter["instructor_id"]);
                    Sql += "AND `instructor_id` = @instructor_id ";
                }

                if (Sql != "") { Sql = "WHERE 1 = 1 " + Sql; }

                /*calculate the total amount of records*/
                string Original_zq61H4SJdL = XMLHelper.GetSql("SQ_zq61H4SJdL");
                Original_zq61H4SJdL = Original_zq61H4SJdL.Replace("@dynamic_condition", Sql);
                DataTable DT_zq61H4SJdL = await DapperHelper.ExecuteSqlDataTableAsync(Original_zq61H4SJdL, DyParam_InstructorList);

                /*Get pagination */
                DyParam_InstructorList.Add("page_number", (Convert.ToInt32(Parameter["page_number"]) - 1) * 10);
                string Original_U83Eb0bl4Q = XMLHelper.GetSql("SQL_U83Eb0bl4Q");
                Original_U83Eb0bl4Q = Original_U83Eb0bl4Q.Replace("@dynamic_condition", Sql);

                DataTable DT_U83Eb0bl4Q = await DapperHelper.ExecuteSqlDataTableAsync(Original_U83Eb0bl4Q, DyParam_InstructorList);

                JArray jArray_Data = JArray.Parse(JsonConvert.SerializeObject(DT_U83Eb0bl4Q));

                JArray jArray_Columns = new JArray();

                for (int i = 0; i < DT_U83Eb0bl4Q.Columns.Count; i++)
                {
                    JObject Json = new JObject();
                    Json.Add("tittle", DT_U83Eb0bl4Q.Columns[i].ColumnName);
                    Json.Add("dataIndex", DT_U83Eb0bl4Q.Columns[i].ColumnName);

                    jArray_Columns.Add(Json);
                }

                for (int j = 0; j < jArray_Data.Count; j++)
                {
                    ((JObject)jArray_Data[j]).Add("key", j);
                }

                JObject Json_Result = new JObject();

                Json_Result.Add("pageSum", Convert.ToInt32(DT_zq61H4SJdL.Rows[0]["pageSum"]));

                Json_Result.Add("columbData", jArray_Columns);

                Json_Result.Add("rowData", jArray_Data);

                return Ok(Json_Result);
            }
            catch (Exception ex)
            {
                return BadRequest("Unknow Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateInstructor(JObject Parameter)
        {

            Object Param_gdUg2TfBXJ = new
            {
                instructor_id = Convert.ToString(Parameter["instructor_id"]),
                title = Convert.ToString(Parameter["title"]),
                first_name = Convert.ToString(Parameter["first_name"]),
                last_name = Convert.ToString(Parameter["last_name"]),
                email = Convert.ToString(Parameter["email"]),
                years_exp = Convert.ToString(Parameter["years_exp"])
            };
            //string aa = " INSERT INTO `QUDMMS_DB`.`QUDM_TAMS_instructorInfo`(`instructor_id`,`title`,`first_name`,`last_name`,`email`,`years_exp`)VALUE(@instructor_id,@title,@first_name,@last_name,@email,@years_exp); ";

            await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_gdUg2TfBXJ"), Param_gdUg2TfBXJ);

            return Ok("Create instructor profile success.");

        }

        [HttpPost]
        public async Task<ActionResult> InstructorDetail(JObject Parameter)
        {
            try
            {
                object Param_Hy8e8CCYuJ = new
                {
                    instructor_id = Convert.ToString(Parameter["instructor_id"])
                };

                DataTable DT_Hy8e8CCYuJ = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_Hy8e8CCYuJ"), Param_Hy8e8CCYuJ);
                JObject Json_Hy8e8CCYuJ = (JObject)JArray.Parse(JsonConvert.SerializeObject(DT_Hy8e8CCYuJ))[0];
                JObject Json_Result = Json_Hy8e8CCYuJ;
                return Ok(Json_Result);
            }
            catch (Exception ex) { return BadRequest("Load Instructor Detail Failed."); }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteInstructor(JObject Parameter)
        {
            try
            {
                Object Param_2fufaYyDsT = new
                {
                    instructor_id = Convert.ToString(Parameter["instructor_id"])
                };

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_2fufaYyDsT"), Param_2fufaYyDsT);
                return Ok("Instructor profile delted");
            }
            catch (Exception ex) { return BadRequest("Delete instructor profile Failed"); }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateInstructor(JObject Parameter)
        {
            try { return Ok(""); } catch (Exception ex) { return BadRequest("UpdateInstructorFailed"); }
        }
        #endregion
    }
}