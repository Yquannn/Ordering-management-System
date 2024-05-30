Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports System.Runtime.Intrinsics.X86

Public Class Form2

    Dim connection As String = "server=127.0.0.1; user=root; database=ordering_management_system; password="
    Dim Con As New MySqlConnection(connection)
    Dim productId = ""
    Public Property product As String

    Private Sub clear()
        prodName.Text = ""
        prodPrice.Text = ""
        productPicture.BackgroundImage = Nothing
    End Sub

    Public Sub getPicture()


    End Sub



    Private Sub displayProduct()
        Dim query As String = "SELECT productName, productPrice FROM product"

        Try
            Con.Open()
            Dim cmd As New MySqlCommand(query, Con)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            While reader.Read()
                productCombobox.Items.Add(reader("productName").ToString())
                product = reader("productName").ToString()
            End While

        Catch ex As MySqlException
            MessageBox.Show("Error: " & ex.Message)
        Finally
            Con.Close()
        End Try
    End Sub


    Private Sub productCombobox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles productCombobox.SelectedIndexChanged
        Dim selectedProduct As String = productCombobox.Text.ToString()
        Dim query As String = "SELECT productName, productPrice FROM product WHERE productName = @productName"

        Try
            Con.Open()
            Dim cmd As New MySqlCommand(query, Con)
            cmd.Parameters.AddWithValue("@productName", selectedProduct)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                price = Convert.ToInt32(reader("productPrice"))
                Dim prod As String = reader("productName").ToString()
                product = prod
            End If
            RichTextBox1.Clear()
        Catch ex As MySqlException
            MessageBox.Show("Error: " & ex.Message)
        Finally
            Con.Close()
        End Try

    End Sub

    Public Sub populateProducts()
        Con.Open()
        Dim sql As String = "SELECT * FROM product"
        Dim cmd As New MySqlCommand(sql, Con)
        Dim adapter As New MySqlDataAdapter(cmd)
        Dim builder As New MySqlCommandBuilder(adapter)
        Dim ds As New DataSet()
        adapter.Fill(ds, "product")
        productDGV.DataSource = ds.Tables("product")
        Con.Close()
    End Sub
    'Public Sub populateSales()
    '    Con.Open()
    '    Dim sql As String = "SELECT * FROM sales_history"
    '    Dim cmd As New MySqlCommand(sql, Con)
    '    Dim adapter As New MySqlDataAdapter(cmd)
    '    Dim builder As New MySqlCommandBuilder(adapter)
    '    Dim ds As New DataSet()
    '    adapter.Fill(ds, "sales_history")
    '    salesHistoryDVG.DataSource = ds.Tables("sales_history")
    '    Con.Close()
    'End Sub


    Private Sub fetchProdPicture()
        Dim pictureBoxes() As PictureBox = {prod1, prod2, prod3, prod4, prod5, prod6, prod7, prod8, prod9, prod10}

        Con.Open()
        Dim query = "SELECT productPicture FROM product"
        Dim cmd = New MySqlCommand(query, Con)
        Dim rd = cmd.ExecuteReader()

        Try
            Dim index As Integer = 0
            While rd.Read() AndAlso index < pictureBoxes.Length
                If Not rd.IsDBNull(0) Then
                    Dim pictureString As String = rd.GetString(0)
                    Dim pictureBytes() As Byte = Convert.FromBase64String(pictureString)
                    Dim ms As New MemoryStream(pictureBytes)
                    pictureBoxes(index).Image = Image.FromStream(ms)
                    pictureBoxes(index).SizeMode = PictureBoxSizeMode.StretchImage
                End If
                index += 1
            End While

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            Con.Close()
        End Try
    End Sub


    Private Sub fetchProduct1()

        Dim pLabels() As Label = {p1, p2, p3, p4, p5, p6, p7, p8, p9, p10}
        Dim prLabels() As Label = {pr1, pr2, pr3, pr4, pr5, pr6, pr7, pr8, pr9, pr10}

        Try
            Con.Open()
            Dim sql As String = "SELECT * FROM product"
            Dim cmd As New MySqlCommand(sql, Con)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim ds As New DataSet()
            adapter.Fill(ds, "product")

            If ds.Tables("product").Rows.Count > 0 Then
                For i As Integer = 0 To Math.Min(ds.Tables("product").Rows.Count - 1, 9) ' Loop up to 10 rows or until the end of dataset
                    ' Display product information for each set of controls
                    pLabels(i).Text = ds.Tables("product").Rows(i)("productName").ToString()

                    prLabels(i).Text = "₱" & ds.Tables("product").Rows(i)("productPrice").ToString()
                Next
            End If
        Catch ex As MySqlException
            MessageBox.Show("Error: " & ex.Message)
        Finally
            Con.Close()
        End Try


    End Sub

    Private lastCheckTime As DateTime = DateTime.MinValue ' Initialize the last check time

    Private Sub getDailyIncome()
        ' Calculate the date 1 day ago from now
        Dim dateOneDayAgo As DateTime = DateTime.Now.AddDays(-1)

        ' Corrected the SQL query
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



    Private Sub Guna2GradientPanel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2GradientPanel1.Paint

    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles prodQuantity.TextChanged

    End Sub
    Function ImageToBase64(ByVal image As Image, ByVal format As System.Drawing.Imaging.ImageFormat) As String
        Dim ms As New MemoryStream
        image.Save(ms, format)
        Dim imageByte() As Byte = ms.ToArray()
        Dim base64String As String = Convert.ToBase64String(imageByte)
        Return base64String
    End Function

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If productId = "" Then
            Dim myId As Guid = Guid.NewGuid()
            Dim myIdString = myId.ToString().Substring(0, 6)
            productId = myIdString
        End If

        Try
            Dim query As String = "INSERT INTO Product (id, productName, productPrice, productPicture) VALUES (@id, @productName, @productPrice, @productPicture)"

            Con.Open()
            Using cmd As New MySqlCommand(query, Con)
                cmd.Parameters.AddWithValue("@id", productId)
                cmd.Parameters.AddWithValue("@productName", prodName.Text.ToUpper)
                Dim price As Decimal
                If Decimal.TryParse(prodPrice.Text, price) Then
                    cmd.Parameters.AddWithValue("@productPrice", price)
                Else
                    Throw New Exception("Invalid price format.")
                End If

                cmd.Parameters.AddWithValue("@productPicture", ImageToBase64(productPicture.BackgroundImage, System.Drawing.Imaging.ImageFormat.Png))


                cmd.ExecuteNonQuery()
            End Using
            MsgBox("Product inserted!")
            Con.Close()
            populateProducts()
            fetchProduct1()
            fetchProdPicture()
            displayProduct()
            productPicture.BackgroundImage = Nothing
            prodName.Text = ""
            prodPrice.Text = ""
            productId = ""


        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        Finally
            Con.Close()
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
        prodPrice.Text = row.Cells("productPrice").Value.ToString()



    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        If productId = "" Then
            MsgBox("please select product")
        Else

            Try
                Con.Open()

                Dim sql As String = "UPDATE product SET productName=@productName, productPrice=@productPrice, productPicture=@productPicture " &
                                    "WHERE id=@id"

                Using cmds As New MySqlCommand(sql, Con)
                    cmds.Parameters.AddWithValue("@productName", prodName.Text.ToUpper())

                    Dim price As Decimal
                    If Decimal.TryParse(prodPrice.Text, price) Then
                        cmds.Parameters.AddWithValue("@productPrice", price)
                    Else
                        Throw New Exception("Invalid price format.")
                    End If

                    If productPicture.BackgroundImage IsNot Nothing Then
                        Dim base64String As String = ImageToBase64(productPicture.BackgroundImage, System.Drawing.Imaging.ImageFormat.Png)
                        cmds.Parameters.AddWithValue("@productPicture", base64String)
                    Else
                        Throw New Exception("Product picture is not set.")
                    End If

                    cmds.Parameters.AddWithValue("@id", productId)

                    cmds.ExecuteNonQuery()
                End Using
                MsgBox("Product updated!")
                Con.Close()
                populateProducts()
                fetchProduct1()
                fetchProdPicture()
                productPicture.BackgroundImage = Nothing
                prodName.Text = ""
                prodPrice.Text = ""
                productId = ""
            Catch ex As Exception
                MsgBox("An error occurred: " & ex.Message)
            Finally
                Con.Close()
            End Try
        End If


    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TabControl1.SelectedTab = TabPage4
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TabControl1.SelectedTab = TabPage3
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Hide()
        Form1.Show()
    End Sub
    Public Property totalBalanceForm2 As Integer = 0
    Public Property totalBalanceForm3 As Integer = 0
    Public Property totalBalance As Integer = 0
    Dim price As Integer
    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Dim quantity As Integer

        If productCombobox.Text = "" OrElse Not Integer.TryParse(prodQuantity.Text, quantity) Then
            MsgBox("Please complete the details with valid values")
            Return
        End If

        If price <= 0 Then
            MsgBox("Please select a product with a valid price")
            Return
        End If

        ' Calculate current bill
        Dim res = price * quantity
        totalBalanceForm2 += res

        Dim res2 = totalBalanceForm2 + totalBalanceForm3 + totalBalance
        totalBill.Text = res2

        ' Append details to RichTextBox
        RichTextBox2.AppendText("(x" & quantity.ToString() & ") " & product & " ₱" & price.ToString() & Environment.NewLine)

        product = ""
        productCombobox.SelectedIndex = -1 ' Clear selection
        prodQuantity.Text = ""
    End Sub





    Private Sub GenerateReceipt()
        ' Clear existing content in RichTextBox1
        RichTextBox1.Clear()

        ' Add receipt header
        RichTextBox1.AppendText("------ Mr. Browsko ------" & vbCrLf)
        RichTextBox1.AppendText("Receipt" & vbCrLf)
        RichTextBox1.AppendText("-----------------------------" & vbCrLf)

        ' Add items to the receipt (sample items)
        RichTextBox1.AppendText(RichTextBox2.Text) ' Append items from RichTextBox2

        ' Add total and footer
        RichTextBox1.AppendText("-----------------------------" & vbCrLf)
        RichTextBox1.AppendText("Total: ₱" & totalBill.Text & vbCrLf)
        RichTextBox1.AppendText("-----------------------------" & vbCrLf)
        RichTextBox1.AppendText("Thank you for shopping with us!" & vbCrLf)

        ' Set font size and style for the entire receipt
        Dim font As New Font("Arial", 10)
        RichTextBox1.SelectAll()
        RichTextBox1.SelectionFont = font

        ' Optionally adjust alignment and other formatting as needed
        RichTextBox1.SelectionAlignment = HorizontalAlignment.Center
    End Sub


    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click

        If RichTextBox2.TextLength = 0 Then
            MsgBox("please add order before confirming order!")
        Else
            GenerateReceipt()

            Try
                Dim query As String = "INSERT INTO sales_history (productName, total, date) VALUES (@productName, @total, NOW())"

                Con.Open()

                Using cmd As New MySqlCommand(query, Con)
                    ' Add parameters with the provided values
                    cmd.Parameters.AddWithValue("@productName", productCombobox.Text.ToUpper())
                    cmd.Parameters.AddWithValue("@total", totalBill.Text.ToString())

                    ' Execute the query
                    cmd.ExecuteNonQuery()
                    Con.Close()
                End Using

                ' Reset input fields

                getTotalIncome()
                getDailyIncome()
            Catch ex As Exception
                ' Display an error message if an exception occurs
                MsgBox("An error occurred: " & ex.Message)
            Finally
                ' Ensure the connection is closed even if an exception occurs
                If Con.State = ConnectionState.Open Then
                    Con.Close()
                End If
            End Try
        End If




    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            If productId = "" Then
                MsgBox("Please select an item to be deleted!")
            Else

                Con.Open()
                Dim deleteQuery As String = "DELETE FROM product WHERE id = @Id"
                Using cmd1 As New MySqlCommand(deleteQuery, Con)
                    cmd1.Parameters.AddWithValue("@Id", productId)
                    cmd1.ExecuteNonQuery()
                    MsgBox("Item deleted")
                    fetchProduct1()
                    fetchProdPicture()
                    displayProduct()
                    populateProducts()
                    Con.Close()
                End Using

                ' Reset fields after successful deletion
                productId.Text = ""
                productPicture.BackgroundImage = Nothing
                prodName.Text = ""
                prodPrice.Text = ""
            End If
        Catch ex As Exception
            MsgBox("Error removing: " & ex.Message)
        End Try
        Con.Close()

    End Sub

    Public Property newPrice As Integer


    Private Sub ShowProductForm(productTextBox As Label, priceTextBox As Label)
        Me.Opacity = 0.8

        Dim prod As New Form3()

        prod.order.Text = productTextBox.Text
        prod.price.Text = priceTextBox.Text

        Dim newProd As String = prod.order.Text
        Dim newPrice As Integer

        ' Try to parse the price text to an integer, removing any currency symbols if necessary
        If Integer.TryParse(priceTextBox.Text.Replace("₱", "").Trim(), newPrice) Then
            prod.price.Text = newPrice.ToString()
            prod.newPrice = newPrice
        Else
            MessageBox.Show("Invalid price format")
            Return
        End If

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


        ShowProductForm(p1, pr1)


    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click

        ShowProductForm(p2, pr2)
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        ShowProductForm(p3, pr3)
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        ShowProductForm(p4, pr4)
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        ShowProductForm(p5, pr5)
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        ShowProductForm(p6, pr6)
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        ShowProductForm(p7, pr7)
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        ShowProductForm(p8, pr8)
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        ShowProductForm(p9, pr9)
    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        ShowProductForm(p10, pr10)
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        RichTextBox2.Clear()
        RichTextBox1.Clear()
        totalBalanceForm2 = 0
        totalBalanceForm3 = 0
        totalBalance = 0
        totalBill.Text = 0
    End Sub
    Dim g, mg As Graphics
    Dim bmp As Bitmap
    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        bmp = New Bitmap(RichTextBox1.Width, RichTextBox1.Height)

        mg = Graphics.FromImage(bmp)

        RichTextBox1.DrawToBitmap(bmp, New Rectangle(0, 0, RichTextBox1.Width, RichTextBox1.Height))

        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.ShowDialog()
        RichTextBox2.Clear()
        productCombobox.Text = ""
        prodQuantity.Text = ""
        totalBill.Text = ""
        totalBalanceForm2 = 0
        totalBalanceForm3 = 0
        totalBalance = 0
        totalBill.Text = 0
        RichTextBox1.Clear()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        e.Graphics.DrawImage(bmp, 0, 0)
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        TabControl1.SelectedTab = TabPage2
    End Sub
End Class