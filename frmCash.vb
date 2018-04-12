Imports System.Data.SqlClient
Public Class frmCash

    Private Sub frmCash_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cSum.Focus()
    End Sub

    Private Sub btCancel_Click(sender As System.Object, e As System.EventArgs) Handles btCancel.Click
        Me.Close()
    End Sub

    Private Sub btOK_Click(sender As System.Object, e As System.EventArgs) Handles btOK.Click
        If Val(cSum.Text) = 0 Then
            MessageBox.Show("«·„»·€ «·–Ì  „ ≈œŒ«·Â €Ì— ’ÕÌÕ!", "Wrong Sum", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cSum.Focus()
            cSum.SelectAll()
        ElseIf cIndication.Text = "" Then
            MessageBox.Show("ÌÃ» ≈œŒ«· »Ì«‰!", "Wrong Indication", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cIndication.Focus()
            cIndication.SelectAll()
        Else


            Dim Query As String = "INSERT INTO tblCash ([Type], [Date], [Time], Qnty, Indication, [User])" _
                                  & " VALUES (N'" & lblTitle.Text & "', '" & Today.ToString("MM/dd/yyyy") & "', '" & Now.ToString("HH:mm") & "', '" & Val(cSum.Text) & "', N'" & cIndication.Text & "','" & GlobalVariables.ID & "')"



            Using cmd = New SqlCommand(Query, frmMain.myConn)
                If frmMain.myConn.State = ConnectionState.Closed Then
                    frmMain.myConn.Open()
                End If
                cmd.ExecuteNonQuery()
                frmMain.myConn.Close()

                cSum.Text = ""
                cIndication.Text = ""
                Me.Close()

            End Using
        End If
    End Sub

    Private Sub cSum_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles cSum.EditValueChanged

    End Sub

    Private Sub cSum_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cSum.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And cSum.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub frmCash_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        cSum.Focus()
    End Sub

    Private Sub frmCash_TextChanged(sender As Object, e As System.EventArgs) Handles Me.TextChanged
        If Me.Text = "IMPORTS" Then
            lblTitle.Text = "Ê«—œ"
        Else
            lblTitle.Text = "„‰’—›"
        End If
    End Sub
End Class
