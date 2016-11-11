using DRP.Code;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.IRepository.DrpServManage;
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
        public List<CustomerProductEntity> GetProductJson(string keyValue)
        {
            //查询客户下已设定的产品
            var expressionCusPro = ExtLinq.True<CustomerProductEntity>();
            expressionCusPro = expressionCusPro.And(t => t.F_CustomerId == keyValue);
            var customerProductList = service.IQueryable(expressionCusPro).ToList();
            return customerProductList;
        }
    }
}
