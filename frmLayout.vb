Imports System.Data.SqlClient
Public Class frmLayout

    Public Shared myConn As New SqlConnection(GV.myConn)
    Private Sub frmLayout_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim Que As String = "SELECT tblOut1.Serial, tblout1.[Date], tblout1.[Time], tblOut1.Customer, tblout1.LaborOverhaul, tblOut1.Transfers," _
                                & " tblOut2.Kind, (CASE WHEN tblOut2.Kind = 'PKG' THEN tblItems.Name + ' **' ELSE tblItems.Name END) AS [Description], tblOut2.Qnty, tblOut2.UnitPrice, tblOut2.Price AS SubTotal," _
                                & " tblOut1.SubTotal AS T_SubTotal, tblOut1.Discount, tblOut1.SaleTax, tblOut1.NetTotal, tblLogin.UserName, tblItems.EnglishName, tblOut1.Currency" _
                                & " FROM tblOut1 LEFT OUTER JOIN tblOut2 ON tblOut1.Serial = tblOut2.Serial" _
                                & " INNER JOIN tblItems ON tblItems.PrKey = tblOut2.Item" _
                                & " INNER JOIN tblLogin ON tblOut1.[User] = tblLogin.Sr"


        If myConn.State = ConnectionState.Closed Then
            myConn.Open()
        End If

        Dim ds As New ReportsDS
        Dim da As New SqlDataAdapter(Que, myConn)
        da.Fill(ds.Tables("tblReceipt"))

        Dim Report As New XtraReceipt()


        Report.DataSource = ds
        Report.DataAdapter = da
        Report.DataMember = "tblReceipt"
        Dim WAmount As String = ""
        Dim vl As Decimal


        Using cmd = New SqlCommand("SELECT SubTotal FROM tblOut1", myConn)
            Using dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read() Then
                    vl = dr(0)
                End If
            End Using
        End Using
        Dim Currency As String = ""
        Try
            WAmount = vl.ToString("N2")


            Currency = ds.Tables("tblReceipt").Rows(0)(17)
            Select Case Currency
                Case "EGP"
                    WAmount = (Numbers.NumericToLiteral.Convert(WAmount, False, "جنيه", "جنيهات")) & " فقط لا غير"
                Case "EUR"
                    WAmount = (Numbers.NumericToLiteral.Convert(WAmount, False, "يورو", "يورو")) & " فقط لا غير"
                Case "USD"
                    WAmount = (Numbers.NumericToLiteral.Convert(WAmount, False, "دولار", "دولارات")) & " فقط لا غير"
                Case "GBP"
                    WAmount = (Numbers.NumericToLiteral.Convert(WAmount, False, "استرليني", "استرليني")) & " فقط لا غير"
                Case "RUB"
                    WAmount = (Numbers.NumericToLiteral.Convert(WAmount, False, "روبل", "روبل")) & " فقط لا غير"
                Case "CHF"
                    WAmount = (Numbers.NumericToLiteral.Convert(WAmount, False, "فرانك", "فرانك")) & " فقط لا غير"
                Case "CNY"
                    WAmount = (Numbers.NumericToLiteral.Convert(WAmount, False, "يوان", "يوان")) & " فقط لا غير"
            End Select


        Catch ex As Exception
        End Try

        Report.XrCurrency.Text = Currency
        Report.LoadLayout("Receipt.repx")
        myConn.Close()

        ReportDesigner1.OpenReport(Report)
    End Sub
End Class