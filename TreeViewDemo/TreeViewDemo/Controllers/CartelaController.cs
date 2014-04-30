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
            ViewBag.id = long.Parse(parentID.Split('_')[0]);

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
                ViewBag.id = long.Parse(parentID.Split('_')[0]);
                ViewBag.parentEchipament = long.Parse(parentID.Split('_')[0]);
            }
            return PartialView();
        }


        public ActionResult AfiseazaCartele(string id, string parentID)
        {
            long x;
            if (long.TryParse(parentID.Split('_')[0], out x))
            {
                ViewBag.id = x;
            }
            else
            {
                x = long.Parse(parentID);
                ViewBag.id = parentID;
            }

            List<object> cart = new List<object>();
            var cartele = (from e in db.echipaments join c in db.cartelas on e.id equals c.echipamentID where e.id == x select c.id).ToList();

            foreach (long idcartela in cartele)
            {
                var atribute = (from a in db.atributs
                                join ea in db.cartela_atribute on a.id equals ea.atributID
                                join e in db.cartelas on ea.cartelaID equals e.id
                                join ta in db.tip_atribut on a.tipID equals ta.id
                                where ea.cartelaID == idcartela
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
                //lista_atr.Add(new { id_atr = atr.id, nume_atr = atr.tipatrden, tip_val = atr.tip_valoare, val_atr = atr.val_csv });
                string tip_cartela = (from tc in db.tip_cartela join c in db.cartelas on tc.id equals c.tipID where c.id == idcartela select tc.denumire).FirstOrDefault();
                cart.Add(new { id = idcartela, tip = tip_cartela, atribute = lista_atr });
            }

            JavaScriptSerializer sr = new JavaScriptSerializer();
            string atr1 = sr.Serialize(cart);
            ViewBag.cartele = atr1;
            
            
            return PartialView();
        }

        [HttpPost]
        public ActionResult Salvare(FormCollection cartelamodel)
        {
            long echipamentID = long.Parse(cartelamodel["id"].ToString());
            if (ModelState.IsValid)
            {
                int randuri = int.Parse(cartelamodel["randuri"].ToString());
                for (int i = 3; i <= randuri; i++)
                {
                    string id = cartelamodel["id_"+i.ToString()];
                    long idcartela = long.Parse(id);
                    if (idcartela == -1)
                    {
                        string tipcartela = cartelamodel["tip_cartela_"+i.ToString()];
                        cartela s = new cartela();
                        s.tipID = (from tc in db.tip_cartela where tc.denumire == tipcartela select tc.id).FirstOrDefault();
                        s.echipamentID = long.Parse(cartelamodel["id"].ToString());
                        db.AddTocartelas(s);
                        db.SaveChanges();

                         switch (tipcartela)
                    {
                        case "ESU":
                        case "ASU":
                        case "IPLU":
                        case "NIU":
                        case "AAU2":
                        case "Management alimentare":
                            {
                                atribut a = new atribut();
                                a.val_string = cartelamodel["bpos_"+i.ToString()];
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
                                a.val_string = cartelamodel["ip_" + i.ToString()];
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
                                a.val_string = cartelamodel["mask_" + i.ToString()];
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
                                a.val_string = cartelamodel["gateway_" + i.ToString()];
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

                                a = new atribut();
                                a.val_string = cartelamodel["switch_" + i.ToString()];
                                a.val_csv = null;
                                a.val_int = null;
                                a.val_nr = null;
                                a.tipID = (from ta in db.tip_atribut where ta.denumire == "Port switch" select ta.id).FirstOrDefault();
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
                                a.val_string = cartelamodel["bpos_" + i.ToString()];
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

                        foreach (var key in cartelamodel.Keys)
                        {
                            if (!key.ToString().StartsWith("switch_") && !key.ToString().StartsWith("gateway_") && !key.ToString().StartsWith("mask_") && !key.ToString().StartsWith("ip_") && !key.ToString().StartsWith("bpos_") && !key.ToString().StartsWith("id_") && !key.ToString().StartsWith("tip_") && key.ToString() != "val_nou" && key.ToString() != "randuri" && key.ToString() != "tip_nou" && key.ToString() != "tipcartela" && key.ToString() != "denumire" && key.ToString() != "parentE" && key.ToString() != "ip" && key.ToString() != "id" && key.ToString() != "mask" && key.ToString() != "gateway" && key.ToString() != "bpos" && !key.ToString().EndsWith(":"))
                            {

                                string idechip_idatr = key.ToString();
                                long idechip = long.Parse(idechip_idatr.Split('_')[0]);
                                long idatr = long.Parse(idechip_idatr.Split('_')[1]);
                                if (idechip == idcartela)
                                {
                                    atribut a = db.atributs.Where(o => o.id == idatr).FirstOrDefault();
                                    string den_tip_atr = (from ta in db.tip_atribut where ta.id == a.tipID select ta.tip_valoare).FirstOrDefault();
                                    //string valoare = cartelamodel[i];
                                    switch (den_tip_atr)
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
                            }
                            else if (key.ToString().StartsWith("tip_") && !key.ToString().StartsWith("tip_cartela") )
                            {
                                int tipnr = int.Parse(key.ToString().Split('_')[1]);
                                if (int.Parse(cartelamodel["id_" + tipnr.ToString()].ToString()) == idcartela)
                                {
                                    cartela s = (from c in db.cartelas where c.id == idcartela select c).FirstOrDefault();
                                    string tipcartela = cartelamodel[key.ToString()];
                                    s.tipID = (from tc in db.tip_cartela where tc.denumire == tipcartela select tc.id).FirstOrDefault();
                                    db.SaveChanges();
                                }
                            }
                            /*else
                                if (key.ToString().StartsWith("bpos_"))
                                {

                                    int row = int.Parse(key.ToString().Split('_')[1]);
                                    atribut a = new atribut();
                                    a.val_string = cartelamodel["bpos_" + row];
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = null;
                                    a.tipID = (from ta in db.tip_atribut where ta.denumire == "BPOS" select ta.id).FirstOrDefault();
                                    db.AddToatributs(a);

                                    cartela_atribute ea = new cartela_atribute();
                                    ea.cartelaID = idcartela;
                                    ea.atributID = a.id;
                                    db.AddTocartela_atribute(ea);

                                    db.SaveChanges();

                                    a = new atribut();
                                    a.val_string = cartelamodel["ip_" + row];
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = null;
                                    a.tipID = (from ta in db.tip_atribut where ta.denumire == "IP" select ta.id).FirstOrDefault();
                                    db.AddToatributs(a);

                                    ea = new cartela_atribute();
                                    ea.cartelaID = idcartela;
                                    ea.atributID = a.id;
                                    db.AddTocartela_atribute(ea);

                                    db.SaveChanges();

                                    a = new atribut();
                                    a.val_string = cartelamodel["mask_" + row];
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = null;
                                    a.tipID = (from ta in db.tip_atribut where ta.denumire == "MASK" select ta.id).FirstOrDefault();
                                    db.AddToatributs(a);

                                    ea = new cartela_atribute();
                                    ea.cartelaID = idcartela;
                                    ea.atributID = a.id;
                                    db.AddTocartela_atribute(ea);

                                    db.SaveChanges();

                                    a = new atribut();
                                    a.val_string = cartelamodel["gateway_" + row];
                                    a.val_csv = null;
                                    a.val_int = null;
                                    a.val_nr = null;
                                    a.tipID = (from ta in db.tip_atribut where ta.denumire == "GATEWAY" select ta.id).FirstOrDefault();
                                    db.AddToatributs(a);

                                    ea = new cartela_atribute();
                                    ea.cartelaID = idcartela;
                                    ea.atributID = a.id;
                                    db.AddTocartela_atribute(ea);

                                    db.SaveChanges();
                                }
                             * */
                        }
                    }
                }
            }
            string namejudet = (from j in db.judetes
                                join s1 in db.sites on j.id equals s1.judetID
                                join e in db.echipaments on s1.id equals e.siteID
                                where e.id == echipamentID
                                select j.denumire).FirstOrDefault();
            return RedirectToAction("Index", "Judete", new { name = namejudet, id = echipamentID.ToString() + "_cartele" });
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
                        case "Management alimentare":
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
                            string tip_nou = key.ToString().ToUpper();// cartelamodel[key.ToString()];
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

        [HttpPost]
        public string GetTipCartele(long? i = 0)
        {
            string tipcartele = "";
            var tip = (from tc in db.tip_cartela select tc.denumire).ToList();
            foreach (string s in tip)
            {
                tipcartele += s + ",";
            }
            tipcartele = tipcartele.TrimEnd(',');
            
            if(i!=-1)
                tipcartele = i.ToString() + "," + tipcartele;

            return tipcartele;
        }

        public string CaleNod(string id)
        {
            string cale = "";
            long idcartela = long.Parse(id.Split('_')[0]);
            var nume = (from j in db.judetes
                        join s1 in db.sites on j.id equals s1.judetID
                        join e in db.echipaments on s1.id equals e.siteID
                        
                        join te in db.tip_echipament on e.tipID equals te.id
                        where e.id == idcartela
                        select new { judet = j.id, site = s1.id, tipechip = te.id }).FirstOrDefault();

            cale = nume.judet + "_judet," + nume.site + "_site," + nume.tipechip + "_tip_echipament_" + nume.site + "_" + nume.judet + "," + idcartela + "_echipament," + idcartela + "_cartele";
            return cale;                        
        }

        
        [HttpPost]
        public string StergeCartela(string id)
        {
            long idcartela = long.Parse(id.Split('_')[0]);

            long idcentrala = (from c in db.cartelas where c.id == idcartela select c.echipamentID).FirstOrDefault();

            var judet = (from j in db.judetes
                         join s in db.sites on j.id equals s.judetID
                         join e in db.echipaments on s.id equals e.siteID
                         join c in db.cartelas
                             on e.id equals c.echipamentID
                         where c.id == idcartela
                         select new { j.id, j.denumire }).FirstOrDefault();

            cartela car = (from c in db.cartelas where c.id == idcartela select c).FirstOrDefault();
            var c_a = (from ca in db.cartela_atribute where ca.cartelaID == idcartela select new {ca.id, ca.atributID}).ToList();
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


            return idcentrala.ToString() + "_echipament";

            //return RedirectToAction("Index", "Judete", new { name = judet.denumire.ToString(), id = idcentrala.ToString() + "_echipament" });
        }
         
    }
}
