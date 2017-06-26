using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using Fundraising_PT.Properties;
using Fundraising_PTDM.Properties;
using Microsoft.Data.ConnectionUI;
using System.IO;
using System.Diagnostics;

namespace Fundraising_PT
{
    public partial class UI_Principal : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Thread Tmessage_bienvenida;

        public UI_Principal()
        {
            Fundraising_PT.Formularios.WaitForm1 WaitForm1 = new Fundraising_PT.Formularios.WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(Fundraising_PT.Formularios.WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Fundraising...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando el Sistema...   .");
            //
            InitializeComponent();
            //
            //this.ribbonControl1.MouseEnter += new EventHandler(ribbonControl1_MouseEnter);
        }

        void ribbonControl1_MouseEnter(object sender, EventArgs e)
        {
            //MessageBox.Show(sender.ToString().Trim());
        }

        private void UI_Principal_Activated(object sender, EventArgs e)
        {
            this.barHeaderItem1.Caption = Settings.Default.Nombre_Sistema.Trim() + " - Pantalla Principal";
        }

        //protected override void OnActivated(EventArgs e)
        //{
        //    base.OnActivated(e);
        //    this.barHeaderItem1.Caption = Settings.Default.Nombre_Sistema.Trim() + " - Pantalla Principal";
        //}

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (Fundraising_PT.Properties.Settings.Default.Activa_Audio == 1)
            {
                if (Tmessage_bienvenida != null )
                {
                    Tmessage_bienvenida.Abort();
                }
            }
            
        }

