using ecoplastol_web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecoplastol_web2.Controllers
{
    public class LoginController : Controller
    {
        LoginModel loginModel = new LoginModel();
        // GET: LoginPage
        public ActionResult Logowanie()
        {
            return View(loginModel);

        }

        [HttpPost]
        public ActionResult Logowanie(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }
            else
            {
                OperatorzyViewModel op;
                op = loginModel.ListaOperatorow.Where(l => l.id == loginModel.Operator).FirstOrDefault();
                if (op.haslo == loginModel.Haslo)
                {
                    return RedirectToAction("WyborZlecenia", "WyborZlecenia", op);
                    //return RedirectToAction("TestAction", "Test", op);

                }
                else
                {
                    return View(loginModel);
                }
                

            }

        }
    }
}