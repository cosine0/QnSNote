Imports System.Data.SQLite

Public NotInheritable Class SqliteStorage
    Private ReadOnly _connection As SQLiteConnection
    Private _insertStatement As SQLiteCommand
    Private Const DbFileName = "qns_log"

    Private Shared _db As SqliteStorage = Nothing
    Public Shared ReadOnly Property Db() As SqliteStorage
        Get
            If _db Is Nothing Then
                _db = New SqliteStorage()
            End If
            Return _db
        End Get
    End Property

    Private Sub New()
        _connection = New SQLiteConnection("Data Source=" + DbFileName)
        _connection.Open()
        Dim cmd = New SQLiteCommand("CREATE TABLE IF NOT EXISTS qns (time TEXT, content TEXT);", _connection)
        cmd.ExecuteNonQuery()
    End Sub

    Private Overloads Sub Finalize()
        _connection.Close()
    End Sub

    Public Sub Insert(content As String)
        If _insertStatement Is Nothing Then
            _insertStatement = New SQLiteCommand("INSERT INTO qns (time, content) VALUES (datetime('now', 'localtime'), @content)", _connection)
            _insertStatement.Prepare()
        End If

        _insertStatement.Parameters.AddWithValue("@content", content)
        _insertStatement.ExecuteNonQuery()
    End Sub
End Class
