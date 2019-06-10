using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;              // import needed for DataSet and other data classes

using Utilities;
using System.Data.SqlClient;

using System.Security.Cryptography;     // needed for the encryption classes
using System.IO;                        // needed for the MemoryStream
using System.Text;                      // needed for the UTF8 encoding
using System.Net;                       // needed for the cookie

namespace FFAPI2.Controllers
{


    [Route("api/SocialNetworkService")]
    public class SocialNetworkServiceController : Controller
    {
        private Byte[] key = { 250, 101, 18, 76, 45, 135, 207, 118, 4, 171, 3, 168, 202, 241, 37, 199 };

        private Byte[] vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 252, 112, 79, 32, 114, 156 };

        [HttpGet("FindUsersByName/{name}")]

        public List<UserOBJ> FindUsersByName(string name)
        {
            List<UserOBJ> Users = new List<UserOBJ>();
            DBConnect objDB = new DBConnect();
            SqlCommand FindUsersByName = new SqlCommand();
            FindUsersByName.CommandType = CommandType.StoredProcedure;
            FindUsersByName.CommandText = "TP_FindUsersByName";
            string[] words = name.Split(' ');
            if (words.Length == 0)
            {

            }
            if (words.Length == 1)
            {
                FindUsersByName.Parameters.AddWithValue("@first", words[0]);
                FindUsersByName.Parameters.AddWithValue("@last", "");
            }
            if (words.Length == 2)
            {
                FindUsersByName.Parameters.AddWithValue("@first", words[0]);
                FindUsersByName.Parameters.AddWithValue("@last", words[1]);
            }



            DataSet ds = objDB.GetDataSetUsingCmdObj(FindUsersByName);



            foreach (DataRow record in ds.Tables[0].Rows)
            {
                UserOBJ thisUser = new UserOBJ();
                thisUser.First = record["First_Name"].ToString();
                thisUser.Last = record["Last_Name"].ToString();
                thisUser.Email = record["Email"].ToString();
                thisUser.City = record["City"].ToString();
                thisUser.State = record["State"].ToString();
                thisUser.Organization = record["Organization"].ToString();
                thisUser.ProfileURL = record["ProfilePhotoURL"].ToString();
                Users.Add(thisUser);
            }

            return Users;

        }
        [HttpGet("FindUsersByLocation")]

        public List<UserOBJ> FindUsersByLocation(string city, string state)
        {
            List<UserOBJ> Users = new List<UserOBJ>();
            DBConnect objDB = new DBConnect();
            SqlCommand FindUsersbyLocation = new SqlCommand();
            FindUsersbyLocation.CommandType = CommandType.StoredProcedure;
            FindUsersbyLocation.CommandText = "TP_FindUsersByLocation";
         
            FindUsersbyLocation.Parameters.AddWithValue("@city", city);
            FindUsersbyLocation.Parameters.AddWithValue("@state", state);

            DataSet ds = objDB.GetDataSetUsingCmdObj(FindUsersbyLocation);

            foreach (DataRow record in ds.Tables[0].Rows)
            {
                UserOBJ thisUser = new UserOBJ();
                thisUser.First = record["First_Name"].ToString();
                thisUser.Last = record["Last_Name"].ToString();
                thisUser.Email = record["Email"].ToString();
                thisUser.City = record["City"].ToString();
                thisUser.State = record["State"].ToString();
                thisUser.Organization = record["Organization"].ToString();
                thisUser.ProfileURL = record["ProfilePhotoURL"].ToString();
                Users.Add(thisUser);
            }

            return Users;

        }
        [HttpGet("FindUsersByOrganization/{Organization}")]

