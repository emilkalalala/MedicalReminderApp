//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MEDICAL_APP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Prescriptions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prescriptions()
        {
            this.IntakeTime = new HashSet<IntakeTime>();
        }
    
        public int UserId { get; set; }
        public int PrescriptionsId { get; set; }
        public string MedicalName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IntakeTime> IntakeTime { get; set; }
        public virtual Users Users { get; set; }
    }
}
