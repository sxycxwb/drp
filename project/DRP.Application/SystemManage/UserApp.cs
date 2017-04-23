/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Code;
using DRP.Domain;
using DRP.Domain.Entity.SystemManage;
using DRP.Domain.IRepository.SystemManage;
using DRP.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DRP.Application.SystemManage
{
    public class UserApp
    {
        private IUserRepository service = new UserRepository();
        private IRoleRepository roleService = new RoleRepository();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();

        public List<UserEntity> GetList()
        {
            var expression = ExtLinq.True<UserEntity>();
            expression = expression.And(t => t.F_DeleteMark == false);
            expression = expression.And(t => t.F_Account != ConstantUtility.ADMIN_CODE);
            return service.IQueryable(expression).ToList();
        }

        public List<UserEntity> GetBelongPersonList()
        {
            var role = roleService.FindEntity(t => t.F_EnCode == "agent");

            var expression = ExtLinq.True<UserEntity>();
            expression = expression.And(t => t.F_DeleteMark == false && t.F_RoleId == role.F_Id);
            expression = expression.And(t => t.F_Account != ConstantUtility.ADMIN_CODE);
            return service.IQueryable(expression).ToList();
        }

        public List<UserEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<UserEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Account.Contains(keyword));
                expression = expression.Or(t => t.F_RealName.Contains(keyword));
                expression = expression.Or(t => t.F_MobilePhone.Contains(keyword));
            }
            expression = expression.And(t => t.F_Account != "admin");
            return service.FindList(expression, pagination);
        }
        public UserEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public UserEntity GetFormByUserCode(string keyValue)
        {
            return service.FindEntity(t=>t.F_Account==keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                userEntity.Modify(keyValue);
            }
            else
            {
                userEntity.Create();
            }
            service.SubmitForm(userEntity, userLogOnEntity, keyValue);
        }
        public void UpdateForm(UserEntity userEntity)
        {
            service.Update(userEntity);
        }
        public UserEntity CheckLogin(string username, string password)
        {
            UserEntity userEntity = service.FindEntity(t => t.F_Account == username);
            if (userEntity != null)
            {
                if (userEntity.F_EnabledMark == true)
                {
                    UserLogOnEntity userLogOnEntity = userLogOnApp.GetForm(userEntity.F_Id);
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(password.ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                    if (dbPassword == userLogOnEntity.F_UserPassword)
                    {
                        DateTime lastVisitTime = DateTime.Now;
                        int LogOnCount = (userLogOnEntity.F_LogOnCount).ToInt() + 1;
                        if (userLogOnEntity.F_LastVisitTime != null)
                        {
                            userLogOnEntity.F_PreviousVisitTime = userLogOnEntity.F_LastVisitTime.ToDate();
                        }
                        userLogOnEntity.F_LastVisitTime = lastVisitTime;
                        userLogOnEntity.F_LogOnCount = LogOnCount;
                        userLogOnApp.UpdateForm(userLogOnEntity);
                        return userEntity;
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
