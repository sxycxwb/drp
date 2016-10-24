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
    public class CustomerBankApp
    {
        private ICustomerBankRepository service = new CustomerBankRepository();

        public void SubmitForm(CustomerBankEntity customerBankEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                customerBankEntity.Modify(keyValue);
            }
            else
            {
                customerBankEntity.Create();
            }
            service.SubmitForm(customerBankEntity, keyValue);
        }

        public CustomerBankEntity SeleceForm(string keyValue)
        {
            return service.FindEntity(t => t.F_BankAccountName == keyValue);
        }

        public CustomerBankEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public List<CustomerBankEntity> GetList(Pagination pagination, string keyword, string customerId)
        {
            var expression = ExtLinq.True<CustomerBankEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_BankAccountName.Contains(keyword));
                expression = expression.Or(t => t.F_BankCardNo.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark == false);
            expression = expression.And(t => t.F_CustomerId == customerId);
            return service.FindList(expression, pagination);
        }
    }
}
