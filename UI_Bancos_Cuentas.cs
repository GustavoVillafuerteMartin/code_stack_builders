﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Bancos_Cuentas : Fundraising_PT.Form_Mant_Base1
    {
        public string lc_codigointegrado = string.Empty;
        
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>     sucursales;
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos>         bancos;
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas> bancos_cuentas;

        public UI_Bancos_Cuentas(DevExpress.XtraBars.BarButtonItem opcionMenu, ref DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra, object objetoExtra1, object objetoExtra2, object objetoExtra3, object objetoExtra4)
            : base(opcionMenu, ref headerMenu, objetoExtra, objetoExtra1, objetoExtra2, objetoExtra3, objetoExtra4)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Cuentas Bancarias...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            this.lookUpEdit_sucursales.Properties.DataSource = Fundraising_PT.Clases.Setting_Sucursales.data_sucursales();
            InitializeComponent();
            //
            sucursales     = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session, true);   
            bancos         = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos>(DevExpress.Xpo.XpoDefault.Session, true);
            bancos_cuentas = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas>(DevExpress.Xpo.XpoDefault.Session, true);
            //
            bindingSource1.DataSource = bancos_cuentas;
            bindingSource1.Sort = "banco.nombre";
        }

        private void Bancos_Cuentas_Load(object sender, EventArgs e)
        {
            filter_sucursales();
            bindingSource1.MoveFirst();
            viewcodigointegrado();
            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
            //
            this.lookUp_sucursal.Properties.DataSource = sucursales;
            this.lookUp_sucursal.Properties.DisplayMember = "nombre";
            this.lookUp_sucursal.Properties.ValueMember = "codigo";
            this.lookUp_sucursal.DataBindings.Clear();
            this.lookUp_sucursal.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", bindingSource1, "sucursal"));
            //
            this.lookUp_banco.SetDataSource(bancos, "nombre", "oid");
            this.grid_Base11.gridView1.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(gridView1_CustomColumnDisplayText);
            this.grid_Base11.gridView1.ViewCaption = "Listado de Cuentas Bancarias";
            this.lookUpEdit_sucursales.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 1);
            textBox_buscacodigointegrado.Validated += textBox_buscacodigointegrado_Validated;
            textBox_buscacodigointegrado.textEdit1.KeyPress += textEdit1_KeyPress;
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void refresca_objetos()
        {
            //
            lookUpEdit_sucursales.RefreshEditValue();
            lookUp_sucursal.Refresh();
            lookUp_banco.gridLookUpEdit1View.RefreshData();
            lookUp_banco.gridLookUpEdit1.RefreshEditValue();
            //
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

        public override void bindingSource1_PositionChanged(object sender, EventArgs e)
        {
            base.bindingSource1_PositionChanged(sender, e);
            viewcodigointegrado();
        }

        void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e != null && e.Column.FieldName == "status")
            {
                e.DisplayText = ((Fundraising_PTDM.Enums.EStatus)e.Value).ToString();
            }
        
            if (e != null && e.Column.FieldName == "sucursal")
            {
                var lc_nombre_sucursal = DevExpress.Xpo.XpoDefault.Session.ExecuteQuery(string.Format("select nombre from sucursales where codigo = {0}", e.Value));
                e.DisplayText = (lc_nombre_sucursal == null ? "[ Vacio ]" : lc_nombre_sucursal.ResultSet[0].Rows[0].Values[0].ToString());
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
            this.grid_Base11.gridControl1 .ShowRibbonPrintPreview();
        }

        public override void guardar(object sender, EventArgs e)
        {
            if (this_primary_object_persistent_current != null)
            {
                Guid lbanco = Guid.Empty;
                string lcuenta = string.Empty;
                string ldescr = string.Empty;
                if (this.lookUp_banco.gridLookUpEdit1.EditValue != null & this.lookUp_banco.gridLookUpEdit1.EditValue.ToString() != String.Empty)
                {
                    lbanco = (Guid)this.lookUp_banco.gridLookUpEdit1.EditValue;
                    lcuenta = this.textBox_codigo_cuenta.lValue;
                    ldescr = this.textBox_descr.lValue;
                    ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).banco = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Bancos>(lbanco);
                    ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).codigo_cuenta = lcuenta;
                    ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).descr = ldescr;
                    ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                    //
                    if (lAccion == "Insertar")
                    { ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).status = 1; }
                    base.guardar(sender, e);
                    viewcodigointegrado();
                }
                else
                {
                    MessageBox.Show("Banco Invalido...", "Guardar");
                    this.lookUp_banco.Focus();
                }
            }
        }
        
        public override void filter_sucursales()
        {
            bancos.Reload();
            bancos_cuentas.CriteriaString = my_sucursal_filter;
            bancos_cuentas.Reload();
            //
            bindingSource1.DataSource = bancos_cuentas;
            bindingSource1.MoveFirst();
        
        }
        
        private void viewcodigointegrado()
        {

            if (((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current) != null)
            {
                lc_codigointegrado = (((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).sucursal == null ? "Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).sucursal.ToString().Trim()) + (((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).banco == null ? "-Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).banco.codigo.Trim()) + (((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).codigo_cuenta == null ? "-Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).codigo_cuenta.Trim());
            }
            else
            {
                lc_codigointegrado = string.Empty;
            }
            this.label_contenidocodigointegrado.lText = " " + lc_codigointegrado;
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

        void textBox_buscacodigointegrado_Validated(object sender, EventArgs e)
        {
            bool sw1 = true;
            if (textBox_buscacodigointegrado.lValue.Trim() != string.Empty)
            {
                //
                XPView vbuscacodigo = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas));
                vbuscacodigo.AddProperty("void", "oid", true, true, DevExpress.Xpo.SortDirection.None);
                vbuscacodigo.AddProperty("vcodigo_integrado", "Trim(ToStr(sucursal))+Trim(banco.codigo)+Trim(codigo_cuenta)", true, true, DevExpress.Xpo.SortDirection.None);
                vbuscacodigo.CriteriaString = string.Format("Trim(ToStr(sucursal))+Trim(banco.codigo)+Trim(codigo_cuenta) = '{0}'", textBox_buscacodigointegrado.lValue.Trim());
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

        public override void cancelar(object sender, EventArgs e)
        {
            base.cancelar(sender, e);
            viewcodigointegrado();
        }

        public override void eliminar(object sender, EventArgs e)
        {
            if (this_primary_object_persistent_current != null)
            {
                try
                {
                    //
                    this_primary_object_persistent_current.Reload();
                    int cantidad_puntos_bancarios = (((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).Puntos_Bancarios == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).Puntos_Bancarios.Count);
                    if (cantidad_puntos_bancarios > 0)
                    {
                        if (MessageBox.Show("NO se puede Eliminar la cuenta bancaria porque existen puntos bancarios asociados," + Environment.NewLine + "Desea cambiar el estatus de la cuenta bancaria a InActiva ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                        {
                            ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).status = 2;
                            this_primary_object_persistent_current.Save();
                            lookUp_status.gridLookUpEdit1.Refresh();
                        }
                    }
                    else
                    {
                        int cantidad_recaidacion_det = (((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).Recaudacion_Det == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).Recaudacion_Det.Count);
                        if (cantidad_recaidacion_det > 0)
                        {
                            if (MessageBox.Show("NO se puede Eliminar la cuenta bancaria porque existen detalle de recaudaciones asociadas," + Environment.NewLine + "Desea cambiar el estatus de la cuenta bancaria a InActiva ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                            {
                                ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).status = 2;
                                this_primary_object_persistent_current.Save();
                                lookUp_status.gridLookUpEdit1.Refresh();
                            }
                        }
                        else
                        {
                            int cantidad_depositos_bancarios = (((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).Depositos_Bancarios == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).Depositos_Bancarios.Count);
                            if (cantidad_depositos_bancarios > 0)
                            {
                                if (MessageBox.Show("NO se puede Eliminar la cuenta bancaria porque existen depositos bancarios asociados," + Environment.NewLine + "Desea cambiar el estatus de la cuenta bancaria a InActiva ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                                {
                                    ((Fundraising_PTDM.FUNDRAISING_PT.Bancos_Cuentas)this_primary_object_persistent_current).status = 2;
                                    this_primary_object_persistent_current.Save();
                                    lookUp_status.gridLookUpEdit1.Refresh();
                                }
                            }
                            else
                            {
                                base.eliminar(sender, e);
                                viewcodigointegrado();
                            }
                        }
                    }
                    //
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

        public override void objects_disposes()
        {
            base.objects_disposes();
            bancos_cuentas.Dispose();
            bancos.Dispose();
            sucursales.Dispose();
            bindingSource1.Dispose();
        }

        public override void datareload()
        {
            base.datareload();
            //
            bancos.Load();
            bancos_cuentas.Load();
            sucursales.Load();
            //
            bancos.Reload();
            bancos_cuentas.Reload();
            sucursales.Reload();
            //
            this.lookUpEdit_sucursales.Properties.DataSource = Fundraising_PT.Clases.Setting_Sucursales.data_sucursales();
            //
            filter_sucursales();
        }

    }
}
