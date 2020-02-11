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

        public ActionResult DodajMeldunek(int id_zlecenie, int id_operator, int id_brygadzista, int zmiana, DateTime data_meldunku)
        {
            
            return View(mvm);
        }
    }
}