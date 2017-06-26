using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace Fundraising_PT.Formularios
{
    public partial class UI_Sesiones : Fundraising_PT.Form_Mant_Base1
    {
        public string lc_codigointegrado = string.Empty;

        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>   usuarios;
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajas>      cajas;
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajeros>    cajeros;
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales> sucursales;
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>   sesiones;

        public UI_Sesiones(DevExpress.XtraBars.BarButtonItem opcionMenu, ref DevExpress.XtraBars.BarHeaderItem headerMenu, object objetoExtra, object objetoExtra1, object objetoExtra2, object objetoExtra3, object objetoExtra4)
            : base(opcionMenu, ref headerMenu, objetoExtra, objetoExtra1, objetoExtra2, objetoExtra3, objetoExtra4)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Sesiones...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            this.lookUpEdit_sucursales.Properties.DataSource = Fundraising_PT.Clases.Setting_Sucursales.data_sucursales();
            InitializeComponent();
            //
            usuarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, true);
            //usuarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(PersistentCriteriaEvaluationBehavior.BeforeTransaction, DevExpress.Xpo.XpoDefault.Session, CriteriaOperator.Parse("1 = 1"));
            cajas = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajas>(DevExpress.Xpo.XpoDefault.Session, true);
            //cajas = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajas>(PersistentCriteriaEvaluationBehavior.BeforeTransaction, DevExpress.Xpo.XpoDefault.Session, CriteriaOperator.Parse(my_sucursal_filter));
            cajeros = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajeros>(DevExpress.Xpo.XpoDefault.Session, true);
            //cajeros = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Cajeros>(PersistentCriteriaEvaluationBehavior.BeforeTransaction, DevExpress.Xpo.XpoDefault.Session, CriteriaOperator.Parse("1 = 1"));
            sucursales = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session, true);
            //sucursales = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(PersistentCriteriaEvaluationBehavior.BeforeTransaction, DevExpress.Xpo.XpoDefault.Session, CriteriaOperator.Parse("1 = 1"));
            sesiones = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(DevExpress.Xpo.XpoDefault.Session, true);
            //sesiones = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(PersistentCriteriaEvaluationBehavior.BeforeTransaction, DevExpress.Xpo.XpoDefault.Session, CriteriaOperator.Parse(my_sucursal_filter));
            //
            bindingSource1.DataSource = sesiones;
            bindingSource1.Sort = "fecha_hora desc";
            //
        }

        private void UI_Sesiones_Load(object sender, EventArgs e)
        {
            filter_sucursales();
            bindingSource1.MoveFirst();
            viewcodigointegrado();
            this_primary_object_persistent_current = ((XPBaseObject)bindingSource1.Current);
            //
            this.lookUpEdit_sucursales.Enabled = (Fundraising_PT.Properties.Settings.Default.U_tipo == 1);
            this.lookUp_caja.SetDataSource(cajas, "nombre", "oid");
            this.lookUp_cajero.SetDataSource(cajeros, "cajero", "oid");
            //
            this.lookUp_sucursal.Properties.DataSource = sucursales;
            this.lookUp_sucursal.Properties.DisplayMember = "nombre";
            this.lookUp_sucursal.Properties.ValueMember = "codigo";
            this.lookUp_sucursal.DataBindings.Clear();
            this.lookUp_sucursal.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", bindingSource1, "sucursal"));
            //
            this.grid_Base11.gridView1.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(gridView1_CustomColumnDisplayText);
            this.grid_Base11.gridView1.ViewCaption = "Listado de Sesiones";
            this.lookUp_elaborado.SetDataSource(usuarios, "usuario", "oid");
            textBox_buscacodigointegrado.Validated += textBox_buscacodigointegrado_Validated;
            textBox_buscacodigointegrado.textEdit1.KeyPress += textEdit1_KeyPress;
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void refresca_objetos()
        {
            //
            //lookUpEdit_sucursales.Update();
            lookUpEdit_sucursales.RefreshEditValue();
            //lookUpEdit_sucursales.Refresh();
            //lookUp_sucursal.Update();
            lookUp_sucursal.Refresh();
            //lookUp_sucursal.Refresh();
            //lookUp_caja.gridLookUpEdit1.Update();
            lookUp_caja.gridLookUpEdit1View.RefreshData();
            lookUp_caja.gridLookUpEdit1.RefreshEditValue();
            //lookUp_caja.gridLookUpEdit1.Refresh();
            //lookUp_cajero.gridLookUpEdit1.Update();
            lookUp_cajero.gridLookUpEdit1View.RefreshData();
            lookUp_cajero.gridLookUpEdit1.RefreshEditValue();
            //lookUp_cajero.gridLookUpEdit1.Refresh();
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

        void textBox_buscacodigointegrado_Validated(object sender, EventArgs e)
        {
            bool sw1 = true;
            if (textBox_buscacodigointegrado.lValue.Trim() != string.Empty)
            {
                //
                XPView vbuscacodigo = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Sesiones));
                vbuscacodigo.AddProperty("void", "oid", true, true, DevExpress.Xpo.SortDirection.None);
                vbuscacodigo.AddProperty("vcodigo_integrado", "Trim(ToStr(sucursal))+Trim(caja.codigo)+Trim(cajero.codigo)+Trim(ToStr(id_sesion))", true, true, DevExpress.Xpo.SortDirection.None);
                vbuscacodigo.CriteriaString = string.Format("Trim(ToStr(sucursal))+Trim(caja.codigo)+Trim(cajero.codigo)+Trim(ToStr(id_sesion)) = '{0}'", textBox_buscacodigointegrado.lValue.Trim());
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
                    MessageBox.Show("No se encontro ningun registro con el Código Integrado: " + textBox_buscacodigointegrado.lValue.Trim(),"Buscar Código Integrado.",MessageBoxButtons.OK,MessageBoxIcon.Information);
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
            viewcodigointegrado();
        }
        //
        //public override void bindingSource1_CurrentChanged(object sender, EventArgs e)
        //{
        //    base.bindingSource1_CurrentChanged(sender, e);
        //    //refresca_objetos();
        //    viewcodigointegrado();
        //}

        void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e != null && e.Column.FieldName == "fecha_hora")
            {
                e.Column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                e.Column.DisplayFormat.FormatString = "g";
            }

            if (e != null && e.Column.FieldName == "status")
            {
                e.DisplayText = ((Fundraising_PTDM.Enums.EStatus_sesion)e.Value).ToString();
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

            this.lookUp_caja.gridLookUpEdit1.EditValue = Guid.Empty;
            this.lookUp_cajero.gridLookUpEdit1.EditValue = Guid.Empty;
            this.dateTime_fecha_hora.dateEdit1.DateTime = DateTime.Now;
            if (lAccion == "Insertar")
            {
                lookUp_status.Enabled = false;
                lookUp_status.gridLookUpEdit1.EditValue = 1;
            }
        }

        public override void editar(object sender, EventArgs e)
        {
            //IXPCustomPropertyStore prop_custom = XPLiteObject.GetCustomPropertyStore((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)bindingSource1.Current);
            //IXPModificationsStore prop_modi = XPLiteObject.GetModificationsStore((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)bindingSource1.Current);

            if (this_primary_object_persistent_current != null)
            {
                try
                {
                    this_primary_object_persistent_current.Reload();
                    if (((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).status == 1)
                    {
                        base.editar(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("No se pueden editar los datos de la sesión porque ya tiene recaudaciones asociadas o en curso.", "Editar Sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {
                    Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("Data Reload", "Continuar", "Ignorar");
                    switch (MessageBox.Show("No se pudo leer los datos desde el servidor para el registro actual..." + Environment.NewLine + Environment.NewLine + "Posiblemente otro usuario lo elimino mientras usted lo tenia seleccionado para editarlo !!!" + Environment.NewLine + Environment.NewLine + "Seleccione una opción para continuar ?" + Environment.NewLine + Environment.NewLine + "Data Reload : Ejecutar (Data Reload) Vuelve a cargar todas las colecciones de datos desde el servidor." + Environment.NewLine + Environment.NewLine + "Cancelar : Cancela la edicion de datos del registro actual y salta al siguiente registro. " + Environment.NewLine + Environment.NewLine + "Ignorar : Ignora la lectura de los datos del registro desde el servidor y permanece sobre el.", "Data Reload (Editar)", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
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

        public override void eliminar(object sender, EventArgs e)
        {
            if (this_primary_object_persistent_current != null)
            {
                try
                {
                    //
                    this_primary_object_persistent_current.Reload();
                    if (((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).status == 1)
                    {
                        int cantidad_recaudaciones = (((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).RecaudacionesCollection == null ? 0 : ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).RecaudacionesCollection.Count);
                        if (cantidad_recaudaciones > 0)
                        {
                            if (MessageBox.Show("NO se puede Eliminar la sesión porque existen recaudaciones asociadas," + Environment.NewLine + "Desea cambiar el estatus de la sesion a Cerrada ?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                            {
                                ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).status = 4;
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
                    else
                    {
                        MessageBox.Show("No se puede eliminar la sesión porque ya tiene recaudaciones asociadas o en curso.", "Eliminar Sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public override void imprimir(object sender, EventArgs e)
        {
            base.imprimir(sender, e);
            this.grid_Base11.gridControl1.ShowRibbonPrintPreview();
        }

        public override void cancelar(object sender, EventArgs e)
        {
            base.cancelar(sender, e);
            viewcodigointegrado();

        }

        public override void guardar(object sender, EventArgs e)
        {
            //if (current_sesion != null)
            if (this_primary_object_persistent_current != null)
            {
                Guid lcaja = Guid.Empty;
                Guid lcajero = Guid.Empty;
                int id_sesion = 0;
                //
                //if (current_sesion.id_sesion <= 0 | current_sesion.id_sesion == null | this.lAccion == "Insertar")
                if (((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).id_sesion <= 0 | ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).id_sesion == null | this.lAccion == "Insertar")
                {
                    DevExpress.Xpo.XPQuery<Fundraising_PTDM.FUNDRAISING_PT.Sesiones> query = new DevExpress.Xpo.XPQuery<Fundraising_PTDM.FUNDRAISING_PT.Sesiones>(DevExpress.Xpo.XpoDefault.Session);
                    id_sesion = (from x in query select x.id_sesion).Max() + 1;
                }
                else
                {
                    //id_sesion = current_sesion.id_sesion;
                    id_sesion = ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).id_sesion;
                }
                if (this.lookUp_caja.gridLookUpEdit1.EditValue != null & this.lookUp_caja.gridLookUpEdit1.EditValue.ToString() != String.Empty)
                {
                    lcaja = (Guid)this.lookUp_caja.gridLookUpEdit1.EditValue;
                }
                if (this.lookUp_cajero.gridLookUpEdit1.EditValue != null & this.lookUp_cajero.gridLookUpEdit1.EditValue.ToString() != String.Empty)
                {
                    lcajero = (Guid)this.lookUp_cajero.gridLookUpEdit1.EditValue;
                }
                //
                if (valida_fechahora_sesion((this.dateTime_fecha_hora.dateEdit1.DateTime == null || this.dateTime_fecha_hora.dateEdit1.DateTime.ToString().Trim() == String.Empty ? DateTime.Now : DateTime.Parse(this.dateTime_fecha_hora.dateEdit1.DateTime.ToString())), DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Cajas>(lcaja)))
                {
                    //current_sesion.id_sesion = id_sesion;
                    //current_sesion.elaborado = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(Fundraising_PT.Properties.Settings.Default.U_oid);
                    //current_sesion.caja = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Cajas>(lcaja);
                    //current_sesion.cajero = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Cajeros>(lcajero);
                    //current_sesion.fecha_hora = (this.dateTime_fecha_hora.dateEdit1.DateTime == null || this.dateTime_fecha_hora.dateEdit1.DateTime.ToString().Trim() == String.Empty ? DateTime.Now : DateTime.Parse(this.dateTime_fecha_hora.dateEdit1.DateTime.ToString()));
                    //current_sesion.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                    //
                    ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).id_sesion = id_sesion;
                    ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).elaborado = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(Fundraising_PT.Properties.Settings.Default.U_oid);
                    ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).caja = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Cajas>(lcaja);
                    ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).cajero = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Cajeros>(lcajero);
                    ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).fecha_hora = (this.dateTime_fecha_hora.dateEdit1.DateTime == null || this.dateTime_fecha_hora.dateEdit1.DateTime.ToString().Trim() == String.Empty ? DateTime.Now : DateTime.Parse(this.dateTime_fecha_hora.dateEdit1.DateTime.ToString()));
                    ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                    //
                    if (this.lAccion == "Insertar")
                    {
                        //current_sesion.status = 1;
                        ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).status = 1;
                    }
                    //
                    base.guardar(sender, e);
                    viewcodigointegrado();

                }
                else
                {
                    MessageBox.Show("No se puede guardar la sesión porque la caja ya tiene una sesión dentro del limite de tiempo NO permitido.", "Guardar Sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public override void filter_sucursales()
        {
            cajas.CriteriaString = my_sucursal_filter;
            cajas.Reload();
            //
            cajeros.Reload();
            //
            sesiones.CriteriaString = my_sucursal_filter;
            sesiones.Reload();
            //
            bindingSource1.DataSource = sesiones;
            bindingSource1.MoveFirst();
        }

        private void viewcodigointegrado()
        {
            //if (current_sesion != null)
            if (this_primary_object_persistent_current != null)
            {
                //lc_codigointegrado = (current_sesion.sucursal == null ? "Sin Asignar" : current_sesion.sucursal.ToString().Trim()) + (current_sesion.caja == null ? "-Sin Asignar" : current_sesion.caja.codigo.Trim()) + (current_sesion.cajero == null ? "-Sin Asignar" : current_sesion.cajero.codigo.Trim()) + (current_sesion.id_sesion == null | current_sesion.id_sesion <= 0 ? "-Sin Asignar" : current_sesion.id_sesion.ToString().Trim());
                lc_codigointegrado = (((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).sucursal == null ? "Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).sucursal.ToString().Trim()) + (((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).caja == null ? "-Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).caja.codigo.Trim()) + (((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).cajero == null ? "-Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).cajero.codigo.Trim()) + (((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).id_sesion == null | ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).id_sesion <= 0 ? "-Sin Asignar" : ((Fundraising_PTDM.FUNDRAISING_PT.Sesiones)this_primary_object_persistent_current).id_sesion.ToString().Trim());
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
            usuarios.Dispose();
            cajas.Dispose();
            cajeros.Dispose();
            sucursales.Dispose();
            sesiones.Dispose();
            bindingSource1.Dispose();
        }

        public override void datareload()
        {
            base.datareload();
            //
            //usuarios.Reload();
            //cajas.Reload();
            //cajeros.Reload();
            //sucursales.Reload();
            //sesiones.Reload();
            //
            //DevExpress.Xpo.XpoDefault.Session.Reload(usuarios, true);
            //DevExpress.Xpo.XpoDefault.Session.Reload(cajas, true);
            //DevExpress.Xpo.XpoDefault.Session.Reload(cajeros, true);
            //DevExpress.Xpo.XpoDefault.Session.Reload(sucursales, true);
            //DevExpress.Xpo.XpoDefault.Session.Reload(sesiones, true);
            //
            usuarios.Load();
            cajas.Load();
            cajeros.Load();
            sucursales.Load();
            sesiones.Load();
            //
            usuarios.Reload();
            cajas.Reload();
            cajeros.Reload();
            sucursales.Reload();
            sesiones.Reload();
            //
            this.lookUpEdit_sucursales.Properties.DataSource = Fundraising_PT.Clases.Setting_Sucursales.data_sucursales();
            //
            filter_sucursales();
        }
        
        static bool valida_fechahora_sesion(DateTime ld_fecha, Fundraising_PTDM.FUNDRAISING_PT.Cajas lo_caja)
        {
            bool pass_fechahora = true;
            int total_minutes_new = (ld_fecha.Hour * 60) + ld_fecha.Minute;
            int time_new_sesion = Fundraising_PT.Properties.Settings.Default.time_new_sesion;
            XPView vhora_sesion = new XPView(XpoDefault.Session, typeof(Fundraising_PTDM.FUNDRAISING_PT.Sesiones));
            vhora_sesion.AddProperty("fecha_hora", "fecha_hora", false, true, DevExpress.Xpo.SortDirection.Descending);
            vhora_sesion.Criteria = CriteriaOperator.Parse(string.Format("GetDay(fecha_hora) = {0} and GetMonth(fecha_hora) = {1} and GetYear(fecha_hora) = {2} and caja.oid = '{3}'", ld_fecha.Day, ld_fecha.Month, ld_fecha.Year, lo_caja.oid));
            //
            if (vhora_sesion.Count > 0)
            {
                int time_diff = 0;
                int total_minutes_exis = 0;
                foreach (ViewRecord items in vhora_sesion)
                {
                    DateTime fecha_hora = (DateTime)items["fecha_hora"];
                    total_minutes_exis = (fecha_hora.Hour * 60) + fecha_hora.Minute;
                    time_diff = total_minutes_new - total_minutes_exis;
                    if (time_diff < time_new_sesion)
                    {
                        pass_fechahora = false;
                        break;
                    }
                }
            }
            //
            vhora_sesion.Dispose();
            //
            return (pass_fechahora);
        }

    }
}
