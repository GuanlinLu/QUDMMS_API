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
            //try
            //{
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest("Unknow Error");
            //}


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
                DyParam_InstructorList.Add("instructor_id", instructor_id);
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
                Json.Add("title", DT_U83Eb0bl4Q.Columns[i].ColumnName);
                Json.Add("dataIndex", DT_U83Eb0bl4Q.Columns[i].ColumnName);

                jArray_Columns.Add(Json);
            }

            for (int j = 0; j < jArray_Data.Count; j++)
            {
                ((JObject)jArray_Data[j]).Add("key", j);
            }

            JObject Json_Result = new JObject();

            Json_Result.Add("pageSum", Convert.ToInt32(DT_zq61H4SJdL.Rows[0]["pageSum"]));

            Json_Result.Add("columnData", jArray_Columns);

            Json_Result.Add("rowData", jArray_Data);

            return Ok(Json_Result);


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
            //try
            //{

            //}
            //catch (Exception ex) { return BadRequest("Update insrtuctor profile failed."); }


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
                available_term = Convert.ToString(Parameter["available_term"]),
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



        }//tested
        #endregion

        #region[QUDMMS]-Admin-TAMS-CourseMS 
        [HttpPost]
        public async Task<ActionResult> BrowseCourseList(JObject Parameter)
        {
            //try
            //{

            //}
            //catch (Exception ex) { return BadRequest("Get course list failed."); }

            /*Validation Required parameter */

            if (string.IsNullOrEmpty(Convert.ToString(Parameter["page_number"])))
            {
                return BadRequest("Bad request, page number is required");
            }

            string Sql = "";
            DynamicParameters DyParam_courseList = new DynamicParameters();

            /*Dynamic Parameter Alter*/

            if (!string.IsNullOrEmpty(Convert.ToString(Parameter["course_code"])))
            {
                string course_code = Convert.ToString(Parameter["course_code"]);
                Sql += "AND `course_code` = @course_code ";
                DyParam_courseList.Add("course_code", course_code);
            }


            if (!string.IsNullOrEmpty(Convert.ToString(Parameter["program"])))
            {
                string program = Convert.ToString(Parameter["program"]);
                Sql += "AND `program` = @program";
                DyParam_courseList.Add("program", program);
            }

            if (Sql != "") { Sql = "WHERE 1 = 1 " + Sql; }

            /*calculate the total amount of records*/
            string Original_8aN4nnYYVz = XMLHelper.GetSql("SQL_8aN4nnYYVz");// get recod amount
            Original_8aN4nnYYVz = Original_8aN4nnYYVz.Replace("@dynamic_condition", Sql);
            DataTable DT_8aN4nnYYVz = await DapperHelper.ExecuteSqlDataTableAsync(Original_8aN4nnYYVz, DyParam_courseList);

            /*Get pagination */
            DyParam_courseList.Add("page_number", (Convert.ToInt32(Parameter["page_number"]) - 1) * 10);
            string Original_yZZSnuIMA4 = XMLHelper.GetSql("SQL_yZZSnuIMA4");
            Original_yZZSnuIMA4 = Original_yZZSnuIMA4.Replace("@dynamic_condition", Sql);
            DataTable DT_yZZSnuIMA4 = await DapperHelper.ExecuteSqlDataTableAsync(Original_yZZSnuIMA4, DyParam_courseList);// get datas

            JArray jArray_Data = JArray.Parse(JsonConvert.SerializeObject(DT_yZZSnuIMA4));

            JArray jArray_Columns = new JArray();

            for (int i = 0; i < DT_yZZSnuIMA4.Columns.Count; i++)
            {
                JObject Json = new JObject();
                Json.Add("title", DT_yZZSnuIMA4.Columns[i].ColumnName);
                Json.Add("dataIndex", DT_yZZSnuIMA4.Columns[i].ColumnName);

                jArray_Columns.Add(Json);
            }

            for (int j = 0; j < jArray_Data.Count; j++)
            {
                ((JObject)jArray_Data[j]).Add("key", j);
            }

            JObject Json_Result = new JObject();

            Json_Result.Add("pageSum", Convert.ToInt32(DT_8aN4nnYYVz.Rows[0]["pageSum"]));

            Json_Result.Add("columnData", jArray_Columns);

            Json_Result.Add("rowData", jArray_Data);

            return Ok(Json_Result);
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

        #region [QUDMMS]-Admin-TAMS-Assignment
        [HttpPost]
        public async Task<ActionResult> BrowseAssnList(JObject Parameter)
        {
            try
            {  /*Validation Required parameter */
                if (string.IsNullOrEmpty(Convert.ToString(Parameter["page_number"])))
                {
                    return BadRequest("Bad request, page number is required");
                }

                string Sql = "";
                DynamicParameters DyParam_InstructorList = new DynamicParameters();

                /*Dynamic Parameter Alter*/

                if (!string.IsNullOrEmpty(Convert.ToString(Parameter["name"])))
                {
                    string Full_name = Convert.ToString(Parameter[""]);
                    var Names = Full_name.Split(' ', '.', ',');
                    string First_name = Names[0];
                    string Last_name = Names[1];

                    Sql += "AND `first_name` = @first_name AND `last_name` = @last_name" + " ";

                    DyParam_InstructorList.Add("first_name", First_name);
                    DyParam_InstructorList.Add("last_name", Last_name);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(Parameter["course_code"])))
                {
                    string course_code = Convert.ToString(Parameter["course_code"]);
                    Sql += "AND `course_code` = @course_code";
                    DyParam_InstructorList.Add("course_code", course_code);
                }

                if (Sql != "") { Sql = "WHERE 1 = 1 " + Sql; }

                /*calculate the total amount of records*/
                string Original_3Y76ML19Mx = XMLHelper.GetSql("SQL_3Y76ML19Mx");// get recod amount
                Original_3Y76ML19Mx = Original_3Y76ML19Mx.Replace("@dynamic_condition", Sql);
                DataTable DT_3Y76ML19Mx = await DapperHelper.ExecuteSqlDataTableAsync(Original_3Y76ML19Mx, DyParam_InstructorList);

                /*Get pagination */
                DyParam_InstructorList.Add("page_number", (Convert.ToInt32(Parameter["page_number"]) - 1) * 10);
                string Original_LfsAIEjVL4 = XMLHelper.GetSql("SQL_LfsAIEjVL4");
                Original_LfsAIEjVL4 = Original_LfsAIEjVL4.Replace("@dynamic_condition", Sql);
                DataTable DT_LfsAIEjVL4 = await DapperHelper.ExecuteSqlDataTableAsync(Original_LfsAIEjVL4, DyParam_InstructorList);// get datas

                JArray jArray_Data = JArray.Parse(JsonConvert.SerializeObject(DT_LfsAIEjVL4));

                JArray jArray_Columns = new JArray();

                for (int i = 0; i < DT_LfsAIEjVL4.Columns.Count; i++)
                {
                    JObject Json = new JObject();
                    Json.Add("title", DT_LfsAIEjVL4.Columns[i].ColumnName);
                    Json.Add("dataIndex", DT_LfsAIEjVL4.Columns[i].ColumnName);

                    jArray_Columns.Add(Json);
                }

                for (int j = 0; j < jArray_Data.Count; j++)
                {
                    ((JObject)jArray_Data[j]).Add("key", j);
                }

                JObject Json_Result = new JObject();

                Json_Result.Add("pageSum", Convert.ToInt32(DT_3Y76ML19Mx.Rows[0]["pageSum"]));

                Json_Result.Add("columnData", jArray_Columns);

                Json_Result.Add("rowData", jArray_Data);

                return Ok(Json_Result);
            }
            catch (Exception ex) { return BadRequest("Get assignment failed."); }

        }//tested

        [HttpPost]
        public async Task<ActionResult> CreateAssnIns(JObject Parameter)
        {
            try
            {
                Object Param_B1TVxNdzqj = new
                {
                    instructor_1_id = Convert.ToString(Parameter["instructor_id"]),
                    instructor_1_name = Convert.ToString(Parameter["instructor_name"]),
                    course_code = Convert.ToString(Parameter["course_code"]),
                    course_title = Convert.ToString(Parameter["course_title"]),
                    section_number = Convert.ToString(Parameter["section_number"]),
                    assn_term = Convert.ToString(Parameter["assn_term"]),
                    assn_year = Convert.ToString(Parameter["assn_year"]),
                    course_delivery = Convert.ToString(Parameter["course_delivery"]),
                    assn_notes = Convert.ToString(Parameter["assn_notes"]),
                    program = Convert.ToString(Parameter["program"])
                };


                //await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_B1TVxNdzqj"));

                DataTable DT_B1TVxNdzqj = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_B1TVxNdzqj"), Param_B1TVxNdzqj);

                JArray Jarray_weeklySchedule = (JArray)Parameter["weeklySchedule"];

                foreach (var jws in Jarray_weeklySchedule)
                {
                    Object Param_I7EwPEEqq7 = new
                    {
                        course_code = Convert.ToString(Parameter["course_code"]),
                        section_number = Convert.ToString(Parameter["section_number"]),
                        lecture_day = Convert.ToString(jws["lecture_day"]),
                        teaching_address = Convert.ToString(jws["teaching_address"]),
                        lecture_start_time = Convert.ToString(jws["startTime"]),
                        lecture_end_time = Convert.ToString(jws["endTime"]),
                        instructor_id = Convert.ToString(Parameter["instructor_id"]),
                        instructor_name = Convert.ToString(Parameter["instructor_name"]),
                        assn_id = Convert.ToString(DT_B1TVxNdzqj.Rows[0]["assn_id"]),
                        program = Convert.ToString(Parameter["program"])
                    };

                    await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_I7EwPEEqq7"), Param_I7EwPEEqq7);
                };

                return (Ok("Create assignment success."));
            }
            catch (Exception ex) { return BadRequest("Create assignment failed."); }

        }//tested

        [HttpPost]
        public async Task<ActionResult> AssnDetail(JObject Parameter)
        {
            try
            {
                Object Param_KVxRK0QXiK = new
                {
                    assn_id = Convert.ToString(Parameter["assn_id"])
                };
                DataTable DT_KVxRK0QXiK = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_KVxRK0QXiK"), Param_KVxRK0QXiK);

                Object Param_9lCwockMTF = new
                {
                    //course_code = Convert.ToString(Parameter["course_code"]),
                    //section_number = Convert.ToString(Parameter["section_number"]),
                    //instructor_id = Convert.ToString(Parameter["instructor_id"])\
                    assn_id = Convert.ToString(Parameter["assn_id"])
                };
                JArray jArray_9lCwockMTF = new JArray();
                DataTable DT_9lCwockMTF = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_9lCwockMTF"), Param_9lCwockMTF);
                for (int i = 0; i < DT_9lCwockMTF.Rows.Count; i++)
                {
                    JObject Json = new JObject();

                    Json.Add("course_code", Convert.ToString(DT_9lCwockMTF.Rows[i]["course_code"]));
                    Json.Add("section_number", Convert.ToString(DT_9lCwockMTF.Rows[i]["section_number"]));
                    Json.Add("lecture_day", Convert.ToString(DT_9lCwockMTF.Rows[i]["lecture_day"]));
                    Json.Add("startTime", Convert.ToString(DT_9lCwockMTF.Rows[i]["lecture_start_time"]));
                    Json.Add("endTime", Convert.ToString(DT_9lCwockMTF.Rows[i]["lecture_end_time"]));
                    Json.Add("teaching_address", Convert.ToString(DT_9lCwockMTF.Rows[i]["teaching_address"]));
                    jArray_9lCwockMTF.Add(Json);
                }
                JObject Json_Result = (JObject)JArray.Parse(JsonConvert.SerializeObject(DT_KVxRK0QXiK))[0];
                Json_Result.Add("weekly_Schedule", jArray_9lCwockMTF);
                return Ok(Json_Result);
            }
            catch (Exception ex) { return BadRequest("Get Assignment Detail Failed."); }

        }//tested

        [HttpPost]
        public async Task<ActionResult> DeleteAssn(JObject Parameter)
        {
            try
            {
                Object Param_2ASmH5jDGB = new
                {
                    assn_id = Convert.ToString(Parameter["assn_id"])
                };

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_2ASmH5jDGB"), Param_2ASmH5jDGB);

                Object Param_PIq2W8EV22 = new
                {
                    assn_id = Convert.ToString(Parameter["assn_id"])
                };
                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_PIq2W8EV22"), Param_PIq2W8EV22);

                return Ok("Assignment Deleted.");
            }
            catch (Exception ex) { return BadRequest("Delete assignment failed."); }

        }//tested

        [HttpPost]
        public async Task<ActionResult> UpdateAssn(JObject Parameter)
        {
            try
            {
                Object Param_sL8HUUUQFg = new
                {
                    assn_id = Convert.ToString(Parameter["assn_id"]),
                    instructor_1_id = Convert.ToString(Parameter["instructor_id"]),
                    instructor_1_name = Convert.ToString(Parameter["instructor_name"]),
                    course_code = Convert.ToString(Parameter["course_code"]),
                    course_title = Convert.ToString(Parameter["course_title"]),
                    section_number = Convert.ToString(Parameter["section_number"]),
                    assn_term = Convert.ToString(Parameter["assn_term"]),
                    assn_year = Convert.ToString(Parameter["assn_year"]),
                    course_delivery = Convert.ToString(Parameter["course_delivery"]),
                    assn_notes = Convert.ToString(Parameter["assn_notes"]),
                    program = Convert.ToString(Parameter["program"])
                };

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_sL8HUUUQFg"), Param_sL8HUUUQFg);//Update Basic &return assn_id


                JArray Jarray_weeklySchedule = (JArray)Parameter["weekly_Schedule"];

                foreach (var jws in Jarray_weeklySchedule)
                {
                    Object Param_fPaqQUQ7mK = new
                    {
                        assn_id = Convert.ToString(Parameter["assn_id"]),
                        lecture_id = Convert.ToString(jws["lecture_id"]),
                        course_code = Convert.ToString(Parameter["course_code"]),
                        section_number = Convert.ToString(Parameter["section_number"]),
                        lecture_day = Convert.ToString(jws["lecture_day"]),
                        teaching_address = Convert.ToString(jws["teaching_address"]),
                        lecture_start_time = Convert.ToString(jws["startTime"]),
                        lecture_end_time = Convert.ToString(jws["endTime"]),
                        instructor_id = Convert.ToString(Parameter["instructor_id"]),
                        instructor_name = Convert.ToString(Parameter["instructor_name"]),
                        program = Convert.ToString(Parameter["program"])

                    };

                    await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_fPaqQUQ7mK"), Param_fPaqQUQ7mK);
                };

                return Ok("Update success.");
            }
            catch (Exception ex) { return BadRequest("Update assignment failed."); }

        }//tested

        [HttpPost]
        public async Task<ActionResult> getCourseOptions()
        {
            DataTable DT_ax4gHdpezv = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_ax4gHdpezv"));
            JArray Json_Result = new JArray();
            for (int i = 0; i < DT_ax4gHdpezv.Rows.Count; i++)
            {
                Json_Result.Add(Convert.ToString(DT_ax4gHdpezv.Rows[i]["course_code"]));
            }
            return Ok(Json_Result);
        }


        [HttpPost]
        public async Task<ActionResult> GetInstructorOptions(JObject Parameter)
        {
            //DataTable DT_hVy4fa7F1I = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_hVy4fa7F1I"));

            //string first_name = JsonConvert.SerializeObject(DT_hVy4fa7F1I))[0][];
            //string last_name = JsonConvert.SerializeObject(DT_hVy4fa7F1I))[1];
            //JArray Json_Result = new JArray();
            //for (int i = 0; i < DT_hVy4fa7F1I.Rows.Count; i++)
            //{
            //    Json_Result.Add(Convert.ToString(DT_hVy4fa7F1I.Rows[i]["subject_name"]));
            //}
            return Ok();
        }

        #endregion

        #region [QUDMMS]- Admin-TAMS-Backup
        #endregion

        #region [QUDMMS]-Admin-TAMS-UserMS
        [HttpPost]

        public async Task<ActionResult> BrowseUserList(JObject Parameter)//tested
        {
            try
            {  /*Validation Required parameter */
                if (string.IsNullOrEmpty(Convert.ToString(Parameter["page_number"])))
                {
                    return BadRequest("Bad request, page number is required");
                }

                string Sql = "";
                DynamicParameters DyParam_InstructorList = new DynamicParameters();

                /*Dynamic Parameter Alter*/

                if (!string.IsNullOrEmpty(Convert.ToString(Parameter["name"])))
                {
                    string Full_name = Convert.ToString(Parameter[""]);
                    var Names = Full_name.Split(' ', '.', ',');
                    string First_name = Names[0];
                    string Last_name = Names[1];

                    Sql += "AND `first_name` = @first_name AND `last_name` = @last_name" + " ";

                    DyParam_InstructorList.Add("first_name", First_name);
                    DyParam_InstructorList.Add("last_name", Last_name);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(Parameter["NetId"])))
                {
                    string NetId = Convert.ToString(Parameter["NetId"]);
                    Sql += "AND `NetId` = @NetId";
                    DyParam_InstructorList.Add("NetId", NetId);
                }

                if (Sql != "") { Sql = "WHERE 1 = 1 " + Sql; }

                /*calculate the total amount of records*/
                string Original_1Dkms3DKvE = XMLHelper.GetSql("SQL_1Dkms3DKvE");// get recod amount
                Original_1Dkms3DKvE = Original_1Dkms3DKvE.Replace("@dynamic_condition", Sql);
                DataTable DT_1Dkms3DKvE = await DapperHelper.ExecuteSqlDataTableAsync(Original_1Dkms3DKvE, DyParam_InstructorList);

                /*Get pagination */
                DyParam_InstructorList.Add("page_number", (Convert.ToInt32(Parameter["page_number"]) - 1) * 10);
                string Original_9thTSLE1wk = XMLHelper.GetSql("SQL_9thTSLE1wk");
                Original_9thTSLE1wk = Original_9thTSLE1wk.Replace("@dynamic_condition", Sql);
                DataTable DT_9thTSLE1wk = await DapperHelper.ExecuteSqlDataTableAsync(Original_9thTSLE1wk, DyParam_InstructorList);// get datas

                JArray jArray_Data = JArray.Parse(JsonConvert.SerializeObject(DT_9thTSLE1wk));

                JArray jArray_Columns = new JArray();

                for (int i = 0; i < DT_9thTSLE1wk.Columns.Count; i++)
                {
                    JObject Json = new JObject();
                    Json.Add("title", DT_9thTSLE1wk.Columns[i].ColumnName);
                    Json.Add("dataIndex", DT_9thTSLE1wk.Columns[i].ColumnName);

                    jArray_Columns.Add(Json);
                }

                for (int j = 0; j < jArray_Data.Count; j++)
                {
                    ((JObject)jArray_Data[j]).Add("key", j);
                }

                JObject Json_Result = new JObject();

                Json_Result.Add("pageSum", Convert.ToInt32(DT_1Dkms3DKvE.Rows[0]["pageSum"]));

                Json_Result.Add("columnData", jArray_Columns);

                Json_Result.Add("rowData", jArray_Data);

                return Ok(Json_Result);
            }
            catch (Exception ex) { return BadRequest("Get user list failed."); };

        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(JObject Parameter)
        {
            try
            {
                Object Param_JKUH1mDln6 = new
                {
                    userNetId = Convert.ToString(Parameter["userNetId"]),
                    first_name = Convert.ToString(Parameter["first_name"]),
                    last_name = Convert.ToString(Parameter["last_name"]),
                    login_pw = Convert.ToString(Parameter["login_pw"]),
                    user_status = Convert.ToString(Parameter["user_status"])
                };

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_JKUH1mDln6"), Param_JKUH1mDln6);//create basic info

                Object Param_hFpkFMv4Xo = new
                {
                    role_name = Convert.ToString(Parameter["role_name"])
                };

                DataTable DT_hFpkFMv4Xo = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_hFpkFMv4Xo"), Param_hFpkFMv4Xo);

                String role_id = Convert.ToString(DT_hFpkFMv4Xo.Rows[0]["role_id"]);

                Object Param_ivKY3cvabw = new
                {
                    userNetId = Convert.ToString(Parameter["userNetId"]),
                    role_id = role_id
                };

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_ivKY3cvabw"), Param_ivKY3cvabw);
                return Ok("Create user success.");
            }
            catch (Exception ex) { return BadRequest("Create user failed."); }

        }//tested

        [HttpPost]
        public async Task<ActionResult> UserDetail(JObject Parameter)
        {
            try
            {
                Object Param_KG6aVK5I15 = new
                {
                    userNetId = Convert.ToString(Parameter["userNetId"])
                };

                DataTable DT_KG6aVK5I15 = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_KG6aVK5I15"), Param_KG6aVK5I15);

                JObject Json_Result = (JObject)JArray.Parse(JsonConvert.SerializeObject(DT_KG6aVK5I15))[0];
                return Ok(Json_Result);
            }
            catch (Exception ex) { return BadRequest("Get user detail failed."); }

        } //tested

        [HttpPost]
        public async Task<ActionResult> DeleteUser(JObject Parameter)
        {
            try
            {
                Object Param_6ZpTkyYMSB = new
                {
                    userNetId = Convert.ToString(Parameter["userNetId"])
                };

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_6ZpTkyYMSB"), Param_6ZpTkyYMSB);

                return Ok("Deleted user success.");
            }
            catch (Exception ex) { return BadRequest("Delete user failed."); }

        }//tested

        [HttpPost]
        public async Task<ActionResult> UpdateUser(JObject Parameter)
        {
            try
            {
                Object Param_DZWayKi0pb = new
                {
                    userNetId = Convert.ToString(Parameter["userNetId"]),
                    first_name = Convert.ToString(Parameter["first_name"]),
                    last_name = Convert.ToString(Parameter["last_name"]),
                    login_pw = Convert.ToString(Parameter["login_pw"]),
                    user_status = Convert.ToString(Parameter["user_status"])

                };

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_DZWayKi0pb"), Param_DZWayKi0pb);//update user basic info


                Object Param_yH9geiA1js = new
                {
                    role_name = Convert.ToString(Parameter["role_name"])
                };

                DataTable DT_yH9geiA1js = await DapperHelper.ExecuteSqlDataTableAsync(XMLHelper.GetSql("SQL_yH9geiA1js"), Param_yH9geiA1js);//get role id

                String role_id = Convert.ToString(DT_yH9geiA1js.Rows[0]["role_id"]);

                Object Param_Q3BCay6wXV = new
                {
                    userNetId = Convert.ToString(Parameter["userNetId"]),
                    role_id = role_id
                };

                await DapperHelper.ExecuteSqlIntAsync(XMLHelper.GetSql("SQL_Q3BCay6wXV"), Param_Q3BCay6wXV);


                return Ok("Update user success");
            }
            catch (Exception ex) { return BadRequest("Update user failed."); }//tested

        }
        #endregion


    }
}
