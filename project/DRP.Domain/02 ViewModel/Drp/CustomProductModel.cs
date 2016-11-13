using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRP.Domain.ViewModel
{
    public class CustomProductModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string F_Id { get; set; }
        /// <summary>
        /// 账户编号
        /// </summary>
        public string F_CustomerId { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public string F_ProductId { get; set; }
        /// <summary>
        /// 提成比
        /// </summary>
        public decimal F_RoyaltyRate { get; set; }
        /// <summary>
        /// 状态 1开通，0欠费，2停用
        /// </summary>
        public int F_Status { get; set; }
        /// <summary>
        /// 计费日期标识
        /// </summary>
        public string F_ChargingDateFlag { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string F_ProductName { get; set; }

        /// <summary>
        /// 计费模式 (0为产品 1为模块)
        /// </summary>
        public string F_ChargePattern { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal F_CostPrice { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        public decimal F_ChargeAmount { get; set; }

        /// <summary>
        /// 计费方式 -年、季、月、周、日
        /// </summary>
        public string F_ChargeStyle { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        public string F_Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string F_Remark { get; set; }
    }
}
