using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Exceptions
{
    public class CVVMismatchException: Exception
    {
        public string ReferenceCvv { get; private set; }
        public string ComparedCvv { get; private set; }
        public CVVMismatchException(string referenceCVV, string comparedCVV) : base($"Cvv values do not match. Referece: ***, compared: {comparedCVV}")
        {
            ReferenceCvv = referenceCVV;
            ComparedCvv = comparedCVV;
        }
    }
}
