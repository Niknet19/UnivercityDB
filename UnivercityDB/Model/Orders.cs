using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UnivercityDB.Model
{
    public class SanctionsOrder: Order
    {
        [Display(Name = "ФИО Сотрудника")]
        public string EmployeeName { get; set; }

        [Display(Name = "Тип")]
        public string SanctionType { get; set; }

        /*public SanctionsOrder(int id, DateTime date, string orderNumber, string employeeName, string sanctionType)
            : base(id, date, orderNumber)
        {
            EmployeeName = employeeName;
            SanctionType = sanctionType;
        }*/
    }



    public class MovementOrder : Order
    {
        [Display(Name = "ФИО Сотрудника")]
        public string EmployeeName { get; set; }

        [Display(Name = "Причина перемещения")]
        public string Reason { get; set; }
    }
}
