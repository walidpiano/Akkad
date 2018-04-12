Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Drawing.Printing
Imports System.Text
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrinting.Native
Imports DevExpress.XtraReports.UI
Public Class GV
    'Public Shared myConn As String = "Data Source=192.168.1.6\Master;Initial Catalog=MasterPro;Persist Security Info=True;User ID=walid;Password=wwzzaa"
    'Public Shared myConn As String = "Data Source=Walid-PC\Master;Initial Catalog=MasterProMarket;User ID=UserConnect;Password=wwzzaa"
    'Public Shared myConn As String = "Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\DB.mdf;Integrated Security=True"
    Public Shared myConn As String = "Data Source=WALID-PC\MASTER;Initial Catalog=AkkadDB;Integrated Security=True"
    'Public Shared myConn As String = "Data Source=SERVER2-PC\MASTER;Initial Catalog=MasterPro;Persist Security Info=True;User ID=walid;Password=wwzzaa"
    Public Shared MarketName As String = "Akkad Center"
    Public Shared appDemo As Boolean = True
    Public Shared deadLine As Date = #7/7/2018#
    Public Shared Invoices As Integer = 300000
    Public Shared CheckQnty As Boolean = True

    ''''to reset db
    ''    --dbcc checkident ('tblOut1', reseed)

    ''delete from tblcash;
    ''delete from tblCompounds;
    ''delete from tblDebit;
    ''delete from tblIn2;
    ''delete from tblin1;
    ''delete from tblOut2;
    ''delete from tblOut1;
    ''delete from tblInInvoice;
    ''delete from tblRate;
    ''delete from tblVendors;
    ''delete from tblLogin;

    Public Shared Sub GenerateSignlePageReport(ByVal report As XtraReport)
        'Dim sumHeight As Single = 0
        'report.CreateDocument()
        'Dim pageSettings As XtraPageSettingsBase = report.PrintingSystem.PageSettings
        '    XtraPageSettingsBase.ApplyPageSettings(pageSettings, PaperKind.Custom, New Size(pageSettings.Bounds.Width,
        ''                                                                                    dim ))
    End Sub

End Class

