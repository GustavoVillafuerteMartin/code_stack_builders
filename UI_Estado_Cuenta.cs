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
    public partial class UI_Estado_Cuenta : DevExpress.XtraEditors.XtraForm
    {
        // declaracion de objetos publicos //
        public DevExpress.XtraBars.BarButtonItem OpcionMenu;
        public DevExpress.XtraBars.BarHeaderItem HeaderMenu;
        public object ObjetoExtra;
        public string lHeader_ant = "";
        public string lAccion = "Reporte del Estado de Cuenta (Recaudado-Depósitado";
       
        // variables para el filtro de los datos del reporte //
        public string lfiltro_report = "";
        public string lfiltro_fp = "";
        public string lfecha_desde = "";
        public string lfecha_hasta = "";
        public int ltipo_forma_pago = 0;
        public Guid loid_recaudador = Guid.Empty;
        public Guid loid_forma_pago = Guid.Empty;

        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Saldos_Recauda_dep> saldos_recauda_dep;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos> formas_pagos;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> usuarios;

        public UI_Estado_Cuenta(DevExpress.XtraBars.BarButtonItem opcionMenu, DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Estado de Cuentas...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
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
            DevExpress.Xpo.SortingCollection orden_formas_pagos = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_formas_pagos.Add(new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            DevExpress.Xpo.SortingCollection orden_saldos_recauda_dep = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("recaudador.usuario", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_saldos_recauda_dep.Add(new DevExpress.Xpo.SortProperty("fecha_string", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_saldos_recauda_dep.Add(new DevExpress.Xpo.SortProperty("forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_saldos_recauda_dep.Add(new DevExpress.Xpo.SortProperty("forma_pago.codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            saldos_recauda_dep = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Saldos_Recauda_dep>(DevExpress.Xpo.XpoDefault.Session);
            formas_pagos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            usuarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("usuario", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            bindingSource_formas_pagos.DataSource = formas_pagos;
            bindingSource_usuarios.DataSource = usuarios;
            formas_pagos.Sorting = orden_formas_pagos;
            saldos_recauda_dep.Sorting = orden_saldos_recauda_dep;
        }

        private void UI_Estado_Cuenta_Load(object sender, EventArgs e)
        {
            dateTime_fecha_desde.EditValue = DateTime.Now;
            dateTime_fecha_hasta.EditValue = DateTime.Now;
            //dateTime_fecha_desde.DateTime = DateTime.Now;
            //dateTime_fecha_hasta.DateTime = DateTime.Now;
            //
            lookUpEdit_tipoformapago.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue("EPago");
            lookUpEdit_tipoformapago.Properties.DisplayMember = "Descripcion";
            lookUpEdit_tipoformapago.Properties.ValueMember = "Valor";
            //
            checkEdit_todos_recaudador.CheckedChanged += new EventHandler(checkEdit_todos_recaudador_CheckedChanged);
            checkEdit_todos_tipo.CheckedChanged += new EventHandler(checkEdit_todos_tipo_CheckedChanged);
            checkEdit_todosdescripcion.CheckedChanged += new EventHandler(checkEdit_todosdescripcion_CheckedChanged);
            lookUpEdit_tipoformapago.EditValueChanged += new EventHandler(lookUpEdit_tipoformapago_EditValueChanged);
            //
            setea_checks();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
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
            this.checkEdit_todosdescripcion.CheckState = CheckState.Checked;
            this.lookUpEdit_formapago.EditValue = null;
            this.lookUpEdit_formapago.Enabled = false;
            setea_checks();
        }

        void checkEdit_todosdescripcion_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todosdescripcion.CheckState == CheckState.Checked)
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

        void checkEdit_todos_tipo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todos_tipo.CheckState == CheckState.Checked)
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

        void checkEdit_todos_recaudador_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todos_recaudador.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_recaudador.EditValue = null;
                this.lookUpEdit_recaudador.Enabled = false;
            }
            else
            {
                if (bindingSource_usuarios.Count > 0)
                {
                    this.lookUpEdit_recaudador.Enabled = true;
                    this.lookUpEdit_recaudador.Focus();
                }
                else
                { this.lookUpEdit_recaudador.Enabled = false; }
            }
        }

        private void simpleButton_imprimir_Click(object sender, EventArgs e)
        {
            XtraReport_Estado_Cuenta estado_cuenta = new XtraReport_Estado_Cuenta();
            //
            lfecha_desde = ((DateTime)dateTime_fecha_desde.EditValue).Date.ToShortDateString();
            lfecha_hasta = ((DateTime)dateTime_fecha_hasta.EditValue).Date.ToShortDateString();
            //
            lfiltro_report = string.Format("fecha_string >= '{0}' and fecha_string <= '{1}'", lfecha_desde, lfecha_hasta);
            //
            if (this.checkEdit_todos_recaudador.CheckState == CheckState.Unchecked)
            {
                loid_recaudador = (Guid)this.lookUpEdit_recaudador.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("recaudador.oid = '{0}'", loid_recaudador);
            }
            //
            if (this.checkEdit_todos_tipo.CheckState == CheckState.Unchecked)
            {
                ltipo_forma_pago = (int)this.lookUpEdit_tipoformapago.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("forma_pago.tpago = '{0}'", ltipo_forma_pago);
            }
            //
            if (this.checkEdit_todosdescripcion.CheckState == CheckState.Unchecked)
            {
                loid_forma_pago = (Guid)this.lookUpEdit_formapago.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("forma_pago.oid = '{0}'", loid_forma_pago);
            }
            //            
            saldos_recauda_dep.Criteria = CriteriaOperator.Parse(lfiltro_report);
            //
            estado_cuenta.DataSource = saldos_recauda_dep;
            estado_cuenta.ShowRibbonPreviewDialog();
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
            if (ObjetoExtra != null)
            {
                ((DevExpress.XtraBars.BarButtonItem)ObjetoExtra).Enabled = true;
            }
            HeaderMenu.Caption = this.lHeader_ant;
        }

        private void setea_checks()
        {
            if (usuarios.Count <= 0)
            { 
                checkEdit_todos_recaudador.CheckState = CheckState.Checked;
                checkEdit_todos_recaudador.Enabled = false;
            }
            else
            { checkEdit_todos_recaudador.Enabled = true; }
            //
            if (formas_pagos.Count <= 0)
            {
                checkEdit_todosdescripcion.CheckState = CheckState.Checked;
                checkEdit_todosdescripcion.Enabled = false;
            }
            else
            { checkEdit_todosdescripcion.Enabled = true; }
        }
    }
}