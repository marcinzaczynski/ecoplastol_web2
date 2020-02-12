﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecoplastol_web2.Models
{
    public class MeldunekViewModel
    {
        public int test { get; set; }
        public WyborZleceniaModel Wzm { get; set; }
        public List<meldunki_wynik> ListaWynikSprawdzenia { get; set; }
        public List<meldunki_wynik> ListaWygladZewnetrzny { get; set; }
        public List<meldunki_wynik> ListaWygladGrzejnika { get; set; }
        public List<meldunki_wynik_prz_maszyny> ListaPrzegladMaszyny { get; set; }
        public List<MeldunekView> ListaMeldunkow { get; set; }
        public meldunki meldunek { get; set; }
        public int ilosc { get; set; }
        public int ilosc_techn { get; set; }
        public DateTime godz_spr_wtr { get; set; }
        public int wynik_spr_wtr { get; set; }
        public int wyglad_zew { get; set; }
        public int wyglad_grzejnika { get; set; }
        public int przeglad_codz_masz { get; set; }
        public string uwagi { get; set; }
        public int wnn1 { get; set; }
        public int wnn2 { get; set; }
        public int wnn3 { get; set; }
        public int wnn4 { get; set; }
        public int wnn5 { get; set; }
        public int wnn6 { get; set; }
        public int wnn7 { get; set; }
        public int wnn8 { get; set; }
        public int wnn9 { get; set; }

        public MeldunekViewModel(WyborZleceniaModel _wzm)
        {
            this.Wzm = new WyborZleceniaModel(_wzm.IdOperator, _wzm.IdBrygadzista, _wzm.IdMaszyna, _wzm.IdZmiana, _wzm.IdZlecenie, _wzm.DataMeldunku);
            this.ListaWynikSprawdzenia = PobierzWyniki(1);
            this.ListaWygladZewnetrzny = PobierzWyniki(1);
            this.ListaWygladGrzejnika = PobierzWyniki(1);
            this.ListaPrzegladMaszyny = PobierzPrzeglady(1);
            this.ListaMeldunkow = PobierzMeldunki2(_wzm.DataMeldunku, _wzm.DataMeldunku, _wzm.IdMaszyna,_wzm.IdZlecenie, _wzm.IdZmiana, _wzm.IdOperator);
            this.meldunek = new meldunki();
            this.ilosc = 0;
            this.ilosc_techn = 0;
            this.godz_spr_wtr = DateTime.Now;
            this.wynik_spr_wtr = 1;
            this.wyglad_zew = 1;
            this.wyglad_grzejnika = 1;
            this.przeglad_codz_masz = 1;
            this.uwagi = String.Empty;
            this.wnn1 = 0;
            this.wnn2 = 0;
            this.wnn3 = 0;
            this.wnn4 = 0;
            this.wnn5 = 0;
            this.wnn6 = 0;
            this.wnn7 = 0;
            this.wnn8 = 0;
            this.wnn9 = 0;
        }

        public static List<MeldunekView> PobierzMeldunki2(DateTime dataOd, DateTime dataDo, int idMaszyny, int idZlecenia, int idZmiany, int idOperatora)
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from m in db.meldunki
                            from mw1 in db.meldunki_wynik
                            from mw2 in db.meldunki_wynik
                            from mw3 in db.meldunki_wynik
                            from mwp in db.meldunki_wynik_prz_maszyny
                            from o in db.operatorzy_maszyn
                            from maszyny in db.maszyny
                            from zlecenia in db.zlecenia_produkcyjne
                            where
                                    //m.id_zlecenie == nrZlecenia &&
                                    //(nrZlecenia > 0) ? m.id_zlecenie == nrZlecenia : m.id_zlecenie.ToString().Contains("*") &&
                                    m.data_meldunku >= dataOd &&
                                    m.data_meldunku <= dataDo &&
                                    mw1.id == m.wynik_spr_wtr &&
                                    mw2.id == m.wyglad_zew &&
                                    mw3.id == m.wyglad_grzejnika &&
                                    mwp.id == m.przeglad_codz_masz &&
                                    o.id == m.id_operator &&
                                    m.id_zlecenie == zlecenia.id &&
                                    zlecenia.zlecenie_nr_maszyny == maszyny.id
                            orderby m.data_meldunku descending, m.zmiana ascending
                            select new MeldunekView
                            {
                                // pola z tabeli meldunki
                                id = m.id,
                                id_zlecenie = m.id_zlecenie,
                                id_operator = m.id_operator,
                                id_brygadzista = m.id_brygadzista,
                                zmiana = m.zmiana,
                                data_meldunku = m.data_meldunku,
                                ilosc = m.ilosc,
                                ilosc_techn = m.ilosc_techn,
                                //id_wady_nn = from s in (from wnn in db.meldunki_wady_nn where wnn.id_meldunek == m.id select wnn.ilosc).ToList()
                                //             where s.
                                //               ?
                                //               ((from wnn in db.meldunki_wady_nn
                                //                 where (wnn.id_meldunek == m.id)
                                //                 select wnn.ilosc).ToList()).Sum()
                                //               :
                                //               0,
                                godz_spr_wtr = m.godz_spr_wtr,
                                wynik_spr_wtr = m.wynik_spr_wtr,
                                wyglad_zew = m.wyglad_zew,
                                wyglad_grzejnika = m.wyglad_grzejnika,
                                opw = m.opw,
                                czasw = m.czasw,
                                opm = m.opm,
                                czasm = m.czasm,
                                przeglad_codz_masz = m.przeglad_codz_masz,
                                uwagi = m.uwagi,
                                // ilosc wad nn
                                ilosc_wad_nn = (
                                             ((from wnn in db.meldunki_wady_nn
                                               where (wnn.id_meldunek == m.id)
                                               select wnn.ilosc).ToList()).Count > 0)
                                               ?
                                               ((from wnn in db.meldunki_wady_nn
                                                 where (wnn.id_meldunek == m.id)
                                                 select wnn.ilosc).ToList()).Sum()
                                               :
                                               0,
                                //dodatkowe pola na potrzeby wyświetlania listy - opisy wartości int z tabeli meldunki
                                nazwa_operatora = o.nazwisko + " " + o.imie,
                                opis_wynik_spr_wtr = mw1.wynik,
                                opis_wyglad_zew = mw2.wynik,
                                opis_wyglad_grzejnika = mw3.wynik,
                                opis_przeglad_codzienny_maszyny = mwp.wynik,

                                // kilka dodatkowych żeby fajnie było widać filtrowanie za pomocą górnych comboboxów
                                nazwa_maszyny = maszyny.nazwa,
                                id_maszyny = maszyny.id,
                                kod_zlecenia = zlecenia.wyrob_kod
                            });

                if (idMaszyny > 0)
                {
                    list = list.Where(r => r.id_maszyny == idMaszyny);
                }

                if (idZlecenia > 0)
                {
                    list = list.Where(r => r.id_zlecenie == idZlecenia);
                }

                if (idZmiany > 0)
                {
                    list = list.Where(r => r.zmiana == idZmiany);
                }

                if (idOperatora > 0)
                {
                    list = list.Where(r => r.id_operator == idOperatora);
                }
                return list.ToList();
            }
        }

        public List<meldunki> PobierzMeldunki(DateTime DataMeldunku, int IdZlecenie, int IdZmiana, int IdOperator)
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from m in db.meldunki
                            where m.data_meldunku == DataMeldunku && m.id_zlecenie == IdZlecenie && m.zmiana == IdZmiana && m.id_operator == IdOperator
                            orderby m.id ascending
                            select m).ToList();
                return list;
            }
        }
        public List<meldunki_wynik> PobierzWyniki(int odId)
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from m in db.meldunki_wynik
                            where m.id >= odId
                            orderby m.id ascending
                            select m).ToList();
                return list;
            }
        }

        public List<meldunki_wynik_prz_maszyny> PobierzPrzeglady(int odId)
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from m in db.meldunki_wynik_prz_maszyny
                            where m.id >= odId
                            orderby m.id ascending
                            select m).ToList();
                return list;
            }
        }
    }
}