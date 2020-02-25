using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ecoplastol_web2.Models
{

    public class WyborZleceniaModel
    {

        public List<OperatorzyViewModel> ListaOperatorow { get; set; }
        public OperatorzyViewModel Operator { get; set; }
        [DisplayName("Operator")]
        public int IdOperator { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("Data produkcji")]
        public DateTime DataMeldunku { get; set; }

        public List<BrygadzisciViewModel> ListaBrygadzistow { get; set; }
        public BrygadzisciViewModel Brygadzista { get; set; }
        [DisplayName("Brygadzista")]
        public int IdBrygadzista { get; set; }

        public List<maszyny> ListaMaszyn { get; set; }
        public maszyny Maszyna { get; set; }
        [DisplayName("Maszyna")]
        public int IdMaszyna { get; set; }

        public List<zmiany> ListaZmian { get; set; }
        public zmiany Zmiana { get; set; }
        [DisplayName("Zmiana")]
        public int IdZmiana { get; set; }

        public List<ZleceniaViewModel> ListaZlecen { get; set; }
        public ZleceniaViewModel Zlecenie { get; set; }
        public int IdZlecenie { get; set; }

        public WyborZleceniaModel() {
            this.ListaOperatorow = PobierzOperatorow();
            this.Operator = new OperatorzyViewModel();
            this.DataMeldunku = DateTime.Now;
            this.ListaBrygadzistow = PobierzBrygadzistow();
            this.Brygadzista = new BrygadzisciViewModel();
            this.ListaMaszyn = PobierzMaszyny();
            this.Maszyna = new maszyny();
            this.ListaZmian = PobierzZmiany();
            this.Zmiana = new zmiany();
        }

        public WyborZleceniaModel(int idOperator, int idBrygadzista, int idMaszyna, int idZmiana, int idZlecenie, DateTime dataMeldunku)
        {
            this.DataMeldunku = dataMeldunku;
            this.IdOperator = idOperator;
            this.IdBrygadzista = idBrygadzista;
            this.IdMaszyna = idMaszyna;
            this.IdZmiana = idZmiana;
            this.IdZlecenie = idZlecenie;
            this.ListaOperatorow = PobierzOperatorow();
            this.Operator = this.ListaOperatorow.Where(o => o.id == idOperator).FirstOrDefault();
            this.ListaBrygadzistow = PobierzBrygadzistow();
            this.Brygadzista = this.ListaBrygadzistow.Where(b => b.id == idBrygadzista).FirstOrDefault();
            this.ListaMaszyn = PobierzMaszyny();
            this.Maszyna = this.ListaMaszyn.Where(m => m.id == idMaszyna).FirstOrDefault();
            this.ListaZmian = PobierzZmiany();
            this.Zmiana = this.ListaZmian.Where(z => z.id == idZmiana).FirstOrDefault();
            this.ListaZlecen = PobierzZlecenia(idMaszyna, dataMeldunku);
            this.Zlecenie = ListaZlecen.Where(z => z.id == idZlecenie).FirstOrDefault();
        }


        public List<ZleceniaViewModel> PobierzZlecenia(int IdMaszyna, DateTime DataProdukcji)
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from m in db.zlecenia_produkcyjne
                            where m.zlecenie_nr_maszyny == IdMaszyna && DataProdukcji >= m.zlecenie_data_rozp && DataProdukcji <= m.zlecenie_data_zak
                            orderby m.id ascending
                            select new ZleceniaViewModel
                            {
                                id = m.id,
                                wyrob_kod_id = m.wyrob_kod_id,
                                wyrob_kod = m.wyrob_kod,
                                wyrob_kod_indeks = m.wyrob_kod_indeks,
                                wyrob_kod_opis = m.wyrob_kod_opis,
                                wyrob_typ = m.wyrob_typ,
                                wyrob_il_w_op_zb = m.wyrob_il_w_op_zb,
                                wyrob_waga_op = m.wyrob_waga_op,
                                wyrob_waga_1szt = m.wyrob_waga_1szt,
                                wyrob_zakres_sdr = m.wyrob_zakres_sdr,
                                wyrob_zast_zaworu = m.wyrob_zast_zaworu,
                                wyrob_rodzaj_drutu = m.wyrob_rodzaj_drutu,
                                wyrob_norma = m.wyrob_norma,
                                wyrob_il_w_partii = m.wyrob_il_w_partii,
                                zlecenie_nr_maszyny = m.zlecenie_nr_maszyny,
                                zlecenie_ilosc = m.zlecenie_ilosc,
                                zlecenie_data_rozp = m.zlecenie_data_rozp,
                                zlecenie_data_zak = m.zlecenie_data_zak,
                                zlecenie_nr_partii_surowca = m.zlecenie_nr_partii_surowca,
                                zlecenie_nr_partii_drutu = m.zlecenie_nr_partii_drutu,
                                itf_kategoria = m.itf_kategoria,
                                itf_znak1 = m.itf_znak1,
                                itf_znak2 = m.itf_znak2,
                                itf_icc = m.itf_icc,
                                itf_cc1 = m.itf_cc1,
                                itf_cc2 = m.itf_cc2,
                                itf_smin = m.itf_smin,
                                itf_smax = m.itf_smax,
                                itf_trn = m.itf_trn,
                                itf_prn = m.itf_prn,
                                itf_rez = m.itf_rez,
                                itf_odch = m.itf_odch,
                                itf_cz1 = m.itf_cz1,
                                itf_cz2 = m.itf_cz2,
                                itf_ke = m.itf_ke,
                                trace_znak1 = m.trace_znak1,
                                trace_znak2 = m.trace_znak2,
                                trace_kategoria = m.trace_kategoria,
                                trace_smin = m.trace_smin,
                                trace_smax = m.trace_smax,
                                trace_partia = m.trace_partia,
                                trace_producent = m.trace_producent,
                                trace_sdr = m.trace_sdr,
                                trace_pe_m = m.trace_pe_m,
                                trace_material = m.trace_material,
                                trace_pe_o = m.trace_pe_o,
                                trace_mfr = m.trace_mfr,
                                opw = m.opw,
                                czasw = m.czasw,
                                opm = m.opm,
                                czasm = m.czasm,
                                //
                                NazwaZlecenia = m.wyrob_kod + " (" + m.wyrob_kod_indeks + ")"
                            }
                            ).ToList();
                return list;
            }
        }
        public List<OperatorzyViewModel> PobierzOperatorow()
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from m in db.operatorzy_maszyn
                            where m.id > 0
                            orderby m.id ascending
                            select new OperatorzyViewModel
                            {
                                id = m.id,
                                imie = m.imie,
                                nazwisko = m.nazwisko,
                                login = m.login,
                                haslo = m.haslo,
                                aktywny = m.aktywny,
                                brygada = m.brygada,

                                NazwaOperatora = m.imie + " " + m.nazwisko
                            }
                            ).ToList();
                return list;
            }
        }
        public List<BrygadzisciViewModel> PobierzBrygadzistow()
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from m in db.brygadzisci
                            where m.id > 0
                            orderby m.id ascending
                            select new BrygadzisciViewModel
                            {
                                id = m.id,
                                imie = m.imie,
                                nazwisko = m.nazwisko,
                                NazwaBrygadzisty = m.imie + " " + m.nazwisko
                            }
                            ).ToList();
                return list;
            }
        }

        public List<maszyny> PobierzMaszyny()
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from m in db.maszyny
                            where m.id > 0
                            orderby m.id ascending
                            select m).ToList();
                return list;
            }
        }
        public List<zmiany> PobierzZmiany()
        {
            using (var db = new ecoplastolEntities())
            {
                var list = (from m in db.zmiany
                            where m.id > 0
                            orderby m.id ascending
                            select m).ToList();
                return list;
            }
        }
    }

   
}