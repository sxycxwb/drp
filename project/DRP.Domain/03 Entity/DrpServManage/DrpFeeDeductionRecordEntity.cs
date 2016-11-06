using DRP.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRP.Domain.Entity.DrpServManage
{
    /// <summary>
    /// 充值记录信息
    /// </summary>
    public class FeeDeductionRecordEntity : CreationAuditedEntity<string>
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string F_CustomerId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public string F_ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string F_ProductName { get; set; }

        /// <summary>
        /// 扣费金额
        /// </summary>
        public decimal F_DeductionFee { get; set; }
    }
}
