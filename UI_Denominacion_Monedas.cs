using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Globalization;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Denominacion_Monedas : Fundraising_PT.Form_Mant_Base1
    {
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas> Denominacion_Monedas;

        public UI_Denominacion_Monedas(DevExpress.XtraBars.BarButtonItem opcionMenu, ref DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra, object objetoExtra1, object objetoExtra2, object objetoExtra3, object objetoExtra4)
            : base(opcionMenu, ref headerMenu, objetoExtra, objetoExtra1, objetoExtra2, objetoExtra3, objetoExtra4)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Denominación de Monedas...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            this.lookUpEdit_sucursales.Properties.DataSource = Fundraising_PT.Clases.Setting_Sucursales.data_sucursales();
            InitializeComponent();
            //
            Denominacion_Monedas = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas>(DevExpress.Xpo.XpoDefault.Session, true);
            //
            DevExpress.Xpo.SortingCollection orden_denominacion_monedas = new DevExpress.Xpo.SortingCollection(new DevExpress.Xpo.SortProperty("tipo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            orden_denominacion_monedas.Add(new DevExpress.Xpo.SortProperty("valor", DevExpress.Xpo.DB.SortingDirection.Descending));
            Denominacion_Monedas.Sorting = orden_denominacion_monedas;
            //
            bindingSource1.DataSource = Denominacion_Monedas;
            //
        }

        private void UI_Denominacion_Monedas_Load(object sender, EventArgs e)
        {
            //
            bindingSource1.MoveFirst();
            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
            lookUpEdit_sucursales.Enabled = false;
            //
            this.grid_Base11.gridView1.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(gridView1_CustomColumnDisplayText);
            this.grid_Base11.gridView1.ViewCaption = "Listado de Denominaciones de Monedas";
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e != null && e.Column.FieldName == "tipo")
            {
                e.DisplayText = ((Fundraising_PTDM.Enums.ETipo_moneda)e.Value).ToString();
            }

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
                lookUp_tipo.gridLookUpEdit1.EditValue = 1;
            }
        }

        public override void imprimir(object sender, EventArgs e)
        {
            base.imprimir(sender, e);
            this.grid_Base11.gridControl1.ShowRibbonPrintPreview();
        }

        public override void  guardar(object sender, EventArgs e)
        {
            int ln_tipo = 0;
            if (int.TryParse(lookUp_tipo.gridLookUpEdit1.EditValue.ToString().Trim(), out ln_tipo))
            {
                decimal ln_valor = decimal.Zero;
                if (decimal.TryParse(textBox_valor.textEdit1.EditValue.ToString().Trim(), out ln_valor))
                {
                    try
                    {
                        //NumberFormatInfo nfi = new NumberFormatInfo();  
                        //nfi.NumberGroupSeparator = ",";
                        //nfi.NumberDecimalSeparator = ".";
                        //ln_valor = decimal.Parse(ln_valor.ToString(nfi));

                        XPView valida_moneda = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas));
                        valida_moneda.AddProperty("oid", "oid", false, true, DevExpress.Xpo.SortDirection.None);
                        valida_moneda.AddProperty("codigo", "codigo", false, true, DevExpress.Xpo.SortDirection.None);
                        valida_moneda.AddProperty("tipo", "tipo", false, true, DevExpress.Xpo.SortDirection.None);
                        valida_moneda.AddProperty("valor", "valor", false, true, DevExpress.Xpo.SortDirection.None);
                        valida_moneda.AddProperty("status", "status", false, true, DevExpress.Xpo.SortDirection.None);
                        valida_moneda.Criteria = CriteriaOperator.Parse(string.Format("oid != '{0}' and tipo = {1} and valor = {2} and status = 1", ((Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas)bindingSource1.Current).oid, ln_tipo, ln_valor));
                        //                    
                        if (valida_moneda != null & valida_moneda.Count > 0)
                        {
                            MessageBox.Show("Ya existe un registro con este Tipo y Denominación, favor cambiar los datos e intentar de nuevo...", "Guardar");
                        }
                        else
                        {
                            ((Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas)this_primary_object_persistent_current).sucursal = 0;  //Fundraising_PT.Properties.Settings.Default.sucursal;
                            if (lAccion == "Insertar")
                            { ((Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas)this_primary_object_persistent_current).status = 1; }
                            base.guardar(sender, e);
                        }
                    }
                    catch (Exception oerror)
                    {
                        MessageBox.Show("Ocurrio un ERROR validando que no existan denominaciones de monedas repetidas..." + "\n" + "Error: " + oerror.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        base.cancelar(sender, e);
                    }
                }
                else
                {
                    MessageBox.Show("Ocurrio un ERROR evaluando el valor de la denominación de la moneda...", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    base.cancelar(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Ocurrio un ERROR evaluando el tipo de la denominación de la moneda...", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                base.cancelar(sender, e);
            }
        }

        public override void eliminar(object sender, EventArgs e)
        {
            if (this_primary_object_persistent_current != null)
            {
                try
                {
                    this_primary_object_persistent_current.Reload();
                    int cantidad_recaudaciones = (((Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas)this_primary_object_persistent_current).Recaudacion_Det_Des == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas)this_primary_object_persistent_current).Recaudacion_Det_Des.Count);
                    if (cantidad_recaudaciones > 0)
                    {
                        if (MessageBox.Show("NO se puede Eliminar la denominación de moneda porque existen recaudaciones asociadas," + Environment.NewLine + "Desea cambiar el estatus de la denominación de moneda a InActiva ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                        {
                            ((Fundraising_PTDM.FUNDRAISING_PT.Denominacion_Monedas)this_primary_object_persistent_current).status = 2;
                            this_primary_object_persistent_current.Save();
                            lookUp_status.gridLookUpEdit1.Refresh();
                        }
                    }
                    else
                    {
                        base.eliminar(sender, e);
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

        public bool validar_datos()
        {
            bool pass = true;
            //
            if (this.textBox_codigo.textEdit1.EditValue == null || this.textBox_codigo.textEdit1.EditValue.ToString().Trim() == string.Empty)
            {
                MessageBox.Show("Código NO puede estar vacio...", "Guardar");
                pass = false;
            }
            if (this.lookUp_tipo.Value == null || (int)this.lookUp_tipo.Value == 0)
            {
                MessageBox.Show("Tipo Invalido...", "Guardar");
                pass = false;
            }
            if (this.textBox_valor.textEdit1.EditValue == null || decimal.Parse(this.textBox_valor.textEdit1.EditValue.ToString()) <= 0)
            {
                MessageBox.Show("Denominación NO puede estar vacio o negativo...", "Guardar");
                pass = false;
            }
            //
            return (pass);
        }

        public override void datareload()
        {
            base.datareload();
            //
            Denominacion_Monedas.Load();
            Denominacion_Monedas.Reload();
            bindingSource1.MoveFirst();
            //
        }

        public override void objects_disposes()
        {
            base.objects_disposes();
            Denominacion_Monedas.Dispose();
            bindingSource1.Dispose();
        }

    }
}
