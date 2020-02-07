using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecoplastol_web2.Models
{
    public class LoginModel_db
    {
        public List<OperatorzyViewModel> OperatorzyMaszyn { get; set; }
        
        public LoginModel_db()
        {
            this.OperatorzyMaszyn = PobierzOperatorow();
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
    }
}