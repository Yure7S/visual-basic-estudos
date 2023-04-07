Imports System.Data

Public Class Tarefa

    Public Title As String
    Public Content As String
    Public Completed As Boolean
    Public DeadLine As Date
    Public Id As Integer

    Public Property tt() As String
        Get
            Return title
        End Get
        Set(ByVal value As String)
            title = value
        End Set
    End Property

    Public Sub Salvar()
        Dim cnn As New Conexao
        Dim dt As DataTable
        Dim dr As DataRow
        Dim strSQL As New StringBuilder

        strSQL.Append(" select * ")
        strSQL.Append(" from tasks")
        strSQL.Append(" where id = " & Id)

        dt = cnn.EditarDataTable(strSQL.ToString)

        If dt.Rows.Count = 0 Then
            dr = dt.NewRow
        Else
            dr = dt.Rows(0)
        End If

        dr("title") = Title
        dr("content") = Content
        dr("completed") = Completed
        dr("deadline") = DeadLine
        dr("id") = Id


        cnn.SalvarDataTable(dr)

        dt.Dispose()
        dt = Nothing

        cnn = Nothing
    End Sub

    Sub New(ByVal title As String, ByVal content As String, ByVal completed As Boolean, ByVal deadline As Date)
        Me.title = title
        Me.Content = content
        Me.Completed = completed
        Me.DeadLine = deadline

    End Sub
    Sub New(ByVal title As String, ByVal content As String, ByVal completed As Boolean, ByVal deadline As Date, ByVal id As Integer)
        Me.title = title
        Me.Content = content
        Me.Completed = completed
        Me.DeadLine = deadline
        Me.Id = id
    End Sub
End Class