        public List<UserOBJ> FindUsersByOrganization(string Organization)
        {
            List<UserOBJ> Users = new List<UserOBJ>();
            DBConnect objDB = new DBConnect();
            SqlCommand FindUsersbyOrganization = new SqlCommand();
            FindUsersbyOrganization.CommandType = CommandType.StoredProcedure;
            FindUsersbyOrganization.CommandText = "TP_FindUsersByOrganization";
            
            FindUsersbyOrganization.Parameters.AddWithValue("@organization", Organization);
           
            DataSet ds = objDB.GetDataSetUsingCmdObj(FindUsersbyOrganization);

            foreach (DataRow record in ds.Tables[0].Rows)
            {
                UserOBJ thisUser = new UserOBJ();
                thisUser.First = record["First_Name"].ToString();
                thisUser.Last = record["Last_Name"].ToString();
                thisUser.Email = record["Email"].ToString();
                thisUser.City = record["City"].ToString();
                thisUser.State = record["State"].ToString();
                thisUser.Organization = record["Organization"].ToString();
                thisUser.ProfileURL = record["ProfilePhotoURL"].ToString();
                Users.Add(thisUser);
            }

            return Users;

        }

        [HttpGet("GetFriends")]
        public List<UserOBJ> GetFriends(String requestingEmail, String requestedEmail, String token)
        {

            DBConnect objDB = new DBConnect();
            List<UserOBJ> Users = new List<UserOBJ>();

            SqlCommand LoginUser = new SqlCommand();
            LoginUser.CommandType = CommandType.StoredProcedure;
            LoginUser.CommandText = "TP_FindUser";
            LoginUser.Parameters.AddWithValue("@Email", requestingEmail);
            LoginUser.Parameters.AddWithValue("@Password", decode(token));
            SqlParameter outputPar = new SqlParameter("@Out", SqlDbType.Int, 50);
            outputPar.Direction = ParameterDirection.Output;
            LoginUser.Parameters.Add(outputPar);
            objDB.GetDataSetUsingCmdObj(LoginUser);
            //check token to see if user is logged in with same Email and pass
            string a = LoginUser.Parameters["@Out"].Value.ToString();
            if (a.Equals("1"))
            {
                SqlCommand GetFriends = new SqlCommand();
                GetFriends.CommandType = CommandType.StoredProcedure;
                GetFriends.CommandText = "TP_GetFriends";
                GetFriends.Parameters.AddWithValue("@Email", requestedEmail);

                DataSet ds = objDB.GetDataSetUsingCmdObj(GetFriends);
                SqlCommand FindUsers = new SqlCommand();
                FindUsers.CommandType = CommandType.StoredProcedure;
                FindUsers.CommandText = "TP_FindUserByEmail";


                foreach (DataRow record in ds.Tables[0].Rows)
                {
                    FindUsers.Parameters.Clear();
                    FindUsers.Parameters.AddWithValue("@Email", record["Person2"].ToString());
                    DataSet Friend = objDB.GetDataSetUsingCmdObj(FindUsers);
                    foreach (DataRow UserInfo in Friend.Tables[0].Rows)
                    {
                        UserOBJ thisUser = new UserOBJ();
                        thisUser.First = UserInfo["First_Name"].ToString();
                        thisUser.Last = UserInfo["Last_Name"].ToString();
                        thisUser.Email = UserInfo["Email"].ToString();
                        thisUser.City = UserInfo["City"].ToString();
                        thisUser.State = UserInfo["State"].ToString();
                        thisUser.Organization = UserInfo["Organization"].ToString();
                        thisUser.ProfileURL = UserInfo["ProfilePhotoURL"].ToString();
                        Users.Add(thisUser);
                    }
                }


                return Users;
            }
            else
            {
                return Users;
            }
        }



