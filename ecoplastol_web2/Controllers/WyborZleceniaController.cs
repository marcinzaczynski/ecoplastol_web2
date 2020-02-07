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
            wzm = new WyborZleceniaModel();
            wzm.Operator = op;
            wzm.Brygadzista = wzm.ListaBrygadzistow.Where(b => b.id == op.brygada).FirstOrDefault();
            return View(wzm);
        }

        [HttpPost]
        public ActionResult WyborZlecenia(WyborZleceniaModel _wzm, string submitButton)
        {
            wzm = new WyborZleceniaModel();
            wzm = _wzm;
            wzm.Operator = _wzm.ListaOperatorow.Where(o => o.id == _wzm.Operator.id).FirstOrDefault();
            wzm.Brygadzista = _wzm.ListaBrygadzistow.Where(b => b.id == _wzm.Brygadzista.id).FirstOrDefault();
            wzm.Zmiana = _wzm.ListaZmian.Where(z => z.id == _wzm.Zmiana.id).FirstOrDefault();
            wzm.Maszyna = _wzm.ListaMaszyn.Where(m => m.id == _wzm.Maszyna.id).FirstOrDefault();
            wzm.ListaZlecen = wzm.PobierzZlecenia(wzm.Maszyna.id, wzm.DataProdukcji);
            switch (submitButton)
            {
                case "Wyszukaj zlecenia":
                    return View(wzm);
                    break;
                case "Pokaż meldunki":
                    wzm.Zlecenie = _wzm.ListaZlecen.Where(m => m.id == _wzm.Zlecenie.id).FirstOrDefault();
                    return HttpNotFound();
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