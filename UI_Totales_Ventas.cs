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

namespace Fundraising_PT.Formularios
{
    public partial class UI_Totales_Ventas : DevExpress.XtraEditors.XtraForm
    {
        private decimal ln_total_ventas = 0;
        private decimal ln_total_ventas_ini = 0;
        private decimal ln_monto_punto = 0;
        public int ln_status_totalventa = 0;
        public int ln_status_totalventa_ini = 0;
        public int ln_status_sesion = 0;
        public bool ll_active_ajust = false;
        private string lAccion = "";
        private string lHeader_ant = "";
        private string lc_fecha_totales = "";
        private DateTime ld_fecha_totales = DateTime.Now;
        private object Form_padre;
        private Guid lg_recaudacion = new Guid();
        private Guid lg_total_venta = new Guid();
        private Guid lg_forma_pago = new Guid();
        private DataTable totales_formas_pagos = new DataTable();
        private DataTable totales_formas_pagos_punto_bancario = new DataTable();
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas> totales_ventas;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas> totales_ventas_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas_Des> totales_ventas_des;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas_Des> totales_ventas_des_aux;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos> formas_pagos;
        private DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios> puntos_bancarios;

        public UI_Totales_Ventas(object form_padre)
        {
            WaitForm1 WaitForm1 = new WaitForm1();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(WaitForm1, typeof(WaitForm1), false, false, false);
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Totales Ventas...");
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("Espere un momento por favor... Cargando y Validando Datos...   .");
            //
            // asignacion de valores a objetos publicos //
            Form_padre = form_padre;
            ln_status_sesion = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).bindingSource1.Current).sesion.status;
            lg_recaudacion = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).bindingSource1.Current).oid;
            ln_status_totalventa = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).bindingSource1.Current).status_tv;
            ln_status_totalventa_ini = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).bindingSource1.Current).status_tv;
            lHeader_ant = ((Fundraising_PT.Formularios.UI_Recaudaciones)form_padre).HeaderMenu.Caption;
            lAccion = "Totales Ventas por Formas de Pago";
            //
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //
            // llena los datos de la coleccion de las formas de pago //
            CriteriaOperator filtro_status = (new OperandProperty("status") == new OperandValue(1));
            DevExpress.Xpo.SortProperty orden_formas_pagos = (new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            formas_pagos = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>(DevExpress.Xpo.XpoDefault.Session, filtro_status, orden_formas_pagos);
            puntos_bancarios = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios>(DevExpress.Xpo.XpoDefault.Session, filtro_status, orden_formas_pagos);
            //
            // llena los datos de la coleccion de los totales x tarjetas y puntos de ventas filtrada x recaudacion //
            CriteriaOperator filtro_totales_ventas_des = (new OperandProperty("total_venta.recaudacion.oid") == new OperandValue(lg_recaudacion));
            DevExpress.Xpo.SortProperty orden_totales_ventas_des = (new DevExpress.Xpo.SortProperty("punto_bancario.codigo", DevExpress.Xpo.DB.SortingDirection.Ascending));
            totales_ventas_des = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas_Des>(DevExpress.Xpo.XpoDefault.Session, filtro_totales_ventas_des, orden_totales_ventas_des);
            // llena los datos de la coleccion de los totales x formas de pago filtrada x recaudacion //
            CriteriaOperator filtro_totales_ventas = (new OperandProperty("recaudacion.oid") == new OperandValue(lg_recaudacion));
            DevExpress.Xpo.SortProperty orden_totales_ventas = (new DevExpress.Xpo.SortProperty("forma_pago.tpago", DevExpress.Xpo.DB.SortingDirection.Ascending));
            totales_ventas = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas>(DevExpress.Xpo.XpoDefault.Session, filtro_totales_ventas, orden_totales_ventas);
        }

        private void UI_Totales_Ventas_Load(object sender, EventArgs e)
        {
            // se crean las columnas al datatable de los totales x formas de pago
            if (totales_formas_pagos.Columns.Count <= 0)
            {
                totales_formas_pagos.Columns.Add("oid_tv", typeof(Guid));
                totales_formas_pagos.Columns.Add("oid", typeof(Guid));
                totales_formas_pagos.Columns.Add("nombre", typeof(string));
                totales_formas_pagos.Columns.Add("monto_manual", typeof(decimal));
                totales_formas_pagos.Columns.Add("datatable_puntos_bancarios", typeof(DataTable));
                totales_formas_pagos.Columns.Add("tpago", typeof(int));
                totales_formas_pagos.PrimaryKey = new DataColumn[] { totales_formas_pagos.Columns["oid"] };
            }
            // borra todas las filas que puedan existir antes de volver a cargarlas ///
            totales_formas_pagos.Rows.Clear();

            // llena datatable de los totales x formas de pago //
            if (totales_formas_pagos.Rows.Count <= 0)
            {
                // carga datos del collection formas_pagos al datatable de los totales x formas de pago //
                foreach (var item_formas_pagos in formas_pagos)
                {
                    totales_formas_pagos.Rows.Add(new Guid(), item_formas_pagos.oid, item_formas_pagos.nombre, 0, new DataTable(), item_formas_pagos.tpago);
                }
            }

            // asigna el datatable de totales_formas_pagos al grid //
            this.gridControl_totales_ventas.DataSource = totales_formas_pagos;
            this.gridControl_totales_ventas.RefreshDataSource();

            // carga los valores desde la coleccion de totales ventas filtrada por recaudacion al datatable y asigna el total inicial // 
            carga_totales_ventas();
            ln_total_ventas_ini = totales_formas_pagos.AsEnumerable().Sum((tot_ventas) => tot_ventas.Field<decimal>("monto_manual"));

            // setea los controles segun el estatus de la carga de totales ventas // 
            seteo_status_totalventa();
            ll_active_ajust = false;
            
            // bindeos necesarios para el funcionamiento //
            //this.gridView_totales_ventas.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(gridView_totales_ventas_CustomColumnDisplayText);
            this.gridColumn_totalesventas_monto.View.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(View_CellValueChanged);
            this.simpleButton_ajustar.Click += new EventHandler(simpleButton_ajustar_Click);
            this.simpleButton_aceptar.Click += new EventHandler(simpleButton_aceptar_Click);
            this.simpleButton_cancelar.Click += new EventHandler(simpleButton_cancelar_Click);
            this.simpleButton_imprimir.Click += new EventHandler(simpleButton_imprimir_Click);
            this.repositoryItemButtonEdit1.Click += new EventHandler(repositoryItemButtonEdit1_Click);
            this.dateEdit_fecha_hora_totales.DateTimeChanged += new EventHandler(dateEdit_fecha_hora_totales_DateTimeChanged);
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, null);
        }

        void dateEdit_fecha_hora_totales_DateTimeChanged(object sender, EventArgs e)
        {
            ld_fecha_totales = dateEdit_fecha_hora_totales.DateTime;
            lc_fecha_totales = ld_fecha_totales.ToShortDateString();
            label_fecha_totales.Text = lc_fecha_totales;
        }

        void simpleButton_ajustar_Click(object sender, EventArgs e)
        {
            try
            {
                ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current.Reload();
                ln_status_totalventa = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current).status_tv;
                ln_status_totalventa_ini = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).status_tv;
                seteo_status_totalventa();
                ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).seteo_status_totalventa();
                //
                if (ln_status_sesion == 4)
                {
                    MessageBox.Show("NO se puede hacer ajustes a la totalización de ventas." + Environment.NewLine + "La sesión asociada se encuentra cerrada.", "Ajustar Totalización de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ll_active_ajust = false;
                }
                else
                {
                    if (ln_status_totalventa == 4)
                    {
                        MessageBox.Show("NO se puede hacer ajustes a la totalización de ventas." + Environment.NewLine + "Se encuentra en estatus: (ANULADA).", "Ajustar Totalización de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ll_active_ajust = false;
                    }
                    else
                    {
                        if (ln_status_totalventa == 6)
                        {
                            MessageBox.Show("NO se puede hacer ajustes a la totalización de ventas." + Environment.NewLine + "Otro usuario la tiene en estatus: (EN PROCESO).", "Ajustar Totalización de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ll_active_ajust = false;
                        }
                        else
                        {
                            ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current).status_tv = 6;
                            ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current.Save();
                            ln_status_totalventa = 6;
                            //
                            ll_active_ajust = true;
                            seteo_status_totalventa();
                            ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).seteo_status_totalventa();
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo actualizar los datos desde el servidor para el registro seleccionado...", "Ajustar Totalización de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void simpleButton_imprimir_Click(object sender, EventArgs e)
        {
            this.gridControl_totales_ventas.ShowRibbonPrintPreview();
        }

        void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            if ((int)this.gridView_totales_ventas.GetFocusedDataRow()["tpago"] == 2)
            {
                Guid lg_oid = new Guid();
                DataTable puntos_bancarios_aux = new DataTable();
                DataRow datarow_gridView_totales_ventas;
                DataRow datarow_table_totales_formas_pagos;
                datarow_gridView_totales_ventas = this.gridView_totales_ventas.GetFocusedDataRow();
                lg_oid = ((Guid)datarow_gridView_totales_ventas["oid"]);
                datarow_table_totales_formas_pagos = totales_formas_pagos.Rows.Find(lg_oid);
                lg_total_venta = ((Guid)datarow_table_totales_formas_pagos["oid_tv"]);
                puntos_bancarios_aux = ((DataTable)datarow_table_totales_formas_pagos["datatable_puntos_bancarios"]);
                // se crean las columnas al datatable del desgloce por puntos bancarios si no se han creeado //
                if (puntos_bancarios_aux.Columns.Count <= 0)
                {
                    puntos_bancarios_aux.Columns.Add("oid", typeof(Guid));
                    puntos_bancarios_aux.Columns.Add("codigo", typeof(string));
                    puntos_bancarios_aux.Columns.Add("descr", typeof(string));
                    puntos_bancarios_aux.Columns.Add("monto_manual_des", typeof(decimal));
                    puntos_bancarios_aux.PrimaryKey = new DataColumn[] { puntos_bancarios_aux.Columns["oid"] };
                }

                // llena datatable del desgloce por puntos bancarios si no se ha cargado //
                if (puntos_bancarios_aux.Rows.Count <= 0)
                {
                    // carga datos del collection puntos bancarios al datatable del desgloce por puntos bancarios //
                    foreach (var item_puntos_bancarios in puntos_bancarios)
                    {
                        ln_monto_punto = 0;
                        CriteriaOperator filtro_totales_ventas_des_aux = (new OperandProperty("total_venta.oid") == new OperandValue(lg_total_venta) & new OperandProperty("punto_bancario.oid") == new OperandValue(item_puntos_bancarios.oid));
                        totales_ventas_des_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas_Des>(totales_ventas_des, filtro_totales_ventas_des_aux);
                        if (totales_ventas_des_aux != null & totales_ventas_des_aux.Count > 0)
                        { ln_monto_punto = totales_ventas_des_aux[0].monto_manual_des; }
                        //
                        puntos_bancarios_aux.Rows.Add(item_puntos_bancarios.oid, item_puntos_bancarios.codigo, item_puntos_bancarios.descr, ln_monto_punto);
                    }
                }
                //
                Formularios.UI_Totales_Ventas_Des form_totales_ventas_des = new Fundraising_PT.Formularios.UI_Totales_Ventas_Des(Form_padre, this, puntos_bancarios_aux, datarow_gridView_totales_ventas);
                //form_totales_ventas_des.MdiParent = this.MdiParent;
                this.gridControl_totales_ventas.Enabled = false;
                this.simpleButton_ajustar.Enabled = false;
                this.simpleButton_aceptar.Enabled = false;
                this.simpleButton_cancelar.Enabled = false;
                this.simpleButton_salir.Enabled = false;
                this.ControlBox = false;
                //form_totales_ventas_des.Show();
                form_totales_ventas_des.ShowDialog(this);
            }
            else
            {
                MessageBox.Show("Detalle Tarjeta x Puntos bancarios solo para formas de pago: (TIPO: TARJETAS)", "Detalle Tarjeta x Puntos bancarios");
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).HeaderMenu.Caption = lHeader_ant + " - Proceso : " + this.lAccion;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).barra_Mant_Base11.Enabled = true;
            ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).simpleButton_datos_ventas.Enabled = true;
            ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).simpleButton_datos_ventas2.Enabled = true;
            ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).grid_Base11.Enabled = true;
            ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).ControlBox = true;
            ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).HeaderMenu.Caption = this.lHeader_ant;
            ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).seteo_status_totalventa();
        }

        void View_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            DataRow current_row_grid = this.gridView_totales_ventas.GetFocusedDataRow();
            if (((int)current_row_grid["tpago"]) != 2)
            {
                ln_total_ventas = totales_formas_pagos.AsEnumerable().Sum((tot_ventas) => tot_ventas.Field<decimal>("monto_manual"));
            }
            else
            {
                //e.Value;
            }
        }

        private void carga_totales_ventas()
        {
            lg_total_venta = new Guid();
            lg_forma_pago = new Guid();
            if (totales_formas_pagos.Rows.Count > 0)
            {
                // carga datos de los totales x formas de pago //
                foreach (DataRow row_tot_fp in totales_formas_pagos.Rows)
                {
                    lg_forma_pago = Guid.Parse(row_tot_fp[1].ToString());
                    //
                    CriteriaOperator filtro_fpago = (new OperandProperty("forma_pago.oid") == new OperandValue(lg_forma_pago));
                    totales_ventas_aux = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas>(totales_ventas, filtro_fpago);
                    //
                    if (totales_ventas_aux != null & totales_ventas_aux.Count > 0)
                    {
                        row_tot_fp["oid_tv"] = totales_ventas_aux[0].oid;
                        row_tot_fp["monto_manual"] = totales_ventas_aux[0].monto_manual;
                    }
                }
            }
            ln_total_ventas = totales_formas_pagos.AsEnumerable().Sum((tot_ventas) => tot_ventas.Field<decimal>("monto_manual"));
        }
       
        void simpleButton_aceptar_Click(object sender, EventArgs e)
        {
            int lnStatus_auxiliar = 0;

            if ((ln_status_totalventa == 0 || ln_status_totalventa == 1 || ln_status_totalventa == 6) & ll_active_ajust == false)
            {
                Fundraising_PT.Clases.MsgBoxUtil.HackMessageBox("Abierta", "Cerrada", "Cancelar");
                switch (MessageBox.Show("Seleccione como desea guardar la Totalización de Ventas ?" + Environment.NewLine + Environment.NewLine + "Abierta : Deja la Totalización abierta para ediciónes de datos." + Environment.NewLine + Environment.NewLine + "Cerrada : Cierra la Totalización y solo se permitiran ajustes.", "Guardar Totalización de Ventas", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Yes:
                        lnStatus_auxiliar = 1;
                        break;
                    case DialogResult.No:
                        if (ln_total_ventas_ini == 0 & ln_total_ventas == 0)
                        {
                            lnStatus_auxiliar = 5;
                        }
                        else
                        {
                            lnStatus_auxiliar = 2;
                        }
                        break;
                    default:
                        lnStatus_auxiliar = 0;
                        break;
                }
                Fundraising_PT.Clases.MsgBoxUtil.UnHackMessageBox();
            }
            else
            {
                if ((ln_status_totalventa == 2 || ln_status_totalventa == 3 || ln_status_totalventa == 5) | (ln_status_totalventa == 0 || ln_status_totalventa == 1 || ln_status_totalventa == 6) & ll_active_ajust == true)
                {
                    if (ln_total_ventas_ini == 0 & ln_total_ventas == 0)
                    {
                        lnStatus_auxiliar = 5;
                    }
                    else
                    {
                        lnStatus_auxiliar = 3;
                    }
                }
                else
                { lnStatus_auxiliar = 0; }
            }
            //
            if (lnStatus_auxiliar != 0 & ln_status_totalventa != 4)
            {
                // GUARDA TOTALES VENTAS POR FORMA DE PAGO //
                if (MessageBox.Show("Esta seguro de GUARDAR los datos con la seleccion correspondiente ?", "Guardar Totales Ventas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                {
                    // se inicia una transaccion para guardar los datos //
                    DevExpress.Xpo.XpoDefault.Session.BeginTransaction();
                    try
                    {
                        // recorre la tabla de totales_formas_pagos y guarda los datos //
                        foreach (DataRow row_totven_fp in totales_formas_pagos.Rows)
                        {
                            Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas totales_ventas_aux1 = null;
                            try
                            {
                                totales_ventas_aux1 = totales_ventas.First<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas>(exis_tot_fp => exis_tot_fp.forma_pago.oid == (Guid)row_totven_fp["oid"]);
                            }
                            catch
                            {
                                totales_ventas_aux1 = null;
                            }
                            //                              
                            if ((decimal)row_totven_fp["monto_manual"] > 0)
                            {
                                if (totales_ventas_aux1 == null)
                                {
                                    totales_ventas_aux1 = new Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas(DevExpress.Xpo.XpoDefault.Session);
                                    totales_ventas_aux1.recaudacion = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones>(lg_recaudacion);
                                    totales_ventas_aux1.forma_pago = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Formas_Pagos>((Guid)row_totven_fp["oid"]);
                                }
                                totales_ventas_aux1.monto_manual = (decimal)row_totven_fp["monto_manual"];
                                totales_ventas_aux1.status = 1;
                                totales_ventas_aux1.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                totales_ventas_aux1.Save();
                            }
                            else
                            {
                                if (totales_ventas_aux1 != null)
                                {
                                    totales_ventas_aux1.Delete();
                                    totales_ventas_aux1.Save();
                                }
                            }
                            //
                        } // final del foreach
                        // cambia estatus de totales ventas en la recaudacion y termina el proceso //
                        Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones recaudacion_aux = DevExpress.Xpo.XpoDefault.Session.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones>(lg_recaudacion);
                        if (recaudacion_aux.status_tv == 2)
                        {
                            ln_status_totalventa = 3;
                            lnStatus_auxiliar = 3;
                            recaudacion_aux.status_tv = 3;
                            recaudacion_aux.Save();
                        }
                        else
                        {
                            if (recaudacion_aux.status_tv == 0 || recaudacion_aux.status_tv == 1 || recaudacion_aux.status_tv == 6)
                            {
                                ln_status_totalventa = lnStatus_auxiliar;
                                recaudacion_aux.supervisor = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(Fundraising_PT.Properties.Settings.Default.U_oid);
                                recaudacion_aux.status_tv = lnStatus_auxiliar;
                                recaudacion_aux.fecha_hora_tv = ld_fecha_totales; 
                                recaudacion_aux.Save();
                            }
                            else
                            {
                                ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current).status_tv = ln_status_totalventa_ini;
                                ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current.Save();
                                ln_status_totalventa = ln_status_totalventa_ini;
                            }
                        }
                        //
                        DevExpress.Xpo.XpoDefault.Session.CommitTransaction();
                        MessageBox.Show("Datos Guardados Correctamente...", "Guardar Totales Ventas");
                        //
                        try
                        {
                            totales_ventas.Reload();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Otro Usuario reviso y modifico datos mientras usted lo tenia en proceso...", "Guardar Totales Ventas");
                        }
                        carga_totales_ventas();
                        //
                        // actualiza el desgloce x puntos bancarios de las formas de pago tipo tarjeta.
                        this.actualiza_detalle_tarjetas_pb(totales_formas_pagos);
                        //
                        ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).seteo_status_totalventa();
                        this.Close();
                        //
                    } // final del 1er. Try
                    catch (Exception oerror)
                    {
                        MessageBox.Show("Ocurrio un ERROR durante el proceso de guardar los datos, se reversara dicho proceso..."+Environment.NewLine + "Error: " + oerror.Message, "Guardar Totales Ventas");
                        DevExpress.Xpo.XpoDefault.Session.RollbackTransaction();
                        //
                        ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).seteo_status_totalventa();
                        this.Close();
                        //
                    }
                } // final del IF MESSAGEBOX
                // FINAL GUARDA TOTALES VENTAS POR FORMA DE PAGO //        
            } // final del IF estatus_tv != 0 and != 4                
        }

        private void actualiza_detalle_tarjetas_pb(DataTable totales_formas_pagos_aux)
        {
            foreach (DataRow row_totven_fpaux in totales_formas_pagos_aux.Rows)
            {
                Guid oid_tv_aux = Guid.Empty;
                int tpago_aux = (int)row_totven_fpaux["tpago"];
                if (tpago_aux == 2)
                {
                    oid_tv_aux = (Guid)row_totven_fpaux["oid_tv"];
                    if ((decimal)row_totven_fpaux["monto_manual"] > 0)
                    {
                        DataTable puntos_bancarios_aux1 = new DataTable();
                        puntos_bancarios_aux1 = ((DataTable)row_totven_fpaux["datatable_puntos_bancarios"]);
                        foreach (DataRow row_tot_tpb in puntos_bancarios_aux1.Rows)
                        {
                            Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas_Des totales_ventas_des_aux1 = null;
                            try
                            {
                                totales_ventas_des_aux1 = totales_ventas_des.First<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas_Des>(exis_tot_tfp => exis_tot_tfp.total_venta.oid == oid_tv_aux & exis_tot_tfp.punto_bancario.oid == (Guid)row_tot_tpb["oid"]);
                            }
                            catch
                            {
                                totales_ventas_des_aux1 = null;
                            }
                            //                              
                            if ((decimal)row_tot_tpb["monto_manual_des"] > 0)
                            {
                                if (totales_ventas_des_aux1 == null)
                                {
                                    totales_ventas_des_aux1 = new Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas_Des(DevExpress.Xpo.XpoDefault.Session);
                                    totales_ventas_des_aux1.total_venta = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas>(oid_tv_aux);
                                    totales_ventas_des_aux1.punto_bancario = DevExpress.Xpo.Session.DefaultSession.GetObjectByKey<Fundraising_PTDM.FUNDRAISING_PT.Puntos_Bancarios>((Guid)row_tot_tpb["oid"]);
                                }
                                totales_ventas_des_aux1.monto_manual_des = (decimal)row_tot_tpb["monto_manual_des"];
                                totales_ventas_des_aux1.sucursal = Fundraising_PT.Properties.Settings.Default.sucursal;
                                totales_ventas_des_aux1.Save();
                            }
                            else
                            {
                                if (totales_ventas_des_aux1 != null)
                                {
                                    totales_ventas_des_aux1.Delete();
                                    totales_ventas_des_aux1.Save();
                                }
                            }
                        } // final del foreach
                    }
                    else
                    {
                        DataTable puntos_bancarios_aux1 = new DataTable();
                        puntos_bancarios_aux1 = ((DataTable)row_totven_fpaux["datatable_puntos_bancarios"]);
                        foreach (DataRow row_tot_tpb in puntos_bancarios_aux1.Rows)
                        {
                            Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas_Des totales_ventas_des_aux1 = null;
                            try
                            {
                                totales_ventas_des_aux1 = totales_ventas_des.First<Fundraising_PTDM.FUNDRAISING_PT.Totales_Ventas_Des>(exis_tot_tfp => exis_tot_tfp.total_venta.oid == oid_tv_aux & exis_tot_tfp.punto_bancario.oid == (Guid)row_tot_tpb["oid"]);
                                totales_ventas_des_aux1.Delete();
                                totales_ventas_des_aux1.Save();
                            }
                            catch
                            {
                                totales_ventas_des_aux1 = null;
                            }

                        }
                    }
                }
            }
        }

        void simpleButton_cancelar_Click(object sender, EventArgs e)
        {
            if (ln_total_ventas != ln_total_ventas_ini)
            {
                if (MessageBox.Show("Esta seguro de CANCALAR las modificaciones ?", "Cancelar Modificacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
                {
                    // carga los valores desde la coleccion de totales ventas filtrada por recaudacion al datatable y asigna el total inicial // 
                    carga_totales_ventas();
                    this.gridControl_totales_ventas.RefreshDataSource();
                    ln_total_ventas_ini = totales_formas_pagos.AsEnumerable().Sum((tot_ventas) => tot_ventas.Field<decimal>("monto_manual"));
                    //
                    if (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current).status_tv != ln_status_totalventa_ini)
                    {
                        ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current).status_tv = ln_status_totalventa_ini;
                        ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current.Save();
                        ln_status_totalventa = ln_status_totalventa_ini;
                    }
                    ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).seteo_status_totalventa();
                    seteo_status_totalventa();
                    ll_active_ajust = false;
                }
            }
        }

        private void simpleButton_salir_Click(object sender, EventArgs e)
        {
            if (ln_total_ventas != ln_total_ventas_ini)
            {
                if (MessageBox.Show("Esta seguro de SALIR ?" + Environment.NewLine + "Los datos se modificaron y se perderan los cambios.", "Salir", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
                {
                    if (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current).status_tv != ln_status_totalventa_ini)
                    {
                        ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current).status_tv = ln_status_totalventa_ini;
                        ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current.Save();
                        ln_status_totalventa = ln_status_totalventa_ini;
                    }
                    ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).seteo_status_totalventa();
                    //
                    this.WindowState = FormWindowState.Normal;
                    this.Close();
                }
            }
            else
            {
                if (((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current).status_tv != ln_status_totalventa_ini)
                {
                    ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current).status_tv = ln_status_totalventa_ini;
                    ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).this_primary_object_persistent_current.Save();
                    ln_status_totalventa = ln_status_totalventa_ini;
                }
                ((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).seteo_status_totalventa();
                //
                this.WindowState = FormWindowState.Normal;
                this.Close();
            }
        }

        public void seteo_status_totalventa()
        {
            if (Fundraising_PT.Properties.Settings.Default.U_tipo != 1)
            {
                this.simpleButton_ajustar.Visible = false;
            }

            switch (ln_status_totalventa)
            {
                case 1:
                    gridColumn_totalesventas_monto.OptionsColumn.ReadOnly = false;
                    //
                    simpleButton_ajustar.Enabled = false;
                    simpleButton_aceptar.Enabled = true;
                    simpleButton_cancelar.Enabled = true;
                    //
                    if (ll_active_ajust)
                    {
                        label_estatus_totalventas.Text = "Abierta para ajustar";
                        //ll_active_ajust = false;
                    }
                    else
                    {
                        label_estatus_totalventas.Text = "Abierta";
                        //ll_active_ajust = false;
                    }
                    label_estatus_totalventas.Appearance.ForeColor = Color.GreenYellow;
                    label_estatus_totalventas.LineColor = Color.GreenYellow;
                    //
                    ld_fecha_totales = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).fecha_hora_tv;
                    lc_fecha_totales = ld_fecha_totales.ToShortDateString();
                    label_fecha_totales.Text = lc_fecha_totales;
                    dateEdit_fecha_hora_totales.DateTime = ld_fecha_totales;
                    dateEdit_fecha_hora_totales.Enabled = true;
                    //
                    break;
                case 2:
                    gridColumn_totalesventas_monto.OptionsColumn.ReadOnly = true;
                    //
                    simpleButton_ajustar.Enabled = true;
                    simpleButton_aceptar.Enabled = false;
                    simpleButton_cancelar.Enabled = false;
                    //
                    label_estatus_totalventas.Text = "Cerrada_Normal";
                    label_estatus_totalventas.Appearance.ForeColor = Color.DeepSkyBlue;
                    label_estatus_totalventas.LineColor = Color.DeepSkyBlue;
                    //
                    ld_fecha_totales = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).fecha_hora_tv;
                    lc_fecha_totales = ld_fecha_totales.ToShortDateString();
                    label_fecha_totales.Text = lc_fecha_totales;
                    dateEdit_fecha_hora_totales.DateTime = ld_fecha_totales;
                    dateEdit_fecha_hora_totales.Enabled = false;
                    //
                    break;
                case 3:
                    gridColumn_totalesventas_monto.OptionsColumn.ReadOnly = true;
                    //
                    simpleButton_ajustar.Enabled = true;
                    simpleButton_aceptar.Enabled = false;
                    simpleButton_cancelar.Enabled = false;
                    //
                    label_estatus_totalventas.Text = "Cerrada_Ajustada";
                    label_estatus_totalventas.Appearance.ForeColor = Color.Gold;
                    label_estatus_totalventas.LineColor = Color.Gold;
                    //
                    ld_fecha_totales = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).fecha_hora_tv;
                    lc_fecha_totales = ld_fecha_totales.ToShortDateString();
                    label_fecha_totales.Text = lc_fecha_totales;
                    dateEdit_fecha_hora_totales.DateTime = ld_fecha_totales;
                    dateEdit_fecha_hora_totales.Enabled = false;
                    //
                    break;
                case 4:
                    gridColumn_totalesventas_monto.OptionsColumn.ReadOnly = true;
                    //
                    simpleButton_ajustar.Enabled = false;
                    simpleButton_aceptar.Enabled = false;
                    simpleButton_cancelar.Enabled = false;
                    //
                    label_estatus_totalventas.Text = "Anulada";
                    label_estatus_totalventas.Appearance.ForeColor = Color.Red;
                    label_estatus_totalventas.LineColor = Color.Red;
                    //
                    ld_fecha_totales = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).fecha_hora_tv;
                    lc_fecha_totales = ld_fecha_totales.ToShortDateString();
                    label_fecha_totales.Text = lc_fecha_totales;
                    dateEdit_fecha_hora_totales.DateTime = ld_fecha_totales;
                    dateEdit_fecha_hora_totales.Enabled = false;
                    //
                    break;
                case 5:
                    gridColumn_totalesventas_monto.OptionsColumn.ReadOnly = true;
                    //
                    simpleButton_ajustar.Enabled = true;
                    simpleButton_aceptar.Enabled = false;
                    simpleButton_cancelar.Enabled = false;
                    //
                    label_estatus_totalventas.Text = "Cerrada_sin_Montos";
                    label_estatus_totalventas.Appearance.ForeColor = Color.DeepSkyBlue;
                    label_estatus_totalventas.LineColor = Color.DeepSkyBlue;
                    //
                    ld_fecha_totales = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).fecha_hora_tv;
                    lc_fecha_totales = ld_fecha_totales.ToShortDateString();
                    label_fecha_totales.Text = lc_fecha_totales;
                    dateEdit_fecha_hora_totales.DateTime = ld_fecha_totales;
                    dateEdit_fecha_hora_totales.Enabled = false;
                    //
                    break;
                case 6:
                    gridColumn_totalesventas_monto.OptionsColumn.ReadOnly = false;
                    //
                    simpleButton_ajustar.Enabled = false;
                    simpleButton_aceptar.Enabled = true;
                    simpleButton_cancelar.Enabled = true;
                    //
                    if (ll_active_ajust)
                    {
                        label_estatus_totalventas.Text = "En Proceso para ajustar";
                        //ll_active_ajust = false;
                    }
                    else
                    {
                        label_estatus_totalventas.Text = "En Proceso";
                        //ll_active_ajust = false;
                    }
                    label_estatus_totalventas.Appearance.ForeColor = Color.LightCyan;
                    label_estatus_totalventas.LineColor = Color.LightCyan;
                    //
                    ld_fecha_totales = ((Fundraising_PTDM.FUNDRAISING_PT.Recaudaciones)((Fundraising_PT.Formularios.UI_Recaudaciones)Form_padre).bindingSource1.Current).fecha_hora_tv;
                    lc_fecha_totales = ld_fecha_totales.ToShortDateString();
                    label_fecha_totales.Text = lc_fecha_totales;
                    dateEdit_fecha_hora_totales.DateTime = ld_fecha_totales;
                    dateEdit_fecha_hora_totales.Enabled = true;
                    //
                    break;
                default:
                    gridColumn_totalesventas_monto.OptionsColumn.ReadOnly = false;
                    //
                    simpleButton_ajustar.Enabled = false;
                    simpleButton_aceptar.Enabled = true;
                    simpleButton_cancelar.Enabled = true;
                    //
                    label_estatus_totalventas.Text = "Sin Elaborar";
                    label_estatus_totalventas.Appearance.ForeColor = Color.Orange;
                    label_estatus_totalventas.LineColor = Color.Orange;
                    //
                    ld_fecha_totales = DateTime.Now;
                    lc_fecha_totales = ld_fecha_totales.ToShortDateString();
                    label_fecha_totales.Text = lc_fecha_totales;
                    dateEdit_fecha_hora_totales.DateTime = ld_fecha_totales;
                    dateEdit_fecha_hora_totales.Enabled = true;
                    //
                    break;
            }
            // activa o desactiva los controles dependiendo el status de la sesion //
            if (ln_status_sesion == 4)
            {
                this.simpleButton_ajustar.Enabled = false;
                this.simpleButton_aceptar.Enabled = false;
                this.simpleButton_cancelar.Enabled = false;
                dateEdit_fecha_hora_totales.Enabled = false;
                this.gridColumn_totalesventas_monto.OptionsColumn.ReadOnly = true;
            }
        }

    }
}