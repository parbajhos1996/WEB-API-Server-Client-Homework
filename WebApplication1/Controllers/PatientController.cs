using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class PatientController : ApiController
    {
        private static List<patient> PatientList = new patientEntities().patients.ToList();

        public IList<patient> Get()
        {
            return PatientList;
        }

        public patient Get(int id)
        {
            if (id >= 0 && id < PatientList.Count)
            {
                return PatientList[id];
            }

            return null;
        }

        public void Post([FromBody]patient patient)
        {
            if (PatientList.Contains(patient))
            {
                PatientList.Remove(patient);
            }
            else if (patient != null)
            {
                PatientList.Add(patient);
            }
        }



    }
}