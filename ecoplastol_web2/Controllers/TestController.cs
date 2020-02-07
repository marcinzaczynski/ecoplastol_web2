using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecoplastol_web2.Controllers
{
    public class TestClass
    {
        public int Int1 { get; set; }
        public string Str1 { get; set; }

        public TestClass()
        {
            this.Int1 = 111;
            this.Str1 = "222";
        }
    }

    public class TestController : Controller
    {
        // GET: Test
        public ActionResult TestAction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestAction(TestClass tc)
        {
            return HttpNotFound();
        }
    }
}