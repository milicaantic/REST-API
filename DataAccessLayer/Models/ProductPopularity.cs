using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ProductPopularity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalAssignments { get; set; }
        public string CreatorName { get; set; }
    }
}
