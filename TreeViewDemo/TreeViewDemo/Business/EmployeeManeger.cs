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
                    long sitID = long.Parse(siteID);
                    List<JsTreeModel> children = (from e1 in _orgDb.tip_echipament
                                                  let copii = (from j in _orgDb.judetes join s in _orgDb.sites on j.id equals s.judetID
                                                                   join e in _orgDb.echipaments on s.id equals e.siteID
                                                                   join te in _orgDb.tip_echipament on e.tipID equals te.id
                                                                   where te.id == e1.id
                                                                   && s.id == sitID
                                                                   select e.id).Count()
                                                  select new JsTreeModel()
                                                  {
                                                      data = new JsTreeData() { title = e1.denumire, icon = "" },
                                                      attr = new JsTreeAttribute() { description = "tip_echipament", id = SqlFunctions.StringConvert((double)e1.id).Trim() + "_tip_echipament_" + siteID + "_" + judetID, selected = false },
                                                      id = SqlFunctions.StringConvert((double)e1.id).Trim() +"_tip_echipament_"+ siteID + "_"+judetID,
                                                      state = copii > 0 ? "closed" : null
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

                                                  let nrmsed = (from e2 in _orgDb.echipaments
                                                                join te1 in _orgDb.tip_echipament
                                                                on e2.tipID equals te1.id
                                                                where te1.id == idroot
                                                                && e2.siteID == idparam && te1.denumire == "MSED"
                                                                where e2.id < e1.id
                                                                select e2.id).Count()

                                                  let nrmc = (from e2 in _orgDb.echipaments
                                                                join te1 in _orgDb.tip_echipament
                                                                on e2.tipID equals te1.id
                                                                where te1.id == idroot
                                                                && e2.siteID == idparam && te1.denumire == "MC"
                                                                where e2.id < e1.id
                                                                select e2.id).Count()
                                                  let nrrouter = (from e2 in _orgDb.echipaments
                                                              join te1 in _orgDb.tip_echipament
                                                              on e2.tipID equals te1.id
                                                              where te1.id == idroot
                                                              && e2.siteID == idparam && te1.denumire == "Router"
                                                              where e2.id < e1.id
                                                              select e2.id).Count()
                                                  let nrswitch = (from e2 in _orgDb.echipaments
                                                                  join te1 in _orgDb.tip_echipament
                                                                  on e2.tipID equals te1.id
                                                                  where te1.id == idroot
                                                                  && e2.siteID == idparam && te1.denumire == "Switch"
                                                                  where e2.id < e1.id
                                                                  select e2.id).Count()
                                                  let nrcon = (from e2 in _orgDb.echipaments
                                                                  join te1 in _orgDb.tip_echipament
                                                                  on e2.tipID equals te1.id
                                                                  where te1.id == idroot
                                                                  && e2.siteID == idparam && te1.denumire == "Concentrator"
                                                                  where e2.id < e1.id
                                                                  select e2.id).Count()
    
    
                                                  let titlu = te.denumire == "PABX" ?
                                                        (from a in _orgDb.atributs
                                                         join ea in _orgDb.echipament_atribute on a.id equals ea.atributID
                                                         join e in _orgDb.echipaments on ea.echipamentID equals e.id
                                                         join ta in _orgDb.tip_atribut on a.tipID equals ta.id
                                                         join te1 in _orgDb.echipament_tip on a.val_int equals te1.id 
                                                         where e.id == e1.id && ta.denumire == "Tip"
                                                         select te1.denumire).FirstOrDefault() :
                                                         (te.denumire == "MSED" ? "MSED" + SqlFunctions.StringConvert((double)nrmsed).Trim() : 
                                                         (te.denumire == "MC" ? "MC" + SqlFunctions.StringConvert((double)nrmc).Trim() :
                                                         (te.denumire == "Router" ? "Router" + SqlFunctions.StringConvert((double)nrrouter).Trim() : (
                                                         te.denumire == "Switch" ? "Switch" + SqlFunctions.StringConvert((double)nrswitch).Trim() : (
                                                         te.denumire == "Concentrator" ? "Concentrator" + SqlFunctions.StringConvert((double)nrcon).Trim() : null)
                                                         )) 
                                                         )
                                                         )
                                                  let copii = (from car in _orgDb.cartelas
                                                               join ech in _orgDb.echipaments on car.echipamentID equals ech.id
                                                               where ech.id == e1.id
                                                               select car.id).Count()
                                                  select new JsTreeModel()
                                                  {
                                                      data = new JsTreeData() { title = titlu, icon = "" },
                                                      attr = new JsTreeAttribute() { description = "echipament", id = SqlFunctions.StringConvert((double)e1.id).Trim() +"_echipament", selected = false },
                                                      id = SqlFunctions.StringConvert((double)e1.id).Trim() + "_echipament",
                                                      state =  copii > 0 ?  "closed" : null
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