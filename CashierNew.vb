Imports System.ComponentModel
Imports System.Text
Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraBars.Helpers
Imports System.Data.SqlClient
Imports DevExpress.XtraLayout
Imports System.IO
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraTabbedMdi
Imports System.Collections

Public Class CashierNew
    Shared Sub New()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.Skins.SkinManager.EnableFormSkins()
    End Sub
    Public Sub New()
        InitializeComponent()
        Me.InitSkinGallery()
        If Not My.Settings.Theme = "" Then
            UserLookAndFeel.Default.SkinName = My.Settings.Theme.ToString()
        End If
    End Sub

    Private Sub InitSkinGallery()
        SkinHelper.InitSkinGallery(rgbiSkins, True)
    End Sub
    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        frmCash.Text = "IMPORTS"
        frmCash.ShowDialog()
    End Sub

    Private Sub BarButtonItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        frmCash.Text = "EXPORTS"
        frmCash.ShowDialog()
    End Sub

    Private Sub BarButtonItem4_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        frmRates.Show()
    End Sub

    Private Sub BarButtonItem5_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        Shell("Calc", AppWinStyle.NormalFocus, True)
    End Sub

    Private Sub BarButtonItem6_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        frmLogin.UsernameTextBox.Text = Nothing
        frmLogin.PasswordTextBox.Text = Nothing
        frmLogin.ShowDialog()
        lblUserName.Caption = GlobalVariables.user

    End Sub

    Private Sub BarButtonItem7_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        frmMain.Show()
        Me.Close()
    End Sub

    Private Sub BarButtonItem8_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem8.ItemClick
        frmPassword.ShowDialog()
    End Sub

    Private Sub BarButtonItem9_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem9.ItemClick
        Me.Close()
        Application.Exit()
    End Sub

    Private Sub CashierNew_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.Theme = UserLookAndFeel.Default.SkinName.ToString
        My.Settings.Save()
    End Sub

    Private Sub CashierNew_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        frmInvoiceNew.MdiParent = Me
        frmInvoiceNew.Show()
    End Sub
End Class