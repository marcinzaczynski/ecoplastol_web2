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
            wzm.IdOperator = op.id;
            wzm.Operator = op;
            wzm.Brygadzista = wzm.ListaBrygadzistow.Where(b => b.id == op.brygada).FirstOrDefault();
            return View(wzm);
        }

        [HttpPost]
        public ActionResult WyborZlecenia(WyborZleceniaModel _wzm, string submitButton)
        {
            wzm = new WyborZleceniaModel();
            wzm = _wzm;
            wzm.Operator = _wzm.ListaOperatorow.Where(o => o.id == _wzm.IdOperator).FirstOrDefault();
            wzm.Brygadzista = _wzm.ListaBrygadzistow.Where(b => b.id == _wzm.IdBrygadzista).FirstOrDefault();
            wzm.Zmiana = _wzm.ListaZmian.Where(z => z.id == _wzm.IdZmiana).FirstOrDefault();
            wzm.Maszyna = _wzm.ListaMaszyn.Where(m => m.id == _wzm.IdMaszyna).FirstOrDefault();
            wzm.ListaZlecen = wzm.PobierzZlecenia(wzm.IdMaszyna, wzm.DataMeldunku);
            switch (submitButton)
            {
                case "Wyszukaj zlecenia":
                    return View(wzm);
                    break;
                case "Pokaż meldunki":
                    wzm.Zlecenie = _wzm.ListaZlecen.Where(m => m.id == _wzm.IdZlecenie).FirstOrDefault();
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