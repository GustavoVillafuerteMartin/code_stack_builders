using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using Fundraising_PT.Reports;
using DevExpress.XtraReports.UI;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Report_Deposito : DevExpress.XtraEditors.XtraForm
    {
        // declaracion de objetos publicos //
        public DevExpress.XtraBars.BarButtonItem OpcionMenu;
        public DevExpress.XtraBars.BarHeaderItem HeaderMenu;
        public object ObjetoExtra;
        public string lHeader_ant = "";
        public string lAccion = "Reporte de Dépositos Bancarios";
       
        // variables para el filtro de los datos del reporte //
        public string lfiltro_report = "";
        public string lfiltro_cuenta = "";
        public string lfecha_desde = "";
        public string lfecha_hasta = "";
        public string lbanco = "";
        public string lcodigointegrado = "";
        public string lsucursal = "";
        public string lcuenta = "";
        public string lresponsable = "";
        public string lelaborado = "";
        public string lrevisado = "";
        public string lndeposito = "";
        public string lncataporte = "";
        public string lstatus = "";
        public Guid loid_banco = Guid.Empty;
        public Guid loid_cuenta = Guid.Empty;
        public Guid loid_responsable = Guid.Empty;
        public Guid loid_elaborado = Guid.Empty;
        public Guid loid_revisado = Guid.Empty;
        public string nro_deposito = "";
        public string nro_cataporte = "";
        public int lnstatus = 0;
        public int lnsucursal = 0;
        public bool llview_tot_fec = true;
        public bool llview_gran_tot = true;
        public int ln_ordenfecha = 1;
        //
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios> depositos;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos> bancos;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas> cuentas;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos> responsables;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> elaborados;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> revisados;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales> sucursales;
        //
        private DevExpress.Xpo.SortingCollection orden_fecha_ascendente_depositos = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Ascending));
        private DevExpress.Xpo.SortingCollection orden_fecha_descendente_depositos = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Descending));

        public UI_Report_Deposito(DevExpress.XtraBars.BarButtonItem opcionMenu, DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Reporte de Dépositos Bancarios...");
            //
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //
            OpcionMenu = opcionMenu;
            HeaderMenu = headerMenu;
            ObjetoExtra = objetoExtra;
            lHeader_ant = HeaderMenu.Caption;
            //
            CriteriaOperator filtro_status = (new OperandProperty("status") == new OperandValue(1));
            CriteriaOperator filtro_depositos = CriteriaOperator.Parse("1 = 2");
            SortProperty orden_depositos = (new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            orden_fecha_ascendente_depositos.Add(new DevExpress.Xpo.SortProperty("sucursal", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_fecha_ascendente_depositos.Add(new DevExpress.Xpo.SortProperty("banco_cuenta.banco.nombre", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_fecha_ascendente_depositos.Add(new DevExpress.Xpo.SortProperty("banco_cuenta.codigo_cuenta", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            orden_fecha_descendente_depositos.Add(new DevExpress.Xpo.SortProperty("sucursal", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_fecha_descendente_depositos.Add(new DevExpress.Xpo.SortProperty("banco_cuenta.banco.nombre", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_fecha_descendente_depositos.Add(new DevExpress.Xpo.SortProperty("banco_cuenta.codigo_cuenta", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            depositos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios>(DevExpress.Xpo.XpoDefault.Session, filtro_depositos, orden_depositos);
            depositos.Sorting = orden_fecha_ascendente_depositos;
            //
            sucursales = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            bancos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("nombre", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cuentas = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("codigo_cuenta", DevExpress.Xpo.DB.SortingDirection.Ascending));
            responsables = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("nombre", DevExpress.Xpo.DB.SortingDirection.Ascending));
            elaborados = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("usuario", DevExpress.Xpo.DB.SortingDirection.Ascending));
            revisados = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("usuario", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            bindingSource_sucursales.DataSource = sucursales;
            bindingSource_banco.DataSource = bancos;
            bindingSource_cuenta.DataSource = cuentas;
            bindingSource_responsable.DataSource = responsables;
            bindingSource_elaborado.DataSource = elaborados;
            bindingSource_revisado.DataSource = revisados;
        }

        private void UI_Report_Deposito_Load(object sender, EventArgs e)
        {
            dateTime_fecha_desde.EditValue = DateTime.Now;
            dateTime_fecha_hasta.EditValue = DateTime.Now;
            //
            lookUpEdit_modeloreport.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue("ETipo_Report");
            lookUpEdit_modeloreport.Properties.DisplayMember = "Descripcion";
            lookUpEdit_modeloreport.Properties.ValueMember = "Valor";
            lookUpEdit_modeloreport.EditValue = 0;
            //
            lookUpEdit_status.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue("EStatus_deposito");
            lookUpEdit_status.Properties.DisplayMember = "Descripcion";
            lookUpEdit_status.Properties.ValueMember = "Valor";
            //
            checkEdit_todossucursal.CheckedChanged += new EventHandler(checkEdit_todossucursal_CheckedChanged);
            checkEdit_todosfecha.CheckedChanged += new EventHandler(checkEdit_todosfecha_CheckedChanged);
            checkEdit_todosbanco.CheckedChanged += new EventHandler(checkEdit_todosbanco_CheckedChanged);
            checkEdit_todoscuenta.CheckedChanged += new EventHandler(checkEdit_todoscuenta_CheckedChanged);
            checkEdit_todosresponsable.CheckedChanged += new EventHandler(checkEdit_todosresponsable_CheckedChanged);
            checkEdit_todoselaborado.CheckedChanged += new EventHandler(checkEdit_todoselaborado_CheckedChanged);
            checkEdit_todosrevisado.CheckedChanged += new EventHandler(checkEdit_todosrevisado_CheckedChanged);
            checkEdit_todosndepositos.CheckedChanged += new EventHandler(checkEdit_todosndepositos_CheckedChanged);
            checkEdit_todosncataporte.CheckedChanged += new EventHandler(checkEdit_todosncataporte_CheckedChanged);
            checkEdit_todosstatus.CheckedChanged += new EventHandler(checkEdit_todosstatus_CheckedChanged);
            //
            lookUpEdit_banco.EditValueChanged += new EventHandler(lookUpEdit_banco_EditValueChanged);
            //
            checkButton_order_fecha_ascendente.CheckedChanged += new EventHandler(checkButton_order_fecha_ascendente_CheckedChanged);
            checkButton_order_fecha_descendente.CheckedChanged += new EventHandler(checkButton_order_fecha_descendente_CheckedChanged);
            //
            simpleButton_imprimir.Click += new EventHandler(simpleButton_imprimir_Click);
            simpleButton_salir.Click += new EventHandler(simpleButton_salir_Click);
            //
            setea_checks();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void checkEdit_todossucursal_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todossucursal.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_sucursal.EditValue = null;
                this.lookUpEdit_sucursal.Enabled = false;
            }
            else
            {
                if (bindingSource_sucursales.Count > 0)
                {
                    this.lookUpEdit_sucursal.Enabled = true;
                    this.lookUpEdit_sucursal.Focus();
                }
                else
                { this.lookUpEdit_sucursal.Enabled = true; }
            }

        }
        
        void checkButton_order_fecha_ascendente_CheckedChanged(object sender, EventArgs e)
        {
            if (checkButton_order_fecha_ascendente.Checked == true)
            {
                checkButton_order_fecha_descendente.Checked = false;
            }
            else
            {
                checkButton_order_fecha_descendente.Checked = true;
            }
        }

        void checkButton_order_fecha_descendente_CheckedChanged(object sender, EventArgs e)
        {
            if (checkButton_order_fecha_descendente.Checked == true)
            {
                checkButton_order_fecha_ascendente.Checked = false;
            }
            else
            {
                checkButton_order_fecha_ascendente.Checked = true;
            }
        }

        void lookUpEdit_banco_EditValueChanged(object sender, EventArgs e)
        {
            lfiltro_cuenta = "status = 1";
            if (lookUpEdit_banco.EditValue != null)
            {
                loid_banco = (Guid)lookUpEdit_banco.EditValue;
                lfiltro_cuenta = lfiltro_cuenta + " and " + string.Format("banco.oid = '{0}'", loid_banco);
            }
            cuentas.Criteria = CriteriaOperator.Parse(lfiltro_cuenta);
            this.checkEdit_todoscuenta.CheckState = CheckState.Checked;
            this.lookUpEdit_cuenta.EditValue = null;
            this.lookUpEdit_cuenta.Enabled = false;
            setea_checks();
        }

        void checkEdit_todosfecha_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todosfecha.CheckState == CheckState.Checked)
            {
                this.dateTime_fecha_desde.Enabled = false;
                this.dateTime_fecha_hasta.Enabled = false;
            }
            else
            {
                this.dateTime_fecha_desde.Enabled = true;
                this.dateTime_fecha_hasta.Enabled = true;
                this.dateTime_fecha_desde.Focus();
            }
        }

        void checkEdit_todosbanco_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todosbanco.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_banco.EditValue = null;
                this.lookUpEdit_banco.Enabled = false;
            }
            else
            {
                if (bindingSource_banco.Count > 0)
                {
                    this.lookUpEdit_banco.Enabled = true;
                    this.lookUpEdit_banco.Focus();
                }
                else
                { this.lookUpEdit_banco.Enabled = false; }
            }
        }

        void checkEdit_todoscuenta_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todoscuenta.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_cuenta.EditValue = null;
                this.lookUpEdit_cuenta.Enabled = false;
            }
            else
            {
                if (bindingSource_cuenta.Count > 0)
                {
                    this.lookUpEdit_cuenta.Enabled = true;
                    this.lookUpEdit_cuenta.Focus();
                }
                else
                { this.lookUpEdit_cuenta.Enabled = false; }
            }
        }

        void checkEdit_todosresponsable_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todosresponsable.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_responsable.EditValue = null;
                this.lookUpEdit_responsable.Enabled = false;
            }
            else
            {
                if (bindingSource_responsable.Count > 0)
                {
                    this.lookUpEdit_responsable.Enabled = true;
                    this.lookUpEdit_responsable.Focus();
                }
                else
                { this.lookUpEdit_responsable.Enabled = false; }
            }
        }

        void checkEdit_todoselaborado_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todoselaborado.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_elaborado.EditValue = null;
                this.lookUpEdit_elaborado.Enabled = false;
            }
            else
            {
                if (bindingSource_elaborado.Count > 0)
                {
                    this.lookUpEdit_elaborado.Enabled = true;
                    this.lookUpEdit_elaborado.Focus();
                }
                else
                { this.lookUpEdit_elaborado.Enabled = false; }
            }
        }

        void checkEdit_todosrevisado_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todosrevisado.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_revisado.EditValue = null;
                this.lookUpEdit_revisado.Enabled = false;
            }
            else
            {
                if (bindingSource_revisado.Count > 0)
                {
                    this.lookUpEdit_revisado.Enabled = true;
                    this.lookUpEdit_revisado.Focus();
                }
                else
                { this.lookUpEdit_revisado.Enabled = false; }
            }
        }

        void checkEdit_todosndepositos_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todosndepositos.CheckState == CheckState.Checked)
            {
                this.textEdit_ndeposito.EditValue = null;
                this.textEdit_ndeposito.Enabled = false;
            }
            else
            {
                this.textEdit_ndeposito.Enabled = true;
                this.textEdit_ndeposito.Focus();
            }
        }

        void checkEdit_todosncataporte_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todosncataporte.CheckState == CheckState.Checked)
            {
                this.textEdit_ncataporte.EditValue = null;
                this.textEdit_ncataporte.Enabled = false;
            }
            else
            {
                this.textEdit_ncataporte.Enabled = true;
                this.textEdit_ncataporte.Focus();
            }
        }

        void checkEdit_todosstatus_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todosstatus.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_status.EditValue = null;
                this.lookUpEdit_status.Enabled = false;
            }
            else
            {
                this.lookUpEdit_status.Enabled = true;
                this.lookUpEdit_status.Focus();
            }
        }

        private void simpleButton_imprimir_Click(object sender, EventArgs e)
        {
            //
            llview_tot_fec = (checkButton_view_tot_fechas.Checked);
            llview_gran_tot = (checkButton_view_gran_total.Checked);
            //
            if (this.checkEdit_todosfecha.CheckState == CheckState.Unchecked)
            {
                lfecha_desde = ((DateTime)dateTime_fecha_desde.EditValue).Date.ToShortDateString();
                lfecha_hasta = ((DateTime)dateTime_fecha_hasta.EditValue).Date.ToShortDateString();
                string lfecha_desde1 = lfecha_desde.Substring(6, 4) + lfecha_desde.Substring(3, 2) + lfecha_desde.Substring(0, 2);
                string lfecha_hasta1 = lfecha_hasta.Substring(6, 4) + lfecha_hasta.Substring(3, 2) + lfecha_hasta.Substring(0, 2);
                lfiltro_report = string.Format("ToStr(GetYear(fecha_hora))+PadLeft(ToStr(GetMonth(fecha_hora)),2,'0')+PadLeft(ToStr(GetDay(fecha_hora)),2,'0') >= '{0}' and ToStr(GetYear(fecha_hora))+PadLeft(ToStr(GetMonth(fecha_hora)),2,'0')+PadLeft(ToStr(GetDay(fecha_hora)),2,'0') <= '{1}'", lfecha_desde1, lfecha_hasta1);
                //
                //lfecha_desde = ((DateTime)dateTime_fecha_desde.EditValue).Date.ToShortDateString();
                //lfecha_hasta = ((DateTime)dateTime_fecha_hasta.EditValue).Date.ToShortDateString();
                //lfiltro_report = string.Format("GetDate(fecha_hora) >= '{0}' and GetDate(fecha_hora) <= '{1}'", ((DateTime)dateTime_fecha_desde.EditValue).Date, ((DateTime)dateTime_fecha_hasta.EditValue).Date);
            }
            else
            {
                lfecha_desde = "Todas";
                lfecha_hasta = "Todas";
                lfiltro_report = "1 = 1";
            }
            //
            if (this.textEdit_codigointegrado.Text.Trim() != string.Empty)
            {
                lcodigointegrado = this.textEdit_codigointegrado.Text.Trim();
                lfiltro_report = lfiltro_report + " and " + string.Format("trim(tostr(sucursal))+trim(banco_cuenta.codigo_cuenta)+trim(nro_deposito) = '{0}'", lcodigointegrado);
            }
            else
            { lcodigointegrado = "Todos"; }
            //
            if (this.checkEdit_todossucursal.CheckState == CheckState.Unchecked)
            {
                lsucursal = this.lookUpEdit_sucursal.Text;
                lnsucursal = (int)this.lookUpEdit_sucursal.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("sucursal = {0}", lnsucursal);
            }
            else
            { lsucursal = "Todas"; }
            //
            if (this.checkEdit_todosbanco.CheckState == CheckState.Unchecked)
            {
                lbanco = this.lookUpEdit_banco.Text;
                loid_banco = (Guid)this.lookUpEdit_banco.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("banco_cuenta.banco.oid = '{0}'", loid_banco);
            }
            else
            { lbanco = "Todos"; }
            //
            if (this.checkEdit_todoscuenta.CheckState == CheckState.Unchecked)
            {
                lcuenta = this.lookUpEdit_cuenta.Text;
                loid_cuenta = (Guid)this.lookUpEdit_cuenta.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("banco_cuenta.oid = '{0}'", loid_cuenta);
            }
            else
            { lcuenta = "Todas"; }
            //
            if (this.checkEdit_todosresponsable.CheckState == CheckState.Unchecked)
            {
                lresponsable = this.lookUpEdit_responsable.Text;
                loid_responsable = (Guid)this.lookUpEdit_responsable.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("responsable_deposito.oid = '{0}'", loid_responsable);
            }
            else
            { lresponsable = "Todos"; }
            //
            if (this.checkEdit_todoselaborado.CheckState == CheckState.Unchecked)
            {
                lelaborado = this.lookUpEdit_elaborado.Text;
                loid_elaborado = (Guid)this.lookUpEdit_elaborado.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("elaborado.oid = '{0}'", loid_elaborado);
            }
            else
            { lelaborado = "Todos"; }
            //
            if (this.checkEdit_todosrevisado.CheckState == CheckState.Unchecked)
            {
                lrevisado = this.lookUpEdit_revisado.Text;
                loid_revisado = (Guid)this.lookUpEdit_revisado.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("revisado.oid = '{0}'", loid_revisado);
            }
            else
            { lrevisado = "Todos"; }
            //
            if (this.checkEdit_todosndepositos.CheckState == CheckState.Unchecked & this.textEdit_ndeposito.Text.Trim() != string.Empty)
            {
                lndeposito = this.textEdit_ndeposito.Text.Trim();
                nro_deposito = this.textEdit_ndeposito.Text.Trim();
                lfiltro_report = lfiltro_report + " and " + string.Format("nro_deposito = '{0}'", nro_deposito);
            }
            else
            { lndeposito = "Todos"; }
            //
            if (this.checkEdit_todosncataporte.CheckState == CheckState.Unchecked & this.textEdit_ncataporte.Text.Trim() != string.Empty)
            {
                lncataporte = this.textEdit_ncataporte.Text.Trim();
                nro_cataporte = this.textEdit_ncataporte.Text.Trim();
                lfiltro_report = lfiltro_report + " and " + string.Format("nro_cataporte = '{0}'", nro_cataporte);
            }
            else
            { lncataporte = "Todos"; }
            //
            if (this.checkEdit_todosstatus.CheckState == CheckState.Unchecked)
            {
                lstatus = this.lookUpEdit_status.Text;
                lnstatus = (int)this.lookUpEdit_status.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("status = '{0}'", lnstatus);
            }
            else
            { lstatus = "Todos"; }
            //        
            if (checkButton_order_fecha_ascendente.Checked == true)
            {
                ln_ordenfecha = 1;
                depositos.Sorting = orden_fecha_ascendente_depositos;
            }
            else
            {
                ln_ordenfecha = 2;
                depositos.Sorting = orden_fecha_descendente_depositos;
            }
            //
            depositos.Criteria = CriteriaOperator.Parse(lfiltro_report);
            //
            if (lookUpEdit_modeloreport.EditValue.ToString().Trim() == "0")
            {
                XtraReport_Depositos1 report_depositos = new XtraReport_Depositos1(lfecha_desde, lfecha_hasta, lbanco, lcuenta, lresponsable, lelaborado, lrevisado, lndeposito, lncataporte, lstatus, llview_tot_fec, llview_gran_tot, ln_ordenfecha, lsucursal);
                report_depositos.Landscape = false;
                report_depositos.DataSource = depositos;
                report_depositos.ShowRibbonPreviewDialog();
            }
            else
            {
                XtraReport_Depositos report_depositos = new XtraReport_Depositos(lfecha_desde, lfecha_hasta, lbanco, lcuenta, lresponsable, lelaborado, lrevisado, lndeposito, lncataporte, lstatus, llview_tot_fec, llview_gran_tot, ln_ordenfecha, lsucursal);
                report_depositos.Landscape = true;
                report_depositos.DataSource = depositos;
                report_depositos.ShowRibbonPreviewDialog();
            }
        }

        private void simpleButton_salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (OpcionMenu != null)
                OpcionMenu.Enabled = true;
            HeaderMenu.Caption = this.lHeader_ant;
        }

        private void setea_checks()
        {
            if (sucursales.Count <= 0)
            {
                checkEdit_todossucursal.CheckState = CheckState.Checked;
                checkEdit_todossucursal.Enabled = false;
            }
            else
            {
                checkEdit_todossucursal.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 1);
            }
            if (bancos.Count <= 0)
            { 
                checkEdit_todosbanco.CheckState = CheckState.Checked;
                checkEdit_todosbanco.Enabled = false;
                checkEdit_todoscuenta.CheckState = CheckState.Checked;
                checkEdit_todoscuenta.Enabled = false;
            }
            else
            { 
                checkEdit_todosbanco.Enabled = true;
                checkEdit_todoscuenta.Enabled = true;
            }
            //
            if (cuentas.Count <= 0)
            {
                checkEdit_todoscuenta.CheckState = CheckState.Checked;
                checkEdit_todoscuenta.Enabled = false;
            }
            else
            {
                checkEdit_todoscuenta.Enabled = true;
            }
            //
            if (responsables.Count <= 0)
            {
                checkEdit_todosresponsable.CheckState = CheckState.Checked;
                checkEdit_todosresponsable.Enabled = false;
            }
            else
            {
                checkEdit_todosresponsable.Enabled = true;
            }
            //
            if (elaborados.Count <= 0)
            {
                checkEdit_todoselaborado.CheckState = CheckState.Checked;
                checkEdit_todoselaborado.Enabled = false;
            }
            else
            { checkEdit_todoselaborado.Enabled = true; }
            //
            if (revisados.Count <= 0)
            {
                checkEdit_todosrevisado.CheckState = CheckState.Checked;
                checkEdit_todosrevisado.Enabled = false;
            }
            else
            { checkEdit_todosrevisado.Enabled = true; }

            // Setea nivel de seguridad x tipo de usuario 
            //if (Fundraising_PT.Properties.Settings.Default.U_tipo == 3)
            //{
            //    checkEdit_todos_recaudador.CheckState = CheckState.Unchecked;
            //    checkEdit_todos_recaudador.Enabled = false;
            //    lookUpEdit_recaudador.EditValue = Fundraising_PT.Properties.Settings.Default.U_oid;
            //    lookUpEdit_recaudador.Enabled = false;
            //}
            //if (Fundraising_PT.Properties.Settings.Default.U_tipo == 2)
            //{
            //    checkEdit_todossupervisor.CheckState = CheckState.Unchecked;
            //    checkEdit_todossupervisor.Enabled = false;
            //    lookUpEdit_supervisor.EditValue = Fundraising_PT.Properties.Settings.Default.U_oid;
            //    lookUpEdit_supervisor.Enabled = false;
            //}
            if (Fundraising_PT.Properties.Settings.Default.U_tipo != 1)
            {
                checkEdit_todossucursal.CheckState = CheckState.Unchecked;
                checkEdit_todossucursal.Enabled = false;
                lookUpEdit_sucursal.EditValue = Fundraising_PT.Properties.Settings.Default.sucursal;
                lookUpEdit_sucursal.Enabled = false;
            }

        }

    }
}