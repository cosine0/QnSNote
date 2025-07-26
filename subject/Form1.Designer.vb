<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        content = New Label()
        SuspendLayout()
        ' 
        ' content
        ' 
        content.AutoSize = True
        content.Dock = DockStyle.Fill
        content.Font = New Font("Microsoft Sans Serif", 19.875F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        content.Location = New Point(65, 24)
        content.Margin = New Padding(23, 24, 23, 24)
        content.Name = "content"
        content.Size = New Size(113, 61)
        content.TabIndex = 0
        content.Text = "Idle"
        content.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(13F, 32F)
        AutoScaleMode = AutoScaleMode.Font
        AutoSize = True
        AutoSizeMode = AutoSizeMode.GrowAndShrink
        BackColor = Color.Lime
        ClientSize = New Size(871, 175)
        Controls.Add(content)
        FormBorderStyle = FormBorderStyle.None
        Location = New Point(1000, 0)
        Margin = New Padding(5, 8, 5, 8)
        Name = "Form1"
        Padding = New Padding(65, 24, 65, 24)
        StartPosition = FormStartPosition.Manual
        Text = "Subject"
        TopMost = True
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents content As Label
End Class
