Imports System.Net.NetworkInformation
Imports System.Data.SqlClient
Imports System.Management


Public Class TestForm
    Public Shared myMac As String
    Public Shared HDD_Serial, MB_Serial As String
    Public Shared Master As String

    Private Function getMacAddress() As String
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

    Private Function getMotherSerial() As String
        Dim mBoard As New ManagementObjectSearcher("SELECT * FROM win32_BaseBoard")
        For Each MB In mBoard.Get
            MB_Serial = MB("SerialNumber")
        Next
        Return MB_Serial
    End Function

    Private Function getHDDSerial() As String
        Dim Sniper1 As New ManagementObjectSearcher("SELECT * FROM win32_DiskDrive")
        For Each HD In Sniper1.Get
            HDD_Serial = HD("SerialNumber")
        Next
        Return HDD_Serial
    End Function
    Private Sub TestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label4.Text = My.Settings.tMac
        Label5.Text = My.Settings.tMB
        Label6.Text = My.Settings.tHDD
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Text = getMacAddress()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Label2.Text = getMotherSerial()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label3.Text = getHDDSerial()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        My.Settings.tMac = Label1.Text
        My.Settings.tMB = Label2.Text
        My.Settings.tHDD = Label3.Text
        My.Settings.Save()
    End Sub
End Class