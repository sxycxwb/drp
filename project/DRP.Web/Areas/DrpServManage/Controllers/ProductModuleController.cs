/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 计费运营系统
 * Website：
*********************************************************************************/

using System.Collections.Generic;
using System.Linq;
using DRP.Application.DrpServManage;
using DRP.Code;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.Entity.SystemManage;
using System.Web.Mvc;


namespace DRP.Web.Areas.DrpServManage.Controllers
{
    public class ProductModuleController : ControllerBase
    {
        private ProductModuleApp productModuleApp = new ProductModuleApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string productId)
        {
            var data = productModuleApp.GetList(productId);
            var treeList = new List<TreeGridModel>();
            foreach (ProductModuleEntity item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.id = item.F_Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = productModuleApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ProductModuleEntity productModuleEntity, string keyValue)
        {
            productModuleApp.SubmitForm(productModuleEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            productModuleApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }          

    }
}
