//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1
{
    using System;
    using System.Collections.Generic;
    
    public partial class patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string TajNumber { get; set; }
        public string Complaint { get; set; }

        public override bool Equals(object obj)
        {
            var otherPatient = obj as patient;
            return otherPatient != null &&
                   FirstName == otherPatient.FirstName &&
                   LastName == otherPatient.LastName &&
                   Address == otherPatient.Address &&
                   TajNumber == otherPatient.TajNumber &&
                   Complaint == otherPatient.Complaint;
        }
    }
}