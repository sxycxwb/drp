using DRP.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRP.Domain.Entity.DrpServManage
{
    /// <summary>
    /// 产品模块信息
    /// </summary>
    public class ProductModuleEntity : FullAuditedEntity<string>
    {
        /// <summary>
        /// 父级模块ID
        /// </summary>
        public string F_ParentId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public string F_ProductId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string F_ModuleName { get; set; }

        /// <summary>
        /// 模块计费金额
        /// </summary>
        public decimal F_ChargeAmount { get; set; }

        /// <summary>
        /// 模块介绍
        /// </summary>
        public string F_Description { get; set; }

    }
}
