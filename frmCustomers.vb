Imports System.Data.SqlClient
Public Class frmCustomers
    Public Shared myConn As New SqlConnection(GV.myConn)

    Private Sub loadCustomer()
        Dim Query As String
        Dim dt As New DataTable
        dt.Columns.Add("ID", GetType(Integer))
        dt.Columns.Add("Customer", GetType(String))
        dt.Columns.Add("Discount", GetType(Single))
        Query = "SELECT * FROM tblCustomers ORDER BY Customer"
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
    End Sub

    Private Sub frmCustomers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadCustomer()
        GridView1.MoveLast()
    End Sub

    Private Sub windowsUIButtonPanel_ButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.ButtonEventArgs) Handles windowsUIButtonPanel.ButtonClick
        If e.Button.Properties.Caption = "New" Then
            GridView1.AddNewRow()
        ElseIf e.Button.Properties.Caption = "Save" Then
            Dim Query As String = ""



            For x As Integer = 0 To GridView1.RowCount - 1
                GridView1.PostEditor()

                GridView1.UpdateCurrentRow()

                
                If Not IsDBNull(GridView1.GetRowCellValue(x, "Customer")) Then
                    If IsDBNull(GridView1.GetRowCellValue(x, "Discount")) Then
                        GridView1.SetRowCellValue(x, "Discount", 0)
                    End If

                    If IsDBNull(GridView1.GetRowCellValue(x, "ID")) Then
                        Query &= " INSERT INTO tblCustomers (Customer, Discount) VALUES (N'" & GridView1.GetRowCellValue(x, "Customer") & "', " & Val(GridView1.GetRowCellValue(x, "Discount")) & ");"
                    Else
                        Query &= "UPDATE tblCustomers SET Customer = N'" & GridView1.GetRowCellValue(x, "Customer") & "', Discount = " & Val(GridView1.GetRowCellValue(x, "Discount")) _
                            & " WHERE ID = " & GridView1.GetRowCellValue(x, "ID") & ";"
                    End If
                End If
            Next
            If Query = "" Then
                Exit Sub
            End If
            Using cmd = New SqlCommand(Query, myConn)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                End If
                cmd.ExecuteNonQuery()
                myConn.Close()
            End Using
            loadCustomer()
            MsgBox("Saved!")

        ElseIf e.Button.Properties.Caption = "Delete" Then
            Dim DiaR As DialogResult = MessageBox.Show("هل تريد بالتأكيد حذف العمل؟", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
            If DiaR = Windows.Forms.DialogResult.Yes Then
                Dim ID As String = GridView1.GetFocusedRowCellValue("ID")
                Dim Query As String = "DELETE FROM tblCustomers WHERE ID = " & ID & ";"
                Using cmd = New SqlCommand(Query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    cmd.ExecuteNonQuery()
                    myConn.Close()
                    loadCustomer()
                End Using
            End If

        End If
    End Sub


End Class