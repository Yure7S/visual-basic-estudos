Imports System.Data.SqlClient
Public Class FactoryConnection

    Private Shared myConn As SqlConnection
    Private Shared myCmd As SqlCommand
    Private Shared myReader As SqlDataReader
    Private Shared banco As String = "ManagementTask"

    Public Shared Function getConnetion() As SqlConnection
        Dim urlConnection = $"Initial Catalog={banco};Data Source=localhost\SQLEXPRESS;Integrated Security=SSPI; Trusted_Connection=True;"
        myConn = New SqlConnection(urlConnection)
        Return myConn
    End Function

    Public Shared Sub CriarComando(SQL As String)
        myCmd = myConn.CreateCommand()
        myCmd.CommandText = SQL
    End Sub

    Public Shared Sub CreateTask(tarefa As Tarefa)
        getConnetion()
        myConn.Open()
        CriarComando($"insert into tasks(title, content, completed, deadline) values('{tarefa.title}', '{tarefa.Content}', 0, '{tarefa.DeadLine}')")
        myCmd.ExecuteNonQuery()
        myConn.Close()
    End Sub

    Public Shared Function GetTasks()
        Dim result As New ArrayList
        getConnetion()
        myCmd = myConn.CreateCommand()
        myCmd.CommandText = "SELECT * FROM tasks"

        myConn.Open()
        myReader = myCmd.ExecuteReader()
        Do While myReader.Read()
            Dim title As String = myReader.GetString(0)
            Dim content As String = myReader.GetString(1)
            Dim completed As Boolean = myReader.GetBoolean(2)
            Dim deadline As Date = myReader.GetDateTime(3)
            Dim id As Integer = myReader.GetValue(4)
            Dim c As New Tarefa(title, content, completed, deadline, id)
            result.Add(c)
        Loop
        myConn.Close()
        Return result
    End Function

    Public Shared Function Search(chunk As String)
        Dim result As New ArrayList
        getConnetion()
        myCmd = myConn.CreateCommand()
        myCmd.CommandText = $"Select * from tasks where title like '%{chunk}%'"
        myConn.Open()
        myReader = myCmd.ExecuteReader()
        Do While myReader.Read()
            Dim title As String = myReader.GetString(0)
            Dim content As String = myReader.GetString(1)
            Dim completed As Boolean = myReader.GetBoolean(2)
            Dim deadline As Date = myReader.GetDateTime(3)
            Dim c As New Tarefa(title, content, completed, deadline)
            result.Add(c)
        Loop
        myConn.Close()
        Return result

    End Function

    Public Shared Function DeleteTask(id As Integer)
        getConnetion()
        myCmd = myConn.CreateCommand()
        myCmd.CommandText = $"delete from tasks where id = {id}"
        myConn.Open()
        myCmd.ExecuteNonQuery()
        myConn.Close()
        Console.Clear()
    End Function

End Class
