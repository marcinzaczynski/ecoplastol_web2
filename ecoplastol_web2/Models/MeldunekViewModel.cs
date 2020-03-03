using ecoplastol_web2.db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ecoplastol_web2.Models
{
    public class MeldunekViewModel
    {
        public string akcja { get; set; }
        public WyborZleceniaModel Wzm { get; set; }
        public List<meldunki_wynik> ListaWynikSprawdzenia { get; set; }
        public List<meldunki_wynik> ListaWygladZewnetrzny { get; set; }
        public List<meldunki_wynik> ListaWygladGrzejnika { get; set; }
        public List<meldunki_wynik_prz_maszyny> ListaPrzegladMaszyny { get; set; }
        public List<MeldunekView> ListaMeldunkow { get; set; }
        public List<meldunki_wady_nn> ListaWadNN { get; set; }
        public meldunki meldunek { get; set; }
        public int id_meldunku { get; set; }

        [Required(ErrorMessage = "Podaj wartość z zakresu 0 - 999")]
        //[RegularExpression("[0-9]")]
        [Range(0, 999)]
        [DisplayName("Ilość (ocena pozytywna)")]
        public int ilosc { get; set; }
        [DisplayName("Ilość (wady technologiczne)")]
        public int ilosc_techn { get; set; }
        public TimeSpan godz_spr_wtr { get; set; }
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
        //
        //public int id_operator { get; set; }
        //public int id_brygadzista { get; set; }
        //public int id_maszyna { get; set; }
        //public int zmiana { get; set; }
        //public int id_zlecenie { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        //public DateTime data_meldunku { get; set; }

        public MeldunekViewModel(WyborZleceniaModel _wzm)
        {
            this.Wzm = new WyborZleceniaModel(_wzm.IdOperator, _wzm.IdBrygadzista, _wzm.IdMaszyna, _wzm.IdZmiana, _wzm.IdZlecenie, _wzm.DataMeldunku);
            this.ListaWynikSprawdzenia = Meldunek_db.PobierzWyniki(1);
            this.ListaWygladZewnetrzny = Meldunek_db.PobierzWyniki(1);
            this.ListaWygladGrzejnika = Meldunek_db.PobierzWyniki(1);
            this.ListaPrzegladMaszyny = Meldunek_db.PobierzPrzeglady(1);
            this.ListaMeldunkow = Meldunek_db.PobierzMeldunki2(_wzm.DataMeldunku, _wzm.DataMeldunku, _wzm.IdMaszyna,_wzm.IdZlecenie, _wzm.IdZmiana, _wzm.IdOperator);
            this.meldunek = new meldunki();
            this.ilosc = 0;
            this.ilosc_techn = 0;
            this.godz_spr_wtr = DateTime.Now.TimeOfDay;
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
            //this.data_meldunku = _wzm.DataMeldunku;
        }

        public MeldunekViewModel()
        {
            this.Wzm = new WyborZleceniaModel();
            this.ListaWynikSprawdzenia = Meldunek_db.PobierzWyniki(1);
            this.ListaWygladZewnetrzny = Meldunek_db.PobierzWyniki(1);
            this.ListaWygladGrzejnika = Meldunek_db.PobierzWyniki(1);
            this.ListaPrzegladMaszyny = Meldunek_db.PobierzPrzeglady(1);
            this.ListaMeldunkow = new List<MeldunekView>();
            this.meldunek = new meldunki();
            this.ilosc = 0;
            this.ilosc_techn = 0;
            this.godz_spr_wtr = DateTime.Now.TimeOfDay;
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
        
    }
}