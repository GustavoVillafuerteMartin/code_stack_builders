using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using Microsoft.Data.ConnectionUI;
using System.Data.SqlClient;
using System.IO;
using DevExpress.Xpo;


namespace Fundraising_PT.Formularios
{
    public partial class UI_claveacceso : DevExpress.XtraEditors.XtraForm
    {
        public bool Lpass_usu { get; set; }
        public bool l_new_sucursal = false;
        public string l_login;
        public string l_clave;
        public string l_string_busqueda;
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios> usuario;
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales> sucursal;
        DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Configuracion> configuracion;
      
        public UI_claveacceso()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //Fundraising_PTDM.MyConnection.SetConnectionString("XpoProvider=MSSqlServer;data source=.;user id=sa;password=123sql456*;initial catalog=FUNDRAISING_PT;Persist Security Info=true");
        }

        private void UI_claveacceso_Load(object sender, EventArgs e)
        {
            // metodo de chequeo de la conexion
            check_connection();
            //
            Fundraising_PT.Properties.Settings.Default.U_oid = Guid.Empty;
            Fundraising_PT.Properties.Settings.Default.U_login = string.Empty;
            Fundraising_PT.Properties.Settings.Default.U_clave = string.Empty;
            Fundraising_PT.Properties.Settings.Default.U_usuario = string.Empty;
            Fundraising_PT.Properties.Settings.Default.U_tipo = 0;
            Fundraising_PT.Properties.Settings.Default.U_status = 0;
            //
            this.button_aceptar.Click +=new EventHandler(button_aceptar_Click);
            this.button_cancelar.Click +=new EventHandler(button_cancelar_Click);
            this.textBox_clave.KeyPress += new KeyPressEventHandler(textBox_clave_KeyPress);
        }

