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
using DevExpress.Xpo.DB;
using DevExpress.XtraReports.UI;
using DevExpress.Xpo;
using DevExpress.XtraRichEdit.API.Word;
using DevExpress.Utils;
using System.IO;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Recaudacion_Det : DevExpress.XtraEditors.XtraForm
    {
        // declaracion de variables operativas //
        private object Form_padre;
        private static int Modo_recaudacion = 0;
        private string lAccion = "";
        private string lHeader_ant = "";
        private string ln_positab = "";
        private string lc_ref1 = "";
        private string lc_ref2 = "";
        private string ln_picture_activo = "";
        private string lc_fecha_recaudacion = "";
        private string lc_sucursal = "";
        private DateTime ld_fecha_recaudacion = DateTime.Now;
        private decimal ln_recaudacion_det_des_cantidad = 0;
        private decimal ln_monto_recaudado = 0;
        private int ln_tpago = 0;
        private int ln_ttarjeta = 1;
        private int ln_ttarjeta_ult = 1;
        private int ln_sucursal = 0;
        private int ln_status_recaudacion = 0;
        private int sesion_status_ant = 0;
        private decimal tot_efectivo_billetes = 0;
        private decimal tot_efectivo_monedas = 0;
        private decimal tot_tarjetas_debito = 0;
        private decimal tot_tarjetas_credito = 0;
        private decimal tot_tarjetas_alimentacion = 0;
        //
        // crea las variables para los GUID de las colleciones principales de datos //
        private Guid lg_forma_pago = new Guid();
        private Guid lg_forma_pago_efectivo_ult = new Guid();
        private Guid lg_forma_pago_tarjetadebito_ult = new Guid();
        private Guid lg_forma_pago_tarjetacredito_ult = new Guid();
        private Guid lg_forma_pago_tarjetaalimento_ult = new Guid();
        private Guid lg_forma_pago_cheque_ult = new Guid();
        private Guid lg_forma_pago_credito_ult = new Guid();
        private Guid lg_forma_pago_otrospago_ult = new Guid();
        private Guid lg_forma_pago_pagosinterno_ult = new Guid();
        private Guid lg_forma_pago_ticket_ult = new Guid();
        private Guid lg_forma_pago_consumointerno_ult = new Guid();
        private Guid lg_forma_pago_prepago_ult = new Guid();
        private Guid lg_forma_pago_deposito_ult = new Guid();
        private Guid lg_forma_pago_retenimp_ult = new Guid();
        private Guid lg_forma_pago_exoimp_ult = new Guid();
        private Guid lg_forma_pago_isrl_ult = new Guid();
        private Guid lg_forma_pago_saldofavor_ult = new Guid();
        private Guid lg_forma_pago_puntosleal_ult = new Guid();
        //
        private Guid lg_proveedor_ta = new Guid();
        private Guid lg_banco = new Guid();
        private Guid lg_banco_cuenta = new Guid();
        private Guid lg_sesion = new Guid();
        private Guid lg_recaudacion = new Guid();
        private Guid lg_recaudacion_det = new Guid();
        //
        // crea datatables temporales para guardar detalles y desgloces por tipos de pagos //
        private DataTable billetes_aux = new DataTable();
        private DataTable monedas_aux = new DataTable();
        private DataTable tarjeta_debito_aux = new DataTable();
        private DataTable tarjeta_credito_aux = new DataTable();
        private DataTable tarjeta_alimentacion_aux = new DataTable();
        private DataTable cheque_aux = new DataTable();
        private DataTable credito_aux = new DataTable();
        private DataTable ticket_aux = new DataTable();
        private DataTable deposito_aux = new DataTable();

        // otros datatables //
        private DataTable puntos_bancarios_aux = new DataTable();
        private DataTable bancos_aux = new DataTable();
        private DataTable bancos_cuentas_aux = new DataTable();
        private DataTable proveedores_ta_aux = new DataTable();
        private DataTable formas_pagos_aux = new DataTable();
        private DataTable distribucion_grafica = new DataTable();
        private DataTable distribucion_depositar = new DataTable();
        //
        // crea variables para guardar los totales operativos por tipos de pagos y totales generales //
        private static decimal ln_total_efectivo = 0, ln_total_tarjeta_debito = 0, ln_total_tarjeta_credito = 0, ln_total_tarjeta_alimentacion = 0;
        private static decimal ln_total_cheque = 0, ln_total_credito = 0, ln_total_otrospagos = 0, ln_total_pagosinternos = 0;
        private static decimal ln_total_ticketalimentacion = 0, ln_total_consumosinternos = 0, ln_total_prepago = 0, ln_total_deposito = 0;
        private static decimal ln_total_retencionimpuesto = 0, ln_total_exoneracionimpuesto = 0, ln_total_islr = 0, ln_total_saldofavor = 0;
        private static decimal ln_total_puntoslealtad = 0, ln_total_ninguno = 0;
        private static decimal ln_totalgeneralrecaudacion = 0, ln_subtotaltarjetas = 0;
        //
        // crea variables para guardar solo los totales iniciales por tipos de pagos y totales generales //
        private static decimal ln_total_efectivo_ini = 0, ln_total_tarjeta_debito_ini = 0, ln_total_tarjeta_credito_ini = 0, ln_total_tarjeta_alimentacion_ini = 0;
        private static decimal ln_total_cheque_ini = 0, ln_total_credito_ini = 0, ln_total_otrospagos_ini = 0, ln_total_pagosinternos_ini = 0;
        private static decimal ln_total_ticketalimentacion_ini = 0, ln_total_consumosinternos_ini = 0, ln_total_prepago_ini = 0, ln_total_deposito_ini = 0;
        private static decimal ln_total_retencionimpuesto_ini = 0, ln_total_exoneracionimpuesto_ini = 0, ln_total_islr_ini = 0, ln_total_saldofavor_ini = 0;
        private static decimal ln_total_puntoslealtad_ini = 0, ln_total_ninguno_ini = 0;
        private static decimal ln_totalgeneralrecaudacion_ini = 0, ln_subtotaltarjetas_ini = 0;
        //
        //// crea el formulario tipo report designer para utilizar el diseñador de reportes
        //private Clases.UI_Report_Designer form_designer_report_det = new Clases.UI_Report_Designer();
        //
        // crea e instancia tables para las formas de pago creadas por tipos de pagos y rows activos por forma y tipo de pagos //
        private DataRow forma_pago_aux_current_row;
        private DataRowView forma_pago_current_row;
        //
        private static DataTable fp_efectivo = new DataTable();
        private DataRow fp_efectivo_current_row;
        private static DataTable fp_tarjeta_debito = new DataTable();
        private DataRow fp_tarjeta_debito_current_row;
        private static DataTable fp_tarjeta_credito = new DataTable();
        private DataRow fp_tarjeta_credito_current_row;
        private static DataTable fp_tarjeta_alimentacion = new DataTable();
        private DataRow fp_tarjeta_alimentacion_current_row;
        private static DataTable fp_cheque = new DataTable();
        private DataRow fp_cheque_current_row;
        private static DataTable fp_credito = new DataTable();
        private DataRow fp_credito_current_row;
        private static DataTable fp_otrospagos = new DataTable();
        private DataRow fp_otrospagos_current_row;
        private static DataTable fp_pagosinternos = new DataTable();
        private DataRow fp_pagosinternos_current_row;
        private static DataTable fp_ticketalimentacion = new DataTable();
        private DataRow fp_ticketalimentacion_current_row;
        private static DataTable fp_consumosinternos = new DataTable();
        private DataRow fp_consumosinternos_current_row;
        private static DataTable fp_prepago = new DataTable();
        private DataRow fp_prepago_current_row;
        private static DataTable fp_deposito = new DataTable();
        private DataRow fp_deposito_current_row;
        private static DataTable fp_retencionimpuesto = new DataTable();
        private DataRow fp_retencionimpuesto_current_row;
        private static DataTable fp_exoneracionimpuesto = new DataTable();
        private DataRow fp_exoneracionimpuesto_current_row;
        private static DataTable fp_islr = new DataTable();
        private DataRow fp_islr_current_row;
        private static DataTable fp_saldofavor = new DataTable();
        private DataRow fp_saldofavor_current_row;
        private static DataTable fp_puntoslealtad = new DataTable();
        private DataRow fp_puntoslealtad_current_row;
        //
        // declaracion de repositorios de items para combos de los grids //
        DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit items_puntos_bancarios = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
        DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit items_bancos = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
        DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit items_bancos_cuentas = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
        //
        // declaracion de colecciones de las denominaciones de monedas, puntos bancarios, proveedores tarjeta alimentacion, formas de pago y saldos creadas en el sistema //
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas> denominacion_monedas_billetes;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas> denominacion_monedas_monedas;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios> puntos_bancarios;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos> bancos;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas> bancos_cuentas;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA> proveedores_ta;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos> formas_pagos;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas> totales_ventas;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas_Des> totales_ventas_des;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Saldos_Recauda_dep> saldos_recauda_dep;
        //
        // declaracion de colecciones por cada tipo de pagos y sus auxiliares filtrada por forma de pago //
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_efectivo;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_efectivo_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_tarjeta_debito;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_tarjeta_debito_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_tarjeta_credito;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_tarjeta_credito_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_tarjeta_alimentacion;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_tarjeta_alimentacion_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_cheque;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_cheque_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_credito;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_credito_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_otrospagos;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_otrospagos_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_pagosinternos;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_pagosinternos_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_ticketalimentacion;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_ticketalimentacion_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_consumosinternos;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_consumosinternos_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_prepago;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_prepago_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_deposito;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_deposito_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_retencionimpuesto;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_retencionimpuesto_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_exoneracionimpuesto;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_exoneracionimpuesto_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_islr;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_islr_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_saldofavor;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_saldofavor_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_puntoslealtad;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_puntoslealtad_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det_ninguno;
        //
        // declaracion de colecciones del detalle de la recaudacion //        
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones> recaudacion;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des> recaudacion_det_des;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des> recaudacion_det_des_efectivo;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des> recaudacion_det_des_efectivo_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des> recaudacion_det_des_ticket;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des> recaudacion_det_des_ticket_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des> recaudacion_det_des_cantidad;

        private Fundraising_PTDM.FUNDRAISING_PT.Sesiones current_sesion;

        public UI_Recaudacion_Det(object form_padre, int modo_recaudacion)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Detalle de la Recaudación...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //
            // asignacion de valores a objetos publicos //
            Form_padre = form_padre;
            Modo_recaudacion = modo_recaudacion;
            switch (Modo_recaudacion)
            {
                case 1:
                    lg_sesion = ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)form_padre).bindingSource1.Current).oid;
                    ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)form_padre).bindingSource1.Current).status = 5;
                    ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)form_padre).bindingSource1.Current).Save();
                    //
                    lg_recaudacion = Guid.Empty;
                    //ln_status_recaudacion = 1;
                    ln_status_recaudacion = 6;
                    lc_fecha_recaudacion = DateTime.Now.ToShortDateString();
                    ld_fecha_recaudacion = DateTime.Now;
                    lHeader_ant = ((Fundraising_PT.Formularios.UI_Sesion_Recauda)form_padre).HeaderMenu.Caption;
                    lAccion = "Formas de Pago";
                    lc_sucursal = Fundraising_PT.Properties.Settings.Default.nombre_sucursal;
                    ln_sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                    break;
                case 2:
                    lg_recaudacion = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).bindingSource1.Current).oid;
                    lg_sesion = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).bindingSource1.Current).sesion.oid;
                    ln_status_recaudacion = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).bindingSource1.Current).status;
                    lc_fecha_recaudacion = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).bindingSource1.Current).fecha_hora.ToShortDateString();
                    ld_fecha_recaudacion = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).bindingSource1.Current).fecha_hora;
                    lHeader_ant = ((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).HeaderMenu.Caption;
                    lc_sucursal = ((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).lookUp_sucursal.Text;
                    ln_sucursal = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).bindingSource1.Current).sucursal;
                    lAccion = "Totales Recaudación";
                    break;
                default:
                    break;
            }
            //
            // Asigno la informacion de la sesion y la recaudacion al SuperToolTip del Titulo Principal y los controles de la fecha //
            label_fecha_recaudacion.Text = lc_fecha_recaudacion;
            dateEdit_fecha_hora_recaudacion.DateTime = ld_fecha_recaudacion;
            asigna_info_titulo_principal();

            // setea botones y accesos segun los niveles de seguridad 
            seteo_nivel_seguridad();
            seteo_status_recaudacion();

            // llena los datos de colecciones de las denominaciones de monedas, puntos bancarios, proveedores tarjetas alimentacion y formas de pago creadas en el sistema //
            CriteriaOperator filtro_billetes = (new OperandProperty("tipo") == new OperandValue(1)) & (new OperandProperty("status") == new OperandValue(1));
            CriteriaOperator filtro_monedas = (new OperandProperty("tipo") == new OperandValue(2)) & (new OperandProperty("status") == new OperandValue(1));
            CriteriaOperator filtro_status = (new OperandProperty("status") == new OperandValue(1));
            //
            DevExpress.Xpo.SortProperty orden_denominaciones = (new DevExpress.Xpo.SortProperty("valor", DevExpress.Xpo.DB.SortingDirection.Descending));
            DevExpress.Xpo.SortProperty orden_formas_pagos = (new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            DevExpress.Xpo.SortProperty orden_puntos_bancarios = (new DevExpress.Xpo.SortProperty("banco_cuenta.banco.nombre", DevExpress.Xpo.DB.SortingDirection.Ascending));
            DevExpress.Xpo.SortProperty orden_bancos_cuentas = (new DevExpress.Xpo.SortProperty("banco.nombre", DevExpress.Xpo.DB.SortingDirection.Ascending));
            DevExpress.Xpo.SortingCollection orden_puntos_bancarios1 = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("banco_cuenta.banco.nombre", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_puntos_bancarios1.Add(new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            DevExpress.Xpo.SortingCollection orden_bancos_cuentas1 = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("banco.nombre", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_bancos_cuentas1.Add(new DevExpress.Xpo.SortProperty("codigo_cuenta", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            denominacion_monedas_billetes = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas>(DevExpress.Xpo.XpoDefault.Session, filtro_billetes, orden_denominaciones);
            denominacion_monedas_monedas = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas>(DevExpress.Xpo.XpoDefault.Session, filtro_monedas, orden_denominaciones);
            puntos_bancarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios>(DevExpress.Xpo.XpoDefault.Session, filtro_status, orden_puntos_bancarios);
            puntos_bancarios.Sorting = orden_puntos_bancarios1;
            bancos_cuentas = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas>(DevExpress.Xpo.XpoDefault.Session, filtro_status, orden_bancos_cuentas);
            bancos_cuentas.Sorting = orden_bancos_cuentas1;
            proveedores_ta = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA>(DevExpress.Xpo.XpoDefault.Session, filtro_status, orden_formas_pagos);
            bancos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos>(DevExpress.Xpo.XpoDefault.Session, filtro_status, orden_formas_pagos);
            
            //CriteriaOperator filtro_forma_pago_aux = (new OperandProperty("total_venta.recaudacion.oid") == new OperandValue(lg_recaudacion));
            formas_pagos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(DevExpress.Xpo.XpoDefault.Session, filtro_status, orden_formas_pagos);

            // llena los datos de la coleccion de los totales x tarjetas y puntos de ventas filtrada x recaudacion //
            CriteriaOperator filtro_totales_ventas_des = (new OperandProperty("total_venta.recaudacion.oid") == new OperandValue(lg_recaudacion));
            DevExpress.Xpo.SortProperty orden_totales_ventas_des = (new DevExpress.Xpo.SortProperty("punto_bancario.codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            totales_ventas_des = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas_Des>(DevExpress.Xpo.XpoDefault.Session, filtro_totales_ventas_des, orden_totales_ventas_des);
            // llena los datos de la coleccion de los totales x formas de pago filtrada x recaudacion //
            CriteriaOperator filtro_totales_ventas = (new OperandProperty("recaudacion.oid") == new OperandValue(lg_recaudacion));
            DevExpress.Xpo.SortProperty orden_totales_ventas = (new DevExpress.Xpo.SortProperty("forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            totales_ventas = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas>(DevExpress.Xpo.XpoDefault.Session, filtro_totales_ventas, orden_totales_ventas);

            // llena los datos de la coleccion del detalle de la recaudacion filtrado x la recaudacion de la sesion correspondiente //
            CriteriaOperator filtro_recaudacion = (new OperandProperty("oid") == new OperandValue(lg_recaudacion));
            DevExpress.Xpo.SortProperty orden_recaudacion = (new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Ascending));
            recaudacion = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones>(DevExpress.Xpo.XpoDefault.Session, filtro_recaudacion, orden_recaudacion);
            
            // llena los datos de la coleccion del detalle de la recaudacion filtrado x la recaudacion de la sesion correspondiente //
            CriteriaOperator filtro_recaudacion_det_des = (new OperandProperty("recaudacion_det.recaudacion.oid") == new OperandValue(lg_recaudacion));
            DevExpress.Xpo.SortProperty orden_recaudacion_des = (new DevExpress.Xpo.SortProperty("denominacion", DevExpress.Xpo.DB.SortingDirection.Ascending));
            recaudacion_det_des = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>(DevExpress.Xpo.XpoDefault.Session, filtro_recaudacion_det_des, orden_recaudacion_des);
            CriteriaOperator filtro_recaudacion_det = (new OperandProperty("recaudacion.oid") == new OperandValue(lg_recaudacion));
            DevExpress.Xpo.SortProperty orden_recaudacion_det = (new DevExpress.Xpo.SortProperty("forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            recaudacion_det = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(DevExpress.Xpo.XpoDefault.Session, filtro_recaudacion_det, orden_recaudacion_det);
            
            // bindeo de datos al bindingsource principal //
            bindingSource1.DataSource = recaudacion_det;

            // llena los datos de la coleccion del desglose(efectivo,ticket alimentacion) del detalle de la recaudacion filtrado x el detalle de la recaudacion correspondiente //
            if (((Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det)bindingSource1.Current) != null)
                { lg_recaudacion_det = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det)bindingSource1.Current).oid; }
            CriteriaOperator filtro_recaudacion_det_des_efectivo = (new OperandProperty("recaudacion_det.recaudacion.oid") == new OperandValue(lg_recaudacion)) & (new OperandProperty("recaudacion_det.forma_pago.tpago") == new OperandValue(1));
            CriteriaOperator filtro_recaudacion_det_des_ticket = (new OperandProperty("recaudacion_det.recaudacion.oid") == new OperandValue(lg_recaudacion)) & (new OperandProperty("recaudacion_det.forma_pago.tpago") == new OperandValue(7));
            DevExpress.Xpo.SortProperty orden_recaudacion_det_des = (new DevExpress.Xpo.SortProperty("recaudacion_det.forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            recaudacion_det_des_efectivo = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>(DevExpress.Xpo.XpoDefault.Session, filtro_recaudacion_det_des_efectivo, orden_recaudacion_det_des);
            recaudacion_det_des_ticket = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>(DevExpress.Xpo.XpoDefault.Session, filtro_recaudacion_det_des_ticket, orden_recaudacion_det_des);
            //
        }

        private void UI_Recaudacion_Det_Load(object sender, EventArgs e)
        {
            if (Modo_recaudacion == 2)
            {
                this.xtraTabPage_formaspago.PageVisible = false;
                this.simpleButton_totales_guardar.Text = "Ajustar";
                this.toolStripMenuItem_borrardatos.Enabled = false;
                lg_forma_pago = new Guid();
                lg_proveedor_ta = new Guid();
                ln_tpago = 0;
                ln_ttarjeta = 0;
            }

            // se crean las columnas al datatable de la distribucion depositar
            if (distribucion_depositar.Columns.Count <= 0)
            {
                distribucion_depositar.Columns.Add("fecha_hora", typeof(DateTime));
                distribucion_depositar.Columns.Add("usuario", typeof(Fundraising_PTDM.FUNDRAISING_PT.Usuarios));
                distribucion_depositar.Columns.Add("forma_pago", typeof(Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos));
            }

            // se crean las columnas al datatable de la distribucion grafica
            if (distribucion_grafica.Columns.Count <= 0)
            {
                distribucion_grafica.Columns.Add("ntipo", typeof(int));
                distribucion_grafica.Columns.Add("ltipo", typeof(string));
                distribucion_grafica.Columns.Add("valor", typeof(decimal));
                distribucion_grafica.PrimaryKey = new DataColumn[] { distribucion_grafica.Columns["ntipo"] };
                distribucion_grafica.DefaultView.Sort = "ntipo";
            }
            
            // se crean las columnas al datatable de los puntos bancarios
            if (puntos_bancarios_aux.Columns.Count <= 0)
            {
                puntos_bancarios_aux.Columns.Add("oid", typeof(Guid));
                puntos_bancarios_aux.Columns.Add("codigo", typeof(string));
                puntos_bancarios_aux.Columns.Add("descr", typeof(string));
                puntos_bancarios_aux.PrimaryKey = new DataColumn[] { puntos_bancarios_aux.Columns["oid"] };
            }

            // se crean las columnas al datatable de los bancos
            if (bancos_aux.Columns.Count <= 0)
            {
                bancos_aux.Columns.Add("oid", typeof(Guid));
                bancos_aux.Columns.Add("codigo", typeof(string));
                bancos_aux.Columns.Add("nombre", typeof(string));
                bancos_aux.PrimaryKey = new DataColumn[] { bancos_aux.Columns["oid"] };
            }

            // se crean las columnas al datatable de las cuentas bancarias
            if (bancos_cuentas_aux.Columns.Count <= 0)
            {
                bancos_cuentas_aux.Columns.Add("oid", typeof(Guid));
                bancos_cuentas_aux.Columns.Add("codigo_cuenta", typeof(string));
                bancos_cuentas_aux.Columns.Add("descr", typeof(string));
                bancos_cuentas_aux.PrimaryKey = new DataColumn[] { bancos_cuentas_aux.Columns["oid"] };
            }

            // se crean las columnas al datatable de las formas de pago
            if (formas_pagos_aux.Columns.Count <= 0)
            {
                formas_pagos_aux.Columns.Add("oid", typeof(Guid));
                formas_pagos_aux.Columns.Add("nombre", typeof(string));
                formas_pagos_aux.Columns.Add("tpago", typeof(int));
                formas_pagos_aux.Columns.Add("ttarjeta", typeof(int));
                formas_pagos_aux.Columns.Add("imagen", typeof(int));
                formas_pagos_aux.Columns.Add("proveedor_ta", typeof(Guid));
                formas_pagos_aux.PrimaryKey = new DataColumn[] { formas_pagos_aux.Columns["oid"] };
            }

            // se crean las columnas a los tables que guardaran las formas de pago por tipos de pagos //
            if (fp_efectivo.Columns.Count <= 0)
            {
                fp_efectivo.Columns.Add("oid", typeof(Guid));
                fp_efectivo.Columns.Add("codigo", typeof(string));
                fp_efectivo.Columns.Add("nombre", typeof(string));
                fp_efectivo.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_efectivo.Columns.Add("colecction_des", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>));
                fp_efectivo.Columns.Add("datatable_billetes", typeof(DataTable));
                fp_efectivo.Columns.Add("datatable_monedas", typeof(DataTable));
                fp_efectivo.Columns.Add("ref1", typeof(string));
                fp_efectivo.Columns.Add("ref2", typeof(string));
                fp_efectivo.Columns.Add("monto_recaudado", typeof(decimal));
                fp_efectivo.PrimaryKey = new DataColumn[] { fp_efectivo.Columns["oid"] };
            }
            //
            if (fp_tarjeta_debito.Columns.Count <= 0)
            {
                fp_tarjeta_debito.Columns.Add("oid", typeof(Guid));
                fp_tarjeta_debito.Columns.Add("nombre", typeof(string));
                fp_tarjeta_debito.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_tarjeta_debito.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_tarjeta_debito.Columns.Add("monto_recaudado", typeof(decimal));
                fp_tarjeta_debito.PrimaryKey = new DataColumn[] { fp_tarjeta_debito.Columns["oid"] };
            }
            //
            if (fp_tarjeta_credito.Columns.Count <= 0)
            {
                fp_tarjeta_credito.Columns.Add("oid", typeof(Guid));
                fp_tarjeta_credito.Columns.Add("nombre", typeof(string));
                fp_tarjeta_credito.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_tarjeta_credito.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_tarjeta_credito.Columns.Add("monto_recaudado", typeof(decimal));
                fp_tarjeta_credito.PrimaryKey = new DataColumn[] { fp_tarjeta_credito.Columns["oid"] };
            }
            //
            if (fp_tarjeta_alimentacion.Columns.Count <= 0)
            {
                fp_tarjeta_alimentacion.Columns.Add("oid", typeof(Guid));
                fp_tarjeta_alimentacion.Columns.Add("nombre", typeof(string));
                fp_tarjeta_alimentacion.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_tarjeta_alimentacion.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_tarjeta_alimentacion.Columns.Add("monto_recaudado", typeof(decimal));
                fp_tarjeta_alimentacion.Columns.Add("proveedor_ta", typeof(Guid));
                fp_tarjeta_alimentacion.PrimaryKey = new DataColumn[] { fp_tarjeta_alimentacion.Columns["oid"] };
            }
            //
            if (fp_cheque.Columns.Count <= 0)
            {
                fp_cheque.Columns.Add("oid", typeof(Guid));
                fp_cheque.Columns.Add("nombre", typeof(string));
                fp_cheque.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_cheque.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_cheque.Columns.Add("monto_recaudado", typeof(decimal));
                fp_cheque.PrimaryKey = new DataColumn[] { fp_cheque.Columns["oid"] };
            }
            //
            if (fp_credito.Columns.Count <= 0)
            {
                fp_credito.Columns.Add("oid", typeof(Guid));
                fp_credito.Columns.Add("nombre", typeof(string));
                fp_credito.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_credito.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_credito.Columns.Add("monto_recaudado", typeof(decimal));
                fp_credito.PrimaryKey = new DataColumn[] { fp_credito.Columns["oid"] };
            }
            //
            if (fp_otrospagos.Columns.Count <= 0)
            {
                fp_otrospagos.Columns.Add("oid", typeof(Guid));
                fp_otrospagos.Columns.Add("nombre", typeof(string));
                fp_otrospagos.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_otrospagos.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_otrospagos.Columns.Add("monto_recaudado", typeof(decimal));
                fp_otrospagos.PrimaryKey = new DataColumn[] { fp_otrospagos.Columns["oid"] };
            }
            //
            if (fp_pagosinternos.Columns.Count <= 0)
            {
                fp_pagosinternos.Columns.Add("oid", typeof(Guid));
                fp_pagosinternos.Columns.Add("nombre", typeof(string));
                fp_pagosinternos.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_pagosinternos.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_pagosinternos.Columns.Add("monto_recaudado", typeof(decimal));
                fp_pagosinternos.PrimaryKey = new DataColumn[] { fp_pagosinternos.Columns["oid"] };
            }
            //
            if (fp_ticketalimentacion.Columns.Count <= 0)
            {
                fp_ticketalimentacion.Columns.Add("oid", typeof(Guid));
                fp_ticketalimentacion.Columns.Add("nombre", typeof(string));
                fp_ticketalimentacion.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_ticketalimentacion.Columns.Add("colecction_des", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>));
                fp_ticketalimentacion.Columns.Add("datatable_desgloce", typeof(DataTable));
                fp_ticketalimentacion.Columns.Add("ref1", typeof(string));
                fp_ticketalimentacion.Columns.Add("ref2", typeof(string));
                fp_ticketalimentacion.Columns.Add("monto_recaudado", typeof(decimal));
                fp_ticketalimentacion.Columns.Add("proveedor_ta", typeof(Guid));
                fp_ticketalimentacion.PrimaryKey = new DataColumn[] { fp_ticketalimentacion.Columns["oid"] };
            }
            //
            if (fp_consumosinternos.Columns.Count <= 0)
            {
                fp_consumosinternos.Columns.Add("oid", typeof(Guid));
                fp_consumosinternos.Columns.Add("nombre", typeof(string));
                fp_consumosinternos.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_consumosinternos.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_consumosinternos.Columns.Add("monto_recaudado", typeof(decimal));
                fp_consumosinternos.PrimaryKey = new DataColumn[] { fp_consumosinternos.Columns["oid"] };
            }
            //
            if (fp_prepago.Columns.Count <= 0)
            {
                fp_prepago.Columns.Add("oid", typeof(Guid));
                fp_prepago.Columns.Add("nombre", typeof(string));
                fp_prepago.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_prepago.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_prepago.Columns.Add("monto_recaudado", typeof(decimal));
                fp_prepago.PrimaryKey = new DataColumn[] { fp_prepago.Columns["oid"] };
            }
            //
            if (fp_deposito.Columns.Count <= 0)
            {
                fp_deposito.Columns.Add("oid", typeof(Guid));
                fp_deposito.Columns.Add("nombre", typeof(string));
                fp_deposito.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_deposito.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_deposito.Columns.Add("monto_recaudado", typeof(decimal));
                fp_deposito.PrimaryKey = new DataColumn[] { fp_deposito.Columns["oid"] };
            }
            //
            if (fp_retencionimpuesto.Columns.Count <= 0)
            {
                fp_retencionimpuesto.Columns.Add("oid", typeof(Guid));
                fp_retencionimpuesto.Columns.Add("nombre", typeof(string));
                fp_retencionimpuesto.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_retencionimpuesto.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_retencionimpuesto.Columns.Add("monto_recaudado", typeof(decimal));
                fp_retencionimpuesto.PrimaryKey = new DataColumn[] { fp_retencionimpuesto.Columns["oid"] };
            }
            //
            if (fp_exoneracionimpuesto.Columns.Count <= 0)
            {
                fp_exoneracionimpuesto.Columns.Add("oid", typeof(Guid));
                fp_exoneracionimpuesto.Columns.Add("nombre", typeof(string));
                fp_exoneracionimpuesto.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_exoneracionimpuesto.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_exoneracionimpuesto.Columns.Add("monto_recaudado", typeof(decimal));
                fp_exoneracionimpuesto.PrimaryKey = new DataColumn[] { fp_exoneracionimpuesto.Columns["oid"] };
            }
            //
            if (fp_islr.Columns.Count <= 0)
            {
                fp_islr.Columns.Add("oid", typeof(Guid));
                fp_islr.Columns.Add("nombre", typeof(string));
                fp_islr.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_islr.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_islr.Columns.Add("monto_recaudado", typeof(decimal));
                fp_islr.PrimaryKey = new DataColumn[] { fp_islr.Columns["oid"] };
            }
            //
            if (fp_saldofavor.Columns.Count <= 0)
            {
                fp_saldofavor.Columns.Add("oid", typeof(Guid));
                fp_saldofavor.Columns.Add("nombre", typeof(string));
                fp_saldofavor.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_saldofavor.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_saldofavor.Columns.Add("monto_recaudado", typeof(decimal));
                fp_saldofavor.PrimaryKey = new DataColumn[] { fp_saldofavor.Columns["oid"] };
            }
            //
            if (fp_puntoslealtad.Columns.Count <= 0)
            {
                fp_puntoslealtad.Columns.Add("oid", typeof(Guid));
                fp_puntoslealtad.Columns.Add("nombre", typeof(string));
                fp_puntoslealtad.Columns.Add("colecction_det", typeof(DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>));
                fp_puntoslealtad.Columns.Add("datatable_detalle", typeof(DataTable));
                fp_puntoslealtad.Columns.Add("monto_recaudado", typeof(decimal));
                fp_puntoslealtad.PrimaryKey = new DataColumn[] { fp_puntoslealtad.Columns["oid"] };
            }
            // borra todas las filas que puedan existir antes de volver a cargarlas ///
            distribucion_grafica.Rows.Clear();
            distribucion_depositar.Rows.Clear();
            puntos_bancarios_aux.Rows.Clear();
            bancos_cuentas_aux.Rows.Clear();
            bancos_aux.Rows.Clear();
            //
            fp_efectivo.Rows.Clear();
            fp_tarjeta_debito.Rows.Clear();
            fp_tarjeta_credito.Rows.Clear();
            fp_tarjeta_alimentacion.Rows.Clear();
            fp_cheque.Rows.Clear();
            fp_credito.Rows.Clear();
            fp_otrospagos.Rows.Clear();
            fp_pagosinternos.Rows.Clear();
            fp_ticketalimentacion.Rows.Clear();
            fp_consumosinternos.Rows.Clear();
            fp_prepago.Rows.Clear();
            fp_deposito.Rows.Clear();
            fp_retencionimpuesto.Rows.Clear();
            fp_exoneracionimpuesto.Rows.Clear();
            fp_islr.Rows.Clear();
            fp_saldofavor.Rows.Clear();
            fp_puntoslealtad.Rows.Clear();

            // llena datatable de puntos bancarios //
            if (puntos_bancarios_aux.Rows.Count <= 0)
            {
                // carga datos del collection al datatable //
                foreach (var item_puntos_bancarios in puntos_bancarios)
                {
                    if (item_puntos_bancarios.sucursal == ln_sucursal)
                    {
                        puntos_bancarios_aux.Rows.Add(item_puntos_bancarios.oid, item_puntos_bancarios.codigo, item_puntos_bancarios.descr);
                    }
                }

                // crea columna del combo de puntos bancarios //
                DevExpress.XtraEditors.Controls.LookUpColumnInfo punto_bancario_descr = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descr","Punto Bancario");

                // vincula el repositorio de items de combo al datatables de puntos bancarios // 
                items_puntos_bancarios.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
                items_puntos_bancarios.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
                items_puntos_bancarios.AutoHeight = false;
                items_puntos_bancarios.HotTrackItems = false;
               
                items_puntos_bancarios.Columns.Add(punto_bancario_descr);
                items_puntos_bancarios.DataSource = puntos_bancarios_aux;
                items_puntos_bancarios.DisplayMember = "descr";
                items_puntos_bancarios.ValueMember = "oid";

                // vincula el repositorio item de combo a la columna del grid //
                gridColumn_tarjetas_punto_bancario.ColumnEdit = items_puntos_bancarios;
            }

            // llena datatable de cuentas bancarias //
            if (bancos_cuentas_aux.Rows.Count <= 0)
            {
                // carga datos del collection al datatable //
                foreach (var item_bancos_cuentas in bancos_cuentas)
                {
                    bancos_cuentas_aux.Rows.Add(item_bancos_cuentas.oid, item_bancos_cuentas.codigo_cuenta, item_bancos_cuentas.banco.nombre.Trim() + "-" + item_bancos_cuentas.codigo_cuenta.Trim() + "-" + item_bancos_cuentas.descr.Trim());
                }

                // crea columna del combo de cuentas bancarias //
                DevExpress.XtraEditors.Controls.LookUpColumnInfo banco_cuenta_descr = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descr", "Cuenta Bancaria");

                // vincula el repositorio de items de combo al datatables de cuentas bancarias // 
                items_bancos_cuentas.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
                items_bancos_cuentas.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
                items_bancos_cuentas.AutoHeight = false;
                items_bancos_cuentas.HotTrackItems = false;

                items_bancos_cuentas.Columns.Add(banco_cuenta_descr);
                items_bancos_cuentas.DataSource = bancos_cuentas_aux;
                items_bancos_cuentas.DisplayMember = "descr";
                items_bancos_cuentas.ValueMember = "oid";

                // vincula el repositorio item de combo a la columna del grid //
                gridColumn_depositos_banco_cuenta.ColumnEdit = items_bancos_cuentas;
            }

            // llena datatable de bancos //
            if (bancos_aux.Rows.Count <= 0)
            {
                // carga datos del collection al datatable //
                foreach (var item_bancos in bancos)
                {
                    bancos_aux.Rows.Add(item_bancos.oid, item_bancos.codigo, item_bancos.nombre);
                }

                // crea columna del combo de bancos //
                DevExpress.XtraEditors.Controls.LookUpColumnInfo banco_nombre = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nombre","Banco");

                // vincula el repositorio de items de combo al datatables de bancos // 
                items_bancos.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
                items_bancos.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
                items_bancos.AutoHeight = false;
                items_bancos.HotTrackItems = false;
                items_bancos.Columns.Add(banco_nombre);
                items_bancos.DataSource = bancos_aux;
                items_bancos.DisplayMember = "nombre";
                items_bancos.ValueMember = "oid";

                // vincula el repositorio item de combo a la columna del grid //
                gridColumn_cheques_banco.ColumnEdit = items_bancos;
            }
            
            // llama al metodo para llenar los datos de las colecciones x formas de pagos //
            this.carga_collection_tipopagos();

            // recorre las formas de pagos creadas en el sistema para activar y los pages necesarios y llenar datos en las colecciones x formas de pagos //
            foreach (Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos forma_pago in formas_pagos)
            {
                // activa los pages x las formas de pago creadas en el sistema //
                foreach (DevExpress.XtraTab.XtraTabPage page_list in this.xtraTabControl_detalle.TabPages)
                {
                    if (page_list.Tag.ToString().Trim() == forma_pago.tpago.ToString().Trim())
                    {
                        page_list.PageVisible = true;
                    }
                }

                // carga los datatables de formas de pagos por cada tipo de pago y sus respectivos colecction de detalles, y activa los totales x formas de pagos //
                switch (forma_pago.tpago)
                {
                    case 1:
                        Guid lg_recauda_det_efec = new Guid();
                        ln_monto_recaudado = 0;
                        lc_ref1 = "";
                        lc_ref2 = "";
                        //
                        CriteriaOperator filtro_recaudacion_det_efectivo = (new OperandProperty("forma_pago.oid") == new OperandValue(forma_pago.oid));
                        recaudacion_det_efectivo_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det_efectivo, filtro_recaudacion_det_efectivo);
                        //
                        if (recaudacion_det_efectivo_aux.Count > 0)
                        {
                            lg_recauda_det_efec = recaudacion_det_efectivo_aux[0].oid;
                            ln_monto_recaudado = recaudacion_det_efectivo_aux[0].monto_recaudado;
                            lc_ref1 = recaudacion_det_efectivo_aux[0].ref1;
                            lc_ref2 = recaudacion_det_efectivo_aux[0].ref2;
                        }
                        //
                        CriteriaOperator filtro_recaudacion_det_des_efectivo = (new OperandProperty("recaudacion_det.oid") == new OperandValue(lg_recauda_det_efec));
                        recaudacion_det_des_efectivo_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>(recaudacion_det_des_efectivo, filtro_recaudacion_det_des_efectivo);
                        //
                        fp_efectivo.Rows.Add(forma_pago.oid, forma_pago.codigo, forma_pago.nombre, recaudacion_det_efectivo_aux, recaudacion_det_des_efectivo_aux, new DataTable(), new DataTable(), lc_ref1, lc_ref2, ln_monto_recaudado);
                        this.carga_detalle_efectivo(1);
                        this.labelControl_matriz_0_0.Enabled = true;
                        this.labelControl_matriz_1_0.Enabled = true;
                        this.labelControl_matriz_0_0.Font = new System.Drawing.Font(this.labelControl_matriz_0_0.Font, FontStyle.Bold);
                        this.labelControl_matriz_1_0.Font = new System.Drawing.Font(this.labelControl_matriz_1_0.Font, FontStyle.Bold);
                        //
                        break;
                    case 2:
                        //
                        ln_ttarjeta = forma_pago.ttarjeta;
                        switch (forma_pago.ttarjeta)
                        {
                            case 1:
                                ln_monto_recaudado = 0;
                                //
                                CriteriaOperator filtro_recaudacion_det_tarjeta_debito = (new OperandProperty("forma_pago.oid") == new OperandValue(forma_pago.oid));
                                recaudacion_det_tarjeta_debito_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det_tarjeta_debito, filtro_recaudacion_det_tarjeta_debito);
                                //
                                if (recaudacion_det_tarjeta_debito_aux.Count > 0)
                                {
                                    ln_monto_recaudado = recaudacion_det_tarjeta_debito_aux[0].monto_recaudado;
                                }
                                //
                                fp_tarjeta_debito.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_tarjeta_debito_aux, new DataTable(), ln_monto_recaudado);
                                this.carga_detalle_tarjetas(1);
                                this.labelControl_matriz_0_2.Enabled = true;
                                this.labelControl_matriz_1_2.Enabled = true;
                                this.labelControl_matriz_0_2.Font = new System.Drawing.Font(this.labelControl_matriz_0_2.Font, FontStyle.Bold);
                                this.labelControl_matriz_1_2.Font = new System.Drawing.Font(this.labelControl_matriz_1_2.Font, FontStyle.Bold);
                                //
                                break;
                            case 2:
                                ln_monto_recaudado = 0;
                                //
                                CriteriaOperator filtro_recaudacion_det_tarjeta_credito = (new OperandProperty("forma_pago.oid") == new OperandValue(forma_pago.oid));
                                recaudacion_det_tarjeta_credito_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det_tarjeta_credito, filtro_recaudacion_det_tarjeta_credito);
                                //
                                if (recaudacion_det_tarjeta_credito_aux.Count > 0)
                                {
                                    ln_monto_recaudado = recaudacion_det_tarjeta_credito_aux[0].monto_recaudado;
                                }
                                //
                                fp_tarjeta_credito.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_tarjeta_credito_aux, new DataTable(), ln_monto_recaudado);
                                this.carga_detalle_tarjetas(1);
                                this.labelControl_matriz_0_3.Enabled = true;
                                this.labelControl_matriz_1_3.Enabled = true;
                                this.labelControl_matriz_0_3.Font = new System.Drawing.Font(this.labelControl_matriz_0_3.Font, FontStyle.Bold);
                                this.labelControl_matriz_1_3.Font = new System.Drawing.Font(this.labelControl_matriz_1_3.Font, FontStyle.Bold);
                                //
                                break;
                            case 3:
                                ln_monto_recaudado = 0;
                                //
                                CriteriaOperator filtro_recaudacion_det_tarjeta_alimentacion = (new OperandProperty("forma_pago.oid") == new OperandValue(forma_pago.oid));
                                recaudacion_det_tarjeta_alimentacion_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det_tarjeta_alimentacion, filtro_recaudacion_det_tarjeta_alimentacion);
                                //
                                if (recaudacion_det_tarjeta_alimentacion_aux.Count > 0)
                                {
                                    ln_monto_recaudado = recaudacion_det_tarjeta_alimentacion_aux[0].monto_recaudado;
                                }
                                //
                                fp_tarjeta_alimentacion.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_tarjeta_alimentacion_aux, new DataTable(), ln_monto_recaudado, forma_pago.proveedor_ta.oid);
                                this.carga_detalle_tarjetas(1);
                                this.labelControl_matriz_0_4.Enabled = true;
                                this.labelControl_matriz_1_4.Enabled = true;
                                this.labelControl_matriz_0_4.Font = new System.Drawing.Font(this.labelControl_matriz_0_4.Font, FontStyle.Bold);
                                this.labelControl_matriz_1_4.Font = new System.Drawing.Font(this.labelControl_matriz_1_4.Font, FontStyle.Bold);
                                //
                                break;
                        }                
                        this.labelControl_matriz_01_1.Enabled = true;
                        this.labelControl_matriz_01_5.Enabled = true;
                        this.labelControl_matriz_01_5.Font = new System.Drawing.Font(this.labelControl_matriz_01_5.Font, FontStyle.Bold);
                        break;
                    case 3:
                        ln_monto_recaudado = 0;
                        //
                        CriteriaOperator filtro_recaudacion_det_cheque = (new OperandProperty("forma_pago.oid") == new OperandValue(forma_pago.oid));
                        recaudacion_det_cheque_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det_cheque, filtro_recaudacion_det_cheque);
                        //
                        if (recaudacion_det_cheque_aux.Count > 0)
                        {
                            ln_monto_recaudado = recaudacion_det_cheque_aux[0].monto_recaudado;
                        }
                        //
                        fp_cheque.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_cheque_aux, new DataTable(), ln_monto_recaudado);
                        this.carga_detalle_cheques(1);
                        this.labelControl_matriz_0_6.Enabled = true;
                        this.labelControl_matriz_1_6.Enabled = true;
                        this.labelControl_matriz_0_6.Font = new System.Drawing.Font(this.labelControl_matriz_0_6.Font, FontStyle.Bold);
                        this.labelControl_matriz_1_6.Font = new System.Drawing.Font(this.labelControl_matriz_1_6.Font, FontStyle.Bold);
                        //
                        break;
                    case 4:
                        ln_monto_recaudado = 0;
                        //
                        CriteriaOperator filtro_recaudacion_det_credito = (new OperandProperty("forma_pago.oid") == new OperandValue(forma_pago.oid));
                        recaudacion_det_credito_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det_credito, filtro_recaudacion_det_credito);
                        //
                        if (recaudacion_det_credito_aux.Count > 0)
                        {
                            ln_monto_recaudado = recaudacion_det_credito_aux[0].monto_recaudado;
                        }
                        //
                        fp_credito.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_credito_aux, new DataTable(), ln_monto_recaudado);
                        this.carga_detalle_creditos(1);
                        this.labelControl_matriz_2_0.Enabled = true;
                        this.labelControl_matriz_3_0.Enabled = true;
                        this.labelControl_matriz_2_0.Font = new System.Drawing.Font(this.labelControl_matriz_2_0.Font, FontStyle.Bold);
                        this.labelControl_matriz_3_0.Font = new System.Drawing.Font(this.labelControl_matriz_3_0.Font, FontStyle.Bold);
                        //
                        break;
                    case 5:
                        recaudacion_det_otrospagos.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_op => det_op.forma_pago.oid.Equals(forma_pago.oid));
                        fp_otrospagos.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_otrospagos, new DataTable(), 0);
                        this.labelControl_matriz_2_1.Enabled = true;
                        this.labelControl_matriz_3_1.Enabled = true;
                        this.labelControl_matriz_2_1.Font = new System.Drawing.Font(this.labelControl_matriz_2_1.Font, FontStyle.Bold);
                        this.labelControl_matriz_3_1.Font = new System.Drawing.Font(this.labelControl_matriz_3_1.Font, FontStyle.Bold);
                        //
                        break;
                    case 6:
                        recaudacion_det_pagosinternos.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_pi => det_pi.forma_pago.oid.Equals(forma_pago.oid));
                        fp_pagosinternos.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_pagosinternos, new DataTable(), 0);
                        this.labelControl_matriz_2_2.Enabled = true;
                        this.labelControl_matriz_3_2.Enabled = true;
                        this.labelControl_matriz_2_2.Font = new System.Drawing.Font(this.labelControl_matriz_2_2.Font, FontStyle.Bold);
                        this.labelControl_matriz_3_2.Font = new System.Drawing.Font(this.labelControl_matriz_3_2.Font, FontStyle.Bold);
                        //
                        break;
                    case 7:
                        Guid lg_recauda_det_ticket = new Guid();
                        ln_monto_recaudado = 0;
                        lc_ref1 = "";
                        lc_ref2 = "";
                        //
                        CriteriaOperator filtro_recaudacion_det_ticket = (new OperandProperty("forma_pago.oid") == new OperandValue(forma_pago.oid));
                        recaudacion_det_ticketalimentacion_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det_ticketalimentacion, filtro_recaudacion_det_ticket);
                        //
                        if (recaudacion_det_ticketalimentacion_aux.Count > 0)
                        {
                            lg_recauda_det_ticket = recaudacion_det_ticketalimentacion_aux[0].oid;
                            ln_monto_recaudado = recaudacion_det_ticketalimentacion_aux[0].monto_recaudado;
                            lc_ref1 = recaudacion_det_ticketalimentacion_aux[0].ref1;
                            lc_ref2 = recaudacion_det_ticketalimentacion_aux[0].ref2;
                        }
                        //
                        CriteriaOperator filtro_recaudacion_det_des_ticket = (new OperandProperty("recaudacion_det.oid") == new OperandValue(lg_recauda_det_ticket));
                        recaudacion_det_des_ticket_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>(recaudacion_det_des_ticket, filtro_recaudacion_det_des_ticket);

                        //
                        fp_ticketalimentacion.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_ticketalimentacion_aux, recaudacion_det_des_ticket_aux, new DataTable(), lc_ref1, lc_ref2, ln_monto_recaudado, forma_pago.proveedor_ta.oid);
                        this.carga_detalle_ticket(1);
                        this.labelControl_matriz_2_3.Enabled = true;
                        this.labelControl_matriz_3_3.Enabled = true;
                        this.labelControl_matriz_2_3.Font = new System.Drawing.Font(this.labelControl_matriz_2_3.Font, FontStyle.Bold);
                        this.labelControl_matriz_3_3.Font = new System.Drawing.Font(this.labelControl_matriz_3_3.Font, FontStyle.Bold);
                      //
                        break;
                    case 8:
                        recaudacion_det_consumosinternos.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_ci => det_ci.forma_pago.oid.Equals(forma_pago.oid));
                        fp_consumosinternos.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_consumosinternos, new DataTable(), 0);
                        this.labelControl_matriz_2_4.Enabled = true;
                        this.labelControl_matriz_3_4.Enabled = true;
                        this.labelControl_matriz_2_4.Font = new System.Drawing.Font(this.labelControl_matriz_2_4.Font, FontStyle.Bold);
                        this.labelControl_matriz_3_4.Font = new System.Drawing.Font(this.labelControl_matriz_3_4.Font, FontStyle.Bold);
                        //
                        break;
                    case 9:
                        recaudacion_det_prepago.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_pr => det_pr.forma_pago.oid.Equals(forma_pago.oid));
                        fp_prepago.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_prepago, new DataTable(), 0);
                        this.labelControl_matriz_2_5.Enabled = true;
                        this.labelControl_matriz_3_5.Enabled = true;
                        this.labelControl_matriz_2_5.Font = new System.Drawing.Font(this.labelControl_matriz_2_5.Font, FontStyle.Bold);
                        this.labelControl_matriz_3_5.Font = new System.Drawing.Font(this.labelControl_matriz_3_5.Font, FontStyle.Bold);
                        //
                        break;
                    case 10:
                        ln_monto_recaudado = 0;
                        //
                        CriteriaOperator filtro_recaudacion_det_deposito = (new OperandProperty("forma_pago.oid") == new OperandValue(forma_pago.oid));
                        recaudacion_det_deposito_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det_deposito, filtro_recaudacion_det_deposito);
                        //
                        if (recaudacion_det_deposito_aux.Count > 0)
                        {
                            ln_monto_recaudado = recaudacion_det_deposito_aux[0].monto_recaudado;
                        }
                        //
                        fp_deposito.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_deposito_aux, new DataTable(), ln_monto_recaudado);
                        this.carga_detalle_depositos(1);
                        this.labelControl_matriz_2_6.Enabled = true;
                        this.labelControl_matriz_3_6.Enabled = true;
                        this.labelControl_matriz_2_6.Font = new System.Drawing.Font(this.labelControl_matriz_2_6.Font, FontStyle.Bold);
                        this.labelControl_matriz_3_6.Font = new System.Drawing.Font(this.labelControl_matriz_3_6.Font, FontStyle.Bold);
                        //
                        break;
                    case 11:
                        recaudacion_det_retencionimpuesto.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_ri => det_ri.forma_pago.oid.Equals(forma_pago.oid));
                        fp_retencionimpuesto.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_retencionimpuesto, new DataTable(), 0);
                        this.labelControl_matriz_4_0.Enabled = true;
                        this.labelControl_matriz_5_0.Enabled = true;
                        this.labelControl_matriz_4_0.Font = new System.Drawing.Font(this.labelControl_matriz_4_0.Font, FontStyle.Bold);
                        this.labelControl_matriz_5_0.Font = new System.Drawing.Font(this.labelControl_matriz_5_0.Font, FontStyle.Bold);
                        //
                        break;
                    case 12:
                        recaudacion_det_exoneracionimpuesto.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_ei => det_ei.forma_pago.oid.Equals(forma_pago.oid));
                        fp_exoneracionimpuesto.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_exoneracionimpuesto, new DataTable(), 0);
                        this.labelControl_matriz_4_1.Enabled = true;
                        this.labelControl_matriz_5_1.Enabled = true;
                        this.labelControl_matriz_4_1.Font = new System.Drawing.Font(this.labelControl_matriz_4_1.Font, FontStyle.Bold);
                        this.labelControl_matriz_5_1.Font = new System.Drawing.Font(this.labelControl_matriz_5_1.Font, FontStyle.Bold);
                        //
                        break;
                    case 13:
                        recaudacion_det_islr.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_is => det_is.forma_pago.oid.Equals(forma_pago.oid));
                        fp_islr.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_islr, new DataTable(), 0);
                        this.labelControl_matriz_4_2.Enabled = true;
                        this.labelControl_matriz_5_2.Enabled = true;
                        this.labelControl_matriz_4_2.Font = new System.Drawing.Font(this.labelControl_matriz_4_2.Font, FontStyle.Bold);
                        this.labelControl_matriz_5_2.Font = new System.Drawing.Font(this.labelControl_matriz_5_2.Font, FontStyle.Bold);
                        //
                        break;
                    case 14:
                        recaudacion_det_saldofavor.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_sf => det_sf.forma_pago.oid.Equals(forma_pago.oid));
                        fp_saldofavor.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_saldofavor, new DataTable(), 0);
                        this.labelControl_matriz_4_3.Enabled = true;
                        this.labelControl_matriz_5_3.Enabled = true;
                        this.labelControl_matriz_4_3.Font = new System.Drawing.Font(this.labelControl_matriz_4_3.Font, FontStyle.Bold);
                        this.labelControl_matriz_5_3.Font = new System.Drawing.Font(this.labelControl_matriz_5_3.Font, FontStyle.Bold);
                        //
                        break;
                    case 15:
                        recaudacion_det_puntoslealtad.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_pl => det_pl.forma_pago.oid.Equals(forma_pago.oid));
                        fp_puntoslealtad.Rows.Add(forma_pago.oid, forma_pago.nombre, recaudacion_det_puntoslealtad, new DataTable(), 0);
                        this.labelControl_matriz_4_4.Enabled = true;
                        this.labelControl_matriz_5_4.Enabled = true;
                        this.labelControl_matriz_4_4.Font = new System.Drawing.Font(this.labelControl_matriz_4_4.Font, FontStyle.Bold);
                        this.labelControl_matriz_5_4.Font = new System.Drawing.Font(this.labelControl_matriz_5_4.Font, FontStyle.Bold);
                        //
                        break;
                    default:
                        break;
                }
            }

            // se asocian los datatables de formas de pagos por tipo de pago, a los combos correspondientes /// 
            if (fp_efectivo != null && fp_efectivo.Rows.Count > 0)  // 01 - EFECTIVO 
            {
                fp_efectivo.DefaultView.Sort = "codigo";
                fp_efectivo = fp_efectivo.DefaultView.ToTable();
                Guid aux_guid_fp_efectivo = (Guid)fp_efectivo.Rows[0]["oid"];
                fp_efectivo.PrimaryKey = new DataColumn[] { fp_efectivo.Columns["oid"] };
                //
                checkedComboBoxEdit_efectivo_formaspagos.Properties.DataSource = fp_efectivo;
                checkedComboBoxEdit_efectivo_formaspagos.Properties.DisplayMember = "nombre";
                checkedComboBoxEdit_efectivo_formaspagos.Properties.ValueMember = "oid";
                checkedComboBoxEdit_efectivo_formaspagos.EditValue = aux_guid_fp_efectivo;
                //checkedComboBoxEdit_efectivo_formaspagos.EditValue = fp_efectivo.Rows[0]["oid"];
            }
            // 02 - TARJETAS
            radioGroup_tarjertas_tipo_tarjeta.Properties.Items[0].Enabled = false;
            if (fp_tarjeta_debito != null && fp_tarjeta_debito.Rows.Count > 0)  // TARJETA DE DEBITO
            {
                radioGroup_tarjertas_tipo_tarjeta.Properties.Items[0].Enabled = true;
                radioGroup_tarjertas_tipo_tarjeta.SelectedIndex = 0;
                //
                checkedComboBoxEdit_tarjetas_formaspagos.Properties.DataSource = fp_tarjeta_debito;
                checkedComboBoxEdit_tarjetas_formaspagos.Properties.DisplayMember = "nombre";
                checkedComboBoxEdit_tarjetas_formaspagos.Properties.ValueMember = "oid";
                checkedComboBoxEdit_tarjetas_formaspagos.EditValue = fp_tarjeta_debito.Rows[0]["oid"];
            }
            else
            {
                radioGroup_tarjertas_tipo_tarjeta.Properties.Items[1].Enabled = false;
                if (fp_tarjeta_credito != null && fp_tarjeta_credito.Rows.Count > 0)  // TARJETA DE CREDITO
                {
                    radioGroup_tarjertas_tipo_tarjeta.Properties.Items[1].Enabled = true;
                    radioGroup_tarjertas_tipo_tarjeta.SelectedIndex = 1;
                    //
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.DataSource = fp_tarjeta_credito;
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.DisplayMember = "nombre";
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.ValueMember = "oid";
                    checkedComboBoxEdit_tarjetas_formaspagos.EditValue = fp_tarjeta_credito.Rows[0]["oid"];
                }
                else
                {
                    radioGroup_tarjertas_tipo_tarjeta.Properties.Items[2].Enabled = false;
                    if (fp_tarjeta_alimentacion != null && fp_tarjeta_alimentacion.Rows.Count > 0)  // TARJETA ALIMENTACION
                    {
                        radioGroup_tarjertas_tipo_tarjeta.Properties.Items[2].Enabled = true;
                        radioGroup_tarjertas_tipo_tarjeta.SelectedIndex = 2;
                        //
                        checkedComboBoxEdit_tarjetas_formaspagos.Properties.DataSource = fp_tarjeta_alimentacion;
                        checkedComboBoxEdit_tarjetas_formaspagos.Properties.DisplayMember = "nombre";
                        checkedComboBoxEdit_tarjetas_formaspagos.Properties.ValueMember = "oid";
                        checkedComboBoxEdit_tarjetas_formaspagos.EditValue = fp_tarjeta_alimentacion.Rows[0]["oid"];
                    }
                    else
                    {
                    //    radioGroup_tarjertas_tipo_tarjeta.SelectedIndex = 0;
                    //    //
                    //    checkedComboBoxEdit_tarjetas_formaspagos.Properties.DataSource = fp_tarjeta_debito;
                    //    checkedComboBoxEdit_tarjetas_formaspagos.Properties.DisplayMember = "nombre";
                    //    checkedComboBoxEdit_tarjetas_formaspagos.Properties.ValueMember = "oid";
                    //    checkedComboBoxEdit_tarjetas_formaspagos.EditValue = fp_tarjeta_debito.Rows[0]["oid"];
                    }
                }
            }
            //
            if (fp_cheque != null && fp_cheque.Rows.Count > 0)  // 03 - CHEQUES 
            {
                checkedComboBoxEdit_cheques_formaspagos.Properties.DataSource = fp_cheque;
                checkedComboBoxEdit_cheques_formaspagos.Properties.DisplayMember = "nombre";
                checkedComboBoxEdit_cheques_formaspagos.Properties.ValueMember = "oid";
                checkedComboBoxEdit_cheques_formaspagos.EditValue = fp_cheque.Rows[0]["oid"];
            }
            //
            if (fp_credito != null && fp_credito.Rows.Count > 0)  // 04 - CREDITOS 
            {
                checkedComboBoxEdit_creditos_formaspagos.Properties.DataSource = fp_credito;
                checkedComboBoxEdit_creditos_formaspagos.Properties.DisplayMember = "nombre";
                checkedComboBoxEdit_creditos_formaspagos.Properties.ValueMember = "oid";
                checkedComboBoxEdit_creditos_formaspagos.EditValue = fp_credito.Rows[0]["oid"];
            }
            //
            if (fp_ticketalimentacion != null && fp_ticketalimentacion.Rows.Count > 0)  // 07 - TICKET ALIMENTACION
            {
                checkedComboBoxEdit_ticket_formaspagos.Properties.DataSource = fp_ticketalimentacion;
                checkedComboBoxEdit_ticket_formaspagos.Properties.DisplayMember = "nombre";
                checkedComboBoxEdit_ticket_formaspagos.Properties.ValueMember = "oid";
                checkedComboBoxEdit_ticket_formaspagos.EditValue = fp_ticketalimentacion.Rows[0]["oid"];
            }
            //
            if (fp_deposito != null && fp_deposito.Rows.Count > 0)  // 10 - DEPOSITOS 
            {
                checkedComboBoxEdit_depositos_formaspagos.Properties.DataSource = fp_deposito;
                checkedComboBoxEdit_depositos_formaspagos.Properties.DisplayMember = "nombre";
                checkedComboBoxEdit_depositos_formaspagos.Properties.ValueMember = "oid";
                checkedComboBoxEdit_depositos_formaspagos.EditValue = fp_deposito.Rows[0]["oid"];
            }
            //

            // llena el datatable de formas de pago y lo asigna al imagelistbox //
            llena_datatable_formaspagos();
            this.imageListBoxControl_formas_pago.DataSource = formas_pagos_aux;
            this.imageListBoxControl_formas_pago.DisplayMember = "nombre";
            this.imageListBoxControl_formas_pago.ValueMember = "oid";
            this.imageListBoxControl_formas_pago.ImageIndexMember = "imagen";

            // llama los metodos para cargar y sumar los totales generales // 
            this.carga_totales_iniciales();
            this.carga_totales(0);
            this.suma_totales();

            // bindeo de eventos necesarios //
            //this.labelControl_matriz_0_0.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_0_2.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_0_3.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_0_4.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_0_6.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_01_1.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_01_5.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_012345_7.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_1_0.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_1_2.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_1_3.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_1_4.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_1_6.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_2_0.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_2_1.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_2_2.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_2_3.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_2_4.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_2_5.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_2_6.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_3_0.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_3_1.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_3_2.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_3_3.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_3_4.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_3_5.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_3_6.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_4_0.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_4_1.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_4_2.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_4_3.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_4_4.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_4_5.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_4_6.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_5_0.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_5_1.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_5_2.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_5_3.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_5_4.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_5_5.Resize += new EventHandler(labelControl_totales_Resize);
            //this.labelControl_matriz_5_6.Resize += new EventHandler(labelControl_totales_Resize);
            //
            //this.labelControl_matriz_1_0.ToolTip = this.labelControl_matriz_1_0.ToolTip + "\n" + this.labelControl_matriz_1_0.Text;
            //this.labelControl_matriz_1_2.ToolTip = this.labelControl_matriz_1_2.ToolTip + "\n" + this.labelControl_matriz_1_2.Text;
            //this.labelControl_matriz_1_3.ToolTip = this.labelControl_matriz_1_3.ToolTip + "\n" + this.labelControl_matriz_1_3.Text;
            //this.labelControl_matriz_1_4.ToolTip = this.labelControl_matriz_1_4.ToolTip + "\n" + this.labelControl_matriz_1_4.Text;
            //this.labelControl_matriz_1_6.ToolTip = this.labelControl_matriz_1_6.ToolTip + "\n" + this.labelControl_matriz_1_6.Text;
            //this.labelControl_matriz_3_0.ToolTip = this.labelControl_matriz_3_0.ToolTip + "\n" + this.labelControl_matriz_3_0.Text;
            //this.labelControl_matriz_3_1.ToolTip = this.labelControl_matriz_3_1.ToolTip + "\n" + this.labelControl_matriz_3_1.Text;
            //this.labelControl_matriz_3_2.ToolTip = this.labelControl_matriz_3_2.ToolTip + "\n" + this.labelControl_matriz_3_2.Text;
            //this.labelControl_matriz_3_3.ToolTip = this.labelControl_matriz_3_3.ToolTip + "\n" + this.labelControl_matriz_3_3.Text;
            //this.labelControl_matriz_3_4.ToolTip = this.labelControl_matriz_3_4.ToolTip + "\n" + this.labelControl_matriz_3_4.Text;
            //this.labelControl_matriz_3_5.ToolTip = this.labelControl_matriz_3_5.ToolTip + "\n" + this.labelControl_matriz_3_5.Text;
            //this.labelControl_matriz_3_6.ToolTip = this.labelControl_matriz_3_6.ToolTip + "\n" + this.labelControl_matriz_3_6.Text;
            //this.labelControl_matriz_5_0.ToolTip = this.labelControl_matriz_5_0.ToolTip + "\n" + this.labelControl_matriz_5_0.Text;
            //this.labelControl_matriz_5_1.ToolTip = this.labelControl_matriz_5_1.ToolTip + "\n" + this.labelControl_matriz_5_1.Text;
            //this.labelControl_matriz_5_2.ToolTip = this.labelControl_matriz_5_2.ToolTip + "\n" + this.labelControl_matriz_5_2.Text;
            //this.labelControl_matriz_5_3.ToolTip = this.labelControl_matriz_5_3.ToolTip + "\n" + this.labelControl_matriz_5_3.Text;
            //this.labelControl_matriz_5_4.ToolTip = this.labelControl_matriz_5_4.ToolTip + "\n" + this.labelControl_matriz_5_4.Text;
            //
            this.xtraTabControl_detalle.Click += new EventHandler(xtraTabControl_detalle_Click);
            this.xtraTabPage_formaspago.Enter += new EventHandler(xtraTabPage_formaspago_Enter);
            this.xtraTabPage_efectivo.Enter += new EventHandler(xtraTabPage_efectivo_Enter);
            this.xtraTabPage_tarjeta.Enter += new EventHandler(xtraTabPage_tarjeta_Enter);
            this.xtraTabPage_cheque.Enter += new EventHandler(xtraTabPage_cheque_Enter);
            this.xtraTabPage_credito.Enter += new EventHandler(xtraTabPage_credito_Enter);
            this.xtraTabPage_ticketa.Enter += new EventHandler(xtraTabPage_ticketa_Enter);
            this.xtraTabPage_deposito.Enter += new EventHandler(xtraTabPage_deposito_Enter);
            //
            this.simpleButton_imprimir_billetes.Click += new EventHandler(simpleButton_imprimir_billetes_Click);
            this.simpleButton_imprimir_monedas.Click += new EventHandler(simpleButton_imprimir_monedas_Click);
            this.simpleButton_imprimir_tarjetas.Click += new EventHandler(simpleButton_imprimir_tarjetas_Click);
            this.simpleButton_imprimir_cheques.Click += new EventHandler(simpleButton_imprimir_cheques_Click);
            this.simpleButton_imprimir_creditos.Click += new EventHandler(simpleButton_imprimir_creditos_Click);
            this.simpleButton_imprimir_tickets.Click += new EventHandler(simpleButton_imprimir_tickets_Click);
            this.simpleButton_imprimir_depositos.Click += new EventHandler(simpleButton_imprimir_depositos_Click);            
            //
            //bindeo de botones de eliminar registros en los grids //
            this.simpleButton_eliminar_tarjetas.Click += new EventHandler(simpleButton_eliminar_registros);
            this.simpleButton_eliminar_cheques1.Click += new EventHandler(simpleButton_eliminar_registros);
            this.simpleButton_eliminar_creditos.Click += new EventHandler(simpleButton_eliminar_registros);
            this.simpleButton_eliminar_tickets.Click += new EventHandler(simpleButton_eliminar_registros);
            this.simpleButton_eliminar_depositos.Click += new EventHandler(simpleButton_eliminar_registros);
            //
            this.checkedComboBoxEdit_efectivo_formaspagos.EditValueChanged += new EventHandler(checkedComboBoxEdit_efectivo_formaspagos_EditValueChanged);
            this.checkedComboBoxEdit_tarjetas_formaspagos.EditValueChanged += new EventHandler(checkedComboBoxEdit_tarjetas_formaspagos_EditValueChanged);
            this.radioGroup_tarjertas_tipo_tarjeta.SelectedIndexChanged += new EventHandler(radioGroup_tarjertas_tipo_tarjeta_SelectedIndexChanged);
            this.checkedComboBoxEdit_cheques_formaspagos.EditValueChanged += new EventHandler(checkedComboBoxEdit_cheques_formaspagos_EditValueChanged);
            this.checkedComboBoxEdit_creditos_formaspagos.EditValueChanged += new EventHandler(checkedComboBoxEdit_creditos_formaspagos_EditValueChanged);
            this.checkedComboBoxEdit_ticket_formaspagos.EditValueChanged += new EventHandler(checkedComboBoxEdit_ticket_formaspagos_EditValueChanged);
            this.checkedComboBoxEdit_depositos_formaspagos.EditValueChanged += new EventHandler(checkedComboBoxEdit_depositos_formaspagos_EditValueChanged);
            //
            this.imageListBoxControl_formas_pago.Click += new EventHandler(imageListBoxControl_formas_pago_Click);
            //
            this.gridColumn_efectivo_billetes_cantidad.View.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(View_CellValueChanged_cantidad_billetes);
            this.gridColumn_efectivo_monedas_cantidad.View.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(View_CellValueChanged_cantidad_monedas);
            //     
            this.gridView_tarjetas.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(gridView_tarjetas_ValidateRow);
            this.gridView_tarjetas.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gridView_tarjetas_RowUpdated);
            //
            this.gridView_cheques.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(gridView_cheques_ValidateRow);
            this.gridView_cheques.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gridView_cheques_RowUpdated);
            //
            this.gridView_creditos.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(gridView_creditos_ValidateRow);
            this.gridView_creditos.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gridView_creditos_RowUpdated);
            //
            this.gridView_tickets.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(gridView_tickets_ValidateRow);
            this.gridView_tickets.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gridView_tickets_RowUpdated);
            //
            this.gridView_depositos.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(gridView_depositos_ValidateRow);
            this.gridView_depositos.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gridView_depositos_RowUpdated);
            //
            this.textBox_efectivo_ref1.Validated += new EventHandler(valid_ref_efectivo);
            this.textBox_efectivo_ref2.Validated += new EventHandler(valid_ref_efectivo);
            this.textBox_ticket_ref1.Validated += new EventHandler(valid_ref_tickets);
            this.textBox_ticket_ref2.Validated += new EventHandler(valid_ref_tickets);
            //
            this.simpleButton_totales_abre_recaudacion_cerrada.Click += new EventHandler(simpleButton_totales_abre_recaudacion_cerrada_Click);
            this.simpleButton_totales_guardar.Click += new EventHandler(simpleButton_totales_guardar_Click);
            //this.simpleButton_visualizar_reportes.Click += new EventHandler(simpleButton_visualizar_reportes_Click);
            this.simpleButton_imprime_cuadre_detallado.Click += new EventHandler(simpleButton_imprime_cuadre_detallado_Click);
            this.simpleButton_imprime_cuadre_resumido.Click += new EventHandler(simpleButton_imprime_cuadre_resumido_Click);
            //
            this.dateEdit_fecha_hora_recaudacion.DateTimeChanged += new EventHandler(dateEdit_fecha_hora_recaudacion_DateTimeChanged);
            //
            this.simpleButton_totales_estadisticasgrafica.Click += new EventHandler(simpleButton_totales_estadisticasgrafica_Click);
            this.simpleButton_salir.Click += new EventHandler(simpleButton_salir_Click);
            //
            this.toolStripMenuItem_verdetalle.Click +=new EventHandler(toolStripMenuItem_verdetalle_Click);
            //
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void dateEdit_fecha_hora_recaudacion_DateTimeChanged(object sender, EventArgs e)
        {
            ld_fecha_recaudacion = dateEdit_fecha_hora_recaudacion.DateTime;
            lc_fecha_recaudacion = ld_fecha_recaudacion.ToShortDateString();
            label_fecha_recaudacion.Text = lc_fecha_recaudacion;
        }

        void simpleButton_imprime_cuadre_resumido_Click(object sender, EventArgs e)
        {
            //
            if (recaudacion_det.Count > 0)
            {
                //string mypath_application = System.Windows.Forms.Application.StartupPath;
                //string mypath_reports_detalle_recaudacion = mypath_application + @"\reports\";
                string myfile_reports_detalle_recaudacion = Fundraising_PT.Properties.Settings.Default.mypath_reports + @"xtrareport_detalle_recaudacion_base.repx";
                //
                if (File.Exists(myfile_reports_detalle_recaudacion))
                {
                    XtraReport xtraReport_det = XtraReport.FromFile(myfile_reports_detalle_recaudacion, true);
                    xtraReport_det.DataSource = data_report();
                    xtraReport_det.Bands["GroupHeader_TipoPago"].PageBreak = PageBreak.None;
                    xtraReport_det.Bands["GroupHeader_CodFormaPago"].Visible = false;
                    xtraReport_det.Bands["GroupHeader_efectivo"].FormattingRules.Clear();
                    xtraReport_det.Bands["GroupHeader_tarjetas_debito_credito"].FormattingRules.Clear();
                    xtraReport_det.Bands["GroupHeader_cheques"].FormattingRules.Clear();
                    xtraReport_det.Bands["GroupHeader_creditos"].FormattingRules.Clear();
                    xtraReport_det.Bands["GroupHeader_depositos"].FormattingRules.Clear();
                    xtraReport_det.Bands["detailBand_DetalleGeneral"].Visible = false;
                    xtraReport_det.Bands["ReportFooter"].Visible = true;
                    xtraReport_det.ShowRibbonPreviewDialog();
                }
                else
                {
                    MessageBox.Show(string.Format("NO se encuentra el archivo de reporte: {0}...",myfile_reports_detalle_recaudacion), "Cuadre Resumido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OpenFileDialog form_open_file = new OpenFileDialog();
                    form_open_file.CheckFileExists = true;
                    form_open_file.CheckPathExists = true;
                    form_open_file.InitialDirectory = Fundraising_PT.Properties.Settings.Default.mypath_sistema;
                    form_open_file.RestoreDirectory = true;
                    form_open_file.ShowDialog();
                    myfile_reports_detalle_recaudacion = form_open_file.FileName;
                    //
                    if (string.IsNullOrEmpty(myfile_reports_detalle_recaudacion) != true)
                    {
                        XtraReport xtraReport_det = XtraReport.FromFile(myfile_reports_detalle_recaudacion, true);
                        if (xtraReport_det != null)
                        {
                            xtraReport_det.DataSource = data_report();
                            xtraReport_det.Bands["GroupHeader_TipoPago"].PageBreak = PageBreak.None;
                            xtraReport_det.Bands["GroupHeader_CodFormaPago"].Visible = false;
                            xtraReport_det.Bands["GroupHeader_efectivo"].FormattingRules.Clear();
                            xtraReport_det.Bands["GroupHeader_tarjetas_debito_credito"].FormattingRules.Clear();
                            xtraReport_det.Bands["GroupHeader_cheques"].FormattingRules.Clear();
                            xtraReport_det.Bands["GroupHeader_creditos"].FormattingRules.Clear();
                            xtraReport_det.Bands["GroupHeader_depositos"].FormattingRules.Clear();
                            xtraReport_det.Bands["detailBand_DetalleGeneral"].Visible = false;
                            xtraReport_det.Bands["ReportFooter"].Visible = true;
                            xtraReport_det.ShowRibbonPreviewDialog();
                        }
                        else
                        {
                            MessageBox.Show("Archivo de reporte NO valido...", "Cuadre Resumido", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("NO hay registros para visualizar...", "Cuadre Resumido", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //
        }

        void simpleButton_imprime_cuadre_detallado_Click(object sender, EventArgs e)
        {
            //
            if (recaudacion_det.Count > 0)
            {
                //string mypath_application = System.Windows.Forms.Application.StartupPath;
                //string mypath_reports_detalle_recaudacion = mypath_application + @"\reports\";
                string myfile_reports_detalle_recaudacion = Fundraising_PT.Properties.Settings.Default.mypath_reports + @"xtrareport_detalle_recaudacion_base.repx";
                //
                if (File.Exists(myfile_reports_detalle_recaudacion))
                {
                    XtraReport xtraReport_det = XtraReport.FromFile(myfile_reports_detalle_recaudacion, true);
                    xtraReport_det.DataSource = data_report();
                    xtraReport_det.ShowRibbonPreviewDialog();
                }
                else
                {
                    MessageBox.Show(string.Format("NO se encuentra el archivo de reporte: {0}...", myfile_reports_detalle_recaudacion), "Cuadre Detallado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OpenFileDialog form_open_file = new OpenFileDialog();
                    form_open_file.CheckFileExists = true;
                    form_open_file.CheckPathExists = true;
                    form_open_file.InitialDirectory = Fundraising_PT.Properties.Settings.Default.mypath_sistema;
                    form_open_file.RestoreDirectory = true;
                    form_open_file.ShowDialog();
                    myfile_reports_detalle_recaudacion = form_open_file.FileName;
                    //
                    if (string.IsNullOrEmpty(myfile_reports_detalle_recaudacion) != true)
                    {
                        XtraReport xtraReport_det = XtraReport.FromFile(myfile_reports_detalle_recaudacion, true);
                        if (xtraReport_det != null)
                        {
                            xtraReport_det.DataSource = data_report();
                            xtraReport_det.ShowRibbonPreviewDialog();
                        }
                        else
                        {
                            MessageBox.Show("Archivo de reporte NO valido...", "Cuadre Detallado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("NO hay registros para visualizar...", "Cuadre Detallado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //
        }

        void simpleButton_salir_Click(object sender, EventArgs e)
        {
            if (ln_totalgeneralrecaudacion_ini != ln_totalgeneralrecaudacion)
            {
                if (MessageBox.Show("Esta seguro de Salir de la Recaudación. ?", "Salir", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                {
                    if (Modo_recaudacion == 1)
                    {
                        if (((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).bindingSource1.Current).status == 5)
                        {
                            ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).bindingSource1.Current).status = 1;
                            ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).bindingSource1.Current).Save();
                        }
                    }
                    this.WindowState = FormWindowState.Normal;
                    this.Close();
                }
            }
            else
            {
                if (Modo_recaudacion == 1)
                {
                    if (((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).bindingSource1.Current).status == 5)
                    {
                        ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).bindingSource1.Current).status = 1;
                        ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).bindingSource1.Current).Save();
                    }
                }
                this.WindowState = FormWindowState.Normal;
                this.Close(); 
            }
            
        }

        void simpleButton_eliminar_registros(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.SimpleButton)sender).Name.Trim() == "simpleButton_eliminar_tarjetas")
            {
                decimal ln_total_tarjeta_debito_aux = 0;
                decimal ln_total_tarjeta_credito_aux = 0;
                decimal ln_total_tarjeta_alimentacion_aux = 0;
                int index = this.gridView_tarjetas.GetFocusedDataSourceRowIndex();
                //
                switch (this.radioGroup_tarjertas_tipo_tarjeta.SelectedIndex)
                {
                    case 1:
                        if (tarjeta_credito_aux.Rows.Count > 0)
                        {
                            tarjeta_credito_aux.Rows[index].Delete();
                            this.gridView_tarjetas.RefreshData();
                            //
                            foreach (DataRow rowtc in tarjeta_credito_aux.Rows)
                            {
                                ln_total_tarjeta_credito_aux = ln_total_tarjeta_credito_aux + Decimal.Parse(rowtc[4].ToString());
                            }
                            fp_tarjeta_credito_current_row["monto_recaudado"] = ln_total_tarjeta_credito_aux;
                            this.carga_totales(3);
                        }
                        break;
                    case 2:
                        if (tarjeta_alimentacion_aux.Rows.Count > 0)
                        {
                            tarjeta_alimentacion_aux.Rows[index].Delete();
                            this.gridView_tarjetas.RefreshData();
                            //
                            foreach (DataRow rowta in tarjeta_alimentacion_aux.Rows)
                            {
                                ln_total_tarjeta_alimentacion_aux = ln_total_tarjeta_alimentacion_aux + Decimal.Parse(rowta[4].ToString());
                            }
                            fp_tarjeta_alimentacion_current_row["monto_recaudado"] = ln_total_tarjeta_alimentacion_aux;
                            this.carga_totales(4);
                        }
                        break;
                    default:
                        if (tarjeta_debito_aux.Rows.Count > 0)
                        {
                            tarjeta_debito_aux.Rows[index].Delete();
                            this.gridView_tarjetas.RefreshData();
                            //
                            foreach (DataRow rowtd in tarjeta_debito_aux.Rows)
                            {
                                ln_total_tarjeta_debito_aux = ln_total_tarjeta_debito_aux + Decimal.Parse(rowtd[4].ToString());
                            }
                            fp_tarjeta_debito_current_row["monto_recaudado"] = ln_total_tarjeta_debito_aux;
                            this.carga_totales(2);
                        }
                        break;
                }
                suma_totales();
            }

            if (((DevExpress.XtraEditors.SimpleButton)sender).Name.Trim() == "simpleButton_eliminar_cheques")
            {
                if (cheque_aux.Rows.Count > 0)
                {
                    int index = this.gridView_cheques.GetFocusedDataSourceRowIndex();
                    cheque_aux.Rows[index].Delete();
                    this.gridView_cheques.RefreshData();
                    //
                    decimal ln_total_cheques_aux = 0;
                    foreach (DataRow rowch in cheque_aux.Rows)
                    {
                        ln_total_cheques_aux = ln_total_cheques_aux + Decimal.Parse(rowch[4].ToString());
                    }
                    fp_cheque_current_row["monto_recaudado"] = ln_total_cheques_aux;
                    this.carga_totales(5);
                    suma_totales();
                }
            }

            if (((DevExpress.XtraEditors.SimpleButton)sender).Name.Trim() == "simpleButton_eliminar_creditos")
            {
                if (credito_aux.Rows.Count > 0)
                {
                    int index = this.gridView_creditos.GetFocusedDataSourceRowIndex();
                    credito_aux.Rows[index].Delete();
                    this.gridView_creditos.RefreshData();
                    //
                    decimal ln_total_creditos_aux = 0;
                    foreach (DataRow rowcr in credito_aux.Rows)
                    {
                        ln_total_creditos_aux = ln_total_creditos_aux + Decimal.Parse(rowcr[3].ToString());
                    }
                    fp_credito_current_row["monto_recaudado"] = ln_total_creditos_aux;
                    this.carga_totales(6);
                    suma_totales();
                }
            }

            if (((DevExpress.XtraEditors.SimpleButton)sender).Name.Trim() == "simpleButton_eliminar_tickets")
            {
                if (ticket_aux.Rows.Count > 0)
                {
                    int index = this.gridView_tickets.GetFocusedDataSourceRowIndex();
                    ticket_aux.Rows[index].Delete();
                    this.gridView_tickets.RefreshData();
                    //
                    decimal ln_total_ticketalimentacion_aux = 0;
                    foreach (DataRow rowt in ticket_aux.Rows)
                    {
                        ln_total_ticketalimentacion_aux = ln_total_ticketalimentacion_aux + (Decimal.Parse(rowt[1].ToString()) * int.Parse(rowt[2].ToString()));
                    }
                    fp_ticketalimentacion_current_row["monto_recaudado"] = ln_total_ticketalimentacion_aux;
                    this.carga_totales(9);
                    suma_totales();
                }
            }

            if (((DevExpress.XtraEditors.SimpleButton)sender).Name.Trim() == "simpleButton_eliminar_depositos")
            {
                if (deposito_aux.Rows.Count > 0)
                {
                    int index = this.gridView_depositos.GetFocusedDataSourceRowIndex();
                    deposito_aux.Rows[index].Delete();
                    this.gridView_depositos.RefreshData();
                    //
                    decimal ln_total_depositos_aux = 0;
                    foreach (DataRow rowdep in deposito_aux.Rows)
                    {
                        ln_total_depositos_aux = ln_total_depositos_aux + Decimal.Parse(rowdep[4].ToString());
                    }
                    fp_deposito_current_row["monto_recaudado"] = ln_total_depositos_aux;
                    this.carga_totales(12);
                    suma_totales();
                }
            }
        }

        void simpleButton_imprimir_depositos_Click(object sender, EventArgs e)
        {
            this.gridControl_depositos.ShowRibbonPrintPreview();
        }

        void simpleButton_imprimir_tickets_Click(object sender, EventArgs e)
        {
            this.gridControl_tickets.ShowRibbonPrintPreview();
        }

        void simpleButton_imprimir_cheques_Click(object sender, EventArgs e)
        {
            this.gridControl_cheques.ShowRibbonPrintPreview();
        }

        void simpleButton_imprimir_creditos_Click(object sender, EventArgs e)
        {
            this.gridControl_creditos.ShowRibbonPrintPreview();
        }

        void simpleButton_imprimir_tarjetas_Click(object sender, EventArgs e)
        {
            this.gridControl_tarjetas.ShowRibbonPrintPreview();
        }

        void simpleButton_imprimir_monedas_Click(object sender, EventArgs e)
        {
            this.gridControl_efectivo_monedas.ShowRibbonPrintPreview();
        }

        void simpleButton_imprimir_billetes_Click(object sender, EventArgs e)
        {
            this.gridControl_efectivo_billetes.ShowRibbonPrintPreview();
        }

        void simpleButton_totales_abre_recaudacion_cerrada_Click(object sender, EventArgs e)
        {
            current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(lg_sesion);
            if (current_sesion != null)
            {
                if (current_sesion.status == 4 | current_sesion.status == 5)
                {
                    MessageBox.Show("NO se puede hacer ajustes a la recaudación." + Environment.NewLine +"La sesión asociada a la recaudación se encuentra en Estatus: (En Proceso o Cerrada).", "Ajustar Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    try
                    {
                        ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).Reload();

                        if (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).status != 4 & ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).status != 6)
                        {
                            //current_sesion = (Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).sesion;
                            current_sesion.status = 5;
                            current_sesion.Save();
                            //
                            ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).status = 6;
                            ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).Save();
                            //
                            gridView_efectivo_billetes.OptionsBehavior.ReadOnly = false;
                            gridView_efectivo_monedas.OptionsBehavior.ReadOnly = false;
                            gridView_tarjetas.OptionsBehavior.ReadOnly = false;
                            gridView_cheques.OptionsBehavior.ReadOnly = false;
                            gridView_creditos.OptionsBehavior.ReadOnly = false;
                            gridView_tickets.OptionsBehavior.ReadOnly = false;
                            gridView_depositos.OptionsBehavior.ReadOnly = false;
                            //
                            simpleButton_eliminar_tarjetas.Enabled = true;
                            simpleButton_eliminar_cheques1.Enabled = true;
                            simpleButton_eliminar_creditos.Enabled = true;
                            simpleButton_eliminar_tickets.Enabled = true;
                            simpleButton_eliminar_depositos.Enabled = true;
                            //
                            textBox_efectivo_ref1.Enabled = true;
                            textBox_efectivo_ref2.Enabled = true;
                            textBox_ticket_ref1.Enabled = true;
                            textBox_ticket_ref2.Enabled = true;
                            //
                            dateEdit_fecha_hora_recaudacion.Enabled = true;
                            //
                            simpleButton_totales_abre_recaudacion_cerrada.Enabled = false;
                            simpleButton_totales_guardar.Enabled = true;
                            simpleButton_salir.Enabled = true;
                            //
                            label_estatus_recaudacion.Text = "En_Proceso";
                            label_estatus_recaudacion.Appearance.ForeColor = Color.LightCyan;
                            label_estatus_recaudacion.LineColor = Color.LightCyan;
                        }
                        else
                        {
                            MessageBox.Show("NO se puede hacer ajustes a la recaudación." + Environment.NewLine + "La recaudación seleccionada se encuentra en Estatus: (En Proceso o Cerrada).", "Ajustar Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("No se pudo actualizar los datos desde el servidor para el registro seleccionado...", "Ajustar Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        //void simpleButton_visualizar_reportes_Click(object sender, EventArgs e)
        //{
        //    //CriteriaOperator filtro_recaudacion_det = (new OperandProperty("recaudacion.oid") == new OperandValue(lg_recaudacion));
        //    //recaudacion_det.Criteria = filtro_recaudacion_det;
        //    //recaudacion_det.Reload();
        //    //
        //    if (recaudacion_det.Count > 0)
        //    {
        //        OpenFileDialog form_open_file = new OpenFileDialog();
        //        form_open_file.CheckFileExists = true;
        //        form_open_file.CheckPathExists = true;

        //        string mypath_application = System.Windows.Forms.Application.StartupPath;
        //        string mypath_reports_detalle_recaudacion = mypath_application + @"\reports\detalle_recaudacion\";
        //        string myfile_reports_detalle_recaudacion = mypath_reports_detalle_recaudacion + @"xtrareport_detalle_recaudacion_base.repx";

        //        //form_open_file.InitialDirectory = mypath_reports_detalle_recaudacion;
        //        //form_open_file.RestoreDirectory = true;
        //        //form_open_file.DefaultExt = "REPX";
        //        //form_open_file.ShowDialog();
        //        //string filenane_report = form_open_file.FileName;
        //        //
        //        //if (string.IsNullOrEmpty(filenane_report) != true)
        //        if (string.IsNullOrEmpty(myfile_reports_detalle_recaudacion) != true)
        //        {
        //            //XtraReport xtraReport_det = XtraReport.FromFile(filenane_report, true);
        //            XtraReport xtraReport_det = XtraReport.FromFile(myfile_reports_detalle_recaudacion, true);
        //            xtraReport_det.DataSource = data_report();
        //            //
        //            Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("Resumido", "Detallado", "Cancelar");
        //            switch (MessageBox.Show("Seleccione el tipo de Reporte a Visualizar", "Visualizar Reporte", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
        //            {
        //                case DialogResult.Yes:
        //                    xtraReport_det.Bands["GroupHeader_TipoPago"].PageBreak = PageBreak.None;
        //                    xtraReport_det.Bands["GroupHeader_CodFormaPago"].Visible = false;
        //                    xtraReport_det.Bands["GroupHeader_efectivo"].FormattingRules.Clear();
        //                    xtraReport_det.Bands["GroupHeader_tarjetas_debito_credito"].FormattingRules.Clear();
        //                    xtraReport_det.Bands["GroupHeader_cheques"].FormattingRules.Clear();
        //                    xtraReport_det.Bands["GroupHeader_creditos"].FormattingRules.Clear();
        //                    xtraReport_det.Bands["GroupHeader_depositos"].FormattingRules.Clear();
        //                    xtraReport_det.Bands["detailBand_DetalleGeneral"].Visible = false;
        //                    xtraReport_det.Bands["ReportFooter"].Visible = true;
        //                    xtraReport_det.ShowRibbonPreviewDialog();
        //                    break;
        //                case DialogResult.No:
        //                    xtraReport_det.ShowRibbonPreviewDialog();
        //                    break;
        //                default:
        //                    break;
        //            }
        //            Fundraising_PT.Clases.MsgBoxUtil.UnHackMessageBox();
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("NO hay registros para visualizar reportes...", "Visualizar Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}

        //void simpleButton_report_designer_Click(object sender, EventArgs e)
        //{
        //    if (recaudacion_det.Count > 0)
        //    {
        //        // crea el formulario tipo report designer para utilizar el diseñador de reportes
        //        Clases.UI_Report_Designer form_designer_report_det = new Clases.UI_Report_Designer();
        //        XtraReport xtraReport_det = new XtraReport();
        //        xtraReport_det.DataSource = data_report();
        //        form_designer_report_det.reportDesigner1.OpenReport(xtraReport_det);
        //        form_designer_report_det.Show();
        //    }
        //    else
        //    {
        //        MessageBox.Show("NO hay registros para crear reportes...", "Crear Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}

        //void simpleButton_report_designer_edit_Click(object sender, EventArgs e)
        //{
        //    if (recaudacion_det.Count > 0)
        //    {
        //        OpenFileDialog form_open_file = new OpenFileDialog();
        //        form_open_file.DefaultExt = "REPX";
        //        form_open_file.ShowDialog();
        //        string filenane_report = form_open_file.FileName;
        //        //
        //        Clases.UI_Report_Designer form_designer_report_det = new Clases.UI_Report_Designer();
        //        XtraReport xtraReport_det = XtraReport.FromFile(filenane_report, true);
        //        xtraReport_det.DataSource = data_report();
        //        form_designer_report_det.reportDesigner1.OpenReport(xtraReport_det);
        //        form_designer_report_det.Show();
        //    }
        //    else
        //    {
        //        MessageBox.Show("NO hay registros para editar reportes...", "Editar Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}

        void gridView_tarjetas_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            // se comenta para dejarlo de ejemplo //
            //int can = this.gridView_tarjetas.GetDataRow(e.RowHandle).ItemArray.Count();
            //for (int i = 0; i < can; i++)
            //{
            //    MessageBox.Show(this.gridView_tarjetas.GetDataRow(e.RowHandle).ItemArray[i].ToString().Trim());               
            //}
            //
            int nerror = 0;
            this.gridView_tarjetas.GetDataRow(e.RowHandle).ClearErrors();
            // VALIDA LA COLUMNA DE PUNTO BANCARIO //
            if (this.gridView_tarjetas.GetDataRow(e.RowHandle).ItemArray[1].ToString().Trim() == string.Empty)
            {
                this.gridView_tarjetas.GetDataRow(e.RowHandle).SetColumnError(1, "Punto Bancario NO puede estar vacio o nulo...");
                nerror = nerror + 1;
            }

            // VALIDA LA COLUMNA DE LOTE //
            if (this.gridView_tarjetas.GetDataRow(e.RowHandle).ItemArray[2].ToString().Trim() == string.Empty)
            {
                this.gridView_tarjetas.GetDataRow(e.RowHandle).SetColumnError(2, "Lote NO puede estar vacio o nulo...");
                nerror = nerror + 1;
            }

            // VALIDA LA COLUMNA DE MONTO //
            if (this.gridView_tarjetas.GetDataRow(e.RowHandle).ItemArray[4].ToString().Trim() == string.Empty || decimal.Parse(this.gridView_tarjetas.GetDataRow(e.RowHandle).ItemArray[4].ToString().Trim()) <= 0 || decimal.Parse(this.gridView_tarjetas.GetDataRow(e.RowHandle).ItemArray[4].ToString().Trim()) > 99999999999999)
            {
                this.gridView_tarjetas.GetDataRow(e.RowHandle).SetColumnError(4, "Monto NO puede ser menor igual a cero (0) o mayor a (99,999,999,999,999.00)...");
                nerror = nerror + 1;
            }

            if (nerror>0)
            {
                e.Valid = false;
            }
            else
            {
                this.gridView_tarjetas.GetDataRow(e.RowHandle).ClearErrors();
                e.Valid = true;
            }
        }

        void gridView_tarjetas_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            decimal ln_total_tarjeta_debito_aux = 0;
            decimal ln_total_tarjeta_credito_aux = 0;
            decimal ln_total_tarjeta_alimentacion_aux = 0;
            //
            switch (this.radioGroup_tarjertas_tipo_tarjeta.SelectedIndex)
            {
                case 1:
                    foreach (DataRow rowtc in tarjeta_credito_aux.Rows)
                    {
                        ln_total_tarjeta_credito_aux = ln_total_tarjeta_credito_aux + Decimal.Parse(rowtc[4].ToString());
                    }
                    fp_tarjeta_credito_current_row["monto_recaudado"] = ln_total_tarjeta_credito_aux;
                    this.carga_totales(3);
                    break;
                case 2:
                    foreach (DataRow rowta in tarjeta_alimentacion_aux.Rows)
                    {
                        ln_total_tarjeta_alimentacion_aux = ln_total_tarjeta_alimentacion_aux + Decimal.Parse(rowta[4].ToString());
                    }
                    fp_tarjeta_alimentacion_current_row["monto_recaudado"] = ln_total_tarjeta_alimentacion_aux;
                    this.carga_totales(4);
                    break;
                default:
                    foreach (DataRow rowtd in tarjeta_debito_aux.Rows)
                    {
                        ln_total_tarjeta_debito_aux = ln_total_tarjeta_debito_aux + Decimal.Parse(rowtd[4].ToString());
                    }
                    fp_tarjeta_debito_current_row["monto_recaudado"] = ln_total_tarjeta_debito_aux;
                    this.carga_totales(2);
                    break;
            }
            suma_totales();
        }

        void gridView_depositos_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            int nerror = 0;
            this.gridView_depositos.GetDataRow(e.RowHandle).ClearErrors();
            // VALIDA LA COLUMNA DE LA CUENTA //
            if (this.gridView_depositos.GetDataRow(e.RowHandle).ItemArray[1].ToString().Trim() == string.Empty)
            {
                this.gridView_depositos.GetDataRow(e.RowHandle).SetColumnError(1, "Cuenta NO puede estar vacio o nulo...");
                nerror = nerror + 1;
            }

            // VALIDA LA COLUMNA DE REFERENCIA 1 //
            if (this.gridView_depositos.GetDataRow(e.RowHandle).ItemArray[2].ToString().Trim() == string.Empty)
            {
                this.gridView_depositos.GetDataRow(e.RowHandle).SetColumnError(2, "Referencia 1 NO puede estar vacio o nulo...");
                nerror = nerror + 1;
            }

            // VALIDA LA COLUMNA DE MONTO //
            if (this.gridView_depositos.GetDataRow(e.RowHandle).ItemArray[4].ToString().Trim() == string.Empty || decimal.Parse(this.gridView_depositos.GetDataRow(e.RowHandle).ItemArray[4].ToString().Trim()) <= 0 || decimal.Parse(this.gridView_depositos.GetDataRow(e.RowHandle).ItemArray[4].ToString().Trim()) > 99999999999999)
            {
                this.gridView_depositos.GetDataRow(e.RowHandle).SetColumnError(4, "Monto NO puede ser menor igual a cero (0) o mayor a (99,999,999,999,999.00)...");
                nerror = nerror + 1;
            }

            if (nerror > 0)
            {
                e.Valid = false;
            }
            else
            {
                this.gridView_depositos.GetDataRow(e.RowHandle).ClearErrors();
                e.Valid = true;
            }
        }

        void gridView_depositos_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            decimal ln_total_depositos_aux = 0;
            foreach (DataRow rowdep in deposito_aux.Rows)
            {
                ln_total_depositos_aux = ln_total_depositos_aux + Decimal.Parse(rowdep[4].ToString());
            }
            fp_deposito_current_row["monto_recaudado"] = ln_total_depositos_aux;
            this.carga_totales(12);
            suma_totales();
        }

        void gridView_tickets_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            int nerror = 0;
            this.gridView_tickets.GetDataRow(e.RowHandle).ClearErrors();
            // VALIDA LA COLUMNA DENOMINACION //
            if (this.gridView_tickets.GetDataRow(e.RowHandle).ItemArray[1].ToString().Trim() == string.Empty || decimal.Parse(this.gridView_tickets.GetDataRow(e.RowHandle).ItemArray[1].ToString().Trim()) <= 0)
            {
                this.gridView_tickets.GetDataRow(e.RowHandle).SetColumnError(1, "Denominacion NO puede ser menor igual a cero (0)...");
                nerror = nerror + 1;
            }

            // VALIDA LA COLUMNA DE CANTIDAD //
            if (this.gridView_tickets.GetDataRow(e.RowHandle).ItemArray[2].ToString().Trim() == string.Empty || decimal.Parse(this.gridView_tickets.GetDataRow(e.RowHandle).ItemArray[2].ToString().Trim()) <= 0)
            {
                this.gridView_tickets.GetDataRow(e.RowHandle).SetColumnError(2, "Cantidad NO puede ser menor igual a cero (0)...");
                nerror = nerror + 1;
            }

            // VALIDA LA COLUMNA DE (DENOMINACION * CANTIDAD) //
            if (decimal.Parse(this.gridView_tickets.GetDataRow(e.RowHandle).ItemArray[1].ToString().Trim()) * decimal.Parse(this.gridView_tickets.GetDataRow(e.RowHandle).ItemArray[2].ToString().Trim()) > 99999999999999)
            {
                this.gridView_tickets.GetDataRow(e.RowHandle).SetColumnError(1, "(Denominacion * Cantidad) NO puede ser mayor a (99,999,999,999,999.00)...");
                nerror = nerror + 1;
            }
            
            if (nerror > 0)
            {
                e.Valid = false;
            }
            else
            {
                this.gridView_tickets.GetDataRow(e.RowHandle).ClearErrors();
                e.Valid = true;
            }
        }

        void gridView_tickets_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            decimal ln_total_ticketalimentacion_aux = 0;
            foreach (DataRow rowt in ticket_aux.Rows)
            {
                ln_total_ticketalimentacion_aux = ln_total_ticketalimentacion_aux + (Decimal.Parse(rowt[1].ToString()) * int.Parse(rowt[2].ToString()));
            }
            fp_ticketalimentacion_current_row["monto_recaudado"] = ln_total_ticketalimentacion_aux;
            this.carga_totales(9);
            suma_totales();
        }

        void gridView_creditos_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            int nerror = 0;
            this.gridView_creditos.GetDataRow(e.RowHandle).ClearErrors();
            // VALIDA LA COLUMNA No. FACTURA //
            if (this.gridView_creditos.GetDataRow(e.RowHandle).ItemArray[1].ToString().Trim() == string.Empty)
            {
                this.gridView_creditos.GetDataRow(e.RowHandle).SetColumnError(1, "No. Factura NO puede estar vacio o nulo...");
                nerror = nerror + 1;
            }

            // VALIDA LA COLUMNA DE MONTO //
            if (this.gridView_creditos.GetDataRow(e.RowHandle).ItemArray[3].ToString().Trim() == string.Empty || decimal.Parse(this.gridView_creditos.GetDataRow(e.RowHandle).ItemArray[3].ToString().Trim()) <= 0 || decimal.Parse(this.gridView_creditos.GetDataRow(e.RowHandle).ItemArray[3].ToString().Trim()) > 99999999999999)
            {
                this.gridView_creditos.GetDataRow(e.RowHandle).SetColumnError(3, "Monto NO puede ser menor igual a cero (0) o mayor a (99,999,999,999,999.00)...");
                nerror = nerror + 1;
            }

            if (nerror > 0)
            {
                e.Valid = false;
            }
            else
            {
                this.gridView_creditos.GetDataRow(e.RowHandle).ClearErrors();
                e.Valid = true;
            }
        }

        void gridView_creditos_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            decimal ln_total_creditos_aux = 0;
            foreach (DataRow rowcr in credito_aux.Rows)
            {
                ln_total_creditos_aux = ln_total_creditos_aux + Decimal.Parse(rowcr[3].ToString());
            }
            fp_credito_current_row["monto_recaudado"] = ln_total_creditos_aux;
            this.carga_totales(6);
            suma_totales();
        }

        void gridView_cheques_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            int nerror = 0;
            this.gridView_cheques.GetDataRow(e.RowHandle).ClearErrors();
            // VALIDA LA COLUMNA DEL BANCO //
            if (this.gridView_cheques.GetDataRow(e.RowHandle).ItemArray[1].ToString().Trim() == string.Empty)
            {
                this.gridView_cheques.GetDataRow(e.RowHandle).SetColumnError(1, "Banco NO puede estar vacio o nulo...");
                nerror = nerror + 1;
            }

            // VALIDA LA COLUMNA DE No. CUENTA //
            if (this.gridView_cheques.GetDataRow(e.RowHandle).ItemArray[2].ToString().Trim() == string.Empty)
            {
                this.gridView_cheques.GetDataRow(e.RowHandle).SetColumnError(2, "No. Cuenta NO puede estar vacio o nulo...");
                nerror = nerror + 1;
            }

            // VALIDA LA COLUMNA DE No. CHEQUE //
            if (this.gridView_cheques.GetDataRow(e.RowHandle).ItemArray[3].ToString().Trim() == string.Empty)
            {
                this.gridView_cheques.GetDataRow(e.RowHandle).SetColumnError(3, "No. Cheque NO puede estar vacio o nulo...");
                nerror = nerror + 1;
            }

            // VALIDA LA COLUMNA DE MONTO //
            if (this.gridView_cheques.GetDataRow(e.RowHandle).ItemArray[4].ToString().Trim() == string.Empty || decimal.Parse(this.gridView_cheques.GetDataRow(e.RowHandle).ItemArray[4].ToString().Trim()) <= 0 || decimal.Parse(this.gridView_cheques.GetDataRow(e.RowHandle).ItemArray[4].ToString().Trim()) > 99999999999999)
            {
                this.gridView_cheques.GetDataRow(e.RowHandle).SetColumnError(4, "Monto NO puede ser menor igual a cero (0) o mayor a (99,999,999,999,999.00)...");
                nerror = nerror + 1;
            }

            if (nerror > 0)
            {
                e.Valid = false;
            }
            else
            {
                this.gridView_cheques.GetDataRow(e.RowHandle).ClearErrors();
                e.Valid = true;
            }
        }

        void gridView_cheques_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            decimal ln_total_cheques_aux = 0;
            foreach (DataRow rowch in cheque_aux.Rows)
            {
                ln_total_cheques_aux = ln_total_cheques_aux + Decimal.Parse(rowch[4].ToString());
            }
            fp_cheque_current_row["monto_recaudado"] = ln_total_cheques_aux;
            this.carga_totales(5);
            suma_totales();
        }

        void checkedComboBoxEdit_efectivo_formaspagos_EditValueChanged(object sender, EventArgs e)
        {
            this.carga_detalle_efectivo(2);
        }

        void checkedComboBoxEdit_tarjetas_formaspagos_EditValueChanged(object sender, EventArgs e)
        {
            this.carga_detalle_tarjetas(2);
        }

        void radioGroup_tarjertas_tipo_tarjeta_SelectedIndexChanged(object sender, EventArgs e)
        {
            ln_ttarjeta = radioGroup_tarjertas_tipo_tarjeta.SelectedIndex+1;
            ln_ttarjeta_ult = radioGroup_tarjertas_tipo_tarjeta.SelectedIndex+1;
            xtraTabPage_tarjeta_Enter(sender, e);
        }

        void checkedComboBoxEdit_cheques_formaspagos_EditValueChanged(object sender, EventArgs e)
        {
            this.carga_detalle_cheques(2);
        }

        void checkedComboBoxEdit_creditos_formaspagos_EditValueChanged(object sender, EventArgs e)
        {
            this.carga_detalle_creditos(2);
        }

        void checkedComboBoxEdit_ticket_formaspagos_EditValueChanged(object sender, EventArgs e)
        {
            this.carga_detalle_ticket(2);
        }

        void checkedComboBoxEdit_depositos_formaspagos_EditValueChanged(object sender, EventArgs e)
        {
            this.carga_detalle_depositos(2);
        }

        void xtraTabControl_detalle_Click(object sender, EventArgs e)
        {
            DevExpress.XtraTab.XtraTabControl tabcontrol_aux = (DevExpress.XtraTab.XtraTabControl)sender;
            switch (tabcontrol_aux.SelectedTabPageIndex)
            {
                case 0:  // FORMAS DE PAGO
                    xtraTabPage_formaspago_Enter(sender, e);
                    break;
                case 1:  // TOTALES RECAUDACION
                    break;
                case 2:  // EFECTIVO
                    xtraTabPage_efectivo_Enter(sender, e);
                    break;
                case 3:  // TARJETAS
                    xtraTabPage_tarjeta_Enter(sender, e);
                    break;
                case 4:  // CHEQUES
                    xtraTabPage_cheque_Enter(sender, e);
                    break;
                case 5:  // CREDITO
                    xtraTabPage_credito_Enter(sender, e);
                    break;
                case 6:  // OTROS PAGOS
                    break;
                case 7:  // PAGOS INTERNOS
                    break;
                case 8:  // TICKET ALIMENTACION
                    xtraTabPage_ticketa_Enter(sender, e);
                    break;
                case 9:  // CONSUMOS INTERNOS
                    break;
                case 10:  // PREPAGO
                    break;
                case 11:  // DEPOSITO
                    xtraTabPage_deposito_Enter(sender, e);
                    break;
                case 12:  // RETENCION IMPUESTO
                    break;
                case 13:  // EXONERACION IMPUESTO
                    break;
                case 14:  // ISRL
                    break;
                case 15:  // SALDO A FAVOR
                    break;
                case 16:  // PUNTOS CLUB LEALTAD
                    break;
            }
        }

        void xtraTabPage_formaspago_Enter(object sender, EventArgs e)
        {
            lg_forma_pago = new Guid();
            lg_proveedor_ta = new Guid();
            ln_tpago = 0;
            ln_ttarjeta = 0;
            this.imageListBoxControl_formas_pago.SelectionMode = SelectionMode.MultiSimple;
            this.imageListBoxControl_formas_pago.UnSelectAll();
            this.imageListBoxControl_formas_pago.SelectionMode = SelectionMode.One;
        }

        void xtraTabPage_efectivo_Enter(object sender, EventArgs e)
        {
            if (lg_forma_pago_efectivo_ult != new Guid())
            {
                lg_forma_pago = lg_forma_pago_efectivo_ult;
            }
            else
            {
                fp_efectivo.DefaultView.Sort = "codigo";
                fp_efectivo = fp_efectivo.DefaultView.ToTable();
                if (fp_efectivo.Rows.Count > 0)
                {
                    lg_forma_pago = (Guid)fp_efectivo.AsEnumerable().FirstOrDefault()["oid"];
                }
                else
                {
                    lg_forma_pago = new Guid();
                }
            }
            checkedComboBoxEdit_efectivo_formaspagos.EditValue = lg_forma_pago;
            checkedComboBoxEdit_efectivo_formaspagos_EditValueChanged(sender, e);
            fp_efectivo.PrimaryKey = new DataColumn[] { fp_efectivo.Columns["oid"] };
        }

        void xtraTabPage_tarjeta_Enter(object sender, EventArgs e)
        {
            if (ln_ttarjeta_ult != 0)
            {
                ln_ttarjeta = ln_ttarjeta_ult;
            }
            
            switch (ln_ttarjeta)
            {
                case 1:
                    radioGroup_tarjertas_tipo_tarjeta.SelectedIndex = 0;
                    //
                    if (lg_forma_pago_tarjetadebito_ult != new Guid())
                    {
                        lg_forma_pago = lg_forma_pago_tarjetadebito_ult;
                    }
                    else
                    {
                        if (fp_tarjeta_debito.Rows.Count > 0)
                        {
                            lg_forma_pago = (Guid)fp_tarjeta_debito.AsEnumerable().FirstOrDefault()["oid"];
                        }
                        else
                        {
                            lg_forma_pago = new Guid();
                        }
                    }
                    //
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.DataSource = fp_tarjeta_debito;
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.DisplayMember = "nombre";
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.ValueMember = "oid";
                    checkedComboBoxEdit_tarjetas_formaspagos.EditValue = lg_forma_pago;
                    break;
                case 2:
                    radioGroup_tarjertas_tipo_tarjeta.SelectedIndex = 1;
                    //
                    if (lg_forma_pago_tarjetacredito_ult != new Guid())
                    {
                        lg_forma_pago = lg_forma_pago_tarjetacredito_ult;
                    }
                    else
                    {
                        if (fp_tarjeta_credito.Rows.Count > 0)
                        {
                            lg_forma_pago = (Guid)fp_tarjeta_credito.AsEnumerable().FirstOrDefault()["oid"];
                        }
                        else
                        {
                            lg_forma_pago = new Guid();
                        }
                    }
                    //
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.DataSource = fp_tarjeta_credito;
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.DisplayMember = "nombre";
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.ValueMember = "oid";
                    checkedComboBoxEdit_tarjetas_formaspagos.EditValue = lg_forma_pago;
                    break;
                case 3:
                    radioGroup_tarjertas_tipo_tarjeta.SelectedIndex = 2;
                    //
                    if (lg_forma_pago_tarjetaalimento_ult != new Guid())
                    {
                        lg_forma_pago = lg_forma_pago_tarjetaalimento_ult;
                    }
                    else
                    {
                        if (fp_tarjeta_alimentacion.Rows.Count > 0)
                        {
                            lg_forma_pago = (Guid)fp_tarjeta_alimentacion.AsEnumerable().FirstOrDefault()["oid"];
                        }
                        else
                        {
                            lg_forma_pago = new Guid();
                        }
                    }
                    //
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.DataSource = fp_tarjeta_alimentacion;
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.DisplayMember = "nombre";
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.ValueMember = "oid";
                    checkedComboBoxEdit_tarjetas_formaspagos.EditValue = lg_forma_pago;
                    break;
                default:
                    radioGroup_tarjertas_tipo_tarjeta.SelectedIndex = 0;
                    //
                    if (lg_forma_pago_tarjetadebito_ult != new Guid())
                    {
                        lg_forma_pago = lg_forma_pago_tarjetadebito_ult;
                    }
                    else
                    {
                        if (fp_tarjeta_debito.Rows.Count > 0)
                        {
                            lg_forma_pago = (Guid)fp_tarjeta_debito.AsEnumerable().FirstOrDefault()["oid"];
                        }
                        else
                        {
                            lg_forma_pago = new Guid();
                        }
                    }
                    //
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.DataSource = fp_tarjeta_debito;
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.DisplayMember = "nombre";
                    checkedComboBoxEdit_tarjetas_formaspagos.Properties.ValueMember = "oid";
                    checkedComboBoxEdit_tarjetas_formaspagos.EditValue = lg_forma_pago;
                    break;
            }
            checkedComboBoxEdit_tarjetas_formaspagos_EditValueChanged(sender, e);
        }

        void xtraTabPage_cheque_Enter(object sender, EventArgs e)
        {
            if (lg_forma_pago_cheque_ult != new Guid())
            {
                lg_forma_pago = lg_forma_pago_cheque_ult;
            }
            else
            {
                if (fp_cheque.Rows.Count > 0)
                {
                    lg_forma_pago = (Guid)fp_cheque.AsEnumerable().FirstOrDefault()["oid"];
                }
                else
                {
                    lg_forma_pago = new Guid();
                }
            }
            checkedComboBoxEdit_cheques_formaspagos.EditValue = lg_forma_pago;
            checkedComboBoxEdit_cheques_formaspagos_EditValueChanged(sender, e);
        }

        void xtraTabPage_credito_Enter(object sender, EventArgs e)
        {
            if (lg_forma_pago_credito_ult != new Guid())
            {
                lg_forma_pago = lg_forma_pago_credito_ult;
            }
            else
            {
                if (fp_credito.Rows.Count > 0)
                {
                    lg_forma_pago = (Guid)fp_credito.AsEnumerable().FirstOrDefault()["oid"];
                }
                else
                {
                    lg_forma_pago = new Guid();
                }
            }
            checkedComboBoxEdit_creditos_formaspagos.EditValue = lg_forma_pago;
            checkedComboBoxEdit_creditos_formaspagos_EditValueChanged(sender, e);
        }

        void xtraTabPage_ticketa_Enter(object sender, EventArgs e)
        {
            if (lg_forma_pago_ticket_ult != new Guid())
            {
                lg_forma_pago = lg_forma_pago_ticket_ult;
            }
            else
            {
                if (fp_ticketalimentacion.Rows.Count > 0)
                {
                    lg_forma_pago = (Guid)fp_ticketalimentacion.AsEnumerable().FirstOrDefault()["oid"];
                }
                else
                {
                    lg_forma_pago = new Guid();
                }
            }
            checkedComboBoxEdit_ticket_formaspagos.EditValue = lg_forma_pago;
            checkedComboBoxEdit_ticket_formaspagos_EditValueChanged(sender, e);
        }

        void xtraTabPage_deposito_Enter(object sender, EventArgs e)
        {
            if (lg_forma_pago_deposito_ult != new Guid())
            {
                lg_forma_pago = lg_forma_pago_deposito_ult;
            }
            else
            {
                if (fp_deposito.Rows.Count > 0)
                {
                    lg_forma_pago = (Guid)fp_deposito.AsEnumerable().FirstOrDefault()["oid"];
                }
                else
                {
                    lg_forma_pago = new Guid();
                }
            }
            checkedComboBoxEdit_depositos_formaspagos.EditValue = lg_forma_pago;
            checkedComboBoxEdit_depositos_formaspagos_EditValueChanged(sender, e);
        }

        void View_CellValueChanged_cantidad_billetes(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            tot_efectivo_billetes = 0;
            foreach (DataRow rowb in billetes_aux.Rows)
            {
                tot_efectivo_billetes = tot_efectivo_billetes + ((Decimal.Parse(rowb[2].ToString()) * Int32.Parse(rowb[3].ToString())));
            }
            this.textBox_efectivo_montototal.textEdit1.EditValue = tot_efectivo_billetes + tot_efectivo_monedas;
            fp_efectivo_current_row["monto_recaudado"] = tot_efectivo_billetes + tot_efectivo_monedas;
            this.carga_totales(1);
            suma_totales();
        }

        void View_CellValueChanged_cantidad_monedas(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            tot_efectivo_monedas = 0;
            foreach (DataRow rowm in monedas_aux.Rows)
            {
                tot_efectivo_monedas = tot_efectivo_monedas + ((Decimal.Parse(rowm[2].ToString()) * Int32.Parse(rowm[3].ToString())));
            }
            this.textBox_efectivo_montototal.textEdit1.EditValue = tot_efectivo_billetes + tot_efectivo_monedas;
            fp_efectivo_current_row["monto_recaudado"] = tot_efectivo_billetes + tot_efectivo_monedas;
            this.carga_totales(1);
            suma_totales();
        }

        void imageListBoxControl_formas_pago_Click(object sender, EventArgs e)
        {
            if (((ImageListBoxControl)sender).SelectedValue != null)
            {
                forma_pago_current_row = (System.Data.DataRowView)((ImageListBoxControl)sender).GetItem(((ImageListBoxControl)sender).SelectedIndex);
                //
                lg_forma_pago = (Guid)(((ImageListBoxControl)sender).SelectedValue);
                int ln_imageindex = ((ImageListBoxControl)sender).GetItemImageIndex(((ImageListBoxControl)sender).SelectedIndex);
                if (forma_pago_current_row != null)
                {
                    lg_proveedor_ta = (Guid)forma_pago_current_row["proveedor_ta"];
                    ln_tpago = (int)forma_pago_current_row["tpago"];
                    ln_ttarjeta = (int)forma_pago_current_row["ttarjeta"];
                }
                else
                {
                    lg_proveedor_ta = new Guid();
                    ln_tpago = 0;
                    ln_ttarjeta = 0;
                }

                switch (ln_imageindex)
                {
                    case 3:
                        ln_imageindex = 2;
                        break;
                    case 4:
                        ln_imageindex = 2;
                        break;
                    default:
                        if (ln_imageindex >= 5)
                        { ln_imageindex = ln_imageindex - 2; }
                        break;
                }
                this.ln_positab = ln_imageindex.ToString();
                foreach (DevExpress.XtraTab.XtraTabPage page_list in this.xtraTabControl_detalle.TabPages)
                {
                    if (page_list.Tag.ToString().Trim() == this.ln_positab.Trim())
                    {
                        page_list.Select();
                        page_list.Focus();
                        page_list.Show();
                    }
                }
            }
            else
            { 
                lg_forma_pago = new Guid();
                lg_proveedor_ta = new Guid();
                ln_tpago = 0;
                ln_ttarjeta = 0;
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            switch (Modo_recaudacion)
            {
                case 1:
                    ((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).HeaderMenu.Caption = lHeader_ant + " - Proceso : Nueva Recaudación  - Acción : " + this.lAccion;
                    break;
                case 2:
                    ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).HeaderMenu.Caption = lHeader_ant + " - Proceso : Consulta Recaudación - Acción : " + this.lAccion;
                    break;
                default:
                    break;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            switch (Modo_recaudacion)
            {
                case 1:
                    ((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).ControlBox = true;
                    ((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).imageListBoxControl_sesiones_activas.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).HeaderMenu.Caption = this.lHeader_ant;
                    ((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).sesiones.Reload();
                    ((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).llena_datatable_sesiones();
                    break;
                case 2:
                    ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).barra_Mant_Base11.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).simpleButton_recaudacion.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).simpleButton_recaudacion2.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).grid_Base11.Enabled = true;
                    ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).ControlBox = true;
                    ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).HeaderMenu.Caption = this.lHeader_ant;
                    break;
                default:
                    break;
            }
        }

        private void llena_datatable_formaspagos()
        {
            // se recorre el collection formas de pago y se llena el datatable
            foreach (Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos forma_pago in formas_pagos)
            {
                int ln_imagen = 0;
                Guid lg_proveedor_ta = new Guid();
                switch (forma_pago.tpago)
                {
                    case 1:
                        ln_imagen = forma_pago.tpago;
                        break;
                    case 2:
                        switch (forma_pago.ttarjeta)
                        {
                            case 1:
                                ln_imagen = forma_pago.tpago;
                                break;
                            case 2:
                                ln_imagen = forma_pago.tpago + 1;
                                break;
                            case 3:
                                ln_imagen = forma_pago.tpago + 2;
                                break;
                            default:
                                ln_imagen = 0;
                                break;
                        }
                        break;
                    default:
                        ln_imagen = forma_pago.tpago + 2;
                        break;
                }

                if (forma_pago.proveedor_ta != null)
                    { lg_proveedor_ta = forma_pago.proveedor_ta.oid; }

                // inserto una nueva fila en el datatable y lo ordeno //
                formas_pagos_aux.Rows.Add(forma_pago.oid, forma_pago.nombre, forma_pago.tpago, forma_pago.ttarjeta, ln_imagen, lg_proveedor_ta);
            }
            formas_pagos_aux.DefaultView.Sort = "imagen";
        }

        private void carga_detalle_efectivo(int ln_modocargadetalle)
        {
            if (ln_modocargadetalle == 1)
            {
                fp_efectivo_current_row = fp_efectivo.Rows[fp_efectivo.Rows.Count - 1];
                lg_forma_pago_efectivo_ult = (Guid)fp_efectivo.Rows[fp_efectivo.Rows.Count - 1]["oid"];
            }
            else
            {
                fp_efectivo_current_row = fp_efectivo.Rows.Find((Guid)this.checkedComboBoxEdit_efectivo_formaspagos.EditValue);
                lg_forma_pago_efectivo_ult = (Guid)this.checkedComboBoxEdit_efectivo_formaspagos.EditValue;
            }
            //            
            recaudacion_det_efectivo_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)fp_efectivo_current_row["colecction_det"], CriteriaOperator.Parse("1 = 1"));
            recaudacion_det_des_efectivo_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>)fp_efectivo_current_row["colecction_des"], CriteriaOperator.Parse("1 = 1"));
            billetes_aux = new DataTable();
            monedas_aux = new DataTable();
            billetes_aux = ((DataTable)fp_efectivo_current_row["datatable_billetes"]);
            monedas_aux = ((DataTable)fp_efectivo_current_row["datatable_monedas"]);
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

            if (billetes_aux.Rows.Count <= 0)
            {
                // carga datos del detalle de efectivo en billetes //
                foreach (var item_billetes in denominacion_monedas_billetes)
                {
                    ln_recaudacion_det_des_cantidad = 0;
                    //
                    CriteriaOperator filtro_recaudacion_det_des_cantidad = (new OperandProperty("denominacion_moneda.oid") == new OperandValue(item_billetes.oid));
                    recaudacion_det_des_cantidad = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>(recaudacion_det_des_efectivo_aux, filtro_recaudacion_det_des_cantidad);
                    //
                    if (recaudacion_det_des_cantidad != null && recaudacion_det_des_cantidad.Count > 0)
                    { ln_recaudacion_det_des_cantidad = recaudacion_det_des_cantidad[0].cantidad; }
                    //
                    billetes_aux.Rows.Add(item_billetes.oid, item_billetes.codigo, item_billetes.valor, ln_recaudacion_det_des_cantidad);
                }
            }
            decimal tot_billetes = billetes_aux.AsEnumerable().Sum((billetes) => billetes.Field<decimal>("valor") * billetes.Field<int>("cantidad"));
            tot_efectivo_billetes = tot_billetes;

            if (monedas_aux.Rows.Count <= 0)
            {
                // carga datos del detalle de efectivo en monedas //
                foreach (var item_monedas in denominacion_monedas_monedas)
                {
                    ln_recaudacion_det_des_cantidad = 0;
                    //
                    CriteriaOperator filtro_recaudacion_det_des_cantidad = (new OperandProperty("denominacion_moneda.oid") == new OperandValue(item_monedas.oid));
                    recaudacion_det_des_cantidad = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>(recaudacion_det_des_efectivo_aux, filtro_recaudacion_det_des_cantidad);
                    //
                    if (recaudacion_det_des_cantidad != null && recaudacion_det_des_cantidad.Count > 0)
                    { ln_recaudacion_det_des_cantidad = recaudacion_det_des_cantidad[0].cantidad; }
                    //
                    monedas_aux.Rows.Add(item_monedas.oid, item_monedas.codigo, item_monedas.valor, ln_recaudacion_det_des_cantidad);
                }
            }
            decimal tot_monedas = monedas_aux.AsEnumerable().Sum((monedas) => monedas.Field<decimal>("valor") * monedas.Field<int>("cantidad"));
            tot_efectivo_monedas = tot_monedas;

            if (ln_modocargadetalle == 2)
            {
                // asigna la coleccion de datos a los grids de billetes y monedas ///
                this.gridControl_efectivo_billetes.DataSource = billetes_aux;
                this.gridControl_efectivo_monedas.DataSource = monedas_aux;
                this.gridControl_efectivo_billetes.RefreshDataSource();
                this.gridControl_efectivo_monedas.RefreshDataSource();

                // calculo del monto total en efectivo //
                this.textBox_efectivo_montototal.textEdit1.EditValue = (tot_billetes + tot_monedas);
                fp_efectivo_current_row["monto_recaudado"] = (tot_billetes + tot_monedas);

                //
                this.textBox_efectivo_ref1.textEdit1.EditValue = ((string)fp_efectivo_current_row["ref1"]);
                this.textBox_efectivo_ref2.textEdit1.EditValue = ((string)fp_efectivo_current_row["ref2"]);
            }
        }

        private void carga_detalle_tarjetas(int ln_modocargadetalle)
        {
            //
            switch (ln_ttarjeta)
            {
                case 1:
                    if (ln_modocargadetalle == 1)
                    {
                        fp_tarjeta_debito_current_row = fp_tarjeta_debito.Rows[fp_tarjeta_debito.Rows.Count - 1];
                        lg_forma_pago_tarjetadebito_ult = (Guid)fp_tarjeta_debito.Rows[fp_tarjeta_debito.Rows.Count - 1]["oid"];
                    }
                    else
                    {
                        fp_tarjeta_debito_current_row = fp_tarjeta_debito.Rows.Find((Guid)this.checkedComboBoxEdit_tarjetas_formaspagos.EditValue);
                        lg_forma_pago_tarjetadebito_ult = (Guid)this.checkedComboBoxEdit_tarjetas_formaspagos.EditValue;
                    }
                    //
                    recaudacion_det_tarjeta_debito_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)fp_tarjeta_debito_current_row["colecction_det"], CriteriaOperator.Parse("1 = 1"));
                    tarjeta_debito_aux = new DataTable();
                    tarjeta_debito_aux = ((DataTable)fp_tarjeta_debito_current_row["datatable_detalle"]);

                    if (tarjeta_debito_aux.Columns.Count <= 0)
                    {
                        // se crean las columnas al datatable del detalle de tarjeta debito
                        tarjeta_debito_aux.Columns.Add("oid", typeof(Guid));
                        tarjeta_debito_aux.Columns.Add("punto_bancario", typeof(Guid));
                        tarjeta_debito_aux.Columns.Add("ref1", typeof(string));
                        tarjeta_debito_aux.Columns.Add("ref2", typeof(string));
                        tarjeta_debito_aux.Columns.Add("monto_recaudado", typeof(decimal));
                    }

                    if (tarjeta_debito_aux.Rows.Count <= 0)
                    {
                        // carga datos del detalle de tarjeta debito //
                        foreach (var item_tarjetas_debito in recaudacion_det_tarjeta_debito_aux)
                        {
                            tarjeta_debito_aux.Rows.Add(item_tarjetas_debito.oid, item_tarjetas_debito.punto_bancario.oid, item_tarjetas_debito.ref1, item_tarjetas_debito.ref2, item_tarjetas_debito.monto_recaudado);
                        }
                    }
                    //
                    if (ln_modocargadetalle == 2)
                    {
                        decimal tot_tarjetas_debito = tarjeta_debito_aux.AsEnumerable().Sum((tarjetasd) => tarjetasd.Field<decimal>("monto_recaudado"));
                        //this.textBox_tarjetas_total_tarjetas1.textEdit1.EditValue = tot_tarjetas_debito;

                        // asigna la coleccion de datos al grid de tarjetas ///
                        this.gridControl_tarjetas.DataSource = tarjeta_debito_aux;
                        this.gridControl_tarjetas.RefreshDataSource();

                        // calculo del monto total en tarjetas debito //
                        fp_tarjeta_debito_current_row["monto_recaudado"] = tot_tarjetas_debito;
                    }
                    break;
                case 2:
                    if (ln_modocargadetalle == 1)
                    {
                        fp_tarjeta_credito_current_row = fp_tarjeta_credito.Rows[fp_tarjeta_credito.Rows.Count - 1];
                        lg_forma_pago_tarjetacredito_ult = (Guid)fp_tarjeta_credito.Rows[fp_tarjeta_credito.Rows.Count - 1]["oid"];
                    }
                    else
                    {
                        fp_tarjeta_credito_current_row = fp_tarjeta_credito.Rows.Find((Guid)this.checkedComboBoxEdit_tarjetas_formaspagos.EditValue);
                        lg_forma_pago_tarjetacredito_ult = (Guid)this.checkedComboBoxEdit_tarjetas_formaspagos.EditValue;
                    }
                    //
                    recaudacion_det_tarjeta_credito_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)fp_tarjeta_credito_current_row["colecction_det"], CriteriaOperator.Parse("1 = 1"));
                    tarjeta_credito_aux = new DataTable();
                    tarjeta_credito_aux = ((DataTable)fp_tarjeta_credito_current_row["datatable_detalle"]);

                    if (tarjeta_credito_aux.Columns.Count <= 0)
                    {
                        // se crean las columnas al datatable del detalle de tarjeta credito
                        tarjeta_credito_aux.Columns.Add("oid", typeof(Guid));
                        tarjeta_credito_aux.Columns.Add("punto_bancario", typeof(Guid));
                        tarjeta_credito_aux.Columns.Add("ref1", typeof(string));
                        tarjeta_credito_aux.Columns.Add("ref2", typeof(string));
                        tarjeta_credito_aux.Columns.Add("monto_recaudado", typeof(decimal));
                    }

                    if (tarjeta_credito_aux.Rows.Count <= 0)
                    {
                        // carga datos del detalle de tarjeta credito //
                        foreach (var item_tarjetas_credito in recaudacion_det_tarjeta_credito_aux)
                        {
                            tarjeta_credito_aux.Rows.Add(item_tarjetas_credito.oid, item_tarjetas_credito.punto_bancario.oid, item_tarjetas_credito.ref1, item_tarjetas_credito.ref2, item_tarjetas_credito.monto_recaudado);
                        }
                    }
                    //
                    if (ln_modocargadetalle == 2)
                    {
                        decimal tot_tarjetas_credito = tarjeta_credito_aux.AsEnumerable().Sum((tarjetasc) => tarjetasc.Field<decimal>("monto_recaudado"));
                        //this.textBox_tarjetas_total_tarjetas1.textEdit1.EditValue = tot_tarjetas_credito;

                        // asigna la coleccion de datos al grid de tarjetas ///
                        this.gridControl_tarjetas.DataSource = tarjeta_credito_aux;
                        this.gridControl_tarjetas.RefreshDataSource();

                        // calculo del monto total en tarjetas credito //
                        fp_tarjeta_credito_current_row["monto_recaudado"] = tot_tarjetas_credito;
                    }
                    break;
                case 3:
                    if (ln_modocargadetalle == 1)
                    {
                        fp_tarjeta_alimentacion_current_row = fp_tarjeta_alimentacion.Rows[fp_tarjeta_alimentacion.Rows.Count - 1];
                        lg_forma_pago_tarjetaalimento_ult = (Guid)fp_tarjeta_alimentacion.Rows[fp_tarjeta_alimentacion.Rows.Count - 1]["oid"];
                    }
                    else
                    {
                        fp_tarjeta_alimentacion_current_row = fp_tarjeta_alimentacion.Rows.Find((Guid)this.checkedComboBoxEdit_tarjetas_formaspagos.EditValue);
                        lg_forma_pago_tarjetaalimento_ult = (Guid)this.checkedComboBoxEdit_tarjetas_formaspagos.EditValue;
                    }
                    //
                    recaudacion_det_tarjeta_alimentacion_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)fp_tarjeta_alimentacion_current_row["colecction_det"], CriteriaOperator.Parse("1 = 1"));
                    tarjeta_alimentacion_aux = new DataTable();
                    tarjeta_alimentacion_aux = ((DataTable)fp_tarjeta_alimentacion_current_row["datatable_detalle"]);

                    if (tarjeta_alimentacion_aux.Columns.Count <= 0)
                    {
                        // se crean las columnas al datatable del detalle de tarjeta alimentacion
                        tarjeta_alimentacion_aux.Columns.Add("oid", typeof(Guid));
                        tarjeta_alimentacion_aux.Columns.Add("punto_bancario", typeof(Guid));
                        tarjeta_alimentacion_aux.Columns.Add("ref1", typeof(string));
                        tarjeta_alimentacion_aux.Columns.Add("ref2", typeof(string));
                        tarjeta_alimentacion_aux.Columns.Add("monto_recaudado", typeof(decimal));
                    }

                    if (tarjeta_alimentacion_aux.Rows.Count <= 0)
                    {
                        // carga datos del detalle de tarjeta alimentacion //
                        foreach (var item_tarjetas_alimentacion in recaudacion_det_tarjeta_alimentacion_aux)
                        {
                            tarjeta_alimentacion_aux.Rows.Add(item_tarjetas_alimentacion.oid, item_tarjetas_alimentacion.punto_bancario.oid, item_tarjetas_alimentacion.ref1, item_tarjetas_alimentacion.ref2, item_tarjetas_alimentacion.monto_recaudado);
                        }
                    }
                    if (ln_modocargadetalle == 2)
                    {
                        decimal tot_tarjetas_alimentacion = tarjeta_alimentacion_aux.AsEnumerable().Sum((tarjetasa) => tarjetasa.Field<decimal>("monto_recaudado"));
                        //this.textBox_tarjetas_total_tarjetas1.textEdit1.EditValue = tot_tarjetas_alimentacion;

                        // asigna la coleccion de datos al grid de tarjetas ///
                        this.gridControl_tarjetas.DataSource = tarjeta_alimentacion_aux;
                        this.gridControl_tarjetas.RefreshDataSource();

                        // calculo del monto total en tarjetas alimentacion //
                        fp_tarjeta_alimentacion_current_row["monto_recaudado"] = tot_tarjetas_alimentacion;
                    }
                    break;
                default:
                    if (ln_modocargadetalle == 1)
                    {
                        fp_tarjeta_debito_current_row = fp_tarjeta_debito.Rows[fp_tarjeta_debito.Rows.Count - 1];
                        lg_forma_pago_tarjetadebito_ult = (Guid)fp_tarjeta_debito.Rows[fp_tarjeta_debito.Rows.Count - 1]["oid"];
                    }
                    else
                    {
                        fp_tarjeta_debito_current_row = fp_tarjeta_debito.Rows.Find((Guid)this.checkedComboBoxEdit_tarjetas_formaspagos.EditValue);
                        lg_forma_pago_tarjetadebito_ult = (Guid)this.checkedComboBoxEdit_tarjetas_formaspagos.EditValue;
                    }
                    //
                    recaudacion_det_tarjeta_debito_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)fp_tarjeta_debito_current_row["colecction_det"], CriteriaOperator.Parse("1 = 1"));
                    tarjeta_debito_aux = new DataTable();
                    tarjeta_debito_aux = ((DataTable)fp_tarjeta_debito_current_row["datatable_detalle"]);

                    if (tarjeta_debito_aux.Columns.Count <= 0)
                    {
                        // se crean las columnas al datatable del detalle de tarjeta debito
                        tarjeta_debito_aux.Columns.Add("oid", typeof(Guid));
                        tarjeta_debito_aux.Columns.Add("punto_bancario", typeof(Guid));
                        tarjeta_debito_aux.Columns.Add("ref1", typeof(string));
                        tarjeta_debito_aux.Columns.Add("ref2", typeof(string));
                        tarjeta_debito_aux.Columns.Add("monto_recaudado", typeof(decimal));
                    }

                    if (tarjeta_debito_aux.Rows.Count <= 0)
                    {
                        // carga datos del detalle de tarjeta debito //
                        foreach (var item_tarjetas_debito in recaudacion_det_tarjeta_debito_aux)
                        {
                            tarjeta_debito_aux.Rows.Add(item_tarjetas_debito.oid, item_tarjetas_debito.punto_bancario.oid, item_tarjetas_debito.ref1, item_tarjetas_debito.ref2, item_tarjetas_debito.monto_recaudado);
                        }
                    }
                    //
                    if (ln_modocargadetalle == 2)
                    {
                        decimal tot_tarjetas_debito = tarjeta_debito_aux.AsEnumerable().Sum((tarjetasd) => tarjetasd.Field<decimal>("monto_recaudado"));
                        //this.textBox_tarjetas_total_tarjetas1.textEdit1.EditValue = tot_tarjetas_debito;

                        // asigna la coleccion de datos al grid de tarjetas ///
                        this.gridControl_tarjetas.DataSource = tarjeta_debito_aux;
                        this.gridControl_tarjetas.RefreshDataSource();

                        // calculo del monto total en tarjetas debito //
                        fp_tarjeta_debito_current_row["monto_recaudado"] = tot_tarjetas_debito;
                    }
                    break;
            }
        }

        private void carga_detalle_cheques(int ln_modocargadetalle)
        {
            if (ln_modocargadetalle == 1)
            {
                fp_cheque_current_row = fp_cheque.Rows[fp_cheque.Rows.Count - 1];
                lg_forma_pago_cheque_ult = (Guid)fp_cheque.Rows[fp_cheque.Rows.Count - 1]["oid"];
            }
            else
            {
                fp_cheque_current_row = fp_cheque.Rows.Find((Guid)this.checkedComboBoxEdit_cheques_formaspagos.EditValue);
                lg_forma_pago_cheque_ult = (Guid)this.checkedComboBoxEdit_cheques_formaspagos.EditValue;
            }
            //
            recaudacion_det_cheque_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)fp_cheque_current_row["colecction_det"], CriteriaOperator.Parse("1 = 1"));
            cheque_aux = new DataTable();
            cheque_aux = ((DataTable)fp_cheque_current_row["datatable_detalle"]);

            if (cheque_aux.Columns.Count <= 0)
            {
                // se crean las columnas al datatable del detalle de cheques
                cheque_aux.Columns.Add("oid", typeof(Guid));
                cheque_aux.Columns.Add("banco", typeof(Guid));
                cheque_aux.Columns.Add("ref1", typeof(string));
                cheque_aux.Columns.Add("ref2", typeof(string));
                cheque_aux.Columns.Add("monto_recaudado", typeof(decimal));
            }

            if (cheque_aux.Rows.Count <= 0)
            {
                // carga datos del detalle de cheques //
                foreach (var item_cheques in recaudacion_det_cheque_aux)
                {
                    cheque_aux.Rows.Add(item_cheques.oid, item_cheques.banco.oid, item_cheques.ref1, item_cheques.ref2, item_cheques.monto_recaudado);
                }
            }
            if (ln_modocargadetalle == 2)
            {
                decimal tot_cheques = cheque_aux.AsEnumerable().Sum((cheqs) => cheqs.Field<decimal>("monto_recaudado"));

                // asigna la coleccion de datos al grid de cheques ///
                this.gridControl_cheques.DataSource = cheque_aux;
                this.gridControl_cheques.RefreshDataSource();

                // calculo del monto total en cheques //
                fp_cheque_current_row["monto_recaudado"] = tot_cheques;
            }
        }

        private void carga_detalle_creditos(int ln_modocargadetalle)
        {
            if (ln_modocargadetalle == 1)
            {
                fp_credito_current_row = fp_credito.Rows[fp_credito.Rows.Count - 1];
                lg_forma_pago_credito_ult = (Guid)fp_credito.Rows[fp_credito.Rows.Count - 1]["oid"];
            }
            else
            {
                fp_credito_current_row = fp_credito.Rows.Find((Guid)this.checkedComboBoxEdit_creditos_formaspagos.EditValue);
                lg_forma_pago_credito_ult = (Guid)this.checkedComboBoxEdit_creditos_formaspagos.EditValue;
            }
            //
            recaudacion_det_credito_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)fp_credito_current_row["colecction_det"], CriteriaOperator.Parse("1 = 1"));
            credito_aux = new DataTable();
            credito_aux = ((DataTable)fp_credito_current_row["datatable_detalle"]);

            if (credito_aux.Columns.Count <= 0)
            {
                // se crean las columnas al datatable del detalle de creditos
                credito_aux.Columns.Add("oid", typeof(Guid));
                credito_aux.Columns.Add("ref1", typeof(string));
                credito_aux.Columns.Add("ref2", typeof(string));
                credito_aux.Columns.Add("monto_recaudado", typeof(decimal));
            }

            if (credito_aux.Rows.Count <= 0)
            {
                // carga datos del detalle de creeditos //
                foreach (var item_creditos in recaudacion_det_credito_aux)
                {
                    credito_aux.Rows.Add(item_creditos.oid, item_creditos.ref1, item_creditos.ref2, item_creditos.monto_recaudado);
                }
            }
            if (ln_modocargadetalle == 2)
            {
                decimal tot_creditos = credito_aux.AsEnumerable().Sum((cred) => cred.Field<decimal>("monto_recaudado"));

                // asigna la coleccion de datos al grid de creditos ///
                this.gridControl_creditos.DataSource = credito_aux;
                this.gridControl_creditos.RefreshDataSource();

                // calculo del monto total en creditos //
                fp_credito_current_row["monto_recaudado"] = tot_creditos;
            }
        }

        private void carga_detalle_ticket(int ln_modocargadetalle)
        {
            if (ln_modocargadetalle == 1)
            {
                fp_ticketalimentacion_current_row = fp_ticketalimentacion.Rows[fp_ticketalimentacion.Rows.Count - 1];
                lg_forma_pago_ticket_ult = (Guid)fp_ticketalimentacion.Rows[fp_ticketalimentacion.Rows.Count - 1]["oid"];
            }
            else
            {
                fp_ticketalimentacion_current_row = fp_ticketalimentacion.Rows.Find((Guid)this.checkedComboBoxEdit_ticket_formaspagos.EditValue);
                lg_forma_pago_ticket_ult = (Guid)this.checkedComboBoxEdit_ticket_formaspagos.EditValue;
            }
            //            
            recaudacion_det_ticketalimentacion_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)fp_ticketalimentacion_current_row["colecction_det"], CriteriaOperator.Parse("1 = 1"));
            recaudacion_det_des_ticket_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>)fp_ticketalimentacion_current_row["colecction_des"], CriteriaOperator.Parse("1 = 1"));
            ticket_aux = new DataTable();
            ticket_aux = ((DataTable)fp_ticketalimentacion_current_row["datatable_desgloce"]);

            if (ticket_aux.Columns.Count <= 0)
            {
                // se crean las columnas al datatable del detalle de ticket alimentacion
                ticket_aux.Columns.Add("oid", typeof(Guid));
                ticket_aux.Columns.Add("valor", typeof(decimal));
                ticket_aux.Columns.Add("cantidad", typeof(int));
            }

            if (ticket_aux.Rows.Count <= 0)
            {
                // carga datos del detalle de ticket alimentacion //
                foreach (var item_tickets in recaudacion_det_des_ticket_aux)
                {
                    ticket_aux.Rows.Add(item_tickets.oid, item_tickets.denominacion, item_tickets.cantidad);
                }
            }
            if (ln_modocargadetalle == 2)
            {
                decimal tot_tickets = ticket_aux.AsEnumerable().Sum((tickets) => tickets.Field<decimal>("valor") * tickets.Field<int>("cantidad"));
                //this.textBox_ticket_montototal.textEdit1.EditValue = tot_tickets;

                // asigna la coleccion de datos al grid de ticket ///
                this.gridControl_tickets.DataSource = ticket_aux;
                this.gridControl_tickets.RefreshDataSource();

                // calculo del monto total en tickets //
                fp_ticketalimentacion_current_row["monto_recaudado"] = tot_tickets;

                //
                this.textBox_ticket_ref1.textEdit1.EditValue = ((string)fp_ticketalimentacion_current_row["ref1"]);
                this.textBox_ticket_ref2.textEdit1.EditValue = ((string)fp_ticketalimentacion_current_row["ref2"]);
            }
        }

        private void carga_detalle_depositos(int ln_modocargadetalle)
        {
            if (ln_modocargadetalle == 1)
            {
                fp_deposito_current_row = fp_deposito.Rows[fp_deposito.Rows.Count - 1];
                lg_forma_pago_deposito_ult = (Guid)fp_deposito.Rows[fp_deposito.Rows.Count - 1]["oid"];
            }
            else
            {
                fp_deposito_current_row = fp_deposito.Rows.Find((Guid)this.checkedComboBoxEdit_depositos_formaspagos.EditValue);
                lg_forma_pago_deposito_ult = (Guid)this.checkedComboBoxEdit_depositos_formaspagos.EditValue;
            }
            //
            recaudacion_det_deposito_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)fp_deposito_current_row["colecction_det"], CriteriaOperator.Parse("1 = 1"));
            deposito_aux = new DataTable();
            deposito_aux = ((DataTable)fp_deposito_current_row["datatable_detalle"]);

            if (deposito_aux.Columns.Count <= 0)
            {
                // se crean las columnas al datatable del detalle de depositos
                deposito_aux.Columns.Add("oid", typeof(Guid));
                deposito_aux.Columns.Add("banco_cuenta", typeof(Guid));
                deposito_aux.Columns.Add("ref1", typeof(string));
                deposito_aux.Columns.Add("ref2", typeof(string));
                deposito_aux.Columns.Add("monto_recaudado", typeof(decimal));
            }

            if (deposito_aux.Rows.Count <= 0)
            {
                // carga datos del detalle de depositos //
                foreach (var item_depositos in recaudacion_det_deposito_aux)
                {
                    deposito_aux.Rows.Add(item_depositos.oid, item_depositos.banco_cuenta.oid, item_depositos.ref1, item_depositos.ref2, item_depositos.monto_recaudado);
                }
            }
            if (ln_modocargadetalle == 2)
            {
                decimal tot_depositos = deposito_aux.AsEnumerable().Sum((deps) => deps.Field<decimal>("monto_recaudado"));

                // asigna la coleccion de datos al grid de depositos ///
                this.gridControl_depositos.DataSource = deposito_aux;
                this.gridControl_depositos.RefreshDataSource();

                // calculo del monto total en depositos //
                fp_deposito_current_row["monto_recaudado"] = tot_depositos;
            }
        }

        private void valid_ref_efectivo(object sender, EventArgs e)
        {
            //
            DataRow fp_efectivo_current_row = fp_efectivo.Rows.Find((Guid)this.checkedComboBoxEdit_efectivo_formaspagos.EditValue);
            //            
            fp_efectivo_current_row["ref1"] = this.textBox_efectivo_ref1.textEdit1.EditValue;
            fp_efectivo_current_row["ref2"] = this.textBox_efectivo_ref2.textEdit1.EditValue;
        }

        private void valid_ref_tickets(object sender, EventArgs e)
        {
            //
            DataRow fp_ticketalimentacion_current_row = fp_ticketalimentacion.Rows.Find((Guid)this.checkedComboBoxEdit_ticket_formaspagos.EditValue);
            //            
            fp_ticketalimentacion_current_row["ref1"] = this.textBox_ticket_ref1.textEdit1.EditValue;
            fp_ticketalimentacion_current_row["ref2"] = this.textBox_ticket_ref2.textEdit1.EditValue;
        }

        private void carga_totales_iniciales()
        {
            // carga los totales generales iniciales //
            //
            ln_total_efectivo_ini = recaudacion_det_efectivo.Sum(det_efectivo => det_efectivo.monto_recaudado);
            ln_total_tarjeta_debito_ini = recaudacion_det_tarjeta_debito.Sum(det_tarjeta_debito => det_tarjeta_debito.monto_recaudado);
            ln_total_tarjeta_credito_ini = recaudacion_det_tarjeta_credito.Sum(det_tarjeta_credito => det_tarjeta_credito.monto_recaudado);
            ln_total_tarjeta_alimentacion_ini = recaudacion_det_tarjeta_alimentacion.Sum(det_tarjeta_alimentacion => det_tarjeta_alimentacion.monto_recaudado);
            ln_total_cheque_ini = recaudacion_det_cheque.Sum(det_cheque => det_cheque.monto_recaudado);
            ln_total_credito_ini = recaudacion_det_credito.Sum(det_credito => det_credito.monto_recaudado);
            ln_total_otrospagos_ini = recaudacion_det_otrospagos.Sum(det_otrospagos => det_otrospagos.monto_recaudado);
            ln_total_pagosinternos_ini = recaudacion_det_pagosinternos.Sum(det_pagosinternos => det_pagosinternos.monto_recaudado);
            ln_total_ticketalimentacion_ini = recaudacion_det_ticketalimentacion.Sum(det_ticket => det_ticket.monto_recaudado);
            ln_total_consumosinternos_ini = recaudacion_det_consumosinternos.Sum(det_consumosinternos => det_consumosinternos.monto_recaudado);
            ln_total_prepago_ini = recaudacion_det_prepago.Sum(det_prepago => det_prepago.monto_recaudado);
            ln_total_deposito_ini = recaudacion_det_deposito.Sum(det_deposito => det_deposito.monto_recaudado);
            ln_total_retencionimpuesto_ini = recaudacion_det_retencionimpuesto.Sum(det_retencionimpuesto => det_retencionimpuesto.monto_recaudado);
            ln_total_exoneracionimpuesto_ini = recaudacion_det_exoneracionimpuesto.Sum(det_exoneracionimpuesto => det_exoneracionimpuesto.monto_recaudado);
            ln_total_islr_ini = recaudacion_det_islr.Sum(det_islr => det_islr.monto_recaudado);
            ln_total_saldofavor_ini = recaudacion_det_saldofavor.Sum(det_saldofavor => det_saldofavor.monto_recaudado);
            ln_total_puntoslealtad_ini = recaudacion_det_puntoslealtad.Sum(det_puntoslealtad => det_puntoslealtad.monto_recaudado);
            ln_total_ninguno_ini = recaudacion_det_ninguno.Sum(det_ninguno => det_ninguno.monto_recaudado);
        }
        
        private void carga_totales(int ln_mod)
        {
            // carga los totales generales //
            //
            if (ln_mod == 0 || ln_mod == 1)
            {
                ln_total_efectivo = 0;

                // remueve rows de efectivo en el datatable de distribucion grafica //
                if (distribucion_grafica.Rows.Count > 0)
                {
                    DataRow row_efectivo = distribucion_grafica.Rows.Find(1);
                    if (row_efectivo != null)
                    {
                        distribucion_grafica.Rows.Remove(row_efectivo);
                    }
                }

                switch (Modo_recaudacion)
                {
                    case 1:
                        ln_total_efectivo = fp_efectivo.AsEnumerable().Sum((fp_efec) => fp_efec.Field<decimal>("monto_recaudado"));
                        break;
                    case 2:
                        if (ln_mod == 0)
                        {
                            if (recaudacion_det_efectivo.Count > 0)
                            { ln_total_efectivo = recaudacion_det_efectivo.Sum(det_efectivo => det_efectivo.monto_recaudado); }
                        }
                        else
                        {
                            ln_total_efectivo = fp_efectivo.AsEnumerable().Sum((fp_efec) => fp_efec.Field<decimal>("monto_recaudado"));
                        }
                        break;
                    default:
                        break;
                }

                // agrega rows de efectivo en el datatable de distribucion grafica //
                if (ln_total_efectivo > 0)
                {
                    distribucion_grafica.Rows.Add(1, "Efectivo", ln_total_efectivo);
                }
            }

            if (ln_mod == 0 || ln_mod == 2)
            {
                ln_total_tarjeta_debito = 0;
 
                // remueve rows de tarjeta debito en el datatable de distribucion grafica //
                if (distribucion_grafica.Rows.Count > 0)
                {
                    DataRow row_td = distribucion_grafica.Rows.Find(2);
                    if (row_td != null)
                    {
                        distribucion_grafica.Rows.Remove(row_td);
                    }
                }

                switch (Modo_recaudacion)
                {
                    case 1:
                        ln_total_tarjeta_debito = fp_tarjeta_debito.AsEnumerable().Sum((fp_tarjeta_d) => fp_tarjeta_d.Field<decimal>("monto_recaudado"));
                        break;
                    case 2:
                        if (ln_mod == 0)
                        {
                            if (recaudacion_det_tarjeta_debito.Count > 0)
                            { ln_total_tarjeta_debito = recaudacion_det_tarjeta_debito.Sum(det_tarjeta_d => det_tarjeta_d.monto_recaudado); }
                        }
                        else
                        {
                            ln_total_tarjeta_debito = fp_tarjeta_debito.AsEnumerable().Sum((fp_tarj_d) => fp_tarj_d.Field<decimal>("monto_recaudado"));
                        }
                        break;
                    default:
                        break;
                }

                // agrega rows de tarjeta debito en el datatable de distribucion grafica //
                if (ln_total_tarjeta_debito > 0)
                {
                    distribucion_grafica.Rows.Add(2, "Tarjetas Debito", ln_total_tarjeta_debito);
                }
            }

            if (ln_mod == 0 || ln_mod == 3)
            {
                ln_total_tarjeta_credito = 0;

                // remueve rows de tarjeta credito en el datatable de distribucion grafica //
                if (distribucion_grafica.Rows.Count > 0)
                {
                    DataRow row_tc = distribucion_grafica.Rows.Find(3);
                    if (row_tc != null)
                    {
                        distribucion_grafica.Rows.Remove(row_tc);
                    }
                }

                switch (Modo_recaudacion)
                {
                    case 1:
                        ln_total_tarjeta_credito = fp_tarjeta_credito.AsEnumerable().Sum((fp_tarjeta_c) => fp_tarjeta_c.Field<decimal>("monto_recaudado"));
                        break;
                    case 2:
                        if (ln_mod == 0)
                        {
                            if (recaudacion_det_tarjeta_credito.Count > 0)
                            { ln_total_tarjeta_credito = recaudacion_det_tarjeta_credito.Sum(det_tarjeta_c => det_tarjeta_c.monto_recaudado); }
                        }
                        else
                        {
                            ln_total_tarjeta_credito = fp_tarjeta_credito.AsEnumerable().Sum((fp_tarj_c) => fp_tarj_c.Field<decimal>("monto_recaudado"));
                        }
                        break;
                    default:
                        break;
                }

                // agrega rows de tarjeta credito en el datatable de distribucion grafica //
                if (ln_total_tarjeta_credito > 0)
                {
                    distribucion_grafica.Rows.Add(3, "Tarjetas Credito", ln_total_tarjeta_credito);
                }
            }
            
            if (ln_mod == 0 || ln_mod == 4)
            {
                ln_total_tarjeta_alimentacion = 0;

                // remueve rows de tarjeta alimentacion en el datatable de distribucion grafica //
                if (distribucion_grafica.Rows.Count > 0)
                {
                    DataRow row_ta = distribucion_grafica.Rows.Find(4);
                    if (row_ta != null)
                    {
                        distribucion_grafica.Rows.Remove(row_ta);
                    }
                }

                switch (Modo_recaudacion)
                {
                    case 1:
                        ln_total_tarjeta_alimentacion = fp_tarjeta_alimentacion.AsEnumerable().Sum((fp_tarjeta_a) => fp_tarjeta_a.Field<decimal>("monto_recaudado"));
                        break;
                    case 2:
                        if (ln_mod == 0)
                        {
                            if (recaudacion_det_tarjeta_alimentacion.Count > 0)
                            { ln_total_tarjeta_alimentacion = recaudacion_det_tarjeta_alimentacion.Sum(det_tarjeta_a => det_tarjeta_a.monto_recaudado); }
                        }
                        else
                        {
                            ln_total_tarjeta_alimentacion = fp_tarjeta_alimentacion.AsEnumerable().Sum((fp_tarj_a) => fp_tarj_a.Field<decimal>("monto_recaudado"));
                        }
                        break;
                    default:
                        break;
                }

                // agrega rows de tarjeta alimentacion en el datatable de distribucion grafica //
                if (ln_total_tarjeta_alimentacion > 0)
                {
                    distribucion_grafica.Rows.Add(4, "Tarjetas Alimentacion", ln_total_tarjeta_alimentacion);
                }
            }
            
            if (ln_mod == 0 || ln_mod == 5)
            {
                ln_total_cheque = 0;

                // remueve rows de cheques en el datatable de distribucion grafica //
                if (distribucion_grafica.Rows.Count > 0)
                {
                    DataRow row_cheques = distribucion_grafica.Rows.Find(5);
                    if (row_cheques != null)
                    {
                        distribucion_grafica.Rows.Remove(row_cheques);
                    }
                }

                switch (Modo_recaudacion)
                {
                    case 1:
                        ln_total_cheque = fp_cheque.AsEnumerable().Sum((fp_ches) => fp_ches.Field<decimal>("monto_recaudado"));
                        break;
                    case 2:
                        if (ln_mod == 0)
                        {
                            if (recaudacion_det_cheque.Count > 0)
                            { ln_total_cheque = recaudacion_det_cheque.Sum(det_ches => det_ches.monto_recaudado); }
                        }
                        else
                        {
                            ln_total_cheque = fp_cheque.AsEnumerable().Sum((fp_ches) => fp_ches.Field<decimal>("monto_recaudado"));
                        }
                        break;
                    default:
                        break;
                }

                // agrega rows de cheque en el datatable de distribucion grafica //
                if (ln_total_cheque > 0)
                {
                    distribucion_grafica.Rows.Add(5, "Cheques", ln_total_cheque);
                }
            }
            
            if (ln_mod == 0 || ln_mod == 6)
            {
                ln_total_credito = 0;

                // remueve rows de creditos en el datatable de distribucion grafica //
                if (distribucion_grafica.Rows.Count > 0)
                {
                    DataRow row_creditos = distribucion_grafica.Rows.Find(6);
                    if (row_creditos != null)
                    {
                        distribucion_grafica.Rows.Remove(row_creditos);
                    }
                }

                switch (Modo_recaudacion)
                {
                    case 1:
                        ln_total_credito = fp_credito.AsEnumerable().Sum((fp_cred) => fp_cred.Field<decimal>("monto_recaudado"));
                        break;
                    case 2:
                        if (ln_mod == 0)
                        {
                            if (recaudacion_det_credito.Count > 0)
                            { ln_total_credito = recaudacion_det_credito.Sum(det_cred => det_cred.monto_recaudado); }
                        }
                        else
                        {
                            ln_total_credito = fp_credito.AsEnumerable().Sum((fp_cred) => fp_cred.Field<decimal>("monto_recaudado"));
                        }
                        break;
                    default:
                        break;
                }

                // agrega rows de credito en el datatable de distribucion grafica //
                if (ln_total_credito > 0)
                {
                    distribucion_grafica.Rows.Add(6, "Creditos", ln_total_credito);
                }
            }
            
            if (ln_mod == 0 || ln_mod == 7)
            {
                recaudacion_det_otrospagos.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_op => true);
                if (recaudacion_det_otrospagos.Count > 0)
                { ln_total_otrospagos = recaudacion_det_otrospagos.Sum(det_otrospagos => det_otrospagos.monto_recaudado); }
                else
                { ln_total_otrospagos = 0; }
            }
            
            if (ln_mod == 0 || ln_mod == 8)
            {
                recaudacion_det_pagosinternos.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_pi => true);
                if (recaudacion_det_pagosinternos.Count > 0)
                { ln_total_pagosinternos = recaudacion_det_pagosinternos.Sum(det_pagosinternos => det_pagosinternos.monto_recaudado); }
                else
                { ln_total_pagosinternos = 0; }
            }
            
            if (ln_mod == 0 || ln_mod == 9)
            {
                ln_total_ticketalimentacion = 0;

                // remueve rows de ticket alimentacion en el datatable de distribucion grafica //
                if (distribucion_grafica.Rows.Count > 0)
                {
                    DataRow row_ticket = distribucion_grafica.Rows.Find(9);
                    if (row_ticket != null)
                    {
                        distribucion_grafica.Rows.Remove(row_ticket);
                    }
                }

                switch (Modo_recaudacion)
                {
                    case 1:
                        ln_total_ticketalimentacion = fp_ticketalimentacion.AsEnumerable().Sum((fp_ticket) => fp_ticket.Field<decimal>("monto_recaudado"));
                        break;
                    case 2:
                        if (ln_mod == 0)
                        {
                            if (recaudacion_det_ticketalimentacion.Count > 0)
                            { ln_total_ticketalimentacion = recaudacion_det_ticketalimentacion.Sum(det_ticket => det_ticket.monto_recaudado); }
                        }
                        else
                        {
                            ln_total_ticketalimentacion = fp_ticketalimentacion.AsEnumerable().Sum((fp_ticket) => fp_ticket.Field<decimal>("monto_recaudado"));
                        }
                        break;
                    default:
                        break;
                }

                // agrega rows de ticket en el datatable de distribucion grafica //
                if (ln_total_ticketalimentacion > 0)
                {
                    distribucion_grafica.Rows.Add(9, "Ticket Alimentacion", ln_total_ticketalimentacion);
                }
            }
            
            if (ln_mod == 0 || ln_mod == 10)
            {
                recaudacion_det_consumosinternos.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_ci => true);
                if (recaudacion_det_consumosinternos.Count > 0)
                { ln_total_consumosinternos = recaudacion_det_consumosinternos.Sum(det_consumosinternos => det_consumosinternos.monto_recaudado); }
                else
                { ln_total_consumosinternos = 0; }
            }
            
            if (ln_mod == 0 || ln_mod == 11)
            {
                recaudacion_det_prepago.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_pre => true);
                if (recaudacion_det_prepago.Count > 0)
                { ln_total_prepago = recaudacion_det_prepago.Sum(det_prepago => det_prepago.monto_recaudado); }
                else
                { ln_total_prepago = 0; }
            }
            
            if (ln_mod == 0 || ln_mod == 12)
            {
                ln_total_deposito = 0;

                // remueve rows de depositos en el datatable de distribucion grafica //
                if (distribucion_grafica.Rows.Count > 0)
                {
                    DataRow row_depositos = distribucion_grafica.Rows.Find(12);
                    if (row_depositos != null)
                    {
                        distribucion_grafica.Rows.Remove(row_depositos);
                    }
                }

                switch (Modo_recaudacion)
                {
                    case 1:
                        ln_total_deposito = fp_deposito.AsEnumerable().Sum((fp_deps) => fp_deps.Field<decimal>("monto_recaudado"));
                        break;
                    case 2:
                        if (ln_mod == 0)
                        {
                            if (recaudacion_det_deposito.Count > 0)
                            { ln_total_deposito = recaudacion_det_deposito.Sum(det_deps => det_deps.monto_recaudado); }
                        }
                        else
                        {
                            ln_total_deposito = fp_deposito.AsEnumerable().Sum((fp_deps) => fp_deps.Field<decimal>("monto_recaudado"));
                        }
                        break;
                    default:
                        break;
                }

                // agrega rows de deposito en el datatable de distribucion grafica //
                if (ln_total_deposito > 0)
                {
                    distribucion_grafica.Rows.Add(12, "Depositos", ln_total_deposito);
                }
            }
            
            if (ln_mod == 0 || ln_mod == 13)
            {
                recaudacion_det_retencionimpuesto.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_ri => true);
                if (recaudacion_det_retencionimpuesto.Count > 0)
                { ln_total_retencionimpuesto = recaudacion_det_retencionimpuesto.Sum(det_retencionimpuesto => det_retencionimpuesto.monto_recaudado); }
                else
                { ln_total_retencionimpuesto = 0; }
            }
            
            if (ln_mod == 0 || ln_mod == 14)
            {
                recaudacion_det_exoneracionimpuesto.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_ei => true);
                if (recaudacion_det_exoneracionimpuesto.Count > 0)
                { ln_total_exoneracionimpuesto = recaudacion_det_exoneracionimpuesto.Sum(det_exoneracionimpuesto => det_exoneracionimpuesto.monto_recaudado); }
                else
                { ln_total_exoneracionimpuesto = 0; }
            }
            
            if (ln_mod == 0 || ln_mod == 15)
            {
                recaudacion_det_islr.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_isrl => true);
                if (recaudacion_det_islr.Count > 0)
                { ln_total_islr = recaudacion_det_islr.Sum(det_islr => det_islr.monto_recaudado); }
                else
                { ln_total_islr = 0; }
            }
            
            if (ln_mod == 0 || ln_mod == 16)
            {
                recaudacion_det_saldofavor.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_sf => true);
                if (recaudacion_det_saldofavor.Count > 0)
                { ln_total_saldofavor = recaudacion_det_saldofavor.Sum(det_saldofavor => det_saldofavor.monto_recaudado); }
                else
                { ln_total_saldofavor = 0; }
            }
            
            if (ln_mod == 0 || ln_mod == 17)
            {
                recaudacion_det_puntoslealtad.Where<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(det_puntos => true);
                if (recaudacion_det_puntoslealtad.Count > 0)
                { ln_total_puntoslealtad = recaudacion_det_puntoslealtad.Sum(det_puntoslealtad => det_puntoslealtad.monto_recaudado); }
                else
                { ln_total_puntoslealtad = 0; }
            }

            if (ln_mod == 0)
            {
                if (recaudacion_det_ninguno.Count > 0)
                { ln_total_ninguno = recaudacion_det_ninguno.Sum(det_ninguno => det_ninguno.monto_recaudado); }
                else
                { ln_total_ninguno = 0; }
            }
        }

        private void suma_totales()
        {
            // asigna los totales a sus tipos y suma los totales generales ///
            //
            this.labelControl_matriz_1_0.lText = ln_total_efectivo.ToString("###,###,###,##0.00");
            this.labelControl_matriz_1_2.lText = ln_total_tarjeta_debito.ToString("###,###,###,##0.00");
            this.labelControl_matriz_1_3.lText = ln_total_tarjeta_credito.ToString("###,###,###,##0.00");
            this.labelControl_matriz_1_4.lText = ln_total_tarjeta_alimentacion.ToString("###,###,###,##0.00");
            this.labelControl_matriz_1_6.lText = ln_total_cheque.ToString("###,###,###,##0.00");
            this.labelControl_matriz_3_0.lText = ln_total_credito.ToString("###,###,###,##0.00");
            this.labelControl_matriz_3_1.lText = ln_total_otrospagos.ToString("###,###,###,##0.00");
            this.labelControl_matriz_3_2.lText = ln_total_pagosinternos.ToString("###,###,###,##0.00");
            this.labelControl_matriz_3_3.lText = ln_total_ticketalimentacion.ToString("###,###,###,##0.00");
            this.labelControl_matriz_3_4.lText = ln_total_consumosinternos.ToString("###,###,###,##0.00");
            this.labelControl_matriz_3_5.lText = ln_total_prepago.ToString("###,###,###,##0.00");
            this.labelControl_matriz_3_6.lText = ln_total_deposito.ToString("###,###,###,##0.00");
            this.labelControl_matriz_5_0.lText = ln_total_retencionimpuesto.ToString("###,###,###,##0.00");
            this.labelControl_matriz_5_1.lText = ln_total_exoneracionimpuesto.ToString("###,###,###,##0.00");
            this.labelControl_matriz_5_2.lText = ln_total_islr.ToString("###,###,###,##0.00");
            this.labelControl_matriz_5_3.lText = ln_total_saldofavor.ToString("###,###,###,##0.00");
            this.labelControl_matriz_5_4.lText = ln_total_puntoslealtad.ToString("###,###,###,##0.00");
            //
            ln_subtotaltarjetas = (ln_total_tarjeta_debito + ln_total_tarjeta_credito + ln_total_tarjeta_alimentacion);
            ln_totalgeneralrecaudacion = (ln_total_efectivo + ln_subtotaltarjetas + ln_total_cheque + ln_total_credito + ln_total_otrospagos);
            ln_totalgeneralrecaudacion = ln_totalgeneralrecaudacion + (ln_total_pagosinternos + ln_total_ticketalimentacion + ln_total_consumosinternos);
            ln_totalgeneralrecaudacion = ln_totalgeneralrecaudacion + (ln_total_prepago + ln_total_deposito + ln_total_retencionimpuesto);
            ln_totalgeneralrecaudacion = ln_totalgeneralrecaudacion + (ln_total_exoneracionimpuesto + ln_total_islr + ln_total_saldofavor + ln_total_puntoslealtad);
            //
            this.labelControl_matriz_01_5.lText = ln_subtotaltarjetas.ToString("#,###,###,###,##0.00");
            this.labelControl_matriz_012345_7.lText = ln_totalgeneralrecaudacion.ToString("###,###,###,###,##0.00");
        }

        private void carga_collection_tipopagos()
        {
            // llena los datos de las colecciones x formas de pagos //
            recaudacion_det_efectivo = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 1"));
            CriteriaOperator filtro_tbedito = (new OperandProperty("forma_pago.tpago") == new OperandValue(2)) & (new OperandProperty("forma_pago.ttarjeta") == new OperandValue(1));
            recaudacion_det_tarjeta_debito = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, filtro_tbedito);
            CriteriaOperator filtro_tcreditp = (new OperandProperty("forma_pago.tpago") == new OperandValue(2)) & (new OperandProperty("forma_pago.ttarjeta") == new OperandValue(2));
            recaudacion_det_tarjeta_credito = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, filtro_tcreditp);
            CriteriaOperator filtro_talimentacion = (new OperandProperty("forma_pago.tpago") == new OperandValue(2)) & (new OperandProperty("forma_pago.ttarjeta") == new OperandValue(3));
            recaudacion_det_tarjeta_alimentacion = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, filtro_talimentacion);
            recaudacion_det_cheque = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 3"));
            recaudacion_det_credito = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 4"));
            recaudacion_det_otrospagos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 5"));
            recaudacion_det_pagosinternos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 6"));
            recaudacion_det_ticketalimentacion = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 7"));
            recaudacion_det_consumosinternos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 8"));
            recaudacion_det_prepago = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 9"));
            recaudacion_det_deposito = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 10"));
            recaudacion_det_retencionimpuesto = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 11"));
            recaudacion_det_exoneracionimpuesto = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 12"));
            recaudacion_det_islr = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 13"));
            recaudacion_det_saldofavor = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 14"));
            recaudacion_det_puntoslealtad = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 15"));
            recaudacion_det_ninguno = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(recaudacion_det, CriteriaOperator.Parse("forma_pago.tpago = 0"));
            //
            DevExpress.Xpo.SortingCollection orden_detalles = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("forma_pago.oid", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            recaudacion_det_efectivo.Sorting = orden_detalles;
            recaudacion_det_tarjeta_debito.Sorting = orden_detalles;
            recaudacion_det_tarjeta_credito.Sorting = orden_detalles;
            recaudacion_det_tarjeta_alimentacion.Sorting = orden_detalles;
            recaudacion_det_cheque.Sorting = orden_detalles;
            recaudacion_det_credito.Sorting = orden_detalles;
            recaudacion_det_otrospagos.Sorting = orden_detalles;
            recaudacion_det_pagosinternos.Sorting = orden_detalles;
            recaudacion_det_ticketalimentacion.Sorting = orden_detalles;
            recaudacion_det_consumosinternos.Sorting = orden_detalles;
            recaudacion_det_prepago.Sorting = orden_detalles;
            recaudacion_det_deposito.Sorting = orden_detalles;
            recaudacion_det_retencionimpuesto.Sorting = orden_detalles;
            recaudacion_det_exoneracionimpuesto.Sorting = orden_detalles;
            recaudacion_det_islr.Sorting = orden_detalles;
            recaudacion_det_saldofavor.Sorting = orden_detalles;
            recaudacion_det_puntoslealtad.Sorting = orden_detalles;
        }

        private void toolStripMenuItem_verdetalle_Click(object sender, EventArgs e)
        {
            foreach (DevExpress.XtraTab.XtraTabPage page_list in this.xtraTabControl_detalle.TabPages)
            {
                if (page_list.Tag.ToString().Trim() == this.ln_picture_activo.Trim())
                {
                    page_list.Select();
                    page_list.Focus();
                    page_list.Show();
                }
            }
        }

        void picture_totales_Click(object sender, EventArgs e)
        {
            this.ln_picture_activo = ((PictureEdit)sender).Tag.ToString();
        }

        private void simpleButton_totales_guardar_Click(object sender, EventArgs e)
        {
            int lnStatus_auxiliar = 0;
            //
            if (ln_totalgeneralrecaudacion > 0)
            {
                if (ln_status_recaudacion == 1 | ln_status_recaudacion == 6)
                {
                    Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("Abierta", "Cerrada", "Cancelar");
                    switch (MessageBox.Show("Seleccione como desea guardar la Recaudación ?" + Environment.NewLine + Environment.NewLine + "Abierta : Deja la Recaudación abierta para ediciónes de datos." + Environment.NewLine + Environment.NewLine + "Cerrada : Cierra la Recaudación y solo se permitiran ajustes.", "Guardar Recaudación", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
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
                {
                    if (ln_status_recaudacion == 2 || ln_status_recaudacion == 3)
                    {
                        lnStatus_auxiliar = 3;
                    }
                    else
                    { 
                        lnStatus_auxiliar = 0;
                    }
                }
                //                
                if (lnStatus_auxiliar != 0 && ln_status_recaudacion != 4)
                {
                    if (MessageBox.Show("Esta seguro de GUARDAR los datos con la seleccion correspondiente ?", "Guardar Recaudación", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                    {
                        distribucion_depositar.Rows.Clear();
                        // se inicia la transaccion para guardar los datos //
                        DevExpress.Xpo.XpoDefault.Session.BeginTransaction();
                        try
                        {
                            // GUARDA ENCABEZADO DE LA RECAUDACION //
                            Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones recaudacion_aux = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones>(lg_recaudacion);
                            //
                            if (recaudacion_aux == null || Modo_recaudacion == 1)
                            {
                                recaudacion_aux = new Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones(DevExpress.Xpo.XpoDefault.Session);
                                recaudacion_aux.sesion = ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).bindingSource1.Current);
                                recaudacion_aux.usuario = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(Fundraising_PT.Properties.Settings.Default.U_oid);
                                //recaudacion_aux.fecha_hora = DateTime.Now;
                                recaudacion_aux.fecha_hora = ld_fecha_recaudacion;
                                //recaudacion_aux.status = lnStatus_auxiliar;
                                recaudacion_aux.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                recaudacion_aux.status = 1;
                                recaudacion_aux.Save();
                                lg_recaudacion = recaudacion_aux.oid;
                            }
                            else
                            {
                                //if (recaudacion_aux.status == 2)
                                //{
                                //    recaudacion_aux.status = 3;
                                //    recaudacion_aux.Save();
                                //}
                            }
                            // FINAL DE GUARDA ENCABEZADO DE LA RECAUDACION //

                            // GUARDA DETALLE EFECTIVO //
                            if (ln_total_efectivo > 0)
                            {
                                foreach (DataRow row_fp_efectivo in fp_efectivo.Rows)
                                {
                                    Guid lg_forma_pago_aux = new Guid();
                                    lg_forma_pago_aux = ((Guid)row_fp_efectivo["oid"]);
                                    recaudacion_det_efectivo_aux = ((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)row_fp_efectivo["colecction_det"]);
                                    recaudacion_det_des_efectivo_aux = ((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>)row_fp_efectivo["colecction_des"]);
                                    DataTable billetes = new DataTable();
                                    DataTable monedas = new DataTable();
                                    billetes.Rows.Clear();
                                    monedas.Rows.Clear();
                                    billetes = ((DataTable)row_fp_efectivo["datatable_billetes"]);
                                    monedas = ((DataTable)row_fp_efectivo["datatable_monedas"]);
                                    decimal ln_total_billetes = billetes.AsEnumerable().Sum((tb) => tb.Field<decimal>("valor") * tb.Field<int>("cantidad"));
                                    decimal ln_total_monedas = monedas.AsEnumerable().Sum((tm) => tm.Field<decimal>("valor") * tm.Field<int>("cantidad"));
                                    //
                                    if (ln_total_billetes + ln_total_monedas > 0)
                                    {
                                        if (recaudacion_det_efectivo_aux.Count <= 0)
                                        {
                                            recaudacion_det_efectivo_aux.Add(new Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det(DevExpress.Xpo.XpoDefault.Session));
                                            recaudacion_det_efectivo_aux[0].recaudacion = recaudacion_aux;
                                            recaudacion_det_efectivo_aux[0].forma_pago = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux);
                                            recaudacion_det_efectivo_aux[0].status = 1;
                                        }
                                        recaudacion_det_efectivo_aux[0].monto_recaudado = ((decimal)row_fp_efectivo["monto_recaudado"]);
                                        recaudacion_det_efectivo_aux[0].monto_retail = 0;
                                        //recaudacion_det_efectivo_aux[0].ref1 = ((string)row_fp_efectivo["ref1"]);
                                        //recaudacion_det_efectivo_aux[0].ref2 = ((string)row_fp_efectivo["ref2"]);
                                        recaudacion_det_efectivo_aux[0].ref1 = row_fp_efectivo["ref1"].ToString();
                                        recaudacion_det_efectivo_aux[0].ref2 = row_fp_efectivo["ref2"].ToString();
                                        recaudacion_det_efectivo_aux[0].sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                        recaudacion_det_efectivo_aux[0].Save();
                                        lg_recaudacion_det = recaudacion_det_efectivo_aux[0].oid;

                                        // si se cierra la recaudacion guarda el total recaudado de la forma de pago tipo efectivo, en la tabla de saldos de recaudaciones para depositar //
                                        if (lnStatus_auxiliar == 2 | lnStatus_auxiliar == 3)
                                        { 
                                            //saldos_recaudaciones_depositos(1, recaudacion_aux.fecha_hora.ToShortDateString(), recaudacion_aux.usuario, recaudacion_det_efectivo_aux[0].forma_pago, recaudacion_det_efectivo_aux[0].monto_recaudado);

                                            
                                            //Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(1, recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                            distribucion_depositar.Rows.Add(recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                        }
                                        
                                        // recorre la tabla de billetes y guarda el desglose de la recaudacion en efectivo-billetes //
                                        foreach (DataRow row_billetes in billetes.Rows)
                                        {
                                            Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des recaudacion_det_des_efectivo_aux1 = null;
                                            try
                                            {
                                                recaudacion_det_des_efectivo_aux1 = recaudacion_det_des_efectivo_aux.First<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>(exis_det_des => exis_det_des.denominacion_moneda.oid == (Guid)row_billetes["oid"]);
                                            }
                                            catch
                                            {
                                                recaudacion_det_des_efectivo_aux1 = null;
                                            }
                                            //                              
                                            if ((int)row_billetes["cantidad"] > 0)
                                            {
                                                if (recaudacion_det_des_efectivo_aux1 == null)
                                                {
                                                    recaudacion_det_des_efectivo_aux1 = new Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des(DevExpress.Xpo.XpoDefault.Session);
                                                    recaudacion_det_des_efectivo_aux1.recaudacion_det = recaudacion_det_efectivo_aux[0];
                                                    recaudacion_det_des_efectivo_aux1.denominacion_moneda = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas>((Guid)row_billetes["oid"]);
                                                }
                                                recaudacion_det_des_efectivo_aux1.denominacion = 0;
                                                recaudacion_det_des_efectivo_aux1.cantidad = (int)row_billetes["cantidad"];
                                                recaudacion_det_des_efectivo_aux1.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                                recaudacion_det_des_efectivo_aux1.Save();
                                            }
                                            else
                                            {
                                                if (recaudacion_det_des_efectivo_aux1 != null)
                                                {
                                                    recaudacion_det_des_efectivo_aux1.Delete();
                                                    recaudacion_det_des_efectivo_aux1.Save();
                                                }
                                            }
                                        }
                                        //
                                        // recorre la tabla de monedas y guarda el desglose de la recaudacion en efectivo-monedas //
                                        foreach (DataRow row_monedas in monedas.Rows)
                                        {
                                            Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des recaudacion_det_des_efectivo_aux1 = null;
                                            try
                                            {
                                                recaudacion_det_des_efectivo_aux1 = recaudacion_det_des_efectivo_aux.First<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>(exis_det_des => exis_det_des.denominacion_moneda.oid == (Guid)row_monedas["oid"]);
                                            }
                                            catch
                                            {
                                                recaudacion_det_des_efectivo_aux1 = null;
                                            }
                                            //                              
                                            if ((int)row_monedas["cantidad"] > 0)
                                            {
                                                if (recaudacion_det_des_efectivo_aux1 == null)
                                                {
                                                    recaudacion_det_des_efectivo_aux1 = new Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des(DevExpress.Xpo.XpoDefault.Session);
                                                    recaudacion_det_des_efectivo_aux1.recaudacion_det = recaudacion_det_efectivo_aux[0];
                                                    recaudacion_det_des_efectivo_aux1.denominacion_moneda = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas>((Guid)row_monedas["oid"]);
                                                }
                                                recaudacion_det_des_efectivo_aux1.denominacion = 0;
                                                recaudacion_det_des_efectivo_aux1.cantidad = (int)row_monedas["cantidad"];
                                                recaudacion_det_des_efectivo_aux1.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                                recaudacion_det_des_efectivo_aux1.Save();
                                            }
                                            else
                                            {
                                                if (recaudacion_det_des_efectivo_aux1 != null)
                                                {
                                                    recaudacion_det_des_efectivo_aux1.Delete();
                                                    recaudacion_det_des_efectivo_aux1.Save();
                                                }
                                            }
                                        }
                                    } // final del if (ln_total_billetes + ln_total_monedas > 0)
                                    else
                                    {
                                        recaudacion_det_efectivo_aux.Session.Delete(recaudacion_det_efectivo_aux);
                                        recaudacion_det_des_efectivo_aux.Session.Delete(recaudacion_det_des_efectivo_aux);
                                        //
                                        recaudacion_det_efectivo_aux.Session.Save(recaudacion_det_efectivo_aux);
                                        recaudacion_det_des_efectivo_aux.Session.Save(recaudacion_det_des_efectivo_aux);
                                        //
                                        //Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(1, recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                        distribucion_depositar.Rows.Add(recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                    }
                                } // final foreach fp_efectivo.rows
                            } // final if del ln_total_efectivo > 0 
                            else
                            {
                                foreach (DataRow row_fp_efectivo in fp_efectivo.Rows)
                                {
                                    Guid lg_forma_pago_aux = new Guid();
                                    lg_forma_pago_aux = ((Guid)row_fp_efectivo["oid"]);
                                    //Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(1, recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                    distribucion_depositar.Rows.Add(recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                }
                                //
                                recaudacion_det_efectivo.Session.Delete(recaudacion_det_efectivo);
                                recaudacion_det_des_efectivo.Session.Delete(recaudacion_det_des_efectivo);
                                //
                                recaudacion_det_efectivo.Session.Save(recaudacion_det_efectivo);
                                recaudacion_det_des_efectivo.Session.Save(recaudacion_det_des_efectivo);
                            }
                            // FINAL DE GUARDA DETALLE EFECTIVO //

                            // GUARDA DETALLE TARJETAS DE DEBITO //
                            if (ln_total_tarjeta_debito > 0)
                            {
                                // primero borra todos los registros de tarjetas de debito, para luego volver a cargarlos ///
                                recaudacion_det_tarjeta_debito.Session.Delete(recaudacion_det_tarjeta_debito);
                                recaudacion_det_tarjeta_debito.Session.Save(recaudacion_det_tarjeta_debito);

                                foreach (DataRow row_fp_tarjeta_d in fp_tarjeta_debito.Rows)
                                {
                                    Guid lg_forma_pago_aux = new Guid();
                                    lg_forma_pago_aux = ((Guid)row_fp_tarjeta_d["oid"]);
                                    recaudacion_det_tarjeta_debito_aux = ((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)row_fp_tarjeta_d["colecction_det"]);
                                    DataTable tarjetas_d = new DataTable();
                                    tarjetas_d.Rows.Clear();
                                    tarjetas_d = ((DataTable)row_fp_tarjeta_d["datatable_detalle"]);
                                    decimal ln_total_td = tarjetas_d.AsEnumerable().Sum((ttd) => ttd.Field<decimal>("monto_recaudado"));
                                    //
                                    if (ln_total_td > 0)
                                    {
                                        // recorre la datatable de tarjetas debito y guarda el detalle //
                                        foreach (DataRow row_tarjetas_d in tarjetas_d.Rows)
                                        {
                                            Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det recaudacion_det_tarjeta_debito_aux1 = null;
                                            try
                                            {
                                                recaudacion_det_tarjeta_debito_aux1 = recaudacion_det_tarjeta_debito_aux.First<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(exis_det => exis_det.oid == (Guid)row_tarjetas_d["oid"]);
                                            }
                                            catch
                                            {
                                                recaudacion_det_tarjeta_debito_aux1 = null;
                                            }
                                            //                              
                                            if ((decimal)row_tarjetas_d["monto_recaudado"] > 0)
                                            {
                                                if (recaudacion_det_tarjeta_debito_aux1 == null)
                                                {
                                                    recaudacion_det_tarjeta_debito_aux1 = new Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det(DevExpress.Xpo.XpoDefault.Session);
                                                    recaudacion_det_tarjeta_debito_aux1.recaudacion = recaudacion_aux;
                                                    recaudacion_det_tarjeta_debito_aux1.status = 1;
                                                }
                                                recaudacion_det_tarjeta_debito_aux1.forma_pago = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux);
                                                recaudacion_det_tarjeta_debito_aux1.punto_bancario = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios>((Guid)row_tarjetas_d["punto_bancario"]);
                                                recaudacion_det_tarjeta_debito_aux1.ref1 = row_tarjetas_d["ref1"].ToString();
                                                recaudacion_det_tarjeta_debito_aux1.ref2 = row_tarjetas_d["ref2"].ToString();
                                                //recaudacion_det_tarjeta_debito_aux1.ref1 = (string)row_tarjetas_d["ref1"];
                                                //if (row_tarjetas_d["ref2"] == null)
                                                //{ recaudacion_det_tarjeta_debito_aux1.ref2 = string.Empty; }
                                                //else
                                                //{ recaudacion_det_tarjeta_debito_aux1.ref2 = (string)row_tarjetas_d["ref2"]; }
                                                recaudacion_det_tarjeta_debito_aux1.monto_recaudado = (decimal)row_tarjetas_d["monto_recaudado"];
                                                recaudacion_det_tarjeta_debito_aux1.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                                recaudacion_det_tarjeta_debito_aux1.Save();
                                            }
                                            else
                                            {
                                                if (recaudacion_det_tarjeta_debito_aux1 != null)
                                                {
                                                    recaudacion_det_tarjeta_debito_aux1.Delete();
                                                    recaudacion_det_tarjeta_debito_aux1.Save();
                                                }
                                            }
                                        }
                                    } // final if ln_total_td > 0
                                    else
                                    {
                                        recaudacion_det_tarjeta_debito_aux.Session.Delete(recaudacion_det_tarjeta_debito_aux);
                                        recaudacion_det_tarjeta_debito_aux.Session.Save(recaudacion_det_tarjeta_debito_aux);
                                    }
                                } // final foreach row_tarjetas_d.rows
                            } // final del if ln_total_tarjeta_debito > 0
                            else
                            {
                                recaudacion_det_tarjeta_debito.Session.Delete(recaudacion_det_tarjeta_debito);
                                recaudacion_det_tarjeta_debito.Session.Save(recaudacion_det_tarjeta_debito);
                            }
                            // FINAL DE GUARDA DETALLE TARJETAS DE DEBITO //

                            // GUARDA DETALLE TARJETAS DE CREDITO //
                            if (ln_total_tarjeta_credito > 0)
                            {
                                // primero borra todos los registros de tarjetas de credito, para luego volver a cargarlos ///
                                recaudacion_det_tarjeta_credito.Session.Delete(recaudacion_det_tarjeta_credito);
                                recaudacion_det_tarjeta_credito.Session.Save(recaudacion_det_tarjeta_credito);

                                foreach (DataRow row_fp_tarjeta_c in fp_tarjeta_credito.Rows)
                                {
                                    Guid lg_forma_pago_aux = new Guid();
                                    lg_forma_pago_aux = ((Guid)row_fp_tarjeta_c["oid"]);
                                    recaudacion_det_tarjeta_credito_aux = ((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)row_fp_tarjeta_c["colecction_det"]);
                                    DataTable tarjetas_c = new DataTable();
                                    tarjetas_c.Rows.Clear();
                                    tarjetas_c = ((DataTable)row_fp_tarjeta_c["datatable_detalle"]);
                                    decimal ln_total_tc = tarjetas_c.AsEnumerable().Sum((ttc) => ttc.Field<decimal>("monto_recaudado"));
                                    //
                                    if (ln_total_tc > 0)
                                    {
                                        // recorre la datatable de tarjetas credito y guarda el detalle //
                                        foreach (DataRow row_tarjetas_c in tarjetas_c.Rows)
                                        {
                                            Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det recaudacion_det_tarjeta_credito_aux1 = null;
                                            try
                                            {
                                                recaudacion_det_tarjeta_credito_aux1 = recaudacion_det_tarjeta_credito_aux.First<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(exis_det => exis_det.oid == (Guid)row_tarjetas_c["oid"]);
                                            }
                                            catch
                                            {
                                                recaudacion_det_tarjeta_credito_aux1 = null;
                                            }
                                            //                              
                                            if ((decimal)row_tarjetas_c["monto_recaudado"] > 0)
                                            {
                                                if (recaudacion_det_tarjeta_credito_aux1 == null)
                                                {
                                                    recaudacion_det_tarjeta_credito_aux1 = new Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det(DevExpress.Xpo.XpoDefault.Session);
                                                    recaudacion_det_tarjeta_credito_aux1.recaudacion = recaudacion_aux;
                                                    recaudacion_det_tarjeta_credito_aux1.status = 1;
                                                }
                                                recaudacion_det_tarjeta_credito_aux1.forma_pago = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux);
                                                recaudacion_det_tarjeta_credito_aux1.punto_bancario = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios>((Guid)row_tarjetas_c["punto_bancario"]);
                                                recaudacion_det_tarjeta_credito_aux1.ref1 = row_tarjetas_c["ref1"].ToString();
                                                recaudacion_det_tarjeta_credito_aux1.ref2 = row_tarjetas_c["ref2"].ToString();
                                                //recaudacion_det_tarjeta_credito_aux1.ref1 = (string)row_tarjetas_c["ref1"];
                                                //if (row_tarjetas_c["ref2"] == null)
                                                //{ recaudacion_det_tarjeta_credito_aux1.ref2 = string.Empty; }
                                                //else
                                                //{ recaudacion_det_tarjeta_credito_aux1.ref2 = (string)row_tarjetas_c["ref2"]; }
                                                recaudacion_det_tarjeta_credito_aux1.monto_recaudado = (decimal)row_tarjetas_c["monto_recaudado"];
                                                recaudacion_det_tarjeta_credito_aux1.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                                recaudacion_det_tarjeta_credito_aux1.Save();
                                            }
                                            else
                                            {
                                                if (recaudacion_det_tarjeta_credito_aux1 != null)
                                                {
                                                    recaudacion_det_tarjeta_credito_aux1.Delete();
                                                    recaudacion_det_tarjeta_credito_aux1.Save();
                                                }
                                            }
                                        }
                                    } // final if ln_total_tc > 0
                                    else
                                    {
                                        recaudacion_det_tarjeta_credito_aux.Session.Delete(recaudacion_det_tarjeta_credito_aux);
                                        recaudacion_det_tarjeta_credito_aux.Session.Save(recaudacion_det_tarjeta_credito_aux);
                                    }
                                } // final foreach row_tarjetas_c.rows
                            } // final del if ln_total_tarjeta_credito > 0
                            else
                            {
                                recaudacion_det_tarjeta_credito.Session.Delete(recaudacion_det_tarjeta_credito);
                                recaudacion_det_tarjeta_credito.Session.Save(recaudacion_det_tarjeta_credito);
                            }
                            // FINAL DE GUARDA DETALLE TARJETAS DE CREDITO //

                            // GUARDA DETALLE TARJETAS ALIMENTACION //
                            if (ln_total_tarjeta_alimentacion > 0)
                            {
                                // primero borra todos los registros de tarjetas alimentacion, para luego volver a cargarlos ///
                                recaudacion_det_tarjeta_alimentacion.Session.Delete(recaudacion_det_tarjeta_alimentacion);
                                recaudacion_det_tarjeta_alimentacion.Session.Save(recaudacion_det_tarjeta_alimentacion);

                                foreach (DataRow row_fp_tarjeta_a in fp_tarjeta_alimentacion.Rows)
                                {
                                    Guid lg_forma_pago_aux = new Guid();
                                    Guid lg_proveedor_ta_aux = new Guid();
                                    lg_forma_pago_aux = ((Guid)row_fp_tarjeta_a["oid"]);
                                    lg_proveedor_ta_aux = ((Guid)row_fp_tarjeta_a["proveedor_ta"]);
                                    //
                                    recaudacion_det_tarjeta_alimentacion_aux = ((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)row_fp_tarjeta_a["colecction_det"]);
                                    //
                                    DataTable tarjetas_a = new DataTable();
                                    tarjetas_a.Rows.Clear();
                                    tarjetas_a = ((DataTable)row_fp_tarjeta_a["datatable_detalle"]);
                                    decimal ln_total_ta = tarjetas_a.AsEnumerable().Sum((tta) => tta.Field<decimal>("monto_recaudado"));
                                    //
                                    if (ln_total_ta > 0)
                                    {
                                        // recorre la datatable de tarjetas alimentacion y guarda el detalle //
                                        foreach (DataRow row_tarjetas_a in tarjetas_a.Rows)
                                        {
                                            Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det recaudacion_det_tarjeta_alimentacion_aux1 = null;
                                            try
                                            {
                                                recaudacion_det_tarjeta_alimentacion_aux1 = recaudacion_det_tarjeta_alimentacion_aux.First<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(exis_det => exis_det.oid == (Guid)row_tarjetas_a["oid"]);
                                            }
                                            catch
                                            {
                                                recaudacion_det_tarjeta_alimentacion_aux1 = null;
                                            }
                                            //                              
                                            if ((decimal)row_tarjetas_a["monto_recaudado"] > 0)
                                            {
                                                if (recaudacion_det_tarjeta_alimentacion_aux1 == null)
                                                {
                                                    recaudacion_det_tarjeta_alimentacion_aux1 = new Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det(DevExpress.Xpo.XpoDefault.Session);
                                                    recaudacion_det_tarjeta_alimentacion_aux1.recaudacion = recaudacion_aux;
                                                    recaudacion_det_tarjeta_alimentacion_aux1.status = 1;
                                                }
                                                recaudacion_det_tarjeta_alimentacion_aux1.forma_pago = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux);
                                                recaudacion_det_tarjeta_alimentacion_aux1.punto_bancario = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios>((Guid)row_tarjetas_a["punto_bancario"]);
                                                recaudacion_det_tarjeta_alimentacion_aux1.ref1 = row_tarjetas_a["ref1"].ToString();
                                                recaudacion_det_tarjeta_alimentacion_aux1.ref2 = row_tarjetas_a["ref2"].ToString();
                                                //recaudacion_det_tarjeta_alimentacion_aux1.ref1 = (string)row_tarjetas_a["ref1"];
                                                //if (row_tarjetas_a["ref2"] == null)
                                                //{ recaudacion_det_tarjeta_alimentacion_aux1.ref2 = string.Empty; }
                                                //else
                                                //{ recaudacion_det_tarjeta_alimentacion_aux1.ref2 = (string)row_tarjetas_a["ref2"]; }
                                                recaudacion_det_tarjeta_alimentacion_aux1.monto_recaudado = (decimal)row_tarjetas_a["monto_recaudado"];
                                                recaudacion_det_tarjeta_alimentacion_aux1.proveedor_ta = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA>(lg_proveedor_ta_aux);
                                                recaudacion_det_tarjeta_alimentacion_aux1.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                                recaudacion_det_tarjeta_alimentacion_aux1.Save();
                                            }
                                            else
                                            {
                                                if (recaudacion_det_tarjeta_alimentacion_aux1 != null)
                                                {
                                                    recaudacion_det_tarjeta_alimentacion_aux1.Delete();
                                                    recaudacion_det_tarjeta_alimentacion_aux1.Save();
                                                }
                                            }
                                        }
                                    } // final if ln_total_ta > 0
                                    else
                                    {
                                        recaudacion_det_tarjeta_alimentacion_aux.Session.Delete(recaudacion_det_tarjeta_alimentacion_aux);
                                        recaudacion_det_tarjeta_alimentacion_aux.Session.Save(recaudacion_det_tarjeta_alimentacion_aux);
                                    }
                                } // final foreach row_tarjetas_a.rows
                            } // final del if ln_total_tarjeta_alimentacion > 0
                            else
                            {
                                recaudacion_det_tarjeta_alimentacion.Session.Delete(recaudacion_det_tarjeta_alimentacion);
                                recaudacion_det_tarjeta_alimentacion.Session.Save(recaudacion_det_tarjeta_alimentacion);
                            }
                            // FINAL DE GUARDA DETALLE TARJETAS ALIMENTACION //

                            // GUARDA DETALLE CHEQUES //
                            if (ln_total_cheque > 0)
                            {
                                // primero borra todos los registros de cheques, para luego volver a cergarlos ///
                                recaudacion_det_cheque.Session.Delete(recaudacion_det_cheque);
                                recaudacion_det_cheque.Session.Save(recaudacion_det_cheque);

                                foreach (DataRow row_fp_cheque in fp_cheque.Rows)
                                {
                                    Guid lg_forma_pago_aux = new Guid();
                                    lg_forma_pago_aux = ((Guid)row_fp_cheque["oid"]);
                                    recaudacion_det_cheque_aux = ((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)row_fp_cheque["colecction_det"]);
                                    DataTable cheques = new DataTable();
                                    cheques.Rows.Clear();
                                    cheques = ((DataTable)row_fp_cheque["datatable_detalle"]);
                                    decimal ln_total_che = cheques.AsEnumerable().Sum((tche) => tche.Field<decimal>("monto_recaudado"));

                                    if (ln_total_che > 0)
                                    {
                                        // recorre la datatable de cheques y guarda el detalle //
                                        foreach (DataRow row_cheques in cheques.Rows)
                                        {
                                            Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det recaudacion_det_cheque_aux1 = null;
                                            try
                                            {
                                                recaudacion_det_cheque_aux1 = recaudacion_det_cheque_aux.First<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(exis_det => exis_det.oid == (Guid)row_cheques["oid"]);
                                            }
                                            catch
                                            {
                                                recaudacion_det_cheque_aux1 = null;
                                            }
                                            //                              
                                            if ((decimal)row_cheques["monto_recaudado"] > 0)
                                            {
                                                if (recaudacion_det_cheque_aux1 == null)
                                                {
                                                    recaudacion_det_cheque_aux1 = new Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det(DevExpress.Xpo.XpoDefault.Session);
                                                    recaudacion_det_cheque_aux1.recaudacion = recaudacion_aux;
                                                    recaudacion_det_cheque_aux1.status = 1;
                                                }
                                                recaudacion_det_cheque_aux1.forma_pago = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux);
                                                recaudacion_det_cheque_aux1.banco = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Bancos>((Guid)row_cheques["banco"]);
                                                recaudacion_det_cheque_aux1.ref1 = row_cheques["ref1"].ToString();
                                                recaudacion_det_cheque_aux1.ref2 = row_cheques["ref2"].ToString();
                                                recaudacion_det_cheque_aux1.monto_recaudado = (decimal)row_cheques["monto_recaudado"];
                                                recaudacion_det_cheque_aux1.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                                recaudacion_det_cheque_aux1.Save();
                                            }
                                            else
                                            {
                                                if (recaudacion_det_cheque_aux1 != null)
                                                {
                                                    recaudacion_det_cheque_aux1.Delete();
                                                    recaudacion_det_cheque_aux1.Save();
                                                }
                                            }
                                        }
                                    } // final if ln_total_che > 0
                                    else
                                    {
                                        recaudacion_det_cheque_aux.Session.Delete(recaudacion_det_cheque_aux);
                                        recaudacion_det_cheque_aux.Session.Save(recaudacion_det_cheque_aux);
                                    }
                                    //
                                    //Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(1, recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                    distribucion_depositar.Rows.Add(recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                    //
                                } // final foreach row_cheques.rows
                            } // final del if ln_total_cheque > 0
                            else
                            {
                                foreach (DataRow row_fp_cheque in fp_cheque.Rows)
                                {
                                    Guid lg_forma_pago_aux = new Guid();
                                    lg_forma_pago_aux = ((Guid)row_fp_cheque["oid"]);
                                    //Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(1, recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                    distribucion_depositar.Rows.Add(recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                }
                                //
                                recaudacion_det_cheque.Session.Delete(recaudacion_det_cheque);
                                recaudacion_det_cheque.Session.Save(recaudacion_det_cheque);
                            }
                            // FINAL DE GUARDA DETALLE CHEQUES //

                            // GUARDA DETALLE CREDITOS //
                            if (ln_total_credito > 0)
                            {
                                // primero borra todos los registros de creditos, para luego volver a cargarlos ///
                                recaudacion_det_credito.Session.Delete(recaudacion_det_credito);
                                recaudacion_det_credito.Session.Save(recaudacion_det_credito);

                                foreach (DataRow row_fp_credito in fp_credito.Rows)
                                {
                                    Guid lg_forma_pago_aux = new Guid();
                                    lg_forma_pago_aux = ((Guid)row_fp_credito["oid"]);
                                    recaudacion_det_credito_aux = ((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)row_fp_credito["colecction_det"]);
                                    DataTable creditos = new DataTable();
                                    creditos.Rows.Clear();
                                    creditos = ((DataTable)row_fp_credito["datatable_detalle"]);
                                    decimal ln_total_cre = creditos.AsEnumerable().Sum((tcre) => tcre.Field<decimal>("monto_recaudado"));
                                    //
                                    if (ln_total_cre > 0)
                                    {
                                        // recorre la datatable de creditos y guarda el detalle //
                                        foreach (DataRow row_creditos in creditos.Rows)
                                        {
                                            Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det recaudacion_det_credito_aux1 = null;
                                            try
                                            {
                                                recaudacion_det_credito_aux1 = recaudacion_det_credito_aux.First<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(exis_det => exis_det.oid == (Guid)row_creditos["oid"]);
                                            }
                                            catch
                                            {
                                                recaudacion_det_credito_aux1 = null;
                                            }
                                            //                              
                                            if ((decimal)row_creditos["monto_recaudado"] > 0)
                                            {
                                                if (recaudacion_det_credito_aux1 == null)
                                                {
                                                    recaudacion_det_credito_aux1 = new Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det(DevExpress.Xpo.XpoDefault.Session);
                                                    recaudacion_det_credito_aux1.recaudacion = recaudacion_aux;
                                                    recaudacion_det_credito_aux1.status = 1;
                                                }
                                                recaudacion_det_credito_aux1.forma_pago = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux);
                                                recaudacion_det_credito_aux1.ref1 = row_creditos["ref1"].ToString();
                                                recaudacion_det_credito_aux1.ref2 = row_creditos["ref2"].ToString();
                                                //recaudacion_det_credito_aux1.ref1 = (string)row_creditos["ref1"];
                                                //if (row_creditos["ref2"] == null)
                                                //{ recaudacion_det_credito_aux1.ref2 = string.Empty; }
                                                //else
                                                //{ recaudacion_det_credito_aux1.ref2 = (string)row_creditos["ref2"]; }
                                                recaudacion_det_credito_aux1.monto_recaudado = (decimal)row_creditos["monto_recaudado"];
                                                recaudacion_det_credito_aux1.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                                recaudacion_det_credito_aux1.Save();
                                            }
                                            else
                                            {
                                                if (recaudacion_det_credito_aux1 != null)
                                                {
                                                    recaudacion_det_credito_aux1.Delete();
                                                    recaudacion_det_credito_aux1.Save();
                                                }
                                            }
                                        }
                                    } // final if ln_total_cre > 0
                                    else
                                    {
                                        recaudacion_det_credito_aux.Session.Delete(recaudacion_det_credito_aux);
                                        recaudacion_det_credito_aux.Session.Save(recaudacion_det_credito_aux);
                                    }
                                } // final foreach row_creditos.rows
                            } // final del if ln_total_credito > 0
                            else
                            {
                                recaudacion_det_credito.Session.Delete(recaudacion_det_credito);
                                recaudacion_det_credito.Session.Save(recaudacion_det_credito);
                            }
                            // FINAL DE GUARDA DETALLE CREDITOS //

                            // GUARDA DETALLE TICKET ALIMENTACION //
                            if (ln_total_ticketalimentacion > 0)
                            {
                                // primero borra todos los registros de tickets alimentacion, para luego volver a cargarlos ///
                                recaudacion_det_ticketalimentacion.Session.Delete(recaudacion_det_ticketalimentacion);
                                recaudacion_det_des_ticket.Session.Delete(recaudacion_det_des_ticket);
                                recaudacion_det_ticketalimentacion.Session.Save(recaudacion_det_ticketalimentacion);
                                recaudacion_det_des_ticket.Session.Save(recaudacion_det_des_ticket);

                                foreach (DataRow row_fp_ticket in fp_ticketalimentacion.Rows)
                                {
                                    Guid lg_forma_pago_aux = new Guid();
                                    Guid lg_proveedor_ta_aux = new Guid();
                                    lg_forma_pago_aux = ((Guid)row_fp_ticket["oid"]);
                                    lg_proveedor_ta_aux = ((Guid)row_fp_ticket["proveedor_ta"]);
                                    //
                                    recaudacion_det_ticketalimentacion_aux = ((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)row_fp_ticket["colecction_det"]);
                                    recaudacion_det_des_ticket_aux = ((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>)row_fp_ticket["colecction_des"]);
                                    //
                                    DataTable tickets = new DataTable();
                                    tickets.Rows.Clear();
                                    tickets = ((DataTable)row_fp_ticket["datatable_desgloce"]);
                                    decimal ln_total_tck = tickets.AsEnumerable().Sum((ttck) => ttck.Field<decimal>("valor") * ttck.Field<int>("cantidad"));
                                    //
                                    if (ln_total_tck > 0)
                                    {
                                        if (recaudacion_det_ticketalimentacion_aux.Count <= 0)
                                        {
                                            recaudacion_det_ticketalimentacion_aux.Add(new Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det(DevExpress.Xpo.XpoDefault.Session));
                                            recaudacion_det_ticketalimentacion_aux[0].recaudacion = recaudacion_aux;
                                            recaudacion_det_ticketalimentacion_aux[0].forma_pago = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux);
                                            recaudacion_det_ticketalimentacion_aux[0].proveedor_ta = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA>(lg_proveedor_ta_aux);
                                            recaudacion_det_ticketalimentacion_aux[0].status = 1;
                                        }
                                        recaudacion_det_ticketalimentacion_aux[0].monto_recaudado = ((decimal)row_fp_ticket["monto_recaudado"]);
                                        recaudacion_det_ticketalimentacion_aux[0].monto_retail = 0;
                                        recaudacion_det_ticketalimentacion_aux[0].ref1 = row_fp_ticket["ref1"].ToString();
                                        recaudacion_det_ticketalimentacion_aux[0].ref2 = row_fp_ticket["ref2"].ToString();
                                        //recaudacion_det_ticketalimentacion_aux[0].ref1 = ((string)row_fp_ticket["ref1"]);
                                        //recaudacion_det_ticketalimentacion_aux[0].ref2 = ((string)row_fp_ticket["ref2"]);
                                        recaudacion_det_ticketalimentacion_aux[0].sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                        recaudacion_det_ticketalimentacion_aux[0].Save();
                                        lg_recaudacion_det = recaudacion_det_ticketalimentacion_aux[0].oid;
                                        //
                                        // si se cierra la recaudacion guarda el total recaudado de la forma de pago tipo tickets alimentacion, en la tabla de saldos de recaudaciones para depositar //
                                        if (lnStatus_auxiliar == 2 | lnStatus_auxiliar == 3)
                                        { 
                                            //saldos_recaudaciones_depositos(1, recaudacion_aux.fecha_hora.ToShortDateString(), recaudacion_aux.usuario, recaudacion_det_ticketalimentacion_aux[0].forma_pago, recaudacion_det_ticketalimentacion_aux[0].monto_recaudado);
                        
                                            
                                            //Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(1, recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                            distribucion_depositar.Rows.Add(recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                        }
                                        // recorre la tabla de tickets y guarda el desglose de la recaudacion en tickets alimentacion //
                                        foreach (DataRow row_tickets in tickets.Rows)
                                        {
                                            Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des recaudacion_det_des_ticket_aux1 = null;
                                            try
                                            {
                                                //recaudacion_det_des_ticket_aux1 = recaudacion_det_des_ticket_aux.First<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>(exis_det_des => exis_det_des.denominacion == (Decimal)row_tickets["valor"]);
                                                recaudacion_det_des_ticket_aux1 = recaudacion_det_des_ticket_aux.First<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des>(exis_det_des => exis_det_des.oid == (Guid)row_tickets["oid"]);
                                            }
                                            catch
                                            {
                                                recaudacion_det_des_ticket_aux1 = null;
                                            }
                                            //                              
                                            if ((int)row_tickets["cantidad"] > 0)
                                            {
                                                if (recaudacion_det_des_ticket_aux1 == null)
                                                {
                                                    recaudacion_det_des_ticket_aux1 = new Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det_Des(DevExpress.Xpo.XpoDefault.Session);
                                                    recaudacion_det_des_ticket_aux1.recaudacion_det = recaudacion_det_ticketalimentacion_aux[0];
                                                }
                                                recaudacion_det_des_ticket_aux1.denominacion = (Decimal)row_tickets["valor"];
                                                recaudacion_det_des_ticket_aux1.cantidad = (int)row_tickets["cantidad"];
                                                recaudacion_det_des_ticket_aux1.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                                recaudacion_det_des_ticket_aux1.Save();
                                            }
                                            else
                                            {
                                                if (recaudacion_det_des_ticket_aux1 != null)
                                                {
                                                    recaudacion_det_des_ticket_aux1.Delete();
                                                    recaudacion_det_des_ticket_aux1.Save();
                                                }
                                            }
                                        }
                                    } // final if ln_total_tck > 0
                                    else
                                    {
                                        recaudacion_det_ticketalimentacion_aux.Session.Delete(recaudacion_det_ticketalimentacion_aux);
                                        recaudacion_det_des_ticket_aux.Session.Delete(recaudacion_det_des_ticket_aux);
                                        //
                                        recaudacion_det_ticketalimentacion_aux.Session.Save(recaudacion_det_ticketalimentacion_aux);
                                        recaudacion_det_des_ticket_aux.Session.Save(recaudacion_det_des_ticket_aux);
                                        //
                                        //Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(1, recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                        distribucion_depositar.Rows.Add(recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                    }
                                } // final foreach fp_ticket.rows
                            } // final del if ln_total_ticketalimentacion > 0
                            else
                            {
                                foreach (DataRow row_fp_ticketalimentacion in fp_ticketalimentacion.Rows)
                                {
                                    Guid lg_forma_pago_aux = new Guid();
                                    lg_forma_pago_aux = ((Guid)row_fp_ticketalimentacion["oid"]);
                                    //Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(1, recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                    distribucion_depositar.Rows.Add(recaudacion_aux.fecha_hora, recaudacion_aux.usuario, DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux));
                                }
                                //
                                recaudacion_det_ticketalimentacion.Session.Delete(recaudacion_det_ticketalimentacion);
                                recaudacion_det_des_ticket.Session.Delete(recaudacion_det_des_ticket);
                                //
                                recaudacion_det_ticketalimentacion.Session.Save(recaudacion_det_ticketalimentacion);
                                recaudacion_det_des_ticket.Session.Save(recaudacion_det_des_ticket);
                            }
                            // FINAL DE GUARDA DETALLE TICKET ALIMENTACION //

                            // GUARDA DETALLE DEPOSITOS //
                            if (ln_total_deposito > 0)
                            {
                                // primero borra todos los registros de depositos, para luego volver a cargarlos ///
                                recaudacion_det_deposito.Session.Delete(recaudacion_det_deposito);
                                recaudacion_det_deposito.Session.Save(recaudacion_det_deposito);

                                foreach (DataRow row_fp_deposito in fp_deposito.Rows)
                                {
                                    Guid lg_forma_pago_aux = new Guid();
                                    lg_forma_pago_aux = ((Guid)row_fp_deposito["oid"]);
                                    recaudacion_det_deposito_aux = ((DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>)row_fp_deposito["colecction_det"]);
                                    DataTable depositos = new DataTable();
                                    depositos.Rows.Clear();
                                    depositos = ((DataTable)row_fp_deposito["datatable_detalle"]);
                                    decimal ln_total_dep = depositos.AsEnumerable().Sum((tdep) => tdep.Field<decimal>("monto_recaudado"));
                                    //
                                    if (ln_total_dep > 0)
                                    {
                                        // recorre la datatable de depositos y guarda el detalle //
                                        foreach (DataRow row_depositos in depositos.Rows)
                                        {
                                            Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det recaudacion_det_deposito_aux1 = null;
                                            try
                                            {
                                                recaudacion_det_deposito_aux1 = recaudacion_det_deposito_aux.First<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det>(exis_det => exis_det.oid == (Guid)row_depositos["oid"]);
                                            }
                                            catch
                                            {
                                                recaudacion_det_deposito_aux1 = null;
                                            }
                                            //                              
                                            if ((decimal)row_depositos["monto_recaudado"] > 0)
                                            {
                                                if (recaudacion_det_deposito_aux1 == null)
                                                {
                                                    recaudacion_det_deposito_aux1 = new Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det(DevExpress.Xpo.XpoDefault.Session);
                                                    recaudacion_det_deposito_aux1.recaudacion = recaudacion_aux;
                                                    recaudacion_det_deposito_aux1.status = 1;
                                                }
                                                recaudacion_det_deposito_aux1.forma_pago = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(lg_forma_pago_aux);
                                                recaudacion_det_deposito_aux1.banco_cuenta = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas>((Guid)row_depositos["banco_cuenta"]);
                                                recaudacion_det_deposito_aux1.ref1 = row_depositos["ref1"].ToString();
                                                recaudacion_det_deposito_aux1.ref2 = row_depositos["ref2"].ToString();
                                                //recaudacion_det_deposito_aux1.ref1 = (string)row_depositos["ref1"];
                                                //if (row_depositos["ref2"] == null)
                                                //{ recaudacion_det_deposito_aux1.ref2 = string.Empty; }
                                                //else
                                                //{ recaudacion_det_deposito_aux1.ref2 = (string)row_depositos["ref2"]; }
                                                recaudacion_det_deposito_aux1.monto_recaudado = (decimal)row_depositos["monto_recaudado"];
                                                recaudacion_det_deposito_aux1.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                                recaudacion_det_deposito_aux1.Save();
                                            }
                                            else
                                            {
                                                if (recaudacion_det_deposito_aux1 != null)
                                                {
                                                    recaudacion_det_deposito_aux1.Delete();
                                                    recaudacion_det_deposito_aux1.Save();
                                                }
                                            }
                                        }
                                    } // final if ln_total_dep > 0
                                    else
                                    {
                                        recaudacion_det_deposito_aux.Session.Delete(recaudacion_det_deposito_aux);
                                        recaudacion_det_deposito_aux.Session.Save(recaudacion_det_deposito_aux);
                                    }
                                } // final foreach row_depositos.rows
                            } // final del if ln_total_deposito > 0
                            else
                            {
                                recaudacion_det_deposito.Session.Delete(recaudacion_det_deposito);
                                recaudacion_det_deposito.Session.Save(recaudacion_det_deposito);
                            }
                            // FINAL DE GUARDA DETALLE DEPOSITOS //

                            // cambia estatus a la sesion y la recaudacion  y termina l el proceso //
                            if (recaudacion_aux.status == 2)
                            {
                                ln_status_recaudacion = 3;
                                lnStatus_auxiliar = 3;
                                recaudacion_aux.status = 3;
                                recaudacion_aux.Save();
                            }
                            else
                            {
                                if (recaudacion_aux.status == 1 | recaudacion_aux.status == 6)
                                {
                                    ln_status_recaudacion = lnStatus_auxiliar;
                                    recaudacion_aux.status = lnStatus_auxiliar;
                                    recaudacion_aux.fecha_hora = ld_fecha_recaudacion;
                                    recaudacion_aux.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                    recaudacion_aux.Save();
                                }
                                else
                                { 
                                    ln_status_recaudacion = recaudacion_aux.status; 
                                }
                            }
                            //
                            Fundraising_PTDM.FUNDRAISING_PT.Sesiones current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(lg_sesion);
                            if (current_sesion != null)
                            {
                                int sesion_status_ant = current_sesion.status;
                                
                                //if (lnStatus_auxiliar == 3)
                                //{ 
                                //    current_sesion.status = lnStatus_auxiliar;
                                //}
                                //else
                                //{ 
                                //    current_sesion.status = lnStatus_auxiliar + 1;
                                //}

                                // gv1 30/12/2015 (Correccion para el cambio de estatus de las sesiones // 
                                switch (ln_status_recaudacion)
                                {
                                    case 1:
                                        current_sesion.status = 2;
                                        break;
                                    case 2:
                                        current_sesion.status = 3;
                                        break;
                                    case 3:
                                        current_sesion.status = 3;
                                        break;
                                    case 4:
                                        current_sesion.status = 1;
                                        break;
                                    case 6:
                                        current_sesion.status = 2;
                                        break;
                                    default:
                                        current_sesion.status = 1;
                                        break;
                                }
                                // gv1 30/12/2015 (Correccion para el cambio de estatus de las sesiones // 

                                current_sesion.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                current_sesion.Save();
                                DevExpress.Xpo.XpoDefault.Session.CommitTransaction();
                                lg_recaudacion = recaudacion_aux.oid;
                                seteo_status_recaudacion();
                            }
                            else
                            {
                                DevExpress.Xpo.XpoDefault.Session.CommitTransaction();
                                seteo_status_recaudacion();
                            }

                            // recarga los datos //
                            reload_data_recaudacion_detalles();

                            // actualiza los salos para depositar...
                            if (distribucion_depositar.Rows.Count > 0)
                            {
                                foreach (DataRow item_distribucion_depositar in distribucion_depositar.Rows)
                                {
                                    Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(1, item_distribucion_depositar.Field<DateTime>("fecha_hora"), item_distribucion_depositar.Field<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>("usuario"), item_distribucion_depositar.Field<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>("forma_pago"));
                                }
                            }
                            distribucion_depositar.Rows.Clear();

                            //((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).bindingSource1.Current).status = lnStatus_auxiliar + 1;
                            //((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).bindingSource1.Current).Save();
                            //DevExpress.Xpo.XpoDefault.Session.CommitTransaction();
                            //
                            switch (Modo_recaudacion)
                            {
                                case 1:
                                    // cambia el estatus de la sesion activa que se esta recaudando //
                                    //((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).bindingSource1.Current).status = lnStatus_auxiliar + 1;
                                    //((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)((Fundraising_PT.Formularios.UI_Sesion_Recauda)Form_padre).bindingSource1.Current).Save();
                                    //DevExpress.Xpo.XpoDefault.Session.CommitTransaction();
                                    MessageBox.Show("Datos Guardados Correctamente...", "Guardar Nueva Recaudación");
                                    break;
                                case 2:
                                    // cambia el estatus de la sesion activa que se esta recaudando //
                                    //((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).sesion)).status = lnStatus_auxiliar + 1;
                                    //((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).sesion)).Save();
                                    //DevExpress.Xpo.XpoDefault.Session.CommitTransaction();
                                    if (current_sesion != null)
                                    {
                                        ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).lookUp_status_sesion.gridLookUpEdit1.EditValue = current_sesion.status;
                                    }
                                    MessageBox.Show("Datos Actualizados Correctamente...", "Actualizar Recaudación Existente");
                                    ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).seteo_status_recaudacion();
                                    break;
                            }
                            this.WindowState = FormWindowState.Normal;
                            this.Close();
                        } // final de try //
                        catch (Exception oerror)
                        {
                            switch (Modo_recaudacion)
                            {
                                case 1:
                                    MessageBox.Show("Ocurrio un ERROR durante el proceso de guardar los datos, se reversara dicho proceso..." + "\n" + "Error: " + oerror.Message, "Guardar Nueva Recaudación");
                                    break;
                                case 2:
                                    MessageBox.Show("Ocurrio un ERROR durante el proceso de actualizar los datos, se reversara dicho proceso..." + "\n" + "Error: " + oerror.Message, "Actualizar Recaudación Existente");
                                    break;
                            }
                            //
                            DevExpress.Xpo.XpoDefault.Session.RollbackTransaction();
                            //
                            Fundraising_PTDM.FUNDRAISING_PT.Sesiones current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(lg_sesion);
                            if (current_sesion != null)
                            {
                                switch (Modo_recaudacion)
                                {
                                    case 1:
                                        current_sesion.status = 1;
                                        break;
                                    case 2:
                                        current_sesion.status = sesion_status_ant;
                                        break;
                                }
                                current_sesion.Save();
                            }
                            //
                            seteo_status_recaudacion();
                            this.WindowState = FormWindowState.Normal;
                            this.Close();
                        }
                    } // final del IF esta seguro de guardar //
                    else
                    {
                        seteo_status_recaudacion();
                    }
                } // final del IF lnStatus_auxiliar //
                else
                {
                    seteo_status_recaudacion();
                }
            } // final del IF ln_totalgeneralrecaudacion mayor a cero //
            else
            {
                switch (Modo_recaudacion)
                {
                    case 1:
                        MessageBox.Show("NO existen datos para guardar...", "Guardar Nueva Recaudación");
                        break;
                    case 2:
                        MessageBox.Show("NO existe ningun monto mayor a cero..." + "\n" + "Si desea eliminar todos los montos, anule la recaudación...", "Actualizar Recaudación Existente");
                        break;
                }
                seteo_status_recaudacion();
            }
        }

        //private void saldos_recaudaciones_depositos(int ln_accion, string lc_fecha_string, Fundraising_PTDM.FUNDRAISING_PT.Usuarios lo_recaudador, Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos lo_forma_pago, decimal ln_monto_recaudado)
        //{
        //    switch (ln_accion)
        //    {
        //        case 1: // guarda el total recaudado de la forma de pago enviada como parametro, en la tabla de saldos de recaudaciones para depositar //
        //            saldos_recauda_dep = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Saldos_Recauda_dep>(CriteriaOperator.Parse(string.Format("fecha_string = '{0}'  and recaudador = '{1}' and forma_pago = '{2}'", lc_fecha_string, lo_recaudador.oid, lo_forma_pago.oid)), new DevExpress.Xpo.SortProperty("fecha_string", DevExpress.Xpo.DB.SortingDirection.Descending));
        //            if (saldos_recauda_dep.Count <= 0)
        //            {
        //                saldos_recauda_dep.Add(new Fundraising_PTDM.FUNDRAISING_PT.Saldos_Recauda_dep(DevExpress.Xpo.XpoDefault.Session));
        //                saldos_recauda_dep[0].fecha_string = lc_fecha_string.Trim();
        //                saldos_recauda_dep[0].recaudador = lo_recaudador;
        //                saldos_recauda_dep[0].forma_pago = lo_forma_pago;
        //                saldos_recauda_dep[0].recaudado = 0;
        //                saldos_recauda_dep[0].status = 1;
        //            }
        //            saldos_recauda_dep[0].recaudado = saldos_recauda_dep[0].recaudado + ln_monto_recaudado;
        //            saldos_recauda_dep[0].depositado = 0;
        //            saldos_recauda_dep[0].Save();
        //            break;
        //        case 2:
        //            saldos_recauda_dep = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Saldos_Recauda_dep>(CriteriaOperator.Parse(string.Format("fecha_string = '{0}'  and recaudador = '{1}' and forma_pago = '{2}'", lc_fecha_string, lo_recaudador.oid, lo_forma_pago.oid)), new DevExpress.Xpo.SortProperty("fecha_string", DevExpress.Xpo.DB.SortingDirection.Descending));
        //            if (saldos_recauda_dep.Count > 0)
        //            {
        //                saldos_recauda_dep.Session.Delete(saldos_recauda_dep);
        //                saldos_recauda_dep.Session.Save(saldos_recauda_dep);
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private void simpleButton_totales_estadisticasgrafica_Click(object sender, EventArgs e)
        {
            //
            this.control_Graficos_distribucion.chartControl_distribucion.Series[0].BindToData(distribucion_grafica, "ltipo", "valor");
            //
            if (this.control_Graficos_distribucion.Visible == false)
            {
                this.control_Graficos_distribucion.Visible = true;
                this.tableLayoutPanel_totales_recaudacion.Visible = false;
                //
                this.simpleButton_totales_estadisticasgrafica.Text = "Totales";
                this.simpleButton_totales_estadisticasgrafica.Image = Fundraising_PT.Properties.Resources.extras;
            }
            else
            {
                this.control_Graficos_distribucion.Visible = false;
                this.tableLayoutPanel_totales_recaudacion.Visible = true;
                //
                this.simpleButton_totales_estadisticasgrafica.Text = "Graficos";
                this.simpleButton_totales_estadisticasgrafica.Image = Fundraising_PT.Properties.Resources.resumen;
            }
        }

        private void seteo_nivel_seguridad()
        {
            switch (Fundraising_PT.Properties.Settings.Default.U_tipo)
            {
                case 1:
                    simpleButton_totales_abre_recaudacion_cerrada.Enabled = true;
                    break;
                case 2:
                    simpleButton_totales_abre_recaudacion_cerrada.Enabled = false;
                    break;
                default:
                    simpleButton_totales_abre_recaudacion_cerrada.Enabled = false;
                    break;
            }
        }

        private void asigna_info_titulo_principal()
        {
            // se cargan variables de texto fijas de la sesion y de la recaudacion //
            string IdSesion = string.Empty;
            string EstatusSesion = string.Empty;
            string FechaRecaudacion = DateTime.Now.ToString();
            string Recaudador = Fundraising_PT.Properties.Settings.Default.U_usuario;
            string Caja = string.Empty;
            string Cajero = string.Empty;
            string FechaAperturaSesion = string.Empty;
            //
            Fundraising_PTDM.FUNDRAISING_PT.Sesiones current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(lg_sesion);
            Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones current_recaudacion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones>(lg_recaudacion);
            //
            if (current_sesion != null)
            {
                IdSesion = current_sesion.id_sesion.ToString().Trim();
                Caja = current_sesion.caja.nombre;
                Cajero = current_sesion.cajero.cajero;
                FechaAperturaSesion = current_sesion.fecha_hora.ToString();
                EstatusSesion = ((Fundraising_PTDM.Enums.EStatus_sesion)current_sesion.status).ToString();
            }
            if (current_recaudacion != null)
            {
                FechaRecaudacion = current_recaudacion.fecha_hora.ToString();
                if (current_recaudacion.usuario != null)
                {
                    Recaudador = current_recaudacion.usuario.usuario;
                }
                else 
                {
                    Recaudador = "[ Sin Asignar ]";
                }
                
            }
            ToolTipItem toolTipTitleItem_sesion = new ToolTipItem();
            toolTipTitleItem_sesion.Text = "Sucursal : " + lc_sucursal + "\n" + "Id Sesión : " + IdSesion + "\n" + "Caja : " + Caja + "\n" + "Cajero : " + Cajero + "\n" + "Apertura : " + FechaAperturaSesion + "\n" + "Estatus : " + EstatusSesion;
            toolTipTitleItem_sesion.Image = Fundraising_PT.Properties.Resources.employee_32x32;
            //
            ToolTipItem toolTipTitleItem_recaudacion = new ToolTipItem();
            toolTipTitleItem_recaudacion.Text = "Recaudador : " + Recaudador + "\n" + "fecha / Hora : " + FechaRecaudacion;
            toolTipTitleItem_recaudacion.Image = Fundraising_PT.Properties.Resources.recauda5;
            //
            SuperToolTip superToolTip_Titulo_principal = new SuperToolTip();
            superToolTip_Titulo_principal.Items.Add(toolTipTitleItem_sesion);
            superToolTip_Titulo_principal.Items.AddSeparator();
            superToolTip_Titulo_principal.Items.Add(toolTipTitleItem_recaudacion);
            //
            Titulo_Principal.SuperTip = superToolTip_Titulo_principal;
        }

        private void seteo_status_recaudacion()
        {
            switch (ln_status_recaudacion)
            {
                case 1:
                    gridView_efectivo_billetes.OptionsBehavior.ReadOnly = false;
                    gridView_efectivo_monedas.OptionsBehavior.ReadOnly = false;
                    gridView_tarjetas.OptionsBehavior.ReadOnly = false;
                    gridView_cheques.OptionsBehavior.ReadOnly = false;
                    gridView_creditos.OptionsBehavior.ReadOnly = false;
                    gridView_tickets.OptionsBehavior.ReadOnly = false;
                    gridView_depositos.OptionsBehavior.ReadOnly = false;
                    //
                    simpleButton_eliminar_tarjetas.Enabled = true;
                    simpleButton_eliminar_cheques1.Enabled = true;
                    simpleButton_eliminar_creditos.Enabled = true;
                    simpleButton_eliminar_tickets.Enabled = true;
                    simpleButton_eliminar_depositos.Enabled = true;
                    //
                    textBox_efectivo_ref1.Enabled = true;
                    textBox_efectivo_ref2.Enabled = true;
                    textBox_ticket_ref1.Enabled = true;
                    textBox_ticket_ref2.Enabled = true;
                    //
                    dateEdit_fecha_hora_recaudacion.Enabled = true;
                    //
                    simpleButton_totales_abre_recaudacion_cerrada.Enabled = false;
                    simpleButton_totales_guardar.Enabled = true;
                    simpleButton_salir.Enabled = true;
                    //
                    label_estatus_recaudacion.Text = "Abierta";
                    label_estatus_recaudacion.Appearance.ForeColor = Color.GreenYellow;
                    label_estatus_recaudacion.LineColor = Color.GreenYellow;
                    break;
                case 2:
                    gridView_efectivo_billetes.OptionsBehavior.ReadOnly = true;
                    gridView_efectivo_monedas.OptionsBehavior.ReadOnly = true;
                    gridView_tarjetas.OptionsBehavior.ReadOnly = true;
                    gridView_cheques.OptionsBehavior.ReadOnly = true;
                    gridView_creditos.OptionsBehavior.ReadOnly = true;
                    gridView_tickets.OptionsBehavior.ReadOnly = true;
                    gridView_depositos.OptionsBehavior.ReadOnly = true;
                    //
                    simpleButton_eliminar_tarjetas.Enabled = false;
                    simpleButton_eliminar_cheques1.Enabled = false;
                    simpleButton_eliminar_creditos.Enabled = false;
                    simpleButton_eliminar_tickets.Enabled = false;
                    simpleButton_eliminar_depositos.Enabled = false;
                    //
                    textBox_efectivo_ref1.Enabled = false;
                    textBox_efectivo_ref2.Enabled = false;
                    textBox_ticket_ref1.Enabled = false;
                    textBox_ticket_ref2.Enabled = false;
                    //
                    dateEdit_fecha_hora_recaudacion.Enabled = false;
                    //
                    simpleButton_totales_abre_recaudacion_cerrada.Enabled = true;
                    simpleButton_totales_guardar.Enabled = false;
                    simpleButton_salir.Enabled = true;
                    //
                    label_estatus_recaudacion.Text = "Cerrada_Normal";
                    label_estatus_recaudacion.Appearance.ForeColor = Color.DeepSkyBlue;
                    label_estatus_recaudacion.LineColor = Color.DeepSkyBlue;
                    break;
                case 3:
                    gridView_efectivo_billetes.OptionsBehavior.ReadOnly = true;
                    gridView_efectivo_monedas.OptionsBehavior.ReadOnly = true;
                    gridView_tarjetas.OptionsBehavior.ReadOnly = true;
                    gridView_cheques.OptionsBehavior.ReadOnly = true;
                    gridView_creditos.OptionsBehavior.ReadOnly = true;
                    gridView_tickets.OptionsBehavior.ReadOnly = true;
                    gridView_depositos.OptionsBehavior.ReadOnly = true;
                    //
                    simpleButton_eliminar_tarjetas.Enabled = false;
                    simpleButton_eliminar_cheques1.Enabled = false;
                    simpleButton_eliminar_creditos.Enabled = false;
                    simpleButton_eliminar_tickets.Enabled = false;
                    simpleButton_eliminar_depositos.Enabled = false;
                    //
                    textBox_efectivo_ref1.Enabled = false;
                    textBox_efectivo_ref2.Enabled = false;
                    textBox_ticket_ref1.Enabled = false;
                    textBox_ticket_ref2.Enabled = false;
                    //
                    dateEdit_fecha_hora_recaudacion.Enabled = false;
                    //
                    simpleButton_totales_abre_recaudacion_cerrada.Enabled = true;
                    simpleButton_totales_guardar.Enabled = false;
                    simpleButton_salir.Enabled = true;
                    //
                    label_estatus_recaudacion.Text = "Cerrada_Ajustada";
                    label_estatus_recaudacion.Appearance.ForeColor = Color.Gold;
                    label_estatus_recaudacion.LineColor = Color.Gold;
                    break;
                case 4:
                    gridView_efectivo_billetes.OptionsBehavior.ReadOnly = true;
                    gridView_efectivo_monedas.OptionsBehavior.ReadOnly = true;
                    gridView_tarjetas.OptionsBehavior.ReadOnly = true;
                    gridView_cheques.OptionsBehavior.ReadOnly = true;
                    gridView_creditos.OptionsBehavior.ReadOnly = true;
                    gridView_tickets.OptionsBehavior.ReadOnly = true;
                    gridView_depositos.OptionsBehavior.ReadOnly = true;
                    //
                    simpleButton_eliminar_tarjetas.Enabled = false;
                    simpleButton_eliminar_cheques1.Enabled = false;
                    simpleButton_eliminar_creditos.Enabled = false;
                    simpleButton_eliminar_tickets.Enabled = false;
                    simpleButton_eliminar_depositos.Enabled = false;
                    //
                    textBox_efectivo_ref1.Enabled = false;
                    textBox_efectivo_ref2.Enabled = false;
                    textBox_ticket_ref1.Enabled = false;
                    textBox_ticket_ref2.Enabled = false;
                    //
                    dateEdit_fecha_hora_recaudacion.Enabled = false;
                    //
                    simpleButton_totales_abre_recaudacion_cerrada.Enabled = false;
                    simpleButton_totales_guardar.Enabled = false;
                    simpleButton_salir.Enabled = true;
                    //
                    label_estatus_recaudacion.Text = "Anulada";
                    label_estatus_recaudacion.Appearance.ForeColor = Color.Red;
                    label_estatus_recaudacion.LineColor = Color.Red;
                    break;
                case 6:
                    gridView_efectivo_billetes.OptionsBehavior.ReadOnly = false;
                    gridView_efectivo_monedas.OptionsBehavior.ReadOnly = false;
                    gridView_tarjetas.OptionsBehavior.ReadOnly = false;
                    gridView_cheques.OptionsBehavior.ReadOnly = false;
                    gridView_creditos.OptionsBehavior.ReadOnly = false;
                    gridView_tickets.OptionsBehavior.ReadOnly = false;
                    gridView_depositos.OptionsBehavior.ReadOnly = false;
                    //
                    simpleButton_eliminar_tarjetas.Enabled = true;
                    simpleButton_eliminar_cheques1.Enabled = true;
                    simpleButton_eliminar_creditos.Enabled = true;
                    simpleButton_eliminar_tickets.Enabled = true;
                    simpleButton_eliminar_depositos.Enabled = true;
                    //
                    textBox_efectivo_ref1.Enabled = true;
                    textBox_efectivo_ref2.Enabled = true;
                    textBox_ticket_ref1.Enabled = true;
                    textBox_ticket_ref2.Enabled = true;
                    //
                    dateEdit_fecha_hora_recaudacion.Enabled = true;
                    //
                    simpleButton_totales_abre_recaudacion_cerrada.Enabled = false;
                    simpleButton_totales_guardar.Enabled = true;
                    simpleButton_salir.Enabled = true;
                    //
                    label_estatus_recaudacion.Text = "En_Proceso";
                    label_estatus_recaudacion.Appearance.ForeColor = Color.LightCyan;
                    label_estatus_recaudacion.LineColor = Color.LightCyan;
                    break;
                default:
                    gridView_efectivo_billetes.OptionsBehavior.ReadOnly = true;
                    gridView_efectivo_monedas.OptionsBehavior.ReadOnly = true;
                    gridView_tarjetas.OptionsBehavior.ReadOnly = true;
                    gridView_cheques.OptionsBehavior.ReadOnly = true;
                    gridView_creditos.OptionsBehavior.ReadOnly = true;
                    gridView_tickets.OptionsBehavior.ReadOnly = true;
                    gridView_depositos.OptionsBehavior.ReadOnly = true;
                    //
                    simpleButton_eliminar_tarjetas.Enabled = false;
                    simpleButton_eliminar_cheques1.Enabled = false;
                    simpleButton_eliminar_creditos.Enabled = false;
                    simpleButton_eliminar_tickets.Enabled = false;
                    simpleButton_eliminar_depositos.Enabled = false;
                    //
                    textBox_efectivo_ref1.Enabled = false;
                    textBox_efectivo_ref2.Enabled = false;
                    textBox_ticket_ref1.Enabled = false;
                    textBox_ticket_ref2.Enabled = false;
                    //
                    dateEdit_fecha_hora_recaudacion.Enabled = false;
                    //
                    simpleButton_totales_abre_recaudacion_cerrada.Enabled = true;
                    simpleButton_totales_guardar.Enabled = false;
                    simpleButton_salir.Enabled = true;
                    //
                    label_estatus_recaudacion.Text = "Total Venta Cerrada Sin Montos";
                    label_estatus_recaudacion.Appearance.ForeColor = Color.DeepSkyBlue;
                    label_estatus_recaudacion.LineColor = Color.DeepSkyBlue;
                    break;
            }
        }

        private void elimina_registros(int ln_tipo)
        {
        }

        private void reload_data_recaudacion_detalles()
        {
            // llena los datos de la coleccion del detalle de la recaudacion filtrado x la recaudacion de la sesion correspondiente //
            CriteriaOperator filtro_recaudacion = (new OperandProperty("oid") == new OperandValue(lg_recaudacion));
            recaudacion.Criteria = filtro_recaudacion;
            recaudacion.Reload();

            // llena los datos de la coleccion del detalle de la recaudacion filtrado x la recaudacion de la sesion correspondiente //
            CriteriaOperator filtro_recaudacion_det_des = (new OperandProperty("recaudacion_det.recaudacion.oid") == new OperandValue(lg_recaudacion));
            CriteriaOperator filtro_recaudacion_det = (new OperandProperty("recaudacion.oid") == new OperandValue(lg_recaudacion));
            recaudacion_det_des.Criteria = filtro_recaudacion_det_des;
            recaudacion_det.Criteria = filtro_recaudacion_det;
            recaudacion_det_des.Reload();
            recaudacion_det.Reload();

            // llena los datos de la coleccion del desglose(efectivo,ticket alimentacion) del detalle de la recaudacion filtrado x el detalle de la recaudacion correspondiente //
            CriteriaOperator filtro_recaudacion_det_des_efectivo = (new OperandProperty("recaudacion_det.recaudacion.oid") == new OperandValue(lg_recaudacion)) & (new OperandProperty("recaudacion_det.forma_pago.tpago") == new OperandValue(1));
            CriteriaOperator filtro_recaudacion_det_des_ticket = (new OperandProperty("recaudacion_det.recaudacion.oid") == new OperandValue(lg_recaudacion)) & (new OperandProperty("recaudacion_det.forma_pago.tpago") == new OperandValue(7));
            recaudacion_det_des_efectivo.Criteria = filtro_recaudacion_det_des_efectivo;
            recaudacion_det_des_ticket.Criteria = filtro_recaudacion_det_des_ticket;
            recaudacion_det_des_efectivo.Reload();
            recaudacion_det_des_ticket.Reload();

            // recarga los datos de las colecciones x formas de pagos //
            recaudacion_det_efectivo.Reload();
            recaudacion_det_tarjeta_debito.Reload();
            recaudacion_det_tarjeta_credito.Reload();
            recaudacion_det_tarjeta_alimentacion.Reload();
            recaudacion_det_cheque.Reload();
            recaudacion_det_credito.Reload();
            recaudacion_det_otrospagos.Reload();
            recaudacion_det_pagosinternos.Reload();
            recaudacion_det_ticketalimentacion.Reload();
            recaudacion_det_consumosinternos.Reload();
            recaudacion_det_prepago.Reload();
            recaudacion_det_deposito.Reload();
            recaudacion_det_retencionimpuesto.Reload();
            recaudacion_det_exoneracionimpuesto.Reload();
            recaudacion_det_islr.Reload();
            recaudacion_det_saldofavor.Reload();
            recaudacion_det_puntoslealtad.Reload();
        }

        public object data_report()
        {
            // se cargan variables de texto fijas de la sesion y de la recaudacion //
            string EstatusSesion = string.Empty;
            string EstatusRecaudacion = label_estatus_recaudacion.Text.Trim();
            DateTime FechaRecaudacion = DateTime.MinValue;
            string Recaudador = string.Empty;
            string Caja = string.Empty;
            string Cajero = string.Empty;
            DateTime FechaAperturaSesion = DateTime.MinValue;
            //
            Fundraising_PTDM.FUNDRAISING_PT.Sesiones current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(lg_sesion);
            Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones current_recaudacion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones>(lg_recaudacion);
            //
            if (current_sesion != null)
            {
                Caja = current_sesion.caja.nombre;
                Cajero = current_sesion.cajero.cajero;
                FechaAperturaSesion = current_sesion.fecha_hora;
                //
                switch (current_sesion.status)
                {
                    case 1:
                        EstatusSesion = "Iniciada";  
                        break;
                    case 2:
                        EstatusSesion = "Recaudación Parcial";  
                        break;
                    case 3:
                        EstatusSesion = "Recaudación Total";  
                        break;
                    case 4:
                        EstatusSesion = "Cerrada";
                        break;
                    case 5:
                        EstatusSesion = "En Proceso";
                        break;
                    default:
                        EstatusSesion = "Ninguno";  
                        break;
                }
            }
            if (current_recaudacion != null)
            {
                FechaRecaudacion = current_recaudacion.fecha_hora;
                Recaudador = current_recaudacion.usuario.usuario;
            }
            // se crea un datatable con la estructura necesaria para vaciar la informacion del reporte.
            DataTable data_report = new DataTable();
            if (data_report.Columns.Count <= 0)
            {
                data_report.Columns.Add("OidFormaPago", typeof(Guid));
                data_report.Columns.Add("TipoPago", typeof(String));
                data_report.Columns.Add("CodFormaPago", typeof(String));
                data_report.Columns.Add("FomaPago", typeof(String));
                data_report.Columns.Add("FechaRecaudacion", typeof(DateTime));
                data_report.Columns.Add("Recaudador", typeof(String));
                data_report.Columns.Add("Caja", typeof(String));
                data_report.Columns.Add("Cajero", typeof(String));
                data_report.Columns.Add("FechaAperturaSesion", typeof(DateTime));
                data_report.Columns.Add("OidDetalle", typeof(Guid));
                data_report.Columns.Add("PuntoBancario", typeof(String));
                data_report.Columns.Add("OidPuntoBancario", typeof(Guid));
                data_report.Columns.Add("ProveedorTA", typeof(String));
                data_report.Columns.Add("Banco", typeof(String));
                data_report.Columns.Add("BancoCuenta", typeof(String));
                data_report.Columns.Add("Ref1", typeof(String));
                data_report.Columns.Add("Ref2", typeof(String));
                data_report.Columns.Add("MontoRecaudado", typeof(Decimal));
                data_report.Columns.Add("MontoRetail", typeof(Decimal));
                data_report.Columns.Add("Denominacion_ticket", typeof(Decimal));
                data_report.Columns.Add("Denominacion_efectivo", typeof(Decimal));
                data_report.Columns.Add("CantidadDenominacion", typeof(int));
                data_report.Columns.Add("OidTotales", typeof(Guid));
                data_report.Columns.Add("MontoFacturadoManual", typeof(Decimal));
                data_report.Columns.Add("MontoFacturadoSistema", typeof(Decimal));
                data_report.Columns.Add("PuntoBancarioFacturado", typeof(String));
                data_report.Columns.Add("MontoFacturadoManualDes", typeof(Decimal));
                data_report.Columns.Add("MontoFacturadoSistemaDes", typeof(Decimal));
                data_report.Columns.Add("StatusRecaudacion", typeof(String));
                data_report.Columns.Add("StatusSesion", typeof(String));
                data_report.PrimaryKey = new DataColumn[] { puntos_bancarios_aux.Columns["OidFormaPago"] };
            }
            data_report.Rows.Clear();
            
            //// Se re-cargan los datos en los collection de los detalles de recaudacion //
            //recaudacion_det.Reload();
            //recaudacion_det_des.Reload();
            
            // se extraen los datos utilizando LinQ. 
            var data_pagos_detalle = from data_pagos in formas_pagos.Cast<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>()
                                     join data_detalle in recaudacion_det
                                     on data_pagos.oid equals data_detalle.forma_pago.oid
                                     into union_detalle
                                     from data_union_detalle in union_detalle.DefaultIfEmpty()
                                     orderby data_pagos.tpago.ToString().Trim() + data_pagos.ttarjeta.ToString().Trim()
                                     select new
                                     {
                                         OidFormaPago = (data_pagos == null ? Guid.Empty : data_pagos.oid),
                                         TipoPago = (data_pagos == null ? String.Empty : data_pagos.tpago.ToString().Trim() + data_pagos.ttarjeta.ToString().Trim()),
                                         CodFormaPago = (data_pagos == null ? String.Empty : data_pagos.codigo),
                                         FomaPago = (data_pagos == null ? String.Empty : data_pagos.nombre),
                                         FechaRecaudacion = FechaRecaudacion,
                                         Recaudador = Recaudador,
                                         Caja = Caja,
                                         Cajero = Cajero,
                                         FechaAperturaSesion = FechaAperturaSesion,
                                         OidDetalle = (data_union_detalle == null || data_union_detalle.oid == null ? Guid.Empty : data_union_detalle.oid),
                                         OidPuntoBancario = (data_union_detalle == null || data_union_detalle.punto_bancario == null ? Guid.Empty : data_union_detalle.punto_bancario.oid),
                                         PuntoBancario = (data_union_detalle == null || data_union_detalle.punto_bancario == null ? String.Empty : data_union_detalle.punto_bancario.descr),
                                         ProveedorTA = (data_union_detalle == null || data_union_detalle.proveedor_ta == null ? String.Empty : data_union_detalle.proveedor_ta.nombre),
                                         Banco = (data_union_detalle == null || data_union_detalle.banco == null ? String.Empty : data_union_detalle.banco.nombre),
                                         BancoCuenta = (data_union_detalle == null || data_union_detalle.banco_cuenta == null ? String.Empty : data_union_detalle.banco_cuenta.descr),
                                         Ref1 = (data_union_detalle == null || data_union_detalle.ref1 == null ? String.Empty : data_union_detalle.ref1),
                                         Ref2 = (data_union_detalle == null || data_union_detalle.ref2 == null ? String.Empty : data_union_detalle.ref2),
                                         MontoRecaudado = (data_union_detalle == null || data_union_detalle.monto_recaudado == null ? Decimal.Zero : data_union_detalle.monto_recaudado),
                                         MontoRetail = (data_union_detalle == null || data_union_detalle.monto_retail == null ? Decimal.Zero : data_union_detalle.monto_retail),
                                         Denominacion_ticket = Decimal.Zero,
                                         Denominacion_efectivo = Decimal.Zero,
                                         CantidadDenominacion = 0
                                     };
            var data_pagos_detalle_des = from data_detalle_des in data_pagos_detalle.ToList()
                                        join detalle_detdes in recaudacion_det_des
                                        on data_detalle_des.OidDetalle equals detalle_detdes.recaudacion_det.oid
                                        into union_detalle_des
                                        from data_union_detalle_des in union_detalle_des.DefaultIfEmpty()
                                        orderby data_detalle_des.TipoPago, data_detalle_des.CodFormaPago
                                        select new
                                        {
                                            OidFormaPago = (data_detalle_des == null ? Guid.Empty : data_detalle_des.OidFormaPago),
                                            TipoPago = (data_detalle_des == null ? String.Empty : data_detalle_des.TipoPago),
                                            CodFormaPago = (data_detalle_des == null ? String.Empty : data_detalle_des.CodFormaPago),
                                            FomaPago = (data_detalle_des == null ? String.Empty : data_detalle_des.FomaPago),
                                            FechaRecaudacion = (data_detalle_des == null ? DateTime.MinValue : data_detalle_des.FechaRecaudacion),
                                            Recaudador = (data_detalle_des == null ? String.Empty : data_detalle_des.Recaudador),
                                            Caja = (data_detalle_des == null ? String.Empty : data_detalle_des.Caja),
                                            Cajero = (data_detalle_des == null ? String.Empty : data_detalle_des.Cajero),
                                            FechaAperturaSesion = (data_detalle_des == null ? DateTime.MinValue : data_detalle_des.FechaAperturaSesion),
                                            OidDetalle = (data_detalle_des == null ? Guid.Empty : data_detalle_des.OidDetalle),
                                            OidPuntoBancario = (data_detalle_des == null ? Guid.Empty : data_detalle_des.OidPuntoBancario),
                                            PuntoBancario = (data_detalle_des == null ? String.Empty : data_detalle_des.PuntoBancario),
                                            ProveedorTA = (data_detalle_des == null ? String.Empty : data_detalle_des.ProveedorTA),
                                            Banco = (data_detalle_des == null ? String.Empty : data_detalle_des.Banco),
                                            BancoCuenta = (data_detalle_des == null ? String.Empty : data_detalle_des.BancoCuenta),
                                            Ref1 = (data_detalle_des == null ? String.Empty : data_detalle_des.Ref1),
                                            Ref2 = (data_detalle_des == null ? String.Empty : data_detalle_des.Ref2),
                                            MontoRecaudado = (data_union_detalle_des == null ? data_detalle_des.MontoRecaudado : data_union_detalle_des.cantidad * (data_union_detalle_des.denominacion + ((data_union_detalle_des.denominacion_moneda == null ? Decimal.Zero : data_union_detalle_des.denominacion_moneda.valor)))),
                                            MontoRetail = (data_detalle_des == null ? Decimal.Zero : data_detalle_des.MontoRetail),
                                            Denominacion_ticket = (data_union_detalle_des == null ? Decimal.Zero : data_union_detalle_des.denominacion),
                                            Denominacion_efectivo = (data_union_detalle_des == null || data_union_detalle_des.denominacion_moneda == null ? Decimal.Zero : data_union_detalle_des.denominacion_moneda.valor),
                                            CantidadDenominacion = (data_union_detalle_des == null ? 0 : data_union_detalle_des.cantidad)
                                        };
            var data_pagos_totales = from pagos_detalle_des in data_pagos_detalle_des.ToList()
                                     join data_totales in totales_ventas
                                     on pagos_detalle_des.OidFormaPago equals data_totales.forma_pago.oid
                                     into union_detalle_totales
                                     from data_union_detalle_totales in union_detalle_totales.DefaultIfEmpty()
                                     orderby pagos_detalle_des.TipoPago, pagos_detalle_des.CodFormaPago
                                     select new 
                                     {
                                         OidFormaPago = (pagos_detalle_des == null ? Guid.Empty : pagos_detalle_des.OidFormaPago),
                                         TipoPago = (pagos_detalle_des == null ? String.Empty : pagos_detalle_des.TipoPago),
                                         CodFormaPago = (pagos_detalle_des == null ? String.Empty : pagos_detalle_des.CodFormaPago),
                                         FomaPago = (pagos_detalle_des == null ? String.Empty : pagos_detalle_des.FomaPago),
                                         FechaRecaudacion = (pagos_detalle_des == null ? DateTime.MinValue : pagos_detalle_des.FechaRecaudacion),
                                         Recaudador = (pagos_detalle_des == null ? String.Empty : pagos_detalle_des.Recaudador),
                                         Caja = (pagos_detalle_des == null ? String.Empty : pagos_detalle_des.Caja),
                                         Cajero = (pagos_detalle_des == null ? String.Empty : pagos_detalle_des.Cajero),
                                         FechaAperturaSesion = (pagos_detalle_des == null ? DateTime.MinValue : pagos_detalle_des.FechaAperturaSesion),
                                         OidDetalle = (pagos_detalle_des == null ? Guid.Empty : pagos_detalle_des.OidDetalle),
                                         OidPuntoBancario = (pagos_detalle_des == null ? Guid.Empty : pagos_detalle_des.OidPuntoBancario),
                                         PuntoBancario = (pagos_detalle_des == null ? String.Empty : pagos_detalle_des.PuntoBancario),
                                         ProveedorTA = (pagos_detalle_des == null ? String.Empty : pagos_detalle_des.ProveedorTA),
                                         Banco = (pagos_detalle_des == null ? String.Empty : pagos_detalle_des.Banco),
                                         BancoCuenta = (pagos_detalle_des == null ? String.Empty : pagos_detalle_des.BancoCuenta),
                                         Ref1 = (pagos_detalle_des == null ? String.Empty : pagos_detalle_des.Ref1),
                                         Ref2 = (pagos_detalle_des == null ? String.Empty : pagos_detalle_des.Ref2),
                                         MontoRecaudado = (pagos_detalle_des == null ? Decimal.Zero : pagos_detalle_des.MontoRecaudado),
                                         MontoRetail = (pagos_detalle_des == null ? Decimal.Zero : pagos_detalle_des.MontoRetail),
                                         Denominacion_ticket = (pagos_detalle_des == null ? Decimal.Zero : pagos_detalle_des.Denominacion_ticket),
                                         Denominacion_efectivo = (pagos_detalle_des == null ? Decimal.Zero : pagos_detalle_des.Denominacion_efectivo),
                                         CantidadDenominacion = (pagos_detalle_des == null ? 0 : pagos_detalle_des.CantidadDenominacion),
                                         OidTotales = (data_union_detalle_totales == null ? Guid.Empty : data_union_detalle_totales.oid),
                                         MontoFacturadoManual = (data_union_detalle_totales == null || data_union_detalle_totales.monto_manual == null ? Decimal.Zero : data_union_detalle_totales.monto_manual),
                                         MontoFacturadoSistema = (data_union_detalle_totales == null || data_union_detalle_totales.monto_retail == null ? Decimal.Zero : data_union_detalle_totales.monto_retail)
                                     };
            var recaudacion_detallada_final = from data_totales_des in data_pagos_totales.ToList()
                                              join data_totales in totales_ventas_des
                                              on data_totales_des.OidTotales.ToString() + data_totales_des.OidPuntoBancario.ToString() equals data_totales.total_venta.oid.ToString() + data_totales.punto_bancario.oid
                                              into union_totales_des
                                              from data_union_totales_des in union_totales_des.DefaultIfEmpty()
                                              orderby data_totales_des.TipoPago + data_totales_des.CodFormaPago
                                              select new
                                              {
                                                  OidFormaPago = (data_totales_des == null ? Guid.Empty : data_totales_des.OidFormaPago),
                                                  TipoPago = (data_totales_des == null ? String.Empty : data_totales_des.TipoPago),
                                                  CodFormaPago = (data_totales_des == null ? String.Empty : data_totales_des.CodFormaPago),
                                                  FomaPago = (data_totales_des == null ? String.Empty : data_totales_des.FomaPago),
                                                  FechaRecaudacion = (data_totales_des == null ? DateTime.MinValue : data_totales_des.FechaRecaudacion),
                                                  Recaudador = (data_totales_des == null ? String.Empty : data_totales_des.Recaudador),
                                                  Caja = (data_totales_des == null ? String.Empty : data_totales_des.Caja),
                                                  Cajero = (data_totales_des == null ? String.Empty : data_totales_des.Cajero),
                                                  FechaAperturaSesion = (data_totales_des == null ? DateTime.MinValue : data_totales_des.FechaAperturaSesion),
                                                  OidDetalle = (data_totales_des == null ? Guid.Empty : data_totales_des.OidDetalle),
                                                  PuntoBancario = (data_totales_des == null ? String.Empty : data_totales_des.PuntoBancario),
                                                  OidPuntoBancario = (data_union_totales_des == null || data_union_totales_des.punto_bancario == null ? data_totales_des.OidPuntoBancario : data_union_totales_des.punto_bancario.oid),
                                                  ProveedorTA = (data_totales_des == null ? String.Empty : data_totales_des.ProveedorTA),
                                                  Banco = (data_totales_des == null ? String.Empty : data_totales_des.Banco),
                                                  BancoCuenta = (data_totales_des == null ? String.Empty : data_totales_des.BancoCuenta),
                                                  Ref1 = (data_totales_des == null ? String.Empty : data_totales_des.Ref1),
                                                  Ref2 = (data_totales_des == null ? String.Empty : data_totales_des.Ref2),
                                                  MontoRecaudado = (data_totales_des == null ? Decimal.Zero : data_totales_des.MontoRecaudado),
                                                  MontoRetail = (data_totales_des == null ? Decimal.Zero : data_totales_des.MontoRetail),
                                                  Denominacion_ticket = (data_totales_des == null ? Decimal.Zero : data_totales_des.Denominacion_ticket),
                                                  Denominacion_efectivo = (data_totales_des == null ? Decimal.Zero : data_totales_des.Denominacion_efectivo),
                                                  CantidadDenominacion = (data_totales_des == null ? 0 : data_totales_des.CantidadDenominacion),
                                                  OidTotales = (data_totales_des == null ? Guid.Empty : data_totales_des.OidTotales),
                                                  MontoFacturadoManual = (data_union_totales_des == null ? data_totales_des.MontoFacturadoManual : (data_union_totales_des.monto_manual_des == null ? data_totales_des.MontoFacturadoManual : data_union_totales_des.monto_manual_des)),
                                                  MontoFacturadoSistema = (data_union_totales_des == null ? data_totales_des.MontoFacturadoSistema : (data_union_totales_des.monto_retail_des == null ? data_totales_des.MontoFacturadoSistema : data_union_totales_des.monto_retail_des)),
                                                  PuntoBancarioFacturado = (data_union_totales_des == null || data_union_totales_des.punto_bancario == null ? String.Empty : data_union_totales_des.punto_bancario.descr),
                                                  MontoFacturadoManualDes = (data_union_totales_des == null || data_union_totales_des.monto_manual_des == null ? Decimal.Zero : data_union_totales_des.monto_manual_des),
                                                  MontoFacturadoSistemaDes = (data_union_totales_des == null || data_union_totales_des.monto_retail_des == null ? Decimal.Zero : data_union_totales_des.monto_retail_des),
                                                  StatusRecaudacion = EstatusRecaudacion,
                                                  StatusSesion = EstatusSesion
                                              };
            // fin de los LinQ's.            
            //
            // recorre el resultado del LinQ para llenar el datatable y colocar el montofacturadomanual a razon de 1 por forma de pago. 
            Guid oidformapago = Guid.Empty;
            foreach (var row_recaudacion_detallada_final in recaudacion_detallada_final)
            {
                data_report.Rows.Add(
                row_recaudacion_detallada_final.OidFormaPago,
                row_recaudacion_detallada_final.TipoPago,
                row_recaudacion_detallada_final.CodFormaPago,
                row_recaudacion_detallada_final.FomaPago,
                row_recaudacion_detallada_final.FechaRecaudacion,
                row_recaudacion_detallada_final.Recaudador,
                row_recaudacion_detallada_final.Caja,
                row_recaudacion_detallada_final.Cajero,
                row_recaudacion_detallada_final.FechaAperturaSesion,
                row_recaudacion_detallada_final.OidDetalle,
                row_recaudacion_detallada_final.PuntoBancario,
                row_recaudacion_detallada_final.OidPuntoBancario,
                row_recaudacion_detallada_final.ProveedorTA,
                row_recaudacion_detallada_final.Banco,
                row_recaudacion_detallada_final.BancoCuenta,
                row_recaudacion_detallada_final.Ref1,
                row_recaudacion_detallada_final.Ref2,
                row_recaudacion_detallada_final.MontoRecaudado,
                row_recaudacion_detallada_final.MontoRetail,
                row_recaudacion_detallada_final.Denominacion_ticket,
                row_recaudacion_detallada_final.Denominacion_efectivo,
                row_recaudacion_detallada_final.CantidadDenominacion,
                row_recaudacion_detallada_final.OidTotales,
                row_recaudacion_detallada_final.MontoFacturadoManual,
                row_recaudacion_detallada_final.MontoFacturadoSistema,
                row_recaudacion_detallada_final.PuntoBancarioFacturado,
                row_recaudacion_detallada_final.MontoFacturadoManualDes,
                row_recaudacion_detallada_final.MontoFacturadoSistemaDes,
                row_recaudacion_detallada_final.StatusRecaudacion,
                row_recaudacion_detallada_final.StatusSesion);
                //
                if (oidformapago != row_recaudacion_detallada_final.OidFormaPago)
                {
                    oidformapago = row_recaudacion_detallada_final.OidFormaPago;
                }
                else
                {
                    data_report.Rows[data_report.Rows.Count - 1]["MontoFacturadoManual"] = 0;
                }
                
                //if (Fundraising_PT.Properties.Settings.Default.U_tipo != 1 & Fundraising_PT.Properties.Settings.Default.U_tipo != 2)
                if (current_recaudacion != null)
                {
                    if (current_recaudacion.status == 1 || current_recaudacion.status_tv == 1)
                    {
                        data_report.Rows[data_report.Rows.Count - 1]["MontoFacturadoManual"] = 0;
                        data_report.Rows[data_report.Rows.Count - 1]["MontoFacturadoSistema"] = 0;
                        data_report.Rows[data_report.Rows.Count - 1]["MontoFacturadoManualDes"] = 0;
                        data_report.Rows[data_report.Rows.Count - 1]["MontoFacturadoSistemaDes"] = 0;
                    }
                }
                else
                {
                    data_report.Rows[data_report.Rows.Count - 1]["MontoFacturadoManual"] = 0;
                    data_report.Rows[data_report.Rows.Count - 1]["MontoFacturadoSistema"] = 0;
                    data_report.Rows[data_report.Rows.Count - 1]["MontoFacturadoManualDes"] = 0;
                    data_report.Rows[data_report.Rows.Count - 1]["MontoFacturadoSistemaDes"] = 0;
                }
            }
            //
            return data_report;
        }

        //private void labelControl_totales_Resize(object sender, EventArgs e)
        //{
        //    decimal aux_font_with = (((LabelControl)sender).Size.Width * 9) / 90;
        //    if (((LabelControl)sender).Name.Substring(20, 1) == "1" | ((LabelControl)sender).Name.Substring(20, 1) == "3" | ((LabelControl)sender).Name.Substring(20, 1) == "4")
        //    {
        //        aux_font_with = (((LabelControl)sender).Size.Width * 9) / 104;
        //    }
        //    if (((LabelControl)sender).Name.Substring(20, 1) == "5")
        //    {
        //        aux_font_with = (((LabelControl)sender).Size.Width * 9) / 93;
        //    }
        //    //
        //    if (((LabelControl)sender).Name == "labelControl_matriz_01_1" | ((LabelControl)sender).Name == "labelControl_matriz_01_5")
        //    {
        //        aux_font_with = (((LabelControl)sender).Size.Width * 10) / 194;
        //    }
        //    if (((LabelControl)sender).Name == "labelControl_matriz_012345_7")
        //    {
        //        aux_font_with = (((LabelControl)sender).Size.Width * 12) / 600;
        //    }
        //    if (((LabelControl)sender).Name == "labelControl_matriz_4_0" | ((LabelControl)sender).Name == "labelControl_matriz_4_4")
        //    {
        //        aux_font_with = ((decimal)((LabelControl)sender).Size.Width * (decimal)7.75) / 104;
        //    }
        //    if (((LabelControl)sender).Name == "labelControl_matriz_4_1")
        //    {
        //        aux_font_with = ((decimal)((LabelControl)sender).Size.Width * (decimal)6.75) / 104;
        //    }
        //    if (((LabelControl)sender).Name == "labelControl_matriz_2_3" | ((LabelControl)sender).Name == "labelControl_matriz_2_4")
        //    {
        //        aux_font_with = ((decimal)((LabelControl)sender).Size.Width * (decimal)6.75) / 90;
        //    }
           
        //    //float x = 3.5F;
        //    ((LabelControl)sender).Font = new System.Drawing.Font(((LabelControl)sender).Font.FontFamily, (float)aux_font_with);
        //}
    }
}