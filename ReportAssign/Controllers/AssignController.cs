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

            if (accession_num != null)
            {

                List<PatientList> patlist = new List<PatientList>();

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

                            patlist.Add(new PatientList()
                            {
                                PatientID = Convert.ToString(Reader[0]),
                                PatientName = Convert.ToString(Reader[1]),
                                AccessionNum = Convert.ToString(Reader[2]),
                                DoctorID = Convert.ToString(Reader[3]),
                                DoctorName = Convert.ToString(Reader[4])
                            });

                            if (patlist.Count >= 1)
                            {
                                ViewBag.CheckData = "ok";
                            }

                        }
                    }


                }

                return View("Index", patlist);

            }
            else
            {
                return View("Index");
            }

        }
    }
}