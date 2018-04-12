Imports System.Net.NetworkInformation
Imports System.Data.SqlClient
Imports System.Management
Imports System.Text
Imports System.Security.Cryptography


Public Class frmSplash

    Public Shared Function getMacAddress() As String
        Try
            Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
            Dim adapter As NetworkInterface
            Dim myMac As String = String.Empty

            For Each adapter In adapters
                Select Case adapter.NetworkInterfaceType
                    'Exclude Tunnels, Loopbacks and PPP
                    Case NetworkInterfaceType.Tunnel, NetworkInterfaceType.Loopback, NetworkInterfaceType.Ppp
                    Case Else
                        If Not adapter.GetPhysicalAddress.ToString = String.Empty And Not adapter.GetPhysicalAddress.ToString = "00000000000000E0" Then
                            myMac = adapter.GetPhysicalAddress.ToString
                            Exit For ' Got a mac so exit for
                        End If
                End Select
            Next adapter

            Return myMac
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Public Shared myMac As String
    Public Shared HDD_Serial, MB_Serial As String
    Public Shared Master As String

    Dim myConn As New SqlConnection(GV.myConn)

 
    Private Sub frmSplash_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control And e.KeyCode = Keys.H Then
            'Me.Timer1.Enabled = False
            frmVerify.Show()
        End If
    End Sub

    Private Sub frmSplash_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Me.TransparencyKey = Me.BackColor
        'Version info
        Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)

        'Load the HDD data & MBoard
        'Dim HDD_Serial, MB_Serial As String
        'HDD_Serial = Nothing
        Dim Sniper1 As New ManagementObjectSearcher("SELECT * FROM win32_DiskDrive")
        For Each HD In Sniper1.Get
            HDD_Serial = HD("SerialNumber")
        Next

        Dim mBoard As New ManagementObjectSearcher("SELECT * FROM win32_BaseBoard")
        For Each MB In mBoard.Get
            MB_Serial = MB("SerialNumber")
        Next
        myMac = getMacAddress()
        Master = GenerateHash((myMac & HDD_Serial).Replace(" ", ""))

    End Sub

    Public Function GenerateHash(ByVal SourceText As String, Optional Type As Integer = 1) As String
        'Create an encoding object to ensure the encoding standard for the source text
        Dim Ue As New UnicodeEncoding()
        'Retrieve a byte array based on the source text
        Dim ByteSourceText() As Byte = Ue.GetBytes(SourceText)
        'Instantiate an MD5 Provider object
        Dim Md5 As New MD5CryptoServiceProvider()
        Dim SHA1 As New SHA1CryptoServiceProvider()

        'Compute the hash value from the source
        Dim ByteHash() As Byte
        If Type = 1 Then
            ByteHash = Md5.ComputeHash(ByteSourceText)
        Else
            ByteHash = SHA1.ComputeHash(ByteSourceText)
        End If
        'And convert it to String format for return
        Return Convert.ToBase64String(ByteHash)
    End Function

    Public Shared Function validity() As Boolean
        Dim vali As Boolean

        Using mc = New SqlCommand("SELECT * FROM tblMaster WHERE [Master] = '" & Master & "'", frmSplash.myConn)
            If frmSplash.myConn.State = ConnectionState.Closed Then
                frmSplash.myConn.Open()
            End If


            Using dr As SqlDataReader = mc.ExecuteReader
                If dr.Read() Then
                    vali = True
                Else
                    vali = False
                End If

            End Using
            frmSplash.myConn.Close()
        End Using
        ''''''this is only for square market!!!!!!!!!!!!!!!
        Dim dttttt As Date = GV.deadLine
        If dttttt >= Today Then
            vali = True
        End If
        'vali = True
        Return vali
        'If vali = True Then
        '    Me.Timer1.Enabled = False
        '    frmLogin.Show()
        '    Me.Close()

        'Else
        '    Me.Timer1.Enabled = False
        '    frmVerify.Show()
        'End If
    End Function
    Private Sub Label1_DoubleClick(sender As Object, e As System.EventArgs)
        ' Me.Timer1.Enabled = False

        Using Reg = New SqlCommand("INSERT INTO tblMaster ([Master]) Values ('" & Master & "')", myConn)
            myConn.Open()
            Reg.ExecuteNonQuery()
            myConn.Close()
        End Using
        Me.Close()

    End Sub

    Private Sub Version_DoubleClick(sender As Object, e As System.EventArgs) Handles Version.DoubleClick
        ' Me.Timer1.Enabled = False
        frmVerify.Show()
    End Sub

    Private Sub lblVerify_Click(sender As Object, e As EventArgs) Handles lblVerify.Click
        ' Me.Timer1.Enabled = False
        frmVerify.Show()
    End Sub
End Class
