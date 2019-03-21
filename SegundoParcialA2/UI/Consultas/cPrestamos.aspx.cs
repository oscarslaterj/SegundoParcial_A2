using BLL;
using Entities;
using SegundoParcialA2.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SegundoParcialA2.UI.Consultas
{

    public partial class cPrestamos : System.Web.UI.Page
    {

        Expression<Func<Prestamos, bool>> filtro = c => true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                DesdeTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                HastaTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void BuscarLinkButton_Click(object sender, EventArgs e)
        {
            PrestamoRepositorio repositorio = new PrestamoRepositorio();
            int id = 0;
            DateTime desde = Convert.ToDateTime(DesdeTextBox.Text);
            DateTime hasta = Convert.ToDateTime(HastaTextBox.Text);

            if(hasta.Date < desde.Date)
            {
                Utilitarios.Utils.MostraMensaje(this, "Hasta es menor que desde", "Error", "warning");
                return;
            }

            switch (FiltroDropDownList.SelectedIndex)
            {
                case 0://Todo
                    filtro = c => true && c.Fecha >= desde && c.Fecha <= hasta;
                    break;

                case 1://PrestamoId
                    id = Utils.ToInt(CriterioTextBox.Text);
                    filtro = c => c.PrestamoId == id && c.Fecha >= desde && c.Fecha <= hasta;
                    break;

                case 2://Fecha
                    filtro = c => c.Fecha.Equals(CriterioTextBox.Text) && c.Fecha >= desde && c.Fecha <= hasta;
                    break;

                case 3://CuentaId
                    id = Utils.ToInt(CriterioTextBox.Text);
                    filtro = c => (c.CuentaId == id) && c.Fecha >= desde && c.Fecha <= hasta;

                    break;


            }

            PrestamoGridView.DataSource = repositorio.GetList(filtro);
            PrestamoGridView.DataBind();
        }
    }
}