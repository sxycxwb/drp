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
    public class RechargeRecordEntity : AuditedEntity<string>
    {
        /// <summary>
        /// 银行账户名
        /// </summary>
        public string F_BankAccountName { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string F_BankCardNo { get; set; }

        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal F_RechargeAccount { get; set; }

        /// <summary>
        /// 到账时间
        /// </summary>
        public DateTime F_PaymentTime { get; set; }

        /// <summary>
        /// 状态 0-手工录账 1-核对通过，本次充值完毕 2-核对未通过 3-撤销
        /// </summary>
        public int F_Status { get; set; }

        /// <summary>
        /// 比对时间
        /// </summary>
        public DateTime F_CheckTime { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string F_Description { get; set; }
    }
}
