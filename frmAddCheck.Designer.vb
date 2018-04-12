<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddCheck
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim ColumnDefinition1 As DevExpress.XtraLayout.ColumnDefinition = New DevExpress.XtraLayout.ColumnDefinition()
        Dim ColumnDefinition2 As DevExpress.XtraLayout.ColumnDefinition = New DevExpress.XtraLayout.ColumnDefinition()
        Dim ColumnDefinition3 As DevExpress.XtraLayout.ColumnDefinition = New DevExpress.XtraLayout.ColumnDefinition()
        Dim ColumnDefinition4 As DevExpress.XtraLayout.ColumnDefinition = New DevExpress.XtraLayout.ColumnDefinition()
        Dim RowDefinition1 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition2 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition3 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition4 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.txtEGP = New DevExpress.XtraEditors.TextEdit()
        Me.deDate = New DevExpress.XtraEditors.DateEdit()
        Me.txtUSD = New DevExpress.XtraEditors.TextEdit()
        Me.txtDetails = New DevExpress.XtraEditors.MemoEdit()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.btnOK = New DevExpress.XtraEditors.SimpleButton()
        Me.btnCancel = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.txtEGP.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.deDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.deDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtUSD.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDetails.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.txtEGP)
        Me.LayoutControl1.Controls.Add(Me.deDate)
        Me.LayoutControl1.Controls.Add(Me.txtUSD)
        Me.LayoutControl1.Controls.Add(Me.txtDetails)
        Me.LayoutControl1.Location = New System.Drawing.Point(12, 12)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(651, 185)
        Me.LayoutControl1.TabIndex = 7
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'txtEGP
        '
        Me.txtEGP.EnterMoveNextControl = True
        Me.txtEGP.Location = New System.Drawing.Point(486, 109)
        Me.txtEGP.MinimumSize = New System.Drawing.Size(0, 30)
        Me.txtEGP.Name = "txtEGP"
        Me.txtEGP.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.txtEGP.Properties.Appearance.Options.UseFont = True
        Me.txtEGP.Properties.Mask.EditMask = "\d+(\R.\d{0,2})?"
        Me.txtEGP.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx
        Me.txtEGP.Size = New System.Drawing.Size(103, 30)
        Me.txtEGP.StyleController = Me.LayoutControl1
        Me.txtEGP.TabIndex = 2
        '
        'deDate
        '
        Me.deDate.EditValue = Nothing
        Me.deDate.EnterMoveNextControl = True
        Me.deDate.Location = New System.Drawing.Point(328, 12)
        Me.deDate.Name = "deDate"
        Me.deDate.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.deDate.Properties.Appearance.Options.UseFont = True
        Me.deDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.deDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.deDate.Properties.CalendarTimeProperties.DisplayFormat.FormatString = "dd/MM/yyyy"
        Me.deDate.Properties.CalendarTimeProperties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom
        Me.deDate.Properties.CalendarTimeProperties.EditFormat.FormatString = "dd/MM/yyyy"
        Me.deDate.Properties.CalendarTimeProperties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom
        Me.deDate.Properties.DisplayFormat.FormatString = ""
        Me.deDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.deDate.Properties.EditFormat.FormatString = ""
        Me.deDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.deDate.Properties.Mask.EditMask = "dd/MM/yyyy"
        Me.deDate.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.deDate.Size = New System.Drawing.Size(261, 26)
        Me.deDate.StyleController = Me.LayoutControl1
        Me.deDate.TabIndex = 0
        '
        'txtUSD
        '
        Me.txtUSD.EnterMoveNextControl = True
        Me.txtUSD.Location = New System.Drawing.Point(486, 143)
        Me.txtUSD.MinimumSize = New System.Drawing.Size(0, 30)
        Me.txtUSD.Name = "txtUSD"
        Me.txtUSD.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.txtUSD.Properties.Appearance.Options.UseFont = True
        Me.txtUSD.Properties.Mask.EditMask = "\d+(\R.\d{0,2})?"
        Me.txtUSD.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx
        Me.txtUSD.Size = New System.Drawing.Size(103, 30)
        Me.txtUSD.StyleController = Me.LayoutControl1
        Me.txtUSD.TabIndex = 2
        '
        'txtDetails
        '
        Me.txtDetails.EditValue = ""
        Me.txtDetails.EnterMoveNextControl = True
        Me.txtDetails.Location = New System.Drawing.Point(12, 42)
        Me.txtDetails.MinimumSize = New System.Drawing.Size(0, 60)
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.txtDetails.Properties.Appearance.Options.UseFont = True
        Me.txtDetails.Size = New System.Drawing.Size(577, 63)
        Me.txtDetails.StyleController = Me.LayoutControl1
        Me.txtDetails.TabIndex = 1
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.AppearanceGroup.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.LayoutControlGroup1.AppearanceGroup.Options.UseFont = True
        Me.LayoutControlGroup1.AppearanceItemCaption.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.LayoutControlGroup1.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem4, Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem6})
        Me.LayoutControlGroup1.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        ColumnDefinition1.SizeType = System.Windows.Forms.SizeType.AutoSize
        ColumnDefinition1.Width = 158.0R
        ColumnDefinition2.SizeType = System.Windows.Forms.SizeType.AutoSize
        ColumnDefinition2.Width = 158.0R
        ColumnDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize
        ColumnDefinition3.Width = 158.0R
        ColumnDefinition4.SizeType = System.Windows.Forms.SizeType.AutoSize
        ColumnDefinition4.Width = 157.0R
        Me.LayoutControlGroup1.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(New DevExpress.XtraLayout.ColumnDefinition() {ColumnDefinition1, ColumnDefinition2, ColumnDefinition3, ColumnDefinition4})
        RowDefinition1.Height = 30.0R
        RowDefinition1.SizeType = System.Windows.Forms.SizeType.Absolute
        RowDefinition2.Height = 67.0R
        RowDefinition2.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition3.Height = 34.0R
        RowDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition4.Height = 34.0R
        RowDefinition4.SizeType = System.Windows.Forms.SizeType.AutoSize
        Me.LayoutControlGroup1.OptionsTableLayoutGroup.RowDefinitions.AddRange(New DevExpress.XtraLayout.RowDefinition() {RowDefinition1, RowDefinition2, RowDefinition3, RowDefinition4})
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(651, 185)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.txtEGP
        Me.LayoutControlItem4.CustomizationFormText = "Point Name:"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(474, 97)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.OptionsTableLayoutItem.ColumnIndex = 3
        Me.LayoutControlItem4.OptionsTableLayoutItem.RowIndex = 2
        Me.LayoutControlItem4.Size = New System.Drawing.Size(157, 34)
        Me.LayoutControlItem4.Text = "مصري:"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(47, 19)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.txtDetails
        Me.LayoutControlItem1.CustomizationFormText = "Point Name:"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 30)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.OptionsTableLayoutItem.ColumnSpan = 4
        Me.LayoutControlItem1.OptionsTableLayoutItem.RowIndex = 1
        Me.LayoutControlItem1.Size = New System.Drawing.Size(631, 67)
        Me.LayoutControlItem1.Text = "البيان:"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(47, 19)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.txtUSD
        Me.LayoutControlItem2.CustomizationFormText = "Point Name:"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(474, 131)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.OptionsTableLayoutItem.ColumnIndex = 3
        Me.LayoutControlItem2.OptionsTableLayoutItem.RowIndex = 3
        Me.LayoutControlItem2.Size = New System.Drawing.Size(157, 34)
        Me.LayoutControlItem2.Text = "دولار:"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(47, 19)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.deDate
        Me.LayoutControlItem6.CustomizationFormText = "Point Name:"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(316, 0)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.OptionsTableLayoutItem.ColumnIndex = 2
        Me.LayoutControlItem6.OptionsTableLayoutItem.ColumnSpan = 2
        Me.LayoutControlItem6.Size = New System.Drawing.Size(315, 30)
        Me.LayoutControlItem6.Text = "التاريخ:"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(47, 19)
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Appearance.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Appearance.ForeColor = System.Drawing.Color.Green
        Me.btnOK.Appearance.Options.UseFont = True
        Me.btnOK.Appearance.Options.UseForeColor = True
        Me.btnOK.Location = New System.Drawing.Point(136, 201)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(106, 37)
        Me.btnOK.TabIndex = 8
        Me.btnOK.Text = "Save"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Appearance.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Appearance.ForeColor = System.Drawing.Color.Red
        Me.btnCancel.Appearance.Options.UseFont = True
        Me.btnCancel.Appearance.Options.UseForeColor = True
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(24, 201)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(106, 37)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "Close"
        '
        'frmAddCheck
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(667, 250)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.LayoutControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmAddCheck"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Check"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.txtEGP.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.deDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.deDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtUSD.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDetails.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents txtEGP As DevExpress.XtraEditors.TextEdit
    Friend WithEvents deDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtUSD As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtDetails As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
End Class
