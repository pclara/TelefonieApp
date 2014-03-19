using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TreeViewDemo.Models;

namespace TreeViewDemo.Controllers
{
    public class CartelaController : Controller
    {
        //
        // GET: /Cartela/
        OrganizationEntities db = new OrganizationEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditCartela(long? id, string parentID = null)
        {
            ViewBag.id = id;

            if (id == -1)
            {
                long x;
                if (long.TryParse(parentID, out x) == false)
                    x = long.Parse(parentID.Split('_')[0]);
                ViewBag.parentEchipament = x;
            }
            else
            {
                string tipcartela = (from tc in db.tip_cartela join c in db.cartelas on tc.id equals c.tipID select tc.denumire).FirstOrDefault();
                ViewBag.tipcartela = tipcartela;

                var atribute = (from a in db.atributs
                                join ea in db.cartela_atribute on a.id equals ea.atributID
                                join e in db.cartelas on ea.cartelaID equals e.id
                                join ta in db.tip_atribut on a.tipID equals ta.id

                                where ea.cartelaID == id
                                select new { a.id, ta.tip_valoare, a.val_csv, a.val_int, a.val_nr, a.val_string, tipatrden = ta.denumire }).ToList();

                List<object> lista_atr = new List<object>();
                foreach (var atr in atribute)
                {
                    switch (atr.tip_valoare)
                    {
                        case "S": lista_atr.Add(new { id_atr = atr.id, nume_atr = atr.tipatrden, tip_val = atr.tip_valoare, val_atr = atr.val_string });
                            break;
                        case "I": lista_atr.Add(new { id_atr = atr.id, nume_atr = atr.tipatrden, tip_val = atr.tip_valoare, val_atr = atr.val_int });
                            break;
                        case "N": lista_atr.Add(new { id_atr = atr.id, nume_atr = atr.tipatrden, tip_val = atr.tip_valoare, val_atr = atr.val_nr });
                            break;
                        case "CSV": lista_atr.Add(new { id_atr = atr.id, nume_atr = atr.tipatrden, tip_val = atr.tip_valoare, val_atr = atr.val_csv });
                            break;
                        default: lista_atr.Add(new { id_atr = atr.id, nume_atr = atr.tipatrden, tip_val = atr.tip_valoare, val_atr = atr.val_string });
                            break;
                    }
                }
                JavaScriptSerializer sr = new JavaScriptSerializer();
                string atr1 = sr.Serialize(lista_atr);
                ViewBag.atribute = atr1;
                ViewBag.id = id;
                ViewBag.parentEchipament = long.Parse(parentID.Split('_')[0]);
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult Edit(FormCollection cartelamodel)
        {
            if (ModelState.IsValid)
            {
                cartela s;
                long id = long.Parse(cartelamodel["id"]);
                string tipcartela = cartelamodel["tipcartela"];
                long parentEchip = long.Parse(cartelamodel["parentE"]);
                if (id == -1)
                {
                    s = new cartela();
                    s.tipID = (from tc in db.tip_cartela where tc.denumire == tipcartela select tc.id).FirstOrDefault();
                    s.echipamentID = parentEchip;
                    db.AddTocartelas(s);
                    db.SaveChanges();

                    switch (tipcartela)
                    {
                        case "ESU":
                        case "ASU":
                        case "IPLU":
                        case "NIU":
                        case "AAU2":
                            {
                                atribut a = new atribut();
                                a.val_string = cartelamodel["bpos"];
                                a.val_csv = null;
                                a.val_int = null;
                                a.val_nr = null;
                                a.tipID = (from ta in db.tip_atribut where ta.denumire == "BPOS" select ta.id).FirstOrDefault();
                                db.AddToatributs(a);

                                cartela_atribute ea = new cartela_atribute();
                                ea.cartelaID = s.id;
                                ea.atributID = a.id;
                                db.AddTocartela_atribute(ea);

                                db.SaveChanges();

                                a = new atribut();
                                a.val_string = cartelamodel["ip"];
                                a.val_csv = null;
                                a.val_int = null;
                                a.val_nr = null;
                                a.tipID = (from ta in db.tip_atribut where ta.denumire == "IP" select ta.id).FirstOrDefault();
                                db.AddToatributs(a);

                                ea = new cartela_atribute();
                                ea.cartelaID = s.id;
                                ea.atributID = a.id;
                                db.AddTocartela_atribute(ea);

                                db.SaveChanges();

                                a = new atribut();
                                a.val_string = cartelamodel["mask"];
                                a.val_csv = null;
                                a.val_int = null;
                                a.val_nr = null;
                                a.tipID = (from ta in db.tip_atribut where ta.denumire == "MASK" select ta.id).FirstOrDefault();
                                db.AddToatributs(a);

                                ea = new cartela_atribute();
                                ea.cartelaID = s.id;
                                ea.atributID = a.id;
                                db.AddTocartela_atribute(ea);

                                db.SaveChanges();

                                a = new atribut();
                                a.val_string = cartelamodel["gateway"];
                                a.val_csv = null;
                                a.val_int = null;
                                a.val_nr = null;
                                a.tipID = (from ta in db.tip_atribut where ta.denumire == "GATEWAY" select ta.id).FirstOrDefault();
                                db.AddToatributs(a);

                                ea = new cartela_atribute();
                                ea.cartelaID = s.id;
                                ea.atributID = a.id;
                                db.AddTocartela_atribute(ea);

                                db.SaveChanges();
                                break;
                            }
                        default:
                            {
                                atribut a = new atribut();
                                a.val_string = cartelamodel["bpos"];
                                a.val_csv = null;
                                a.val_int = null;
                                a.val_nr = null;
                                a.tipID = (from ta in db.tip_atribut where ta.denumire == "BPOS" select ta.id).FirstOrDefault();
                                db.AddToatributs(a);

                                cartela_atribute ea = new cartela_atribute();
                                ea.cartelaID = s.id;
                                ea.atributID = a.id;
                                db.AddTocartela_atribute(ea);

                                db.SaveChanges();
                                break;
                            }
                    }


                    db.SaveChanges();


                }
                else
                {
                    s = db.cartelas.Where(o => o.id == id).FirstOrDefault();
                    int i = 0;
                    foreach (var key in cartelamodel.Keys)
                    {
                        if (key.ToString() != "val_nou" && key.ToString() != "tip_nou" && key.ToString() != "tipcartela" && key.ToString() != "denumire" && key.ToString() != "parentE" && key.ToString() != "ip" && key.ToString() != "id" && key.ToString() != "mask" && key.ToString() != "gateway" && key.ToString() != "bpos" && !key.ToString().EndsWith(":"))
                        {
                            string idechip_idatr = key.ToString();
                            long idechip = long.Parse(idechip_idatr.Split('_')[0]);
                            long idatr = long.Parse(idechip_idatr.Split('_')[1]);

                            atribut a = db.atributs.Where(o => o.id == idatr).FirstOrDefault();

                            string valoare = cartelamodel[i];
                            switch (cartelamodel[key.ToString() + ":"])
                            {
                                case "S": a.val_string = cartelamodel[key.ToString()];
                                    break;
                                case "I": a.val_int = int.Parse(cartelamodel[key.ToString()]);
                                    break;
                                case "N": a.val_nr = decimal.Parse(cartelamodel[key.ToString()]);
                                    break;
                                case "CSV": a.val_csv = cartelamodel[key.ToString()];
                                    break;
                                default: a.val_string = cartelamodel[key.ToString()].ToString();
                                    break;
                            }
                            db.SaveChanges();
                        }
                        else if (key.ToString() == "mask" || key.ToString() == "ip" || key.ToString() == "bpos" || key.ToString() == "gateway" )
                        {
                            atribut a = new atribut();
                            string tip_nou = cartelamodel[key.ToString()];
                            var taid = (from ta in db.tip_atribut where ta.denumire == tip_nou select new { ta.id, ta.tip_valoare }).FirstOrDefault();
                            a.tipID = taid.id;
                            switch (taid.tip_valoare)
                            {
                                case "S":
                                    a.val_string = cartelamodel[key.ToString()];
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = null;
                                    break;
                                case "I":
                                    a.val_string = null;
                                    a.val_csv = null;
                                    a.val_int = int.Parse(cartelamodel[key.ToString()].ToString());
                                    a.val_nr = null;
                                    break;
                                case "N":
                                    a.val_string = null;
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = decimal.Parse(cartelamodel[key.ToString()].ToString());
                                    break;
                                case "CSV":
                                    a.val_string = null;
                                    a.val_csv = cartelamodel[key.ToString()];
                                    a.val_int = null;
                                    a.val_nr = null;
                                    break;
                                default:
                                    a.val_string = cartelamodel[key.ToString()];
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = null;
                                    break;
                            }

                            cartela_atribute ea = new cartela_atribute();
                            ea.atributID = a.id;
                            ea.cartelaID = id;

                            db.AddToatributs(a);
                            db.AddTocartela_atribute(ea);
                            db.SaveChanges();
                        }
                        else if (key.ToString() == "tip_nou")
                        {
                            atribut a = new atribut();
                            string tip_nou = cartelamodel["tip_nou"];
                            var taid = (from ta in db.tip_atribut where ta.denumire == tip_nou select new { ta.id, ta.tip_valoare }).FirstOrDefault();
                            a.tipID = taid.id;
                            switch (taid.tip_valoare)
                            {
                                case "S":
                                    a.val_string = cartelamodel["val_nou"];
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = null;
                                    break;
                                case "I":
                                    a.val_string = null;
                                    a.val_csv = null;
                                    a.val_int = int.Parse(cartelamodel["val_nou"].ToString());
                                    a.val_nr = null;
                                    break;
                                case "N":
                                    a.val_string = null;
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = decimal.Parse(cartelamodel["val_nou"].ToString());
                                    break;
                                case "CSV":
                                    a.val_string = null;
                                    a.val_csv = cartelamodel["val_nou"];
                                    a.val_int = null;
                                    a.val_nr = null;
                                    break;
                                default:
                                    a.val_string = cartelamodel["val_nou"];
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = null;
                                    break;
                            }

                            cartela_atribute ea = new cartela_atribute();
                            ea.atributID = a.id;
                            ea.cartelaID = id;

                            db.AddToatributs(a);
                            db.AddTocartela_atribute(ea);
                            db.SaveChanges();
                        }
                    }
                }

                string namejudet = (from j in db.judetes join s1 in db.sites on j.id equals s1.judetID 
                                    join e in db.echipaments on s1.id equals e.siteID
                                    join c in db.cartelas on e.id equals c.echipamentID
                                    where c.id == s.id select j.denumire).FirstOrDefault();

                return RedirectToAction("Index", "Judete", new { name = namejudet, id = s.id.ToString() + "_cartela" });
            }
            return RedirectToAction("Index", "Home");
        }


        public string GetTipCartele()
        {
            string tipcartele = "";
            var tip = (from tc in db.tip_cartela select tc.denumire).ToList();
            foreach (string s in tip)
            {
                tipcartele += s + ",";
            }
            tipcartele = tipcartele.TrimEnd(',');
            return tipcartele;
        }

        public string CaleNod(string id)
        {
            string cale = "";
            long idcartela = long.Parse(id.Split('_')[0]);
            var nume = (from j in db.judetes
                        join s1 in db.sites on j.id equals s1.judetID
                        join e in db.echipaments on s1.id equals e.siteID
                        join c in db.cartelas on e.id equals c.echipamentID
                        join te in db.tip_echipament on e.tipID equals te.id
                        where c.id == idcartela
                        select new { judet = j.id, site = s1.id, tipechip = te.id , parinteE = c.echipamentID, id = c.id}).FirstOrDefault();

            cale = nume.judet + "_judet," + nume.site + "_site," + nume.tipechip + "_tip_echipament_" + nume.judet+"_"+nume.site+","+nume.parinteE+"_echipament,"+nume.id+"_cartela";
            return cale;                        
        }
    }
}
