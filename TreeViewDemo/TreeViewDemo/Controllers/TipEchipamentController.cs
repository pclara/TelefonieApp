using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreeViewDemo.Models;

namespace TreeViewDemo.Controllers
{
    public class TipEchipamentController : Controller
    {
        //
        // GET: /TipEchipament/
        OrganizationEntities db = new OrganizationEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditEchipament(long? id, string parentID = null)
        {
            string site = parentID.Split(':')[0];
            string judet = parentID.Split(':')[1];
            long? siteID = long.Parse(site.Split('_')[0]);
            long? judetID = long.Parse(site.Split('_')[0]);

            ViewBag.siteID = siteID;
            ViewBag.judetID = judetID;

            if (id == -1)
            {
                var numeechip = "";
                ViewBag.numeechip = numeechip;
                ViewBag.id = -1;
            }
            else
            {
                

                var numeechip = (from j in db.tip_echipament where j.id == id select new { j.denumire, j.id }).FirstOrDefault();
                ViewBag.numeechip = numeechip.denumire.ToString();
                ViewBag.id = long.Parse(numeechip.id.ToString());
               
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult Edit(FormCollection echipmodel)
        {
            if (ModelState.IsValid)
            {
                tip_echipament s;
                long? id = long.Parse(echipmodel["id"]);
                if (id == -1)
                {
                    s = new tip_echipament();
                    s.denumire = echipmodel["denumire"].ToString();
                    db.AddTotip_echipament(s);
                    db.SaveChanges();
                }
                else
                {
                    s = db.tip_echipament.Where(o => o.id == id).FirstOrDefault();
                    s.denumire = echipmodel["denumire"].ToString();
                    db.SaveChanges();
                }

                int judetID = int.Parse(echipmodel["judetID"].ToString());

                var numejudet = (from j in db.judetes where j.id == judetID select j.denumire).FirstOrDefault();
                string tip_tree = s.id.ToString() + "_tip_echipament_" + echipmodel["siteID"].ToString() + "_" + echipmodel["judetID"].ToString();
                return RedirectToAction("Index", "Judete", new { name = numejudet, id = tip_tree });
            }
            else
            {
                return Json("Salvarea nu a fost efectuata!");
            }
        }

        public string CaleNod(string id)
        {
            string idtip = id.Split('_')[0];
            string idsite = id.Split('_')[3];
            string idjudet = id.Split('_')[4];
            string cale = "";
            cale = idjudet + "_judet" + "," + idsite + "_site" + "," + id;
            return cale;
        }
    }
}
