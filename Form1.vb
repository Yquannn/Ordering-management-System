Public Class Form1
    Private Sub Guna2CustomGradientPanel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2CustomGradientPanel1.Paint

    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        If email.Text = "admin" And pass.Text = "admin" Then
            Me.Hide()
            Form2.Show()
        Else
            MsgBox("Invalid email or password")
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            pass.PasswordChar = ControlChars.NullChar ' Show the password
        Else
            pass.PasswordChar = "*"c ' Hide the password
        End If
    End Sub
End Class
