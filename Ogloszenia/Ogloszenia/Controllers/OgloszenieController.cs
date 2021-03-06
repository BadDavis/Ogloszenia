﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repozytorium.Models;
using System.Diagnostics;
using Repozytorium.Repo;
using Repozytorium.IRepo;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Ogloszenia.Controllers
{
    [Authorize]
    public class OgloszenieController : Controller
    {
        private readonly IOgloszenieRepo _repo;

        public OgloszenieController(IOgloszenieRepo repo)
        {
            _repo = repo;
        }

        // GET: Ogloszenie
        [AllowAnonymous]
        public ActionResult Index(int? page, string sortOrder)
        {
            int currentPage = page ?? 1;
            int naStronie = 5;
            


            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSort = String.IsNullOrEmpty(sortOrder) ? "IdAsc" : "";
            ViewBag.DataDodaniaSort = sortOrder == "DataDodania" ? "DataDodaniaAsc" : "DataDodania";
            ViewBag.DataZakonczeniaSort = sortOrder == "DataZakonczenia" ? "DataZakonczeniaAsc" : "DataZakonczenia";
            ViewBag.TytulSort = sortOrder == "TytulAsc" ? "Tytul" : "TytulAsc";
            ViewBag.CenaSort = sortOrder == "CenaAsc" ? "Cena" : "CenaAsc";

            // var ogloszenia = db.Ogloszenia.Include(o => o.Uzykownik); // wyłącza LazyLoading
            var ogloszenia = _repo.PobierzOgloszenia();

            switch (sortOrder)
            {
                case "DataDodania":
                    ogloszenia = ogloszenia.OrderByDescending(s => s.DataDodania);
                    break;

                case "DataDodaniaAsc":
                    ogloszenia = ogloszenia.OrderBy(s => s.DataDodania);
                    break;

                case "Tytul":
                    ogloszenia = ogloszenia.OrderByDescending(s => s.Tytul);
                    break;

                case "TytulAsc":
                    ogloszenia = ogloszenia.OrderBy(s => s.Tytul);
                    break;

                case "DataZakonczenia":
                    ogloszenia = ogloszenia.OrderByDescending(s => s.DataZakonczenia);
                    break;

                case "DataZakonczeniaAsc":
                    ogloszenia = ogloszenia.OrderBy(s => s.DataZakonczenia);
                    break;

                case "Cena":
                    ogloszenia = ogloszenia.OrderByDescending(s => s.Cena);
                    break;

                case "CenaAsc":
                    ogloszenia = ogloszenia.OrderBy(s => s.Cena);
                    break;

                case "IdAsc":
                    ogloszenia = ogloszenia.OrderBy(s => s.Id);
                    break;

                default: //by Id
                    ogloszenia = ogloszenia.OrderByDescending(s => s.Id);
                    break;
            }
            return View(ogloszenia.ToPagedList<Ogloszenie>(currentPage, naStronie));
        }

        [OutputCache(Duration = 1)]
        public ActionResult MojeOgloszenie(int? page)
        {
            ViewBag.userName = User.Identity.Name;
            int currentPage = page ?? 1;
            int naStronie = 5;
            string userID = User.Identity.GetUserId();
            var ogloszenia = _repo.PobierzOgloszenia();
            ogloszenia = ogloszenia.OrderByDescending(d => d.DataDodania)
                .Where(o => o.UzytkownikId == userID);
            return View(ogloszenia.ToPagedList<Ogloszenie>(currentPage, naStronie));
        }


        // GET: Ogloszenie/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
            if (ogloszenie == null)
            {
                return HttpNotFound();
            }
            ViewBag.userName = User.Identity.GetUserName();
            return View(ogloszenie);
        }

        // GET: Ogloszenie/Create
        //[Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ogloszenie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       // [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tytul,Tresc,Cena,Stan")] Ogloszenie ogloszenie)
        {
            if (ModelState.IsValid)
            {

                ogloszenie.UzytkownikId = User.Identity.GetUserId();
                ogloszenie.DataDodania = DateTime.Now;
                ogloszenie.DataZakonczenia = DateTime.Now.AddDays(14);
                try
                {
                    _repo.Dodaj(ogloszenie);
                    _repo.SaveChanges();
                    return RedirectToAction("MojeOgloszenie");
                }
                catch
                {
                    return View(ogloszenie);
                }
            }
            return View(ogloszenie);
        }

        // GET: Ogloszenie/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
            if (ogloszenie == null)
            {
                return HttpNotFound();
            }
            else if (ogloszenie.UzytkownikId != User.Identity.GetUserId() && !(User.IsInRole("Admin") || User.IsInRole("Pracownik")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(ogloszenie);
        }

        // POST: Ogloszenie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Tytul,Tresc,Cena,Stan")] Ogloszenie ogloszenie)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   // ogloszenie.UzytkownikId = "ddad";
                    _repo.Aktualizuj(ogloszenie);
                    _repo.SaveChanges();
                }
                catch (Exception)
                {
                    ViewBag.Blad = true;
                    return View(ogloszenie);
                }
            }
            ViewBag.Blad = false;
            //return View(ogloszenie);
            return RedirectToAction("Details", ogloszenie);
        }

        // GET: Ogloszenie/Delete/5
        //[Authorize]
        public ActionResult Delete(int? id, bool? blad)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
            if (ogloszenie == null)
            {
                return HttpNotFound();
            }
            else if (ogloszenie.UzytkownikId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (blad != null)
            {
                ViewBag.Blad = true;
            }
            return View(ogloszenie);
        }

        // POST: Ogloszenie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.UsunOgloszenie(id);
            try
            {
                _repo.SaveChanges();
            }
            catch
            {
                return RedirectToAction("Delete", new { id = id, blad = true });
            }
            return RedirectToAction("Index");
        }


        public ActionResult Partial(int? page)
        {
            int currentPage = page ?? 1;
            int naStronie = 5;
            var ogloszenia = _repo.PobierzOgloszenia();
            ogloszenia = ogloszenia.OrderByDescending(d => d.DataDodania);
            return PartialView("Index", ogloszenia.ToPagedList<Ogloszenie>(currentPage, naStronie));
        }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            db.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }
    }
}
