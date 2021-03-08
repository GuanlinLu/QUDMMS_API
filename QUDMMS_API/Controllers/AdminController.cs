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
    public class AdminController : ControllerBase
    {
        #region[QUDMMS] - Admin - TAMS - InstructorMS
        [HttpPost]
        public async Task<ActionResult> BrowseInstructorList(JObject Parameter)
        {
            try
            {
                /*Validation Required parameter */
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
                string Original_zq61H4SJdL = XMLHelper.GetSql("SQL_zq61H4SJdL");// get recod amount
                Original_zq61H4SJdL = Original_zq61H4SJdL.Replace("@dynamic_condition", Sql);
                DataTable DT_zq61H4SJdL = await DapperHelper.ExecuteSqlDataTableAsync(Original_zq61H4SJdL, DyParam_InstructorList);

                /*Get pagination */
                DyParam_InstructorList.Add("page_number", (Convert.ToInt32(Parameter["page_number"]) - 1) * 10);
                string Original_U83Eb0bl4Q = XMLHelper.GetSql("SQL_U83Eb0bl4Q");
                Original_U83Eb0bl4Q = Original_U83Eb0bl4Q.Replace("@dynamic_condition", Sql);
                DataTable DT_U83Eb0bl4Q = await DapperHelper.ExecuteSqlDataTableAsync(Original_U83Eb0bl4Q, DyParam_InstructorList);// get datas

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


        }//tested

        [HttpPost]
        public async Task<ActionResult> CreateInstructor(JObject Parameter)//tested
        {
            try
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

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_gdUg2TfBXJ"), Param_gdUg2TfBXJ);// Insert InstructorInfo

                JArray Jarray_teachingHistory = (JArray)Parameter["teaching_history"];

                foreach (var jjs in Jarray_teachingHistory)
                {
                    Object Param_XaFayq70bN = new
                    {
                        instructor_id = Convert.ToString(Parameter["instructor_id"]),
                        course_code = Convert.ToString(jjs["course_code"]),
                        course_title = Convert.ToString(jjs["course_title"]),
                        section_number = Convert.ToString(jjs["section_number"]),
                        teaching_year = Convert.ToString(jjs["teaching_year"])
                    };
                    await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_XaFayq70bN"), Param_XaFayq70bN);
                }

                return Ok("Create instructor profile success.");

            }
            catch (Exception ex) { return BadRequest("Create insrtuctor profile failed."); }


        }

        [HttpPost]
        public async Task<ActionResult> InstructorDetail(JObject Parameter)//tested
        {
            try
            {
                object Param_Hy8e8CCYuJ = new
                {
                    instructor_id = Convert.ToString(Parameter["instructor_id"])
                };

                DataTable DT_Hy8e8CCYuJ = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_Hy8e8CCYuJ"), Param_Hy8e8CCYuJ);
                //JObject Json_Hy8e8CCYuJ = (JObject)JArray.Parse(JsonConvert.SerializeObject(DT_Hy8e8CCYuJ))[0];

                object Param_abyki3T37e = new
                {
                    instructor_id = Convert.ToString(Parameter["instructor_id"])
                };

                DataTable DT_abyki3T37e = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_abyki3T37e"), Param_abyki3T37e);
                JArray jArray_abyki3T37e = new JArray();
                for (int i = 0; i < DT_abyki3T37e.Rows.Count; i++)
                {
                    JObject Json = new JObject();
                    Json.Add("course_code", Convert.ToString(DT_abyki3T37e.Rows[i]["course_code"]));
                    Json.Add("course_title", Convert.ToString(DT_abyki3T37e.Rows[i]["course_title"]));
                    Json.Add("teaching_year", Convert.ToString(DT_abyki3T37e.Rows[i]["teaching_year"]));
                    Json.Add("section_number", Convert.ToString(DT_abyki3T37e.Rows[i]["section_number"]));
                    jArray_abyki3T37e.Add(Json);
                }
                JObject Json_Result = (JObject)JArray.Parse(JsonConvert.SerializeObject(DT_Hy8e8CCYuJ))[0];
                Json_Result.Add("teaching_history", jArray_abyki3T37e);
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
        }//tested

        [HttpPost]
        public async Task<ActionResult> UpdateInstructor(JObject Parameter)
        {
            try
            {
                Object Param_E6W9vT3JdU = new
                {
                    instructor_id = Convert.ToString(Parameter["instructor_id"]),
                    title = Convert.ToString(Parameter["title"]),
                    first_name = Convert.ToString(Parameter["first_name"]),
                    last_name = Convert.ToString(Parameter["last_name"]),
                    email = Convert.ToString(Parameter["email"]),
                    years_exp = Convert.ToString(Parameter["years_exp"]),
                    current_FTE = Convert.ToString(Parameter["current_FTE"]),
                    instructor_status = Convert.ToString(Parameter["instructor_status"]),
                    c_working_status = Convert.ToString(Parameter["c_working_status"]),
                    avaliable_term = Convert.ToString(Parameter["avaliable_term"]),
                    teaching_load = Convert.ToString(Parameter["teaching_load"]),
                    admin_load = Convert.ToString(Parameter["admin_load"]),
                    cfwd_load = Convert.ToString(Parameter["cfwd_load"]),
                    total_load = Convert.ToString(Parameter["total_load"])
                };
                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_E6W9vT3JdU"), Param_E6W9vT3JdU);// update basic info

                JArray Jarray_teachingHistory = (JArray)Parameter["teaching_history"];
                foreach (var jjs in Jarray_teachingHistory)
                {
                    Object Param_b2nuUdjHBS = new
                    {
                        instructor_id = Convert.ToString(Parameter["instructor_id"]),
                        course_code = Convert.ToString(jjs["course_code"]),
                        course_title = Convert.ToString(jjs["course_title"]),
                        section_number = Convert.ToString(jjs["section_number"]),
                        teaching_year = Convert.ToString(jjs["teaching_year"])
                    };
                    await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_b2nuUdjHBS"), Param_b2nuUdjHBS);// update teaching history
                }

                return Ok("Update instructor profile success.");
            }
            catch (Exception ex) { return BadRequest("Create insrtuctor profile failed."); }



        }//tested
        #endregion

        #region[QUDMMS]-Admin-TAMS-CourseMS 
        [HttpPost]
        public async Task<ActionResult> BrowseCourseList(JObject Parameter)
        {
            try
            {
                /*Validation Required parameter */
                if (string.IsNullOrEmpty(Convert.ToString(Parameter["page_number"])))
                {
                    return BadRequest("Bad request, page number is required");
                }

                string Sql = "";
                DynamicParameters DyParam_InstructorList = new DynamicParameters();

                /*Dynamic Parameter Alter*/

                if (!string.IsNullOrEmpty(Convert.ToString(Parameter["course_code"])))
                {
                    string instructor_id = Convert.ToString(Parameter["course_code"]);
                    Sql += "AND `course_code` = @course_code ";
                }


                if (!string.IsNullOrEmpty(Convert.ToString(Parameter["program"])))
                {
                    string instructor_id = Convert.ToString(Parameter["program"]);
                    Sql += "AND `program` = @program";
                }

                if (Sql != "") { Sql = "WHERE 1 = 1 " + Sql; }

                /*calculate the total amount of records*/
                string Original_8aN4nnYYVz = XMLHelper.GetSql("SQL_8aN4nnYYVz");// get recod amount
                Original_8aN4nnYYVz = Original_8aN4nnYYVz.Replace("@dynamic_condition", Sql);
                DataTable DT_8aN4nnYYVz = await DapperHelper.ExecuteSqlDataTableAsync(Original_8aN4nnYYVz, DyParam_InstructorList);

                /*Get pagination */
                DyParam_InstructorList.Add("page_number", (Convert.ToInt32(Parameter["page_number"]) - 1) * 10);
                string Original_yZZSnuIMA4 = XMLHelper.GetSql("SQL_yZZSnuIMA4");
                Original_yZZSnuIMA4 = Original_yZZSnuIMA4.Replace("@dynamic_condition", Sql);
                DataTable DT_yZZSnuIMA4 = await DapperHelper.ExecuteSqlDataTableAsync(Original_yZZSnuIMA4, DyParam_InstructorList);// get datas

                JArray jArray_Data = JArray.Parse(JsonConvert.SerializeObject(DT_yZZSnuIMA4));

                JArray jArray_Columns = new JArray();

                for (int i = 0; i < DT_yZZSnuIMA4.Columns.Count; i++)
                {
                    JObject Json = new JObject();
                    Json.Add("tittle", DT_yZZSnuIMA4.Columns[i].ColumnName);
                    Json.Add("dataIndex", DT_yZZSnuIMA4.Columns[i].ColumnName);

                    jArray_Columns.Add(Json);
                }

                for (int j = 0; j < jArray_Data.Count; j++)
                {
                    ((JObject)jArray_Data[j]).Add("key", j);
                }

                JObject Json_Result = new JObject();

                Json_Result.Add("pageSum", Convert.ToInt32(DT_8aN4nnYYVz.Rows[0]["pageSum"]));

                Json_Result.Add("columbData", jArray_Columns);

                Json_Result.Add("rowData", jArray_Data);

                return Ok(Json_Result);
            }
            catch (Exception ex) { return BadRequest("Get course list failed."); }
        }//tested

        [HttpPost]
        public async Task<ActionResult> CreateCourse(JObject Parameter)
        {
            try
            {
                Object Param_wX7GCRLjmN = new
                {
                    course_code = Convert.ToString(Parameter["course_code"]),
                    program = Convert.ToString(Parameter["program"]),
                    course_level = Convert.ToString(Parameter["course_level"]),
                    course_title = Convert.ToString(Parameter["course_title"]),
                    course_description = Convert.ToString(Parameter["course_description"]),
                    course_prerequisite = Convert.ToString((JArray)Parameter["course_prerequisite"]),
                    course_topics = Convert.ToString(Parameter["course_topics"]),
                    course_weight = Convert.ToString(Parameter["course_weight"]),
                    course_teaching_load = Convert.ToString(Parameter["course_teaching_load"]),
                    course_note = Convert.ToString(Parameter["course_note"]),
                    combined_course = Convert.ToString(Parameter["combined_course"])
                };

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_wX7GCRLjmN"), Param_wX7GCRLjmN);

                return Ok("Create course success.");

            }
            catch (Exception ex)
            {
                return BadRequest("Create course failed.");

            }


        }//tested

        [HttpPost]
        public async Task<ActionResult> CourseDetail(JObject Parameter)
        {
            try
            {
                Object Param_YmSO0uis0Y = new
                {
                    course_code = Convert.ToString(Parameter["course_code"])
                };
                DataTable DT_YmSO0uis0Y = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_YmSO0uis0Y"), Param_YmSO0uis0Y);

                JObject Json_Result = (JObject)JArray.Parse(JsonConvert.SerializeObject(DT_YmSO0uis0Y))[0];

                return Ok(Json_Result);
            }
            catch (Exception ex) { return BadRequest("Get course detail failed."); }
        }//tested

        [HttpPost]
        public async Task<ActionResult> DeleteCourse(JObject Parameter)
        {
            try
            {
                Object Param_274n2jCFPi = new
                {
                    course_code = Convert.ToString(Parameter["course_code"])
                };
                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_274n2jCFPi"), Param_274n2jCFPi);
                return Ok("Course Deleted.");
            }
            catch (Exception ex) { return BadRequest("Delete course failed."); }
        }//tested

        [HttpPost]
        public async Task<ActionResult> UpdateCourse(JObject Parameter)
        {
            try
            {
                Object Param_UOMyQmL2rw = new
                {
                    course_code = Convert.ToString(Parameter["course_code"]),
                    program = Convert.ToString(Parameter["program"]),
                    course_level = Convert.ToString(Parameter["course_level"]),
                    course_title = Convert.ToString(Parameter["course_title"]),
                    course_description = Convert.ToString(Parameter["course_description"]),
                    course_prerequisite = Convert.ToString((JArray)Parameter["course_prerequisite"]),
                    course_topics = Convert.ToString(Parameter["course_topics"]),
                    course_weight = Convert.ToString(Parameter["course_weight"]),
                    course_teaching_load = Convert.ToString(Parameter["course_teaching_load"]),
                    course_note = Convert.ToString(Parameter["course_note"]),
                    combined_course = Convert.ToString(Parameter["combined_course"])
                };

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_UOMyQmL2rw"), Param_UOMyQmL2rw);

                return Ok("Update course Information success.");
            }
            catch (Exception ex) { return BadRequest("Update course information failed."); }
        }//tested
        #endregion

        #region [QUDMMS]-Admin-TAMS-Committee
        #endregion
    }
}
