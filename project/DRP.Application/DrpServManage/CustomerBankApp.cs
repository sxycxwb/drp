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
            return service.SeleceForm(keyValue);
        }
    }
}
