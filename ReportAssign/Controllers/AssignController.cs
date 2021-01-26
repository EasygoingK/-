using ReportAssign.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ReportAssign.Controllers
{
    public class AssignController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //查詢資料
        [HttpPost]
        public ActionResult QueryReport(string accession_num)
        {

            if (accession_num != null)
            {
                string sql = "SELECT PatientID, PatientName, AccessionNum, DoctorID, DoctorName " +
                        "FROM patientlist Where AccessionNum = @AccessionNum";

                var pms = new SqlParameter("@AccessionNum",SqlDbType.VarChar,50);
                pms.Value = accession_num;

                var queryResult = SqlHelper.ExcuteReader(CommandType.Text, sql, pms);

                List<PatientList> patlist = new List<PatientList>();

                if (queryResult.HasRows)
                {
                    while (queryResult.Read())
                    {
                        patlist.Add(new PatientList()
                        {
                            PatientID = Convert.ToString(queryResult["PatientID"]),
                            PatientName = Convert.ToString(queryResult["PatientName"]),
                            AccessionNum = Convert.ToString(queryResult["AccessionNum"]),
                            DoctorID = Convert.ToString(queryResult["DoctorID"]),
                            DoctorName = Convert.ToString(queryResult["DoctorName"])
                        });
                    }
                }

                if (patlist.Count >= 1)
                {
                    ViewBag.checkdata = "ok";
                }

                ViewBag.Test = new SelectList(patlist, "AccessionNum", "PatientName");

                return View("Index", patlist);

            }
            else
            {
                return View("Index");
            }

        }

        //新增資料
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PatientList data)
        {
            if (ModelState.IsValid)
            {
                string sql = "Insert into patientlist (PatientID, PatientName, AccessionNum) Values (@PatientID, @PatientName, @AccessionNum)";

                var pms = new SqlParameter[]
                {
                    new SqlParameter("@PatientID", SqlDbType.VarChar, 50) { Value = data.PatientID },
                    new SqlParameter("@PatientName", SqlDbType.VarChar, 50) { Value = data.PatientID },
                    new SqlParameter("@AccessionNum", SqlDbType.VarChar, 50) { Value = data.PatientID }
                };


                var result = SqlHelper.ExecuteNonQuery(CommandType.Text, sql, pms);

                if (result >= 1)
                {
                    TempData["Result"] = "資料新增成功!";
                }

                return RedirectToAction("Index");
            }

            return View(data);
        }

        //編輯資料
        public ActionResult Edit(string accession_num)
        {
            var data = new PatientList();

            var sql = "SELECT PatientID, PatientName, AccessionNum, DoctorID, DoctorName " +
                   "FROM patientlist Where AccessionNum = @AccessionNum";

            var pms = new SqlParameter("@AccessionNum", SqlDbType.VarChar, 50) { Value = accession_num};
            
            var result = SqlHelper.ExcuteReader(CommandType.Text, sql, pms);

            if (result.HasRows)
            {
                while (result.Read())
                {
                    data.PatientID = Convert.ToString(result["PatientID"]);
                    data.PatientName = Convert.ToString(result["PatientName"]);
                    data.AccessionNum = Convert.ToString(result["AccessionNum"]);
                }
            }

            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(PatientList data)
        {
            if (ModelState.IsValid)
            {
                string sql = "update patientlist  set PatientID = @PatientID, PatientName = @PatientName where AccessionNum = @AccessionNum";

                var pms = new SqlParameter[]
                {
                    new SqlParameter("@PatientID",SqlDbType.VarChar,50){Value=data.PatientID },
                    new SqlParameter("@PatientName",SqlDbType.VarChar,50){Value=data.PatientID },
                    new SqlParameter("@AccessionNum",SqlDbType.VarChar,50){Value=data.AccessionNum }
                };

                var result = SqlHelper.ExecuteNonQuery(CommandType.Text, sql, pms);

                if (result >= 1)
                {
                    TempData["Result"] = "資料更新成功!";
                }

                return RedirectToAction("Index");
            }

            return View(data);
        }

        //刪除資料
        public ActionResult Delete(string accession_num)
        {
            var data = new PatientList();

            var sql = "SELECT PatientID, PatientName, AccessionNum, DoctorID, DoctorName " +
                "FROM patientlist Where AccessionNum = @AccessionNum";

            var pms = new SqlParameter("@AccessionNum", SqlDbType.VarChar, 50) { Value = accession_num };

            var result = SqlHelper.ExcuteReader(CommandType.Text, sql, pms);

            if (result.HasRows)
            {
                if (result.Read())
                {
                    data.PatientID = Convert.ToString(result["PatientID"]);
                    data.PatientName = Convert.ToString(result["PatientName"]);
                    data.AccessionNum = Convert.ToString(result["AccessionNum"]);
                };
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PatientList data)
        {
            if (ModelState.IsValid)
            {
                string sql = "delete patientlist where AccessionNum = @AccessionNum";

                var pms = new SqlParameter("@AccessionNum", SqlDbType.VarChar, 50) { Value = data.AccessionNum };

                var result = SqlHelper.ExecuteNonQuery(CommandType.Text, sql, pms);

                if (result >= 1)
                {
                    TempData["Result"] = "資料刪除成功!";
                }

                return RedirectToAction("Index");
            }

            return View(data);
        }
    }
}