        [HttpGet("GetProfile")]
        public UserOBJ GetProfile(String requestingEmail, String requestedEmail, String token)
        {
            DBConnect objDB = new DBConnect();
            UserOBJ thisUser = new UserOBJ();
            SqlCommand LoginUser = new SqlCommand();
            LoginUser.CommandType = CommandType.StoredProcedure;
            LoginUser.CommandText = "TP_FindUser";
            LoginUser.Parameters.AddWithValue("@Email", requestingEmail);
            LoginUser.Parameters.AddWithValue("@Password", decode(token));
            SqlParameter outputPar = new SqlParameter("@Out", SqlDbType.Int, 50);
            outputPar.Direction = ParameterDirection.Output;
            LoginUser.Parameters.Add(outputPar);
            objDB.GetDataSetUsingCmdObj(LoginUser);
            //check token to see if user is logged in with same Email and pass
            string a = LoginUser.Parameters["@Out"].Value.ToString();
            if (a.Equals("1"))
            {
                SqlCommand FindSettings = new SqlCommand();
                FindSettings.CommandType = CommandType.StoredProcedure;
                FindSettings.CommandText = "TP_FindUserSettings";
                FindSettings.Parameters.AddWithValue("@Email", requestedEmail);
                DataSet Settings = objDB.GetDataSetUsingCmdObj(FindSettings);
                SettingsOBJ mySettings = new SettingsOBJ();
                foreach (DataRow UserSetting in Settings.Tables[0].Rows)
                {
                    mySettings.Login = UserSetting["LoginSetting"].ToString();
                    mySettings.Personal = UserSetting["PersonalContactSetting"].ToString();
                    mySettings.Photos = UserSetting["PhotoSetting"].ToString();
                    mySettings.Profile = UserSetting["ProfileInfoSetting"].ToString();

                }

                SqlCommand FindUsers = new SqlCommand();
                FindUsers.CommandType = CommandType.StoredProcedure;
                FindUsers.CommandText = "TP_FindUserByEmail";


                FindUsers.Parameters.Clear();
                FindUsers.Parameters.AddWithValue("@Email", requestedEmail);
                DataSet Friend = objDB.GetDataSetUsingCmdObj(FindUsers);
                if (requestingEmail != requestedEmail)
                {


                    SqlCommand CheckFriends = new SqlCommand();
                    CheckFriends.CommandType = CommandType.StoredProcedure;
                    CheckFriends.CommandText = "TP_ChecksFriendship";
                    CheckFriends.Parameters.AddWithValue("@Email1", requestingEmail);
                    CheckFriends.Parameters.AddWithValue("@Email2", requestedEmail);
                    SqlParameter outputPar1 = new SqlParameter("@found", SqlDbType.Int, 50);
                    outputPar1.Direction = ParameterDirection.Output;
                    CheckFriends.Parameters.Add(outputPar1);
                    objDB.GetDataSetUsingCmdObj(CheckFriends).ToString();
                    string Friends = CheckFriends.Parameters["@found"].Value.ToString();
                    //SqlCommand CheckFriendsofFriends = new SqlCommand();
                    //CheckFriendsofFriends.CommandType = CommandType.StoredProcedure;
                    //CheckFriendsofFriends.CommandText = "TP_ChecksFriendship";
                    //CheckFriendsofFriends.Parameters.AddWithValue("@Email1", requestingEmail);
                    //CheckFriendsofFriends.Parameters.AddWithValue("@Email2", requestedEmail);
                    //string FriendsofFriends = objDB.GetDataSetUsingCmdObj(CheckFriendsofFriends).ToString();
                    switch (mySettings.Profile)
                    {
                        case "Public":

                            foreach (DataRow UserInfo in Friend.Tables[0].Rows)
                            {
                                thisUser = new UserOBJ();
                                thisUser.First = UserInfo["First_Name"].ToString();
                                thisUser.Last = UserInfo["Last_Name"].ToString();
                                thisUser.Email = UserInfo["Email"].ToString();
                                if (mySettings.Personal == "Public")
                                {

                                    thisUser.Phone = UserInfo["Phone#"].ToString();
                                }
                                else if (mySettings.Personal == "Only Friends")
                                {
                                    if (Friends == "1")
                                    {

                                        thisUser.Phone = UserInfo["Phone#"].ToString();
                                    }
                                    else
                                    {

                                        thisUser.Phone = "Only Friends can view this info";
                                    }
                                }


                                thisUser.City = UserInfo["City"].ToString();

                                thisUser.State = UserInfo["State"].ToString();
                                thisUser.Organization = UserInfo["Organization"].ToString();
                                thisUser.ProfileURL = UserInfo["ProfilePhotoURL"].ToString();
                                return thisUser;
                            }
                            break;
                        case "Only Friends":
                            if (Friends == "1")
                            {
                                foreach (DataRow UserInfo in Friend.Tables[0].Rows)
                                {
                                    thisUser = new UserOBJ();
                                    thisUser.First = UserInfo["First_Name"].ToString();
                                    thisUser.Email = UserInfo["Email"].ToString();
                                    thisUser.Last = UserInfo["Last_Name"].ToString();
                                    if (mySettings.Personal == "Public")
                                    {

                                        thisUser.Phone = UserInfo["Phone#"].ToString();
                                    }
                                    else if (mySettings.Personal == "Only Friends")
                                    {
                                        if (Friends == "1")
                                        {

                                            thisUser.Phone = UserInfo["Phone#"].ToString();
                                        }
                                        else
                                        {

                                            thisUser.Phone = "Only Friends can view this info";
                                        }
                                    }

                                    thisUser.City = UserInfo["City"].ToString();
                                    thisUser.State = UserInfo["State"].ToString();
                                    thisUser.Organization = UserInfo["Organization"].ToString();
                                    thisUser.ProfileURL = UserInfo["ProfilePhotoURL"].ToString();
                                    return thisUser;
                                }
                            }
                            else
                            {
                                foreach (DataRow UserInfo in Friend.Tables[0].Rows)
                                {
                                    thisUser = new UserOBJ();
                                    thisUser.First = "";
                                    thisUser.Last = "";
                                    thisUser.Email = "";
                                    thisUser.City = "";
                                    thisUser.Phone = "";
                                    thisUser.State = "";
                                    thisUser.Organization = "";
                                    thisUser.ProfileURL = "";
                                    return thisUser;
                                }
                            }
                            break;
                        case "Friends and Friends of Friends":
                            if (Friends == "1")
                            {
                                foreach (DataRow UserInfo in Friend.Tables[0].Rows)
                                {
                                    thisUser = new UserOBJ();
                                    thisUser.First = UserInfo["First_Name"].ToString();
                                    thisUser.Last = UserInfo["Last_Name"].ToString();
                                    thisUser.Email = UserInfo["Email"].ToString();
                                    thisUser.City = UserInfo["City"].ToString();
                                    if (mySettings.Personal == "Public")
                                    {
                                        thisUser.Phone = UserInfo["Phone#"].ToString();
                                    }
                                    else if (mySettings.Personal == "Only Friends")
                                    {
                                        if (Friends == "1")
                                        {
                                            thisUser.Phone = UserInfo["Phone#"].ToString();
                                        }
                                        else
                                        {
                                            thisUser.Phone = "Only Friends can view this info";
                                        }
                                    }
                                    thisUser.State = UserInfo["State"].ToString();
                                    thisUser.Organization = UserInfo["Organization"].ToString();
                                    thisUser.ProfileURL = UserInfo["ProfilePhotoURL"].ToString();
                                    return thisUser;
                                }
                            }
                            else
                            {
                                foreach (DataRow UserInfo in Friend.Tables[0].Rows)
                                {
                                    thisUser = new UserOBJ();
                                    thisUser.First = "";
                                    thisUser.Last = "";
                                    thisUser.Email = "";
                                    thisUser.City = "";
                                    thisUser.Phone = "";
                                    thisUser.State = "";
                                    thisUser.Organization = "";
                                    thisUser.ProfileURL = "";
                                    return thisUser;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    foreach (DataRow UserInfo in Friend.Tables[0].Rows)
                    {
                        thisUser = new UserOBJ();
                        thisUser.First = UserInfo["First_Name"].ToString();
                        thisUser.Last = UserInfo["Last_Name"].ToString();
                        thisUser.Email = UserInfo["Email"].ToString();
                        thisUser.City = UserInfo["City"].ToString();
                        thisUser.Phone = UserInfo["Phone#"].ToString();
                        thisUser.State = UserInfo["State"].ToString();
                        thisUser.Organization = UserInfo["Organization"].ToString();
                        thisUser.ProfileURL = UserInfo["ProfilePhotoURL"].ToString();
                        return thisUser;
                    }
                }

            }
            return thisUser;

        }


        [HttpGet("GetNewsFeed")]
        public List<PostOBJ> GetNewsFeed(String requestingEmail, String requestedEmail, String token)
        {
            DBConnect objDB = new DBConnect();
            List<PostOBJ> myNewsFeed = new List<PostOBJ>();
            SqlCommand LoginUser = new SqlCommand();
            LoginUser.CommandType = CommandType.StoredProcedure;
            LoginUser.CommandText = "TP_FindUser";
            LoginUser.Parameters.AddWithValue("@Email", requestingEmail);
            LoginUser.Parameters.AddWithValue("@Password", decode(token));
            SqlParameter outputPar = new SqlParameter("@Out", SqlDbType.Int, 50);
            outputPar.Direction = ParameterDirection.Output;
            LoginUser.Parameters.Add(outputPar);
            objDB.GetDataSetUsingCmdObj(LoginUser);
            //check token to see if user is logged in with same Email and pass
            string a = LoginUser.Parameters["@Out"].Value.ToString();
            if (a.Equals("1"))
            {

                SqlCommand GetFriends = new SqlCommand();
                GetFriends.CommandType = CommandType.StoredProcedure;
                GetFriends.CommandText = "TP_GetFriends";
                GetFriends.Parameters.AddWithValue("@Email", requestedEmail);
                DataSet Friends = objDB.GetDataSetUsingCmdObj(GetFriends);

                SqlCommand GetPosts = new SqlCommand();
                GetPosts.CommandType = CommandType.StoredProcedure;
                GetPosts.CommandText = "TP_GetPosts";

                SqlCommand CheckFollow = new SqlCommand();
                CheckFollow.CommandType = CommandType.StoredProcedure;
                CheckFollow.CommandText = "TP_CheckFollow";
                //get my Posts and add them to news feed collection
                GetPosts.Parameters.AddWithValue("@Email", requestingEmail);
                DataSet myPosts = objDB.GetDataSetUsingCmdObj(GetPosts);
                foreach (DataRow thisUsersPosts in myPosts.Tables[0].Rows)
                {
                        PostOBJ thisPost = new PostOBJ();
                        thisPost.EmailOwner = thisUsersPosts["EmailOwner"].ToString();
                        thisPost.EmailWriter = thisUsersPosts["EmailWriter"].ToString();
                        thisPost.Content = thisUsersPosts["Content"].ToString();
                        thisPost.Date = DateTime.Parse(thisUsersPosts["Date"].ToString());
                        myNewsFeed.Add(thisPost);
                }
                foreach (DataRow record in Friends.Tables[0].Rows)
                {
                    GetPosts.Parameters.Clear();
                    GetPosts.Parameters.AddWithValue("@Email", record["Person2"].ToString());
                    DataSet Posts = objDB.GetDataSetUsingCmdObj(GetPosts);
                    foreach (DataRow thisUsersPosts in Posts.Tables[0].Rows)
                    {
                        CheckFollow.Parameters.Clear();
                        CheckFollow.Parameters.AddWithValue("@Email1", record["Person1"].ToString());
                        CheckFollow.Parameters.AddWithValue("@Email2", record["Person2"].ToString());
                        SqlParameter outputPar1 = new SqlParameter("@found", SqlDbType.Int, 50);
                        outputPar1.Direction = ParameterDirection.Output;
                        CheckFollow.Parameters.Add(outputPar1);
                        objDB.GetDataSetUsingCmdObj(CheckFollow);
                        if (CheckFollow.Parameters["@found"].Value.ToString() == "1")
                        {
                            PostOBJ thisPost = new PostOBJ();
                            thisPost.EmailOwner = thisUsersPosts["EmailOwner"].ToString();
                            thisPost.EmailWriter = thisUsersPosts["EmailWriter"].ToString();
                            thisPost.Content = thisUsersPosts["Content"].ToString();
                            thisPost.Date = DateTime.Parse(thisUsersPosts["Date"].ToString());
                            myNewsFeed.Add(thisPost);
                        }

                    }
                }
                List<PostOBJ> SortedList = myNewsFeed.OrderByDescending(o => o.Date).ToList();

                return SortedList;
            }
            else
            {
                return myNewsFeed;
            }
        }
        [HttpGet("GetWall")]
        public List<PostOBJ> GetWall(String requestingEmail, String requestedEmail, String token)
        {
            DBConnect objDB = new DBConnect();
            List<PostOBJ> myNewsFeed = new List<PostOBJ>();
            SqlCommand LoginUser = new SqlCommand();
            LoginUser.CommandType = CommandType.StoredProcedure;
            LoginUser.CommandText = "TP_FindUser";
            LoginUser.Parameters.AddWithValue("@Email", requestingEmail);
            LoginUser.Parameters.AddWithValue("@Password", decode(token));
            SqlParameter outputPar = new SqlParameter("@Out", SqlDbType.Int, 50);
            outputPar.Direction = ParameterDirection.Output;
            LoginUser.Parameters.Add(outputPar);
            objDB.GetDataSetUsingCmdObj(LoginUser);
            //check token to see if user is logged in with same Email and pass
            string a = LoginUser.Parameters["@Out"].Value.ToString();
            if (a.Equals("1"))
            {
                SqlCommand GetWall = new SqlCommand();
                GetWall.CommandType = CommandType.StoredProcedure;
                GetWall.CommandText = "TP_GetWall";
                GetWall.Parameters.AddWithValue("@Email", requestedEmail);
                DataSet myWall = objDB.GetDataSetUsingCmdObj(GetWall);
                foreach (DataRow thisUsersPosts in myWall.Tables[0].Rows)
                {
                    PostOBJ thisPost = new PostOBJ();
                    thisPost.EmailOwner = thisUsersPosts["EmailOwner"].ToString();
                    thisPost.EmailWriter = thisUsersPosts["EmailWriter"].ToString();
                    thisPost.Content = thisUsersPosts["Content"].ToString();
                    thisPost.Date = DateTime.Parse(thisUsersPosts["Date"].ToString());
                    myNewsFeed.Add(thisPost);
                }
                List<PostOBJ> SortedList = myNewsFeed.OrderByDescending(o => o.Date).ToList();

                return SortedList;
            }
            else
            {
                return myNewsFeed;
            }
        }


        public string decode(String pass)
            {
            String encryptedPassword = pass;
            string converted = encryptedPassword.Replace(' ', '+');
            
            Byte[] encryptedPasswordBytes = Convert.FromBase64String(converted);
            Byte[] textBytes;
            String plainTextPassword;
            UTF8Encoding encoder = new UTF8Encoding();
            // Perform Decryption
            //-------------------
            // Create an instances of the decryption algorithm (Rinjdael AES) for the encryption to perform,
            // a memory stream used to store the decrypted data temporarily, and
            // a crypto stream that performs the decryption algorithm.
            RijndaelManaged rmEncryption = new RijndaelManaged();
            MemoryStream myMemoryStream = new MemoryStream();
            CryptoStream myDecryptionStream = new CryptoStream(myMemoryStream, rmEncryption.CreateDecryptor(key, vector), CryptoStreamMode.Write);
            // Use the crypto stream to perform the decryption on the encrypted data in the byte array.
            myDecryptionStream.Write(encryptedPasswordBytes, 0, encryptedPasswordBytes.Length);
            myDecryptionStream.FlushFinalBlock();
            // Retrieve the decrypted data from the memory stream, and write it to a separate byte array.
            myMemoryStream.Position = 0;
            textBytes = new Byte[myMemoryStream.Length];
            myMemoryStream.Read(textBytes, 0, textBytes.Length);
            // Close all the streams.
            myDecryptionStream.Close();
            myMemoryStream.Close();
            // Convert the bytes to a string and display it.
            plainTextPassword = encoder.GetString(textBytes);
            return plainTextPassword;


        }


    }
}
