using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRP.Data;
using DRP.Domain.IRepository.DrpServManage;
using DRP.Repository.DrpServManage;
using DRP.Repository.SystemManage;

namespace DRP.Application.DrpServManage
{
    public class ScheduleTaskApp
    {
        //1.使用系数 = 当前月能使用的天数/本月天数
        //2.产品提成系数 每种产品都有给代理商设定的提成系数
        //3.客户产品提成系数 默认1 方便以后控制代理商提成
        //代理商提成 =（销售价-成本价） * 使用系数 * 产品提成系数 * 客户产品提成系数


        // 客户产品关系 中添加 提成比率系数
        // 扣费：每月月初扣费，如果使用不到一个月，当前月使用天数/本月天数，超过一个月按一个月计算 使用系数 = 当前月使用天数/当前月天数
        // 代理商提成费用计算 = （销售价-成本价） * 使用系数 * 代理提成率 * 提成比率系数
        // 系统收益计算 = （销售价-成本价） * 使用系数 - 代理商提成费用

        private ICustomerRepository customerService = new CustomerRepository();
        private IProductRepository productService = new ProductRepository();
        private ICustomerProductRepository customerProductService = new CustomerProductRepository();

        public void ProfitCalculate(string customerId = "")
        {
            //获取所有有效客户信息
            var customerList = customerService.IQueryable(t => t.F_DeleteMark == false).ToList();
            //获取所有有效商品
            var productList = productService.IQueryable(t => t.F_DeleteMark == false).ToList();
            var customerProductList = customerProductService.IQueryable().ToList();

            //循环为每位客户进行扣费，计算收益
            foreach (var customer in customerList)
            {
                //使用系数默认为1
                decimal useCoefficient = 1;
                //查询该客户绑定的产品
                var cusProductList = customerProductList.Where(t => t.F_CustomerId == customer.F_Id);
                //循环客户产品

                decimal totalProductProfit = 0;
                foreach (var cusProduct in cusProductList)
                {
                    //客户产品提成系数
                    var cusProRoyalRate = cusProduct.F_RoyaltyRate;
                    //产品
                    var product = productList.FirstOrDefault(t => t.F_Id == cusProduct.F_ProductId);
                    //产品提成系数
                    var productRoyalRate = product.F_RoyaltyRate;
                    //销售价
                    var chargeAmount = product.F_ChargeAmount;
                    //成本价
                    var costPrice = product.F_CostPrice;
                }

            }

            using (var db = new RepositoryBase().BeginTrans())
            {
            }
        }



    }
}
