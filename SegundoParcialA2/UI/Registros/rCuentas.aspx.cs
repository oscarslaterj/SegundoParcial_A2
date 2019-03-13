using BLL;
using Entities;
using SegundoParcialA2.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SegundoParcialA2.UI.Registros
{
    public partial class rCuentas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FechaTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
            BalanceTextBox.Text = "0";
        }


        private void Limpiar()
        {
            CuentaIdTextBox.Text = "0";
            FechaTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
            NombreTextBox.Text = "";
            BalanceTextBox.Text = "0";
        }

        private Cuentas LlenaClase()
        {
            Cuentas cb = new Cuentas();

            cb.CuentaID = Utils.ToInt(CuentaIdTextBox.Text);
            cb.Fecha = Convert.ToDateTime(FechaTextBox.Text).Date;
            cb.Nombre = NombreTextBox.Text;
            cb.Balance = Utils.ToDecimal(BalanceTextBox.Text);

            return cb;

        }

        protected void BuscarLinkButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<Cuentas> repositoriobase = new RepositorioBase<Cuentas>();
            Cuentas cuentasbancarias = repositoriobase.Buscar(Utils.ToInt(CuentaIdTextBox.Text));
            if (cuentasbancarias != null)
            {
                FechaTextBox.Text = cuentasbancarias.Fecha.ToString();
                NombreTextBox.Text = cuentasbancarias.Nombre;
                BalanceTextBox.Text = cuentasbancarias.Balance.ToString();
                Utils.MostraMensaje(this, "Busqueda exitosa", "Exito", "success");
            }
            else
            {
                Utils.MostraMensaje(this, "No Hay Resultado", "Error", "error");
            }
        }

        protected void NuevoButton_Click(object sender)
        {
            Limpiar();
        }

        protected void GuardarButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<Cuentas> repositorio = new RepositorioBase<Cuentas>();
            Cuentas cuentas = new Cuentas();

            if (IsValid)
            {
                cuentas = repositorio.Buscar(Utils.ToInt(CuentaIdTextBox.Text));

                if (cuentas == null)
                {
                    if (repositorio.Guardar(LlenaClase()))
                    {
                        Utils.MostraMensaje(this, "Guardado correctamente", "Informacion", "success");
                        Limpiar();
                    }
                    else
                    {
                        Utils.MostraMensaje(this, "No se pudo guardar", "Informacion", "error");

                    }
                }
                else
                {
                    if (repositorio.Modificar(LlenaClase()))
                    {
                        Utils.MostraMensaje(this, "Modificado correctamente", "Informacion", "success");
                        Limpiar();
                    }
                    else
                    {
                        Utils.MostraMensaje(this, "No se pudo modificado", "Informacion", "error");

                    }

                }

            }

            /*RepositorioBase<Cuentas> repositorio = new RepositorioBase<Cuentas>();
            Cuentas cuentasbancarias = new Cuentas();
            bool paso = false;


            cuentasbancarias = LlenaClase();

            if (cuentasbancarias.CuentaID == 0)
            {
                paso = repositorio.Guardar(cuentasbancarias);
                Utils.MostraMensaje(this, "Guardado", "Exito", "success");
                Limpiar();
            }
            else
            {
                Cuentas user = new Cuentas();
                int id = Utils.ToInt(CuentaIdTextBox.Text);
                RepositorioBase<Cuentas> repository = new RepositorioBase<Cuentas>();
                cuentasbancarias = repository.Buscar(id);

                if (user != null)
                {
                    paso = repositorio.Modificar(LlenaClase());
                    Utils.MostraMensaje(this, "Modificado", "Exito", "success");
                }
                else
                    Utils.MostraMensaje(this, "Id no existe", "Error", "error");
            }

            if (paso)
            {
                Limpiar();
            }
            else
                Response.Write("<script>alert('No se pudo guardar');</script>");*/
        }

        protected void EliminarButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<Cuentas> repositorio = new RepositorioBase<Cuentas>();
            int id = Utils.ToInt(CuentaIdTextBox.Text);

            var CuentasBancarias = repositorio.Buscar(id);

            if (CuentasBancarias != null)
            {
                if (repositorio.Eliminar(id))
                {
                    Utils.MostraMensaje(this, "Eliminado", "Exito", "success");
                    Limpiar();
                }
                else
                    Utils.MostraMensaje(this, "No se pudo eliminar", "Error", "error");
            }
            else
                Utils.MostraMensaje(this, "No existe", "Error", "error");
        }
    }
}
