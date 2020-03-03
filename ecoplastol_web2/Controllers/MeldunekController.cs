﻿using ecoplastol_web2.db;
using ecoplastol_web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecoplastol_web2.Controllers
{
    public class MeldunekController : Controller
    {
        MeldunekViewModel mvm;
        // GET: Meldunek
        public ActionResult ListaMeldunkow(WyborZleceniaModel _wzm)
        {
            // "pokaż meldunki" z widoku wybór zlecenia
            mvm = new MeldunekViewModel(_wzm);
            return View(mvm);
        }

        public ActionResult SzczegolyMeldunku(MeldunekViewModel _mvm)
        {
            return View(_mvm);
        }


        // używany do action linka z listy meldunków, do dodawania nowego meldunku zanim zrobiłem na formularzu
        public ActionResult NowyMeldunek(int id_operator, int id_brygadzista, int id_maszyna, int id_zmiana, int id_zlecenie, DateTime data_meldunku)
        {
            WyborZleceniaModel wzm = new WyborZleceniaModel(id_operator, id_brygadzista, id_maszyna, id_zmiana, id_zlecenie, data_meldunku);
            MeldunekViewModel mvm = new MeldunekViewModel(wzm);
            mvm.akcja = "D";
            return View("SzczegolyMeldunku", mvm);
        }
       
        [HttpPost]
        public ActionResult NowyMeldunek(MeldunekViewModel _mvm)
        {
            //przycisk "utworz meldunek" na liście meldunków
            WyborZleceniaModel wzm = new WyborZleceniaModel(_mvm.Wzm.IdOperator,
                                                          _mvm.Wzm.IdBrygadzista,
                                                          _mvm.Wzm.IdMaszyna,
                                                          _mvm.Wzm.IdZmiana,
                                                          _mvm.Wzm.IdZlecenie,
                                                          _mvm.Wzm.DataMeldunku);
            MeldunekViewModel mvm = new MeldunekViewModel(wzm);
            mvm.akcja = "D";
            return RedirectToAction("SzczegolyMeldunku", "Meldunek", mvm);
        }

        [HttpPost]
        public ActionResult DodajMeldunek(MeldunekViewModel _mvm)
        {
            // przycisk "dodaj" z widoku nowy meldunek - zatwierdzenie dodania meldunku
            // przycisk "popraw" z widoku poprawiania meldunku - zatwierdzenie poprawienia meldunku
            WyborZleceniaModel wzm = new WyborZleceniaModel(_mvm.Wzm.IdOperator,
                                                          _mvm.Wzm.IdBrygadzista,
                                                          _mvm.Wzm.IdMaszyna,
                                                          _mvm.Wzm.IdZmiana,
                                                          _mvm.Wzm.IdZlecenie,
                                                          _mvm.Wzm.DataMeldunku);
            MeldunekViewModel mvm = new MeldunekViewModel(wzm);
            mvm.akcja = _mvm.akcja;
            mvm.id_meldunku = _mvm.id_meldunku;
            mvm.ilosc = _mvm.ilosc;
            mvm.ilosc_techn = _mvm.ilosc_techn;
            mvm.godz_spr_wtr = _mvm.godz_spr_wtr;
            mvm.wynik_spr_wtr = _mvm.wynik_spr_wtr;
            mvm.wyglad_zew = _mvm.wyglad_zew;
            mvm.wyglad_grzejnika = _mvm.wyglad_grzejnika;
            mvm.przeglad_codz_masz = _mvm.przeglad_codz_masz;
            mvm.uwagi = _mvm.uwagi;
            mvm.wnn1 = _mvm.wnn1;
            mvm.wnn2 = _mvm.wnn2;
            mvm.wnn3 = _mvm.wnn3;
            mvm.wnn4 = _mvm.wnn4;
            mvm.wnn5 = _mvm.wnn5;
            mvm.wnn6 = _mvm.wnn6;
            mvm.wnn7 = _mvm.wnn7;
            mvm.wnn8 = _mvm.wnn8;
            mvm.wnn9 = _mvm.wnn9;

            switch (mvm.akcja)
            {
                case "D":
                    Meldunek_db.DodajMeldunek(mvm);
                    break;
                case "P":
                    Meldunek_db.PoprawMeldunek(mvm);
                    break;
                default:
                    break;
            }
            
            //powrót do listy meldunków
          
            return RedirectToAction("ListaMeldunkow", "Meldunek", wzm);
            //return HttpNotFound();
        }

        public ActionResult PoprawMeldunek(int id_operator, int id_brygadzista, int id_maszyna, int id_zmiana, int id_zlecenie, DateTime data_meldunku, int id_meldunku)
        {
            //przycisk "popraw" z widoku lista meldunków
            WyborZleceniaModel wzm = new WyborZleceniaModel(id_operator, id_brygadzista, id_maszyna, id_zmiana, id_zlecenie, data_meldunku);
            MeldunekViewModel mvm = new MeldunekViewModel(wzm);
            mvm.meldunek = mvm.ListaMeldunkow.Where(l => l.id == id_meldunku).FirstOrDefault();
            mvm.id_meldunku = id_meldunku;
            mvm.ListaWadNN = Meldunek_db.PobierzWadyNN(id_meldunku);
            mvm.ilosc = mvm.meldunek.ilosc;
            mvm.ilosc_techn = mvm.meldunek.ilosc_techn;
            mvm.godz_spr_wtr = mvm.meldunek.godz_spr_wtr;
            mvm.wynik_spr_wtr = mvm.meldunek.wynik_spr_wtr;
            mvm.wyglad_zew = mvm.meldunek.wyglad_zew;
            mvm.wyglad_grzejnika = mvm.meldunek.wyglad_grzejnika;
            mvm.przeglad_codz_masz = mvm.meldunek.przeglad_codz_masz;
            mvm.uwagi = mvm.meldunek.uwagi;
            mvm.akcja = "P";
            foreach (var item in mvm.ListaWadNN)
            {
                switch (item.id_wada_nn)
                {
                    case 1: 
                        mvm.wnn1 = item.ilosc;
                        break;
                    case 2:
                        mvm.wnn2 = item.ilosc;
                        break;
                    case 3:
                        mvm.wnn3 = item.ilosc;
                        break;
                    case 4:
                        mvm.wnn4 = item.ilosc;
                        break;
                    case 5:
                        mvm.wnn5 = item.ilosc;
                        break;
                    case 6:
                        mvm.wnn6 = item.ilosc;
                        break;
                    case 7:
                        mvm.wnn7 = item.ilosc;
                        break;
                    case 8:
                        mvm.wnn8 = item.ilosc;
                        break;
                    case 9:
                        mvm.wnn9 = item.ilosc;
                        break;
                    default:
                        break;
                }
            }
            return View("SzczegolyMeldunku",mvm);
        }

        public ActionResult CzyUsunacMeldunek(int id_operator, int id_brygadzista, int id_maszyna, int id_zmiana, int id_zlecenie, DateTime data_meldunku, int id_meldunku)
        {
            //przycisk "usuń" z widoku lista meldunków
            WyborZleceniaModel wzm = new WyborZleceniaModel(id_operator, id_brygadzista, id_maszyna, id_zmiana, id_zlecenie, data_meldunku);
            MeldunekViewModel mvm = new MeldunekViewModel(wzm);
            mvm.meldunek = mvm.ListaMeldunkow.Where(l => l.id == id_meldunku).FirstOrDefault();
            mvm.id_meldunku = id_meldunku;
            mvm.ListaWadNN = Meldunek_db.PobierzWadyNN(id_meldunku);
            mvm.ilosc = mvm.meldunek.ilosc;
            mvm.ilosc_techn = mvm.meldunek.ilosc_techn;
            mvm.godz_spr_wtr = mvm.meldunek.godz_spr_wtr;
            mvm.wynik_spr_wtr = mvm.meldunek.wynik_spr_wtr;
            mvm.wyglad_zew = mvm.meldunek.wyglad_zew;
            mvm.wyglad_grzejnika = mvm.meldunek.wyglad_grzejnika;
            mvm.przeglad_codz_masz = mvm.meldunek.przeglad_codz_masz;
            mvm.uwagi = mvm.meldunek.uwagi;
            mvm.akcja = "U";
            foreach (var item in mvm.ListaWadNN)
            {
                switch (item.id_wada_nn)
                {
                    case 1:
                        mvm.wnn1 = item.ilosc;
                        break;
                    case 2:
                        mvm.wnn2 = item.ilosc;
                        break;
                    case 3:
                        mvm.wnn3 = item.ilosc;
                        break;
                    case 4:
                        mvm.wnn4 = item.ilosc;
                        break;
                    case 5:
                        mvm.wnn5 = item.ilosc;
                        break;
                    case 6:
                        mvm.wnn6 = item.ilosc;
                        break;
                    case 7:
                        mvm.wnn7 = item.ilosc;
                        break;
                    case 8:
                        mvm.wnn8 = item.ilosc;
                        break;
                    case 9:
                        mvm.wnn9 = item.ilosc;
                        break;
                    default:
                        break;
                }
            }
            return View(mvm);
        }

        public ActionResult UsunMeldunek(int id_operator, int id_brygadzista, int id_maszyna, int id_zmiana, int id_zlecenie, DateTime data_meldunku, int id_meldunku)
        {
            Meldunek_db.UsunMeldunek(id_meldunku);

            WyborZleceniaModel wzm = new WyborZleceniaModel(id_operator, id_brygadzista, id_maszyna, id_zmiana, id_zlecenie, data_meldunku);
            MeldunekViewModel mvm = new MeldunekViewModel(wzm);

            return View("ListaMeldunkow", mvm);
        }

        public ActionResult AnulujUsuwanieMeldunku(int id_operator, int id_brygadzista, int id_maszyna, int id_zmiana, int id_zlecenie, DateTime data_meldunku, int id_meldunku)
        {
            WyborZleceniaModel wzm = new WyborZleceniaModel(id_operator, id_brygadzista, id_maszyna, id_zmiana, id_zlecenie, data_meldunku);
            MeldunekViewModel mvm = new MeldunekViewModel(wzm);

            return View("ListaMeldunkow", mvm);
        }
    }
}