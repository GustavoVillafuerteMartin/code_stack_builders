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
using DevExpress.Xpo;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Depositos_Bancarios_Det : DevExpress.XtraEditors.XtraForm
    {
        // declaracion de objetos publicos //
        private decimal ln_total_saldo = 0;
        private decimal ln_total_saldo_ini = 0;
        private decimal ln_total_monto = 0;
        private decimal ln_total_monto_ini = 0;
        private decimal ln_total_monto_precargado = 0;
        private decimal ln_total_monto_precargado_ini = 0;
        private decimal ln_saldo = 0;
        private decimal ln_monto = 0;
        private decimal ln_monto_aux = 0;
        private decimal ln_monto_precargado = 0;
        private decimal ln_monto_precargado_aux = 0;
        private int ln_tpago = 0;
        public  int ln_modo = 0;
        public  int ln_status_deposito = 0;
        public  int ln_sucursal_dep_det = 0;
        private string lAccion = "";
        public  string lHeader_ant = "";
        public  object Form_padre;
        public  Guid lg_deposito_bancario = new Guid();
        private Guid lg_recaudador = new Guid();
        private Guid lg_forma_pago = new Guid();
        //public DataTable Totales_deposito = new DataTable();
        public DataTable saldos_formas_pagos = new DataTable();
        public XPView vtotales_deposito_aux = new XPView();
        private XPView vsaldos_formas_pagos = new XPView();   
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Saldos_Recauda_dep> saldos_recauda_dep;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det> depositos_bancarios_det;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> usuarios;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos> formas_pagos;
        private CriteriaOperator filtro_deposito_bancario_det;
        private CriteriaOperator filtro_usuarios;
        private SortProperty orden_usuarios;
        public  DevExpress.XtraBars.BarHeaderItem obj_headerMenu_ant;

        public UI_Depositos_Bancarios_Det(object form_padre, int lntpago)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Saldos Pendientes para Depósitar...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            // asignacion de valores a objetos publicos //
            Form_padre = form_padre;
            //totales_deposito_aux = ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).totales_deposito;
            //Totales_deposito = totales_deposito;
            lg_deposito_bancario = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)((Fundraising_PT.Formularios.UI_Depositos_Bancarios)form_padre).bindingSource1.Current).oid;
            ln_status_deposito = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)((Fundraising_PT.Formularios.UI_Depositos_Bancarios)form_padre).bindingSource1.Current).status;
            ln_sucursal_dep_det = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)((Fundraising_PT.Formularios.UI_Depositos_Bancarios)form_padre).bindingSource1.Current).sucursal;
            obj_headerMenu_ant = ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)form_padre).HeaderMenu;
            lHeader_ant = ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)form_padre).HeaderMenu.Caption;
            lAccion = "Detalle del depósito por forma de pago";
            ln_modo = (((Fundraising_PT.Formularios.UI_Depositos_Bancarios)form_padre).lAccion == "Insertar" ? 1 : ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)form_padre).lAccion == "Editar" ? 2 : 0);
            ln_tpago = lntpago; 
            //
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //
            // llena los datos de la coleccion de las formas de pago y usuarios //
            CriteriaOperator filtro_status = (new OperandProperty("status") == new OperandValue(1));
            DevExpress.Xpo.SortProperty orden_formas_pagos = (new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            DevExpress.Xpo.SortProperty orden_usuarios = (new DevExpress.Xpo.SortProperty("usuario", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            formas_pagos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(DevExpress.Xpo.XpoDefault.Session, filtro_status, orden_formas_pagos);

            // se crean las columnas al datatable de saldos ///
            saldos_formas_pagos.Columns.Add("oid_forma_pago", typeof(Guid));
            saldos_formas_pagos.Columns.Add("oid_recaudador", typeof(Guid));
            saldos_formas_pagos.Columns.Add("cod_forma_pago", typeof(string));
            saldos_formas_pagos.Columns.Add("recaudador", typeof(string));
            saldos_formas_pagos.Columns.Add("forma_pago", typeof(string));
            saldos_formas_pagos.Columns.Add("monto_precargado", typeof(decimal));
            saldos_formas_pagos.Columns.Add("monto", typeof(decimal));
            saldos_formas_pagos.Columns.Add("saldo", typeof(decimal));
            saldos_formas_pagos.PrimaryKey = new DataColumn[] { saldos_formas_pagos.Columns["oid_forma_pago"], saldos_formas_pagos.Columns["oid_recaudador"] };

            // se crean las columnas a la vista que llenara los datos del datatable de saldos ///
            vsaldos_formas_pagos = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Saldos_Recauda_dep));
            vsaldos_formas_pagos.AddProperty("oid_forma_pago", "forma_pago.oid", true, true, DevExpress.Xpo.SortDirection.None);
            vsaldos_formas_pagos.AddProperty("oid_recaudador", "recaudador.oid", true, true, DevExpress.Xpo.SortDirection.None);
            vsaldos_formas_pagos.AddProperty("cod_forma_pago", "forma_pago.codigo", true, true, DevExpress.Xpo.SortDirection.Ascending);
            vsaldos_formas_pagos.AddProperty("recaudador", "recaudador.usuario", true, true, DevExpress.Xpo.SortDirection.Ascending);
            vsaldos_formas_pagos.AddProperty("forma_pago", "forma_pago.nombre", true, true, DevExpress.Xpo.SortDirection.None);
            vsaldos_formas_pagos.AddProperty("recaudado", "Sum(recaudado)", false, true, DevExpress.Xpo.SortDirection.None);
            vsaldos_formas_pagos.AddProperty("depositado", "Sum(depositado)", false, true, DevExpress.Xpo.SortDirection.None);
            vsaldos_formas_pagos.AddProperty("saldo", "Sum(recaudado-depositado)", false, true, DevExpress.Xpo.SortDirection.None);

            // llena los datos de la coleccion del detalle del deposito //
            filtro_deposito_bancario_det = (new OperandProperty("deposito_bancario.oid") == new OperandValue(lg_deposito_bancario) & new OperandProperty("forma_pago.tpago") == new OperandValue(ln_tpago));
            DevExpress.Xpo.SortProperty orden_deposito_bancario_det = (new DevExpress.Xpo.SortProperty("forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            DevExpress.Xpo.SortingCollection orden_deposito_bancario_det1 = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_deposito_bancario_det1.Add(new DevExpress.Xpo.SortProperty("forma_pago.codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_deposito_bancario_det1.Add(new DevExpress.Xpo.SortProperty("recaudador.usuario", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            depositos_bancarios_det = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det>(DevExpress.Xpo.XpoDefault.Session, filtro_deposito_bancario_det, orden_deposito_bancario_det);
            depositos_bancarios_det.Sorting = orden_deposito_bancario_det1;
            //
            seteo_nivel_seguridad();
            //
            // bindeo de datos a los bindingsource principales //
            bindingSource_recaudador.DataSource = usuarios;
            bindingSource_depositos_bancarios_det.DataSource = depositos_bancarios_det;
        }

        private void UI_Depositos_Bancarios_Det_Load(object sender, EventArgs e)
        {
            switch (ln_tpago)
            {   
                case 1:
                    this.Tituto_Principal.Appearance.Image = imageCollection1.Images[1];
                    this.Tituto_Principal.Text = "EFECTIVO";
                    this.simpleButton_desgloce.Visible = true;
                    break;
                case 3:
                    this.Tituto_Principal.Appearance.Image = imageCollection1.Images[2];
                    this.Tituto_Principal.Text = "CHEQUES";
                    this.simpleButton_desgloce.Visible = false;
                    break;
                case 7:
                    this.Tituto_Principal.Appearance.Image = imageCollection1.Images[3];
                    this.Tituto_Principal.Text = "TICKETS";
                    this.simpleButton_desgloce.Visible = false;
                    break;
                default:
                    this.Tituto_Principal.Appearance.Image = imageCollection1.Images[0];
                    this.Tituto_Principal.Text = "NO DEFINIDO";
                    this.simpleButton_desgloce.Visible = false;
                    break;
            }

            // se cargan los saldos al datatable de saldos desde la vista o collection
            carga_saldos(1);
            
            // asigna el datatable de totales_formas_pagos al grid //
            this.gridControl_saldos_forma_pago.DataSource = saldos_formas_pagos;
            this.gridControl_saldos_forma_pago.RefreshDataSource();

            // setea los controles segun el estatus del deposito // 
            seteo_status_depositos();

            // bindeos necesarios para el funcionamiento //
            this.gridView_saldos_forma_pago.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(gridView_saldos_forma_pago_ValidateRow);
            this.gridView_saldos_forma_pago.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gridView_saldos_forma_pago_RowUpdated);
            this.simpleButton_aceptar.Click += new EventHandler(simpleButton_aceptar_Click);
            this.simpleButton_cancelar.Click += new EventHandler(simpleButton_cancelar_Click);
            this.simpleButton_imprimir.Click += new EventHandler(simpleButton_imprimir_Click);
            this.simpleButton_salir.Click +=new EventHandler(simpleButton_salir_Click);
            this.simpleButton_desgloce.Click += new EventHandler(simpleButton_desgloce_Click);

            if (Fundraising_PT.Properties.Settings.Default.U_tipo == 1)
            {
                this.checkEdit_todos.CheckStateChanged += new EventHandler(checkEdit_todos_CheckStateChanged);
                this.lookUpEdit_recaudador.EditValueChanged += new EventHandler(lookUpEdit_recaudador_EditValueChanged);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void simpleButton_desgloce_Click(object sender, EventArgs e)
        {
            if (ln_total_monto > 0)
            {
                lookUpEdit_recaudador.Enabled = false;
                checkEdit_todos.Enabled = false;
                gridColumn_saldos_monto.OptionsColumn.ReadOnly = true;
                simpleButton_desgloce.Enabled = false;
                simpleButton_imprimir.Enabled = false;
                simpleButton_aceptar.Enabled = false;
                simpleButton_cancelar.Enabled = false;
                //
                ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).desgloce_efectivo_process(this, 2, ln_total_monto);
            }
            else
            {
                MessageBox.Show("NO existe monto en efectivo para el desgloce...", "Desgloce de Efectivo");
            }
        }

        void gridView_saldos_forma_pago_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            if (gridView_saldos_forma_pago.RowCount > 0)
            {
                int nerror = 0;
                this.gridView_saldos_forma_pago.GetDataRow(e.RowHandle).ClearErrors();

                // VALIDA LA COLUMNA DE MONTO //
                if (this.gridView_saldos_forma_pago.GetDataRow(e.RowHandle).ItemArray[6].ToString().Trim() == string.Empty || decimal.Parse(this.gridView_saldos_forma_pago.GetDataRow(e.RowHandle).ItemArray[6].ToString().Trim()) < 0 || decimal.Parse(this.gridView_saldos_forma_pago.GetDataRow(e.RowHandle).ItemArray[5].ToString().Trim()) + decimal.Parse(this.gridView_saldos_forma_pago.GetDataRow(e.RowHandle).ItemArray[6].ToString().Trim()) > decimal.Parse(this.gridView_saldos_forma_pago.GetDataRow(e.RowHandle).ItemArray[7].ToString().Trim()))
                {
                    this.gridView_saldos_forma_pago.GetDataRow(e.RowHandle).SetColumnError(6, "Monto NO puede ser nulo, menor a cero (0) o que sobrepase el saldo...");
                    nerror = nerror + 1;
                }

                if (nerror > 0)
                {
                    e.Valid = false;
                }
                else
                {
                    this.gridView_saldos_forma_pago.GetDataRow(e.RowHandle).ClearErrors();
                    e.Valid = true;
                }
            }
        }

        void gridView_saldos_forma_pago_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            ln_total_monto_precargado = 0;
            ln_total_monto = 0;
            ln_total_saldo = 0;
            foreach (DataRow rowsaldo in saldos_formas_pagos.Rows)
            {
                ln_total_monto_precargado = ln_total_monto_precargado + Decimal.Parse(rowsaldo[5].ToString());
                ln_total_monto = ln_total_monto + Decimal.Parse(rowsaldo[6].ToString());
                ln_total_saldo = ln_total_saldo + Decimal.Parse(rowsaldo[7].ToString());
            }
        }

        void simpleButton_imprimir_Click(object sender, EventArgs e)
        {
            this.gridControl_saldos_forma_pago.ShowRibbonPrintPreview();
        }

        void simpleButton_aceptar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Al ACEPTAR las modificaciones, se GUARDARAN al momento de guardar el depósito bancario.", "Guardar detalle del depósito", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
            {
                saldos_formas_pagos.DefaultView.RowFilter = string.Empty;
                ln_total_monto_precargado = saldos_formas_pagos.AsEnumerable().Sum((tot_monto_precargado) => tot_monto_precargado.Field<decimal>("monto_precargado"));
                ln_total_monto = saldos_formas_pagos.AsEnumerable().Sum((tot_monto) => tot_monto.Field<decimal>("monto"));
                ln_total_saldo = saldos_formas_pagos.AsEnumerable().Sum((tot_saldo) => tot_saldo.Field<decimal>("saldo"));
                //
                foreach (DataRow item_saldos_formas_pagos in saldos_formas_pagos.Rows)
                {
                    Guid lg_forma_pago_aux = new Guid();
                    Guid lg_recaudador_aux = new Guid();
                    decimal ln_monto_precargado_aux = 0;
                    decimal ln_monto_aux = 0;
                    decimal ln_saldo_aux = 0;
                    //
                    lg_forma_pago_aux = ((Guid)item_saldos_formas_pagos["oid_forma_pago"]);
                    lg_recaudador_aux = ((Guid)item_saldos_formas_pagos["oid_recaudador"]);
                    ln_monto_precargado_aux = ((decimal)item_saldos_formas_pagos["monto_precargado"]);
                    ln_monto_aux = ((decimal)item_saldos_formas_pagos["monto"]);
                    ln_saldo_aux = ((decimal)item_saldos_formas_pagos["saldo"]);
                    //
                    object[] val_array = new object[5] { lg_forma_pago_aux, lg_recaudador_aux, ln_monto_precargado_aux, ln_monto_aux, ln_saldo_aux };
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).totales_deposito.LoadDataRow(val_array, LoadOption.OverwriteChanges);
                    //
                    //Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det detalle_deposito = new Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det(XpoDefault.Session);
                    //detalle_deposito.deposito_bancario = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios>(lg_deposito_bancario);
                    //detalle_deposito.recaudador = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(lg_recaudador_aux);
                    //detalle_deposito.forma_pago = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux);
                    //detalle_deposito.monto_precargado = ln_monto_precargado_aux;
                    //detalle_deposito.monto = ln_monto_aux;
                    //detalle_deposito.saldo = ln_saldo_aux;
                    //detalle_deposito.referencia = string.Empty;
                    //detalle_deposito.sucursal = ln_sucursal_dep_det;
                    //((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).colection_totales_deposito.Add(detalle_deposito);
                    //MessageBox.Show(((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).colection_totales_deposito.Count.ToString());
                }
                this.Close();
            }
        }

        void simpleButton_cancelar_Click(object sender, EventArgs e)
        {
            if (ln_total_monto != ln_total_monto_ini)
            {
                if (MessageBox.Show("Esta seguro de CANCALAR las modificaciones ?", "Cancelar Modificacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
                {
                    // se cargan los saldos al datatable de saldos desde la vista o collection
                    carga_saldos(1);
                    
                    this.gridControl_saldos_forma_pago.RefreshDataSource();

                    // setea los controles segun el estatus del deposito // 
                    seteo_status_depositos();

                    // borra la informacion del desgloce de billetes y monedas (EFECTIVO).
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).billetes_aux.Clear();
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).monedas_aux.Clear();
                }
            }
        }

        private void simpleButton_salir_Click(object sender, EventArgs e)
        {
            if (ln_total_monto != ln_total_monto_ini)
            {
                if (MessageBox.Show("Esta seguro de SALIR ? \nLos datos se modificaron \nSe perderan los cambios.", "Salir", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
                {
                    // se cargan los saldos iniciales
                    carga_saldos(1);

                    this.Close();
                }
            }
            else
            { this.Close(); }
        }

        void lookUpEdit_recaudador_EditValueChanged(object sender, EventArgs e)
        {
            lg_recaudador = (Guid)lookUpEdit_recaudador.EditValue;
            recaudador_change();
            carga_saldos(2);
        }

        void checkEdit_todos_CheckStateChanged(object sender, EventArgs e)
        {
            recaudador_change();
            carga_saldos(2);
        }

        private void carga_saldos(int ln_modo1)
        {
            if (ln_modo1 == 1)
            {
                // borra todas las filas que puedan existir antes de volver a cargarlas desde el dataview original de saldos ///
                saldos_formas_pagos.Rows.Clear();
            }

            // llena las filas del datatable de los saldos, si no se han cargado //
            if (saldos_formas_pagos.Rows.Count <= 0 || ln_modo1 == 1)
            {
                switch (ln_modo)
                {
                    case 1:  // Modo INSERTAR
                        // carga datos al DATATABLE de saldos desde la VISTA de saldos pendientes //
                        foreach (ViewRecord item_vsaldos_formas_pagos in vsaldos_formas_pagos)
                        {
                            IEnumerable<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det> depaux = depositos_bancarios_det.Where<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det>(det_dep => (det_dep.deposito_bancario.status.Equals(1) | det_dep.deposito_bancario.status.Equals(4)) & det_dep.forma_pago.oid.Equals((Guid)item_vsaldos_formas_pagos["oid_forma_pago"]) & det_dep.recaudador.oid.Equals((Guid)item_vsaldos_formas_pagos["oid_recaudador"]));
                            IEnumerable<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det> depaux1 = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det>(DevExpress.Xpo.XpoDefault.Session).Where<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det>(det_dep1 => (det_dep1.deposito_bancario.status.Equals(1) | det_dep1.deposito_bancario.status.Equals(4)) & det_dep1.forma_pago.oid.Equals((Guid)item_vsaldos_formas_pagos["oid_forma_pago"]) & det_dep1.recaudador.oid.Equals((Guid)item_vsaldos_formas_pagos["oid_recaudador"]));
                            ln_monto_aux = depaux.Sum(det_dep => det_dep.monto);
                            ln_monto_precargado_aux = depaux1.Sum(det_dep1 => det_dep1.monto);
                            //
                            saldos_formas_pagos.Rows.Add(
                                                            (Guid)item_vsaldos_formas_pagos["oid_forma_pago"],
                                                            (Guid)item_vsaldos_formas_pagos["oid_recaudador"],
                                                            (string)item_vsaldos_formas_pagos["cod_forma_pago"],
                                                            (string)item_vsaldos_formas_pagos["recaudador"],
                                                            (string)item_vsaldos_formas_pagos["forma_pago"],
                                                            ln_monto_precargado_aux,
                                                            ln_monto_aux,
                                                            //(ln_status_deposito == 0 ? 0 : ln_monto_aux),
                                                            (decimal)item_vsaldos_formas_pagos["saldo"]
                                                        );
                        }
                        break;
                    case 2:  // Modo EDITAR
                        // carga datos al DATATABLE de saldos desde la VISTA de saldos pendientes //
                        foreach (ViewRecord item_vsaldos_formas_pagos in vsaldos_formas_pagos)
                        {
                            //IEnumerable<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det> depaux = depositos_bancarios_det.Where<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det>(det_dep => det_dep.forma_pago.oid.Equals((Guid)item_vsaldos_formas_pagos["oid_forma_pago"]) & det_dep.recaudador.oid.Equals((Guid)item_vsaldos_formas_pagos["oid_recaudador"]));
                            IEnumerable<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det> depaux = depositos_bancarios_det.Where<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det>(det_dep => (det_dep.deposito_bancario.status.Equals(1) | det_dep.deposito_bancario.status.Equals(4)) & det_dep.forma_pago.oid.Equals((Guid)item_vsaldos_formas_pagos["oid_forma_pago"]) & det_dep.recaudador.oid.Equals((Guid)item_vsaldos_formas_pagos["oid_recaudador"]));
                            IEnumerable<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det> depaux1 = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det>(DevExpress.Xpo.XpoDefault.Session).Where<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det>(det_dep1 => (det_dep1.deposito_bancario.status.Equals(1) | det_dep1.deposito_bancario.status.Equals(4)) & det_dep1.forma_pago.oid.Equals((Guid)item_vsaldos_formas_pagos["oid_forma_pago"]) & det_dep1.recaudador.oid.Equals((Guid)item_vsaldos_formas_pagos["oid_recaudador"]));
                            ln_monto_aux = depaux.Sum(det_dep => det_dep.monto);
                            ln_monto_precargado_aux = depaux1.Sum(det_dep1 => det_dep1.monto);
                            ln_monto_precargado_aux = ln_monto_precargado_aux - ln_monto_aux;
                            //
                            saldos_formas_pagos.Rows.Add(
                                                            (Guid)item_vsaldos_formas_pagos["oid_forma_pago"],
                                                            (Guid)item_vsaldos_formas_pagos["oid_recaudador"],
                                                            (string)item_vsaldos_formas_pagos["cod_forma_pago"],
                                                            (string)item_vsaldos_formas_pagos["recaudador"],
                                                            (string)item_vsaldos_formas_pagos["forma_pago"],
                                                            ln_monto_precargado_aux,
                                                            ln_monto_aux,
                                                            //(ln_status_deposito == 0 ? 0 : ln_monto_aux),
                                                            (decimal)item_vsaldos_formas_pagos["saldo"]
                                                        );
                        }
                        break;
                    default:  // Navegando Consultando
                        // carga datos al DATATABLE de saldos desde el COLLECTION del detalle del deposito //
                        foreach (var item_depositos_bancarios_det in depositos_bancarios_det)
                        {
                            saldos_formas_pagos.Rows.Add(
                                                            item_depositos_bancarios_det.forma_pago.oid,
                                                            item_depositos_bancarios_det.recaudador.oid,
                                                            item_depositos_bancarios_det.forma_pago.codigo,
                                                            item_depositos_bancarios_det.recaudador.usuario,
                                                            item_depositos_bancarios_det.forma_pago.nombre,
                                                            item_depositos_bancarios_det.monto_precargado,
                                                            item_depositos_bancarios_det.monto,
                                                            item_depositos_bancarios_det.saldo
                                                        );
                        }
                        break;
                }  // Final del switch (ln_modo) //
            } // Final del if (saldos_formas_pagos.Rows.Count <= 0 || ln_modo1 == 1) //
            //
            if (ln_modo1==1)
            {
                ln_total_monto_precargado_ini = saldos_formas_pagos.AsEnumerable().Sum((tot_monto_precargado_ini) => tot_monto_precargado_ini.Field<decimal>("monto_precargado"));
                ln_total_monto_ini = saldos_formas_pagos.AsEnumerable().Sum((tot_monto_ini) => tot_monto_ini.Field<decimal>("monto"));
                ln_total_saldo_ini = saldos_formas_pagos.AsEnumerable().Sum((tot_saldo_ini) => tot_saldo_ini.Field<decimal>("saldo"));
            }
            ln_total_monto_precargado = saldos_formas_pagos.AsEnumerable().Sum((tot_monto_precargado_ini) => tot_monto_precargado_ini.Field<decimal>("monto_precargado"));
            ln_total_monto = saldos_formas_pagos.AsEnumerable().Sum((tot_monto_ini) => tot_monto_ini.Field<decimal>("monto"));
            ln_total_saldo = saldos_formas_pagos.AsEnumerable().Sum((tot_saldo_ini) => tot_saldo_ini.Field<decimal>("saldo"));
        }

        public void seteo_status_depositos()
        {
            if (ln_modo == 0)
            {
                gridColumn_saldos_monto.OptionsColumn.ReadOnly = true;
                //
                simpleButton_aceptar.Enabled = false;
                simpleButton_cancelar.Enabled = false;
                //
                checkEdit_todos.Enabled = false;
                lookUpEdit_recaudador.Enabled = false;
            }
            else
            {
                switch (ln_status_deposito)
                {
                    case 0:
                        gridColumn_saldos_monto.OptionsColumn.ReadOnly = false;
                        //
                        simpleButton_aceptar.Enabled = true;
                        simpleButton_cancelar.Enabled = true;
                        checkEdit_todos.Enabled = true;
                        //
                        if (this.checkEdit_todos.CheckState == CheckState.Checked)
                        { this.lookUpEdit_recaudador.Enabled = false; }
                        else
                        { this.lookUpEdit_recaudador.Enabled = true; }
                        //
                        break;
                    case 1:
                        gridColumn_saldos_monto.OptionsColumn.ReadOnly = false;
                        //
                        simpleButton_aceptar.Enabled = true;
                        simpleButton_cancelar.Enabled = true;
                        checkEdit_todos.Enabled = true;
                        //
                        if (this.checkEdit_todos.CheckState == CheckState.Checked)
                        { this.lookUpEdit_recaudador.Enabled = false; }
                        else
                        { this.lookUpEdit_recaudador.Enabled = true; }
                        //
                        break;
                    case 4:
                        gridColumn_saldos_monto.OptionsColumn.ReadOnly = false;
                        //
                        simpleButton_aceptar.Enabled = true;
                        simpleButton_cancelar.Enabled = true;
                        checkEdit_todos.Enabled = true;
                        //
                        if (this.checkEdit_todos.CheckState == CheckState.Checked)
                        { this.lookUpEdit_recaudador.Enabled = false; }
                        else
                        { this.lookUpEdit_recaudador.Enabled = true; }
                        //
                        break;
                    default:
                        gridColumn_saldos_monto.OptionsColumn.ReadOnly = true;
                        //
                        simpleButton_aceptar.Enabled = false;
                        simpleButton_cancelar.Enabled = false;
                        //
                        checkEdit_todos.Enabled = false;
                        lookUpEdit_recaudador.Enabled = false;
                        //
                        break;
                }
            }
        }

        public void recaudador_change()
        {
            if (this.checkEdit_todos.CheckState == CheckState.Checked)
            {
                filtro_deposito_bancario_det = (new OperandProperty("deposito_bancario.oid") == new OperandValue(lg_deposito_bancario) & new OperandProperty("forma_pago.tpago") == new OperandValue(ln_tpago));
                depositos_bancarios_det.Criteria = filtro_deposito_bancario_det;
                //
                //vsaldos_formas_pagos.Criteria = CriteriaOperator.Parse(string.Format("forma_pago.tpago = {0}", ln_tpago));
                vsaldos_formas_pagos.Criteria = CriteriaOperator.Parse(string.Format("sucursal = {0} and forma_pago.tpago = {1}", Fundraising_PT.Properties.Settings.Default.sucursal, ln_tpago));
                vsaldos_formas_pagos.Filter = CriteriaOperator.Parse("saldo > 0");
                saldos_formas_pagos.DefaultView.RowFilter = string.Empty;
                //
                this.lookUpEdit_recaudador.Enabled = false;
            }
            else
            {
                filtro_deposito_bancario_det = (new OperandProperty("deposito_bancario.oid") == new OperandValue(lg_deposito_bancario) & new OperandProperty("forma_pago.tpago") == new OperandValue(ln_tpago) & new OperandProperty("recaudador.oid") == new OperandValue(lg_recaudador));
                depositos_bancarios_det.Criteria = filtro_deposito_bancario_det;
                //
                //vsaldos_formas_pagos.Criteria = CriteriaOperator.Parse(string.Format("forma_pago.tpago = {0} and recaudador.oid = '{1}'", ln_tpago, lg_recaudador));
                vsaldos_formas_pagos.Criteria = CriteriaOperator.Parse(string.Format("sucursal = {0} and forma_pago.tpago = {1} and recaudador.oid = '{2}'", Fundraising_PT.Properties.Settings.Default.sucursal, ln_tpago, lg_recaudador));
                vsaldos_formas_pagos.Filter = CriteriaOperator.Parse("saldo > 0");
                string string_filter = string.Format("oid_recaudador = '{0}'", lg_recaudador);
                saldos_formas_pagos.DefaultView.RowFilter = string.Empty;
                saldos_formas_pagos.DefaultView.RowFilter = string_filter.Trim();
                //
                this.lookUpEdit_recaudador.Enabled = true;
            }
        }

        private void seteo_nivel_seguridad()
        {
            switch (Fundraising_PT.Properties.Settings.Default.U_tipo)
            {
                case 1:
                    filtro_usuarios = (new OperandProperty("status") == new OperandValue(1));
                    usuarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, filtro_usuarios, orden_usuarios);
                    if (usuarios.Count > 0)
                    { lg_recaudador = usuarios[0].oid; }
                    else
                    { lg_recaudador = Guid.Empty; }
                    //
                    this.checkEdit_todos.CheckState = CheckState.Checked;
                    label_recaudador.Visible = true;
                    checkEdit_todos.Visible = true;
                    checkEdit_todos.Enabled = true;
                    lookUpEdit_recaudador.Visible = true;
                    lookUpEdit_recaudador.Enabled = true;
                    break;
                default:
                    filtro_usuarios = (new OperandProperty("status") == new OperandValue(1) & new OperandProperty("oid") == new OperandValue(Fundraising_PT.Properties.Settings.Default.U_oid));
                    usuarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, filtro_usuarios, orden_usuarios);
                    if (usuarios.Count > 0)
                    { lg_recaudador = usuarios[0].oid; }
                    else
                    { lg_recaudador = Guid.Empty; }
                    //
                    this.checkEdit_todos.CheckState = CheckState.Unchecked;
                    label_recaudador.Visible = false;
                    checkEdit_todos.Visible = false;
                    checkEdit_todos.Enabled = false;
                    lookUpEdit_recaudador.Visible = false;
                    lookUpEdit_recaudador.Enabled = false;
                    break;
            }
            recaudador_change();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).HeaderMenu.Caption = lHeader_ant + " - Sub-Proceso : " + this.lAccion;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //
            if (ln_total_monto != ln_total_monto_ini)
            {
                switch (ln_tpago)
                {
                    case 1: // EFECTIVO
                        ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).ln_total_efectivo = ln_total_monto;
                        ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).label_totales_monto_efectivo.Text = ln_total_monto.ToString("###,###,###,##0.00");
                        break;
                    case 3: // CHEQUES
                        ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).ln_total_cheques = ln_total_monto;
                        ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).label_totales_monto_cheque.Text = ln_total_monto.ToString("###,###,###,##0.00");
                        break;
                    case 7: // TICKETS
                        ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).ln_total_tickets = ln_total_monto;
                        ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).label_totales_monto_ticket.Text = ln_total_monto.ToString("###,###,###,##0.00");
                        break;
                }
                ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).ln_total_general = ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).ln_total_efectivo + ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).ln_total_cheques + ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).ln_total_tickets;
                ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).label_total_general.Text = ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).ln_total_general.ToString("###,###,###,##0.00");
            }
            ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).barra_Mant_Base11.Enabled = true;
            ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).picture_totales_efectivo.Enabled = true;
            ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).picture_totales_cheque.Enabled = true;
            ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).picture_totales_tickets.Enabled = true;
            ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).simpleButton_desgloce_efectivo.Enabled = true;
            ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).grid_Base11.Enabled = true;
            ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).ControlBox = true;
            //
            switch (ln_tpago)
            {
                case 1: // EFECTIVO
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).picture_totales_efectivo.Focus();
                    break;
                case 3: // CHEQUES
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).picture_totales_cheque.Focus();
                    break;
                case 7: // TICKETS
                    ((Fundraising_PT.Formularios.UI_Depositos_Bancarios)Form_padre).picture_totales_tickets.Focus();
                    break;
            }
        }
    }
}