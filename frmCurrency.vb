Imports System.Data.SqlClient
Public Class frmCurrency
    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub
    Private Sub fillData()
        Dim Query As String = "SELECT TOP(1) tblCurrency.[Date] + tblCurrency.[Time] AS [Time], tblCurrency.EUR, tblCurrency.EGP," _
                              & " tblCurrency.GBP, tblCurrency.RUB, tblCurrency.CHF, tblCurrency.CNY, tblLogin.UserName" _
                              & " FROM tblCurrency INNER JOIN tblLogin ON tblCurrency.[User] = tblLogin.Sr" _
                              & " ORDER BY PrKey DESC;"
        Using cmd = New SqlCommand(Query, frmMain.myConn)
            If frmMain.myConn.State = ConnectionState.Closed Then
                frmMain.myConn.Open()
            End If
            Using dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read() Then
                    lblLastUpdate.Text = dr(0)
                    txtEUR.Text = dr(1)
                    txtEGP.Text = dr(2)
                    txtGBP.Text = dr(3)
                    txtRUB.Text = dr(4)
                    txtCHF.Text = dr(5)
                    txtCNY.Text = dr(6)
                    lblUser.Text = dr(7)
                End If
            End Using
            frmMain.myConn.Close()
        End Using
    End Sub

    Private Sub UpdateData()
        Dim Query As String = "INSERT INTO tblCurrency ([Date], [Time], EUR, EGP, GBP, RUB, CHF, CNY, [User])" _
                              & " VALUES ('" & Today.ToString("MM/dd/yyyy") & "', '" & Now.ToString("HH:mm:ss") & "', " _
                              & Val(txtEUR.Text) & ", " & Val(txtEGP.Text) & ", " & Val(txtGBP.Text) & ", " & Val(txtRUB.Text) _
                              & ", " & Val(txtCHF.Text) & ", " & Val(txtCNY.Text) & ", " & GlobalVariables.ID & ");"

        Using cmd = New SqlCommand(Query, frmMain.myConn)
            If frmMain.myConn.State = ConnectionState.Closed Then
                frmMain.myConn.Open()
            End If
            cmd.ExecuteNonQuery()
            frmMain.myConn.Close()
        End Using
        fillData()
        MsgBox("Rates updated successfully!")
    End Sub

    Private Sub frmCurrency_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        frmCashier.getCurrency()
    End Sub
    Private Sub frmCurrency_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fillData()
    End Sub

    Private Sub txtEUR_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEUR.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And txtEUR.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtEGP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEGP.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And txtEGP.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtGBP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtGBP.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And txtGBP.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtRUB_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRUB.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And txtRUB.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtCHF_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCHF.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And txtCHF.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtCNY_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCNY.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = "." Then
            e.Handled = True
        End If
        If e.KeyChar = "." And txtCNY.Text Like "*" & "." & "*" Then
            e.Handled = True
        End If
    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        If GlobalVariables.authority <> "Admin" AndAlso ExClass.validAccess = False Then
            Exit Sub
        End If

        If Val(txtEUR.Text) = 0 Then
            MsgBox("You should insert the EUR rate!")
            txtEUR.Focus()
            txtEUR.SelectAll()
        ElseIf Val(txtEGP.Text) = 0 Then
            MsgBox("You should insert the EGP rate!")
            txtEGP.Focus()
            txtEGP.SelectAll()
        ElseIf Val(txtGBP.Text) = 0 Then
            MsgBox("You should insert the GBP rate!")
            txtGBP.Focus()
            txtGBP.SelectAll()
        ElseIf Val(txtRUB.Text) = 0 Then
            MsgBox("You should insert the RUB rate!")
            txtRUB.Focus()
            txtRUB.SelectAll()
        ElseIf Val(txtCHF.Text) = 0 Then
            MsgBox("You should insert the UAH rate!")
            txtCHF.Focus()
            txtCHF.SelectAll()
        ElseIf Val(txtCNY.Text) = 0 Then
            MsgBox("You should insert the CNY rate!")
            txtCNY.Focus()
            txtCNY.SelectAll()
        Else
            UpdateData()
            frmCashier.OutTotalize()
        End If
    End Sub
End Class