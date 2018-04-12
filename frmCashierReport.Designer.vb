<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCashierReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCashierReport))
        Me.KryptonLabel77 = New ComponentFactory.Krypton.Toolkit.KryptonLabel()
        Me.dailyDateFrom = New ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker()
        Me.dailyDateTill = New ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker()
        Me.tmFrom = New ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker()
        Me.tmTill = New ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker()
        Me.KryptonLabel1 = New ComponentFactory.Krypton.Toolkit.KryptonLabel()
        Me.KryptonLabel2 = New ComponentFactory.Krypton.Toolkit.KryptonLabel()
        Me.cbCashiers = New System.Windows.Forms.ComboBox()
        Me.btnCurrency = New DevExpress.XtraEditors.SimpleButton()
        Me.Cancel = New DevExpress.XtraEditors.SimpleButton()
        Me.btnItems = New DevExpress.XtraEditors.SimpleButton()
        Me.rgPayment = New DevExpress.XtraEditors.RadioGroup()
        CType(Me.rgPayment.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'KryptonLabel77
        '
        Me.KryptonLabel77.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.KryptonLabel77.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel
        Me.KryptonLabel77.Location = New System.Drawing.Point(336, 21)
        Me.KryptonLabel77.Name = "KryptonLabel77"
        Me.KryptonLabel77.Size = New System.Drawing.Size(59, 24)
        Me.KryptonLabel77.StateCommon.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far
        Me.KryptonLabel77.StateNormal.ShortText.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KryptonLabel77.TabIndex = 7
        Me.KryptonLabel77.Values.Text = ":التاريخ"
        '
        'dailyDateFrom
        '
        Me.dailyDateFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dailyDateFrom.CalendarTodayDate = New Date(2015, 9, 7, 0, 0, 0, 0)
        Me.dailyDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dailyDateFrom.Location = New System.Drawing.Point(187, 21)
        Me.dailyDateFrom.Name = "dailyDateFrom"
        Me.dailyDateFrom.Size = New System.Drawing.Size(129, 25)
        Me.dailyDateFrom.StateCommon.Content.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dailyDateFrom.TabIndex = 8
        '
        'dailyDateTill
        '
        Me.dailyDateTill.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dailyDateTill.CalendarTodayDate = New Date(2015, 9, 7, 0, 0, 0, 0)
        Me.dailyDateTill.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dailyDateTill.Location = New System.Drawing.Point(20, 21)
        Me.dailyDateTill.Name = "dailyDateTill"
        Me.dailyDateTill.Size = New System.Drawing.Size(129, 25)
        Me.dailyDateTill.StateCommon.Content.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dailyDateTill.TabIndex = 9
        '
        'tmFrom
        '
        Me.tmFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tmFrom.CalendarTodayDate = New Date(2015, 6, 18, 0, 0, 0, 0)
        Me.tmFrom.CustomFormat = "HH:mm"
        Me.tmFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.tmFrom.Location = New System.Drawing.Point(227, 52)
        Me.tmFrom.Name = "tmFrom"
        Me.tmFrom.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.tmFrom.ShowUpDown = True
        Me.tmFrom.Size = New System.Drawing.Size(89, 25)
        Me.tmFrom.StateCommon.Content.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tmFrom.TabIndex = 58
        Me.tmFrom.TabStop = False
        '
        'tmTill
        '
        Me.tmTill.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tmTill.CalendarTodayDate = New Date(2015, 6, 18, 0, 0, 0, 0)
        Me.tmTill.CustomFormat = "HH:mm"
        Me.tmTill.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.tmTill.Location = New System.Drawing.Point(60, 52)
        Me.tmTill.Name = "tmTill"
        Me.tmTill.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.tmTill.ShowUpDown = True
        Me.tmTill.Size = New System.Drawing.Size(89, 25)
        Me.tmTill.StateCommon.Content.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tmTill.TabIndex = 59
        Me.tmTill.TabStop = False
        '
        'KryptonLabel1
        '
        Me.KryptonLabel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.KryptonLabel1.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel
        Me.KryptonLabel1.Location = New System.Drawing.Point(339, 53)
        Me.KryptonLabel1.Name = "KryptonLabel1"
        Me.KryptonLabel1.Size = New System.Drawing.Size(56, 24)
        Me.KryptonLabel1.StateCommon.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far
        Me.KryptonLabel1.StateNormal.ShortText.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KryptonLabel1.TabIndex = 60
        Me.KryptonLabel1.Values.Text = ":الوقت"
        '
        'KryptonLabel2
        '
        Me.KryptonLabel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.KryptonLabel2.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalPanel
        Me.KryptonLabel2.Location = New System.Drawing.Point(326, 90)
        Me.KryptonLabel2.Name = "KryptonLabel2"
        Me.KryptonLabel2.Size = New System.Drawing.Size(69, 24)
        Me.KryptonLabel2.StateCommon.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far
        Me.KryptonLabel2.StateNormal.ShortText.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KryptonLabel2.TabIndex = 61
        Me.KryptonLabel2.Values.Text = ":الكاشير"
        '
        'cbCashiers
        '
        Me.cbCashiers.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCashiers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbCashiers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbCashiers.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbCashiers.FormattingEnabled = True
        Me.cbCashiers.Location = New System.Drawing.Point(40, 87)
        Me.cbCashiers.Name = "cbCashiers"
        Me.cbCashiers.Size = New System.Drawing.Size(276, 27)
        Me.cbCashiers.TabIndex = 78
        Me.cbCashiers.TabStop = False
        '
        'btnCurrency
        '
        Me.btnCurrency.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCurrency.Appearance.Font = New System.Drawing.Font("Tahoma", 16.0!)
        Me.btnCurrency.Appearance.Options.UseFont = True
        Me.btnCurrency.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCurrency.ImageOptions.Image = CType(resources.GetObject("btnCurrency.ImageOptions.Image"), System.Drawing.Image)
        Me.btnCurrency.Location = New System.Drawing.Point(271, 173)
        Me.btnCurrency.Name = "btnCurrency"
        Me.btnCurrency.Size = New System.Drawing.Size(109, 34)
        Me.btnCurrency.TabIndex = 79
        Me.btnCurrency.Text = "عملة"
        '
        'Cancel
        '
        Me.Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel.Appearance.Font = New System.Drawing.Font("Tahoma", 16.0!)
        Me.Cancel.Appearance.Options.UseFont = True
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.ImageOptions.Image = CType(resources.GetObject("Cancel.ImageOptions.Image"), System.Drawing.Image)
        Me.Cancel.Location = New System.Drawing.Point(20, 173)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(109, 34)
        Me.Cancel.TabIndex = 80
        Me.Cancel.Text = "CLOSE"
        '
        'btnItems
        '
        Me.btnItems.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnItems.Appearance.Font = New System.Drawing.Font("Tahoma", 16.0!)
        Me.btnItems.Appearance.Options.UseFont = True
        Me.btnItems.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnItems.ImageOptions.Image = CType(resources.GetObject("btnItems.ImageOptions.Image"), System.Drawing.Image)
        Me.btnItems.Location = New System.Drawing.Point(146, 173)
        Me.btnItems.Name = "btnItems"
        Me.btnItems.Size = New System.Drawing.Size(109, 34)
        Me.btnItems.TabIndex = 81
        Me.btnItems.Text = "صنف"
        '
        'rgPayment
        '
        Me.rgPayment.EditValue = CType(0, Byte)
        Me.rgPayment.Location = New System.Drawing.Point(146, 120)
        Me.rgPayment.Name = "rgPayment"
        Me.rgPayment.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.rgPayment.Properties.Appearance.Options.UseFont = True
        Me.rgPayment.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(0, Byte), "كاش", True, CType(0, Byte)), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(1, Byte), "آجل", True, CType(1, Byte))})
        Me.rgPayment.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.rgPayment.Size = New System.Drawing.Size(170, 26)
        Me.rgPayment.TabIndex = 82
        '
        'frmCashierReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel
        Me.ClientSize = New System.Drawing.Size(421, 219)
        Me.Controls.Add(Me.rgPayment)
        Me.Controls.Add(Me.btnItems)
        Me.Controls.Add(Me.btnCurrency)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.cbCashiers)
        Me.Controls.Add(Me.KryptonLabel2)
        Me.Controls.Add(Me.KryptonLabel1)
        Me.Controls.Add(Me.tmTill)
        Me.Controls.Add(Me.tmFrom)
        Me.Controls.Add(Me.dailyDateTill)
        Me.Controls.Add(Me.dailyDateFrom)
        Me.Controls.Add(Me.KryptonLabel77)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmCashierReport"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cashier Monitor"
        CType(Me.rgPayment.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents KryptonLabel77 As ComponentFactory.Krypton.Toolkit.KryptonLabel
    Friend WithEvents dailyDateFrom As ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker
    Friend WithEvents dailyDateTill As ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker
    Friend WithEvents tmFrom As ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker
    Friend WithEvents tmTill As ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker
    Private WithEvents KryptonLabel1 As ComponentFactory.Krypton.Toolkit.KryptonLabel
    Private WithEvents KryptonLabel2 As ComponentFactory.Krypton.Toolkit.KryptonLabel
    Friend WithEvents cbCashiers As System.Windows.Forms.ComboBox
    Friend WithEvents btnCurrency As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Cancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnItems As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents rgPayment As DevExpress.XtraEditors.RadioGroup
End Class
