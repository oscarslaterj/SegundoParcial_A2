using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Cuentas
    {
        [Key]
        public int CuentaID { get; set; }
        public string Nombre { get; set; }
        public decimal Balance { get; set; }
        public DateTime Fecha { get; set; }

        public Cuentas()
        {
            CuentaID = 0;
            Nombre = string.Empty;
            Balance = 0;
            Fecha = DateTime.Now;
        }
    }
}
