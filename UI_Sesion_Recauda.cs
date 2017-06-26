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
    public partial class UI_Sesion_Recauda : DevExpress.XtraEditors.XtraForm
    {
        // declaracion de objetos publicos //
        public DevExpress.XtraBars.BarButtonItem OpcionMenu;
        public DevExpress.XtraBars.BarHeaderItem HeaderMenu;
        public Guid lg_sesion = Guid.Empty;
        public int ln_status_tv = 0;
        public object ObjetoExtra;
        bool ribbon_status_enabled = false;  

        public Timer sesion_timer = new Timer();
        public Timer sesion_timer_aux = new Timer();

        public object ObjetoExtra1;
        public object ObjetoExtra2;
        public object ObjetoExtra3;
        public object ObjetoExtra4; 

        public string lHeader_ant = "";
        public string lAccion = "Sesiones Activas - Navegando";
        public string string_filter_principal = "";
        public DataTable sesiones_activas = new DataTable();

        // declaracion de colecciones de sesiones activas y recaudaciones//
        public DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sesiones> sesiones;
        public DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones> recaudaciones;

        SortingCollection orden_sesiones;

        public UI_Sesion_Recauda(DevExpress.XtraBars.BarButtonItem opcionMenu, DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra, object objetoExtra1, object objetoExtra2, object objetoExtra3, object objetoExtra4)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Sesiones Activas...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //
            OpcionMenu = opcionMenu;
            HeaderMenu = headerMenu;
            ObjetoExtra = objetoExtra;

            ObjetoExtra1 = objetoExtra1;
            ObjetoExtra2 = objetoExtra2;
            ObjetoExtra3 = objetoExtra3;
            ObjetoExtra4 = objetoExtra4;
            
            lHeader_ant = HeaderMenu.Caption;

            // llena los datos de colecciones //
            CriteriaOperator filtro_sesiones = (new OperandProperty("status") == new OperandValue(1)) | (new OperandProperty("status") == new OperandValue(2)) | (new OperandProperty("status") == new OperandValue(3)) | (new OperandProperty("status") == new OperandValue(5));
            sesiones = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(DevExpress.Xpo.XpoDefault.Session, true);
            orden_sesiones = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("id_sesion", DevExpress.Xpo.DB.SortingDirection.Descending));

            // bindeo de datos al bindingsource principal //
            string_filter_principal = Fundraising_PT.Properties.Settings.Default.sucursal_filter + " and (" + filtro_sesiones.ToString().Trim() + ")" ;
            bindingSource1.DataSource = sesiones;
            sesiones.CriteriaString = string_filter_principal;
            sesiones.Sorting = orden_sesiones;
            sesiones.BeginLoad(true);
        }

        private void UI_Sesion_Recauda_Load(object sender, EventArgs e)
        {
            Titulo_Principal.Text = Titulo_Principal.Text + " - Sucursal : " + Fundraising_PT.Properties.Settings.Default.nombre_sucursal;
            //
            if (ObjetoExtra != null)
            {
                Type objetoExtra_type = ObjetoExtra.GetType();
                if (objetoExtra_type.ToString().Trim() == "DevExpress.XtraBars.Ribbon.RibbonControl")
                {
                    ribbon_status_enabled = ((DevExpress.XtraBars.Ribbon.RibbonControl)ObjetoExtra).Enabled;
                    ((DevExpress.XtraBars.Ribbon.RibbonControl)ObjetoExtra).Enabled = false;
                }
            }

            // llena el datatable de sesiones activas y lo asigna al imagelistbox //
            llena_datatable_sesiones();
            this.imageListBoxControl_sesiones_activas.DataSource = sesiones_activas;
            this.imageListBoxControl_sesiones_activas.DisplayMember = "descripcion";
            this.imageListBoxControl_sesiones_activas.ValueMember = "oid";
            this.imageListBoxControl_sesiones_activas.ImageIndexMember = "status";

            // bindeo de eventos necesarios //
            sesion_timer.Interval = 10000000;
            sesion_timer_aux.Interval = 10000000;
            sesion_timer.Stop();
            sesion_timer_aux.Stop(); 
            sesion_timer.Tick += sesion_timer_Tick;
            sesion_timer_aux.Tick += sesion_timer_aux_Tick;
            this.imageListBoxControl_sesiones_activas.Click += new EventHandler(imageListBoxControl_sesiones_activas_Click);
            this.label_salir.Click += new EventHandler(label_salir_Click);
            this.label_reload.Click += label_reload_Click;
            this.picture_view_automatic_datareload.Click += picture_view_automatic_datareload_Click;
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void picture_view_automatic_datareload_Click(object sender, EventArgs e)
        {
            if (sesion_timer.Interval == 10000000 & sesion_timer_aux.Interval == 10000000)
            {
                sesion_timer.Interval = 5000;
                sesion_timer_aux.Interval = 1000;
                //
                sesion_timer.Start();
                sesion_timer_aux.Start();
            }
            else
            {
                sesion_timer.Interval = 10000000;
                sesion_timer_aux.Interval = 10000000;
                //
                sesion_timer.Stop();
                sesion_timer_aux.Stop();
            }
            picture_view_automatic_datareload.Visible = true;
        }

        void sesion_timer_aux_Tick(object sender, EventArgs e)
        {
            if (picture_view_automatic_datareload.Visible == true)
            { picture_view_automatic_datareload.Visible = false; }
            else
            { picture_view_automatic_datareload.Visible = true; }

        }

        void sesion_timer_Tick(object sender, EventArgs e)
        {
            datareload();
        }

        void label_reload_Click(object sender, EventArgs e)
        {
            datareload();
        }

        void label_salir_Click(object sender, EventArgs e)
        {
            this.Close();
            sesiones.EndLoad(sesiones);
            objects_disposes();
        }

        void imageListBoxControl_sesiones_activas_Click(object sender, EventArgs e)
        {
            if (((ImageListBoxControl)sender).SelectedValue != null)
            {
                // se mueve el puntero del bindingSource a la posicion seleccionada del ImageListBoxControl //
                int posi_sel = bindingSource1.Find("oid", ((ImageListBoxControl)sender).SelectedValue);
                bindingSource1.Position = posi_sel;
                //
                try
                {
                    ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)bindingSource1.Current).Reload();

                    if (((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)bindingSource1.Current).status == 1)
                    {
                        this.ControlBox = false;
                        this.imageListBoxControl_sesiones_activas.Enabled = false;
                        Formularios.UI_Recaudacion_Det form_recaudacion_det = new Formularios.UI_Recaudacion_Det(this, 1);
                        form_recaudacion_det.MdiParent = this.MdiParent;
                        form_recaudacion_det.Show();
                    }
                    else
                    {
                        switch (((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)bindingSource1.Current).status)
                        {
                            case 2:
                                MessageBox.Show("No se puede crear una nueva recaudación." + Environment.NewLine + "La sesión seleccionada YA tiene una recaudación en estatus: (RECAUDACION_PARCIAL)." + Environment.NewLine + "Ir al módulo de Consulta y Ajustes.", "Nueva Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            case 3:
                                if (Fundraising_PT.Properties.Settings.Default.U_tipo == 1)
                                {
                                    lg_sesion = ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)bindingSource1.Current).oid;
                                    // Mensaje de opcion de cerrar la sesion con estatus de racaudacion_total.
                                    if (MessageBox.Show("La sesión seleccionada esta en estatus de 'Recaudacion_Total'." + Environment.NewLine + "Desea cerrar la sesión ?", "Cerrar Sesión", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                                    {
                                        // chequea el status de la carga de los totales ventas.
                                        CriteriaOperator filtro_recaudacion = (new OperandProperty("sesion.oid") == new OperandValue(lg_sesion));
                                        DevExpress.Xpo.SortProperty orden_recaudacion = (new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Descending));
                                        recaudaciones = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones>(DevExpress.Xpo.XpoDefault.Session, filtro_recaudacion, orden_recaudacion);
                                        if (recaudaciones.Count > 0)
                                        {
                                            ln_status_tv = recaudaciones[0].status_tv;
                                        }
                                        else
                                        {
                                            ln_status_tv = 0;
                                        }
                                        //
                                        if (ln_status_tv == 2 | ln_status_tv == 3 | ln_status_tv == 5)
                                        {
                                            ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)bindingSource1.Current).status = 4;
                                            ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)bindingSource1.Current).Save();
                                            // vuelve a llenar el datatable de sesiones activas//
                                            datareload();
                                        }
                                        else
                                        {
                                            MessageBox.Show("No se puede cerrar la sesión." + Environment.NewLine + "La sesión debe tener en status (Cerrada Normal, Carrada Ajustada o Cerrrada sin Montos) los totales de ventas." + Environment.NewLine + "Ir al módulo de Consulta y Ajustes.", "Cerrar Sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No se puede cerrar la sesión." + Environment.NewLine + "El usuario NO tiene el nivel de autorización.", "Cerrar Sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                break;
                            case 4:
                                MessageBox.Show("La sesión YA se encuentra en estatus: (CERRADA).", "Cerrar Sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            case 5:
                                MessageBox.Show("No se puede crear una nueva recaudación." + Environment.NewLine + "La sesión seleccionada tiene en estos momentos una recaudación en curso, estatus: (EN_PROCESO).", "Nueva Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            default:
                                MessageBox.Show("La sesión seleccionada esta con estatus: (NINGUNO)." + Environment.NewLine + "Comunicarse con el administrador del sistema.", "Nueva Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                        }
                        datareload();
                    }
                }
                catch (Exception)
                {
                    Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("Data Reload", "Cancelar", "Inactivo");
                    switch (MessageBox.Show("No se pudo actualizar los datos de la sesión seleccionada desde el servidor..." + Environment.NewLine + Environment.NewLine + "Seleccione una opción para continuar ?" + Environment.NewLine + Environment.NewLine + "Data Reload : Vuelve a cargar la colección de datos de las sesiones desde el servidor." + Environment.NewLine + Environment.NewLine + "Cancelar : Cancela la operación.", "Nueva Recaudación", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        case DialogResult.OK:
                            datareload();
                            break;
                        default:
                            break;
                    }
                    Fundraising_PT.Clases.MsgBoxUtil.UnHackMessageBox();
                }
                //
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            HeaderMenu.Caption = Fundraising_PT.Properties.Settings.Default.Nombre_Sistema.Trim() + " - Módulo : " + this.Text.Trim() + " - Acción : " + this.lAccion;
            this.imageListBoxControl_sesiones_activas.SelectionMode = SelectionMode.MultiSimple;
            this.imageListBoxControl_sesiones_activas.UnSelectAll();
            this.imageListBoxControl_sesiones_activas.SelectionMode = SelectionMode.One;
            //
            if (sesion_timer.Interval == 5000 & sesion_timer_aux.Interval == 1000)
            {
                sesion_timer.Start();
                sesion_timer_aux.Start();
            }
            else
            {
                sesion_timer.Interval = 10000000;
                sesion_timer_aux.Interval = 10000000;
                sesion_timer.Stop();
                sesion_timer_aux.Stop();
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            //
            sesion_timer.Interval = 10000000;
            sesion_timer_aux.Interval = 10000000;
            sesion_timer.Stop();
            sesion_timer_aux.Stop();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (OpcionMenu != null)
            { OpcionMenu.Enabled = true; }

            //if (ObjetoExtra != null)
            //{
            //    ((DevExpress.XtraBars.BarButtonItem)ObjetoExtra).Enabled = true;
            //}
            //if (ObjetoExtra1 != null)
            //{
            //    ((DevExpress.XtraBars.BarButtonItem)ObjetoExtra1).Enabled = true;
            //}
            //if (ObjetoExtra2 != null)
            //{
            //    ((DevExpress.XtraBars.BarButtonItem)ObjetoExtra2).Enabled = true;
            //}
            //if (ObjetoExtra3 != null)
            //{
            //    ((DevExpress.XtraBars.BarButtonItem)ObjetoExtra3).Enabled = true;
            //}
            //if (ObjetoExtra4 != null)
            //{
            //    ((DevExpress.XtraBars.BarButtonItem)ObjetoExtra4).Enabled = true;
            //}

            if (ObjetoExtra != null)
            {
                Type objetoExtra_type = ObjetoExtra.GetType();
                if (objetoExtra_type.ToString().Trim() == "DevExpress.XtraBars.Ribbon.RibbonControl")
                {
                    ((DevExpress.XtraBars.Ribbon.RibbonControl)ObjetoExtra).Enabled = ribbon_status_enabled;
                }
            }

            HeaderMenu.Caption = this.lHeader_ant;
        }

        public void llena_datatable_sesiones()
        {
            // se crean las columnas al datatable de las sesiones activas
            if (sesiones_activas.Columns.Count <= 0)
            {
                sesiones_activas.Columns.Add("oid", typeof(Guid));
                sesiones_activas.Columns.Add("oid_caja", typeof(Guid));
                sesiones_activas.Columns.Add("caja", typeof(DevExpress.Xpo.XPBaseObject));
                sesiones_activas.Columns.Add("oid_cajero", typeof(Guid));
                sesiones_activas.Columns.Add("cajero", typeof(DevExpress.Xpo.XPBaseObject));
                sesiones_activas.Columns.Add("nombre_caja", typeof(string));
                sesiones_activas.Columns.Add("nombre_cajero", typeof(string));
                sesiones_activas.Columns.Add("descripcion", typeof(string));
                sesiones_activas.Columns.Add("id_sesion", typeof(int));
                sesiones_activas.Columns.Add("fecha_hora", typeof(DateTime));
                sesiones_activas.Columns.Add("status", typeof(int));
            }
            sesiones_activas.Rows.Clear();

            // se recorre el collection sesiones y se llena el datatable
            foreach (Fundraising_PTDM.FUNDRAISING_PT.Sesiones sesion in sesiones)
            {
                string l_status = ((Fundraising_PTDM.Enums.EStatus_sesion)sesion.status).ToString().Trim();
                string l_descripcion = " Id Sesión: " + sesion.id_sesion.ToString().Trim() + "\n Caja: " + sesion.caja.nombre.Trim() + "\n Cajero: " + sesion.cajero.cajero.Trim() + "\n Fecha y Hora: " + sesion.fecha_hora.ToString().Trim() + "\n Status: " + l_status;

                // inserto una nueva fila en el datatable //
                sesiones_activas.Rows.Add(sesion.oid, sesion.caja.oid, sesion.caja, sesion.cajero.oid, sesion.cajero, sesion.caja.nombre, sesion.cajero.cajero, l_descripcion, sesion.id_sesion, sesion.fecha_hora, sesion.status);
                sesiones_activas.DefaultView.Sort = "id_sesion desc";
            }
        }

        public void datareload()
        {
            sesiones.Load();
            sesiones.Reload();
            llena_datatable_sesiones();
            //
            bindingSource1.MoveFirst();
            //
            this.imageListBoxControl_sesiones_activas.DataSource = sesiones_activas;
            this.imageListBoxControl_sesiones_activas.DisplayMember = "descripcion";
            this.imageListBoxControl_sesiones_activas.ValueMember = "oid";
            this.imageListBoxControl_sesiones_activas.ImageIndexMember = "status";
            //
            this.imageListBoxControl_sesiones_activas.Refresh();
        }

        public void objects_disposes()
        {
            sesiones.Dispose();
            if (recaudaciones != null)
            {
                recaudaciones.Dispose();
            }
            sesiones_activas.Dispose();
            bindingSource1.Dispose();
        }

    }
}