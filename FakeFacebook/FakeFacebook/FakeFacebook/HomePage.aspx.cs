using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;     // needed for the encryption classes
using System.IO;                        // needed for the MemoryStream
using System.Text;                      // needed for the UTF8 encoding
using System.Net;                       // needed for the cookie

using System.Web.Script.Serialization;  // needed for JSON serializers
namespace FakeFacebook
{
    public partial class HomePage : System.Web.UI.Page
    {
        static String Email;

        DBConnect objDB = new DBConnect();
        string strSQL;
        JavaScriptSerializer js = new JavaScriptSerializer();
        SqlCommand AddFriend = new SqlCommand();
        SqlCommand DELETEFriend = new SqlCommand();
        SqlCommand ChecksFriendshship = new SqlCommand();
        SqlCommand FriendRequests = new SqlCommand();
        SqlCommand ConfirmFriendReq = new SqlCommand();
        SqlCommand Subscribe = new SqlCommand();
        SqlCommand UnSubscribe = new SqlCommand();
        SqlCommand GetMessages = new SqlCommand();
        SqlCommand SendMessage = new SqlCommand();
        SqlCommand DeleteMessage = new SqlCommand();
        SqlCommand DeleteAllMessages = new SqlCommand();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["LoginCookie"] != null)
            {
                HttpCookie myCookie = Request.Cookies["LoginCookie"];
                Email = myCookie.Values["Email"];
                lblGreeting.Text = "Hello " + Email;
                if (!IsPostBack)
                {
                    View("NewsFeed");
                }
                
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }
        // Uploads an image file and stores it in the database as a binary object for a specific product

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            
            DBConnect objDB = new DBConnect();
            SqlCommand MakePost = new SqlCommand();
            try

            {
                // Use the FileUpload control to get the uploaded data
 
                    MakePost.CommandText = "TP_MakePost";
                    MakePost.CommandType = CommandType.StoredProcedure;
                    MakePost.Parameters.AddWithValue("@EmailWriter", Email);
                    MakePost.Parameters.AddWithValue("@EmailOwner", Email);
                    MakePost.Parameters.AddWithValue("@Content", txtContentNF.Text);
                    
                    MakePost.Parameters.AddWithValue("@Date", DateTime.Now);
                    objDB.DoUpdateUsingCmdObj(MakePost);
            }

            catch (Exception ex)
            {

                lblError.Text = "Error ocurred: [" + ex.Message + "]";

            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            HttpCookie myCookie = Request.Cookies["LoginCookie"];
            myCookie.Values["Email"] = Email;
            myCookie.Values["Password"] = "";
            myCookie.Expires = new DateTime(2020, 2, 1);
            Response.Cookies.Add(myCookie);
            Response.Redirect("Login.aspx");
        }

        protected void btnSettings_Click(object sender, EventArgs e)
        {
            Response.Redirect("Settings.aspx");
        }

