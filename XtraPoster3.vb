Public Class XtraPoster3

    Private Sub XrBarcode_BeforePrint(sender As Object, e As Printing.PrintEventArgs) Handles XrBarcode.BeforePrint
        If XrBarcode.Text = "" Then
            e.Cancel = True
        End If
    End Sub
End Class