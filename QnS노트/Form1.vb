Imports System.ComponentModel
Imports System.IO
Imports System.Text

Public Class Form1
    Private Enum NoteAreas
        Item
        Context
    End Enum

    Private _focusedArea = NoteAreas.Item
    Private _focusedNum = 0
    Private _movingItem = Nothing
    Private _shifting = False
    Private _lastSavedSerialization = ""
    Private _lastExecutionSerialization = ""

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub BroadcastKey(key As String)
        SendKeys.Send(key)
    End Sub

    Private Sub FocusOn(area As NoteAreas, order As Integer)
        Dim item As TextBox = Nothing
        UnFocus()
        _focusedArea = area
        _focusedNum = order
        If area = NoteAreas.Item Then
            item = _lay.GetControlFromPosition(0, order)
        Else
            ' something...
        End If
        item.BackColor = Color.Aquamarine
    End Sub

    Private Sub Form1_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        Dim textBox = New TextBox() With
                {
                .Multiline = True,
                .Dock = DockStyle.Fill,
                .Height = 25,
                .Font = New Font("맑은 고딕", 10.0!),
                .ImeMode = ImeMode.Hangul
                }
        AddHandler textBox.TextChanged, New EventHandler(AddressOf AdjustTexboxSize)
        AddHandler textBox.KeyDown, New KeyEventHandler(AddressOf Item_KeyDown)
        AddHandler textBox.KeyUp, New KeyEventHandler(AddressOf Item_Keyup)
        AddHandler textBox.GotFocus, New EventHandler(AddressOf Item_GotFocus)
        AddHandler textBox.LostFocus, New EventHandler(AddressOf Item_LostFocus)
        _lay.Controls.Add(textBox, 0, _lay.RowCount)
        Dim rowCount As TableLayoutPanel = _lay
        rowCount.RowCount = rowCount.RowCount + 1
        textBox.Select()
    End Sub

    Private Sub Form1_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        ActiveControl = Nothing
        Close()
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        e.SuppressKeyPress = False
        Select Case e.KeyCode
            Case Keys.Up
                Dim focusedItem As TextBox = _lay.GetControlFromPosition(0, _focusedNum)
                If Not (e.Alt AndAlso e.Shift) Then
                    If _focusedNum > 0 Then
                        FocusOn(_focusedArea, _focusedNum - 1)
                    End If
                Else
                    If Not e.Control Then
                        If _focusedNum > 0 Then
                            Swap(focusedItem, _lay.GetControlFromPosition(0, _focusedNum - 1))
                            FocusOn(_focusedArea, _focusedNum - 1)
                            _movingItem = focusedItem
                        End If
                    Else
                        If _movingItem IsNot Nothing Then
                            ' ctrl+alt+shift-up moving
                            If _focusedNum > 0 Then
                                Swap(focusedItem, _lay.GetControlFromPosition(0, _focusedNum - 1))
                                FocusOn(_focusedArea, _focusedNum - 1)
                            End If
                        Else
                            ' ctrl+alt+shift-up new
                            If focusedItem.Text <> "" Then
                                Dim copiedItem = New TextBox() With
                                    {
                                    .Multiline = True,
                                    .Dock = DockStyle.Fill,
                                    .Height = 25,
                                    .Font = New Font("맑은 고딕", 10.0!),
                                    .Tag = _lay.RowCount,
                                    .ImeMode = ImeMode.Hangul,
                                    .Text = focusedItem.Text,
                                    .BackColor = Color.PaleVioletRed
                                    }
                                AddHandler copiedItem.TextChanged, New EventHandler(AddressOf AdjustTexboxSize)
                                AddHandler copiedItem.KeyDown, New KeyEventHandler(AddressOf Item_KeyDown)
                                AddHandler copiedItem.KeyUp, New KeyEventHandler(AddressOf Item_Keyup)
                                AddHandler copiedItem.GotFocus, New EventHandler(AddressOf Item_GotFocus)
                                AddHandler copiedItem.LostFocus, New EventHandler(AddressOf Item_LostFocus)
                                Insert(copiedItem, _lay.GetRow(focusedItem))
                                _movingItem = copiedItem
                                FocusOn(_focusedArea, _focusedNum - 1)
                            End If
                        End If
                    End If
                End If
                e.Handled = True
                e.SuppressKeyPress = True
            Case Keys.Down
                Dim focusedItem As TextBox = _lay.GetControlFromPosition(0, _focusedNum)
                If Not (e.Alt AndAlso e.Shift) Then
                    If _focusedNum < _lay.RowCount - 1 Then
                        FocusOn(_focusedArea, _focusedNum + 1)
                    End If
                Else
                    If Not e.Control Then
                        If _focusedNum < _lay.RowCount - 1 Then
                            Swap(focusedItem, _lay.GetControlFromPosition(0, _focusedNum + 1))
                            FocusOn(_focusedArea, _focusedNum + 1)
                            _movingItem = focusedItem
                        End If
                    Else
                        If _movingItem IsNot Nothing Then
                            ' ctrl+alt+shift-down moving
                            If _focusedNum < _lay.RowCount - 1 Then
                                Swap(focusedItem, _lay.GetControlFromPosition(0, _focusedNum + 1))
                                FocusOn(_focusedArea, _focusedNum + 1)
                            End If
                        Else
                            ' ctrl+alt+shift-down new
                            If focusedItem.Text <> "" Then
                                Dim copiedItem = New TextBox() With
                                    {
                                    .Multiline = True,
                                    .Dock = DockStyle.Fill,
                                    .Height = 25,
                                    .Font = New Font("맑은 고딕", 10.0!),
                                    .Tag = _lay.RowCount,
                                    .ImeMode = ImeMode.Hangul,
                                    .Text = focusedItem.Text,
                                    .BackColor = Color.PaleVioletRed
                                    }
                                AddHandler copiedItem.TextChanged, New EventHandler(AddressOf AdjustTexboxSize)
                                AddHandler copiedItem.KeyDown, New KeyEventHandler(AddressOf Item_KeyDown)
                                AddHandler copiedItem.KeyUp, New KeyEventHandler(AddressOf Item_Keyup)
                                AddHandler copiedItem.GotFocus, New EventHandler(AddressOf Item_GotFocus)
                                AddHandler copiedItem.LostFocus, New EventHandler(AddressOf Item_LostFocus)
                                Insert(copiedItem, _lay.GetRow(focusedItem) + 1)
                                _movingItem = copiedItem
                                FocusOn(_focusedArea, _focusedNum + 1)
                            End If
                        End If
                    End If
                End If
                e.Handled = True
                e.SuppressKeyPress = True
            Case Keys.F2, Keys.Right
                Dim textLength As TextBox = _lay.GetControlFromPosition(0, _focusedNum)
                textLength.SelectionStart = textLength.TextLength
                _lay.GetControlFromPosition(0, _focusedNum).[Select]()
            Case Keys.Left
                Dim textLength1 As TextBox = _lay.GetControlFromPosition(0, _focusedNum)
                textLength1.SelectionStart = textLength1.TextLength - 1
                _lay.GetControlFromPosition(0, _focusedNum).[Select]()
            Case Keys.[Return]
                If (DirectCast(_lay.GetControlFromPosition(0, _focusedNum), TextBox)).TextLength > 0 Then
                    Dim textBox2 = New TextBox() With
                            {
                            .Multiline = True,
                            .Dock = DockStyle.Fill,
                            .Height = 25,
                            .Font = New Font("맑은 고딕", 10.0!),
                            .Tag = _lay.RowCount,
                            .ImeMode = ImeMode.Hangul
                            }
                    AddHandler textBox2.TextChanged, New EventHandler(AddressOf AdjustTexboxSize)
                    AddHandler textBox2.KeyDown, New KeyEventHandler(AddressOf Item_KeyDown)
                    AddHandler textBox2.KeyUp, New KeyEventHandler(AddressOf Item_Keyup)
                    AddHandler textBox2.GotFocus, New EventHandler(AddressOf Item_GotFocus)
                    AddHandler textBox2.LostFocus, New EventHandler(AddressOf Item_LostFocus)
                    If Not e.Shift Then
                        Insert(textBox2, _focusedNum + 1)
                    Else
                        Insert(textBox2, _focusedNum)
                    End If
                    textBox2.[Select]()
                End If
                e.Handled = True
                e.SuppressKeyPress = True
            Case Keys.Delete
                Remove(_lay, _lay.GetControlFromPosition(0, _focusedNum))
                If (_focusedNum = _lay.RowCount) Then
                    FocusOn(_focusedArea, _focusedNum - 1)
                ElseIf _lay.RowCount = 1 Then
                    _lay.Controls(0).Text = ""
                    FocusOn(_focusedArea, _focusedNum)
                End If
                e.Handled = True
            Case Keys.Escape
                Close()
            Case Keys.C
                If e.Control AndAlso Not e.Alt AndAlso Not e.Shift Then
                    Dim focusedItem As TextBox = _lay.GetControlFromPosition(0, _focusedNum)
                    Clipboard.SetText(focusedItem.Text)
                    e.SuppressKeyPress = True
                    e.Handled = True
                End If
            Case Keys.X
                If e.Control AndAlso Not e.Alt AndAlso Not e.Shift Then
                    Dim focusedItem As TextBox = _lay.GetControlFromPosition(0, _focusedNum)
                    Clipboard.SetText(focusedItem.Text)
                    focusedItem.[Select]()
                    focusedItem.Text = ""
                    e.SuppressKeyPress = True
                    e.Handled = True
                End If
        End Select
    End Sub

    Private Sub Form1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar <> ChrW(Keys.[Return]) Then
            If _focusedArea = NoteAreas.Item Then
                Dim focusedItem As TextBox = _lay.GetControlFromPosition(0, _focusedNum)
                focusedItem.Focus()
                focusedItem.SelectionStart = focusedItem.TextLength
                BroadcastKey(e.KeyChar.ToString())
            End If
        End If
    End Sub

    Private Sub Form1_Keyup(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If _movingItem IsNot Nothing AndAlso Not (e.Alt AndAlso e.Shift) Then
            FocusOn(_focusedArea, _lay.GetRow(_movingItem))
            _movingItem = Nothing
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim resumeSaved = False
        StartPosition = FormStartPosition.Manual
        Location = New Point(1500, 100)
        If (File.Exists("QnS.ini")) Then
            If (New FileInfo("QnS.ini")).Length > 0 Then
                resumeSaved = True
            End If
        End If
        If Not resumeSaved Then
            Dim textBox As TextBox
            textBox = New TextBox() With
                {
                .Multiline = True,
                .Dock = DockStyle.Fill,
                .Height = 25,
                .Font = New Font("맑은 고딕", 10.0!),
                .Text = "컨텍스트 : "
                }
            AddHandler textBox.TextChanged, New EventHandler(AddressOf AdjustTexboxSize)
            AddHandler textBox.KeyDown, New KeyEventHandler(AddressOf Item_KeyDown)
            AddHandler textBox.KeyUp, New KeyEventHandler(AddressOf Item_Keyup)
            AddHandler textBox.GotFocus, New EventHandler(AddressOf Item_GotFocus)
            AddHandler textBox.LostFocus, New EventHandler(AddressOf Item_LostFocus)
            _lay.Controls.Add(textBox, 0, 0)
            textBox.Focus()
        Else
            Dim savedLines = File.ReadAllText("QnS.ini")
            _lay.RowCount = 0
            For Each line In savedLines.Split(ControlChars.CrLf.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                Dim textBox As TextBox
                textBox = New TextBox() With
                    {
                    .Multiline = True,
                    .Dock = DockStyle.Fill,
                    .Height = 25,
                    .Font = New Font("맑은 고딕", 10.0!),
                    .Text = line,
                    .ImeMode = ImeMode.Hangul
                    }
                AddHandler textBox.TextChanged, New EventHandler(AddressOf AdjustTexboxSize)
                AddHandler textBox.KeyDown, New KeyEventHandler(AddressOf Item_KeyDown)
                AddHandler textBox.KeyUp, New KeyEventHandler(AddressOf Item_Keyup)
                AddHandler textBox.GotFocus, New EventHandler(AddressOf Item_GotFocus)
                AddHandler textBox.LostFocus, New EventHandler(AddressOf Item_LostFocus)
                _lay.Controls.Add(textBox, 0, _lay.RowCount)
                _lay.RowCount += 1
                AdjustTexboxSize(textBox, Nothing)
            Next
            _lay.GetControlFromPosition(0, _lay.RowCount - 1).Focus()
            _lastExecutionSerialization = savedLines
            _lastSavedSerialization = savedLines
        End If
    End Sub

    Private Sub Insert(ByRef controlToInsert As Control, Optional at As Integer = 1)
        _lay.RowCount += 1
        Dim enumerator = _lay.Controls.GetEnumerator()
        While enumerator.MoveNext()
            Dim current As Control = enumerator.Current
            If (_lay.GetRow(current) >= at) Then
                _lay.SetRow(current, _lay.GetRow(current) + 1)
            End If
        End While
        If _focusedNum >= at Then
            _focusedNum += 1
        End If
        _lay.Controls.Add(controlToInsert, 0, at)
    End Sub

    Private Sub Item_GotFocus(sender As TextBox, e As EventArgs)
        UnFocus()
        If sender IsNot _movingItem Then
            sender.BackColor = Color.AliceBlue
        End If
    End Sub

    Private Sub Item_KeyDown(sender As TextBox, e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.Escape
                ActiveControl = Nothing
                If sender.TextLength <> 0 OrElse _lay.GetRow(sender) <= 0 Then
                    FocusOn(NoteAreas.Item, _lay.GetRow(sender))
                Else
                    FocusOn(NoteAreas.Item, _lay.GetRow(sender) - 1)
                End If
                e.Handled = True
                e.SuppressKeyPress = True
            Case Keys.[Return]
                If sender.TextLength > 0 Then
                    Dim newItem = New TextBox() With
                            {
                            .Multiline = True,
                            .Dock = DockStyle.Fill,
                            .Height = 25,
                            .Font = New Font("맑은 고딕", 10.0!),
                            .Tag = _lay.RowCount,
                            .ImeMode = ImeMode.Hangul
                            }
                    AddHandler newItem.TextChanged, New EventHandler(AddressOf AdjustTexboxSize)
                    AddHandler newItem.KeyDown, New KeyEventHandler(AddressOf Item_KeyDown)
                    AddHandler newItem.KeyUp, New KeyEventHandler(AddressOf Item_Keyup)
                    AddHandler newItem.GotFocus, New EventHandler(AddressOf Item_GotFocus)
                    AddHandler newItem.LostFocus, New EventHandler(AddressOf Item_LostFocus)
                    If Not e.Shift Then
                        Insert(newItem, _lay.GetRow(sender) + 1)
                    Else
                        Insert(newItem, _lay.GetRow(sender))
                    End If
                    newItem.Focus()
                End If
                e.Handled = True
                e.SuppressKeyPress = True
            Case Keys.Up
                Dim itemNum As Integer = _lay.GetRow(sender)
                If e.Alt AndAlso e.Shift Then
                    If Not e.Control Then
                        If itemNum > 0 Then
                            Swap(sender, _lay.GetControlFromPosition(0, itemNum - 1))
                            _movingItem = sender
                        End If
                    Else
                        If _movingItem IsNot Nothing Then
                            ' ctrl+alt+shift-down moving
                            If itemNum > 0 Then
                                Swap(sender, _lay.GetControlFromPosition(0, itemNum - 1))
                            End If
                        Else
                            ' ctrl+alt+shift-down new
                            If sender.Text <> "" Then
                                Dim copiedItem = New TextBox() With
                                    {
                                    .Multiline = True,
                                    .Dock = DockStyle.Fill,
                                    .Height = 25,
                                    .Font = New Font("맑은 고딕", 10.0!),
                                    .Tag = _lay.RowCount,
                                    .ImeMode = ImeMode.Hangul,
                                    .Text = sender.Text,
                                    .BackColor = Color.PaleVioletRed
                                    }
                                AddHandler copiedItem.TextChanged, New EventHandler(AddressOf AdjustTexboxSize)
                                AddHandler copiedItem.KeyDown, New KeyEventHandler(AddressOf Item_KeyDown)
                                AddHandler copiedItem.KeyUp, New KeyEventHandler(AddressOf Item_Keyup)
                                AddHandler copiedItem.GotFocus, New EventHandler(AddressOf Item_GotFocus)
                                AddHandler copiedItem.LostFocus, New EventHandler(AddressOf Item_LostFocus)
                                Insert(copiedItem, _lay.GetRow(sender))
                                copiedItem.Focus()
                                _movingItem = copiedItem
                            End If
                        End If
                    End If
                    e.Handled = True
                    e.SuppressKeyPress = True
                ElseIf sender.GetLineFromCharIndex(sender.SelectionStart) = 0 AndAlso itemNum > 0 Then
                    FocusOn(_focusedArea, itemNum - 1)
                    ActiveControl = Nothing
                    e.Handled = True
                    e.SuppressKeyPress = True
                End If
            Case Keys.Down
                Dim itemNum As Integer = _lay.GetRow(sender)
                If e.Alt AndAlso e.Shift Then
                    If Not e.Control Then
                        If itemNum < _lay.RowCount - 1 Then
                            Swap(sender, _lay.GetControlFromPosition(0, itemNum + 1))
                            _movingItem = sender
                        End If
                    Else
                        If _movingItem IsNot Nothing Then
                            ' ctrl+alt+shift-down moving
                            If itemNum < _lay.RowCount - 1 Then
                                Swap(sender, _lay.GetControlFromPosition(0, itemNum + 1))
                            End If
                        Else
                            ' ctrl+alt+shift-down new
                            If sender.Text <> "" Then
                                Dim copiedItem = New TextBox() With
                                    {
                                    .Multiline = True,
                                    .Dock = DockStyle.Fill,
                                    .Height = 25,
                                    .Font = New Font("맑은 고딕", 10.0!),
                                    .Tag = _lay.RowCount,
                                    .ImeMode = ImeMode.Hangul,
                                    .Text = sender.Text,
                                    .BackColor = Color.PaleVioletRed
                                    }
                                AddHandler copiedItem.TextChanged, New EventHandler(AddressOf AdjustTexboxSize)
                                AddHandler copiedItem.KeyDown, New KeyEventHandler(AddressOf Item_KeyDown)
                                AddHandler copiedItem.KeyUp, New KeyEventHandler(AddressOf Item_Keyup)
                                AddHandler copiedItem.GotFocus, New EventHandler(AddressOf Item_GotFocus)
                                AddHandler copiedItem.LostFocus, New EventHandler(AddressOf Item_LostFocus)
                                Insert(copiedItem, _lay.GetRow(sender) + 1)
                                copiedItem.Focus()
                                _movingItem = copiedItem
                            End If
                        End If
                    End If
                    e.Handled = True
                    e.SuppressKeyPress = True
                ElseIf sender.GetLineFromCharIndex(sender.SelectionStart) = sender.GetLineFromCharIndex(sender.TextLength) AndAlso itemNum < _lay.RowCount - 1 Then
                    FocusOn(_focusedArea, itemNum + 1)
                    ActiveControl = Nothing
                    e.Handled = True
                    e.SuppressKeyPress = True
                End If
            Case Keys.A
                If Not e.Control OrElse e.Alt OrElse e.Shift Then
                    OnKeyDown(e)
                Else
                    sender.SelectAll()
                End If
        End Select
    End Sub

    Private Sub Item_Keyup(sender As TextBox, e As KeyEventArgs)
        If _movingItem IsNot Nothing AndAlso Not (e.Alt AndAlso e.Shift) Then
            _movingItem.BackColor = Color.AliceBlue
            _movingItem = Nothing
            e.Handled = True
        End If
    End Sub
    '
    Private Sub Item_LostFocus(sender As TextBox, e As EventArgs)
        sender.BackColor = Color.White
        If sender.TextLength = 0 Then
            Remove(_lay, sender)
        End If
        If _lay.Controls.Count = _lay.RowCount Then
            SaveAll()
        End If
    End Sub

    Private Sub AdjustTexboxSize(sender As TextBox, e As EventArgs)
        Dim numberOfLines As Integer = sender.GetLineFromCharIndex(sender.TextLength) + 1
        Dim outerMargin As Integer = sender.Height - sender.ClientSize.Height
        sender.Height = sender.Font.Height * numberOfLines + 3 + outerMargin
    End Sub

    Private Sub Remove(ByRef layout As TableLayoutPanel, ByRef controlToInsert As Control)
        If _lay.RowCount <> 1 Then
            Dim rowToRemove As Integer = _lay.GetRow(controlToInsert)
            ' 레이아웃에서 제거
            layout.Controls.Remove(controlToInsert)
            ' 밑에 있는 칸들 위로 밀기
            Dim enumerator = layout.Controls.GetEnumerator()
            While enumerator.MoveNext()
                Dim current As TextBox = enumerator.Current
                If (layout.GetRow(current) >= rowToRemove) Then
                    layout.SetRow(current, layout.GetRow(current) - 1)
                End If
            End While

            layout.RowCount -= 1
            If _focusedNum > rowToRemove OrElse _focusedNum >= layout.RowCount Then
                FocusOn(_focusedArea, _focusedNum - 1)
            ElseIf _focusedNum = rowToRemove Then
                FocusOn(_focusedArea, _focusedNum)
            End If
            If _lay.Controls.Count = _lay.RowCount Then
                SaveAll()
            End If
        End If
    End Sub
    '
    Private Sub SaveAll()
        Dim stringBuilder = New StringBuilder()
        Dim maxNum As Integer = _lay.RowCount - 1
        For itemNum = 0 To maxNum
            Dim controlFromPosition = DirectCast(_lay.GetControlFromPosition(0, itemNum), TextBox)
            If controlFromPosition.TextLength > 0 Then
                stringBuilder.AppendLine(controlFromPosition.Text)
            End If
        Next
        Dim serialized = stringBuilder.ToString()
        If serialized = _lastSavedSerialization Then
            Return
        End If
        Using streamWriter = New StreamWriter("QnS.ini")
            streamWriter.Write(serialized)
        End Using
        _lastSavedSerialization = serialized
    End Sub

    Private Sub Swap(ByRef a As Control, ByRef b As Control)
        Dim aRow As Integer = _lay.GetRow(a)
        _lay.SetRow(a, _lay.GetRow(b))
        _lay.SetRow(b, aRow)
        SaveAll()
    End Sub

    Private Sub UnFocus()
        Dim focusedItem As TextBox = Nothing
        If _focusedNum < _lay.RowCount Then
            If _focusedArea = NoteAreas.Item Then
                focusedItem = _lay.GetControlFromPosition(0, _focusedNum)
            End If
            focusedItem.BackColor = Color.White
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub Form1_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        If _lastSavedSerialization <> _lastExecutionSerialization Then
            SqliteStorage.Db.Insert(_lastSavedSerialization)
        End If
    End Sub
End Class