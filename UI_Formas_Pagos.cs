using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Xpo;


namespace Fundraising_PT.Formularios
{
    public partial class UI_Formas_Pagos : Fundraising_PT.Form_Mant_Base1
    {
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA> proveedores_ta;
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos> formas_pagos;

        public UI_Formas_Pagos(DevExpress.XtraBars.BarButtonItem opcionMenu, ref DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra, object objetoExtra1, object objetoExtra2, object objetoExtra3, object objetoExtra4)
            : base(opcionMenu, ref headerMenu, objetoExtra, objetoExtra1, objetoExtra2, objetoExtra3, objetoExtra4)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Formas de Pago...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            InitializeComponent();
            //
            proveedores_ta = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA>(DevExpress.Xpo.XpoDefault.Session, true);
            formas_pagos   = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(DevExpress.Xpo.XpoDefault.Session, true);
            //
            this.lookUpEdit_sucursales.Properties.DataSource = Fundraising_PT.Clases.Setting_Sucursales.data_sucursales();
            bindingSource1.DataSource = formas_pagos;
            bindingSource1.Sort = "codigo";
            //
        }

        private void UI_Formas_Pagos_Load(object sender, EventArgs e)
        {
            bindingSource1.MoveFirst();
            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
            lookUpEdit_sucursales.Enabled = false;

            this.lookUp_proveedor_ta.SetDataSource(proveedores_ta, "nombre", "oid");
            this.grid_Base11.gridView1.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(gridView1_CustomColumnDisplayText);
            this.grid_Base11.gridView1.ViewCaption = "Listado de Formas de Pago";
            this.lookUp_tpago.gridLookUpEdit1.EditValueChanged += new EventHandler(gridLookUpEdit1_EditValueChanged);
            this.lookUp_ttarjeta.gridLookUpEdit1.EditValueChanged += new EventHandler(gridLookUpEdit1_EditValueChanged);
            if (bindingSource1.Count > 0)
                { this.gridLookUpEdit1_EditValueChanged(null, null); }

            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (this.lookUp_tpago.Value != null)
            {
                if (this.lookUp_tpago.gridLookUpEdit1.EditValue.ToString().Trim() == "2")
                {
                    this.label_Base25.Visible = true;
                    this.lookUp_ttarjeta.Visible = true;
                    if (this.lookUp_ttarjeta.Value != null)
                    {
                        if (this.lookUp_ttarjeta.gridLookUpEdit1.EditValue.ToString().Trim() == "3")
                        {
                            this.label_Base26.Visible = true;
                            this.lookUp_proveedor_ta.Visible = true;
                        }
                        else
                        {
                            this.label_Base26.Visible = false;
                            this.lookUp_proveedor_ta.Visible = false;
                        }
                    }
                }
                else
                {
                    this.label_Base25.Visible = false;
                    this.lookUp_ttarjeta.Visible = false;
                    if (this.lookUp_tpago.Value != null)
                    {
                        if (this.lookUp_tpago.gridLookUpEdit1.EditValue.ToString().Trim() == "7")
                        {
                            this.label_Base26.Visible = true;
                            this.lookUp_proveedor_ta.Visible = true;
                        }
                        else
                        {
                            this.label_Base26.Visible = false;
                            this.lookUp_proveedor_ta.Visible = false;
                        }
                    }
                }
            }
        }

