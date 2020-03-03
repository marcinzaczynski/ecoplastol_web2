using ecoplastol_web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecoplastol_web2.db
{
    public class Meldunek_db
    {
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
                            orderby m.data_meldunku descending, m.id_zmiana ascending
                            select new MeldunekView
                            {
                                // pola z tabeli meldunki
                                id = m.id,
                                id_zlecenie = m.id_zlecenie,
                                id_operator = m.id_operator,
                                id_brygadzista = m.id_brygadzista,
                                id_zmiana = m.id_zmiana,
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
                                przeglad_codz_masz = m.przeglad_codz_masz,
                                uwagi = m.uwagi,
                                zatwierdzony = m.zatwierdzony,
                                opw = m.opw,
                                czasw = m.czasw,
                                opm = m.opm,
                                czasm = m.czasm,
                                
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
                    list = list.Where(r => r.id_zmiana == idZmiany);
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
                            where m.data_meldunku == DataMeldunku && m.id_zlecenie == IdZlecenie && m.id_zmiana == IdZmiana && m.id_operator == IdOperator
                            orderby m.id ascending
                            select m).ToList();
                return list;
            }
        }
        public static List<meldunki_wynik> PobierzWyniki(int odId)
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

        public static List<meldunki_wynik_prz_maszyny> PobierzPrzeglady(int odId)
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

        public static void DodajMeldunek(MeldunekViewModel mvm) 
        {
            // najpierw ustalam ID meldunku
            int Id = UstawMeldunekId();
            // teraz tworzę meldunek
            meldunki meldunek = new meldunki();
            meldunek.id = Id;
            meldunek.id_zlecenie = mvm.Wzm.IdZlecenie;
            meldunek.id_operator = mvm.Wzm.IdOperator;
            meldunek.id_brygadzista = mvm.Wzm.IdBrygadzista;
            meldunek.id_zmiana = mvm.Wzm.IdZmiana;
            meldunek.data_meldunku = mvm.Wzm.DataMeldunku;
            meldunek.ilosc = mvm.ilosc;
            meldunek.ilosc_techn = mvm.ilosc_techn;
            meldunek.godz_spr_wtr = DateTime.Now.TimeOfDay;
            meldunek.wynik_spr_wtr = mvm.wynik_spr_wtr;
            meldunek.wyglad_zew = mvm.wyglad_zew;
            meldunek.wyglad_grzejnika = mvm.wyglad_grzejnika;
            meldunek.przeglad_codz_masz = mvm.przeglad_codz_masz;
            meldunek.uwagi = mvm.uwagi;
            meldunek.zatwierdzony = false;
            meldunek.opw = mvm.Wzm.Operator.login;
            meldunek.czasw = DateTime.Now;
            meldunek.opm = mvm.Wzm.Operator.login;
            meldunek.czasm = DateTime.Now;
            // dodaję meldunek do bazy
            using (var db = new ecoplastolEntities())
            {

                db.meldunki.Add(meldunek);
                db.SaveChanges();
            }
            // a teraz po kolei dodaje do bazy wady nn
            if (mvm.wnn1 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = Id;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 1;
                wnn.ilosc = mvm.wnn1;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn2 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = Id;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 2;
                wnn.ilosc = mvm.wnn2;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn3 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = Id;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 3;
                wnn.ilosc = mvm.wnn3;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn4 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = Id;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 4;
                wnn.ilosc = mvm.wnn4;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn5 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = Id;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 5;
                wnn.ilosc = mvm.wnn5;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn6 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = Id;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 6;
                wnn.ilosc = mvm.wnn6;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn7 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = Id;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 7;
                wnn.ilosc = mvm.wnn7;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn8 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = Id;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 8;
                wnn.ilosc = mvm.wnn8;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn9 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = Id;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 9;
                wnn.ilosc = mvm.wnn9;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
        }

        public static void PoprawMeldunek(MeldunekViewModel mvm)
        {
            meldunki meldunek = new meldunki();

            using (var db = new ecoplastolEntities())
            {
                meldunek = (from m in db.meldunki
                            where m.id >= mvm.id_meldunku
                            select m).FirstOrDefault();
                
            }
            // teraz tworzę meldunek
            
            //meldunek.id = Id;
            //meldunek.id_zlecenie = mvm.Wzm.IdZlecenie;
            //meldunek.id_operator = mvm.Wzm.IdOperator;
            //meldunek.id_brygadzista = mvm.Wzm.IdBrygadzista;
            //meldunek.id_zmiana = mvm.Wzm.IdZmiana;
            //meldunek.data_meldunku = mvm.Wzm.DataMeldunku;
            meldunek.ilosc = mvm.ilosc;
            meldunek.ilosc_techn = mvm.ilosc_techn;
            meldunek.godz_spr_wtr = DateTime.Now.TimeOfDay;
            meldunek.wynik_spr_wtr = mvm.wynik_spr_wtr;
            meldunek.wyglad_zew = mvm.wyglad_zew;
            meldunek.wyglad_grzejnika = mvm.wyglad_grzejnika;
            meldunek.przeglad_codz_masz = mvm.przeglad_codz_masz;
            meldunek.uwagi = mvm.uwagi;
            //meldunek.zatwierdzony = false;
            //meldunek.opw = mvm.Wzm.Operator.login;
            //meldunek.czasw = DateTime.Now;
            meldunek.opm = mvm.Wzm.Operator.login;
            meldunek.czasm = DateTime.Now;
            
            using (var db = new ecoplastolEntities())
            {
                db.Entry(meldunek).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            // usuwam wnn
            using (var db = new ecoplastolEntities())
            {
                var list = (from w in db.meldunki_wady_nn
                            where w.id_meldunek == mvm.id_meldunku
                            select w).ToList();
                foreach (var detail in list)
                {
                    db.meldunki_wady_nn.Remove(detail);
                }
                db.SaveChanges();
            }
            // a teraz po kolei dodaje do bazy wady nn
            if (mvm.wnn1 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = mvm.id_meldunku;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 1;
                wnn.ilosc = mvm.wnn1;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn2 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = mvm.id_meldunku;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 2;
                wnn.ilosc = mvm.wnn2;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn3 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = mvm.id_meldunku;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 3;
                wnn.ilosc = mvm.wnn3;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn4 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = mvm.id_meldunku;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 4;
                wnn.ilosc = mvm.wnn4;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn5 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = mvm.id_meldunku;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 5;
                wnn.ilosc = mvm.wnn5;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn6 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = mvm.id_meldunku;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 6;
                wnn.ilosc = mvm.wnn6;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn7 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = mvm.id_meldunku;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 7;
                wnn.ilosc = mvm.wnn7;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn8 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = mvm.id_meldunku;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 8;
                wnn.ilosc = mvm.wnn8;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
            if (mvm.wnn9 > 0)
            {
                meldunki_wady_nn wnn = new meldunki_wady_nn();
                wnn.id = UstawMeldunekWadaId();
                wnn.id_meldunek = mvm.id_meldunku;
                wnn.id_zlecenie = mvm.Wzm.IdZlecenie;
                wnn.id_wada_nn = 9;
                wnn.ilosc = mvm.wnn9;
                wnn.opw = mvm.Wzm.Operator.login;
                wnn.czasw = DateTime.Now;
                wnn.opm = mvm.Wzm.Operator.login;
                wnn.czasm = DateTime.Now;
                using (var db = new ecoplastolEntities())
                {

                    db.meldunki_wady_nn.Add(wnn);
                    db.SaveChanges();
                }
            }
        }

        public static int UstawMeldunekId()
        {
            using (var db = new ecoplastolEntities())
            {
                int newId = db.meldunki.Max(p => p.id) + 1;
                return newId;
            }
        }

        public static int UstawMeldunekWadaId()
        {
            using (var db = new ecoplastolEntities())
            {
                int newId = db.meldunki_wady_nn.Max(p => p.id) + 1;
                return newId;
            }
        }

        public static List<meldunki_wady_nn> PobierzWadyNN(int IdMeldunku)
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from m in db.meldunki_wady_nn
                            where m.id_meldunek == IdMeldunku
                            orderby m.id ascending
                            select m).ToList();
                return list;
            }
        }

        public static void UsunMeldunek(int id_meldunku)
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from w in db.meldunki
                            where w.id == id_meldunku
                            select w).ToList();
                foreach (var detail in list)
                {
                    db.meldunki.Remove(detail);
                }
                db.SaveChanges();
            }

            using (var db = new ecoplastolEntities())
            {
                var list = (from w in db.meldunki_wady_nn
                            where w.id_meldunek == id_meldunku
                            select w).ToList();
                foreach (var detail in list)
                {
                    db.meldunki_wady_nn.Remove(detail);
                }
                db.SaveChanges();
            }
        }
    }
}