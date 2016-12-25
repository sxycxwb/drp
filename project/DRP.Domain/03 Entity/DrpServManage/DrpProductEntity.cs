using DRP.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRP.Domain.Entity.DrpServManage
{
    /// <summary>
    /// 产品信息
    /// </summary>
    public class ProductEntity : FullAuditedEntity<string>
    {
        /// <summary>
        /// 产品分类ID
        /// </summary>
        public string F_CategoryId { get; set; }
        
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
        /// 销售最低价
        /// </summary>
        public decimal? F_ChargeAmountMin { get; set; }

        /// <summary>
        /// 销售最高价
        /// </summary>
        public decimal? F_ChargeAmountMax { get; set; }

        /// <summary>
        /// 代理商提成率
        /// </summary>
        public decimal F_RoyaltyRate { get; set; }

        /// <summary>
        /// 计费方式 -年、季、月、周、日
        /// </summary>
        public string F_ChargeStyle { get; set; }

        /// <summary>
        /// 计费规则
        /// </summary>
        public string F_ChargeRule { get; set; }

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
