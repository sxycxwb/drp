using DRP.Code;
using DRP.Data;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.IRepository.DrpServManage;
using DRP.Domain.ViewModel;
using DRP.Repository.DrpServManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRP.Application.DrpServManage
{
    public class CustomerProductApp
    {
        private ICustomerProductRepository service = new CustomerProductRepository();
        /// <summary>
        /// 获取当前用户的产品
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<CustomProductModel> GetProductJson(string keyValue)
        {
            //查询客户下已设定的产品
            var dbRepository = new RepositoryBase();
            var customerProductList =
                dbRepository.FindList<CustomProductModel>(
                    $@"select a.F_Id,F_CustomerId,F_ProductId,a.F_RoyaltyRate,F_Status,F_ChargingDateFlag,F_ProductName,F_ChargePattern,
                                F_CostPrice,F_ChargeAmount,F_ChargeStyle,F_Description,F_Remark
                                 from drp_customerproduct a left join drp_product b on a.F_ProductId=b.F_Id 
                                where a.F_CustomerId='{keyValue}';");

            return customerProductList;
        }

        public CustomerProductEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="customerProductEntity"></param>
        public void UpdateForm(CustomerProductEntity customerProductEntity)
        {
            service.Update(customerProductEntity);
        }
    }
}
