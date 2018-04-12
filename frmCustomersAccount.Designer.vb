<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class frmCustomersAccount
    Inherits DevExpress.XtraEditors.XtraForm
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    '''
    Private Sub InitializeComponent()
        Dim WindowsUIButtonImageOptions1 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsUIButtonImageOptions2 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsUIButtonImageOptions3 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsUIButtonImageOptions4 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsUIButtonImageOptions5 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim GridFormatRule1 As DevExpress.XtraGrid.GridFormatRule = New DevExpress.XtraGrid.GridFormatRule()
        Dim FormatConditionRuleExpression1 As DevExpress.XtraEditors.FormatConditionRuleExpression = New DevExpress.XtraEditors.FormatConditionRuleExpression()
        Me.windowsUIButtonPanel = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.GridColumn1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn4 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn2 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn6 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn11 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn3 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn5 = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'windowsUIButtonPanel
        '
        Me.windowsUIButtonPanel.AppearanceButton.Hovered.BackColor = System.Drawing.Color.FromArgb(CType(CType(130, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(130, Byte), Integer))
        Me.windowsUIButtonPanel.AppearanceButton.Hovered.FontSizeDelta = -1
        Me.windowsUIButtonPanel.AppearanceButton.Hovered.ForeColor = System.Drawing.Color.FromArgb(CType(CType(130, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(130, Byte), Integer))
        Me.windowsUIButtonPanel.AppearanceButton.Hovered.Options.UseBackColor = True
        Me.windowsUIButtonPanel.AppearanceButton.Hovered.Options.UseFont = True
        Me.windowsUIButtonPanel.AppearanceButton.Hovered.Options.UseForeColor = True
        Me.windowsUIButtonPanel.AppearanceButton.Normal.FontSizeDelta = -1
        Me.windowsUIButtonPanel.AppearanceButton.Normal.Options.UseFont = True
        Me.windowsUIButtonPanel.AppearanceButton.Pressed.BackColor = System.Drawing.Color.FromArgb(CType(CType(159, Byte), Integer), CType(CType(159, Byte), Integer), CType(CType(159, Byte), Integer))
        Me.windowsUIButtonPanel.AppearanceButton.Pressed.FontSizeDelta = -1
        Me.windowsUIButtonPanel.AppearanceButton.Pressed.ForeColor = System.Drawing.Color.FromArgb(CType(CType(159, Byte), Integer), CType(CType(159, Byte), Integer), CType(CType(159, Byte), Integer))
        Me.windowsUIButtonPanel.AppearanceButton.Pressed.Options.UseBackColor = True
        Me.windowsUIButtonPanel.AppearanceButton.Pressed.Options.UseFont = True
        Me.windowsUIButtonPanel.AppearanceButton.Pressed.Options.UseForeColor = True
        Me.windowsUIButtonPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(63, Byte), Integer), CType(CType(63, Byte), Integer), CType(CType(63, Byte), Integer))
        WindowsUIButtonImageOptions1.ImageUri.Uri = "New;Size32x32;GrayScaled"
        WindowsUIButtonImageOptions2.ImageUri.Uri = "Edit;Size32x32;GrayScaled"
        WindowsUIButtonImageOptions3.ImageUri.Uri = "Edit/Delete;Size32x32;GrayScaled"
        WindowsUIButtonImageOptions4.ImageUri.Uri = "Refresh;Size32x32;GrayScaled"
        WindowsUIButtonImageOptions5.ImageUri.Uri = "Preview;Size32x32;GrayScaled"
        Me.windowsUIButtonPanel.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraBars.Docking2010.WindowsUIButton("New", True, WindowsUIButtonImageOptions1), New DevExpress.XtraBars.Docking2010.WindowsUIButton("Edit", True, WindowsUIButtonImageOptions2), New DevExpress.XtraBars.Docking2010.WindowsUIButton("Delete", True, WindowsUIButtonImageOptions3), New DevExpress.XtraBars.Docking2010.WindowsUIButton("Refresh", True, WindowsUIButtonImageOptions4), New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton("Print", True, WindowsUIButtonImageOptions5)})
        Me.windowsUIButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.windowsUIButtonPanel.EnableImageTransparency = True
        Me.windowsUIButtonPanel.ForeColor = System.Drawing.Color.White
        Me.windowsUIButtonPanel.Location = New System.Drawing.Point(0, 501)
        Me.windowsUIButtonPanel.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.windowsUIButtonPanel.MaximumSize = New System.Drawing.Size(0, 60)
        Me.windowsUIButtonPanel.MinimumSize = New System.Drawing.Size(60, 60)
        Me.windowsUIButtonPanel.Name = "windowsUIButtonPanel"
        Me.windowsUIButtonPanel.Size = New System.Drawing.Size(697, 60)
        Me.windowsUIButtonPanel.TabIndex = 5
        Me.windowsUIButtonPanel.Text = "windowsUIButtonPanel"
        Me.windowsUIButtonPanel.UseButtonBackgroundImages = False
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(0, 0)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.GridControl1.Size = New System.Drawing.Size(697, 501)
        Me.GridControl1.TabIndex = 16
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Appearance.ColumnFilterButton.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.ColumnFilterButton.Options.UseFont = True
        Me.GridView1.Appearance.ColumnFilterButtonActive.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.ColumnFilterButtonActive.Options.UseFont = True
        Me.GridView1.Appearance.CustomizationFormHint.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.CustomizationFormHint.Options.UseFont = True
        Me.GridView1.Appearance.DetailTip.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.DetailTip.Options.UseFont = True
        Me.GridView1.Appearance.Empty.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.Empty.Options.UseFont = True
        Me.GridView1.Appearance.EvenRow.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.EvenRow.Options.UseFont = True
        Me.GridView1.Appearance.FilterCloseButton.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.FilterCloseButton.Options.UseFont = True
        Me.GridView1.Appearance.FilterPanel.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.FilterPanel.Options.UseFont = True
        Me.GridView1.Appearance.FixedLine.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.FixedLine.Options.UseFont = True
        Me.GridView1.Appearance.FocusedCell.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.FocusedCell.Options.UseFont = True
        Me.GridView1.Appearance.FocusedRow.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.FocusedRow.Options.UseFont = True
        Me.GridView1.Appearance.FooterPanel.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.FooterPanel.Options.UseFont = True
        Me.GridView1.Appearance.GroupButton.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.GroupButton.Options.UseFont = True
        Me.GridView1.Appearance.GroupFooter.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.GroupFooter.Options.UseFont = True
        Me.GridView1.Appearance.GroupPanel.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.GroupPanel.Options.UseFont = True
        Me.GridView1.Appearance.GroupRow.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.GroupRow.Options.UseFont = True
        Me.GridView1.Appearance.HeaderPanel.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GridView1.Appearance.HeaderPanel.Options.UseFont = True
        Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
        Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView1.Appearance.HideSelectionRow.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.HideSelectionRow.Options.UseFont = True
        Me.GridView1.Appearance.HorzLine.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.HorzLine.Options.UseFont = True
        Me.GridView1.Appearance.OddRow.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.OddRow.Options.UseFont = True
        Me.GridView1.Appearance.Preview.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.Preview.Options.UseFont = True
        Me.GridView1.Appearance.Row.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.Row.Options.UseFont = True
        Me.GridView1.Appearance.RowSeparator.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.RowSeparator.Options.UseFont = True
        Me.GridView1.Appearance.SelectedRow.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.SelectedRow.Options.UseFont = True
        Me.GridView1.Appearance.TopNewRow.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.TopNewRow.Options.UseFont = True
        Me.GridView1.Appearance.VertLine.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.VertLine.Options.UseFont = True
        Me.GridView1.Appearance.ViewCaption.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.GridView1.Appearance.ViewCaption.Options.UseFont = True
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.GridColumn1, Me.GridColumn4, Me.GridColumn2, Me.GridColumn6, Me.GridColumn11, Me.GridColumn3, Me.GridColumn5})
        GridFormatRule1.ApplyToRow = True
        GridFormatRule1.Name = "Format0"
        FormatConditionRuleExpression1.Appearance.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        FormatConditionRuleExpression1.Appearance.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        FormatConditionRuleExpression1.Appearance.ForeColor = System.Drawing.Color.Red
        FormatConditionRuleExpression1.Appearance.Options.UseFont = True
        FormatConditionRuleExpression1.Appearance.Options.UseForeColor = True
        FormatConditionRuleExpression1.Expression = "[Serial] = 'Payment'"
        FormatConditionRuleExpression1.PredefinedName = "Red Text"
        GridFormatRule1.Rule = FormatConditionRuleExpression1
        Me.GridView1.FormatRules.Add(GridFormatRule1)
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.GroupSummary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Amount", Nothing, "(SUM={0:0.##})")})
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.[True]
        Me.GridView1.OptionsView.ShowFooter = True
        '
        'GridColumn1
        '
        Me.GridColumn1.Caption = "ID"
        Me.GridColumn1.FieldName = "ID"
        Me.GridColumn1.Name = "GridColumn1"
        Me.GridColumn1.OptionsColumn.AllowEdit = False
        Me.GridColumn1.OptionsColumn.AllowShowHide = False
        Me.GridColumn1.Width = 57
        '
        'GridColumn4
        '
        Me.GridColumn4.Caption = "العميل"
        Me.GridColumn4.FieldName = "Customer"
        Me.GridColumn4.Name = "GridColumn4"
        Me.GridColumn4.OptionsColumn.AllowEdit = False
        Me.GridColumn4.Visible = True
        Me.GridColumn4.VisibleIndex = 0
        Me.GridColumn4.Width = 144
        '
        'GridColumn2
        '
        Me.GridColumn2.Caption = "التاريخ"
        Me.GridColumn2.DisplayFormat.FormatString = "yyyy/MM/dd"
        Me.GridColumn2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom
        Me.GridColumn2.FieldName = "Date"
        Me.GridColumn2.Name = "GridColumn2"
        Me.GridColumn2.OptionsColumn.AllowEdit = False
        Me.GridColumn2.Visible = True
        Me.GridColumn2.VisibleIndex = 1
        Me.GridColumn2.Width = 65
        '
        'GridColumn6
        '
        Me.GridColumn6.Caption = "الوقت"
        Me.GridColumn6.DisplayFormat.FormatString = "HH:mm"
        Me.GridColumn6.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom
        Me.GridColumn6.FieldName = "Time"
        Me.GridColumn6.Name = "GridColumn6"
        Me.GridColumn6.OptionsColumn.AllowEdit = False
        Me.GridColumn6.Visible = True
        Me.GridColumn6.VisibleIndex = 2
        Me.GridColumn6.Width = 43
        '
        'GridColumn11
        '
        Me.GridColumn11.Caption = "البيان"
        Me.GridColumn11.FieldName = "Serial"
        Me.GridColumn11.Name = "GridColumn11"
        Me.GridColumn11.OptionsColumn.AllowEdit = False
        Me.GridColumn11.Visible = True
        Me.GridColumn11.VisibleIndex = 3
        Me.GridColumn11.Width = 57
        '
        'GridColumn3
        '
        Me.GridColumn3.Caption = "المبلغ"
        Me.GridColumn3.FieldName = "Amount"
        Me.GridColumn3.Name = "GridColumn3"
        Me.GridColumn3.OptionsColumn.AllowEdit = False
        Me.GridColumn3.Summary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Amount", "SUM={0:0.##}")})
        Me.GridColumn3.Visible = True
        Me.GridColumn3.VisibleIndex = 4
        Me.GridColumn3.Width = 78
        '
        'GridColumn5
        '
        Me.GridColumn5.Caption = "User"
        Me.GridColumn5.FieldName = "Cashier"
        Me.GridColumn5.Name = "GridColumn5"
        Me.GridColumn5.OptionsColumn.AllowEdit = False
        Me.GridColumn5.Visible = True
        Me.GridColumn5.VisibleIndex = 5
        Me.GridColumn5.Width = 104
        '
        'frmCustomersAccount
        '
        Me.Appearance.BackColor = System.Drawing.Color.White
        Me.Appearance.Options.UseBackColor = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(697, 561)
        Me.Controls.Add(Me.GridControl1)
        Me.Controls.Add(Me.windowsUIButtonPanel)
        Me.Name = "frmCustomersAccount"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Customer Account"
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private WithEvents windowsUIButtonPanel As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridColumn1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn2 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn6 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn11 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn3 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn5 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn4 As DevExpress.XtraGrid.Columns.GridColumn
End Class
