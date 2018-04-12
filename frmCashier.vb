Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrinting.Native
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Text


Public Class frmCashier
    Public Shared myConn As New SqlConnection(GV.myConn)
    Dim counter As Integer = 0
    ' Dim ValidQnty As Boolean = False
    Dim itemPackage As Boolean = False
    Dim tValueAmount As Double
    Public Shared cEUR, cEGP, cGBP, cRUB, cCHF, cCNY As Double


    Private Sub fillCustomers()
        Using cmd = New SqlCommand("SELECT * FROM tblCustomers ORDER BY Customer;", myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Dim adt As New SqlDataAdapter
            Dim ds As New DataSet()
            adt.SelectCommand = cmd
            adt.Fill(ds)
            adt.Dispose()


            oCustomer.Properties.DataSource = ds.Tables(0)
            oCustomer.Properties.DisplayMember = "Customer"
            oCustomer.Properties.ValueMember = "ID"
            oCustomer.EditValue = Nothing
            oItemSerial.Focus()
            oItemSerial.SelectAll()


            myConn.Close()

        End Using
    End Sub
    Private Sub Authorize()
        If GlobalVariables.authority = "Admin" Or GlobalVariables.authority = "Developer" Then
            'btnSettings.Visible = True
            SimpleButton8.Visible = True
            'btnReturn.Visible = True
            'btnCost.Visible = True
            LabelControl1.Visible = True
            LabelControl2.Visible = True
            LabelControl3.Visible = True
            LabelControl14.Visible = True
            LabelControl17.Visible = True
            LabelControl18.Visible = True

            oCustomer.Visible = True
            txtVAT.Visible = True
            txtTax1.Visible = True
            txtTax2.Visible = True
            ceDebit.Visible = True
            Try
                oDgv.Columns(5).ReadOnly = False
            Catch ex As Exception

            End Try
        Else
            'btnSettings.Visible = False
            SimpleButton8.Visible = False
            'btnReturn.Visible = False
            'btnCost.Visible = False
            'Try
            '    oDgv.Columns(5).ReadOnly = True
            'Catch ex As Exception

            'End Try
            LabelControl1.Visible = False
            LabelControl2.Visible = False
            LabelControl3.Visible = False
            oCustomer.Visible = False
            LabelControl14.Visible = False
            LabelControl17.Visible = False
            LabelControl18.Visible = False
            txtVAT.Visible = False
            txtTax1.Visible = False
            txtTax2.Visible = False
            ceDebit.Checked = False
            ceDebit.Visible = False
        End If
    End Sub
    Private Function QntyGet() As Single

        Dim Result() As String = GetItem(oItemSerial.Text).Split(";")


        'Show the Net Qnty
        Dim NetQuery As String
        NetQuery = "SELECT CONVERT(FLOAT, (ttlIn.Total_In - COALESCE(ttlOut.Total_Out , 0))) AS Net_Amount" _
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
            & " WHERE tblItems.PrKey = " & Result(0)

        Dim theQuant As Single
        Using cmd = New SqlCommand(NetQuery, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Try
                theQuant = cmd.ExecuteScalar
            Catch ex As Exception
                theQuant = 0
            End Try
            myConn.Close()
        End Using
        'check the item if found in the grid
        For x As Integer = 0 To oDgv.Rows.Count - 1
            If (oDgv.Rows(x).Cells(7).Value.ToString = Result(0)) Then
                theQuant -= oDgv.Rows(x).Cells(0).Value
            End If
        Next
        Return theQuant

    End Function
    Private Sub saveMemory(ByVal number As Integer)
        Dim str As String = ""
        str = oCustomer.EditValue & ";" & tbCurrency.Text & ";" & lblCurrency.Text & ";" & ceVisaCurrency.Checked & ";" & tValueAmount & ";" & txtVAT.Text & ";" & txtTax1.Text & ";" & txtTax2.Text & ";" & ceDebit.Checked.ToString & "$"

        For x As Integer = 0 To oDgv.RowCount - 1
            str += oDgv.Rows(x).Cells(0).Value & ";" & oDgv.Rows(x).Cells(1).Value & ";" & oDgv.Rows(x).Cells(2).Value & ";" & _
                oDgv.Rows(x).Cells(3).Value & ";" & oDgv.Rows(x).Cells(4).Value & ";" & oDgv.Rows(x).Cells(5).Value & ";" & oDgv.Rows(x).Cells(6).Value & ";" _
                & oDgv.Rows(x).Cells(7).Value & ";0;" & oDgv.Rows(x).Cells(9).Value & ";" & oDgv.Rows(x).Cells(10).Value & ";" & oDgv.Rows(x).Cells(11).Value & "$"
        Next

        Select Case number
            Case 1
                My.Settings.M1 = str
            Case 2
                My.Settings.M2 = str
            Case 3
                My.Settings.M3 = str
            Case 4
                My.Settings.M4 = str
            Case 5
                My.Settings.M5 = str
            Case 6
                My.Settings.M6 = str
            Case 7
                My.Settings.M7 = str
            Case 8
                My.Settings.M8 = str
            Case 9
                My.Settings.M9 = str
        End Select
        My.Settings.Save()
        KryptonButton4.PerformClick()
    End Sub

    Private Sub recallMemory(ByVal number As Integer)
        Dim str As String = ""
        Select Case number
            Case 1
                str = My.Settings.M1
            Case 2
                str = My.Settings.M2
            Case 3
                str = My.Settings.M3
            Case 4
                str = My.Settings.M4
            Case 5
                str = My.Settings.M5
            Case 6
                str = My.Settings.M6
            Case 7
                str = My.Settings.M7
            Case 8
                str = My.Settings.M8
            Case 9
                str = My.Settings.M9
        End Select

        If str = "" Or str = ";$" Then
            Return
        End If

        Dim memory() As String = str.Split("$")
        Dim record() As String
        oDgv.Rows.Clear()
        
        For x As Integer = 0 To memory.Count - 1
            record = memory(x).Split(";")
            If x = 0 Then
                oCustomer.EditValue = record(0)
                tbCurrency.Text = record(1)
                lblCurrency.Text = record(2)
                ceVisaCurrency.Checked = CBool(record(3))
                tValueAmount = Val(record(4))
                txtVAT.Text = record(5)
                txtTax1.Text = record(6)
                txtTax2.Text = record(7)
                ceDebit.Checked = CBool(record(8))
            ElseIf memory(x) <> "" Then
                oDgv.Rows.Add(record(0), record(1), record(2), record(3), record(4), record(5), record(6), record(7), record(8), record(9), record(10), record(11))
            End If
        Next
        Call OutTotalize()

    End Sub
    Private Function GetItem(ByVal Serial As String) As String
        Dim Query As String = "DECLARE @Code NVARCHAR(MAX) = '" & Serial & "';" _
                              & " SELECT tblItems.PrKey, tblItems.Serial, tblItems.Name, tblItems.Price, tblItems.PackageUnits, tblItems.PackagePrice, tblItems.PackageSerial" _
                              & " FROM tblItems" _
                              & " LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                              & " WHERE tblItems.Serial = @Code OR tblItems.PackageSerial = @Code OR tblMultiCodes.Code = @Code"
        Dim result As String
        Using cmd = New SqlCommand(Query, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If

            Using dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read() Then
                    result = dr(0).ToString & ";" & dr(1) & ";" & dr(2) & ";" & dr(3) & ";" & dr(4) & ";" & dr(5) & ";" & dr(6)
                Else
                    result = "0;0;0;0;0;0;0"
                End If
            End Using
            myConn.Close()
        End Using
        Return result
    End Function
    Private Sub Notification(ByVal Notify As String)
        Timer1.Enabled = False
        counter = 0
        lbMonitor.Text = Notify

        lbMonitor.Visible = True
        Timer1.Enabled = True

    End Sub

    Private Sub SaveOrder(ByVal SaveAndPrint As Boolean)
        ''''''''''''''''
        'for demo
        Cursor = Cursors.WaitCursor
        If GV.appDemo = True Then
            Using cmd = New SqlCommand("SELECT COUNT(Serial) FROM tblOut1", myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read() Then
                        If dr(0) > GV.Invoices Then
                            Exit Sub
                        End If
                    End If
                End Using

                myConn.Close()
            End Using
        End If
        '''''''''''''''''''
Restart:

        If ceDebit.Checked = True Then
            tbEgyptian.Text = "0"
            ceVisaEGP.Checked = False
            tbCurrency.Text = "0"
            ceVisaCurrency.Checked = False
        End If

        If oDgv.RowCount = 0 Then
            MessageBox.Show("·«  ÊÃœ √Ì »Ì«‰«  ·Ì „ Õ›ŸÂ«!", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            oItemSerial.Focus()
        ElseIf Val(tbRest.Text) < 0 AndAlso ceDebit.Checked = False Then
            MessageBox.Show("!«·„»·€ «·„œ›Ê⁄ €Ì— ’ÕÌÕ", "Wrong Payment", MessageBoxButtons.OK, MessageBoxIcon.Information)
            tbEgyptian.Focus()
            tbEgyptian.SelectAll()
        ElseIf ceDebit.Checked = True AndAlso oCustomer.EditValue = Nothing Then
            MessageBox.Show("!ÌÃ» ≈œŒ«· «”„ «·⁄„Ì· ··œ›⁄ «·¬Ã·", "No Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
            oCustomer.Focus()
            oCustomer.SelectAll()
        Else

            Call frmMain.AutoRate(Today)
            Dim oDate As String = Today.ToString("MM/dd/yyyy")
            Dim oTime As String = Now.ToString("HH:mm")


            'check if the valid qnty
            If CheckQnty() = False Then
                Exit Sub
            End If


            'add to out1
            Dim pEGP, pEUR, pUSD, pGBP, pRUB, pCHF, pCNY As Double
            pEGP = 0
            pEUR = 0
            pUSD = 0
            pGBP = 0
            pRUB = 0
            pCHF = 0
            pCNY = 0
            Select Case lblCurrency.Text
                Case "EUR"
                    pEUR = Val(tbCurrency.Text)
                Case "USD"
                    pUSD = Val(tbCurrency.Text)
                Case "GBP"
                    pGBP = Val(tbCurrency.Text)
                Case "RUB"
                    pRUB = Val(tbCurrency.Text)
                Case "CHF"
                    pCHF = Val(tbCurrency.Text)
                Case "CNY"
                    pCNY = Val(tbCurrency.Text)
            End Select

            pEGP = Val(tbEgyptian.Text)
            Dim Cust As String = "NULL"
            If Not oCustomer.EditValue = Nothing Then
                Cust = oCustomer.EditValue
            End If

            Dim Query As String = "INSERT INTO tblOut1 ([Date], [Time], Customer, LaborOverhaul, Transfers, SubTotal, Discount, SaleTax, NetTotal, [User], pEGP, pEUR, pUSD, pGBP, pRUB, pCHF, pCNY, RealValue, Currency, Visa1, Visa2, VAT, Tax1, Tax2, Debit) " _
                                  & "VALUES ('" & oDate & "', '" & oTime & "', " & Cust & ", '" & "0" & "', '" & "0" & "', '" & tbTotal.Text & "', '" & Val(tbCurrency.Text) & "', '" & "0" & "', '" & tbRest.Text & "', '" & GlobalVariables.ID & "', " _
                                  & pEGP & " , " & pEUR & ", " & pUSD & ", " & pGBP & ", " & pRUB & ", " & pCHF & ", " & pCNY & ", " & tValueAmount & ", '" & lblCurrency.Text & "', " & CInt(ceVisaEGP.Checked) & ", " & CInt(ceVisaCurrency.Checked) & ", " & Val(txtVAT.Text) _
                                  & ", " & Val(txtTax1.Text) & ", " & Val(txtTax2.Text) & ", " & CInt(ceDebit.Checked) & ")"

            Using cmd = New SqlCommand(Query, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                cmd.ExecuteNonQuery()

                cmd.CommandText = "SELECT @@IDENTITY;"
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() Then
                        OSerial.Text = dt(0).ToString
                    End If
                End Using
                myConn.Close()
            End Using


            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Dim theQuery As String = "INSERT INTO tblOut2 (Serial, Kind, Item, Compound, Qnty, Price, Discount, UnitPrice, [User], VAT, Tax1, Tax2) VALUES"

            For Each oRow As DataGridViewRow In oDgv.Rows
                theQuery += "('" & OSerial.Text & "', '" & oRow.Cells(1).Value & "', '" & oRow.Cells(7).Value & "', '0', '" & oRow.Cells(0).Value & "', '" & oRow.Cells(6).Value & "', '" & oRow.Cells(5).Value & "', '" & oRow.Cells(4).Value & "', '" & GlobalVariables.ID & "', " _
                    & Val(oRow.Cells(9).Value) & ", " & Val(oRow.Cells(10).Value) & " , " & Val(oRow.Cells(11).Value) & "), "
            Next

            theQuery = theQuery.Substring(0, theQuery.Length - 2)

            Using cmd = New SqlCommand(theQuery, myConn)
                cmd.ExecuteNonQuery()
            End Using
            myConn.Close()
            'Sync.SellingCode()
            If CheckEdit2.Checked = True Then
                Call Sync.Sync()
            End If

            'clear
            oQnty.Text = "1"
            oItemSerial.Text = Nothing
            oItemName.Text = Nothing
            tbCurrency.Text = ""
            tbEgyptian.Text = ""
            oUnitPrice.Text = ""
            oSubTotal.Text = ""
            oDgv.Rows.Clear()
            oItemSerial.Focus()
            oQnty.SelectAll()
            oCustomer.EditValue = Nothing
            lblCurrency.Text = "USD"
            ceVisaCurrency.Checked = False
            txtVAT.Text = ""
            txtTax1.Text = ""
            txtTax2.Text = ""
            ceDebit.Checked = False
            getCurrency()
            Call Notification("Invoice Added")

            If SaveAndPrint = True Then
                krInvoicePrint.PerformClick()
            End If

            'Update the Customer combobox
            fillCustomers()

            
        End If

        Cursor = Cursors.Default


    End Sub

    Public Sub PreTotalize()
        If Not oCustomer.EditValue = Nothing Then
            Dim Qnty, Price, Discount, Gross, Value As Single
            Dim VAT, Tax1, Tax2 As Single
            Dim dVAT, dTax1, dTax2 As Single

            dVAT = Val(txtVAT.Text)
            dTax1 = Val(txtTax1.Text)
            dTax2 = Val(txtTax2.Text)
            Dim DiscPerc As Single = Val(lblDiscount.Text.Replace("%", ""))
            For x As Integer = 0 To oDgv.RowCount - 1
                Qnty = oDgv.Rows(x).Cells(0).Value
                Price = oDgv.Rows(x).Cells(4).Value

                Discount = ExClass.GetPercentage(Price, DiscPerc)
                Discount = Discount * Qnty
                Gross = (Price * Qnty) - Discount
                VAT = ExClass.GetPercentage(Gross, dVAT)
                Tax1 = ExClass.GetPercentage(Gross, dTax1)
                Tax2 = ExClass.GetPercentage(Gross, dTax2)
                Value = Gross + VAT + Tax1 + Tax2
                oDgv.Rows(x).Cells(5).Value = Math.Round(Discount, 2, MidpointRounding.AwayFromZero)
                oDgv.Rows(x).Cells(6).Value = Math.Round(Value, 2, MidpointRounding.AwayFromZero)

                oDgv.Rows(x).Cells(9).Value = Math.Round(VAT, 2, MidpointRounding.AwayFromZero)
                oDgv.Rows(x).Cells(10).Value = Math.Round(Tax1, 2, MidpointRounding.AwayFromZero)
                oDgv.Rows(x).Cells(11).Value = Math.Round(Tax2, 2, MidpointRounding.AwayFromZero)
            Next
        End If
    End Sub
  
    Public Sub OutTotalize()

        PreTotalize()
        Dim tSales As Decimal = 0
        Dim Sales As Decimal = 0
        Dim Curr As String = lblCurrency.Text

        Dim Price, Qnty, Discount As Decimal

        For x As Integer = 0 To oDgv.RowCount - 1
            Price = oDgv.Rows(x).Cells(4).Value
            Qnty = oDgv.Rows(x).Cells(0).Value
            Discount = oDgv.Rows(x).Cells(5).Value

            
            tSales += (Price * Qnty) - Discount
        Next


        If Curr = "USD" Then
            tValueAmount = tSales
            tbTotal.Text = tSales
        Else
            tValueAmount = tSales

            Select Case Curr
                Case "EUR"
                    tSales = tSales * cEUR
                Case "EGP"
                    tSales = tSales * cEGP
                Case "GBP"
                    tSales = tSales * cGBP
                Case "RUB"
                    tSales = tSales * cRUB
                Case "CHF"
                    tSales = tSales * cCHF
                Case "CNY"
                    tSales = tSales * cCNY
            End Select

            tValueAmount = Math.Round(tValueAmount, 0, MidpointRounding.AwayFromZero)
            tSales = Math.Round(tSales, 0, MidpointRounding.AwayFromZero)
            If tSales < 1 Then
                tSales = 1
            End If
            tbTotal.Text = tSales
        End If
        getRest()

    End Sub


    Public Sub getCurrency()
        Dim query As String = "SELECT TOP(1) * FROM tblCurrency ORDER BY PrKey DESC;"
        Dim result As Double
        Using cmd = New SqlCommand(query, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Try
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() Then
                        cEUR = dt(3)
                        cGBP = dt(4)
                        cRUB = dt(5)
                        cCHF = dt(6)
                        cCNY = dt(7)
                        cEGP = dt(9)
                    Else
                        cEUR = 1
                        cEGP = 1
                        cGBP = 1
                        cRUB = 1
                        cCHF = 1
                        cCNY = 1
                    End If
                End Using
            Catch ex As Exception
                result = 1
            End Try

            myConn.Close()
        End Using
    End Sub

    Private Function CheckQnty() As Boolean
        If oDgv.RowCount = 0 Then
            Return True
        End If

        If GV.CheckQnty = False Then
            Return True
        End If

        Dim Query As String
        Dim vQnty As Boolean = True


        Dim theEnteredItems As String = "("
        For x As Integer = 0 To oDgv.RowCount - 1
            theEnteredItems += oDgv.Rows(x).Cells(7).Value.ToString & ", "
        Next

        theEnteredItems = theEnteredItems.Substring(0, theEnteredItems.Length - 2)
        theEnteredItems += ")"
        Query = "SELECT tblItems.PrKey, COALESCE((ttlIn.Total_In - COALESCE(ttlOut.Total_Out , 0)), 0) AS Net_Amount" _
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
              & " WHERE tblItems.PrKey IN " & theEnteredItems & ";"
        Dim dt As New DataTable
        If myConn.State = ConnectionState.Closed Then
            myConn.Open()
        End If
        Using cmd = New SqlCommand(Query, myConn)
            Using dr As SqlDataReader = cmd.ExecuteReader
                dt.Load(dr)
            End Using
        End Using
        myConn.Close()
        Dim itm, stk As String
        For x As Integer = 0 To dt.Rows.Count - 1
            itm = dt.Rows(x)(0).ToString
            stk = dt.Rows(x)(1).ToString

            For y As Integer = 0 To oDgv.RowCount - 1
                If oDgv.Rows(y).Cells(7).Value = itm Then
                    oDgv.Rows(y).Cells(8).Value = stk
                End If
            Next
        Next

        For Each line As DataGridViewRow In oDgv.Rows
            If Val(line.Cells(0).Value) > Val(line.Cells(8).Value) Then
                Cursor = Cursors.Default
                MessageBox.Show("!«·ﬂ„Ì… «· Ì  „ ≈œŒ«·Â« √ﬂÀ— „‰ «·⁄œœ «·„ «Õ", "Invalid Qnty", MessageBoxButtons.OK, MessageBoxIcon.Information)
                oDgv.ClearSelection()

                line.Selected = True
                vQnty = False
                Exit For
            End If

        Next
        Return vQnty
        Cursor = Cursors.Default

    End Function

    Private Sub frmCashier_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        oItemSerial.Focus()
        oItemSerial.SelectAll()
    End Sub

    Private Sub frmCashier_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control = True Then
            al1.Visible = True
            al2.Visible = True
            al3.Visible = True
            al4.Visible = True
            al5.Visible = True
            al6.Visible = True
            al7.Visible = True
            al8.Visible = True
            al9.Visible = True


            If e.KeyCode = Keys.D1 Then
                saveMemory(1)
            ElseIf e.KeyCode = Keys.D2 Then
                saveMemory(2)
            ElseIf e.KeyCode = Keys.D3 Then
                saveMemory(3)
            ElseIf e.KeyCode = Keys.D4 Then
                saveMemory(4)
            ElseIf e.KeyCode = Keys.D5 Then
                saveMemory(5)
            ElseIf e.KeyCode = Keys.D6 Then
                saveMemory(6)
            ElseIf e.KeyCode = Keys.D7 Then
                saveMemory(7)
            ElseIf e.KeyCode = Keys.D8 Then
                saveMemory(8)
            ElseIf e.KeyCode = Keys.D9 Then
                saveMemory(9)
            ElseIf e.KeyCode = Keys.Down Then
                e.Handled = True
                Select Case lblCurrency.Text
                    Case "EGP"
                        lblCurrency.Text = "EUR"
                    Case "EUR"
                        lblCurrency.Text = "USD"
                    Case "USD"
                        lblCurrency.Text = "GBP"
                    Case "GBP"
                        lblCurrency.Text = "RUB"
                    Case "RUB"
                        lblCurrency.Text = "CHF"
                    Case "CHF"
                        lblCurrency.Text = "CNY"
                    Case "CNY"
                        lblCurrency.Text = "EGP"
                End Select
                getRest()
            ElseIf e.KeyCode = Keys.Up Then
                e.Handled = True
                Select Case lblCurrency.Text
                    Case "EGP"
                        lblCurrency.Text = "CNY"
                    Case "EUR"
                        lblCurrency.Text = "EGP"
                    Case "USD"
                        lblCurrency.Text = "EUR"
                    Case "GBP"
                        lblCurrency.Text = "USD"
                    Case "RUB"
                        lblCurrency.Text = "GBP"
                    Case "CHF"
                        lblCurrency.Text = "RUB"
                    Case "CNY"
                        lblCurrency.Text = "CHF"
                End Select
                getRest()
            End If
        ElseIf e.Alt = True Then
            If e.KeyCode = Keys.D1 Then
                recallMemory(1)
            ElseIf e.KeyCode = Keys.D2 Then
                recallMemory(2)
            ElseIf e.KeyCode = Keys.D3 Then
                recallMemory(3)
            ElseIf e.KeyCode = Keys.D4 Then
                recallMemory(4)
            ElseIf e.KeyCode = Keys.D5 Then
                recallMemory(5)
            ElseIf e.KeyCode = Keys.D6 Then
                recallMemory(6)
            ElseIf e.KeyCode = Keys.D7 Then
                recallMemory(7)
            ElseIf e.KeyCode = Keys.D8 Then
                recallMemory(8)
            ElseIf e.KeyCode = Keys.D9 Then
                recallMemory(9)
            ElseIf e.KeyCode = Keys.V Then
                e.Handled = True
                If ceVisaCurrency.Checked = True Then
                    ceVisaCurrency.Checked = False
                Else
                    ceVisaCurrency.Checked = True
                End If
            End If
        End If

        If e.KeyCode = Keys.F1 Then
            oQnty.Focus()
            oQnty.SelectAll()

        ElseIf e.KeyCode = Keys.F2 Then
            oItemSerial.Focus()
            oItemSerial.SelectAll()
        ElseIf e.KeyCode = Keys.F3 Then
            oItemName.Focus()
            oItemName.SelectAll()
        ElseIf e.KeyCode = Keys.F4 Then
            e.Handled = True
            tbCurrency.Focus()
            tbCurrency.SelectAll()
        ElseIf e.KeyCode = Keys.F5 Or (e.Control And e.KeyCode = Keys.S) Then
            KryptonButton5.PerformClick()
        ElseIf e.KeyCode = Keys.F6 Then
            krInvoicePrint.PerformClick()
        ElseIf e.KeyCode = Keys.F7 Then
            KryptonButton4.PerformClick()
        ElseIf e.KeyValue = Keys.F And e.Control Then
            oSearch.Focus()
            oSearch.SelectAll()
        ElseIf e.KeyCode = Keys.F8 Then
            If KryptonPanel6.Visible = True Then
                KryptonPanel6.Visible = False
            Else
                KryptonPanel6.Visible = True
            End If
            My.Settings.VisiblePnl = KryptonPanel6.Visible
            My.Settings.Save()
        End If


    End Sub

    Private Sub frmCashier_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If Char.IsDigit(e.KeyChar) AndAlso oDgv.Focused = True AndAlso oDgv.RowCount <> 0 Then

            Dim Num As String = e.KeyChar
            If IsNumeric(Num) Then
                Dim NetQuery As String = "SELECT tblItems.PrKey, tblItems.Serial, tblItems.Name, ttlIn.Total_In, COALESCE(ttlOut.Total_Out , 0) AS Total_Out,  CONVERT(FLOAT, (ttlIn.Total_In - COALESCE(ttlOut.Total_Out , 0))) AS Net_Amount" _
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
        & " WHERE tblItems.PrKey = " & oDgv.CurrentRow.Cells(7).Value & ";"
                Dim theQuant As Decimal
                Using cmd = New SqlCommand(NetQuery, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Using dt As SqlDataReader = cmd.ExecuteReader
                        If dt.Read() And Not IsDBNull(dt(5)) Then
                            theQuant = dt(5)
                        Else
                            theQuant = 0
                        End If
                    End Using
                    myConn.Close()
                End Using

                oDgv.CurrentRow.Cells(0).Value = Num
                oDgv.CurrentRow.Cells(5).Value = "0"
                oDgv.CurrentRow.Cells(6).Value = Math.Round(oDgv.CurrentRow.Cells(0).Value * oDgv.CurrentRow.Cells(4).Value, 2, MidpointRounding.AwayFromZero)
                lbNetQnty.Text = Math.Round(Val(theQuant - Val(Num)), 2, MidpointRounding.AwayFromZero)
            End If
            Call OutTotalize()
        ElseIf (e.KeyChar = "+" OrElse e.KeyChar = "-") AndAlso oDgv.Focused = True AndAlso oDgv.RowCount <> 0 Then
            Dim Num As Integer

            If e.KeyChar = "+" Then
                Num = oDgv.CurrentRow.Cells(0).Value + 1
            ElseIf e.KeyChar = "-" Then
                Num = oDgv.CurrentRow.Cells(0).Value - 1
            End If

            Dim NetQuery As String = "SELECT tblItems.PrKey, tblItems.Serial, tblItems.Name, ttlIn.Total_In, COALESCE(ttlOut.Total_Out , 0) AS Total_Out,  (ttlIn.Total_In - COALESCE(ttlOut.Total_Out , 0)) AS Net_Amount" _
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
    & " WHERE tblItems.PrKey = " & oDgv.CurrentRow.Cells(7).Value & ";"
            Dim theQuant As Decimal
            Using cmd = New SqlCommand(NetQuery, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() And Not IsDBNull(dt(5)) Then
                        theQuant = dt(5)
                    Else
                        theQuant = 0
                    End If
                End Using
                myConn.Close()
            End Using

            If Num = 0 Then
                oDgv.Rows.Remove(oDgv.CurrentRow)
                oQnty.Text = "1"
                lbNetQnty.Text = Val(theQuant - Num)
                oItemSerial.Focus()
                oItemSerial.SelectAll()
            Else
                oDgv.CurrentRow.Cells(0).Value = Num
                oDgv.CurrentRow.Cells(5).Value = "0"
                oDgv.CurrentRow.Cells(6).Value = Math.Round(oDgv.CurrentRow.Cells(0).Value * oDgv.CurrentRow.Cells(4).Value, 2, MidpointRounding.AwayFromZero)
                lbNetQnty.Text = Val(theQuant - Num)

            End If

            Call OutTotalize()
        End If
    End Sub

    Private Sub frmCashier_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.Control = False Then
            al1.Visible = False
            al2.Visible = False
            al3.Visible = False
            al4.Visible = False
            al5.Visible = False
            al6.Visible = False
            al7.Visible = False
            al8.Visible = False
            al9.Visible = False
        End If
    End Sub


    Private Sub frmCashier_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        oDgv.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        oDgv.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        oDgv.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        oDgv.Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        oDgv.Columns(5).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        oDgv.Columns(6).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        oDgv.Columns(7).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        getCurrency()

        ToolStripStatusLabel3.Text = GlobalVariables.user
        'Fill the items
        Dim FillQuery As String = "SELECT PrKey, Serial, Name FROM tblItems" _
                                                          & " UNION ALL" _
                                                          & " SELECT tblItems.PrKey, tblMultiCodes.Code AS Serial, tblItems.Name" _
                                                          & " FROM tblMultiCodes INNER JOIN tblItems ON tblMultiCodes.Item = tblItems.PrKey" _
                                                          & " UNION ALL" _
                                                          & " SELECT PrKey, PackageSerial AS Code, Name + '**' FROM tblItems" _
                                                          & " WHERE PackageSerial IS NOT NULL AND PackageSerial !=''" _
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
            oSearch.Text = Nothing

            oItemName.DataSource = ds.Tables(0)
            oItemName.DisplayMember = "Name"
            oItemName.Text = Nothing

            myConn.Close()

        End Using

        fillCustomers()

        oQnty.Text = "1"
        oItemSerial.Text = ""
        oItemSerial.Focus()

        Dim rep As New DevExpress.XtraReports.UI.XtraReport()


        CheckEdit1.Checked = My.Settings.SaveOrPrint
        KryptonPanel6.Visible = My.Settings.VisiblePnl
        Call frmMain.AutoRate(Today)

        Authorize()
        ''close all the other forms
        'For i As Integer = My.Application.OpenForms.Count - 1 To 0 Step -1
        '    If My.Application.OpenForms.Item(i) IsNot Me Then
        '        My.Application.OpenForms.Item(i).Close()
        '    End If
        'Next
    End Sub

    Private Sub KryptonButton8_Click(sender As System.Object, e As System.EventArgs) Handles KryptonButton8.Click

        If Val(oQnty.Text) = 0 Then
            MessageBox.Show("ÌÃ» ≈œŒ«· ﬂ„Ì… ’ÕÌÕ…!!", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information)
            oQnty.Focus()
            oQnty.SelectAll()
        ElseIf oItemSerial.Text = "" Then
            MessageBox.Show("ÌÃ» ≈œŒ«· ﬂÊœ ’ÕÌÕ!", "Invalid Serial", MessageBoxButtons.OK, MessageBoxIcon.Information)
            oItemSerial.Focus()
            oItemSerial.SelectAll()

        Else

            'check if item already exists

            Dim result() As String = GetItem(oItemSerial.Text).Split(";")
            Dim PrKey As Integer = result(0)
            If PrKey = 0 Then
                MessageBox.Show("«·’‰› «·–Ì ﬁ„  »≈œŒ«·Â €Ì— „ÊÃÊœ!", "Invalid Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                oItemSerial.Focus()
                oItemSerial.SelectAll()
                Exit Sub
            End If

            Dim prQnty As Decimal
            Dim type As String = "SGL"
            Dim wholeQnty As Single = Math.Round(Val(oQnty.Text), 2, MidpointRounding.AwayFromZero)
            Dim dsc As Single = 0
            If oItemSerial.Text = result(6) Then
                type = "PKG"
                wholeQnty = result(4)
                dsc = Val(result(3)) - (Val(result(5)) / Val(result(4)))

            End If


            For x As Integer = 0 To oDgv.RowCount - 1

                If (oDgv.Rows(x).Cells(7).Value.ToString = result(0)) AndAlso (oDgv.Rows(x).Cells(1).Value.ToString = type) Then
                    prQnty = oDgv.Rows(x).Cells(0).Value
                    dsc *= (wholeQnty + prQnty)
                    dsc = Math.Round(dsc, 2, MidpointRounding.AwayFromZero)
                    oDgv.Rows(x).SetValues((wholeQnty + prQnty), type, oItemSerial.Text.ToUpper, result(2).ToUpper, Math.Round(Val(oUnitPrice.Text), 2, MidpointRounding.AwayFromZero), dsc, Math.Round((Val(oUnitPrice.Text) * (wholeQnty + prQnty) - dsc), 2, MidpointRounding.AwayFromZero), PrKey)




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


            Next

            'Add the row to the datagrid
            dsc *= (wholeQnty + prQnty)
            dsc = Math.Round(dsc, 2, MidpointRounding.AwayFromZero)
            oDgv.Rows.Add((wholeQnty + prQnty), type, oItemSerial.Text.ToUpper, result(2).ToUpper, Math.Round(Val(oUnitPrice.Text), 2, MidpointRounding.AwayFromZero), dsc, Math.Round((Val(oUnitPrice.Text) * (wholeQnty + prQnty) - dsc), 2, MidpointRounding.AwayFromZero), PrKey)


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
        ' End If
        Call OutTotalize()

    End Sub

    Private Sub oEdit_Click(sender As System.Object, e As System.EventArgs) Handles oEdit.Click

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
            oItemSerial.Focus()
            oItemSerial.SelectAll()
        Else

            'check if item already exists
            Dim result() As String = GetItem(oItemSerial.Text).Split(";")
            Dim PrKey As Integer = result(0)

            If PrKey = 0 Then
                MessageBox.Show("«·’‰› «·–Ì ﬁ„  »≈œŒ«·Â €Ì— „ÊÃÊœ!", "Invalid Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                oItemSerial.Focus()
                oItemSerial.SelectAll()
                Exit Sub
            End If



            'check if the item has been added to the grid before

            If oDgv.CurrentRow.Cells(7).Value <> result(0) Then
                For x As Integer = 0 To oDgv.RowCount - 1
                    If oDgv.Rows(x).Cells(7).Value = result(0) Then
                        MessageBox.Show("·ﬁœ ﬁ„  »«·›⁄· »≈œŒ«· Â–« «·’‰› „‰ ﬁ»·!", "Duplicated Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
                Next
            End If

            Dim cr As Integer = oDgv.CurrentRow.Index

            For x As Integer = 0 To oDgv.RowCount - 1

                If Not cr = x Then
                    If (oDgv.Rows(x).Cells(7).Value.ToString = result(0)) Then
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

            Dim type As String = "SGL"
            Dim wholeQnty As Single = Val(oQnty.Text)
            Dim dsc As Single = 0

            If oItemSerial.Text = result(6) Then
                type = "PKG"
                wholeQnty = result(4)
                dsc = (Val(result(3)) * wholeQnty) - Val(result(5))
                dsc = Val(result(3)) - (Val(result(5)) / Val(result(4)))
                dsc = Math.Round(dsc, 2, MidpointRounding.AwayFromZero)
            End If


            Dim theRow As String()
            theRow = New String() {(wholeQnty), type, oItemSerial.Text.ToUpper, result(2).ToUpper, Math.Round(Val(oUnitPrice.Text), 2, MidpointRounding.AwayFromZero), dsc, Math.Round((Val(oUnitPrice.Text) * (wholeQnty) - dsc), 2, MidpointRounding.AwayFromZero), PrKey}

            oDgv.CurrentRow.SetValues(theRow)

            KryptonButton8.Enabled = True
            oRemove.Enabled = True



            Call Notification("Item modified!")
            oQnty.Text = "1"
            oItemSerial.Text = Nothing
            oItemName.Text = Nothing
            oUnitPrice.Text = ""
            oSubTotal.Text = ""
            lblCurrency.Text = "USD"
            ceVisaCurrency.Checked = False
            oItemSerial.Focus()
            Call OutTotalize()
            oQnty.SelectAll()
        End If



    End Sub

    Private Sub oRemove_Click(sender As System.Object, e As System.EventArgs) Handles oRemove.Click
        If Not oDgv.RowCount = 0 Then
            oDgv.Rows.Remove(oDgv.CurrentRow)

            'update the discount 
            Call OutTotalize()
            oQnty.Text = "1"
            oItemSerial.Text = Nothing
            oItemName.Text = Nothing
            oUnitPrice.Text = ""
            oSubTotal.Text = ""

            oItemSerial.Focus()
            oQnty.SelectAll()
            Notification("Current item removed!")
            Call OutTotalize()
        End If
    End Sub

    Private Sub KryptonButton5_Click(sender As System.Object, e As System.EventArgs) Handles KryptonButton5.Click
        If CheckEdit1.Checked = True Then
            SaveOrder(True)
        Else
            SaveOrder(False)
        End If
    End Sub

    Private Sub KryptonButton4_Click(sender As System.Object, e As System.EventArgs) Handles KryptonButton4.Click
        oQnty.Text = "1"
        oItemSerial.Text = Nothing
        oItemName.Text = Nothing
        tbCurrency.Text = ""
        tbEgyptian.Text = ""
        oUnitPrice.Text = ""
        oSubTotal.Text = ""
        oDgv.Rows.Clear()
        oItemSerial.Focus()
        oQnty.SelectAll()
        oCustomer.EditValue = Nothing
        oSearch.Text = ""
        tValueAmount = 0
        ceVisaCurrency.Checked = False
        lblCurrency.Text = "USD"
        LabelControl15.Visible = False
        LabelControl16.Visible = False
        oTime.Visible = False
        oUser.Visible = False
        txtVAT.Text = ""
        txtTax1.Text = ""
        txtTax2.Text = ""
        ceDebit.Checked = False
        Call OutTotalize()
        KryptonButton5.Enabled = True
    End Sub

    Private Sub krInvoicePrint_Click(sender As System.Object, e As System.EventArgs) Handles krInvoicePrint.Click
        ExReport.getReceipt(OSerial.Text, True)
    End Sub

    Private Sub krInvoiceShow_Click(sender As System.Object, e As System.EventArgs) Handles krInvoiceShow.Click
        ExReport.getReceipt(OSerial.Text, False)
    End Sub

    Private Sub oSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles oSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Not oSearch.Text = "" Then
                Dim Query1, Query2 As String

                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If

                Query1 = "SELECT *, (pEUR + pUSD + pGBP + pRUB + pCHF + pCNY) AS OtherCurrency FROM tblOut1 WHERE Serial = " & Val(oSearch.Text)
                Dim cust As Integer = Nothing

                Using cmd = New SqlCommand(Query1, myConn)
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            tbEgyptian.Text = dr(12).ToString
                            OSerial.Text = dr(0).ToString
                            If Not IsDBNull(dr(4)) Then
                                cust = dr(4)
                            End If

                            ceVisaEGP.Checked = dr(21)
                            ceVisaCurrency.Checked = dr(22)
                            tValueAmount = dr(19)
                            lblCurrency.Text = dr(20)
                            txtVAT.Text = dr(23)
                            txtTax1.Text = dr(24)
                            txtTax2.Text = dr(25)
                            ceDebit.Checked = dr(26)
                            tbCurrency.Text = dr(27)
                        End If
                    End Using
                End Using

                oCustomer.EditValue = cust
                Query2 = "SELECT tblOut2.Serial, tblOut2.Kind, tblOut2.Item, tblItems.Serial AS ItemSerial, tblItems.Name AS ItemName, tblOut2.Compound, CASE WHEN (tblOut2.Compound = '' OR tblOut2.Compound = 0 ) THEN tblItems.Name ELSE tblCompounds.Name END AS CompoundName, tblOut2.Qnty, tblOut2.Price, tblOut2.UnitPrice, tblOut2.Discount" _
                    & " FROM tblOut2 LEFT OUTER JOIN tblItems" _
                    & " ON tblOut2.Item = tblItems.PrKey" _
                    & " LEFT OUTER JOIN tblCompounds" _
                    & " ON tblOut2.Compound = tblCompounds.Serial" _
                    & " WHERE(tblOut2.Serial = " & Val(oSearch.Text) & ")" _
                    & " ORDER BY tblOut2.PrKey"
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If


                Using cmd = New SqlCommand(Query2, myConn)

                    Using dr As SqlDataReader = cmd.ExecuteReader
                        Dim dt As New DataTable
                        dt.Load(dr)
                        oDgv.Rows.Clear()
                        For x As Integer = 0 To dt.Rows.Count - 1
                            oDgv.Rows.Add(dt(x)(7), dt(x)(1), dt(x)(3), dt(x)(4), dt(x)(9), dt(x)(10), dt(x)(8), dt(x)(2))
                        Next
                    End Using

                End Using


                Call OutTotalize()

                LabelControl16.Visible = True
                LabelControl15.Visible = True
                oTime.Visible = True
                oUser.Visible = True
                KryptonButton5.Enabled = False


                'show the date and the username of the invoice
                Dim theQuery As String = "SELECT tblOut1.[Date], tblLogin.UserName AS [User], CONVERT(NVARCHAR(5), tblOut1.[Time], 108) AS [Time] FROM tblOut1 INNER JOIN tblLogin ON tblOut1.[User] = tblLogin.Sr" _
                                         & " WHERE tblOut1.Serial = '" & Val(oSearch.Text) & "'"

                Using cmd = New SqlCommand(theQuery, myConn)
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            oTime.Text = dr(0) & "   " & dr(2)
                            oUser.Text = dr(1)
                        Else
                            oTime.Text = ""
                            oUser.Text = ""
                        End If
                    End Using
                End Using
                myConn.Close()
                If GlobalVariables.authority <> "User" And GlobalVariables.authority <> "Cashier" Then
                    SimpleButton6.Visible = True
                End If
            Else
                oQnty.Text = "1"
                oItemSerial.Text = Nothing
                oItemName.Text = Nothing
                oUnitPrice.Text = ""
                oSubTotal.Text = ""
                SimpleButton6.Visible = False
                oDgv.Rows.Clear()
                oItemSerial.Focus()
                lblCurrency.Text = "USD"
                ceVisaCurrency.Checked = False
                tValueAmount = 0
                oCustomer.EditValue = Nothing
                KryptonButton5.Enabled = True
                txtVAT.Text = ""
                txtTax1.Text = ""
                txtTax2.Text = ""
                ceDebit.Checked = False
                LabelControl16.Visible = False
                LabelControl15.Visible = False
                oTime.Visible = False
                oUser.Visible = False
            End If
        End If
    End Sub

    Private Sub oSearch_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles oSearch.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub oSearch_TextChanged(sender As Object, e As System.EventArgs) Handles oSearch.TextChanged
        If oSearch.Text = "" Then
            oQnty.Text = "1"
            oItemSerial.Text = Nothing
            oItemName.Text = Nothing
            tbCurrency.Text = ""
            tbEgyptian.Text = ""
            oUnitPrice.Text = ""
            oSubTotal.Text = ""
            oDgv.Rows.Clear()
            oItemSerial.Focus()
            oCustomer.EditValue = Nothing
            oSearch.Text = ""
            lblCurrency.Text = "USD"
            txtVAT.Text = ""
            txtTax1.Text = ""
            txtTax2.Text = ""
            ceDebit.Checked = False
            ceVisaCurrency.Checked = False
            tValueAmount = 0
            KryptonButton5.Enabled = True
        End If
    End Sub

    Private Sub oItemSerial_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles oItemSerial.KeyDown
        If e.Control = True And e.KeyCode = Keys.Enter Then
            tbEgyptian.Focus()
            tbEgyptian.SelectAll()
            Exit Sub
        End If

        If e.KeyCode = Keys.Enter And Not oItemSerial.Text = "" Then

            'shortcut to save the invoice
            If oItemSerial.Text = " " Then
                SaveOrder(False)
                Exit Sub
            ElseIf oItemSerial.Text = "  " Then
                SaveOrder(True)
                Exit Sub
            End If

            'oItemSerial.Text = oItemSerial.Text.ToUpper

            If Not oItemSerial.Text = "" Then
                Dim resString As String = GetItem(oItemSerial.Text)
                Dim result() As String = resString.Split(";")
                Application.DoEvents()
                If result(2) = "0" And oItemSerial.Text Like "200" & "*" Then
                    '   oItemName.Text = result(2)
                    'Else
                    ''''to fix the barcode error!
                    Dim brcd As Int64
                    Dim extraQuery As String = "SELECT COUNT(tblItems.PrKey) FROM tblItems" _
                                               & " LEFT OUTER JOIN tblMultiCodes ON tblItems.PrKey = tblMultiCodes.Item" _
                                               & " WHERE tblItems.Serial = '" & oItemSerial.Text & "' OR tblMultiCodes.Code = '" & oItemSerial.Text & "'"
                    Using cmd = New SqlCommand(extraQuery, myConn)
                        If myConn.State = ConnectionState.Closed Then
                            myConn.Open()
                        End If
                        brcd = cmd.ExecuteScalar
                        myConn.Close()
                    End Using
                    If brcd = 0 Then
                        Try
                            '####
                            'add weighed items
                            'Dim wwcode As String
                            Dim wwQnty As Decimal = 0.0
                            Dim FstPart, SndPart As String
                            FstPart = oItemSerial.Text.Substring(0, 7)
                            SndPart = oItemSerial.Text.Substring(8, 2) & "." & oItemSerial.Text.Substring(10)
                            SndPart *= 10
                            oItemSerial.Text = FstPart
                            If myConn.State = ConnectionState.Closed Then
                                myConn.Open()
                            End If
                            Using cmd2 = New SqlCommand("SELECT * FROM tblItems WHERE Serial = '" & FstPart & "'", myConn)
                                Using dr As SqlDataReader = cmd2.ExecuteReader
                                    If dr.Read() Then
                                        wwQnty = dr(3)
                                        oUnitPrice.Text = wwQnty
                                        oQnty.Text = (Convert.ToDecimal(SndPart)) / 100
                                    Else
                                        oItemName.Text = Nothing
                                    End If
                                End Using
                            End Using
                            myConn.Close()
                        Catch ex As Exception

                        End Try

                    End If


                End If
            Else
                oItemName.Text = Nothing
            End If
            KryptonButton8.PerformClick()
        ElseIf e.Control = True And e.KeyCode = Keys.K And Not oItemSerial.Text = "" Then
            oItemSerial.Text = frmMain.FindSerial(oItemSerial.Text)

        End If

    End Sub

    Private Sub oItemSerial_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles oItemSerial.KeyPress
        If e.KeyChar = "+" And oItemSerial.Text Like "[*]" & "*" Then
            Dim Num As String = oItemSerial.Text.Substring(1)
            If IsNumeric(Num) Then
                Num = Math.Round(Val(Num), 3, MidpointRounding.AwayFromZero)
                e.Handled = True
                Try

                    Dim NetQuery As String = "SELECT tblItems.PrKey, tblItems.Serial, tblItems.Name, ttlIn.Total_In, COALESCE(ttlOut.Total_Out , 0) AS Total_Out,  (ttlIn.Total_In - COALESCE(ttlOut.Total_Out , 0)) AS Net_Amount" _
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
                   & " WHERE tblItems.PrKey = '" & oDgv.CurrentRow.Cells(7).Value & "'"
                    Dim theQuant As Decimal
                    Using cmd = New SqlCommand(NetQuery, myConn)
                        If myConn.State = ConnectionState.Closed Then
                            myConn.Open()
                        End If
                        Using dt As SqlDataReader = cmd.ExecuteReader
                            If dt.Read() Then
                                theQuant = dt(5)
                            End If
                        End Using
                    End Using

                    For Each row In oDgv.SelectedRows
                        row.Cells(0).Value = Num
                        row.Cells(6).Value = Math.Round(row.Cells(0).Value * row.Cells(4).Value, 2, MidpointRounding.AwayFromZero)
                    Next

                    Call OutTotalize()

                Catch ex As Exception

                End Try
                oItemSerial.Text = ""
                oItemSerial.Focus()
            End If
        ElseIf e.KeyChar = "-" And Not oDgv.Rows.Count = 0 And oItemSerial.Text = "" Then
            e.Handled = True
            If oDgv.CurrentRow.Cells(0).Value <= 1 Then
                oDgv.Rows.Remove(oDgv.CurrentRow)
            Else
                For Each row In oDgv.SelectedRows
                    row.Cells(0).Value -= 1
                    row.Cells(6).Value = Math.Round(row.Cells(0).Value * row.Cells(4).Value, 2, MidpointRounding.AwayFromZero)
                    row.Cells(5).Value = "0"
                Next
            End If
            Call OutTotalize()
        ElseIf e.KeyChar = "+" And oItemSerial.Text = "" And Not oDgv.RowCount = 0 Then

            e.Handled = True

            For Each row In oDgv.SelectedRows
                row.Cells(0).Value += 1
                row.Cells(5).Value = "0"
                row.cells(6).Value = Math.Round(row.Cells(0).Value * row.Cells(4).Value, 2, MidpointRounding.AwayFromZero)
            Next
            Call OutTotalize()
        End If


    End Sub

    Private Sub oItemSerial_LostFocus(sender As Object, e As System.EventArgs) Handles oItemSerial.LostFocus
        oItemSerial.Text = oItemSerial.Text.ToUpper
        Dim itmName As String = ""
        If Not oItemSerial.Text = "" Then
            Dim result() As String = GetItem(oItemSerial.Text).Split(";")
            itmName = result(2)
        End If

        If itmName = "0" Then
            oItemName.Text = ""
        Else
            oItemName.Text = itmName
        End If
    End Sub

    Private Sub oItemSerial_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles oItemSerial.SelectedIndexChanged
        If oItemSerial.Text <> "" Then

            'enter the value


            ''''To get the price:
            Dim Result() As String = GetItem(oItemSerial.Text).Split(";")

            oUnitPrice.Text = Result(3)


            Try
                'Show the Net Qnty
                Dim NetQuery As String
                NetQuery = "SELECT tblItems.PrKey, tblItems.Serial, tblItems.Name, ttlIn.Total_In, COALESCE(ttlOut.Total_Out , 0) AS Total_Out,  CONVERT(FLOAT, (ttlIn.Total_In - COALESCE(ttlOut.Total_Out , 0))) AS Net_Amount" _
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
                    & " WHERE tblItems.PrKey = " & Result(0)

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
                        If (oDgv.Rows(x).Cells(7).Value.ToString = Result(0)) Then
                            theQuant -= oDgv.Rows(x).Cells(0).Value
                        End If
                    Next
                    lbNetQnty.Text = theQuant.ToString

                    myConn.Close()

                End Using
            Catch ex As Exception
                lbNetQnty.Text = "00"
            End Try
        Else
            lbNetQnty.Text = "00"
        End If
    End Sub

    Private Sub oItemName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles oItemName.KeyDown

        If e.KeyCode = Keys.Enter And Not oItemName.Text = "" Then
            oItemSerial.Focus()
        End If
    End Sub

    Private Sub oItemName_LostFocus(sender As Object, e As System.EventArgs) Handles oItemName.LostFocus

        If Not oItemName.Text = "" Then
            Dim Query As String
            Dim str As String = oItemName.Text
            If str Like "*" & "[**]" Then
                str = str.Substring(0, str.Length - 2)
                Query = "SELECT PackageSerial FROM tblItems WHERE Name = N'" & str & "';"
            Else
                Query = "SELECT Serial FROM tblItems WHERE Name = N'" & str & "';"
            End If
            Using cmd = New SqlCommand(Query, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Try
                    oItemSerial.Text = cmd.ExecuteScalar
                Catch ex As Exception
                    oItemSerial.Text = ""
                End Try

                myConn.Close()
            End Using
        Else
            oItemSerial.Text = Nothing
        End If

        Dim result() As String = GetItem(oItemSerial.Text).Split(";")
        oUnitPrice.Text = result(3)

    End Sub

    Private Sub oQnty_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles oQnty.EditValueChanged
        Dim str1, str2 As String
        Try
            str1 = oQnty.Text.Substring(0, oQnty.Text.IndexOf(".") + 4)
        Catch ex As Exception
            str1 = oQnty.Text
        End Try
        str2 = (Val(str1) * Val(oUnitPrice.Text))
        If str2 Like "*" & "." & "*" Then
            Try
                str2 = str2.Substring(0, str2.IndexOf(".") + 3)
            Catch ex As Exception

            End Try
        End If
        oSubTotal.Text = str2

    End Sub

    Private Sub oUnitPrice_EditValueChanged(sender As System.Object, e As System.EventArgs)
        oSubTotal.Text = Val(oQnty.Text) * Val(oUnitPrice.Text)
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
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

    Private Sub oDgv_CellClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles oDgv.CellClick
        Try
            oDgv.CurrentRow.Selected = True

            oQnty.Text = oDgv.CurrentRow.Cells(0).Value
            oItemName.Text = oDgv.CurrentRow.Cells(3).Value
            oItemSerial.Text = oDgv.CurrentRow.Cells(2).Value
            oUnitPrice.Text = oDgv.CurrentRow.Cells(4).Value
            oSubTotal.Text = oDgv.CurrentRow.Cells(6).Value

        Catch ex As Exception

        End Try
    End Sub

    Private Sub oDgv_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles oDgv.RowsAdded
        oDgv.Rows(oDgv.RowCount - 1).Selected = True
    End Sub

    Private Sub oDgv_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles oDgv.RowsRemoved
        If Not oDgv.RowCount = 0 Then
            oDgv.Rows(oDgv.RowCount - 1).Selected = True
        End If
    End Sub

    Private Sub tbTotal_TextChanged(sender As Object, e As System.EventArgs) Handles tbTotal.TextChanged
        Try
            tbRest.Text = Val(tbCurrency.Text) - Val(tbTotal.Text)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub oUnitPrice_TextChanged(sender As Object, e As System.EventArgs) Handles oUnitPrice.TextChanged
        oSubTotal.Text = Val(oQnty.Text) * Val(oUnitPrice.Text)
    End Sub

    Private Sub oQnty_GotFocus(sender As Object, e As System.EventArgs) Handles oQnty.GotFocus
        oQnty.SelectAll()
    End Sub

    Private Sub oQnty_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles oQnty.KeyDown
        If (e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down) And Not Val(oQnty.Text) = 0 Then
            oItemSerial.Focus()
            oItemSerial.SelectAll()
        End If
    End Sub

    Private Sub SimpleButton3_Click(sender As System.Object, e As System.EventArgs) Handles btnChangeUser.Click
        frmLogin.Close()
        frmLogin.UsernameTextBox.Text = Nothing
        frmLogin.PasswordTextBox.Text = Nothing
        frmLogin.OpenCashier = True
        frmLogin.ShowDialog()
        frmLogin.OpenCashier = False
        ToolStripStatusLabel3.Text = GlobalVariables.user
        Authorize()
        getCurrency()
    End Sub

    Private Sub SimpleButton4_Click(sender As System.Object, e As System.EventArgs) Handles btnChangePassword.Click
        frmPassword.ShowDialog()
    End Sub

    Private Sub SimpleButton1_Click(sender As System.Object, e As System.EventArgs) Handles btnIncome.Click
        frmCash.Text = "IMPORTS"
        frmCash.ShowDialog()
        oItemSerial.Focus()
        oItemSerial.SelectAll()
    End Sub

    Private Sub SimpleButton2_Click(sender As System.Object, e As System.EventArgs) Handles btnExpenditure.Click
        frmCash.Text = "EXPORTS"
        frmCash.ShowDialog()
        oItemSerial.Focus()
        oItemSerial.SelectAll()
    End Sub

    Private Sub SimpleButton5_Click(sender As System.Object, e As System.EventArgs) Handles btnCurrency.Click
        frmCurrency.ShowDialog()
        OutTotalize()
    End Sub

    Private Sub oQnty_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles oQnty.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        ElseIf e.KeyChar = "." And oQnty.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub CheckEdit1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckEdit1.CheckedChanged
        If CheckEdit1.Checked = True Then
            KryptonButton5.Text = "Õ›Ÿ Êÿ»«⁄…"
        Else
            KryptonButton5.Text = "Õ›‹‹‹‹Ÿ"
        End If
        My.Settings.SaveOrPrint = CheckEdit1.Checked
        My.Settings.Save()
    End Sub
    Private Sub tbPaid_GotFocus(sender As Object, e As System.EventArgs) Handles tbCurrency.GotFocus
        tbCurrency.SelectAll()
    End Sub

    Private Sub tbPaid_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles tbCurrency.KeyDown
        If e.KeyCode = Keys.Enter Then
            KryptonButton5.PerformClick()
        ElseIf e.KeyCode = Keys.V Then
            If ceVisaCurrency.Checked = True Then
                ceVisaCurrency.Checked = False
            Else
                ceVisaCurrency.Checked = True
            End If
            e.Handled = True
        End If
    End Sub

    Private Sub tbPaid_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles tbCurrency.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        ElseIf e.KeyChar = "." And tbCurrency.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub SimpleButton7_Click(sender As System.Object, e As System.EventArgs) Handles btnSettings.Click
        If GlobalVariables.authority = "Admin" OrElse ExClass.validAccess Then
            frmMain.Show()
            Me.Close()
        End If
    End Sub

    Private Sub ToolStripStatusLabel3_TextChanged(sender As Object, e As System.EventArgs) Handles ToolStripStatusLabel3.TextChanged
        If GlobalVariables.authority <> "User" And GlobalVariables.authority <> "Cashier" Then
            'SimpleButton7.Visible = True
            'SimpleButton8.Visible = True
        Else
            'SimpleButton7.Visible = False
            'SimpleButton8.Visible = False
        End If
    End Sub

    Private Sub SimpleButton6_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton6.Click
        If GlobalVariables.authority <> "Admin" AndAlso ExClass.validAccess = False Then
            Exit Sub
        End If

Restart:

        If ceDebit.Checked = True Then
            tbEgyptian.Text = "0"
            ceVisaEGP.Checked = False
            tbCurrency.Text = "0"
            ceVisaCurrency.Checked = False
        End If

        If Val(tbRest.Text) < 0 AndAlso ceDebit.Checked = False Then
            MessageBox.Show("«·„»·€ «·„œ›Ê€ €Ì— ’ÕÌÕ!", "Wrong Payment", MessageBoxButtons.OK, MessageBoxIcon.Information)
            tbCurrency.Focus()
            tbCurrency.SelectAll()
        ElseIf ceDebit.Checked = True AndAlso oCustomer.EditValue = Nothing Then
            MessageBox.Show("!ÌÃ» ≈œŒ«· «”„ «·⁄„Ì· ··œ›⁄ «·¬Ã·", "No Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
            oCustomer.Focus()
            oCustomer.SelectAll()
        Else


            'check if the valid qnty
            If CheckQnty() = False Then
                Exit Sub
            End If


            'add to out1
            Dim pEGP, pEUR, pUSD, pGBP, pRUB, pCHF, pCNY As Double
            pEGP = 0
            pEUR = 0
            pUSD = 0
            pGBP = 0
            pRUB = 0
            pCHF = 0
            pCNY = 0
            Select Case lblCurrency.Text
                Case "EUR"
                    pEUR = Val(tbTotal.Text)
                Case "USD"
                    pUSD = Val(tbTotal.Text)
                Case "GBP"
                    pGBP = Val(tbTotal.Text)
                Case "RUB"
                    pRUB = Val(tbTotal.Text)
                Case "CHF"
                    pCHF = Val(tbTotal.Text)
                Case "CNY"
                    pCNY = Val(tbTotal.Text)
            End Select
            pEGP = Val(tbEgyptian.Text)
            Dim Cust As String = "NULL"
            If Not oCustomer.EditValue = Nothing Then
                Cust = oCustomer.EditValue
            End If
            Dim Query As String = "UPDATE tblOut1 SET Customer = " & Cust & ", SubTotal = '" & tbTotal.Text & "', Discount = '" & Val(tbCurrency.Text) & "', NetTotal = '" & tbRest.Text & "', [User] = '" & GlobalVariables.ID & "'" _
                                  & ", pEGP = " & pEGP & ", pEUR = " & pEUR & ", pUSD = " & pUSD & ", pGBP = " & pGBP _
                                  & ", pRUB = " & pRUB & ", pCHF = " & pCHF & ", pCNY = " & pCNY & ", RealValue = " & tValueAmount & ", Currency = '" & lblCurrency.Text & "'" _
                                  & ", Visa1 = " & CInt(ceVisaEGP.Checked) & ", Visa2 = " & CInt(ceVisaCurrency.Checked) _
                                  & ", VAT = " & Val(txtVAT.Text) & ", Tax1 = " & Val(txtTax1.Text) & ", Tax2 = " & Val(txtTax2.Text) & ", Debit = " & CInt(ceDebit.Checked) _
                                  & " WHERE Serial = '" & oSearch.Text & "'"

            Using cmd = New SqlCommand(Query, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                cmd.ExecuteNonQuery()
                myConn.Close()

            End Using


            'Add to Out2

            'remove first then add

            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If


            'remove
            Using cmd = New SqlCommand("DELETE FROM tblOut2 WHERE Serial = '" & oSearch.Text & "'", myConn)
                cmd.ExecuteNonQuery()
            End Using

            If Not oDgv.RowCount = 0 Then
                Dim theQuery As String = "INSERT INTO tblOut2 (Serial, Kind, Item, Compound, Qnty, Price, Discount, UnitPrice, [User], VAT, Tax1, Tax2) VALUES"

                For Each oRow As DataGridViewRow In oDgv.Rows
                    theQuery += "('" & OSerial.Text & "', '" & oRow.Cells(1).Value & "', '" & oRow.Cells(7).Value & "', '0', '" & oRow.Cells(0).Value & "', '" & oRow.Cells(6).Value & "', '" & oRow.Cells(5).Value & "', '" & oRow.Cells(4).Value & "', '" & GlobalVariables.ID & "', " & Val(oRow.Cells(9).Value) & ", " & Val(oRow.Cells(10).Value) & " ," & Val(oRow.Cells(11).Value) & "), "
                Next

                theQuery = theQuery.Substring(0, theQuery.Length - 2)

                Using cmd = New SqlCommand(theQuery, myConn)
                    cmd.ExecuteNonQuery()
                End Using
                myConn.Close()
            End If



            myConn.Close()

            'clear
            oQnty.Text = "1"
            oItemSerial.Text = Nothing
            oItemName.Text = Nothing
            tbCurrency.Text = ""
            tbEgyptian.Text = ""
            oUnitPrice.Text = ""
            oSubTotal.Text = ""
            oDgv.Rows.Clear()
            oItemSerial.Focus()
            oQnty.SelectAll()
            lblCurrency.Text = "USD"
            ceVisaCurrency.Checked = False
            tValueAmount = 0
            oCustomer.EditValue = Nothing
            txtVAT.Text = ""
            txtTax1.Text = ""
            txtTax2.Text = ""
            ceDebit.Checked = False
            Call Notification("Invoice Added")

            If KryptonButton5.Text <> "Õ›‹‹‹‹Ÿ" Then
                krInvoicePrint.PerformClick()
            End If

        End If

    End Sub

    Private Sub SimpleButton8_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton8.Click

        If GlobalVariables.authority <> "Admin" AndAlso ExClass.validAccess = False Then
            Exit Sub
        End If

Restart:

        Dim dia As DialogResult = MessageBox.Show("Â·  —Ìœ »«· «ﬂÌœ ≈Œ—«Ã «·›« Ê—… ﬂÂÊ«·ﬂø", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If dia = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        If oDgv.RowCount = 0 Then
            MessageBox.Show("·«  ÊÃœ √Ì »Ì«‰«  ·Ì „ Õ›ŸÂ«!", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            oItemSerial.Focus()
        Else
            Dim oDate As String = Today.ToString("MM/dd/yyyy")
            Dim oTime As String = Now.ToString("HH:mm")

            'check if the valid qnty
            If CheckQnty() = False Then
                Exit Sub
            End If

            'make the money as zero value
            For x As Integer = 0 To oDgv.RowCount - 1
                oDgv.Rows(x).Cells(4).Value = 0
                oDgv.Rows(x).Cells(5).Value = 0
                oDgv.Rows(x).Cells(6).Value = 0
            Next
            lblCurrency.Text = "USD"
            ceVisaCurrency.Checked = False
            tValueAmount = 0

            Call OutTotalize()
            Call frmMain.AutoRate(Today)
            'add to out1

            Dim Query As String = "INSERT INTO tblOut1 ([Date], [Time], Customer, LaborOverhaul, Transfers, SubTotal, Discount, SaleTax, NetTotal, [User], pEGP, pEUR, pUSD, pGBP, pRUB, pCHF, pCNY, RealValue, Currency, Visa1, Visa2) " _
                                  & "VALUES ('" & oDate & "', '" & oTime & "', NULL, '" & "0" & "', '" & "0" & "', '" & tbTotal.Text & "', '" & Val(tbCurrency.Text) & "', '" & "0" & "', '" & tbRest.Text & "', '" & GlobalVariables.ID _
                                  & "', 0, 0, 0, 0, 0, 0, 0, 0, 'EGP', 0, 0)"

            Using cmd = New SqlCommand(Query, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                cmd.ExecuteNonQuery()

                cmd.CommandText = "SELECT @@IDENTITY;"
                Using dt As SqlDataReader = cmd.ExecuteReader
                    If dt.Read() Then
                        OSerial.Text = dt(0).ToString
                    End If
                End Using
                myConn.Close()
            End Using


            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Dim theQuery As String = "INSERT INTO tblOut2 (Serial, Kind, Item, Compound, Qnty, Price, Discount, UnitPrice, [User]) VALUES"

            For Each oRow As DataGridViewRow In oDgv.Rows
                theQuery += "('" & OSerial.Text & "', '" & oRow.Cells(1).Value & "', '" & oRow.Cells(7).Value & "', '0', '" & oRow.Cells(0).Value & "', '" & oRow.Cells(6).Value & "', '" & oRow.Cells(5).Value & "', '" & oRow.Cells(4).Value & "', '" & GlobalVariables.ID & "'), "
            Next

            theQuery = theQuery.Substring(0, theQuery.Length - 2)

            Using cmd = New SqlCommand(theQuery, myConn)
                cmd.ExecuteNonQuery()
            End Using
            myConn.Close()

            'clear
            oQnty.Text = "1"
            oItemSerial.Text = Nothing
            oItemName.Text = Nothing
            tbCurrency.Text = ""
            tbEgyptian.Text = ""
            oUnitPrice.Text = ""
            oSubTotal.Text = ""
            oDgv.Rows.Clear()
            oItemSerial.Focus()
            oQnty.SelectAll()
            oCustomer.EditValue = Nothing
            txtVAT.Text = ""
            txtTax1.Text = ""
            txtTax2.Text = ""
            ceDebit.Checked = False
            Call Notification("Invoice Added")

            If KryptonButton5.Text <> "Õ›‹‹‹‹Ÿ" Then
                krInvoicePrint.PerformClick()
            End If

        End If


    End Sub

    Private Sub bt15_Click(sender As System.Object, e As System.EventArgs)
        tbCurrency.Text = "15"
        tbCurrency.Focus()
        tbCurrency.SelectAll()
    End Sub

    Private Sub bt25_Click(sender As System.Object, e As System.EventArgs)
        tbCurrency.Text = "25"
        tbCurrency.Focus()
        tbCurrency.SelectAll()
    End Sub

    Private Sub bt35_Click(sender As System.Object, e As System.EventArgs)
        tbCurrency.Text = "30"
        tbCurrency.Focus()
        tbCurrency.SelectAll()
    End Sub

    Private Sub SimpleButton10_Click(sender As System.Object, e As System.EventArgs) Handles btnCalculator.Click
        Shell("Calc", AppWinStyle.NormalFocus, True)
    End Sub

    Private Sub CheckEdit2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckEdit2.CheckedChanged
        Cursor = Cursors.WaitCursor
        If CheckEdit2.Checked = True Then
            CheckEdit2.Text = "Online"
            Call Sync.Sync()
        Else
            CheckEdit2.Text = "Offline"
        End If
        Cursor = Cursors.Default
    End Sub

    Private Sub SimpleButton11_Click(sender As System.Object, e As System.EventArgs) Handles SimpleButton11.Click
        Cursor = Cursors.WaitCursor
        SimpleButton11.Enabled = False

        Dim Query As String = "SELECT tblItems.Serial, tblItems.Name AS Item, tblItems.Price, tblItems.[Minimum], TIn.QIn AS T_In, COALESCE(TOut.QOut, 0) AS T_Out, TIn.QIn - COALESCE(TOut.QOut, 0) AS Net, (CASE WHEN ( TIn.QIn - COALESCE(TOut.QOut, 0)) < tblItems.[Minimum] THEN 1 ELSE 0 END) as [Needed]" _
                              & " FROM tblItems" _
                              & " INNER Join" _
                              & " (" _
                              & " SELECT tblIn2.Item, SUM(tblIn2.Qnty) AS QIn" _
                              & " FROM tblIn2 INNER JOIN tblIn1" _
                              & " ON tblIn2.Serial = tblIn1.PrKey" _
                              & " GROUP BY tblIn2.Item" _
                              & " ) TIn" _
                              & " ON TIn.Item = tblItems.PrKey" _
                              & " LEFT OUTER JOIN" _
                              & " (" _
                              & " SELECT tblOut2.Item, SUM(tblOut2.Qnty) AS QOut" _
                              & " FROM tblOut2 INNER JOIN tblOut1" _
                              & " ON tblOut2.Serial = tblOut1.Serial" _
                              & " GROUP BY tblOut2.Item" _
                              & " ) TOut" _
                              & " ON TOut.Item = tblItems.PrKey" _
                              & " ORDER BY tblItems.Name"

        Dim Str As String = ""
        Using cmd = New SqlCommand(Query, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Using dr As SqlDataReader = cmd.ExecuteReader
                Dim dt As New DataTable
                dt.Load(dr)

                For x As Integer = 0 To dt.Rows.Count - 1
                    Str += " ('" & dt(x)(0) & "', '" & dt(x)(1) & "', " & dt(x)(2) & ", " & dt(x)(3) & ", " & dt(x)(4) & ", " & dt(x)(5) & ", " & dt(x)(6) & ", " & dt(x)(7) & "),"
                Next

                Str = "USE MasterPro; DELETE FROM tblStock; DBCC SHRINKDATABASE (MasterPro); INSERT INTO tblStock (Code, Item, Price, Minimum, T_In, T_Out, Rest, Needed) VALUES" & Str & "!@#$"
                Str = Str.Replace(",!@#$", ";")

            End Using
            myConn.Close()
        End Using

        Dim SConn As New SqlConnection("workstation id=MasterPro.mssql.somee.com;packet size=4096;user id=walid_SQLLogin_1;pwd=mxy3rvwula;data source=MasterPro.mssql.somee.com;persist security info=False;initial catalog=MasterPro")
        If SConn.State = ConnectionState.Closed Then
            SConn.Open()
        End If
        Using cmd = New SqlCommand(Str, SConn)
            cmd.ExecuteNonQuery()
        End Using

        Using cmd = New SqlCommand("UPDATE tblUpdate SET [Date] = '" & Today.ToString("MM/dd/yyyy") & "', [Time] = '" & Now.ToString("HH:mm") & "'", SConn)
            cmd.ExecuteNonQuery()
        End Using

        SConn.Close()

        Cursor = Cursors.Default
        SimpleButton11.Enabled = True
    End Sub

    Private Sub SimpleButton12_Click(sender As System.Object, e As System.EventArgs)
        tbCurrency.Text = tbTotal.Text
        tbCurrency.Focus()
        tbCurrency.SelectAll()
        KryptonButton5.PerformClick()
    End Sub

    Private Sub oDgv_LostFocus(sender As Object, e As System.EventArgs) Handles oDgv.LostFocus
        lblIndicateTotal.Visible = False
        oQnty.Text = "1"
    End Sub

    Private Sub oDgv_SelectionChanged(sender As Object, e As System.EventArgs) Handles oDgv.SelectionChanged
        Dim Total As Decimal = 0.0
        For x As Integer = 0 To oDgv.RowCount - 1
            If oDgv.Rows(x).Selected = True Then
                Total += oDgv.Rows(x).Cells(6).Value
            End If
        Next
        If Total <> 0 And oDgv.Focused = True Then
            lblIndicateTotal.Visible = True
            lblIndicateTotal.Text = Total
        Else
            lblIndicateTotal.Visible = False
        End If
    End Sub

    Private Sub btnCost_Click(sender As Object, e As EventArgs) Handles btnCost.Click
        If GlobalVariables.authority <> "Admin" AndAlso ExClass.validAccess = False Then
            Exit Sub
        End If

        If oItemSerial.Text <> "" Then
            frmCheckItemCost2.TextEdit1.Text = oItemSerial.Text
            frmCheckItemCost2.CheckItem(frmCheckItemCost2.TextEdit1.Text)
        End If

        frmCheckItemCost2.TextEdit1.Focus()
        frmCheckItemCost2.TextEdit1.SelectAll()
        frmCheckItemCost2.ShowDialog()
        oItemSerial.Focus()
        oItemSerial.SelectAll()
    End Sub

    Private Sub PictureEdit2_Click(sender As Object, e As EventArgs) Handles PictureEdit2.Click
        If KryptonPanel6.Visible = True Then
            KryptonPanel6.Visible = False
        Else
            KryptonPanel6.Visible = True
        End If
        My.Settings.VisiblePnl = KryptonPanel6.Visible
        My.Settings.Save()
    End Sub

    Private Sub oItemName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles oItemName.SelectedIndexChanged

    End Sub

    Private Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
        If GlobalVariables.authority <> "Admin" AndAlso ExClass.validAccess = False Then
            Exit Sub
        End If

        For x As Integer = 0 To oDgv.RowCount - 1
            If oDgv.Rows(x).Cells(0).Value > 0 Then
                oDgv.Rows(x).Cells(0).Value = "-" & oDgv.Rows(x).Cells(0).Value
                oDgv.Rows(x).Cells(6).Value = (oDgv.Rows(0).Cells(0).Value * oDgv.Rows(x).Cells(4).Value) - oDgv.Rows(x).Cells(5).Value
            End If

        Next
        OutTotalize()
        oItemSerial.Focus()
        oItemSerial.SelectAll()
    End Sub

    Private Sub oDgv_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles oDgv.CellValueChanged
        Try
            If oDgv.CurrentCell.ColumnIndex = 5 Then
                If oDgv.CurrentRow.Cells(6).Value < oDgv.CurrentRow.Cells(5).Value Then
                    MsgBox("!«·„»·€ «·–Ì  „ ≈œŒ«·Â €Ì— ’ÕÌÕ")
                Else
                    oDgv.CurrentRow.Cells(6).Value = (oDgv.CurrentRow.Cells(0).Value * oDgv.CurrentRow.Cells(4).Value) - oDgv.CurrentRow.Cells(5).Value
                    OutTotalize()
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub lblCurrency_TextChanged(sender As Object, e As EventArgs) Handles lblCurrency.TextChanged
        If lblCurrency.Text = "EGP" Then
            tbCurrency.Text = ""
            tbCurrency.Enabled = False
            ceVisaCurrency.Checked = False
            ceVisaCurrency.Enabled = False
        ElseIf lblCurrency.Text = "USD" Then
            tbCurrency.Enabled = True
            ceVisaCurrency.Enabled = True
            tbCurrency.Enabled = True
        Else
            ceVisaCurrency.Checked = False
            ceVisaCurrency.Enabled = False
            tbCurrency.Enabled = True
        End If
        Call OutTotalize()
    End Sub

    Private Sub getRest()
        Dim rest, dueAmount, paidEGP, paidForeign As Double
        rest = 0
        dueAmount = Val(tbTotal.Text)
        paidEGP = Val(tbEgyptian.Text) / cEGP
        paidForeign = Val(tbCurrency.Text)

        Select Case lblCurrency.Text
            Case "EUR"
                paidEGP = paidEGP * cEUR
            Case "GBP"
                paidEGP = paidEGP * cGBP
            Case "RUB"
                paidEGP = paidEGP * cRUB
            Case "CHF"
                paidEGP = paidEGP * cCHF
            Case "CNY"
                paidEGP = paidEGP * cCNY
            Case "EGP"
                paidEGP = paidEGP * cEGP
        End Select

        rest = dueAmount - (paidEGP + paidForeign)
        
        rest = Math.Round(rest, 0, MidpointRounding.AwayFromZero)
        tbRest.Text = -rest

    End Sub

    Private Sub tbEgyptian_KeyDown(sender As Object, e As KeyEventArgs) Handles tbEgyptian.KeyDown
        If e.Control = True And e.KeyCode = Keys.Enter Then
            oItemSerial.Focus()
            oItemSerial.SelectAll()
        ElseIf e.KeyCode = Keys.Enter Then
            If tbCurrency.Enabled = True Then
                tbCurrency.Focus()
                tbCurrency.SelectAll()
            Else
                KryptonButton5.PerformClick()
            End If
        ElseIf e.KeyCode = Keys.V Then
            If ceVisaEGP.Checked = True Then
                ceVisaEGP.Checked = False
            Else
                ceVisaEGP.Checked = True
            End If
            e.Handled = True
        End If
    End Sub
    Private Sub tbEgyptian_TextChanged(sender As Object, e As EventArgs) Handles tbEgyptian.TextChanged
        getRest()
    End Sub

    Private Sub tbCurrency_TextChanged(sender As Object, e As EventArgs) Handles tbCurrency.TextChanged
        getRest()
    End Sub

    Private Sub getCashierReport()

        Dim Que As String = "SELECT tblOut1.[Date], tblOut1.[Time], tblOut1.Serial," _
                            & " (CASE WHEN tblOut1.Currency = 'USD' AND tblOut1.Visa2 = 0 THEN tblOut1.pUSD - tblOut1.NetTotal ELSE 0 END) AS USD," _
                            & " (CASE WHEN tblOut1.Visa1 = 0 THEN (CASE WHEN tblOut1.Currency = 'EGP' THEN tblOut1.RealValue ELSE tblOut1.pEGP END) ELSE 0 END) AS EGP," _
                            & " (CASE WHEN tblOut1.Currency = 'EUR' THEN tblOut1.pEUR - tblOut1.NetTotal ELSE 0 END) AS EUR," _
                            & " (CASE WHEN tblOut1.Currency = 'GBP' THEN tblOut1.pGBP - tblOut1.NetTotal ELSE 0 END) AS GBP," _
                            & " (CASE WHEN tblOut1.Currency = 'RUB' THEN tblOut1.pRUB - tblOut1.NetTotal ELSE 0 END) AS RUB," _
                            & " (CASE WHEN tblOut1.Currency = 'CHF' THEN tblOut1.pCHF - tblOut1.NetTotal ELSE 0 END) AS CHF," _
                            & " (CASE WHEN tblOut1.Currency = 'CNY' THEN tblOut1.pCNY - tblOut1.NetTotal ELSE 0 END) AS CNY," _
                            & " (CASE WHEN tblOut1.Visa1 = 1 THEN (CASE WHEN tblOut1.Currency = 'EGP' THEN tblOut1.RealValue ELSE tblOut1.pEGP END) ELSE 0 END) AS VisaEGP," _
                            & " (CASE WHEN tblOut1.Currency = 'USD' AND tblOut1.Visa2 = 1 THEN tblOut1.pUSD - tblOut1.NetTotal ELSE 0 END) AS VisaUSD," _
                            & " tblLogin.UserName AS [User]" _
                            & " FROM tblOut1 INNER JOIN tblLogin ON tblOut1.[User] = tblLogin.Sr" _
                            & " ORDER BY tblOut1.[Date], tblOut1.[Time];"


        If myConn.State = ConnectionState.Closed Then
            myConn.Open()
        End If

        Dim ds As New ReportsDS
        Dim da As New SqlDataAdapter(Que, myConn)
        da.Fill(ds.Tables("XtraCashier"))

        Dim Report As New XtraCashier


        Report.DataSource = ds
        Report.DataAdapter = da
        Report.DataMember = "XtraCashier"
        
        Dim tool As ReportPrintTool = New ReportPrintTool(Report)

        Report.CreateDocument()

        Report.ShowPreview()
        myConn.Close()

    End Sub

    Private Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click
        frmCashierReport.ShowDialog()
        'getCashierReport()
    End Sub

    Private Sub oCustomer_EditValueChanged(sender As Object, e As EventArgs) Handles oCustomer.EditValueChanged
        If oCustomer.EditValue = Nothing Then
            lblDiscount.Visible = False
            lblDiscount.Text = "0"
            If GlobalVariables.authority = "Admin" Then
                oDgv.Columns(5).ReadOnly = False
            End If
            oDgv.Columns(4).ReadOnly = True
        Else
            lblDiscount.Visible = True
            Dim disc As String
            Dim Query As String = "SELECT Discount FROM tblCustomers WHERE ID = " & oCustomer.EditValue
            Using cmd = New SqlCommand(Query, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Try
                    disc = cmd.ExecuteScalar
                Catch ex As Exception
                    disc = 0
                End Try
                myConn.Close()
            End Using
            lblDiscount.Text = disc & "%"

            oDgv.Columns(5).ReadOnly = True
            If GlobalVariables.authority = "Admin" Then
                oDgv.Columns(4).ReadOnly = False
            End If
        End If
        OutTotalize()
    End Sub

    Private Sub oCustomer_KeyDown(sender As Object, e As KeyEventArgs) Handles oCustomer.KeyDown
        If e.KeyCode = Keys.Escape Then
            oCustomer.EditValue = Nothing
        End If
    End Sub

    Private Sub oCustomer_TextChanged1(sender As Object, e As EventArgs) Handles oCustomer.TextChanged
        If oCustomer.Text = "" Then
            oCustomer.EditValue = Nothing
        End If
    End Sub

    Private Sub tbEgyptian_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbEgyptian.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        ElseIf e.KeyChar = "." And tbEgyptian.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtVAT_KeyDown(sender As Object, e As KeyEventArgs) Handles txtVAT.KeyDown
        If e.KeyCode = 189 Then
            If txtVAT.Text Like "*" & "-" & "*" Then
                txtVAT.Text = txtVAT.Text.Replace("-", "")
            Else
                txtVAT.Text = "-" & txtVAT.Text
            End If
            txtVAT.Select(txtVAT.Text.Length, 1)
        End If

    End Sub

    Private Sub txtVAT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtVAT.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        ElseIf e.KeyChar = "." And txtVAT.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtTax1_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTax1.KeyDown
        If e.KeyCode = 189 Then
            If txtTax1.Text Like "*" & "-" & "*" Then
                txtTax1.Text = txtTax1.Text.Replace("-", "")
            Else
                txtTax1.Text = "-" & txtTax1.Text
            End If
            txtTax1.Select(txtTax1.Text.Length, 1)
        End If
    End Sub

    Private Sub txtTax1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTax1.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        ElseIf e.KeyChar = "." And txtTax1.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtTax2_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTax2.KeyDown
        If e.KeyCode = 189 Then
            If txtTax2.Text Like "*" & "-" & "*" Then
                txtTax2.Text = txtTax2.Text.Replace("-", "")
            Else
                txtTax2.Text = "-" & txtTax2.Text
            End If
            txtTax2.Select(txtTax2.Text.Length, 1)
        End If
    End Sub

    Private Sub txtTax2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTax2.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        ElseIf e.KeyChar = "." And txtTax2.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtVAT_EditValueChanged(sender As Object, e As EventArgs) Handles txtVAT.EditValueChanged
        OutTotalize()
    End Sub

    Private Sub txtTax1_TextChanged(sender As Object, e As EventArgs) Handles txtTax1.TextChanged
        OutTotalize()
    End Sub

    Private Sub txtTax2_TextChanged(sender As Object, e As EventArgs) Handles txtTax2.TextChanged
        OutTotalize()
    End Sub

    Private Sub ceDebit_CheckedChanged(sender As Object, e As EventArgs) Handles ceDebit.CheckedChanged
        If ceDebit.Checked = True Then
            tbEgyptian.Text = "0"
            ceVisaEGP.Checked = False
            tbCurrency.Text = "0"
            ceVisaCurrency.Checked = False
        End If
    End Sub
    Private Sub LabelControl1_DoubleClick(sender As Object, e As EventArgs) Handles LabelControl1.DoubleClick
        frmCustomersAccount.ShowDialog()
    End Sub

    Private Sub oDgv_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles oDgv.CellEndEdit
        If oDgv.CurrentCell.ColumnIndex = 4 OrElse oDgv.CurrentCell.ColumnIndex = 5 Then
            oDgv.CurrentCell.Value = Val(oDgv.CurrentCell.Value)
        End If
        OutTotalize()
    End Sub

    Private Sub oDgv_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles oDgv.CellLeave
        OutTotalize()
    End Sub

    Private Sub oDgv_CellValidated(sender As Object, e As DataGridViewCellEventArgs) Handles oDgv.CellValidated
        If oDgv.CurrentCell.ColumnIndex = 4 OrElse oDgv.CurrentCell.ColumnIndex = 5 Then
            oDgv.CurrentCell.Value = Val(oDgv.CurrentCell.Value)
        End If
        OutTotalize()
    End Sub

    Private Sub oDgv_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles oDgv.CellValidating
        If oDgv.CurrentCell.ColumnIndex = 4 OrElse oDgv.CurrentCell.ColumnIndex = 5 Then
            If Not IsNumeric(e.FormattedValue) Then
                e.Cancel = True
            End If
        End If
    End Sub
End Class

