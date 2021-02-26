using Dapper;
using ReportAssign.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private static readonly string constr = ConfigurationManager.ConnectionStrings["ReportDB"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        //查詢資料
        [HttpPost]
        public ActionResult QueryReport(string accession_num)
        {
            List<RIS_WORKLIST> patList = new List<RIS_WORKLIST>();

            if (accession_num != null)
            {
                using (IDbConnection db = new SqlConnection(constr))
                {
                    string sql = "select r.patient_id, r.patient_name, r.accession_num, r.assign_physician, u.user_realname " +
                                 "from ris_worklist r, systemuser u " +
                                 "where r.assign_physician = u.user_name and accession_num = @ACCESSION_NUM";

                    patList = db.Query<RIS_WORKLIST>(sql, new { ACCESSION_NUM = accession_num }).ToList();

                    if (patList.Count == 1)
                    {
                        ViewBag.checkdata = "ok";
                    }
                    else
                    {
                        TempData["Result"] = "查無此報告，請重新輸入檢查流水號!";
                    }

                    ViewBag.Test = new SelectList(patList, "ACCESSION_NUM", "PATIENT_NAME");

                    return View("Index", patList);
                }
            }
            else
            {
                return View("Index");
            }

        }

        //下拉選單取得醫師資料
        [HttpPost]
        public ActionResult GetDocList()
        {
            List<SYSTEMUSER> docList = new List<SYSTEMUSER>();

            using (IDbConnection db = new SqlConnection(constr))
            {
                string sql = "select user_name, user_realname from systemuser";

                docList = db.Query<SYSTEMUSER>(sql).ToList();
            }
                   
            return Json(docList);
        }

        //更新醫師資料
        [HttpPost]
        public ActionResult UpdateDoc(string doclist, RIS_WORKLIST item)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(doclist))
                {
                    var user_name = doclist.Trim().Split('-');

                    using (IDbConnection db = new SqlConnection(constr))
                    {

                        string docCheck = "select user_name, user_realname from systemuser where user_name = @user_name";

                        var result = db.Query(docCheck,new { user_name = user_name[0] }).ToList();

                        if (result.Count == 1)
                        {
                            var sql = "update ris_worklist set assign_physician = @ASSIGN_PHYSICIAN " +
                                      "where accession_num = @ACCESSION_NUM";

                            var data = db.Execute(sql, new { assign_physician = user_name[0], accession_num = item.ACCESSION_NUM });

                            if (data == 1)
                            {
                                TempData["Result"] = "報告分派成功!";
                            }
                        }
                        else
                        {
                            TempData["Result"] = "查無此醫師帳號，請重新輸入!";

                        }
                    }
                }
                else
                {
                    TempData["Result"] = "未指定分派醫師，請重新輸入!";
                }

                return RedirectToAction("Index");
            }

            return View(item);

        }
    }
}