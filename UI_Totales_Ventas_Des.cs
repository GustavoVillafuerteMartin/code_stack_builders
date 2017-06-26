using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Totales_Ventas_Des : DevExpress.XtraEditors.XtraForm
    {
        private decimal ln_total_tarjeta_puntos_bancarios = 0;
        private decimal ln_total_tarjeta_puntos_bancarios_ini = 0;
        private string lAccion = "";
        private string lHeader_ant = "";
        private object Form_padre;
        private object Form_anterior;
        private DataTable table_detalle_x_punto_bancaio = new DataTable();
        private DataTable table_detalle_x_punto_bancaio_ini = new DataTable();
        private DataRow datarow_gridView_totales_ventas_current;

        public UI_Totales_Ventas_Des(object form_padre, object form_anterior, DataTable Table_detalle_x_punto_bancaio, DataRow Datarow_gridView_totales_ventas_current)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Total Tarjetas x Puntos Bancarios...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            // asignacion de valores a objetos publicos //
            Form_padre = form_padre;
            Form_anterior = form_anterior;
            table_detalle_x_punto_bancaio = Table_detalle_x_punto_bancaio;
            datarow_gridView_totales_ventas_current = Datarow_gridView_totales_ventas_current;
            lHeader_ant = ((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).HeaderMenu.Caption;
            lAccion = "Total Tarjeta por Puntos Bancarios";
            //
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //
        }

        private void UI_Totales_Ventas_Des_Load(object sender, EventArgs e)
        {
            table_detalle_x_punto_bancaio_ini.Columns.Clear();
            table_detalle_x_punto_bancaio_ini.Rows.Clear();
            // se crean las columnas al datatable del total tarjeta x puntos bancarios inicial
            table_detalle_x_punto_bancaio_ini.Columns.Add("oid", typeof(Guid));
            table_detalle_x_punto_bancaio_ini.Columns.Add("codigo", typeof(string));
            table_detalle_x_punto_bancaio_ini.Columns.Add("descr", typeof(string));
            table_detalle_x_punto_bancaio_ini.Columns.Add("monto_manual_des", typeof(decimal));
            table_detalle_x_punto_bancaio_ini.PrimaryKey = new DataColumn[] { table_detalle_x_punto_bancaio_ini.Columns["oid"] };
            // se llenan las filas al datatable del total tarjeta x puntos bancarios inicial
            if (table_detalle_x_punto_bancaio.Rows.Count > 0)
            {
                foreach (DataRow row_pb in table_detalle_x_punto_bancaio.Rows)
                {
                    table_detalle_x_punto_bancaio_ini.Rows.Add(Guid.Parse(row_pb["oid"].ToString()), row_pb["codigo"].ToString(), row_pb["descr"].ToString(), decimal.Parse(row_pb["monto_manual_des"].ToString()));
                }
            }
            //
            this.Location = new Point(((Fundraising_PT.Formularios.UI_Totales_Ventas)Form_anterior).Location.X + 180, ((Fundraising_PT.Formularios.UI_Totales_Ventas)Form_anterior).Location.Y + 30); 
            this.gridControl_totales_ventas.DataSource = table_detalle_x_punto_bancaio;
            this.gridControl_totales_ventas.RefreshDataSource();
            //
            ln_total_tarjeta_puntos_bancarios_ini = table_detalle_x_punto_bancaio.AsEnumerable().Sum((tot_tpb) => tot_tpb.Field<decimal>("monto_manual_des"));
            ln_total_tarjeta_puntos_bancarios = ln_total_tarjeta_puntos_bancarios_ini;
            this.textBox_montototal_ventas.textEdit1.EditValue = ln_total_tarjeta_puntos_bancarios;

            // bindeos necesarios para el funcionamiento //
            this.gridColumn_totalesventas_monto.View.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(View_CellValueChanged);
            this.simpleButton_aceptar.Click += new EventHandler(simpleButton_aceptar_Click);
            this.simpleButton_cancelar.Click += new EventHandler(simpleButton_cancelar_Click);
            this.simpleButton_imprimir.Click += new EventHandler(simpleButton_imprimir_Click);

            // activa o desactiva los controles dependiendo el status de la sesion //
            if (((Fundraising_PT.Formularios.UI_Totales_Ventas)Form_anterior).ln_status_sesion == 4)
            {
                this.simpleButton_aceptar.Enabled = false;
                this.simpleButton_cancelar.Enabled = false;
                this.gridColumn_totalesventas_monto.OptionsColumn.ReadOnly = true;
            }
            else
            {
                if (((Fundraising_PT.Formularios.UI_Totales_Ventas)Form_anterior).ln_status_totalventa > 1 & ((Fundraising_PT.Formularios.UI_Totales_Ventas)Form_anterior).ln_status_totalventa < 6)
                {
                    this.simpleButton_aceptar.Enabled = false;
                    this.simpleButton_cancelar.Enabled = false;
                    this.gridColumn_totalesventas_monto.OptionsColumn.ReadOnly = true;
                }
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void simpleButton_imprimir_Click(object sender, EventArgs e)
        {
            this.gridControl_totales_ventas.ShowRibbonPrintPreview();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).HeaderMenu.Caption = lHeader_ant + " - Sub-Proceso : " + this.lAccion;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //if (((Fundraising_PT.Formularios.UI_Totales_Ventas)Form_anterior).ln_status_sesion != 4)
            //{
            //    ((Fundraising_PT.Formularios.UI_Totales_Ventas)Form_anterior).simpleButton_aceptar.Enabled = true;
            //    ((Fundraising_PT.Formularios.UI_Totales_Ventas)Form_anterior).simpleButton_cancelar.Enabled = true;
            //}
            ((Fundraising_PT.Formularios.UI_Totales_Ventas)Form_anterior).gridControl_totales_ventas.Enabled = true;
            ((Fundraising_PT.Formularios.UI_Totales_Ventas)Form_anterior).simpleButton_salir.Enabled = true;
            ((Fundraising_PT.Formularios.UI_Totales_Ventas)Form_anterior).ControlBox = true;
            ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).HeaderMenu.Caption = this.lHeader_ant;
            //
            ((Fundraising_PT.Formularios.UI_Totales_Ventas)Form_anterior).seteo_status_totalventa();
        }

        void View_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            ln_total_tarjeta_puntos_bancarios = table_detalle_x_punto_bancaio.AsEnumerable().Sum((tot_puntos) => tot_puntos.Field<decimal>("monto_manual_des"));
            this.textBox_montototal_ventas.textEdit1.EditValue = ln_total_tarjeta_puntos_bancarios;
        }

        void simpleButton_aceptar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Al ACEPTAR las modificaciones, se GUARDARAN al monento de guardar los Totales Ventas por Formas de Pago.", "Guardar Total Tarjeta", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
            {
                ln_total_tarjeta_puntos_bancarios_ini = ln_total_tarjeta_puntos_bancarios;
                datarow_gridView_totales_ventas_current["monto_manual"] = ln_total_tarjeta_puntos_bancarios;
                this.Close();
            }
        }

        void simpleButton_cancelar_Click(object sender, EventArgs e)
        {
            if (ln_total_tarjeta_puntos_bancarios != ln_total_tarjeta_puntos_bancarios_ini)
            {
                if (MessageBox.Show("Esta seguro de CANCALAR las modificaciones ?", "Cancelar Modificacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
                {
                    carga_datos_iniciales();
                    this.gridControl_totales_ventas.DataSource = table_detalle_x_punto_bancaio;
                    this.gridControl_totales_ventas.RefreshDataSource();
                    this.gridControl_totales_ventas.Refresh();
                    //
                    ln_total_tarjeta_puntos_bancarios = table_detalle_x_punto_bancaio.AsEnumerable().Sum((tot_tpb) => tot_tpb.Field<decimal>("monto_manual_des"));
                    this.textBox_montototal_ventas.textEdit1.EditValue = ln_total_tarjeta_puntos_bancarios;
                }
            }
        }

        private void simpleButton_salir_Click(object sender, EventArgs e)
        {
            if (ln_total_tarjeta_puntos_bancarios != ln_total_tarjeta_puntos_bancarios_ini)
            {
                if (MessageBox.Show("Esta seguro de SALIR ? \nLos datos se modificaron \nSe perderan los cambios.", "Salir", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
                {
                    carga_datos_iniciales();
                    this.Close();
                }
            }
            else
            { this.Close(); }
        }

        private void carga_datos_iniciales()
        {
            table_detalle_x_punto_bancaio.Clear();
            if (table_detalle_x_punto_bancaio_ini.Rows.Count > 0)
            {
                foreach (DataRow row_pbini in table_detalle_x_punto_bancaio_ini.Rows)
                {
                    table_detalle_x_punto_bancaio.Rows.Add(Guid.Parse(row_pbini["oid"].ToString()), row_pbini["codigo"].ToString(), row_pbini["descr"].ToString(), decimal.Parse(row_pbini["monto_manual_des"].ToString()));
                }
            }
        }

    }
}