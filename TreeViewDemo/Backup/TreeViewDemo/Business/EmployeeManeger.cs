using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TreeViewDemo.ViewModel;
using TreeViewDemo.Models;

namespace TreeViewDemo.Business
{
    public class EmployeeManeger
    {
        OrganizationEntities _orgDb = new OrganizationEntities();


        public TreeViewNodeVM GetTreeVeiwList()
        {
            TreeViewNodeVM rootNode = (from e1 in _orgDb.Employees
                                       where e1.Reporting == null
                                       select new TreeViewNodeVM()
                                       {
                                           EmployeeCode = e1.Emp_Code,
                                           EmployeeName = e1.Name
                                       }).SingleOrDefault();
            BuildChildNode(rootNode);

            return rootNode;
        }


        private void BuildChildNode(TreeViewNodeVM rootNode)
        {
            if (rootNode != null)
            {
                List<TreeViewNodeVM> chidNode = (from e1 in _orgDb.Employees
                                                  where e1.Reporting == rootNode.EmployeeCode
                                                  select new TreeViewNodeVM()
                                                  {
                                                      EmployeeCode = e1.Emp_Code,
                                                      EmployeeName = e1.Name
                                                  }).ToList<TreeViewNodeVM>();
                if (chidNode.Count > 0)
                {
                    foreach (var childRootNode in chidNode)
                    {
                        BuildChildNode(childRootNode);
                        rootNode.ChildNode.Add(childRootNode);
                    }
                }

            }
        }
    }
}