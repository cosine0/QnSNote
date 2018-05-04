<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
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

    'Windows Form 디자이너에 필요합니다.
    Private components As System.ComponentModel.IContainer

    '참고: 다음 프로시저는 Windows Form 디자이너에 필요합니다.
    '수정하려면 Windows Form 디자이너를 사용하십시오.  
    '코드 편집기에서는 수정하지 마세요.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me._lay = New System.Windows.Forms.TableLayoutPanel()
        Me.SuspendLayout()
        '
        '_lay
        '
        Me._lay.AutoSize = True
        Me._lay.ColumnCount = 1
        Me._lay.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._lay.Location = New System.Drawing.Point(14, 11)
        Me._lay.Name = "_lay"
        Me._lay.RowCount = 1
        Me._lay.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._lay.Size = New System.Drawing.Size(334, 9)
        Me._lay.TabIndex = 1
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(343, 309)
        Me.ControlBox = False
        Me.Controls.Add(Me._lay)
        Me.ImeMode = System.Windows.Forms.ImeMode.Hangul
        Me.Name = "Form1"
        Me.Opacity = 0.97R
        Me.Text = "큐&스택노트"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents _lay As TableLayoutPanel
End Class
