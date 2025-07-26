Imports System.IO

Public Class Form1
    Private _fileWatcher As FileSystemWatcher

    Private _isDragging As Boolean = False
    Private _dragCursorPoint As Point
    Private _dragFormPoint As Point

    Private Sub Form_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        _fileWatcher = New FileSystemWatcher(Environment.CurrentDirectory) With
            { 
            .Filter = Path.GetFileName("QnS.ini"),
            .EnableRaisingEvents = True
            }
        AddHandler _fileWatcher.Changed, AddressOf UpdateContent
    End Sub

    Function ReadLineWithNumberFrom(filePath As String, lineNumber As Integer) As String
        Using file As New StreamReader(filePath)
            ' Skip all preceding lines
            For i = 1 To lineNumber - 1
                If file.ReadLine() Is Nothing Then
                    Throw New ArgumentOutOfRangeException("lineNumber")
                End If
            Next
            ' Attempt to read the line you're interested in
            Dim line As String = file.ReadLine()
            If line Is Nothing Then
                Throw New ArgumentOutOfRangeException("lineNumber")
            End If
            ' Succeeded!
            Return line
        End Using
    End Function

    Private Sub Form1_MouseHover(sender As Object, e As EventArgs) Handles Me.MouseHover
        UpdateContent()
    End Sub

    Sub UpdateContent()
        If (File.Exists("QnS.ini")) Then
            Try 
                content.Text = ReadLineWithNumberFrom("QnS.ini", 2)
                Location = New Point(Screen.PrimaryScreen.Bounds.Width - Size.Width,
                                     Screen.PrimaryScreen.Bounds.Height*0.1)
                Me.Text = content.Text & "- Subject"
                If content.Text.StartsWith("[!]") Then
                    Me.BackColor = Color.Red
                    content.ForeColor = Color.White
                Else
                    Me.BackColor = Color.Lime
                    content.ForeColor = Color.Black
                End If
                Return
            Catch
            End Try
        End If
        content.Text = "Idle "
        Location = New Point(Screen.PrimaryScreen.Bounds.Width - Size.Width, Screen.PrimaryScreen.Bounds.Height*0.1)
    End Sub

    Private Sub Form1_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        UpdateContent()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = MouseButtons.Left Then
            _isDragging = True
            _dragCursorPoint = Cursor.Position
            _dragFormPoint = Me.Location
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If _isDragging Then
            Dim currentScreenPos = Cursor.Position
            Location = New Point(_dragFormPoint.X + currentScreenPos.X - _dragCursorPoint.X,
                                 _dragFormPoint.Y + currentScreenPos.Y - _dragCursorPoint.Y)
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        _isDragging = False
    End Sub
End Class
