using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Collections;
using DevExpress.Xpo;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Usuarios : Fundraising_PT.Form_Mant_Base1
    {
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> usuarios;

        public UI_Usuarios(DevExpress.XtraBars.BarButtonItem opcionMenu, ref DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra, object objetoExtra1, object objetoExtra2, object objetoExtra3, object objetoExtra4)
            : base(opcionMenu, ref headerMenu, objetoExtra, objetoExtra1, objetoExtra2, objetoExtra3, objetoExtra4)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Usuarios...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            InitializeComponent();
            //
            usuarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, true);
            //
            //usuarios.Where(e => e.login.Trim() == "gustavo");
            //usuarios.Reload();
            //bindingSource1.DataSource = usuarios.Where(e => e.login.Trim() == "gustavo");
            //bindingSource1.Filter.Where(e => e.login == "gustavo");
            //
            this.lookUpEdit_sucursales.Properties.DataSource = Fundraising_PT.Clases.Setting_Sucursales.data_sucursales();
            bindingSource1.DataSource = usuarios;
            bindingSource1.Sort = "usuario";
            //
            this.textBox_clave.textEdit1.Properties.PasswordChar = char.Parse("*");
            //
        }

        private void UI_Usuarios_Load(object sender, EventArgs e)
        {
            bindingSource1.MoveFirst();
            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
            lookUpEdit_sucursales.Enabled = false;

            this.grid_Base11.gridView1.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(gridView1_CustomColumnDisplayText);
            this.grid_Base11.gridView1.ViewCaption = "Listado de Usuarios";
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e != null && e.Column.FieldName == "tipo")
            {
                e.DisplayText = ((Fundraising_PTDM.Enums.ETipo)e.Value).ToString();
                e.Column.DisplayFormat.FormatString.PadLeft(15);
            }

            if (e != null && e.Column.FieldName == "status")
            {
                e.DisplayText = ((Fundraising_PTDM.Enums.EStatus)e.Value).ToString();
                e.Column.DisplayFormat.FormatString.PadLeft(10);
            }
        }

        public override void insertar(object sender, EventArgs e)
        {
            base.insertar(sender, e);
            if (lAccion == "Insertar")
            {
                lookUp_status.Enabled = false;
                lookUp_status.gridLookUpEdit1.EditValue = 1;
                lookUp_tipo.gridLookUpEdit1.EditValue = 3;
            }
        }
        
        public override void imprimir(object sender, EventArgs e)
        {
            base.imprimir(sender, e);
            this.grid_Base11.gridControl1.ShowRibbonPrintPreview();
        }

        public override void guardar(object sender, EventArgs e)
        {
            //((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)bindingSource1.Current).tipo = (lookUp_tipo.gridLookUpEdit1.EditValue == null || (int)lookUp_tipo.gridLookUpEdit1.EditValue == 0 ? 3 : (int)lookUp_tipo.gridLookUpEdit1.EditValue);
            ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).tipo = (lookUp_tipo.gridLookUpEdit1.EditValue == null || int.Parse(lookUp_tipo.gridLookUpEdit1.EditValue.ToString()) == 0 ? 3 : int.Parse(lookUp_tipo.gridLookUpEdit1.EditValue.ToString()));
            ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).sucursal = 0; //Fundraising_PT.Properties.Settings.Default.sucursal;
            if (lAccion == "Insertar")
            { ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).status = 1; }
            base.guardar(sender, e);
        }

        public override void eliminar(object sender, EventArgs e)
        {
            if (this_primary_object_persistent_current != null)
            {
                try
                {
                    this_primary_object_persistent_current.Reload();
                    int cantidad_saldos_recauda = (((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).Saldos_Recauda_dep == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).Saldos_Recauda_dep.Count);
                    int cantidad_saldos_recauda1 = (((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).Saldos_Recauda_dep1 == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).Saldos_Recauda_dep1.Count);
                    int cantidad_saldos_recauda2 = (((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).Saldos_Recauda_dep2 == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).Saldos_Recauda_dep2.Count);
                    if (cantidad_saldos_recauda > 0 || cantidad_saldos_recauda1 > 0 || cantidad_saldos_recauda2 > 0)
                    {
                        if (MessageBox.Show("NO se puede Eliminar el usuario porque existen saldos de racaudaciones asociados," + Environment.NewLine + "Desea cambiar el estatus del usuario a InActivo ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                        {
                            ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).status = 2;
                            this_primary_object_persistent_current.Save();
                            lookUp_status.gridLookUpEdit1.Refresh();
                        }
                    }
                    else
                    {
                        int cantidad_recadaciones = (((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).RecaudacionesCollection == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).RecaudacionesCollection.Count);
                        if (cantidad_recadaciones > 0)
                        {
                            if (MessageBox.Show("NO se puede Eliminar el usuario porque existen recaudaciones asociadas," + Environment.NewLine + "Desea cambiar el estatus del usuario a InActivo ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                            {
                                ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).status = 2;
                                this_primary_object_persistent_current.Save();
                                lookUp_status.gridLookUpEdit1.Refresh();
                            }
                        }
                        else
                        {
                            int cantidad_depositos_bancarios = (((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).Depositos_Bancarios == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).Depositos_Bancarios.Count);
                            int cantidad_depositos_bancarios1 = (((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).Depositos_Bancarios1 == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).Depositos_Bancarios1.Count);
                            if (cantidad_depositos_bancarios > 0 || cantidad_depositos_bancarios1 > 0)
                            {
                                if (MessageBox.Show("NO se puede Eliminar el usuario porque existen depositos bancarios asociados," + Environment.NewLine + "Desea cambiar el estatus del usuario a InActivo ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                                {
                                    ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).status = 2;
                                    this_primary_object_persistent_current.Save();
                                    lookUp_status.gridLookUpEdit1.Refresh();
                                }
                            }
                            else
                            {
                                int cantidad_depositos_bancarios_det = (((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).Depositos_Bancarios_Det == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).Depositos_Bancarios_Det.Count);
                                if (cantidad_depositos_bancarios_det > 0)
                                {
                                    if (MessageBox.Show("NO se puede Eliminar el usuario porque existen detalle de depositos bancarios asociados," + Environment.NewLine + "Desea cambiar el estatus del usuario a InActivo ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                                    {
                                        ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).status = 2;
                                        this_primary_object_persistent_current.Save();
                                        lookUp_status.gridLookUpEdit1.Refresh();
                                    }
                                }
                                else
                                {
                                    int cantidad_sesiones = (((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).SesionesCollection == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).SesionesCollection.Count);
                                    if (cantidad_sesiones > 0)
                                    {
                                        if (MessageBox.Show("NO se puede Eliminar el usuario porque existen sesiones asociadas," + Environment.NewLine + "Desea cambiar el estatus del usuario a InActivo ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                                        {
                                            ((Fundraising_PTDM.FUNDRAISING_PT.Usuarios)this_primary_object_persistent_current).status = 2;
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
            usuarios.Load();
            usuarios.Reload();
            bindingSource1.MoveFirst();
            //
        }

        public override void objects_disposes()
        {
            base.objects_disposes();
            usuarios.Dispose();
            bindingSource1.Dispose();
        }


    }
}
