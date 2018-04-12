<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class XtraPoster2
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
        Me.XrBarcode = New DevExpress.XtraReports.UI.XRBarCode()
        Me.XrlbItemName = New DevExpress.XtraReports.UI.XRLabel()
        Me.ReportsDS1 = New MASTER_pro.ReportsDS()
        Me.TopMarginBand1 = New DevExpress.XtraReports.UI.TopMarginBand()
        Me.BottomMarginBand1 = New DevExpress.XtraReports.UI.BottomMarginBand()
        CType(Me.ReportsDS1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'Detail
        '
        Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrPanel1})
        Me.Detail.HeightF = 82.6772!
        Me.Detail.MultiColumn.ColumnWidth = 204.7244!
        Me.Detail.MultiColumn.Layout = DevExpress.XtraPrinting.ColumnLayout.AcrossThenDown
        Me.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnWidth
        Me.Detail.Name = "Detail"
        Me.Detail.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
        Me.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'XrPanel1
        '
        Me.XrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrPanel1.CanGrow = False
        Me.XrPanel1.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrBarcode, Me.XrlbItemName})
        Me.XrPanel1.LocationFloat = New DevExpress.Utils.PointFloat(0.0!, 0.0!)
        Me.XrPanel1.Name = "XrPanel1"
        Me.XrPanel1.SizeF = New System.Drawing.SizeF(204.7244!, 82.6772!)
        Me.XrPanel1.StylePriority.UseBorders = False
        '
        'XrBarcode
        '
        Me.XrBarcode.AutoModule = True
        Me.XrBarcode.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrBarcode.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "tblBarcode.PrKey")})
        Me.XrBarcode.Font = New System.Drawing.Font("Times New Roman", 8.0!, System.Drawing.FontStyle.Bold)
        Me.XrBarcode.LocationFloat = New DevExpress.Utils.PointFloat(31.42834!, 9.103577!)
        Me.XrBarcode.Name = "XrBarcode"
        Me.XrBarcode.Padding = New DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100.0!)
        Me.XrBarcode.SizeF = New System.Drawing.SizeF(141.8677!, 43.42462!)
        Me.XrBarcode.StylePriority.UseBorders = False
        Me.XrBarcode.StylePriority.UseFont = False
        Me.XrBarcode.StylePriority.UseTextAlignment = False
        Me.XrBarcode.Symbology = Code128Generator1
        Me.XrBarcode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter
        '
        'XrlbItemName
        '
        Me.XrlbItemName.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrlbItemName.CanGrow = False
        Me.XrlbItemName.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "tblBarcode.Item")})
        Me.XrlbItemName.Font = New System.Drawing.Font("Times New Roman", 8.0!)
        Me.XrlbItemName.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 52.5282!)
        Me.XrlbItemName.Name = "XrlbItemName"
        Me.XrlbItemName.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrlbItemName.SizeF = New System.Drawing.SizeF(184.7244!, 23.04543!)
        Me.XrlbItemName.StylePriority.UseBorders = False
        Me.XrlbItemName.StylePriority.UseFont = False
        Me.XrlbItemName.StylePriority.UseTextAlignment = False
        Me.XrlbItemName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'ReportsDS1
        '
        Me.ReportsDS1.DataSetName = "ReportsDS"
        Me.ReportsDS1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'TopMarginBand1
        '
        Me.TopMarginBand1.HeightF = 0.0!
        Me.TopMarginBand1.Name = "TopMarginBand1"
        '
        'BottomMarginBand1
        '
        Me.BottomMarginBand1.HeightF = 0.0!
        Me.BottomMarginBand1.Name = "BottomMarginBand1"
        '
        'XtraPoster2
        '
        Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.Detail, Me.TopMarginBand1, Me.BottomMarginBand1})
        Me.DataMember = "tblBarcode"
        Me.DataSource = Me.ReportsDS1
        Me.Margins = New System.Drawing.Printing.Margins(0, 0, 0, 0)
        Me.PageHeight = 1169
        Me.PageWidth = 827
        Me.PaperKind = System.Drawing.Printing.PaperKind.A4
        Me.ReportPrintOptions.DetailCountOnEmptyDataSource = 56
        Me.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic
        Me.Version = "15.1"
        CType(Me.ReportsDS1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
    Friend WithEvents ReportsDS1 As MASTER_pro.ReportsDS
    Friend WithEvents XrPanel1 As DevExpress.XtraReports.UI.XRPanel
    Friend WithEvents XrBarcode As DevExpress.XtraReports.UI.XRBarCode
    Friend WithEvents XrlbItemName As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents TopMarginBand1 As DevExpress.XtraReports.UI.TopMarginBand
    Friend WithEvents BottomMarginBand1 As DevExpress.XtraReports.UI.BottomMarginBand
End Class
