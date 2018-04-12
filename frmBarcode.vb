Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Public Class frmBarcode
    Public Shared myConn As New SqlConnection(GV.myConn)
    Private Sub frmBarcode_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

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
    End Sub

    Private Sub iSerial_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles iSerial.SelectedIndexChanged
        Dim USDExchange As Double
        frmCashier.getCurrency()
        USDExchange = 1

        Dim Query As String = "SELECT tblIn1.Vendor, tblIn1.Serial, tblIn1.[Date], tblIn1.[Time], tblItems.Serial AS Code, (CASE WHEN tblItems.EnglishName = '' THEN tblItems.Name ELSE tblItems.EnglishName END) AS Item, tblIn2.Qnty, tblIn2.UnitPrice, tblIn2.[Value], tblIn1.Paid, tblItems.Price" _
                              & " FROM tblIn1 INNER JOIN tblIn2 ON tblIn1.PrKey = tblIn2.Serial" _
                              & " INNER JOIN tblItems ON tblItems.PrKey = tblIn2.Item" _
                              & " INNER JOIN tblVendors ON tblVendors.Sr = tblIn1.Vendor" _
                              & " WHERE tblVendors.Name = N'" & iVendor.Text & "'" _
                              & " AND tblIn1.Serial = N'" & iSerial.Text & "'"


        Using cmd = New SqlCommand(Query, myConn)

            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Using dr As SqlDataReader = cmd.ExecuteReader
                Try
                    Dim dt As New DataTable
                    dt.Load(dr)
                    Dim theUSDPrice As Double = 0


                    iDgv.Rows.Clear()
                    For x As Integer = 0 To dt.Rows.Count - 1

                        theUSDPrice = dt.Rows(x)(10)
                        theUSDPrice = theUSDPrice * USDExchange
                        theUSDPrice = Math.Round(theUSDPrice, 0, MidpointRounding.AwayFromZero)
                        If theUSDPrice < 1 Then
                            theUSDPrice = 1
                        End If
                        iDgv.Rows.Add(dt.Rows(x)(4), dt.Rows(x)(5), dt.Rows(x)(6), theUSDPrice)
                    Next

                Catch ex As Exception

                End Try

            End Using

            myConn.Close()
        End Using


    End Sub

    Private Sub iVendor_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles iVendor.SelectedIndexChanged
        If Not iVendor.Text = "" Then
            Dim Query As String = "SELECT tblIn1.Serial, tblVendors.Name AS Vendor" _
                                  & " FROM tblIn1 INNER JOIN tblVendors ON tblIn1.Vendor = tblVendors.Sr" _
                                  & " WHERE tblVendors.Name = N'" & iVendor.Text & "'"

            Using cmd = New SqlCommand(Query, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                iSerial.DataSource = ds.Tables(0)
                iSerial.DisplayMember = "Serial"
                iSerial.Text = Nothing

                myConn.Close()
            End Using
        End If
    End Sub

    Private Sub iItem_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
        'If iItem.Text = "" Then
        '    ilbNumber.Visible = False
        '    iNumber.Visible = False
        'Else

        '    ilbNumber.Visible = True
        '    iNumber.Visible = True

        '    If myConn.State = ConnectionState.Closed Then
        '        myConn.Open()
        '    End If
        '    Dim Query As String = "SELECT tblItems.PrKey, tblItems.Name AS Item, tblIn.Qnty FROM tblItems" _
        '                         & " INNER JOIN tblIn" _
        '                         & " ON tblItems.PrKey = tblIn.Item" _
        '                         & " WHERE tblIn.Serial = '" & iSerial.SelectedValue & "' AND tblItems.Name = '" & iItem.Text & "'"
        '    Using cmd = New SqlCommand(Query, myConn)
        '        Using dr As SqlDataReader = cmd.ExecuteReader
        '            If dr.Read() Then
        '                iNumber.Value = dr(2)
        '            End If
        '        End Using
        '    End Using
        '    myConn.Close()
        'End If

    End Sub

    Private Sub iItem_TextChanged(sender As Object, e As System.EventArgs)
        'If iItem.Text = "" Then
        '    ilbNumber.Visible = False
        '    iNumber.Visible = False
        'Else

        '    ilbNumber.Visible = True
        '    iNumber.Visible = True

        '    If myConn.State = ConnectionState.Closed Then
        '        myConn.Open()
        '    End If
        '    Dim Query As String = "SELECT tblItems.PrKey, tblItems.Name AS Item, tblIn.Qnty FROM tblItems" _
        '                         & " INNER JOIN tblIn" _
        '                         & " ON tblItems.PrKey = tblIn.Item" _
        '                         & " WHERE tblIn.Serial = '" & iSerial.SelectedValue & "' AND tblItems.Name = '" & iItem.Text & "'"
        '    Using cmd = New SqlCommand(Query, myConn)
        '        Using dr As SqlDataReader = cmd.ExecuteReader
        '            If dr.Read() Then
        '                iNumber.Value = dr(2)
        '            End If
        '        End Using
        '    End Using
        '    myConn.Close()
        'End If
    End Sub

    Private Sub btnPrintA4_Click(sender As System.Object, e As System.EventArgs) Handles btnPrintA41.Click
        If iDgv.RowCount = 0 Then
            MessageBox.Show("áÇ ÊæÌÏ ÈíÇäÇÊ áíÊã ØÈÇÚÊåÇ!", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            'check the numbers
            Dim num As String
            For x As Integer = 0 To iDgv.RowCount - 1
                num = iDgv.Rows(x).Cells(2).Value
                If Not IsNumeric(num) Then
                    MsgBox("áÞÏ ÃÏÎáÊ ÑÞã ÛíÑ ÕÍíÍ!")
                    iDgv.ClearSelection()
                    iDgv.Rows(x).Cells(2).Selected = True
                    Exit Sub
                End If
            Next

            Dim theQuery As String = ""

            'to set the start
            For s As Integer = 0 To (NumericUpDown1.Value * 6) - 1
                theQuery += "SELECT NULL AS PrKey, N'' AS Item, NULL AS Price UNION ALL "
            Next

            For s As Integer = 0 To (NumericUpDown2.Value) - 1
                theQuery += "SELECT NULL AS PrKey, N'' AS Item, NULL AS Price UNION ALL "
            Next


            Dim sr, itm As String
            Dim co As Integer
            Dim price As Decimal
            For x As Integer = 0 To iDgv.RowCount - 1
                sr = iDgv.Rows(x).Cells(0).Value
                itm = iDgv.Rows(x).Cells(1).Value
                co = iDgv.Rows(x).Cells(2).Value
                price = iDgv.Rows(x).Cells(3).Value

                For y As Integer = 0 To co - 1
                    theQuery += "SELECT '" & sr & "' AS PrKey, N'" & itm & "' AS Item, " & price & " AS Price UNION ALL "
                Next

            Next
            If Not theQuery = "" Then
                theQuery = theQuery.Substring(0, theQuery.LastIndexOf(" UNION ALL "))
            End If

            Dim ds As New ReportsDS
            Dim da As New SqlDataAdapter(theQuery, myConn)
            da.Fill(ds.Tables("tblBarcode"))

            Dim rep As New XtraPoster3
            rep.DataSource = ds
            rep.DataAdapter = da
            rep.DataMember = "tblBarcode"
            Dim tool As ReportPrintTool = New ReportPrintTool(rep)
            rep.CreateDocument()
            rep.ShowPreview()

        End If

    End Sub

    Private Sub iVendor_TextChanged(sender As Object, e As System.EventArgs) Handles iVendor.TextChanged
        If Not iVendor.Text = "" Then
            Dim Query As String = "SELECT tblIn1.Serial, tblVendors.Name AS Vendor" _
                                  & " FROM tblIn1 INNER JOIN tblVendors ON tblIn1.Vendor = tblVendors.Sr" _
                                  & " WHERE tblVendors.Name = N'" & iVendor.Text & "'"

            Using cmd = New SqlCommand(Query, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                Dim adt As New SqlDataAdapter
                Dim ds As New DataSet()
                adt.SelectCommand = cmd
                adt.Fill(ds)
                adt.Dispose()

                iSerial.DataSource = ds.Tables(0)
                iSerial.DisplayMember = "Serial"
                iSerial.Text = Nothing

                myConn.Close()
            End Using
        End If
    End Sub

    Private Sub TextBox_keyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If Char.IsDigit(CChar(CStr(e.KeyChar))) = False Then e.Handled = True
    End Sub

    Private Sub iDgv_EditingControlShowing(sender As Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles iDgv.EditingControlShowing
        If iDgv.CurrentCell.ColumnIndex = 2 Then
            AddHandler CType(e.Control, TextBox).KeyPress, AddressOf TextBox_keyPress
        End If
    End Sub

    Private Sub btnPrintLabels_Click(sender As Object, e As EventArgs) Handles btnPrintLabels.Click
        If iDgv.RowCount = 0 Then
            MessageBox.Show("áÇ ÊæÌÏ ÈíÇäÇÊ áíÊã ØÈÇÚÊåÇ!", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            'check the numbers
            Dim num As String
            For x As Integer = 0 To iDgv.RowCount - 1
                num = iDgv.Rows(x).Cells(2).Value
                If Not IsNumeric(num) Then
                    MsgBox("áÞÏ ÃÏÎáÊ ÑÞã ÛíÑ ÕÍíÍ!")
                    iDgv.ClearSelection()
                    iDgv.Rows(x).Cells(2).Selected = True
                    Exit Sub
                End If
            Next

            Dim theQuery As String = ""

            Dim sr, itm As String
            Dim co As Integer
            For x As Integer = 0 To iDgv.RowCount - 1
                sr = iDgv.Rows(x).Cells(0).Value
                itm = iDgv.Rows(x).Cells(1).Value
                co = iDgv.Rows(x).Cells(2).Value

                For y As Integer = 0 To co - 1
                    theQuery += "SELECT '" & sr & "' AS PrKey, N'" & itm & "' AS Item UNION ALL "
                Next

            Next
            If Not theQuery = "" Then
                theQuery = theQuery.Substring(0, theQuery.LastIndexOf(" UNION ALL "))
            End If

            Dim ds As New ReportsDS
            Dim da As New SqlDataAdapter(theQuery, myConn)
            da.Fill(ds.Tables("tblBarcode"))

            Dim rep As New XtraBarcodes
            rep.DataSource = ds
            rep.DataAdapter = da
            rep.DataMember = "tblBarcode"
            Dim tool As ReportPrintTool = New ReportPrintTool(rep)
            rep.CreateDocument()
            rep.ShowPreview()

        End If

    End Sub

    Private Sub btnPrintBadge_Click(sender As Object, e As EventArgs) Handles btnPrintBadge.Click
        If iDgv.RowCount = 0 Then
            MessageBox.Show("áÇ ÊæÌÏ ÈíÇäÇÊ áíÊã ØÈÇÚÊåÇ!", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            'check the numbers
            Dim num As String
            For x As Integer = 0 To iDgv.RowCount - 1
                num = iDgv.Rows(x).Cells(2).Value
                If Not IsNumeric(num) Then
                    MsgBox("áÞÏ ÃÏÎáÊ ÑÞã ÛíÑ ÕÍíÍ!")
                    iDgv.ClearSelection()
                    iDgv.Rows(x).Cells(2).Selected = True
                    Exit Sub
                End If
            Next

            Dim theQuery As String = ""

            'to set the start
            For s As Integer = 0 To (NumericUpDown1.Value * 3) - 1
                theQuery += "SELECT N'' AS Item, NULL AS Price UNION ALL "
            Next

            For s As Integer = 0 To (NumericUpDown2.Value) - 1
                theQuery += "SELECT N'' AS Item, NULL AS Price UNION ALL "
            Next


            Dim itm, prc As String
            Dim co As Integer
            For x As Integer = 0 To iDgv.RowCount - 1
                'sr = iDgv.Rows(x).Cells(0).Value
                itm = iDgv.Rows(x).Cells(1).Value
                co = iDgv.Rows(x).Cells(2).Value
                prc = iDgv.Rows(x).Cells(3).Value
                For y As Integer = 0 To co - 1
                    theQuery += "SELECT N'" & itm & "' AS Item, '" & prc & "' AS Price UNION ALL "
                Next

            Next
            If Not theQuery = "" Then
                theQuery = theQuery.Substring(0, theQuery.LastIndexOf(" UNION ALL "))
            End If

            Dim ds As New ReportsDS
            Dim da As New SqlDataAdapter(theQuery, myConn)
            da.Fill(ds.Tables("tblBarcode"))

            Dim rep As New XtraLabels
            rep.DataSource = ds
            rep.DataAdapter = da
            rep.DataMember = "tblBarcode"
            rep.XrLabel3.Text = GV.MarketName
            Dim tool As ReportPrintTool = New ReportPrintTool(rep)
            rep.CreateDocument()
            rep.ShowPreview()

        End If

    End Sub

    Private Sub btnPrintA42_Click(sender As Object, e As EventArgs) Handles btnPrintA42.Click
        If iDgv.RowCount = 0 Then
            MessageBox.Show("áÇ ÊæÌÏ ÈíÇäÇÊ áíÊã ØÈÇÚÊåÇ!", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            'check the numbers
            Dim num As String
            For x As Integer = 0 To iDgv.RowCount - 1
                num = iDgv.Rows(x).Cells(2).Value
                If Not IsNumeric(num) Then
                    MsgBox("áÞÏ ÃÏÎáÊ ÑÞã ÛíÑ ÕÍíÍ!")
                    iDgv.ClearSelection()
                    iDgv.Rows(x).Cells(2).Selected = True
                    Exit Sub
                End If
            Next

            Dim theQuery As String = ""

            'to set the start
            For s As Integer = 0 To (NumericUpDown1.Value * 4) - 1
                theQuery += "SELECT NULL AS PrKey, N'' AS Item UNION ALL "
            Next

            For s As Integer = 0 To (NumericUpDown2.Value) - 1
                theQuery += "SELECT NULL AS PrKey, N'' AS Item UNION ALL "
            Next


            Dim sr, itm As String
            Dim co As Integer
            For x As Integer = 0 To iDgv.RowCount - 1
                sr = iDgv.Rows(x).Cells(0).Value
                itm = iDgv.Rows(x).Cells(1).Value
                co = iDgv.Rows(x).Cells(2).Value

                For y As Integer = 0 To co - 1
                    theQuery += "SELECT '" & sr & "' AS PrKey, N'" & itm & "' AS Item UNION ALL "
                Next

            Next
            If Not theQuery = "" Then
                theQuery = theQuery.Substring(0, theQuery.LastIndexOf(" UNION ALL "))
            End If

            Dim ds As New ReportsDS
            Dim da As New SqlDataAdapter(theQuery, myConn)
            da.Fill(ds.Tables("tblBarcode"))

            Dim rep As New XtraPoster2
            rep.DataSource = ds
            rep.DataAdapter = da
            rep.DataMember = "tblBarcode"
            Dim tool As ReportPrintTool = New ReportPrintTool(rep)
            rep.CreateDocument()
            rep.ShowPreview()

        End If

    End Sub

    Private Sub btnClearQnty_Click(sender As Object, e As EventArgs) Handles btnClearQnty.Click
        For x As Integer = 0 To iDgv.RowCount - 1
            iDgv.Rows(x).Cells(2).Value = "0"
        Next
    End Sub
End Class
