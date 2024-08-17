Imports MySql.Data.MySqlClient

Public Class Form3
    Dim connection As String = "server=127.0.0.1; user=root; database=ordering_management_system; password="
    Dim Con As New MySqlConnection(connection)
    Public Property total As Integer = 0
    Public Property newPrice As Integer = 0
    Dim quantity As Integer


    Public Sub UpdateTotalPriceForm()

        Dim orderForm As Form2 = CType(Application.OpenForms("Form2"), Form2)

        ' Check if Form2 is open
        If orderForm IsNot Nothing Then
            ' Create a new row in the DataGridView
            Dim newRow As DataGridViewRow = CType(orderForm.orderDGV.Rows(orderForm.orderDGV.Rows.Add()), DataGridViewRow)

            ' Set the values for the new row
            newRow.Cells("Code").Value = codeName.Text
            newRow.Cells("Quantity").Value = prodQuantity.Text
            newRow.Cells("Order").Value = order.Text
            newRow.Cells("Price").Value = priceLabel.Text

            Dim total As Decimal
            ' Parse the price text to decimal
            If Decimal.TryParse(priceLabel.Text, total) Then
                Dim quantityValue As Integer
                If Integer.TryParse(prodQuantity.Text, quantityValue) Then
                    ' Increment the total balance for Form2
                    orderForm.totalBalanceForm3 += total * quantityValue
                Else
                    MessageBox.Show("Invalid quantity value.")
                End If
            Else
                MessageBox.Show("Invalid price value.")
            End If

            ' Update the total bill display
            orderForm.totalBill.Text = (orderForm.totalBalance + orderForm.totalBalanceForm2 + orderForm.totalBalanceForm3).ToString("F2")



            ' Notify the user
            MsgBox("Added to order")

            ' Clear the form fields
            codeName.Text = ""
            prodQuantity.Text = ""
            priceLabel.Text = ""

            ' Hide the current form
            Me.Hide()
        Else
            MessageBox.Show("Form2 is not open.")
        End If
    End Sub




    ' Event handler for when the product quantity text changes
    Private Sub prodQuantity_TextChanged(sender As Object, e As EventArgs)
        If Integer.TryParse(prodQuantity.Text, quantity) Then
            ' Calculate the total price
            total = newPrice * quantity
            totalPrice.Text = total.ToString("F2")

        End If
    End Sub


    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        UpdateTotalPriceForm()
    End Sub


    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Me.Hide()
    End Sub


    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'prodPrice.Enabled = False
    End Sub

    Private Sub getPrice()
        Try
            Con.Open()
            Dim sql As String = "SELECT ProductName, small, medium, large FROM product"
            Dim cmd As New MySqlCommand(sql, Con)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim tb1 As New DataTable()
            adapter.Fill(tb1)

            ' Bind data to the sizing ComboBox
            sizeComboBox.DataSource = tb1
            sizeComboBox.DisplayMember = "ProductName" ' Show product names in the ComboBox
            sizeComboBox.ValueMember = "ProductName"   ' Set the value to the product name or ID

            If sizeComboBox.Items.Count > 0 Then
                sizeComboBox.SelectedIndex = -1 ' No selection initially
            End If
        Catch ex As MySqlException
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try
    End Sub



    Private Sub prodQuantity_TextChanged_1(sender As Object, e As EventArgs) Handles prodQuantity.TextChanged
        Dim quantity As Integer
        Dim price As Decimal
        Dim total As Decimal

        ' Try to parse the quantity from the TextBox
        If Not Integer.TryParse(prodQuantity.Text, quantity) Then

            Exit Sub
        End If


        If Not Decimal.TryParse(priceLabel.Text, price) Then

            Exit Sub
        End If

        ' Calculate the total price
        total = price * quantity

        ' Display the total price
        totalPrice.Text = total.ToString("0.00")

    End Sub



    Private Sub Guna2GradientPanel2_Paint(sender As Object, e As PaintEventArgs) Handles Guna2GradientPanel2.Paint

    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click

    End Sub

    Dim sizePrices As New Dictionary(Of String, Decimal) From {
    {"small", 10D},
    {"medium", 15D},
    {"large", 20D}
    }
    Private Function GetPriceFromDatabase(size As String) As Decimal
        Dim price As Decimal = 0.0D

        Try
            Con.Open()

            ' Construct the query to select the price based on the size
            Dim query As String = "SELECT " & size & " FROM product WHERE productName = @productName"

            Using command As New MySqlCommand(query, Con)
                command.Parameters.AddWithValue("@productName", order.Text)

                ' Execute the query and get the result
                Dim result As Object = command.ExecuteScalar()
                If result IsNot Nothing AndAlso IsNumeric(result) Then
                    price = Convert.ToDecimal(result)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching price: " & ex.Message)
        Finally

            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try

        Return price
    End Function

    Private Sub sizeComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sizeComboBox.SelectedIndexChanged

        Dim selectedSize As String = sizeComboBox.SelectedItem.ToString()


        Dim price As Decimal = GetPriceFromDatabase(selectedSize)


        priceLabel.Text = price.ToString("0.00")
    End Sub

End Class
