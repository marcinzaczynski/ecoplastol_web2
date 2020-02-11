using ecoplastol_web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecoplastol_web2.Controllers
{
    public class WyborZleceniaController : Controller
    {
        WyborZleceniaModel wzm;
        // GET: WyborZlecenia
        public ActionResult WyborZlecenia(OperatorzyViewModel op)
        {
            wzm = new WyborZleceniaModel(op.id, op.brygada, 1, 1, 0, DateTime.Now);
            return View(wzm);
        }

        [HttpPost]
        public ActionResult WyborZlecenia(WyborZleceniaModel _wzm, string submitButton)
        {
            wzm = new WyborZleceniaModel(_wzm.IdOperator, _wzm.IdBrygadzista, _wzm.IdMaszyna, _wzm.IdZmiana, _wzm.IdZlecenie, _wzm.DataMeldunku);
            switch (submitButton)
            {
                case "Wyszukaj zlecenia":
                    return View(wzm);
                    break;
                case "Pokaż meldunki":
                    //wzm.Zlecenie = _wzm.ListaZlecen.Where(m => m.id == _wzm.IdZlecenie).FirstOrDefault();
                    return RedirectToAction("ListaMeldunkow", "Meldunek", wzm);
                    break;
                case "Utwórz meldunek":
                    wzm.Zlecenie = _wzm.ListaZlecen.Where(m => m.id == _wzm.Zlecenie.id).FirstOrDefault();
                    return HttpNotFound();
                    break;
                default:
                    return HttpNotFound();
                    break;
            }
            
            //return HttpNotFound();
        }

        public ActionResult WyszukajZlecenia(WyborZleceniaModel _wzm)
        {

            //return HttpNotFound();
            return View(_wzm);
        }
    }
}