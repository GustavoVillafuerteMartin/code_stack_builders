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
    public partial class UI_Report_Recauda : DevExpress.XtraEditors.XtraForm
    {
        // declaracion de objetos publicos //
        public DevExpress.XtraBars.BarButtonItem OpcionMenu;
        public DevExpress.XtraBars.BarHeaderItem HeaderMenu;
        public object ObjetoExtra;
        public string lHeader_ant = "";
        public string lAccion = "Reporte de Recaudaciones";
       
        // variables para el filtro de los datos del reporte //
        public string lfiltro_report = "";
        public string lfiltro_fp = "";
        public string lfecha_desde = "";
        public string lfecha_hasta = "";
        public string lcodigointegrado = "";
        public string lserialfiscal = "";
        public string lnroz = "";
        public string lsucursal = "";
        public string lrecaudador = "";
        public string lsupervisor = "";
        public string lcaja = "";
        public string lcajero = "";
        public string ltipo_fp = "";
        public string ldescr_fp = "";
        public string lstatus_sesion = "";
        public string lstatus_recaudacion = "";
        public int ltipo_forma_pago = 0;
        public int lntipo_reporte = 0;
        public Guid loid_recaudador = Guid.Empty;
        public Guid loid_supervisor = Guid.Empty;
        public Guid loid_caja = Guid.Empty;
        public Guid loid_cajero = Guid.Empty;
        public int lnstatus_sesion = 0;
        public int lnstatus_recaudacion = 0;
        public int lnsucursal = 0;
        public Guid loid_forma_pago = Guid.Empty;
        public bool llview_tot_fec = true;
        public bool llview_gran_tot = true;
        public int ln_ordenfecha = 1;
        public int ln_groupby = 0;
        //
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones> recaudaciones;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos> formas_pagos;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> usuarios;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajas> cajas;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajeros> cajeros;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales> sucursales;
        //
        private DevExpress.Xpo.SortingCollection orden_fecha_ascendente_recaudaciones = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Ascending));
        private DevExpress.Xpo.SortingCollection orden_fecha_descendente_recaudaciones = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Descending));

        public UI_Report_Recauda(DevExpress.XtraBars.BarButtonItem opcionMenu, DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);

            switch ((int)objetoExtra)
            {
                case 1:
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Reporte de Recaudaciones...");
                    break;
                case 2:
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Reporte de Totales de Ventas...");
                    break;
                case 3:
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Reporte de Diferencias (Ventas-Recaudaciones)...");
                    break;
                default:
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Reporte de Recaudaciones...");
                    break;
            }
            //
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //
            OpcionMenu = opcionMenu;
            HeaderMenu = headerMenu;
            ObjetoExtra = objetoExtra;
            lHeader_ant = HeaderMenu.Caption;
            if (objetoExtra != null)
            {
                lntipo_reporte = (int)objetoExtra;
            }
            //
            orden_fecha_ascendente_recaudaciones.Add(new DevExpress.Xpo.SortProperty("sucursal", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_fecha_descendente_recaudaciones.Add(new DevExpress.Xpo.SortProperty("sucursal", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            CriteriaOperator filtro_status = (new OperandProperty("status") == new OperandValue(1));
            CriteriaOperator filtro_recaudaciones = CriteriaOperator.Parse("1 = 2");
            DevExpress.Xpo.SortingCollection orden_formas_pagos = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_formas_pagos.Add(new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            DevExpress.Xpo.SortProperty orden_recaudaciones = (new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            sucursales = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            recaudaciones = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones>(DevExpress.Xpo.XpoDefault.Session, filtro_recaudaciones, orden_recaudaciones);
            formas_pagos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            formas_pagos.Sorting = orden_formas_pagos;
            usuarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("usuario", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cajas = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajas>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cajeros = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajeros>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("cajero", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            bindingSource_sucursales.DataSource = sucursales;
            bindingSource_recaudadores.DataSource = usuarios;
            bindingSource_supersisores.DataSource = usuarios;
            bindingSource_cajas.DataSource = cajas;
            bindingSource_cajeros.DataSource = cajeros;
            bindingSource_formas_pagos.DataSource = formas_pagos;
        }

        private void UI_Report_Recauda_Load(object sender, EventArgs e)
        {
            switch (lntipo_reporte)
            {
                case 1:
                    this.Text = "Reporte de Recaudaciones";
                    this.labelControl_titulo.Text = "Reporte de Recaudaciones";
                    this.label_status_recaudacion.lText = "Status Recaudación : ";
                    this.lookUpEdit_status_recaudacion.ToolTip = "Selecciona el status de la recaudación.";
                    this.lookUpEdit_status_recaudacion.ToolTipTitle = "Status Recaudación";
                    this.checkEdit_todosstatus_recaudacion.ToolTip = "Selecciona todos los status de recaudación.";
                    this.checkEdit_todosstatus_recaudacion.ToolTipTitle = "Todos los status de recaudación";
                    break;
                case 2:
                    this.Text = "Reporte de Totales de Ventas";
                    this.labelControl_titulo.Text = "Reporte de Totales de Ventas";
                    this.label_status_recaudacion.lText = "Status T. Ventas : ";
                    this.lookUpEdit_status_recaudacion.ToolTip = "Selecciona el status de los totales de ventas.";
                    this.lookUpEdit_status_recaudacion.ToolTipTitle = "Status T. Ventas";
                    this.checkEdit_todosstatus_recaudacion.ToolTip = "Selecciona todos los status de los totales de ventas.";
                    this.checkEdit_todosstatus_recaudacion.ToolTipTitle = "Todos los status de los totales ventas";
                    break;
                case 3:
                    this.Text = "Reporte de Diferencias (Ventas-Recaudaciones)";
                    this.labelControl_titulo.Text = "Reporte de Diferencias (Ventas-Recaudaciones)";
                    this.label_status_recaudacion.lText = "Status Recaudación : ";
                    this.lookUpEdit_status_recaudacion.ToolTip = "Selecciona el status de la recaudación.";
                    this.lookUpEdit_status_recaudacion.ToolTipTitle = "Status Recaudación";
                    this.checkEdit_todosstatus_recaudacion.ToolTip = "Selecciona todos los status de recaudación.";
                    this.checkEdit_todosstatus_recaudacion.ToolTipTitle = "Todos los status de recaudación";
                    break;
                default:
                    this.Text = "Reporte de Recaudaciones";
                    this.labelControl_titulo.Text = "Reporte de Recaudaciones";
                    this.label_status_recaudacion.lText = "Status Recaudación : ";
                    this.lookUpEdit_status_recaudacion.ToolTip = "Selecciona el status de la recaudación.";
                    this.lookUpEdit_status_recaudacion.ToolTipTitle = "Status Recaudación";
                    this.checkEdit_todosstatus_recaudacion.ToolTip = "Selecciona todos los status de recaudación.";
                    this.checkEdit_todosstatus_recaudacion.ToolTipTitle = "Todos los status de recaudación";
                    break;
            }
            //
            dateTime_fecha_desde.EditValue = DateTime.Now;
            dateTime_fecha_hasta.EditValue = DateTime.Now;
            //
            lookUpEdit_modeloreport.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue("ETipo_Report");
            lookUpEdit_modeloreport.Properties.DisplayMember = "Descripcion";
            lookUpEdit_modeloreport.Properties.ValueMember = "Valor";
            lookUpEdit_modeloreport.EditValue = 0;
            //
            lookUpEdit_status_sesion.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue("EStatus_sesion");
            lookUpEdit_status_sesion.Properties.DisplayMember = "Descripcion";
            lookUpEdit_status_sesion.Properties.ValueMember = "Valor";
            //
            lookUpEdit_status_recaudacion.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue("EStatus_recaudacion");
            lookUpEdit_status_recaudacion.Properties.DisplayMember = "Descripcion";
            lookUpEdit_status_recaudacion.Properties.ValueMember = "Valor";
            //
            lookUpEdit_tipoformapago.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue("EPago");
            lookUpEdit_tipoformapago.Properties.DisplayMember = "Descripcion";
            lookUpEdit_tipoformapago.Properties.ValueMember = "Valor";
            //
            checkEdit_todosfecha.CheckedChanged += new EventHandler(checkEdit_todosfecha_CheckedChanged);
            checkEdit_todossucursal.CheckedChanged += new EventHandler(checkEdit_todossucursal_CheckedChanged);
            checkEdit_todos_recaudador.CheckedChanged += new EventHandler(checkEdit_todos_recaudador_CheckedChanged);
            checkEdit_todossupervisor.CheckedChanged += new EventHandler(checkEdit_todossupervisor_CheckedChanged);
            checkEdit_todoscajas.CheckedChanged += new EventHandler(checkEdit_todoscajas_CheckedChanged);
            checkEdit_todoscajeros.CheckedChanged += new EventHandler(checkEdit_todoscajeros_CheckedChanged);
            checkEdit_todostiposfp.CheckedChanged += new EventHandler(checkEdit_todostiposfp_CheckedChanged);
            checkEdit_todosdescrpf.CheckedChanged += new EventHandler(checkEdit_todosdescrpf_CheckedChanged);
            checkEdit_todosstatus_sesion.CheckedChanged += new EventHandler(checkEdit_todosstatus_sesion_CheckedChanged);
            checkEdit_todosstatus_recaudacion.CheckedChanged += new EventHandler(checkEdit_todosstatus_recaudacion_CheckedChanged);
            lookUpEdit_tipoformapago.EditValueChanged += new EventHandler(lookUpEdit_tipoformapago_EditValueChanged);
            checkButton_order_fecha_ascendente.CheckedChanged += new EventHandler(checkButton_order_fecha_ascendente_CheckedChanged);
            checkButton_order_fecha_descendente.CheckedChanged += new EventHandler(checkButton_order_fecha_descendente_CheckedChanged);
            checkButton_aserialfiscal.CheckedChanged += checkButton_aserialfiscal_CheckedChanged;
            //checkButton_anroz.CheckedChanged += checkButton_anroz_CheckedChanged;
            //
            simpleButton_imprimir.Click += new EventHandler(simpleButton_imprimir_Click);
            simpleButton_salir.Click += new EventHandler(simpleButton_salir_Click);
            //
            setea_checks();
            //
            textEdit_codigointegrado.Focus();
            //
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        //void checkButton_anroz_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkButton_anroz.Checked == true)
        //    {
        //        checkButton_aserialfiscal.Checked = false;
        //    }
        //}

        void checkButton_aserialfiscal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkButton_aserialfiscal.Checked == true)
            {
                checkButton_anroz.Checked = true;
                checkButton_anroz.Enabled = true;
            }
            else 
            {
                checkButton_anroz.Checked = false;
                checkButton_anroz.Enabled = false;
            }
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

        void checkEdit_todosstatus_recaudacion_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todosstatus_recaudacion.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_status_recaudacion.EditValue = null;
                this.lookUpEdit_status_recaudacion.Enabled = false;
            }
            else
            {
                this.lookUpEdit_status_recaudacion.Enabled = true;
                this.lookUpEdit_status_recaudacion.Focus();
            }
        }

        void checkEdit_todosstatus_sesion_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todosstatus_sesion.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_status_sesion.EditValue = null;
                this.lookUpEdit_status_sesion.Enabled = false;
            }
            else
            {
                this.lookUpEdit_status_sesion.Enabled = true;
                this.lookUpEdit_status_sesion.Focus();
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

        void checkEdit_todoscajeros_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todoscajeros.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_cajero.EditValue = null;
                this.lookUpEdit_cajero.Enabled = false;
            }
            else
            {
                if (bindingSource_cajeros.Count > 0)
                {
                    this.lookUpEdit_cajero.Enabled = true;
                    this.lookUpEdit_cajero.Focus();
                }
                else
                { this.lookUpEdit_cajero.Enabled = false; }
            }
        }

        void checkEdit_todoscajas_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todoscajas.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_caja.EditValue = null;
                this.lookUpEdit_caja.Enabled = false;
            }
            else
            {
                if (bindingSource_cajas.Count > 0)
                {
                    this.lookUpEdit_caja.Enabled = true;
                    this.lookUpEdit_caja.Focus();
                }
                else
                { this.lookUpEdit_caja.Enabled = false; }
            }
        }

        void checkEdit_todossupervisor_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit_todossupervisor.CheckState == CheckState.Checked)
            {
                this.lookUpEdit_supervisor.EditValue = null;
                this.lookUpEdit_supervisor.Enabled = false;
            }
            else
            {
                if (bindingSource_supersisores.Count > 0)
                {
                    this.lookUpEdit_supervisor.Enabled = true;
                    this.lookUpEdit_supervisor.Focus();
                }
                else
                { this.lookUpEdit_supervisor.Enabled = false; }
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
            if (checkButton_aserialfiscal.Checked)
            {
                orden_fecha_ascendente_recaudaciones.Add(new DevExpress.Xpo.SortProperty("sesion.s_fiscal", DevExpress.Xpo.DB.SortingDirection.Ascending));
                orden_fecha_descendente_recaudaciones.Add(new DevExpress.Xpo.SortProperty("sesion.s_fiscal", DevExpress.Xpo.DB.SortingDirection.Ascending));
                //
                orden_fecha_ascendente_recaudaciones.Add(new DevExpress.Xpo.SortProperty("sesion.z_fiscal", DevExpress.Xpo.DB.SortingDirection.Ascending));
                orden_fecha_descendente_recaudaciones.Add(new DevExpress.Xpo.SortProperty("sesion.z_fiscal", DevExpress.Xpo.DB.SortingDirection.Ascending));
            }
            else 
            {
                orden_fecha_ascendente_recaudaciones.Add(new DevExpress.Xpo.SortProperty("sesion.id_sesion", DevExpress.Xpo.DB.SortingDirection.Ascending));
                orden_fecha_descendente_recaudaciones.Add(new DevExpress.Xpo.SortProperty("sesion.id_sesion", DevExpress.Xpo.DB.SortingDirection.Ascending));
            }
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
                //lfiltro_report = string.Format("GetDate(fecha_hora) >= '{0}' and GetDate(fecha_hora) <= '{1}'", ((DateTime)dateTime_fecha_desde.EditValue).Date, ((DateTime)dateTime_fecha_hasta.EditValue).Date);
                //lfiltro_report = string.Format("ToStr(GetDate(fecha_hora)) >= '{0}' and GetDate(fecha_hora) <= '{1}'", ((DateTime)dateTime_fecha_desde.EditValue).Date, ((DateTime)dateTime_fecha_hasta.EditValue).Date);
                //
                //XPView aaa = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones));
                //aaa.AddProperty("dia", "PadLeft(ToStr(GetDay(fecha_hora)),2,'0')", true, true, DevExpress.Xpo.SortDirection.None);
                //aaa.AddProperty("mes", "PadLeft(ToStr(GetMonth(fecha_hora)),2,'0')", true, true, DevExpress.Xpo.SortDirection.None);
                //aaa.AddProperty("anio", "ToStr(GetYear(fecha_hora))", true, true, DevExpress.Xpo.SortDirection.None);
                //
            }
            else
            {
                lfecha_desde   = "Todas";
                lfecha_hasta   = "Todas";
                lfiltro_report = "1 = 1";
            }
            //
            if (this.textEdit_codigointegrado.Text.Trim() != string.Empty)
            {
                lcodigointegrado = this.textEdit_codigointegrado.Text.Trim();
                lfiltro_report = lfiltro_report + " and " + string.Format("trim(tostr(sucursal))+trim(sesion.caja.codigo)+trim(sesion.cajero.codigo)+trim(tostr(sesion.id_sesion)) = '{0}'", lcodigointegrado);
            }
            else
            { lcodigointegrado = "Todos"; }
            //
            if (this.textEdit_serialfiscal.Text.Trim() != string.Empty)
            {
                lserialfiscal = this.textEdit_serialfiscal.Text.Trim();
                lfiltro_report = lfiltro_report + " and " + string.Format("trim(sesion.s_fiscal) = '{0}'", lserialfiscal);
            }
            else
            { lserialfiscal = "Todos"; }
            //
            if (this.textEdit_nroz.Text.Trim() != string.Empty)
            {
                lnroz = this.textEdit_nroz.Text.Trim();
                lfiltro_report = lfiltro_report + " and " + string.Format("trim(sesion.z_fiscal) = '{0}'", lnroz);
            }
            else
            { lnroz = "Todos"; }
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
            if (this.checkEdit_todos_recaudador.CheckState == CheckState.Unchecked)
            {
                lrecaudador = this.lookUpEdit_recaudador.Text;
                loid_recaudador = (Guid)this.lookUpEdit_recaudador.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("usuario.oid = '{0}'", loid_recaudador);
            }
            else
            { lrecaudador = "Todos"; }
            //
            if (this.checkEdit_todossupervisor.CheckState == CheckState.Unchecked)
            {
                lsupervisor = this.lookUpEdit_supervisor.Text;
                loid_supervisor = (Guid)this.lookUpEdit_supervisor.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("supervisor.oid = '{0}'", loid_supervisor);
            }
            else
            { lsupervisor = "Todos"; }
            //
            if (this.checkEdit_todoscajas.CheckState == CheckState.Unchecked)
            {
                lcaja = this.lookUpEdit_caja.Text;
                loid_caja = (Guid)this.lookUpEdit_caja.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("sesion.caja.oid = '{0}'", loid_caja);
            }
            else
            { lcaja = "Todos"; }
            //
            if (this.checkEdit_todoscajeros.CheckState == CheckState.Unchecked)
            {
                lcajero = this.lookUpEdit_cajero.Text;
                loid_cajero = (Guid)this.lookUpEdit_cajero.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("sesion.cajero.oid = '{0}'", loid_cajero);
            }
            else
            { lcajero = "Todos"; }
            //
            if (this.checkEdit_todosstatus_sesion.CheckState == CheckState.Unchecked)
            {
                lstatus_sesion = this.lookUpEdit_status_sesion.Text;
                lnstatus_sesion = (int)this.lookUpEdit_status_sesion.EditValue;
                lfiltro_report = lfiltro_report + " and " + string.Format("sesion.status = '{0}'", lnstatus_sesion);
            }
            else
            { lstatus_sesion = "Todos"; }
            //
            if (this.checkEdit_todosstatus_recaudacion.CheckState == CheckState.Unchecked)
            {
                lstatus_recaudacion = this.lookUpEdit_status_recaudacion.Text;
                lnstatus_recaudacion = (int)this.lookUpEdit_status_recaudacion.EditValue;
                switch (lntipo_reporte)
                {
                    case 1:
                        lfiltro_report = lfiltro_report + " and " + string.Format("status = '{0}'", lnstatus_recaudacion);
                        break;
                    case 2:
                        lfiltro_report = lfiltro_report + " and " + string.Format("status_tv = '{0}'", lnstatus_recaudacion);
                        break;
                    case 3:
                        lfiltro_report = lfiltro_report + " and " + string.Format("status = '{0}'", lnstatus_recaudacion);
                        break;
                    default:
                        lfiltro_report = lfiltro_report + " and " + string.Format("status = '{0}'", lnstatus_recaudacion);
                        break;
                }
            }
            else
            { lstatus_recaudacion = "Todos"; }
            //
            if (this.checkEdit_todostiposfp.CheckState == CheckState.Unchecked)
            {
                ltipo_fp = this.lookUpEdit_tipoformapago.Text;
                ltipo_forma_pago = (int)this.lookUpEdit_tipoformapago.EditValue;
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
            }
            else
            { 
                ldescr_fp = "Todos";
                loid_forma_pago = Guid.Empty;
            }
            //  
            if (checkButton_order_fecha_ascendente.Checked == true)
            {
                ln_ordenfecha = 1;
                recaudaciones.Sorting = orden_fecha_ascendente_recaudaciones;
            }
            else
            {
                ln_ordenfecha = 2;
                recaudaciones.Sorting = orden_fecha_descendente_recaudaciones;
            }
            //
            recaudaciones.Criteria = CriteriaOperator.Parse(lfiltro_report);
            //IEnumerable<IGrouping<string, string>> resulgroup = from recaudaciones_grouping in recaudaciones group recaudaciones_grouping.sesion.s_fiscal by recaudaciones_grouping.sesion.z_fiscal;
            //
            switch (lntipo_reporte)
            {
                case 1:
                    if (lookUpEdit_modeloreport.EditValue.ToString().Trim() == "0")
                    {
                        XtraReport_Recaudaciones1 report_recaudaciones = new XtraReport_Recaudaciones1(lfecha_desde, lfecha_hasta, lrecaudador, lsupervisor, lcaja, lcajero, ltipo_fp, ltipo_forma_pago, ldescr_fp, loid_forma_pago, lstatus_sesion, lstatus_recaudacion, llview_tot_fec, llview_gran_tot, ln_ordenfecha, lsucursal, checkButton_aserialfiscal.Checked, checkButton_anroz.Checked);
                        report_recaudaciones.Landscape = false;
                        report_recaudaciones.DataSource = recaudaciones;
                        report_recaudaciones.ShowRibbonPreviewDialog();
                    }
                    else
                    {
                        XtraReport_Recaudaciones report_recaudaciones = new XtraReport_Recaudaciones(lfecha_desde, lfecha_hasta, lrecaudador, lsupervisor, lcaja, lcajero, ltipo_fp, ltipo_forma_pago, ldescr_fp, loid_forma_pago, lstatus_sesion, lstatus_recaudacion, llview_tot_fec, llview_gran_tot, ln_ordenfecha, lsucursal, checkButton_aserialfiscal.Checked, checkButton_anroz.Checked);
                        report_recaudaciones.Landscape = true;
                        report_recaudaciones.DataSource = recaudaciones;
                        report_recaudaciones.ShowRibbonPreviewDialog();
                    }
                    break;
                case 2:
                    if (lookUpEdit_modeloreport.EditValue.ToString().Trim() == "0")
                    {
                        XtraReport_Totales_Ventas1 report_totales_ventas = new XtraReport_Totales_Ventas1(lfecha_desde, lfecha_hasta, lrecaudador, lsupervisor, lcaja, lcajero, ltipo_fp, ltipo_forma_pago, ldescr_fp, loid_forma_pago, lstatus_sesion, lstatus_recaudacion, llview_tot_fec, llview_gran_tot, ln_ordenfecha, lsucursal, checkButton_aserialfiscal.Checked, checkButton_anroz.Checked);
                        report_totales_ventas.Landscape = false;
                        report_totales_ventas.DataSource = recaudaciones;
                        report_totales_ventas.ShowRibbonPreviewDialog();
                    }
                    else
                    {
                        XtraReport_Totales_Ventas report_totales_ventas = new XtraReport_Totales_Ventas(lfecha_desde, lfecha_hasta, lrecaudador, lsupervisor, lcaja, lcajero, ltipo_fp, ltipo_forma_pago, ldescr_fp, loid_forma_pago, lstatus_sesion, lstatus_recaudacion, llview_tot_fec, llview_gran_tot, ln_ordenfecha, lsucursal, checkButton_aserialfiscal.Checked, checkButton_anroz.Checked);
                        report_totales_ventas.Landscape = true;
                        report_totales_ventas.DataSource = recaudaciones;
                        report_totales_ventas.ShowRibbonPreviewDialog();
                    }
                    break;
                case 3:
                    if (lookUpEdit_modeloreport.EditValue.ToString().Trim() == "0")
                    {
                        XtraReport_Diferencias_VR2 report_diferencias = new XtraReport_Diferencias_VR2(recaudaciones, lfecha_desde, lfecha_hasta, lrecaudador, lsupervisor, lcaja, lcajero, ltipo_fp, ltipo_forma_pago, ldescr_fp, loid_forma_pago, lstatus_sesion, lstatus_recaudacion, llview_tot_fec, llview_gran_tot, ln_ordenfecha, lsucursal);
                        report_diferencias.Landscape = false;
                        report_diferencias.ShowRibbonPreviewDialog();
                    }
                    else
                    {
                        XtraReport_Diferencias_VR report_diferencias = new XtraReport_Diferencias_VR(recaudaciones, lfecha_desde, lfecha_hasta, lrecaudador, lsupervisor, lcaja, lcajero, ltipo_fp, ltipo_forma_pago, ldescr_fp, loid_forma_pago, lstatus_sesion, lstatus_recaudacion, llview_tot_fec, llview_gran_tot, ln_ordenfecha, lsucursal);
                        report_diferencias.Landscape = true;
                        report_diferencias.ShowRibbonPreviewDialog();
                    }
                    break;
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
            //if (ObjetoExtra != null)
            //{
            //    ((DevExpress.XtraBars.BarButtonItem)ObjetoExtra).Enabled = true;
            //}
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
                checkEdit_todos_recaudador.CheckState = CheckState.Checked;
                checkEdit_todos_recaudador.Enabled = false;
                checkEdit_todossupervisor.CheckState = CheckState.Checked;
                checkEdit_todossupervisor.Enabled = false;
            }
            else
            { 
                checkEdit_todos_recaudador.Enabled = true;
                checkEdit_todossupervisor.Enabled = true;
            }
            //
            if (cajas.Count <= 0)
            {
                checkEdit_todoscajas.CheckState = CheckState.Checked;
                checkEdit_todoscajas.Enabled = false;
            }
            else
            {
                checkEdit_todoscajas.Enabled = true;
            }
            //
            if (cajeros.Count <= 0)
            {
                checkEdit_todoscajeros.CheckState = CheckState.Checked;
                checkEdit_todoscajeros.Enabled = false;
            }
            else
            {
                checkEdit_todoscajeros.Enabled = true;
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
            if (Fundraising_PT.Properties.Settings.Default.U_tipo == 3)
            {
                checkEdit_todos_recaudador.CheckState = CheckState.Unchecked;
                checkEdit_todos_recaudador.Enabled = false;
                lookUpEdit_recaudador.EditValue = Fundraising_PT.Properties.Settings.Default.U_oid;
                lookUpEdit_recaudador.Enabled = false;
            }
            if (Fundraising_PT.Properties.Settings.Default.U_tipo == 2)
            {
                checkEdit_todossupervisor.CheckState = CheckState.Unchecked;
                checkEdit_todossupervisor.Enabled = false;
                lookUpEdit_supervisor.EditValue = Fundraising_PT.Properties.Settings.Default.U_oid;
                lookUpEdit_supervisor.Enabled = false;
            }
            if (Fundraising_PT.Properties.Settings.Default.U_tipo != 1)
            {
                checkEdit_todossucursal.CheckState = CheckState.Unchecked;
                checkEdit_todossucursal.Enabled = false;
                lookUpEdit_sucursal.EditValue = Fundraising_PT.Properties.Settings.Default.sucursal;
                lookUpEdit_sucursal.Enabled = false;
            }
            //
        }
    }
}