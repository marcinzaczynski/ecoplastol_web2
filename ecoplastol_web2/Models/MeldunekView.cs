using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ecoplastol_web2.Models
{
    public class MeldunekView : meldunki
    {

        // ilość wad nn
        [DisplayName("Ilość (ocena negatywna)")]
        public int ilosc_wad_nn { get; set; }

        //dodatkowe pola na potrzeby wyświetlania listy - opisy wartości int z tabeli meldunki
        public string nazwa_operatora { get; set; }
        public string opis_wynik_spr_wtr { get; set; }
        public string opis_wyglad_zew { get; set; }
        public string opis_wyglad_grzejnika { get; set; }
        public string opis_przeglad_codzienny_maszyny { get; set; }

        // kilka dodatkowych żeby fajnie było widać filtrowanie za pomocą górnych comboboxów
        public string nazwa_maszyny { get; set; }
        public int id_maszyny { get; set; }
        public string kod_zlecenia { get; set; }
    }
}