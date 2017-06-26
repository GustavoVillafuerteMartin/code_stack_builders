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
using Fundraising_PT.Reports;
using DevExpress.XtraReports.UI;


namespace Fundraising_PT.Formularios
{
    public partial class UI_Depositos_Bancarios_Det_Des : DevExpress.XtraEditors.XtraForm
    {
        private decimal ln_total_efectivo_depositos_det = 0;
        private decimal ln_total_efectivo_desgloce_ini = 0;
        private decimal ln_total_efectivo_desgloce_act = 0;
        private decimal ln_total_general_deposito = 0;
        private string lAccion = "";
        private string lHeader_ant = "";
        private object Form_padre;
        private DataTable billetes_aux;
        private DataTable monedas_aux;
        private DataTable billetes_aux_ini;
        private DataTable monedas_aux_ini;
        private int ln_modo = 0;
        private int ln_parent_process = 0;
        private decimal tot_efectivo_billetes = 0;
        private decimal tot_efectivo_monedas = 0;
        private Guid lg_deposito = Guid.Empty;
        private Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios current_deposito;

        public UI_Depositos_Bancarios_Det_Des(object form_padre, int lntipo, int parent_process, ref DataTable billetes, ref DataTable monedas, decimal ln_total_efectivo)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Desgloce de Billetes y Monedas (DEPOSITO-EFECTIVO)...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            // asignacion de valores a objetos publicos //
            Form_padre = form_padre;
            billetes_aux = billetes;
            monedas_aux = monedas;
            ln_total_efectivo_depositos_det = ln_total_efectivo;
            lAccion = "Desgloce de Billetes y Monedas (DEPOSITO-EFECTIVO)";
            ln_parent_process = parent_process;
            //
            switch (ln_parent_process)
            {
                case 1:
                    lHeader_ant = ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)form_padre).HeaderMenu.Caption;
                    ln_modo = (((Fundraising_PT.Formularios.UI_Depositos_Bancarios)form_padre).lAccion == "Insertar" ? 1 : ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)form_padre).lAccion == "Editar" ? 2 : 0);
                    lg_deposito = ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)form_padre).lg_deposito_bancario_aux;
                    ln_total_general_deposito = ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)form_padre).ln_total_general;
                    break;
                case 2:
                    lHeader_ant = ((Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det)form_padre).lHeader_ant;
                    ln_modo = ((Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det)form_padre).ln_modo;
                    lg_deposito = ((Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det)form_padre).lg_deposito_bancario;
                    ln_total_general_deposito = ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)((Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det)form_padre).Form_padre).ln_total_general;
                    break;
                default:
                    break;
            }
            //
            InitializeComponent();
            //
            current_deposito = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios>(lg_deposito);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //
        }

        private void UI_Depositos_Bancarios_Det_Des_Load(object sender, EventArgs e)
        {
            this.textBox_total_deposito_efectivo.textEdit1.Text = ln_total_efectivo_depositos_det.ToString("###,###,###,##0.00");
            this.gridControl_efectivo_billetes.DataSource = billetes_aux;
            this.gridControl_efectivo_monedas.DataSource = monedas_aux;
            this.gridControl_efectivo_billetes.RefreshDataSource();
            this.gridControl_efectivo_monedas.RefreshDataSource();
            //
            this.simpleButton_aceptar.Click +=new EventHandler(simpleButton_aceptar_Click);
            this.simpleButton_cancelar.Click +=new EventHandler(simpleButton_cancelar_Click);
            this.simpleButton_imprimir.Click +=new EventHandler(simpleButton_imprimir_Click);
            this.simpleButton_salir.Click +=new EventHandler(simpleButton_salir_Click);
            //
            this.gridColumn_efectivo_billetes_cantidad.View.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(View_CellValueChanged_cantidad_billetes);
            this.gridColumn_efectivo_monedas_cantidad.View.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(View_CellValueChanged_cantidad_monedas);
            this.gridView_efectivo_billetes.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(gridView_efectivo_billetes_ValidateRow);
            this.gridView_efectivo_monedas.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(gridView_efectivo_monedas_ValidateRow);
            //
            if (ln_modo == 0)
            {
                this.simpleButton_aceptar.Enabled = false;
                this.simpleButton_cancelar.Enabled = false;
                //
                gridColumn_efectivo_billetes_cantidad.OptionsColumn.ReadOnly = true;
                gridColumn_efectivo_monedas_cantidad.OptionsColumn.ReadOnly = true;
            }
            //
            calcula_monto_total_desgloce();
            //
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void simpleButton_imprimir_Click(object sender, EventArgs e)
        {
            XtraReport_Desgloce_Deposito desgloce_deposito = new XtraReport_Desgloce_Deposito(billetes_aux, monedas_aux, current_deposito, ln_total_general_deposito);
            desgloce_deposito.ShowRibbonPreviewDialog();
        }

        void View_CellValueChanged_cantidad_billetes(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            tot_efectivo_billetes = 0;
            foreach (DataRow rowb in billetes_aux.Rows)
            {
                tot_efectivo_billetes = tot_efectivo_billetes + ((Decimal.Parse(rowb[2].ToString()) * Int32.Parse(rowb[3].ToString())));
            }
            this.textBox_monto_total_desgloce.textEdit1.EditValue = tot_efectivo_billetes + tot_efectivo_monedas;
            ln_total_efectivo_desgloce_act = (tot_efectivo_billetes + tot_efectivo_monedas);
        }

        void View_CellValueChanged_cantidad_monedas(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            tot_efectivo_monedas = 0;
            foreach (DataRow rowm in monedas_aux.Rows)
            {
                tot_efectivo_monedas = tot_efectivo_monedas + ((Decimal.Parse(rowm[2].ToString()) * Int32.Parse(rowm[3].ToString())));
            }
            this.textBox_monto_total_desgloce.textEdit1.EditValue = tot_efectivo_billetes + tot_efectivo_monedas;
            ln_total_efectivo_desgloce_act = (tot_efectivo_billetes + tot_efectivo_monedas);
        }

        void gridView_efectivo_billetes_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            int nerror = 0;
            this.gridView_efectivo_billetes.GetDataRow(e.RowHandle).ClearErrors();
            // VALIDA LA COLUMNA DE CANTIDAD //
            if ((decimal)this.textBox_monto_total_desgloce.textEdit1.EditValue > ln_total_efectivo_depositos_det)
            {
                this.gridView_efectivo_billetes.GetDataRow(e.RowHandle).SetColumnError(3, "Monto del desgloce NO puede mayor al monto total del efectivo...");
                nerror = nerror + 1;
            }

            if (nerror > 0)
            {
                e.Valid = false;
            }
            else
            {
                this.gridView_efectivo_billetes.GetDataRow(e.RowHandle).ClearErrors();
                e.Valid = true;
            }
        }

        void gridView_efectivo_monedas_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            int nerror = 0;
            this.gridView_efectivo_monedas.GetDataRow(e.RowHandle).ClearErrors();
            // VALIDA LA COLUMNA DE CANTIDAD //
            if ((decimal)this.textBox_monto_total_desgloce.textEdit1.EditValue > ln_total_efectivo_depositos_det)
            {
                this.gridView_efectivo_monedas.GetDataRow(e.RowHandle).SetColumnError(3, "Monto del desgloce NO puede mayor al monto total del efectivo...");
                nerror = nerror + 1;
            }

            if (nerror > 0)
            {
                e.Valid = false;
            }
            else
            {
                this.gridView_efectivo_monedas.GetDataRow(e.RowHandle).ClearErrors();
                e.Valid = true;
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            switch (ln_parent_process)
            {
                case 1:
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).HeaderMenu.Caption = lHeader_ant + " - Sub-Proceso : " + this.lAccion;
                    break;
                case 2:
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det)Form_padre).obj_headerMenu_ant.Caption = lHeader_ant + " - Sub-Proceso : " + this.lAccion;
                    break;
                default:
                    break;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //
            switch (ln_parent_process)
            {
                case 1:
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).barra_Mant_Base11.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).picture_totales_efectivo.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).picture_totales_cheque.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).picture_totales_tickets.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).simpleButton_desgloce_efectivo.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).grid_Base11.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).ControlBox = true;
                    break;
                case 2:
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det)Form_padre).lookUpEdit_recaudador.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det)Form_padre).checkEdit_todos.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det)Form_padre).gridColumn_saldos_monto.OptionsColumn.ReadOnly = false;
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det)Form_padre).simpleButton_desgloce.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det)Form_padre).simpleButton_imprimir.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det)Form_padre).simpleButton_aceptar.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det)Form_padre).simpleButton_cancelar.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        void simpleButton_aceptar_Click(object sender, EventArgs e)
        {
            if (ln_total_efectivo_desgloce_act < ln_total_efectivo_depositos_det)
            {
                MessageBox.Show(this,"Monto del Desgloce NO puede ser menor al monto total del efectivo...", "Desgloce de Efectivo");
            }
            else
            {
                if (MessageBox.Show("Al ACEPTAR las modificaciones, se GUARDARAN al monento de guardar el depósito bancario.", "Guardar desgloce del efectivo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
                {
                    ln_total_efectivo_desgloce_ini = ln_total_efectivo_desgloce_act;
                    this.Close();
                }
            }
        }

        void simpleButton_cancelar_Click(object sender, EventArgs e)
        {
            if (ln_total_efectivo_desgloce_act != ln_total_efectivo_desgloce_ini)
            {
                if (MessageBox.Show("Esta seguro de CANCALAR las modificaciones ?", "Cancelar Modificacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
                {
                    billetes_aux.Clear();
                    monedas_aux.Clear();
                    billetes_aux = billetes_aux_ini.Copy();
                    monedas_aux = monedas_aux_ini.Copy();
                    this.gridControl_efectivo_billetes.DataSource = billetes_aux;
                    this.gridControl_efectivo_monedas.DataSource = monedas_aux;
                    this.gridControl_efectivo_billetes.RefreshDataSource();
                    this.gridControl_efectivo_monedas.RefreshDataSource();
                    calcula_monto_total_desgloce();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void simpleButton_salir_Click(object sender, EventArgs e)
        {
            if (ln_total_efectivo_desgloce_act != ln_total_efectivo_desgloce_ini)
            {
                if (MessageBox.Show("Esta seguro de SALIR sin Aceptar? \nLos datos se modificaron \nSe perderan los cambios.", "Salir", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
                {
                    billetes_aux.Clear();
                    monedas_aux.Clear();
                    billetes_aux = billetes_aux_ini.Copy();
                    monedas_aux = monedas_aux_ini.Copy();
                    this.Close();
                }
            }
            else
            { this.Close(); }
        }

        private void calcula_monto_total_desgloce()
        {
            tot_efectivo_billetes = 0;
            foreach (DataRow rowb in billetes_aux.Rows)
            {
                tot_efectivo_billetes = tot_efectivo_billetes + ((Decimal.Parse(rowb[2].ToString()) * Int32.Parse(rowb[3].ToString())));
            }
            //
            tot_efectivo_monedas = 0;
            foreach (DataRow rowm in monedas_aux.Rows)
            {
                tot_efectivo_monedas = tot_efectivo_monedas + ((Decimal.Parse(rowm[2].ToString()) * Int32.Parse(rowm[3].ToString())));
            }
            //
            this.textBox_monto_total_desgloce.textEdit1.EditValue = tot_efectivo_billetes + tot_efectivo_monedas;
            if (ln_total_efectivo_desgloce_ini == 0)
            { 
                ln_total_efectivo_desgloce_ini = (tot_efectivo_billetes + tot_efectivo_monedas);
                billetes_aux_ini = billetes_aux.Copy();
                monedas_aux_ini = monedas_aux.Copy();
            }
            ln_total_efectivo_desgloce_act = (tot_efectivo_billetes + tot_efectivo_monedas);
            //
        }

    }
}