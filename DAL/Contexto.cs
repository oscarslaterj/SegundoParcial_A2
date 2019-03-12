using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Contexto : DbContext
    {
        public DbSet<Cuentas> Cuenta { get; set; }
        public DbSet<Prestamos> Prestamo { get; set; }
        public DbSet<CuotasDetalle> Cuotas { get; set; }
        public Contexto() : base("ConStr")
        {

        }

    }
}
