using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRP.Domain.ViewModel
{
    public class CustomProductFeeModel
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 产品费用
        /// </summary>
        public decimal ProductFee { get; set; }
    }
}
