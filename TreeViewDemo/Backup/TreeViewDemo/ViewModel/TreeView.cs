using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeViewDemo.ViewModel
{
    public class TreeViewNodeVM
    {
        public TreeViewNodeVM()
        {
            ChildNode = new List<TreeViewNodeVM>();
        }

        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string NodeName
        {
            get { return GetNodeName(); }
        }
        public IList<TreeViewNodeVM> ChildNode { get; set; }

        public string GetNodeName()
        {
            string temp = ChildNode.Count > 0 ? "    This employee manages " + ChildNode.Count : "    This employee is working without westing time in managing.";
            return this.EmployeeCode + "  " + this.EmployeeName + temp;
        }
    }




}