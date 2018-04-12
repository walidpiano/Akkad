Imports System.Data.SqlClient
Public Class frmAddCheck
    Dim myConn As New SqlConnection(GV.myConn)
    Public Shared ID As Integer = 0
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub clear()
        deDate.EditValue = Today
        txtDetails.Text = ""
        txtEGP.Text = ""
        txtUSD.Text = ""
        ID = 0
    End Sub
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If deDate.EditValue = Nothing Then
            MsgBox("!برجاء إدخال التاريخ")
            deDate.Focus()
            deDate.SelectAll()
        ElseIf txtDetails.Text = "" Then
            MsgBox("!برجاء إدخال التفاصيل")
            txtDetails.Focus()
        ElseIf Val(txtEGP.Text) = 0 AndAlso Val(txtUSD.Text) = 0 Then
            MsgBox("!يجب إدخال المبلغ")
        Else
            Dim Query As String = ""
            If ID = 0 Then
                Query = "INSERT INTO tblCheck (cDate, cTime, EGP, USD, cDetails, cUser) VALUES (@Date, @Time, @EGP, @USD, @Details, @User);"
            Else
                Query = "UPDATE tblCheck SET cDate = @Date, EGP = @EGP, USD = @USD, cDetails = @Details, cUser = @User WHERE ID = " & ID & ";"
            End If


            Using cmd = New SqlCommand(Query, myConn)
                cmd.Parameters.AddWithValue("@Date", deDate.EditValue)
                cmd.Parameters.AddWithValue("@Time", Now.ToString("HH:mm"))
                cmd.Parameters.AddWithValue("@EGP", Val(txtEGP.Text))
                cmd.Parameters.AddWithValue("@USD", Val(txtUSD.Text))
                cmd.Parameters.AddWithValue("@Details", txtDetails.Text)
                cmd.Parameters.AddWithValue("@User", GlobalVariables.ID)
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