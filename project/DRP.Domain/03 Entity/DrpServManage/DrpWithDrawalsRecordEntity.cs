using DRP.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRP.Domain.Entity.DrpServManage
{
    /// <summary>
    /// 提现记录信息
    /// </summary>
    public class WithDrawalsRecordEntity : CreationAuditedEntity<string>
    {
        /// <summary>
        /// 提现人ID
        /// </summary>
        public string F_WithdrawPersonId { get; set; }

        /// <summary>
        /// 提现人
        /// </summary>
        public string F_WithdrawPersonName { get; set; }

        /// <summary>
        /// 提现说明
        /// </summary>
        public string F_WithdrawRemark { get; set; }

        /// <summary>
        /// 审核人ID
        /// </summary>
        public string F_CheckPersonId { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string F_CheckPersonName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime F_CheckTime { get; set; }

        /// <summary>
        /// 审核备注
        /// </summary>
        public string F_CheckRemark { get; set; }

        /// <summary>
        /// 驳回说明
        /// </summary>
        public string F_TurndownRemark { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal F_WithdrawAmount { get; set; }

        /// <summary>
        /// 状态 0为新申请，1为审核通过，2为驳回
        /// </summary>
        public int F_Status { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string F_Type { get; set; }
    }
}
