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
    public partial class rPrestamos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LlenarCombos();
                int id = Utils.ToInt(Request.QueryString["id"]);
                FechaTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LlenarDropDownCuentas();
                if (id > 0)
                {
                    PrestamoRepositorio repositorio = new PrestamoRepositorio();
                    Prestamos prestamo = repositorio.Buscar(id);

                    if (prestamo == null)
                        Mensaje(TipoMensaje.Error, "Registro No Encontrado");
                    else
                        LlenaCampos(prestamo);

                }
            }
        }

        protected void BuscarLinkButton_Click(object sender, EventArgs e)
        {
            PrestamoRepositorio repositorio = new PrestamoRepositorio();
            Prestamos prestamo = repositorio.Buscar(Utils.ToInt(IdTextBox.Text));

            if (prestamo != null)
            {
                Limpiar();
                LlenaCampos(prestamo);
            }
            else
            {
                Mensaje(TipoMensaje.Error, "No Se Pudo Encontrar");
                Limpiar();
            }
        }

        protected void CalcularLinkButton_Click(object sender, EventArgs e)
        {
            int id = 0;
            PrestamoRepositorio repositorio = new PrestamoRepositorio();

            if (IdTextBox.Text == string.Empty)
                ViewState["PrestamosDetalles"] = repositorio.CalcularCuotas(Utils.ToInt(TiempoTextBox.Text), Utils.ToDouble(CapitalTextBox.Text), (Utils.ToDouble(InteresTextBox.Text)) / 100 / 12);
            else
                ViewState["PrestamosDetalles"] = repositorio.CalcularCuotasModificadas((List<CuotasDetalle>)ViewState["PrestamosDetalles"], id, Utils.ToInt(TiempoTextBox.Text), Utils.ToDouble(CapitalTextBox.Text), (Utils.ToDouble(InteresTextBox.Text) / 100 / 12));
            CuotaGridView.DataSource = ViewState["PrestamosDetalles"];
            CuotaGridView.DataBind();
        }

        protected void NuevoLinkButton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void LlenarDropDownCuentas()
        {
            RepositorioBase<Cuentas> repositorioBase = new RepositorioBase<Cuentas>();
            CuentaDropDownList.DataSource = repositorioBase.GetList(x => true);
            CuentaDropDownList.DataValueField = "CuentaID";
            CuentaDropDownList.DataTextField = "Nombre";
            CuentaDropDownList.AppendDataBoundItems = true;
            CuentaDropDownList.DataBind();
        }

        protected void GuardarLinkButton_Click(object sender, EventArgs e)
        {
            PrestamoRepositorio repositorio = new PrestamoRepositorio();
            Prestamos prestamo = repositorio.Buscar(Utils.ToInt(IdTextBox.Text));

            if (prestamo == null)
            {
                if (repositorio.Guardar(LlenaClase()))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "Popup", "alert('Guardado Correctamente')", true);
                    Limpiar();
                }
                else
                {
                    Mensaje(TipoMensaje.Error, "No Se Pudo Guardar");
                    Limpiar();
                }
            }
            else
            {
                if (repositorio.Modificar(LlenaClase()))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "Popup", "alert('Modificado Correctamente')", true);
                    Limpiar();
                }
                else
                {
                    Mensaje(TipoMensaje.Error, "No Se Pudo Modificar");
                    Limpiar();
                }
            }
        }

        protected void EliminarLinkButton_Click(object sender, EventArgs e)
        {
            PrestamoRepositorio repositorio = new PrestamoRepositorio();
            Prestamos prestamo = repositorio.Buscar(Utils.ToInt(IdTextBox.Text));

            if (prestamo != null)
            {
                repositorio.Eliminar(prestamo.PrestamoId);
                Mensaje(TipoMensaje.Sucess, "Eliminado Correctamente");
                Limpiar();
            }
            else
            {
                Mensaje(TipoMensaje.Error, "No Se Pudo Eliminar");
                Limpiar();
            }
        }

        private Prestamos LlenaClase()
        {
            Prestamos prestamo = new Prestamos();

            prestamo.PrestamoId = Utils.ToInt(IdTextBox.Text);
            prestamo.Fecha = Utils.ToDateTime(FechaTextBox.Text);
            prestamo.CuentaId = Utils.ToInt(CuentaDropDownList.SelectedValue);
            prestamo.TasaInteres = Utils.ToInt(InteresTextBox.Text);
            prestamo.Capital = Utils.ToDecimal(CapitalTextBox.Text);
            prestamo.Tiempo = Utils.ToInt(TiempoTextBox.Text);
            prestamo.Detalle = (List<CuotasDetalle>)ViewState["PrestamosDetalles"];

            return prestamo;
        }

        private void LlenaCampos(Prestamos prestamo)
        {
            IdTextBox.Text = prestamo.PrestamoId.ToString();
            FechaTextBox.Text = prestamo.Fecha.ToString("yyyy-MM-dd");
            CuentaDropDownList.Text = prestamo.CuentaId.ToString();
            CapitalTextBox.Text = prestamo.Capital.ToString();
            InteresTextBox.Text = prestamo.TasaInteres.ToString();
            TiempoTextBox.Text = prestamo.Tiempo.ToString();
            this.BindGrid();
        }

        protected void BindGrid()
        {
            CuotaGridView.DataSource = ((Prestamos)ViewState["PrestamosDetalles"]).Detalle;
            CuotaGridView.DataBind();
        }

        private void Limpiar()
        {
            IdTextBox.Text = "";
            FechaTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
            CuentaDropDownList.SelectedIndex = 0;
            CapitalTextBox.Text = "";
            InteresTextBox.Text = "";
            TiempoTextBox.Text = "";
            ViewState["PrestamosDetalles"] = null;
        }

        private void LlenarCombos()
        {
            RepositorioBase<Cuentas> repositorio = new RepositorioBase<Cuentas>();
            CuentaDropDownList.DataSource = repositorio.GetList(c => true);
            CuentaDropDownList.DataValueField = "CuentaId";
            CuentaDropDownList.DataTextField = "Nombre";
            CuentaDropDownList.DataBind();
            CuentaDropDownList.Items.Insert(0, new ListItem("", ""));
        }

        void Mensaje(TipoMensaje tipo, string mensaje)
        {
            MensajeLabel.Text = mensaje;
            if (tipo == TipoMensaje.Sucess)
                MensajeLabel.CssClass = "alert-success";
            else
                MensajeLabel.CssClass = "alert-danger";
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args.Value == string.Empty)
                args.IsValid = false;
            else
                args.IsValid = true;
        }
    }
}