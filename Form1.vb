Imports MySql.Data.MySqlClient

Public Class Form1
    Dim connection As String = "server=127.0.0.1; user=root; database=ordering_management_system; password="
    Dim Con As New MySqlConnection(connection)
    Private Sub Guna2CustomGradientPanel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2CustomGradientPanel1.Paint

    End Sub
    Private Sub Login(username As String, password As String, userType As String)
        Try
            Dim query As String = "SELECT name, type FROM employees WHERE email = @Username AND password = @Password AND type = @Usertype"

            Con.Open()

            Using command As New MySqlCommand(query, Con)
                command.Parameters.AddWithValue("@Username", username)
                command.Parameters.AddWithValue("@Password", password)
                command.Parameters.AddWithValue("@Usertype", userType)

                Using reader As MySqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        Dim name As String = reader.GetString(reader.GetOrdinal("name"))
                        Dim type As String = reader.GetString(reader.GetOrdinal("type"))

                        Dim newForm2 As New Form2
                        newForm2.userName.Text = name
                        newForm2.user.Text = type

                        Me.Hide()
                        newForm2.Show()


                        email.Text = ""
                        pass.Text = ""
                    Else
                        MessageBox.Show("Invalid username or password.")
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Login(email.Text, pass.Text, type.Text)

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            pass.PasswordChar = ControlChars.NullChar ' Show the password
        Else
            pass.PasswordChar = "*"c ' Hide the password
        End If
    End Sub
End Class
