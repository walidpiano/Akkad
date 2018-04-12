Imports System.Data.SqlClient
Public Class frmSQL
    Dim myConn As New SqlConnection(GV.myConn)
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click

        Using cmd = New SqlCommand(TextEdit1.Text, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Try
                cmd.ExecuteNonQuery()
                MsgBox("Done!")
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            myConn.Close()
        End Using

    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        frmLayout.Show()
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        TextEdit1.Text = Nothing
        TextEdit1.Focus()
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        Dim dt As New DataTable
        Using cmd = New SqlCommand(TextEdit1.Text, myConn)
            If myConn.State = ConnectionState.Closed Then
                myConn.Open()
            End If
            Try
                Using dr As SqlDataReader = cmd.ExecuteReader
                    dt.Load(dr)
                End Using
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            myConn.Close()
        End Using
        If dt.Rows.Count > 0 Then
            frmSQLGrid.GridControl1.DataSource = Nothing
            frmSQLGrid.GridView1.PopulateColumns()
            frmSQLGrid.GridControl1.DataSource = dt
            frmSQLGrid.GridView1.PopulateColumns()
            frmSQLGrid.ShowDialog()
        End If
    End Sub
End Class