        void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e != null && e.Column.FieldName == "tpago")
            {
                e.DisplayText = ((Fundraising_PTDM.Enums.EPago)e.Value).ToString();
            }

            if (e != null && e.Column.FieldName == "ttarjeta")
            {
                e.DisplayText = ((Fundraising_PTDM.Enums.ETarjeta)e.Value).ToString();
                //if (e.RowHandle >= 0)
                //{
                //    if (this.grid_Base11.gridView1.GetRowCellValue(e.RowHandle, "tpago").ToString().Trim() == "2")
                //    {
                //        e.DisplayText = ((Fundraising_PTDM.Enums.ETarjeta)e.Value).ToString();
                //    }
                //    else
                //    {
                //        e.DisplayText = "Ninguno";
                //    }
                //}
                //else { e.DisplayText = ((Fundraising_PTDM.Enums.ETarjeta)e.Value).ToString(); }
            }

            if (e != null && e.Column.FieldName == "proveedor_ta.nombre")
            {
                if (e.Value != null)
                {
                    //if (this.grid_Base11.gridView1.GetRowCellValue(e.RowHandle, "tpago").ToString().Trim() != "2" || this.grid_Base11.gridView1.GetRowCellValue(e.RowHandle, "ttarjeta").ToString().Trim() != "2")
                    //{
                    //    if (this.grid_Base11.gridView1.GetRowCellValue(e.RowHandle, "tpago").ToString().Trim() != "7")
                    //    {
                    //        e.DisplayText = "Ninguno";
                    //    }
                    //}
                }
                else
                {
                    e.DisplayText = "Ninguno";
                }
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
            }
        }
        
        public override void imprimir(object sender, EventArgs e)
        {
            base.imprimir(sender, e);
            this.grid_Base11.gridControl1.ShowRibbonPrintPreview();
        }

        public override void guardar(object sender, EventArgs e)
        {
            if (this.lookUp_ttarjeta.Visible)
                { this.lookUp_ttarjeta.LValid_empty_null = true; }
            else
                { this.lookUp_ttarjeta.LValid_empty_null = false; }
            //
            if (this.lookUp_proveedor_ta.Visible)
            { 
                this.lookUp_proveedor_ta.LValid_empty_null = true;
                if (this.lookUp_proveedor_ta.gridLookUpEdit1.EditValue != null & this.lookUp_proveedor_ta.gridLookUpEdit1.EditValue.ToString() != String.Empty)
                {
                    ((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).proveedor_ta =
                    DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Proveedores_TA>(this.lookUp_proveedor_ta.gridLookUpEdit1.EditValue);
                }
            }
            else
                { this.lookUp_proveedor_ta.LValid_empty_null = false; }
            //
            //
            ((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).sucursal = 0;  //Fundraising_PT.Properties.Settings.Default.sucursal;
            if (lAccion == "Insertar")
            { ((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).status = 1; }
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
                    int cantidad_saldos_recauda = (((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).Saldos_Recauda_dep == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).Saldos_Recauda_dep.Count);
                    if (cantidad_saldos_recauda > 0)
                    {
                        if (MessageBox.Show("NO se puede Eliminar la forma de pago porque existen saldos de racaudaciones asociados," + Environment.NewLine + "Desea cambiar el estatus de la forma de pago a InActiva ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                        {
                            ((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).status = 2;
                            this_primary_object_persistent_current.Save();
                            lookUp_status.gridLookUpEdit1.Refresh();
                        }
                    }
                    else
                    {
                        int cantidad_recaidacion_det = (((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).Recaudacion_Det == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).Recaudacion_Det.Count);
                        if (cantidad_recaidacion_det > 0)
                        {
                            if (MessageBox.Show("NO se puede Eliminar la forma de pago porque existen detalle de recaudaciones asociadas," + Environment.NewLine + "Desea cambiar el estatus de la forma de pago a InActiva ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                            {
                                ((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).status = 2;
                                this_primary_object_persistent_current.Save();
                                lookUp_status.gridLookUpEdit1.Refresh();
                            }
                        }
                        else
                        {
                            int cantidad_depositos_bancarios = (((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).Depositos_Bancarios_Det == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).Depositos_Bancarios_Det.Count);
                            if (cantidad_depositos_bancarios > 0)
                            {
                                if (MessageBox.Show("NO se puede Eliminar la forma de pago porque existen detalle de depositos bancarios asociados," + Environment.NewLine + "Desea cambiar el estatus de la forma de pago a InActiva ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                                {
                                    ((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).status = 2;
                                    this_primary_object_persistent_current.Save();
                                    lookUp_status.gridLookUpEdit1.Refresh();
                                }
                            }
                            else
                            {
                                int cantidad_totales_ventas = (((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).Totales_Ventas == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).Totales_Ventas.Count);
                                if (cantidad_totales_ventas > 0)
                                {
                                    if (MessageBox.Show("NO se puede Eliminar la forma de pago porque existen totales de ventas asociados," + Environment.NewLine + "Desea cambiar el estatus de la forma de pago a InActiva ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                                    {
                                        ((Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos)this_primary_object_persistent_current).status = 2;
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
            formas_pagos.Load();
            formas_pagos.Reload();
            bindingSource1.MoveFirst();
            //
        }

        public override void objects_disposes()
        {
            base.objects_disposes();
            proveedores_ta.Dispose();
            formas_pagos.Dispose();
            bindingSource1.Dispose();
        }

        void refresca_objetos()
        {
            //
            lookUpEdit_sucursales.RefreshEditValue();
            lookUp_proveedor_ta.gridLookUpEdit1View.RefreshData();
            lookUp_proveedor_ta.gridLookUpEdit1.RefreshEditValue();
            lookUp_ttarjeta.gridLookUpEdit1View.RefreshData();
            lookUp_ttarjeta.gridLookUpEdit1.RefreshEditValue();
            lookUp_tpago.gridLookUpEdit1View.RefreshData();
            lookUp_tpago.gridLookUpEdit1.RefreshEditValue();
            //
        }

    }
}
