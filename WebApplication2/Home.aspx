<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Home.aspx.vb" Inherits="WebApplication2.WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="Style/Style.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-GLhlTQ8iRABdZLl6O3oVMWSktQOp6b7In1Zl3/Jr59b6EGGoI1aFkw7cmDA6j6gD" crossorigin="anonymous">
    <title>Gerenciador de tarefas</title>
</head>
<body runat="server" style="height: 707px; width: 103%">
    <form class="gap-2 d-flex flex-column container-sm" id="form1" runat="server">
        <div style="width: 18rem;"" >
            <label class="form-label">Título</label>
            <asp:TextBox CssClass="form-control" ID="title" runat="server"></asp:TextBox>
            <label class="form-label">Conteúdo</label>
            <asp:TextBox CssClass="form-control" ID="content" runat="server"></asp:TextBox>
            <label class="form-label">Data limite</label>
            <asp:TextBox CssClass="form-control" TextMode="Date" ID="deadline" runat="server"></asp:TextBox>
            <asp:Button CssClass="btn btn-primary gap-3" ID="CreateTask" runat="server" Text="Adicionar" />
        </div>

        <asp:Label ID="lblRegistros" runat="server" CssClass="badge bg-aqua" />
        <asp:GridView ID="GrdTarefa" 
            runat="server" 
            CssClass="table table-bordered" 
            PagerStyle-CssClass="paginacao" 
            AllowSorting="True" 
            AllowPaging="True" 
            PageSize="20" 
            AutoGenerateColumns="False" 
            DataKeyNames="id">
            <HeaderStyle CssClass="bg-aqua" ForeColor="White" />
            <Columns>
                <asp:BoundField DataField="id" SortExpression="id" HeaderText="id" />
                <asp:BoundField DataField="title" SortExpression="title" HeaderText="title" />
                <asp:BoundField DataField="content" SortExpression="content" HeaderText="content" />
                <asp:BoundField DataField="completed" SortExpression="completed" HeaderText="completed" />
                <asp:BoundField DataField="deadline" SortExpression="deadline" HeaderText="deadline" />
                <asp:TemplateField HeaderText="" SortExpression="" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                    <ItemTemplate>
                        <div class="btn-group">
                            <asp:LinkButton ID="lnkExcluirTarefa" runat="server" Text="EXCLUIR" class="btn btn-social-icon bg-red" CommandName="EXCLUIR" ToolTip="ExcluirTarefa">
                                <i id="iExcluirTarefa" runat="server" class="fa fa-trash"></i>
                            </asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" SortExpression="" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                    <ItemTemplate>
                        <div class="btn-group">
                            <asp:CheckBox ID="linkToggle" runat="server" class="btn btn-social-icon bg-red" CommandName="CHECK" ToolTip="ExcluirTarefa" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Button class="btn btn-primary" runat="server" ID="btnSalvar" Text="Salvar" />
    </form>



</body>
</html>
