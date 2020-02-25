using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ecoplastol_web2.Models
{
    public class OperatorzyViewModel
    {
        public int id { get; set; }
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public string login { get; set; }
        public string haslo { get; set; }
        public bool aktywny { get; set; }
        public int brygada { get; set; }
        //
        
        public string NazwaOperatora { get; set; }
    }
}