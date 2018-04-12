<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class XtraBarcodes
    Inherits DevExpress.XtraReports.UI.XtraReport

    'XtraReport overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Designer
    'It can be modified using the Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim Code128Generator1 As DevExpress.XtraPrinting.BarCode.Code128Generator = New DevExpress.XtraPrinting.BarCode.Code128Generator()
        Me.Detail = New DevExpress.XtraReports.UI.DetailBand()
        Me.XrPanel1 = New DevExpress.XtraReports.UI.XRPanel()
        Me.XrlbItemName = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrBarcode = New DevExpress.XtraReports.UI.XRBarCode()
        Me.TopMarginBand1 = New DevExpress.XtraReports.UI.TopMarginBand()
        Me.BottomMarginBand1 = New DevExpress.XtraReports.UI.BottomMarginBand()
        Me.ReportsDS1 = New MASTER_pro.ReportsDS()
        CType(Me.ReportsDS1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'Detail
        '
        Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrPanel1})
        Me.Detail.HeightF = 136.7035!
        Me.Detail.MultiColumn.ColumnSpacing = 10.0!
        Me.Detail.MultiColumn.ColumnWidth = 355.0!
        Me.Detail.MultiColumn.Layout = DevExpress.XtraPrinting.ColumnLayout.AcrossThenDown
        Me.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnWidth
        Me.Detail.Name = "Detail"
        Me.Detail.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
        Me.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'XrPanel1
        '
        Me.XrPanel1.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrPanel1.CanGrow = False
        Me.XrPanel1.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrlbItemName, Me.XrBarcode})
        Me.XrPanel1.LocationFloat = New DevExpress.Utils.PointFloat(9.999998!, 9.999998!)
        Me.XrPanel1.Name = "XrPanel1"
        Me.XrPanel1.SizeF = New System.Drawing.SizeF(145.012!, 114.4259!)
        '
        'XrlbItemName
        '
        Me.XrlbItemName.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "tblBarcode.Item")})
        Me.XrlbItemName.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.XrlbItemName.LocationFloat = New DevExpress.Utils.PointFloat(2.000006!, 72.22035!)
        Me.XrlbItemName.Name = "XrlbItemName"
        Me.XrlbItemName.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrlbItemName.SizeF = New System.Drawing.SizeF(138.4973!, 32.99998!)
        Me.XrlbItemName.StylePriority.UseFont = False
        Me.XrlbItemName.StylePriority.UseTextAlignment = False
        Me.XrlbItemName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrBarcode
        '
        Me.XrBarcode.AutoModule = True
        Me.XrBarcode.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "tblBarcode.PrKey")})
        Me.XrBarcode.Font = New System.Drawing.Font("Times New Roman", 10.0!)
        Me.XrBarcode.LocationFloat = New DevExpress.Utils.PointFloat(2.000006!, 10.00001!)
        Me.XrBarcode.Name = "XrBarcode"
        Me.XrBarcode.Padding = New DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100.0!)
        Me.XrBarcode.SizeF = New System.Drawing.SizeF(138.4973!, 62.22035!)
        Me.XrBarcode.StylePriority.UseFont = False
        Me.XrBarcode.Symbology = Code128Generator1
        Me.XrBarcode.Text = "8690757202509"
        '
        'TopMarginBand1
        '
        Me.TopMarginBand1.HeightF = 13.0!
        Me.TopMarginBand1.Name = "TopMarginBand1"
        '
        'BottomMarginBand1
        '
        Me.BottomMarginBand1.HeightF = 10.0!
        Me.BottomMarginBand1.Name = "BottomMarginBand1"
        '
        'ReportsDS1
        '
        Me.ReportsDS1.DataSetName = "ReportsDS"
        Me.ReportsDS1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'XtraBarcodes
        '
        Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.Detail, Me.TopMarginBand1, Me.BottomMarginBand1})
        Me.BorderColor = System.Drawing.Color.Transparent
        Me.DataMember = "tblBarcode"
        Me.DataSource = Me.ReportsDS1
        Me.DisplayName = "Labels"
        Me.Margins = New System.Drawing.Printing.Margins(0, 33, 13, 10)
        Me.PageHeight = 1169
        Me.PageWidth = 198
        Me.PaperKind = System.Drawing.Printing.PaperKind.Custom
        Me.ReportPrintOptions.DetailCountOnEmptyDataSource = 12
        Me.RollPaper = True
        Me.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic
        Me.Version = "15.1"
        CType(Me.ReportsDS1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
    Friend WithEvents XrPanel1 As DevExpress.XtraReports.UI.XRPanel
    Friend WithEvents TopMarginBand1 As DevExpress.XtraReports.UI.TopMarginBand
    Friend WithEvents BottomMarginBand1 As DevExpress.XtraReports.UI.BottomMarginBand
    Friend WithEvents XrBarcode As DevExpress.XtraReports.UI.XRBarCode
    Friend WithEvents XrlbItemName As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents ReportsDS1 As MASTER_pro.ReportsDS
End Class
