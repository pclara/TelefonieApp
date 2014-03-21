using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TreeViewDemo.Models;

namespace TreeViewDemo.Controllers
{
    public class EchipamentController : Controller
    {
        //
        // GET: /Echipament/
        OrganizationEntities db = new OrganizationEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditEchipament(long? id, string parentID = null, string site = null)
        {
            if (id == -1)
            {
                long tipechipID ;
                if (long.TryParse(parentID, out tipechipID) == false)
                    tipechipID = long.Parse(parentID.Split('_')[0]);

                string idPABX = (from te in db.tip_echipament where te.id == tipechipID select te.denumire).FirstOrDefault();
                //var numeechip = "";
                ViewBag.numeechip = idPABX;
                ViewBag.id = -1;
                ViewBag.tipechipament = parentID;
                long par = 0;
                if (long.TryParse(parentID, out par) == true)
                {
                    ViewBag.tipechipament = par;
                    ViewBag.siteID = site;
                }
                else
                {
                    ViewBag.tipechipament = parentID.Split('_')[0];
                    ViewBag.siteID = parentID.Split('_')[3];
                }
            }
            else
            {
                var numeechip = (from j in db.echipaments where j.id == id select new { j.denumire, j.id }).FirstOrDefault();
                var atribute = (from a in db.atributs
                                join ea in db.echipament_atribute on a.id equals ea.atributID
                                join e in db.echipaments on ea.echipamentID equals e.id
                                join ta in db.tip_atribut on a.tipID equals ta.id
                                
                                where ea.echipamentID == id
                                select new { a.id, ta.tip_valoare, a.val_csv, a.val_int, a.val_nr, a.val_string, tipatrden = ta.denumire }).ToList();

                List<object> lista_atr = new List<object>();

                foreach (var atr in atribute)
                {
                    switch (atr.tip_valoare)
                    {
                        case "S": lista_atr.Add(new  { id_atr = atr.id, nume_atr = atr.tipatrden,  tip_val = atr.tip_valoare, val_atr = atr.val_string });
                            break;
                        case "I":
                            {
                                if (atr.tipatrden != "Versiune" && atr.tipatrden != "Tip")
                                    lista_atr.Add(new { id_atr = atr.id, nume_atr = atr.tipatrden, tip_val = atr.tip_valoare, val_atr = atr.val_int });
                                else if (atr.tipatrden == "Versiune")
                                {
                                    string versiune = (from ver in db.echipament_versiuni where ver.id == atr.val_int select ver.denumire).FirstOrDefault();
                                    lista_atr.Add(new { id_atr = atr.id, nume_atr = atr.tipatrden, tip_val = atr.tip_valoare, val_atr = new { idver = atr.val_int, den_ver = versiune } });
                                }
                                else if (atr.tipatrden == "Tip")
                                {
                                    string versiune = (from ver in db.echipament_tip where ver.id == atr.val_int select ver.denumire).FirstOrDefault();
                                    lista_atr.Add(new { id_atr = atr.id, nume_atr = atr.tipatrden, tip_val = atr.tip_valoare, val_atr = new { idsel = atr.val_int, den_tip = versiune } });
                                }
                                break;
                            }
                        case "N": lista_atr.Add(new { id_atr = atr.id, nume_atr = atr.tipatrden, tip_val = atr.tip_valoare, val_atr = atr.val_nr });
                            break;
                        case "CSV": lista_atr.Add(new { id_atr = atr.id, nume_atr = atr.tipatrden, tip_val = atr.tip_valoare, val_atr = atr.val_csv });
                            break;
                        default: lista_atr.Add(new { id_atr = atr.id, nume_atr = atr.tipatrden, tip_val = atr.tip_valoare, val_atr = atr.val_string });
                            break;
                    }
                }
                JavaScriptSerializer sr = new JavaScriptSerializer();
                
                JsonResult jr = new JsonResult();
                jr.Data = lista_atr;
                string atr1 = sr.Serialize(lista_atr);
                ViewBag.atribute = atr1 ;
                ViewBag.numeechip = numeechip.denumire.ToString();
                ViewBag.tipechipament = parentID.Split('_')[0];
                ViewBag.id = long.Parse(numeechip.id.ToString());
                ViewBag.siteID = 0;
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult Edit(FormCollection echipmodel)
        {
            if (ModelState.IsValid)
            {
                echipament s;
                long id = long.Parse(echipmodel["id"]);
                string tipechip = echipmodel["numeechip"];
                if (id == -1)
                {
                    if (tipechip == "PABX")
                    {
                        s = new echipament();
                        s.denumire = echipmodel["denumire"].ToString();

                        int x = 0;
                        if(int.TryParse(echipmodel["tipechipament"].ToString(), out x) == true)
                            s.tipID = int.Parse(echipmodel["tipechipament"].ToString());
                        else
                            s.tipID = int.Parse(echipmodel["tipechipament"].ToString().Split('_')[0]);


                        s.siteID = long.Parse(echipmodel["siteID"].ToString()); /// ???
                        db.AddToechipaments(s);

                        atribut a = new atribut();
                        a.val_string = null;
                        a.val_csv = null;
                        a.val_int = int.Parse(echipmodel["tip_val"]);
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Tip" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        echipament_atribute ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();

                        a = new atribut();
                        a.val_string = null;
                        a.val_csv = null;
                        a.val_int = int.Parse(echipmodel["ver_val"].ToString());
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Versiune" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();

                        a = new atribut();
                        a.val_string = echipmodel["ip_val"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "IP" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);
                    }
                    else
                        s = new echipament();

                    db.SaveChanges();
                }
                else
                {
                    s = db.echipaments.Where(o => o.id == id).FirstOrDefault();
                    int i = 0;
                    foreach (var key in echipmodel.Keys)
                    {
                        if (key.ToString() != "numeechip" && key.ToString() != "tip_nou" && key.ToString() != "denumire" && key.ToString() != "tip_val" && key.ToString() != "val_nou" && key.ToString() != "id" && !key.ToString().EndsWith(":"))
                        {
                            string idechip_idatr = key.ToString();
                            long idechip = long.Parse(idechip_idatr.Split('_')[0]);
                            long idatr = long.Parse(idechip_idatr.Split('_')[1]);

                            atribut a = db.atributs.Where(o=> o.id == idatr).FirstOrDefault();

                            string valoare = echipmodel[i];
                            switch (echipmodel[key.ToString() + ":"])
                            {
                                case "S" : a.val_string = echipmodel[key.ToString()];
                                            break;
                                case "I": a.val_int = int.Parse(echipmodel[key.ToString()]);
                                            break;
                                case "N" : a.val_nr = decimal.Parse(echipmodel[key.ToString()]);
                                            break;
                                case "CSV": a.val_csv = echipmodel[key.ToString()];
                                            break;
                                default:    a.val_string = echipmodel[key.ToString()].ToString();
                                            break;
                            }
                            db.SaveChanges();
                        }
                        else if (key.ToString() == "tip_nou")
                        {
                            atribut a = new atribut();
                            string tip_nou = echipmodel["tip_nou"];
                            var taid = (from ta in db.tip_atribut where ta.denumire == tip_nou select new { ta.id, ta.tip_valoare }).FirstOrDefault();
                            a.tipID = taid.id;
                            switch (taid.tip_valoare)
                            {
                                case "S":
                                    a.val_string = echipmodel["val_nou"];
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = null;
                                    break;
                                case "I":
                                    a.val_string = null;
                                    a.val_csv = null;
                                    a.val_int = int.Parse(echipmodel["val_nou"].ToString());
                                    a.val_nr = null;
                                    break;
                                case "N":
                                    a.val_string = null;
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = decimal.Parse(echipmodel["val_nou"].ToString());
                                    break;
                                case "CSV":
                                    a.val_string = null;
                                    a.val_csv = echipmodel["val_nou"];
                                    a.val_int = null;
                                    a.val_nr = null;
                                    break;
                                default:
                                    a.val_string = echipmodel["val_nou"];
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = null;
                                    break;
                            }

                            echipament_atribute ea = new echipament_atribute();
                            ea.atributID = a.id;
                            ea.echipamentID = id;

                            db.AddToatributs(a);
                            db.AddToechipament_atribute(ea);
                            db.SaveChanges();
                        }
                    }
                }
                var judet = (from j in db.judetes
                             join s1 in db.sites on j.id equals s1.judetID
                             join e in db.echipaments on s1.id equals e.siteID
                             where e.id == s.id
                             select new { j.id, j.denumire }).FirstOrDefault();

                return RedirectToAction("Index", "Judete", new { name = judet.denumire.ToString() , id = s.id.ToString() + "_echipament" });
            }
            else
            {
                return Json("Salvarea nu a fost efectuata!");
            }
        }

        public string CaleNod(string id)
        {
            long idechip = long.Parse(id.Split('_')[0]);
            string cale = "";

            long siteID = (from s in db.sites join e in db.echipaments on s.id equals e.siteID where e.id == idechip select s.id).FirstOrDefault();
            long judetID = (from s in db.sites join e in db.judetes on s.judetID equals e.id where s.id == siteID select e.id).FirstOrDefault();
            var tip_echip = (from t in db.tip_echipament join e in db.echipaments on t.id equals e.tipID where e.id == idechip select new { t.id, t.denumire }).FirstOrDefault();
            cale = judetID.ToString() + "_judet," + siteID.ToString() + "_site," + tip_echip.id + "_tip_echipament_" + siteID.ToString() +  "_" + judetID.ToString() + "," + id;
            return cale;
        }

        public string GetVersiuni(long? idver)
        {
            string tipcartele = "";
            var tip = (from tc in db.echipament_versiuni select new { tc.id, tc.denumire }).ToList();
            foreach (var s in tip)
            {
                tipcartele += s.id +":" + s.denumire + ",";
            }
            if (idver != null)
                tipcartele += idver.ToString();
            else
                tipcartele = tipcartele.TrimEnd(',');

            return tipcartele;
        }

        public string GetTip(long? idsel)
        {
            string tipcartele = "";
            var tip = (from tc in db.echipament_tip select new { tc.id, tc.denumire }).ToList();
            foreach (var s in tip)
            {
                tipcartele += s.id + ":" + s.denumire + ",";
            }
            if (idsel != null)
                tipcartele += idsel.ToString();
            else
                tipcartele = tipcartele.TrimEnd(',');

            return tipcartele;
        }



        [HttpPost]
        public string StergeEchipament(string id)
        {
            long idechipament = long.Parse(id.Split('_')[0]);

           // long idcentrala = (from c in db.cartelas where c.id == idcartela select c.echipamentID).FirstOrDefault();
            long tip_id = (from e in db.echipaments where e.id == idechipament select e.tipID).FirstOrDefault();

            var denumire = (from t in db.tip_echipament where t.id == tip_id select t.denumire).FirstOrDefault();


            if (denumire == "PABX")
            {

                var listacartele = (from c in db.cartelas where c.echipamentID == idechipament select c.id);

                foreach (long idcartela in listacartele)
                {
                    var judet = (from j in db.judetes
                                 join s in db.sites on j.id equals s.judetID
                                 join e in db.echipaments on s.id equals e.siteID
                                 join c in db.cartelas
                                     on e.id equals c.echipamentID
                                 where c.id == idcartela
                                 select new { j.id, j.denumire }).FirstOrDefault();

                    cartela car = (from c in db.cartelas where c.id == idcartela select c).FirstOrDefault();
                    var c_a = (from ca in db.cartela_atribute where ca.cartelaID == idcartela select new { ca.id, ca.atributID }).ToList();
                    //var atr = ( from a in db.atributs join ca in db.cartela_atribute on a.id equals ca.atributID  select a.id).ToList();


                    foreach (var x in c_a)
                    {
                        cartela_atribute y = (from a in db.cartela_atribute where a.id == x.id select a).FirstOrDefault();
                        db.cartela_atribute.DeleteObject(y);
                        db.SaveChanges();
                    }

                    foreach (var x in c_a)
                    {
                        atribut y = (from a in db.atributs where a.id == x.atributID select a).FirstOrDefault();
                        db.atributs.DeleteObject(y);
                        db.SaveChanges();
                    }


                    db.cartelas.DeleteObject(car);
                    db.SaveChanges();

                }
            }


                var listaatr = (from e in db.echipament_atribute where e.echipamentID == idechipament select new { e.id, e.atributID }).ToList();

           

                foreach (var x in listaatr)
                {
                    atribut y = (from a in db.atributs where a.id == x.atributID select a).FirstOrDefault();
                    db.atributs.DeleteObject(y);
                    db.SaveChanges();
                }


                foreach (var x in listaatr)
                {
                    echipament_atribute y = (from a in db.echipament_atribute where a.id == x.id select a).FirstOrDefault();
                    db.echipament_atribute.DeleteObject(y);
                    db.SaveChanges();
                }


             
                    echipament z = (from a in db.echipaments where a.id == idechipament select a).FirstOrDefault();
                    db.echipaments.DeleteObject(z);
                     db.SaveChanges();

                     //}

                var listaindex = (from e in db.echipaments 
                                 join s in db.sites on e.siteID equals s.id
                                 join j in db.judetes on s.judetID equals j.id select new {jid = j.id,sid = s.id}).FirstOrDefault();

                return tip_id.ToString() + "_tip_echipament_" + listaindex.sid.ToString() + "_" + listaindex.jid.ToString();

            //return RedirectToAction("Index", "Judete", new { name = judet.denumire.ToString(), id = idcentrala.ToString() + "_echipament" });
        }
    }
}
