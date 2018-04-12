<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class XtraPoster3
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
        Me.FormattingRule1 = New DevExpress.XtraReports.UI.FormattingRule()
        Me.Detail = New DevExpress.XtraReports.UI.DetailBand()
        Me.XrPanel1 = New DevExpress.XtraReports.UI.XRPanel()
        Me.XrLabel1 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrBarcode = New DevExpress.XtraReports.UI.XRBarCode()
        Me.XrlbItemName = New DevExpress.XtraReports.UI.XRLabel()
        Me.TopMarginBand1 = New DevExpress.XtraReports.UI.TopMarginBand()
        Me.BottomMarginBand1 = New DevExpress.XtraReports.UI.BottomMarginBand()
        Me.ReportsDS1 = New MASTER_pro.ReportsDS()
        CType(Me.ReportsDS1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'FormattingRule1
        '
        Me.FormattingRule1.Condition = "[PrKey]=''"
        '
        '
        '
        Me.FormattingRule1.Formatting.Visible = DevExpress.Utils.DefaultBoolean.[False]
        Me.FormattingRule1.Name = "FormattingRule1"
        '
        'Detail
        '
        Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrPanel1})
        Me.Detail.Dpi = 254.0!
        Me.Detail.HeightF = 123.0!
        Me.Detail.MultiColumn.ColumnWidth = 350.012!
        Me.Detail.MultiColumn.Layout = DevExpress.XtraPrinting.ColumnLayout.AcrossThenDown
        Me.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnWidth
        Me.Detail.Name = "Detail"
        Me.Detail.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254.0!)
        Me.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'XrPanel1
        '
        Me.XrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrPanel1.CanGrow = False
        Me.XrPanel1.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel1, Me.XrBarcode, Me.XrlbItemName})
        Me.XrPanel1.Dpi = 254.0!
        Me.XrPanel1.LocationFloat = New DevExpress.Utils.PointFloat(10.41797!, 1.000007!)
        Me.XrPanel1.Name = "XrPanel1"
        Me.XrPanel1.SizeF = New System.Drawing.SizeF(339.594!, 122.0!)
        Me.XrPanel1.StylePriority.UseBorders = False
        '
        'XrLabel1
        '
        Me.XrLabel1.AutoWidth = True
        Me.XrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel1.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "tblBarcode.Price", "{0:$0.00}")})
        Me.XrLabel1.Dpi = 254.0!
        Me.XrLabel1.Font = New System.Drawing.Font("Times New Roman", 9.0!, System.Drawing.FontStyle.Bold)
        Me.XrLabel1.LocationFloat = New DevExpress.Utils.PointFloat(18.00002!, 44.99998!)
        Me.XrLabel1.Name = "XrLabel1"
        Me.XrLabel1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.XrLabel1.SizeF = New System.Drawing.SizeF(125.099!, 41.57198!)
        Me.XrLabel1.StylePriority.UseBorders = False
        Me.XrLabel1.StylePriority.UseFont = False
        Me.XrLabel1.StylePriority.UseTextAlignment = False
        Me.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrLabel1.WordWrap = False
        '
        'XrBarcode
        '
        Me.XrBarcode.AutoModule = True
        Me.XrBarcode.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrBarcode.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "tblBarcode.PrKey")})
        Me.XrBarcode.Dpi = 254.0!
        Me.XrBarcode.Font = New System.Drawing.Font("Times New Roman", 7.0!, System.Drawing.FontStyle.Bold)
        Me.XrBarcode.LocationFloat = New DevExpress.Utils.PointFloat(5.00001!, 0.0!)
        Me.XrBarcode.Module = 5.08!
        Me.XrBarcode.Name = "XrBarcode"
        Me.XrBarcode.Padding = New DevExpress.XtraPrinting.PaddingInfo(25, 25, 0, 0, 254.0!)
        Me.XrBarcode.SizeF = New System.Drawing.SizeF(331.4895!, 84.36711!)
        Me.XrBarcode.StylePriority.UseBorders = False
        Me.XrBarcode.StylePriority.UseFont = False
        Me.XrBarcode.StylePriority.UseTextAlignment = False
        Me.XrBarcode.Symbology = Code128Generator1
        Me.XrBarcode.Text = "123456789012"
        Me.XrBarcode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        '
        'XrlbItemName
        '
        Me.XrlbItemName.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrlbItemName.CanGrow = False
        Me.XrlbItemName.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "tblBarcode.Item")})
        Me.XrlbItemName.Dpi = 254.0!
        Me.XrlbItemName.Font = New System.Drawing.Font("Times New Roman", 6.0!)
        Me.XrlbItemName.LocationFloat = New DevExpress.Utils.PointFloat(5.000006!, 78.95198!)
        Me.XrlbItemName.Name = "XrlbItemName"
        Me.XrlbItemName.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.XrlbItemName.SizeF = New System.Drawing.SizeF(331.4894!, 43.04804!)
        Me.XrlbItemName.StylePriority.UseBorders = False
        Me.XrlbItemName.StylePriority.UseFont = False
        Me.XrlbItemName.StylePriority.UseTextAlignment = False
        Me.XrlbItemName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'TopMarginBand1
        '
        Me.TopMarginBand1.Dpi = 254.0!
        Me.TopMarginBand1.HeightF = 4.0!
        Me.TopMarginBand1.Name = "TopMarginBand1"
        '
        'BottomMarginBand1
        '
        Me.BottomMarginBand1.Dpi = 254.0!
        Me.BottomMarginBand1.HeightF = 0.0!
        Me.BottomMarginBand1.Name = "BottomMarginBand1"
        '
        'ReportsDS1
        '
        Me.ReportsDS1.DataSetName = "ReportsDS"
        Me.ReportsDS1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'XtraPoster3
        '
        Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.Detail, Me.TopMarginBand1, Me.BottomMarginBand1})
        Me.DataMember = "tblBarcode"
        Me.DataSource = Me.ReportsDS1
        Me.DisplayName = "Barcodes"
        Me.Dpi = 254.0!
        Me.FormattingRuleSheet.AddRange(New DevExpress.XtraReports.UI.FormattingRule() {Me.FormattingRule1})
        Me.Margins = New System.Drawing.Printing.Margins(0, 0, 4, 0)
        Me.PageHeight = 2970
        Me.PageWidth = 2100
        Me.PaperKind = System.Drawing.Printing.PaperKind.A4
        Me.ReportPrintOptions.DetailCountOnEmptyDataSource = 102
        Me.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter
        Me.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic
        Me.ShowPrintMarginsWarning = False
        Me.Version = "15.1"
        CType(Me.ReportsDS1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents FormattingRule1 As DevExpress.XtraReports.UI.FormattingRule
    Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
    Friend WithEvents XrPanel1 As DevExpress.XtraReports.UI.XRPanel
    Friend WithEvents TopMarginBand1 As DevExpress.XtraReports.UI.TopMarginBand
    Friend WithEvents BottomMarginBand1 As DevExpress.XtraReports.UI.BottomMarginBand
    Friend WithEvents XrBarcode As DevExpress.XtraReports.UI.XRBarCode
    Friend WithEvents XrlbItemName As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel1 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents ReportsDS1 As MASTER_pro.ReportsDS
End Class
