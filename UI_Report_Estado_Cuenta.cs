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
    public partial class UI_Report_Estado_Cuenta : DevExpress.XtraEditors.XtraForm
    {
        // declaracion de objetos publicos //
        public DevExpress.XtraBars.BarButtonItem OpcionMenu;
        public DevExpress.XtraBars.BarHeaderItem HeaderMenu;
        public object ObjetoExtra;
        public string lHeader_ant = "";
        public string lAccion = "Reporte del Estado de Cuenta (Recaudado-Dépositado)";
       
        // variables para el filtro de los datos del reporte //
        public string lfiltro_report = "";
        public string lfiltro_fp = "";
        public string lfecha_desde = "";
        public string lfecha_hasta = "";
        public string lrecaudador = "";
        public string lsucursal = "";
        public string ltipo_fp = "";
        public string ldescr_fp = "";
        public string lstatus = "";
        public int ltipo_forma_pago = 0;
        public Guid loid_recaudador = Guid.Empty;
        public int lnstatus = 0;
        public int lnsucursal = 0;
        public Guid loid_forma_pago = Guid.Empty;
        //
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Saldos_Recauda_dep> saldos_recauda_dep;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos> formas_pagos;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> usuarios;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales> sucursales;

        public UI_Report_Estado_Cuenta(DevExpress.XtraBars.BarButtonItem opcionMenu, DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Reporte del Estado de Cuenta (Recaudado-Dépositado)...");
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
            CriteriaOperator filtro_saldos_recauda_dep = CriteriaOperator.Parse("1 = 2");
            DevExpress.Xpo.SortingCollection orden_formas_pagos = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_formas_pagos.Add(new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            DevExpress.Xpo.SortingCollection orden_saldos_recauda_dep = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_saldos_recauda_dep.Add(new DevExpress.Xpo.SortProperty("recaudador.usuario", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_saldos_recauda_dep.Add(new DevExpress.Xpo.SortProperty("forma_pago.codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_saldos_recauda_dep.Add(new DevExpress.Xpo.SortProperty("Substring(fecha_string,6,4)+Substring(fecha_string,3,2)+Substring(fecha_string,0,2)", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //orden_saldos_recauda_dep.Add(new DevExpress.Xpo.SortProperty("fecha_string", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            sucursales = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            saldos_recauda_dep = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Saldos_Recauda_dep>(DevExpress.Xpo.XpoDefault.Session, filtro_saldos_recauda_dep, new DevExpress.Xpo.SortProperty("forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            saldos_recauda_dep.Sorting = orden_saldos_recauda_dep; 
            formas_pagos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            formas_pagos.Sorting = orden_formas_pagos;
            usuarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("usuario", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            bindingSource_sucursales.DataSource = sucursales;
            bindingSource_recaudadores.DataSource = usuarios;
            bindingSource_formas_pagos.DataSource = formas_pagos;
        }

        private void UI_Report_Deposito_Load(object sender, EventArgs e)
        {
            //
            dateTime_fecha_desde.EditValue = DateTime.Now;
            dateTime_fecha_hasta.EditValue = DateTime.Now;
            //
            lookUpEdit_status.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue("EStatus");
            lookUpEdit_status.Properties.DisplayMember = "Descripcion";
            lookUpEdit_status.Properties.ValueMember = "Valor";
            //
            lookUpEdit_tipoformapago.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue("EPago");
            lookUpEdit_tipoformapago.Properties.DisplayMember = "Descripcion";
            lookUpEdit_tipoformapago.Properties.ValueMember = "Valor";
            //
            checkEdit_todossucursal.CheckedChanged += new EventHandler(checkEdit_todossucursal_CheckedChanged);
            checkEdit_todosfecha.CheckedChanged += new EventHandler(checkEdit_todosfecha_CheckedChanged);
            checkEdit_todosrecaudador.CheckedChanged += new EventHandler(checkEdit_todosrecaudador_CheckedChanged);
            checkEdit_todostiposfp.CheckedChanged += new EventHandler(checkEdit_todostiposfp_CheckedChanged);
            checkEdit_todosdescrpf.CheckedChanged += new EventHandler(checkEdit_todosdescrpf_CheckedChanged);
            checkEdit_todosstatus.CheckedChanged += new EventHandler(checkEdit_todosstatus_CheckedChanged);
            lookUpEdit_tipoformapago.EditValueChanged += new EventHandler(lookUpEdit_tipoformapago_EditValueChanged);
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

        void checkEdit_todosdescrpf_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todosdescrpf.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_formapago.EditValue = null;
                this.lookUpEdit_formapago.Enabled = false;
            }
            else
            {
                if (bindingSource_formas_pagos.Count > 0)
                {
                    this.lookUpEdit_formapago.Enabled = true;
                    this.lookUpEdit_formapago.Focus();
                }
                else
                { this.lookUpEdit_formapago.Enabled = false; }
            }
        }

        void checkEdit_todostiposfp_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todostiposfp.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_tipoformapago.EditValue = null;
                this.lookUpEdit_tipoformapago.Enabled = false;
            }
            else
            {
                this.lookUpEdit_tipoformapago.Enabled = true;
                this.lookUpEdit_tipoformapago.Focus();
            }
        }

        void checkEdit_todosrecaudador_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todosrecaudador.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_recaudador.EditValue = null;
                this.lookUpEdit_recaudador.Enabled = false;
            }
            else
            {
                if (bindingSource_recaudadores.Count > 0)
                {
                    this.lookUpEdit_recaudador.Enabled = true;
                    this.lookUpEdit_recaudador.Focus();
                }
                else
                { this.lookUpEdit_recaudador.Enabled = false; }
            }
        }

        void lookUpEdit_tipoformapago_EditValueChanged(object sender, EventArgs e)
        {
            lfiltro_fp = "status = 1";
            if (lookUpEdit_tipoformapago.EditValue != null)
            {
                ltipo_forma_pago = (int)lookUpEdit_tipoformapago.EditValue;
                lfiltro_fp = lfiltro_fp + " and " + string.Format("tpago = '{0}'", ltipo_forma_pago);
            }
            formas_pagos.Criteria = CriteriaOperator.Parse(lfiltro_fp);
            this.checkEdit_todosdescrpf.CheckState = CheckState.Checked;
            this.lookUpEdit_formapago.EditValue = null;
            this.lookUpEdit_formapago.Enabled = false;
            setea_checks();
        }

        private void simpleButton_imprimir_Click(object sender, EventArgs e)
        {
            //
            if (this.checkEdit_todosfecha.CheckState == CheckState.Unchecked)
            {
                lfecha_desde = ((DateTime)dateTime_fecha_desde.EditValue).Date.ToShortDateString();
                lfecha_hasta = ((DateTime)dateTime_fecha_hasta.EditValue).Date.ToShortDateString();
                string lfecha_desde1 = lfecha_desde.Substring(6, 4) + lfecha_desde.Substring(3, 2) + lfecha_desde.Substring(0, 2);
                string lfecha_hasta1 = lfecha_hasta.Substring(6, 4) + lfecha_hasta.Substring(3, 2) + lfecha_hasta.Substring(0, 2);
                //
                lfiltro_report = string.Format("Substring(fecha_string,6,4)+Substring(fecha_string,3,2)+Substring(fecha_string,0,2) >= '{0}' and Substring(fecha_string,6,4)+Substring(fecha_string,3,2)+Substring(fecha_string,0,2) <= '{1}'", lfecha_desde1, lfecha_hasta1);
                //lfiltro_report = string.Format("fecha_string >= '{0}' and fecha_string <= '{1}'", lfecha_desde, lfecha_hasta);
                //
            }
            else
            {
                lfecha_desde = "Todas";
                lfecha_hasta = "Todas";
                lfiltro_report = "1 = 1";
            }
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
            if (this.checkEdit_todosrecaudador.CheckState == CheckState.Unchecked)
            {
                lrecaudador = this.lookUpEdit_recaudador.Text;
                loid_recaudador = (Guid)this.lookUpEdit_recaudador.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("recaudador.oid = '{0}'", loid_recaudador);
            }
            else
            { lrecaudador = "Todos"; }
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


            if (this.checkEdit_todostiposfp.CheckState == CheckState.Unchecked)
            {
                ltipo_fp = this.lookUpEdit_tipoformapago.Text;
                ltipo_forma_pago = (int)this.lookUpEdit_tipoformapago.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("forma_pago.tpago = '{0}'", ltipo_forma_pago);
            }
            else
            {
                ltipo_fp = "Todos";
                ltipo_forma_pago = 0;
            }
            //
            if (this.checkEdit_todosdescrpf.CheckState == CheckState.Unchecked)
            {
                ldescr_fp = this.lookUpEdit_formapago.Text;
                loid_forma_pago = (Guid)this.lookUpEdit_formapago.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("forma_pago.oid = '{0}'", loid_forma_pago);
            }
            else
            {
                ldescr_fp = "Todos";
                loid_forma_pago = Guid.Empty;
            }
            //            
            saldos_recauda_dep.Criteria = CriteriaOperator.Parse(lfiltro_report);
            //
            XtraReport_Estado_Cuenta1 report_estado_cuenta1 = new XtraReport_Estado_Cuenta1(lfecha_desde, lfecha_hasta, lrecaudador, ltipo_fp, ldescr_fp, lstatus, lsucursal);
            report_estado_cuenta1.DataSource = saldos_recauda_dep;
            report_estado_cuenta1.ShowRibbonPreviewDialog();
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
            if (usuarios.Count <= 0)
            {
                checkEdit_todosrecaudador.CheckState = CheckState.Checked;
                checkEdit_todosrecaudador.Enabled = false;
            }
            else
            {
                checkEdit_todosrecaudador.Enabled = true;
            }
            //
            if (formas_pagos.Count <= 0)
            {
                checkEdit_todosdescrpf.CheckState = CheckState.Checked;
                checkEdit_todosdescrpf.Enabled = false;
            }
            else
            { checkEdit_todosdescrpf.Enabled = true; }

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