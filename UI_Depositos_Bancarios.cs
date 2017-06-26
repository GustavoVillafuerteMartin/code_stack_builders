using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using Fundraising_PT.Reports;
using DevExpress.XtraReports.UI;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Depositos_Bancarios : Fundraising_PT.Form_Mant_Base1
    {
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales> sucursales =
            new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session, true);

        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> usuarios =
                new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, true);
      
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos> responsable_depositos =
                new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos>(DevExpress.Xpo.XpoDefault.Session, true);

        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas> bancos_cuentas =
                new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas>(DevExpress.Xpo.XpoDefault.Session, true);

        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios> depositos_bancarios_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios> depositos_bancarios;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det> depositos_bancarios_det_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det> depositos_bancarios_det;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det_Des> depositos_bancarios_det_des_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det_Des> deposito_det_des_efectivo_cantidad;
        public  DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas> denominacion_monedas;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas> denominacion_monedas_billetes;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas> denominacion_monedas_monedas;
        private CriteriaOperator filtro_deposito_bancario;
        private CriteriaOperator filtro_deposito_bancario_det_aux;
        private CriteriaOperator filtro_deposito_bancario_det_des_aux;
        private SortProperty orden_deposito_bancario = (new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Descending));
 
        private SortProperty orden_deposito_bancario_det_aux;
        private SortProperty orden_deposito_bancario_det_des_aux;
        //
        public DataTable billetes_aux = new DataTable();
        public DataTable monedas_aux = new DataTable();
        public DataTable totales_deposito = new DataTable();
        //public DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det> colection_totales_deposito = new XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det>(XpoDefault.Session, false);
        //public object[] val_array_dep_det;
        
        private XPView vtotales_deposito = new XPView();
        private XPView vtotales = new XPView();
        //
        private decimal ln_total_efectivo_depositos_det = 0;
        private decimal ln_total_efectivo_depositos_det_ini = 0;
        private int ln_deposito_det_des_cantidad = 0;
        //
        public decimal ln_total_efectivo_ini = 0;
        public decimal ln_total_cheques_ini = 0;
        public decimal ln_total_tickets_ini = 0;
        public decimal ln_total_general_ini = 0;
        //
        public decimal ln_total_efectivo = 0;
        public decimal ln_total_cheques = 0;
        public decimal ln_total_tickets = 0;
        public decimal ln_total_general = 0;
        //
        int lnStatus_auxiliar = 0;
        int lnStatus_anterior = 0;
        string lAccion_anterior = "Ninguno";
        public Guid lg_deposito_bancario_aux = Guid.Empty;
        public Guid lg_deposito_bancario_aux1 = Guid.Empty;
        public Guid lg_dep_ban = Guid.Empty;
        bool opc = false;
        public string lc_codigointegrado = string.Empty;
        public DevExpress.XtraBars.BarHeaderItem obj_headerMenu_aux;

        public UI_Depositos_Bancarios(DevExpress.XtraBars.BarButtonItem opcionMenu, ref DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra, object objetoExtra1, object objetoExtra2, object objetoExtra3, object objetoExtra4)
            : base(opcionMenu, ref headerMenu, objetoExtra, objetoExtra1, objetoExtra2, objetoExtra3, objetoExtra4)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Depósitos Bancarios...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            InitializeComponent();
            //
            this.lookUpEdit_sucursales.Properties.DataSource = Fundraising_PT.Clases.Setting_Sucursales.data_sucursales();
            obj_headerMenu_aux = headerMenu;
            my_sucursal_filter = string.Format("sucursal in({0})", lookUpEdit_sucursales.EditValue.ToString().Trim());
            lookUpEdit_sucursales.ToolTip = my_sucursal_filter;
            seteo_nivel_seguridad();
            //filter_sucursales();
            bindingSource1.MoveFirst();
            viewcodigointegrado();
            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);

            if (bindingSource1.Count > 0)
            {
                lg_deposito_bancario_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                lg_dep_ban = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                //
                seteo_status_deposito();
                calcula_totales();
            }
            else
            {
                lg_deposito_bancario_aux = Guid.Empty;
                lg_dep_ban = Guid.Empty;
            }
        }

        private void UI_Depositos_Bancarios_Load(object sender, EventArgs e)
        {
            CriteriaOperator filtro_billetes = (new OperandProperty("tipo") == new OperandValue(1)) & (new OperandProperty("status") == new OperandValue(1));
            CriteriaOperator filtro_monedas = (new OperandProperty("tipo") == new OperandValue(2)) & (new OperandProperty("status") == new OperandValue(1));
            CriteriaOperator filtro_status = (new OperandProperty("status") == new OperandValue(1));
            DevExpress.Xpo.SortProperty orden_denominaciones = (new DevExpress.Xpo.SortProperty("valor", DevExpress.Xpo.DB.SortingDirection.Descending));
            //
            denominacion_monedas_billetes = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas>(DevExpress.Xpo.XpoDefault.Session, filtro_billetes, orden_denominaciones);
            denominacion_monedas_monedas = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas>(DevExpress.Xpo.XpoDefault.Session, filtro_monedas, orden_denominaciones);
            //
            if (billetes_aux.Columns.Count <= 0)
            {
                // se crean las columnas al datatable del detalle de efectivo en billetes
                billetes_aux.Columns.Add("oid", typeof(Guid));
                billetes_aux.Columns.Add("codigo", typeof(string));
                billetes_aux.Columns.Add("valor", typeof(decimal));
                billetes_aux.Columns.Add("cantidad", typeof(int));
            }

            if (monedas_aux.Columns.Count <= 0)
            {
                // se crean las columnas al datatable del detalle de efectivo en monedas
                monedas_aux.Columns.Add("oid", typeof(Guid));
                monedas_aux.Columns.Add("codigo", typeof(string));
                monedas_aux.Columns.Add("valor", typeof(decimal));
                monedas_aux.Columns.Add("cantidad", typeof(int));
            }
            //
            // se crean las columnas al datatable de los totales del deposito si no estan creadas
            if (totales_deposito.Columns.Count <= 0)
            {
                totales_deposito.Columns.Add("forma_pago", typeof(Guid));
                totales_deposito.Columns.Add("recaudador", typeof(Guid));
                totales_deposito.Columns.Add("monto_precargado", typeof(decimal));
                totales_deposito.Columns.Add("monto", typeof(decimal));
                totales_deposito.Columns.Add("saldo", typeof(decimal));
                totales_deposito.PrimaryKey = new DataColumn[] { totales_deposito.Columns["forma_pago"], totales_deposito.Columns["recaudador"] };
            }
            //
            this.lookUp_banco_cuenta.SetDataSource(bancos_cuentas, "codigo_cuenta", "oid");
            this.lookUp_responsable_deposito.SetDataSource(responsable_depositos, "nombre", "oid");
            this.lookUp_elaborado.SetDataSource(usuarios, "usuario", "oid");
            this.lookUp_revisado.SetDataSource(usuarios, "usuario", "oid");
            //
            this.lookUp_sucursal.Properties.DataSource = sucursales;
            this.lookUp_sucursal.Properties.DisplayMember = "nombre";
            this.lookUp_sucursal.Properties.ValueMember = "codigo";
            this.lookUp_sucursal.DataBindings.Clear();
            this.lookUp_sucursal.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", bindingSource1, "sucursal"));
            //
            this.grid_Base11.gridView1.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(gridView1_CustomColumnDisplayText);
            this.grid_Base11.gridView1.ViewCaption = "Listado de Depósitos Bancarios";
            //
            //this.bindingSource1.CurrentItemChanged += new EventHandler(bindingSource1_CurrentItemChanged);
            this.picture_totales_efectivo.Click += new EventHandler(picture_totales_Click);
            this.picture_totales_cheque.Click += new EventHandler(picture_totales_Click);
            this.picture_totales_tickets.Click += new EventHandler(picture_totales_Click);
            this.simpleButton_desgloce_efectivo.Click += new EventHandler(simpleButton_desgloce_efectivo_Click);
            //bindingSource1.CurrentChanged += bindingSource1_CurrentChanged;
            textBox_buscacodigointegrado.Validated += textBox_buscacodigointegrado_Validated;
            textBox_buscacodigointegrado.textEdit1.KeyPress += textEdit1_KeyPress;
            //
            this.lookUpEdit_sucursales.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 1);
            //
            //viewcodigointegrado();
            //
            billetes_aux.Clear();
            monedas_aux.Clear();
            //depositos_bancarios.Reload();

            seteo_nivel_seguridad();
            filter_sucursales();
            bindingSource1.MoveFirst();
            viewcodigointegrado();
            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);

            if (bindingSource1.Count > 0)
            {
                lg_deposito_bancario_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                lg_dep_ban = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                //
                seteo_status_deposito();
                calcula_totales();
            }
            else
            {
                lg_deposito_bancario_aux = Guid.Empty;
                lg_dep_ban = Guid.Empty;
            }
            //
            denominacion_monedas = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas>(DevExpress.Xpo.XpoDefault.Session, filtro_status, orden_denominaciones);
            //
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                textBox_buscacodigointegrado.lValue = string.Empty;
                textBox_buscacodigointegrado.textEdit1.Text = string.Empty;
                textBox_buscacodigointegrado.Enabled = false;
                textBox_buscacodigointegrado.Visible = false;
            }
        }

        void textBox_buscacodigointegrado_Validated(object sender, EventArgs e)
        {
            bool sw1 = true;
            if (textBox_buscacodigointegrado.lValue.Trim() != string.Empty)
            {
                //
                XPView vbuscacodigo = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios));
                vbuscacodigo.AddProperty("void", "oid", true, true, DevExpress.Xpo.SortDirection.None);
                vbuscacodigo.AddProperty("vcodigo_integrado", "Trim(ToStr(sucursal))+Trim(banco_cuenta.codigo_cuenta)+Trim(nro_deposito)", true, true, DevExpress.Xpo.SortDirection.None);
                vbuscacodigo.CriteriaString = string.Format("Trim(ToStr(sucursal))+Trim(banco_cuenta.codigo_cuenta)+Trim(nro_deposito) = '{0}'", textBox_buscacodigointegrado.lValue.Trim());
                //
                int seek_position = -1;
                Guid v_oid = Guid.Empty;
                string v_codigo_integrado = string.Empty;
                foreach (ViewRecord item_buscacodigo in vbuscacodigo)
                {
                    v_oid = (item_buscacodigo["void"] == null ? Guid.Empty : (Guid)item_buscacodigo["void"]);
                    v_codigo_integrado = (item_buscacodigo["vcodigo_integrado"] == null ? string.Empty : (String)item_buscacodigo["vcodigo_integrado"]);
                    seek_position = bindingSource1.Find("oid", v_oid);
                    bindingSource1.Position = seek_position;
                }
                //
                textBox_buscacodigointegrado.Enabled = false;
                textBox_buscacodigointegrado.Visible = false;
                //
                if (vbuscacodigo.Count <= 0 | seek_position < 0)
                {
                    MessageBox.Show("No se encontro ningun registro con el Código Integrado: " + textBox_buscacodigointegrado.lValue.Trim(), "Buscar Código Integrado.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sw1 = false;
                }
                //
                vbuscacodigo.Dispose();
                //
            }
            else
            {
                textBox_buscacodigointegrado.Enabled = false;
                textBox_buscacodigointegrado.Visible = false;
                sw1 = false;
            }
            //
            textBox_buscacodigointegrado.lValue = string.Empty;
            textBox_buscacodigointegrado.textEdit1.Text = string.Empty;
            //
        }

        //void bindingSource1_CurrentChanged(object sender, EventArgs e)
        //{
        //    viewcodigointegrado();
        //}

        public override void bindingSource1_PositionChanged(object sender, EventArgs e)
        {
            base.bindingSource1_PositionChanged(sender, e);
            //lg_deposito_bancario_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
            //lg_dep_ban = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
            //viewcodigointegrado();

            //////////
            billetes_aux.Clear();
            monedas_aux.Clear();
            if (bindingSource1.Count > 0)
            {
                lg_deposito_bancario_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                lg_dep_ban = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                viewcodigointegrado();
                seteo_status_deposito();
                calcula_totales();
            }
            else
            { 
                lg_deposito_bancario_aux = Guid.Empty;
                lg_dep_ban = Guid.Empty;
            }
            /////////////
                
        }

        void simpleButton_desgloce_efectivo_Click(object sender, EventArgs e)
        {
            if (bindingSource1.Count > 0)
            {
                if (ln_total_efectivo > 0)
                {
                    this.desgloce_efectivo_process(this, 1, ln_total_efectivo);
                }
                else
                {
                    MessageBox.Show("NO existe monto en efectivo para el desgloce...", "Desgloce de Efectivo");
                    this.picture_totales_efectivo.Focus();
                }
            }
        }

        //void bindingSource1_CurrentItemChanged(object sender, EventArgs e)
        //{
        //    billetes_aux.Clear();
        //    monedas_aux.Clear();
        //    if (bindingSource1.Count > 0)
        //    {
        //        lg_deposito_bancario_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
        //        lg_dep_ban = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
        //        seteo_status_deposito();
        //        calcula_totales();
        //    }
        //    else
        //    { 
        //        lg_deposito_bancario_aux = Guid.Empty;
        //        lg_dep_ban = Guid.Empty;
        //    }
        //}

        void picture_totales_Click(object sender, EventArgs e)
        {
            if (bindingSource1.Count > 0)
            {
                lg_deposito_bancario_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                lg_dep_ban = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                ((DevExpress.XtraEditors.PictureEdit)sender).Tag.ToString().Trim();
                int lntpago = int.Parse(((DevExpress.XtraEditors.PictureEdit)sender).Tag.ToString().Trim());
                Formularios.UI_Depositos_Bancarios_Det form_depositos_bancarios_det = new Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det(this, lntpago);
                //form_depositos_bancarios_det.MdiParent = this.MdiParent;
                this.barra_Mant_Base11.Enabled = false;
                this.picture_totales_efectivo.Enabled = false;
                this.picture_totales_cheque.Enabled = false;
                this.picture_totales_tickets.Enabled = false;
                this.simpleButton_desgloce_efectivo.Enabled = false;
                this.grid_Base11.Enabled = false;
                this.ControlBox = false;
                //form_depositos_bancarios_det.Show();
                form_depositos_bancarios_det.ShowDialog(this);
            }
        }

        void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e != null && e.Column.FieldName == "status")
            {
                e.DisplayText = ((Fundraising_PTDM.Enums.EStatus_deposito)e.Value).ToString();
            }
  
            if (e != null && e.Column.FieldName == "sucursal")
            {
                var lc_nombre_sucursal = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("select nombre from sucursales where codigo = {0}", e.Value));
                e.DisplayText = (lc_nombre_sucursal == null ? "[ Vacio ]" : lc_nombre_sucursal.ResultSet[0].Rows[0].Values[0].ToString());
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            obj_headerMenu_aux.Caption = Fundraising_PT.Properties.Settings.Default.Nombre_Sistema.Trim() + " - Módulo : " + this.Text.Trim() + " - Acción : " + this.lAccion;
        }

        public override void buscar(object sender, EventArgs e)
        {
            base.buscar(sender, e);
            textBox_buscacodigointegrado.lValue = string.Empty;
            textBox_buscacodigointegrado.textEdit1.Text = string.Empty;
            if (textBox_buscacodigointegrado.Visible == true)
            {
                textBox_buscacodigointegrado.Enabled = false;
                textBox_buscacodigointegrado.Visible = false;
            }
            else
            {
                textBox_buscacodigointegrado.Enabled = true;
                textBox_buscacodigointegrado.Visible = true;
                textBox_buscacodigointegrado.Focus();
            }
        }

        public override void imprimir(object sender, EventArgs e)
        {
            base.imprimir(sender, e);
            //
            if (this.tabControl1.SelectedTabPage.Text == "Datos")
            {
                XtraReport_Planilla_Deposito planilla_derposito = new XtraReport_Planilla_Deposito(((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).sucursal == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).sucursal);
                //
                if (bindingSource1.Count > 0)
                { lg_deposito_bancario_aux1 = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid; }
                else
                { lg_deposito_bancario_aux1 = Guid.Empty; }
                //
                DevExpress.Xpo.SortingCollection orden_depositos_bancarios_det = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
                orden_depositos_bancarios_det.Add(new DevExpress.Xpo.SortProperty("forma_pago.codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
                //
                depositos_bancarios_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios>(DevExpress.Xpo.XpoDefault.Session, CriteriaOperator.Parse(string.Format("oid = '{0}'", lg_deposito_bancario_aux1)), orden_deposito_bancario);
                depositos_bancarios_det = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det>(DevExpress.Xpo.XpoDefault.Session, CriteriaOperator.Parse(string.Format("deposito_bancario.oid = '{0}'", lg_deposito_bancario_aux1)), new DevExpress.Xpo.SortProperty("forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
                depositos_bancarios_det.Sorting = orden_depositos_bancarios_det;
                //
                planilla_derposito.DataSource = depositos_bancarios_det;
                planilla_derposito.ShowRibbonPreviewDialog();
            }
            else
            { this.grid_Base11.gridControl1.ShowRibbonPrintPreview(); }
        }

        public override void cancelar(object sender, EventArgs e)
        {
            if (ln_total_general != ln_total_general_ini | ln_total_efectivo != ln_total_efectivo_ini | ln_total_cheques != ln_total_cheques_ini | ln_total_tickets != ln_total_tickets_ini)
            {
                if (MessageBox.Show("Esta seguro de CANCELAR ? " + Environment.NewLine + "Los datos se modificaron " + Environment.NewLine + "Se perderan los cambios.", "Cancelar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
                {
                    lAccion_anterior = lAccion;
                    //
                    base.cancelar(sender, e);
                    //
                    if (lAccion_anterior == "Editar" & ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status == 4)
                    {
                        ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status = 1;
                        bindingSource1.EndEdit();
                        ((XPBaseObject)bindingSource1.Current).Save();
                    }
                    //
                    depositos_bancarios.Reload();
                    viewcodigointegrado();
                    this.picture_totales_efectivo.Enabled = true;
                    this.picture_totales_cheque.Enabled = true;
                    this.picture_totales_tickets.Enabled = true;
                    if (bindingSource1.Count > 0)
                    {
                        lg_deposito_bancario_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                        lg_dep_ban = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                        seteo_status_deposito();
                        calcula_totales();
                    }
                    else
                    {
                        lg_deposito_bancario_aux = Guid.Empty;
                        lg_dep_ban = Guid.Empty;
                    }
                }
            }
            else
            {
                lAccion_anterior = lAccion;
                //
                base.cancelar(sender, e);
                //
                if (lAccion_anterior == "Editar" & ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status == 4)
                {
                    ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status = 1;
                    bindingSource1.EndEdit();
                    ((XPBaseObject)bindingSource1.Current).Save();
                }
                depositos_bancarios.Reload();
                viewcodigointegrado();
                this.picture_totales_efectivo.Enabled = true;
                this.picture_totales_cheque.Enabled = true;
                this.picture_totales_tickets.Enabled = true;
                if (bindingSource1.Count > 0)
                {
                    lg_deposito_bancario_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                    lg_dep_ban = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                    seteo_status_deposito();
                    calcula_totales();
                }
                else
                { 
                    lg_deposito_bancario_aux = Guid.Empty;
                    lg_dep_ban = Guid.Empty;
                }
            }
        }

        public override void insertar(object sender, EventArgs e)
        {
            base.insertar(sender, e);
            dateTime_fecha_hora.dateEdit1.DateTime = DateTime.Now;
            dateTime_fecha_hora.dateEdit1.EditValue = DateTime.Now;
            this.picture_totales_efectivo.Enabled = false;
            this.picture_totales_cheque.Enabled = false;
            this.picture_totales_tickets.Enabled = false;

        }

        public override void editar(object sender, EventArgs e)
        {
            if (((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current) != null)
            {
                try
                {
                    ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).Reload();
                    //
                    this.picture_totales_efectivo.Enabled = true;
                    this.picture_totales_cheque.Enabled = true;
                    this.picture_totales_tickets.Enabled = true;
                    //
                    if (((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status == 1)
                    {
                        ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status = 4;
                        ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).revisado = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(Fundraising_PT.Properties.Settings.Default.U_oid);
                        bindingSource1.EndEdit();
                        ((XPBaseObject)bindingSource1.Current).Save();
                        //
                        base.editar(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("No se pueden editar los datos del deposito..." + Environment.NewLine + "Se encuentra en Status: Enviado, Anulado o En Proceso...", "Editar Deposito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {
                    Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("Data Reload", "Continuar", "Ignorar");
                    switch (MessageBox.Show("No se pudo leer los datos desde el servidor para el registro actual..." + Environment.NewLine + Environment.NewLine + "Posiblemente otro usuario lo elimino mientras usted lo tenia seleccionado para editarlo !!!" + Environment.NewLine + Environment.NewLine + "Seleccione una opción para continuar ?" + Environment.NewLine + Environment.NewLine + "Data Reload : Ejecutar (Data Reload) Vuelve a cargar todas las colecciones de datos desde el servidor." + Environment.NewLine + Environment.NewLine + "Cancelar : Cancela la edicion de datos del registro actual y salta al siguiente registro. " + Environment.NewLine + Environment.NewLine + "Ignorar : Ignora la lectura de los datos del registro desde el servidor y permanece sobre el.", "Data Reload (Editar)", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        case DialogResult.Yes:
                            this.datareload();
                            bindingSource1.MoveFirst();
                            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                            break;
                        case DialogResult.No:
                            if (bindingSource1.Count <= 0)
                            {
                                bindingSource1.MoveFirst();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                            if (bindingSource1.Count > 0 & bindingSource1.Position >= bindingSource1.Count)
                            {
                                bindingSource1.MovePrevious();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                            if (bindingSource1.Count > 0 & bindingSource1.Position == 0)
                            {
                                bindingSource1.MoveNext();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                            if (bindingSource1.Count > 0 & (bindingSource1.Position > 0 & bindingSource1.Position < bindingSource1.Count))
                            {
                                bindingSource1.MoveNext();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                            else
                            {
                                bindingSource1.MoveFirst();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                    }
                    Fundraising_PT.Clases.MsgBoxUtil.UnHackMessageBox();
                }
            }
        }

        public override void eliminar(object sender, EventArgs e)
        {
            if (((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current) != null)
            {
                try
                {
                    //
                    ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).Reload();
                    if (((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status == 2)
                    {
                        if (Fundraising_PT.Properties.Settings.Default.U_tipo == 1)
                        {
                            if (MessageBox.Show("NO se puede Eliminar el depósito se encuentra en estatus: (ENVIADO)." + Environment.NewLine + "Desea ANULAR el depósito ?", "Eliminar-Anular", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                            {
                                try
                                {
                                    //
                                    ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status = 3;
                                    ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).revisado = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(Fundraising_PT.Properties.Settings.Default.U_oid);
                                    bindingSource1.EndEdit();
                                    ((XPBaseObject)bindingSource1.Current).Save();
                                    //
                                    depositos_bancarios.Reload();
                                    //
                                    // se inicia la transaccion para reconstruir columna de depositado en la tabla de saldos//
                                    DevExpress.Xpo.XpoDefault.Session.BeginTransaction();
                                    //
                                    XPView recaudadores_aux = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det));
                                    recaudadores_aux.AddProperty("deposito_bancario", "deposito_bancario", true, true, DevExpress.Xpo.SortDirection.None);
                                    recaudadores_aux.AddProperty("recaudador", "recaudador", true, true, DevExpress.Xpo.SortDirection.None);
                                    recaudadores_aux.AddProperty("forma_pago", "forma_pago", true, true, DevExpress.Xpo.SortDirection.None);
                                    recaudadores_aux.Criteria = CriteriaOperator.Parse(string.Format("deposito_bancario.oid = '{0}'", ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid));
                                    //
                                    foreach (ViewRecord items in recaudadores_aux)
                                    {
                                        Guid oid_recaudador = (Guid)items["recaudador"];
                                        Guid oid_forma_pago = (Guid)items["forma_pago"];
                                        Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(2, ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).fecha_hora, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(oid_recaudador), DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(oid_forma_pago));
                                    }
                                    //
                                    DevExpress.Xpo.XpoDefault.Session.CommitTransaction();
                                    MessageBox.Show("Depósito ANULADO Correctamente...", "Anular Depósito");
                                    //
                                }
                                catch (Exception oerror)
                                {
                                    MessageBox.Show("Ocurrio un ERROR durante el proceso de anulación y reconstruccion de saldos, se reversara dicho proceso..." + Environment.NewLine + "favor revisar la tabla de saldos y hacer los ajustes necesarios..." + Environment.NewLine + "Error: " + oerror.Message, "Anular Depósito", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    DevExpress.Xpo.XpoDefault.Session.RollbackTransaction();
                                }

                            }
                        }
                        else
                        {
                            MessageBox.Show("NO tiene permiso para ANULAR depósitos YA enviados," + Environment.NewLine + "Solo autorizado para usuarios nivel ADMINISTRADOR...", "Eliminar");
                        }
                    }
                    else
                    {
                        switch (((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status)
                        {
                            case 3:
                                MessageBox.Show("Depósito YA se encuentra en estatus: (ANULADO).", "Eliminar-Anular");
                                break;
                            case 4:
                                MessageBox.Show("Depósito se encuentra en estatus: (EN PROCESO).", "Eliminar-Anular");
                                break;
                            default:
                                base.eliminar(sender, e);
                                depositos_bancarios.Reload();
                                bindingSource1.MoveFirst();
                                viewcodigointegrado();
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                    Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("Data Reload", "Continuar", "Ignorar");
                    switch (MessageBox.Show("No se pudo leer los datos desde el servidor para el registro actual..." + Environment.NewLine + Environment.NewLine + "Posiblemente otro usuario lo elimino mientras usted lo tenia seleccionado para eliminarno !!!" + Environment.NewLine + Environment.NewLine + "Seleccione una opción para continuar ?" + Environment.NewLine + Environment.NewLine + "Data Reload : Ejecutar (Data Reload) Vuelve a cargar todas las colecciones de datos desde el servidor." + Environment.NewLine + Environment.NewLine + "Cancelar : Cancela la eliminación de datos del registro actual y salta al siguiente registro. " + Environment.NewLine + Environment.NewLine + "Ignorar : Ignora la lectura de los datos del registro desde el servidor y permanece sobre el.", "Data Reload (Eliminar)", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        case DialogResult.Yes:
                            this.datareload();
                            bindingSource1.MoveFirst();
                            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                            break;
                        case DialogResult.No:
                            if (bindingSource1.Count <= 0)
                            {
                                bindingSource1.MoveFirst();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                            if (bindingSource1.Count > 0 & bindingSource1.Position >= bindingSource1.Count)
                            {
                                bindingSource1.MovePrevious();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                            if (bindingSource1.Count > 0 & bindingSource1.Position == 0)
                            {
                                bindingSource1.MoveNext();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                            if (bindingSource1.Count > 0 & (bindingSource1.Position > 0 & bindingSource1.Position < bindingSource1.Count))
                            {
                                bindingSource1.MoveNext();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                            else
                            {
                                bindingSource1.MoveFirst();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                    }
                    Fundraising_PT.Clases.MsgBoxUtil.UnHackMessageBox();
                }
            }
        }

        public override void guardar(object sender, EventArgs e)
        {
            if (this.error_validation())
            {
                if (lAccion == "Editar")
                {
                    Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("Abierto", "Enviado", "Cancelar");
                    switch (MessageBox.Show("Seleccione como desea guardar el depósito ?" + Environment.NewLine + Environment.NewLine + "Abierto : Deja el depósito abierto para ediciónes de datos." + Environment.NewLine + Environment.NewLine + "Enviado : Cierra el depósito, rebaja saldos y solo se permitiran anulaciones.", "Guardar Depósito", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        case DialogResult.Yes:
                            lnStatus_auxiliar = 1;
                            break;
                        case DialogResult.No:
                            lnStatus_auxiliar = 2;
                            break;
                        default:
                            lnStatus_auxiliar = 0;
                            break;
                    }
                    Fundraising_PT.Clases.MsgBoxUtil.UnHackMessageBox();
                }
                else
                { lnStatus_auxiliar = 1; }
                //
                if (lnStatus_auxiliar != 0)
                {
                    // GUARDA DEPOSITO //
                    if (lAccion == "Insertar")
                    { opc = (MessageBox.Show("Esta seguro de GUARDAR los datos ?", "Guardar Depósitos", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK); }
                    if (lAccion == "Editar")
                    { opc = (MessageBox.Show("Esta seguro de GUARDAR los datos con la seleccion correspondiente ?", "Guardar Depósitos", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK); }
                    if (opc)
                    {
                        if (lAccion == "Editar" & ln_total_general <= 0 & lnStatus_auxiliar == 2)
                        {
                            MessageBox.Show("No existen montos cargados para enviar a Depositar...", "Guardar Depósito");
                            return; 
                        }

                        // se inicia una transaccion para guardar los datos //
                        int sw = 0;

                        //DevExpress.Xpo.XpoDefault.Session.BeginTransaction();
                        
                        try
                        {
                            Guid lg_banco_cuenta = Guid.Empty;
                            Guid lg_responsable_deposito = Guid.Empty;

                            //if (this.lookUp_banco_cuenta.Value != null)
                            if (this.lookUp_banco_cuenta.gridLookUpEdit1.EditValue != null & this.lookUp_banco_cuenta.gridLookUpEdit1.EditValue.ToString() != String.Empty)
                            {
                                //lg_banco_cuenta = (Guid)this.lookUp_banco_cuenta.Value;
                                lg_banco_cuenta = (Guid)this.lookUp_banco_cuenta.gridLookUpEdit1.EditValue;
                            }

                            //if (this.lookUp_responsable_deposito.Value != null)
                            if (this.lookUp_responsable_deposito.gridLookUpEdit1.EditValue != null & this.lookUp_responsable_deposito.gridLookUpEdit1.EditValue.ToString() != String.Empty)
                            {
                                //lg_responsable_deposito = (Guid)this.lookUp_responsable_deposito.Value;
                                lg_responsable_deposito = (Guid)this.lookUp_responsable_deposito.gridLookUpEdit1.EditValue;
                            }

                            ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;

                            ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).banco_cuenta =
                                DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas>(lg_banco_cuenta);

                            ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).responsable_deposito =
                                DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos>(lg_responsable_deposito);

                            ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).elaborado =
                                DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(Fundraising_PT.Properties.Settings.Default.U_oid);
                            
                            ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status = 1;
                            
                            if (dateTime_fecha_hora.dateEdit1.EditValue != null & dateTime_fecha_hora.dateEdit1.EditValue.ToString() != String.Empty)
                            {
                                ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).fecha_hora = (DateTime)this.dateTime_fecha_hora.dateEdit1.EditValue;
                            }
                            else
                            {
                                dateTime_fecha_hora.dateEdit1.DateTime = DateTime.Now;
                                dateTime_fecha_hora.dateEdit1.EditValue = DateTime.Now;
                                ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).fecha_hora = (DateTime)this.dateTime_fecha_hora.dateEdit1.EditValue;
                            }

                            //base.guardar(sender, e);
                            this.dxErrorProvider1.ClearErrors();
                            this.tabControl1.TabPages.Add(tabPage2);
                            bindingSource1.EndEdit();
                            ((XPBaseObject)bindingSource1.Current).Save();
                            //
                            if (lAccion == "Editar")
                            {
                                // graba el detalle del deposito //
                                //MessageBox.Show(totales_deposito.Rows.Count.ToString());
                                //MessageBox.Show(colection_totales_deposito.Count.ToString());
                                //if (val_array_dep_det != null)
                                //{
                                //    if (val_array_dep_det.Rank > 0)
                                //    {
                                //        totales_deposito.LoadDataRow(val_array_dep_det, LoadOption.OverwriteChanges);
                                //    }
                                //}
                                // 
                                //actualiza_detalle_depositos(((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current));
                                ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status = lnStatus_auxiliar;
                                ((XPBaseObject)bindingSource1.Current).Save();
                            }

                            //DevExpress.Xpo.XpoDefault.Session.CommitTransaction(); // cierra la transaccion de guardar el deposito y su detalle //

                            this_primary_object_persistent_current.Session.FlushChanges();
                            this_primary_object_persistent_current.Session.ExplicitCommitTransaction();

                            // graba el detalle del deposito //
                            try
                            {
                                if (lAccion == "Editar")
                                {
                                    actualiza_detalle_depositos(((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current));
                                }
                                MessageBox.Show("Datos del Depósito Guardados Correctamente...", "Guardar Depósito");
                                sw = 1;
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Error!!! Guardando detalle del Depósito..." + Environment.NewLine + "Favor revisar detalle del deposito y saldos por depositar...", "Guardar Depósito");
                                sw = 0;
                            }
                            //
                            //MessageBox.Show("Datos del Depósito Guardados Correctamente...", "Guardar Depósito");
                            //sw = 1;
                            this.lAccion = "Navegando";
                            this.picture_totales_efectivo.Enabled = true;
                            this.picture_totales_cheque.Enabled = true;
                            this.picture_totales_tickets.Enabled = true;
                            //
                            depositos_bancarios.Reload();
                            viewcodigointegrado();
                            if (bindingSource1.Count > 0)
                            {
                                lg_deposito_bancario_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                                lg_dep_ban = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                                Control_Mode(0);
                                seteo_status_deposito();
                                calcula_totales();
                            }
                            else
                            {
                                lg_deposito_bancario_aux = Guid.Empty;
                                lg_dep_ban = Guid.Empty;
                                Control_Mode(2);
                            }
                            this.Refresh();
                            HeaderMenu.Caption = Fundraising_PT.Properties.Settings.Default.Nombre_Sistema.Trim() + " - Módulo : " + this.Text.Trim() + " - Acción : " + this.lAccion;
                            //

                            if (sw == 1) // se guardo el deposito satisfactoriamente y procede a recostruir los saldos //
                            {
                                try
                                {
                                    // se inicia la transaccion para reconstruir columna de depositado en la tabla de saldos//
                                    DevExpress.Xpo.XpoDefault.Session.BeginTransaction();

                                    XPView recaudadores_aux = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det));
                                    recaudadores_aux.AddProperty("deposito_bancario", "deposito_bancario", true, true, DevExpress.Xpo.SortDirection.None);
                                    recaudadores_aux.AddProperty("recaudador", "recaudador", true, true, DevExpress.Xpo.SortDirection.None);
                                    recaudadores_aux.AddProperty("forma_pago", "forma_pago", true, true, DevExpress.Xpo.SortDirection.None);
                                    recaudadores_aux.Criteria = CriteriaOperator.Parse(string.Format("deposito_bancario.oid = '{0}'", ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid));
                                    //
                                    foreach (ViewRecord items in recaudadores_aux)
                                    {
                                        Guid oid_recaudador = (Guid)items["recaudador"];
                                        Guid oid_forma_pago = (Guid)items["forma_pago"];
                                        Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(2, ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).fecha_hora, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(oid_recaudador), DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(oid_forma_pago));
                                    }

                                    DevExpress.Xpo.XpoDefault.Session.CommitTransaction();
                                }
                                catch (Exception oerror)
                                {
                                    MessageBox.Show("Ocurrio un ERROR durante el proceso de reconstruccion de saldos, se reversara dicho proceso..." + Environment.NewLine + "favor revisar la tabla de saldos y hacer los ajustes necesarios..." + Environment.NewLine + "Error: " + oerror.Message, "Reconstrucción de saldos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    DevExpress.Xpo.XpoDefault.Session.RollbackTransaction();
                                }
                            }
                            sw = 0;
                        }
                        catch (Exception oerror)
                        {
                            MessageBox.Show("Ocurrio un ERROR durante el proceso de guardar los datos del depósito, se reversara dicho proceso... " + Environment.NewLine + oerror.Message, "Guardar Depósito");
                            
                            //DevExpress.Xpo.XpoDefault.Session.RollbackTransaction();

                            if (this_primary_object_persistent_current.Session.InTransaction)
                            {
                                this_primary_object_persistent_current.Session.DropChanges();
                                this_primary_object_persistent_current.Session.ExplicitRollbackTransaction();
                            }

                            ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).Reload();

                            this.lAccion = "Navegando";
                            this.picture_totales_efectivo.Enabled = true;
                            this.picture_totales_cheque.Enabled = true;
                            this.picture_totales_tickets.Enabled = true;
                            //
                            depositos_bancarios.Reload();
                            viewcodigointegrado();
                            if (bindingSource1.Count > 0)
                            {
                                lg_deposito_bancario_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                                lg_dep_ban = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                                Control_Mode(0);
                                seteo_status_deposito();
                                calcula_totales();
                            }
                            else
                            {
                                lg_deposito_bancario_aux = Guid.Empty;
                                lg_dep_ban = Guid.Empty;
                                Control_Mode(2);
                            }
                            this.Refresh();
                            HeaderMenu.Caption = Fundraising_PT.Properties.Settings.Default.Nombre_Sistema.Trim() + " - Módulo : " + this.Text.Trim() + " - Acción : " + this.lAccion;
                            
                            sw = 0;
                        }
                    }
                }
            }
        }

        private void actualiza_detalle_depositos(Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios obj_deposito)
        {
            // declaracion de variables de busqueda //
            lg_deposito_bancario_aux = obj_deposito.oid;
            lg_dep_ban = obj_deposito.oid;
            Guid lg_recaudador = Guid.Empty;
            Guid lg_forma_pago       = Guid.Empty;
            orden_deposito_bancario_det_aux = (new DevExpress.Xpo.SortProperty("forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            // recorre la tabla del detalle del deposito //
            foreach (DataRow row_totales_deposito in totales_deposito.Rows)
            {
                //MessageBox.Show(row_totales_deposito["forma_pago"].ToString() + " " + row_totales_deposito["recaudador"].ToString() + " " + row_totales_deposito["monto"].ToString() + " " + row_totales_deposito["saldo"].ToString());
                //DevExpress.Xpo.DB.SelectedData resul_data0 = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("select oid from depositos_bancarios_det where deposito_bancario = '{0}' and recaudador = '{1}' and forma_pago = '{2}'", loid_deposito_bancarios, loid_recaudador, loid_forma_pago));

                // busca registro en el objeto persistente del detalle de deposito para actualizarlo //
                lg_recaudador = (Guid)row_totales_deposito["recaudador"];
                lg_forma_pago = (Guid)row_totales_deposito["forma_pago"];
                filtro_deposito_bancario_det_aux = (new OperandProperty("deposito_bancario.oid") == new OperandValue(lg_deposito_bancario_aux) & new OperandProperty("recaudador.oid") == new OperandValue(lg_recaudador) & new OperandProperty("forma_pago.oid") == new OperandValue(lg_forma_pago));
                depositos_bancarios_det_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det>(DevExpress.Xpo.XpoDefault.Session, filtro_deposito_bancario_det_aux, orden_deposito_bancario_det_aux);
                //
                if (depositos_bancarios_det_aux.Count <= 0)
                {
                    depositos_bancarios_det_aux.Add(new Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det(DevExpress.Xpo.XpoDefault.Session));
                    depositos_bancarios_det_aux[0].deposito_bancario = obj_deposito;
                    depositos_bancarios_det_aux[0].recaudador = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(lg_recaudador);
                    depositos_bancarios_det_aux[0].forma_pago = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago);
                    depositos_bancarios_det_aux[0].sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                }
                if ((decimal)row_totales_deposito["monto"] > 0)
                {
                    depositos_bancarios_det_aux[0].monto_precargado = ((decimal)row_totales_deposito["monto_precargado"]);
                    depositos_bancarios_det_aux[0].monto = ((decimal)row_totales_deposito["monto"]);
                    depositos_bancarios_det_aux[0].saldo = ((decimal)row_totales_deposito["saldo"]);
                    depositos_bancarios_det_aux[0].Save();
                }
                else
                {
                    depositos_bancarios_det_aux[0].Delete();
                    depositos_bancarios_det_aux.Session.Save(depositos_bancarios_det_aux);
                }
                //                              
            } // final del foreach la tabla del detalle del deposito //
            //
            //
            // recorre las tablas del desgloce de billetes y monedas del (DEPOSITO-EFECTIVO) //
            orden_deposito_bancario_det_des_aux = (new DevExpress.Xpo.SortProperty("tipo_fp", DevExpress.Xpo.DB.SortingDirection.Ascending));
            filtro_deposito_bancario_det_des_aux = (new OperandProperty("depositos_bancarios.oid") == new OperandValue(lg_deposito_bancario_aux) & new OperandProperty("tipo_fp") == new OperandValue(1));
            depositos_bancarios_det_des_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det_Des>(DevExpress.Xpo.XpoDefault.Session, filtro_deposito_bancario_det_des_aux, orden_deposito_bancario_det_des_aux);
            //            
            if (billetes_aux.Rows.Count > 0)
            {
                // guarda datos del detalle de efectivo en billetes //
                foreach (DataRow item_billetes in billetes_aux.Rows)
                {
                    CriteriaOperator filtro_deposito_det_des_cantidad = (new OperandProperty("denominacion_moneda.oid") == new OperandValue((Guid)item_billetes["oid"]));
                    deposito_det_des_efectivo_cantidad = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det_Des>(depositos_bancarios_det_des_aux, filtro_deposito_det_des_cantidad);
                    //
                    if (deposito_det_des_efectivo_cantidad != null && deposito_det_des_efectivo_cantidad.Count > 0)
                    { 
                        if ((int)item_billetes["cantidad"] > 0)
                        {
                            deposito_det_des_efectivo_cantidad[0].sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                            deposito_det_des_efectivo_cantidad[0].cantidad = (int)item_billetes["cantidad"];
                            deposito_det_des_efectivo_cantidad[0].Save();
                        }
                        else
                        {
                            deposito_det_des_efectivo_cantidad[0].Delete();
                            deposito_det_des_efectivo_cantidad.Session.Save(deposito_det_des_efectivo_cantidad);
                        }
                    }
                    else 
                    {
                        deposito_det_des_efectivo_cantidad.Add(new Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det_Des(DevExpress.Xpo.XpoDefault.Session));
                        deposito_det_des_efectivo_cantidad[0].depositos_bancarios = obj_deposito;
                        deposito_det_des_efectivo_cantidad[0].denominacion_moneda = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas>((Guid)item_billetes["oid"]);
                        deposito_det_des_efectivo_cantidad[0].denominacion = 0;
                        deposito_det_des_efectivo_cantidad[0].cantidad = (int)item_billetes["cantidad"];
                        deposito_det_des_efectivo_cantidad[0].tipo_fp = 1;
                        deposito_det_des_efectivo_cantidad[0].sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                        deposito_det_des_efectivo_cantidad[0].Save();
                    }
                    //
                }
            }

            if (monedas_aux.Rows.Count > 0)
            {
                // guarda datos del detalle de efectivo en monedas //
                foreach (DataRow item_monedas in monedas_aux.Rows)
                {
                    CriteriaOperator filtro_deposito_det_des_cantidad = (new OperandProperty("denominacion_moneda.oid") == new OperandValue((Guid)item_monedas["oid"]));
                    deposito_det_des_efectivo_cantidad = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det_Des>(depositos_bancarios_det_des_aux, filtro_deposito_det_des_cantidad);
                    //
                    if (deposito_det_des_efectivo_cantidad != null && deposito_det_des_efectivo_cantidad.Count > 0)
                    {
                        if ((int)item_monedas["cantidad"] > 0)
                        {
                            deposito_det_des_efectivo_cantidad[0].sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                            deposito_det_des_efectivo_cantidad[0].cantidad = (int)item_monedas["cantidad"];
                            deposito_det_des_efectivo_cantidad[0].Save();
                        }
                        else
                        {
                            deposito_det_des_efectivo_cantidad[0].Delete();
                            deposito_det_des_efectivo_cantidad.Session.Save(deposito_det_des_efectivo_cantidad);
                        }
                    }
                    else
                    {
                        deposito_det_des_efectivo_cantidad.Add(new Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det_Des(DevExpress.Xpo.XpoDefault.Session));
                        deposito_det_des_efectivo_cantidad[0].depositos_bancarios = obj_deposito;
                        deposito_det_des_efectivo_cantidad[0].denominacion_moneda = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas>((Guid)item_monedas["oid"]);
                        deposito_det_des_efectivo_cantidad[0].denominacion = 0;
                        deposito_det_des_efectivo_cantidad[0].cantidad = (int)item_monedas["cantidad"];
                        deposito_det_des_efectivo_cantidad[0].tipo_fp = 1;
                        deposito_det_des_efectivo_cantidad[0].sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                        deposito_det_des_efectivo_cantidad[0].Save();
                    }
                    //
                }
            }
        }

        public void seteo_status_deposito()
        {
            this.label_estatus.Text = " "+((Fundraising_PTDM.Enums.EStatus_deposito)((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status).ToString();
            this.label_estatus.Appearance.ImageIndex = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status;
            //
            switch (((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).status)
            {
                case 1:
                    this.label_estatus.Appearance.ForeColor = Color.GreenYellow;
                    this.barra_Mant_Base11.boton_editar.Enabled = this.barra_Mant_Base11.lEditar;
                    this.barra_Mant_Base11.boton_eliminar.Enabled = this.barra_Mant_Base11.lEliminar;
                    break;
                case 2:
                    this.label_estatus.Appearance.ForeColor = Color.DeepSkyBlue;
                    this.barra_Mant_Base11.boton_editar.Enabled = false;
                    this.barra_Mant_Base11.boton_eliminar.Enabled = this.barra_Mant_Base11.lEliminar;
                    break;
                case 3:
                    this.label_estatus.Appearance.ForeColor = Color.Red;
                    this.barra_Mant_Base11.boton_editar.Enabled = false;
                    this.barra_Mant_Base11.boton_eliminar.Enabled = false;
                    break;
                case 4:
                    this.label_estatus.Appearance.ForeColor = Color.WhiteSmoke;
                    this.barra_Mant_Base11.boton_editar.Enabled = false;
                    this.barra_Mant_Base11.boton_eliminar.Enabled = false;
                    break;
                default:
                    this.label_estatus.Appearance.ForeColor = Color.GreenYellow;
                    this.barra_Mant_Base11.boton_editar.Enabled = this.barra_Mant_Base11.lEditar;
                    this.barra_Mant_Base11.boton_eliminar.Enabled = this.barra_Mant_Base11.lEliminar;
                    break;
            }
        }

        public void calcula_totales()
        {
            ln_total_efectivo = 0;
            ln_total_cheques = 0;
            ln_total_tickets = 0;
            ln_total_general = 0;
            //
            //lg_deposito_bancario_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
            //lg_dep_ban = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
            vtotales = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det));
            vtotales.AddProperty("tipo_forma_pago", "forma_pago.tpago", true, true, DevExpress.Xpo.SortDirection.Ascending);
            vtotales.AddProperty("monto_total", "Sum(monto)", false, true, DevExpress.Xpo.SortDirection.None);
            vtotales.Criteria = CriteriaOperator.Parse(string.Format("deposito_bancario.oid = '{0}'", lg_dep_ban));
            //
            if (vtotales.Count > 0)
            {
                foreach (ViewRecord item_vtotales in vtotales)
                {
                    int tipo = (int)item_vtotales["tipo_forma_pago"];
                    switch (tipo)
                    {
                        case 1:
                            ln_total_efectivo = (decimal)item_vtotales["monto_total"];
                            break;
                        case 3:
                            ln_total_cheques = (decimal)item_vtotales["monto_total"];
                            break;
                        case 7:
                            ln_total_tickets = (decimal)item_vtotales["monto_total"];
                            break;
                        default:
                            break;
                    }
                }
            }
            ln_total_general = ln_total_efectivo + ln_total_cheques + ln_total_tickets;
            //
            label_totales_monto_efectivo.Text = ln_total_efectivo.ToString("###,###,###,##0.00");
            label_totales_monto_cheque.Text = ln_total_cheques.ToString("###,###,###,##0.00");
            label_totales_monto_ticket.Text = ln_total_tickets.ToString("###,###,###,##0.00");
            label_total_general.Text = ln_total_general.ToString("###,###,###,##0.00");

            // se asignan montos a las variables de totales iniciales, si no se han asignado... // 
            if (ln_total_efectivo_ini <= 0)
                { ln_total_efectivo_ini = ln_total_efectivo; }

            if (ln_total_cheques_ini <= 0)
                { ln_total_cheques_ini = ln_total_cheques; }

            if (ln_total_tickets_ini <= 0)
                { ln_total_tickets_ini = ln_total_tickets; }

            if (ln_total_general_ini <= 0)
                { ln_total_general_ini = ln_total_general; }

            // crea la vista desde el objeto persistente Depositos_Bancarios_Det, para cargar el datatable de totales_depositos 
            vtotales_deposito = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det));
            vtotales_deposito.AddProperty("forma_pago", "forma_pago.oid", false, true, DevExpress.Xpo.SortDirection.Ascending);
            vtotales_deposito.AddProperty("recaudador", "recaudador.oid", false, true, DevExpress.Xpo.SortDirection.Ascending);
            vtotales_deposito.AddProperty("monto_precargado", "monto_precargado", false, true, DevExpress.Xpo.SortDirection.None);
            vtotales_deposito.AddProperty("monto", "monto", false, true, DevExpress.Xpo.SortDirection.None);
            vtotales_deposito.AddProperty("saldo", "saldo", false, true, DevExpress.Xpo.SortDirection.None);
            vtotales_deposito.Criteria = CriteriaOperator.Parse(string.Format("deposito_bancario.oid = '{0}'", lg_dep_ban));
        
            // carga datos al DATATABLE desde la VISTA //
            totales_deposito.Rows.Clear();
            foreach (ViewRecord item_vtotales_deposito in vtotales_deposito)
            {
                totales_deposito.Rows.Add(
                                                (Guid)item_vtotales_deposito["forma_pago"],
                                                (Guid)item_vtotales_deposito["recaudador"],
                                                (decimal)item_vtotales_deposito["monto_precargado"],
                                                (decimal)item_vtotales_deposito["monto"],
                                                (decimal)item_vtotales_deposito["saldo"]
                                            );
            }
        }

        private void seteo_nivel_seguridad()
        {
            //orden_deposito_bancario = (new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Descending));
            switch (Fundraising_PT.Properties.Settings.Default.U_tipo)
            {
                case 1:
                    filtro_deposito_bancario = CriteriaOperator.Parse("1=1");
                    break;
                default:
                    filtro_deposito_bancario = (new OperandProperty("elaborado.oid") == new OperandValue(Fundraising_PT.Properties.Settings.Default.U_oid));
                    break;
            }
            depositos_bancarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios>(DevExpress.Xpo.XpoDefault.Session, filtro_deposito_bancario, orden_deposito_bancario);
            //bindingSource1.DataSource = depositos_bancarios;
            depositos_bancarios.Reload();
            depositos_bancarios.Sorting = new SortingCollection(orden_deposito_bancario);
            bindingSource1.DataSource = depositos_bancarios;
            filter_sucursales();
            if (depositos_bancarios.Count > 0)
            { 
                lg_deposito_bancario_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
                lg_dep_ban = ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).oid;
            }
        }

        public override void filter_sucursales()
        {
            //depositos_bancarios.CriteriaString = Fundraising_PT.Properties.Settings.Default.sucursal_filter;
            //depositos_bancarios.CriteriaString = my_sucursal_filter;
            depositos_bancarios.CriteriaString = my_sucursal_filter + " and (" + filtro_deposito_bancario.ToString().Trim() + ")";
            depositos_bancarios.Reload();
            depositos_bancarios.Sorting = new SortingCollection(orden_deposito_bancario);
            //
            bindingSource1.DataSource = depositos_bancarios;
            bindingSource1.MoveFirst();

        }

        public void desgloce_efectivo_process(object form_activo, int parent_process, decimal total_monto_efectivo)
        {
            orden_deposito_bancario_det_des_aux = (new DevExpress.Xpo.SortProperty("tipo_fp", DevExpress.Xpo.DB.SortingDirection.Ascending));
            filtro_deposito_bancario_det_des_aux = (new OperandProperty("depositos_bancarios.oid") == new OperandValue(lg_deposito_bancario_aux) & new OperandProperty("tipo_fp") == new OperandValue(1));
            depositos_bancarios_det_des_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det_Des>(DevExpress.Xpo.XpoDefault.Session, filtro_deposito_bancario_det_des_aux, orden_deposito_bancario_det_des_aux);
            //            
            if (billetes_aux.Rows.Count <= 0)
            {
                // carga datos del detalle de efectivo en billetes //
                foreach (var item_billetes in denominacion_monedas_billetes)
                {
                    ln_deposito_det_des_cantidad = 0;
                    //
                    CriteriaOperator filtro_deposito_det_des_cantidad = (new OperandProperty("denominacion_moneda.oid") == new OperandValue(item_billetes.oid));
                    deposito_det_des_efectivo_cantidad = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det_Des>(depositos_bancarios_det_des_aux, filtro_deposito_det_des_cantidad);
                    //
                    if (deposito_det_des_efectivo_cantidad != null && deposito_det_des_efectivo_cantidad.Count > 0)
                    { ln_deposito_det_des_cantidad = deposito_det_des_efectivo_cantidad[0].cantidad; }
                    //
                    billetes_aux.Rows.Add(item_billetes.oid, item_billetes.codigo, item_billetes.valor, ln_deposito_det_des_cantidad);
                }
            }

            if (monedas_aux.Rows.Count <= 0)
            {
                // carga datos del detalle de efectivo en monedas //
                foreach (var item_monedas in denominacion_monedas_monedas)
                {
                    ln_deposito_det_des_cantidad = 0;
                    //
                    CriteriaOperator filtro_deposito_det_des_cantidad = (new OperandProperty("denominacion_moneda.oid") == new OperandValue(item_monedas.oid));
                    deposito_det_des_efectivo_cantidad = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det_Des>(depositos_bancarios_det_des_aux, filtro_deposito_det_des_cantidad);
                    //
                    if (deposito_det_des_efectivo_cantidad != null && deposito_det_des_efectivo_cantidad.Count > 0)
                    { ln_deposito_det_des_cantidad = deposito_det_des_efectivo_cantidad[0].cantidad; }
                    //
                    monedas_aux.Rows.Add(item_monedas.oid, item_monedas.codigo, item_monedas.valor, ln_deposito_det_des_cantidad);
                }
            }
            //
            Formularios.UI_Depositos_Bancarios_Det_Des form_depositos_bancarios_det_des = new Fundraising_PT.Formularios.UI_Depositos_Bancarios_Det_Des(form_activo, 1, parent_process, ref this.billetes_aux, ref this.monedas_aux, total_monto_efectivo);
            //form_depositos_bancarios_det_des.MdiParent = this.MdiParent;
            this.barra_Mant_Base11.Enabled = false;
            this.picture_totales_efectivo.Enabled = false;
            this.picture_totales_cheque.Enabled = false;
            this.picture_totales_tickets.Enabled = false;
            this.simpleButton_desgloce_efectivo.Enabled = false;
            this.grid_Base11.Enabled = false;
            this.ControlBox = false;
            //form_depositos_bancarios_det_des.Show();
            form_depositos_bancarios_det_des.ShowDialog(this);
        }

        private void viewcodigointegrado()
        {

            if (((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current) != null)
            {
                lc_codigointegrado = (((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).sucursal == null ? "Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).sucursal.ToString().Trim()) + (((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).banco_cuenta == null ? "-Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).banco_cuenta.codigo_cuenta.Trim()) + (((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).nro_deposito == null ? "-Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios)bindingSource1.Current).nro_deposito.Trim());
            }
            else
            {
                lc_codigointegrado = string.Empty;
            }
            this.label_contenidocodigointegrado.lText = " " + lc_codigointegrado;
        }

        public override void objects_disposes()
        {
            base.objects_disposes();
            sucursales.Dispose();
            usuarios.Dispose();
            responsable_depositos.Dispose();
            bancos_cuentas.Dispose();
            //
            if (depositos_bancarios_aux != null)
            { depositos_bancarios_aux.Dispose(); }

            if (depositos_bancarios != null)
            { depositos_bancarios.Dispose(); }

            if (depositos_bancarios_det_aux != null)
            { depositos_bancarios_det_aux.Dispose(); }

            if (depositos_bancarios_det != null)
            { depositos_bancarios_det.Dispose(); }

            if (depositos_bancarios_det_des_aux != null)
            { depositos_bancarios_det_des_aux.Dispose(); }

            if (deposito_det_des_efectivo_cantidad != null)
            { deposito_det_des_efectivo_cantidad.Dispose(); }

            if (denominacion_monedas != null)
            { denominacion_monedas.Dispose(); }

            if (denominacion_monedas_billetes != null)
            { denominacion_monedas_billetes.Dispose(); }

            if (denominacion_monedas_monedas != null)
            { denominacion_monedas_monedas.Dispose(); }
            //
            bindingSource1.Dispose();
        }

        public override void datareload()
        {
            base.datareload();
            //
            sucursales.Load();
            sucursales.Reload();
            usuarios.Load();
            usuarios.Reload();
            responsable_depositos.Load();
            responsable_depositos.Reload();
            bancos_cuentas.Load();
            bancos_cuentas.Reload();
            //
            if (depositos_bancarios_aux != null)
            {
                depositos_bancarios_aux.Load();
                depositos_bancarios_aux.Reload();
            }

            if (depositos_bancarios != null)
            {
                depositos_bancarios.Load();
                depositos_bancarios.Reload();
            }

            if (depositos_bancarios_det_aux != null)
            {
                depositos_bancarios_det_aux.Load();
                depositos_bancarios_det_aux.Reload();
            }

            if (depositos_bancarios_det != null)
            {
                depositos_bancarios_det.Load();
                depositos_bancarios_det.Reload();
            }

            if (depositos_bancarios_det_des_aux != null)
            {
                depositos_bancarios_det_des_aux.Load();
                depositos_bancarios_det_des_aux.Reload();
            }

            if (deposito_det_des_efectivo_cantidad != null)
            {
                deposito_det_des_efectivo_cantidad.Load();
                deposito_det_des_efectivo_cantidad.Reload();
            }

            if (denominacion_monedas != null)
            {
                denominacion_monedas.Load();
                denominacion_monedas.Reload();
            }

            if (denominacion_monedas_billetes != null)
            {
                denominacion_monedas_billetes.Load();
                denominacion_monedas_billetes.Reload();
            }

            if (denominacion_monedas_monedas != null)
            {
                denominacion_monedas_monedas.Load();
                denominacion_monedas_monedas.Reload();
            }
            //
            this.lookUpEdit_sucursales.Properties.DataSource = Fundraising_PT.Clases.Setting_Sucursales.data_sucursales();
            //
            filter_sucursales();
        }

        //public void llena_totales_depositos(object[] aux_var_array)
        //{
        //    if (aux_var_array != null)
        //    {
        //        if (aux_var_array.Rank > 0)
        //        {
        //            totales_deposito.LoadDataRow(aux_var_array, LoadOption.OverwriteChanges);
        //        }
        //    }
        //    //if (val_array_dep_det != null)
        //    //{
        //    //    if (val_array_dep_det.Rank > 0)
        //    //    {
        //    //        totales_deposito.LoadDataRow(val_array_dep_det, LoadOption.OverwriteChanges);
        //    //    }
        //    //}
        //}

        private void contenedorDatos2_Load(object sender, EventArgs e)
        {

        }
    }
}
