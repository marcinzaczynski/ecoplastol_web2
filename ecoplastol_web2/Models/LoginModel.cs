using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ecoplastol_web2.Models
{
    public class LoginModel
    {
        [Display(Name = "Login")]
        public List<OperatorzyViewModel> ListaOperatorow { get; set; }
        public int Operator { get; set; }

        [Required]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Haslo { get; set; }

        public LoginModel()
        {
            var loginModeldb = new LoginModel_db();
            this.ListaOperatorow = loginModeldb.OperatorzyMaszyn;
            
        }
    }
}