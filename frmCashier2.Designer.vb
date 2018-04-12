<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCashier2
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
        Me.TileControl1 = New DevExpress.XtraEditors.TileControl()
        Me.NavButton2 = New DevExpress.XtraBars.Navigation.NavButton()
        Me.NavButton3 = New DevExpress.XtraBars.Navigation.NavButton()
        Me.NavButton4 = New DevExpress.XtraBars.Navigation.NavButton()
        Me.NavButton1 = New DevExpress.XtraBars.Navigation.NavButton()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.oDgv = New ComponentFactory.Krypton.Toolkit.KryptonDataGridView()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.oDgv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TileControl1
        '
        Me.TileControl1.BackgroundImage = Global.MASTER_pro.My.Resources.Resources.TT16s
        Me.TileControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.TileControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TileControl1.DragSize = New System.Drawing.Size(0, 0)
        Me.TileControl1.Location = New System.Drawing.Point(0, 0)
        Me.TileControl1.MaxId = 11
        Me.TileControl1.Name = "TileControl1"
        Me.TileControl1.Size = New System.Drawing.Size(937, 451)
        Me.TileControl1.TabIndex = 0
        Me.TileControl1.Text = "TileControl1"
        '
        'NavButton2
        '
        Me.NavButton2.Caption = "Main Menu"
        Me.NavButton2.IsMain = True
        Me.NavButton2.Name = "NavButton2"
        '
        'NavButton3
        '
        Me.NavButton3.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Right
        Me.NavButton3.Caption = "NavButton3"
        Me.NavButton3.Name = "NavButton3"
        '
        'NavButton4
        '
        Me.NavButton4.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Right
        Me.NavButton4.Caption = "NavButton4"
        Me.NavButton4.Name = "NavButton4"
        '
        'NavButton1
        '
        Me.NavButton1.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Right
        Me.NavButton1.Caption = "NavButton5"
        Me.NavButton1.Name = "NavButton1"
        '
        'PanelControl1
        '
        Me.PanelControl1.Appearance.BackColor = System.Drawing.Color.DimGray
        Me.PanelControl1.Appearance.BackColor2 = System.Drawing.Color.DimGray
        Me.PanelControl1.Appearance.BorderColor = System.Drawing.Color.Transparent
        Me.PanelControl1.Appearance.Options.UseBackColor = True
        Me.PanelControl1.Appearance.Options.UseBorderColor = True
        Me.PanelControl1.Location = New System.Drawing.Point(725, 12)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(200, 427)
        Me.PanelControl1.TabIndex = 2
        '
        'oDgv
        '
        Me.oDgv.AllowUserToAddRows = False
        Me.oDgv.AllowUserToDeleteRows = False
        Me.oDgv.AllowUserToResizeRows = False
        Me.oDgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.oDgv.ColumnHeadersHeight = 40
        Me.oDgv.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column4, Me.Column9, Me.Column5, Me.Column6, Me.Column7, Me.Column8})
        Me.oDgv.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed
        Me.oDgv.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnList
        Me.oDgv.GridStyles.StyleColumn = ComponentFactory.Krypton.Toolkit.GridStyle.Sheet
        Me.oDgv.GridStyles.StyleRow = ComponentFactory.Krypton.Toolkit.GridStyle.Sheet
        Me.oDgv.Location = New System.Drawing.Point(180, 41)
        Me.oDgv.MultiSelect = False
        Me.oDgv.Name = "oDgv"
        Me.oDgv.ReadOnly = True
        Me.oDgv.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.oDgv.RowHeadersVisible = False
        Me.oDgv.Size = New System.Drawing.Size(468, 253)
        Me.oDgv.TabIndex = 16
        Me.oDgv.TabStop = False
        '
        'Column4
        '
        Me.Column4.HeaderText = "الكمية"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Width = 66
        '
        'Column9
        '
        Me.Column9.HeaderText = "Sort"
        Me.Column9.Name = "Column9"
        Me.Column9.ReadOnly = True
        Me.Column9.Visible = False
        Me.Column9.Width = 57
        '
        'Column5
        '
        Me.Column5.HeaderText = "الكود"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Width = 61
        '
        'Column6
        '
        Me.Column6.HeaderText = "الصنف"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        Me.Column6.Width = 71
        '
        'Column7
        '
        Me.Column7.HeaderText = "سعر الوحدة"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        Me.Column7.Width = 86
        '
        'Column8
        '
        Me.Column8.HeaderText = "القيمة"
        Me.Column8.Name = "Column8"
        Me.Column8.ReadOnly = True
        Me.Column8.Width = 66
        '
        'frmCashier2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(937, 451)
        Me.Controls.Add(Me.oDgv)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.TileControl1)
        Me.Name = "frmCashier2"
        Me.Text = "Cashier"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.oDgv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TileControl1 As DevExpress.XtraEditors.TileControl
    Friend WithEvents NavButton2 As DevExpress.XtraBars.Navigation.NavButton
    Friend WithEvents NavButton3 As DevExpress.XtraBars.Navigation.NavButton
    Friend WithEvents NavButton4 As DevExpress.XtraBars.Navigation.NavButton
    Friend WithEvents NavButton1 As DevExpress.XtraBars.Navigation.NavButton
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents oDgv As ComponentFactory.Krypton.Toolkit.KryptonDataGridView
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
