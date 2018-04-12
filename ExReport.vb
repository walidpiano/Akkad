Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI


Public Class ExReport
    Public Shared myConn As New SqlConnection(GV.myConn)
    Public Shared Sub getReceipt(ByVal Serial As String, ByVal Print As Boolean)
        If Not Serial = "" Then

            Dim Query As String = "DECLARE @Serial BIGINT = " & Val(Serial) & ";" _
                                  & " DECLARE @Date DATETIME = (SELECT [Date] FROM tblOut1 WHERE Serial = @Serial);" _
                                  & " DECLARE @Time DATETIME = (SELECT [Time] FROM tblOut1 WHERE Serial = @Serial);" _
                                  & " DECLARE @Curr NCHAR(3) = (SELECT Currency FROM tblOut1 WHERE Serial = @Serial);" _
                                  & " SELECT tblOut1.Serial, tblout1.[Date], tblout1.[Time], tblOut1.Customer, tblout1.LaborOverhaul, " _
                                  & " tblOut1.Transfers, tblOut2.Kind, " _
                                  & " (CASE WHEN tblOut2.Kind = 'PKG' THEN tblItems.Name + ' **' ELSE tblItems.Name END) AS [Description], " _
                                  & " tblOut2.Qnty, tblOut2.UnitPrice," _
                                  & " (CASE WHEN tblOut2.Kind = 'PKG' THEN tblItems.Name + ' **' ELSE tblItems.Name END) AS [Description], " _
                                  & " tblOut2.Qnty, " _
                                  & " ((tblOut2.UnitPrice * tblOut2.Qnty) - tblOut2.Discount) AS SubTotal," _
                                  & " tblOut1.SubTotal AS T_SubTotal, tblOut1.Discount, tblOut1.SaleTax, tblOut1.NetTotal, " _
                                  & " tblLogin.UserName, tblItems.EnglishName, tblOut1.Currency" _
                                  & " FROM tblOut1 LEFT OUTER JOIN tblOut2 ON tblOut1.Serial = tblOut2.Serial" _
                                  & " INNER JOIN tblItems ON tblItems.PrKey = tblOut2.Item" _
                                  & " INNER JOIN tblLogin ON tblOut1.[User] = tblLogin.Sr" _
                                  & " WHERE tblOut1.Serial = @Serial;"


            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If

            Dim ds As New ReportsDS
            Dim da As New SqlDataAdapter(Query, myConn)
            da.Fill(ds.Tables("tblReceipt"))

            Dim Report As New XtraReceipt


            Report.DataSource = ds
            Report.DataAdapter = da
            Report.DataMember = "tblReceipt"
            
            Report.LoadLayout("Receipt.repx")

            Dim tool As ReportPrintTool = New ReportPrintTool(Report)

            Report.CreateDocument()
            If Print Then
                Report.Print()
            Else
                Report.ShowPreview()
            End If

            myConn.Close()


        End If
    End Sub
End Class
