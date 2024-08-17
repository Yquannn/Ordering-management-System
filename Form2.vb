Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports System.Runtime.Intrinsics.X86
Imports System.Text
Imports Mysqlx.Crud

Public Class Form2

    Dim connection As String = "server=127.0.0.1; user=root; database=ordering_management_system; password="
    Dim Con As New MySqlConnection(connection)
    Dim productId = ""
    Public Property product As String

    Private Sub SetupDataGridView()
        ' Clear existing columns if necessary
        orderDGV.Columns.Clear()

        ' Add columns to DataGridView
        orderDGV.Columns.Add("Code", "Code")
        orderDGV.Columns.Add("Quantity", "Quantity")
        orderDGV.Columns.Add("Order", "Order")
        orderDGV.Columns.Add("Price", "Price")
    End Sub


    Private Sub clear()
        prodName.Text = ""
        largePrice.Text = ""
        category.Text = ""
        subName.Text = ""
        productPicture.BackgroundImage = Nothing
    End Sub

    Private Sub getBestSeller()


        Try
            ' Open the connection
            Con.Open()

            ' SQL query to get top 3 best-sellers
            Dim sql As String = "SELECT p.productName, SUM(o.totalSold) as totalSold " &
                                "FROM product o " &
                                "JOIN product p ON o.id = p.id " &
                                "GROUP BY p.productName " &
                                "ORDER BY totalSold DESC " &
                                "LIMIT 3;"

            ' MySQL command
            Dim cmd As New MySqlCommand(sql, Con)

            ' Execute the command and read the results
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            ' Loop through the top 3 results
            If reader.HasRows Then
                Dim index As Integer = 1
                While reader.Read()
                    Select Case index
                        Case 1
                            lblProduct1.Text = reader("productName").ToString()
                        Case 2
                            lblProduct2.Text = reader("productName").ToString()
                        Case 3
                            lblProduct3.Text = reader("productName").ToString()
                    End Select
                    index += 1
                End While
            End If

            ' Close the reader
            reader.Close()
        Catch ex As MySqlException
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ' Close the connection
            Con.Close()
        End Try
    End Sub



    Private Sub populateEmployees()
        Try
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If

            Con.Open()
            Dim sql As String = "SELECT * FROM employees"
            Dim cmd As New MySqlCommand(sql, Con)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim builder As New MySqlCommandBuilder(adapter)
            Dim ds As New DataSet()
            adapter.Fill(ds, "employees")
            EmployeeDGV.DataSource = ds.Tables("employees")
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try
    End Sub

    Public Sub populateProducts()
        Try
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If

            Con.Open()
            Dim sql As String = "SELECT * FROM product"
            Dim cmd As New MySqlCommand(sql, Con)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim builder As New MySqlCommandBuilder(adapter)
            Dim ds As New DataSet()
            adapter.Fill(ds, "product")
            productDGV.DataSource = ds.Tables("product")
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try
    End Sub


    Private Sub displayProduct()
        Dim query As String = "SELECT productName FROM product"

        Try
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If

            Con.Open()
            Dim cmd As New MySqlCommand(query, Con)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            productCombobox.Items.Clear() ' Clear existing items before adding new ones
            While reader.Read()
                productCombobox.Items.Add(reader("productName").ToString())


            End While

            reader.Close() ' Ensure the reader is closed
            Con.Close()
        Catch ex As MySqlException
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try
    End Sub
    'Private Sub displayCategory()
    '    Dim query As String = "SELECT category FROM product"

    '    Try
    '        If Con.State = ConnectionState.Open Then
    '            Con.Close()
    '        End If

    '        Con.Open()
    '        Dim cmd As New MySqlCommand(query, Con)
    '        Dim reader As MySqlDataReader = cmd.ExecuteReader()

    '        categoryProd.Items.Clear() ' Clear existing items before adding new ones
    '        While reader.Read()
    '            categoryProd.Items.Add(reader("category").ToString())
    '        End While

    '        reader.Close() ' Ensure the reader is closed
    '        Con.Close()
    '    Catch ex As MySqlException
    '        MessageBox.Show("Error: " & ex.Message)
    '    Finally
    '        If Con.State = ConnectionState.Open Then
    '            Con.Close()
    '        End If
    '    End Try
    'End Sub




    Private Sub productCombobox_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedProduct As String = productCombobox.Text
        Dim query As String = "SELECT productName, small, medium, large FROM product WHERE productName = @productName"

        Try
            Con.Open()
            Dim cmd As New MySqlCommand(query, Con)
            cmd.Parameters.AddWithValue("@productName", selectedProduct)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                ' Convert the price from the appropriate column
                Dim smallPrice As Decimal
                If Decimal.TryParse(reader("small").ToString(), smallPrice) Then
                    price = smallPrice
                Else
                    ' Handle the case where conversion fails
                    price = 0
                End If

                Dim prod As String = reader("productName").ToString()
                product = prod
            End If

            RichTextBox1.Clear()
        Catch ex As MySqlException
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try
    End Sub



    Private Sub fetchProdPicture()
        Dim pictureBoxes() As PictureBox = {prod1, prod2, prod3, prod4, prod5, prod6, prod7, prod8, prod9, prod10, prod11, prod12, prod13, prod14, prod15}

        Try
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If

            Con.Open()
            Dim query = "SELECT productPicture FROM product"
            Dim cmd = New MySqlCommand(query, Con)
            Dim rd = cmd.ExecuteReader()

            Dim index As Integer = 0
            While rd.Read() AndAlso index < pictureBoxes.Length
                If Not rd.IsDBNull(0) Then
                    Dim pictureString As String = rd.GetString(0)
                    Dim pictureBytes() As Byte = Convert.FromBase64String(pictureString)
                    Using ms As New MemoryStream(pictureBytes)
                        pictureBoxes(index).Image = Image.FromStream(ms)
                        pictureBoxes(index).SizeMode = PictureBoxSizeMode.StretchImage
                    End Using
                End If
                index += 1
            End While

            rd.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try
    End Sub



    Private Sub fetchProduct1()
        Dim pLabels() As Label = {p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15}
        Dim prLabels() As Label = {pr1, pr2, pr3, pr4, pr5, pr6, pr7, pr8, pr9, pr10, pr11, pr12, pr13, pr14, pr15}
        Dim snLabels() As Label = {sn1, sn2, sn3, sn4, sn5, sn6, sn7, sn8, sn9, sn10, sn11, sn12, sn13, sn14, sn15}

        Try
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If

            Con.Open()
            Dim sql As String = "SELECT * FROM product"
            Dim cmd As New MySqlCommand(sql, Con)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim ds As New DataSet()
            adapter.Fill(ds, "product")

            If ds.Tables("product").Rows.Count > 0 Then
                For i As Integer = 0 To Math.Min(ds.Tables("product").Rows.Count - 1, 14)
                    ' Display product information for each set of controls
                    pLabels(i).Text = ds.Tables("product").Rows(i)("productName").ToString()
                    prLabels(i).Text = "₱" & ds.Tables("product").Rows(i)("small").ToString()
                    snLabels(i).Text = ds.Tables("product").Rows(i)("subName").ToString()
                Next
            End If
        Catch ex As MySqlException
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try
    End Sub



    Private lastCheckTime As DateTime = DateTime.MinValue ' Initialize the last check time

    Private Sub getDailyIncome()
        ' Calculate the date 1 day ago from now
        Dim dateOneDayAgo As DateTime = DateTime.Now.AddDays(-1)


        Dim query As String = "SELECT SUM(total) FROM sales_history WHERE date >= @date"

        Con.Open()

        Using command As New MySqlCommand(query, Con)
            ' Use the calculated dateOneDayAgo
            command.Parameters.AddWithValue("@date", dateOneDayAgo)

            Dim dailyIncome As Decimal = 0
            Dim result = command.ExecuteScalar()

            ' Check if the result is not DBNull
            If result IsNot DBNull.Value Then
                dailyIncome = Convert.ToDecimal(result)
            End If

            dailyEarning.Text = dailyIncome.ToString("C2") ' Format as currency

            ' Update lastCheckTime for future reference if needed
            lastCheckTime = DateTime.Now
        End Using

        Con.Close()
    End Sub
    Private Sub getWeeklyIncome()
        ' Calculate the date 1 day ago from now
        Dim dateOneDayAgo As DateTime = DateTime.Now.AddDays(-7)


        Dim query As String = "SELECT SUM(total) FROM sales_history WHERE date >= @date"

        Con.Open()

        Using command As New MySqlCommand(query, Con)
            ' Use the calculated dateOneDayAgo
            command.Parameters.AddWithValue("@date", dateOneDayAgo)

            Dim dailyIncome As Decimal = 0
            Dim result = command.ExecuteScalar()

            ' Check if the result is not DBNull
            If result IsNot DBNull.Value Then
                dailyIncome = Convert.ToDecimal(result)
            End If

            weeklyIncome.Text = dailyIncome.ToString("C2") ' Format as currency

            ' Update lastCheckTime for future reference if needed
            lastCheckTime = DateTime.Now
        End Using

        Con.Close()
    End Sub


    Private Sub getTotalIncome()
        Dim query As String = "SELECT SUM(total) FROM sales_history"


        Dim command As New MySqlCommand(query, Con)

        Try
            Con.Open()
            Dim result As Object = command.ExecuteScalar()
            Dim total As Decimal

            If result IsNot DBNull.Value Then
                total = Convert.ToDecimal(result)
                totalEarning.Text = total.ToString("C2")
            Else
                total = 0
            End If

            Con.Close()
        Catch ex As MySqlException
            MsgBox("Error: " & ex.Message)
        End Try

    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        populateProducts()
        displayProduct()
        fetchProduct1()
        fetchProdPicture()
        'populateSales()
        getTotalIncome()
        getDailyIncome()
        populateEmployees()
        getWeeklyIncome()
        'displayCategory()
        SetupDataGridView()
        getBestSeller()

    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs)

    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        TabControl1.SelectedTab = TabPage1
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Application.Exit()
    End Sub

    Private Sub TabPage1_Click_1(sender As Object, e As EventArgs)

    End Sub

    Private Sub Guna2CustomGradientPanel3_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Try
            With OpenFileDialog1
                .Filter = "Image Files|* .png;*.jpeg;*.jpg"
                .FilterIndex = 1
            End With
            OpenFileDialog1.FileName = ""
            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                productPicture.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        productPicture.BackgroundImage = Nothing
    End Sub





    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs)

    End Sub
    Function ImageToBase64(ByVal image As Image, ByVal format As System.Drawing.Imaging.ImageFormat) As String
        Dim ms As New MemoryStream
        image.Save(ms, format)
        Dim imageByte() As Byte = ms.ToArray()
        Dim base64String As String = Convert.ToBase64String(imageByte)
        Return base64String
    End Function

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If String.IsNullOrEmpty(productId) Then
            Dim myId As Guid = Guid.NewGuid()
            Dim myIdString = myId.ToString().Substring(0, 6)
            productId = myIdString
        End If

        Dim query As String = "INSERT INTO Product (id, productName, subName, category, small, medium, large  productPicture) " &
                      "VALUES (@id, @productName, @subName, @category,@small, @medium, @large, @productPicture)"

        Try
            Con.Open()
            Using cmd As New MySqlCommand(query, Con)
                cmd.Parameters.AddWithValue("@id", productId)
                cmd.Parameters.AddWithValue("@productName", prodName.Text.ToUpper())
                cmd.Parameters.AddWithValue("@subName", subName.Text.ToUpper())
                cmd.Parameters.AddWithValue("@category", category.Text.ToUpper())


                Dim price As Decimal
                If Decimal.TryParse(largePrice.Text, price) Then
                    cmd.Parameters.AddWithValue("@small", price)
                    cmd.Parameters.AddWithValue("@medium", price)
                    cmd.Parameters.AddWithValue("@large", price)
                Else
                    Throw New Exception("Invalid price format.")
                End If

                cmd.Parameters.AddWithValue("@productPicture", ImageToBase64(productPicture.BackgroundImage, System.Drawing.Imaging.ImageFormat.Png))

                cmd.ExecuteNonQuery()
            End Using

            MsgBox("Product inserted!")
            populateEmployees()
            populateProducts()
            fetchProduct1()
            fetchProdPicture()
            displayProduct()
            productPicture.BackgroundImage = Nothing
            prodName.Text = ""
            largePrice.Text = ""
            subName.Text = ""
            category.Text = ""
            productId = ""

        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)

        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try

    End Sub

    Private Sub productDGV_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles productDGV.CellContentClick

        Dim row As DataGridViewRow = Me.productDGV.Rows(e.RowIndex)

        Dim base64String As String = row.Cells("productPicture").Value.ToString()

        Dim imageBytes As Byte() = Convert.FromBase64String(base64String)
        Using ms As New MemoryStream(imageBytes)
            productPicture.BackgroundImage = Image.FromStream(ms)
        End Using

        productId = row.Cells("id").Value.ToString()
        prodName.Text = row.Cells("productName").Value.ToString()
        largePrice.Text = row.Cells("large").Value.ToString()
        smallPrice.Text = row.Cells("small").Value.ToString()
        mediumPrice.Text = row.Cells("medium").Value.ToString()
        category.Text = row.Cells("category").Value.ToString()
        subName.Text = row.Cells("subName").Value.ToString()



    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        If String.IsNullOrEmpty(productId) Then
            MsgBox("Please select a product")
        Else
            Try
                Con.Open()

                ' Ensure there is a space before WHERE clause in the SQL string
                Dim sql As String = "UPDATE product SET productName=@productName, small=@small, medium=@medium, large=@large, productPicture=@productPicture, subName=@subName, category=@category WHERE id=@id"

                Using cmds As New MySqlCommand(sql, Con)
                    cmds.Parameters.AddWithValue("@productName", prodName.Text.ToUpper())
                    cmds.Parameters.AddWithValue("@category", category.Text.ToUpper())
                    cmds.Parameters.AddWithValue("@subName", subName.Text.ToUpper())

                    ' Convert text to decimal values
                    Dim sp As Decimal
                    Dim mp As Decimal
                    Dim lp As Decimal

                    If Not Decimal.TryParse(smallPrice.Text, sp) Then
                        Throw New Exception("Invalid small price format.")
                    End If

                    If Not Decimal.TryParse(mediumPrice.Text, mp) Then
                        Throw New Exception("Invalid medium price format.")
                    End If

                    If Not Decimal.TryParse(largePrice.Text, lp) Then
                        Throw New Exception("Invalid large price format.")
                    End If

                    cmds.Parameters.AddWithValue("@small", sp)
                    cmds.Parameters.AddWithValue("@medium", mp)
                    cmds.Parameters.AddWithValue("@large", lp)

                    ' Handle image conversion
                    If productPicture.BackgroundImage IsNot Nothing Then
                        Dim base64String As String = ImageToBase64(productPicture.BackgroundImage, System.Drawing.Imaging.ImageFormat.Png)
                        cmds.Parameters.AddWithValue("@productPicture", base64String)
                    Else
                        cmds.Parameters.AddWithValue("@productPicture", DBNull.Value) ' Set to DBNull.Value if no image
                    End If

                    cmds.Parameters.AddWithValue("@id", productId)

                    cmds.ExecuteNonQuery()
                End Using

                MsgBox("Product updated!")
                populateProducts()
                fetchProduct1()
                fetchProdPicture()
                populateEmployees()

                ' Clear fields
                productPicture.BackgroundImage = Nothing
                prodName.Text = ""
                smallPrice.Text = ""
                mediumPrice.Text = ""
                largePrice.Text = ""
                subName.Text = ""
                category.Text = ""
                productId = ""

            Catch ex As Exception
                MsgBox("An error occurred: " & ex.Message)
            Finally
                ' Ensure the connection is always closed
                If Con.State = ConnectionState.Open Then
                    Con.Close()
                End If
            End Try
        End If



    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        If user.Text = "CASHIER" Then

            MsgBox("This module is not available for cashier")
        Else
            TabControl1.SelectedTab = TabPage4
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TabControl1.SelectedTab = TabPage3
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Application.Restart()
    End Sub



    Dim change As Decimal

    Private Sub GenerateReceipt()
        ' Clear existing content in RichTextBox1
        RichTextBox1.Clear()

        ' Add receipt header
        RichTextBox1.AppendText("------ Mr. Browsko ------" & vbCrLf)
        RichTextBox1.AppendText("Taysan, San Jose Batangas" & vbCrLf)
        RichTextBox1.AppendText("09657010746" & vbCrLf)
        RichTextBox1.AppendText("Receipt" & vbCrLf)
        RichTextBox1.AppendText("Cashier: " & userName.Text & vbCrLf)
        RichTextBox1.AppendText("-----------------------------" & vbCrLf)

        ' Add items from the orderDGV to the receipt
        For Each row As DataGridViewRow In orderDGV.Rows
            If Not row.IsNewRow Then
                Dim quantity As String = row.Cells("Quantity").Value.ToString()
                Dim order As String = row.Cells("Order").Value.ToString()
                Dim price As String = row.Cells("Price").Value.ToString()

                ' Format and append the item details
                RichTextBox1.AppendText(quantity & " x " & order & " - ₱" & price & vbCrLf)
            End If
        Next

        ' Add total and footer
        RichTextBox1.AppendText("-----------------------------" & vbCrLf)
        RichTextBox1.AppendText("Total: ₱" & totalBill.Text & vbCrLf)
        RichTextBox1.AppendText("Received Amount: ₱" & receivedAmount.Text & vbCrLf)
        RichTextBox1.AppendText("Change: ₱" & change & vbCrLf)
        RichTextBox1.AppendText("-----------------------------" & vbCrLf)
        RichTextBox1.AppendText("Thank you for shopping with us!" & vbCrLf)

        ' Optionally adjust font and alignment
        Dim font As New Font("Arial", 10)
        RichTextBox1.SelectAll()
        RichTextBox1.SelectionFont = font
        RichTextBox1.SelectionAlignment = HorizontalAlignment.Center
    End Sub




    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            If employeeID = "" Then
                MsgBox("Please select an product to be deleted!")
            Else
                ' Ensure the connection is closed before opening it
                If Con.State = ConnectionState.Open Then
                    Con.Close()
                End If

                Con.Open()


                Dim deleteQuery As String = "DELETE FROM product WHERE id = @Id"
                Using cmd As New MySqlCommand(deleteQuery, Con)
                    cmd.Parameters.AddWithValue("@Id", employeeID)
                    cmd.ExecuteNonQuery()
                    MsgBox("product deleted")
                    populateEmployees()
                End Using
                productPicture.BackgroundImage = Nothing
                prodName.Text = ""
                largePrice.Text = ""
                subName.Text = ""
                category.Text = ""
                productId = ""
                populateProducts()
                ' Close the connection after using it
                Con.Close()
            End If
        Catch ex As Exception
            MsgBox("Error removing: " & ex.Message)
        Finally
            ' Ensure the connection is closed in the Finally block
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try


    End Sub

    Public Property newPrice As Integer


    Private Sub ShowProductForm(productTextBox As Label, codeTextbox As Label)


        Dim prod As New Form3()

        prod.order.Text = productTextBox.Text
        'prod.price.Text = priceTextBox.Text
        prod.codeName.Text = codeTextbox.Text

        Dim newProd As String = prod.order.Text
        Dim newPrice As Integer

        prod.order.Text = newProd
        prod.Show()
    End Sub


    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click

        'Me.Opacity = 0.8

        'Dim prod As New Form3()

        'prod.order.Text = p1.Text
        'prod.price.Text = pr1.Text

        'Dim newProd As String = prod.order.Text
        'newPrice = prod.price.Text



        'prod.price.Text = newPrice
        'prod.order.Text = newProd
        'prod.newPrice = newPrice

        'prod.Show()


        ShowProductForm(p1, sn1)


    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click

        ShowProductForm(p2, sn2)
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        ShowProductForm(p3, sn3)
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        ShowProductForm(p4, sn4)
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        ShowProductForm(p5, sn5)
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        ShowProductForm(p6, sn6)
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        ShowProductForm(p7, sn7)
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        ShowProductForm(p8, sn8)
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        ShowProductForm(p9, sn9)
    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        ShowProductForm(p10, sn10)
    End Sub


    Dim g, mg As Graphics
    Dim bmp As Bitmap


    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        e.Graphics.DrawImage(bmp, 0, 0)
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        If user.Text = "CASHIER" Then

            MsgBox("This module is not available for cashier")
        Else
            TabControl1.SelectedTab = TabPage2
        End If


    End Sub

    Private Sub Guna2CustomGradientPanel15_Paint(sender As Object, e As PaintEventArgs) Handles Guna2CustomGradientPanel15.Paint

    End Sub

    Private Sub Receipt()
        bmp = New Bitmap(RichTextBox1.Width, RichTextBox1.Height)
        mg = Graphics.FromImage(bmp)
        RichTextBox1.DrawToBitmap(bmp, New Rectangle(0, 0, RichTextBox1.Width, RichTextBox1.Height))

        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.ShowDialog()

        ' Clear the form fields

        productCombobox.Text = ""
        prodQuantity.Text = ""
        totalBill.Text = ""
        receivedAmount.Text = ""
        totalBalanceForm2 = 0
        totalBalanceForm3 = 0
        totalBalance = 0
        RichTextBox1.Clear()


    End Sub




    Private Sub Guna2Button7_Click(sender As Object, e As EventArgs) Handles Guna2Button7.Click
        If orderDGV.RowCount = 0 Then
            MsgBox("Please add order before confirming order!")
        Else
            Dim totalBillValue As Decimal
            Dim receivedAmountValue As Decimal

            ' Convert text to numeric values
            If Decimal.TryParse(totalBill.Text, totalBillValue) AndAlso Decimal.TryParse(receivedAmount.Text, receivedAmountValue) Then
                If receivedAmountValue >= totalBillValue Then
                    Con.Close()
                    GenerateReceipt()
                    Con.Open()

                    Try

                        ' Loop through each row in the DataGridView (orderDGV)
                        For Each row As DataGridViewRow In orderDGV.Rows
                            ' Ensure the row is not a new row
                            If Not row.IsNewRow Then
                                ' Get productName, quantity, and total from the DataGridView
                                Dim productName As String = row.Cells("Order").Value.ToString()
                                Dim quantity As Integer = Convert.ToInt32(row.Cells("Quantity").Value)
                                Dim total As Decimal = Convert.ToDecimal(row.Cells("Price").Value)

                                ' Insert into sales_history table
                                Dim insertQuery As String = "INSERT INTO sales_history (productName, total, date) VALUES (@productName, @total, NOW())"
                                Using cmdInsert As New MySqlCommand(insertQuery, Con)
                                    cmdInsert.Parameters.AddWithValue("@productName", productName.ToUpper())
                                    cmdInsert.Parameters.AddWithValue("@total", total)
                                    cmdInsert.ExecuteNonQuery()
                                End Using

                                ' Update totalSold in the product table
                                Dim updateQuery As String = "UPDATE product SET totalSold = totalSold + @quantity WHERE productName = @productName"
                                Using cmdUpdate As New MySqlCommand(updateQuery, Con)
                                    cmdUpdate.Parameters.AddWithValue("@quantity", quantity)
                                    cmdUpdate.Parameters.AddWithValue("@productName", productName.ToUpper())
                                    cmdUpdate.ExecuteNonQuery()

                                End Using
                            End If
                        Next

                        Con.Close()
                        orderDGV.Rows.Clear()
                        getBestSeller()
                        Receipt()
                        getTotalIncome()
                        getDailyIncome()
                        getWeeklyIncome()
                        prodCode.Text = ""

                    Catch ex As Exception
                        MsgBox("An error occurred: " & ex.Message)
                    Finally

                        If Con.State = ConnectionState.Open Then
                            Con.Close()
                        End If
                    End Try

                Else
                    MsgBox("Invalid amount")
                End If
            Else
                MsgBox("Invalid amount format.")
            End If
        End If


    End Sub

    Private Sub UpdateTotalBalanceAfterDeletion()
        Dim totalBalance As Decimal = 0

        ' Get the reference to Form2
        Dim orderForm As Form2 = CType(Application.OpenForms("Form2"), Form2)

        ' Iterate through the rows in orderDGV
        For Each row As DataGridViewRow In orderForm.orderDGV.Rows
            If Not row.IsNewRow Then
                Dim price As Decimal
                Dim quantity As Integer

                ' Try to parse the Price and Quantity cells
                If Decimal.TryParse(row.Cells("Price").Value.ToString(), price) AndAlso
               Integer.TryParse(row.Cells("Quantity").Value.ToString(), quantity) Then
                    ' Calculate total for the row and add to totalBalance
                    totalBalance += price * quantity
                End If
            End If
        Next

        ' Update the total balance form3 directly from the calculated totalBalance
        orderForm.totalBalanceForm3 = totalBalance

        ' Calculate and update the totalBill
        Dim newTotalBill As Decimal = orderForm.totalBalance + orderForm.totalBalanceForm2 + orderForm.totalBalanceForm3
        orderForm.totalBill.Text = newTotalBill.ToString("F2")
    End Sub



    Private Sub Guna2Button5_Click(sender As Object, e As EventArgs) Handles Guna2Button5.Click

        Dim orderForm As Form2 = CType(Application.OpenForms("Form2"), Form2)


        If orderForm.orderDGV.SelectedRows.Count > 0 Then

            Dim selectedRow As DataGridViewRow = orderForm.orderDGV.SelectedRows(0)


            orderForm.orderDGV.Rows.Remove(selectedRow)





            MessageBox.Show("Order removed successfully.")
            UpdateTotalBalanceAfterDeletion()

            prodCode.Text = ""

        End If

    End Sub


    Public Property totalBalanceForm2 As Integer = 0
    Public Property totalBalanceForm3 As Integer = 0
    Public Property totalBalance As Integer = 0
    Dim price As Integer

    Private Sub Guna2Button6_Click(sender As Object, e As EventArgs) Handles Guna2Button6.Click
        Dim quantity As Integer

        ' Convert the quantity from the textbox
        If Not Integer.TryParse(prodQuantity.Text, quantity) OrElse quantity <= 0 Then
            MsgBox("Please enter a valid quantity.")
            Return
        End If

        ' Assume 'price' is a valid Decimal variable defined elsewhere in your code
        If price <= 0 Then
            MsgBox("Please select a product with a valid price")
            Return
        End If

        ' Calculate current bill
        Dim res As Decimal = price * quantity
        totalBalanceForm2 += res

        ' Calculate the total bill
        Dim res2 As Decimal = totalBalanceForm2 + totalBalanceForm3 + totalBalance
        totalBill.Text = res2.ToString()


        Dim newRow As DataGridViewRow = CType(orderDGV.Rows(orderDGV.Rows.Add()), DataGridViewRow)

        ' Set the values for the new row
        newRow.Cells("Quantity").Value = quantity
        newRow.Cells("Order").Value = productCombobox.Text
        newRow.Cells("Price").Value = price.ToString()
        newRow.Cells("Code").Value = prodCode.Text

        ' Clear the input fields for the next entry
        product = ""
        productCombobox.SelectedIndex = -1 ' Clear selection
        prodQuantity.Text = ""
        prodCode.Text = ""

    End Sub
    Private Sub productCombobox_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles productCombobox.SelectedIndexChanged
        Dim selectedProduct As String = productCombobox.Text
        Dim query As String = "SELECT productName, small, medium, large, subName FROM product WHERE productName = @productName"

        Try
            Con.Open()
            Dim cmd As New MySqlCommand(query, Con)
            cmd.Parameters.AddWithValue("@productName", selectedProduct)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                ' Ensure the price is converted to Decimal if it's a decimal value
                Dim smallPrice As Decimal
                If Decimal.TryParse(reader("small").ToString(), smallPrice) Then
                    price = smallPrice
                Else
                    ' Handle conversion error if needed
                    price = 0
                End If

                Dim prod As String = reader("productName").ToString()
                prodCode.Text = reader("subName").ToString()
                product = prod
            Else
                ' Handle the case when no data is found if needed
            End If

            RichTextBox1.Clear()
        Catch ex As MySqlException
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try
    End Sub



    Private Sub Label39_Click(sender As Object, e As EventArgs) Handles sn10.Click

    End Sub

    Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Button26.Click
        ShowProductForm(p11, sn11)
    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs) Handles Button25.Click
        ShowProductForm(p12, sn12)
    End Sub

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        ShowProductForm(p13, sn13)
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        ShowProductForm(p14, sn14)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ShowProductForm(p15, sn15)
    End Sub

    Dim employeeID As String
    Private Sub Button29_Click(sender As Object, e As EventArgs) Handles Button29.Click
        If String.IsNullOrEmpty(employeeID) Then
            Dim myId As Guid = Guid.NewGuid()
            Dim myIdString = myId.ToString().Substring(0, 6)
            employeeID = myIdString
        End If

        Dim query As String = "INSERT INTO employees (id, email, password, type, name, address, contactNumber) " &
                      "VALUES (@id, @email, @password, @type, @name, @address, @contactNumber)"

        Try
            Con.Open()
            Using cmd As New MySqlCommand(query, Con)
                cmd.Parameters.AddWithValue("@id", employeeID)
                cmd.Parameters.AddWithValue("@email", email.Text.ToUpper())
                cmd.Parameters.AddWithValue("@password", password.Text.ToUpper())
                cmd.Parameters.AddWithValue("@type", type.Text.ToUpper())
                cmd.Parameters.AddWithValue("@name", employeeName.Text.ToUpper())
                cmd.Parameters.AddWithValue("@address", address.Text.ToUpper())
                cmd.Parameters.AddWithValue("@contactNumber", contactNumber.Text.ToUpper())
                cmd.ExecuteNonQuery()
            End Using

            MsgBox("added successfully!")
            populateEmployees()
            employeeID = ""
            email.Text = ""
            password.Text = ""
            type.Text = ""
            employeeName.Text = ""
            address.Text = ""
            contactNumber.Text = ""


        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)

        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try
    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        If employeeID = "" Then
            MsgBox("please select employee")
        Else

            Try
                Con.Open()

                Dim sql As String = "UPDATE employees SET email=@email, password=@password, type=@type , name=@name, address=@address, contactNumber=@contactNumber " &
                                    "WHERE id=@id"

                Using cmds As New MySqlCommand(sql, Con)
                    cmds.Parameters.AddWithValue("@id", employeeID)
                    cmds.Parameters.AddWithValue("@email", email.Text.ToUpper())
                    cmds.Parameters.AddWithValue("@password", password.Text.ToUpper())
                    cmds.Parameters.AddWithValue("@type", type.Text.ToUpper())
                    cmds.Parameters.AddWithValue("@name", employeeName.Text.ToUpper())
                    cmds.Parameters.AddWithValue("@address", address.Text.ToUpper())
                    cmds.Parameters.AddWithValue("@contactNumber", contactNumber.Text.ToUpper())
                    cmds.ExecuteNonQuery()
                End Using
                MsgBox("Employee updated!")
                Con.Close()
                populateEmployees()
            Catch ex As Exception
                MsgBox("An error occurred: " & ex.Message)
            Finally
                Con.Close()
            End Try
        End If
    End Sub

    Private Sub EmployeeDGV_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles EmployeeDGV.CellContentClick
        Dim row As DataGridViewRow = Me.EmployeeDGV.Rows(e.RowIndex)
        employeeID = row.Cells("id").Value.ToString()
        email.Text = row.Cells("email").Value.ToString()
        password.Text = row.Cells("password").Value.ToString()
        employeeName.Text = row.Cells("name").Value.ToString()
        type.Text = row.Cells("type").Value.ToString()
        address.Text = row.Cells("address").Value.ToString()
        contactNumber.Text = row.Cells("contactNumber").Value.ToString()


    End Sub

    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Button27.Click
        Try
            If employeeID = "" Then
                MsgBox("Please select an item to be deleted!")
            Else
                ' Ensure the connection is closed before opening it
                If Con.State = ConnectionState.Open Then
                    Con.Close()
                End If

                Con.Open()


                Dim deleteQuery As String = "DELETE FROM employees WHERE id = @Id"
                Using cmd As New MySqlCommand(deleteQuery, Con)
                    cmd.Parameters.AddWithValue("@Id", employeeID)
                    cmd.ExecuteNonQuery()
                    MsgBox("Item deleted")
                    populateEmployees()
                End Using
                employeeID = ""
                email.Text = ""
                password.Text = ""
                type.Text = ""
                employeeName.Text = ""
                address.Text = ""
                contactNumber.Text = ""
                ' Close the connection after using it
                Con.Close()
            End If
        Catch ex As Exception
            MsgBox("Error removing: " & ex.Message)
        Finally
            ' Ensure the connection is closed in the Finally block
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try


    End Sub

    Private Sub TabPage1_Click_2(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub




    Private Sub CalculateChange()
        Dim totalBillValue As Decimal = 0D
        Dim receivedAmountValue As Decimal = 0D

        ' Try to parse the text values to decimals
        Decimal.TryParse(totalBill.Text, totalBillValue)
        Decimal.TryParse(receivedAmount.Text, receivedAmountValue)

        ' Calculate the change
        change = receivedAmountValue - totalBillValue
        Customerchange.Text = change

    End Sub

    Private Sub receivedAmount_TextChanged(sender As Object, e As EventArgs) Handles receivedAmount.TextChanged
        CalculateChange()
    End Sub




    Private Sub customerOrder_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles orderDGV.CellContentClick
        orderDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect



    End Sub
End Class