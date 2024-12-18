using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UnivercityDB.Model
{
    internal class PreviousEmployment
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [Display(Name = "Название предприятия")]
        public string CompanyName { get; set; } = "OOO Неизвестно";
        [Display(Name = "Должность на предприятии")]
        public string Position { get; set; }
        [Display(Name = "Начало работы")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Окончание работы")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Причина окончания работы")]
        public string ReasonForLeaving { get; set; }
        [Display(Name = "Номер телефона предприятия")]
        public string CompanyPhone { get; set; }
        [Display(Name = "Улица")]
        public string Street { get; set; }
        [Display(Name = "Город")]
        public string City { get; set; }
        [Display(Name = "Номер дома")]
        public string HouseNumber { get; set; }
    }
}