        protected void btnSearchbyName_Click(object sender, EventArgs e)
        {
            View("Search");

            if (txtSearch.Text == "" || txtSearch.Text == " ")
            {
                return;
            }
            WebRequest findPeoplebyName = WebRequest.Create("http://cis-iis2.temple.edu/Fall2018/CIS3342_tuf97565/TermProjectWS/api/socialnetworkservice/FindUsersByName/" + txtSearch.Text);
            //WebRequest findPeoplebyName = WebRequest.Create("http://localhost:4004/api/socialnetworkservice/FindUsersByName/" + txtSearch.Text);

            WebResponse responseUsers = findPeoplebyName.GetResponse();

            Stream theDataStream1 = responseUsers.GetResponseStream();
            StreamReader reader1 = new StreamReader(theDataStream1);
            String found = reader1.ReadToEnd();
            reader1.Close();
            responseUsers.Close();

            // Deserialize a JSON string into a Team object.
            List<UserOBJ> userList = js.Deserialize<List<UserOBJ>>(found);

            gvSearch.DataSource = userList;
            gvSearch.DataBind();

        }
        protected void gvSearch_RowCommand(Object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {

            // Get the index of the row that a command was issued on

            int rowIndex = int.Parse(e.CommandArgument.ToString());

            ChecksFriendshship.Parameters.Clear();
            ChecksFriendshship.CommandText = "TP_ChecksFriendship";

            ChecksFriendshship.CommandType = CommandType.StoredProcedure;
            ChecksFriendshship.Parameters.AddWithValue("@Email1", Email);
            ChecksFriendshship.Parameters.AddWithValue("@Email2", gvSearch.Rows[rowIndex].Cells[0].Text);
            SqlParameter outputPar = new SqlParameter("@found", SqlDbType.Int, 100);
            outputPar.Direction = ParameterDirection.Output;
            ChecksFriendshship.Parameters.Add(outputPar);

            objDB.GetDataSetUsingCmdObj(ChecksFriendshship);
            string friendstatus = ChecksFriendshship.Parameters["@found"].Value.ToString();

            if (e.CommandName == "AddFriend")
            {
                if (friendstatus == "0")
                {
                    if (Email == gvSearch.Rows[rowIndex].Cells[0].Text)
                    {
                        return;
                    }
                    AddFriend.CommandText = "TP_SendFriendRequest";

                    AddFriend.CommandType = CommandType.StoredProcedure;
                    AddFriend.Parameters.AddWithValue("@Email1", Email);
                    AddFriend.Parameters.AddWithValue("@Email2", gvSearch.Rows[rowIndex].Cells[0].Text);
                    AddFriend.Parameters.AddWithValue("@Sender", Email);

                    objDB.DoUpdateUsingCmdObj(AddFriend);
                    AddFriend.Parameters.Clear();
                    AddFriend.Parameters.AddWithValue("@Email2", Email);
                    AddFriend.Parameters.AddWithValue("@Email1", gvSearch.Rows[rowIndex].Cells[0].Text);
                    AddFriend.Parameters.AddWithValue("@Sender", Email);
                    objDB.DoUpdateUsingCmdObj(AddFriend);
                }


            }
            if (e.CommandName == "RemoveFriend")
            {
                if (friendstatus == "1")
                {

                    DELETEFriend.CommandText = "TP_RemoveFriend";

                    DELETEFriend.CommandType = CommandType.StoredProcedure;
                    DELETEFriend.Parameters.AddWithValue("@Email1", Email);
                    DELETEFriend.Parameters.AddWithValue("@Email2", gvSearch.Rows[rowIndex].Cells[0].Text);


                    objDB.DoUpdateUsingCmdObj(DELETEFriend);
                    DELETEFriend.Parameters.Clear();
                    DELETEFriend.Parameters.AddWithValue("@Email2", Email);
                    DELETEFriend.Parameters.AddWithValue("@Email1", gvSearch.Rows[rowIndex].Cells[0].Text);
                    objDB.DoUpdateUsingCmdObj(DELETEFriend);
                }


            }

        }
        protected void gvFriends_RowCommand(Object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            HttpCookie myCookie = Request.Cookies["LoginCookie"];
            if (myCookie == null)
            {
                return;
            }
            String token = myCookie.Values["Password"];
            int rowIndex = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "ViewProfile")
            {
                View("Profile");
                String url = "http://cis-iis2.temple.edu/Fall2018/CIS3342_tuf97565/TermProjectWS/api/SocialNetworkService/GetProfile?requestingEmail=" + Email + "&requestedEmail=" + gvFriends.Rows[rowIndex].Cells[0].Text + "&token=" + token;
                
                //String url = "http://localhost:4004/api/SocialNetworkService/GetProfile?requestingEmail=" + Email + "&requestedEmail=" + gvFriends.Rows[rowIndex].Cells[0].Text + "&token=" + token;

                Info.Visible = false;
                btnUpdateInfo.Visible = false;

                //GetFriends/{RequestingUsername}/{RequestedUsername}/{VerificationToken}
                WebRequest findPeoplebyName = WebRequest.Create(url);

                WebResponse responseUsers = findPeoplebyName.GetResponse();

                Stream theDataStream1 = responseUsers.GetResponseStream();
                StreamReader reader1 = new StreamReader(theDataStream1);
                String found = reader1.ReadToEnd();
                reader1.Close();
                responseUsers.Close();

                // Deserialize a JSON string into a Team object.
                UserOBJ user = js.Deserialize<UserOBJ>(found);

                lblShowName.Text = user.First + " " + user.Last;
                lblShowEmail.Text = user.Email;
                lblShowOrganization.Text = user.Organization;
                lblShowCity.Text = user.City;
                lblShowPhone.Text = user.Phone;
                lblShowState.Text = user.State;
                String url2 = "http://cis-iis2.temple.edu/Fall2018/CIS3342_tuf97565/TermProjectWS/api/SocialNetworkService/GetWall?requestingEmail=" + Email + "&requestedEmail=" + lblShowEmail.Text + "&token=" + token;
                //String url2 = "http://localhost:4004/api/SocialNetworkService/GetWall?requestingEmail=" + Email + "&requestedEmail=" + lblShowEmail.Text + "&token=" + token;


                //GetFriends/{RequestingUsername}/{RequestedUsername}/{VerificationToken}
                WebRequest getWall = WebRequest.Create(url2);

                WebResponse responseWall = getWall.GetResponse();

                Stream theDataStream2 = responseWall.GetResponseStream();
                StreamReader reader2 = new StreamReader(theDataStream2);
                String found2 = reader2.ReadToEnd();
                reader2.Close();
                responseWall.Close();

                // Deserialize a JSON string into a Team object.
                List<PostOBJ> myWall = js.Deserialize<List<PostOBJ>>(found2);
                gvWall.DataSource = myWall;
                gvWall.DataBind();

            }
            if (e.CommandName == "SendMessage")
            {
                View("Messages");
                lblReciver.Text = gvFriends.Rows[rowIndex].Cells[0].Text;
                GetMessages.CommandText = "TP_GetMessages";
                GetMessages.CommandType = CommandType.StoredProcedure;
                GetMessages.Parameters.AddWithValue("@Email1", Email);
                GetMessages.Parameters.AddWithValue("@Email2", gvFriends.Rows[rowIndex].Cells[0].Text);
                gvMessages.DataSource = objDB.GetDataSetUsingCmdObj(GetMessages);
                gvMessages.DataBind();

            }
            if (e.CommandName == "Subscribe")
            {

                Subscribe.CommandText = "TP_Subscribe";
                Subscribe.CommandType = CommandType.StoredProcedure;
                Subscribe.Parameters.AddWithValue("@Email1", Email);
                Subscribe.Parameters.AddWithValue("@Email2", gvFriends.Rows[rowIndex].Cells[0].Text);
           
                objDB.DoUpdateUsingCmdObj(Subscribe);

            }
            if (e.CommandName == "UnSubscribe")
            {

                UnSubscribe.CommandText = "TP_UnSubscribe";
                UnSubscribe.CommandType = CommandType.StoredProcedure;
                UnSubscribe.Parameters.AddWithValue("@Email1", Email);
                UnSubscribe.Parameters.AddWithValue("@Email2", gvFriends.Rows[rowIndex].Cells[0].Text);

                objDB.DoUpdateUsingCmdObj(UnSubscribe);
            }
        }
        protected void gvMessages_RowCommand(Object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            HttpCookie myCookie = Request.Cookies["LoginCookie"];
            if (myCookie == null)
            {
                return;
            }
            String token = myCookie.Values["Password"];
            int rowIndex = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "DeleteMessage")
            {
                DeleteMessage.CommandText = "TP_DeleteMessage";
                DeleteMessage.CommandType = CommandType.StoredProcedure;
                DeleteMessage.Parameters.AddWithValue("@Email1", gvMessages.Rows[rowIndex].Cells[0].Text);
                DeleteMessage.Parameters.AddWithValue("@Email2", gvMessages.Rows[rowIndex].Cells[1].Text);
                DeleteMessage.Parameters.AddWithValue("@Message", gvMessages.Rows[rowIndex].Cells[2].Text);
                objDB.DoUpdateUsingCmdObj(DeleteMessage);
                GetMessages.CommandText = "TP_GetMessages";
                GetMessages.CommandType = CommandType.StoredProcedure;
                GetMessages.Parameters.AddWithValue("@Email1", Email);
                GetMessages.Parameters.AddWithValue("@Email2", lblReciver.Text);
                gvMessages.DataSource = objDB.GetDataSetUsingCmdObj(GetMessages);
                gvMessages.DataBind();
            }
        }
        protected void gvFriendRequests_RowCommand(Object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {

            int rowIndex = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "AcceptFriendRequest")
            {
                ConfirmFriendReq.CommandText = "TP_ConfirmFriendRequest";

                ConfirmFriendReq.CommandType = CommandType.StoredProcedure;
                ConfirmFriendReq.Parameters.AddWithValue("@Email1", Email);
                ConfirmFriendReq.Parameters.AddWithValue("@Email2", gvFriendRequests.Rows[rowIndex].Cells[0].Text);


                objDB.DoUpdateUsingCmdObj(ConfirmFriendReq);
                ConfirmFriendReq.Parameters.Clear();
                ConfirmFriendReq.Parameters.AddWithValue("@Email2", Email);
                ConfirmFriendReq.Parameters.AddWithValue("@Email1", gvFriendRequests.Rows[rowIndex].Cells[0].Text);
                objDB.DoUpdateUsingCmdObj(ConfirmFriendReq);

            }
            if (e.CommandName == "DeclineFriendRequest")
            {


                DELETEFriend.CommandText = "TP_RemoveFriend";

                DELETEFriend.CommandType = CommandType.StoredProcedure;
                DELETEFriend.Parameters.AddWithValue("@Email1", Email);
                DELETEFriend.Parameters.AddWithValue("@Email2", gvFriendRequests.Rows[rowIndex].Cells[0].Text);


                objDB.DoUpdateUsingCmdObj(DELETEFriend);
                DELETEFriend.Parameters.Clear();
                DELETEFriend.Parameters.AddWithValue("@Email2", Email);
                DELETEFriend.Parameters.AddWithValue("@Email1", gvFriendRequests.Rows[rowIndex].Cells[0].Text);
                objDB.DoUpdateUsingCmdObj(DELETEFriend);

            }
        }

        protected void lnkMyAccount_Click(object sender, EventArgs e)
        {
            View("Profile");
            btnUpdateInfo.Visible = true;
            HttpCookie myCookie = Request.Cookies["LoginCookie"];
            if (myCookie == null)
            {
                return;
            }
            String token = myCookie.Values["Password"];
            String myEmail = myCookie.Values["Email"];
            String url = "http://cis-iis2.temple.edu/Fall2018/CIS3342_tuf97565/TermProjectWS/api/SocialNetworkService/GetProfile?requestingEmail=" + myEmail + "&requestedEmail=" + myEmail + "&token=" + token;
            //String url = "http://localhost:4004/api/SocialNetworkService/GetProfile?requestingEmail=" + myEmail + "&requestedEmail=" + myEmail + "&token=" + token;


            //GetFriends/{RequestingUsername}/{RequestedUsername}/{VerificationToken}
            WebRequest findPeoplebyName = WebRequest.Create(url);

            WebResponse responseUsers = findPeoplebyName.GetResponse();

            Stream theDataStream1 = responseUsers.GetResponseStream();
            StreamReader reader1 = new StreamReader(theDataStream1);
            String found = reader1.ReadToEnd();
            reader1.Close();
            responseUsers.Close();

            // Deserialize a JSON string into a Team object.
            UserOBJ user = js.Deserialize<UserOBJ>(found);

            lblShowName.Text = user.First + " " + user.Last;
            lblShowEmail.Text = user.Email;
            lblShowOrganization.Text = user.Organization;
            lblShowCity.Text = user.City;
            lblShowPhone.Text = user.Phone;
            lblShowState.Text = user.State;

            String url2 = "http://cis-iis2.temple.edu/Fall2018/CIS3342_tuf97565/TermProjectWS/api/SocialNetworkService/GetWall?requestingEmail=" + myEmail + "&requestedEmail=" + myEmail + "&token=" + token;
            //String url2 = "http://localhost:4004/api/SocialNetworkService/GetWall?requestingEmail=" + myEmail + "&requestedEmail=" + myEmail + "&token=" + token;

            //GetFriends/{RequestingUsername}/{RequestedUsername}/{VerificationToken}
            WebRequest getWall = WebRequest.Create(url2);

            WebResponse responseWall = getWall.GetResponse();

            Stream theDataStream2 = responseWall.GetResponseStream();
            StreamReader reader2 = new StreamReader(theDataStream2);
            String found2 = reader2.ReadToEnd();
            reader2.Close();
            responseWall.Close();

            // Deserialize a JSON string into a Team object.
            List<PostOBJ> myWall = js.Deserialize<List<PostOBJ>>(found2);
            gvWall.DataSource = myWall;
            gvWall.DataBind();
            
        }

        protected void lnkFriends_Click(object sender, EventArgs e)
        {
            View("Friends");

            HttpCookie myCookie = Request.Cookies["LoginCookie"];
            if (myCookie == null)
            {
                return;
            }
            String token = myCookie.Values["Password"];

            FriendRequests.CommandText = "TP_FindRequests";

            FriendRequests.CommandType = CommandType.StoredProcedure;
            FriendRequests.Parameters.AddWithValue("@Email", Email);
            gvFriendRequests.DataSource = objDB.GetDataSetUsingCmdObj(FriendRequests);
            gvFriendRequests.DataBind();


            //GetFriends/{requestingEmail}/{requestedEmail}/{Token}
            String url = "http://cis-iis2.temple.edu/Fall2018/CIS3342_tuf97565/TermProjectWS/api/SocialNetworkService/GetFriends?requestingEmail=" + Email + "&requestedEmail=" + Email + "&token=" + token;
            //String url = "http://localhost:4004/api/SocialNetworkService/GetFriends?requestingEmail=" + Email + "&requestedEmail=" + Email + "&token=" + token;

            //GetFriends/{RequestingUsername}/{RequestedUsername}/{VerificationToken}
            WebRequest findPeoplebyName = WebRequest.Create(url);

            WebResponse responseUsers = findPeoplebyName.GetResponse();

            Stream theDataStream1 = responseUsers.GetResponseStream();
            StreamReader reader1 = new StreamReader(theDataStream1);
            String found = reader1.ReadToEnd();
            reader1.Close();
            responseUsers.Close();

            // Deserialize a JSON string into a Team object.
            List<UserOBJ> userList = js.Deserialize<List<UserOBJ>>(found);

            gvFriends.DataSource = userList;
            gvFriends.DataBind();


        }
        protected void lnkHome_Click(object sender, EventArgs e)
        {
            View("NewsFeed");

            HttpCookie myCookie = Request.Cookies["LoginCookie"];
            if (myCookie == null)
            {
                return;
            }
            String token = myCookie.Values["Password"];

            //GetFriends/{requestingEmail}/{requestedEmail}/{Token}
            String url = "http://cis-iis2.temple.edu/Fall2018/CIS3342_tuf97565/TermProjectWS/api/SocialNetworkService/GetNewsFeed?requestingEmail=" + Email + "&requestedEmail=" + Email + "&token=" + token;
            //String url = "http://localhost:4004/api/SocialNetworkService/GetNewsFeed?requestingEmail=" + Email + "&requestedEmail=" + Email + "&token=" + token;

            //GetFriends/{RequestingUsername}/{RequestedUsername}/{VerificationToken}
            WebRequest getNewsFeed = WebRequest.Create(url);

            WebResponse responseUsers = getNewsFeed.GetResponse();

            Stream theDataStream1 = responseUsers.GetResponseStream();
            StreamReader reader1 = new StreamReader(theDataStream1);
            String found = reader1.ReadToEnd();
            reader1.Close();
            responseUsers.Close();

            // Deserialize a JSON string into a Team object.
            List<PostOBJ> News = js.Deserialize<List<PostOBJ>>(found);

            gvNewsFeed.DataSource = News;
            gvNewsFeed.DataBind();


        }
        protected void View(string section)
        {
            if (section == "NewsFeed")
            {
                NewsFeed.Visible = true;
                Search.Visible = false;
                profile.Visible = false;
                friends.Visible = false;
                Messages.Visible = false;
            }
            if (section == "Search")
            {
                NewsFeed.Visible = false;
                Search.Visible = true;
                profile.Visible = false;
                friends.Visible = false;
                Messages.Visible = false;
            }
            if (section == "Profile")
            {
                NewsFeed.Visible = false;
                Search.Visible = false;
                profile.Visible = true;
                friends.Visible = false;
                Messages.Visible = false;
            }
            if (section == "Friends")
            {
                Messages.Visible = false;
                NewsFeed.Visible = false;
                Search.Visible = false;
                profile.Visible = false;
                friends.Visible = true;
            }
            if (section == "Messages")
            {
                Messages.Visible = true;
                NewsFeed.Visible = false;
                Search.Visible = false;
                profile.Visible = false;
                friends.Visible = false;
            }
            
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            SqlCommand MakePost = new SqlCommand();
            MakePost.CommandText = "TP_MakePost";
            MakePost.CommandType = CommandType.StoredProcedure;
            MakePost.Parameters.AddWithValue("@EmailWriter", Email);
            MakePost.Parameters.AddWithValue("@EmailOwner", lblShowEmail.Text);
            MakePost.Parameters.AddWithValue("@Content", txtContentP.Text);
            MakePost.Parameters.AddWithValue("@Date", DateTime.Now);
            objDB.DoUpdateUsingCmdObj(MakePost);

        }

        protected void btnUploadImage_Click(object sender, EventArgs e)
        {
            
            DBConnect objDB = new DBConnect();
            SqlCommand SaveImage = new SqlCommand();
            
            int result = 0, imageSize;
            string fileExtension, imageType, imageName, imageTitle;
            try

            {
                // Use the FileUpload control to get the uploaded data

                if (FileUploadP.HasFile)
                {

                    imageSize = FileUploadP.PostedFile.ContentLength;

                    byte[] imageData = new byte[imageSize];

                    FileUploadP.PostedFile.InputStream.Read(imageData, 0, imageSize);
                    imageName = FileUploadP.PostedFile.FileName;
                    imageType = FileUploadP.PostedFile.ContentType;

                    if (txtTitleP.Text != "")

                        imageTitle = txtTitleP.Text;

                    else

                        imageTitle = "";

                    fileExtension = imageName.Substring(imageName.LastIndexOf("."));

                    fileExtension = fileExtension.ToLower();



                    if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".bmp" || fileExtension == ".gif")
                    {

                        // INSERT an image (BLOB) into the database using a stored procedure 'storeProductImage'

                        SaveImage.CommandText = "TP_StoreProductImage";

                        SaveImage.CommandType = CommandType.StoredProcedure;
                        SaveImage.Parameters.AddWithValue("@Email", Email);
                        SaveImage.Parameters.AddWithValue("@ImageTitle", imageTitle);

                        SaveImage.Parameters.AddWithValue("@URL", imageTitle);
                        result = objDB.DoUpdateUsingCmdObj(SaveImage);

                    }
                }

                else
                {

                    lblError.Text = "Only jpg, bmp, and gif file formats supported.";

                }

            }

            catch (Exception ex)

            {

                lblError.Text = "Error ocurred: [" + ex.Message + "] cmd=" + result;

            }
        }

        protected void btnViewPhotos_Click(object sender, EventArgs e)
        {
            gvPhotos.Visible = true;
            

            //gvPhotos.DataSource = ;
            //gvPhotos.DataBind();
        }

        protected void btnHidePhotos_Click(object sender, EventArgs e)
        {
            gvPhotos.Visible = false;
        }

        protected void btnUpdateInfo_Click(object sender, EventArgs e)
        {
            if(Email == lblShowEmail.Text)
            {
                Info.Visible = true;
            }
            
        }

        protected void btnSaveInfo_Click(object sender, EventArgs e)
        {
            if(Email == lblShowEmail.Text)
            {
                SqlCommand UpdateInfo = new SqlCommand();
                UpdateInfo.CommandType = CommandType.StoredProcedure;
                UpdateInfo.CommandText = "TP_UpdateInfo";
                UpdateInfo.Parameters.AddWithValue("@Email", Email);
                UpdateInfo.Parameters.AddWithValue("@First", txtFirstName.Text);
                UpdateInfo.Parameters.AddWithValue("@Last", txtLastName.Text);
                UpdateInfo.Parameters.AddWithValue("@Phone", txtPhone.Text);
                UpdateInfo.Parameters.AddWithValue("@City", txtCity.Text);
                UpdateInfo.Parameters.AddWithValue("@State", txtState.Text);
                UpdateInfo.Parameters.AddWithValue("@Organization", txtOrganization.Text);
                objDB.DoUpdateUsingCmdObj(UpdateInfo);
            }

            Info.Visible = false;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
           
            SendMessage.CommandType = CommandType.StoredProcedure;
            SendMessage.CommandText = "TP_SendMessage";
            SendMessage.Parameters.AddWithValue("@Email1", Email);
            SendMessage.Parameters.AddWithValue("@Email2", lblReciver.Text);
            SendMessage.Parameters.AddWithValue("@Message", txtMessage.Text);
            objDB.DoUpdateUsingCmdObj(SendMessage);

            GetMessages.CommandText = "TP_GetMessages";
            GetMessages.CommandType = CommandType.StoredProcedure;
            GetMessages.Parameters.AddWithValue("@Email1", Email);
            GetMessages.Parameters.AddWithValue("@Email2", lblReciver.Text);
            gvMessages.DataSource = objDB.GetDataSetUsingCmdObj(GetMessages);
            gvMessages.DataBind();
            View("Messages");
        }

        protected void btnDeleteConversation_Click(object sender, EventArgs e)
        {
            DeleteAllMessages.CommandType = CommandType.StoredProcedure;
            DeleteAllMessages.CommandText = "TP_DeleteAllMessages";
            DeleteAllMessages.Parameters.AddWithValue("@Email1", Email);
            DeleteAllMessages.Parameters.AddWithValue("@Email2", lblReciver.Text);
            objDB.DoUpdateUsingCmdObj(DeleteAllMessages);
            DeleteAllMessages.Parameters.Clear();
            DeleteAllMessages.Parameters.AddWithValue("@Email1", lblReciver.Text);
            DeleteAllMessages.Parameters.AddWithValue("@Email2", Email);
            objDB.DoUpdateUsingCmdObj(DeleteAllMessages);
            GetMessages.CommandText = "TP_GetMessages";
            GetMessages.CommandType = CommandType.StoredProcedure;
            GetMessages.Parameters.AddWithValue("@Email1", Email);
            GetMessages.Parameters.AddWithValue("@Email2", lblReciver.Text);
            gvMessages.DataSource = objDB.GetDataSetUsingCmdObj(GetMessages);
            gvMessages.DataBind();

        }

        protected void btnSearchbyLocation_Click(object sender, EventArgs e)
        {
            View("Search");
            lblError.Text = "";
            if (txtSearch.Text == "" || txtSearch.Text == " ")
            {
                return;
            }
            string[] words = txtSearch.Text.Split(' ');
            if (words.Length == 0)
            {
                lblError.Text = "Please enter the city followed by a space and then the state";
            }
            if (words.Length == 1)
            {
                lblError.Text = "Please enter the city followed by a space and then the state";
            }
            if (words.Length == 2)
            {

                WebRequest findPeoplebyName = WebRequest.Create("http://cis-iis2.temple.edu/Fall2018/CIS3342_tuf97565/TermProjectWS/api/socialnetworkservice/FindUsersByLocation?city=" + words[0] + "&state=" + words[1]);
                // WebRequest findPeoplebyName = WebRequest.Create("http://localhost:4004/api/socialnetworkservice/FindUsersByLocation?city=" + words[0] + "&state=" + words[1]);
                WebResponse responseUsers = findPeoplebyName.GetResponse();

                Stream theDataStream1 = responseUsers.GetResponseStream();
                StreamReader reader1 = new StreamReader(theDataStream1);
                String found = reader1.ReadToEnd();
                reader1.Close();
                responseUsers.Close();

                // Deserialize a JSON string into a Team object.
                List<UserOBJ> userList = js.Deserialize<List<UserOBJ>>(found);

                gvSearch.DataSource = userList;
                gvSearch.DataBind();
            }
        }

        protected void btnSearchbyOrganization_Click(object sender, EventArgs e)
        {
            View("Search");

            if (txtSearch.Text == "" || txtSearch.Text == " ")
            {
                return;
            }
            WebRequest findPeoplebyName = WebRequest.Create("http://cis-iis2.temple.edu/Fall2018/CIS3342_tuf97565/TermProjectWS/api/socialnetworkservice/FindUsersByOrganization/" + txtSearch.Text);
            //WebRequest findPeoplebyName = WebRequest.Create("http://localhost:4004/api/socialnetworkservice/FindUsersByOrganization/" + txtSearch.Text);
            WebResponse responseUsers = findPeoplebyName.GetResponse();

            Stream theDataStream1 = responseUsers.GetResponseStream();
            StreamReader reader1 = new StreamReader(theDataStream1);
            String found = reader1.ReadToEnd();
            reader1.Close();
            responseUsers.Close();

            // Deserialize a JSON string into a Team object.
            List<UserOBJ> userList = js.Deserialize<List<UserOBJ>>(found);

            gvSearch.DataSource = userList;
            gvSearch.DataBind();
        }
    }
}