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
            mvm.test = 77;
            return View(mvm);
        }

        public ActionResult DodajMeldunek(MeldunekViewModel mvm)
        {
            
            return View(mvm);
        }
    }
}