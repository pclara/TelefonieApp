using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using TreeViewDemo.Models;
using System.Data.Objects.SqlClient;

namespace TreeViewDemo.Controllers
{
    public class LocatiiController : Controller
    {
        //
        // GET: /Locatoo/
        OrganizationEntities db = new OrganizationEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditLocatii(long? id, string parentID = null)
        {
            Site judete1 = new Site();


            if (id == -1)
            {
                long? judetid = long.Parse(parentID.Split('_')[0]);
                var jud = (from j in db.judetes select new { j.id, j.denumire }).ToList();
                foreach (var x in jud)
                    judete1.lista.Add(new TreeViewDemo.Models.Site.Judet(x.id, x.denumire));

                var namesite = "";
                judete1.denumire = namesite.ToString();
                judete1.id = id;
                judete1.parinteJudet = judetid;
            }
            else
            {
                long? judetid = long.Parse(parentID.Split('_')[0]);
                var jud = (from j in db.judetes select new { j.id, j.denumire }).ToList();
                foreach (var x in jud)
                    judete1.lista.Add(new TreeViewDemo.Models.Site.Judet(x.id, x.denumire));

                var namesite = (from j in db.sites where j.id == id select j.denumire).FirstOrDefault();
                judete1.denumire = namesite.ToString();
                judete1.id = id;
                judete1.parinteJudet = judetid;
            }
            return PartialView(judete1);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection sitemodel)
        {
            if (ModelState.IsValid)
            {
                site s ;
                    long? id = long.Parse(sitemodel["id"]);
                    if (id == -1)
                    {
                        s = new site();
                        s.denumire = sitemodel["denumire"].ToString();
                        s.judetID = int.Parse(sitemodel["judet"]);
                        db.AddTosites(s);
                        db.SaveChanges();
                    }
                    else
                    {
                        s = db.sites.Where(o => o.id == id).FirstOrDefault();
                        s.denumire = sitemodel["denumire"].ToString();
                        db.SaveChanges();
                    }
                var namejudet = (from j in db.judetes where j.id == s.judetID select j.denumire).FirstOrDefault();
                return RedirectToAction("Index", "Judete", new { name = namejudet, id = s.id.ToString() + "_site" });
            }
            else
            {
                return Json("Salvarea nu a fost efectuata!");
            }
        }

        public string CaleNod(long id)
        {
            string cale = "";
            long judetID = (from j in db.judetes join s in db.sites on j.id equals s.judetID where s.id == id select j.id).FirstOrDefault();
            cale = judetID.ToString() + "_judet"+ "," + id.ToString() +"_site";
            return cale;
        }

        [HttpPost]
        public ActionResult StergeLocatie(string id)
        {
            long idsite = long.Parse(id.Split('_')[0]);

            var idjudet = (from j in db.sites join judet1 in db.judetes on j.judetID equals judet1.id where j.id == idsite select new { j.judetID , judet1.denumire}).FirstOrDefault();

            var echipamente = (from ec in db.echipaments where ec.siteID == idsite select ec.id).ToList();
            var atribute = (from at in db.atributs
                            join ae in db.echipament_atribute on at.id equals ae.atributID
                            join ec in db.echipaments on ae.echipamentID equals ec.id
                            select new { idae = ae.id, aid = at.id }).ToList();
            foreach (var x in atribute)
            {

                echipament_atribute y = (from ea in db.echipament_atribute where ea.id == x.idae select ea).FirstOrDefault();
                db.echipament_atribute.DeleteObject(y);
                db.SaveChanges();
            }

            foreach (var x in atribute)
            {
                atribut y = (from ea in db.atributs where ea.id == x.aid select ea).FirstOrDefault();
                db.atributs.DeleteObject(y);
                db.SaveChanges();
            }

            foreach (var x in echipamente)
            {
                echipament y = (from ea in db.echipaments where ea.id == x select ea).FirstOrDefault();
                db.echipaments.DeleteObject(y);
                db.SaveChanges();
            }

            site v = (from s in db.sites where s.id == idsite select s).FirstOrDefault();
            db.sites.DeleteObject(v);
            db.SaveChanges();

            return RedirectToAction("Index", "Judete", new { name = idjudet.denumire});
        }

        public ActionResult CautaLocatie(long? id, string parentID = "")
        {
            ViewBag.id = id;
            var numejudet = (from j in db.judetes join s in db.sites on j.id equals s.judetID where s.id == id select j.denumire).FirstOrDefault();
            ViewBag.numejudet = numejudet;
            return PartialView();
        }

        public ActionResult ListLocatiiContent(long? id)
        {
            var page1 = Request["page"];
            int rows = 15;
            var name = (from s in db.sites join e in db.echipaments on s.id equals e.siteID join te in db.tip_echipament  on e.tipID equals te.id select new { e.id, te.denumire }).ToList().Distinct();
            var detalii = (from e in db.echipaments join ea in db.echipament_atribute on e.id equals ea.echipamentID join
                          at in db.atributs on ea.atributID equals at.id join ta in db.tip_atribut on at.tipID equals ta.id join te in db.tip_echipament
                          on e.tipID equals te.id
                           where e.siteID == id
                           let nrmsed = (from e2 in db.echipaments
                                         join te1 in db.tip_echipament
                                         on e2.tipID equals te1.id
                                         where te1.id == te.id
                                         && e2.siteID == id && te1.denumire == "MSED"
                                         where e2.id < e.id
                                         select e2.id).Count()
                           
                           select new
                           {
                               id = e.id,
                               denumire = te.denumire + SqlFunctions.StringConvert((double)nrmsed).Trim(),
                               tipdenumire = ta.denumire,
                           valoare = (ta.tip_valoare == "S" ? at.val_string : (ta.tip_valoare == "I" ? SqlFunctions.StringConvert((double)at.val_int).Trim() : (ta.tip_valoare == "N" ? SqlFunctions.StringConvert((double)at.val_nr).Trim() : (ta.tip_valoare == "CSV" ? at.val_csv : null)))) 
                          //    valcsv = at.val_csv, valint = at.val_int, valnr = at.val_nr, valstr = at.val_string 
                          }).ToList().Distinct();

            int totalrecords = detalii.Count();
            var jsonData =
                           new
                           {

                               page = page1,
                               total = (totalrecords + rows - 1) / rows,
                               rows = (from r in detalii
                                       select new
                                       {
                                           id = r.id,
                                           cell = new[]
                                        {
                                            r.denumire.ToString(),
                                            r.tipdenumire.ToString(),
                                            r.valoare.ToString()
                                        }
                                       })
                           };
            return Json(jsonData, JsonRequestBehavior.AllowGet); ;
        }
    }
}
