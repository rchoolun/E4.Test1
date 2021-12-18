<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Test1._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" type="text/css" />
    <link href="https://unpkg.com/gijgo@1.9.13/css/gijgo.css" rel="stylesheet" type="text/css" />
    <style>
        .form-row { display: flex; margin-bottom: 29px; }
        .form-row:last-child { margin-bottom: 0px; }
        .margin-top-10 { margin-top: 10px; }
        .float-left { float: left; }
        .float-right { float: right; }
        .display-inline { display: inline; }
        .display-inline-block { display: inline-block; }
        .width-200 { width: 200px; }
        .clear-both { clear: both; }
        .gj-display-none { display: none; }
    </style>
    <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <script src="https://unpkg.com/gijgo@1.9.13/js/gijgo.js" type="text/javascript"></script>

      <div class="margin-top-10">
        <div class="float-left">

        </div>
        <div class="float-right">
            <button id="btnAdd" type="button" class="gj-button-md">Add New Record</button>
        </div>
    </div>
    <div class="clear-both"></div>
    <div class="margin-top-10">
        <table id="grid"></table>
    </div>

    <div id="dialog" class="gj-display-none">
        <div data-role="body">
            <input type="hidden" id="Id" />
            <div class="form-row">
                <input type="text" class="gj-textbox-md" id="Name" placeholder="Name...">
            </div>
            <div class="form-row">
                <input type="text" class="gj-textbox-md" id="Surname" placeholder="Surname...">
            </div>
             <div class="form-row">
                <input type="text" class="gj-textbox-md" id="Phone" placeholder="Phone..." maxlength="8">
            </div>
        </div>
        <div data-role="footer">
            <button type="button" id="btnSave" class="gj-button-md">Save</button>
            <button type="button" id="btnCancel" class="gj-button-md">Cancel</button>
        </div>
    </div>

    <script type="text/javascript">

        function Edit(e) {
            $('#Id').val(e.data.id);
            $('#Name').val(e.data.record.Name);
            $('#Surname').val(e.data.record.Surname);
            $('#Phone').val(e.data.record.Phone);
            
            dialog.open('Edit User');
        }
        function Save() {
            $.ajax({
                type: "POST",
                url: "/AjaxProcess.aspx/SaveUser",
                data: "{Id:'" + $('#Id').val() + "',Name:'" + $('#Name').val() + "',Surname:'" + $('#Surname').val() + "',Phone:'" + $('#Phone').val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    alert(result.d);
                    dialog.close();
                    grid.reload();
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        function Delete(e) {
            if (confirm('Are you sure?')) {
                $.ajax({
                    type: "POST",
                    url: "/AjaxProcess.aspx/DeleteUser",
                    data: '{Id: "' + e.data.id + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(result) {
                        alert(result.d);
                        grid.reload();
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
               });
            }
        }

        $(document).ready(function () {
            grid = $('#grid').grid({
                primaryKey: 'Id',
                dataSource: 'AjaxProcess.aspx?method=GetAllUsers',
                columns: [
                    { field: 'Name', sortable: true },
                    { field: 'Surname', title: 'Surname', sortable: true },
                    { field: 'Phone', title: 'Phone Number', sortable: true },
                    { width: 64, tmpl: '<span class="material-icons gj-cursor-pointer">edit</span>', align: 'center', events: { 'click': Edit } },
                    { width: 64, tmpl: '<span class="material-icons gj-cursor-pointer">delete</span>', align: 'center', events: { 'click': Delete } }
                ],
                pager: { limit: 25 }
            });
            dialog = $('#dialog').dialog({
                autoOpen: false,
                resizable: false,
                modal: true,
                width: 360
            });

            $('#btnAdd').on('click', function () {
                $('#Id').val('');
                $('#Name').val('');
                $('#Surname').val('');
                $('#Phone').val('');
                dialog.open('Add User');
            });
            $('#btnSave').on('click', Save);
            $('#btnCancel').on('click', function () {
                dialog.close();
            });
            $('#btnSearch').on('click', function () {
                grid.reload({ page: 1, name: $('#Name').val(), surname: $('#Surname').val() });
            });
            $('#btnClear').on('click', function () {
                $('#Name').val('');
                $('#Surname').val('');
                grid.reload({ name: '', surname: '' });
            });
        });
    </script>

</asp:Content>
