/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Code;
using DRP.Domain.Entity.SystemManage;
using DRP.Domain.IRepository.SystemManage;
using DRP.Domain.ViewModel;
using DRP.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DRP.Application.SystemManage
{
    public class RoleAuthorizeApp
    {
        private IRoleAuthorizeRepository service = new RoleAuthorizeRepository();
        private IModuleRepository moduleService = new ModuleRepository();
        private ModuleApp moduleApp = new ModuleApp();
        private ModuleButtonApp moduleButtonApp = new ModuleButtonApp();

        public List<RoleAuthorizeEntity> GetList(string ObjectId)
        {
            return service.IQueryable(t => t.F_ObjectId == ObjectId).ToList();
        }

        /// <summary>
        /// 后台用户是否有权限查看用户中心
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool SystemUserToUseCenterValidate(string roleId)
        {
            var customerCenterMenuId = Configs.GetValue("CustomerCenterMenuId");

            if ((OperatorProvider.Provider.GetCurrent() != null && OperatorProvider.Provider.GetCurrent().IsSystem) || (ClientOperatorProvider.Provider.GetCurrent() != null && ClientOperatorProvider.Provider.GetCurrent().IsSystem))
                return true;
            var menuCount = moduleService.IQueryable(t => t.F_Id == customerCenterMenuId || t.F_ParentId == customerCenterMenuId).Count();
            if (menuCount > 1)
                return true;
            else
                return false;
        }

        public List<ModuleEntity> GetMenuList(string roleId)
        {
            var data = new List<ModuleEntity>();
            var customerCenterMenuId = Configs.GetValue("CustomerCenterMenuId");
            if ((OperatorProvider.Provider.GetCurrent()!=null&&OperatorProvider.Provider.GetCurrent().IsSystem) || (ClientOperatorProvider.Provider.GetCurrent() != null && ClientOperatorProvider.Provider.GetCurrent().IsSystem))
            {
                data = moduleApp.GetList();
                if (!string.IsNullOrEmpty(customerCenterMenuId))
                    data = data.Where(t => t.F_Id != customerCenterMenuId).ToList();//过滤用户中心菜单
            }
            else
            {
                var moduledata = moduleApp.GetList();
                if (!string.IsNullOrEmpty(customerCenterMenuId))
                    moduledata = moduledata.Where(t => t.F_Id != customerCenterMenuId).ToList();//过滤用户中心菜单
                var authorizedata = service.IQueryable(t => t.F_ObjectId == roleId && t.F_ItemType == 1).ToList();
                foreach (var item in authorizedata)
                {
                    ModuleEntity moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId);
                    if (moduleEntity != null)
                    {
                        data.Add(moduleEntity);
                    }
                }
            }
            return data.OrderBy(t => t.F_SortCode).ToList();
        }
        public List<ModuleButtonEntity> GetButtonList(string roleId)
        {
            var data = new List<ModuleButtonEntity>();

            if ((OperatorProvider.Provider.GetCurrent() != null && OperatorProvider.Provider.GetCurrent().IsSystem) || (ClientOperatorProvider.Provider.GetCurrent() != null && ClientOperatorProvider.Provider.GetCurrent().IsSystem))
            {
                data = moduleButtonApp.GetList();
            }
            else
            {
                var buttondata = moduleButtonApp.GetList();
                var authorizedata = service.IQueryable(t => t.F_ObjectId == roleId && t.F_ItemType == 2).ToList();
                foreach (var item in authorizedata)
                {
                    ModuleButtonEntity moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId);
                    if (moduleButtonEntity != null)
                    {
                        data.Add(moduleButtonEntity);
                    }
                }
            }
            return data.OrderBy(t => t.F_SortCode).ToList();
        }
        public bool ActionValidate(string roleId, string moduleId, string action)
        {
            var authorizeurldata = new List<AuthorizeActionModel>();
            var cachedata = CacheFactory.Cache().GetCache<List<AuthorizeActionModel>>("authorizeurldata_" + roleId);
            if (cachedata == null)
            {
                var moduledata = moduleApp.GetList();
                var buttondata = moduleButtonApp.GetList();
                var authorizedata = service.IQueryable(t => t.F_ObjectId == roleId).ToList();
                foreach (var item in authorizedata)
                {
                    if (item.F_ItemType == 1)
                    {
                        ModuleEntity moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId);
                        authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleEntity.F_Id, F_UrlAddress = moduleEntity.F_UrlAddress });
                    }
                    else if (item.F_ItemType == 2)
                    {
                        ModuleButtonEntity moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId);
                        authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleButtonEntity.F_ModuleId, F_UrlAddress = moduleButtonEntity.F_UrlAddress });
                    }
                }
                CacheFactory.Cache().WriteCache(authorizeurldata, "authorizeurldata_" + roleId, DateTime.Now.AddMinutes(5));
            }
            else
            {
                authorizeurldata = cachedata;
            }
            authorizeurldata = authorizeurldata.FindAll(t => t.F_Id.Equals(moduleId));
            foreach (var item in authorizeurldata)
            {
                if (!string.IsNullOrEmpty(item.F_UrlAddress))
                {
                    string[] url = item.F_UrlAddress.Split('?');
                    if (item.F_Id == moduleId && url[0] == action)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
