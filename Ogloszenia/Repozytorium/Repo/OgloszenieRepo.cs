﻿using Microsoft.AspNet.Identity;
using Repozytorium.IRepo;
using Repozytorium.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Repozytorium.Repo
{
    public class OgloszenieRepo : IOgloszenieRepo
    {
        private readonly IOglContext _db;

        public OgloszenieRepo(IOglContext db)
        {
            _db = db;
        }

        public Ogloszenie GetOgloszenieById(int id)
        {
            Ogloszenie ogloszenie = _db.Ogloszenia.Find(id);
            return ogloszenie;
        }

        public IQueryable<Ogloszenie> PobierzOgloszenia()
        {
            _db.Database.Log = message => Trace.WriteLine(message);//pokazuje zapytanie do bazy w output
            var ogloszenia = _db.Ogloszenia.AsNoTracking();//wyl śledzenie przez kontekst, uzywa lazyloading
            return ogloszenia;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void UsunOgloszenie(int id)
        {
            UsunPowiazanieOgloszenieKategoria(id);
            Ogloszenie ogloszenie = _db.Ogloszenia.Find(id);
            _db.Ogloszenia.Remove(ogloszenie);
        }

        private void UsunPowiazanieOgloszenieKategoria(int idOgloszenia)
        {
            var list = _db.Ogloszenie_Kategoria.Where(o => o.OgloszenieId == idOgloszenia);
            foreach (var el in list)
            {
                _db.Ogloszenie_Kategoria.Remove(el);
            }
        }

        public void Dodaj(Ogloszenie ogloszenie)
        {
            _db.Ogloszenia.Add(ogloszenie);
        }

        public void Aktualizuj(Ogloszenie ogloszenie)
        {
            _db.Entry(ogloszenie).State = EntityState.Modified;
        }

        public IQueryable<Ogloszenie> PobierzStrone(int? page = 1, int? pageSize = 10)
        {
            var ogloszenie = _db.Ogloszenia
                .OrderByDescending(o => o.DataDodania)
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
            return ogloszenie;
        }
    }
}