using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace MEDICAL_APP.Models
{
    [Serializable]
    public class MultiAddMedical
    {
        public List<Medical> Medicals { get; set; }

        public MultiAddMedical()
        {
            Medicals = new List<Medical>();
        }
    }
}