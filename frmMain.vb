Imports System.Text
Imports System.Security.Cryptography
Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions
Imports DevExpress.XtraReports.UI
Imports System.Data.SqlClient
Imports System.Globalization

Public Class frmMain

    'Public Shared myConn As New SqlConnection(My.Settings.DBConnectionString)
    Public Shared myConn As New SqlConnection(GV.myConn)
    Dim counter As Integer = 0
    Dim ValidQnty As Boolean = False

    'to convert numbers to letters
    Public Function AmountInWords(ByVal nAmount As String, Optional ByVal wAmount _
                   As String = vbNullString, Optional ByVal nSet As Object = Nothing) As String
        'Let's make sure entered value is numeric
        If Not IsNumeric(nAmount) Then Return "Please enter numeric values only."

        Dim tempDecValue As String = String.Empty : If InStr(nAmount, ".") Then _
            tempDecValue = nAmount.Substring(nAmount.IndexOf("."))
        nAmount = Replace(nAmount, tempDecValue, String.Empty)

        Try
            Dim intAmount As Long = nAmount
            If intAmount > 0 Then
                nSet = IIf((intAmount.ToString.Trim.Length / 3) _
                 > (CLng(intAmount.ToString.Trim.Length / 3)), _
                  CLng(intAmount.ToString.Trim.Length / 3) + 1, _
                   CLng(intAmount.ToString.Trim.Length / 3))
                Dim eAmount As Long = Microsoft.VisualBasic.Left(intAmount.ToString.Trim, _
                  (intAmount.ToString.Trim.Length - ((nSet - 1) * 3)))
                Dim multiplier As Long = 10 ^ (((nSet - 1) * 3))

                Dim Ones() As String = _
                {"", "One", "Two", "Three", _
                  "Four", "Five", _
                  "Six", "Seven", "Eight", "Nine"}
                Dim Teens() As String = {"", _
                "Eleven", "Twelve", "Thirteen", _
                  "Fourteen", "Fifteen", _
                  "Sixteen", "Seventeen", "Eighteen", "Nineteen"}
                Dim Tens() As String = {"", "Ten", _
                "Twenty", "Thirty", _
                  "Forty", "Fifty", "Sixty", _
                  "Seventy", "Eighty", "Ninety"}
                Dim HMBT() As String = {"", "", _
                "Thousand", "Million", _
                  "Billion", "Trillion", _
                  "Quadrillion", "Quintillion"}

                intAmount = eAmount

                Dim nHundred As Integer = intAmount \ 100 : intAmount = intAmount Mod 100
                Dim nTen As Integer = intAmount \ 10 : intAmount = intAmount Mod 10
                Dim nOne As Integer = intAmount \ 1

                If nHundred > 0 Then wAmount = wAmount & _
                Ones(nHundred) & " Hundred " 'This is for hundreds                
                If nTen > 0 Then 'This is for tens and teens
                    If nTen = 1 And nOne > 0 Then 'This is for teens 
                        wAmount = wAmount & Teens(nOne) & " "
                    Else 'This is for tens, 10 to 90
                        wAmount = wAmount & Tens(nTen) & IIf(nOne > 0, "-", " ")
                        If nOne > 0 Then wAmount = wAmount & Ones(nOne) & " "
                    End If
                Else 'This is for ones, 1 to 9
                    If nOne > 0 Then wAmount = wAmount & Ones(nOne) & " "
                End If
                wAmount = wAmount & HMBT(nSet) & " "
                wAmount = AmountInWords(CStr(CLng(nAmount) - _
                  (eAmount * multiplier)).Trim & tempDecValue, wAmount, nSet - 1)
            Else
                If Val(nAmount) = 0 Then nAmount = nAmount & _
                tempDecValue : tempDecValue = String.Empty
                If (Math.Round(Val(nAmount), 2) * 100) > 0 Then wAmount = _
                  Trim(AmountInWords(CStr(Math.Round(Val(nAmount), 2) * 100), _
                  wAmount.Trim & " Pesos And ", 1)) & " Cents"
            End If
        Catch ex As Exception
            MessageBox.Show("Error Encountered: " & ex.Message, _
              "Convert Numbers To Words", _
              MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return "!#ERROR_ENCOUNTERED"
        End Try

        'Trap null values
        If IsNothing(wAmount) = True Then wAmount = String.Empty Else wAmount = _
          IIf(InStr(wAmount.Trim.ToLower, "pesos"), _
          wAmount.Trim, wAmount.Trim & " Pesos")

        'Display the result
        Return wAmount
    End Function

    Private Function CreateEAN() As String
        Dim Sr As String = RandomString(5, 5)
        Sr = "3000000" & Sr
        Dim ssr() As Char = Sr.ToCharArray()
        Dim CheckSum As Integer = 0
        CheckSum += Val(ssr(0))
        CheckSum += Val(ssr(1)) * 3
        CheckSum += Val(ssr(2))
        CheckSum += Val(ssr(3)) * 3
        CheckSum += Val(ssr(4))
        CheckSum += Val(ssr(5)) * 3
        CheckSum += Val(ssr(6))
        CheckSum += Val(ssr(7)) * 3
        CheckSum += Val(ssr(8))
        CheckSum += Val(ssr(9)) * 3
        CheckSum += Val(ssr(10))
        CheckSum += Val(ssr(11)) * 3

        If CheckSum > 9 Then
            CheckSum = CheckSum.ToString.Substring(CheckSum.ToString.Length - 2)
        End If

        Select Case CheckSum
            Case 0 To 10
                CheckSum = 10 - CheckSum
            Case 11 To 20
                CheckSum = 20 - CheckSum
            Case 21 To 30
                CheckSum = 30 - CheckSum
            Case 31 To 40
                CheckSum = 40 - CheckSum
            Case 41 To 50
                CheckSum = 50 - CheckSum
            Case 51 To 60
                CheckSum = 60 - CheckSum
            Case 61 To 70
                CheckSum = 70 - CheckSum
            Case 71 To 80
                CheckSum = 80 - CheckSum
            Case 81 To 90
                CheckSum = 90 - CheckSum
            Case Else
                CheckSum = 100 - CheckSum
        End Select

        Return Sr & CheckSum.ToString
    End Function

    Public Function FindSerial(ByVal Serial As String) As String
        Dim result As String = ""
        Dim Query As String = "SELECT Serial FROM tblItems WHERE Serial LIKE '%" & Serial & "%'" _
                              & " UNION ALL" _
                              & " SELECT PackageSerial AS Serial FROM tblItems WHERE PackageSerial LIKE '%" & Serial & "%'" _
                              & " UNION ALL" _
                              & " SELECT Code AS Serial FROM tblMultiCodes WHERE Code LIKE '%" & Serial & "%'" _
                              & " ORDER BY Serial;"

        Using cmd = New SqlCommand(Query, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Dim adt As New SqlDataAdapter
            Dim ds As New DataSet()
            adt.SelectCommand = cmd
            adt.Fill(ds)
            adt.Dispose()

            frmFind.ListBoxControl1.DataSource = ds.Tables(0)
            frmFind.ListBoxControl1.DisplayMember = "Serial"
            myConn.Close()

            If frmFind.ListBoxControl1.ItemCount = 1 Then
                result = frmFind.ListBoxControl1.GetItemText(0)
            ElseIf frmFind.ListBoxControl1.ItemCount > 1 Then
                frmFind.ShowDialog()
                result = frmFind.Result
            End If
            Return result
        End Using
    End Function
    Private Sub Authorize()
        Dim Kind As String = GlobalVariables.authority
        Select Case Kind
            Case "Admin", "Developer"
                'KryptonPage2.Visible = True
                KryptonPage3.Visible = True
                KryptonPage4.Visible = True
                KryptonPage5.Visible = True
                KryptonPage6.Visible = True
                KryptonPage7.Visible = True
                KryptonPage8.Visible = True
                '   KryptonPage9.Visible = True
                KryptonPage10.Visible = True
                KryptonPage11.Visible = True

                KryptonRibbonGroupButton14.Visible = True
                KryptonRibbonGroupButton15.Visible = True
                KryptonRibbonGroupButton16.Visible = True
                KryptonRibbonGroupButton17.Visible = True

                ' KryptonRibbonGroupButton2.Visible = True
                KryptonRibbonGroupButton3.Visible = True
                KryptonRibbonGroupButton4.Visible = True
                KryptonRibbonGroupButton5.Visible = True
                itemReport.Visible = True
                outReport.Visible = True
                inReport.Visible = True
                itemMonitor.Visible = True
                ' totalMonitor.Visible = True
                dailyMonitor.Visible = True
                btnPassKey.Visible = True


                iDgv.Columns(2).ReadOnly = False
                iDgv.Columns(3).ReadOnly = False

            Case "User"
                iDgv.Columns(2).ReadOnly = True
                iDgv.Columns(3).ReadOnly = True
            Case "Cashier"
                '   KryptonPage2.Visible = False
                KryptonPage3.Visible = False
                KryptonPage4.Visible = False
                KryptonPage5.Visible = False
                KryptonPage6.Visible = False
                KryptonPage7.Visible = False
                KryptonPage8.Visible = False
                '     KryptonPage9.Visible = False
                'KryptonPage10.Visible = False
                KryptonPage11.Visible = False

                KryptonRibbonGroupButton14.Visible = False
                KryptonRibbonGroupButton15.Visible = False
                KryptonRibbonGroupButton16.Visible = False
                KryptonRibbonGroupButton17.Visible = False

                ' KryptonRibbonGroupButton2.Visible = False
                KryptonRibbonGroupButton3.Visible = False
                KryptonRibbonGroupButton4.Visible = False
                KryptonRibbonGroupButton5.Visible = False
                itemReport.Visible = False
                outReport.Visible = False
                inReport.Visible = False
                itemMonitor.Visible = False
                '    totalMonitor.Visible = False
                'dailyMonitor.Visible = False
                btnPassKey.Visible = True

                iDgv.Columns(2).ReadOnly = True
                iDgv.Columns(3).ReadOnly = True
        End Select
    End Sub

    'to auto fill the rate
    Public Sub AutoRate(ByVal Dat As Date)

        'check if the rate is found
        Dim rateFound As Boolean
        Using cmd = New SqlCommand("SELECT * FROM tblRate WHERE [Day] = '" & Dat.ToString("MM/dd/yyyy") & "'", myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Using dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read() Then
                    rateFound = True
                Else
                    rateFound = False
                End If
            End Using
        End Using
        If rateFound = False Then
            Using cmd = New SqlCommand("INSERT INTO tblRate (Day, Rate) VALUES ('" & Dat.ToString("MM/dd/yyyy") & "', '9')", frmMain.myConn)
                cmd.ExecuteNonQuery()
            End Using
        End If
        myConn.Close()
    End Sub

    '' TO SUPRESS THE TEXT IN DATAGRIDS
    Sub TextNumberKeypress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Put the validations for your cell
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
    End Sub
    Private Sub RibbonClear()
        KryptonRibbonGroupButton1.Checked = False
        KryptonRibbonGroupButton2.Checked = False
        KryptonRibbonGroupButton3.Checked = False
        KryptonRibbonGroupButton4.Checked = False
        KryptonRibbonGroupButton5.Checked = False
        itemReport.Checked = False
        outReport.Checked = False
        inReport.Checked = False
        itemMonitor.Checked = False
        totalMonitor.Checked = False
        dailyMonitor.Checked = False
    End Sub

    Private Sub AlterCodes(ByVal Code As String, ByVal Codes As String)

        Codes = Codes.Replace(vbNewLine, "")
        Codes = Codes.Trim
        If Codes <> "" Then
            If Codes.Substring(Codes.Length - 1, 1) = ";" Then
                Codes = Codes.Substring(0, Codes.Length - 1)
            End If
        End If
        Dim CodesRange As String = "'" & Codes.Replace(";", "', '") & "'"
        Dim CheckQuery1 As String = "SELECT COUNT(*) FROM tblItems WHERE Serial IN (" & CodesRange & ");"
        Dim CheckQuery2 As String = "SELECT COUNT(*) FROM tblMultiCodes WHERE Code IN (" & CodesRange & ") " _
                                    & " AND NOT Item = (SELECT PrKey FROM tblItems WHERE Serial = '" & Code & "')"
        Dim Query As String = "DECLARE @PrKey INT = (SELECT PrKey FROM tblItems WHERE Serial = '" & Code & "');" _
                              & " DELETE FROM tblMultiCodes WHERE Item = @PrKey; "

        Dim rng() As String = Codes.Split(";")
        If rng.Count <> 0 And Not Codes = "" Then
            Query += "INSERT INTO tblMultiCodes (Item, Code) VALUES"

            For x As Integer = 0 To rng.Count - 1
                If rng(x).Trim <> "" Then
                    Query += " (@PrKey, '" & rng(x).Trim & "'),"
                End If
            Next
            Query = Query.Substring(0, Query.Length - 1)
            Query += ";"
        End If

        Using cmd = New SqlCommand(CheckQuery1, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            If cmd.ExecuteScalar <> 0 Then
                MsgBox("! ÊÃœ √ﬂÊ«œ „” Œœ„… „‰ ﬁ»·")
                myConn.Close()
                Exit Sub
            End If
            cmd.CommandText = CheckQuery2
            If cmd.ExecuteScalar <> 0 Then
                MsgBox("! ÊÃœ √ﬂÊ«œ „” Œœ„… „‰ ﬁ»·")
                myConn.Close()
                Exit Sub
            End If

            cmd.CommandText = Query
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox("!›‘·  ⁄·„Ì… Õ›Ÿ √ﬂÊ«œ ≈÷«›Ì…")
            End Try
            myConn.Close()
        End Using

    End Sub

    Private Sub SearchCash()
        If myConn.State = ConnectionState.Closed Then
            myConn.Open()
        End If
        Dim Query As String = "SELECT tblCash.PrKey, tblCash.[Type], CONVERT(NVARCHAR(12), tblCash.[Date], 103) AS [Date], CONVERT(NVARCHAR(5), tblCash.[Time], 108) AS [Time], tblCash.Qnty, tblCash.Indication, tblLogin.UserName AS [User]" _
                              & " FROM tblCash LEFT OUTER JOIN tblLogin" _
                              & " ON tblCash.[User] = tblLogin.Sr" _
                              & " WHERE tblCash.[Date] = '" & DateTimePicker1.Value.ToString("MM/dd/yyyy") & "'" _
                              & " ORDER BY tblCash.[Date], tblCash.[Time]"
        CashDGV.Rows.Clear()
        Using cmd = New SqlCommand(Query, myConn)
            Using dr As SqlDataReader = cmd.ExecuteReader

                Using dt As New DataTable
                    dt.Load(dr)
                    For x As Integer = 0 To dt.Rows.Count - 1
                        CashDGV.Rows.Add(dt.Rows(x)(0), dt.Rows(x)(1), dt.Rows(x)(2), dt.Rows(x)(3), dt.Rows(x)(4), dt.Rows(x)(5), dt.Rows(x)(6))

                    Next
                End Using

            End Using
        End Using
        myConn.Close()
    End Sub

    Private Sub CheckQnty()
        Dim Query As String
        Dim Qnty As Decimal
        ValidQnty = True
        If myConn.State = ConnectionState.Closed Then
            myConn.Open()
        End If
        For Each line As DataGridViewRow In oDgv.Rows

            Query = "SELECT tblItems.PrKey, tblItems.Serial, tblItems.Name, ttlIn.Total_In, COALESCE(ttlOut.Total_Out , 0) AS Total_Out,  (ttlIn.Total_In - COALESCE(ttlOut.Total_Out , 0)) AS Net_Amount" _
                & " FROM tblItems" _
                & " LEFT OUTER JOIN" _
                & " (" _
                & " SELECT tblIn2.Item, SUM(tblIn2.Qnty) AS Total_In FROM tblIn2" _
                & " GROUP BY tblIn2.Item" _
                & " ) ttlIn" _
                & " ON tblItems.PrKey = ttlIn.Item" _
                & " LEFT OUTER JOIN" _
                & " (" _
                & " SELECT tblOut2.Item, SUM(tblOut2.Qnty) AS Total_Out FROM tblOut2" _
                & " GROUP BY tblOut2.Item" _
                & " ) ttlOut" _
                & " ON tblItems.PrKey = ttlOut.Item" _
                & " WHERE tblItems.Serial = '" & line.Cells(2).Value & "' AND tblItems.Name = N'" & line.Cells(3).Value & "'"


            Using cmd = New SqlCommand(Query, myConn)
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() Then
                        Qnty = dt(5)
                    End If
                End Using
            End Using

            If Qnty < line.Cells(0).Value Then
                MessageBox.Show("The entered quantity is more than the stocked amount. Please check!", "Invalid Qnty", MessageBoxButtons.OK, MessageBoxIcon.Information)
                oDgv.ClearSelection()

                line.Selected = True
                ValidQnty = False
                Exit For
            End If
        Next

        myConn.Close()

    End Sub


    Private Sub OutTotalize()
        Dim tSales As Decimal = 0
        'Dim tDisc As Decimal = 0

        For x As Integer = 0 To oDgv.RowCount - 1
            tSales += oDgv.Rows(x).Cells(5).Value
        Next
        tbTotal.Text = tSales

    End Sub

    Private Sub inTotalize()
        Dim tSales As Decimal = 0

        For x As Integer = 0 To iDgv.RowCount - 1
            tSales += Val(iDgv.Rows(x).Cells(4).Value)
        Next

        iTotalSales.Text = tSales
        iRest.Text = (tSales - Val(itPaid.Text))
    End Sub


    Private Sub VendorsName()
        Using cmd = New SqlCommand("SELECT Name FROM tblVendors ORDER BY Name", myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Dim adt As New SqlDataAdapter
            Dim ds As New DataSet()
            adt.SelectCommand = cmd
            adt.Fill(ds)
            adt.Dispose()

            vSearch.DataSource = ds.Tables(0)
            vSearch.DisplayMember = "Name"
            vSearch.Text = Nothing


            myConn.Close()

        End Using
    End Sub

    Private Sub Notification(ByVal Notify As String)
        Me.Timer1.Enabled = False
        counter = 0
        lbMonitor.Text = Notify

        lbMonitor.Visible = True
        Me.Timer1.Enabled = True

    End Sub


    'Public Function GenerateHash(ByVal SourceText As String, Optional Type As Integer = 1) As String
    '    'Create an encoding object to ensure the encoding standard for the source text
    '    Dim Ue As New UnicodeEncoding()
    '    'Retrieve a byte array based on the source text
    '    Dim ByteSourceText() As Byte = Ue.GetBytes(SourceText)
    '    'Instantiate an MD5 Provider object
    '    Dim Md5 As New MD5CryptoServiceProvider()
    '    Dim SHA1 As New SHA1CryptoServiceProvider()

    '    'Compute the hash value from the source
    '    Dim ByteHash() As Byte
    '    If Type = 1 Then
    '        ByteHash = Md5.ComputeHash(ByteSourceText)
    '    Else
    '        ByteHash = SHA1.ComputeHash(ByteSourceText)
    '    End If
    '    'And convert it to String format for return
    '    Return Convert.ToBase64String(ByteHash)
    'End Function

    Private Sub frmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Control = True AndAlso e.KeyCode = Keys.S AndAlso KryptonDockableNavigator1.SelectedIndex = 0 Then
            Dim str As String = ""
            str = iVendor.SelectedIndex.ToString & ";" & iSerial.Text & ";" & itPaid.Text & "$"

            For x As Integer = 0 To iDgv.RowCount - 1
                str += iDgv.Rows(x).Cells(0).Value & ";" & iDgv.Rows(x).Cells(1).Value & ";" & iDgv.Rows(x).Cells(2).Value & ";" _
                    & iDgv.Rows(x).Cells(3).Value & ";" & iDgv.Rows(x).Cells(4).Value & "$"
            Next
            My.Settings.InTemp = str
            My.Settings.InTempDate = ieDate.Value
            My.Settings.InTempTime = ieTime.Value
            My.Settings.Save()
        ElseIf e.Control = True AndAlso e.KeyCode = Keys.R AndAlso My.Settings.InTemp <> "" Then
            Dim str As String = My.Settings.InTemp
            ieDate.Value = My.Settings.InTempDate
            ieTime.Value = My.Settings.InTempTime
            Dim data() As String = str.Split("$")
            Dim data2() As String
            iDgv.Rows.Clear()
            For x As Integer = 0 To data.Count - 2
                data2 = data(x).Split(";")
                If x = 0 Then
                    iVendor.SelectedIndex = data2(0)
                    iSerial.Text = data2(1)
                    itPaid.Text = data2(2)
                Else
                    iDgv.Rows.Add(data2(0), data2(1), data2(2), data2(3), data2(4))
                End If
            Next
            inTotalize()
        End If
    End Sub

    Private Sub frmMain_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.Control = False Then
            al1.Visible = False
        End If
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        KryptonDockableNavigator1.SelectedIndex = 0

        Dim dddddddd As Date
        Using cmd = New SqlCommand("SELECT MAX([Date]) AS dt FROM tblOut1;", myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Using dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read() Then
                    If Not IsDBNull(dr(0)) Then
                        dddddddd = dr(0)
                    Else
                        dddddddd = Today
                    End If
                Else
                    dddddddd = Today
                End If
            End Using
            myConn.Close()
        End Using

        If GV.appDemo = True And dddddddd >= GV.deadLine Then
            Using cmd = New SqlCommand("DELETE FROM tblMaster", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                cmd.ExecuteNonQuery()
                myConn.Close()
            End Using
            MsgBox("Error 179!")
            Application.Exit()
        End If

        If GV.appDemo = True Then
            Dim invNu As Integer
            Dim query As String = "SELECT COUNT(*) FROM tblOut1"
            Using cmd = New SqlCommand(query, myConn)
                myConn.Open()
                invNu = cmd.ExecuteScalar
                myConn.Close()
            End Using

            If invNu > GV.Invoices Then
                MsgBox("Error 172!")
                Application.Exit()
            End If
        End If

        ToolStripStatusLabel3.Text = GlobalVariables.user

        If KryptonDockableNavigator1.SelectedIndex = 0 Then
            Using cmd = New SqlCommand("SELECT Name, Sr FROM tblVendors ORDER BY Name", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                iVendor.DataSource = ds.Tables(0)
                iVendor.DisplayMember = "Name"
                iVendor.ValueMember = "Sr"
                iVendor.Text = Nothing

                iVendor.Focus()
                myConn.Close()

            End Using
        End If
        'clear the customers combobox
        oCustomer.Text = Nothing

        'fill the invoice combobox

        Using cmd = New SqlCommand("SELECT DISTINCT TOP(200) Serial FROM tblOut1 ORDER BY Serial DESC", myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Dim adt As New SqlDataAdapter
            Dim ds As New DataSet()
            adt.SelectCommand = cmd
            adt.Fill(ds)
            adt.Dispose()

            krSerial.DataSource = ds.Tables(0)
            krSerial.DisplayMember = "Serial"
            krSerial.Text = Nothing

            myConn.Close()

        End Using

        'loading a dummy report
        Dim ds2 As New ReportsDS
        Dim da2 As New SqlDataAdapter("SELECT ''", myConn)
        da2.Fill(ds2.Tables("tblDummy"))

        Dim rep As New crDummy
        rep.SetDataSource(ds2.Tables("tblDummy"))
        CrystalReportViewer1.ReportSource = rep

        CrystalReportViewer1.ReportSource = Nothing
        CrystalReportViewer1.Refresh()

        'load the virtual rate
        Call AutoRate(Today)
        RadioGroup1.SelectedIndex = 0

        Call Authorize()

    End Sub

    Private Sub KryptonRibbonGroupButton13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonRibbonGroupButton13.Click
        frmLogin.Close()
        frmLogin.UsernameTextBox.Text = Nothing
        frmLogin.PasswordTextBox.Text = Nothing
        frmLogin.OpenCashier = False
        frmLogin.ShowDialog()
        ToolStripStatusLabel3.Text = GlobalVariables.user
        Call Authorize()
    End Sub

    Private Sub KryptonRibbonGroupButton14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonRibbonGroupButton14.Click
        frmSignup.Show()
    End Sub

    Private Sub KryptonRibbonGroupButton15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonRibbonGroupButton15.Click
        frmManageUsers.Show()
    End Sub

    Private Sub KryptonRibbonGroupButton16_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles KryptonRibbonGroupButton16.Click
        Dim fbd As New FolderBrowserDialog()
        Dim path As String
        Dim dR As DialogResult = fbd.ShowDialog

        If dR = Windows.Forms.DialogResult.OK Then
            path = fbd.SelectedPath
            Try
                System.IO.File.Copy("DB.sdf", String.Format(path & "\DB.sdf", Date.Today), True)
            Catch ex As Exception
                MsgBox(ex)
            End Try
        End If

    End Sub

    Private Sub KryptonRibbonGroupButton17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonRibbonGroupButton17.Click
        frmImport.Show()

    End Sub

    Private Sub KryptonRibbonGroupButton18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonRibbonGroupButton18.Click
        frmSupport.Show()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If counter < 7 Then
            If lbMonitor.Visible = True Then
                lbMonitor.Visible = False
            Else
                lbMonitor.Visible = True
            End If
            counter += 1
        Else
            lbMonitor.Visible = False
        End If
    End Sub

    Private Sub KryptonButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles vClear.Click
        vName.Text = Nothing
        vPhone1.Clear()
        vPhone2.Clear()
        vEmail.Text = Nothing
        vAddress.Text = Nothing
        vNotes.Text = Nothing
        vName.Focus()

        Call Notification("Cleared!")
    End Sub

    Private Sub rdVAddNew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdVAddNew.CheckedChanged
        If rdVAddNew.Checked = True Then
            vAdd.Text = "Õ›Ÿ"
            vSearch.Visible = False

        ElseIf rdVModify.Checked = True Then
            vAdd.Text = " ⁄œÌ·"
            vSearch.Visible = True
        Else
            vAdd.Text = "Õ–›"
            vSearch.Visible = True
        End If
    End Sub

    Private Sub rdVModify_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdVModify.CheckedChanged
        If rdVAddNew.Checked = True Then
            vAdd.Text = "Õ›Ÿ"
            vSearch.Visible = False

        ElseIf rdVModify.Checked = True Then
            vAdd.Text = " ⁄œÌ·"
            vSearch.Visible = True
        Else
            vAdd.Text = "Õ–›"
            vSearch.Visible = True
        End If
    End Sub

    Private Sub rdVDelete_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdVDelete.CheckedChanged
        If rdVAddNew.Checked = True Then
            vAdd.Text = "Õ›Ÿ"
            vSearch.Visible = False

        ElseIf rdVModify.Checked = True Then
            vAdd.Text = " ⁄œÌ·"
            vSearch.Visible = True
        Else
            vAdd.Text = "Õ–›"
            vSearch.Visible = True
        End If
    End Sub

    Private Sub mstSPhone1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mstSPhone1.Click
        vPhone1.Clear()
        vPhone1.Focus()

    End Sub

    Private Sub ButtonSpecAny1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSpecAny1.Click
        vPhone2.Clear()
        vPhone2.Focus()
    End Sub

    Private Sub vName_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles vName.PreviewKeyDown
        If e.KeyCode = Keys.Down Or e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            vPhone1.Focus()
            vPhone1.SelectAll()
        End If
    End Sub

    Private Sub vEmail_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles vEmail.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            vAddress.Focus()
            vAddress.SelectAll()
        ElseIf e.KeyCode = Keys.Up Then
            vPhone2.Focus()
            vPhone2.SelectAll()
        End If
    End Sub

    Private Sub vAddress_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles vAddress.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            vNotes.Focus()
            vNotes.SelectAll()
        ElseIf e.KeyCode = Keys.Up Then
            vEmail.Focus()
            vEmail.SelectAll()
        End If
    End Sub

    Private Sub vNotes_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles vNotes.PreviewKeyDown
        If e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            vAdd.Focus()
        ElseIf e.KeyCode = Keys.Up Then
            vAddress.Focus()
            vAddress.SelectAll()

        End If
    End Sub

    Private Sub vNotes_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles vNotes.TextChanged

    End Sub

    Private Sub vAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles vAdd.Click
        If vAdd.Text = "Õ›Ÿ" Then
            'check if the name is duplicate
            Dim dup As Boolean

            Using cmd = New SqlCommand("SELECT * FROM tblVendors WHERE Name = N'" & vName.Text & "'", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        dup = True
                    Else
                        dup = False
                    End If
                End Using

                myConn.Close()

            End Using

            If vName.Text = "" Then

                MessageBox.Show("ÌÃ» ≈œŒ«· «·«”‹‹„!!", "Empty Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                vName.Focus()
            ElseIf dup = True Then
                MessageBox.Show("«·«”„ «·–Ì  „ ≈œŒ«·Â „ÊÃÊœ »«·›⁄·!", "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                vName.Focus()
                vName.SelectAll()

            Else

                'Save new record

                Dim query As String
                Dim ph As String = vPhone1.Text.Replace("(", "").Trim.Replace(")", "").Replace("-", "").Replace(" ", "")
                If ph.Length <> 0 Then
                    ph = vPhone1.Text
                End If

                Dim ph2 As String = vPhone2.Text.Replace("(", "").Trim.Replace(")", "").Replace("-", "").Replace(" ", "")
                If ph2.Length <> 0 Then
                    ph2 = vPhone2.Text
                End If

                query = "INSERT INTO tblVendors (Name, Phone1, Phone2, [E-mail], Address, Notes)" _
                    & " VALUES (N'" & vName.Text & "', '" & ph & "', '" & ph2 & "', '" & vEmail.Text & "','" & vAddress.Text & "', '" & vNotes.Text & "')"
                Using cmd = New SqlCommand(query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    cmd.ExecuteNonQuery()
                    myConn.Close()

                End Using
                Call Notification("'" & vName.Text & " ' added!")
                vName.Text = Nothing
                vPhone1.Clear()
                vPhone2.Clear()
                vEmail.Text = Nothing
                vAddress.Text = Nothing
                vNotes.Text = Nothing
                vName.Focus()
            End If
        ElseIf vAdd.Text = " ⁄œÌ·" Then
            'check if the name is duplicate
            Dim dup As Boolean

            Using cmd = New SqlCommand("SELECT * FROM tblVendors WHERE Name = N'" & vName.Text & "'", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        dup = True
                    Else
                        dup = False
                    End If
                End Using

                myConn.Close()

            End Using

            If vName.Text = "" Then
                MessageBox.Show("ÌÃ» ≈œŒ«· «·«”‹‹„!!", "Empty Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                vName.Focus()
            ElseIf dup = True And vName.Text <> vSearch.Text Then
                MessageBox.Show("«·«”„ «·–Ì  „ ≈œŒ«·Â „ÊÃÊœ »«·›⁄·!", "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                vName.Focus()
                vName.SelectAll()

            Else

                'Modify new record

                Dim query As String

                Dim ph As String = vPhone1.Text.Replace("(", "").Trim.Replace(")", "").Replace("-", "").Replace(" ", "")
                If ph.Length <> 0 Then
                    ph = vPhone1.Text
                End If

                Dim ph2 As String = vPhone2.Text.Replace("(", "").Trim.Replace(")", "").Replace("-", "").Replace(" ", "")
                If ph2.Length <> 0 Then
                    ph2 = vPhone2.Text
                End If

                query = "UPDATE tblVendors SET Name = N'" & vName.Text & "', Phone1 = '" & ph & "', Phone2 = '" & ph2 & "', [E-mail] = '" & vEmail.Text & "', Address = '" & vAddress.Text & "', Notes = '" & vNotes.Text & "'" _
                    & " WHERE Name = N'" & vSearch.Text & "'"

                Using cmd = New SqlCommand(query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    cmd.ExecuteNonQuery()
                    myConn.Close()

                End Using
                Call Notification("'" & vName.Text & " ' modified!")
                Call VendorsName()
            End If
        ElseIf vAdd.Text = "Õ–›" Then
            Dim dia As DialogResult
            dia = MessageBox.Show("Â·  —Ìœ »«· √ﬂÌœ Õ–› '" & vSearch.Text & "'ø", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If dia = Windows.Forms.DialogResult.Yes Then
                Try
                    Using cmd = New SqlCommand("DELETE FROM tblVendors WHERE Name = N'" & vSearch.Text & "'", myConn)
                        If myConn.State = ConnectionState.Closed Then
                            myConn.Open()
                        End If
                        cmd.ExecuteNonQuery()

                        myConn.Close()

                    End Using

                    Call Notification("'" & vSearch.Text & "' deleted!")
                    Call VendorsName()
                Catch ex As Exception
                    MsgBox("!·« Ì„ﬂ‰ Õ–› Â–« «·«”„ ÕÌÀ √‰Â „” Œœ„ ›Ì ⁄„Ì«  ‘—«¡")
                End Try
            End If

        End If
    End Sub

    Private Sub vSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles vSearch.SelectedIndexChanged
        If vSearch.Text <> "" Then
            Using cmd = New SqlCommand("SELECT * FROM tblVendors WHERE Name = N'" & vSearch.Text & "'", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim ds As New DataSet()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(ds, "tblVendors")
                Dim dt As New DataTable
                dt = ds.Tables(0)
                Try
                    vName.Text = dt.Rows(0)(1).ToString
                    vPhone1.Text = dt.Rows(0)(2).ToString
                    vPhone2.Text = dt.Rows(0)(3).ToString
                    vEmail.Text = dt.Rows(0)(4).ToString
                    vAddress.Text = dt.Rows(0)(6).ToString
                    vNotes.Text = dt.Rows(0)(5).ToString
                Catch ex As Exception

                End Try

                myConn.Close()
            End Using
        Else
            vName.Text = Nothing
            vPhone1.Clear()
            vPhone2.Clear()
            vEmail.Text = Nothing
            vAddress.Text = Nothing
            vNotes.Text = Nothing
        End If
    End Sub

    Private Sub vSearch_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles vSearch.VisibleChanged
        If vSearch.Visible = True Then
            Call VendorsName()
        End If
    End Sub

    Private Sub KryptonDockableNavigator1_SelectedPageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles KryptonDockableNavigator1.SelectedPageChanged
        Call RibbonClear()
        If KryptonDockableNavigator1.SelectedIndex = 0 Then

            KryptonRibbonGroupButton1.Checked = True
            Using cmd = New SqlCommand("SELECT Sr, Name FROM tblVendors ORDER BY Name", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                iVendor.DataSource = ds.Tables(0)
                iVendor.DisplayMember = "Name"
                iVendor.ValueMember = "Sr"
                iVendor.Text = Nothing


                myConn.Close()

            End Using

            'fill the serials and items combos
            Try
                Dim FillQuery As String = "SELECT PrKey, Serial, Name FROM tblItems" _
                                                          & " UNION ALL" _
                                                          & " SELECT tblItems.PrKey, tblMultiCodes.Code AS Serial, tblItems.Name" _
                                                          & " FROM tblMultiCodes INNER JOIN tblItems ON tblMultiCodes.Item = tblItems.PrKey" _
                                                          & " ORDER BY Name;"

                Using cmd = New SqlCommand(FillQuery, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Dim adt As New SqlDataAdapter
                    Dim ds As New DataSet()
                    adt.SelectCommand = cmd
                    adt.Fill(ds)
                    adt.Dispose()

                    iItem.DataSource = ds.Tables(0)
                    iItem.DisplayMember = "Serial"
                    iItem.ValueMember = "PrKey"

                    iItemName.DataSource = ds.Tables(0)
                    iItemName.DisplayMember = "Name"
                    iItemName.Text = Nothing

                    myConn.Close()

                End Using
            Catch ex As Exception

            End Try
            iItemName.Text = ""
            iItem.Text = ""
        ElseIf KryptonDockableNavigator1.SelectedIndex = 1 Then

            KryptonRibbonGroupButton2.Checked = True

            'Fill the items
            Dim FillQuery As String = "SELECT PrKey, Serial, Name FROM tblItems" _
                                                          & " UNION ALL" _
                                                          & " SELECT tblItems.PrKey, tblMultiCodes.Code AS Serial, tblItems.Name" _
                                                          & " FROM tblMultiCodes INNER JOIN tblItems ON tblMultiCodes.Item = tblItems.PrKey" _
                                                          & " ORDER BY Name;"


            Using cmd = New SqlCommand(FillQuery, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                oItemSerial.DataSource = ds.Tables(0)
                oItemSerial.DisplayMember = "Serial"
                oItemSerial.ValueMember = "PrKey"
                oItemSerial.Text = Nothing


                oItemName.DataSource = ds.Tables(0)
                oItemName.DisplayMember = "Name"
                oItemName.Text = Nothing

                myConn.Close()

            End Using

            'Fill the customers names
            Using cmd = New SqlCommand("SELECT DISTINCT Customer FROM tblOut1 ORDER BY Customer", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                oCustomer.DataSource = ds.Tables(0)
                oCustomer.DisplayMember = "Customer"
                oCustomer.Text = Nothing

                myConn.Close()
            End Using

        ElseIf KryptonDockableNavigator1.SelectedIndex = 2 Then
            KryptonRibbonGroupButton3.Checked = True

        ElseIf KryptonDockableNavigator1.SelectedIndex = 3 Then
            KryptonRibbonGroupButton4.Checked = True

            'fill the items search
            Dim FillQuery As String = "SELECT PrKey, Serial, Name FROM tblItems" _
                                                          & " UNION ALL" _
                                                          & " SELECT tblItems.PrKey, tblMultiCodes.Code AS Serial, tblItems.Name" _
                                                          & " FROM tblMultiCodes INNER JOIN tblItems ON tblMultiCodes.Item = tblItems.PrKey" _
                                                          & " ORDER BY Name;"

            Using cmd = New SqlCommand(FillQuery, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                iiSearch.DataSource = ds.Tables(0)
                iiSearch.DisplayMember = "Serial"
                iiSearch.ValueMember = "PrKey"
                iiSearch.Text = Nothing

                myConn.Close()

            End Using

        ElseIf KryptonDockableNavigator1.SelectedIndex = 4 Then
            KryptonRibbonGroupButton5.Checked = True
            cSum.Focus()
        ElseIf KryptonDockableNavigator1.SelectedIndex = 5 Then
            inReport.Checked = True
            Using cmd = New SqlCommand("SELECT Name, Sr FROM tblVendors ORDER BY Name", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                isVendor.DataSource = ds.Tables(0)
                isVendor.DisplayMember = "Name"
                isVendor.ValueMember = "Sr"
                isVendor.Text = Nothing
                isVendor.Focus()

                myConn.Close()

            End Using
        ElseIf KryptonDockableNavigator1.SelectedIndex = 6 Then
            outReport.Checked = True

            Using cmd = New SqlCommand("SELECT ID, Customer FROM tblCustomers ORDER BY Customer", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                osCustomer.DataSource = ds.Tables(0)
                osCustomer.DisplayMember = "Customer"
                osCustomer.ValueMember = "ID"
                osCustomer.Text = Nothing
                osCustomer.Focus()

                myConn.Close()

            End Using

            'load item names
            Using cmd = New SqlCommand("SELECT Serial, Name FROM tblItems ORDER BY Name", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                osItem.DataSource = ds.Tables(0)
                osItem.DisplayMember = "Name"
                osItem.Text = Nothing

                osCode.DataSource = ds.Tables(0)
                osCode.DisplayMember = "Serial"
                osCode.Text = Nothing

                myConn.Close()

            End Using

        ElseIf KryptonDockableNavigator1.SelectedIndex = 7 Then
            itemReport.Checked = True

            'fil the vendors list
            Using cmd = New SqlCommand("SELECT Name, PrKey, Serial FROM tblItems ORDER BY Name", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                siItem.DataSource = ds.Tables(0)
                siItem.DisplayMember = "Name"
                siItem.ValueMember = "PrKey"

                siCode.DataSource = ds.Tables(0)
                siCode.DisplayMember = "Serial"
                siCode.ValueMember = "PrKey"

                siCode.Text = Nothing

                siItem.Focus()

                myConn.Close()
                siItem.Text = ""
            End Using
        ElseIf KryptonDockableNavigator1.SelectedIndex = 8 Then
            itemMonitor.Checked = True
            'fill the items
            Dim FillQuery As String = "SELECT PrKey, Serial FROM tblItems" _
                                                          & " UNION ALL" _
                                                          & " SELECT tblItems.PrKey, tblMultiCodes.Code AS Serial" _
                                                          & " FROM tblMultiCodes INNER JOIN tblItems ON tblMultiCodes.Item = tblItems.PrKey" _
                                                          & " ORDER BY Serial;"

            Using cmd = New SqlCommand(FillQuery, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                monItem.DataSource = ds.Tables(0)
                monItem.DisplayMember = "Serial"
                monItem.ValueMember = "PrKey"
                monItem.Text = Nothing
                monItem.Focus()

                myConn.Close()

            End Using

        ElseIf KryptonDockableNavigator1.SelectedIndex = 9 Then
            totalMonitor.Checked = True
        ElseIf KryptonDockableNavigator1.SelectedIndex = 10 Then
            dailyMonitor.Checked = True

            Dim FillQuery As String = "SELECT UserName FROM tblLogin;"

            Using cmd = New SqlCommand(FillQuery, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                cbCashiers.DataSource = ds.Tables(0)
                cbCashiers.DisplayMember = "UserName"
                'cbCashiers.ValueMember = "PrKey"
                cbCashiers.Text = Nothing
                cbCashiers.Focus()
                myConn.Close()

            End Using
            tmFrom.Value = Today & " 00:00"
            tmTill.Value = Today & " 23:59"



        End If
    End Sub

    Private Sub iItem_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles iItem.KeyDown

        If Not iItem.Text = "" And e.KeyCode = Keys.Enter Then

            Dim ne As Boolean
            Dim NQuery2 As String = "SELECT tblItems.Name, tblItems.Price FROM tblItems LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                      & " WHERE tblItems.Serial = '" & iItem.Text & "' OR tblMultiCodes.Code = '" & iItem.Text & "';"

            Using cmd = New SqlCommand(NQuery2, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() Then
                        ne = False
                        iItemName.Text = dt(0).ToString
                        KryptonPanel22.Visible = False
                        lblSellingPrice.Text = "”⁄— «·»Ì⁄: " & dt(1)
                        lblSellingPrice.Visible = True
                    Else
                        ne = True
                        KryptonPanel22.Visible = True
                        iItemName.Text = ""
                        lblSellingPrice.Visible = False
                    End If
                End Using
                myConn.Close()
            End Using
        Else
            lblSellingPrice.Visible = False
        End If

        'check if the grid has the same item

        If e.KeyCode = Keys.Enter Then
            'check if the grid has the same item
            For x As Integer = 0 To iDgv.RowCount - 1
                If (iDgv.Rows(x).Cells(0).Value.ToString.ToUpper = iItem.Text.ToUpper) AndAlso (iDgv.Rows(x).Cells(1).Value.ToString.ToUpper = iItemName.Text.ToUpper) Then
                    MessageBox.Show("Â–« «·’‰›  „ »«·›⁄· ≈œŒ«·Â ›Ì «·›« Ê—… „‰ ﬁ»·!!", "Duplicate Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    iDgv.ClearSelection()
                    iDgv.Rows(x).Selected = True
                    iItem.Focus()
                    iItem.SelectAll()
                End If
            Next
        End If

        If e.Control = True And e.KeyCode = Keys.K And Not iItem.Text = "" Then
            iItem.Text = FindSerial(iItem.Text)
        End If
    End Sub

    Private Sub iItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles iItem.LostFocus

        If Not iItem.Text = "" Then
            Dim ne As Boolean
            Dim NQuery2 As String = "SELECT tblItems.Name, tblItems.Price FROM tblItems LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                      & " WHERE tblItems.Serial = '" & iItem.Text & "' OR tblMultiCodes.Code = '" & iItem.Text & "';"

            Using cmd = New SqlCommand(NQuery2, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() Then
                        ne = False
                        iItemName.Text = dt(0).ToString
                        KryptonPanel22.Visible = False
                        lblSellingPrice.Visible = True
                    Else
                        ne = True
                        KryptonPanel22.Visible = True
                        iItemName.Text = ""
                        lblSellingPrice.Visible = False
                    End If
                End Using
                myConn.Close()
            End Using

        Else
            lblSellingPrice.Visible = False
        End If

    End Sub

    Private Sub iItem_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iItem.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            iItemName.Focus()
        End If
    End Sub

    Private Sub iItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles iItemName.LostFocus
        iItemName.Text = iItemName.Text.ToUpper
        If Not iItemName.Text = "" Then
            Dim ne As Boolean
            Using cmd = New SqlCommand("SELECT Serial FROM tblItems WHERE Name = N'" & iItemName.Text & "'", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() Then
                        ne = False
                        iItem.Text = dt(0).ToString
                        KryptonPanel22.Visible = False
                    Else
                        ne = True
                        KryptonPanel22.Visible = True
                    End If
                End Using
                myConn.Close()

            End Using
        End If
    End Sub

    Private Sub iItemName_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iItemName.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            If nEnglishName.Visible = True Then
                nEnglishName.Focus()
            Else
                iQnty.Focus()
            End If
        End If
    End Sub

    Private Sub iItemName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub iQnty_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles iQnty.GotFocus
        iQnty.SelectAll()
    End Sub

    Private Sub iQnty_KeyDown(sender As Object, e As KeyEventArgs) Handles iQnty.KeyDown
        If e.KeyCode = Keys.Enter And Not iQnty.Text = "" Then
            Dim SC As New MSScriptControl.ScriptControl
            Dim Formula As String = iQnty.Text
            SC.Language = "VBSCRIPT"
            Try
                Dim result As Single = Convert.ToSingle(SC.Eval(Formula))
                iQnty.Text = result
            Catch ex As Exception
                iQnty.Text = "0"
            End Try
        End If
    End Sub

    Private Sub iQnty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles iQnty.KeyPress

        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." And Not Char.IsPunctuation(e.KeyChar) Then
            e.Handled = True
        End If
        If e.KeyChar = "." And iQnty.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If

    End Sub

    Private Sub iQnty_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles iQnty.LostFocus
        If Not iQnty.Text = "" Then
            Dim SC As New MSScriptControl.ScriptControl
            Dim Formula As String = iQnty.Text
            SC.Language = "VBSCRIPT"
            Try
                Dim result As Single = Convert.ToSingle(SC.Eval(Formula))
                iQnty.Text = result
            Catch ex As Exception
                iQnty.Text = "0"
            End Try
        End If
        iValue.Text = Val(iQnty.Text) * Val(iUnitPrice.Text)
    End Sub

    Private Sub iQnty_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iQnty.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Or e.KeyCode = Keys.Down Then
            iUnitPrice.Focus()
        ElseIf e.KeyCode = Keys.Up Then
            iItem.Focus()
            iItem.SelectAll()
        End If
    End Sub

    Private Sub iUnitPrice_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles iUnitPrice.GotFocus
        iUnitPrice.SelectAll()
    End Sub

    Private Sub iUnitPrice_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles iUnitPrice.KeyPress

        If e.KeyChar = "." AndAlso iUnitPrice.Text = "" Then
            iUnitPrice.Text = "0."
            iUnitPrice.Select(2, 0)

        End If

        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And iUnitPrice.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If

    End Sub

    Private Sub iUnitPrice_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles iUnitPrice.LostFocus
        'If Not Val(iUnitPrice.Text) = 0 Then
        '    Try
        '        Dim vlu As Decimal = Val(iQnty.Text) * Val(iUnitPrice.Text)
        '        vlu = Math.Round(vlu, 2, MidpointRounding.AwayFromZero)
        '        iValue.Text = vlu.ToString("G29")
        '    Catch ex As Exception

        '    End Try
        'End If
        'iValue.Text = Val(iQnty.Text) * Val(iUnitPrice.Text)
    End Sub

    Private Sub iUnitPrice_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iUnitPrice.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Or e.KeyCode = Keys.Down Then
            iValue.Focus()
            iValue.SelectAll()
        ElseIf e.KeyCode = Keys.Up Then
            iQnty.Focus()
            iQnty.SelectAll()
        End If
    End Sub

    Private Sub iValue_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles iValue.GotFocus
        iValue.SelectAll()
    End Sub

    Private Sub iValue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles iValue.KeyPress
        If e.KeyChar = "." AndAlso iUnitPrice.Text = "" Then
            iUnitPrice.Text = "0."
            iUnitPrice.Select(2, 0)

        End If

        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And iValue.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If

    End Sub

    Private Sub iValue_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles iValue.LostFocus

        'If Not Val(iValue.Text) = 0 Then
        '    Try
        '        Dim vlu As Decimal = Val(iQnty.Text) * Val(iUnitPrice.Text)
        '        vlu = Math.Round(vlu, 2, MidpointRounding.AwayFromZero)
        '        iValue.Text = vlu.ToString("G29")
        '    Catch ex As Exception

        '    End Try
        'End If
        'If Not Val(iUnitPrice.Text) = 0 Then
        '    Try
        '        Dim vlu As Decimal = Val(iQnty.Text) * Val(iUnitPrice.Text)
        '        vlu = Math.Round(vlu, 2, MidpointRounding.AwayFromZero)
        '        iValue.Text = vlu.ToString("G29")
        '    Catch ex As Exception

        '    End Try
        'End If
        'If Not Val(iUnitPrice.Text) = 0 Then
        '    iUnitPrice.Text = Val(iValue.Text) / Val(iQnty.Text)
        'End If
    End Sub

    Private Sub iValue_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iValue.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Or e.KeyCode = Keys.Down Then
            iItemAdd.Focus()
        ElseIf e.KeyCode = Keys.Up Then
            iUnitPrice.Focus()
            iUnitPrice.SelectAll()
        End If
    End Sub

    Private Sub iValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iValue.TextChanged
        'If iValue.Focused = True Then
        '    iUnitPrice.Text = Val(iValue.Text) / Val(iQnty.Text)
        'End If
        'If iValue.Focused = False Then
        '    If Not Val(iValue.Text) = 0 Then
        '        Try
        '            Dim vlu As Decimal = Val(iValue.Text) / Val(iQnty.Text)
        '            vlu = Math.Round(vlu, 2, MidpointRounding.AwayFromZero)
        '            iUnitPrice.Text = vlu.ToString("G29")
        '        Catch ex As Exception

        '        End Try
        '    End If

        'End If
        'iUnitPrice.Text = Val(iValue.Text) / Val(iQnty.Text)
    End Sub

    Private Sub iItemAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iItemAdd.Click
        If iItem.Text = "" Then
            MessageBox.Show("ÌÃ» ≈œŒ«· «”„ «·’‰›!", "Invalid Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iItem.Focus()
        ElseIf iItemName.Text = "" Then
            MessageBox.Show("ÌÃ» ≈œŒ«· «”„ «·’‰›!", "Invalid Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iItemName.Focus()
        ElseIf Val(iQnty.Text) = 0 Then
            MessageBox.Show("ÌÃ» ≈œŒ«· ﬂ„Ì… ’ÕÌÕ…!", "Invalid Qnty", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iQnty.Focus()
            iQnty.SelectAll()
        ElseIf Val(iUnitPrice.Text) = 0 Then
            MessageBox.Show("ÌÃ» ≈œŒ«· ”⁄— «·ÊÕœ… ’ÕÌÕ!", "Invalid Unit Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iUnitPrice.Focus()
            iUnitPrice.SelectAll()
        ElseIf Val(iValue.Text) = 0 Then
            MessageBox.Show("ÌÃ» ≈œŒ«· ﬁÌ„… ’ÕÌÕ…!", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iValue.Focus()
            iValue.SelectAll()
        Else

            'check if the item doesn't exist
            Dim old As Boolean
            Dim NQuery As String = "SELECT tblItems.* FROM tblItems LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                      & " WHERE tblItems.Serial = '" & iItem.Text & "' OR tblMultiCodes.Code = '" & iItem.Text & "';"

            Using cmd = New SqlCommand(NQuery, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        old = True
                    Else
                        old = False
                    End If
                End Using

                myConn.Close()

            End Using

            If old = False Then
                Dim dia As DialogResult = MessageBox.Show("«”„ «·’‰› «·–Ì ﬁ„  »≈œŒ«·Â €Ì— „ÊÃÊœ. Â·  —Ìœ »«·›⁄· ≈÷«› Âø", "New Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If dia = Windows.Forms.DialogResult.Yes Then
                    'check if the serial is dup
                    Dim dupSr As Boolean
                    Dim NQuery2 As String = "SELECT tblItems.* FROM tblItems LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                      & " WHERE tblItems.Serial = '" & iItem.Text & "' OR tblMultiCodes.Code = '" & iItem.Text & "' OR tblItems.PackageSerial = '" & iItem.Text & "';"

                    Using cmd = New SqlCommand(NQuery2, myConn)
                        If myConn.State = ConnectionState.Closed Then
                            myConn.Open()
                        End If
                        Using dr As SqlDataReader = cmd.ExecuteReader
                            If dr.Read() Then
                                dupSr = True
                            Else
                                dupSr = False
                            End If
                        End Using
                        myConn.Close()
                    End Using

                    If dupSr = True Then
                        MessageBox.Show("«·ﬂÊœ «·–Ì ﬁ„  »≈œŒ«·Â „” Œœ„ „‰ ﬁ»· ·’‰› ¬Œ—!", "Invalid Serial", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        iItem.Focus()
                        iItem.SelectAll()
                    ElseIf Val(nItemPrice.Text) = 0 Then
                        MessageBox.Show("ÌÃ» ≈œŒ«· ”⁄— «·’‰› «·„—«œ ≈÷«› Â!", "Empty Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        nItemPrice.Focus()
                        nItemPrice.SelectAll()
                    Else

                        'check if itemname is dup
                        Using cmd = New SqlCommand("SELECT * FROM tblItems WHERE Name = N'" & iItemName.Text & "'", myConn)
                            If myConn.State = ConnectionState.Closed Then
                                myConn.Open()
                            End If
                            Using dr As SqlDataReader = cmd.ExecuteReader
                                If dr.Read() Then
                                    MessageBox.Show("«·’‰› «·–Ì ﬁ„  »≈œŒ«·Â „” Œœ„ „‰ ﬁ»· »ﬂÊœ '" & dr(1) & "'!", "Invalid Item Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    iItemName.Focus()
                                    iItemName.SelectAll()
                                    Exit Sub
                                End If
                            End Using
                            myConn.Close()
                        End Using

                        ' Add new Item to tblItems
                        Dim Query As String = "INSERT INTO tblItems (Serial, Name, Price, [Minimum], EnglishName) VALUES ('" & iItem.Text.ToUpper & "', N'" & iItemName.Text.ToUpper & "', '" & Val(nItemPrice.Text) & "', '" & Val(KryptonTextBox1.Text) & "', N'" & nEnglishName.Text & "')"
                        Using cmd = New SqlCommand(Query, myConn)
                            If myConn.State = ConnectionState.Closed Then
                                myConn.Open()
                            End If
                            cmd.ExecuteNonQuery()

                            myConn.Close()

                        End Using
                        KryptonPanel22.Visible = False

                        Dim serlll, itmmmm As String
                        serlll = iItem.Text.ToUpper
                        itmmmm = iItemName.Text.ToUpper
                        'fill the serials and items combos
                        Try
                            Dim FillQuery As String = "SELECT PrKey, Serial, Name FROM tblItems" _
                                                      & " UNION ALL" _
                                                      & " SELECT tblItems.PrKey, tblMultiCodes.Code AS Serial, tblItems.Name" _
                                                      & " FROM tblMultiCodes INNER JOIN tblItems ON tblMultiCodes.Item = tblItems.PrKey" _
                                                      & " ORDER BY Name;"

                            Using cmd = New SqlCommand(FillQuery, myConn)
                                If myConn.State = ConnectionState.Closed Then
                                    myConn.Open()
                                End If
                                Dim adt As New SqlDataAdapter
                                Dim ds As New DataSet()
                                adt.SelectCommand = cmd
                                adt.Fill(ds)
                                adt.Dispose()

                                iItem.DataSource = ds.Tables(0)
                                iItem.DisplayMember = "Serial"
                                iItem.ValueMember = "PrKey"

                                iItemName.DataSource = ds.Tables(0)
                                iItemName.DisplayMember = "Name"
                                iItemName.Text = Nothing

                                myConn.Close()

                            End Using
                        Catch ex As Exception

                        End Try
                        iItem.Text = serlll
                        iItemName.Text = itmmmm

                        GoTo record

                    End If

                End If
            Else
record:


                'check if the grid has the same item
                For x As Integer = 0 To iDgv.RowCount - 1
                    If (iDgv.Rows(x).Cells(0).Value.ToString.ToUpper = iItem.Text.ToUpper) AndAlso (iDgv.Rows(x).Cells(1).Value.ToString.ToUpper = iItemName.Text.ToUpper) Then
                        MessageBox.Show("Â–« «·’‰›  „ »«·›⁄· ≈œŒ«·Â ›Ì «·›« Ê—… „‰ ﬁ»·!!", "Duplicate Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        iDgv.ClearSelection()
                        iDgv.Rows(x).Selected = True
                        iItem.Focus()
                        iItem.SelectAll()
                        Return
                    End If
                Next


                'check the logical profit
                Dim Query As String = "SELECT tblItems.Price FROM tblItems" _
                                      & " LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                      & " WHERE tblItems.Serial = '" & iItem.Text & "' OR tblMultiCodes.Code = '" & iItem.Text & "';"
                Using cmd As New SqlCommand(Query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            If dr(0) <= Val(iUnitPrice.Text) Then
                                MessageBox.Show("!ÌÃ» √‰ ÌﬂÊ‰ ”⁄— «·‘—«¡ √ﬁ· „‰ ”⁄— «·»Ì⁄", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                iUnitPrice.Focus()
                                iUnitPrice.SelectAll()
                                Exit Sub
                            End If
                        Else
                            MsgBox("Error")
                        End If
                    End Using
                    myConn.Close()
                End Using



                'Add a new record to the datagrid
                'iVendor.Enabled = False

                Dim theRow As String()
                theRow = New String() {iItem.Text.ToUpper, iItemName.Text.ToUpper, Val(iQnty.Text), Math.Round(Val(iUnitPrice.Text), 2, MidpointRounding.AwayFromZero), Math.Round(Val(iValue.Text), 2, MidpointRounding.AwayFromZero)}

                iDgv.Rows.Add(theRow)

                iDgv.FirstDisplayedScrollingRowIndex = iDgv.RowCount - 1
                iDgv.ClearSelection()

                iDgv.Rows(iDgv.RowCount - 1).Selected = True

                iItem.Text = Nothing
                iItemName.Text = Nothing
                iQnty.Text = Nothing
                iUnitPrice.Text = Nothing
                iValue.Text = Nothing

                iItem.Focus()
                Call Notification("New row added!")

            End If

        End If
    End Sub

    Private Sub iItemRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iItemRemove.Click
        If Not iDgv.RowCount = 0 Then
            Dim dia As DialogResult
            dia = MessageBox.Show("Â·  —Ìœ »«· √ﬂÌœ „”Õ Â–« «·’›ø", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If dia = Windows.Forms.DialogResult.Yes Then
                iDgv.Rows.Remove(iDgv.CurrentRow)

                Notification("Current row removed!")
            End If
        End If
    End Sub

    Private Sub iDgv_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles iDgv.CellClick
        Try
            iDgv.CurrentRow.Selected = True

            iItem.Text = iDgv.CurrentRow.Cells(0).Value
            iItemName.Text = iDgv.CurrentRow.Cells(1).Value
            iQnty.Text = iDgv.CurrentRow.Cells(2).Value
            iUnitPrice.Text = iDgv.CurrentRow.Cells(3).Value
            iValue.Text = iDgv.CurrentRow.Cells(4).Value

        Catch ex As Exception

        End Try
    End Sub

    Private Sub iItemEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iItemEdit.Click

        If iItem.Text = "" Then
            MessageBox.Show("ÌÃ» ≈œŒ«· «”„ «·’‰›!", "Invalid Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iItem.Focus()
        ElseIf iItemName.Text = "" Then
            MessageBox.Show("ÌÃ» ≈œŒ«· «”„ «·’‰›!", "Invalid Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iItemName.Focus()
        ElseIf Val(iQnty.Text) = 0 Then
            MessageBox.Show("ÌÃ» ≈œŒ«· ﬂ„Ì… ’ÕÌÕ…!", "Invalid Qnty", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iQnty.Focus()
            iQnty.SelectAll()
        ElseIf Val(iUnitPrice.Text) = 0 Then
            MessageBox.Show("ÌÃ» ≈œŒ«· ”⁄— «·ÊÕœ… ’ÕÌÕ!", "Invalid Unit Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iUnitPrice.Focus()
            iUnitPrice.SelectAll()
        ElseIf Val(iValue.Text) = 0 Then
            MessageBox.Show("ÌÃ» ≈œŒ«· ﬁÌ„… ’ÕÌÕ…!", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iValue.Focus()
            iValue.SelectAll()
        ElseIf iVendor.Text = "" Then
            MessageBox.Show("ÌÃ» ≈œŒ«· «”„ „Ê“⁄ ’ÕÌÕ!!", "Invalid Vendor", MessageBoxButtons.OK)
            iVendor.Focus()
            iVendor.SelectAll()
        Else

            'check if the item doesn't exist
            Dim old As Boolean
            Dim NQuery As String = "SELECT tblItems.* FROM tblItems LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                  & " WHERE tblItems.Serial = '" & iItem.Text & "' OR tblMultiCodes.Code = '" & iItem.Text & "' OR tblItems.PackageSerial = '" & iItem.Text & "';"


            Using cmd = New SqlCommand(NQuery, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        old = True
                    Else
                        old = False
                    End If
                End Using

                myConn.Close()

            End Using

            If old = False Then
                Dim dia As DialogResult = MessageBox.Show("«”„ «·’‰› «·–Ì ﬁ„  »≈œŒ«·Â €Ì— „ÊÃÊœ. Â·  —Ìœ »«·›⁄· ≈÷«› Âø", "New Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If dia = Windows.Forms.DialogResult.Yes Then
                    'check if the serial is dup
                    Dim dupSr As Boolean
                    Using cmd = New SqlCommand(NQuery, myConn)
                        If myConn.State = ConnectionState.Closed Then
                            myConn.Open()
                        End If
                        Using dr As SqlDataReader = cmd.ExecuteReader
                            If dr.Read() Then
                                dupSr = True
                            Else
                                dupSr = False
                            End If
                        End Using
                        myConn.Close()
                    End Using

                    If dupSr = True Then
                        MessageBox.Show("«·ﬂÊœ «·–Ì ﬁ„  »≈œŒ«·Â „” Œœ„ „‰ ﬁ»· ·’‰› ¬Œ—!", "Invalid Serial", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        iItem.Focus()
                        iItem.SelectAll()
                    ElseIf Val(nItemPrice.Text) = 0 Then
                        MessageBox.Show("ÌÃ» ≈œŒ«· ”⁄— «·’‰› «·„—«œ ≈÷«› Â!", "Empty Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        nItemPrice.Focus()
                        nItemPrice.SelectAll()
                    Else

                        'check if itemname is dup
                        Using cmd = New SqlCommand("SELECT * FROM tblItems WHERE Name = N'" & iItemName.Text & "'", myConn)
                            If myConn.State = ConnectionState.Closed Then
                                myConn.Open()
                            End If
                            Using dr As SqlDataReader = cmd.ExecuteReader
                                If dr.Read() Then
                                    MessageBox.Show("«·’‰› «·–Ì ﬁ„  »≈œŒ«·Â „” Œœ„ „‰ ﬁ»· »ﬂÊœ '" & dr(1) & "'!", "Invalid Item Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    iItemName.Focus()
                                    iItemName.SelectAll()
                                    Exit Sub
                                End If
                            End Using
                            myConn.Close()
                        End Using

                        ' Add new Item to tblItems
                        Dim Query As String = "INSERT INTO tblItems (Serial, Name, Price, [Minimum]) VALUES ('" & iItem.Text.ToUpper & "', N'" & iItemName.Text.ToUpper & "', '" & Val(nItemPrice.Text) & "', '" & Val(KryptonTextBox1.Text) & "')"
                        Using cmd = New SqlCommand(Query, myConn)
                            If myConn.State = ConnectionState.Closed Then
                                myConn.Open()
                            End If
                            cmd.ExecuteNonQuery()

                            myConn.Close()

                        End Using
                        KryptonPanel22.Visible = False

                        Dim serlll, itmmmm As String
                        serlll = iItem.Text.ToUpper
                        itmmmm = iItemName.Text.ToUpper
                        'fill the serials and items combos
                        Try
                            Dim FillQuery As String = "SELECT PrKey, Serial, Name FROM tblItems" _
                                                      & " UNION ALL" _
                                                      & " SELECT tblItems.PrKey, tblMultiCodes.Code AS Serial, tblItems.Name" _
                                                      & " FROM tblMultiCodes INNER JOIN tblItems ON tblMultiCodes.Item = tblItems.PrKey" _
                                                      & " ORDER BY Name;"

                            Using cmd = New SqlCommand(FillQuery, myConn)
                                If myConn.State = ConnectionState.Closed Then
                                    myConn.Open()
                                End If
                                Dim adt As New SqlDataAdapter
                                Dim ds As New DataSet()
                                adt.SelectCommand = cmd
                                adt.Fill(ds)
                                adt.Dispose()

                                iItem.DataSource = ds.Tables(0)
                                iItem.DisplayMember = "Serial"
                                iItem.ValueMember = "PrKey"

                                iItemName.DataSource = ds.Tables(0)
                                iItemName.DisplayMember = "Name"
                                iItemName.Text = Nothing

                                myConn.Close()

                            End Using
                        Catch ex As Exception

                        End Try
                        iItem.Text = serlll
                        iItemName.Text = itmmmm

                        GoTo record

                    End If

                End If
            Else
record:


                'check if the grid has the same item
                Dim cr As Integer = iDgv.CurrentRow.Index

                For x As Integer = 0 To iDgv.RowCount - 1
                    If Not x = cr Then
                        If (iDgv.Rows(x).Cells(0).Value.ToString.ToUpper = iItem.Text.ToUpper) AndAlso (iDgv.Rows(x).Cells(1).Value.ToString.ToUpper = iItemName.Text.ToUpper) Then
                            MessageBox.Show("Â–« «·’‰› Àﬁœ  „ »«·›⁄· ≈œŒ«·Â „‰ ﬁ»·!", "Duplicate Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            iDgv.ClearSelection()
                            iDgv.Rows(x).Selected = True
                            iItem.Focus()
                            iItem.SelectAll()
                            Return
                        End If
                    End If
                Next



                'check the logical profit
                Dim Query As String = "SELECT tblItems.Price FROM tblItems" _
                                     & " LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                     & " WHERE tblItems.Serial = '" & iItem.Text & "' OR tblMultiCodes.Code = '" & iItem.Text & "';"
                Using cmd As New SqlCommand(Query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            If dr(0) <= Val(iUnitPrice.Text) Then
                                MessageBox.Show("!ÌÃ» √‰ ÌﬂÊ‰ ”⁄— «·‘—«¡ √ﬁ· „‰ ”⁄— «·»Ì⁄", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                iUnitPrice.Focus()
                                iUnitPrice.SelectAll()
                                Exit Sub
                            End If
                        Else
                            MsgBox("Error")
                        End If
                    End Using
                    myConn.Close()
                End Using



                'modify the record of the datagrid
                'iVendor.Enabled = False

                Dim theRow As String()
                theRow = New String() {iItem.Text.ToUpper, iItemName.Text.ToUpper, Val(iQnty.Text), Math.Round(Val(iUnitPrice.Text), 2, MidpointRounding.AwayFromZero), Math.Round(Val(iValue.Text), 2, MidpointRounding.AwayFromZero)}

                iDgv.CurrentRow.SetValues(theRow)
                'iDgv.Rows.Add(theRow)

                iDgv.FirstDisplayedScrollingRowIndex = iDgv.RowCount - 1
                iDgv.ClearSelection()

                iDgv.Rows(iDgv.RowCount - 1).Selected = True

                iItem.Text = Nothing
                iItemName.Text = Nothing
                iQnty.Text = Nothing
                iUnitPrice.Text = Nothing
                iValue.Text = Nothing

                iItem.Focus()
                Call Notification("Row Modified!")

            End If

        End If
    End Sub

    Private Sub iDgv_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles iDgv.CellEndEdit
        If iDgv.CurrentCell.ColumnIndex = 2 OrElse iDgv.CurrentCell.ColumnIndex = 3 Then
            Dim result As Decimal = Val(iDgv.Rows(iDgv.CurrentCell.RowIndex).Cells(2).Value) * Val(iDgv.Rows(iDgv.CurrentCell.RowIndex).Cells(3).Value)
            result = Math.Round(result, 2, MidpointRounding.AwayFromZero)
            iDgv.Rows(iDgv.CurrentCell.RowIndex).Cells(4).Value = result
        End If
        inTotalize()
    End Sub

    Private Sub iDgv_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles iDgv.CellLeave
        inTotalize()
    End Sub

    Private Sub iDgv_CellValidated(sender As Object, e As DataGridViewCellEventArgs) Handles iDgv.CellValidated
        If iDgv.CurrentCell.ColumnIndex = 2 OrElse iDgv.CurrentCell.ColumnIndex = 3 Then
            Dim result As Decimal = Val(iDgv.Rows(iDgv.CurrentCell.RowIndex).Cells(2).Value) * Val(iDgv.Rows(iDgv.CurrentCell.RowIndex).Cells(3).Value)
            result = Math.Round(result, 2, MidpointRounding.AwayFromZero)
            iDgv.Rows(iDgv.CurrentCell.RowIndex).Cells(4).Value = result
        End If
        inTotalize()
    End Sub

    Private Sub iDgv_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles iDgv.CellValidating
        Try
            If iDgv.CurrentCell.ColumnIndex = 2 OrElse iDgv.CurrentCell.ColumnIndex = 3 Then
                If Not IsNumeric(e.FormattedValue) OrElse e.FormattedValue = 0 Then
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub iDgv_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles iDgv.CellValueChanged
        Try
            If iDgv.CurrentCell.ColumnIndex = 2 OrElse iDgv.CurrentCell.ColumnIndex = 3 Then
                Dim result As Decimal = Val(iDgv.Rows(iDgv.CurrentCell.RowIndex).Cells(2).Value) * Val(iDgv.Rows(iDgv.CurrentCell.RowIndex).Cells(3).Value)
                result = Math.Round(result, 2, MidpointRounding.AwayFromZero)
                iDgv.Rows(iDgv.CurrentCell.RowIndex).Cells(4).Value = result
            End If
            inTotalize()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub iDgv_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles iDgv.RowHeaderMouseClick
        iDgv.CurrentRow.Selected = True

        iItem.Text = iDgv.CurrentRow.Cells(0).Value
        iItemName.Text = iDgv.CurrentRow.Cells(1).Value
        iQnty.Text = iDgv.CurrentRow.Cells(2).Value
        iUnitPrice.Text = iDgv.CurrentRow.Cells(3).Value
        iValue.Text = iDgv.CurrentRow.Cells(4).Value
    End Sub

    Private Sub iSerial_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iSerial.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Or e.KeyCode = Keys.Down Then
            ieDate.Focus()
        ElseIf e.KeyCode = Keys.Up Then
            iVendor.Focus()
            iVendor.SelectAll()
        End If
    End Sub

    Private Sub iClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iClear.Click
        Dim dia As DialogResult
        dia = MessageBox.Show("Â·  —Ìœ »«· √ﬂÌœ „”Õ ﬂ· «·»Ì«‰«  «·Õ«·Ì…ø", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If dia = Windows.Forms.DialogResult.Yes Then
            iDgv.Rows.Clear()
            iVendor.Text = Nothing
            ' iVendor.Enabled = True
            itPaid.Text = ""
            iSerial.Text = Nothing
            iItem.Text = Nothing
            iItemName.Text = Nothing
            iQnty.Text = Nothing
            iUnitPrice.Text = Nothing
            iValue.Text = Nothing
            ieDate.Value = Today
            ieTime.Value = Now
            iItemEdit.Text = " ’ÕÌÕ"
            iItemAdd.Enabled = True
            iItemRemove.Enabled = True
            Call Notification("Items cleared!")
        End If

    End Sub

    Private Sub iDgv_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles iDgv.RowsAdded
        Call inTotalize()
    End Sub

    Private Sub iDgv_RowsRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles iDgv.RowsRemoved
        Call inTotalize()
    End Sub

    Private Sub iDate_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles ieDate.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            ieTime.Focus()
        End If
    End Sub

    Private Sub iTime_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles ieTime.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            iItem.Focus()
        End If
    End Sub

    Private Sub rdiAdd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdiAdd.CheckedChanged
        If rdiAdd.Checked = True Then
            iAdd.Text = "Õ›Ÿ"
            iSearch.Visible = False


            'clear the items
            iDgv.Rows.Clear()
            iVendor.Text = Nothing
            ' iVendor.Enabled = True
            iSerial.Text = Nothing
            iItem.Text = Nothing
            iItemName.Text = Nothing
            iQnty.Text = Nothing
            iUnitPrice.Text = Nothing
            iValue.Text = Nothing

            ieDate.Value = Today
            ieTime.Value = Now
        ElseIf rdiModify.Checked = True Then
            iAdd.Text = " ⁄œÌ·"
            iSearch.Visible = True

            'fill the combobox
            Using cmd = New SqlCommand("SELECT Serial FROM tblIn1 WHERE Vendor = '" & iVendor.SelectedValue & "' ORDER BY Serial DESC", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                iSearch.DataSource = ds.Tables(0)
                iSearch.DisplayMember = "Serial"
                iSearch.Text = Nothing


                myConn.Close()

            End Using


        Else
            iAdd.Text = "Õ–›"
            iSearch.Visible = True

            Using cmd = New SqlCommand("SELECT Serial FROM tblIn1 WHERE Vendor = '" & iVendor.SelectedValue & "' ORDER BY Serial DESC", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                iSearch.DataSource = ds.Tables(0)
                iSearch.DisplayMember = "Serial"
                iSearch.Text = Nothing

                myConn.Close()


            End Using

        End If
    End Sub

    Private Sub iAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iAdd.Click
        ''''''''''''''''
        'for demo
        If GV.appDemo = True Then
            Using cmd = New SqlCommand("SELECT COUNT(Serial) FROM tblIn1", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        If dr(0) > 700 Then
                            Exit Sub
                        End If
                    End If
                End Using

                myConn.Close()
            End Using
        End If
        '''''''''''''''''''
        If iAdd.Text = "Õ›Ÿ" Then
restart:

            If iVendor.Text = "" Then
                MessageBox.Show("ÌÃ» ≈Œ Ì«— «”„ „Ê“⁄!!", "Vendor", MessageBoxButtons.OK)
                iVendor.Focus()
            ElseIf iSerial.Text = "" Then
                MessageBox.Show("ÌÃ» « Œ Ì«— —ﬁ„ «·›« Ê—…!", "Invoice Serial", MessageBoxButtons.OK)
                iSerial.Focus() '
            ElseIf iDgv.RowCount = 0 Then
                MessageBox.Show("·«  ÊÃœ √’‰«› ··Õ›Ÿ!", "No Items", MessageBoxButtons.OK)
                iItem.Focus()
            ElseIf Val(iRest.Text) < 0 Then
                MessageBox.Show("«·„»·€ «·–Ì  „ ≈œŒ«·Â €Ì— ’ÕÌÕ!", "Wrong Amoun", MessageBoxButtons.OK)
                itPaid.Focus()
                itPaid.SelectAll()
            Else

                'Check if it's debit invoice
                If Val(itPaid.Text) = 0 Or Val(itPaid.Text) < Val(iTotalSales.Text) Then
                    Dim dia As DialogResult
                    dia = MessageBox.Show("·„ Ì „ ≈œŒ«· «·„»·€ »«·ﬂ«„·. Â·  —Ìœ Õ›Ÿ «·›« Ê—… »œ›⁄ ¬Ã·ø", "Unpaid Invoice", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If dia = Windows.Forms.DialogResult.No Then
                        itPaid.Focus()
                        itPaid.SelectAll()
                        Exit Sub
                    End If
                End If

                'check if the rate is found
                Dim rateFound As Boolean
                Using cmd = New SqlCommand("SELECT * FROM tblRate WHERE [Day] = '" & ieDate.Value.ToString("MM/dd/yyyy") & "'", myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            rateFound = True
                        Else
                            rateFound = False
                        End If
                    End Using

                    myConn.Close()
                End Using

                If rateFound = False Then
                    Call AutoRate(ieDate.Value)
                    GoTo restart
                Else

                    'check if the vendor exists
                    Dim existingVendor As Boolean
                    Using cmd = New SqlCommand("SELECT Name FROM tblVendors WHERE Name = N'" & iVendor.Text & "'", myConn)
                        If myConn.State = ConnectionState.Closed Then
                            myConn.Open()
                        End If
                        Using dr As SqlDataReader = cmd.ExecuteReader
                            If Not dr.Read() Then
                                existingVendor = True
                            Else
                                existingVendor = False
                            End If
                        End Using

                        myConn.Close()
                    End Using

                    If existingVendor = False Then
                        'check if the serial is unique
                        Dim unSer As Boolean
                        Using cmd = New SqlCommand("SELECT * FROM tblIn1 WHERE Serial = N'" & iSerial.Text & "'", myConn)
                            If myConn.State = ConnectionState.Closed Then
                                myConn.Open()
                            End If
                            Using dr As SqlDataReader = cmd.ExecuteReader
                                If dr.Read() Then
                                    unSer = False
                                Else
                                    unSer = True
                                End If
                            End Using

                            myConn.Close()
                        End Using

                        If unSer = False Then
                            MessageBox.Show("—ﬁ„ «·›« Ê—… «·–Ì √œŒ· Â „ÊÃÊœ „‰ ﬁ»·!", "Duplicate Serial", MessageBoxButtons.OK)
                            iSerial.Focus()
                            iSerial.SelectAll()
                        Else
                            'start adding the invoice
                            Dim vndr, sril, timm As String
                            Dim usr As Integer
                            Dim qntity, untpric, vlu As Decimal
                            Dim datt As Date
                            vndr = iVendor.SelectedValue
                            'sril = iSerial.Text
                            datt = ieDate.Value
                            timm = ieTime.Value.ToString("HH:mm")
                            'temporary
                            usr = GlobalVariables.ID

                            If myConn.State = ConnectionState.Closed Then
                                myConn.Open()
                            End If


                            Dim Query1, Query2, Query3 As String
                            'insert into the first tblIn1

                            Query1 = "INSERT INTO tblIn1 (Serial, [Date], [Time], Amount, Paid, Rest, [User], Vendor)" _
                                & " VALUES (N'" & iSerial.Text.ToUpper & "','" & datt.ToString("MM/dd/yyyy") & "', '" & timm & "', '" & Val(iTotalSales.Text) & "', '" & Val(itPaid.Text) & "', '" & Val(iRest.Text) & "', '" & usr & "', '" & vndr & "')"

                            Using cmd = New SqlCommand(Query1, myConn)
                                cmd.ExecuteNonQuery()
                            End Using
                            Dim InvSerial As Integer = 0
                            Using cmd = New SqlCommand("SELECT PrKey FROM tblIn1 WHERE Serial = N'" & iSerial.Text.ToUpper & "'", myConn)
                                Using dr As SqlDataReader = cmd.ExecuteReader
                                    If dr.Read() Then
                                        InvSerial = dr(0)
                                    Else
                                        MsgBox("Error")
                                    End If
                                End Using
                            End Using

                            For x As Integer = 0 To iDgv.RowCount - 1
                                'get the item prKey
                                Dim QQuery As String = "SELECT tblItems.PrKey FROM tblItems" _
                                                       & " LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                                       & " WHERE tblItems.Serial = '" & iDgv.Rows(x).Cells(0).Value & "' OR tblMultiCodes.Code = '" & iDgv.Rows(x).Cells(0).Value & "';"
                                Using cmd = New SqlCommand(QQuery, myConn)
                                    Using dr As SqlDataReader = cmd.ExecuteReader
                                        If dr.Read() Then
                                            sril = dr(0)
                                        Else
                                            sril = ""
                                        End If
                                    End Using
                                End Using

                                'compose the query string

                                qntity = iDgv.Rows(x).Cells(2).Value
                                untpric = iDgv.Rows(x).Cells(3).Value
                                vlu = iDgv.Rows(x).Cells(4).Value


                                Query2 = "INSERT INTO tblIn2 (Serial, Sr, Item, Qnty, Sold, UnitPrice, [Value])" _
                                    & " VALUES ('" & InvSerial & "', '" & x + 1 & "', '" & sril & "', '" & qntity & "', '0', '" & untpric & "', '" & vlu & "')"


                                Using cmd = New SqlCommand(Query2, myConn)
                                    cmd.ExecuteNonQuery()
                                End Using

                            Next

                            'inter the paid value
                            Query3 = "INSERT INTO tblDebit (Serial, Amount, [Date], [Time], [User])" _
                                & " VALUES ('" & InvSerial & "', '" & Val(itPaid.Text) & "', '" & datt.ToString("MM/dd/yyyy") & "', '" & timm & "', '" & usr & "')"

                            Using cmd = New SqlCommand(Query3, myConn)
                                cmd.ExecuteNonQuery()
                            End Using


                            myConn.Close()

                            Call Notification("Invoice added")

                            'clear the items
                            iDgv.Rows.Clear()
                            iVendor.Text = Nothing
                            'iVendor.Enabled = True
                            itPaid.Text = ""
                            iSerial.Text = Nothing
                            iItem.Text = Nothing
                            iItemName.Text = Nothing
                            iQnty.Text = Nothing
                            iUnitPrice.Text = Nothing
                            iValue.Text = Nothing
                            ieDate.Value = Today
                            ieTime.Value = Now
                        End If
                    Else
                        MessageBox.Show("·ﬁœ √œŒ·  «”„ „Ê“⁄ €Ì— ’ÕÌÕ!", "Invalid Vendor", MessageBoxButtons.OK)
                        iVendor.Focus()
                        iVendor.SelectAll()
                    End If

                End If
            End If
        ElseIf iAdd.Text = " ⁄œÌ·" Then
            If GlobalVariables.authority = "User" Or GlobalVariables.authority = "Cashier" Then
                MsgBox("!√‰  ·«  „·ﬂ «·’·«ÕÌ… ·· ⁄œÌ· ›Ì «·›« Ê—…")
                Exit Sub
            End If
restart2:

            Dim dia As DialogResult
            dia = MessageBox.Show("Â·  —Ìœ »«· √ﬂÌœ  ⁄œÌ· «·›« Ê—… «·Õ«·Ì…ø", "Modify", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If dia = Windows.Forms.DialogResult.Yes Then
                If iVendor.Text = "" Then
                    MessageBox.Show("ÌÃ» «Œ Ì«— «”„ «·„Ê“⁄!", "Vendor", MessageBoxButtons.OK)
                    iVendor.Focus()
                ElseIf iSerial.Text = "" Then
                    MessageBox.Show("ÌÃ» «Œ Ì«— —ﬁ„ «·›« Ê—…!", "Invoice Serial", MessageBoxButtons.OK)
                    iSerial.Focus() '
                ElseIf iDgv.RowCount = 0 Then
                    MessageBox.Show("·«  ÊÃœ √’‰«› ··Õ›Ÿ!", "No Items", MessageBoxButtons.OK)
                    iItem.Focus()
                ElseIf Val(iRest.Text) < 0 Then
                    MessageBox.Show("«·„»·€ «·–Ì  „ ≈œŒ«·Â €Ì— ’ÕÌÕ!", "Wrong Amoun", MessageBoxButtons.OK)
                    itPaid.Focus()
                    itPaid.SelectAll()
                Else

                    'check if the rate is found
                    Dim rateFound As Boolean
                    Using cmd = New SqlCommand("SELECT * FROM tblRate WHERE [Day] = '" & ieDate.Value.ToString("MM/dd/yyyy") & "'", myConn)
                        If myConn.State = ConnectionState.Closed Then
                            myConn.Open()
                        End If
                        Using dr As SqlDataReader = cmd.ExecuteReader
                            If dr.Read() Then
                                rateFound = True
                            Else
                                rateFound = False
                            End If
                        End Using

                        myConn.Close()

                    End Using

                    If rateFound = False Then
                        Call AutoRate(ieDate.Value)
                        GoTo restart2
                    Else

                        'check if the vendor exists
                        Dim existingVendor As Boolean
                        Using cmd = New SqlCommand("SELECT Name FROM tblVendors WHERE Name = N'" & iVendor.Text & "'", myConn)
                            If myConn.State = ConnectionState.Closed Then
                                myConn.Open()
                            End If
                            Using dr As SqlDataReader = cmd.ExecuteReader
                                If Not dr.Read() Then
                                    existingVendor = True
                                Else
                                    existingVendor = False
                                End If
                            End Using

                            myConn.Close()

                        End Using

                        If existingVendor = False Then


                            'check if the serial is unique
                            Dim unSer As Boolean
                            Using cmd = New SqlCommand("SELECT * FROM tblIn1 WHERE Serial = N'" & iSerial.Text & "'", myConn)
                                If myConn.State = ConnectionState.Closed Then
                                    myConn.Open()
                                End If
                                Using dr As SqlDataReader = cmd.ExecuteReader
                                    If dr.Read() Then
                                        unSer = False
                                    Else
                                        unSer = True
                                    End If
                                End Using

                                myConn.Close()
                            End Using

                            If unSer = False And iSerial.Text <> iSearch.Text Then
                                MessageBox.Show("—ﬁ„ «·›« Ê—… «·–Ì ﬁ„  »≈œŒ«·Â „ÊÃÊœ „‰ ﬁ»·!", "Duplicate Serial", MessageBoxButtons.OK)
                                iSerial.Focus()
                                iSerial.SelectAll()
                            Else

                                'Check if the invoice has a paid debits
                                Dim Query1 As String = "SELECT tblIn1.Serial, COUNT(tblDebit.Amount) AS COU " _
                                                       & " FROM tblDebit INNER JOIN tblIn1" _
                                                       & " ON tblIn1.PrKey = tblDebit.Serial" _
                                                       & " WHERE tblIn1.Serial = N'" & iSearch.Text & "'" _
                                                       & " GROUP BY tblIn1.Serial"
                                Using cmd = New SqlCommand(Query1, myConn)
                                    If myConn.State = ConnectionState.Closed Then
                                        myConn.Open()
                                    End If
                                    Using dr As SqlDataReader = cmd.ExecuteReader
                                        If dr.Read() Then
                                            If dr(1) <> 1 Then
                                                MessageBox.Show("·« Ì„ﬂ‰  ⁄œÌ· «·›« Ê—… ÕÌÀ  „ œ›⁄ œÌÊ‰ „‰ ⁄·ÌÂ«!", "Can't Modify", MessageBoxButtons.OK)
                                                iSearch.Focus()
                                                iSearch.SelectAll()
                                                Exit Sub
                                            End If
                                        Else
                                            MsgBox("Error")
                                        End If
                                    End Using
                                    myConn.Close()
                                End Using

                                'start adding the invoice
                                Dim vndr, sril, datt, timm As String
                                Dim usr As Integer
                                Dim qntity, untpric, vlu As Decimal

                                vndr = iVendor.SelectedValue
                                'sril = iSerial.Text
                                datt = ieDate.Value.ToString("MM/dd/yyyy")
                                timm = ieTime.Value.ToString("HH:mm")
                                'temporary
                                usr = GlobalVariables.ID

                                If myConn.State = ConnectionState.Closed Then
                                    myConn.Open()
                                End If

                                'remove the old data
                                Dim invoSer As Integer
                                Using cmd = New SqlCommand("SELECT PrKey FROM tblIn1 WHERE Serial = N'" & iSearch.Text & "'", myConn)
                                    If myConn.State = ConnectionState.Closed Then
                                        myConn.Open()
                                    End If
                                    Using dr As SqlDataReader = cmd.ExecuteReader
                                        If dr.Read() Then
                                            invoSer = dr(0)
                                        Else
                                            MsgBox("Error")
                                        End If
                                    End Using

                                End Using
                                Using cmd = New SqlCommand("DELETE FROM tblIn2 WHERE Serial = '" & invoSer & "'", myConn)
                                    cmd.ExecuteNonQuery()
                                End Using

                                Dim Query2, Query3 As String

                                For x As Integer = 0 To iDgv.RowCount - 1
                                    'get the item prKey
                                    Dim QQuery As String = "SELECT tblItems.PrKey FROM tblItems" _
                                                      & " LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                                      & " WHERE tblItems.Serial = '" & iDgv.Rows(x).Cells(0).Value & "' OR tblMultiCodes.Code = '" & iDgv.Rows(x).Cells(0).Value & "';"
                                    Using cmd = New SqlCommand(QQuery, myConn)
                                        Using dr As SqlDataReader = cmd.ExecuteReader
                                            If dr.Read() Then
                                                sril = dr(0)
                                            Else
                                                sril = ""
                                            End If
                                        End Using
                                    End Using

                                    'compose the query string
                                    qntity = iDgv.Rows(x).Cells(2).Value
                                    untpric = iDgv.Rows(x).Cells(3).Value
                                    vlu = iDgv.Rows(x).Cells(4).Value

                                    Query2 = "INSERT INTO tblIn2 (Serial, Sr, Item, Qnty, Sold, UnitPrice, [Value])" _
                                   & " VALUES ('" & invoSer & "', '" & x + 1 & "', '" & sril & "', '" & qntity & "', '0', '" & untpric & "', '" & vlu & "')"

                                    Using cmd = New SqlCommand(Query2, myConn)
                                        cmd.ExecuteNonQuery()
                                    End Using
                                Next


                                'Update the tblIn1, and tblDebit

                                Query3 = "UPDATE tblIn1 SET [Date] = '" & datt & "', [Time] = '" & timm & "', Amount = '" & Val(iTotalSales.Text) & "', Paid = '" & Val(itPaid.Text) & "', Rest = '" & Val(iRest.Text) & "', [User] = '" & usr & "', Vendor = '" & vndr & "'" _
                                    & " WHERE PrKey = N'" & invoSer & "'"

                                Using cmd = New SqlCommand(Query3, myConn)
                                    cmd.ExecuteNonQuery()
                                End Using

                                Dim Query4 As String
                                Query4 = "UPDATE tblDebit SET Amount = '" & Val(itPaid.Text) & "', [Date] = '" & datt & "', [Time] = '" & timm & "', [User] = '" & usr & "'" _
                                    & " WHERE Serial = N'" & invoSer & "'"

                                Using cmd = New SqlCommand(Query4, myConn)
                                    cmd.ExecuteNonQuery()
                                End Using


                                myConn.Close()

                                Call Notification("Invoice Modified")

                                'clear the items
                                iDgv.Rows.Clear()
                                iVendor.Text = Nothing
                                'iVendor.Enabled = True
                                itPaid.Text = ""
                                iSerial.Text = Nothing
                                iItem.Text = Nothing
                                iItemName.Text = Nothing
                                iQnty.Text = Nothing
                                iUnitPrice.Text = Nothing
                                iValue.Text = Nothing
                                ieDate.Value = Today
                                ieTime.Value = Now
                            End If
                        Else
                            MessageBox.Show("·ﬁœ ﬁ„  »≈œŒ«· «”„ „Ê“⁄ €Ì— ’ÕÌÕ!", "Invalid Vendor", MessageBoxButtons.OK)
                            iVendor.Focus()
                            iVendor.SelectAll()
                        End If

                    End If

                End If
            End If
        ElseIf iAdd.Text = "Õ–›" Then
            If GlobalVariables.authority = "User" Or GlobalVariables.authority = "Cashier" Then
                MsgBox("!√‰  ·«  „·ﬂ «·’·«ÕÌ… ·Õ–› «·›« Ê—…")
                Exit Sub
            End If
            Dim dia As DialogResult
            dia = MessageBox.Show("Â·  —Ìœ »«· √ﬂÌœ Õ–› «·›« Ê—… «·Õ«·Ì…ø ·‰  ﬁœ— ⁄·Ï «” —Ã«⁄Â« „—… √Œ—Ï!", "Invoice Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If dia = Windows.Forms.DialogResult.Yes Then


                Dim invoSer As Integer
                Using cmd = New SqlCommand("SELECT PrKey FROM tblIn1 WHERE Serial = N'" & iSearch.Text & "'", myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            invoSer = dr(0)
                        Else
                            MsgBox("Error")
                        End If
                    End Using

                End Using

                'Check if the invoice has a paid debits
                Dim Query1 As String = "SELECT tblIn1.Serial, COUNT(tblDebit.Amount) AS COU " _
                                       & " FROM tblDebit INNER JOIN tblIn1" _
                                       & " ON tblIn1.PrKey = tblDebit.Serial" _
                                       & " WHERE tblIn1.Serial = N'" & iSearch.Text & "'" _
                                       & " GROUP BY tblIn1.Serial"
                Using cmd = New SqlCommand(Query1, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            If dr(1) <> 1 Then
                                MessageBox.Show("·« Ì„ﬂ‰ Õ–› «·›« Ê—… ÕÌÀ  „ œ›⁄ œÌÊ‰ „‰ ⁄·ÌÂ«!", "Can't Delete", MessageBoxButtons.OK)
                                iSearch.Focus()
                                iSearch.SelectAll()
                                Exit Sub
                            End If
                        Else
                            MsgBox("Error")
                        End If
                    End Using
                    myConn.Close()
                End Using

                'strrt deleting

                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using cmd = New SqlCommand("DELETE FROM tblDebit WHERE Serial = '" & invoSer & "'", myConn)
                    cmd.ExecuteNonQuery()
                End Using

                Using cmd = New SqlCommand("DELETE FROM tblIn2 WHERE Serial = '" & invoSer & "'", myConn)
                    cmd.ExecuteNonQuery()
                End Using

                Using cmd = New SqlCommand("DELETE FROM tblIn1 WHERE PrKey = '" & invoSer & "'", myConn)
                    cmd.ExecuteNonQuery()
                End Using

                myConn.Close()

                'clear the items
                iDgv.Rows.Clear()
                iVendor.Text = Nothing
                'iVendor.Enabled = True
                iSerial.Text = Nothing
                iItem.Text = Nothing
                iItemName.Text = Nothing
                iQnty.Text = Nothing
                iUnitPrice.Text = Nothing
                iValue.Text = Nothing
                ieDate.Value = Today
                ieTime.Value = Now

                Call Notification("Invoice Deleted!")
            End If
        End If
    End Sub

    Private Sub rdiModify_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdiModify.CheckedChanged
        If rdiAdd.Checked = True Then
            iAdd.Text = "Õ›Ÿ"
            iSearch.Visible = False

            'clear the items
            iDgv.Rows.Clear()
            iVendor.Text = Nothing
            'iVendor.Enabled = True
            iSerial.Text = Nothing
            iItem.Text = Nothing
            iItemName.Text = Nothing
            iQnty.Text = Nothing
            iUnitPrice.Text = Nothing
            iValue.Text = Nothing

        ElseIf rdiModify.Checked = True Then
            iAdd.Text = " ⁄œÌ·"
            iSearch.Visible = True

            'fill the combobox
            Using cmd = New SqlCommand("SELECT Serial FROM tblIn1 WHERE Vendor = '" & iVendor.SelectedValue & "' ORDER BY Serial DESC", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                iSearch.DataSource = ds.Tables(0)
                iSearch.DisplayMember = "Serial"
                iSearch.Text = Nothing

                myConn.Close()
            End Using

        Else
            iAdd.Text = "Õ–›"
            iSearch.Visible = True

            Using cmd = New SqlCommand("SELECT Serial FROM tblIn1 WHERE Vendor = '" & iVendor.SelectedValue & "' ORDER BY Serial DESC", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                iSearch.DataSource = ds.Tables(0)
                iSearch.DisplayMember = "Serial"
                iSearch.Text = Nothing

                myConn.Close()

            End Using

        End If
    End Sub

    Private Sub rdiDelete_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdiDelete.CheckedChanged
        If rdiAdd.Checked = True Then
            iAdd.Text = "Õ›Ÿ"
            iSearch.Visible = False

            'clear the items
            iDgv.Rows.Clear()
            iVendor.Text = Nothing
            'iVendor.Enabled = True
            iSerial.Text = Nothing
            iItem.Text = Nothing
            iItemName.Text = Nothing
            iQnty.Text = Nothing
            iUnitPrice.Text = Nothing
            iValue.Text = Nothing

        ElseIf rdiModify.Checked = True Then
            iAdd.Text = " ⁄œÌ·"
            iSearch.Visible = True

            'fill the combobox
            Using cmd = New SqlCommand("SELECT Serial FROM tblIn1 WHERE Vendor = '" & iVendor.SelectedValue & "' ORDER BY Serial DESC", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                iSearch.DataSource = ds.Tables(0)
                iSearch.DisplayMember = "Serial"
                iSearch.Text = Nothing

                myConn.Close()

            End Using

        Else
            iAdd.Text = "Õ–›"
            iSearch.Visible = True

            Using cmd = New SqlCommand("SELECT Serial FROM tblIn1 WHERE Vendor = '" & iVendor.SelectedValue & "' ORDER BY Serial DESC", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                iSearch.DataSource = ds.Tables(0)
                iSearch.DisplayMember = "Serial"
                iSearch.Text = Nothing

                myConn.Close()


            End Using

        End If
    End Sub


    Private Sub iSearch_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles iSearch.LostFocus

        Dim Query As String
        Query = "SELECT tblIn1.Vendor, tblIn1.Serial, tblIn1.[Date], tblIn1.[Time], tblItems.Serial AS Item, tblItems.Name, tblIn2.Qnty, tblIn2.UnitPrice, tblIn2.[Value], tblIn1.Paid" _
            & " FROM tblIn1 INNER JOIN" _
            & " tblIn2 ON tblIn1.PrKey = tblIn2.Serial" _
            & " INNER JOIN tblItems" _
            & " ON tblIn2.Item = tblItems.PrKey" _
            & " WHERE tblIn1.Vendor = '" & iVendor.SelectedValue & "' AND tblIn1.Serial = N'" & iSearch.Text & "'"

        Using cmd = New SqlCommand(Query, myConn)

            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Using dr As SqlDataReader = cmd.ExecuteReader
                Try
                    Dim dt As New DataTable
                    dt.Load(dr)

                    iDgv.Rows.Clear()
                    For x As Integer = 0 To dt.Rows.Count - 1
                        iDgv.Rows.Add(dt.Rows(x)(4), dt.Rows(x)(5), dt.Rows(x)(6), dt.Rows(x)(7), dt.Rows(x)(8), dt.Rows(x)(9))
                    Next

                    iSerial.Text = dt.Rows(0)(1).ToString
                    ieDate.Value = dt.Rows(0)(2)
                    ieTime.Value = dt.Rows(0)(3)

                Catch ex As Exception

                End Try

            End Using


            myConn.Close()

        End Using

    End Sub

    Private Sub iSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iSearch.SelectedIndexChanged

        Dim Query As String
        Query = "SELECT tblIn1.Vendor, tblIn1.Serial, tblIn1.[Date], tblIn1.[Time], tblItems.Serial AS Item, tblItems.Name, tblIn2.Qnty, tblIn2.UnitPrice, tblIn2.[Value], tblIn1.Paid" _
            & " FROM tblIn1 INNER JOIN" _
            & " tblIn2 ON tblIn1.PrKey = tblIn2.Serial" _
            & " INNER JOIN tblItems" _
            & " ON tblIn2.Item = tblItems.PrKey" _
            & " WHERE tblIn1.Vendor = '" & iVendor.SelectedValue & "' AND tblIn1.Serial = N'" & iSearch.Text & "'"

        Using cmd = New SqlCommand(Query, myConn)

            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Using dr As SqlDataReader = cmd.ExecuteReader
                Try
                    Dim dt As New DataTable
                    dt.Load(dr)

                    iDgv.Rows.Clear()
                    For x As Integer = 0 To dt.Rows.Count - 1
                        iDgv.Rows.Add(dt.Rows(x)(4), dt.Rows(x)(5), dt.Rows(x)(6), dt.Rows(x)(7), dt.Rows(x)(8), dt.Rows(x)(9))
                    Next

                    iSerial.Text = dt.Rows(0)(1).ToString
                    ieDate.Value = dt.Rows(0)(2)
                    ieTime.Value = dt.Rows(0)(3)
                    itPaid.Text = dt.Rows(0)(9)

                Catch ex As Exception

                End Try

            End Using

            myConn.Close()
        End Using
    End Sub

    Private Sub iVendor_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles iVendor.LostFocus


        If Not iVendor.Text = "" Then
            If iSearch.Visible = True Then
                Using cmd = New SqlCommand("SELECT Serial FROM tblIn1 WHERE Vendor = '" & iVendor.SelectedValue & "' ORDER BY Serial DESC", myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Dim adt As New SqlDataAdapter
                    Dim ds As New DataSet()
                    adt.SelectCommand = cmd
                    adt.Fill(ds)
                    adt.Dispose()

                    iSearch.DataSource = ds.Tables(0)
                    iSearch.DisplayMember = "Serial"
                    iSearch.Text = Nothing


                    myConn.Close()

                End Using
            End If

        End If
    End Sub

    Private Sub ComboBox1_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iVendor.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            iSerial.Focus()
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iVendor.SelectedIndexChanged
        If Not iVendor.Text = "" Then

            If iSearch.Visible = True Then
                Using cmd = New SqlCommand("SELECT Serial FROM tblIn1 WHERE Vendor = '" & iVendor.SelectedValue & "' ORDER BY Serial DESC", myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Dim adt As New SqlDataAdapter
                    Dim ds As New DataSet()
                    adt.SelectCommand = cmd
                    adt.Fill(ds)
                    adt.Dispose()

                    iSearch.DataSource = ds.Tables(0)
                    iSearch.DisplayMember = "Serial"
                    iSearch.Text = Nothing

                End Using
            End If

            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            'get the vendor's details:
            Dim Q1, Q2 As String

            Q1 = "SELECT COALESCE(SUM(tblIn1.Rest), 0) AS Rest" _
                & " FROM tblIn1" _
                & " INNER JOIN tblVendors" _
                & " ON tblVendors.Sr = tblIn1.Vendor" _
                & " WHERE tblVendors.Name = N'" & iVendor.Text & "'"


            Using cmd = New SqlCommand(Q1, myConn)
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        KryptonLabel20.Text = Val(dr(0))
                    Else
                        KryptonLabel20.Text = "00"
                    End If
                End Using
            End Using

            Q2 = "SELECT TOP(1) [Date] FROM tblIn1" _
                & " INNER JOIN tblVendors" _
                & " ON tblIn1.Vendor = tblVendors.Sr" _
                & " WHERE tblVendors.Name = N'" & iVendor.Text & "'" _
                & " ORDER BY tblIn1.[Date], tblIn1.[Time] DESC"

            Using cmd = New SqlCommand(Q2, myConn)
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        KryptonLabel38.Text = dr(0)
                    Else
                        KryptonLabel38.Text = ""
                    End If
                End Using
            End Using
            myConn.Close()
            KryptonPanel7.Visible = True
        Else
            KryptonLabel38.Text = ""
            KryptonLabel20.Text = "00"
            KryptonPanel7.Visible = False
        End If
    End Sub

    Private Sub iVendor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles iVendor.TextChanged
        If Not iVendor.Text = "" Then

            If iSearch.Visible = True Then
                Using cmd = New SqlCommand("SELECT Serial FROM tblIn1 WHERE Vendor = '" & iVendor.SelectedValue & "' ORDER BY Serial DESC", myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Dim adt As New SqlDataAdapter
                    Dim ds As New DataSet()
                    adt.SelectCommand = cmd
                    adt.Fill(ds)
                    adt.Dispose()

                    iSearch.DataSource = ds.Tables(0)
                    iSearch.DisplayMember = "Serial"
                    iSearch.Text = Nothing

                End Using
            End If

            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            'get the vendor's details:
            Dim Q1, Q2 As String

            Q1 = "SELECT COALESCE(SUM(tblIn1.Rest), 0) AS Rest" _
                & " FROM tblIn1" _
                & " INNER JOIN tblVendors" _
                & " ON tblVendors.Sr = tblIn1.Vendor" _
                & " WHERE tblVendors.Name = N'" & iVendor.Text & "'"

            Using cmd = New SqlCommand(Q1, myConn)
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        KryptonLabel20.Text = Val(dr(0))
                    Else
                        KryptonLabel20.Text = "00"
                    End If
                End Using
            End Using

            Q2 = "SELECT TOP(1) [Date] FROM tblIn1" _
                & " INNER JOIN tblVendors" _
                & " ON tblIn1.Vendor = tblVendors.Sr" _
                & " WHERE tblVendors.Name = N'" & iVendor.Text & "'" _
                & " ORDER BY tblIn1.[Date], tblIn1.[Time] DESC"

            Using cmd = New SqlCommand(Q2, myConn)
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        KryptonLabel38.Text = dr(0)
                    Else
                        KryptonLabel38.Text = ""
                    End If
                End Using
            End Using
            myConn.Close()
            KryptonPanel7.Visible = True
        Else
            KryptonLabel38.Text = ""
            KryptonLabel20.Text = "00"
            KryptonPanel7.Visible = False
        End If
    End Sub

    Private Sub iItemAdd_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iItemAdd.PreviewKeyDown
        If e.KeyCode = Keys.Down Then
            iItemEdit.Focus()
        End If
    End Sub

    Private Sub iItemEdit_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iItemEdit.PreviewKeyDown
        If e.KeyCode = Keys.Down Then
            iItemRemove.Focus()
        ElseIf e.KeyCode = Keys.Up Then
            iItemAdd.Focus()
        End If
    End Sub

    Private Sub iItemRemove_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iItemRemove.PreviewKeyDown
        If e.KeyCode = Keys.Up Then
            iItemEdit.Focus()
        End If
    End Sub

    Private Sub vPhone1_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles vPhone1.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            vPhone2.Focus()
            vPhone2.SelectAll()
        ElseIf e.KeyCode = Keys.Up Then
            vName.Focus()
            vName.SelectAll()

        End If
    End Sub

    Private Sub vPhone2_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles vPhone2.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            vEmail.Focus()
            vEmail.SelectAll()
        ElseIf e.KeyCode = Keys.Up Then
            vPhone1.Focus()
            vPhone1.SelectAll()
        End If
    End Sub

    Private Sub vAdd_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles vAdd.PreviewKeyDown
        If e.KeyCode = Keys.Left Then
            vNotes.Focus()
            vNotes.SelectAll()
        ElseIf e.KeyCode = Keys.Right Then
            iClear.Focus()
        End If
    End Sub

    Private Sub KryptonRibbonGroupButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '  Call AmountInWords("122", "123", Me.Text)

    End Sub

    Private Sub nItemPrice_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles nItemPrice.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And nItemPrice.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub nItemPrice_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles nItemPrice.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Or e.KeyCode = Keys.Down Then
            KryptonTextBox1.Focus()
        ElseIf e.KeyCode = Keys.Up Then
            nEnglishName.Focus()
        End If
    End Sub

    Private Sub iiPrice_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles iiPrice.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And iiPrice.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub iiPrice_PreviewKeyDown(sender As Object, e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iiPrice.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            If e.KeyCode = Keys.Shift Then
                iiEnglishName.Focus()
            Else
                iiMinimumQnty.Focus()
            End If
        ElseIf e.KeyCode = Keys.Up Then
            iiEnglishName.Focus()
        End If
    End Sub

    Private Sub iiRdNew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iiRdNew.CheckedChanged
        If iiRdNew.Checked Then
            iiAdd.Text = "Õ›Ÿ"
            iiSearch.Visible = False
        ElseIf iiRdModify.Checked = True Then
            iiAdd.Text = " ⁄œÌ·"
            iiSearch.Visible = True
            iiSearch.Text = Nothing
        ElseIf iiRdDelete.Checked = True Then
            iiAdd.Text = "Õ–›"
            iiSearch.Visible = True
            iiSearch.Text = Nothing
        End If
    End Sub

    Private Sub iiAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iiAdd.Click
        If iiAdd.Text = "Õ›Ÿ" Then
            If iiSerial.Text = "" Then
                MessageBox.Show("ÌÃ» ≈œŒ«· —ﬁ„ ﬂÊœ!", "Empty Serial", MessageBoxButtons.OK, MessageBoxIcon.Information)
                iiSerial.Focus()
            ElseIf iiItem.Text = "" Then
                MessageBox.Show("ÌÃ» ≈œŒ«· «”„ «·’‰›!", "Empty Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                iiItem.Focus()
            ElseIf Val(iiPrice.Text) = 0 Then
                MessageBox.Show("ÌÃ» ≈œŒ«· ”⁄— ’ÕÌÕ!", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                iiPrice.Focus()
            ElseIf iiSerial2.Text <> "" And (Val(iiGroupPrice.Text) = 0 Or Val(iiUnitNumber.Text) = 0) Then
                If Val(iiGroupPrice.Text) = 0 Then
                    MessageBox.Show("ÌÃ» ≈œŒ«· ”⁄— «·Ã„·…!", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    iiGroupPrice.Focus()
                Else
                    MessageBox.Show("ÌÃ» ≈œŒ«· ⁄œœ «·ÊÕœ« !", "Invalid Units", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    iiUnitNumber.Focus()
                End If
            ElseIf (Val(iiUnitNumber.Text) <> 0 AndAlso Val(iiGroupPrice.Text) = 0) Or (Val(iiUnitNumber.Text) = 0 AndAlso Val(iiGroupPrice.Text) <> 0) Then
                MessageBox.Show("ÌÃ» ≈œŒ«· ”⁄— «·Ã„·…!", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                iiGroupPrice.Focus()
                'ElseIf (Val(iiPrice.Text) > Val(iiGroupPrice.Text)) And Not Val(iiGroupPrice.Text) = 0 Then
                '    MessageBox.Show("!ÌÃ» √‰ ÌﬂÊ‰ ”⁄— «·Ã„·… √ﬂ»— „‰ ”⁄— «·ÊÕœ…", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '    iiGroupPrice.Focus()
                'ElseIf (Val(iiGroupPrice.Text) / Val(iiUnitNumber.Text)) >= Val(iiPrice.Text) Then
                '    MessageBox.Show("!ÌÃ» √‰ ÌﬂÊ‰ ”⁄— «·ÊÕœ… ›Ì ««·Ã„·… √’€— „‰ ”⁄— «·ÊÕœ…", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '    iiGroupPrice.Focus()
            Else

                'check duplicate serial
                'new
                Dim mlc() As String = iiAlterCodes.Text.Split(";")
                For x As Integer = 0 To mlc.Count - 1
                    If mlc(x).Trim = iiSerial.Text.Trim Then
                        MsgBox("!«·ﬂÊœ «·—∆Ì”Ì ·« Ì„ﬂ‰ √‰ ÌﬂÊ‰ ﬂÊœ ≈÷«›Ì")
                        Exit Sub
                    End If
                Next


                Dim NQuery As String = "SELECT tblItems.PrKey FROM tblItems " _
                                       & " LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                       & " WHERE tblItems.Serial = N'" & iiSerial.Text & "' OR tblMultiCodes.Code = N'" & iiSerial.Text & "'"
                Using cmd = New SqlCommand(NQuery, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If

                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            myConn.Close()
                            MessageBox.Show("!«·ﬂÊœ «·–Ì ﬁ„  »≈œŒ«·Â „” Œœ„ ·’‰› ¬Œ—", "Invalid Serial", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            iiSerial.Focus()
                            iiSerial.SelectAll()
                            Exit Sub
                        End If
                    End Using

                    myConn.Close()

                End Using

                'check duplicate names
                Using cmd = New SqlCommand("SELECT Serial FROM tblItems WHERE Name = N'" & iiItem.Text & "'", myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If

                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            myConn.Close()
                            MessageBox.Show("!«”„ «·’‰› «·–Ì ﬁ„  »≈œŒ«·Â „” Œœ„ ·’‰› ¬Œ—", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            iItem.Focus()
                            iItem.SelectAll()
                            Exit Sub
                        End If
                    End Using

                    myConn.Close()

                End Using

                'check duplicate package serial2
                Dim CheckQuery3 As String = "SELECT Name FROM tblItems WHERE PackageSerial = N'" & iiSerial2.Text & "' AND PackageSerial IS NOT NULL AND PackageSerial != ''"
                Using cmd = New SqlCommand(CheckQuery3, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If

                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            myConn.Close()
                            MessageBox.Show("!ﬂÊœ «·Ã„·… «·–Ì ﬁ„  »≈œŒ«·Â „” Œœ„ ·’‰› ¬Œ—", "Invalid Code", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            iiSerial2.Focus()
                            iiSerial2.SelectAll()
                            Exit Sub
                        End If
                    End Using

                    myConn.Close()

                End Using


                ' Add new Item to tblItems
                Dim Query As String = "INSERT INTO tblItems (Serial, Name, Price, [Minimum], PackageSerial, PackagePrice, PackageUnits, EnglishName)" _
                                      & " VALUES ('" & iiSerial.Text.ToUpper & "', N'" & iiItem.Text.ToUpper & "', '" _
                                      & Val(iiPrice.Text) & "', '" & Val(iiMinimumQnty.Text) & "', N'" & iiSerial2.Text & "', '" & iiGroupPrice.Text _
                                      & "', '" & iiUnitNumber.Text & "', N'" & iiEnglishName.Text & "')"

                Using cmd = New SqlCommand(Query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    cmd.ExecuteNonQuery()

                    myConn.Close()

                End Using

                Call AlterCodes(iiSerial.Text, iiAlterCodes.Text)


                'iiVendor.Text = Nothing
                iiSerial.Text = Nothing
                iiItem.Text = Nothing
                iiPrice.Text = Nothing
                iiSerial2.Text = Nothing
                iiGroupPrice.Text = Nothing
                iiUnitNumber.Text = Nothing
                iiAlterCodes.Text = Nothing
                iiEnglishName.Text = ""
                iiSerial.Focus()
                'refill the items search

                Dim FillQuery As String = "SELECT PrKey, Serial, Name FROM tblItems" _
                                                          & " UNION ALL" _
                                                          & " SELECT tblItems.PrKey, tblMultiCodes.Code AS Serial, tblItems.Name" _
                                                          & " FROM tblMultiCodes INNER JOIN tblItems ON tblMultiCodes.Item = tblItems.PrKey" _
                                                          & " ORDER BY Name;"

                Using cmd = New SqlCommand(FillQuery, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Dim adt As New SqlDataAdapter
                    Dim ds As New DataSet()
                    adt.SelectCommand = cmd
                    adt.Fill(ds)
                    adt.Dispose()

                    iiSearch.DataSource = ds.Tables(0)
                    iiSearch.DisplayMember = "Serial"
                    iiSearch.ValueMember = "PrKey"
                    iiSearch.Text = Nothing

                    myConn.Close()

                End Using

                Call Notification("Item Added!")

            End If
        ElseIf iiAdd.Text = " ⁄œÌ·" Then
            If iiSerial.Text = "" Then
                MessageBox.Show("ÌÃ» ≈œŒ«· «·ﬂÊœ!", "Empty Serial", MessageBoxButtons.OK, MessageBoxIcon.Information)
                iiSerial.Focus()
            ElseIf iiItem.Text = "" Then
                MessageBox.Show("ÌÃ» «œŒ«· «”„ «·’‰›!", "Empty Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                iiItem.Focus()
            ElseIf Val(iiPrice.Text) = 0 Then
                MessageBox.Show("ÌÃ» ≈œŒ«· ”⁄— ’ÕÌÕ!", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                iiPrice.Focus()
            ElseIf iiSerial2.Text <> "" And (Val(iiGroupPrice.Text) = 0 Or Val(iiUnitNumber.Text) = 0) Then
                If Val(iiGroupPrice.Text) = 0 Then
                    MessageBox.Show("ÌÃ» ≈œŒ«· ”⁄— «·Ã„·…!", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    iiGroupPrice.Focus()
                Else
                    MessageBox.Show("ÌÃ» ≈œŒ«· ⁄œœ «·ÊÕœ« !", "Invalid Units", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    iiUnitNumber.Focus()
                End If
            ElseIf (Val(iiUnitNumber.Text) <> 0 AndAlso Val(iiGroupPrice.Text) = 0) Or (Val(iiUnitNumber.Text) = 0 AndAlso Val(iiGroupPrice.Text) <> 0) Then
                MessageBox.Show("ÌÃ» ≈œŒ«· ”⁄— «·Ã„·…!", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                iiGroupPrice.Focus()
                'ElseIf (Val(iiPrice.Text) > Val(iiGroupPrice.Text)) And Not Val(iiGroupPrice.Text) = 0 Then
                '    MessageBox.Show("!ÌÃ» √‰ ÌﬂÊ‰ ”⁄— «·Ã„·… √ﬂ»— „‰ ”⁄— «·ÊÕœ…", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '    iiGroupPrice.Focus()
                'ElseIf (Val(iiGroupPrice.Text) / Val(iiUnitNumber.Text)) >= Val(iiPrice.Text) Then
                '    MessageBox.Show("!ÌÃ» √‰ ÌﬂÊ‰ ”⁄— «·ÊÕœ… ›Ì ««·Ã„·… √’€— „‰ ”⁄— «·ÊÕœ…", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '    iiGroupPrice.Focus()

            Else
                'new
                Dim mlc() As String = iiAlterCodes.Text.Split(";")
                For x As Integer = 0 To mlc.Count - 1
                    If mlc(x).Trim = iiSerial.Text.Trim Then
                        MsgBox("!«·ﬂÊœ «·—∆Ì”Ì ·« Ì„ﬂ‰ √‰ ÌﬂÊ‰ ﬂÊœ ≈÷«›Ì")
                        Exit Sub
                    End If
                Next


                'check duplicate serial
                Dim NQuery As String = "SELECT COUNT(*) FROM tblItems WHERE Serial = '" & iiSerial.Text & "' " _
                                       & " AND NOT tblItems.PrKey IN (" _
                                       & " SELECT tblItems.PrKey" _
                                       & " FROM tblItems LEFT OUTER JOIN tblMultiCodes" _
                                       & " ON tblItems.PrKey = tblMultiCodes.Item" _
                                       & " WHERE tblItems.Serial = '" & iiSearch.Text & "' OR tblMultiCodes.Code = '" & iiSearch.Text & "');"

                Using cmd = New SqlCommand(NQuery, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If

                    'Using dr As SqlDataReader = cmd.ExecuteReader
                    If cmd.ExecuteScalar <> 0 Then
                        myConn.Close()
                        MessageBox.Show("!«·ﬂÊœ «·–Ì ﬁ„  »≈œŒ«·Â „” Œœ„ ·’‰› ¬Œ—", "Invalid Serial", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        iiSerial.Focus()
                        iiSerial.SelectAll()
                        Exit Sub
                    End If
                    'End Using

                    myConn.Close()

                End Using

                'check duplicate names
                Using cmd = New SqlCommand("SELECT Serial FROM tblItems WHERE Name = N'" & iiItem.Text & "' " _
                                           & " AND NOT tblItems.PrKey = (SELECT tblItems.PrKey FROM tblMultiCodes RIGHT OUTER JOIN tblItems ON tblMultiCodes.Item = tblItems.PrKey WHERE tblMultiCodes.Code = N'" & iiSearch.Text & "')", myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If

                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            myConn.Close()
                            MessageBox.Show("!«”„ «·’‰› «·–Ì ﬁ„  »≈œŒ«·Â „” Œœ„ ·’‰› ¬Œ—", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            iItemName.Focus()
                            iItemName.SelectAll()
                            Exit Sub
                        End If
                    End Using

                    myConn.Close()

                End Using

                'check duplicate package serial2
                Dim CheckQuery3 As String = "SELECT Name FROM tblItems" _
                                            & " WHERE PackageSerial = N'" & iiSerial2.Text & "'" _
                                            & " AND PackageSerial IS NOT NULL" _
                                            & " AND PackageSerial != ''" _
                                            & " AND NOT PrKey = (" _
                                            & " SELECT tblItems.PrKey" _
                                            & " FROM tblMultiCodes RIGHT OUTER JOIN tblItems" _
                                            & " ON tblMultiCodes.Item = tblItems.PrKey" _
                                            & " WHERE tblMultiCodes.Code = N'" & iiSearch.Text & "' OR tblItems.Serial = N'" & iiSearch.Text & "')"
                Using cmd = New SqlCommand(CheckQuery3, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If

                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            myConn.Close()
                            MessageBox.Show("!ﬂÊœ «·Ã„·… «·–Ì ﬁ„  »≈œŒ«·Â „” Œœ„ ·’‰› ¬Œ—", "Invalid Code", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            iiSerial2.Focus()
                            iiSerial2.SelectAll()
                            Exit Sub
                        End If
                    End Using

                    myConn.Close()

                End Using





                ' Add new Item to tblItems
                Dim Query As String = "UPDATE tblItems SET Serial = '" & iiSerial.Text & "', Name = N'" & iiItem.Text _
                                      & "', Price = " & iiPrice.Text & ", [Minimum] = " & Val(iiMinimumQnty.Text) _
                                      & ", PackageSerial= N'" & iiSerial2.Text & "', PackagePrice = " & Val(iiGroupPrice.Text) _
                                      & ", PackageUnits = " & Val(iiUnitNumber.Text) & ", EnglishName = N'" & iiEnglishName.Text & "'" _
                                      & " WHERE PrKey IN (" _
                                      & " SELECT tblItems.PrKey FROM tblItems LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                      & " WHERE tblItems.Serial = '" & iiSearch.Text & "' OR tblMultiCodes.Code = '" & iiSearch.Text & "');"

                Using cmd = New SqlCommand(Query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Try
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                        myConn.Close()
                        Exit Sub
                    End Try

                    myConn.Close()

                End Using

                Call AlterCodes(iiSerial.Text, iiAlterCodes.Text)

                iiSearch.Text = iiSerial.Text
                iiSerial.Text = Nothing
                iiItem.Text = Nothing
                iiPrice.Text = Nothing
                iiMinimumQnty.Text = Nothing
                iiSerial2.Text = Nothing
                iiGroupPrice.Text = Nothing
                iiUnitNumber.Text = Nothing
                iiAlterCodes.Text = Nothing
                iiEnglishName.Text = Nothing
                iiSearch.Focus()
                iiSearch.SelectAll()

                'fill the items search
                Dim FillQuery As String = "SELECT PrKey, Serial, Name FROM tblItems" _
                                                  & " UNION ALL" _
                                                  & " SELECT tblItems.PrKey, tblMultiCodes.Code AS Serial, tblItems.Name" _
                                                  & " FROM tblMultiCodes INNER JOIN tblItems ON tblMultiCodes.Item = tblItems.PrKey" _
                                                  & " ORDER BY Name;"

                Using cmd = New SqlCommand(FillQuery, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Dim adt As New SqlDataAdapter
                    Dim ds As New DataSet()
                    adt.SelectCommand = cmd
                    adt.Fill(ds)
                    adt.Dispose()

                    iiSearch.DataSource = ds.Tables(0)
                    iiSearch.DisplayMember = "Serial"
                    iiSearch.ValueMember = "PrKey"
                    iiSearch.Text = Nothing

                    myConn.Close()

                End Using
                Call Notification("Item Updated!")

            End If
        ElseIf iiAdd.Text = "Õ–›" Then
            Dim dia As DialogResult
            dia = MessageBox.Show("Â·  —Ìœ »«· √ﬂÌœ Õ–› «·’‰› «·Õ«·Ìø", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If dia = Windows.Forms.DialogResult.Yes Then
                Dim DelQuery As String = "DECLARE @PrKey INT;" _
                                         & " SET @prkey = (SELECT tblItems.PrKey FROM tblItems LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                         & " WHERE tblItems.Serial = '" & iiSearch.Text & "' OR tblMultiCodes.Code = '" & iiSearch.Text & "');" _
                                         & " DELETE FROM tblMultiCodes WHERE Item = @PrKey;" _
                                         & " DELETE FROM tblItems WHERE PrKey = @PrKey"

                Using cmd = New SqlCommand(DelQuery, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Try
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        MsgBox("·« Ì„ﬂ‰ Õ–› «·’‰› «·Õ«·Ì∫ ÕÌÀ √‰Â „” Œœ„ ›Ì ⁄„·Ì«  √Œ—Ï!")
                    End Try

                    myConn.Close()

                End Using
                'Call AlterCodes(iiSerial.Text, iiAlterCodes.Text)

                iiSerial.Text = Nothing
                iiItem.Text = Nothing
                iiPrice.Text = Nothing
                iiMinimumQnty.Text = Nothing
                iiSerial2.Text = Nothing
                iiGroupPrice.Text = Nothing
                iiUnitNumber.Text = Nothing
                iiAlterCodes.Text = Nothing
                iiEnglishName.Text = ""
                'fill the items search
                Dim FillQuery As String = "SELECT PrKey, Serial, Name FROM tblItems" _
                                                          & " UNION ALL" _
                                                          & " SELECT tblItems.PrKey, tblMultiCodes.Code AS Serial, tblItems.Name" _
                                                          & " FROM tblMultiCodes INNER JOIN tblItems ON tblMultiCodes.Item = tblItems.PrKey" _
                                                          & " ORDER BY Name;"

                Using cmd = New SqlCommand(FillQuery, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Dim adt As New SqlDataAdapter
                    Dim ds As New DataSet()
                    adt.SelectCommand = cmd
                    adt.Fill(ds)
                    adt.Dispose()

                    iiSearch.DataSource = ds.Tables(0)
                    iiSearch.DisplayMember = "Serial"
                    iiSearch.ValueMember = "PrKey"
                    iiSearch.Text = Nothing

                    myConn.Close()
                End Using

                Call Notification("Item Deleted!")

            End If
        End If
    End Sub

    Private Sub iiClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iiClear.Click

        iiSerial.Text = Nothing
        iiItem.Text = Nothing
        iiPrice.Text = Nothing
        iiMinimumQnty.Text = Nothing
        iiSerial2.Text = Nothing
        iiGroupPrice.Text = Nothing
        iiUnitNumber.Text = Nothing
        iiAlterCodes.Text = Nothing
        iiEnglishName.Text = Nothing
        Call Notification("Record Cleared!")

    End Sub

    Private Sub iiSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iiSearch.SelectedIndexChanged
        If iiSearch.Text <> "" Then
            Dim NQuery As String = "SELECT tblItems.* FROM tblItems LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                      & " WHERE tblItems.Serial = '" & iiSearch.Text & "' OR tblMultiCodes.Code = '" & iiSearch.Text & "';"
            Using cmd = New SqlCommand(NQuery, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If

                Dim ds As New DataSet()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(ds, "tblItems")
                Dim dt As New DataTable
                dt = ds.Tables(0)

                Try
                    iiSerial.Text = dt.Rows(0)(1).ToString
                    iiItem.Text = dt.Rows(0)(2).ToString
                    iiPrice.Text = dt.Rows(0)(3).ToString
                    iiMinimumQnty.Text = dt.Rows(0)(5).ToString
                    iiSerial2.Text = dt.Rows(0)(6).ToString
                    iiGroupPrice.Text = dt.Rows(0)(7).ToString
                    iiUnitNumber.Text = dt.Rows(0)(8).ToString
                    iiEnglishName.Text = dt.Rows(0)(9).ToString
                Catch ex As Exception

                End Try


                Try
                    cmd.CommandText = "SELECT Code + ';' FROM tblMultiCodes WHERE Item = " & dt(0)(0).ToString & " FOR XML PATH('');"
                    iiAlterCodes.Text = cmd.ExecuteScalar
                Catch ex As Exception
                    iiAlterCodes.Text = ""
                End Try
                myConn.Close()
            End Using
        Else
            iiSerial.Text = Nothing
            iiItem.Text = Nothing
            iiPrice.Text = Nothing
            iiSerial2.Text = Nothing
            iiGroupPrice.Text = Nothing
            iiUnitNumber.Text = Nothing
            iiAlterCodes.Text = Nothing
            iiEnglishName.Text = Nothing
        End If
    End Sub

    Private Sub iiRdModify_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iiRdModify.CheckedChanged
        If iiRdNew.Checked Then
            iiAdd.Text = "Õ›Ÿ"
            iiSearch.Visible = False
        ElseIf iiRdModify.Checked = True Then
            iiAdd.Text = " ⁄œÌ·"
            iiSearch.Visible = True
            iiSearch.Text = Nothing
        ElseIf iiRdDelete.Checked = True Then
            iiAdd.Text = "Õ–›"
            iiSearch.Visible = True
            iiSearch.Text = Nothing
        End If
    End Sub

    Private Sub iiRdDelete_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iiRdDelete.CheckedChanged
        If iiRdNew.Checked Then
            iiAdd.Text = "Õ›Ÿ"
            iiSearch.Visible = False
        ElseIf iiRdModify.Checked = True Then
            iiAdd.Text = " ⁄œÌ·"
            iiSearch.Visible = True
            iiSearch.Text = Nothing
        ElseIf iiRdDelete.Checked = True Then
            iiAdd.Text = "Õ–›"
            iiSearch.Visible = True
            iiSearch.Text = Nothing
        End If
    End Sub

    Private Sub oItemSerial_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles oItemSerial.LostFocus
        oItemSerial.Text = oItemSerial.Text.ToUpper

        If Not oItemSerial.Text = "" Then
            Using cmd = New SqlCommand("SELECT Name FROM tblItems WHERE Serial = '" & oItemSerial.Text & "'", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() Then
                        oItemName.Text = dt(0).ToString
                    Else
                        oItemName.Text = Nothing
                    End If
                End Using

                myConn.Close()

            End Using
        Else
            oItemName.Text = Nothing
        End If

    End Sub

    Private Sub oItemSerial_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles oItemSerial.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab And oItemName.Visible = True Then
            oItemName.Focus()
        End If
    End Sub

    Private Sub oItemSerial_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oItemSerial.SelectedIndexChanged
        If oItemSerial.Text <> "" Then

            'enter the value

            'oCompound.Visible = True
            Dim Query As String
            Query = "SELECT Price FROM tblItems WHERE Serial = '" & oItemSerial.Text & "'"
            Using cmd As New SqlCommand(Query, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() Then
                        oUnitPrice.Text = dt(0)
                    Else
                        oUnitPrice.Text = ""
                    End If
                End Using


                myConn.Close()

            End Using


            Try
                'Show the Net Qnty
                Dim NetQuery As String
                NetQuery = "SELECT tblItems.PrKey, tblItems.Serial, tblItems.Name, ttlIn.Total_In, COALESCE(ttlOut.Total_Out , 0) AS Total_Out,  (ttlIn.Total_In - COALESCE(ttlOut.Total_Out , 0)) AS Net_Amount" _
                    & " FROM tblItems" _
                    & " LEFT OUTER JOIN" _
                    & " (" _
                    & " SELECT tblIn2.Item, SUM(tblIn2.Qnty) AS Total_In FROM tblIn2" _
                    & " GROUP BY tblIn2.Item" _
                    & " ) ttlIn" _
                    & " ON tblItems.PrKey = ttlIn.Item" _
                    & " LEFT OUTER JOIN" _
                    & " (" _
                    & " SELECT tblOut2.Item, SUM(tblOut2.Qnty) AS Total_Out FROM tblOut2" _
                    & " GROUP BY tblOut2.Item" _
                    & " ) ttlOut" _
                    & " ON tblItems.PrKey = ttlOut.Item" _
                    & " WHERE tblItems.PrKey = " & oItemSerial.SelectedValue
                Dim theQuant As Decimal
                Using cmd = New SqlCommand(NetQuery, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Using dt As SqlDataReader = cmd.ExecuteReader
                        If dt.Read() Then
                            theQuant = dt(5)
                        Else
                            theQuant = "00"
                        End If
                    End Using

                    'check the item if found in the grid
                    For x As Integer = 0 To oDgv.Rows.Count - 1
                        If (oDgv.Rows(x).Cells(2).Value.ToString.ToUpper = oItemSerial.Text.ToUpper) Then
                            theQuant -= oDgv.Rows(x).Cells(0).Value
                        End If
                    Next
                    lbNetQnty.Text = theQuant

                    myConn.Close()

                End Using
            Catch ex As Exception
                lbNetQnty.Text = "00"
            End Try
        Else
            lbNetQnty.Text = "00"
        End If

    End Sub

    Private Sub oQnty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles oQnty.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And oQnty.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If

    End Sub

    Private Sub oQnty_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles oQnty.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            oItemSerial.Focus()
        End If
    End Sub

    Private Sub oQnty_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oQnty.TextChanged
        oSubTotal.Text = Val(oQnty.Text) * Val(oUnitPrice.Text)
    End Sub

    Private Sub oUnitPrice_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles oUnitPrice.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And oUnitPrice.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If

    End Sub

    Private Sub oUnitPrice_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles oUnitPrice.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            oSubTotal.Focus()
        End If
    End Sub

    Private Sub oUnitPrice_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oUnitPrice.TextChanged
        oSubTotal.Text = Val(oQnty.Text) * Val(oUnitPrice.Text)
    End Sub

    Private Sub KryptonButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton8.Click

        If Val(oQnty.Text) = 0 Then
            MessageBox.Show("ÌÃ» ≈œŒ«· ﬂ„Ì… ’ÕÌÕ…!!", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information)
            oQnty.Focus()
            oQnty.SelectAll()
        ElseIf oItemSerial.Text = "" Then
            MessageBox.Show("ÌÃ» ≈œŒ«· ﬂÊœ ’ÕÌÕ!", "Invalid Serial", MessageBoxButtons.OK, MessageBoxIcon.Information)
            oItemSerial.Focus()
            oItemSerial.SelectAll()
        ElseIf oItemName.Text = "" Then
            MessageBox.Show("ÌÃ» ≈œŒ«· «”„ ’‰› ’ÕÌÕ!", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            oItemName.Focus()
            oItemName.SelectAll()
        ElseIf Val(oQnty.Text) > Val(lbNetQnty.Text) Then
            MessageBox.Show("«·ﬂ„Ì… «· Ì ﬁ„  »≈œŒ«·Â« √ﬂ»— „‰ «·—’Ìœ «·„ÊÃÊœ!", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information)
            oQnty.Focus()
            oQnty.SelectAll()
        Else

            ' We need to find the total available value of every item


            'check if item already exists
            Dim ExistingItem As Boolean
            Using cmd = New SqlCommand("SELECT PrKey FROM tblItems WHERE Serial = '" & oItemSerial.Text & "' AND Name = N'" & oItemName.Text & "'", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() Then
                        ExistingItem = True
                    Else
                        ExistingItem = False
                    End If
                End Using

                myConn.Close()

            End Using

            If ExistingItem = False Then
                MessageBox.Show("«·’‰› «·–Ì ﬁ„  »≈œŒ«·Â €Ì— „ÊÃÊœ!", "Invalid Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                oItemSerial.Focus()
                oItemSerial.SelectAll()
            Else
                Dim prQnty As Decimal
                For x As Integer = 0 To oDgv.RowCount - 1
                    ' If Not x = cr Then


                    If (oDgv.Rows(x).Cells(2).Value.ToString.ToUpper = oItemSerial.Text.ToUpper) AndAlso (oDgv.Rows(x).Cells(3).Value.ToString.ToUpper = oItemName.Text.ToUpper) Then
                        prQnty = oDgv.Rows(x).Cells(0).Value

                        If Val(oQnty.Text) > Val(lbNetQnty.Text) Then
                            MessageBox.Show("«·ﬂ„Ì… «· Ì ﬁ„  »≈œŒ«·Â« √ﬂ»— „‰ «·—’Ìœ «·„ÊÃÊœ!", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            oQnty.Focus()
                            oQnty.SelectAll()
                            Exit Sub
                        Else
                            oDgv.Rows(x).SetValues((Val(oQnty.Text) + prQnty), "Elm.", oItemSerial.Text.ToUpper, oItemName.Text.ToUpper, Val(oUnitPrice.Text), (Val(oUnitPrice.Text) * (Val(oQnty.Text) + prQnty)))
                            Call Notification("New quantity added!")
                            Call OutTotalize()
                            oQnty.Text = "1"
                            oItemSerial.Text = Nothing
                            oItemName.Text = Nothing
                            oUnitPrice.Text = ""
                            oSubTotal.Text = ""
                            oDgv.FirstDisplayedScrollingRowIndex = x
                            oDgv.ClearSelection()

                            oDgv.Rows(x).Selected = True
                            oItemSerial.Focus()
                            oItemSerial.SelectAll()
                            Exit Sub
                        End If



                    End If

                Next

                'Add the row to the datagrid
                oDgv.Rows.Add(Val(oQnty.Text), "Elm.", oItemSerial.Text.ToUpper, oItemName.Text.ToUpper, Val(oUnitPrice.Text), Val(oSubTotal.Text))
                Call Notification("New item added!")
                oQnty.Text = "1"
                oItemSerial.Text = Nothing
                oItemName.Text = Nothing
                oUnitPrice.Text = ""
                oSubTotal.Text = ""
                oDgv.FirstDisplayedScrollingRowIndex = oDgv.RowCount - 1
                oDgv.ClearSelection()

                oDgv.Rows(oDgv.RowCount - 1).Selected = True
                oItemSerial.Focus()
                oItemSerial.SelectAll()

            End If
        End If


        Call OutTotalize()

    End Sub

    Private Sub oItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles oItemName.LostFocus

        If Not oItemName.Text = "" Then
            Using cmd = New SqlCommand("SELECT Serial FROM tblItems WHERE Name = N'" & oItemName.Text & "'", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() Then
                        oItemSerial.Text = dt(0).ToString
                    Else
                        oItemSerial.Text = Nothing
                    End If
                End Using
                myConn.Close()
            End Using
        Else
            oItemSerial.Text = Nothing
        End If


        'enter the value
        'oCompound.Visible = True
        Dim Query As String
        Query = "SELECT Price FROM tblItems WHERE Serial = '" & oItemSerial.Text & "' AND Name = N'" & oItemName.Text & "'"
        Using cmd As New SqlCommand(Query, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Using dt As SqlDataReader = cmd.ExecuteReader
                If dt.Read() Then
                    oUnitPrice.Text = dt(0)
                Else
                    oUnitPrice.Text = ""
                End If
            End Using

            myConn.Close()
        End Using
    End Sub

    Private Sub oItemName_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles oItemName.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            oUnitPrice.Focus()
        End If
    End Sub

    Private Sub oItemName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oItemName.SelectedIndexChanged

        'enter the value

        'oCompound.Visible = True
        Dim Query As String
        Query = "SELECT Price FROM tblItems WHERE Name = N'" & oItemName.Text & "'"
        Using cmd As New SqlCommand(Query, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Using dtt As SqlDataReader = cmd.ExecuteReader
                If dtt.Read() Then
                    oUnitPrice.Text = dtt(0)
                Else
                    oUnitPrice.Text = ""
                End If
            End Using


            myConn.Close()

        End Using

        Try
            'Show the Net Qnty
            Dim NetQuery As String
            NetQuery = "SELECT tblItems.PrKey, tblItems.Serial, tblItems.Name, ttlIn.Total_In, COALESCE(ttlOut.Total_Out , 0) AS Total_Out,  (ttlIn.Total_In - COALESCE(ttlOut.Total_Out , 0)) AS Net_Amount" _
                & " FROM tblItems" _
                & " LEFT OUTER JOIN" _
                & " (" _
                & " SELECT tblIn2.Item, SUM(tblIn2.Qnty) AS Total_In FROM tblIn2" _
                & " GROUP BY tblIn2.Item" _
                & " ) ttlIn" _
                & " ON tblItems.PrKey = ttlIn.Item" _
                & " LEFT OUTER JOIN" _
                & " (" _
                & " SELECT tblOut2.Item, SUM(tblOut2.Qnty) AS Total_Out FROM tblOut2" _
                & " GROUP BY tblOut2.Item" _
                & " ) ttlOut" _
                & " ON tblItems.PrKey = ttlOut.Item" _
                & " WHERE tblItems.Name = N'" & oItemName.Text & "'"

            Dim theQuant As Decimal
            Using cmd = New SqlCommand(NetQuery, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() Then
                        theQuant = dt(5)
                    Else
                        theQuant = "00"
                    End If
                End Using

                'check the item if found in the grid
                For x As Integer = 0 To oDgv.Rows.Count - 1
                    If (oDgv.Rows(x).Cells(2).Value.ToString.ToUpper = oItemSerial.Text.ToUpper) Then
                        theQuant -= oDgv.Rows(x).Cells(0).Value
                    End If
                Next
                lbNetQnty.Text = theQuant

                myConn.Close()

            End Using
        Catch ex As Exception
            lbNetQnty.Text = "00"
        End Try

    End Sub

    Private Sub oDgv_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles oDgv.CellClick
        If oDgv.CurrentRow.Cells(1).Value = "Elm." Then
            oDgv.CurrentRow.Selected = True
        Else
            Dim comName As String
            comName = oDgv.CurrentRow.Cells(2).Value

            For Each oRow As DataGridViewRow In oDgv.Rows
                If oRow.Cells(2).Value = comName Then
                    oRow.Selected = True
                End If
            Next
        End If

        ''
        oQnty.Text = oDgv.CurrentRow.Cells(0).Value
        oItemSerial.Text = oDgv.CurrentRow.Cells(2).Value
        oItemName.Text = oDgv.CurrentRow.Cells(3).Value
        oUnitPrice.Text = oDgv.CurrentRow.Cells(4).Value
        oSubTotal.Text = oDgv.CurrentRow.Cells(5).Value


    End Sub

    Private Sub KryptonButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton4.Click
        oQnty.Text = "1"
        oItemSerial.Text = Nothing
        oItemName.Text = Nothing
        tbPaid.Text = ""
        oUnitPrice.Text = ""
        oSubTotal.Text = ""
        oDgv.Rows.Clear()
        oQnty.Focus()
        oQnty.SelectAll()
        oCustomer.Text = Nothing

    End Sub

    Private Sub oEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oEdit.Click
        If Not oDgv.RowCount = 0 Then
            If oEdit.Text = " ’ÕÌÕ" Then
                KryptonButton8.Enabled = False
                oRemove.Enabled = False
                oEdit.Text = " „"

                oQnty.Text = oDgv.CurrentRow.Cells(0).Value
                oItemSerial.Text = oDgv.CurrentRow.Cells(2).Value
                oItemName.Text = oDgv.CurrentRow.Cells(3).Value
                oUnitPrice.Text = oDgv.CurrentRow.Cells(4).Value
                oSubTotal.Text = oDgv.CurrentRow.Cells(5).Value

            Else
                'modifiy the selected tow

                If Val(oQnty.Text) = 0 Then
                    MessageBox.Show("ÌÃ» ≈œŒ«· ﬂ„Ì… ’ÕÌÕ…!", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    oQnty.Focus()
                    oQnty.SelectAll()
                ElseIf oItemSerial.Text = "" Then
                    MessageBox.Show("ÌÃ» ≈œŒ«· ﬂÊœ ’ÕÌÕ!", "Invalid Serial", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    oItemSerial.Focus()
                    oItemSerial.SelectAll()
                ElseIf oItemName.Text = "" Then
                    MessageBox.Show("ÌÃ» ≈œŒ«· «”’„ ’‰› ’ÕÌÕ!", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    oItemName.Focus()
                    oItemName.SelectAll()
                ElseIf Val(oUnitPrice.Text) <= 0 Then
                    MessageBox.Show("ÌÃ» ≈œŒ«· ”⁄— ÊÕœ… ’ÕÌÕ!", "Invalid Unit Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    oUnitPrice.Focus()
                    oUnitPrice.SelectAll()
                ElseIf Val(oSubTotal.Text) <= 0 Then
                    MessageBox.Show("ÌÃ» ≈œŒ«· ﬁÌ„… ’ÕÌÕ…!", "Invalid Sub Total", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    oSubTotal.Focus()
                    oSubTotal.SelectAll()
                Else

                    ' We need to find the total available value of every item
                    'check if item already exists
                    Dim ExistingItem As Boolean
                    Using cmd = New SqlCommand("SELECT PrKey FROM tblItems WHERE Serial = '" & oItemSerial.Text & "' AND Name = N'" & oItemName.Text & "'", myConn)
                        If myConn.State = ConnectionState.Closed Then
                            myConn.Open()
                        End If
                        Using dt As SqlDataReader = cmd.ExecuteReader
                            If dt.Read() Then
                                ExistingItem = True
                            Else
                                ExistingItem = False
                            End If
                        End Using

                        myConn.Close()
                    End Using

                    If ExistingItem = False Then
                        MessageBox.Show("«·’‰› «·–Ì ﬁ„  »≈œŒ«·Â €Ì— „ÊÃÊœ!", "Invalid Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        oItemSerial.Focus()
                        oItemSerial.SelectAll()
                    Else
                        'check if the item has been added to the grid before
                        Dim addedBefore As Boolean = False
                        If oDgv.CurrentRow.Cells(2).Value <> oItemSerial.Text Then
                            For x As Integer = 0 To oDgv.RowCount - 1
                                If oDgv.Rows(x).Cells(2).Value = oItemSerial.Text And oDgv.Rows(x).Cells(3).Value = oItemName.Text Then
                                    addedBefore = True
                                    MessageBox.Show("·ﬁœ ﬁ„  »«·›⁄· »≈œŒ«· Â–« «·’‰› „‰ ﬁ»·!", "Duplicated Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Exit Sub
                                End If
                            Next
                        End If

                        Dim cr As Integer = oDgv.CurrentRow.Index

                        For x As Integer = 0 To oDgv.RowCount - 1

                            If oDgv.Rows(x).Cells(1).Value = "Elm." And Not cr = x Then
                                If (oDgv.Rows(x).Cells(2).Value.ToString.ToUpper = oItemSerial.Text.ToUpper) AndAlso (oDgv.Rows(x).Cells(3).Value.ToString.ToUpper = oItemName.Text.ToUpper) Then
                                    MessageBox.Show("·ﬁœ  „ ≈œŒ«· Â–« «·’‰› „‰ ﬁ»·!", "Duplicate Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    oDgv.ClearSelection()
                                    oDgv.Rows(x).Selected = True
                                    oItemSerial.Focus()
                                    oItemSerial.SelectAll()
                                    Return
                                End If
                            End If
                        Next


                        'Add the row to the datagrid
                        Dim theRow As String()
                        theRow = New String() {Val(oQnty.Text), "Elm.", oItemSerial.Text.ToUpper, oItemName.Text.ToUpper, Val(oUnitPrice.Text), Val(oSubTotal.Text)}

                        oDgv.CurrentRow.SetValues(theRow)

                        oEdit.Text = " ’ÕÌÕ"
                        KryptonButton8.Enabled = True
                        oRemove.Enabled = True

                        Call Notification("Item modified!")
                        oQnty.Text = "1"
                        oItemSerial.Text = Nothing
                        oItemName.Text = Nothing
                        oUnitPrice.Text = ""
                        oSubTotal.Text = ""

                        oQnty.Focus()
                        Call OutTotalize()
                        oQnty.SelectAll()
                    End If
                End If

            End If
        End If


    End Sub

    Private Sub oRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oRemove.Click
        If Not oDgv.RowCount = 0 Then
            Dim dia As DialogResult
            dia = MessageBox.Show("Â·  —Ìœ »«· √ﬂÌœ Õ–› «·’› «·Õ«·Ìø", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If dia = Windows.Forms.DialogResult.Yes Then
                oDgv.Rows.Remove(oDgv.CurrentRow)

                'update the discount 
                Call OutTotalize()
                oQnty.Text = "1"
                oItemSerial.Text = Nothing
                oItemName.Text = Nothing
                oUnitPrice.Text = ""
                oSubTotal.Text = ""

                oQnty.Focus()
                oQnty.SelectAll()
                Notification("Current item removed!")
                Call OutTotalize()
            End If

        End If
    End Sub

    Private Sub oDgv_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles oDgv.RowHeaderMouseClick
        oQnty.Text = oDgv.CurrentRow.Cells(0).Value
        oItemSerial.Text = oDgv.CurrentRow.Cells(2).Value
        oItemName.Text = oDgv.CurrentRow.Cells(3).Value
        oUnitPrice.Text = oDgv.CurrentRow.Cells(4).Value
        oSubTotal.Text = oDgv.CurrentRow.Cells(5).Value
    End Sub

    Private Sub KryptonButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton5.Click

        ''''''''''''''''
        'for demo
        If GV.appDemo = True Then
            Using cmd = New SqlCommand("SELECT COUNT(Serial) FROM tblOut1", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        If dr(0) > 700 Then
                            Exit Sub
                        End If
                    End If
                End Using

                myConn.Close()
            End Using
        End If
        '''''''''''''''''''

Restart:
        If KryptonButton5.Text = "Õ›Ÿ" Then

            If oDgv.RowCount = 0 Then
                MessageBox.Show("·«  ÊÃœ √Ì »Ì«‰«  ·Ì „ Õ›ŸÂ«!", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                oQnty.Focus()
            ElseIf Val(tbPaid.Text) = 0 Then
                MessageBox.Show("·„ Ì „ ≈œÕ«· «·„»·€ «·„œ›Ê⁄!", "No Payment", MessageBoxButtons.OK, MessageBoxIcon.Information)
                tbPaid.Focus()
            ElseIf Val(tbRest.Text) < 0 Then
                MessageBox.Show("«·„»·€ «·„œ›Ê€ €Ì— ’ÕÌÕ!", "Wrong Payment", MessageBoxButtons.OK, MessageBoxIcon.Information)
                tbPaid.Focus()
                tbPaid.SelectAll()
            Else


                'check if the valid qnty
                Call CheckQnty()
                If ValidQnty = False Then
                    Exit Sub
                End If

                'add to out1
                Dim Query As String = "INSERT INTO tblOut1 ([Date], [Time], Customer, LaborOverhaul, Transfers, SubTotal, Discount, SaleTax, NetTotal, [User]) " _
                                      & "VALUES ('" & oDate.Value.ToString("MM/dd/yyyy") & "', '" & oTime.Value.ToString("HH:mm") & "', N'" & oCustomer.Text.ToUpper & "', '" & "0" & "', '" & "0" & "', '" & tbTotal.Text & "', '" & Val(tbPaid.Text) & "', '" & "0" & "', '" & tbRest.Text & "', '" & GlobalVariables.ID & "')"

                Using cmd = New SqlCommand(Query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    cmd.ExecuteNonQuery()

                    myConn.Close()

                End Using

                'Show the Invoice Serial
                Query = "select Serial from tblOut1 " _
                    & "WHERE [Date] = '" & oDate.Value.ToString("MM/dd/yyyy") & "' AND [Time] = '" & oTime.Value.ToString("HH:mm") & "' " _
                    & "AND Customer = N'" & oCustomer.Text & "' " _
                    & "AND SubTotal = '" & tbTotal.Text & "' AND Discount = '" & tbPaid.Text & "' " _
                    & "AND NetTotal = '" & tbRest.Text & "'"
                Using cmd = New SqlCommand(Query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Using dt As SqlDataReader = cmd.ExecuteReader
                        If dt.Read() Then
                            OSerial.Text = dt(0).ToString
                        End If
                    End Using

                    myConn.Close()

                End Using
                'Add to Out2
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim line1, line2, line3, line4 As String
                Dim Code, Discr As String
                For Each oRow As DataGridViewRow In oDgv.Rows

                    If oRow.Cells(1).Value = "Elm." Then
                        line3 = "SELECT PrKey FROM tblItems WHERE Serial = '" & oRow.Cells(2).Value & "' AND Name = N'" & oRow.Cells(3).Value & "'"
                        Using cmd = New SqlCommand(line3, myConn)
                            Using dr As SqlDataReader = cmd.ExecuteReader
                                If dr.Read() Then
                                    Code = dr(0).ToString
                                Else
                                    Code = ""
                                End If
                            End Using
                        End Using


                        line1 = "INSERT INTO tblOut2 (Serial, Kind, Item, Compound, Qnty, Price, UnitPrice, [User])" _
                            & " VALUES ('" & OSerial.Text & "', 'Elm.', '" & Code & "', '0', '" & oRow.Cells(0).Value & "', '" & oRow.Cells(5).Value & "', '" & oRow.Cells(4).Value & "', '" & GlobalVariables.ID & "')"
                        Using cmd = New SqlCommand(line1, myConn)
                            cmd.ExecuteNonQuery()
                        End Using
                    Else
                        line4 = "SELECT tblCompounds.Serial, tblCompounds.Name, tblCompounds.Item, tblItems.Name AS [Item Name] FROM tblCompounds" _
                            & " INNER JOIN tblItems" _
                            & " ON tblItems.PrKey = tblCompounds.Item" _
                            & " WHERE tblCompounds.Name = N'" & oRow.Cells(2).Value & "'" _
                            & " AND tblItems.Name = N'" & oRow.Cells(3).Value & "'"

                        Using cmd = New SqlCommand(line4, myConn)
                            Using dr As SqlDataReader = cmd.ExecuteReader
                                If dr.Read() Then
                                    Code = dr(0).ToString
                                    Discr = dr(2).ToString
                                Else
                                    Code = ""
                                    Discr = ""
                                End If
                            End Using
                        End Using
                        ' MsgBox(Code & " " & Discr)

                        line2 = "INSERT INTO tblOut2 (Serial, Kind, Item, Compound, Qnty, Price, UnitPrice, [User])" _
                            & " VALUES ('" & OSerial.Text & "', 'Cmp.', '" & Discr & "', '" & Code & "', '" & oRow.Cells(0).Value & "', '" & oRow.Cells(5).Value & "', '" & oRow.Cells(4).Value & "', '" & GlobalVariables.ID & "')"

                        'My.Computer.Clipboard.GetText(line2.ToString)
                        Using cmd = New SqlCommand(line2, myConn)
                            cmd.ExecuteNonQuery()
                        End Using
                    End If
                Next

                myConn.Close()
                'clear
                oQnty.Text = "1"
                oItemSerial.Text = Nothing
                oItemName.Text = Nothing
                tbPaid.Text = ""
                oUnitPrice.Text = ""
                oSubTotal.Text = ""
                oDgv.Rows.Clear()
                oQnty.Focus()
                oQnty.SelectAll()
                oCustomer.Text = Nothing
                oDate.Value = Today
                oTime.Value = Now
                Call Notification("Invoice Added")


                'Update the Customer combobox

                Using cmd = New SqlCommand("SELECT DISTINCT Customer FROM tblOut1 ORDER BY Customer", myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Dim adt As New SqlDataAdapter
                    Dim ds As New DataSet()
                    adt.SelectCommand = cmd
                    adt.Fill(ds)
                    adt.Dispose()

                    oCustomer.DataSource = ds.Tables(0)
                    oCustomer.DisplayMember = "Customer"
                    oCustomer.Text = Nothing
                    oCustomer.Focus()


                    myConn.Close()

                End Using

            End If
        ElseIf KryptonButton5.Text = " ⁄œÌ·" Then
            If oDgv.RowCount = 0 Then
                MessageBox.Show("·«  ÊÃœ √Ì »Ì«‰«  ·Ì „ Õ›ŸÂ«!", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                oQnty.Focus()
            ElseIf Val(tbPaid.Text) = 0 Then
                MessageBox.Show("·„ Ì „ ≈œÕ«· «·„»·€ «·„œ›Ê⁄!", "No Payment", MessageBoxButtons.OK, MessageBoxIcon.Information)
                tbPaid.Focus()
            ElseIf Val(tbRest.Text) < 0 Then
                MessageBox.Show("«·„»·€ «·„œ›Ê€ €Ì— ’ÕÌÕ!", "Wrong Payment", MessageBoxButtons.OK, MessageBoxIcon.Information)
                tbPaid.Focus()
                tbPaid.SelectAll()
            Else

                'check if the rate is found
                Dim rateFound As Boolean
                Using cmd = New SqlCommand("SELECT * FROM tblRate WHERE [Day] = '" & oDate.Value.ToString("MM/dd/yyyy") & "'", myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            rateFound = True
                        Else
                            rateFound = False
                        End If
                    End Using

                    myConn.Close()

                End Using

                If rateFound = False Then
                    Call AutoRate(ieDate.Value)
                    GoTo restart
                Else

                    'check if the valid qnty
                    Call CheckQnty()
                    If ValidQnty = False Then
                        Exit Sub
                    End If

                    'update out1
                    Dim Query As String = "UPDATE tblOut1 SET [Date] = '" & oDate.Value.ToString("MM/dd/yyyy") & "', [Time] = '" & oTime.Value.ToString("HH:mm") & "', Customer = N'" & oCustomer.Text & "', Subtotal = '" & tbTotal.Text & "', Discount = '" & tbPaid.Text & "', NetTotal = '" & tbRest.Text & "', [User] = '" & GlobalVariables.ID & "'" _
                                          & " WHERE Serial = '" & oSearch.Text & "'"

                    Using cmd = New SqlCommand(Query, myConn)
                        If myConn.State = ConnectionState.Closed Then
                            myConn.Open()
                        End If
                        cmd.ExecuteNonQuery()

                        myConn.Close()

                    End Using

                    'delete the old data from tblOut2
                    Using cmd = New SqlCommand("DELETE FROM tblOut2 WHERE Serial = '" & oSearch.Text & "'", myConn)
                        If myConn.State = ConnectionState.Closed Then
                            myConn.Open()
                        End If
                        cmd.ExecuteNonQuery()

                        myConn.Close()
                    End Using
                    'Add to Out2
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Dim line1, line2, line3, line4 As String
                    Dim Code, Discr As String
                    For Each oRow As DataGridViewRow In oDgv.Rows

                        If oRow.Cells(1).Value = "Elm." Then
                            line3 = "SELECT PrKey FROM tblItems WHERE Serial = '" & oRow.Cells(2).Value & "' AND Name = N'" & oRow.Cells(3).Value & "'"
                            Using cmd = New SqlCommand(line3, myConn)
                                Using dr As SqlDataReader = cmd.ExecuteReader
                                    If dr.Read() Then
                                        Code = dr(0).ToString
                                    Else
                                        Code = ""
                                    End If
                                End Using
                            End Using


                            line1 = "INSERT INTO tblOut2 (Serial, Kind, Item, Compound, Qnty, Price, UnitPrice, [User])" _
                                & " VALUES ('" & OSerial.Text & "', 'Elm.', '" & Code & "', '0', '" & oRow.Cells(0).Value & "', '" & oRow.Cells(5).Value & "', '" & oRow.Cells(4).Value & "', '" & GlobalVariables.ID & "')"
                            Using cmd = New SqlCommand(line1, myConn)
                                cmd.ExecuteNonQuery()
                            End Using
                        Else
                            line4 = "SELECT tblCompounds.Serial, tblCompounds.Name, tblCompounds.Item, tblItems.Name AS [Item Name] FROM tblCompounds" _
                                & " INNER JOIN tblItems" _
                                & " ON tblItems.PrKey = tblCompounds.Item" _
                                & " WHERE tblCompounds.Name = '" & oRow.Cells(2).Value & "'" _
                                & " AND tblItems.Name = N'" & oRow.Cells(3).Value & "'"

                            Using cmd = New SqlCommand(line4, myConn)
                                Using dr As SqlDataReader = cmd.ExecuteReader
                                    If dr.Read() Then
                                        Code = dr(0).ToString
                                        Discr = dr(2).ToString
                                    Else
                                        Code = ""
                                        Discr = ""
                                    End If
                                End Using
                            End Using
                            ' MsgBox(Code & " " & Discr)

                            line2 = "INSERT INTO tblOut2 (Serial, Kind, Item, Compound, Qnty, Price, UnitPrice, [User])" _
                                & " VALUES ('" & OSerial.Text & "', 'Cmp.', '" & Discr & "', '" & Code & "', '" & oRow.Cells(0).Value & "', '" & oRow.Cells(5).Value & "', '" & oRow.Cells(4).Value & "', '" & GlobalVariables.ID & "')"
                            MsgBox(line2)
                            'My.Computer.Clipboard.GetText(line2)
                            Using cmd = New SqlCommand(line2, myConn)
                                cmd.ExecuteNonQuery()
                            End Using
                        End If
                    Next

                    myConn.Close()
                    'clear the items
                    oQnty.Text = "1"
                    oItemSerial.Text = Nothing
                    tbPaid.Text = ""
                    oItemName.Text = Nothing
                    oUnitPrice.Text = ""
                    oSubTotal.Text = ""
                    oDgv.Rows.Clear()
                    oQnty.Focus()
                    oQnty.SelectAll()
                    oCustomer.Text = Nothing
                    oDate.Value = Today
                    oTime.Value = Now
                    Call Notification("Invoice Modified")


                    'Update the Customer combobox

                    Using cmd = New SqlCommand("SELECT DISTINCT Customer FROM tblOut1 ORDER BY Customer", myConn)
                        If myConn.State = ConnectionState.Closed Then
                            myConn.Open()
                        End If
                        Dim adt As New SqlDataAdapter
                        Dim ds As New DataSet()
                        adt.SelectCommand = cmd
                        adt.Fill(ds)
                        adt.Dispose()

                        oCustomer.DataSource = ds.Tables(0)
                        oCustomer.DisplayMember = "Customer"
                        oCustomer.Text = Nothing
                        oCustomer.Focus()


                        myConn.Close()
                    End Using
                End If
            End If
        ElseIf KryptonButton5.Text = "Õ–›" Then
            Dim diaR As DialogResult
            diaR = MessageBox.Show("Â·  —Ìœ »«· √ﬂÌœ Õ–› «·›« Ê—… «·Õ«·Ì…ø ·‰  ” ÿÌ⁄ «” —Ã«⁄Â« „—… √Œ—Ï!", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If diaR = Windows.Forms.DialogResult.Yes Then
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If

                'Delete from Out2
                Using cmd = New SqlCommand("DELETE FROM tblOut2 WHERE Serial = " & Val(oSearch.Text), myConn)
                    cmd.ExecuteNonQuery()
                End Using

                'Delete from Out1
                Using cmd = New SqlCommand("DELETE FROM tblOut1 WHERE Serial = " & Val(oSearch.Text), myConn)
                    cmd.ExecuteNonQuery()
                End Using



                myConn.Close()


                'Update the Customer combobox

                Using cmd = New SqlCommand("SELECT DISTINCT Customer FROM tblOut1 ORDER BY Customer", myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Dim adt As New SqlDataAdapter
                    Dim ds As New DataSet()
                    adt.SelectCommand = cmd
                    adt.Fill(ds)
                    adt.Dispose()

                    oCustomer.DataSource = ds.Tables(0)
                    oCustomer.DisplayMember = "Customer"
                    oCustomer.Text = Nothing
                    oCustomer.Focus()

                    myConn.Close()
                End Using

                'Update the Search combobox

                Using cmd = New SqlCommand("SELECT Serial FROM tblOut1 ORDER BY Serial", myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Dim adt As New SqlDataAdapter
                    Dim ds As New DataSet()
                    adt.SelectCommand = cmd
                    adt.Fill(ds)
                    adt.Dispose()

                    oSearch.DataSource = ds.Tables(0)
                    oSearch.DisplayMember = "Serial"
                    oSearch.Text = Nothing
                    krSerial.DataSource = ds.Tables(0)
                    krSerial.DisplayMember = "Serial"
                    krSerial.Text = Nothing

                    myConn.Close()

                End Using

                'clear the items
                oQnty.Text = "1"
                oItemSerial.Text = Nothing
                tbPaid.Text = Nothing
                oItemName.Text = Nothing
                oUnitPrice.Text = ""
                oSubTotal.Text = ""
                oDgv.Rows.Clear()
                oQnty.Focus()
                oQnty.SelectAll()
                oCustomer.Text = Nothing
                oDate.Value = Today
                oTime.Value = Now
                Call Notification("Invoice Deleted!")

            End If

        End If
    End Sub

    Private Sub oCustomer_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles oCustomer.LostFocus
        oCustomer.Text = oCustomer.Text.ToUpper
    End Sub

    Private Sub oCustomer_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles oCustomer.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            oDate.Focus()
        End If
    End Sub

    Private Sub oCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oCustomer.SelectedIndexChanged
        If oCustomer.Text = "" Then
            KryptonPanel21.Visible = False
        Else
            KryptonPanel21.Visible = True
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Try
                Using cmd = New SqlCommand("SELECT TOP(1) [Date] FROM tblOut1 WHERE Customer = N'" & oCustomer.Text & "' ORDER BY [Date] DESC", myConn)
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            KryptonLabel52.Text = dr(0)
                        Else
                            KryptonLabel52.Text = ""
                        End If
                    End Using
                End Using

                Using cmd = New SqlCommand("SELECT SUM(SubTotal) AS Amount FROM tblOut1 WHERE Customer = N'" & oCustomer.Text & "'", myConn)
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            KryptonLabel44.Text = dr(0)
                        Else
                            KryptonLabel44.Text = "00"
                        End If
                    End Using
                End Using

            Catch ex As Exception

            End Try
            myConn.Close()
        End If
    End Sub

    Private Sub oAddNew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oAddNew.CheckedChanged
        If oAddNew.Checked = True Then
            KryptonButton5.Text = "Õ›Ÿ"
            iSearch.Visible = False

            'clear the items
            oQnty.Text = "1"
            oItemSerial.Text = Nothing
            oItemName.Text = Nothing
            oUnitPrice.Text = ""
            oSubTotal.Text = ""
            oDgv.Rows.Clear()
            oQnty.Focus()
            oQnty.SelectAll()
            oCustomer.Text = Nothing

            oSearch.Visible = False
            oDate.Value = Today
            oTime.Value = Now
        ElseIf oModify.Checked = True Then
            KryptonButton5.Text = " ⁄œÌ·"
            oSearch.Visible = True

            'fill the combobox
            Using cmd = New SqlCommand("SELECT DISTINCT Serial FROM tblOut1 ORDER BY Serial DESC", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                oSearch.DataSource = ds.Tables(0)
                oSearch.DisplayMember = "Serial"
                oSearch.Text = Nothing
                krSerial.DataSource = ds.Tables(0)
                krSerial.DisplayMember = "Serial"
                krSerial.Text = Nothing


                myConn.Close()

            End Using


        Else
            KryptonButton5.Text = "Õ–›"
            oSearch.Visible = True

            Using cmd = New SqlCommand("SELECT DISTINCT Serial FROM tblOut1 ORDER BY Serial DESC", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                oSearch.DataSource = ds.Tables(0)
                oSearch.DisplayMember = "Serial"
                oSearch.Text = Nothing
                krSerial.DataSource = ds.Tables(0)
                krSerial.DisplayMember = "Serial"
                krSerial.Text = Nothing

                myConn.Close()


            End Using

        End If
    End Sub

    Private Sub oModify_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oModify.CheckedChanged
        If oAddNew.Checked = True Then
            KryptonButton5.Text = "Õ›Ÿ"
            iSearch.Visible = False

            'clear the items
            oQnty.Text = "1"
            oItemSerial.Text = Nothing
            oItemName.Text = Nothing
            oUnitPrice.Text = ""
            oSubTotal.Text = ""
            oDgv.Rows.Clear()
            oQnty.Focus()
            oQnty.SelectAll()
            oCustomer.Text = Nothing

            oSearch.Visible = False
        ElseIf oModify.Checked = True Then
            KryptonButton5.Text = " ⁄œÌ·"
            oSearch.Visible = True

            'fill the combobox
            Using cmd = New SqlCommand("SELECT DISTINCT Serial FROM tblOut1 ORDER BY Serial DESC", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                oSearch.DataSource = ds.Tables(0)
                oSearch.DisplayMember = "Serial"
                oSearch.Text = Nothing

                krSerial.DataSource = ds.Tables(0)
                krSerial.DisplayMember = "Serial"
                krSerial.Text = Nothing


                myConn.Close()

            End Using


        Else
            KryptonButton5.Text = "Õ–›"
            oSearch.Visible = True

            Using cmd = New SqlCommand("SELECT DISTINCT Serial FROM tblOut1 ORDER BY Serial ORDER BY Serial DESC", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                oSearch.DataSource = ds.Tables(0)
                oSearch.DisplayMember = "Serial"
                oSearch.Text = Nothing
                krSerial.DataSource = ds.Tables(0)
                krSerial.DisplayMember = "Serial"
                krSerial.Text = Nothing

                myConn.Close()


            End Using

        End If
    End Sub

    Private Sub oDelete_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oDelete.CheckedChanged
        If oAddNew.Checked = True Then
            KryptonButton5.Text = "Õ›Ÿ"
            iSearch.Visible = False

            'clear the items
            oQnty.Text = "1"
            oItemSerial.Text = Nothing
            oItemName.Text = Nothing
            oUnitPrice.Text = ""
            oSubTotal.Text = ""
            oDgv.Rows.Clear()
            oQnty.Focus()
            oQnty.SelectAll()
            oCustomer.Text = Nothing
            oSearch.Visible = False
        ElseIf oModify.Checked = True Then
            KryptonButton5.Text = " ⁄œÌ·"
            oSearch.Visible = True

            'fill the combobox
            Using cmd = New SqlCommand("SELECT DISTINCT Serial FROM tblOut1 ORDER BY Serial DESC", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                oSearch.DataSource = ds.Tables(0)
                oSearch.DisplayMember = "Serial"
                oSearch.Text = Nothing
                krSerial.DataSource = ds.Tables(0)
                krSerial.DisplayMember = "Serial"
                krSerial.Text = Nothing

                myConn.Close()

            End Using


        Else
            KryptonButton5.Text = "Õ–›"
            oSearch.Visible = True

            Using cmd = New SqlCommand("SELECT DISTINCT Serial FROM tblOut1 ORDER BY Serial DESC", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                oSearch.DataSource = ds.Tables(0)
                oSearch.DisplayMember = "Serial"
                oSearch.Text = Nothing
                krSerial.DataSource = ds.Tables(0)
                krSerial.DisplayMember = "Serial"
                krSerial.Text = Nothing

                myConn.Close()


            End Using

        End If
    End Sub

    Private Sub oSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oSearch.SelectedIndexChanged
        If Not oSearch.Text = "" Then
            Dim Query1, Query2 As String

            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If

            Query1 = "SELECT * FROM tblOut1 WHERE Serial = " & Val(oSearch.Text)
            Try
                Using cmd = New SqlCommand(Query1, myConn)
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            OSerial.Text = dr(0).ToString
                            oCustomer.Text = dr(4).ToString
                            oDate.Value = dr(1)
                            oTime.Value = dr(2)
                            tbTotal.Text = dr(7)
                            tbPaid.Text = dr(8)
                        End If
                    End Using
                End Using

            Catch ex As Exception

            End Try
            'oDgv.Rows.Clear()
            Query2 = "SELECT tblOut2.Serial, tblOut2.Kind, tblOut2.Item, tblItems.Serial AS ItemSerial, tblItems.Name AS ItemName, tblOut2.Compound, CASE WHEN (tblOut2.Compound = '' OR tblOut2.Compound = 0 ) THEN tblItems.Name ELSE tblCompounds.Name END AS CompoundName, tblOut2.Qnty, tblOut2.Price, tblOut2.UnitPrice" _
                & " FROM tblOut2 LEFT OUTER JOIN tblItems" _
                & " ON tblOut2.Item = tblItems.PrKey" _
                & " LEFT OUTER JOIN tblCompounds" _
                & " ON tblOut2.Compound = tblCompounds.Serial" _
                & " WHERE(tblOut2.Serial = " & Val(oSearch.Text) & ")" _
                & " ORDER BY tblOut2.PrKey"


            Try
                Using cmd = New SqlCommand(Query2, myConn)

                    Using dr As SqlDataReader = cmd.ExecuteReader
                        Dim dt As New DataTable
                        dt.Load(dr)
                        oDgv.Rows.Clear()
                        For x As Integer = 0 To dt.Rows.Count - 1
                            If dt(x)(1) = "Elm." Then
                                oDgv.Rows.Add(dt(x)(7), "Elm.", dt(x)(3), dt(x)(4), dt(x)(9), dt(x)(8))

                            ElseIf dt(x)(1) = "Cmp." Then
                                oDgv.Rows.Add(dt(x)(7), "Cmp.", dt(x)(6), dt(x)(4), dt(x)(9), dt(x)(8))
                            End If
                        Next
                    End Using


                    myConn.Close()

                End Using
            Catch ex As Exception

            End Try


            myConn.Close()

        Else
            oQnty.Text = "1"
            oItemSerial.Text = Nothing
            oItemName.Text = Nothing
            oUnitPrice.Text = ""
            oSubTotal.Text = ""
            oDgv.Rows.Clear()
            oQnty.Focus()
            oQnty.SelectAll()
            oCustomer.Text = Nothing
        End If
    End Sub


    Private Sub KryptonRibbonGroupButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonRibbonGroupButton1.Click
        Call RibbonClear()
        KryptonRibbonGroupButton1.Checked = True
        KryptonDockableNavigator1.SelectedIndex = 0
    End Sub

    Private Sub KryptonRibbonGroupButton2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonRibbonGroupButton2.Click
        Call RibbonClear()
        KryptonRibbonGroupButton2.Checked = True
        KryptonDockableNavigator1.SelectedIndex = 1
    End Sub



    Private Sub KryptonRibbonGroupButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonRibbonGroupButton8.Click
        frmCurrency.Show()
    End Sub

    Private Sub KryptonRibbonGroupButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonRibbonGroupButton3.Click
        Call RibbonClear()
        KryptonRibbonGroupButton3.Checked = True
        KryptonDockableNavigator1.SelectedIndex = 2
    End Sub

    Private Sub KryptonRibbonGroupButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonRibbonGroupButton4.Click
        Call RibbonClear()
        KryptonRibbonGroupButton4.Checked = True
        KryptonDockableNavigator1.SelectedIndex = 3
    End Sub

    Private Sub oDate_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles oDate.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            oTime.Focus()
        End If
    End Sub

    Private Sub oDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oDate.ValueChanged

    End Sub

    Private Sub oTime_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles oTime.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            oQnty.Focus()
        End If
    End Sub

    Private Sub oTime_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oTime.ValueChanged

    End Sub

    Private Sub krInvoiceShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles krInvoiceShow.Click
        ExReport.getReceipt(OSerial.Text, False)
    End Sub

    Private Sub krInvoicePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles krInvoicePrint.Click
        ExReport.getReceipt(OSerial.Text, True)
    End Sub

    Private Sub KryptonTextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbTotal.TextChanged
        Try
            tbRest.Text = Val(tbPaid.Text) - Val(tbTotal.Text)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub oCbComp_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs)
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            oItemSerial.Focus()
        End If
    End Sub

    Private Sub oSubTotal_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles oSubTotal.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            KryptonButton8.Focus()
        End If
    End Sub

    Private Sub oSubTotal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oSubTotal.TextChanged

    End Sub

    Private Sub krInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles krInvoice.Click
        ExReport.getReceipt(krSerial.Text, False)
    End Sub

    Private Sub krPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles krPrint.Click
        ExReport.getReceipt(krSerial.Text, True)
    End Sub

    Private Sub isSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles isSearch.Click
        Cursor = Cursors.WaitCursor
        Dim fDate, SDate As String
        If isDateFrom.Checked = True Then
            fDate = isDateFrom.Value.ToString("MM/dd/yyyy")
        Else
            fDate = "01/01/1999"
        End If
        If isDateTill.Checked = True Then
            SDate = isDateTill.Value.ToString("MM/dd/yyyy")
        Else
            SDate = "01/01/2500"
        End If

        Dim Vndr As String
        If isVendor.Text = "" Then
            Vndr = ""
        Else
            Vndr = " AND tblIn1.Vendor = '" & isVendor.SelectedValue & "'"
        End If

        Dim Query As String = "SELECT tblItems.Serial, tblItems.Name AS Item, SUM(tblIn2.Qnty) AS Qnty, AVG(tblIn2.UnitPrice) AS UnitAverage, SUM(tblIn2.[Value]) AS [Value]" _
                              & " FROM tblItems INNER JOIN tblIn2 ON tblItems.PrKey = tblIn2.Item" _
                              & " INNER JOIN tblIn1 ON tblIn1.PrKey = tblIn2.Serial" _
                              & " WHERE (tblIn1.[Date] BETWEEN '" & fDate & "' AND '" & SDate & "')" _
                              & Vndr _
                              & " GROUP BY tblItems.Serial, tblItems.Name" _
                              & " ORDER BY tblItems.Name"


        Dim ds As New ReportsDS
        Dim da As New SqlDataAdapter(Query, myConn)
        da.Fill(ds.Tables("tblIn"))

        Dim rep As New crIn
        rep.SetDataSource(ds.Tables("tblIn"))
        CrystalReportViewer2.ReportSource = rep

        If isVendor.Text = "" Then
            rep.SetParameterValue("ParVA", "")
            rep.SetParameterValue("ParVendor", "")
        Else
            rep.SetParameterValue("ParVA", ":«·„Ê“⁄")
            rep.SetParameterValue("ParVendor", isVendor.Text.ToUpper)
        End If

        If isDateFrom.Checked = True Then
            rep.SetParameterValue("ParDF", ":«·› —…")
            rep.SetParameterValue("ParDateFrom", isDateFrom.Value.ToString("dd/MM/yyyy"))
        Else
            rep.SetParameterValue("ParDF", "")
            rep.SetParameterValue("ParDateFrom", "")
        End If
        If isDateTill.Checked = True Then
            rep.SetParameterValue("ParDF", ":«·› —…")
            rep.SetParameterValue("ParDateTill", isDateTill.Value.ToString("dd/MM/yyyy"))
        Else
            rep.SetParameterValue("ParDateTill", "")
        End If
        Cursor = Cursors.Default
        CrystalReportViewer2.Refresh()
        CrystalReportViewer2.Zoom(1)

    End Sub

    Private Sub KryptonButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton9.Click
        Cursor = Cursors.WaitCursor
        Dim fDate, SDate As String
        If osDateFrom.Checked = True Then
            fDate = osDateFrom.Value.ToString("MM/dd/yyyy")
        Else
            fDate = "01/01/1999"
        End If
        If osDateTill.Checked = True Then
            SDate = osDateTill.Value.ToString("MM/dd/yyyy")
        Else
            SDate = "01/01/2500"
        End If

        Dim Cust As String
        If osCustomer.Text = "" Then
            Cust = ""
        Else
            Cust = " AND tblOut1.Customer = " & osCustomer.SelectedValue
        End If

        Dim exp As String
        If KryptonCheckBox2.Checked = False Then
            exp = " AND tblOut2.Price != 0 "
        Else
            exp = " AND tblOut2.Price = 0 "
        End If

        Dim itmm As String
        If osItem.Text <> "" Then
            itmm = " AND tblItems.Name LIKE N'%" & osItem.Text & "%'"
        Else
            itmm = ""
        End If

        Dim Query As String = "SELECT tblItems.Serial AS Code, tblItems.Name AS Item, QIn.UnitPrice, LIn.UnitPrice AS LastInPrice, AVG(tblOut2.UnitPrice) AS [Disc%], SUM(tblOut2.Qnty) AS Qnty, SUM(tblOut2.Price) AS [Value], (AVG(tblOut2.UnitPrice) - AVG(QIn.UnitPrice)) * SUM(tblOut2.Qnty) AS NetAVG, (AVG(tblOut2.UnitPrice) - AVG(LIn.UnitPrice)) * SUM(tblOut2.Qnty) AS Profit" _
                              & " FROM tblItems INNER Join" _
                              & " (" _
                              & " SELECT Item, AVG(UnitPrice) AS UnitPrice" _
                              & " FROM tblIn2" _
                              & " GROUP BY Item" _
                              & " ) QIn" _
                              & " ON tblItems.PrKey = QIn.Item" _
                              & " INNER Join" _
                              & " (" _
                              & " SELECT Item, UnitPrice AS UnitPrice" _
                              & " FROM tblIn2" _
                              & " WHERE PrKey IN (" _
                              & " SELECT MAX(PrKey)" _
                              & " FROM tblIn2" _
                              & " GROUP BY Item" _
                              & " )" _
                              & " ) LIn" _
                              & " ON tblItems.PrKey = LIn.Item" _
                              & " INNER JOIN tblOut2 ON tblItems.PrKey = tblOut2.Item" _
                              & " INNER JOIN tblOut1 ON tblOut2.Serial = tblOut1.Serial" _
                              & " WHERE tblOut1.[Date] BETWEEN '" & fDate & "' AND '" & SDate & "'" _
                              & exp & Cust & itmm _
                              & " GROUP BY tblItems.Serial, tblItems.Name, QIn.UnitPrice, LIn.UnitPrice" _
                              & " ORDER BY tblItems.Name"

        Dim ds As New ReportsDS
        Dim da As New SqlDataAdapter(Query, myConn)
        da.Fill(ds.Tables("tblOut"))

        Dim rep As New crOut
        rep.SetDataSource(ds.Tables("tblOut"))
        CrystalReportViewer1.ReportSource = rep


        If osCustomer.Text = Nothing Then
            rep.SetParameterValue("PartCustomer", " ")
            rep.SetParameterValue("ParCustomerName", " ")
        Else
            rep.SetParameterValue("PartCustomer", ":«·⁄„Ì·")
            rep.SetParameterValue("ParCustomerName", osCustomer.Text.ToUpper)
        End If

        If osDateFrom.Checked = True Then
            rep.SetParameterValue("ParDates", ":«·› —…")
            rep.SetParameterValue("ParoDateFrom", osDateFrom.Value.ToString("dd/MM/yyyy"))
        Else
            rep.SetParameterValue("ParDates", " ")
            rep.SetParameterValue("ParoDateFrom", " ")
        End If
        If osDateTill.Checked = True Then
            rep.SetParameterValue("ParDates", ":«·› —…")
            rep.SetParameterValue("ParoDateTill", osDateTill.Value.ToString("dd/MM/yyyy"))
        Else
            rep.SetParameterValue("ParoDateTill", " ")
        End If
        Cursor = Cursors.Default

        ' rep.SetParameterValue("ParTitle", "IN")
        CrystalReportViewer1.Refresh()
        CrystalReportViewer1.Zoom(1)

    End Sub

    Private Sub siSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles siSearch.Click
        Cursor = Cursors.WaitCursor
        Dim fDate, SDate As String
        If siDateFrom.Checked = True Then
            fDate = siDateFrom.Value.ToString("MM/dd/yyyy")
        Else
            fDate = "01/01/1999"
        End If
        If siDateTill.Checked = True Then
            SDate = siDateTill.Value.ToString("MM/dd/yyyy")
        Else
            SDate = "01/01/2500"
        End If

        Dim itm As String

        If siItem.Text = "" Then
            itm = ""
        Else
            itm = " AND (tblItems.Name LIKE N'%" & siItem.Text & "%')"
        End If

        Dim qnty1, qnty2 As Double
        qnty1 = Val(siQntyFrom.Text)
        qnty2 = Val(siQntyTill.Text)

        Dim quan As String = ""

        If siQntyFrom.Text <> "" And siQntyTill.Text <> "" Then
            quan = " AND TIn.QIn - COALESCE(TOut.QOut, 0) BETWEEN '" & qnty1 & "' AND '" & qnty2 & "'"
        ElseIf siQntyFrom.Text <> "" And siQntyTill.Text = "" Then
            quan = " AND TIn.QIn - COALESCE(TOut.QOut, 0) >= '" & qnty1 & "'"
        ElseIf siQntyFrom.Text = "" And siQntyTill.Text <> "" Then
            quan = " AND TIn.QIn - COALESCE(TOut.QOut, 0) <= '" & qnty2 & "'"
        Else
            quan = ""
        End If

        Dim needed As String
        If KryptonCheckBox1.Checked = False Then
            needed = ""
        Else
            needed = " AND (TIn.QIn - COALESCE(TOut.QOut, 0) < tblItems.[Minimum])"
        End If



        If quan <> "" And itm <> "" Then
            Cursor = Cursors.Default
            MessageBox.Show("≈–« √—œ  «·«” ⁄·«„ ⁄‰ ’‰› ÌÃ» „”Õ «·ﬂ„Ì«  «·„Ê÷Ê⁄…!", "Wrong Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
            siItem.Focus()
            Exit Sub
        End If

        Dim Query As String = "SELECT tblItems.Serial, tblItems.Name AS Item, tblItems.Price, tblItems.[Minimum], TIn.QIn AS T_In, COALESCE(TOut.QOut, 0) AS T_Out, TIn.QIn - COALESCE(TOut.QOut, 0) AS Net, (CASE WHEN ( TIn.QIn - COALESCE(TOut.QOut, 0)) < tblItems.[Minimum] THEN 1 ELSE 0 END) as [Needed]" _
                              & " FROM tblItems" _
                              & " INNER Join" _
                              & " (" _
                              & " SELECT tblIn2.Item, SUM(tblIn2.Qnty) AS QIn" _
                              & " FROM tblIn2 INNER JOIN tblIn1" _
                              & " ON tblIn2.Serial = tblIn1.PrKey" _
                              & " WHERE tblIn1.[Date] BETWEEN '" & fDate & "' AND '" & SDate & "'" _
                              & " GROUP BY tblIn2.Item" _
                              & " ) TIn" _
                              & " ON TIn.Item = tblItems.PrKey" _
                              & " LEFT OUTER JOIN" _
                              & " (" _
                              & " SELECT tblOut2.Item, SUM(tblOut2.Qnty) AS QOut" _
                              & " FROM tblOut2 INNER JOIN tblOut1" _
                              & " ON tblOut2.Serial = tblOut1.Serial" _
                              & " WHERE tblOut1.[Date] BETWEEN '" & fDate & "' AND '" & SDate & "'" _
                              & " GROUP BY tblOut2.Item" _
                              & " ) TOut" _
                              & " ON TOut.Item = tblItems.PrKey WHERE 1=1 " _
                              & quan & itm & needed _
                              & " ORDER BY tblItems.Name"


        Dim ds As New ReportsDS
        Dim da As New SqlDataAdapter(Query, myConn)
        da.Fill(ds.Tables("tblItems"))

        Dim rep As New crItems
        rep.SetDataSource(ds.Tables("tblItems"))
        CrystalReportViewer4.ReportSource = rep


        If siDateFrom.Checked = True Then
            rep.SetParameterValue("ParDF", ":«·› —…")
            rep.SetParameterValue("ParDateFrom", siDateFrom.Value.ToString("dd/MM/yyyy"))
        Else
            rep.SetParameterValue("ParDF", "")
            rep.SetParameterValue("ParDateFrom", "")
        End If
        If siDateTill.Checked = True Then
            rep.SetParameterValue("ParDF", ":«·› —…")
            rep.SetParameterValue("ParDateTill", siDateTill.Value.ToString("dd/MM/yyyy"))
        Else
            rep.SetParameterValue("ParDateTill", "")
        End If
        Cursor = Cursors.Default
        'rep.SetParameterValue("parTitle", "IN REPORT")
        ' rep.SetParameterValue("ParTitle", "IN")
        CrystalReportViewer4.Refresh()
        CrystalReportViewer4.Zoom(1)

    End Sub

    Private Sub monSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles monSearch.Click
        If monItem.Text = "" Then
            MessageBox.Show("ÌÃ» ≈Œ Ì«— ﬂÊœ «·’‰›!", "No Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
            monItem.Focus()
        Else
            Cursor = Cursors.WaitCursor
            Dim Que As String

            Que = "SELECT tblItems.Name AS ItemName, tblIn1.[Date], N'‘—«¡' AS [Type], CONVERT(NVARCHAR(5), tblIn1.[Time], 108) AS Time, tblIn2.Item, tblIn2.Qnty, tblIn2.UnitPrice, tblIn2.[Value]" _
                & " FROM tblIn2 INNER JOIN tblIn1 ON tblIn2.Serial = tblIn1.PrKey" _
                & " INNER JOIN tblItems ON tblItems.PrKey = tblIn2.Item" _
                & " WHERE tblItems.Serial = N'" & monItem.Text & "'" _
                & " AND  tblIn1.[Date] BETWEEN '" & monDateFrom.Value.ToString("MM/dd/yyyy") & "' AND '" & monDateTill.Value.ToString("MM/dd/yyyy") & "'" _
                & " UNION ALL" _
                & " SELECT tblItems.Name AS ItemName, tblOut1.[Date], N'»Ì⁄' AS [Type], CONVERT(NVARCHAR(5), tblOut1.[Time], 108) AS [Time], tblOut2.Item, tblOut2.Qnty, tblOut2.UnitPrice, tblOut2.Price AS [Value]" _
                & " FROM tblOut2 INNER JOIN tblOut1 ON tblOut2.Serial = tblOut1.Serial" _
                & " INNER JOIN tblItems ON tblItems.PrKey = tblOut2.Item" _
                & " WHERE tblItems.Serial = N'" & monItem.Text & "'" _
                & " AND  tblOut1.[Date] BETWEEN '" & monDateFrom.Value.ToString("MM/dd/yyyy") & "' AND '" & monDateTill.Value.ToString("MM/dd/yyyy") & "'" _
                & " ORDER BY [Date], [Time]"



            Dim ds As New ReportsDS
            Dim da As New SqlDataAdapter(Que, myConn)
            da.Fill(ds.Tables("tblItemMonitor"))


            Dim rep As New crItemMonitor
            rep.SetDataSource(ds.Tables("tblItemMonitor"))
            CrystalReportViewer3.ReportSource = rep
            rep.SetParameterValue("parTitle", monItem.Text.ToUpper)
            rep.SetParameterValue("ParoDateFrom", monDateFrom.Value.ToString("dd/MM/yyyy"))
            rep.SetParameterValue("ParoDateTill", monDateTill.Value.ToString("dd/MM/yyyy"))
            rep.SetParameterValue("ParDates", ":«·› —…")
            Cursor = Cursors.Default
            CrystalReportViewer3.Refresh()
            CrystalReportViewer3.Zoom(1)
            myConn.Close()
        End If
    End Sub

    Private Sub KryptonButton19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton19.Click
        Dim Que As String
        Cursor = Cursors.WaitCursor
        Que = "SELECT tblItems.Serial AS Code, tblItems.Name AS Item, COALESCE(PrevIn.T_In, 0) - COALESCE(PrevOut.T_Out, 0) AS Prev, COALESCE(CurrIn.T_In, 0) AS [In], COALESCE(CurrOut.T_Out, 0) AS Out, (COALESCE(CurrIn.T_In, 0) - COALESCE(CurrOut.T_Out, 0) + (COALESCE(PrevIn.T_In, 0) - COALESCE(PrevOut.T_Out, 0))) AS Net" _
            & " FROM tblItems LEFT OUTER JOIN" _
            & " (" _
            & " SELECT tblIn.Item, SUM(tblIn.Qnty) AS T_In" _
            & " FROM tblIn" _
            & " WHERE tblIn.[Date] > '" & MonFrom.Value.ToString("MM/dd/yyyy") & "'" _
            & " GROUP BY tblIn.Item" _
            & " ) PrevIn" _
            & " ON tblItems.PrKey = PrevIn.Item" _
            & " LEFT OUTER JOIN" _
            & " (" _
            & " SELECT tblOut2.Item, SUM(tblOut2.Qnty) AS T_Out" _
            & " FROM tblOut2 INNER JOIN tblOut1" _
            & " ON tblOut1.Serial = tblOut2.Serial" _
            & " WHERE tblOut1.[Date] > '" & MonFrom.Value.ToString("MM/dd/yyyy") & "'" _
            & " GROUP BY tblOut2.Item" _
            & " ) PrevOut" _
            & " ON tblItems.PrKey = PrevOut.Item" _
            & " LEFT OUTER JOIN" _
            & " (" _
            & " SELECT tblIn.Item, SUM(tblIn.Qnty) AS T_In" _
            & " FROM tblIn" _
            & " WHERE tblIn.[Date] BETWEEN '" & MonFrom.Value.ToString("MM/dd/yyyy") & "' AND '" & MonTill.Value.ToString("MM/dd/yyyy") & "'" _
            & " GROUP BY tblIn.Item" _
            & " ) CurrIn" _
            & " ON tblItems.PrKey = CurrIn.Item" _
            & " LEFT OUTER JOIN" _
            & " (" _
            & " SELECT tblOut2.Item, SUM(tblOut2.Qnty) AS T_Out" _
            & " FROM tblOut2 INNER JOIN tblOut1" _
            & " ON tblOut1.Serial = tblOut2.Serial" _
            & " WHERE tblOut1.[Date] BETWEEN '" & MonFrom.Value.ToString("MM/dd/yyyy") & "' AND '" & MonTill.Value.ToString("MM/dd/yyyy") & "'" _
            & " GROUP BY tblOut2.Item" _
            & " ) CurrOut" _
            & " ON tblItems.PrKey = CurrOut.Item" _
            & " ORDER BY tblItems.Name"



        Dim ds As New ReportsDS
        Dim da As New SqlDataAdapter(Que, myConn)
        da.Fill(ds.Tables("tblMonitor"))

        Dim rep As New crMonitor
        rep.SetDataSource(ds.Tables("tblMonitor"))
        rep.SetParameterValue("ParDF", ":«·› —…")
        rep.SetParameterValue("ParDateFrom", MonFrom.Value.ToString("dd/MM/yyyy"))
        rep.SetParameterValue("ParDateTill", MonTill.Value.ToString("dd/MM/yyyy"))
        CrystalReportViewer5.ReportSource = rep
        Cursor = Cursors.Default
        CrystalReportViewer5.Refresh()
        CrystalReportViewer5.Zoom(1)
        'frmStatement.ShowDialog()


    End Sub

    Private Sub dailySearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dailySearch.Click
        Cursor = Cursors.WaitCursor
        Dim Cashier As String = ""
        If cbCashiers.Text <> "" Then
            Cashier = " AND (tblLogin.UserName = N'" & cbCashiers.Text & "')"
        End If

        Dim timFrom, timTill As String
        timFrom = dailyDateFrom.Value.ToString("MM/dd/yyyy") & " " & tmFrom.Value.ToString("HH:mm") & ":00.000"
        timTill = dailyDateTill.Value.ToString("MM/dd/yyyy") & " " & tmTill.Value.ToString("HH:mm") & ":59.999"
        Dim Debit As Integer = 0
        Debit = rgPayment.SelectedIndex



        Dim Que As String = "(" _
                            & " SELECT tblIn1.[Date], CONVERT(NVARCHAR(5), tblIn1.[Time], 108) AS [Time], tblIn1.Serial AS Indication, N'‘—«¡' AS [Type], tblDebit.Amount, tblLogin.UserName AS [User]" _
                            & " FROM tblIn1 INNER JOIN tblDebit" _
                            & " ON tblDebit.Serial = tblIn1.PrKey AND tblDebit.[Date] = tblIn1.[Date] AND tblIn1.[Time] = tblDebit.[Time]" _
                            & " INNER JOIN tblLogin ON tblLogin.Sr = tblIn1.[User]" _
                            & " WHERE (tblIn1.[Date] + tblIn1.[Time] BETWEEN '" & timFrom & "' AND '" & timTill & "')" & Cashier _
                            & " UNION ALL" _
                            & " SELECT tblDebit.[Date], CONVERT(NVARCHAR(5), tblDebit.[Time], 108) AS [Time], tblIn1.Serial AS Indication, N'”œ«œ' AS [Type], tblDebit.Amount, tblLogin.UserName AS [User]" _
                            & " FROM tblDebit RIGHT OUTER JOIN tblIn1 ON tblDebit.Serial = tblIn1.PrKey" _
                            & " INNER JOIN tblLogin ON tblLogin.Sr = tblDebit.[User]" _
                            & " WHERE (tblDebit.[Date] != tblIn1.[Date] OR tblDebit.[Time] != tblIn1.[Time])" _
                            & " AND (tblDebit.[Date] + tblDebit.[Time] BETWEEN '" & timFrom & "' AND '" & timTill & "')" & Cashier _
                            & " UNION ALL" _
                            & " SELECT tblOut1.[Date], CONVERT(NVARCHAR(5), tblOut1.[Time], 108) AS [Time], CONVERT(NVARCHAR(20), tblOut1.Serial) AS Indication, N'»Ì⁄' AS [Type], tblOut1.RealValue AS Amount, tblLogin.UserName AS [User]" _
                            & " FROM tblOut1 INNER JOIN tblLogin ON tblLogin.Sr = tblOut1.[User]" _
                            & " WHERE (tblOut1.Debit = " & Debit & ") AND (tblOut1.[Date] + tblOut1.[Time] BETWEEN '" & timFrom & "' AND '" & timTill & "')" & Cashier _
                            & " UNION ALL" _
                            & " SELECT tblCash.[Date] AS [Date], CONVERT(NVARCHAR(5), tblCash.[Time], 108) AS [Time], tblCash.Indication, tblCash.[Type], tblCash.Qnty AS Amount, tblLogin.UserName AS [User]" _
                            & " FROM tblCash INNER JOIN tblLogin ON tblCash.[User] = tblLogin.Sr" _
                            & " WHERE (tblCash.[Date] + tblCash.[Time] BETWEEN '" & timFrom & "' AND '" & timTill & "')" & Cashier _
                            & " ) ORDER BY [Date], [Time]"



        Dim ds As New ReportsDS
        Dim da As New SqlDataAdapter(Que, myConn)
        da.Fill(ds.Tables("tblDaily"))

        Dim rep As New crDaily
        rep.SetDataSource(ds.Tables("tblDaily"))
        rep.SetParameterValue("ParDates", ":«·› —…")
        rep.SetParameterValue("ParoDateFrom", dailyDateFrom.Value.ToString("yyyy/MM/dd") & " " & tmFrom.Value.ToString("HH:mm"))
        rep.SetParameterValue("ParoDateTill", dailyDateTill.Value.ToString("yyyy/MM/dd") & " " & tmTill.Value.ToString("HH:mm"))
        CrystalReportViewer6.ReportSource = rep
        Cursor = Cursors.Default
        CrystalReportViewer6.Refresh()
        CrystalReportViewer6.Zoom(1)

    End Sub

    Private Sub inReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles inReport.Click
        Call RibbonClear()
        inReport.Checked = True
        KryptonDockableNavigator1.SelectedIndex = 5
    End Sub

    Private Sub outReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles outReport.Click
        Call RibbonClear()
        outReport.Checked = True
        KryptonDockableNavigator1.SelectedIndex = 6
    End Sub

    Private Sub itemReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles itemReport.Click
        Call RibbonClear()
        itemReport.Checked = True
        KryptonDockableNavigator1.SelectedIndex = 7
    End Sub

    Private Sub itemMonitor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles itemMonitor.Click
        Call RibbonClear()
        itemMonitor.Checked = True
        KryptonDockableNavigator1.SelectedIndex = 8
    End Sub

    Private Sub totalMonitor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles totalMonitor.Click
        Call RibbonClear()
        totalMonitor.Checked = True
        KryptonDockableNavigator1.SelectedIndex = 9
    End Sub

    Private Sub dailyMonitor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dailyMonitor.Click
        Call RibbonClear()
        dailyMonitor.Checked = True
        KryptonDockableNavigator1.SelectedIndex = 10
    End Sub

    Private Sub btnChangePassword_Click(sender As System.Object, e As System.EventArgs) Handles btnChangePassword.Click
        frmPassword.ShowDialog()
    End Sub

    Private Sub kryptonContextMenuItem1_Click(sender As Object, e As System.EventArgs) Handles kryptonContextMenuItem1.Click
        Application.Exit()
    End Sub

    Private Sub cSave_Click(sender As System.Object, e As System.EventArgs) Handles cSave.Click
        If Val(cSum.Text) = 0 Then
            MessageBox.Show("«·„»·€ «·–Ì  „ ≈œŒ«·Â €Ì— ’ÕÌÕ!", "Wrong Sum", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cSum.Focus()
            cSum.SelectAll()
        ElseIf cIndication.Text = "" Then
            MessageBox.Show("ÌÃ» ≈œŒ«· »Ì«‰!", "Wrong Indication", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cIndication.Focus()
            cIndication.SelectAll()
        Else
            Dim tp As String
            If RadioGroup1.SelectedIndex = 0 Then
                tp = "Ê«—œ"
            Else
                tp = "„‰’—›"
            End If

            Dim Query As String = "INSERT INTO tblCash ([Type], [Date], [Time], Qnty, Indication, [User])" _
                                  & " VALUES (N'" & tp & "', '" & DateTimePicker1.Value.ToString("MM/dd/yyyy") & "', '" & Now.ToString("HH:mm") & "', '" & Val(cSum.Text) & "', N'" & cIndication.Text & "','" & GlobalVariables.ID & "')"

            Using cmd = New SqlCommand(Query, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                cmd.ExecuteNonQuery()
                myConn.Close()

                cSum.Text = ""
                cIndication.Text = ""
                cSum.Focus()
                Call SearchCash()
            End Using
        End If
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles DateTimePicker1.ValueChanged
        Call SearchCash()
    End Sub

    Private Sub cEdit_Click(sender As System.Object, e As System.EventArgs) Handles cEdit.Click
        Dim dia As DialogResult = MessageBox.Show("Â·  —Ìœ »«· √ﬂÌœ  ⁄œÌ· Â–« «·„œŒ·ø", "Modify", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If dia = Windows.Forms.DialogResult.Yes Then

            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If

            If Val(cSum.Text) = 0 Then
                MessageBox.Show("«·„»·€ «·–Ì  „ ≈œŒ«·Â €Ì— ’ÕÌÕ!", "Wrong Sum", MessageBoxButtons.OK, MessageBoxIcon.Information)
                cSum.Focus()
                cSum.SelectAll()
            ElseIf cIndication.Text = "" Then
                MessageBox.Show("ÌÃ» ≈œŒ«· »Ì«‰!", "Wrong Indication", MessageBoxButtons.OK, MessageBoxIcon.Information)
                cIndication.Focus()
                cIndication.SelectAll()
            Else
                Dim tp As String
                If RadioGroup1.SelectedIndex = 0 Then
                    tp = "Ê«—œ"
                Else
                    tp = "„‰’—›"
                End If
                Dim PrKey As Integer = CashDGV.CurrentRow.Cells(0).Value
                Dim Query As String = "UPDATE tblCash SET [Type] = N'" & tp & "', Qnty = '" & Val(cSum.Text) & "', Indication = N'" & cIndication.Text & "', [User] = '" & GlobalVariables.ID & "' WHERE PrKey = '" & PrKey & "'"
                Using cmd = New SqlCommand(Query, myConn)
                    cmd.ExecuteNonQuery()
                End Using

            End If
            myConn.Close()
            SearchCash()
        End If
    End Sub

    Private Sub cDelete_Click(sender As System.Object, e As System.EventArgs) Handles cDelete.Click
        Dim dia As DialogResult = MessageBox.Show("Â·  —Ìœ »«· √ﬂÌœ Õ–› Â–« «·„œŒ·ø", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If dia = Windows.Forms.DialogResult.Yes Then

            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Dim PrKey As Integer = CashDGV.CurrentRow.Cells(0).Value
            Dim Query As String = "DELETE FROM tblCash WHERE PrKey = '" & PrKey & "'"
            Using cmd = New SqlCommand(Query, myConn)
                cmd.ExecuteNonQuery()
            End Using
            myConn.Close()
            SearchCash()
        End If
    End Sub

    Private Sub CashDGV_CellClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles CashDGV.CellClick
        CashDGV.ClearSelection()

        Try
            CashDGV.CurrentRow.Selected = True
            cSum.Text = CashDGV.CurrentRow.Cells(4).Value
            If CashDGV.CurrentRow.Cells(1).Value = "Ê«—œ" Then
                RadioGroup1.SelectedIndex = 0
            Else
                RadioGroup1.SelectedIndex = 1
            End If
            cIndication.Text = CashDGV.CurrentRow.Cells(5).Value

        Catch ex As Exception

        End Try
    End Sub

    Private Sub KryptonRibbonGroupButton5_Click(sender As System.Object, e As System.EventArgs) Handles KryptonRibbonGroupButton5.Click
        Call RibbonClear()
        inReport.Checked = True
        KryptonDockableNavigator1.SelectedIndex = 4
    End Sub

    Private Sub itPaid_KeyDown(sender As Object, e As KeyEventArgs) Handles itPaid.KeyDown
        If e.KeyCode = Keys.Enter Then
            iAdd.PerformClick()
        End If
    End Sub

    Private Sub itPaid_TextChanged(sender As System.Object, e As System.EventArgs) Handles itPaid.TextChanged
        Call inTotalize()
    End Sub

    Private Sub tbPaid_GotFocus(sender As Object, e As System.EventArgs) Handles tbPaid.GotFocus
        tbPaid.SelectAll()
    End Sub

    Private Sub tbPaid_TextChanged(sender As System.Object, e As System.EventArgs) Handles tbPaid.TextChanged
        Try
            tbRest.Text = Val(tbPaid.Text) - Val(tbTotal.Text)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub krpBarcode_Click(sender As System.Object, e As System.EventArgs) Handles krpBarcode.Click
        frmBarcode.Show()
    End Sub

    Private Sub oCustomer_TextChanged(sender As Object, e As System.EventArgs) Handles oCustomer.TextChanged
        If oCustomer.Text = "" Then
            KryptonPanel21.Visible = False
        Else
            KryptonPanel21.Visible = True
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Try
                Using cmd = New SqlCommand("SELECT TOP(1) [Date] FROM tblOut1 WHERE Customer = N'" & oCustomer.Text & "' ORDER BY [Date] DESC", myConn)
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            KryptonLabel52.Text = dr(0)
                        Else
                            KryptonLabel52.Text = ""
                        End If
                    End Using
                End Using

                Using cmd = New SqlCommand("SELECT SUM(SubTotal) AS Amount FROM tblOut1 WHERE Customer = N'" & oCustomer.Text & "'", myConn)
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            KryptonLabel44.Text = dr(0)
                        Else
                            KryptonLabel44.Text = "00"
                        End If
                    End Using
                End Using
            Catch ex As Exception

            End Try

            myConn.Close()
        End If
    End Sub
    'to get a random serial
    Function RandomString(minCharacters As Integer, maxCharacters As Integer)
        Dim s As String = "0123456789"
        Static r As New Random
        Dim chactersInString As Integer = r.Next(minCharacters, maxCharacters)
        Dim sb As New StringBuilder
        For i As Integer = 1 To chactersInString
            Dim idx As Integer = r.Next(0, s.Length)
            sb.Append(s.Substring(idx, 1))
        Next
        Return sb.ToString()
    End Function


    Private Sub SimpleButton2_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton2.Click
retry:
        'Dim Sr As String = RandomString(6, 6)
        'Sr = "3000000" & Sr
        ''Me.Text = Sr
        Dim Sr As String = CreateEAN()

        Dim NQuery As String = "SELECT tblItems.* FROM tblItems LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                      & " WHERE tblItems.Serial = '" & Sr & "' OR tblMultiCodes.Code = '" & Sr & "';"

        Using cmd = New SqlCommand(NQuery, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Using dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read() Then
                    GoTo retry
                Else
                    iItem.Text = Sr
                End If
            End Using
            myConn.Close()
        End Using
        iItemName.Focus()
        iItemName.SelectAll()
    End Sub

    Private Sub SimpleButton3_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton3.Click
retry:
        'Dim Sr As String = RandomString(6, 6)
        'Sr = "3000000" & Sr
        Dim Sr As String = CreateEAN()

        Dim NQuery As String = "SELECT tblItems.* FROM tblItems LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                             & " WHERE tblItems.Serial = '" & Sr & "' OR tblMultiCodes.Code = '" & Sr & "';"

        Using cmd = New SqlCommand(NQuery, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Using dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read() Then
                    GoTo retry
                Else
                    iiSerial.Text = Sr
                End If
            End Using
            myConn.Close()
        End Using

    End Sub

    Private Sub KryptonRibbonGroupButton6_Click(sender As System.Object, e As System.EventArgs) Handles KryptonRibbonGroupButton6.Click
        frmCashier.Show()
        Me.Close()
    End Sub

    Private Sub KryptonTextBox1_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles KryptonTextBox1.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And KryptonTextBox1.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub KryptonTextBox1_PreviewKeyDown(sender As Object, e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles KryptonTextBox1.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Or e.KeyCode = Keys.Down Then
            iQnty.Focus()
        ElseIf e.KeyCode = Keys.Up Then
            nItemPrice.Focus()
        End If
    End Sub

    Private Sub KryptonTextBox2_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles iiMinimumQnty.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And iiMinimumQnty.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub KryptonTextBox2_PreviewKeyDown(sender As Object, e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iiMinimumQnty.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            If e.KeyCode = Keys.Shift Then
                iiPrice.Focus()
            Else
                iiSerial2.Focus()
            End If
        ElseIf e.KeyCode = Keys.Up Then
            iiPrice.Focus()
        End If
    End Sub

    Private Sub KryptonPanel22_VisibleChanged(sender As Object, e As System.EventArgs) Handles KryptonPanel22.VisibleChanged
        If KryptonPanel22.Visible = False Then
            nItemPrice.Text = ""
            KryptonTextBox1.Text = ""
            nEnglishName.Text = ""
            lblSellingPrice.Visible = False
        Else
            lblSellingPrice.Visible = True
        End If
    End Sub

    Private Sub iItem_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles iItem.SelectedIndexChanged
        'iItem.Text = iItem.Text.ToUpper
        If Not iItem.Text = "" Then
            Dim ne As Boolean
            Dim NQuery As String = "SELECT tblItems.* FROM tblItems LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                      & " WHERE tblItems.Serial = '" & iItem.Text & "' OR tblMultiCodes.Code = '" & iItem.Text & "';"
            Using cmd = New SqlCommand(NQuery, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Try
                    Using dt As SqlDataReader = cmd.ExecuteReader
                        If dt.Read() Then
                            ne = False
                            iItemName.Text = dt(2).ToString
                            KryptonPanel22.Visible = False
                            lblSellingPrice.Text = "”⁄— «·»Ì⁄: " & dt(3)
                            lblSellingPrice.Visible = True
                        Else
                            ne = True
                            KryptonPanel22.Visible = True
                            iItemName.Text = ""
                            lblSellingPrice.Visible = False
                        End If
                    End Using
                Catch ex As Exception
                    ' lblSellingPrice.Visible = False
                End Try
                myConn.Close()
            End Using
        Else
            lblSellingPrice.Visible = False
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton1.Click
        frmDebit.ShowDialog()
    End Sub

    Private Sub cSum_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cSum.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And cSum.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub iiSerial_PreviewKeyDown(sender As Object, e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iiSerial.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            If Not e.KeyCode = Keys.Shift Then
                iiItem.Focus()
            End If
        End If
    End Sub

    Private Sub iiItem_PreviewKeyDown(sender As Object, e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles iiItem.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            If e.KeyCode = Keys.Shift Then
                iiSerial.Focus()
            Else
                iiEnglishName.Focus()
            End If
        ElseIf e.KeyCode = Keys.Up Then
            iiSerial.Focus()
        End If
    End Sub

    Private Sub lblSellingPrice_Click(sender As System.Object, e As System.EventArgs) Handles lblSellingPrice.Click
        If iItem.Text <> "" And iItemName.Text <> "" Then
            If GlobalVariables.authority = "Admin" Or GlobalVariables.authority = "Developer" Then
                frmPriceChange.ShowDialog()
                iQnty.Focus()
            Else
                MsgBox("!√‰  ·«  „·ﬂ «·’·«ÕÌ… · €ÌÌ— ”⁄— «·’‰›")
            End If
        End If
    End Sub

    Private Sub KryptonRibbonGroupButton7_Click(sender As System.Object, e As System.EventArgs) Handles KryptonRibbonGroupButton7.Click
        Shell("Calc", AppWinStyle.NormalFocus, True)

    End Sub

    Private Sub KryptonRibbonGroupButton9_Click(sender As System.Object, e As System.EventArgs) Handles KryptonRibbonGroupButton9.Click
        frmSync.ShowDialog()
    End Sub


    Private Sub iUnitPrice_EditValueChanged(sender As Object, e As EventArgs) Handles iUnitPrice.EditValueChanged

        iValue.Text = Val(iQnty.Text) * Val(iUnitPrice.Text)

    End Sub

    Private Sub iValue_EditValueChanged(sender As Object, e As EventArgs) Handles iValue.EditValueChanged
        iUnitPrice.Text = Val(iValue.Text) / Val(iQnty.Text)
    End Sub


    Private Sub iiSerial2_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles iiSerial2.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            If e.KeyCode = Keys.Shift Then
                iiMinimumQnty.Focus()
            Else
                iiGroupPrice.Focus()
            End If
        ElseIf e.KeyCode = Keys.Up Then
            iiMinimumQnty.Focus()
        End If
    End Sub

    Private Sub iiGroupPrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles iiGroupPrice.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And iiGroupPrice.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub iiGroupPrice_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles iiGroupPrice.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            If e.KeyCode = Keys.Shift Then
                iiSerial2.Focus()
            Else
                iiUnitNumber.Focus()
            End If
        ElseIf e.KeyCode = Keys.Up Then
            iiSerial2.Focus()
        End If
    End Sub

    Private Sub iiUnitNumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles iiUnitNumber.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And iiUnitNumber.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub iiUnitNumber_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles iiUnitNumber.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            If e.KeyCode = Keys.Shift Then
                iiGroupPrice.Focus()
            Else
                iiAlterCodes.Focus()
            End If
        ElseIf e.KeyCode = Keys.Up Then
            iiGroupPrice.Focus()
        End If
    End Sub

    Private Sub LabelControl1_Click(sender As Object, e As EventArgs) Handles LabelControl1.Click
        itPaid.Text = iTotalSales.Text
        itPaid.Focus()
        itPaid.SelectAll()
    End Sub

    Private Sub iSearch_VisibleChanged(sender As Object, e As EventArgs) Handles iSearch.VisibleChanged
        If iSearch.Visible = True Then
            iSerial.Visible = False
        Else
            iSerial.Visible = True
        End If
    End Sub

    Private Sub iiAlterCodes_KeyDown(sender As Object, e As KeyEventArgs) Handles iiAlterCodes.KeyDown
        If e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            If e.KeyCode = Keys.Shift Then
                iiUnitNumber.Focus()
            Else
                iiAdd.Focus()
            End If
        ElseIf e.KeyCode = Keys.Up Then
            iiUnitNumber.Focus()
        End If

        If e.KeyCode = Keys.Enter And Not iiAlterCodes.Text = "" Then
            e.Handled = True
            iiAlterCodes.Text += ";"
            iiAlterCodes.Text = iiAlterCodes.Text.Replace(vbNewLine, "")
            iiAlterCodes.Select(iiAlterCodes.Text.Length, 0)

        End If
    End Sub

    Private Sub iiEnglishName_GotFocus(sender As Object, e As EventArgs) Handles iiEnglishName.GotFocus
        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("EN-US"))
    End Sub

    Private Sub iiEnglishName_LostFocus(sender As Object, e As EventArgs) Handles iiEnglishName.LostFocus
        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("AR-EG"))
    End Sub

    Private Sub iiEnglishName_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles iiEnglishName.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Tab Then
            If e.KeyCode = Keys.Shift Then
                iiItem.Focus()
            Else
                iiPrice.Focus()
            End If
        ElseIf e.KeyCode = Keys.Up Then
            iiItem.Focus()
        End If
    End Sub

    Private Sub nEnglishName_GotFocus(sender As Object, e As EventArgs) Handles nEnglishName.GotFocus
        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("EN-US"))
    End Sub

    Private Sub nEnglishName_LostFocus(sender As Object, e As EventArgs) Handles nEnglishName.LostFocus
        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("AR-EG"))
    End Sub

    Private Sub nEnglishName_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles nEnglishName.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Or e.KeyCode = Keys.Down Then
            nItemPrice.Focus()
        ElseIf e.KeyCode = Keys.Up Then
            iItemName.Focus()
        End If
    End Sub

    Private Sub nEnglishName_TextChanged(sender As Object, e As EventArgs) Handles nEnglishName.TextChanged

    End Sub

    Private Sub KryptonRibbonGroupButton10_Click(sender As Object, e As EventArgs) Handles KryptonRibbonGroupButton10.Click
        frmMerge.ShowDialog()
    End Sub

    Private Sub KryptonLabel33_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles KryptonLabel33.MouseDoubleClick
        Dim diar As DialogResult = MessageBox.Show("Â·  —Ìœ »«· √ﬂÌœ ⁄„· Õ–› ‰Â«∆Ì ·Â–« «·’‰› ”Ê› ÌƒÀ— Â–« ⁄·Ï ⁄„·Ì«  «·»Ì⁄ Ê«·‘—«¡ «· Ì œÂ· ›ÌÂ« Â–« «·„‰ Ãø", "Hard Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If diar = Windows.Forms.DialogResult.Yes AndAlso GlobalVariables.authority = "Admin" Then
            Dim diar2 As DialogResult = MessageBox.Show("«÷€ÿ „Ê«›ﬁ ··Õ–›. ÌÊ’Ï »√Œ– ‰”Œ… «Õ Ì«ÿÌ… „‰ ﬁ«⁄œ… «·»Ì«‰«  ﬁ»· «·Õ–›", "Confirm", MessageBoxButtons.OKCancel)
            If diar2 = Windows.Forms.DialogResult.OK Then
                Dim Query As String = "DECLARE @Serial BIGINT = (SELECT PrKey FROM tblItems WHERE Serial = '" & iiSearch.Text & "');" _
                                      & " DELETE FROM tblOut2 WHERE Item = @Serial;" _
                                      & " DELETE FROM tblIn2 WHERE Item = @Serial;" _
                                      & " DELETE FROM tblMultiCodes WHERE Item = @Serial;" _
                                      & " DELETE FROM tblItems WHERE PrKey = @Serial;"
                Using cmd = New SqlCommand(Query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Try
                        cmd.ExecuteNonQuery()

                        iiSerial.Text = Nothing
                        iiItem.Text = Nothing
                        iiPrice.Text = Nothing
                        iiMinimumQnty.Text = Nothing
                        iiSerial2.Text = Nothing
                        iiGroupPrice.Text = Nothing
                        iiUnitNumber.Text = Nothing
                        iiAlterCodes.Text = Nothing
                        iiEnglishName.Text = Nothing
                        Call Notification("Record Deleted!")

                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try
                    myConn.Close()
                End Using
            End If
        End If
    End Sub

    Private Sub KryptonLabel33_Paint(sender As Object, e As PaintEventArgs) Handles KryptonLabel33.Paint

    End Sub

    Private Sub btnCurrency_Click(sender As Object, e As EventArgs) Handles btnCurrency.Click
        Cursor = Cursors.WaitCursor
        Dim Cashier As String = ""
        If cbCashiers.Text <> "" Then
            Cashier = " AND (tblLogin.UserName = N'" & cbCashiers.Text & "')"
        End If

        Dim timFrom, timTill As String
        timFrom = dailyDateFrom.Value.ToString("MM/dd/yyyy") & " " & tmFrom.Value.ToString("HH:mm") & ":00.000"
        timTill = dailyDateTill.Value.ToString("MM/dd/yyyy") & " " & tmTill.Value.ToString("HH:mm") & ":59.999"

        Dim Debit As Integer = 0
        Debit = rgPayment.SelectedIndex


        Dim Que As String = "(" _
                            & " SELECT tblIn1.[Date], tblIn1.[Time], tblIn1.Serial AS Indication, N'‘—«¡' AS [Type], 0 AS EGP, tblDebit.Amount AS USD, 0 AS EUR, 0 AS GBP, 0 AS RUB, 0 AS CHF, 0 AS CNY, 0 AS VisaUSD, 0 AS VisaEGP, tblLogin.UserName AS [User]" _
                            & " FROM tblIn1 INNER JOIN tblDebit" _
                            & " ON tblDebit.Serial = tblIn1.PrKey AND tblDebit.[Date] = tblIn1.[Date] AND tblIn1.[Time] = tblDebit.[Time]" _
                            & " INNER JOIN tblLogin ON tblLogin.Sr = tblIn1.[User]" _
                            & " WHERE (tblIn1.[Date] + tblIn1.[Time] BETWEEN '" & timFrom & "' AND '" & timTill & "')" & Cashier _
                            & " UNION ALL" _
                            & " SELECT tblDebit.[Date], tblDebit.[Time], tblIn1.Serial AS Indication, N'”œ«œ' AS [Type], 0 AS EGP , tblDebit.Amount AS USD, 0 AS EUR, 0 AS GBP, 0 AS RUB, 0 AS CHF, 0 AS CNY, 0 AS VisaUSD, 0 AS VisaEGP, tblLogin.UserName AS [User]" _
                            & " FROM tblDebit RIGHT OUTER JOIN tblIn1 ON tblDebit.Serial = tblIn1.PrKey" _
                            & " INNER JOIN tblLogin ON tblLogin.Sr = tblDebit.[User]" _
                            & " WHERE (tblDebit.[Date] != tblIn1.[Date] OR tblDebit.[Time] != tblIn1.[Time])" _
                            & " AND (tblDebit.[Date] + tblDebit.[Time] BETWEEN '" & timFrom & "' AND '" & timTill & "')" & Cashier _
                            & " UNION ALL" _
                            & " SELECT tblOut1.[Date], tblOut1.[Time], CONVERT(NVARCHAR(20), tblOut1.Serial) AS Indication, N'»Ì⁄' AS [Type]," _
                            & " (CASE WHEN tblOut1.Visa1 = 0 THEN (CASE WHEN tblOut1.Currency = 'EGP' THEN tblOut1.pEGP - tblOut1.NetTotal ELSE tblOut1.pEGP END) ELSE 0 END) AS EGP," _
                            & " (CASE WHEN tblOut1.Currency = 'USD' AND tblOut1.Visa2 = 0 THEN tblOut1.pUSD - tblOut1.NetTotal ELSE 0 END) AS USD," _
                            & " (CASE WHEN tblOut1.Currency = 'EUR' THEN tblOut1.pEUR - tblOut1.NetTotal ELSE 0 END) AS EUR," _
                            & " (CASE WHEN tblOut1.Currency = 'GBP' THEN tblOut1.pGBP - tblOut1.NetTotal ELSE 0 END) AS GBP," _
                            & " (CASE WHEN tblOut1.Currency = 'RUB' THEN tblOut1.pRUB - tblOut1.NetTotal ELSE 0 END) AS RUB," _
                            & " (CASE WHEN tblOut1.Currency = 'CHF' THEN tblOut1.pCHF - tblOut1.NetTotal ELSE 0 END) AS CHF," _
                            & " (CASE WHEN tblOut1.Currency = 'CNY' THEN tblOut1.pCNY - tblOut1.NetTotal ELSE 0 END) AS CNY," _
                            & " (CASE WHEN tblOut1.Currency = 'USD' AND tblOut1.Visa2 = 1 THEN tblOut1.pUSD - tblOut1.NetTotal ELSE 0 END) AS VisaUSD," _
                            & " (CASE WHEN tblOut1.Visa1 = 1 THEN (CASE WHEN tblOut1.Currency = 'EGP' THEN tblOut1.pEGP - tblOut1.NetTotal ELSE tblOut1.pEGP END) ELSE 0 END) AS VisaEGP," _
                            & " tblLogin.UserName AS [User]" _
                            & " FROM tblOut1 INNER JOIN tblLogin ON tblOut1.[User] = tblLogin.Sr" _
                            & " WHERE (tblOut1.Debit = " & Debit & ") AND (tblOut1.[Date] + tblOut1.[Time] BETWEEN '" & timFrom & "' AND '" & timTill & "')" & Cashier _
                            & " UNION ALL" _
                            & " SELECT tblCash.[Date] AS [Date], tblCash.[Time], tblCash.Indication, tblCash.[Type], tblCash.Qnty AS EGP, 0 AS USD, 0 AS EUR, 0 AS GBP, 0 AS RUB, 0 AS CHF, 0 AS CNY, 0 AS VisaUSD, 0 AS VisaEGP, tblLogin.UserName AS [User]" _
                            & " FROM tblCash INNER JOIN tblLogin ON tblCash.[User] = tblLogin.Sr" _
                            & " WHERE (tblCash.[Date] + tblCash.[Time] BETWEEN '" & timFrom & "' AND '" & timTill & "')" & Cashier _
                            & " ) ORDER BY [Date], [Time]"



        If myConn.State = ConnectionState.Closed Then
            myConn.Open()
        End If

        Dim ds As New ReportsDS
        Dim da As New SqlDataAdapter(Que, frmMain.myConn)
        da.Fill(ds.Tables("XtraNewDaily"))

        Dim Report As New XtraNewDaily


        Report.DataSource = ds
        Report.DataAdapter = da
        Report.DataMember = "XtraNewDaily"
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
        myConn.Close()


    End Sub

    Private Sub btnChecks_Click(sender As Object, e As EventArgs) Handles btnChecks.Click
        frmChecks.ShowDialog()
    End Sub

    Private Sub ToolStripStatusLabel3_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel3.Click
        If Not GlobalVariables.authority = "Cashier" Then
            frmSQL.ShowDialog()
        End If
    End Sub

    Private Sub btnPassKey_Click(sender As Object, e As EventArgs) Handles btnPassKey.Click
        frmPassKey.ShowDialog()
    End Sub

    Private Sub btnCustomers_Click(sender As Object, e As EventArgs) Handles btnCustomers.Click
        frmCustomers.ShowDialog()
    End Sub


    Private Sub iDgv_SelectionChanged(sender As Object, e As EventArgs) Handles iDgv.SelectionChanged

        inTotalize()
    End Sub
End Class
