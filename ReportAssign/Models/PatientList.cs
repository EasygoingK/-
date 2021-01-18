using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportAssign.Models
{
    public class PatientList
    {
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string AccessionNum { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
    }
}