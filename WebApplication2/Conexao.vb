Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text
Imports System.IO.Path
Imports System.IO

Public Class Conexao
    Private strStringConexao As String = System.Configuration.ConfigurationManager.ConnectionStrings("StringConexao").ToString

    Private cnn As SqlConnection
    Private cmd As SqlCommand
    Private da As SqlDataAdapter
    Private tra As SqlTransaction

    Public ReadOnly Property ConnectionString() As String
        Get
            Return strStringConexao
        End Get
    End Property

    Public Sub New()

    End Sub

    Public Sub IniciarTransacao()
        cmd = New SqlCommand
        cnn = New SqlConnection(strStringConexao)

        cnn.Open()

        cmd = cnn.CreateCommand

        tra = cnn.BeginTransaction()

        cmd.Connection = cnn

        cmd.Transaction = tra
    End Sub

    Public Sub ConfirmarTransacao()
        tra.Commit()

        'Log(da)

        cnn.Close()

        cmd.Dispose()
        cnn.Dispose()
        tra.Dispose()

        cmd = Nothing
        cnn = Nothing
        tra = Nothing
    End Sub

    Public Sub CancelarTransacao()
        tra.Rollback()

        cnn.Close()

        cmd.Dispose()
        cnn.Dispose()
        tra.Dispose()

        cmd = Nothing
        cnn = Nothing
        tra = Nothing
    End Sub

    Public Function AbrirDataTable(ByVal SQL As String) As DataTable
        Dim dt As New DataTable

        If tra Is Nothing Then
            'Se não foi passado transação, seta a conexão
            cnn = New SqlConnection(strStringConexao)
            cmd = New SqlCommand
            cmd.Connection = cnn
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandText = SQL

        da = New SqlDataAdapter(cmd)
        da.Fill(dt)

        If tra Is Nothing Then
            cnn.Close()

            cnn.Dispose()
            cmd.Dispose()

            cnn = Nothing
            cmd = Nothing
        End If

        da.Dispose()
        da = Nothing

        Return dt
    End Function

    Public Function AbrirDataTable_StoredProcedure(ByVal Valor As String, ByVal NomeParametro As String, ByVal StoredProc As String) As DataTable
        Dim dt As New DataTable

        If tra Is Nothing Then
            'Se não foi passado transação, seta a conexão
            cnn = New SqlConnection(strStringConexao)
            cmd = New SqlCommand
        End If

        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = StoredProc

        cmd.Parameters.Add(NomeParametro)
        cmd.Parameters(NomeParametro).Value = Valor

        da = New SqlDataAdapter(cmd)
        da.Fill(dt)

        If tra Is Nothing Then
            cnn.Close()

            cnn.Dispose()
            cmd.Dispose()

            cnn = Nothing
            cmd = Nothing
        End If

        da.Dispose()
        da = Nothing

        Return dt
    End Function

    Public Function ExecutarSQL(ByVal SQL As String) As Integer
        Dim RowsAffected As Integer

        If tra Is Nothing Then
            cnn = New SqlConnection(strStringConexao)
            cmd = New SqlCommand(SQL, cnn)

            cnn.Open()
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandText = SQL

        Try

            RowsAffected = cmd.ExecuteNonQuery()

        Catch

            RowsAffected = -1

        End Try

        If tra Is Nothing Then
            cnn.Close()

            cmd.Dispose()
            cnn.Dispose()

            cnn = Nothing
            cmd = Nothing
        End If

        Return RowsAffected
    End Function

    Public Function EditarDataTable(ByVal SQL As String) As DataTable
        Dim dt As New DataTable

        If tra Is Nothing Then
            'Se não foi passado transação, seta a conexão
            cnn = New SqlConnection(strStringConexao)
            cmd = New SqlCommand
            cmd.Connection = cnn
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandText = SQL
        da = New SqlDataAdapter(cmd)
        da.Fill(dt)

        If tra Is Nothing Then
            cmd.Dispose()
            cmd = Nothing
        End If

        Return dt
    End Function

    Public Function SalvarDataTable(ByVal dRow As DataRow, Optional ByVal GravarLog As Boolean = True, Optional ByVal CampoRetorno As String = "") As Boolean
        Dim objBuilder As New SqlCommandBuilder(da)
        Dim dt As DataTable = dRow.Table
        Dim blnRetorno As Boolean = True

        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dRow)
            da.InsertCommand = objBuilder.GetInsertCommand
        Else
            da.UpdateCommand = objBuilder.GetUpdateCommand
        End If

        da.Update(dt)

        'If GravarLog And tra Is Nothing Then
        '    Log(da)
        'End If

        If tra Is Nothing Then
            cnn.Close()
            cnn.Dispose()
            cnn = Nothing
        End If


        da.Dispose()
        objBuilder.Dispose()
        dt.Dispose()

        da = Nothing
        objBuilder = Nothing
        dt = Nothing

        Return blnRetorno
    End Function

    Public Sub CancelarDataTable()
        If Not tra Is Nothing Then
            CancelarTransacao()

            cnn.Close()
            cnn.Dispose()
            cnn = Nothing

            tra.Dispose()
            tra = Nothing
        End If

        da.Dispose()
        da = Nothing
    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
