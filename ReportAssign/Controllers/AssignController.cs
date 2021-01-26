using ReportAssign.Models;
using System;
using System.Collections.Generic;
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

        [HttpPost]
        public ActionResult QueryReport(string accession_num)
        {
           //viewModel
           //Model Entity
            if (accession_num != null)
            {

                List<PatientList> patlist = new List<PatientList>();

                string sql = "SELECT PatientID, PatientName, AccessionNum, DoctorID, DoctorName " +
                        "FROM patientlist Where AccessionNum = @AccessionNum";
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ReportDB"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@AccessionNum", accession_num);
                    //cmd.Parameters.Add(new SqlParameter())
                    using (SqlDataReader Reader = cmd.ExecuteReader())
                    {

                        if (Reader.HasRows)
                        {
                            while (Reader.Read())
                            {
                                patlist.Add(new PatientList()
                                {
                                    PatientID = Convert.ToString(Reader[0]),
                                    PatientName = Convert.ToString(Reader[1]),
                                    AccessionNum = Convert.ToString(Reader[2]),
                                    DoctorID = Convert.ToString(Reader[3]),
                                    DoctorName = Convert.ToString(Reader[4])
                                });
                            }
                        }
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PatientList data)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ReportDB"].ConnectionString))
                {
                    conn.Open();

                    string sql = "Insert into patientlist (PatientID, PatientName, AccessionNum) Values (@PatientID, @PatientName, @AccessionNum)";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@PatientID", data.PatientID);
                    cmd.Parameters.AddWithValue("@PatientName", data.PatientName);
                    cmd.Parameters.AddWithValue("@AccessionNum", data.AccessionNum);

                    var result = cmd.ExecuteNonQuery();

                    if (result >= 1)
                    {
                        TempData["Result"] = "資料新增成功!";
                    }

                }

                return RedirectToAction("Index");
            }

            return View(data);
        }

        public ActionResult Edit(string accession_num)
        {
            var data = new PatientList();

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ReportDB"].ConnectionString))
            {
                conn.Open();

                string sql = "SELECT PatientID, PatientName, AccessionNum, DoctorID, DoctorName " +
                    "FROM patientlist Where AccessionNum = @AccessionNum";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@AccessionNum", accession_num);

                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    if (Reader.Read())
                    {
                        data.PatientID = Convert.ToString(Reader[0]);
                        data.PatientName = Convert.ToString(Reader[1]);
                        data.AccessionNum = Convert.ToString(Reader[2]);
                       
                    };
                }
            }

            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(PatientList data)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ReportDB"].ConnectionString))
                {
                    conn.Open();

                    
                    //"update patientlist  set PatientID = @PatientID, PatientName = @PatientName where AccessionNum = "+ AccessionNum + "
                    string sql = "update patientlist  set PatientID = @PatientID, PatientName = @PatientName where AccessionNum = @AccessionNum";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@PatientID", data.PatientID);
                    cmd.Parameters.AddWithValue("@PatientName", data.PatientName);
                    cmd.Parameters.AddWithValue("@AccessionNum", data.AccessionNum);

                    var result = cmd.ExecuteNonQuery();

                    if (result >= 1)
                    {
                        TempData["Result"] = "資料更新成功!";
                    }

                }

                return RedirectToAction("Index");
            }

            return View(data);
        }

        public ActionResult Delete(string accession_num)
        {
            var data = new PatientList();

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ReportDB"].ConnectionString))
            {
                conn.Open();

                string sql = "SELECT PatientID, PatientName, AccessionNum, DoctorID, DoctorName " +
                    "FROM patientlist Where AccessionNum = @AccessionNum";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@AccessionNum", accession_num);

                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    if (Reader.Read())
                    {
                        data.PatientID = Convert.ToString(Reader[0]);
                        data.PatientName = Convert.ToString(Reader[1]);
                        data.AccessionNum = Convert.ToString(Reader[2]);
                    };
                }
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PatientList data)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ReportDB"].ConnectionString))
                {
                    conn.Open();

                    string sql = "delete patientlist where AccessionNum = @AccessionNum";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@AccessionNum", data.AccessionNum);

                    var result = cmd.ExecuteNonQuery();

                    if (result >= 1)
                    {
                        TempData["Result"] = "資料刪除成功!";
                    }

                }

                return RedirectToAction("Index");
            }

            return View(data);
        }
    }
}