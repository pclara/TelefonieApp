﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreeViewDemo.ViewModel;
using TreeViewDemo.Business;

namespace TreeViewDemo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        EmployeeManeger _emp = new EmployeeManeger();

        public ActionResult Index()
        {
            //JsTreeModel treeView = _emp.GetTreeVeiwList();
            return View();
        }

        public PartialViewResult ChildNode()
        {
            return PartialView();
        }

    }
}
