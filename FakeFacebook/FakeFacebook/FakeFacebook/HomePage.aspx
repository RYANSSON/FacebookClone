<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="FakeFacebook.HomePage" %>

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
            <li><asp:LinkButton ID="linkSettings" onclick="btnSettings_Click" runat="server" style="color: floralwhite;">Settings</asp:LinkButton></li>
          </ul>
          <ul class="nav navbar-nav navbar-right">
            <li><asp:LinkButton ID="linkLogout" onclick="Button1_Click" runat="server" style="color: floralwhite;"><span class="glyphicon glyphicon-log-out"></span>Logout</asp:LinkButton></li>
          </ul>
        </div>
      </div>
    </nav>

        <div runat="server" id="greeting" style="padding-bottom: 2%;">
            <asp:Label ID="lblGreeting" runat="server" Text="Hello!" style="font-size: 25px;margin-left: 3%;color: floralwhite;"></asp:Label>
           
            
            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
            <asp:Button ID="btnSearchbyName" runat="server" Text="Search Person" OnClick="btnSearchbyName_Click" />
            <asp:Button ID="btnSearchbyLocation" runat="server" Text="Search People By Location" OnClick="btnSearchbyLocation_Click" />
            
            <asp:Button ID="btnSearchbyOrganization" runat="server" Text="Search by Organization" OnClick="btnSearchbyOrganization_Click" />
           
            <asp:LinkButton ID="lnkMyProfile" runat="server" OnClick="lnkMyAccount_Click">My Profile   </asp:LinkButton>
            <asp:LinkButton ID="lnkHome" runat="server" OnClick="lnkHome_Click">Home   </asp:LinkButton>
            <asp:LinkButton ID="lnkFriends" runat="server" OnClick="lnkFriends_Click">Friends   </asp:LinkButton>
            
         </div>
         
        <div runat="server" id="Search" visible="false" name="Search">
            <asp:GridView ID="gvSearch" runat="server" AutoGenerateColumns="False" OnRowCommand="gvSearch_RowCommand" >
            <Columns>
              
                <asp:BoundField DataField="Email" HeaderText="Email"/>
                <asp:BoundField DataField="First" HeaderText="First"/>
                <asp:BoundField DataField="Last" HeaderText="Last" />
                

                <asp:ButtonField Text="Add Friend" HeaderText=""  CommandName="AddFriend"  />
                <asp:ButtonField Text="Remove Friend" HeaderText=""  CommandName="RemoveFriend"  />
              
        
            </Columns>
                </asp:GridView>
                </div>
                <div runat="server" id="friends" visible ="false" name="friends">

        
                
            
            <asp:GridView ID="gvFriendRequests" runat="server"  OnRowCommand="gvFriendRequests_RowCommand" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Person2" HeaderText="Email"/>
                    
                    <asp:ButtonField Text="Accept" HeaderText=""  CommandName="AcceptFriendRequest" />
                    <asp:ButtonField Text="Decline" HeaderText=""  CommandName="DeclineFriendRequest"/>
                </Columns>
            </asp:GridView>
                    <asp:GridView ID="gvFriends" OnRowCommand="gvFriends_RowCommand" AutoGenerateColumns="False" runat="server">
                <Columns>
                    <asp:BoundField DataField="Email" HeaderText="Email"/>
                    <asp:BoundField DataField="First" HeaderText="First"/>
                    <asp:BoundField DataField="Last" HeaderText="Last"/>
                    
                    <asp:ButtonField Text="View Profile" HeaderText=""  CommandName="ViewProfile" />
                    <asp:ButtonField Text="Send Message" HeaderText=""  CommandName="SendMessage" />
                   
                    <asp:ButtonField Text="Subscribe"  HeaderText=""  CommandName="Subscribe" />
                    <asp:ButtonField Text="UnSubscribe"  HeaderText=""  CommandName="UnSubscribe" />
                   
                </Columns>
            </asp:GridView>
        </div>
        <div runat="server" name="NewsFeed" visible ="true" id="NewsFeed">
            <asp:TextBox ID="txtContentNF" placeholder="Write post here" CssClass="content" runat="server" MaxLength="100"></asp:TextBox>
           
            <asp:Label ID="lblError" runat="server" Text="Label"></asp:Label>
            <asp:Button ID="btnUpload" runat="server" Text="Post" OnClick="btnUpload_Click" />
            <br />

            <asp:GridView ID="gvNewsFeed" runat="server"></asp:GridView>
            
            
        </div>
        <div runat="server" id="profile" visible ="false" name="profile">


            <asp:Label ID="lblName" runat="server" Text="Name: "></asp:Label>
            <asp:Label ID="lblShowName" runat="server" Text=""></asp:Label> <br />
            <asp:Label ID="lblEmail" runat="server" Text="Email: "></asp:Label>
            <asp:Label ID="lblShowEmail" runat="server" Text=""></asp:Label> <br />
            <asp:Label ID="lblCity" runat="server" Text="City: "></asp:Label>
            <asp:Label ID="lblShowCity" runat="server" Text=""></asp:Label><br />
            <asp:Label ID="lblPhone" runat="server" Text="Phone #: "></asp:Label>
            <asp:Label ID="lblShowPhone" runat="server" Text=""></asp:Label><br />
            <asp:Label ID="lblState" runat="server" Text="State: "></asp:Label>
            <asp:Label ID="lblShowState" runat="server" Text=""></asp:Label><br />
            <asp:Label ID="lblOrganization" runat="server" Text="Organization: "></asp:Label>
            <asp:Label ID="lblShowOrganization" runat="server" Text=""></asp:Label>
            
            <br />
            <asp:Button ID="btnUpdateInfo" runat="server" Text="Update Info" Visible="false" OnClick="btnUpdateInfo_Click" />
            <br />
            <br />
            <div id="Info" name="info" runat="server" visible="false">
            <asp:TextBox ID="txtFirstName" runat="server" placeholder="First Name" style="margin-bottom: 1%;margin-right: 1%;"></asp:TextBox>
            
            <asp:TextBox ID="txtLastName" runat="server" placeholder="Last name"></asp:TextBox>
            <br />
         
            <asp:TextBox ID="txtPhone" runat="server" placeholder="Phone number" style="margin-bottom: 1%;margin-right: 1%;"></asp:TextBox>
            <asp:TextBox ID="txtCity" runat="server" placeholder="City"></asp:TextBox>
            <br />
            <asp:TextBox ID="txtState" runat="server" placeholder="State"></asp:TextBox>
            <asp:TextBox ID="txtOrganization" runat="server" placeholder="Organization"></asp:TextBox><br />
                <asp:Button ID="btnSaveInfo" runat="server" Text="Save Info" OnClick="btnSaveInfo_Click" />
                <br />
            </div>
            <asp:FileUpload ID="FileUploadP" runat="server" />
            <asp:TextBox ID="txtTitleP" placeholder="ImageTitle" runat="server"></asp:TextBox>
            
            <asp:Button ID="btnUploadImage" runat="server" Text="Add Photo" OnClick="btnUploadImage_Click" />
            
            <style>.content {
                }</style>
            <br />
            <asp:TextBox ID="txtContentP" placeholder="Write post here" CssClass="content" runat="server" MaxLength="100"></asp:TextBox>
           
            <asp:Button ID="btnPost" runat="server" Text="Post" OnClick="btnPost_Click" />
            <br />
            <asp:Button ID="btnViewPhotos" runat="server" Text="View Photos" OnClick="btnViewPhotos_Click" />
            <asp:Button ID="btnHidePhotos" runat="server" Text="Hide Photos" OnClick="btnHidePhotos_Click" />
            <center> 
         <asp:GridView ID="gvPhotos" runat="server" Visible="false"  AutoGenerateColumns="False"
            style="z-index: 1; left: 273px; top: 151px; position: absolute; height: 528px; width: 390px">
            <Columns>
                <asp:BoundField DataField="imageTitle" HeaderText="Title">
                   <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:ImageField DataImageUrlField="imgFile" HeaderText="Product Photo">
                    <ItemStyle Height="100px" HorizontalAlign="Center" Width="120px" />
                </asp:ImageField>
            </Columns>
        </asp:GridView>

                <asp:GridView ID="gvWall"  runat="server"></asp:GridView>
            </center>
        </div>
        <div id="Messages" name="Messages" runat="server" visible="false">
            <asp:Label ID="lblReciver" runat="server" Text=""></asp:Label>
            <asp:GridView ID="gvMessages"  AutoGenerateColumns="false" OnRowCommand="gvMessages_RowCommand" runat="server">
                <Columns>
                    <asp:BoundField DataField="Sender" HeaderText="Sender" />
                    <asp:BoundField DataField="Reciever" HeaderText="Receiver" />
                    <asp:BoundField DataField="Message" HeaderText="Message"/>
                    <asp:ButtonField Text="Delete" HeaderText=""  CommandName="DeleteMessage"/>
                </Columns>
            </asp:GridView>
            <asp:TextBox ID="txtMessage" placeholder="Write Message here" CssClass="content" runat="server" MaxLength="100" Width="229px"></asp:TextBox>
           
            <asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" />
            <br />
            <asp:Button ID="btnDeleteConversation" runat="server" Text="Delete Conversation" OnClick="btnDeleteConversation_Click" />
            <br />

        </div>
        
    </form>
</body>
</html>
