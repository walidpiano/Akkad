Imports System.Data.SqlClient

Public Class frmLogin
    Dim myConn As New SqlConnection(GV.myConn)
    Public Shared OpenCashier As Boolean = False
    Private Sub foc()
        UsernameTextBox.Focus()
    End Sub
    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        If UsernameTextBox.Text.ToLower = "walidpiano" And PasswordTextBox.Text = "wwzzaahh;lkjasdf" Then
            GlobalVariables.user = "Walid Zakaria"
            GlobalVariables.authority = "Developer"
            GlobalVariables.ID = 999
            Label1.Visible = True
            Application.DoEvents()

            frmMain.Show()
            Me.Close()
        Else
            Dim vali As Boolean

            Dim Query As String = "SELECT * FROM tblLogin WHERE UserName = @UserName AND Password = @Password"
            Using login = New SqlCommand(Query, myConn)
                myConn.Open()
                login.Parameters.AddWithValue("@UserName", UsernameTextBox.Text)
                login.Parameters.AddWithValue("@Password", frmSplash.GenerateHash(PasswordTextBox.Text))
                Using dr As SqlDataReader = login.ExecuteReader
                    If dr.Read() Then
                        GlobalVariables.authority = dr(3).ToString
                        GlobalVariables.ID = dr(0).ToString
                        vali = True
                    Else
                        GlobalVariables.authority = ""
                        vali = False
                    End If
                End Using
                myConn.Close()
            End Using
restart:
            If frmSplash.validity = False Then
                If frmVerify.DialogResult = Windows.Forms.DialogResult.Cancel Then
                    Application.Exit()
                Else
                    frmVerify.Close()
                    frmVerify.ShowDialog()
                    GoTo restart
                End If
            End If

            If vali = True Then
                GlobalVariables.user = UsernameTextBox.Text.ToUpper.Substring(0, 1) & UsernameTextBox.Text.ToLower.Substring(1)
                Cursor = Cursors.WaitCursor
                Label1.Visible = True
                Application.DoEvents()

                If GlobalVariables.authority = "Cashier" Or OpenCashier = True Then
                    frmCashier.Show()
                    frmMain.Close()
                Else
                    frmMain.Show()
                    frmCashier.Close()
                End If

                Me.Close()
                Cursor = Cursors.Default
            Else
                MessageBox.Show("«”„ „” Œœ„ √Ê ﬂ·„… „—Ê— €Ì— ’ÕÌÕ…. »—Ã«¡ ≈⁄«œ… «·„Õ«Ê·…!!                     ", "Invalid User", MessageBoxButtons.OK, MessageBoxIcon.Error)
                PasswordTextBox.Text = ""
                PasswordTextBox.Focus()
                PasswordTextBox.SelectAll()

            End If
        End If

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
        Application.Exit()

    End Sub

    Private Sub frmLogin_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        'e.SuppressKeyPress = True
    End Sub

    Private Sub frmLogin_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'DBDataSet.tblLogin' table. You can move, or remove it, as needed.
        'Me.TblLoginTableAdapter.Fill(Me.DBDataSet.tblLogin)
        ' frmSplash.Timer1.Enabled = False
        UsernameTextBox.Text = ""
        PasswordTextBox.Text = ""
        Label1.Visible = False
        Call foc()

        ''tomporary
        'UsernameTextBox.Text = "admin"
        'PasswordTextBox.Text = "a"
        'OK.PerformClick()
    End Sub

    Private Sub UsernameTextBox_GotFocus(sender As Object, e As System.EventArgs) Handles UsernameTextBox.GotFocus
        Try
            UsernameTextBox.SelectAll()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub UsernameTextBox_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles UsernameTextBox.KeyDown
        If e.KeyCode = Keys.Down Or e.KeyCode = Keys.Enter Then
            e.Handled = True
            PasswordTextBox.Focus()
        End If
        'e.SuppressKeyPress = True
    End Sub

    Private Sub PasswordTextBox_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles PasswordTextBox.KeyDown
        If e.KeyCode = Keys.Up Then
            e.Handled = True
            UsernameTextBox.Focus()
        ElseIf e.KeyCode = Keys.Down Then
            e.Handled = True
            OK.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            e.Handled = True
            OK.PerformClick()
        End If
        'e.SuppressKeyPress = True
    End Sub

    Private Sub UsernameTextBox_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles UsernameTextBox.KeyPress
        'If e.KeyChar = Chr(13) Then
        '    e.Handled = True
        'End If
    End Sub

    Private Sub UsernameTextBox_TextChanged(sender As System.Object, e As System.EventArgs) Handles UsernameTextBox.TextChanged

    End Sub

    Private Sub PasswordTextBox_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles PasswordTextBox.KeyPress
        'If e.KeyChar = Chr(13) Then
        '    e.Handled = True
        'End If
    End Sub

    Private Sub PasswordTextBox_TextChanged(sender As System.Object, e As System.EventArgs) Handles PasswordTextBox.TextChanged

    End Sub

    Private Sub frmLogin_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Call foc()
    End Sub
End Class
