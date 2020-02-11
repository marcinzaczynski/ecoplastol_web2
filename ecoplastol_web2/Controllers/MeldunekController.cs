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
            mvm = new MeldunekViewModel(_wzm);
            return View(mvm);
        }

        public ActionResult DodajMeldunek(int id_operator, int id_brygadzista, int id_maszyna, int zmiana, int id_zlecenie, DateTime data_meldunku)
        {
            WyborZleceniaModel wzm = new WyborZleceniaModel(id_operator, id_brygadzista, id_maszyna, zmiana, id_zlecenie, data_meldunku);
            MeldunekViewModel mvm = new MeldunekViewModel(wzm);
            return View(mvm);
        }
    }
}