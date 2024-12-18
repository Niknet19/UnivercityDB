using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UnivercityDB.Model
{
    public class Order
    {
        public int Id { get; set; }
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
        [Display(Name = "Номер приказа")]
        public string OrderNumber { get; set; } = string.Empty;

        /*protected Order(int id, DateTime date, string orderNumber)
        {
            Id = id;
            Date = date;
            OrderNumber = orderNumber;
        }*/

    }
}
