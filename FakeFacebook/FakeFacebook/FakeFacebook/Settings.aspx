<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="FakeFacebook.Settings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.3.0/css/datepicker.css" rel="stylesheet" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.3.0/js/bootstrap-datepicker.js"></script>
    <link rel="stylesheet" href="TermProject.css" />
</head>

<body>
    <form id="form1" runat="server">
        <nav class="navbar-inverse" role="navigation">
      <div class="container-fluid">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <a class="navbar-brand" href="https://www.temple.edu/">Temple</a>
        </div>
        <div class="collapse navbar-collapse" id="myNavbar">
          <ul class="nav navbar-nav">
            <li><asp:LinkButton ID="linkHomePage" href="HomePage.aspx" runat="server" style="color: floralwhite;">Home</asp:LinkButton></li>
          </ul>
          <ul class="nav navbar-nav navbar-right">
            <li><asp:LinkButton ID="linkLogout" onclick="Button1_Click" runat="server" style="color: floralwhite;"><span class="glyphicon glyphicon-log-out"></span>Logout</asp:LinkButton></li>
          </ul>
        </div>
      </div>
    </nav>

        <div id="settings">
            <h3>Settings</h3>
            <h4>Login</h4>
            <asp:DropDownList ID="ddLoginSettings" runat="server">
                <asp:ListItem>Fast-Login</asp:ListItem>
                <asp:ListItem>Auto-Login</asp:ListItem>
                <asp:ListItem>None</asp:ListItem>
            </asp:DropDownList>
            <h4>Photos</h4>
            <asp:DropDownList ID="ddPhotosSettings" runat="server">
                <asp:ListItem>Public</asp:ListItem>
                <asp:ListItem>Only Friends</asp:ListItem>
                <asp:ListItem>Friends and Friends of Friends</asp:ListItem>
            </asp:DropDownList>
            <h4>Profile Info</h4>
            <asp:DropDownList ID="ddPProfileSettings0" runat="server">
                <asp:ListItem>Public</asp:ListItem>
                <asp:ListItem>Only Friends</asp:ListItem>
                <asp:ListItem>Friends and Friends of Friends</asp:ListItem>
            </asp:DropDownList>
            <h4>Personal Contact Info</h4>
            <asp:DropDownList ID="ddContactSettings" runat="server">
                <asp:ListItem>Public</asp:ListItem>
                <asp:ListItem>Only Friends</asp:ListItem>
                <asp:ListItem>Friends and Friends of Friends</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Button ID="btnSaveSettings" runat="server" Text="Save Settings" OnClick="btnSaveSettings_Click" />
        </div>
    </form>
</body>
</html>
