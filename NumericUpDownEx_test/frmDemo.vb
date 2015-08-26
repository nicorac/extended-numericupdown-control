Public Class frmDemo

    Private Sub NumericUpDown1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles NumericUpDown1.MouseEnter
        Debug.Print("NumericUpDown - MouseEnter")
    End Sub

    Private Sub NumericUpDown1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles NumericUpDown1.MouseLeave
        Debug.Print("NumericUpDown - MouseLeave")
    End Sub

    Private Sub NumericUpDownEx1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles NumericUpDownEx1.MouseEnter
        Debug.Print("NumericUpDownEx - MouseEnter")
    End Sub

    Private Sub NumericUpDownEx1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles NumericUpDownEx1.MouseLeave
        Debug.Print("NumericUpDownEx - MouseLeave")
    End Sub

    Private Sub _GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles NumericUpDown1.GotFocus, NumericUpDownEx1.GotFocus
        PropertyGrid1.SelectedObject = sender
    End Sub

End Class
