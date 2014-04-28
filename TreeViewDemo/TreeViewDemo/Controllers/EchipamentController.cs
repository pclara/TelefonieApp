using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
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
                long id1 = long.Parse(numeechip.id.ToString());
                if (numeechip.denumire.ToString() == "PABX")
                {
                    string tip = (from e in db.echipaments
                                  join ea in db.echipament_atribute on e.id equals ea.echipamentID
                                  join a in db.atributs on ea.atributID equals a.id
                                  join ta in db.tip_atribut on a.tipID equals ta.id
                                  join et in db.echipament_tip on a.val_int equals et.id
                                  where ta.denumire == "Tip" && e.id == id1
                                  select et.denumire).FirstOrDefault(); 

                    ViewBag.tipCentrala = tip;
                }
                else
                    ViewBag.tipCentrala = "";
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

                        int val_int;
                        long val_long;
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
                        if( int.TryParse(echipmodel["tip_val"] , out val_int))
                            a.val_int = int.Parse(echipmodel["tip_val"]) ;
                        else 
                            a.val_int = null;
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
                        if (int.TryParse(echipmodel["ver_val"], out val_int))
                            a.val_int = int.Parse(echipmodel["ver_val"]);
                        else
                            a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Versiune" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);
                        db.SaveChanges();

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);
                        db.SaveChanges();
                        ////
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
                        db.SaveChanges();
                        ///
                        int idtip = int.Parse(echipmodel["tip_val"].ToString());
                        string tip_den = ( from a1 in db.atributs join ea1 in db.echipament_atribute on a1.id equals ea1.atributID join
                                            e1 in db.echipaments on ea1.echipamentID equals e1.id join at1 in db.tip_atribut on a1.tipID equals at1.id
                                            join et1 in db.echipament_tip on  a1.val_int equals et1.id
                                           where at1.denumire == "Tip" && et1.id == idtip
                                            select et1.denumire).FirstOrDefault();
                            

                        a = new atribut();
                        a.val_string = (tip_den == "MD" || tip_den == "MX-ONE") ? echipmodel["licenta1"].ToString() + echipmodel["licenta2"].ToString() : echipmodel["licenta1"].ToString();
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Licenta" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);
                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);
                        db.SaveChanges();
                        ////
                        string contract = echipmodel["contract"].ToString();
                        if(echipmodel["contract"].ToString()=="Altul")
                            contract = echipmodel["contract-text"].ToString();

                        a = new atribut();
                        a.val_string = contract;
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Contract" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);
                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);
                        db.SaveChanges();
                        ///
                        a = new atribut();
                        a.val_string = null;
                        a.val_csv = echipmodel["ab_analogici_dela"].ToString() + "," + echipmodel["ab_analogici_panala"].ToString();
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Abonati analogici" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);
                        db.SaveChanges();
                        
                        a = new atribut();
                        a.val_string = null;
                        a.val_csv = echipmodel["ab_digitali_dela"].ToString() + "," + echipmodel["ab_digitali_panala"].ToString();
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Abonati digitali" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);
                        db.SaveChanges();
                       
                        a = new atribut();
                        a.val_string = null;
                        a.val_csv = echipmodel["ab_IP_dela"].ToString() + "," + echipmodel["ab_IP_panala"].ToString();
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Abonati IP" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();
                        
                        a = new atribut();
                        a.val_string = null;
                        a.val_csv = echipmodel["ab_DECT_dela"].ToString() + "," + echipmodel["ab_DECT_panala"].ToString();
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Abonati DECT" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();
                        
                        a = new atribut();
                        a.val_string = null;
                        a.val_csv = echipmodel["ab_total_dela"].ToString() + "," + echipmodel["ab_total_panala"].ToString();
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Abonati total" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);
                        db.SaveChanges();


                        a = new atribut();
                        a.val_string = echipmodel["nrruta"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Numar ruta" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();

                        a = new atribut();
                        a.val_string = echipmodel["tipconexiune"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Tip Conexiune" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();

                        a = new atribut();
                        a.val_string = echipmodel["destinatie"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Destinatie" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();


                        a = new atribut();
                        a.val_string = echipmodel["pozitie"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Poz. echip." select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();

                        a = new atribut();
                        a.val_string = echipmodel["tiptransport"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Tip echip." select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();

                        foreach (string key in echipmodel.Keys)
                        {
                            if (key.StartsWith("nrrutanou") && !key.EndsWith(":"))
                            {
                                a = new atribut();
                                a.val_string = echipmodel[key];
                                a.val_csv = null;
                                a.val_int = null;
                                a.val_nr = null;
                                a.tipID = (from ta in db.tip_atribut where ta.denumire == "Numar ruta" select ta.id).FirstOrDefault();
                                db.AddToatributs(a);

                                ea = new echipament_atribute();
                                ea.echipamentID = s.id;
                                ea.atributID = a.id;
                                db.AddToechipament_atribute(ea);
                                db.SaveChanges();
                            }
                            else if (key.StartsWith("tipcnou") && !key.EndsWith(":"))
                            {
                                a = new atribut();
                                a.val_string = echipmodel[key];
                                a.val_csv = null;
                                a.val_int = null;
                                a.val_nr = null;
                                a.tipID = (from ta in db.tip_atribut where ta.denumire == "Tip Conexiune" select ta.id).FirstOrDefault();
                                db.AddToatributs(a);

                                ea = new echipament_atribute();
                                ea.echipamentID = s.id;
                                ea.atributID = a.id;
                                db.AddToechipament_atribute(ea);

                                db.SaveChanges();
                            }
                            else if (key.StartsWith("destinatienou") && !key.EndsWith(":"))
                            {
                                a = new atribut();
                                a.val_string = echipmodel[key];
                                a.val_csv = null;
                                a.val_int = null;
                                a.val_nr = null;
                                a.tipID = (from ta in db.tip_atribut where ta.denumire == "Destinatie" select ta.id).FirstOrDefault();
                                db.AddToatributs(a);

                                ea = new echipament_atribute();
                                ea.echipamentID = s.id;
                                ea.atributID = a.id;
                                db.AddToechipament_atribute(ea);

                                db.SaveChanges();
                            }
                            else if (key.StartsWith("pozitienou") && !key.EndsWith(":"))
                            {
                                a = new atribut();
                                a.val_string = echipmodel[key];
                                a.val_csv = null;
                                a.val_int = null;
                                a.val_nr = null;
                                a.tipID = (from ta in db.tip_atribut where ta.denumire == "Poz. echip." select ta.id).FirstOrDefault();
                                db.AddToatributs(a);

                                ea = new echipament_atribute();
                                ea.echipamentID = s.id;
                                ea.atributID = a.id;
                                db.AddToechipament_atribute(ea);

                                db.SaveChanges();
                            }
                            else if (key.StartsWith("tiptransportnou") && !key.EndsWith(":"))
                            {
                                a = new atribut();
                                a.val_string = echipmodel[key];
                                a.val_csv = null;
                                a.val_int = null;
                                a.val_nr = null;
                                a.tipID = (from ta in db.tip_atribut where ta.denumire == "Tip echip." select ta.id).FirstOrDefault();
                                db.AddToatributs(a);

                                ea = new echipament_atribute();
                                ea.echipamentID = s.id;
                                ea.atributID = a.id;
                                db.AddToechipament_atribute(ea);

                                db.SaveChanges();
                            }
                        }


                        if (echipmodel["lim"] == "limmain")
                        {
                            a = new atribut();
                            a.val_string = "1";
                            a.val_csv = null;
                            a.val_int = null;
                            a.val_nr = null;
                            a.tipID = (from ta in db.tip_atribut where ta.denumire == "LIM MAIN" select ta.id).FirstOrDefault();
                            db.AddToatributs(a);

                            ea = new echipament_atribute();
                            ea.echipamentID = s.id;
                            ea.atributID = a.id;
                            db.AddToechipament_atribute(ea);
                            db.SaveChanges();
                        }
                        else
                        {
                            a = new atribut();
                            a.val_string = echipmodel["limdist-text"];
                            a.val_csv = null;
                            a.val_int = null;
                            a.val_nr = null;
                            a.tipID = (from ta in db.tip_atribut where ta.denumire == "LIM DISTANT" select ta.id).FirstOrDefault();
                            db.AddToatributs(a);

                            ea = new echipament_atribute();
                            ea.echipamentID = s.id;
                            ea.atributID = a.id;
                            db.AddToechipament_atribute(ea);

                            db.SaveChanges();
                        }

                    }
                    else if (tipechip == "MSED")
                    {
                        s = new echipament();
                        s.denumire = echipmodel["denumire"].ToString();

                        int x = 0;
                        if (int.TryParse(echipmodel["tipechipament"].ToString(), out x) == true)
                            s.tipID = int.Parse(echipmodel["tipechipament"].ToString());
                        else
                            s.tipID = int.Parse(echipmodel["tipechipament"].ToString().Split('_')[0]);


                        s.siteID = long.Parse(echipmodel["siteID"].ToString()); /// ???
                        db.AddToechipaments(s);


                        atribut a = new atribut();
                        a.val_string = echipmodel["locatieremote"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "Locatie remote" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        echipament_atribute ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();



                        a = new atribut();
                        a.val_string = echipmodel["ipces"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "IP CES" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();

                        a = new atribut();
                        a.val_string = echipmodel["ipmanagement"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "IP Management" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();

                        a = new atribut();
                        a.val_string = echipmodel["ipcesremote"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "IP CES Remote" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();

                        a = new atribut();
                        a.val_string = echipmodel["ipmanagementremote"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "IP Management Remote" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();


                        a = new atribut();
                        a.val_string = echipmodel["mask"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "MASK" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();

                        a = new atribut();
                        a.val_string = echipmodel["gateway"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "GATEWAY" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);

                        db.SaveChanges();


                    }
                    else if (tipechip == "MC" || tipechip == "Concentrator" || tipechip == "Router" || tipechip == "Switch")
                    {
                        s = new echipament();
                        s.denumire = echipmodel["denumire"].ToString();

                        int x = 0;
                        if (int.TryParse(echipmodel["tipechipament"].ToString(), out x) == true)
                            s.tipID = int.Parse(echipmodel["tipechipament"].ToString());
                        else
                            s.tipID = int.Parse(echipmodel["tipechipament"].ToString().Split('_')[0]);


                        s.siteID = long.Parse(echipmodel["siteID"].ToString()); /// ???
                        db.AddToechipaments(s);


                        atribut a = new atribut();
                        a.val_string = echipmodel["IP"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "IP" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        echipament_atribute ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);
                        db.SaveChanges();

                        a = new atribut();
                        a.val_string = echipmodel["MASK"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "MASK" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);
                        db.SaveChanges();

                        a = new atribut();
                        a.val_string = echipmodel["GATEWAY"];
                        a.val_csv = null;
                        a.val_int = null;
                        a.val_nr = null;
                        a.tipID = (from ta in db.tip_atribut where ta.denumire == "GATEWAY" select ta.id).FirstOrDefault();
                        db.AddToatributs(a);

                        ea = new echipament_atribute();
                        ea.echipamentID = s.id;
                        ea.atributID = a.id;
                        db.AddToechipament_atribute(ea);
                        db.SaveChanges();

                        if (tipechip == "MC")
                        {
                            a = new atribut();
                            a.val_string = echipmodel["ENDPOINT"];
                            a.val_csv = null;
                            a.val_int = null;
                            a.val_nr = null;
                            a.tipID = (from ta in db.tip_atribut where ta.denumire == "ENDPOINT" select ta.id).FirstOrDefault();
                            db.AddToatributs(a);

                            ea = new echipament_atribute();
                            ea.echipamentID = s.id;
                            ea.atributID = a.id;
                            db.AddToechipament_atribute(ea);
                            db.SaveChanges();
                        }

                        if (tipechip == "Concentrator")
                        {
                            for (int k = 1; k <= 10; k++)
                            {
                                a = new atribut();
                                a.val_string = echipmodel["port" + k.ToString()];
                                a.val_csv = null;
                                a.val_int = null;
                                a.val_nr = null;
                                a.tipID = (from ta in db.tip_atribut where ta.denumire == "Port" select ta.id).FirstOrDefault();
                                db.AddToatributs(a);

                                ea = new echipament_atribute();
                                ea.echipamentID = s.id;
                                ea.atributID = a.id;
                                db.AddToechipament_atribute(ea);
                                db.SaveChanges();
                            }
                        }

                    }
                    else
                        s = new echipament();

                    db.SaveChanges();
                }
                else//exista deja
                {
                    s = db.echipaments.Where(o => o.id == id).FirstOrDefault();
                    int i = 0;
                    foreach (var key in echipmodel.Keys)
                    {
                        if (key.ToString() != "lim" && !key.ToString().StartsWith("nrrutanou") && !key.ToString().StartsWith("pozitienou") && !key.ToString().StartsWith("tiptransportnou") && !key.ToString().StartsWith("tipcnou") && !key.ToString().StartsWith("destinatienou") && !key.ToString().EndsWith("-text") && key.ToString() != "tipcnou" && key.ToString() != "destinatienou" && key.ToString() != "nrrutanou" && key.ToString() != "destinatie" && key.ToString() != "tipconexiune" && key.ToString() != "nrruta" && key.ToString() != "siteID" && key.ToString() != "ipces" && key.ToString() != "licenta" && key.ToString() != "locatieremote" && key.ToString() != "ipmanagement" && key.ToString() != "ipcesremote" && key.ToString() != "ipmanagementremote" && key.ToString() != "mask" && key.ToString() != "gateway" && key.ToString() != "numeechip" && key.ToString() != "ab_analogici_dela" && key.ToString() != "ab_digitali_dela" && key.ToString() != "ab_IP_dela" && key.ToString() != "ab_DECT_dela" && key.ToString() != "ab_total_dela" && key.ToString() != "ab_analogici_panala" && key.ToString() != "ab_digitali_panala" && key.ToString() != "ab_IP_panala" && key.ToString() != "ab_DECT_panala" && key.ToString() != "ab_total_panala" && key.ToString() != "tip_nou" && key.ToString() != "denumire" && key.ToString() != "tip_val" && key.ToString() != "val_nou" && key.ToString() != "id" && !key.ToString().EndsWith(":"))
                        {
                            string idechip_idatr = key.ToString();
                            int nr_split = idechip_idatr.Split('_').Count();
                            long idechip = 0;
                            long idatr = 0;
                            
                                idechip = long.Parse(idechip_idatr.Split('_')[0]);
                                idatr = long.Parse(idechip_idatr.Split('_')[1]);

                            atribut a = db.atributs.Where(o=> o.id == idatr).FirstOrDefault();

                            string valoare = echipmodel[i];


                            string tip = "";
                            try
                            {
                                tip = (from a1 in db.atributs
                                       join at1 in db.tip_atribut on a1.tipID equals at1.id
                                       where a1.id == a.id
                                       select at1.denumire).FirstOrDefault();
                            }
                            catch
                            {
                                // plan de numerotatie

                                idatr = long.Parse(idatr.ToString().Substring(0, idatr.ToString().Length - 1));
                                a = db.atributs.Where(o => o.id == idatr).FirstOrDefault();
                                tip = (from a1 in db.atributs
                                       join at1 in db.tip_atribut on a1.tipID equals at1.id
                                       where a1.id == a.id
                                       select at1.denumire).FirstOrDefault();
                            }

                            string den_centrala = "";
                            if (tip == "Licenta")
                            {
                                den_centrala = (from e in db.echipaments
                                  join ea in db.echipament_atribute on e.id equals ea.echipamentID
                                  join a1 in db.atributs on ea.atributID equals a1.id
                                  join ta in db.tip_atribut on a1.tipID equals ta.id
                                  join et in db.echipament_tip on a1.val_int equals et.id
                                  where ta.denumire == "Tip" && e.id == idechip
                                  select et.denumire).FirstOrDefault(); 
                            }
                            
                            int val_int;
                            decimal val_long;
                            string cheie = echipmodel[key.ToString() + ":"];
                            if (tip.Contains("Abonati"))
                                cheie = echipmodel[key.ToString().Substring(0, key.ToString().Length - 1) + ":"];

                            switch (cheie)
                            {
                                case "S": a.val_string = 
                                    (tip == "Licenta" && (den_centrala == "MD" || den_centrala == "MX-ONE") && key.ToString().EndsWith("licenta1")) ?
                                   echipmodel[key.ToString()] + echipmodel[key.ToString().Replace("licenta1", "licenta2")] :
                                   (key.ToString().EndsWith("licenta2") ? a.val_string :
                                   (tip == "Contract" && echipmodel[key.ToString()] == "Altul" ? echipmodel[key.ToString()+"-text"]  : echipmodel[key.ToString()]));
                                    ;
                                            break;
                                case "I": if (int.TryParse(echipmodel[key.ToString()], out val_int))
                                                a.val_int = int.Parse(echipmodel[key.ToString()]);
                                            else
                                                a.val_int = null; 
                                            break;
                                case "N": if (decimal.TryParse(echipmodel[key.ToString()], out val_long))
                                                a.val_nr = decimal.Parse(echipmodel[key.ToString()]);
                                            else
                                                a.val_nr = null; 
                                            break;
                                case "CSV": a.val_csv = (tip.Contains("Abonati") == true && key.ToString().EndsWith("1") ? echipmodel[key.ToString()] + "," + echipmodel[key.ToString().Substring(0,key.ToString().Length-1) + "2"] :
                                    (tip.Contains("Abonati") == true && key.ToString().EndsWith("2") ? a.val_csv : echipmodel[key.ToString()]));
                                            break;
                                default:    a.val_string = echipmodel[key.ToString()].ToString();
                                            break;
                            }
                            db.SaveChanges();
                        }
                        else if (key.ToString() == "lim")
                        {
                            int val_int;
                            decimal val_long;
                            atribut a2 = (from a in db.atributs
                                         join ea in db.echipament_atribute on a.id equals ea.atributID
                                         join e in db.echipaments on ea.echipamentID equals e.id
                                         join ta in db.tip_atribut on a.tipID equals ta.id
                                         where e.id == id
                                         && ta.denumire == "LIM MAIN"
                                         select a).FirstOrDefault();
                            atribut a1 = (from a in db.atributs
                                         join ea in db.echipament_atribute on a.id equals ea.atributID
                                         join e in db.echipaments on ea.echipamentID equals e.id
                                         join ta in db.tip_atribut on a.tipID equals ta.id
                                         where e.id == id
                                         && ta.denumire == "LIM DISTANT"
                                         select a).FirstOrDefault();

                            if (echipmodel["lim"] == "limmain")
                            {
                                try
                                {
                                    if (a2 == null)
                                    {
                                        a2 = new atribut();
                                        a2.tipID = (from ta in db.tip_atribut where ta.denumire == "LIM MAIN" select ta.id).FirstOrDefault();
                                        a2.val_string = "1";
                                        a2.val_csv = null;
                                        a2.val_int = null;
                                        a2.val_nr = null;

                                        echipament_atribute ea = new echipament_atribute();
                                        ea.atributID = a2.id;
                                        ea.echipamentID = id;

                                        db.AddToatributs(a2);
                                        db.AddToechipament_atribute(ea);
                                    }
                                    else
                                    {
                                        a2.val_string = "1";
                                        a2.val_csv = null;
                                        a2.val_int = null;
                                        a2.val_nr = null;

                                    }
                                    db.SaveChanges();
                                }
                                catch { }

                                try
                                {
                                    if (a1 != null)
                                    {
                                        a1.val_string = null;
                                        a1.val_csv = null;
                                        a1.val_int = null;
                                        a1.val_nr = null;
                                        db.SaveChanges();
                                    }
                                }
                                catch { }

                            }
                            else
                            {
                                try
                                {
                                    if (a2 != null)
                                    {

                                        a2.val_string = "0";
                                        a2.val_csv = null;
                                        a2.val_int = null;
                                        a2.val_nr = null;
                                        db.SaveChanges();
                                    }
                                }
                                catch { }

                                try
                                {
                                    if (a1 == null)
                                    {
                                        a1 = new atribut();
                                        a1.tipID = (from ta in db.tip_atribut where ta.denumire == "LIM DISTANT" select ta.id).FirstOrDefault();

                                        a1.val_string = echipmodel["limdist-text"];
                                        a1.val_csv = null;
                                        a1.val_int = null;
                                        a1.val_nr = null;

                                        echipament_atribute ea = new echipament_atribute();
                                        ea.atributID = a1.id;
                                        ea.echipamentID = id;

                                        db.AddToatributs(a1);
                                        db.AddToechipament_atribute(ea);
                                    }
                                    else
                                    {
                                        a1.val_string = echipmodel["limdist-text"];
                                        a1.val_csv = null;
                                        a1.val_int = null;
                                        a1.val_nr = null;
                                    }

                                    db.SaveChanges();
                                }
                                catch{}
                            }
                        }
                        else if (key.ToString().StartsWith("nrrutanou") && !key.ToString().EndsWith(":"))
                        {
                            int val_int;
                            decimal val_long;
                            atribut a = new atribut();
                            var taid = (from ta in db.tip_atribut where ta.denumire == "Numar ruta" select new { ta.id, ta.tip_valoare }).FirstOrDefault();
                            a.tipID = taid.id;
                            a.val_string = echipmodel[key.ToString()];
                            a.val_csv = null;
                            a.val_int = null;
                            a.val_nr = null;

                            echipament_atribute ea = new echipament_atribute();
                            ea.atributID = a.id;
                            ea.echipamentID = id;

                            db.AddToatributs(a);
                            db.AddToechipament_atribute(ea);
                            db.SaveChanges();
                            //



                        }
                        else if (key.ToString().StartsWith("tipcnou") && !key.ToString().EndsWith(":"))
                        {
                            int val_int;
                            decimal val_long;
                            atribut a = new atribut();
                            var taid = (from ta in db.tip_atribut where ta.denumire == "Tip Conexiune" select new { ta.id, ta.tip_valoare }).FirstOrDefault();
                            a.tipID = taid.id;
                            a.val_string = echipmodel[key.ToString()];
                            a.val_csv = null;
                            a.val_int = null;
                            a.val_nr = null;

                            echipament_atribute ea = new echipament_atribute();
                            ea.atributID = a.id;
                            ea.echipamentID = id;

                            db.AddToatributs(a);
                            db.AddToechipament_atribute(ea);
                            db.SaveChanges();
                        }
                        else if (key.ToString().StartsWith("destinatienou") && !key.ToString().EndsWith(":"))
                        {
                            //
                            int val_int;
                            decimal val_long;
                            atribut a = new atribut();
                            var taid = (from ta in db.tip_atribut where ta.denumire == "Destinatie" select new { ta.id, ta.tip_valoare }).FirstOrDefault();
                            a.tipID = taid.id;
                            a.val_string = echipmodel[key.ToString()];
                            a.val_csv = null;
                            a.val_int = null;
                            a.val_nr = null;

                            echipament_atribute ea = new echipament_atribute();
                            ea.atributID = a.id;
                            ea.echipamentID = id;

                            db.AddToatributs(a);
                            db.AddToechipament_atribute(ea);
                            db.SaveChanges();
                        }
                        else if (key.ToString().StartsWith("pozitienou") && !key.ToString().EndsWith(":"))
                        {
                            int val_int;
                            decimal val_long;
                            atribut a = new atribut();
                            var taid = (from ta in db.tip_atribut where ta.denumire == "Poz. echip." select new { ta.id, ta.tip_valoare }).FirstOrDefault();
                            a.tipID = taid.id;
                            a.val_string = echipmodel[key.ToString()];
                            a.val_csv = null;
                            a.val_int = null;
                            a.val_nr = null;

                            echipament_atribute ea = new echipament_atribute();
                            ea.atributID = a.id;
                            ea.echipamentID = id;

                            db.AddToatributs(a);
                            db.AddToechipament_atribute(ea);
                            db.SaveChanges();
                            //



                        }
                        else if (key.ToString().StartsWith("tiptransportnou") && !key.ToString().EndsWith(":"))
                        {
                            int val_int;
                            decimal val_long;
                            atribut a = new atribut();
                            var taid = (from ta in db.tip_atribut where ta.denumire == "Tip echip." select new { ta.id, ta.tip_valoare }).FirstOrDefault();
                            a.tipID = taid.id;
                            a.val_string = echipmodel[key.ToString()];
                            a.val_csv = null;
                            a.val_int = null;
                            a.val_nr = null;

                            echipament_atribute ea = new echipament_atribute();
                            ea.atributID = a.id;
                            ea.echipamentID = id;

                            db.AddToatributs(a);
                            db.AddToechipament_atribute(ea);
                            db.SaveChanges();
                            //



                        }
                        else if (key.ToString() == "tip_nou" && echipmodel["tip_nou"].ToString() == "Conexiune")
                        {
                            int val_int;
                            decimal val_long;
                            atribut a = new atribut();
                            var taid = (from ta in db.tip_atribut where ta.denumire == "Numar ruta" select new { ta.id, ta.tip_valoare }).FirstOrDefault();
                            a.tipID = taid.id;
                            a.val_string = echipmodel["nrrutanou"];
                            a.val_csv = null;
                            a.val_int = null;
                            a.val_nr = null;

                            echipament_atribute ea = new echipament_atribute();
                            ea.atributID = a.id;
                            ea.echipamentID = id;

                            db.AddToatributs(a);
                            db.AddToechipament_atribute(ea);
                            db.SaveChanges();
                            //
                            a = new atribut();
                            taid = (from ta in db.tip_atribut where ta.denumire == "Tip Conexiune" select new { ta.id, ta.tip_valoare }).FirstOrDefault();
                            a.tipID = taid.id;
                            a.val_string = echipmodel["tipcnou"];
                            a.val_csv = null;
                            a.val_int = null;
                            a.val_nr = null;

                            ea = new echipament_atribute();
                            ea.atributID = a.id;
                            ea.echipamentID = id;

                            db.AddToatributs(a);
                            db.AddToechipament_atribute(ea);
                            db.SaveChanges();

                            //
                            a = new atribut();
                            taid = (from ta in db.tip_atribut where ta.denumire == "Destinatie" select new { ta.id, ta.tip_valoare }).FirstOrDefault();
                            a.tipID = taid.id;
                            a.val_string = echipmodel["destinatienou"];
                            a.val_csv = null;
                            a.val_int = null;
                            a.val_nr = null;

                            ea = new echipament_atribute();
                            ea.atributID = a.id;
                            ea.echipamentID = id;

                            db.AddToatributs(a);
                            db.AddToechipament_atribute(ea);
                            db.SaveChanges();
                        }
                        else if (key.ToString() == "tip_nou" && echipmodel["val_nou"].ToString() != "")
                        {
                            int val_int;
                            decimal val_long;
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
                                    if (int.TryParse(echipmodel["val_nou"].ToString(), out val_int))
                                        a.val_int = int.Parse(echipmodel["val_nou"]);
                                    else
                                        a.val_int = null;
                                    a.val_nr = null;
                                    break;
                                case "N":
                                    a.val_string = null;
                                    a.val_csv = null;
                                    a.val_int = null;
                                    if (decimal.TryParse(echipmodel["val_nou"].ToString(), out val_long))
                                        a.val_nr = decimal.Parse(echipmodel["val_nou"]);
                                    else
                                        a.val_nr = null;
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

        public string GetVersiuni(long? idver, string tip_ales = "")
        {
            string tipcartele = "";
            if (tip_ales != "" && (tip_ales == "1" || tip_ales=="2"))
            {
                int tip_a = Int32.Parse(tip_ales);
                var tipp = (from te in db.echipament_tip where te.id == tip_a select te.denumire).FirstOrDefault();
                var tip = (from tc in db.echipament_versiuni where tc.echipament == tipp select new { tc.id, tc.denumire }).ToList();
                foreach (var s in tip)
                {
                    tipcartele += s.id + ":" + s.denumire + ",";
                }
                if (idver != null)
                    tipcartele += idver.ToString();
                else
                    tipcartele = tipcartele.TrimEnd(',');

            }
            else
            {
                var tip = (from tc in db.echipament_versiuni select new { tc.id, tc.denumire }).ToList();
                foreach (var s in tip)
                {
                    tipcartele += s.id + ":" + s.denumire + ",";
                }
                if (idver != null)
                    tipcartele += idver.ToString();
                else
                    tipcartele = tipcartele.TrimEnd(',');

            }
            
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
            var namejudet = (from j in db.judetes join s in db.sites on j.id equals s.judetID join e in db.echipaments on s.id equals e.siteID where e.id == idechipament select j.denumire).FirstOrDefault();
            var listaindex = (from e in db.echipaments
                              join s in db.sites on e.siteID equals s.id
                              join j in db.judetes on s.judetID equals j.id
                              where e.id == idechipament
                              select new { jid = j.id, sid = s.id }).FirstOrDefault();
           // long idcentrala = (from c in db.cartelas where c.id == idcartela select c.echipamentID).FirstOrDefault();
            long tip_id = (from e in db.echipaments where e.id == idechipament select e.tipID).FirstOrDefault();

            var denumire = (from t in db.tip_echipament where t.id == tip_id select t.denumire).FirstOrDefault();


            if (denumire == "PABX")
            {

                var listacartele = (from c in db.cartelas where c.echipamentID == idechipament select c.id).ToList();

                foreach (long idcartela in listacartele.ToList())
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


                    foreach (var x in c_a.ToList())
                    {
                        cartela_atribute y = (from a in db.cartela_atribute where a.id == x.id select a).FirstOrDefault();
                        db.cartela_atribute.DeleteObject(y);
                    }
                    db.SaveChanges();


                    foreach (var x in c_a)
                    {
                        atribut y = (from a in db.atributs where a.id == x.atributID select a).FirstOrDefault();
                        db.atributs.DeleteObject(y);
                    }

                    db.SaveChanges();

                    db.cartelas.DeleteObject(car);
                    db.SaveChanges();

                }
            }


                var listaatr = (from e in db.echipament_atribute where e.echipamentID == idechipament select new { e.id, e.atributID }).ToList();


                foreach (var x in listaatr)
                {
                    echipament_atribute y = (from a in db.echipament_atribute where a.id == x.id select a).FirstOrDefault();
                    db.echipament_atribute.DeleteObject(y);
                    db.SaveChanges();
                }

                foreach (var x in listaatr)
                {
                    atribut y = (from a in db.atributs where a.id == x.atributID select a).FirstOrDefault();
                    db.atributs.DeleteObject(y);
                    db.SaveChanges();
                }


               


             
                    echipament z = (from a in db.echipaments where a.id == idechipament select a).FirstOrDefault();
                    db.echipaments.DeleteObject(z);
                     db.SaveChanges();

                     //}

                

                
                return namejudet +"," + tip_id.ToString() + "_tip_echipament_" + listaindex.sid.ToString() + "_" + listaindex.jid.ToString();

            //return RedirectToAction("Index", "Judete", new { name = judet.denumire.ToString(), id = idcentrala.ToString() + "_echipament" });
        }
    }
}
