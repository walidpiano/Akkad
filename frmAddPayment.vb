Imports System.Data.SqlClient
Public Class frmAddPayment
    Dim myConn As New SqlConnection(GV.myConn)
    Public Shared ID As Integer = 0

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub clear()
        deDate.EditValue = Today
        oCustomer.EditValue = Nothing
        txtAmount.Text = ""
        ID = 0
    End Sub
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If deDate.EditValue = Nothing Then
            MsgBox("!برجاء إدخال التاريخ")
            deDate.Focus()
            deDate.SelectAll()
        ElseIf oCustomer.EditValue = Nothing Then
            MsgBox("!برجاء إدخال اسم العميل")
            oCustomer.Focus()
        ElseIf Val(txtAmount.Text) = 0 Then
            MsgBox("!يجب إدخال المبلغ")
        Else
            Dim Query As String = ""
            If ID = 0 Then
                Query = "INSERT INTO tblCustomerPayment (Customer, cDate, cTime, Amount, Cashier) VALUES (@Customer, @cDate, @cTime, @Amount, @Cashier);"
            Else
                Query = "UPDATE tblCustomerPayment SET Customer = @Customer, cDate = @cDate, cTime = @cTime, Amount = @Amount, Cashier = @Cashier WHERE ID = " & ID & ";"
            End If


            Using cmd = New SqlCommand(Query, myConn)
                cmd.Parameters.AddWithValue("@Customer", oCustomer.EditValue)
                cmd.Parameters.AddWithValue("@cDate", deDate.EditValue)
                cmd.Parameters.AddWithValue("@cTime", Now.ToString("HH:mm"))
                cmd.Parameters.AddWithValue("@Amount", Val(txtAmount.Text))
                cmd.Parameters.AddWithValue("@Cashier", GlobalVariables.ID)
                If myConn.State = ConnectionState.Closed Then
                    myConn.Open()
                    cmd.ExecuteNonQuery()
                    myConn.Close()
                End If
            End Using
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If

    End Sub

    Private Sub frmAddCheck_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        clear()
    End Sub

    Private Sub frmAddCheck_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub frmAddCheck_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        deDate.Focus()
    End Sub
End Class