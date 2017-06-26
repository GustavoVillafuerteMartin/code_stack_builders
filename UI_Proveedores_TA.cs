using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Xpo;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Proveedores_TA : Fundraising_PT.Form_Mant_Base1
    {
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA> proveedores_ta;

        public UI_Proveedores_TA(DevExpress.XtraBars.BarButtonItem opcionMenu, ref DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra, object objetoExtra1, object objetoExtra2, object objetoExtra3, object objetoExtra4)
            : base(opcionMenu, ref headerMenu, objetoExtra, objetoExtra1, objetoExtra2, objetoExtra3, objetoExtra4)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Proveedores de Ticket Alimentación...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            InitializeComponent();
            //
            proveedores_ta = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA>(DevExpress.Xpo.XpoDefault.Session, true);
            //
            this.lookUpEdit_sucursales.Properties.DataSource = Fundraising_PT.Clases.Setting_Sucursales.data_sucursales();
            bindingSource1.DataSource = proveedores_ta;
            bindingSource1.Sort = "codigo";
            //
        }

        private void UI_Proveedores_TA_Load(object sender, EventArgs e)
        {
            bindingSource1.MoveFirst();
            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
            lookUpEdit_sucursales.Enabled = false;

            this.grid_Base11.gridView1.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(gridView1_CustomColumnDisplayText);
            this.grid_Base11.gridView1.ViewCaption = "Listado de Proveedores de Ticket Alimentación";
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e != null && e.Column.FieldName == "status")
            {
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
            ((Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA)this_primary_object_persistent_current).sucursal = 0;  //Fundraising_PT.Properties.Settings.Default.sucursal;
            if (lAccion == "Insertar")
            { ((Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA)this_primary_object_persistent_current).status = 1; }
            base.guardar(sender, e);
        }

        public override void eliminar(object sender, EventArgs e)
        {
            if (this_primary_object_persistent_current != null)
            {
                try
                {
                    this_primary_object_persistent_current.Reload();
                    int cantidad_formas_pago = (((Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA)this_primary_object_persistent_current).Formas_Pagos == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA)this_primary_object_persistent_current).Formas_Pagos.Count);
                    if (cantidad_formas_pago > 0)
                    {
                        if (MessageBox.Show("NO se puede Eliminar el proveedor de tarjetas de alimentación porque existen formas de pago asociadas," + Environment.NewLine + "Desea cambiar el estatus del proveedor a InActivo ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                        {
                            ((Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA)this_primary_object_persistent_current).status = 2;
                            this_primary_object_persistent_current.Save();
                            lookUp_status.gridLookUpEdit1.Refresh();
                        }
                    }
                    else
                    {
                        int cantidad_recaidacion_det = (((Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA)this_primary_object_persistent_current).Recaudacion_Det == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA)this_primary_object_persistent_current).Recaudacion_Det.Count);
                        if (cantidad_recaidacion_det > 0)
                        {
                            if (MessageBox.Show("NO se puede Eliminar el proveedor de tarjetas de alimentación porque existen formas de pago asociadas," + Environment.NewLine + "Desea cambiar el estatus del proveedor a InActivo ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                            {
                                ((Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA)this_primary_object_persistent_current).status = 2;
                                this_primary_object_persistent_current.Save();
                                lookUp_status.gridLookUpEdit1.Refresh();
                            }
                        }
                        else
                        {
                            base.eliminar(sender, e);
                        }
                    }
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

        public override void datareload()
        {
            base.datareload();
            //
            proveedores_ta.Load();
            proveedores_ta.Reload();
            bindingSource1.MoveFirst();
            //
        }

        public override void objects_disposes()
        {
            base.objects_disposes();
            proveedores_ta.Dispose();
            bindingSource1.Dispose();
        }

    }
}
