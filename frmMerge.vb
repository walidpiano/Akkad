Imports System.Data.SqlClient
Public Class frmMerge

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        If Not TextEdit1.Text = "" And Not TextEdit2.Text = "" Then
            Dim query As String = "DECLARE @fOne NVARCHAR(20) = '" & TextEdit1.Text & "'" _
                                  & " DECLARE @sOne NVARCHAR(20) = '" & TextEdit2.Text & "'" _
                                  & " DECLARE @fItem INT = (SELECT PrKey FROM tblItems WHERE Serial = @fOne);" _
                                  & " DECLARE @sItem INT = (SELECT PrKey FROM tblItems WHERE Serial = @sOne);" _
                                  & " UPDATE tblOut2 SET Item = @fItem WHERE Item = @sItem;" _
                                  & " UPDATE tblIn2 SET Item = @fItem WHERE Item = @sItem;" _
                                  & " DELETE FROM tblItems WHERE PrKey = @sItem;" _
                                  & " INSERT INTO tblMultiCodes (Item, Code) VALUES (@fItem, @sOne);"
            Using cmd = New SqlCommand(query, frmMain.myConn)
                If frmMain.myConn.State = ConnectionState.Closed Then
                    frmMain.myConn.Open()
                End If
                Try
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("فشل الدمج")
                End Try
                frmMain.myConn.Close()
            End Using


        End If
    End Sub
End Class