        private void UI_Principal_Load(object sender, EventArgs e)
        {
            if (Fundraising_PT.Properties.Settings.Default.Activa_Audio == 1)
            {
                Tmessage_bienvenida = new Thread(new ThreadStart(message_bienvenida));
                Tmessage_bienvenida.Start();
            }

            // Seteo de separadores de miles y decimales y cantidad de digitos para datos numeric,  currency, percent, y activa la aplicacion
            try
            {
                //System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator = ",";
                //System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator = ".";
                //System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalDigits = 3;
                ////
                //System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSeparator = ",";
                //System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator = ".";
                //System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalDigits = 3;
                ////
                //System.Globalization.NumberFormatInfo.CurrentInfo.PercentGroupSeparator = ",";
                //System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator = ".";
                //System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalDigits = 3;

                //Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator = ",";
                //Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
                //Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits = 4;
                //
                //Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ",";
                //Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator = ".";
                //Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits = 4;
                //
                //Thread.CurrentThread.CurrentCulture.NumberFormat.PercentGroupSeparator = ",";
                //Thread.CurrentThread.CurrentCulture.NumberFormat.PercentDecimalSeparator = ".";
                //Thread.CurrentThread.CurrentCulture.NumberFormat.PercentDecimalDigits = 4;

                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
                                
                this.ribbonControl1.ApplicationCaption = "Sistema de Recaudación de Fondos (Fundraising VSoft) - Usuario : " + Fundraising_PT.Properties.Settings.Default.U_usuario + " - Tipo : " + ((Fundraising_PTDM.Enums.ETipo)Fundraising_PT.Properties.Settings.Default.U_tipo).ToString().Trim();
                ActivaMenuPrincipal();
                //
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
            }
            catch (Exception oerror)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
                MessageBox.Show("Ocurrio un error inicializando el sistema..." + Environment.NewLine + "Error: " + oerror.Message, "Inicializando Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void message_bienvenida()
        {
            string lc_mday = "";
            string lc_mensaje_bienvenida = " Bienvenidos al sistema de recaudacion de fondos";
            System.Speech.Synthesis.SpeechSynthesizer Speech = new System.Speech.Synthesis.SpeechSynthesizer();
            DateTime fecha_hora_entrada = DateTime.Now;
            if (fecha_hora_entrada.Hour >= 6 & fecha_hora_entrada.Hour <= 11)
            { lc_mday = "Buenos dias "; }
            if (fecha_hora_entrada.Hour >= 12 & fecha_hora_entrada.Hour <= 17)
            { lc_mday = "Buenas tardes "; }
            if (fecha_hora_entrada.Hour >= 18 & fecha_hora_entrada.Hour <= 5)
            { lc_mday = "Buenas noches "; }
            lc_mensaje_bienvenida = lc_mday + Fundraising_PT.Properties.Settings.Default.U_usuario + ",Bienvenidos al sistema de recaudacion de fondos";
            Speech.Speak(lc_mensaje_bienvenida);
        }

        private void link_facebook(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("http://www.facebook.com/vsoft.developers");
        }

        private void link_twitter(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("http://twitter.com/visualsoft_EC");
        }
        private void link_youtube(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UC91S1YYg8cckhCLFHqJzbnQ");
        }

        private void barButtonItem_basededatos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataConnectionDialog dcd = new DataConnectionDialog();
            DataSource.AddStandardDataSources(dcd);
            //
            dcd.SelectedDataSource = DataSource.SqlDataSource;
            dcd.SelectedDataProvider = DataProvider.SqlDataProvider;
            //
            dcd.ConnectionString = Fundraising_PTDM.MyConnection.GetConnectionString();
            //
            if (DataConnectionDialog.Show(dcd) == System.Windows.Forms.DialogResult.OK)
            {
                SqlConnectionStringBuilder sqlcsb = new SqlConnectionStringBuilder(dcd.ConnectionString);
                Fundraising_PTDM.MyConnection.SetConnectionString(dcd.ConnectionString, sqlcsb.DataSource, sqlcsb.InitialCatalog, sqlcsb.UserID, sqlcsb.Password);
                MessageBox.Show("Para que los cambios en la configuración tengan efecto, debe salir y volver a entrar al sistema...", "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void barButtonItem_configuraciongeneral_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_configuraciongeneral.Enabled = false;
            //OpenForm(new Formularios.UI_Configuracion_general(2, this.barButtonItem_configuraciongeneral, this.barHeaderItem1, null, this.barButtonItem_configuracion_current_sucursal, this.barButtonItem_datos_current_sucursal, this.barButtonItem_procesos_current_sucursal, this.barButtonItem_reportes_current_sucursal));
            //
            Formularios.UI_Configuracion_general configuracion_general = new Formularios.UI_Configuracion_general(2, this.barButtonItem_configuraciongeneral, this.barHeaderItem1, null, this.barButtonItem_configuracion_current_sucursal, this.barButtonItem_datos_current_sucursal, this.barButtonItem_procesos_current_sucursal, this.barButtonItem_reportes_current_sucursal);
            configuracion_general.ShowDialog(this);
            //
        }

        private void barButtonItem_usuarios_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_usuarios.Enabled = false;
            OpenForm(new Fundraising_PT.Formularios.UI_Usuarios(this.barButtonItem_usuarios, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
        }

        private void barButtonItem_sucursales_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_sucursales.Enabled = false;
            OpenForm(new Fundraising_PT.Formularios.UI_Sucursales(this.barButtonItem_sucursales, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
        }

        private void barButtonItem_mantenimiento_ts_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_mantenimiento_ts.Enabled = false;
            OpenForm(new Formularios.UI_Mantenimientos(this.barButtonItem_mantenimiento_ts, this.barHeaderItem1, this));
        }

        private void barButtonItem_sesiones_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajas> cajas =
                    new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajas>(DevExpress.Xpo.XpoDefault.Session);
            DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajeros> cajeros =
                    new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajeros>(DevExpress.Xpo.XpoDefault.Session);
            //
            if (cajas.Count > 0)
            {
                cajas.Dispose();
                if (cajeros.Count > 0)
                {
                    cajeros.Dispose();
                    this.barButtonItem_sesiones.Enabled = false;
                    OpenForm(new Fundraising_PT.Formularios.UI_Sesiones(this.barButtonItem_sesiones, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
                }
                else
                {
                    MessageBox.Show("No existen cajeros cargadas en el sistema," + Environment.NewLine + "Favor cargar al menos uno para utilizar Sesiones.", "Sesiones", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cajeros.Dispose();
                }
            }
            else
            {
                MessageBox.Show("No existen cajas cargadas en el sistema," + Environment.NewLine + "Favor cargar al menos una para utilizar Sesiones.", "Sesiones", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cajas.Dispose();
                cajeros.Dispose();
            }
        }

        private void barButtonItem_cajas_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_cajas.Enabled = false;
            OpenForm(new Fundraising_PT.Formularios.UI_Cajas(this.barButtonItem_cajas, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
        }

        private void barButtonItem_cajeros_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_cajeros.Enabled = false;
            OpenForm(new Fundraising_PT.Formularios.UI_Cajeros(this.barButtonItem_cajeros, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
        }

        private void barButtonItem_denominacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_denominacion.Enabled = false;
            OpenForm(new Fundraising_PT.Formularios.UI_Denominacion_Monedas(this.barButtonItem_denominacion, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
        }

        private void barButtonItem_formaspago_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_formaspago.Enabled = false;
            OpenForm(new Fundraising_PT.Formularios.UI_Formas_Pagos(this.barButtonItem_formaspago, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
        }

        private void barButtonItem_bancos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_bancos.Enabled = false;
            OpenForm(new Fundraising_PT.Formularios.UI_Bancos(this.barButtonItem_bancos, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
        }

        private void barButtonItem_cuentasbancarias_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos> bancos =
                    new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos>(DevExpress.Xpo.XpoDefault.Session);
            //
            if (bancos.Count > 0)
            {
                bancos.Dispose();
                this.barButtonItem_cuentasbancarias.Enabled = false;
                OpenForm(new Fundraising_PT.Formularios.UI_Bancos_Cuentas(this.barButtonItem_cuentasbancarias, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
            }
            else
            {
                MessageBox.Show("No existen bancos cargados en el sistema," + Environment.NewLine + "Favor cargar al menos uno para utilizar Cuentas Bancarias.", "Cuentas Bancarias", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bancos.Dispose();
            }
        }
        
        private void barButtonItempuntosbancarios_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas> bancos_cuentas =
                    new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas>(DevExpress.Xpo.XpoDefault.Session);
            //
            if (bancos_cuentas.Count > 0)
            {
                bancos_cuentas.Dispose();
                this.barButtonItempuntosbancarios.Enabled = false;
                OpenForm(new Formularios.UI_Puntos_Bancarios(this.barButtonItempuntosbancarios, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
            }
            else
            {
                MessageBox.Show("No existen cuentas bancarias cargadas en el sistema," + Environment.NewLine + "Favor cargar al menos una para utilizar Puntos Bancarios.", "Puntos Bancarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bancos_cuentas.Dispose();
            }
        }

        private void barButtonItem_proveedoresta_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_proveedoresta.Enabled = false;
            OpenForm(new Formularios.UI_Proveedores_TA(this.barButtonItem_proveedoresta, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
        }

        private void barButtonItem_responsablesdepositos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_responsablesdepositos.Enabled = false;
            OpenForm(new Formularios.UI_Responsable_Depositos(this.barButtonItem_responsablesdepositos, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
        }

        private void barButtonItem_sesiones_recauda_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sesiones> sesiones =
                    new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(DevExpress.Xpo.XpoDefault.Session);
            DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos> formas_pagos =
                    new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(DevExpress.Xpo.XpoDefault.Session);
            //
            if (sesiones.Count > 0)
            {
                sesiones.Dispose();
                if (formas_pagos.Count > 0)
                {
                    formas_pagos.Dispose();
                    //this.barButtonItem_sesiones_recauda.Enabled = false;
                    //this.barButtonItem_recaudaciones.Enabled = false;

                    //// gv1 30/12/2015 //
                    //this.barButtonItem_configuracion_salir.Enabled = false;
                    //this.barButtonItem_datos_salir.Enabled = false;
                    //this.barButtonItem_procesos_salir.Enabled = false;
                    //this.barButtonItem_reportes_salir.Enabled = false;
                    //// gv1 30/12/2015 //

                    //OpenForm(new Formularios.UI_Sesion_Recauda(this.barButtonItem_sesiones_recauda, this.barHeaderItem1, this.barButtonItem_recaudaciones, this.barButtonItem_configuracion_salir, this.barButtonItem_datos_salir, this.barButtonItem_procesos_salir, this.barButtonItem_reportes_salir));
                    OpenForm(new Formularios.UI_Sesion_Recauda(this.barButtonItem_sesiones_recauda, this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
                }
                else
                {
                    MessageBox.Show("No existen formas de pago cargadas en el sistema," + Environment.NewLine + "Favor cargar al menos una para utilizar Nueva Recaudación.", "Nueva Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    formas_pagos.Dispose();
                }
            }
            else
            {
                MessageBox.Show("No existen sesiones cargadas en el sistema," + Environment.NewLine + "Favor cargar al menos una para utilizar Nueva Recaudación.", "Nueva Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                sesiones.Dispose();
                formas_pagos.Dispose();
            }
        }

        private void barButtonItem_recaudaciones_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones> recaudaciones =
                    new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones>(DevExpress.Xpo.XpoDefault.Session);
            //
            if (recaudaciones.Count > 0)
            {
                recaudaciones.Dispose();
                this.barButtonItem_sesiones_recauda.Enabled = false;
                this.barButtonItem_recaudaciones.Enabled = false;

                // gv1 30/12/2015 //
                this.barButtonItem_configuracion_salir.Enabled = false;
                this.barButtonItem_datos_salir.Enabled = false;
                this.barButtonItem_procesos_salir.Enabled = false;
                this.barButtonItem_reportes_salir.Enabled = false;
                // gv1 30/12/2015 //

                OpenForm(new Formularios.UI_Recaudaciones(this.barButtonItem_recaudaciones, ref this.barHeaderItem1, this.barButtonItem_sesiones_recauda, this.barButtonItem_configuracion_salir, this.barButtonItem_datos_salir, this.barButtonItem_procesos_salir, this.barButtonItem_reportes_salir));
            }
            else
            {
                MessageBox.Show("No existen recaudaciones cargadas en el sistema," + Environment.NewLine + "Favor cargar al menos una para utilizar Consulta y Ajustes.", "Consulta y Ajustes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                recaudaciones.Dispose();
            }
        }

        private void barButtonItem_depositos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas> bancos_cuentas =
                    new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas>(DevExpress.Xpo.XpoDefault.Session);
            DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos> responsable_depositos =
                    new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Responsable_depositos>(DevExpress.Xpo.XpoDefault.Session);
            //
            if (bancos_cuentas.Count > 0)
            {
                bancos_cuentas.Dispose();
                if (responsable_depositos.Count > 0)
                {
                    responsable_depositos.Dispose();
                    this.barButtonItem_depositos.Enabled = false;
                    OpenForm(new Formularios.UI_Depositos_Bancarios(this.barButtonItem_depositos, ref this.barHeaderItem1, this.ribbonControl1, null, null, null, null));
                }
                else
                {
                    MessageBox.Show("No existen responsables de deposito cargados en el sistema," + Environment.NewLine + "Favor cargar al menos uno para utilizar Depositos Bancarios.", "Depositos Bancarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    responsable_depositos.Dispose();
                }
            }
            else
            {
                MessageBox.Show("No existen cuentas bancarias cargadas en el sistema," + Environment.NewLine + "Favor cargar al menos una para utilizar Depositos Bancarios.", "Depositos Bancarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bancos_cuentas.Dispose();
                responsable_depositos.Dispose();
            }
        }
        private void barButtonItem_preferenciasusuario_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_preferenciasusuario.Enabled = false;
            //OpenForm(new Formularios.UI_Configuracion_general(1,this.barButtonItem_preferenciasusuario, this.barHeaderItem1, null, this.barButtonItem_configuracion_current_sucursal, this.barButtonItem_datos_current_sucursal, this.barButtonItem_procesos_current_sucursal, this.barButtonItem_reportes_current_sucursal));
            //
            Formularios.UI_Configuracion_general preferencias_usuario = new Formularios.UI_Configuracion_general(1, this.barButtonItem_preferenciasusuario, this.barHeaderItem1, null, this.barButtonItem_configuracion_current_sucursal, this.barButtonItem_datos_current_sucursal, this.barButtonItem_procesos_current_sucursal, this.barButtonItem_reportes_current_sucursal);
            preferencias_usuario.ShowDialog(this);
            setea_titulos_sucursal();
        }
        
        //private void barButtonItem_salir_ItemClick()
        //{
        //    if (MessageBox.Show("Esta seguro de Salir del Sistema ?", "Salir del Sistema", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
        //        { this.Close(); }
        //}

        private void barButtonItem_reporterecaudaciones_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_reporterecaudaciones.Enabled = false;
            OpenForm(new Formularios.UI_Report_Recauda(this.barButtonItem_reporterecaudaciones, this.barHeaderItem1, 1));
        }

        private void barButtonItem_reportetotalesventas_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_reportetotalesventas.Enabled = false;
            OpenForm(new Formularios.UI_Report_Recauda(this.barButtonItem_reportetotalesventas, this.barHeaderItem1, 2));
        }

        private void barButtonItem_reportedepositosbancarios_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_reportedepositosbancarios.Enabled = false;
            OpenForm(new Formularios.UI_Report_Deposito(this.barButtonItem_reportedepositosbancarios, this.barHeaderItem1, null));
        }

        private void barButtonItem_reportediferenciasVR_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_reportediferenciasVR.Enabled = false;
            OpenForm(new Formularios.UI_Report_Recauda(this.barButtonItem_reportediferenciasVR, this.barHeaderItem1, 3));
        }

        private void barButtonItem_reporteestadocuenta_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_reporteestadocuenta.Enabled = false;
            //OpenForm(new Formularios.UI_Estado_Cuenta(this.barButtonItem_reporteestadocuenta, this.barHeaderItem1, null));
            OpenForm(new Formularios.UI_Report_Estado_Cuenta(this.barButtonItem_reporteestadocuenta, this.barHeaderItem1, null));
        }

        private void OpenForm(DevExpress.XtraEditors.XtraForm xtraForm)
        {
            if (xtraForm != null)
            {
                xtraForm.MdiParent = this;
                xtraForm.Show();
            }
        }

        public void ActivaMenuPrincipal()
        {
            //
            if ((Fundraising_PT.Properties.Settings.Default.sucursal_oid == Guid.Empty | Fundraising_PT.Properties.Settings.Default.sucursal <= 0) | (Fundraising_PT.Properties.Settings.Default.sucursal == null | Fundraising_PT.Properties.Settings.Default.sucursal_oid == null) | (Fundraising_PT.Properties.Settings.Default.sucursal_filter == null | Fundraising_PT.Properties.Settings.Default.sucursal_filter == string.Empty))
            {
                this.barButtonItem_preferenciasusuario.Enabled = false;
                //OpenForm(new Formularios.UI_Configuracion_general(1,this.barButtonItem_preferenciasusuario, this.barHeaderItem1, null, this.barButtonItem_configuracion_current_sucursal, this.barButtonItem_datos_current_sucursal, this.barButtonItem_procesos_current_sucursal, this.barButtonItem_reportes_current_sucursal));
                //
                Formularios.UI_Configuracion_general preferencias_usuario = new Formularios.UI_Configuracion_general(1, this.barButtonItem_preferenciasusuario, this.barHeaderItem1, null, this.barButtonItem_configuracion_current_sucursal, this.barButtonItem_datos_current_sucursal, this.barButtonItem_procesos_current_sucursal, this.barButtonItem_reportes_current_sucursal);
                preferencias_usuario.ShowDialog(this);
            }
            //
            setea_titulos_sucursal();
            //
            switch (Fundraising_PT.Properties.Settings.Default.U_tipo)
            {
                case 1:
                    barButtonItem_basededatos.Enabled = true;
                    barButtonItem_configuraciongeneral.Enabled = true;
                    barButtonItem_usuarios.Enabled = true;
                    barButtonItem_mapeos.Enabled = true;
                    //
                    barButtonItem_sesiones.Enabled = true;
                    barButtonItem_cajas.Enabled = true;
                    barButtonItem_cajeros.Enabled = true;
                    barButtonItem_denominacion.Enabled = true;
                    barButtonItem_formaspago.Enabled = true;
                    barButtonItem_bancos.Enabled = true;
                    barButtonItem_cuentasbancarias.Enabled = true;
                    barButtonItempuntosbancarios.Enabled = true;
                    barButtonItem_proveedoresta.Enabled = true;
                    //
                    barButtonItem_reporterecaudaciones.Enabled = true;
                    barButtonItem_reportetotalesventas.Enabled = true;
                    barButtonItem_reportedepositosbancarios.Enabled = true;
                    barButtonItem_reportediferenciasVR.Enabled = true;
                    barButtonItem_reporteestadocuenta.Enabled = true;
                    //
                    ribbonPageGroup_configuracion.Visible = true;
                    ribbonPageGroup_datos.Visible = true;
                    ribbonPageGroup_reportes.Visible = true;
                    //
                    ribbonPage_configuracion.Visible = true;
                    ribbonPage_datos.Visible = true;                    
                    ribbonPage_reportes.Visible = true;
                    //
                    break;
                case 2:
                    barButtonItem_basededatos.Enabled = false;
                    barButtonItem_configuraciongeneral.Enabled = false;
                    barButtonItem_usuarios.Enabled = false;
                    barButtonItem_mapeos.Enabled = false;
                    //
                    barButtonItem_sesiones.Enabled = true;
                    barButtonItem_cajas.Enabled = true;
                    barButtonItem_cajeros.Enabled = true;
                    barButtonItem_denominacion.Enabled = true;
                    barButtonItem_formaspago.Enabled = true;
                    barButtonItem_bancos.Enabled = true;
                    barButtonItem_cuentasbancarias.Enabled = true;
                    barButtonItempuntosbancarios.Enabled = true;
                    barButtonItem_proveedoresta.Enabled = true;
                    //
                    barButtonItem_reporterecaudaciones.Enabled = false;
                    barButtonItem_reportetotalesventas.Enabled = true;
                    barButtonItem_reportedepositosbancarios.Enabled = true;
                    barButtonItem_reportediferenciasVR.Enabled = false;
                    barButtonItem_reporteestadocuenta.Enabled = false;
                    //
                    ribbonPageGroup_configuracion.Visible = false;
                    ribbonPageGroup_datos.Visible = true;
                    ribbonPageGroup_reportes.Visible = true;
                    //
                    ribbonPage_configuracion.Visible = false;
                    ribbonPage_datos.Visible = true;                    
                    ribbonPage_reportes.Visible = true;
                    //
                    break;
                default:
                    barButtonItem_basededatos.Enabled = false;
                    barButtonItem_configuraciongeneral.Enabled = false;
                    barButtonItem_usuarios.Enabled = false;
                    barButtonItem_mapeos.Enabled = false;
                    //
                    barButtonItem_sesiones.Enabled = false;
                    barButtonItem_cajas.Enabled = false;
                    barButtonItem_cajeros.Enabled = false;
                    barButtonItem_denominacion.Enabled = false;
                    barButtonItem_formaspago.Enabled = false;
                    barButtonItem_bancos.Enabled = false;
                    barButtonItem_cuentasbancarias.Enabled = false;
                    barButtonItempuntosbancarios.Enabled = false;
                    barButtonItem_proveedoresta.Enabled = false;
                    //
                    barButtonItem_reporterecaudaciones.Enabled = true;
                    barButtonItem_reportetotalesventas.Enabled = false;
                    barButtonItem_reportedepositosbancarios.Enabled = false;
                    barButtonItem_reportediferenciasVR.Enabled = false;
                    barButtonItem_reporteestadocuenta.Enabled = false;
                    //
                    ribbonPageGroup_configuracion.Visible = false;
                    ribbonPageGroup_datos.Visible = false;
                    ribbonPageGroup_reportes.Visible = true;
                    //
                    ribbonPage_configuracion.Visible = false;
                    ribbonPage_datos.Visible = false;                    
                    ribbonPage_reportes.Visible = true;
                    //
                    break;
            }
            //
            barButtonItem_configuracion_salir.ItemClick += barButtonItem_configuracion_salir_ItemClick;
            barButtonItem_datos_salir.ItemClick += barButtonItem_configuracion_salir_ItemClick;
            barButtonItem_procesos_salir.ItemClick += barButtonItem_configuracion_salir_ItemClick;
            barButtonItem_reportes_salir.ItemClick += barButtonItem_configuracion_salir_ItemClick;
        }

        void barButtonItem_configuracion_salir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Esta seguro de Salir del Sistema ?", "Salir del Sistema", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
            { this.Close(); }
        }

        public void setea_titulos_sucursal()
        {
            barButtonItem_configuracion_current_sucursal.Caption = Fundraising_PT.Properties.Settings.Default.nombre_sucursal;
            barButtonItem_datos_current_sucursal.Caption = Fundraising_PT.Properties.Settings.Default.nombre_sucursal;
            barButtonItem_procesos_current_sucursal.Caption = Fundraising_PT.Properties.Settings.Default.nombre_sucursal;
            barButtonItem_reportes_current_sucursal.Caption = Fundraising_PT.Properties.Settings.Default.nombre_sucursal;
            //
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_configuracion_current_sucursal).SuperTip.Items.Clear();
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_configuracion_current_sucursal).SuperTip.Items.AddTitle("SUCURAL POR DEFECTO : " + Fundraising_PT.Properties.Settings.Default.nombre_sucursal);
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_configuracion_current_sucursal).SuperTip.Items.AddTitle("Filtro Sucursal : " + Fundraising_PT.Properties.Settings.Default.sucursal_filter);
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_configuracion_current_sucursal).SuperTip.Items.AddSeparator();
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_configuracion_current_sucursal).SuperTip.Items.AddTitle("SERVIDOR DE DATOS : " + Fundraising_PTDM.MyConnection.GetDataServer());
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_configuracion_current_sucursal).SuperTip.Items.AddTitle("BASE DE DATOS : " + Fundraising_PTDM.MyConnection.GetDataBase());
            //
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_datos_current_sucursal).SuperTip.Items.Clear();
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_datos_current_sucursal).SuperTip.Items.AddTitle("SUCURAL POR DEFECTO : " + Fundraising_PT.Properties.Settings.Default.nombre_sucursal);
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_datos_current_sucursal).SuperTip.Items.AddTitle("Filtro Sucursal : " + Fundraising_PT.Properties.Settings.Default.sucursal_filter);
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_datos_current_sucursal).SuperTip.Items.AddSeparator();
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_datos_current_sucursal).SuperTip.Items.AddTitle("SERVIDOR DE DATOS : " + Fundraising_PTDM.MyConnection.GetDataServer());
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_datos_current_sucursal).SuperTip.Items.AddTitle("BASE DE DATOS : " + Fundraising_PTDM.MyConnection.GetDataBase());
            //
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_procesos_current_sucursal).SuperTip.Items.Clear();
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_procesos_current_sucursal).SuperTip.Items.AddTitle("SUCURAL POR DEFECTO : " + Fundraising_PT.Properties.Settings.Default.nombre_sucursal);
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_procesos_current_sucursal).SuperTip.Items.AddTitle("Filtro Sucursal : " + Fundraising_PT.Properties.Settings.Default.sucursal_filter);
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_procesos_current_sucursal).SuperTip.Items.AddSeparator();
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_procesos_current_sucursal).SuperTip.Items.AddTitle("SERVIDOR DE DATOS : " + Fundraising_PTDM.MyConnection.GetDataServer());
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_procesos_current_sucursal).SuperTip.Items.AddTitle("BASE DE DATOS : " + Fundraising_PTDM.MyConnection.GetDataBase());
            //
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_reportes_current_sucursal).SuperTip.Items.Clear();
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_reportes_current_sucursal).SuperTip.Items.AddTitle("SUCURAL POR DEFECTO : " + Fundraising_PT.Properties.Settings.Default.nombre_sucursal);
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_reportes_current_sucursal).SuperTip.Items.AddTitle("Filtro Sucursal : " + Fundraising_PT.Properties.Settings.Default.sucursal_filter);
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_reportes_current_sucursal).SuperTip.Items.AddSeparator();
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_reportes_current_sucursal).SuperTip.Items.AddTitle("SERVIDOR DE DATOS : " + Fundraising_PTDM.MyConnection.GetDataServer());
            ((DevExpress.XtraBars.BarButtonItem)barButtonItem_reportes_current_sucursal).SuperTip.Items.AddTitle("BASE DE DATOS : " + Fundraising_PTDM.MyConnection.GetDataBase());
        }

 
        //public Image carga_current_imagen()
        //{
        //    Image logotipo = null;
        //    string nane_imagen = Fundraising_PT.Properties.Settings.Default.logotipo.Trim();
        //    string filenane_imagen = Fundraising_PT.Properties.Settings.Default.mypath_imagenes + nane_imagen;
        //    //
        //    if (string.IsNullOrEmpty(filenane_imagen) != true)
        //    {
        //        if (File.Exists(filenane_imagen))
        //        {
        //            logotipo = Image.FromFile(filenane_imagen);
        //        }
        //    }
        //    return logotipo;
        //}

    }
}