        void textBox_clave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == "\r")
            { button_aceptar_Click(this.button_aceptar, EventArgs.Empty); }
        }

        private void button_cancelar_Click(object sender, EventArgs e)
        {
            Lpass_usu = false;
            this.Close();
        }

        private void button_aceptar_Click(object sender, EventArgs e)
        {
            Lpass_usu = false;
            if (this.textBox_login.Text.Trim().Length >= 1)
            {
                l_login = this.textBox_login.Text.Trim();
                l_clave = this.textBox_clave.Text.Trim();
                //l_string_busqueda = string.Format("login = {0}", l_login);
                CriteriaOperator filtro_usuario = (new OperandProperty("login") == new OperandValue(l_login));
                DevExpress.Xpo.SortProperty orden_usuario = (new DevExpress.Xpo.SortProperty("login", DevExpress.Xpo.DB.SortingDirection.Descending));
                //usuario = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, CriteriaOperator.Parse(l_string_busqueda));
                usuario = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session, filtro_usuario, orden_usuario);
                //                
                if (usuario != null && usuario.Count() >= 1)
                {
                    //Lpass_usu = (from u in usuario select u.status).Equals(1);
                    //if (Lpass_usu)
                    if (usuario[0].status == 1)
                    {
                        //bool pass_clave = (from u in usuario select u.clave.Trim()).Equals(l_clave);
                        //if (pass_clave)
                        if (usuario[0].clave.Trim() == l_clave)
                        {
                            Fundraising_PT.Properties.Settings.Default.U_oid     = usuario[0].oid;
                            Fundraising_PT.Properties.Settings.Default.U_login   = usuario[0].login;
                            Fundraising_PT.Properties.Settings.Default.U_clave   = usuario[0].clave;
                            Fundraising_PT.Properties.Settings.Default.U_usuario = usuario[0].usuario;
                            Fundraising_PT.Properties.Settings.Default.U_tipo    = usuario[0].tipo;
                            Fundraising_PT.Properties.Settings.Default.U_status  = usuario[0].status;
                            //
                            configuracion = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Configuracion>(DevExpress.Xpo.XpoDefault.Session);
                            if (configuracion != null & configuracion.Count > 0)
                            {
                                Fundraising_PT.Properties.Settings.Default.Activa_Audio = configuracion[0].activa_audio;
                                Fundraising_PT.Properties.Settings.Default.time_new_sesion = configuracion[0].time_new_sesion;
                                if (configuracion[0].sucursal != null)
                                {
                                    //Fundraising_PT.Properties.Settings.Default.sucursal = configuracion[0].sucursal.codigo;
                                    //Fundraising_PT.Properties.Settings.Default.nombre_sucursal = configuracion[0].sucursal.nombre;
                                    //Fundraising_PT.Properties.Settings.Default.logotipo = configuracion[0].sucursal.logotipo;
                                    //Fundraising_PT.Properties.Settings.Default.sucursal_filter = string.Format("sucursal = {0}", configuracion[0].sucursal.codigo);
                                }
                                else
                                {
                                    //MessageBox.Show("La Sesion NO Tiene ninguna sucursal asociada en su setting..."+Environment.NewLine+"Favor ir a la opcion de asociar sucursal...", "Inicio de Sesion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //                        
                                    //Fundraising_PT.Properties.Settings.Default.sucursal = 0;
                                    //Fundraising_PT.Properties.Settings.Default.nombre_sucursal = "";
                                    //Fundraising_PT.Properties.Settings.Default.logotipo = "";
                                    //Fundraising_PT.Properties.Settings.Default.sucursal_filter = string.Format("sucursal = {0}", 0);
                                }
                            }
                            //
                            Fundraising_PT.Properties.Settings.Default.mypath_sistema = System.Windows.Forms.Application.StartupPath;
                            Fundraising_PT.Properties.Settings.Default.mypath_imagenes = System.Windows.Forms.Application.StartupPath + @"\imagenes\";
                            Fundraising_PT.Properties.Settings.Default.mypath_reports = System.Windows.Forms.Application.StartupPath + @"\reports\";
                            Fundraising_PT.Properties.Settings.Default.Save();
                            //
                            if (Directory.Exists(Fundraising_PT.Properties.Settings.Default.mypath_imagenes) == false)
                            {
                                Directory.CreateDirectory(Fundraising_PT.Properties.Settings.Default.mypath_imagenes);
                            }
                            if (Directory.Exists(Fundraising_PT.Properties.Settings.Default.mypath_reports) == false)
                            {
                                Directory.CreateDirectory(Fundraising_PT.Properties.Settings.Default.mypath_reports);
                            }
                            //
                            Lpass_usu = true;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Clave de usuario invalida...","Clave de Acceso",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            Lpass_usu = false;
                            this.textBox_clave.Clear();
                            this.textBox_clave.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Usuario NO esta activo...", "Clave de Acceso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Lpass_usu = false;
                        this.textBox_login.Clear();
                        this.textBox_clave.Clear();
                        this.textBox_login.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Login de usuario NO existe...", "Clave de Acceso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Lpass_usu = false;
                    this.textBox_login.Clear();
                    this.textBox_clave.Clear();
                    this.textBox_login.Focus();
                }
            }
            else
            {
                MessageBox.Show("Login de usuario NO puede estar vacio...", "Clave de Acceso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Lpass_usu = false;
                this.textBox_login.Clear();
                this.textBox_clave.Clear();
                this.textBox_login.Focus();
            }
        }

        private void check_connection()
        {
            if (Fundraising_PTDM.MyConnection.OpenConnection() == true)
            {
                bool validconnect = Fundraising_PTDM.MyConnection.IsConnected();
                if (!validconnect)
                {
                    if (MessageBox.Show("No se pudo establecer la conexión con el servidor de la base de datos... \nDesea configurar la conexión... ?", "Conexión con servidor de datos", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                    {
                        config_connection();
                    }
                    else
                    {
                        Lpass_usu = false;
                        Close();
                    }
                }
                else
                {
                    // se establece los parametros para "xpo.default.sesion" que es la sesion usada para todas las conexiones a las tablas entre (Collection, Object Persist, tablas sql).
                    XpoDefault.Session.LockingOption = LockingOption.Optimistic;
                    XpoDefault.Session.OptimisticLockingReadBehavior = OptimisticLockingReadBehavior.MergeCollisionThrowException;
                    //XpoDefault.Session.OptimisticLockingReadBehavior = OptimisticLockingReadBehavior.ThrowException;
                    XpoDefault.Session.TrackPropertiesModifications = true;

                    //XPBaseCollection.EnableObjectChangedNotificationsWhileEditing = true;

                    //XPLiteObject.AutoSaveOnEndEdit = false;
                    XPBaseObject.AutoSaveOnEndEdit = true;
                    //

                    // se crea un usuario administrativo por primera ves //
                    usuario = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Usuarios>(DevExpress.Xpo.XpoDefault.Session);
                    if (usuario == null || usuario.Count() <= 0)
                    {
                        usuario.Add(new Fundraising_PTDM.FUNDRAISING_PT.Usuarios(DevExpress.Xpo.XpoDefault.Session));
                        usuario[0].login = "1";
                        usuario[0].clave = "1";
                        usuario[0].usuario = "POS&TOUCH Administrator";
                        usuario[0].tipo = 1;
                        usuario[0].status = 1;
                        usuario[0].Save();
                    }
                    //
                    // se crea una sucursal unica por primera ves //
                    sucursal = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session);
                    if (sucursal == null || sucursal.Count() <= 0)
                    {
                        l_new_sucursal = true;
                        sucursal.Add(new Fundraising_PTDM.FUNDRAISING_PT.Sucursales(DevExpress.Xpo.XpoDefault.Session));
                        sucursal[0].codigo = 9999999;
                        sucursal[0].nombre = "Fundraising Sucursal Unica";
                        sucursal[0].logotipo = "";
                        sucursal[0].select = true;
                        sucursal[0].status = 1;
                        sucursal[0].Save();
                        //
                        Fundraising_PT.Properties.Settings.Default.sucursal_oid = sucursal[0].oid;
                        Fundraising_PT.Properties.Settings.Default.sucursal = 9999999;
                        Fundraising_PT.Properties.Settings.Default.nombre_sucursal = "Fundraising Sucursal Unica";
                        Fundraising_PT.Properties.Settings.Default.logotipo = "";
                        Fundraising_PT.Properties.Settings.Default.sucursal_filter = string.Format("sucursal = {0}", 9999999);
                        Fundraising_PT.Properties.Settings.Default.Save();
                        //
                    }
                    //
                    // se crea un registro por primera ves, con la informacion basica de configuracion // 
                    configuracion = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Configuracion>(DevExpress.Xpo.XpoDefault.Session);
                    if (configuracion == null || configuracion.Count() <= 0)
                    {
                        //sucursal = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session, (new OperandProperty("status") == new OperandValue(1) & new OperandProperty("select") == new OperandValue(true)), (new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending)));
                        configuracion.Add(new Fundraising_PTDM.FUNDRAISING_PT.Configuracion(DevExpress.Xpo.XpoDefault.Session));
                        configuracion[0].activa_audio = 0;
                        configuracion[0].time_new_sesion = 0;
                        //configuracion[0].sucursal = (sucursal == null || sucursal.Count() <= 0 ? null : sucursal[0]);
                        configuracion[0].Save();
                        //
                    }
                    else
                    {
                        //if (l_new_sucursal)
                        //{
                        //    sucursal = new DevExpress.Xpo.XPCollection<Fundraising_PTDM.FUNDRAISING_PT.Sucursales>(DevExpress.Xpo.XpoDefault.Session, (new OperandProperty("status") == new OperandValue(1) & new OperandProperty("select") == new OperandValue(true)), (new DevExpress.Xpo.SortProperty("codigo", DevExpress.Xpo.DB.SortingDirection.Ascending)));
                        //    configuracion[0].sucursal = (sucursal == null || sucursal.Count() <= 0 ? null : sucursal[0]);
                        //    configuracion[0].Save();
                        //}
                    }
                    //
                    this.textBox_login.Focus();
                }
            }
            else
            {
                if (MessageBox.Show("No se pudo establecer la conexión con el servidor de la base de datos... \nDesea configurar la conexión... ?", "Conexión con servidor de datos", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                {
                    config_connection();
                }
                else
                {
                    Lpass_usu = false;
                    Close();
                }
            }
        }
        
        private void config_connection()
        {
            DataConnectionDialog dcd = new DataConnectionDialog();
            DataSource.AddStandardDataSources(dcd);
            //
            dcd.SelectedDataSource = DataSource.SqlDataSource;
            dcd.SelectedDataProvider = DataProvider.SqlDataProvider;
            //
            dcd.ConnectionString = Fundraising_PTDM.MyConnection.GetConnectionString();
            //
            if (DataConnectionDialog.Show(dcd) == System.Windows.Forms.DialogResult.OK)
            {
                SqlConnectionStringBuilder sqlcsb = new SqlConnectionStringBuilder(dcd.ConnectionString);
                //
                Fundraising_PTDM.MyConnection.SetConnectionString(dcd.ConnectionString, sqlcsb.DataSource, sqlcsb.InitialCatalog, sqlcsb.UserID, sqlcsb.Password);
                check_connection();
            }
            else
            {
                Lpass_usu = false;
                Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            usuario.Dispose();
            sucursal.Dispose();
            configuracion.Dispose();
            //
            base.OnClosed(e);
        }
    }
}
