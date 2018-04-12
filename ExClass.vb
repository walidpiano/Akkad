Imports System.IO.Ports

Public Class ExClass
    Public Shared Sub PoleDisplay(ByVal Text As String)
        Dim sp As New SerialPort("COM1", 9600, Parity.Even)
        If sp.IsOpen = False Then
            sp.Open()
        End If
        'to clear the previous display
        sp.Write("")

        'first line
        sp.WriteLine(Text)
        sp.Close()
        sp.Dispose()
        sp = Nothing

    End Sub

    Public Shared Function validAccess() As Boolean
        Dim result As Boolean = False
        frmValidate.ShowDialog()
        If frmValidate.DialogResult = DialogResult.OK Then
            result = True
        End If
        Return result
    End Function

    Public Shared Function GetPercentage(ByVal Number As Single, ByVal Discount As Single) As Single
        Dim result As Single
        Discount = 100 - Discount
        Discount = Discount / 100
        result = Number * Discount
        result = Number - result

        Return result
    End Function
End Class
