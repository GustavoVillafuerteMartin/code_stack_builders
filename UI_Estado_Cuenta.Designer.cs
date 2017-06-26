namespace Fundraising_PT.Formularios
{
    partial class UI_Estado_Cuenta
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraEditors.DXErrorProvider.CompareAgainstControlValidationRule compareAgainstControlValidationRule1 = new DevExpress.XtraEditors.DXErrorProvider.CompareAgainstControlValidationRule();
            DevExpress.XtraEditors.DXErrorProvider.CompareAgainstControlValidationRule compareAgainstControlValidationRule2 = new DevExpress.XtraEditors.DXErrorProvider.CompareAgainstControlValidationRule();
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule1 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule2 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule3 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI_Estado_Cuenta));
            this.dateTime_fecha_hasta = new DevExpress.XtraEditors.DateEdit();
            this.dateTime_fecha_desde = new DevExpress.XtraEditors.DateEdit();
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            this.lookUpEdit_recaudador = new DevExpress.XtraEditors.LookUpEdit();
            this.bindingSource_usuarios = new System.Windows.Forms.BindingSource(this.components);
            this.lookUpEdit_formapago = new DevExpress.XtraEditors.LookUpEdit();
            this.bindingSource_formas_pagos = new System.Windows.Forms.BindingSource(this.components);
            this.lookUpEdit_tipoformapago = new DevExpress.XtraEditors.LookUpEdit();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.label_rangofechas = new Fundraising_PT.Controles.Label_Base2();
            this.label_desde = new Fundraising_PT.Controles.Label_Base2();
            this.label_hasta = new Fundraising_PT.Controles.Label_Base2();
            this.label_recaudador = new Fundraising_PT.Controles.Label_Base2();
            this.label_formapago = new Fundraising_PT.Controles.Label_Base2();
            this.checkEdit_todos_recaudador = new DevExpress.XtraEditors.CheckEdit();
            this.label_tipo = new Fundraising_PT.Controles.Label_Base2();
            this.label_descripcion = new Fundraising_PT.Controles.Label_Base2();
            this.checkEdit_todos_tipo = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit_todosdescripcion = new DevExpress.XtraEditors.CheckEdit();
            this.panelControl_botones = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.simpleButton_imprimir = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton_salir = new DevExpress.XtraEditors.SimpleButton();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTime_fecha_hasta.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTime_fecha_hasta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTime_fecha_desde.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTime_fecha_desde.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_recaudador.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_usuarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_formapago.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_formas_pagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_tipoformapago.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_todos_recaudador.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_todos_tipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_todosdescripcion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_botones)).BeginInit();
            this.panelControl_botones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // dateTime_fecha_hasta
            // 
            this.dateTime_fecha_hasta.EditValue = new System.DateTime(2015, 1, 22, 10, 42, 16, 146);
            this.dateTime_fecha_hasta.Location = new System.Drawing.Point(245, 38);
            this.dateTime_fecha_hasta.Name = "dateTime_fecha_hasta";
            this.dateTime_fecha_hasta.Properties.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTime_fecha_hasta.Properties.Appearance.Options.UseFont = true;
            this.dateTime_fecha_hasta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateTime_fecha_hasta.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateTime_fecha_hasta.Properties.Mask.BeepOnError = true;
            this.dateTime_fecha_hasta.Size = new System.Drawing.Size(100, 20);
            this.dateTime_fecha_hasta.TabIndex = 1;
            this.dateTime_fecha_hasta.ToolTip = "Rango de Fecha Hasta.";
            this.dateTime_fecha_hasta.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.dateTime_fecha_hasta.ToolTipTitle = "Fecha Hasta";
            compareAgainstControlValidationRule1.CaseSensitive = true;
            compareAgainstControlValidationRule1.CompareControlOperator = DevExpress.XtraEditors.DXErrorProvider.CompareControlOperator.Greater;
            compareAgainstControlValidationRule1.Control = this.dateTime_fecha_desde;
            compareAgainstControlValidationRule1.ErrorText = "Fecha Hasta NO puede ser menor a Fecha Desde.";
            compareAgainstControlValidationRule1.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            this.dxValidationProvider1.SetValidationRule(this.dateTime_fecha_hasta, compareAgainstControlValidationRule1);
            // 
            // dateTime_fecha_desde
            // 
            this.dateTime_fecha_desde.EditValue = new System.DateTime(2015, 1, 22, 10, 38, 27, 884);
            this.dateTime_fecha_desde.Location = new System.Drawing.Point(131, 38);
            this.dateTime_fecha_desde.Name = "dateTime_fecha_desde";
            this.dateTime_fecha_desde.Properties.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTime_fecha_desde.Properties.Appearance.Options.UseFont = true;
            this.dateTime_fecha_desde.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateTime_fecha_desde.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateTime_fecha_desde.Properties.Mask.BeepOnError = true;
            this.dateTime_fecha_desde.Size = new System.Drawing.Size(100, 20);
            this.dateTime_fecha_desde.TabIndex = 0;
            this.dateTime_fecha_desde.ToolTip = "Rango de Fecha Desde.";
            this.dateTime_fecha_desde.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.dateTime_fecha_desde.ToolTipTitle = "Fecha Desde";
            compareAgainstControlValidationRule2.CaseSensitive = true;
            compareAgainstControlValidationRule2.CompareControlOperator = DevExpress.XtraEditors.DXErrorProvider.CompareControlOperator.Less;
            compareAgainstControlValidationRule2.Control = this.dateTime_fecha_hasta;
            compareAgainstControlValidationRule2.ErrorText = "Fecha Desde NO puede ser mayor a Fecha Hasta.";
            compareAgainstControlValidationRule2.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            this.dxValidationProvider1.SetValidationRule(this.dateTime_fecha_desde, compareAgainstControlValidationRule2);
            // 
            // dxValidationProvider1
            // 
            this.dxValidationProvider1.ValidationMode = DevExpress.XtraEditors.DXErrorProvider.ValidationMode.Auto;
            // 
            // lookUpEdit_recaudador
            // 
            this.lookUpEdit_recaudador.Enabled = false;
            this.lookUpEdit_recaudador.Location = new System.Drawing.Point(131, 72);
            this.lookUpEdit_recaudador.Name = "lookUpEdit_recaudador";
            this.lookUpEdit_recaudador.Properties.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_recaudador.Properties.Appearance.Options.UseFont = true;
            this.lookUpEdit_recaudador.Properties.AppearanceDisabled.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_recaudador.Properties.AppearanceDisabled.Options.UseFont = true;
            this.lookUpEdit_recaudador.Properties.AppearanceFocused.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_recaudador.Properties.AppearanceFocused.Options.UseFont = true;
            this.lookUpEdit_recaudador.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_recaudador.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.lookUpEdit_recaudador.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.lookUpEdit_recaudador.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit_recaudador.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("usuario", 60, "Recaudador")});
            this.lookUpEdit_recaudador.Properties.DataSource = this.bindingSource_usuarios;
            this.lookUpEdit_recaudador.Properties.DisplayMember = "usuario";
            this.lookUpEdit_recaudador.Properties.LookAndFeel.SkinName = "Office 2013";
            this.lookUpEdit_recaudador.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.lookUpEdit_recaudador.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.lookUpEdit_recaudador.Properties.LookAndFeel.UseWindowsXPTheme = true;
            this.lookUpEdit_recaudador.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lookUpEdit_recaudador.Properties.ValueMember = "oid";
            this.lookUpEdit_recaudador.Size = new System.Drawing.Size(300, 20);
            this.lookUpEdit_recaudador.TabIndex = 2;
            this.lookUpEdit_recaudador.ToolTip = "Selecciona el Recaudador.";
            this.lookUpEdit_recaudador.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.lookUpEdit_recaudador.ToolTipTitle = "Recaudador.";
            conditionValidationRule1.CaseSensitive = true;
            conditionValidationRule1.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule1.ErrorText = "Recaudador NO Puede estar Vacio o Nulo.";
            conditionValidationRule1.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            this.dxValidationProvider1.SetValidationRule(this.lookUpEdit_recaudador, conditionValidationRule1);
            // 
            // lookUpEdit_formapago
            // 
            this.lookUpEdit_formapago.Enabled = false;
            this.lookUpEdit_formapago.Location = new System.Drawing.Point(330, 130);
            this.lookUpEdit_formapago.Name = "lookUpEdit_formapago";
            this.lookUpEdit_formapago.Properties.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_formapago.Properties.Appearance.Options.UseFont = true;
            this.lookUpEdit_formapago.Properties.AppearanceDisabled.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_formapago.Properties.AppearanceDisabled.Options.UseFont = true;
            this.lookUpEdit_formapago.Properties.AppearanceFocused.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_formapago.Properties.AppearanceFocused.Options.UseFont = true;
            this.lookUpEdit_formapago.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_formapago.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.lookUpEdit_formapago.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.lookUpEdit_formapago.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit_formapago.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nombre", 40, "Forma de Pago")});
            this.lookUpEdit_formapago.Properties.DataSource = this.bindingSource_formas_pagos;
            this.lookUpEdit_formapago.Properties.DisplayMember = "nombre";
            this.lookUpEdit_formapago.Properties.LookAndFeel.SkinName = "Office 2013";
            this.lookUpEdit_formapago.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.lookUpEdit_formapago.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.lookUpEdit_formapago.Properties.LookAndFeel.UseWindowsXPTheme = true;
            this.lookUpEdit_formapago.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lookUpEdit_formapago.Properties.ValueMember = "oid";
            this.lookUpEdit_formapago.Size = new System.Drawing.Size(250, 20);
            this.lookUpEdit_formapago.TabIndex = 7;
            this.lookUpEdit_formapago.ToolTip = "Selecciona la Forma de Pago.";
            this.lookUpEdit_formapago.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.lookUpEdit_formapago.ToolTipTitle = "Descripción.";
            conditionValidationRule2.CaseSensitive = true;
            conditionValidationRule2.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule2.ErrorText = "Descripción de Forma de Pago Invalida.";
            conditionValidationRule2.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            this.dxValidationProvider1.SetValidationRule(this.lookUpEdit_formapago, conditionValidationRule2);
            // 
            // lookUpEdit_tipoformapago
            // 
            this.lookUpEdit_tipoformapago.Enabled = false;
            this.lookUpEdit_tipoformapago.Location = new System.Drawing.Point(131, 130);
            this.lookUpEdit_tipoformapago.Name = "lookUpEdit_tipoformapago";
            this.lookUpEdit_tipoformapago.Properties.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_tipoformapago.Properties.Appearance.Options.UseFont = true;
            this.lookUpEdit_tipoformapago.Properties.AppearanceDisabled.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_tipoformapago.Properties.AppearanceDisabled.Options.UseFont = true;
            this.lookUpEdit_tipoformapago.Properties.AppearanceFocused.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_tipoformapago.Properties.AppearanceFocused.Options.UseFont = true;
            this.lookUpEdit_tipoformapago.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_tipoformapago.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.lookUpEdit_tipoformapago.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.lookUpEdit_tipoformapago.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit_tipoformapago.Properties.LookAndFeel.SkinName = "Office 2013";
            this.lookUpEdit_tipoformapago.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.lookUpEdit_tipoformapago.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.lookUpEdit_tipoformapago.Properties.LookAndFeel.UseWindowsXPTheme = true;
            this.lookUpEdit_tipoformapago.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lookUpEdit_tipoformapago.Size = new System.Drawing.Size(186, 20);
            this.lookUpEdit_tipoformapago.TabIndex = 5;
            this.lookUpEdit_tipoformapago.ToolTip = "Selecciona el Tipo de la Forma de Pago.";
            this.lookUpEdit_tipoformapago.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.lookUpEdit_tipoformapago.ToolTipTitle = "Tipo.";
            conditionValidationRule3.CaseSensitive = true;
            conditionValidationRule3.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule3.ErrorText = "Tipo de Forma de Pago Invalido.";
            conditionValidationRule3.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            this.dxValidationProvider1.SetValidationRule(this.lookUpEdit_tipoformapago, conditionValidationRule3);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // label_rangofechas
            // 
            this.label_rangofechas.AccessibleName = "Label_Base2";
            this.label_rangofechas.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.label_rangofechas.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_rangofechas.Appearance.Options.UseBackColor = true;
            this.label_rangofechas.Appearance.Options.UseBorderColor = true;
            this.label_rangofechas.Appearance.Options.UseFont = true;
            this.label_rangofechas.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.label_rangofechas.ldefault_size = new System.Drawing.Size(96, 20);
            this.label_rangofechas.lfont = new System.Drawing.Font("Arial", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_rangofechas.lforecolor = System.Drawing.SystemColors.ControlText;
            this.label_rangofechas.lhalignment = DevExpress.Utils.HorzAlignment.Far;
            this.label_rangofechas.Location = new System.Drawing.Point(33, 38);
            this.label_rangofechas.lText = "Rango de Fechas :";
            this.label_rangofechas.Name = "label_rangofechas";
            this.label_rangofechas.Size = new System.Drawing.Size(96, 20);
            this.label_rangofechas.TabIndex = 7;
            // 
            // label_desde
            // 
            this.label_desde.AccessibleName = "Label_Base2";
            this.label_desde.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.label_desde.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_desde.Appearance.Options.UseBackColor = true;
            this.label_desde.Appearance.Options.UseBorderColor = true;
            this.label_desde.Appearance.Options.UseFont = true;
            this.label_desde.Appearance.Options.UseTextOptions = true;
            this.label_desde.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.label_desde.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.label_desde.ldefault_size = new System.Drawing.Size(100, 20);
            this.label_desde.lfont = new System.Drawing.Font("Arial", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_desde.lforecolor = System.Drawing.SystemColors.ControlText;
            this.label_desde.lhalignment = DevExpress.Utils.HorzAlignment.Center;
            this.label_desde.Location = new System.Drawing.Point(131, 17);
            this.label_desde.lText = "Desde";
            this.label_desde.Name = "label_desde";
            this.label_desde.Size = new System.Drawing.Size(100, 20);
            this.label_desde.TabIndex = 10;
            // 
            // label_hasta
            // 
            this.label_hasta.AccessibleName = "Label_Base2";
            this.label_hasta.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.label_hasta.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_hasta.Appearance.Options.UseBackColor = true;
            this.label_hasta.Appearance.Options.UseBorderColor = true;
            this.label_hasta.Appearance.Options.UseFont = true;
            this.label_hasta.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.label_hasta.ldefault_size = new System.Drawing.Size(100, 20);
            this.label_hasta.lfont = new System.Drawing.Font("Arial", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_hasta.lforecolor = System.Drawing.SystemColors.ControlText;
            this.label_hasta.lhalignment = DevExpress.Utils.HorzAlignment.Center;
            this.label_hasta.Location = new System.Drawing.Point(245, 17);
            this.label_hasta.lText = "Hasta";
            this.label_hasta.Name = "label_hasta";
            this.label_hasta.Size = new System.Drawing.Size(100, 20);
            this.label_hasta.TabIndex = 11;
            // 
            // label_recaudador
            // 
            this.label_recaudador.AccessibleName = "Label_Base2";
            this.label_recaudador.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.label_recaudador.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_recaudador.Appearance.Options.UseBackColor = true;
            this.label_recaudador.Appearance.Options.UseBorderColor = true;
            this.label_recaudador.Appearance.Options.UseFont = true;
            this.label_recaudador.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.label_recaudador.ldefault_size = new System.Drawing.Size(96, 20);
            this.label_recaudador.lfont = new System.Drawing.Font("Arial", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_recaudador.lforecolor = System.Drawing.SystemColors.ControlText;
            this.label_recaudador.lhalignment = DevExpress.Utils.HorzAlignment.Far;
            this.label_recaudador.Location = new System.Drawing.Point(33, 72);
            this.label_recaudador.lText = "Recaudador :";
            this.label_recaudador.Name = "label_recaudador";
            this.label_recaudador.Size = new System.Drawing.Size(96, 20);
            this.label_recaudador.TabIndex = 12;
            // 
            // label_formapago
            // 
            this.label_formapago.AccessibleName = "Label_Base2";
            this.label_formapago.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.label_formapago.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_formapago.Appearance.Options.UseBackColor = true;
            this.label_formapago.Appearance.Options.UseBorderColor = true;
            this.label_formapago.Appearance.Options.UseFont = true;
            this.label_formapago.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.label_formapago.ldefault_size = new System.Drawing.Size(96, 20);
            this.label_formapago.lfont = new System.Drawing.Font("Arial", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_formapago.lforecolor = System.Drawing.SystemColors.ControlText;
            this.label_formapago.lhalignment = DevExpress.Utils.HorzAlignment.Far;
            this.label_formapago.Location = new System.Drawing.Point(33, 130);
            this.label_formapago.lText = "Forma de Pago :";
            this.label_formapago.Name = "label_formapago";
            this.label_formapago.Size = new System.Drawing.Size(96, 20);
            this.label_formapago.TabIndex = 13;
            // 
            // checkEdit_todos_recaudador
            // 
            this.checkEdit_todos_recaudador.EditValue = 1;
            this.checkEdit_todos_recaudador.Location = new System.Drawing.Point(435, 70);
            this.checkEdit_todos_recaudador.Name = "checkEdit_todos_recaudador";
            this.checkEdit_todos_recaudador.Properties.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkEdit_todos_recaudador.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.checkEdit_todos_recaudador.Properties.Appearance.Options.UseFont = true;
            this.checkEdit_todos_recaudador.Properties.Appearance.Options.UseForeColor = true;
            this.checkEdit_todos_recaudador.Properties.AppearanceFocused.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkEdit_todos_recaudador.Properties.AppearanceFocused.Options.UseFont = true;
            this.checkEdit_todos_recaudador.Properties.Caption = "Todos";
            this.checkEdit_todos_recaudador.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.checkEdit_todos_recaudador.Properties.ValueChecked = 1;
            this.checkEdit_todos_recaudador.Properties.ValueUnchecked = 0;
            this.checkEdit_todos_recaudador.Size = new System.Drawing.Size(76, 22);
            this.checkEdit_todos_recaudador.TabIndex = 3;
            this.checkEdit_todos_recaudador.ToolTip = "Selecciona a todos los recaudadores.";
            this.checkEdit_todos_recaudador.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.checkEdit_todos_recaudador.ToolTipTitle = "Selecciona a todos los recaudadores.";
            // 
            // label_tipo
            // 
            this.label_tipo.AccessibleName = "Label_Base2";
            this.label_tipo.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.label_tipo.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_tipo.Appearance.Options.UseBackColor = true;
            this.label_tipo.Appearance.Options.UseBorderColor = true;
            this.label_tipo.Appearance.Options.UseFont = true;
            this.label_tipo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.label_tipo.ldefault_size = new System.Drawing.Size(130, 20);
            this.label_tipo.lfont = new System.Drawing.Font("Arial", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_tipo.lforecolor = System.Drawing.SystemColors.ControlText;
            this.label_tipo.lhalignment = DevExpress.Utils.HorzAlignment.Center;
            this.label_tipo.Location = new System.Drawing.Point(131, 109);
            this.label_tipo.lText = "Tipo";
            this.label_tipo.Name = "label_tipo";
            this.label_tipo.Size = new System.Drawing.Size(130, 20);
            this.label_tipo.TabIndex = 35;
            // 
            // label_descripcion
            // 
            this.label_descripcion.AccessibleName = "Label_Base2";
            this.label_descripcion.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.label_descripcion.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_descripcion.Appearance.Options.UseBackColor = true;
            this.label_descripcion.Appearance.Options.UseBorderColor = true;
            this.label_descripcion.Appearance.Options.UseFont = true;
            this.label_descripcion.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.label_descripcion.ldefault_size = new System.Drawing.Size(187, 20);
            this.label_descripcion.lfont = new System.Drawing.Font("Arial", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_descripcion.lforecolor = System.Drawing.SystemColors.ControlText;
            this.label_descripcion.lhalignment = DevExpress.Utils.HorzAlignment.Center;
            this.label_descripcion.Location = new System.Drawing.Point(330, 109);
            this.label_descripcion.lText = "Descripción";
            this.label_descripcion.Name = "label_descripcion";
            this.label_descripcion.Size = new System.Drawing.Size(187, 20);
            this.label_descripcion.TabIndex = 36;
            // 
            // checkEdit_todos_tipo
            // 
            this.checkEdit_todos_tipo.AutoSizeInLayoutControl = true;
            this.checkEdit_todos_tipo.EditValue = 1;
            this.checkEdit_todos_tipo.Location = new System.Drawing.Point(263, 107);
            this.checkEdit_todos_tipo.Name = "checkEdit_todos_tipo";
            this.checkEdit_todos_tipo.Properties.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkEdit_todos_tipo.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.checkEdit_todos_tipo.Properties.Appearance.Options.UseFont = true;
            this.checkEdit_todos_tipo.Properties.Appearance.Options.UseForeColor = true;
            this.checkEdit_todos_tipo.Properties.AppearanceFocused.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkEdit_todos_tipo.Properties.AppearanceFocused.Options.UseFont = true;
            this.checkEdit_todos_tipo.Properties.AutoWidth = true;
            this.checkEdit_todos_tipo.Properties.Caption = "Todos";
            this.checkEdit_todos_tipo.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.checkEdit_todos_tipo.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.checkEdit_todos_tipo.Properties.ValueChecked = 1;
            this.checkEdit_todos_tipo.Properties.ValueUnchecked = 0;
            this.checkEdit_todos_tipo.Size = new System.Drawing.Size(54, 22);
            this.checkEdit_todos_tipo.TabIndex = 4;
            this.checkEdit_todos_tipo.ToolTip = "Selecciona a todos los tipos de formas de pago.";
            this.checkEdit_todos_tipo.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.checkEdit_todos_tipo.ToolTipTitle = "Selecciona a todos los tipos de formas de pago.";
            // 
            // checkEdit_todosdescripcion
            // 
            this.checkEdit_todosdescripcion.AutoSizeInLayoutControl = true;
            this.checkEdit_todosdescripcion.EditValue = 1;
            this.checkEdit_todosdescripcion.Location = new System.Drawing.Point(519, 107);
            this.checkEdit_todosdescripcion.Name = "checkEdit_todosdescripcion";
            this.checkEdit_todosdescripcion.Properties.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkEdit_todosdescripcion.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.checkEdit_todosdescripcion.Properties.Appearance.Options.UseFont = true;
            this.checkEdit_todosdescripcion.Properties.Appearance.Options.UseForeColor = true;
            this.checkEdit_todosdescripcion.Properties.AppearanceFocused.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkEdit_todosdescripcion.Properties.AppearanceFocused.Options.UseFont = true;
            this.checkEdit_todosdescripcion.Properties.AutoWidth = true;
            this.checkEdit_todosdescripcion.Properties.Caption = "Todos";
            this.checkEdit_todosdescripcion.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.checkEdit_todosdescripcion.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.checkEdit_todosdescripcion.Properties.ValueChecked = 1;
            this.checkEdit_todosdescripcion.Properties.ValueUnchecked = 0;
            this.checkEdit_todosdescripcion.Size = new System.Drawing.Size(54, 22);
            this.checkEdit_todosdescripcion.TabIndex = 6;
            this.checkEdit_todosdescripcion.ToolTip = "Selecciona a todas las formas de pago.";
            this.checkEdit_todosdescripcion.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.checkEdit_todosdescripcion.ToolTipTitle = "Selecciona a todas las formas de pago.";
            // 
            // panelControl_botones
            // 
            this.panelControl_botones.Appearance.BackColor = System.Drawing.Color.Black;
            this.panelControl_botones.Appearance.BackColor2 = System.Drawing.Color.Gray;
            this.panelControl_botones.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelControl_botones.Appearance.Options.UseBackColor = true;
            this.panelControl_botones.Appearance.Options.UseFont = true;
            this.panelControl_botones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControl_botones.Controls.Add(this.splitContainerControl1);
            this.panelControl_botones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl_botones.Location = new System.Drawing.Point(0, 170);
            this.panelControl_botones.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl_botones.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl_botones.Name = "panelControl_botones";
            this.panelControl_botones.Size = new System.Drawing.Size(601, 65);
            this.panelControl_botones.TabIndex = 39;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitContainerControl1.Location = new System.Drawing.Point(456, 3);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.simpleButton_imprimir);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.simpleButton_salir);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(142, 59);
            this.splitContainerControl1.SplitterPosition = 70;
            this.splitContainerControl1.TabIndex = 10;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // simpleButton_imprimir
            // 
            this.simpleButton_imprimir.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.simpleButton_imprimir.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton_imprimir.Appearance.ForeColor = System.Drawing.Color.LightCyan;
            this.simpleButton_imprimir.Appearance.Options.UseBackColor = true;
            this.simpleButton_imprimir.Appearance.Options.UseFont = true;
            this.simpleButton_imprimir.Appearance.Options.UseForeColor = true;
            this.simpleButton_imprimir.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButton_imprimir.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton_imprimir.Image")));
            this.simpleButton_imprimir.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.simpleButton_imprimir.Location = new System.Drawing.Point(1, 2);
            this.simpleButton_imprimir.Name = "simpleButton_imprimir";
            this.simpleButton_imprimir.Size = new System.Drawing.Size(70, 55);
            this.simpleButton_imprimir.TabIndex = 0;
            this.simpleButton_imprimir.Text = "Imprimir";
            this.simpleButton_imprimir.ToolTip = "Imprimir el Reporte del Estado de Cuenta.";
            this.simpleButton_imprimir.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.simpleButton_imprimir.ToolTipTitle = "Imprimir";
            this.simpleButton_imprimir.Click += new System.EventHandler(this.simpleButton_imprimir_Click);
            // 
            // simpleButton_salir
            // 
            this.simpleButton_salir.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.simpleButton_salir.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton_salir.Appearance.ForeColor = System.Drawing.Color.LightCyan;
            this.simpleButton_salir.Appearance.Options.UseBackColor = true;
            this.simpleButton_salir.Appearance.Options.UseFont = true;
            this.simpleButton_salir.Appearance.Options.UseForeColor = true;
            this.simpleButton_salir.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButton_salir.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton_salir.Image = global::Fundraising_PT.Properties.Resources.Salir_Xp;
            this.simpleButton_salir.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.simpleButton_salir.Location = new System.Drawing.Point(0, 2);
            this.simpleButton_salir.Name = "simpleButton_salir";
            this.simpleButton_salir.Size = new System.Drawing.Size(66, 55);
            this.simpleButton_salir.TabIndex = 0;
            this.simpleButton_salir.Text = "Salir";
            this.simpleButton_salir.ToolTip = "Salir sin Imprimir.";
            this.simpleButton_salir.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.simpleButton_salir.ToolTipTitle = "Salir";
            this.simpleButton_salir.Click += new System.EventHandler(this.simpleButton_salir_Click);
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(541, 2);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.AllowFocused = false;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit1.Size = new System.Drawing.Size(57, 58);
            this.pictureEdit1.TabIndex = 40;
            // 
            // UI_Estado_Cuenta
            // 
            this.Appearance.BackColor = System.Drawing.Color.DimGray;
            this.Appearance.BackColor2 = System.Drawing.Color.LightGray;
            this.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.Appearance.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseBorderColor = true;
            this.Appearance.Options.UseFont = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(601, 235);
            this.Controls.Add(this.dateTime_fecha_hasta);
            this.Controls.Add(this.dateTime_fecha_desde);
            this.Controls.Add(this.lookUpEdit_tipoformapago);
            this.Controls.Add(this.lookUpEdit_formapago);
            this.Controls.Add(this.lookUpEdit_recaudador);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.panelControl_botones);
            this.Controls.Add(this.checkEdit_todosdescripcion);
            this.Controls.Add(this.checkEdit_todos_tipo);
            this.Controls.Add(this.label_descripcion);
            this.Controls.Add(this.label_tipo);
            this.Controls.Add(this.checkEdit_todos_recaudador);
            this.Controls.Add(this.label_formapago);
            this.Controls.Add(this.label_recaudador);
            this.Controls.Add(this.label_hasta);
            this.Controls.Add(this.label_desde);
            this.Controls.Add(this.label_rangofechas);
            this.DoubleBuffered = true;
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "UI_Estado_Cuenta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Estado de Cuenta (Recaudado-Depósitado)";
            this.Load += new System.EventHandler(this.UI_Estado_Cuenta_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dateTime_fecha_hasta.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTime_fecha_hasta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTime_fecha_desde.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTime_fecha_desde.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_recaudador.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_usuarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_formapago.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_formas_pagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_tipoformapago.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_todos_recaudador.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_todos_tipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_todosdescripcion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_botones)).EndInit();
            this.panelControl_botones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
        private Controles.Label_Base2 label_rangofechas;
        public System.Windows.Forms.BindingSource bindingSource_formas_pagos;
        private Controles.Label_Base2 label_formapago;
        private Controles.Label_Base2 label_recaudador;
        private Controles.Label_Base2 label_hasta;
        private Controles.Label_Base2 label_desde;
        private DevExpress.XtraEditors.CheckEdit checkEdit_todosdescripcion;
        private DevExpress.XtraEditors.CheckEdit checkEdit_todos_tipo;
        private Controles.Label_Base2 label_descripcion;
        private Controles.Label_Base2 label_tipo;
        private DevExpress.XtraEditors.CheckEdit checkEdit_todos_recaudador;
        private DevExpress.XtraEditors.PanelControl panelControl_botones;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton_imprimir;
        private DevExpress.XtraEditors.SimpleButton simpleButton_salir;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        public System.Windows.Forms.BindingSource bindingSource_usuarios;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit_recaudador;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit_formapago;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit_tipoformapago;
        private DevExpress.XtraEditors.DateEdit dateTime_fecha_desde;
        private DevExpress.XtraEditors.DateEdit dateTime_fecha_hasta;
    }
}