using System;
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

        public MeldunekViewModel(WyborZleceniaModel _wzm)
        {
            this.Wzm = new WyborZleceniaModel(_wzm.IdOperator, _wzm.IdBrygadzista, _wzm.IdMaszyna, _wzm.IdZmiana, _wzm.IdZlecenie, _wzm.DataMeldunku);
            this.ListaWynikSprawdzenia = PobierzWyniki();
            this.ListaWygladZewnetrzny = PobierzWyniki();
            this.ListaWygladGrzejnika = PobierzWyniki();
            this.ListaPrzegladMaszyny = PobierzPrzeglady();
            this.ListaMeldunkow = PobierzMeldunki2(_wzm.DataMeldunku, _wzm.DataMeldunku, _wzm.IdMaszyna,_wzm.IdZlecenie, _wzm.IdZmiana, _wzm.IdOperator);
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
        public List<meldunki_wynik> PobierzWyniki()
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from m in db.meldunki_wynik
                            orderby m.id ascending
                            select m).ToList();
                return list;
            }
        }

        public List<meldunki_wynik_prz_maszyny> PobierzPrzeglady()
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from m in db.meldunki_wynik_prz_maszyny
                            orderby m.id ascending
                            select m).ToList();
                return list;
            }
        }
    }
}