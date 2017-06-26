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
using System.IO;
using DevExpress.Data.Filtering;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Configuracion_general : DevExpress.XtraEditors.XtraForm
    {
        // declaracion de objetos publicos //
        public DevExpress.XtraBars.BarButtonItem OpcionMenu;
        public DevExpress.XtraBars.BarHeaderItem HeaderMenu;
        public int Ln_modo = 0; 
        public object ObjetoExtra;
        public string lHeader_ant = "";
        public string lAccion = "Preferencias del Usuario - Editando";
        private Guid lg_sucursal = Guid.Empty;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Configuracion> configuracion;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales> sucursales;
        private Fundraising_PTDM.FUNDRAISING_PT.Sucursales current_sucursal_inicial;
        public DevExpress.XtraBars.BarButtonItem ObjetoExtra1;
        public DevExpress.XtraBars.BarButtonItem ObjetoExtra2;
        public DevExpress.XtraBars.BarButtonItem ObjetoExtra3;
        public DevExpress.XtraBars.BarButtonItem ObjetoExtra4;

        public UI_Configuracion_general(int ln_modo, DevExpress.XtraBars.BarButtonItem opcionMenu, DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra, object objetoExtra1, object objetoExtra2, object objetoExtra3, object objetoExtra4)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            if (ln_modo == 1)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Preferencias del Usuario...");
                lAccion = "Preferencias del Usuario - Editando";
            }
            else 
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Configuracion General...");
                lAccion = "Configuracion General - Editando";
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //
            Ln_modo = ln_modo;
            OpcionMenu = opcionMenu;
            HeaderMenu = headerMenu;
            ObjetoExtra = objetoExtra;
            ObjetoExtra1 = (DevExpress.XtraBars.BarButtonItem)objetoExtra1;
            ObjetoExtra2 = (DevExpress.XtraBars.BarButtonItem)objetoExtra2;
            ObjetoExtra3 = (DevExpress.XtraBars.BarButtonItem)objetoExtra3;
            ObjetoExtra4 = (DevExpress.XtraBars.BarButtonItem)objetoExtra4;
            lHeader_ant = HeaderMenu.Caption;
        }

        private void UI_Configuracion_general_Load(object sender, EventArgs e)
        {
            if (Ln_modo == 1)
            {
                Text = "Preferencias del Usuario";
                labelControl_titulo.Text = "Preferencias del Usuario";
                label_nombre_sucursal.Visible = true;
                label_time_new_sesion.Visible = false;
                label_time_new_sesion2.Visible = false;
                label_activasonido.Visible = false;
                lookUpEdit_sucursal.Visible = true;
                spinEdit_time_new_sesion.Visible = false;
                lookUp_activa_audio.Visible = false;
            }
            else
            {
                this.Text = "Configuracion General";
                this.labelControl_titulo.Text = "Configuracion General";
                label_nombre_sucursal.Visible = false;
                label_time_new_sesion.Visible = true;
                label_time_new_sesion2.Visible = true;
                label_activasonido.Visible = true;
                lookUpEdit_sucursal.Visible = false;
                spinEdit_time_new_sesion.Visible = true;
                lookUp_activa_audio.Visible = true;
            }
            sucursales = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session, (new OperandProperty("status") == new OperandValue(1)), (new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending)));
            sucursales.LoadingEnabled = true;
            sucursales.Reload();
            //
            configuracion = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Configuracion>(DevExpress.Xpo.XpoDefault.Session, true);
            configuracion.LoadingEnabled = true;
            configuracion.Reload();
            //
            bindingSource_sucursales.DataSource = sucursales;
            bindingSource_configuraciongeneral.DataSource = configuracion;
            //
            if (bindingSource_configuraciongeneral.Count > 0)
            {
                if (((Fundraising_PTDM.FUNDRAISING_PT.Configuracion)bindingSource_configuraciongeneral.Current).sucursal != null)
                {
                    lookUpEdit_sucursal.EditValue = ((Fundraising_PTDM.FUNDRAISING_PT.Configuracion)bindingSource_configuraciongeneral.Current).sucursal.oid;
                    current_sucursal_inicial = ((Fundraising_PTDM.FUNDRAISING_PT.Configuracion)bindingSource_configuraciongeneral.Current).sucursal;
                }
            }
            //
            lookUp_activa_audio.gridLookUpEdit1View.OptionsBehavior.AutoPopulateColumns = true;
            lookUp_activa_audio.gridLookUpEdit1.Properties.DataSource = Fundraising_PTDM.Enums.GetListValue(lookUp_activa_audio.Enum);
            lookUp_activa_audio.gridLookUpEdit1.Properties.DisplayMember = "Descripcion";
            lookUp_activa_audio.gridLookUpEdit1.Properties.ValueMember = "Valor";
            lookUp_activa_audio.gridLookUpEdit1.DataBindings.Clear();
            lookUp_activa_audio.gridLookUpEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", bindingSource_configuraciongeneral, "activa_audio", true, DataSourceUpdateMode.Never));
            //
            spinEdit_time_new_sesion.DataBindings.Clear();
            spinEdit_time_new_sesion.DataBindings.Add(new System.Windows.Forms.Binding("Value", bindingSource_configuraciongeneral, "time_new_sesion", true, DataSourceUpdateMode.Never));
            //
            simpleButton_guardar.Click += new EventHandler(simpleButton_guardar_Click);
            simpleButton_salir.Click += new EventHandler(simpleButton_salir_Click);
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        private void simpleButton_guardar_Click(object sender, EventArgs e)
        {
            lg_sucursal = (Guid)this.lookUpEdit_sucursal.EditValue;
            Fundraising_PTDM.FUNDRAISING_PT.Sucursales current_sucursal = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(lg_sucursal);
            //((Fundraising_PTDM.FUNDRAISING_PT.Configuracion)bindingSource_configuraciongeneral.Current).sucursal = current_sucursal;
            //
            if (Ln_modo == 1)
            {
                Fundraising_PT.Properties.Settings.Default.sucursal_oid = current_sucursal.oid;
                Fundraising_PT.Properties.Settings.Default.sucursal = current_sucursal.codigo;
                Fundraising_PT.Properties.Settings.Default.nombre_sucursal = current_sucursal.nombre;
                Fundraising_PT.Properties.Settings.Default.logotipo = current_sucursal.logotipo;
                Fundraising_PT.Properties.Settings.Default.sucursal_filter = string.Format("sucursal = {0}", current_sucursal.codigo);
                Fundraising_PT.Properties.Settings.Default.Save();
                //
                //ObjetoExtra1.Caption = Fundraising_PT.Properties.Settings.Default.nombre_sucursal;
                //ObjetoExtra2.Caption = Fundraising_PT.Properties.Settings.Default.nombre_sucursal;
                //ObjetoExtra3.Caption = Fundraising_PT.Properties.Settings.Default.nombre_sucursal;
                //ObjetoExtra4.Caption = Fundraising_PT.Properties.Settings.Default.nombre_sucursal;
                //
                MessageBox.Show("Preferencias del Usuario Guardados Correctamente...", "Guardar preferencias del usuario");
                Close();
            }
            else 
            {
                ((Fundraising_PTDM.FUNDRAISING_PT.Configuracion)bindingSource_configuraciongeneral.Current).time_new_sesion = (int)spinEdit_time_new_sesion.Value;
                ((Fundraising_PTDM.FUNDRAISING_PT.Configuracion)bindingSource_configuraciongeneral.Current).activa_audio = 0;
                ((XPBaseObject)bindingSource_configuraciongeneral.Current).Save();
                //
                Fundraising_PT.Properties.Settings.Default.Activa_Audio = 0;
                Fundraising_PT.Properties.Settings.Default.time_new_sesion = (int)spinEdit_time_new_sesion.Value;
                Fundraising_PT.Properties.Settings.Default.Save();
                //
                MessageBox.Show("Configuracion General Guardada Correctamente...", "Guardar Configuracion general");
            }
            //
            //           
            //((XPBaseObject)bindingSource_configuraciongeneral.Current).Save();
            //if (current_sucursal_inicial != null)
            //{
            //    current_sucursal_inicial.select = false;
            //    current_sucursal_inicial.Save();
            //    current_sucursal.select = true;
            //    current_sucursal.Save();
            //}
            //
        }

        private void simpleButton_salir_Click(object sender, EventArgs e)
        {
            if ((Fundraising_PT.Properties.Settings.Default.sucursal_oid == Guid.Empty | Fundraising_PT.Properties.Settings.Default.sucursal <= 0 | (Fundraising_PT.Properties.Settings.Default.sucursal_oid == null | Fundraising_PT.Properties.Settings.Default.sucursal == null) | (Fundraising_PT.Properties.Settings.Default.sucursal_filter == null | Fundraising_PT.Properties.Settings.Default.sucursal_filter == string.Empty)) & Ln_modo == 1)
            {
                MessageBox.Show("No se ha seleccionado ninguna sucursal por defecto..."+Environment.NewLine+"Favor seleccionar una sucursal y guarde los datos.", "Preferencias del Usuario");
            }
            else 
            {
                this.Close();
            }
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

    }
}