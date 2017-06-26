using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Xpo;
using System.IO;
using DevExpress.Data.Filtering;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Sucursales : Fundraising_PT.Form_Mant_Base1
    {
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales> sucursales;
        //
        public string filenane_imagen_origen = "";
        public string filenane_imagen = "";
        public string nane_imagen = "";
        //
        public UI_Sucursales(DevExpress.XtraBars.BarButtonItem opcionMenu, ref DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra, object objetoExtra1, object objetoExtra2, object objetoExtra3, object objetoExtra4)
            : base(opcionMenu, ref headerMenu, objetoExtra, objetoExtra1, objetoExtra2, objetoExtra3, objetoExtra4)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Sucursales...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            InitializeComponent();
            //
            sucursales = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session, true);
            //
            bindingSource1.DataSource = sucursales;
            bindingSource1.Sort = "codigo";
            //
        }

        private void UI_Sucursales_Load(object sender, EventArgs e)
        {
            bindingSource1.MoveFirst();
            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
            carga_current_imagen();
            lookUpEdit_sucursales.Enabled = false;
            //
            this.pictureEdit_logotipo_sucursal.DoubleClick += new EventHandler(pictureEdit_logotipo_sucursal_DoubleClick);
            this.grid_Base11.gridView1.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(gridView1_CustomColumnDisplayText);
            this.grid_Base11.gridView1.ViewCaption = "Listado de Sucursales";
            this.lookUpEdit_sucursales.Visible = false;
            this.label_Base_sucursales.Visible = false;
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        public override void bindingSource1_PositionChanged(object sender, EventArgs e)
        {
            base.bindingSource1_PositionChanged(sender, e);
            carga_current_imagen();
        }

        void pictureEdit_logotipo_sucursal_DoubleClick(object sender, EventArgs e)
        {
            if (lAccion == "Insertar" | lAccion == "Editar")
            {
                OpenFileDialog form_open_file = new OpenFileDialog();
                form_open_file.CheckFileExists = true;
                form_open_file.CheckPathExists = true;
                form_open_file.ValidateNames = true;
                //
                form_open_file.InitialDirectory = Fundraising_PT.Properties.Settings.Default.mypath_imagenes;
                form_open_file.RestoreDirectory = true;
                form_open_file.ShowDialog();
                if (string.IsNullOrEmpty(form_open_file.SafeFileName) != true)
                {
                    nane_imagen = form_open_file.SafeFileName.Trim();
                    filenane_imagen_origen = form_open_file.FileName.Trim();
                    Image logotipo = Image.FromFile(filenane_imagen_origen);
                    pictureEdit_logotipo_sucursal.Image = logotipo;
                    if (pictureEdit_logotipo_sucursal.DoValidate() != true)
                    {
                        MessageBox.Show("Error cargando archivo de imagen...");
                        pictureEdit_logotipo_sucursal.Image = null;
                        nane_imagen = "";
                        filenane_imagen_origen = "";
                        filenane_imagen = "";
                    }
                    pictureEdit_logotipo_sucursal.Refresh();
                }
                else
                {
                    MessageBox.Show("No selecciono ningun archivo de imagen...");
                }
            }
        }

        void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e != null && e.Column.FieldName == "status")
            {
                //e.DisplayText = ((Fundraising_PTDM.Enums.EStatus)Convert.ToInt32(e.Value)).ToString();
                e.DisplayText = ((Fundraising_PTDM.Enums.EStatus)e.Value).ToString();
            }
        }

        public override void insertar(object sender, EventArgs e)
        {
            base.insertar(sender, e);
            if (lAccion == "Insertar")
            {
                lookUp_status.Enabled = false;
                lookUp_status.gridLookUpEdit1.EditValue = 1;
            }
        }

        public override void imprimir(object sender, EventArgs e)
        {
            base.imprimir(sender, e);
            this.grid_Base11.gridControl1.ShowRibbonPrintPreview();
        }

        public override void guardar(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nane_imagen) != true)
            {
                filenane_imagen = Fundraising_PT.Properties.Settings.Default.mypath_imagenes + nane_imagen;
                if (string.IsNullOrEmpty(filenane_imagen_origen) != true)
                {
                    if (filenane_imagen_origen.Trim() != filenane_imagen.Trim())
                    {
                        try
                        {
                            File.Copy(filenane_imagen_origen, filenane_imagen, true);
                        }
                        catch
                        {
                            MessageBox.Show("Error copiando archivo de imagen al directorio imagenes del sistema...");
                            filenane_imagen_origen = "";
                            filenane_imagen = "";
                            nane_imagen = "";
                        }
                    }
                }
            }
            //
            ((Fundraising_PTDM.FUNDRAISING_PT.Sucursales)this_primary_object_persistent_current).status = (lookUp_status.gridLookUpEdit1.EditValue == null || (int)lookUp_status.gridLookUpEdit1.EditValue == 0 ? 1 : (int)lookUp_status.gridLookUpEdit1.EditValue);
            ((Fundraising_PTDM.FUNDRAISING_PT.Sucursales)this_primary_object_persistent_current).logotipo = nane_imagen;
            //
            if (lAccion == "Insertar")
            { ((Fundraising_PTDM.FUNDRAISING_PT.Sucursales)this_primary_object_persistent_current).status = 1; }
            //
            base.guardar(sender, e);
        }

        public override void eliminar(object sender, EventArgs e)
        {
            if (this_primary_object_persistent_current != null)
            {
                try
                {
                    this_primary_object_persistent_current.Reload();
                    //
                    XPView vcuentas_bancarias = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas));
                    vcuentas_bancarias.AddProperty("void", "oid", true, true, DevExpress.Xpo.SortDirection.None);
                    //
                    XPView vcajas = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Cajas));
                    vcajas.AddProperty("void", "oid", true, true, DevExpress.Xpo.SortDirection.None);
                    //
                    XPView vdepositos = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Depositos_Bancarios));
                    vdepositos.AddProperty("void", "oid", true, true, DevExpress.Xpo.SortDirection.None);
                    //
                    XPView vpuntos_bancarios = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios));
                    vpuntos_bancarios.AddProperty("void", "oid", true, true, DevExpress.Xpo.SortDirection.None);
                    //
                    XPView vsesiones = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Sesiones));
                    vsesiones.AddProperty("void", "oid", true, true, DevExpress.Xpo.SortDirection.None);
                    //
                    XPView vrecaudaciones = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones));
                    vrecaudaciones.AddProperty("void", "oid", true, true, DevExpress.Xpo.SortDirection.None);
                    //
                    vcuentas_bancarias.Criteria = CriteriaOperator.Parse(string.Format("sucursal = {0}", ((Fundraising_PTDM.FUNDRAISING_PT.Sucursales)this_primary_object_persistent_current).codigo));
                    vcajas.Criteria = CriteriaOperator.Parse(string.Format("sucursal = {0}", ((Fundraising_PTDM.FUNDRAISING_PT.Sucursales)this_primary_object_persistent_current).codigo));
                    vdepositos.Criteria = CriteriaOperator.Parse(string.Format("sucursal = {0}", ((Fundraising_PTDM.FUNDRAISING_PT.Sucursales)this_primary_object_persistent_current).codigo));
                    vpuntos_bancarios.Criteria = CriteriaOperator.Parse(string.Format("sucursal = {0}", ((Fundraising_PTDM.FUNDRAISING_PT.Sucursales)this_primary_object_persistent_current).codigo));
                    vsesiones.Criteria = CriteriaOperator.Parse(string.Format("sucursal = {0}", ((Fundraising_PTDM.FUNDRAISING_PT.Sucursales)this_primary_object_persistent_current).codigo));
                    vrecaudaciones.Criteria = CriteriaOperator.Parse(string.Format("sucursal = {0}", ((Fundraising_PTDM.FUNDRAISING_PT.Sucursales)this_primary_object_persistent_current).codigo));
                    //
                    int cantidad_asociaciones = vcuentas_bancarias.Count+vcajas.Count+vdepositos.Count+vpuntos_bancarios.Count+vsesiones.Count+vrecaudaciones.Count;
                    //
                    if (cantidad_asociaciones > 0 | ((Fundraising_PTDM.FUNDRAISING_PT.Sucursales)this_primary_object_persistent_current).codigo == Fundraising_PT.Properties.Settings.Default.sucursal)
                    {
                        if (MessageBox.Show("NO se puede Eliminar la sucursal porque existen asociaciones de datos con la sucursal seleccionada." + Environment.NewLine + "Desea cambiar el estatus de la sucursal a InActiva ?", "Eliminar Sucursal", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                        {
                            ((Fundraising_PTDM.FUNDRAISING_PT.Bancos)this_primary_object_persistent_current).status = 2;
                            this_primary_object_persistent_current.Save();
                            lookUp_status.gridLookUpEdit1.Refresh();
                         }
                    }
                    else
                    {
                        base.eliminar(sender, e);
                    }
                    //
                    vcuentas_bancarias.Dispose();
                    vcajas.Dispose();
                    vdepositos.Dispose();
                    vpuntos_bancarios.Dispose();
                    vsesiones.Dispose();
                    vrecaudaciones.Dispose();
                }
                catch (Exception)
                {
                    Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("Data Reload", "Continuar", "Ignorar");
                    switch (MessageBox.Show("No se pudo leer los datos desde el servidor para el registro actual..." + Environment.NewLine + Environment.NewLine + "Posiblemente otro usuario lo elimino mientras usted lo tenia seleccionado para eliminarno !!!" + Environment.NewLine + Environment.NewLine + "Seleccione una opción para continuar ?" + Environment.NewLine + Environment.NewLine + "Data Reload : Ejecutar (Data Reload) Vuelve a cargar todas las colecciones de datos desde el servidor." + Environment.NewLine + Environment.NewLine + "Cancelar : Cancela la eliminación de datos del registro actual y salta al siguiente registro. " + Environment.NewLine + Environment.NewLine + "Ignorar : Ignora la lectura de los datos del registro desde el servidor y permanece sobre el.", "Data Reload (Eliminar)", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        case DialogResult.Yes:
                            this.datareload();
                            bindingSource1.MoveFirst();
                            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                            break;
                        case DialogResult.No:
                            if (bindingSource1.Count <= 0)
                            {
                                bindingSource1.MoveFirst();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                            if (bindingSource1.Count > 0 & bindingSource1.Position >= bindingSource1.Count)
                            {
                                bindingSource1.MovePrevious();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                            if (bindingSource1.Count > 0 & bindingSource1.Position == 0)
                            {
                                bindingSource1.MoveNext();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                            if (bindingSource1.Count > 0 & (bindingSource1.Position > 0 & bindingSource1.Position < bindingSource1.Count))
                            {
                                bindingSource1.MoveNext();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                            else
                            {
                                bindingSource1.MoveFirst();
                                this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
                                break;
                            }
                    }
                    Fundraising_PT.Clases.MsgBoxUtil.UnHackMessageBox();
                }
            }
        }

        private void carga_current_imagen()
        {
            filenane_imagen_origen = "";
            filenane_imagen = "";
            nane_imagen = "";
            pictureEdit_logotipo_sucursal.Image = null;
            pictureEdit_logotipo_sucursal.Refresh();
            if (bindingSource1.Count > 0)
            {
                if (string.IsNullOrEmpty(((Fundraising_PTDM.FUNDRAISING_PT.Sucursales)this_primary_object_persistent_current).logotipo) != true)
                {
                    nane_imagen = ((Fundraising_PTDM.FUNDRAISING_PT.Sucursales)this_primary_object_persistent_current).logotipo.Trim();
                    filenane_imagen = Fundraising_PT.Properties.Settings.Default.mypath_imagenes + nane_imagen;
                }
                //
                if (string.IsNullOrEmpty(filenane_imagen) != true)
                {
                    if (File.Exists(filenane_imagen))
                    {
                        Image logotipo = Image.FromFile(filenane_imagen);
                        pictureEdit_logotipo_sucursal.Image = logotipo;
                        if (pictureEdit_logotipo_sucursal.DoValidate() != true)
                        {
                            MessageBox.Show("Error cargando archivo de imagen...");
                            pictureEdit_logotipo_sucursal.Image = null;
                            nane_imagen = "";
                            filenane_imagen_origen = "";
                            filenane_imagen = "";
                        }
                        pictureEdit_logotipo_sucursal.Refresh();
                    }
                }
            }
        }
    }
}
