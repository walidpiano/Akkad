Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrinting.Native
Public Class frmCashierReport

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub frmCashierReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim FillQuery As String = "SELECT UserName FROM tblLogin;"

        Using cmd = New SqlCommand(FillQuery, frmMain.myConn)
            If frmMain.myConn.State = ConnectionState.Closed Then
                frmMain.myConn.Open()
            End If
            Dim adt As New SqlDataAdapter
            Dim ds As New DataSet()
            adt.SelectCommand = cmd
            adt.Fill(ds)
            adt.Dispose()

            cbCashiers.DataSource = ds.Tables(0)
            cbCashiers.DisplayMember = "UserName"
            cbCashiers.Text = Nothing
            cbCashiers.Focus()
            frmMain.myConn.Close()

        End Using
        tmFrom.Value = Today & " 00:00"
        tmTill.Value = Today & " 23:59"

    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles btnCurrency.Click
        Cursor = Cursors.WaitCursor
        Dim Cashier As String = ""
        If cbCashiers.Text <> "" Then
            Cashier = " AND (tblLogin.UserName = N'" & cbCashiers.Text & "')"
        End If

        Dim timFrom, timTill As String
        timFrom = dailyDateFrom.Value.ToString("MM/dd/yyyy") & " " & tmFrom.Value.ToString("HH:mm") & ":00.000"
        timTill = dailyDateTill.Value.ToString("MM/dd/yyyy") & " " & tmTill.Value.ToString("HH:mm") & ":59.999"
        Dim PaymentType As Integer
        PaymentType = rgPayment.SelectedIndex

        Dim Que As String = "SELECT tblOut1.[Date], tblOut1.[Time], tblOut1.Serial," _
                            & " (CASE WHEN tblOut1.Currency = 'USD' AND tblOut1.Visa2 = 0 THEN tblOut1.pUSD - tblOut1.NetTotal ELSE 0 END) AS USD," _
                            & " (CASE WHEN tblOut1.Visa1 = 0 THEN (CASE WHEN tblOut1.Currency = 'EGP' THEN tblOut1.pEGP - tblOut1.NetTotal ELSE tblOut1.pEGP END) ELSE 0 END) AS EGP," _
                            & " (CASE WHEN tblOut1.Currency = 'EUR' THEN tblOut1.pEUR - tblOut1.NetTotal ELSE 0 END) AS EUR," _
                            & " (CASE WHEN tblOut1.Currency = 'GBP' THEN tblOut1.pGBP - tblOut1.NetTotal ELSE 0 END) AS GBP," _
                            & " (CASE WHEN tblOut1.Currency = 'RUB' THEN tblOut1.pRUB - tblOut1.NetTotal ELSE 0 END) AS RUB," _
                            & " (CASE WHEN tblOut1.Currency = 'CHF' THEN tblOut1.pCHF - tblOut1.NetTotal ELSE 0 END) AS CHF," _
                            & " (CASE WHEN tblOut1.Currency = 'CNY' THEN tblOut1.pCNY - tblOut1.NetTotal ELSE 0 END) AS CNY," _
                            & " (CASE WHEN tblOut1.Visa1 = 1 THEN (CASE WHEN tblOut1.Currency = 'EGP' THEN tblOut1.pEGP - tblOut1.NetTotal ELSE tblOut1.pEGP END) ELSE 0 END) AS VisaEGP," _
                            & " (CASE WHEN tblOut1.Currency = 'USD' AND tblOut1.Visa2 = 1 THEN tblOut1.pUSD - tblOut1.NetTotal ELSE 0 END) AS VisaUSD," _
                            & " tblLogin.UserName AS [User]" _
                            & " FROM tblOut1 INNER JOIN tblLogin ON tblOut1.[User] = tblLogin.Sr" _
                            & " WHERE (tblOut1.Debit = " & PaymentType & ") AND (tblOut1.[Date] + tblOut1.[Time] BETWEEN '" & timFrom & "' AND '" & timTill & "')" & Cashier _
                            & " ORDER BY tblOut1.[Date], tblOut1.[Time];"


        If frmMain.myConn.State = ConnectionState.Closed Then
            frmMain.myConn.Open()
        End If

        Dim ds As New ReportsDS
        Dim da As New SqlDataAdapter(Que, frmMain.myConn)
        da.Fill(ds.Tables("XtraCashier"))

        Dim Report As New XtraCashier


        Report.DataSource = ds
        Report.DataAdapter = da
        Report.DataMember = "XtraCashier"
        Report.XrFrom.Text = dailyDateFrom.Value.ToString("yyyy/MM/dd") & " " & tmFrom.Value.ToString("HH:mm")
        Report.XrTo.Text = dailyDateTill.Value.ToString("yyyy/MM/dd") & " " & tmTill.Value.ToString("HH:mm")
        If cbCashiers.Text <> "" Then
            Report.XrlCashier.Visible = True
            Report.XrCashier.Visible = True
            Report.XrCashier.Text = cbCashiers.Text
        Else
            Report.XrlCashier.Visible = False
            Report.XrCashier.Visible = False
            Report.XrCashier.Text = ""
        End If

        Dim tool As ReportPrintTool = New ReportPrintTool(Report)

        Report.CreateDocument()
        Cursor = Cursors.Default
        Report.ShowPreview()
        frmMain.myConn.Close()

    End Sub

    Private Sub btnItems_Click(sender As Object, e As EventArgs) Handles btnItems.Click
        Cursor = Cursors.WaitCursor
        Dim Cashier As String = ""
        If cbCashiers.Text <> "" Then
            Cashier = " AND (tblLogin.UserName = N'" & cbCashiers.Text & "')"
        End If

        Dim timFrom, timTill As String
        timFrom = dailyDateFrom.Value.ToString("MM/dd/yyyy") & " " & tmFrom.Value.ToString("HH:mm") & ":00.000"
        timTill = dailyDateTill.Value.ToString("MM/dd/yyyy") & " " & tmTill.Value.ToString("HH:mm") & ":59.999"
        Dim PaymentType As Integer
        PaymentType = rgPayment.SelectedIndex


        Dim Que As String = "SELECT tblOut1.[Date], tblOut1.[Time], tblOut1.Serial, tblItems.Serial AS Code, tblItems.Name AS Item," _
                            & " ((CASE WHEN tblOut2.UnitPrice * dbo.CurCurrency(tblOut1.[Date], tblOut1.[Time], tblOut1.Currency) < 1 THEN 1" _
                            & " ELSE tblOut2.UnitPrice * dbo.CurCurrency(tblOut1.[Date], tblOut1.[Time], tblOut1.Currency) END) / dbo.CurCurrency(tblOut1.[Date], tblOut1.[Time], tblOut1.Currency)) AS UnitPrice, " _
                            & " tblOut2.Discount, tblOut2.Qnty, tblLogin.UserName AS [User]" _
                            & " FROM tblOut2 INNER JOIN tblOut1 ON tblOut1.Serial = tblOut2.Serial" _
                            & " INNER JOIN tblItems ON tblOut2.Item = tblItems.PrKey" _
                            & " INNER JOIN tblLogin ON tblOut1.[User] = tblLogin.Sr" _
                            & " WHERE (tblOut1.Debit = " & PaymentType & ") AND (tblOut1.[Date] + tblOut1.[Time] BETWEEN '" & timFrom & "' AND '" & timTill & "')" & Cashier _
                            & " ORDER BY tblOut1.[Date], tblOut1.[Time];"


        If frmMain.myConn.State = ConnectionState.Closed Then
            frmMain.myConn.Open()
        End If

        Dim ds As New ReportsDS
        Dim da As New SqlDataAdapter(Que, frmMain.myConn)
        da.Fill(ds.Tables("XtraCashierItems"))

        Dim Report As New XraCashierItems


        Report.DataSource = ds
        Report.DataAdapter = da
        Report.DataMember = "XtraCashierItems"
        Report.XrFrom.Text = dailyDateFrom.Value.ToString("yyyy/MM/dd") & " " & tmFrom.Value.ToString("HH:mm")
        Report.XrTo.Text = dailyDateTill.Value.ToString("yyyy/MM/dd") & " " & tmTill.Value.ToString("HH:mm")
        If cbCashiers.Text <> "" Then
            Report.XrlCashier.Visible = True
            Report.XrCashier.Visible = True
            Report.XrCashier.Text = cbCashiers.Text
        Else
            Report.XrlCashier.Visible = False
            Report.XrCashier.Visible = False
            Report.XrCashier.Text = ""
        End If

        Dim tool As ReportPrintTool = New ReportPrintTool(Report)

        Report.CreateDocument()
        Cursor = Cursors.Default
        Report.ShowPreview()
        frmMain.myConn.Close()

    End Sub

End Class