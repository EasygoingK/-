using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReportAssign.Models
{
    public class RIS_WORKLIST
    {
        [Display(Name ="病歷號")]
        public string PATIENT_ID { get; set; }
        [Display(Name ="姓名")]
        public string PATIENT_NAME { get; set; }
        [Display(Name ="檢查流水號")]
        public string ACCESSION_NUM { get; set; }
        [Display(Name ="醫師帳號")]
        public string ASSIGN_PHYSICIAN { get; set; }
        [Display(Name ="醫師姓名")]
        public string USER_REALNAME { get; set; }
    }
}