using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DRP.Code;
using DRP.Data;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.IRepository.DrpServManage;
using DRP.Domain.IRepository.SystemManage;
using DRP.Domain.ViewModel;
using DRP.Repository.DrpServManage;
using DRP.Repository.SystemManage;

namespace DRP.Application.DrpServManage
{
    public class ScheduleTaskApp
    {
        #region 收益计算任务
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
        private IUserRepository agentService = new UserRepository();

        private IRechargeRecordRepository rechargeService = new RechargeRecordRepository();
        private ICustomerBankRepository cusBankService = new CustomerBankRepository();

        public void ProfitCalculateTask(string customerId = "")
        {
            //获取所有有效客户信息
            var customerList = customerService.IQueryable(t => t.F_DeleteMark == false).ToList();
            //获取所有有效商品
            var productList = productService.IQueryable(t => t.F_DeleteMark == false).ToList();
            var customerProductList = customerProductService.IQueryable().ToList();

            #region 获取客户代缴费总和

            var dbRepository = new RepositoryBase();
            var productFeeList =
                dbRepository.FindList<CustomProductFeeModel>(
                    @"SELECT A.F_ID CUSTOMERID,SUM(C.F_CHARGEAMOUNT) PRODUCTFEE 
            FROM DRP_CUSTOMER A,DRP_CUSTOMERPRODUCT B,DRP_PRODUCT C
            WHERE A.F_ID = B.F_CUSTOMERID AND B.F_PRODUCTID = C.F_ID
            AND A.F_DELETEMARK = 0 AND C.F_CHARGESTYLE='MONTH' AND B.F_STATUS = 1
            GROUP BY A.F_ID,C.F_CHARGEAMOUNT");

            #endregion

            //循环为每位客户进行扣费，计算收益
            foreach (var customer in customerList)
            {
                //使用系数默认为1
                decimal useCoefficient = 1;

                #region 客户账户余额与待扣费产品销售额总价比较 如果余额不足，则将产品状态至为欠费 否则执行扣费操作

                //1.客户账户余额
                decimal? balance = customer.F_AccountBalance;
                //2.客户产品代缴费总额
                decimal totalFee = 0;
                var productFee = productFeeList.FirstOrDefault(t => t.CustomerId == customer.F_Id);
                if (productFee != null)
                    totalFee = productFee.ProductFee;

                //查询该客户绑定的产品
                var cusProductList = customerProductList.Where(t => t.F_CustomerId == customer.F_Id);
                //查客户对应代理人信息
                var agent = agentService.FindEntity(t => t.F_Id == customer.F_BelongPersonId);

                #region 账户余额不足，则将产品状态至为欠费

                if (balance < totalFee)
                {
                    using (var db = new RepositoryBase().BeginTrans())
                    {
                        foreach (var cusProduct in cusProductList)
                        {
                            //欠费
                            cusProduct.F_Status = 0;
                            db.Update(cusProduct);
                        }
                        db.Commit();
                    }
                }

                #endregion

                #region 账户余额充足，执行扣款操作；并且同时计算系统收益和代理商提成

                else
                {
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

                        //每个产品的代理人提成 TODO 目前只有一级代理人获取收益，后期会加入多级
                        var toyal = (chargeAmount - costPrice) * productRoyalRate * cusProRoyalRate * useCoefficient;
                        //每个产品的系统收益
                        var systemToyal = (chargeAmount - costPrice) - toyal;

                        #region 业务实体赋值，更新数据库

                        #region 1.更新代理人余额

                        agent.F_AccountBalance += toyal;

                        #endregion

                        #region 2.新增记录中间环节收益和系统收益 的记录

                        var comisssionRecord = new ComissionRecordEntity()
                        {
                            F_Id = Common.GuId(),
                            F_ProductId = product.F_Id,
                            F_CommissionAmount = toyal,
                            F_CommissionPersonId = customer.F_BelongPersonId,
                            F_CustomerId = customer.F_Id,
                            F_CreatorTime= DateTime.Now,
                            F_Type = ""
                        };
                        //系统收益记录信息
                        var sysComisssionRecord = comisssionRecord;
                        sysComisssionRecord.F_Id = Common.GuId();
                        sysComisssionRecord.F_CreatorTime = DateTime.Now;
                        sysComisssionRecord.F_CommissionAmount = systemToyal;
                        sysComisssionRecord.F_CommissionPersonId = "";

                        #endregion

                        #region 3.更新客户账户余额，减去当前产品的销售价格;增加扣费记录
                        customer.F_AccountBalance -= chargeAmount;

                        var deduction = new FeeDeductionRecordEntity();
                        #endregion

                        #region 4.执行数据库操作
                        using (var db = new RepositoryBase().BeginTrans())
                        {
                            db.Update(agent); //更新代理人账户余额
                            db.Update(customer); //更新客户账户余额
                            db.Insert(comisssionRecord); //新增代理人收益记录
                            db.Insert(sysComisssionRecord); //新增系统收益记录
                            db.Commit();
                        }
                        #endregion

                        #endregion
                    }

                    #endregion

                }
                #endregion
            }
        }
        #endregion

        #region 充值任务
        /// <summary>
        /// 充值任务
        /// </summary>
        /// <param name="bankAccountName">银行账户名</param>
        public void RechargeTask(string bankAccountName="")
        {
            //查询所有充值记录中 状态 为 0-手工录账 2-核对未通过
            Expression<Func<RechargeRecordEntity, bool>> exp = t => t.F_Status == 0 || t.F_Status == 2;//状态条件
            if (!string.IsNullOrEmpty(bankAccountName))
            {
                exp.And(t => t.F_BankAccountName == bankAccountName);//拼接银行账户名的查询条件
            }
            var rechargeList = rechargeService.IQueryable(exp).ToList();

            //查询所有银行卡相关信息
            Expression<Func<CustomerBankEntity, bool>> exp2 = null;
            if (!string.IsNullOrEmpty(bankAccountName))
            {
                exp2 =t => t.F_BankAccountName == bankAccountName;//拼接银行账户名的查询条件
            }
            var cusBankList = cusBankService.IQueryable(exp2).ToList();

            foreach (var recharge in rechargeList)
            {
                var accountBankName = recharge.F_BankAccountName;
                var cusBank = cusBankList.FirstOrDefault(t => t.F_BankAccountName == accountBankName);
                //如果比对银行账户名失败，则将充值记录状态0改为2, 原来是2则保持不变
                if (cusBank == null)
                {
                    if (recharge.F_Status == 0)
                    {
                        recharge.F_Status = 2;
                        //更新数据库
                    }
                }
                //如果比对成功，将状态0或2改为1，为客户充值
                else
                {
                    recharge.F_Status = 1;
                    //更新数据库
                }
            }
        }

        #endregion
    }
}
