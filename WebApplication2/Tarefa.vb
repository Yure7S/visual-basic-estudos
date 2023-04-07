Imports System.Data

Public Class Tarefa

    Private _Title As String
    Private _Content As String
    Private _Completed As String
    Private _Deadline As Date
    Private _Id As Integer
    Public Property Content() As String
        Get
            Return _Content
        End Get
        Set(ByVal value As String)
            _Content = value
        End Set
    End Property

    Public Property Title() As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = value
        End Set
    End Property

    Public Property Completed() As Boolean
        Get
            Return _Completed
        End Get
        Set(ByVal value As Boolean)
            _Completed = value
        End Set
    End Property

    Public Property Deadline() As Date
        Get
            Return _Deadline
        End Get
        Set(ByVal value As Date)
            _Deadline = value
        End Set
    End Property

    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
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
        dr("deadline") = _Deadline
        dr("id") = Id


        cnn.SalvarDataTable(dr)

        dt.Dispose()
        dt = Nothing

        cnn = Nothing
    End Sub

    Public Sub New(Optional id As Integer = 0)
        If id > 0 Then
            Obter(id)
        End If
    End Sub

    Sub New(ByVal title As String, ByVal content As String, ByVal completed As Boolean, ByVal deadline As Date)
        Me.Title = title
        Me.Content = content
        Me.Completed = completed
        Me.Deadline = deadline
    End Sub
    Sub New(ByVal title As String, ByVal content As String, ByVal completed As Boolean, ByVal deadline As Date, ByVal id As Integer)
        Me.Title = title
        Me.Content = content
        Me.Completed = completed
        Me.Deadline = deadline
        Me.Id = id
    End Sub

    Public Sub Obter(ByVal Codigo As Integer)
        Dim cnn As New Conexao
        Dim dt As DataTable
        Dim dr As DataRow
        Dim strSQL As New StringBuilder

        strSQL.Append(" select * ")
        strSQL.Append(" from tasks")
        strSQL.Append(" where id = " & Codigo)

        dt = cnn.AbrirDataTable(strSQL.ToString)

        If dt.Rows.Count > 0 Then
            dr = dt.Rows(0)

            Title = dr("title")
            Content = dr("content")
            Completed = dr("completed")
            Deadline = dr("deadline")
            Id = dr("id")
        End If

        cnn = Nothing
    End Sub

    Public Function Pesquisar(Optional ByVal Sort As String = "",
                              Optional ByVal id As Integer = 0,
                              Optional ByVal title As String = "",
                              Optional ByVal content As String = "",
                              Optional ByVal deadline As String = "",
                              Optional ByVal completed As Boolean = False) As DataTable

        Dim cnn As New Conexao
        Dim strSQL As New StringBuilder

        strSQL.Append(" select * ")
        strSQL.Append(" from tasks")
        strSQL.Append(" where id is not null")


        If id > 0 Then
            strSQL.Append(" and id = " & id)
        End If

        If title <> "" Then
            strSQL.Append(" and title like '%" & title & "%'")
        End If

        If content <> "" Then
            strSQL.Append(" and content like '%" & content & "%'")
        End If

        If completed Then
            strSQL.Append(" and completed =" & completed)
        End If

        If IsDate(deadline) Then
            strSQL.Append(" and deadline = convert(datatime, '" & deadline & "', 103)")
        End If

        strSQL.Append(" Order By " & IIf(Sort = "", "title", Sort))

        Return cnn.AbrirDataTable(strSQL.ToString)
    End Function

    Public Function Excluir(ByVal Codigo As Integer) As Integer
        Dim cnn As New Conexao
        Dim strSQL As New StringBuilder
        Dim LinhasAfetadas As Integer

        strSQL.Append(" delete ")
        strSQL.Append(" from tasks")
        strSQL.Append(" where id = " & Codigo)

        LinhasAfetadas = cnn.ExecutarSQL(strSQL.ToString)

        '
        cnn = Nothing

        Return LinhasAfetadas
    End Function
End Class


