/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Data;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.IRepository.DrpServManage;

namespace DRP.Repository.SystemManage
{
    public class WithDrawalsRecordRepository : RepositoryBase<WithDrawalsRecordEntity>, IDrpWithDrawalsRecordRepository
    {
        public void WithdrawSubmitForm(WithDrawalsRecordEntity withDrawalsRecord)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Insert(withDrawalsRecord);
                db.Commit();
            }
        }
    }
}
