Imports System.Data.SqlClient
Public Class frmCheckCost
    'Sub New()
    '    InitializeComponent()
    'End Sub

    'Public Overrides Sub ProcessCommand(ByVal cmd As System.Enum, ByVal arg As Object)
    '    MyBase.ProcessCommand(cmd, arg)
    'End Sub

    'Public Enum SplashScreenCommand
    '    SomeCommandId
    'End Enum

    Private Sub frmCheckCost_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        TextEdit1.Focus()

    End Sub

    Private Sub frmCheckCost_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        frmCashier.oItemSerial.Focus()
        frmCashier.oItemSerial.SelectAll()
    End Sub

    Private Sub frmCheckCost_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub frmCheckCost_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Opacity = 100%
        TextEdit1.Focus()
    End Sub

    Private Sub TextEdit1_EditValueChanged(sender As Object, e As EventArgs) Handles TextEdit1.EditValueChanged
        If TextEdit1.Text = "" Then
            LabelControl2.Text = "اسم الصنف"
            LabelControl3.Text = "0.00"
        End If
    End Sub

    Private Sub TextEdit1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextEdit1.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyCode = Keys.Enter And TextEdit1.Text <> "" Then
            Call CheckItem(TextEdit1.Text)
        End If
    End Sub

    Public Sub CheckItem(ByVal Serial As String)
        Dim Query As String = "select tblItems.Name, avg(tblin2.UnitPrice) as UnitPrice" _
                              & " from tblin2 inner join tblItems on tblin2.item = tblItems.PrKey" _
                              & " where tblItems.Serial = '" & Serial & "'" _
                              & " group by tblItems.Name"

        Using cmd = New SqlCommand(Query, frmMain.myConn)
            If frmMain.myConn.State = ConnectionState.Closed Then
                frmMain.myConn.Open()
            End If
            Using dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read() Then
                    LabelControl2.Text = dr(0)
                    LabelControl3.Text = dr(1)
                Else
                    LabelControl2.Text = "اسم الصنف"
                    LabelControl3.Text = "0.00"
                    TextEdit1.Focus()
                    TextEdit1.SelectAll()
                End If
            End Using
            frmMain.myConn.Close()
        End Using
    End Sub
End Class
