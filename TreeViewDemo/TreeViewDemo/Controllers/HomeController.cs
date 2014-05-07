using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreeViewDemo.ViewModel;
using TreeViewDemo.Business;
using TreeViewDemo.Models;
using System.Web.Script.Serialization;

namespace TreeViewDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        EmployeeManeger _emp = new EmployeeManeger();
        public static OrganizationEntities db = new OrganizationEntities();

        [Authorize]
        public ActionResult Index()
        {
            //JsTreeModel treeView = _emp.GetTreeVeiwList();
            return View();
        }

        public PartialViewResult ChildNode()
        {
            return PartialView();
        }

        public ActionResult Cautare()
        {
            return View();
        }

        public string Locatii(long? id = null)
        {
            if (id == null)
                return "";

            string result = "";
            var list = (from l in db.sites where l.judetID == id select new { l.id, l.denumire }).ToList();
            foreach(var x in list)
            {
                result += x.id +"," + x.denumire + ":";
            }
            result = result.TrimEnd(':');
            return result;
        }

        public string GetJudete()
        {
            string result = "";
            var list = (from l in db.judetes select new { l.id, l.denumire }).ToList();
            foreach (var x in list)
            {
                result += x.id + "," + x.denumire + ":";
            }
            result = result.TrimEnd(':');
            return result;
        }

        public string GetTipEchip()
        {
            string result = "";
            var list = (from l in db.tip_echipament select new { l.id, l.denumire }).ToList();
            foreach (var x in list)
            {
                result += x.id + "," + x.denumire + ":";
            }
            result = result.TrimEnd(':');
            return result;
        }

        public string GetVersiuni()
        {
            string result = "";
            var list = (from l in db.echipament_versiuni select new { l.id, l.denumire }).ToList();
            foreach (var x in list)
            {
                result += x.id + "," + x.denumire + ":";
            }
            result = result.TrimEnd(':');
            return result;
        }

        public string Cauta(long? judetID, long? siteID, long? tipechipID, string ip, string tippabx, string versiune, string licenta, string contract,string tipcon, bool abonati)
        {
            if (siteID == null)
                siteID = -1;
            if (judetID == null)
                judetID = -1;
            if (tipechipID == null)
                tipechipID = -1;

            long? tipechippabxID = (from te in db.tip_echipament where te.denumire == "PABX" select te.id).FirstOrDefault();
            if(tipechippabxID == tipechipID) //pabx
            {
                long? atr_tipID ;
                long? vertipID ;

                //string tipechip = (from te in db.tip_echipament where te.id == tipechipID select te.denumire).FirstOrDefault();

                    atr_tipID = tippabx != "-1" ? (from et in db.echipament_tip where et.denumire == tippabx select et.id).FirstOrDefault() : -1;
                    vertipID = versiune != "-1" ? (from et in db.echipament_versiuni where et.denumire == versiune && et.echipament == tippabx select et.id).FirstOrDefault() : -1;

                    var query = (from j in db.judetes
                                 join s in db.sites on j.id equals s.judetID
                                 join ec in db.echipaments on s.id equals ec.siteID
                                 join te in db.tip_echipament on ec.tipID equals te.id
                                 //join ea in db.echipament_atribute on ec.id equals ea.echipamentID
                                 //join a in db.atributs on ea.atributID equals a.id
                                 //join ta in db.tip_atribut on a.tipID equals ta.id
                                 let idtip = (from ec1 in db.echipaments
                                            join ea1 in db.echipament_atribute on ec1.id equals ea1.echipamentID
                                            join a1 in db.atributs on ea1.atributID equals a1.id
                                            join ta1 in db.tip_atribut on a1.tipID equals ta1.id
                                            where ec1.id == ec.id && ta1.denumire == "Tip" 
                                            select a1.val_int).FirstOrDefault()
                                 let tip = (from ev in db.echipament_tip where ev.id == idtip select ev.denumire).FirstOrDefault()
                                 let idver = (from ec2 in db.echipaments
                                  join ea2 in db.echipament_atribute on ec2.id equals ea2.echipamentID
                                  join a2 in db.atributs on ea2.atributID equals a2.id
                                  join ta2 in db.tip_atribut on a2.tipID equals ta2.id
                                  where ec2.id == ec.id && ta2.denumire == "Versiune"
                                  select a2.val_int).FirstOrDefault()
                                 let ver1 = (from ev1 in db.echipament_versiuni where ev1.id == idver select ev1.denumire).FirstOrDefault()
                                 let ip1 = (from ec3 in db.echipaments
                                           join ea3 in db.echipament_atribute on ec3.id equals ea3.echipamentID
                                           join a3 in db.atributs on ea3.atributID equals a3.id
                                           join ta3 in db.tip_atribut on a3.tipID equals ta3.id
                                           where ec3.id == ec.id && (ta3.denumire == "IP" || ta3.denumire == "IP CES" || ta3.denumire == "IP Management")
                                           select a3.val_string).FirstOrDefault()
                                 let licenta1 = (from ec4 in db.echipaments
                                                join ea4 in db.echipament_atribute on ec4.id equals ea4.echipamentID
                                                join a4 in db.atributs on ea4.atributID equals a4.id
                                                join ta4 in db.tip_atribut on a4.tipID equals ta4.id
                                                where ec4.id == ec.id && ta4.denumire == "Licenta"
                                                select a4.val_string).FirstOrDefault()
                                 let contract1 = (from ec4 in db.echipaments
                                                 join ea4 in db.echipament_atribute on ec4.id equals ea4.echipamentID
                                                 join a4 in db.atributs on ea4.atributID equals a4.id
                                                 join ta4 in db.tip_atribut on a4.tipID equals ta4.id
                                                 where ec4.id == ec.id && ta4.denumire == "Contract"
                                                 select a4.val_string).FirstOrDefault()
                                 let tipconn1 = (from ec4 in db.echipaments
                                                  join ea4 in db.echipament_atribute on ec4.id equals ea4.echipamentID
                                                  join a4 in db.atributs on ea4.atributID equals a4.id
                                                  join ta4 in db.tip_atribut on a4.tipID equals ta4.id
                                                  where ec4.id == ec.id && ta4.denumire == "Tip echip."
                                                  && a4.val_string == tipcon
                                                  select a4.val_string ).FirstOrDefault()
                                 where ((j.id == judetID && judetID != -1) || judetID == -1)
                                 && ((s.id == siteID && siteID != -1) || siteID == -1)
                                 && ((te.id == tipechipID && tipechipID != -1) || tipechipID == -1)
                                 &&
                                  (idtip == atr_tipID || atr_tipID == -1)
                                  &&
                                  (idver == vertipID || vertipID == -1)
                                  &&
                                  (ip1 == ip || ip =="")
                                  &&
                                  (licenta1 == licenta || licenta == "")
                                  &&
                                  (contract1 == contract || contract == "-1")
                                  &&
                                  (tipconn1 == tipcon || tipcon == "-1")
                                  select new {ec.id, judetDen = j.denumire, siteDen = s.denumire, tipechip = te.denumire, ip1 = ip1,
                                           tip1 = tip,
                                           ver = ver1,
                                           lic1 = licenta1,
                                           con = contract1, 
                                           tipcon1 = tipconn1 }).Distinct().ToList();
         
                
                    List<object> lista = new List<object>();

                    JavaScriptSerializer sr = new JavaScriptSerializer();
                    return sr.Serialize(query);
            }
            else
            {
                var query = (from j in db.judetes
                             join s in db.sites on j.id equals s.judetID
                             join ec in db.echipaments on s.id equals ec.siteID
                             join te in db.tip_echipament on ec.tipID equals te.id
                             join ea in db.echipament_atribute on ec.id equals ea.echipamentID
                             join a in db.atributs on ea.atributID equals a.id
                             join ta in db.tip_atribut on a.tipID equals ta.id
                             let ip1 = (from ec3 in db.echipaments
                                        join ea3 in db.echipament_atribute on ec3.id equals ea3.echipamentID
                                        join a3 in db.atributs on ea3.atributID equals a3.id
                                        join ta3 in db.tip_atribut on a3.tipID equals ta3.id
                                        where ec3.id == ec.id && (ta3.denumire == "IP" || ta3.denumire == "IP CES" || ta3.denumire == "IP Management")
                                        select a3.val_string).FirstOrDefault()
                             where ((j.id == judetID && judetID != -1) || judetID == -1)
                             && ((s.id == siteID && siteID != -1) || siteID == -1)
                             && ((te.id == tipechipID && tipechipID != -1) || tipechipID == -1)
                             &&
                                  (ip1 == ip || ip == "")
                             select new { ec.id, judetDen = j.denumire, siteDen = s.denumire, tipechip = te.denumire, ip1 = ip1 }).Distinct().ToList();

                JavaScriptSerializer sr = new JavaScriptSerializer();
                return sr.Serialize(query);
            }
           
        }

    }
}
