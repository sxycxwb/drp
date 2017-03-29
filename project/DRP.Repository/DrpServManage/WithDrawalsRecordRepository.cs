/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Data;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.Entity.SystemManage;
using DRP.Domain.IRepository.DrpServManage;

namespace DRP.Repository.SystemManage
{
    public class WithDrawalsRecordRepository : RepositoryBase<WithDrawalsRecordEntity>, IDrpWithDrawalsRecordRepository
    {
        public int UpdateWithDrawals(WithDrawalsRecordEntity entity)
        {
            var status = entity.F_Status;
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (status == 2)//如果为审核通过状态
                {
                    var userEntity = db.FindEntity<UserEntity>(entity.F_WithdrawPersonId);
                    userEntity.F_AccountBalance -= entity.F_WithdrawAmount;
                    db.Update(userEntity);
                }

                db.Update(entity);
                db.Commit();
            }
            
            return 0;
        }
    }
}
