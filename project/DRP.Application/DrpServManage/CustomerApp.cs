/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Code;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.Entity.SystemManage;
using DRP.Domain.IRepository.DrpServManage;
using DRP.Domain.IRepository.SystemManage;
using DRP.Repository.DrpServManage;
using DRP.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using DRP.Domain;

namespace DRP.Application.DrpServManage
{
    public class CustomerApp
    {
        private ICustomerRepository service = new CustomerRepository();
        private IProductRepository productService = new ProductRepository();
        private ICustomerProductRepository customerProductService = new CustomerProductRepository();

        public List<CustomerEntity> GetList(string keyword)
        {
            var expression = ExtLinq.True<CustomerEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_CompanyName.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark == false);
            return service.IQueryable(expression).ToList();
        }

        public List<CustomerEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<CustomerEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Account.Contains(keyword));
                expression = expression.Or(t => t.F_AccountCode.Contains(keyword));
                expression = expression.Or(t => t.F_MobilePhone.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark == false);
            if (!OperatorProvider.Provider.GetCurrent().IsSystem) //不是超级管理员
            {
                var currentUserId = OperatorProvider.Provider.GetCurrent().UserId;
                expression = expression.And(t => t.F_CreatorUserId == currentUserId || t.F_BelongPersonId == currentUserId);
            }

            return service.FindList(expression, pagination);
        }
        public CustomerEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public object GetProductJson(string keyValue)
        {
            //查询所有有效产品
            var expression = ExtLinq.True<ProductEntity>();
            expression = expression.And(t => t.F_DeleteMark == false);
            var productList = productService.IQueryable(expression).OrderByDescending(t => t.F_CreatorTime);

            //查询客户下已设定的产品
            var expressionCusPro = ExtLinq.True<CustomerProductEntity>();
            expressionCusPro = expressionCusPro.And(t => t.F_CustomerId == keyValue);
            var customerProductList = customerProductService.IQueryable(expressionCusPro).ToList();

            List<object> productSelectList = new List<object>();
            foreach (var customerProduct in customerProductList)
            {
                var customerProductId = customerProduct.F_ProductId;
                var product = productList.ToList().Find(t => t.F_Id == customerProductId);

                var productId = product.F_Id;

                productSelectList.Add(new
                {
                    fid = customerProduct.F_Id,
                    productId = productId,
                    productPrice = customerProduct.F_ChargeAmount,
                    productName = product.F_ProductName,
                    productDes = product.F_Description
                });
            }
            return productSelectList;
        }

        public void SubmitForm(CustomerEntity customerEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                customerEntity.Modify(keyValue);
            }
            else
            {
                customerEntity.Create();
                customerEntity.F_AccountBalance = 0;//账户余额初始化
            }
            service.SubmitForm(customerEntity, keyValue);
        }

        public void SubmitProduct(ProductEntity productEntity, string keyValue, string customerId, string flag)
        {
            //判断是否添加
            var cusProModel = customerProductService.FindEntity(t => t.F_ProductId == keyValue && t.F_CustomerId == customerId);
            if (cusProModel != null && flag == "add")
                throw new Exception("该产品已添加！");

            //判断销售价是否在产品定义的 销售价范围内
            var chargeAmount = productEntity.F_ChargeAmount;
            var product = productService.FindEntity(t => t.F_Id == keyValue);
            if (chargeAmount < product.F_ChargeAmountMin || chargeAmount > product.F_ChargeAmountMax)
                throw new Exception("销售价不在有效范围内，请重新填写！");

            var customerProductEntity = new CustomerProductEntity();
            customerProductEntity.Create();
            customerProductEntity.F_CustomerId = customerId;
            customerProductEntity.F_ChargeAmount = chargeAmount;//销售价
            customerProductEntity.F_ProductId = keyValue;
            customerProductEntity.F_RoyaltyRate = 100;//默认提成比率为100
            service.SubmitProduct(customerProductEntity, customerId);
        }

        public void RemoveProduct(string keyValue)
        {
            customerProductService.Delete(t => t.F_Id == keyValue);
        }


        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void UpdateForm(CustomerEntity customerEntity)
        {
            service.Update(customerEntity);
        }

        public CustomerEntity CheckLogin(string username, string password)
        {
            CustomerEntity customerEntity = service.FindEntity(t => t.F_Account == username);
            if (customerEntity != null)
            {
                if (customerEntity.F_EnabledMark == true)
                {
                    //UserLogOnEntity userLogOnEntity = userLogOnApp.GetForm(userEntity.F_Id);
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(password.ToLower(), ConstantUtility.CUSTOMER_MD5_SECRETKEY).ToLower(), 32).ToLower();
                    if (dbPassword == customerEntity.F_Password)
                    {
                        return customerEntity;
                    }
                    else
                    {
                        throw new Exception("密码不正确，请重新输入");
                    }
                }
                else
                {
                    throw new Exception("账户被系统锁定,请联系管理员");
                }
            }
            else
            {
                throw new Exception("账户不存在，请重新输入");
            }
        }
    }
}
