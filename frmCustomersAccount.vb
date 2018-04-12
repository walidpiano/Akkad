Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors

Partial Public Class frmCustomersAccount
    Dim myConn As New SqlConnection(GV.myConn)
    Public Sub New()
        InitializeComponent()

    End Sub

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


            frmAddPayment.oCustomer.Properties.DataSource = ds.Tables(0)
            frmAddPayment.oCustomer.Properties.DisplayMember = "Customer"
            frmAddPayment.oCustomer.Properties.ValueMember = "ID"
            frmAddPayment.oCustomer.EditValue = Nothing
            myConn.Close()

        End Using
    End Sub
    Private Sub windowsUIButtonPanel_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.Docking2010.ButtonEventArgs) Handles windowsUIButtonPanel.ButtonClick
        If e.Button.Properties.Caption = "Print" Then
            GridControl1.ShowRibbonPrintPreview()
        ElseIf e.Button.Properties.Caption = "Refresh" Then
            loadData()
        ElseIf e.Button.Properties.Caption = "Delete" Then
            If GridView1.GetFocusedRowCellValue("Serial") = "Payment" Then
                Dim ID As String = GridView1.GetFocusedRowCellValue("ID")
                fillCustomers()
                Dim Query As String = "DELETE FROM tblCustomerPayment WHERE ID = " & ID & ";"
                Using cmd = New SqlCommand(Query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    cmd.ExecuteNonQuery()
                    myConn.Close()
                End Using
                loadData()
            End If
        ElseIf e.Button.Properties.Caption = "New" Then
            fillCustomers()
            frmAddPayment.deDate.EditValue = Today
            frmAddPayment.ShowDialog()
            If frmAddPayment.DialogResult = Windows.Forms.DialogResult.OK Then
                loadData()
            End If
        ElseIf e.Button.Properties.Caption = "Edit" Then
            If GridView1.GetFocusedRowCellValue("Serial") = "Payment" Then
                Dim ID As String = GridView1.GetFocusedRowCellValue("ID")
                Dim Query As String = "SELECT * FROM tblCustomerPayment WHERE ID = " & ID & ";"
                fillCustomers()
                Using cmd = New SqlCommand(Query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            With frmAddPayment
                                frmAddPayment.ID = dr(0)
                                .oCustomer.EditValue = dr(1)
                                Dim dtt As Date
                                Date.TryParse(dr(2), dtt)
                                .deDate.EditValue = dtt
                                .txtAmount.Text = dr(4)
                            End With
                        End If
                    End Using
                    myConn.Close()
                End Using
                frmAddPayment.ID = ID
                frmAddPayment.ShowDialog()
                If frmAddPayment.DialogResult = Windows.Forms.DialogResult.OK Then
                    loadData()
                End If
            End If
        End If
    End Sub

    Private Sub frmChecks_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadData()
        GridView1.MoveLast()
    End Sub

    Private Sub loadData()
        Dim Query As String = "(SELECT 0 AS ID, tblCustomers.Customer, tblOut1.[Date], tblOut1.[Time], CONVERT(NVARCHAR(10), tblOut1.Serial) AS Serial, tblOut1.RealValue AS Amount, tblLogin.UserName AS Cashier" _
                              & " FROM tblOut1" _
                              & " INNER JOIN tblCustomers ON tblCustomers.ID = tblOut1.Customer" _
                              & " INNER JOIN tblLogin ON tblLogin.Sr = tblOut1.[User]" _
                              & " WHERE tblOut1.Debit = 1)" _
                              & " UNION ALL" _
                              & " (" _
                              & " SELECT tblCustomerPayment.ID, tblCustomers.Customer, tblCustomerPayment.cDate AS [Date]," _
                              & " tblCustomerPayment.cTime AS [Time], 'Payment' AS Serial, -tblCustomerPayment.Amount, tblLogin.UserName AS Cashier" _
                              & " FROM tblCustomerPayment" _
                              & " INNER JOIN tblCustomers ON tblCustomers.ID = tblCustomerPayment.Customer" _
                              & " INNER JOIN tblLogin ON tblLogin.Sr = tblCustomerPayment.Cashier" _
                              & " )" _
                              & " ORDER BY [Date], [Time]"

        Dim dt As New DataTable

        Using cmd = New SqlCommand(Query, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Using dr As SqlDataReader = cmd.ExecuteReader
                dt.Load(dr)
            End Using
            myConn.Close()
        End Using
        GridControl1.DataSource = dt
        GridView1.BestFitColumns()
    End Sub

End Class
