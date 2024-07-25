using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ProductInfoResponse
    {
        public int TotalProductCount { get; set; }
        public int AveragePrice { get; set; }
        public int LowestPrice { get; set; }
        public int HighestPrice { get; set; }
        public int TotalAssignedProductsCount { get; set; }
    }
}
