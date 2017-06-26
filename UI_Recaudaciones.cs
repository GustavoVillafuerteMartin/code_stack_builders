using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Recaudaciones : Fundraising_PT.Form_Mant_Base1
    {
        public string lc_codigointegrado = string.Empty;
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales> sucursales =
            new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session, true);
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sesiones> sesiones =
               new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(DevExpress.Xpo.XpoDefault.Session, true);
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> usuarios =
               new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, true);
      
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones> recaudaciones;
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudacion_Det> recaudaciones_det_aux;

        private CriteriaOperator filtro_recaudaciones;
        private SortingCollection orden_recaudaciones;

        Fundraising_PTDM.FUNDRAISING_PT.Sesiones current_sesion;

        public string string_filter_principal = "";
        public string current_sesion_state = string.Empty;
        public string this_primary_object_persistent_current_state = "None";

        public UI_Recaudaciones(DevExpress.XtraBars.BarButtonItem opcionMenu, ref DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra, object objetoExtra1, object objetoExtra2, object objetoExtra3, object objetoExtra4)
            : base(opcionMenu, ref headerMenu, objetoExtra, objetoExtra1, objetoExtra2, objetoExtra3, objetoExtra4)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Consulta y Ajustes Recaudaciones...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            InitializeComponent();
            //
            this.lookUpEdit_sucursales.Properties.DataSource = Fundraising_PT.Clases.Setting_Sucursales.data_sucursales();
            //
            recaudaciones = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones>(DevExpress.Xpo.XpoDefault.Session, true);
            orden_recaudaciones = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("fecha_hora", DevExpress.Xpo.DB.SortingDirection.Descending));
            switch (Fundraising_PT.Properties.Settings.Default.U_tipo)
            {
                case 3:
                    filtro_recaudaciones = (new OperandProperty("usuario.oid") == new OperandValue(Fundraising_PT.Properties.Settings.Default.U_oid));
                    break;
                default:
                    filtro_recaudaciones = CriteriaOperator.Parse("1=1");
                    break;
            }
            //
            //recaudaciones = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones>(DevExpress.Xpo.XpoDefault.Session, filtro_recaudaciones, orden_recaudaciones);
            //bindingSource1.DataSource = recaudaciones;
            //
            // bindeo de datos al bindingsource principal //
            string_filter_principal = Fundraising_PT.Properties.Settings.Default.sucursal_filter + " and (" + filtro_recaudaciones.ToString().Trim() + ")";
            bindingSource1.DataSource = recaudaciones;
            recaudaciones.CriteriaString = string_filter_principal;
            recaudaciones.Sorting = orden_recaudaciones;
            bindingSource1.MoveFirst();
            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
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

            if (ObjetoExtra1 != null)
            {
                ((DevExpress.XtraBars.BarButtonItem)ObjetoExtra1).Enabled = true;
            }
            if (ObjetoExtra2 != null)
            {
                ((DevExpress.XtraBars.BarButtonItem)ObjetoExtra2).Enabled = true;
            }
            if (ObjetoExtra3 != null)
            {
                ((DevExpress.XtraBars.BarButtonItem)ObjetoExtra3).Enabled = true;
            }
            if (ObjetoExtra4 != null)
            {
                ((DevExpress.XtraBars.BarButtonItem)ObjetoExtra4).Enabled = true;
            }
            
            
            HeaderMenu.Caption = this.lHeader_ant;
        }

        private void UI_Recaudaciones_Load(object sender, EventArgs e)
        {
            filter_sucursales();
            bindingSource1.MoveFirst();
            viewcodigointegrado();
            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
            if (this_primary_object_persistent_current != null)
            {
                this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                if (current_sesion != null)
                {
                    current_sesion_state = ((XPBaseObject)current_sesion).ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                }
            }
            //
            this.lookUpEdit_sucursales.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 1);
            this.lookUp_sesion.SetDataSource(sesiones, "id_sesion", "oid");
            this.dateTime_fecha_hora.SetBinding();
            this.lookUp_usuario.SetDataSource(usuarios, "usuario", "oid");
            this.lookUp_supervisor.SetDataSource(usuarios, "usuario", "oid");
            this.lookUp_status_sesion.SetDataSource();
            //
            this.lookUp_sucursal.Properties.DataSource = sucursales;
            this.lookUp_sucursal.Properties.DisplayMember = "nombre";
            this.lookUp_sucursal.Properties.ValueMember = "codigo";
            this.lookUp_sucursal.DataBindings.Clear();
            this.lookUp_sucursal.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", bindingSource1, "sucursal"));
            //
            this.textBox_s_fiscal.textEdit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource1, "sesion.s_fiscal", true));
            this.textBox_z_fiscal.textEdit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource1, "sesion.z_fiscal", true));
            //
            seteo_status_recaudacion();
            seteo_status_totalventa();
            //
            this.grid_Base11.gridView1.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(gridView1_CustomColumnDisplayText);
            this.grid_Base11.gridView1.ViewCaption = "Listado de Recaudaciones";
            //            
            this.lookUp_sesion.gridLookUpEdit1.EditValueChanged += new EventHandler(gridLookUpEdit1_EditValueChanged);
            this.gridLookUpEdit1_EditValueChanged(null, null);
            this.simpleButton_recaudacion.Click +=new EventHandler(simpleButton_recaudacion_Click);
            this.simpleButton_recaudacion2.Click += new EventHandler(simpleButton_recaudacion_Click);
            this.simpleButton_datos_ventas.Click += new EventHandler(simpleButton_datos_ventas_Click);
            this.simpleButton_datos_ventas2.Click += new EventHandler(simpleButton_datos_ventas_Click);
            this.simpleButton_editar_informacion_fiscal.Click += new EventHandler(simpleButton_editar_informacion_fiscal_Click);
            //
            if (Fundraising_PT.Properties.Settings.Default.Activa_Audio == 1)
            {
                this.simpleButton_recaudacion.MouseEnter += new EventHandler(simpleButton_recaudacion_MouseEnter);
                this.simpleButton_recaudacion2.MouseEnter += new EventHandler(simpleButton_recaudacion_MouseEnter);
                this.simpleButton_datos_ventas.MouseEnter += new EventHandler(simpleButton_recaudacion_MouseEnter);
                this.simpleButton_datos_ventas2.MouseEnter += new EventHandler(simpleButton_recaudacion_MouseEnter);
                this.simpleButton_editar_informacion_fiscal.MouseEnter += new EventHandler(simpleButton_recaudacion_MouseEnter);
            }
            //
            this.bindingSource1.CurrentItemChanged += new EventHandler(bindingSource1_CurrentItemChanged);
            textBox_buscacodigointegrado.Validated += textBox_buscacodigointegrado_Validated;
            textBox_buscacodigointegrado.textEdit1.KeyPress += textEdit1_KeyPress;

            if (bindingSource1.Count <= 0)
            {
                //this.barra_Mant_Base11.Enabled = false;
                this.simpleButton_recaudacion.Enabled = false;
                this.simpleButton_recaudacion2.Enabled = false;
                this.simpleButton_datos_ventas.Enabled = false;
                this.simpleButton_datos_ventas2.Enabled = false;
                this.simpleButton_editar_informacion_fiscal.Enabled = false;
            }
            else 
            {
                this.simpleButton_recaudacion.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
                this.simpleButton_recaudacion2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                this.simpleButton_datos_ventas.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                this.simpleButton_datos_ventas2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
                this.simpleButton_editar_informacion_fiscal.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
            //if (Fundraising_PT.Properties.Settings.Default.U_tipo != 1 & Fundraising_PT.Properties.Settings.Default.U_tipo != 3)
            //{
            //    this.simpleButton_recaudacion.Enabled = false;
            //    this.simpleButton_recaudacion2.Enabled = false;
            //}
            //if (Fundraising_PT.Properties.Settings.Default.U_tipo != 1 & Fundraising_PT.Properties.Settings.Default.U_tipo != 2)
            //{
            //    this.simpleButton_datos_ventas.Enabled = false;
            //    this.simpleButton_datos_ventas2.Enabled = false;
            //}
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
                XPView vbuscacodigo = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones));
                vbuscacodigo.AddProperty("void", "oid", true, true, DevExpress.Xpo.SortDirection.None);
                vbuscacodigo.AddProperty("vcodigo_integrado", "Trim(ToStr(sucursal))+Trim(sesion.caja.codigo)+Trim(sesion.cajero.codigo)+Trim(ToStr(sesion.id_sesion))", true, true, DevExpress.Xpo.SortDirection.None);
                vbuscacodigo.CriteriaString = string.Format("Trim(ToStr(sucursal))+Trim(sesion.caja.codigo)+Trim(sesion.cajero.codigo)+Trim(ToStr(sesion.id_sesion)) = '{0}'", textBox_buscacodigointegrado.lValue.Trim());
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

        public override void bindingSource1_PositionChanged(object sender, EventArgs e)
        {
            base.bindingSource1_PositionChanged(sender, e);
            //
            try
            {
                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                if (this_primary_object_persistent_current != null)
                {
                    this_primary_object_persistent_current.Reload();
                    this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                    current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                    if (current_sesion != null)
                    {
                        current_sesion.Reload();
                        current_sesion_state = ((XPBaseObject)current_sesion).ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                    }
                }
                //
                viewcodigointegrado();
                seteo_status_recaudacion();
                seteo_status_totalventa();
                //
                if (bindingSource1.Count <= 0)
                {
                    //this.barra_Mant_Base11.Enabled = false;
                    this.simpleButton_recaudacion.Enabled = false;
                    this.simpleButton_recaudacion2.Enabled = false;
                    this.simpleButton_datos_ventas.Enabled = false;
                    this.simpleButton_datos_ventas2.Enabled = false;
                    this.simpleButton_editar_informacion_fiscal.Enabled = false;
                }
                else
                {
                    //this.barra_Mant_Base11.Enabled = true;
                    this.simpleButton_recaudacion.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
                    this.simpleButton_recaudacion2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                    this.simpleButton_datos_ventas.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                    this.simpleButton_datos_ventas2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
                    this.simpleButton_editar_informacion_fiscal.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                }
            }
            catch (Exception)
            {
                Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("Data Reload", "Continuar", "Ignorar");
                switch (MessageBox.Show("No se pudo leer los datos desde el servidor para el registro actual..." + Environment.NewLine + Environment.NewLine + "Posiblemente otro usuario lo elimino o bloqueo, mientras usted lo tenia seleccionado para editarlo !!!" + Environment.NewLine + Environment.NewLine + "Seleccione una opción para continuar ?" + Environment.NewLine + Environment.NewLine + "Data Reload : Ejecutar (Data Reload) Vuelve a cargar todas las colecciones de datos desde el servidor." + Environment.NewLine + Environment.NewLine + "Cancelar : Cancela la edicion de datos del registro actual y salta al siguiente registro. " + Environment.NewLine + Environment.NewLine + "Ignorar : Ignora la lectura de los datos del registro desde el servidor y permanece sobre el.", "Data Reload (Navegando)", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Yes:
                        this.datareload();
                        bindingSource1.MoveFirst();
                        this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                        if (this_primary_object_persistent_current != null)
                        {
                            this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                            current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                            if (current_sesion != null)
                            {
                                current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                            }
                        }
                        break;
                    case DialogResult.No:
                        if (bindingSource1.Count <= 0)
                        {
                            bindingSource1.MoveFirst();
                            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                            if (this_primary_object_persistent_current != null)
                            {
                                this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                if (current_sesion != null)
                                {
                                    current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                }
                            }
                            break;
                        }
                        if (bindingSource1.Count > 0 & bindingSource1.Position >= bindingSource1.Count)
                        {
                            bindingSource1.MovePrevious();
                            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                            if (this_primary_object_persistent_current != null)
                            {
                                this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                if (current_sesion != null)
                                {
                                    current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                }
                            }
                            break;
                        }
                        if (bindingSource1.Count > 0 & bindingSource1.Position == 0)
                        {
                            bindingSource1.MoveNext();
                            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                            if (this_primary_object_persistent_current != null)
                            {
                                this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                if (current_sesion != null)
                                {
                                    current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                }
                            }
                            break;
                        }
                        if (bindingSource1.Count > 0 & (bindingSource1.Position > 0 & bindingSource1.Position < bindingSource1.Count))
                        {
                            bindingSource1.MoveNext();
                            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                            if (this_primary_object_persistent_current != null)
                            {
                                this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                if (current_sesion != null)
                                {
                                    current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                }
                            }
                            break;
                        }
                        else
                        {
                            bindingSource1.MoveFirst();
                            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                            if (this_primary_object_persistent_current != null)
                            {
                                this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                if (current_sesion != null)
                                {
                                    current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                }
                            }
                            break;
                        }
                }
                Fundraising_PT.Clases.MsgBoxUtil.UnHackMessageBox();
            }
        }
        
        void simpleButton_editar_informacion_fiscal_Click(object sender, EventArgs e)
        {
            if (this.simpleButton_editar_informacion_fiscal.Text == "Editar")
            {
                try
                {
                    if (this_primary_object_persistent_current != null & current_sesion != null)
                    {
                        this_primary_object_persistent_current.Reload();
                        current_sesion.Reload();

                        this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                        current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();

                        this.textBox_s_fiscal.Refresh();
                        this.textBox_z_fiscal.Refresh();
                        //
                        //this_primary_object_persistent_current.Session.LockingOption = LockingOption.Optimistic;
                        //this_primary_object_persistent_current.Session.OptimisticLockingReadBehavior = OptimisticLockingReadBehavior.MergeCollisionThrowException;
                        //this_primary_object_persistent_current.Session.ExplicitBeginTransaction();
                        //this_primary_object_persistent_current.Session.BeginTrackingChanges();
                        //
                        current_sesion.Session.LockingOption = LockingOption.Optimistic;
                        current_sesion.Session.OptimisticLockingReadBehavior = OptimisticLockingReadBehavior.MergeCollisionThrowException;
                        current_sesion.Session.ExplicitBeginTransaction();
                        current_sesion.Session.BeginTrackingChanges();
                        //

                        this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                        current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();

                        this.lAccion = "Editar";
                        this.simpleButton_datareload.Enabled = false;
                        this.lookUpEdit_sucursales.Enabled = false;
                        this.barra_Mant_Base11.Enabled = false;
                        this.simpleButton_recaudacion.Enabled = false;
                        this.simpleButton_recaudacion2.Enabled = false;
                        this.simpleButton_datos_ventas.Enabled = false;
                        this.simpleButton_datos_ventas2.Enabled = false;
                        this.grid_Base11.Enabled = false;
                        this.ControlBox = false;
                        //
                        this.textBox_s_fiscal.Enabled = true;
                        this.textBox_z_fiscal.Enabled = true;
                        this.textBox_s_fiscal.Focus();
                        //
                        this.simpleButton_editar_informacion_fiscal.Image = Fundraising_PT.Properties.Resources.Guardar_Mozilla;
                        this.simpleButton_editar_informacion_fiscal.Text = "Guardar";
                    }
                }
                catch (Exception)
                {
                    Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("Data Reload", "Continuar", "Ignorar");
                    switch (MessageBox.Show("No se pudo leer los datos desde el servidor para el registro actual..." + Environment.NewLine + Environment.NewLine + "Posiblemente otro usuario lo elimino o bloqueo, mientras usted lo tenia seleccionado para editarlo !!!" + Environment.NewLine + Environment.NewLine + "Seleccione una opción para continuar ?" + Environment.NewLine + Environment.NewLine + "Data Reload : Ejecutar (Data Reload) Vuelve a cargar todas las colecciones de datos desde el servidor." + Environment.NewLine + Environment.NewLine + "Cancelar : Cancela la edicion de datos del registro actual y salta al siguiente registro. " + Environment.NewLine + Environment.NewLine + "Ignorar : Ignora la lectura de los datos del registro desde el servidor y permanece sobre el.", "Data Reload (Editar)", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        case DialogResult.Yes:
                            this.datareload();
                            bindingSource1.MoveFirst();
                            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                            if (this_primary_object_persistent_current != null)
                            {
                                this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                if (current_sesion != null)
                                {
                                    current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                }
                            }
                            break;
                        case DialogResult.No:
                            if (bindingSource1.Count <= 0)
                            {
                                bindingSource1.MoveFirst();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                if (this_primary_object_persistent_current != null)
                                {
                                    this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                    current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                    if (current_sesion != null)
                                    {
                                        current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                    }
                                }
                                break;
                            }
                            if (bindingSource1.Count > 0 & bindingSource1.Position >= bindingSource1.Count)
                            {
                                bindingSource1.MovePrevious();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                if (this_primary_object_persistent_current != null)
                                {
                                    this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                    current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                    if (current_sesion != null)
                                    {
                                        current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                    }
                                }
                                break;
                            }
                            if (bindingSource1.Count > 0 & bindingSource1.Position == 0)
                            {
                                bindingSource1.MoveNext();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                if (this_primary_object_persistent_current != null)
                                {
                                    this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                    current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                    if (current_sesion != null)
                                    {
                                        current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                    }
                                }
                                break;
                            }
                            if (bindingSource1.Count > 0 & (bindingSource1.Position > 0 & bindingSource1.Position < bindingSource1.Count))
                            {
                                bindingSource1.MoveNext();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                if (this_primary_object_persistent_current != null)
                                {
                                    this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                    current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                    if (current_sesion != null)
                                    {
                                        current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                    }
                                }
                                break;
                            }
                            else
                            {
                                bindingSource1.MoveFirst();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                if (this_primary_object_persistent_current != null)
                                {
                                    this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                    current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                    if (current_sesion != null)
                                    {
                                        current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                    }
                                }
                                break;
                            }
                    }
                    Fundraising_PT.Clases.MsgBoxUtil.UnHackMessageBox();
                    //
                    this.lAccion = "Navegando";
                    this.simpleButton_datareload.Enabled = true;
                    this.lookUpEdit_sucursales.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 1);
                    this.barra_Mant_Base11.Enabled = true;
                    seteo_status_recaudacion();
                    seteo_status_totalventa();
                    this.grid_Base11.Enabled = true;
                    this.ControlBox = true;
                    //
                    this.simpleButton_recaudacion.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
                    this.simpleButton_recaudacion2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                    this.simpleButton_datos_ventas.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                    this.simpleButton_datos_ventas2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
                    this.simpleButton_editar_informacion_fiscal.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                    //
                    this.textBox_s_fiscal.Enabled = false;
                    this.textBox_z_fiscal.Enabled = false;
                    //
                    this.simpleButton_editar_informacion_fiscal.Image = Fundraising_PT.Properties.Resources.Modificar_Mozilla;
                    this.simpleButton_editar_informacion_fiscal.Text = "Editar";
                }
            }
            else
            {
                if (this.error_validation())
                {
                    try
                    {

                        ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.s_fiscal = this.textBox_s_fiscal.textEdit1.Text;
                        ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.z_fiscal = this.textBox_z_fiscal.textEdit1.Text;
                        this_primary_object_persistent_current.Save();
                        //this_primary_object_persistent_current.Session.FlushChanges();
                        //this_primary_object_persistent_current.Session.ExplicitCommitTransaction();
                        //
                        current_sesion.s_fiscal = this.textBox_s_fiscal.textEdit1.Text;
                        current_sesion.z_fiscal = this.textBox_z_fiscal.textEdit1.Text;
                        current_sesion.Save();
                        current_sesion.Session.FlushChanges();
                        current_sesion.Session.ExplicitCommitTransaction();
                        //
                        bindingSource1.EndEdit();
                        //
                        this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                        if (this_primary_object_persistent_current != null)
                        {
                            this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                            current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                            if (current_sesion != null)
                            {
                                current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                            }
                        }
                        //
                        if (this_primary_object_persistent_current != null & current_sesion != null)
                        {
                            this_primary_object_persistent_current.Reload();
                            current_sesion.Reload();

                            this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                            current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                        }
                        //
                        this.lAccion = "Navegando";
                        this.simpleButton_datareload.Enabled = true;
                        this.lookUpEdit_sucursales.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 1);
                        this.barra_Mant_Base11.Enabled = true;
                        seteo_status_recaudacion();
                        this.grid_Base11.Enabled = true;
                        this.ControlBox = true;
                        //
                        this.simpleButton_recaudacion.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
                        this.simpleButton_recaudacion2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                        this.simpleButton_datos_ventas.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                        this.simpleButton_datos_ventas2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
                        this.simpleButton_editar_informacion_fiscal.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                        //
                        this.textBox_s_fiscal.Enabled = false;
                        this.textBox_z_fiscal.Enabled = false;
                        //
                        this.simpleButton_editar_informacion_fiscal.Image = Fundraising_PT.Properties.Resources.Modificar_Mozilla;
                        this.simpleButton_editar_informacion_fiscal.Text = "Editar";

                    }
                    catch (Exception)
                    {
                        Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("DataR-Editar", "Cancelar", "Inactivo");
                        switch (MessageBox.Show("Otro Usuario modifico o elimino el registro, mientras usted lo editaba..." + Environment.NewLine + Environment.NewLine + "Seleccione una opción para continuar ?" + Environment.NewLine + Environment.NewLine + "DataR-Editar : Vuelve a cargar los datos del registro desde el servidor y lo deja abierto en edición." + Environment.NewLine + Environment.NewLine + "Cancelar : Cancela la edición de datos y Vuelve a cargar los datos del registro desde el servidor.", "Guardar Cambios", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {
                            case DialogResult.OK:
                                current_sesion.Session.DropChanges();
                                current_sesion.Session.ExplicitRollbackTransaction();
                                //this_primary_object_persistent_current.Session.DropChanges();
                                //this_primary_object_persistent_current.Session.ExplicitRollbackTransaction();
                                bindingSource1.ResetCurrentItem();
                                bindingSource1.CancelEdit();
                                this_primary_object_persistent_current.Reload();
                                current_sesion.Reload();
                                this.textBox_s_fiscal.Refresh();
                                this.textBox_z_fiscal.Refresh();
                                //this_primary_object_persistent_current.Session.ExplicitBeginTransaction();
                                //this_primary_object_persistent_current.Session.BeginTrackingChanges();
                                current_sesion.Session.ExplicitBeginTransaction();
                                current_sesion.Session.BeginTrackingChanges();

                                this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                                                
                                this.textBox_s_fiscal.Focus();
                                break;
                            default:
                                //this_primary_object_persistent_current.Session.DropChanges();
                                //this_primary_object_persistent_current.Session.ExplicitRollbackTransaction();
                                current_sesion.Session.DropChanges();
                                current_sesion.Session.ExplicitRollbackTransaction();
                                bindingSource1.ResetCurrentItem();
                                bindingSource1.CancelEdit();
                                this_primary_object_persistent_current.Reload();
                                current_sesion.Reload();
                                this.textBox_s_fiscal.Refresh();
                                this.textBox_z_fiscal.Refresh();
                                //
                                this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                                                
                                this.lAccion = "Navegando";
                                this.simpleButton_datareload.Enabled = true;
                                this.lookUpEdit_sucursales.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 1);
                                this.barra_Mant_Base11.Enabled = true;
                                seteo_status_recaudacion();
                                seteo_status_totalventa();
                                this.grid_Base11.Enabled = true;
                                this.ControlBox = true;
                                //
                                this.simpleButton_recaudacion.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
                                this.simpleButton_recaudacion2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                                this.simpleButton_datos_ventas.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                                this.simpleButton_datos_ventas2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
                                this.simpleButton_editar_informacion_fiscal.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                                //
                                this.textBox_s_fiscal.Enabled = false;
                                this.textBox_z_fiscal.Enabled = false;
                                //
                                this.simpleButton_editar_informacion_fiscal.Image = Fundraising_PT.Properties.Resources.Modificar_Mozilla;
                                this.simpleButton_editar_informacion_fiscal.Text = "Editar";
                                //
                                break;
                        }
                        Fundraising_PT.Clases.MsgBoxUtil.UnHackMessageBox();
                    }
                }
            }
        }

        void simpleButton_recaudacion_MouseEnter(object sender, EventArgs e)
        {
            Fundraising_PT.Clases.Voice_message.msg_voice(((DevExpress.XtraEditors.SimpleButton)sender).ToolTipTitle);
        }

        void bindingSource1_CurrentItemChanged(object sender, EventArgs e)
        {
            this.seteo_status_recaudacion();
            this.seteo_status_totalventa();
        }

        void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (bindingSource1.Count > 0 && this.lookUp_sesion.gridLookUpEdit1.EditValue!=null)
            {
                this.textBox_caja.textEdit1.EditValue = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(this.lookUp_sesion.Value).caja.nombre;
                this.textBox_cajero.textEdit1.EditValue = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(this.lookUp_sesion.Value).cajero.cajero;
                this.dateTime_fecha_hora_sesion.dateEdit1.DateTime = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(this.lookUp_sesion.Value).fecha_hora;
                this.lookUp_status_sesion.gridLookUpEdit1.EditValue = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(this.lookUp_sesion.Value).status;
                this.textBox_s_fiscal.textEdit1.Text = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(this.lookUp_sesion.Value).s_fiscal;
                this.textBox_z_fiscal.textEdit1.Text = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(this.lookUp_sesion.Value).z_fiscal;
                //if (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)bindingSource1.Current).sesion.status == 4)
                //{
                //    this.simpleButton_editar_informacion_fiscal.Enabled = false;
                //}
                //else 
                //{
                //    this.simpleButton_editar_informacion_fiscal.Enabled = true;
                //}
            }
            else
            {
                this.textBox_caja.textEdit1.EditValue = "";
                this.textBox_cajero.textEdit1.EditValue = "";
                this.dateTime_fecha_hora_sesion.dateEdit1.DateTime = System.DateTime.MinValue;
                this.lookUp_status_sesion.gridLookUpEdit1.EditValue = 0;
                this.textBox_s_fiscal.textEdit1.EditValue = "";
                this.textBox_z_fiscal.textEdit1.EditValue = "";
                this.simpleButton_editar_informacion_fiscal.Enabled = false;
            }
        }

        void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e != null && e.Column.FieldName == "fecha_hora")
            {
                e.Column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                e.Column.DisplayFormat.FormatString = "g";
            }

            if (e != null && e.Column.FieldName == "status")
            {
                e.DisplayText = ((Fundraising_PTDM.Enums.EStatus_recaudacion)e.Value).ToString();
                e.Column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            }

            if (e != null && e.Column.FieldName == "status_tv")
            {
                e.DisplayText = ((Fundraising_PTDM.Enums.EStatus_recaudacion)e.Value).ToString();
                e.Column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            }

            if (e != null && e.Column.FieldName == "sesion.fecha_hora")
            {
                e.Column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                e.Column.DisplayFormat.FormatString = "g";
            }

            if (e != null && e.Column.FieldName == "sesion.status")
            {
                e.DisplayText = ((Fundraising_PTDM.Enums.EStatus_sesion)e.Value).ToString();
                e.Column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            }

            if (e != null && e.Column.FieldName == "sucursal")
            {
                var lc_nombre_sucursal = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("select nombre from sucursales where codigo = {0}", e.Value));
                e.DisplayText = (lc_nombre_sucursal == null ? "[ Vacio ]" : lc_nombre_sucursal.ResultSet[0].Rows[0].Values[0].ToString());
            }
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

        public override void eliminar(object sender, EventArgs e)
        {
            //base.eliminar(sender, e);
            try
            {
                this_primary_object_persistent_current.Reload();
                if (this_primary_object_persistent_current != null)
                {
                    this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                    if (current_sesion != null)
                    {
                        current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                    }
                }
                //
                if (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).status != 4 & ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).status != 0)
                {
                    if (Fundraising_PT.Properties.Settings.Default.U_tipo == 1)
                    {
                        if (MessageBox.Show("Al Anular la recaudación Seleccionada," + "\n" + "no podra modificarla o ajustarla," + "\n" + "solo podra consultarla." + "\n" + "\n" + "La sesión asosiada volvera a estar en Estatus 'INICIADA'," + "\n" + "y se podra realizar otra recaudación a la sesión." + "\n" + "\n" + "Esta seguro de ANULAR la Recaudación ?", "Anular Recaudación", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                        {
                            if (DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(this.lookUp_sesion.Value) != null)
                            {
                                int sw = 0;
                                DevExpress.Xpo.XpoDefault.Session.BeginTransaction();
                                try
                                {
                                    // cambio status 'Anulado' a la recaudacion y a la totalizacion de ventas //
                                    ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).status = 4;
                                    ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).status_tv = 4;
                                    this_primary_object_persistent_current.Save();
                                    // cambio status 'Iniciada' a la sesion //
                                    DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(this.lookUp_sesion.Value).status = 1;
                                    DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(this.lookUp_sesion.Value).Save();
                                    this.lookUp_status_sesion.gridLookUpEdit1.EditValue = 1;
                                    //
                                    DevExpress.Xpo.XpoDefault.Session.CommitTransaction();
                                    //
                                    if (this_primary_object_persistent_current != null)
                                    {
                                        this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                        if (current_sesion != null)
                                        {
                                            current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                        }
                                    }

                                    sw = 1;
                                    seteo_status_recaudacion();
                                    seteo_status_totalventa();
                                    MessageBox.Show("Se anulo la recaudación satisfactoriamente y se volvio a iniciar la sesión...", "Anular Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                catch (Exception oerror)
                                {
                                    sw = 0;
                                    MessageBox.Show("Ocurrio un ERROR durante el proceso de anulación, se reversara dicho proceso..." + "\n" + "Error: " + oerror.Message, "Anular Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    DevExpress.Xpo.XpoDefault.Session.RollbackTransaction();
                                    if (this_primary_object_persistent_current != null)
                                    {
                                        this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                        if (current_sesion != null)
                                        {
                                            current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                        }
                                    }
                                }

                                if (sw == 1) // anulo la recaudacion satisfactoriamente y procede a recostruir los saldos para depositar //
                                {
                                    try
                                    {
                                        // se inicia la transaccion para reconstruir columna de recaudado en la tabla de saldos//
                                        DevExpress.Xpo.XpoDefault.Session.BeginTransaction();
                                        //
                                        // reconstruye los saldos recaudado en la tabla "saldos_recauda_dep" 
                                        DateTime aux_fecha_hora = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).fecha_hora;
                                        Fundraising_PTDM.FUNDRAISING_PT.Usuarios aux_recaudador = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).usuario;
                                        //
                                        string filtro_formas_pago = " forma_pago.tpago = 1 or forma_pago.tpago = 3 or forma_pago.tpago = 7 ";
                                        recaudaciones_det_aux = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).Recaudacion_Det;
                                        recaudaciones_det_aux.Criteria = CriteriaOperator.Parse(filtro_formas_pago);
                                        //
                                        foreach (var item_recaudacion_det in recaudaciones_det_aux)
                                        {
                                            Fundraising_PT.Clases.saldos_recaudaciones_depositos.reconstruir_saldos_recaudaciones_depositos(1, aux_fecha_hora, aux_recaudador, item_recaudacion_det.forma_pago);
                                        }
                                        //
                                        DevExpress.Xpo.XpoDefault.Session.CommitTransaction();
                                        //
                                    }
                                    catch (Exception oerror)
                                    {
                                        MessageBox.Show("Ocurrio un ERROR durante el proceso de reconstruccion de saldos del recaudado en la tabla: 'saldos_recauda_dep', se reversara dicho proceso..." + "\n" + "favor revisar los saldos y hacer mantenimiento en el módulo de configuración..." + "\n" + "Error: " + oerror.Message, "Reconstrucción de saldos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        DevExpress.Xpo.XpoDefault.Session.RollbackTransaction();
                                    }
                                }
                                sw = 0;
                            }
                            else
                            {
                                MessageBox.Show("No se encuentra la sesión asociada a la recaudación...", "Anular Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Solo los Usuarios Administradores tienen autorización para Anular Recaudaciones.", "Anular Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
                else
                {
                    MessageBox.Show("Recaudación esta Anulada o Sin Elaborar...", "Anular Recaudación", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception)
            {
                Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("Data Reload", "Continuar", "Ignorar");
                switch (MessageBox.Show("No se pudo leer los datos desde el servidor para el siguiente registro..." + Environment.NewLine + Environment.NewLine + "Posiblemente otro usuario lo elimino o lo tiene bloqueado !!!" + Environment.NewLine + Environment.NewLine + "Seleccione una opción para continuar ?" + Environment.NewLine + Environment.NewLine + "Data Reload : Ejecutar (Data Reload) Vuelve a cargar todas las colecciones de datos desde el servidor." + Environment.NewLine + Environment.NewLine + "Continuar : Salta el registro actual y continua con la lectura de datos del siguiente registro. " + Environment.NewLine + Environment.NewLine + "Ignorar : Ignora la lectura de los datos del registro desde el servidor y permanece sobre el.", "Data Reload (Anular Recaudación)", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Yes:
                        this.datareload();
                        bindingSource1.MoveFirst();
                        this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                        if (this_primary_object_persistent_current != null)
                        {
                            this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                            current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                            if (current_sesion != null)
                            {
                                current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                            }
                        }
                        break;
                    case DialogResult.No:
                        switch (lultimo_boton_barra_mantenimiento)
                        {
                            case "Primero":
                                bindingSource1.MoveFirst();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                if (this_primary_object_persistent_current != null)
                                {
                                    this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                    current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                    if (current_sesion != null)
                                    {
                                        current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                    }
                                 }
                                break;
                            case "Ultimo":
                                bindingSource1.MoveLast();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                if (this_primary_object_persistent_current != null)
                                {
                                    this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                    current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                    if (current_sesion != null)
                                    {
                                        current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                    }
                                }
                                break;
                            case "Anterior":
                                bindingSource1.MovePrevious();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                if (this_primary_object_persistent_current != null)
                                {
                                    this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                    current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                    if (current_sesion != null)
                                    {
                                        current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                    }
                                }
                                break;
                            case "Siguiente":
                                bindingSource1.MoveNext();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                if (this_primary_object_persistent_current != null)
                                {
                                    this_primary_object_persistent_current_state = this_primary_object_persistent_current.ClassInfo.GetMember("_state").GetValue(this_primary_object_persistent_current).ToString();
                                    current_sesion = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.oid);
                                    if (current_sesion != null)
                                    {
                                        current_sesion_state = current_sesion.ClassInfo.GetMember("_state").GetValue(current_sesion).ToString();
                                    }
                                }
                                break;
                        }
                        break;
                    default:
                        break;
                }
                Fundraising_PT.Clases.MsgBoxUtil.UnHackMessageBox();
            }
        }

        public override void imprimir(object sender, EventArgs e)
        {
            base.imprimir(sender, e);
            //this.grid_Base11.gridControl1.PrintDialog();
            this.grid_Base11.gridControl1.ShowRibbonPrintPreview();
        }

        //public override void guardar(object sender, EventArgs e)
        //{
        //    ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)bindingSource1.Current).sesion =
        //        DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(this.lookUp_sesion.Value);
        //    ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)bindingSource1.Current).usuario =
        //        DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(this.lookUp_usuario.Value);
        //    base.guardar(sender, e);
        //    this.simpleButton_recaudacion.Enabled = true;
        //    this.dateTime_fecha_hora.dateEdit1.Refresh();
        //}

        //public override void cancelar(object sender, EventArgs e)
        //{
        //    base.cancelar(sender, e);
        //    this.simpleButton_recaudacion.Enabled = true;
        //    this.dateTime_fecha_hora.dateEdit1.Refresh();
        //}

        private void simpleButton_recaudacion_Click(object sender, EventArgs e)
        {
            try
            {
                this_primary_object_persistent_current.Reload();
                current_sesion.Reload();
                seteo_status_recaudacion();
                //
                if (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).status != 6)
                {
                    //System.Reflection.PropertyInfo proper = this.GetType().GetProperty("ControlBox");
                    //System.Object obj = ((object)this.ControlBox);
                    //obj.GetType().GetProperty("ControlBox").SetValue(obj, false, null);
                    Formularios.UI_Recaudacion_Det form_recaudacion_det = new Fundraising_PT.Formularios.UI_Recaudacion_Det(this, 2);
                    form_recaudacion_det.MdiParent = this.MdiParent;
                    this.barra_Mant_Base11.Enabled = false;
                    this.simpleButton_recaudacion.Enabled = false;
                    this.simpleButton_recaudacion2.Enabled = false;
                    this.grid_Base11.Enabled = false;
                    this.ControlBox = false;
                    form_recaudacion_det.Show();
                    //form_recaudacion_det.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show("No se puede acceder al Detalle de la Recaudaci☺n..." + Environment.NewLine + "Otro usuario la tiene en estatus: (EN PROCESO).", "Detalle Recaudaci☺n", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo actualizar los datos desde el servidor para el registro seleccionado...", "Detalle Recaudaci☺n", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void simpleButton_datos_ventas_Click(object sender, EventArgs e)
        {
            try
            {
                this_primary_object_persistent_current.Reload();
                current_sesion.Reload();
                seteo_status_totalventa();
                //
                if (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).status_tv != 6)
                {
                    Formularios.UI_Totales_Ventas form_totales_ventas = new Fundraising_PT.Formularios.UI_Totales_Ventas(this);
                    //form_totales_ventas.MdiParent = this.MdiParent;
                    this.barra_Mant_Base11.Enabled = false;
                    this.simpleButton_datos_ventas.Enabled = false;
                    this.simpleButton_datos_ventas2.Enabled = false;
                    this.grid_Base11.Enabled = false;
                    this.ControlBox = false;
                    //form_totales_ventas.Show();
                    form_totales_ventas.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show("No se puede acceder al Detalle de los Totales Ventas..." + Environment.NewLine + "Otro usuario la tiene en estatus: (EN PROCESO).", "Detalle Totales Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo actualizar los datos desde el servidor para el registro seleccionado...", "Detalle Totales Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void seteo_status_recaudacion()
        {
            //if (Fundraising_PT.Properties.Settings.Default.U_tipo == 3)
            //{
            //    this.simpleButton_datos_ventas.Enabled = false;
            //    this.simpleButton_datos_ventas2.Enabled = false;
            //}
            //else
            //{
            //    if (Fundraising_PT.Properties.Settings.Default.U_tipo == 1)
            //    {
            //        this.simpleButton_recaudacion.Enabled = true;
            //        this.simpleButton_recaudacion2.Enabled = true;
            //    }
            //    else
            //    {
            //        if (bindingSource1.Count > 0 && ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).usuario.oid == Fundraising_PT.Properties.Settings.Default.U_oid)
            //        {
            //            this.simpleButton_recaudacion.Enabled = true;
            //            this.simpleButton_recaudacion2.Enabled = true;
            //        }
            //        else
            //        {
            //            this.simpleButton_recaudacion.Enabled = false;
            //            this.simpleButton_recaudacion2.Enabled = false;
            //        }
            //    }
            //}
            this.simpleButton_recaudacion.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
            this.simpleButton_recaudacion2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
            this.simpleButton_datos_ventas.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
            this.simpleButton_datos_ventas2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
            this.simpleButton_editar_informacion_fiscal.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
            //
            if (bindingSource1.Count > 0)
            {
                switch (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)bindingSource1.Current).status)
                {
                    case 1:
                        label_estatus_recaudacion.Text = "Abierta";
                        label_estatus_recaudacion.Appearance.ForeColor = Color.GreenYellow;
                        break;
                    case 2:
                        label_estatus_recaudacion.Text = "Cerrada_Normal";
                        label_estatus_recaudacion.Appearance.ForeColor = Color.DeepSkyBlue;
                        break;
                    case 3:
                        label_estatus_recaudacion.Text = "Cerrada_Ajustada";
                        label_estatus_recaudacion.Appearance.ForeColor = Color.Gold;
                        break;
                    case 4:
                        label_estatus_recaudacion.Text = "Anulada";
                        label_estatus_recaudacion.Appearance.ForeColor = Color.Red;
                        break;
                    case 6:
                        label_estatus_recaudacion.Text = "En_Proceso";
                        label_estatus_recaudacion.Appearance.ForeColor = Color.LightCyan;
                        break;
                    default:
                        label_estatus_recaudacion.Text = "Total Venta C/S";
                        label_estatus_recaudacion.Appearance.ForeColor = Color.DeepSkyBlue;
                        break;
                }
            }
        }

        public void seteo_status_totalventa()
        {
            this.simpleButton_recaudacion.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
            this.simpleButton_recaudacion2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
            this.simpleButton_datos_ventas.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
            this.simpleButton_datos_ventas2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
            this.simpleButton_editar_informacion_fiscal.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
            //
            if (bindingSource1.Count > 0)
            {
                switch (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)bindingSource1.Current).status_tv)
                {
                    case 1:
                        label_estatus_totalventas.Text = "Abierta";
                        label_estatus_totalventas.Appearance.ForeColor = Color.GreenYellow;
                        break;
                    case 2:
                        label_estatus_totalventas.Text = "Cerrada_Normal";
                        label_estatus_totalventas.Appearance.ForeColor = Color.DeepSkyBlue;
                        break;
                    case 3:
                        label_estatus_totalventas.Text = "Cerrada_Ajustada";
                        label_estatus_totalventas.Appearance.ForeColor = Color.Gold;
                        break;
                    case 4:
                        label_estatus_totalventas.Text = "Anulada";
                        label_estatus_totalventas.Appearance.ForeColor = Color.Red;
                        break;
                    case 5:
                        label_estatus_totalventas.Text = "Cerrada sin Montos";
                        label_estatus_totalventas.Appearance.ForeColor = Color.DeepSkyBlue;
                        break;
                    case 6:
                        label_estatus_totalventas.Text = "En_Proceso";
                        label_estatus_totalventas.Appearance.ForeColor = Color.LightCyan;
                        break;
                    default:
                        label_estatus_totalventas.Text = "Sin Elaborar";
                        label_estatus_totalventas.Appearance.ForeColor = Color.Orange;
                        break;
                }
            }
        }

        public override void lookUpEdit_sucursales_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            //base.lookUpEdit_sucursales_Closed(sender, e);
            my_sucursal_filter = string.Format("sucursal in({0})", lookUpEdit_sucursales.EditValue.ToString().Trim());
            //  
            string_filter_principal = my_sucursal_filter + " and (" + filtro_recaudaciones.ToString().Trim() + ")";
            lookUpEdit_sucursales.ToolTip = string_filter_principal;
            filter_sucursales();
            if (bindingSource1.Count > 0)
            { Control_Mode(0); }
            else
            { Control_Mode(2); }
            this.Refresh();
        }

        public override void filter_sucursales()
        {
            //recaudaciones.CriteriaString = my_sucursal_filter + " and (" + filtro_recaudaciones.ToString().Trim() + ")"; ;
            recaudaciones.CriteriaString = string_filter_principal;
            recaudaciones.Reload();
            bindingSource1.DataSource = recaudaciones;
            bindingSource1.MoveFirst();
            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
            seteo_status_recaudacion();
            seteo_status_totalventa();
            if (bindingSource1.Count <= 0)
            {
                //this.barra_Mant_Base11.Enabled = false;
                this.simpleButton_recaudacion.Enabled = false;
                this.simpleButton_recaudacion2.Enabled = false;
                this.simpleButton_datos_ventas.Enabled = false;
                this.simpleButton_datos_ventas2.Enabled = false;
                this.simpleButton_editar_informacion_fiscal.Enabled = false;
            }
            else
            {
                //this.barra_Mant_Base11.Enabled = true;
                this.simpleButton_recaudacion.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
                this.simpleButton_recaudacion2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 3 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                this.simpleButton_datos_ventas.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
                this.simpleButton_datos_ventas2.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1); //true;
                this.simpleButton_editar_informacion_fiscal.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 2 | Fundraising_PT.Properties.Settings.Default.U_tipo == 1);  //true;
            }
        }

        private void viewcodigointegrado()
        {

            if (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current) != null)
            {
                lc_codigointegrado = (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sucursal == null ? "Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sucursal.ToString().Trim()) + (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion == null ? "-Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.caja.codigo.Trim()) + (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion == null ? "-Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.cajero.codigo.Trim()) + (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion == null ? "-Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)this_primary_object_persistent_current).sesion.id_sesion.ToString().Trim());
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
            sesiones.Dispose();
            usuarios.Dispose();
            if (recaudaciones!=null)
            {
                recaudaciones.Dispose();
            }
            if (recaudaciones_det_aux!=null)
            {
                recaudaciones_det_aux.Dispose();
            }
            bindingSource1.Dispose();
        }

        public override void datareload()
        {
            base.datareload();
            //
            sucursales.Load();
            sucursales.Reload();
            sesiones.Load();
            sesiones.Reload();
            usuarios.Load();
            usuarios.Reload();
            if (recaudaciones != null)
            {
                recaudaciones.Load();
                recaudaciones.Reload();
            }
            if (recaudaciones_det_aux != null)
            {
                recaudaciones_det_aux.Load();
                recaudaciones_det_aux.Reload();
            }
            //
            this.lookUpEdit_sucursales.Properties.DataSource = Fundraising_PT.Clases.Setting_Sucursales.data_sucursales();
            //
            filter_sucursales();
        }
    }
}
