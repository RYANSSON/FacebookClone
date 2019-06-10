<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FakeFacebook.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <link rel="stylesheet" href="TermProject.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <div id="Login">

            <div id="title">
                <asp:Label ID="lblTitle" runat="server" Text="Book of Faces"></asp:Label>
            </div>

            <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server" style="color: black;"></asp:TextBox>
            <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" style="color: black;"></asp:TextBox>
            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" style="color: black;" />
            <br />
            <asp:LinkButton ID="linkbtnForgot" onclick="lblForgot_Click" runat="server" style="color: floralwhite;">Forgot Password?</asp:LinkButton>

            <div id="invalidLogin" class="alert alert-danger" runat="server">
                <strong>Denied!</strong> Your Username or Password is Incorrect
            </div>
             
        </div>
        
        <div id="Register">
            <div id="SecurityQuestions" runat="server" style="margin-left: -8%; margin-bottom: 3%;">
                <h1><asp:Label ID="lblQuestionsInstructions" runat="server" style="font-size: 25px;" Text="Please Fill Out The Security Questions"></asp:Label></h1>
                <br />
            <asp:Label ID="lblQuestion1" runat="server" Text="What was the name of your first pet?"></asp:Label>
            <asp:TextBox ID="txtQ1" runat="server"></asp:TextBox><br />
            <asp:Label ID="lblQuestion2" runat="server" Text="What was the Last name of your least favorite teacher?"></asp:Label>
            <asp:TextBox ID="txtQ2" runat="server"></asp:TextBox><br />
            <asp:Label ID="lblQuestion3" runat="server" Text="What was the name of your childhood best friend?"></asp:Label>
            <asp:TextBox ID="txtQ3" runat="server"></asp:TextBox><br />
            <asp:Button ID="btnAnswerQ" runat="server" Text="Submit" OnClick="btnAnswerQ_Click" style="margin-left: 18%;"></asp:Button>
            </div>

            <h1><asp:Label ID="lblSignupInstructions" runat="server" style="font-size: 25px;" Text="Sign Up (All Fields Required)"></asp:Label></h1>
                <br />

            <asp:TextBox ID="txtFirstName" runat="server" placeholder="First Name" style="margin-bottom: 1%;margin-right: 1%;"></asp:TextBox>
            
            <asp:TextBox ID="txtLastName" runat="server" placeholder="Last name"></asp:TextBox>
            <br />
            <asp:TextBox ID="txtRegEmail" runat="server" placeholder="Email" style="margin-bottom: 1%;margin-right: 1%;"></asp:TextBox>

            <asp:TextBox ID="txtRegPassword" runat="server" placeholder="Password"></asp:TextBox>
            <br />
            <asp:TextBox ID="txtPhone" runat="server" placeholder="Phone number"></asp:TextBox>
            <asp:TextBox ID="txtCity" runat="server" placeholder="City"></asp:TextBox>
            <br />
            <asp:TextBox ID="txtState" runat="server" placeholder="State"></asp:TextBox>
            <asp:TextBox ID="txtOrganization" runat="server" placeholder="Organization"></asp:TextBox><br />

            <asp:Button ID="btnSignUp" runat="server" text="Sign Up" OnClick="btnSignUp_Click" style="color: black;" />

            <div id="invalidRegister" class="alert alert-danger" runat="server">
                <strong>Denied!</strong> You left something blank
            </div>
            <div id="duplicateRecord" class="alert alert-danger" runat="server">
                <strong>Denied!</strong> This email is already registered
            </div>

        </div>
        
        <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
    </form>
</body>
</html>
