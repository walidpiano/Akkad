<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImport
    Inherits ComponentFactory.Krypton.Toolkit.KryptonForm

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
        Me.components = New System.ComponentModel.Container()
        Me.KryptonPanel = New ComponentFactory.Krypton.Toolkit.KryptonPanel()
        Me.KryptonListBox2 = New ComponentFactory.Krypton.Toolkit.KryptonListBox()
        Me.KryptonTextBox2 = New ComponentFactory.Krypton.Toolkit.KryptonTextBox()
        Me.KryptonWrapLabel1 = New ComponentFactory.Krypton.Toolkit.KryptonWrapLabel()
        Me.KryptonButton4 = New ComponentFactory.Krypton.Toolkit.KryptonButton()
        Me.KryptonListBox1 = New ComponentFactory.Krypton.Toolkit.KryptonListBox()
        Me.KryptonTextBox1 = New ComponentFactory.Krypton.Toolkit.KryptonTextBox()
        Me.KryptonButton1 = New ComponentFactory.Krypton.Toolkit.KryptonButton()
        Me.KryptonButton2 = New ComponentFactory.Krypton.Toolkit.KryptonButton()
        Me.KryptonButton3 = New ComponentFactory.Krypton.Toolkit.KryptonButton()
        Me.KryptonManager = New ComponentFactory.Krypton.Toolkit.KryptonManager(Me.components)
        CType(Me.KryptonPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.KryptonPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'KryptonPanel
        '
        Me.KryptonPanel.Controls.Add(Me.KryptonListBox2)
        Me.KryptonPanel.Controls.Add(Me.KryptonTextBox2)
        Me.KryptonPanel.Controls.Add(Me.KryptonWrapLabel1)
        Me.KryptonPanel.Controls.Add(Me.KryptonButton4)
        Me.KryptonPanel.Controls.Add(Me.KryptonListBox1)
        Me.KryptonPanel.Controls.Add(Me.KryptonTextBox1)
        Me.KryptonPanel.Controls.Add(Me.KryptonButton1)
        Me.KryptonPanel.Controls.Add(Me.KryptonButton2)
        Me.KryptonPanel.Controls.Add(Me.KryptonButton3)
        Me.KryptonPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.KryptonPanel.Location = New System.Drawing.Point(0, 0)
        Me.KryptonPanel.Name = "KryptonPanel"
        Me.KryptonPanel.Size = New System.Drawing.Size(867, 327)
        Me.KryptonPanel.TabIndex = 0
        '
        'KryptonListBox2
        '
        Me.KryptonListBox2.Location = New System.Drawing.Point(596, 3)
        Me.KryptonListBox2.Name = "KryptonListBox2"
        Me.KryptonListBox2.Size = New System.Drawing.Size(259, 280)
        Me.KryptonListBox2.TabIndex = 10
        '
        'KryptonTextBox2
        '
        Me.KryptonTextBox2.Location = New System.Drawing.Point(24, 295)
        Me.KryptonTextBox2.Name = "KryptonTextBox2"
        Me.KryptonTextBox2.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.KryptonTextBox2.Size = New System.Drawing.Size(256, 20)
        Me.KryptonTextBox2.TabIndex = 8
        Me.KryptonTextBox2.UseSystemPasswordChar = True
        '
        'KryptonWrapLabel1
        '
        Me.KryptonWrapLabel1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.KryptonWrapLabel1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(59, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(59, Byte), Integer))
        Me.KryptonWrapLabel1.Location = New System.Drawing.Point(389, 286)
        Me.KryptonWrapLabel1.Name = "KryptonWrapLabel1"
        Me.KryptonWrapLabel1.Size = New System.Drawing.Size(205, 15)
        Me.KryptonWrapLabel1.Text = "Serial - Name - Price - Qnty - Vendors"
        '
        'KryptonButton4
        '
        Me.KryptonButton4.Enabled = False
        Me.KryptonButton4.Location = New System.Drawing.Point(24, 238)
        Me.KryptonButton4.Name = "KryptonButton4"
        Me.KryptonButton4.Size = New System.Drawing.Size(90, 25)
        Me.KryptonButton4.TabIndex = 6
        Me.KryptonButton4.Values.Text = "In"
        '
        'KryptonListBox1
        '
        Me.KryptonListBox1.Location = New System.Drawing.Point(135, 3)
        Me.KryptonListBox1.Name = "KryptonListBox1"
        Me.KryptonListBox1.Size = New System.Drawing.Size(459, 280)
        Me.KryptonListBox1.TabIndex = 1
        '
        'KryptonTextBox1
        '
        Me.KryptonTextBox1.Location = New System.Drawing.Point(396, 26)
        Me.KryptonTextBox1.Multiline = True
        Me.KryptonTextBox1.Name = "KryptonTextBox1"
        Me.KryptonTextBox1.Size = New System.Drawing.Size(180, 66)
        Me.KryptonTextBox1.TabIndex = 2
        Me.KryptonTextBox1.Text = "KryptonTextBox1"
        '
        'KryptonButton1
        '
        Me.KryptonButton1.Location = New System.Drawing.Point(24, 207)
        Me.KryptonButton1.Name = "KryptonButton1"
        Me.KryptonButton1.Size = New System.Drawing.Size(90, 25)
        Me.KryptonButton1.TabIndex = 3
        Me.KryptonButton1.Values.Text = "Item"
        '
        'KryptonButton2
        '
        Me.KryptonButton2.Enabled = False
        Me.KryptonButton2.Location = New System.Drawing.Point(24, 176)
        Me.KryptonButton2.Name = "KryptonButton2"
        Me.KryptonButton2.Size = New System.Drawing.Size(90, 25)
        Me.KryptonButton2.TabIndex = 4
        Me.KryptonButton2.Values.Text = "Vendor"
        '
        'KryptonButton3
        '
        Me.KryptonButton3.Location = New System.Drawing.Point(24, 145)
        Me.KryptonButton3.Name = "KryptonButton3"
        Me.KryptonButton3.Size = New System.Drawing.Size(90, 25)
        Me.KryptonButton3.TabIndex = 5
        Me.KryptonButton3.Values.Text = "Import"
        '
        'KryptonManager
        '
        Me.KryptonManager.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Silver
        '
        'frmImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(867, 327)
        Me.Controls.Add(Me.KryptonPanel)
        Me.Name = "frmImport"
        Me.Text = "frmImport"
        CType(Me.KryptonPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.KryptonPanel.ResumeLayout(False)
        Me.KryptonPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents KryptonPanel As ComponentFactory.Krypton.Toolkit.KryptonPanel
    Friend WithEvents KryptonManager As ComponentFactory.Krypton.Toolkit.KryptonManager

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Friend WithEvents KryptonListBox1 As ComponentFactory.Krypton.Toolkit.KryptonListBox
    Friend WithEvents KryptonTextBox1 As ComponentFactory.Krypton.Toolkit.KryptonTextBox
    Friend WithEvents KryptonButton1 As ComponentFactory.Krypton.Toolkit.KryptonButton
    Friend WithEvents KryptonButton2 As ComponentFactory.Krypton.Toolkit.KryptonButton
    Friend WithEvents KryptonButton3 As ComponentFactory.Krypton.Toolkit.KryptonButton
    Friend WithEvents KryptonButton4 As ComponentFactory.Krypton.Toolkit.KryptonButton
    Friend WithEvents KryptonTextBox2 As ComponentFactory.Krypton.Toolkit.KryptonTextBox
    Friend WithEvents KryptonWrapLabel1 As ComponentFactory.Krypton.Toolkit.KryptonWrapLabel
    Friend WithEvents KryptonListBox2 As ComponentFactory.Krypton.Toolkit.KryptonListBox
End Class
