Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CarregarGrid()
        End If
    End Sub

    Protected Sub Tarefa(Optional id As Integer = 0)
        Dim tarefa As New Tarefa(id)
        With tarefa
            .Title = title.Text
            .Content = content.Text
            .Completed = False
            .Deadline = deadline.Text
            .Salvar()
        End With
        CarregarGrid()
    End Sub

    Protected Sub SalvarTarefa(Optional id As Integer = 0)
        Dim rowId As Integer

        For Each row As GridViewRow In GrdTarefa.Rows
            Dim cb As CheckBox = row.FindControl("linkToggle")
            If cb IsNot Nothing AndAlso cb.Checked Then
                rowId = Convert.ToInt32(GrdTarefa.DataKeys(row.RowIndex).Item(0))
                Dim tarefa As New Tarefa(rowId)
                If tarefa.Completed = True Then
                    With tarefa
                        .Completed = False
                        .Salvar()
                    End With
                Else
                    With tarefa
                        .Completed = True
                        .Salvar()
                    End With
                End If
            End If
        Next

        CarregarGrid()
    End Sub

    Private Sub CarregarGrid()
        Dim objTarefa As New Tarefa
        GrdTarefa.DataSource = objTarefa.Pesquisar(ViewState("OrderBy"))
        GrdTarefa.DataBind()
        objTarefa = Nothing
        lblRegistros.Text = DirectCast(GrdTarefa.DataSource, Data.DataTable).Rows.Count & " Tarefas(s)"
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles CreateTask.Click
        Try
            Tarefa()
        Catch err As Exception
            Console.WriteLine(err)
        End Try
    End Sub

    Enum ColunasGrid_grdTarefa As Integer
        buttons
    End Enum

    Private Sub grdTarefa_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GrdTarefa.RowDataBound
        Select Case e.Row.RowType
            Case DataControlRowType.Header

            Case DataControlRowType.DataRow

                If e.Row.Cells(3).Text = False Then
                    e.Row.Cells(3).Text = "Incompleta"
                Else
                    e.Row.Cells(3).Text = "Completa"
                End If

                ' Clonando botão
                Dim lnkExcluirTarefa As New LinkButton
                lnkExcluirTarefa = DirectCast(e.Row.Cells(ColunasGrid_grdTarefa.buttons).FindControl("lnkExcluirTarefa"), LinkButton)
                lnkExcluirTarefa.CommandArgument = e.Row.RowIndex
                lnkExcluirTarefa = Nothing

                For Each row As GridViewRow In GrdTarefa.Rows
                    Dim cb As CheckBox = row.FindControl("linkToggle")
                    If e.Row.Cells(3).Text = "Incompleta" Then
                        cb.Checked = False
                    Else
                        cb.Checked = True
                    End If
                Next

        End Select
    End Sub

    Private Sub grdTarefa_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdTarefa.PageIndexChanging
        GrdTarefa.PageIndex = e.NewPageIndex
        CarregarGrid()
    End Sub

    Private Sub grdTarefa_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GrdTarefa.Sorting
        ViewState("OrderByDirection") = IIf(ViewState("OrderByDirection") = "asc", "desc", "asc")
        ViewState("OrderBy") = e.SortExpression & " " & ViewState("OrderByDirection")
        CarregarGrid()
    End Sub

    Protected Sub grdTarefa_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GrdTarefa.RowCommand
        If e.CommandName = "" Then
            Response.Redirect(Request.Url.ToString)
        ElseIf e.CommandName = "EXCLUIR" Then
            Excluir(GrdTarefa.DataKeys(e.CommandArgument).Item(0))
        ElseIf e.CommandName = "CHECK" Then
            SalvarTarefa(GrdTarefa.DataKeys(e.CommandArgument).Item(0))
        End If
    End Sub

    Protected Sub Excluir(ByVal CodigoPessoa As Integer)
        Dim objTarefa As New Tarefa

        If objTarefa.Excluir(CodigoPessoa) > 0 Then
            Console.WriteLine("Deu certo")
        Else
            Console.WriteLine("Deu certo")
        End If

        objTarefa = Nothing


        CarregarGrid()
    End Sub

    Protected Sub title_TextChanged(sender As Object, e As EventArgs) Handles title.TextChanged

    End Sub

    Protected Sub deadline_TextChanged(sender As Object, e As EventArgs) Handles deadline.TextChanged

    End Sub

    Protected Sub GrdTarefa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdTarefa.SelectedIndexChanged

    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        SalvarTarefa()
        CarregarGrid()
    End Sub
End Class