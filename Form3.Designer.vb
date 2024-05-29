<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Guna2Button2 = New Guna.UI2.WinForms.Guna2Button()
        prodQuantity = New TextBox()
        Label37 = New Label()
        Guna2GradientPanel2 = New Guna.UI2.WinForms.Guna2GradientPanel()
        totalPrice = New Label()
        Label3 = New Label()
        Label2 = New Label()
        price = New Label()
        Label1 = New Label()
        order = New TextBox()
        Button9 = New Button()
        Guna2GradientPanel2.SuspendLayout()
        SuspendLayout()
        ' 
        ' Guna2Button2
        ' 
        Guna2Button2.BackColor = Color.Transparent
        Guna2Button2.BorderRadius = 10
        Guna2Button2.CustomizableEdges = CustomizableEdges1
        Guna2Button2.DisabledState.BorderColor = Color.DarkGray
        Guna2Button2.DisabledState.CustomBorderColor = Color.DarkGray
        Guna2Button2.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        Guna2Button2.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        Guna2Button2.FillColor = Color.FromArgb(CByte(255), CByte(128), CByte(0))
        Guna2Button2.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
        Guna2Button2.ForeColor = Color.Black
        Guna2Button2.Location = New Point(48, 191)
        Guna2Button2.Name = "Guna2Button2"
        Guna2Button2.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        Guna2Button2.Size = New Size(259, 36)
        Guna2Button2.TabIndex = 26
        Guna2Button2.Text = "Add order"
        ' 
        ' prodQuantity
        ' 
        prodQuantity.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point)
        prodQuantity.Location = New Point(132, 70)
        prodQuantity.Name = "prodQuantity"
        prodQuantity.Size = New Size(91, 27)
        prodQuantity.TabIndex = 25
        ' 
        ' Label37
        ' 
        Label37.AutoSize = True
        Label37.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
        Label37.Location = New Point(21, 72)
        Label37.Name = "Label37"
        Label37.Size = New Size(73, 21)
        Label37.TabIndex = 27
        Label37.Text = "Quantity:"
        ' 
        ' Guna2GradientPanel2
        ' 
        Guna2GradientPanel2.BackColor = Color.Transparent
        Guna2GradientPanel2.BorderRadius = 25
        Guna2GradientPanel2.Controls.Add(totalPrice)
        Guna2GradientPanel2.Controls.Add(Label3)
        Guna2GradientPanel2.Controls.Add(Label2)
        Guna2GradientPanel2.Controls.Add(price)
        Guna2GradientPanel2.Controls.Add(Label1)
        Guna2GradientPanel2.Controls.Add(Guna2Button2)
        Guna2GradientPanel2.Controls.Add(order)
        Guna2GradientPanel2.Controls.Add(Label37)
        Guna2GradientPanel2.Controls.Add(prodQuantity)
        Guna2GradientPanel2.CustomBorderColor = Color.FromArgb(CByte(255), CByte(128), CByte(0))
        Guna2GradientPanel2.CustomizableEdges = CustomizableEdges3
        Guna2GradientPanel2.FillColor = Color.PeachPuff
        Guna2GradientPanel2.FillColor2 = Color.Snow
        Guna2GradientPanel2.GradientMode = Drawing2D.LinearGradientMode.ForwardDiagonal
        Guna2GradientPanel2.Location = New Point(36, 41)
        Guna2GradientPanel2.Name = "Guna2GradientPanel2"
        Guna2GradientPanel2.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        Guna2GradientPanel2.Size = New Size(342, 243)
        Guna2GradientPanel2.TabIndex = 28
        ' 
        ' totalPrice
        ' 
        totalPrice.AutoSize = True
        totalPrice.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
        totalPrice.Location = New Point(166, 156)
        totalPrice.Name = "totalPrice"
        totalPrice.Size = New Size(19, 21)
        totalPrice.TabIndex = 33
        totalPrice.Text = "0"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
        Label3.Location = New Point(115, 161)
        Label3.Name = "Label3"
        Label3.Size = New Size(36, 15)
        Label3.TabIndex = 32
        Label3.Text = "Total:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
        Label2.Location = New Point(21, 119)
        Label2.Name = "Label2"
        Label2.Size = New Size(47, 21)
        Label2.TabIndex = 31
        Label2.Text = "Price:"
        ' 
        ' price
        ' 
        price.AutoSize = True
        price.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
        price.Location = New Point(132, 119)
        price.Name = "price"
        price.Size = New Size(19, 21)
        price.TabIndex = 30
        price.Text = "0"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
        Label1.Location = New Point(21, 29)
        Label1.Name = "Label1"
        Label1.Size = New Size(89, 21)
        Label1.TabIndex = 29
        Label1.Text = "Item Order:"
        ' 
        ' order
        ' 
        order.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point)
        order.Location = New Point(132, 29)
        order.Name = "order"
        order.Size = New Size(190, 27)
        order.TabIndex = 28
        ' 
        ' Button9
        ' 
        Button9.BackColor = Color.Transparent
        Button9.BackgroundImageLayout = ImageLayout.Center
        Button9.FlatAppearance.BorderSize = 0
        Button9.FlatStyle = FlatStyle.Flat
        Button9.Font = New Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point)
        Button9.Location = New Point(362, 0)
        Button9.Name = "Button9"
        Button9.Size = New Size(54, 35)
        Button9.TabIndex = 29
        Button9.Text = "X"
        Button9.UseVisualStyleBackColor = False
        ' 
        ' Form3
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(414, 296)
        Controls.Add(Button9)
        Controls.Add(Guna2GradientPanel2)
        FormBorderStyle = FormBorderStyle.None
        Name = "Form3"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Form3"
        Guna2GradientPanel2.ResumeLayout(False)
        Guna2GradientPanel2.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Guna2Button2 As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents prodQuantity As TextBox
    Friend WithEvents Label37 As Label
    Friend WithEvents Guna2GradientPanel2 As Guna.UI2.WinForms.Guna2GradientPanel
    Friend WithEvents Label1 As Label
    Friend WithEvents order As TextBox
    Friend WithEvents Button9 As Button
    Friend WithEvents price As Label
    Friend WithEvents totalPrice As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
End Class
