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
using Microsoft.Data.ConnectionUI;
using System.Data.SqlClient;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Mantenimientos : DevExpress.XtraEditors.XtraForm
    {
        // declaracion de objetos publicos //
        public DevExpress.XtraBars.BarButtonItem OpcionMenu;
        public DevExpress.XtraBars.BarHeaderItem HeaderMenu;
        public object ObjetoExtra;
        public string lHeader_ant = "";
        public string lAccion = "Mantenimiento (TABLAS/SALDOS)";

        // variables para el filtro de los datos del reporte //
        public string lfiltro_saldos_recauda = "";
        public string lfiltro_saldos_deposit = "";
        public string lfiltro_fp = "";
        public string lfecha_desde = "";
        public string lfecha_hasta = "";
        public string lstatus = "";
        public string cadena_seleccion_tablas = "";
        public string cadena_seleccion_saldos = "";
        public int ltipo_forma_pago = 0;
        public Guid loid_recaudador = Guid.Empty;
        public int lnstatus = 0;
        public Guid loid_recaudacion = Guid.Empty;
        public Guid loid_deposito = Guid.Empty;
        public Guid loid_forma_pago = Guid.Empty;
        //
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Saldos_Recauda_dep> saldos_recauda_dep;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos> formas_pagos;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> usuarios;
        //
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones> recaudaciones;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudacion_det;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios> depositos_bancarios;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios_Det> depositos_bancarios_det;
        //
        Fundraising_PT.Clases.funciones_varias obj_funciones_varias = new Clases.funciones_varias();
        //
        DataConnectionDialog dc_import = new DataConnectionDialog();
        DevExpress.Xpo.Session sesion_import_origen = new Session();

        public UI_Mantenimientos(DevExpress.XtraBars.BarButtonItem opcionMenu, DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Modulo de Mantenimiento de (TABLAS/SALDOS)...");
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
            saldos_recauda_dep = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Saldos_Recauda_dep>(DevExpress.Xpo.XpoDefault.Session, filtro_saldos_recauda_dep, new DevExpress.Xpo.SortProperty("forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            saldos_recauda_dep.Sorting = orden_saldos_recauda_dep;
            formas_pagos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            formas_pagos.Sorting = orden_formas_pagos;
            usuarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, filtro_status, new DevExpress.Xpo.SortProperty("usuario", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            recaudaciones = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones>(DevExpress.Xpo.XpoDefault.Session, filtro_saldos_recauda_dep, new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Ascending));
            depositos_bancarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios>(DevExpress.Xpo.XpoDefault.Session, filtro_saldos_recauda_dep, new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //
            bindingSource_recaudadores.DataSource = usuarios;
            bindingSource_formas_pagos.DataSource = formas_pagos;
        }

        private void UI_Mantenimientos_Load(object sender, EventArgs e)
        {
            //
            dateTime_fecha_desde.EditValue = DateTime.Now;
            dateTime_fecha_hasta.EditValue = DateTime.Now;
            //
            lookUpEdit_tablas.Properties.DataSource = obj_funciones_varias.tablas_sistema();
            lookUpEdit_tablas.Properties.DisplayMember = "primary_table";
            lookUpEdit_tablas.Properties.ValueMember = "primary_table";
            //
            lookUpEdit_saldos.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue("EMantenimiento_saldos");
            lookUpEdit_saldos.Properties.DisplayMember = "Descripcion";
            lookUpEdit_saldos.Properties.ValueMember = "Valor";
            //
            lookUpEdit_status.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue("EStatus");
            lookUpEdit_status.Properties.DisplayMember = "Descripcion";
            lookUpEdit_status.Properties.ValueMember = "Valor";
            //
            lookUpEdit_tipoformapago.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue("EPago");
            lookUpEdit_tipoformapago.Properties.DisplayMember = "Descripcion";
            lookUpEdit_tipoformapago.Properties.ValueMember = "Valor";
            //
            checkButton_importar_datos.Click += checkButton_importar_datos_Click;
            checkEdit_todosfecha.CheckedChanged += new EventHandler(checkEdit_todosfecha_CheckedChanged);
            checkEdit_todosrecaudador.CheckedChanged += new EventHandler(checkEdit_todosrecaudador_CheckedChanged);
            checkEdit_todostiposfp.CheckedChanged += new EventHandler(checkEdit_todostiposfp_CheckedChanged);
            checkEdit_todosdescrpf.CheckedChanged += new EventHandler(checkEdit_todosdescrpf_CheckedChanged);
            checkEdit_todosstatus.CheckedChanged += new EventHandler(checkEdit_todosstatus_CheckedChanged);
            checkEdit_replace_codigo_sucursal_1.CheckedChanged += checkEdit_replace_codigo_sucursal_1_CheckedChanged;
            lookUpEdit_tipoformapago.EditValueChanged += new EventHandler(lookUpEdit_tipoformapago_EditValueChanged);
            //
            simpleButton_mantenimiento.Click += new EventHandler(simpleButton_mantenimiento_Click);
            simpleButton_salir.Click += new EventHandler(simpleButton_salir_Click);
            //
            setea_checks();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void checkButton_importar_datos_Click(object sender, EventArgs e)
        {
            if (checkButton_importar_datos.Checked == false)
            {
                DataSource.AddStandardDataSources(dc_import);
                //
                dc_import.SelectedDataSource = DataSource.SqlDataSource;
                dc_import.SelectedDataProvider = DataProvider.SqlDataProvider;
                //
                dc_import.ConnectionString = Fundraising_PTDM.MyConnection.GetConnectionString();
                //
                if (DataConnectionDialog.Show(dc_import) == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        sesion_import_origen.Connect(XpoDefault.GetDataLayer(dc_import.ConnectionString, DevExpress.Xpo.DB.AutoCreateOption.None));
                        if (sesion_import_origen.IsConnected)
                        {
                            MessageBox.Show("Se ejecuto la conexion a la base de datos origen satisfactoriamente..." + Environment.NewLine + "Seleccione las tablas desde las cuales desea importar sus datos...", "Importar Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //
                            modo_controles(1);
                        }
                        else
                        {
                            MessageBox.Show("NO se pudo hacer la conexion a la base de datos origen..." + Environment.NewLine + "Favor intente de nuevo con otros parametros de conexion...", "Importar Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            modo_controles(2);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("NO se pudo hacer la conexion a la base de datos origen..." + Environment.NewLine + "Favor intente de nuevo con otros parametros de conexion...", "Importar Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        modo_controles(2);
                    }
                }
                else
                {
                    modo_controles(2);
                }
            }
            else
            {
                modo_controles(2);
            }
        }

        void checkEdit_replace_codigo_sucursal_1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit_replace_codigo_sucursal_1.CheckState == CheckState.Checked)
            {
                MessageBox.Show("ADVERTENCIA..." + Environment.NewLine + " Una ves realizada esta operación es irreversible, todos los movimientos y demas datos se unificaran en una sola sucursal.", "Reemplazar códigos de sucursal", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void simpleButton_mantenimiento_Click(object sender, EventArgs e)
        {
            //
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            //
            if (checkButton_importar_datos.Checked == true)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Limpiando todas las (TABLAS) de la (BASE DE DATOS) destino...");
                if (obj_funciones_varias.limpiar_tablas_sistema())
                {
                    //
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Datos...");
                    //
                    cadena_seleccion_tablas = string.Empty;
                    if (lookUpEdit_tablas.EditValue.ToString().Trim() != string.Empty)
                    {
                        System.Text.StringBuilder StringBuilder_tablas = new StringBuilder(lookUpEdit_tablas.EditValue.ToString().Trim());
                        for (int x = 0; x < StringBuilder_tablas.Length; x++)
                        {
                            if (StringBuilder_tablas[x] != ',')
                            {
                                if (StringBuilder_tablas[x] != ' ')
                                {
                                    cadena_seleccion_tablas = cadena_seleccion_tablas + StringBuilder_tablas[x];
                                }
                            }
                            else
                            {
                                importar_tablas(cadena_seleccion_tablas);
                                cadena_seleccion_tablas = string.Empty;
                            }
                        }
                        if (cadena_seleccion_tablas != string.Empty)
                        {
                            importar_tablas(cadena_seleccion_tablas);
                            cadena_seleccion_tablas = string.Empty;
                        }
                    }
                    else
                    {
                        MessageBox.Show("NO se ha seleccionado ninguna tabla de la base de datos origen..." + Environment.NewLine + "Favor seleccione al menos una tabla para ejecutar la transferencia de datos...", "Importar Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("NO se pudo limpiar las (TABLAS) de la (BASE DE DATOS) destino..." + Environment.NewLine + "Se Cancelara la transferencia de datos...", "Importar Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                //
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Ejecutando Mantenimiento...");
                //
                if (checkEdit_replace_codigo_sucursal.CheckState == CheckState.Checked)
                {
                    Fundraising_PTDM.data_integrity.check_data_integrity(1, "", Fundraising_PT.Properties.Settings.Default.sucursal, null);
                }
                //
                if (checkEdit_replace_codigo_sucursal_1.CheckState == CheckState.Checked)
                {
                    Fundraising_PTDM.data_integrity.check_data_integrity(2, "", Fundraising_PT.Properties.Settings.Default.sucursal, null);
                }
                //
                if (checkEdit_replace_status.CheckState == CheckState.Checked)
                {
                    Fundraising_PTDM.data_integrity.check_data_integrity(3, "", Fundraising_PT.Properties.Settings.Default.sucursal, null);
                }
                //
                if (checkEdit_correct_status_sesion_recaudacion_deposito.CheckState == CheckState.Checked)
                {
                    Fundraising_PTDM.data_integrity.check_data_integrity(4, "", Fundraising_PT.Properties.Settings.Default.sucursal, null);
                }

                if (checkEdit_close_automatic_sesion.CheckState == CheckState.Checked)
                {
                    string lconvert_fechahora_sqlserver = "SUBSTRING(CONVERT(varchar(10), fecha_hora, 103), 7, 4)+SUBSTRING(CONVERT(varchar(10), fecha_hora, 103), 4, 2)+SUBSTRING(CONVERT(varchar(10), fecha_hora, 103), 1, 2)";
                    if (this.checkEdit_todosfecha.CheckState == CheckState.Unchecked)
                    {
                        lfecha_desde = ((DateTime)dateTime_fecha_desde.EditValue).Date.ToShortDateString();
                        lfecha_hasta = ((DateTime)dateTime_fecha_hasta.EditValue).Date.ToShortDateString();
                        string lfecha_desde11 = lfecha_desde.Substring(6, 4) + lfecha_desde.Substring(3, 2) + lfecha_desde.Substring(0, 2);
                        string lfecha_hasta11 = lfecha_hasta.Substring(6, 4) + lfecha_hasta.Substring(3, 2) + lfecha_hasta.Substring(0, 2);
                        var sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("update sesiones set status = 4 where sucursal = {0} and status = 3 and {1} >= {2} and {1} <= {3}", Fundraising_PT.Properties.Settings.Default.sucursal, lconvert_fechahora_sqlserver, lfecha_desde11, lfecha_hasta11));
                    }
                    else
                    {
                        var sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("update sesiones set status = 4 where sucursal = {0} and status = 3", Fundraising_PT.Properties.Settings.Default.sucursal));
                    }
                }
                //
                recaudaciones.Reload();
                depositos_bancarios.Reload();
                //
                cadena_seleccion_saldos = string.Empty;
                if (lookUpEdit_saldos.EditValue.ToString().Trim() != string.Empty)
                {
                    System.Text.StringBuilder StringBuilder_saldos = new StringBuilder(lookUpEdit_saldos.EditValue.ToString().Trim());
                    for (int x = 0; x < StringBuilder_saldos.Length; x++)
                    {
                        if (StringBuilder_saldos[x] != ',')
                        {
                            if (StringBuilder_saldos[x] != ' ')
                            {
                                cadena_seleccion_saldos = cadena_seleccion_saldos + StringBuilder_saldos[x];
                            }
                        }
                        else
                        {
                            mantenimiento_saldos(cadena_seleccion_saldos);
                            cadena_seleccion_saldos = string.Empty;
                        }
                    }
                    if (cadena_seleccion_saldos != string.Empty)
                    {
                        mantenimiento_saldos(cadena_seleccion_saldos);
                        cadena_seleccion_saldos = string.Empty;
                    }
                }
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
            if (checkButton_importar_datos.Checked == true)
            {
                MessageBox.Show("Transferencia de Datos culmino satisfactoriamente..." + Environment.NewLine + "Debe salir y volver a entrar al sistema para que se refejen las transferencias de datos...");
                ((DevExpress.XtraBars.Ribbon.RibbonForm)ObjetoExtra).Close();
            }
            else
            {
                MessageBox.Show("Mantenimiento culmino satisfactoriamente...");
            }
            //
        }

        private void importar_tablas(string lc_tabla)
        {
            int xi = 0;
            switch (lc_tabla)
            {
                case "Sucursales":
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Sucursales...");
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales> sucursal_origen = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(sesion_import_origen);
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales> sucursal_destino = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session);
                    //
                    //var sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("delete from {0}", lc_tabla));
                    xi = 0;
                    foreach (Fundraising_PTDM.FUNDRAISING_PT.Sucursales item_origen in sucursal_origen)
                    {
                        sucursal_destino.Add(new Fundraising_PTDM.FUNDRAISING_PT.Sucursales(DevExpress.Xpo.XpoDefault.Session));
                        //
                        sucursal_destino[xi].oid = item_origen.oid;
                        sucursal_destino[xi].codigo = item_origen.codigo;
                        sucursal_destino[xi].nombre = item_origen.nombre;
                        sucursal_destino[xi].status = item_origen.status;
                        sucursal_destino[xi].select = item_origen.select;
                        //
                        try
                        {
                            sucursal_destino[xi].Save();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No se pudo añadir la sucursal : " + item_origen.codigo);
                        }
                    }
                    sucursal_origen.Dispose();
                    sucursal_destino.Dispose();
                    break;
                case "Usuarios":
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Usuarios...");
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> usuario_origen = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(sesion_import_origen);
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> usuario_destino = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session);
                    //
                    //sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("delete from {0}", lc_tabla));
                    xi = 0;
                    foreach (Fundraising_PTDM.FUNDRAISING_PT.Usuarios item_origen in usuario_origen)
                    {
                        usuario_destino.Add(new Fundraising_PTDM.FUNDRAISING_PT.Usuarios(DevExpress.Xpo.XpoDefault.Session));
                        //
                        usuario_destino[xi].oid = item_origen.oid;
                        usuario_destino[xi].login = item_origen.login;
                        usuario_destino[xi].clave = item_origen.clave;
                        usuario_destino[xi].tipo = item_origen.tipo;
                        usuario_destino[xi].usuario = item_origen.usuario;
                        usuario_destino[xi].sys = item_origen.sys;
                        usuario_destino[xi].status = item_origen.status;
                        usuario_destino[xi].sucursal = item_origen.sucursal;
                        //
                        try
                        {
                            usuario_destino[xi].Save();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No se pudo añadir el usuario : " + item_origen.login);
                        }
                    }
                    usuario_origen.Dispose();
                    usuario_destino.Dispose();
                    break;
                case "Bancos":
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Bancos...");
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos> bancos_origen = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos>(sesion_import_origen);
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos> bancos_destino = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos>(DevExpress.Xpo.XpoDefault.Session);
                    //
                    //sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("delete from {0}", lc_tabla));
                    xi = 0;
                    foreach (Fundraising_PTDM.FUNDRAISING_PT.Bancos item_origen in bancos_origen)
                    {
                        bancos_destino.Add(new Fundraising_PTDM.FUNDRAISING_PT.Bancos(DevExpress.Xpo.XpoDefault.Session));
                        //
                        bancos_destino[xi].oid = item_origen.oid;
                        bancos_destino[xi].codigo = item_origen.codigo;
                        bancos_destino[xi].nombre = item_origen.nombre;
                        bancos_destino[xi].status = item_origen.status;
                        bancos_destino[xi].sucursal = item_origen.sucursal;
                        bancos_destino[xi].td_tasa_mb = item_origen.td_tasa_mb;
                        bancos_destino[xi].td_tasa_ob = item_origen.td_tasa_ob;
                        bancos_destino[xi].tc_tasa_mb = item_origen.tc_tasa_mb;
                        bancos_destino[xi].tc_tasa_ob = item_origen.tc_tasa_ob;
                        bancos_destino[xi].tc_tasa_islr = item_origen.tc_tasa_islr;
                        //
                        try
                        {
                            bancos_destino[xi].Save();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No se pudo añadir el banco : " + item_origen.codigo);
                        }
                    }
                    bancos_origen.Dispose();
                    bancos_destino.Dispose();
                    break;
                case "Cajas":
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Cajas...");
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajas> cajas_origen = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajas>(sesion_import_origen);
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajas> cajas_destino = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajas>(DevExpress.Xpo.XpoDefault.Session);
                    //
                    //sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("delete from {0}", lc_tabla));
                    xi = 0;
                    foreach (Fundraising_PTDM.FUNDRAISING_PT.Cajas item_origen in cajas_origen)
                    {
                        cajas_destino.Add(new Fundraising_PTDM.FUNDRAISING_PT.Cajas(DevExpress.Xpo.XpoDefault.Session));
                        //
                        cajas_destino[xi].oid = item_origen.oid;
                        cajas_destino[xi].codigo = item_origen.codigo;
                        cajas_destino[xi].nombre = item_origen.nombre;
                        cajas_destino[xi].status = item_origen.status;
                        cajas_destino[xi].sucursal = item_origen.sucursal;
                        //
                        try
                        {
                            cajas_destino[xi].Save();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No se pudo añadir la caja : " + item_origen.codigo);
                        }
                    }
                    cajas_origen.Dispose();
                    cajas_destino.Dispose();
                    break;
                case "Cajeros":
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Cajeros...");
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajeros> cajeros_origen = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajeros>(sesion_import_origen);
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajeros> cajeros_destino = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajeros>(DevExpress.Xpo.XpoDefault.Session);
                    //
                    //sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("delete from {0}", lc_tabla));
                    xi = 0;
                    foreach (Fundraising_PTDM.FUNDRAISING_PT.Cajeros item_origen in cajeros_origen)
                    {
                        cajeros_destino.Add(new Fundraising_PTDM.FUNDRAISING_PT.Cajeros(DevExpress.Xpo.XpoDefault.Session));
                        //
                        cajeros_destino[xi].oid = item_origen.oid;
                        cajeros_destino[xi].codigo = item_origen.codigo;
                        cajeros_destino[xi].cajero = item_origen.cajero;
                        cajeros_destino[xi].status = item_origen.status;
                        cajeros_destino[xi].sucursal = item_origen.sucursal;
                        //
                        try
                        {
                            cajeros_destino[xi].Save();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No se pudo añadir el cajero : " + item_origen.codigo);
                        }
                    }
                    cajeros_origen.Dispose();
                    cajeros_destino.Dispose();
                    break;
                case "Denominacion_Monedas":
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Denominacion de Monedas...");
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas> denominacion_origen = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas>(sesion_import_origen);
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas> denominacion_destino = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas>(DevExpress.Xpo.XpoDefault.Session);
                    //
                    //sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("delete from {0}", lc_tabla));
                    xi = 0;
                    foreach (Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas item_origen in denominacion_origen)
                    {
                        denominacion_destino.Add(new Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas(DevExpress.Xpo.XpoDefault.Session));
                        //
                        denominacion_destino[xi].oid = item_origen.oid;
                        denominacion_destino[xi].codigo = item_origen.codigo;
                        denominacion_destino[xi].tipo = item_origen.tipo;
                        denominacion_destino[xi].valor = item_origen.valor;
                        denominacion_destino[xi].status = item_origen.status;
                        denominacion_destino[xi].sucursal = item_origen.sucursal;
                        //
                        try
                        {
                            denominacion_destino[xi].Save();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No se pudo añadir la denominacion de moneda : " + item_origen.codigo);
                        }
                    }
                    denominacion_origen.Dispose();
                    denominacion_destino.Dispose();
                    break;
                case "Proveedores_TA":
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Proveedores TA...");
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA> proveedoresta_origen = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA>(sesion_import_origen);
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA> proveedoresta_destino = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA>(DevExpress.Xpo.XpoDefault.Session);
                    //
                    //sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("delete from {0}", lc_tabla));
                    xi = 0;
                    foreach (Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA item_origen in proveedoresta_origen)
                    {
                        proveedoresta_destino.Add(new Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA(DevExpress.Xpo.XpoDefault.Session));
                        //
                        proveedoresta_destino[xi].oid = item_origen.oid;
                        proveedoresta_destino[xi].codigo = item_origen.codigo;
                        proveedoresta_destino[xi].nombre = item_origen.nombre;
                        proveedoresta_destino[xi].tasa = item_origen.tasa;
                        proveedoresta_destino[xi].serv_adi = item_origen.serv_adi;
                        proveedoresta_destino[xi].material = item_origen.material;
                        proveedoresta_destino[xi].impuesto = item_origen.impuesto;
                        proveedoresta_destino[xi].status = item_origen.status;
                        proveedoresta_destino[xi].sucursal = item_origen.sucursal;
                        //
                        try
                        {
                            proveedoresta_destino[xi].Save();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No se pudo añadir el proveedor TA : " + item_origen.codigo);
                        }
                    }
                    proveedoresta_origen.Dispose();
                    proveedoresta_destino.Dispose();
                    break;
                case "Responsable_depositos":
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Responsables Depositos...");
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos> responsable_dep_origen = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos>(sesion_import_origen);
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos> responsable_dep_destino = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos>(DevExpress.Xpo.XpoDefault.Session);
                    //
                    //sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("delete from {0}", lc_tabla));
                    xi = 0;
                    foreach (Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos item_origen in responsable_dep_origen)
                    {
                        responsable_dep_destino.Add(new Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos(DevExpress.Xpo.XpoDefault.Session));
                        //
                        responsable_dep_destino[xi].oid = item_origen.oid;
                        responsable_dep_destino[xi].codigo = item_origen.codigo;
                        responsable_dep_destino[xi].nombre = item_origen.nombre;
                        responsable_dep_destino[xi].status = item_origen.status;
                        responsable_dep_destino[xi].sucursal = item_origen.sucursal;
                        //
                        try
                        {
                            responsable_dep_destino[xi].Save();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No se pudo añadir el responsable de depositos : " + item_origen.codigo);
                        }
                    }
                    responsable_dep_origen.Dispose();
                    responsable_dep_destino.Dispose();
                    break;
                case "Bancos_Cuentas":
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Cuentas Bancarias...");
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas> bancos_cuentas_origen = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas>(sesion_import_origen);
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas> bancos_cuentas_destino = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas>(DevExpress.Xpo.XpoDefault.Session);
                    //
                    //sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("delete from {0}", lc_tabla));
                    xi = 0;
                    foreach (Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas item_origen in bancos_cuentas_origen)
                    {
                        bancos_cuentas_destino.Add(new Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas(DevExpress.Xpo.XpoDefault.Session));
                        //
                        bancos_cuentas_destino[xi].oid = item_origen.oid;
                        bancos_cuentas_destino[xi].banco = (item_origen.banco == null ? null : DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Bancos>(item_origen.banco.oid));
                        bancos_cuentas_destino[xi].codigo_cuenta = item_origen.codigo_cuenta;
                        bancos_cuentas_destino[xi].descr = item_origen.descr;
                        bancos_cuentas_destino[xi].status = item_origen.status;
                        bancos_cuentas_destino[xi].sucursal = item_origen.sucursal;
                        //
                        try
                        {
                            bancos_cuentas_destino[xi].Save();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No se pudo añadir la cuenta bancaria : " + item_origen.codigo_cuenta);
                        }
                    }
                    bancos_cuentas_origen.Dispose();
                    bancos_cuentas_destino.Dispose();
                    break;
                case "Formas_Pagos":
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Formas de Pago...");
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos> formapago_origen = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(sesion_import_origen);
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos> formapago_destino = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(DevExpress.Xpo.XpoDefault.Session);
                    //
                    //sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("delete from {0}", lc_tabla));
                    xi = 0;
                    foreach (Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos item_origen in formapago_origen)
                    {
                        formapago_destino.Add(new Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos(DevExpress.Xpo.XpoDefault.Session));
                        //
                        formapago_destino[xi].oid = item_origen.oid;
                        formapago_destino[xi].codigo = item_origen.codigo;
                        formapago_destino[xi].nombre = item_origen.nombre;
                        formapago_destino[xi].tpago = item_origen.tpago;
                        formapago_destino[xi].ttarjeta = item_origen.ttarjeta;
                        formapago_destino[xi].proveedor_ta = (item_origen.proveedor_ta == null ? null : DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA>(item_origen.proveedor_ta.oid));
                        formapago_destino[xi].status = item_origen.status;
                        formapago_destino[xi].sucursal = item_origen.sucursal;
                        //
                        try
                        {
                            formapago_destino[xi].Save();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No se pudo añadir la forma de pago : " + item_origen.codigo);
                        }
                    }
                    formapago_origen.Dispose();
                    formapago_destino.Dispose();
                    break;
                case "Puntos_Bancarios":
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Puntos Bancarios...");
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios> puntos_bancarios_origen = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios>(sesion_import_origen);
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios> puntos_bancarios_destino = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios>(DevExpress.Xpo.XpoDefault.Session);
                    //
                    //sw = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("delete from {0}", lc_tabla));
                    xi = 0;
                    foreach (Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios item_origen in puntos_bancarios_origen)
                    {
                        puntos_bancarios_destino.Add(new Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios(DevExpress.Xpo.XpoDefault.Session));
                        //
                        puntos_bancarios_destino[xi].oid = item_origen.oid;
                        puntos_bancarios_destino[xi].banco_cuenta = (item_origen.banco_cuenta == null ? null : DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas>(item_origen.banco_cuenta.oid));
                        puntos_bancarios_destino[xi].codigo = item_origen.codigo;
                        puntos_bancarios_destino[xi].descr = item_origen.descr;
                        puntos_bancarios_destino[xi].status = item_origen.status;
                        puntos_bancarios_destino[xi].sucursal = item_origen.sucursal;
                        //
                        try
                        {
                            puntos_bancarios_destino[xi].Save();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No se pudo añadir el punto bancario : " + item_origen.codigo);
                        }
                    }
                    puntos_bancarios_origen.Dispose();
                    puntos_bancarios_destino.Dispose();
                    break;
                case "Configuracion":
                    DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Espere un momento..." + Environment.NewLine + "Transfiriendo Configuracion...");
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Configuracion> configuracion_origen = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Configuracion>(sesion_import_origen);
                    DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Configuracion> configuracion_destino = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Configuracion>(DevExpress.Xpo.XpoDefault.Session);
                    //
                    //var sw1 = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("delete from {0}", "configuracion"));
                    foreach (Fundraising_PTDM.FUNDRAISING_PT.Configuracion item_origen in configuracion_origen)
                    {
                        configuracion_destino.Add(new Fundraising_PTDM.FUNDRAISING_PT.Configuracion(DevExpress.Xpo.XpoDefault.Session));
                        //
                        configuracion_destino[0].oid = item_origen.oid;
                        configuracion_destino[0].activa_audio = item_origen.activa_audio;
                        configuracion_destino[0].time_new_sesion = item_origen.time_new_sesion;
                        configuracion_destino[0].sucursal = (item_origen.sucursal == null ? null : DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(item_origen.sucursal.oid));
                        //
                        try
                        {
                            configuracion_destino[0].Save();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No se pudo añadir el registro de configuracion");
                        }
                    }
                    configuracion_origen.Dispose();
                    configuracion_destino.Dispose();
                    break;
                default:
                    break;
            }
        }

        private void mantenimiento_saldos(string lc_tiposaldo)
        {
            string filtro_formas_pago = " forma_pago.tpago = 1 or forma_pago.tpago = 3 or forma_pago.tpago = 7 ";
            if (this.checkEdit_todosfecha.CheckState == CheckState.Unchecked)
            {
                lfecha_desde = ((DateTime)dateTime_fecha_desde.EditValue).Date.ToShortDateString();
                lfecha_hasta = ((DateTime)dateTime_fecha_hasta.EditValue).Date.ToShortDateString();
                string lfecha_desde1 = lfecha_desde.Substring(6, 4) + lfecha_desde.Substring(3, 2) + lfecha_desde.Substring(0, 2);
                string lfecha_hasta1 = lfecha_hasta.Substring(6, 4) + lfecha_hasta.Substring(3, 2) + lfecha_hasta.Substring(0, 2);
                lfiltro_saldos_recauda = string.Format("ToStr(GetYear(fecha_hora))+PadLeft(ToStr(GetMonth(fecha_hora)),2,'0')+PadLeft(ToStr(GetDay(fecha_hora)),2,'0') >= '{0}' and ToStr(GetYear(fecha_hora))+PadLeft(ToStr(GetMonth(fecha_hora)),2,'0')+PadLeft(ToStr(GetDay(fecha_hora)),2,'0') <= '{1}'", lfecha_desde1, lfecha_hasta1);
                lfiltro_saldos_deposit = string.Format("ToStr(GetYear(fecha_hora))+PadLeft(ToStr(GetMonth(fecha_hora)),2,'0')+PadLeft(ToStr(GetDay(fecha_hora)),2,'0') >= '{0}' and ToStr(GetYear(fecha_hora))+PadLeft(ToStr(GetMonth(fecha_hora)),2,'0')+PadLeft(ToStr(GetDay(fecha_hora)),2,'0') <= '{1}'", lfecha_desde1, lfecha_hasta1);
            }
            else
            {
                lfiltro_saldos_recauda = "1 = 1";
                lfiltro_saldos_deposit = "1 = 1";
            }
            //
            lfiltro_saldos_recauda = lfiltro_saldos_recauda + string.Format(" and sucursal = {0}", Fundraising_PT.Properties.Settings.Default.sucursal);
            lfiltro_saldos_deposit = lfiltro_saldos_deposit + string.Format(" and sucursal = {0}", Fundraising_PT.Properties.Settings.Default.sucursal);
            //
            if (this.checkEdit_todosrecaudador.CheckState == CheckState.Unchecked)
            {
                loid_recaudador = (Guid)this.lookUpEdit_recaudador.EditValue;
                lfiltro_saldos_recauda = lfiltro_saldos_recauda + " and " + string.Format("usuario.oid = '{0}'", loid_recaudador);
                lfiltro_saldos_deposit = lfiltro_saldos_deposit + " and " + string.Format("elaborado.oid = '{0}'", loid_recaudador);
            }
            //
            if (this.checkEdit_todostiposfp.CheckState == CheckState.Unchecked)
            {
                ltipo_forma_pago = (int)this.lookUpEdit_tipoformapago.EditValue;
                filtro_formas_pago = filtro_formas_pago + string.Format(" and forma_pago.tpago = {0}", ltipo_forma_pago);
            }
            //
            if (this.checkEdit_todosdescrpf.CheckState == CheckState.Unchecked)
            {
                loid_forma_pago = (Guid)this.lookUpEdit_formapago.EditValue;
                filtro_formas_pago = filtro_formas_pago + string.Format(" and forma_pago.oid = '{0}'", loid_forma_pago);
            }
            //
            switch (lc_tiposaldo)
            {
                case "0":  // Saldos de Recaudado-Depositado.
                    CriteriaOperator filtro_saldos_recaudaciones = CriteriaOperator.Parse(lfiltro_saldos_recauda);
                    CriteriaOperator filtro_saldos_depositos = CriteriaOperator.Parse(lfiltro_saldos_deposit);
                    recaudaciones.Criteria = filtro_saldos_recaudaciones;
                    depositos_bancarios.Criteria = filtro_saldos_depositos;
                    //
                    foreach (var item_recaudaciones in recaudaciones)
                    {
                        DateTime aux_fecha_hora = item_recaudaciones.fecha_hora;
                        Fundraising_PTDM.FUNDRAISING_PT.Usuarios aux_recaudador = item_recaudaciones.usuario;
                        //
                        recaudacion_det = item_recaudaciones.Recaudacion_Det;
                        recaudacion_det.Criteria = CriteriaOperator.Parse(filtro_formas_pago);
                        //
                        foreach (var item_recaudacion_det in recaudacion_det)
                        {
                            Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(1, aux_fecha_hora, aux_recaudador, item_recaudacion_det.forma_pago);
                        }
                    }
                    //
                    foreach (var item_depositos in depositos_bancarios)
                    {
                        DateTime aux_fecha_hora = item_depositos.fecha_hora;
                        Fundraising_PTDM.FUNDRAISING_PT.Usuarios aux_recaudador = item_depositos.elaborado;
                        //
                        depositos_bancarios_det = item_depositos.Depositos_Bancarios_Det;
                        depositos_bancarios_det.Criteria = CriteriaOperator.Parse(filtro_formas_pago);
                        //
                        foreach (var item_depositos_bancarios_det in depositos_bancarios_det)
                        {
                            Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(2, aux_fecha_hora, aux_recaudador, item_depositos_bancarios_det.forma_pago);
                        }
                    }
                    //                
                    break;
                case "1":
                    break;
                default:
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
            HeaderMenu.Caption = this.lHeader_ant;
        }

        private void modo_controles(int lnmodo)
        {
            switch (lnmodo)
            {
                case 1:
                    lookUpEdit_saldos.Enabled = false;
                    dateTime_fecha_desde.Enabled = false;
                    dateTime_fecha_hasta.Enabled = false;
                    checkEdit_todosfecha.Enabled = false;
                    checkEdit_todosrecaudador.Enabled = false;
                    checkEdit_todostiposfp.Enabled = false;
                    checkEdit_todosdescrpf.Enabled = false;
                    checkEdit_todosstatus.Enabled = false;
                    //
                    lookUpEdit_recaudador.Enabled = false;
                    lookUpEdit_tipoformapago.Enabled = false;
                    lookUpEdit_formapago.Enabled = false;
                    //
                    checkEdit_replace_codigo_sucursal.Enabled = false;
                    checkEdit_replace_codigo_sucursal_1.Enabled = false;
                    checkEdit_replace_status.Enabled = false;
                    checkEdit_correct_status_sesion_recaudacion_deposito.Enabled = false;
                    checkEdit_close_automatic_sesion.Enabled = false;
                    //
                    lookUpEdit_tablas.Enabled = true;
                    lookUpEdit_tablas.Focus();
                    break;
                case 2:
                    lookUpEdit_saldos.Enabled = true;
                    dateTime_fecha_desde.EditValue = DateTime.Now;
                    dateTime_fecha_hasta.EditValue = DateTime.Now;
                    dateTime_fecha_desde.Enabled = false;
                    dateTime_fecha_hasta.Enabled = false;
                    //
                    checkEdit_todosfecha.Checked = true;
                    checkEdit_todosrecaudador.Checked = true;
                    checkEdit_todostiposfp.Checked = true;
                    checkEdit_todosdescrpf.Checked = true;
                    checkEdit_todosstatus.Checked = true;
                    //
                    checkEdit_todosfecha.Enabled = true;
                    checkEdit_todosrecaudador.Enabled = true;
                    checkEdit_todostiposfp.Enabled = true;
                    checkEdit_todosdescrpf.Enabled = true;
                    checkEdit_todosstatus.Enabled = true;
                    //
                    lookUpEdit_recaudador.EditValue = string.Empty;
                    lookUpEdit_tipoformapago.EditValue = string.Empty;
                    lookUpEdit_formapago.EditValue = string.Empty;
                    //
                    lookUpEdit_recaudador.Enabled = false;
                    lookUpEdit_tipoformapago.Enabled = false;
                    lookUpEdit_formapago.Enabled = false;
                    //
                    checkEdit_replace_codigo_sucursal.Checked = false;
                    checkEdit_replace_codigo_sucursal_1.Checked = false;
                    checkEdit_replace_status.Checked = false;
                    checkEdit_correct_status_sesion_recaudacion_deposito.Checked = false;
                    checkEdit_close_automatic_sesion.Checked = false;
                    //
                    checkEdit_replace_codigo_sucursal.Enabled = true;
                    checkEdit_replace_codigo_sucursal_1.Enabled = true;
                    checkEdit_replace_status.Enabled = true;
                    checkEdit_correct_status_sesion_recaudacion_deposito.Enabled = true;
                    checkEdit_close_automatic_sesion.Enabled = true;
                    //
                    lookUpEdit_tablas.Enabled = false;
                    break;
                default:
                    lookUpEdit_saldos.Enabled = true;
                    dateTime_fecha_desde.EditValue = DateTime.Now;
                    dateTime_fecha_hasta.EditValue = DateTime.Now;
                    dateTime_fecha_desde.Enabled = false;
                    dateTime_fecha_hasta.Enabled = false;
                    //
                    checkEdit_todosfecha.Checked = true;
                    checkEdit_todosrecaudador.Checked = true;
                    checkEdit_todostiposfp.Checked = true;
                    checkEdit_todosdescrpf.Checked = true;
                    checkEdit_todosstatus.Checked = true;
                    //
                    checkEdit_todosfecha.Enabled = true;
                    checkEdit_todosrecaudador.Enabled = true;
                    checkEdit_todostiposfp.Enabled = true;
                    checkEdit_todosdescrpf.Enabled = true;
                    checkEdit_todosstatus.Enabled = true;
                    //
                    lookUpEdit_recaudador.EditValue = string.Empty;
                    lookUpEdit_tipoformapago.EditValue = string.Empty;
                    lookUpEdit_formapago.EditValue = string.Empty;
                    //
                    lookUpEdit_recaudador.Enabled = false;
                    lookUpEdit_tipoformapago.Enabled = false;
                    lookUpEdit_formapago.Enabled = false;
                    //
                    checkEdit_replace_codigo_sucursal.Checked = false;
                    checkEdit_replace_codigo_sucursal_1.Checked = false;
                    checkEdit_replace_status.Checked = false;
                    checkEdit_correct_status_sesion_recaudacion_deposito.Checked = false;
                    checkEdit_close_automatic_sesion.Checked = false;
                    //
                    checkEdit_replace_codigo_sucursal.Enabled = true;
                    checkEdit_replace_codigo_sucursal_1.Enabled = true;
                    checkEdit_replace_status.Enabled = true;
                    checkEdit_correct_status_sesion_recaudacion_deposito.Enabled = true;
                    checkEdit_close_automatic_sesion.Enabled = true;
                    //
                    lookUpEdit_tablas.Enabled = false;
                    break;
            }
        }

        private void setea_checks()
        {
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
            //if (formas_pagos.Count <= 0)
            //{
            //    checkEdit_todosdescrpf.CheckState = CheckState.Checked;
            //    checkEdit_todosdescrpf.Enabled = false;
            //}
            //else
            //{ checkEdit_todosdescrpf.Enabled = true; }
        }
    }
}
