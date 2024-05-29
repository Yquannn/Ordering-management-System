Public Class Form3

    Private Sub UpdateTotalPriceForm()
        ' Append text to the RichTextBox in the instance of Form2
        Form2.RichTextBox2.AppendText("(x" & prodQuantity.Text & ") " & order.Text & " ₱" & price.Text & Environment.NewLine)
        Form2.totalBalanceForm3 += total
        Form2.totalBill.Text = Form2.totalBalance + Form2.totalBalanceForm2 + Form2.totalBalanceForm3



        ' Clear the text fields
        order.Text = ""
        prodQuantity.Text = ""
        price.Text = ""
        Me.Hide()
        Form2.Opacity = 1




        ' Update total bill on Form2
        'Dim currentTotal As Integer
        'If Integer.TryParse(Form2.totalBill.Text, currentTotal) Then
        '    currentTotal += Integer.Parse(total)
        '    Form2.totalBill.Text = currentTotal.ToString()
        'Else

        '    'Dim res = Form2.bills.ToString + total.ToString
        '    'res += Form2.bills
        '    'MsgBox(res)
        'End
    End Sub

    Public Property total As Integer = 0
    Public Property newPrice As Integer = 0

    Private Sub prodQuantity_TextChanged(sender As Object, e As EventArgs) Handles prodQuantity.TextChanged

        Dim quantity As Integer
        If Not Integer.TryParse(prodQuantity.Text, quantity) Then
            ' Handle the case where the quantity is not a valid integer
            MessageBox.Show("Added to order")
        Else

            ' Calculate the total price
            total = newPrice * quantity

            ' Display the total price (assuming totalPrice is a TextBox or Label)
            totalPrice.Text = total.ToString()


        End If
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        UpdateTotalPriceForm()

    End Sub


    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Me.Hide()
        Form2.Opacity = 1
    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub






End Class