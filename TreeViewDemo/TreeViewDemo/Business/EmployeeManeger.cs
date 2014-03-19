using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TreeViewDemo.ViewModel;
using TreeViewDemo.Models;
using System.Web.Script.Serialization;
using System.Data.Sql;
using System.Data.Objects.SqlClient;

namespace TreeViewDemo.Business
{
    public class EmployeeManeger
    {
        OrganizationEntities _orgDb = new OrganizationEntities();


        public JsTreeModel GetTreeVeiwList(string name)
        {
            JsTreeModel root = (from e1 in _orgDb.judetes
                                where e1.denumire == name
                                select new JsTreeModel()
                                {
                                    data = new JsTreeData(){ title = e1.denumire, icon = "" },
                                    attr = new JsTreeAttribute() { description = "judet", id = SqlFunctions.StringConvert((double)e1.id).Trim() + "_judet", selected = false, style = "font-weight:bold;" },
                                    id = SqlFunctions.StringConvert((double)e1.id).Trim(),
                                    state = "closed"
                                }).SingleOrDefault();

            
            
            BuildChildNode(root);
            
            return root;
        }


        private void BuildChildNode(JsTreeModel rootNode, string id = null)
        {
            int idparam  = 0;
            if(id!=null)
                idparam = Int32.Parse(id.Split(new char[] { '_' })[0]);

            int idroot = Int32.Parse(rootNode.id.Split(new char[]{'_'})[0]);
            if (rootNode != null)
            {
                if (rootNode.attr.description == "judet")
                {
                    string parentID = rootNode.attr.id.Split('_')[0];
                    List<JsTreeModel> children = (from e1 in _orgDb.sites
                                                  where e1.judetID == idroot
                                                  select new JsTreeModel()
                                                  {
                                                      data = new JsTreeData() { title = e1.denumire, icon = "" },
                                                      attr = new JsTreeAttribute() { description = "site", id = SqlFunctions.StringConvert((double)e1.id).Trim() + "_site", selected = false, parentJudet = parentID },
                                                      id = SqlFunctions.StringConvert((double)e1.id).Trim() + "_site",
                                                      state = "closed"
                                                  }).ToList<JsTreeModel>();

                    if (children.Count > 0)
                    {
                        foreach (var childRootNode in children)
                        {
                            BuildChildNode(childRootNode);
                            rootNode.children.Add(childRootNode);
                        }
                    }
                }
                else if (rootNode.attr.description == "site")
                {
                    string siteID = rootNode.attr.id.Split('_')[0];
                    string judetID = rootNode.attr.parentJudet;

                    List<JsTreeModel> children = (from e1 in _orgDb.tip_echipament
                                                  select new JsTreeModel()
                                                  {
                                                      data = new JsTreeData() { title = e1.denumire, icon = "" },
                                                      attr = new JsTreeAttribute() { description = "tip_echipament", id = SqlFunctions.StringConvert((double)e1.id).Trim() + "_tip_echipament_" + siteID + "_" + judetID, selected = false },
                                                      id = SqlFunctions.StringConvert((double)e1.id).Trim() +"_tip_echipament_"+ siteID + "_"+judetID,
                                                      state = "closed"
                                                  }).ToList<JsTreeModel>();

                    if (children.Count > 0)
                    {
                        foreach (var childRootNode in children)
                        {
                            BuildChildNode(childRootNode, rootNode.attr.id);
                            rootNode.children.Add(childRootNode);
                        }
                    }
                }
                else if(rootNode.attr.description == "tip_echipament")
                {
                    List<JsTreeModel> children = (from e1 in _orgDb.echipaments
                                                  join te in _orgDb.tip_echipament 
                                                  on e1.tipID equals te.id
                                                  where te.id == idroot
                                                  && e1.siteID == idparam
                                                  let titlu = (from a in _orgDb.atributs join ea in _orgDb.echipament_atribute on a.id equals ea.atributID
                                                                   join e in _orgDb.echipaments on ea.echipamentID equals e.id
                                                                   join ta in _orgDb.tip_atribut on a.tipID equals ta.id
                                                                   where e.id == e1.id && ta.denumire == "Tip"
                                                                   select a.val_string).FirstOrDefault()
                                                  select new JsTreeModel()
                                                  {
                                                      data = new JsTreeData() { title = titlu, icon = "" },
                                                      attr = new JsTreeAttribute() { description = "echipament", id = SqlFunctions.StringConvert((double)e1.id).Trim() +"_echipament", selected = false },
                                                      id = SqlFunctions.StringConvert((double)e1.id).Trim() + "_echipament",
                                                      state = "closed"
                                                  }).ToList<JsTreeModel>();

                    if (children.Count > 0)
                    {
                        foreach (var childRootNode in children)
                        {
                            BuildChildNode(childRootNode);
                            rootNode.children.Add(childRootNode);
                        }
                    }
                }
                else if (rootNode.attr.description == "echipament")
                {
                    List<JsTreeModel> children = (from c in _orgDb.cartelas
                                                  join tc in _orgDb.tip_cartela on c.tipID equals tc.id
                                                  join e1 in _orgDb.echipaments on c.echipamentID equals e1.id
                                                  where e1.id == idroot
                                                  let titlu = (from a in _orgDb.atributs
                                                               join ea in _orgDb.cartela_atribute on a.id equals ea.atributID
                                                               join e in _orgDb.cartelas on ea.cartelaID equals e.id
                                                               join ta in _orgDb.tip_cartela on e.tipID equals ta.id
                                                               where e.id == c.id
                                                               select ta.denumire).FirstOrDefault()
                                                  select new JsTreeModel()
                                                  {
                                                      data = new JsTreeData() { title = titlu, icon = "" },
                                                      attr = new JsTreeAttribute() { description = "cartela", id = SqlFunctions.StringConvert((double)c.id).Trim() + "_cartela", selected = false },
                                                      id = SqlFunctions.StringConvert((double)c.id).Trim() + "_cartela",
                                                      state = null
                                                  }).ToList<JsTreeModel>();
                    if (children.Count > 0)
                    {
                        foreach (var childRootNode in children)
                        {
                            //BuildChildNode(childRootNode);
                            rootNode.children.Add(childRootNode);
                        }
                    }
                }

            }
        }
    }
}