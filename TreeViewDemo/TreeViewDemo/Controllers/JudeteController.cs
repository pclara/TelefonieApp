﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TreeViewDemo.Business;
using TreeViewDemo.ViewModel;
using TreeViewDemo.Models;

namespace TreeViewDemo.Controllers
{
    public class JudeteController : Controller
    {
        //
        // GET: /Judete/
        EmployeeManeger _emp = new EmployeeManeger();
        OrganizationEntities db = new OrganizationEntities();

        public ActionResult Index(string name, string id = null)
        {
            JsTreeModel treeView = _emp.GetTreeVeiwList(name);
            TreeNodesViewModel vm = new TreeNodesViewModel();
            ViewBag.variables = new JavaScriptSerializer().Serialize(treeView);
            ViewBag.selectedID = id;

            return View(treeView);
        }

        public PartialViewResult ChildNode()
        {
            return PartialView();
        }

        public ActionResult EditJudet(long? id)
        {
            var name = (from j in db.judetes where j.id == id select j.denumire).FirstOrDefault();
            ViewBag.nume = name.ToString();
            ViewBag.id = id;
            return PartialView();
        }
    }
}