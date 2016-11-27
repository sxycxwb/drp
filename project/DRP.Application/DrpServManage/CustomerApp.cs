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

            List<object> productCheckList = new List<object>();
            foreach (var product in productList)
            {
                var productId = product.F_Id;
                var isChecked = false;
                var checkProduct = customerProductList.Find(t => t.F_ProductId == productId && t.F_CustomerId == keyValue);
                if (checkProduct != null)
                    isChecked = true;
                productCheckList.Add(new
                {
                    productId = productId,
                    productName = product.F_ProductName,
                    productDes = product.F_Description,
                    isChecked = isChecked
                });
            }
            return productCheckList;
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

        public void SubmitProduct(string[] productIds, string keyValue)
        {
            var customerProductList = new List<CustomerProductEntity>();
            foreach (var itemId in productIds)
            {
                var customerProductEntity = new CustomerProductEntity();
                customerProductEntity.Create();
                customerProductEntity.F_CustomerId = keyValue;
                customerProductEntity.F_ProductId = itemId;
                customerProductEntity.F_RoyaltyRate = 100;//默认提成比率为100
                customerProductList.Add(customerProductEntity);
            }
            service.SubmitProduct(customerProductList, keyValue);
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
