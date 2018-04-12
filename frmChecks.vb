Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors

Partial Public Class frmChecks
    Dim myConn As New SqlConnection(GV.myConn)
    Public Sub New()
        InitializeComponent()

    End Sub
    Private Sub windowsUIButtonPanel_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.Docking2010.ButtonEventArgs) Handles windowsUIButtonPanel.ButtonClick
        If e.Button.Properties.Caption = "Print" Then
            GridControl1.ShowRibbonPrintPreview()
        ElseIf e.Button.Properties.Caption = "Refresh" Then
            loadData()
        ElseIf e.Button.Properties.Caption = "Delete" Then
            If GridView1.GetFocusedRowCellValue("Details") <> "Selling" Then
                Dim ID As String = GridView1.GetFocusedRowCellValue("ID")
                Dim Query As String = "DELETE FROM tblCheck WHERE ID = " & ID & ";"
                Using cmd = New SqlCommand(Query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    cmd.ExecuteNonQuery()
                    myConn.Close()
                    loadData()
                End Using
            End If
        ElseIf e.Button.Properties.Caption = "New" Then
            frmAddCheck.deDate.EditValue = Today
            frmAddCheck.ShowDialog()
            If frmAddCheck.DialogResult = Windows.Forms.DialogResult.OK Then
                loadData()
            End If
        ElseIf e.Button.Properties.Caption = "Edit" Then
            If GridView1.GetFocusedRowCellValue("Details") <> "Selling" Then
                Dim ID As String = GridView1.GetFocusedRowCellValue("ID")
                Dim Query As String = "SELECT * FROM tblCheck WHERE ID = " & ID & ";"
                Using cmd = New SqlCommand(Query, myConn)
                    If myConn.State = ConnectionState.Closed Then
                        myConn.Open()
                    End If
                    Using dr As SqlDataReader = cmd.ExecuteReader
                        If dr.Read() Then
                            With frmAddCheck
                                Dim dtt As Date
                                Date.TryParse(dr(1), dtt)
                                .deDate.EditValue = dtt
                                If Not IsDBNull(dr(3)) Then
                                    .txtEGP.Text = dr(3)
                                End If
                                If Not IsDBNull(dr(4)) Then
                                    .txtUSD.Text = dr(4)
                                End If
                                .txtDetails.Text = dr(5)
                            End With
                        End If
                    End Using
                    myConn.Close()
                End Using
                frmAddCheck.ID = ID
                frmAddCheck.ShowDialog()
                If frmAddCheck.DialogResult = Windows.Forms.DialogResult.OK Then
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
        Dim Query As String = "(" _
                              & " SELECT 0 AS ID, [Date], [Time], " _
                              & " (CASE WHEN Visa1 = 1 THEN" _
                              & " (CASE WHEN Currency = 'EGP' THEN pEGP - NetTotal ELSE pEGP END) ELSE 0 END) AS EGP," _
                              & " (CASE WHEN Visa2 = 1 THEN" _
                              & " (CASE WHEN Currency = 'USD' THEN pUSD - NetTotal ELSE pUSD END) ELSE 0 END) AS USD," _
                              & " 'Selling' AS Details, tblLogin.UserName" _
                              & " FROM tblOut1" _
                              & " INNER JOIN tblLogin ON tblOut1.[User] = tblLogin.Sr" _
                              & " WHERE Visa1 = 1 OR Visa2 = 1" _
                              & " )" _
                              & " UNION ALL" _
                              & " (" _
                              & " SELECT ID, cDate AS [Date], cTime AS [Time], -EGP, -USD, cDetails AS Details, tblLogin.UserName" _
                              & " FROM tblCheck" _
                              & " INNER JOIN tblLogin ON tblCheck.cUser = tblLogin.Sr" _
                              & " )" _
                              & " ORDER BY [Date], [Time];"
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
