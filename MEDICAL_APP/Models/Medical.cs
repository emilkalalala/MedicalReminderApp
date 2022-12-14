using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MEDICAL_APP.Models
{
    
    public class Medical
    {
        public Medical()
        {
            Monday = new List<CheckBoxListItem>();
            Tuesday = new List<CheckBoxListItem>();
            Wednesday = new List<CheckBoxListItem>();
            Thuersday = new List<CheckBoxListItem>();
            Friday = new List<CheckBoxListItem>();
            Saturday = new List<CheckBoxListItem>();
            Sunday = new List<CheckBoxListItem>();
        }
        [Display(Name = "MedicalName")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "required field")]
        public string MedicalName { get; set; }
        public List<string> MedicalNameList { get;set; }
        public List<CheckBoxListItem> Monday { get; set; }
        public List<CheckBoxListItem> Tuesday { get; set; }
        public List<CheckBoxListItem> Wednesday { get; set; }
        public List<CheckBoxListItem> Thuersday { get; set; }
        public List<CheckBoxListItem> Friday { get; set; }
        public List<CheckBoxListItem> Saturday { get; set; }
        public List<CheckBoxListItem> Sunday { get; set; }


    }
    public class CheckBoxListItem
    {
        public long Id { get; set; }
        public string Hour{ get; set; }
        public bool IsSelected { get; set; }
    }
}