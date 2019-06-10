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

namespace FakeFacebook
{
    public partial class Login : System.Web.UI.Page
    {
        private Byte[] key = { 250, 101, 18, 76, 45, 135, 207, 118, 4, 171, 3, 168, 202, 241, 37, 199 };

        private Byte[] vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 252, 112, 79, 32, 114, 156 };

        static string operand;
        static string LoginSetting;
        static bool flag = false;
        DBConnect objDB = new DBConnect();
        SqlCommand LoginUser = new SqlCommand();
        SqlCommand RegisterUSER = new SqlCommand();
        SqlCommand InsertQuestions = new SqlCommand();
        SqlCommand FindQuestions = new SqlCommand();
        SqlCommand GetPassword = new SqlCommand();
        SqlCommand GetLoginSetting = new SqlCommand();
        protected void Page_Load(object sender, EventArgs e)
        {
            invalidLogin.Visible = false;
            invalidRegister.Visible = false;
            duplicateRecord.Visible = false;
            if(flag == false)
            {
                SecurityQuestions.Visible = false;
            }
            // Read encrypted password from cookie
         
            if (!IsPostBack && Request.Cookies["LoginCookie"] != null)
            {
                HttpCookie myCookie = Request.Cookies["LoginCookie"];
                GetLoginSetting.CommandType = CommandType.StoredProcedure;
                GetLoginSetting.CommandText = "TP_GetLoginSetting";
                string a = myCookie.Values["Email"].ToString();
                GetLoginSetting.Parameters.AddWithValue("@Email", myCookie.Values["Email"].ToString());
                SqlParameter outputPar = new SqlParameter("@LoginSetting", SqlDbType.VarChar, 100);
                outputPar.Direction = ParameterDirection.Output;
                GetLoginSetting.Parameters.Add(outputPar);
                objDB.GetDataSetUsingCmdObj(GetLoginSetting);
                LoginSetting = GetLoginSetting.Parameters["@LoginSetting"].Value.ToString();
                if (LoginSetting == "Auto-Login")
                {
                    txtEmail.Text = myCookie.Values["Email"];
                    txtPassword.Text = Decrypt();
                }
                if (LoginSetting == "Fast-Login")
                {
                    txtEmail.Text = myCookie.Values["Email"];
                    
                }
                if (LoginSetting == "None")
                {
                    txtEmail.Text = "";
                    txtPassword.Text = "";
                }

            }
            else
            {
            }
        }
        protected string Decrypt()
        {
            if (Request.Cookies["LoginCookie"] != null)
            {
                HttpCookie myCookie = Request.Cookies["LoginCookie"];
                if (myCookie.Values["Password"] != "")
                {
                    String encryptedPassword = myCookie.Values["Password"];
                    Byte[] encryptedPasswordBytes = Convert.FromBase64String(encryptedPassword);
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
            return "";
        }
            protected void AddCookieEmailPassword(string NewEmail, string Newpass)
        {

            Response.Cookies.Remove("LoginCookie");
            String Email = NewEmail;
            String plainTextPassword = Newpass;
            String encryptedPassword;
            UTF8Encoding encoder = new UTF8Encoding();      // used to convert bytes to characters, and back
            Byte[] textBytes;                               // stores the plain text data as bytes
            textBytes = encoder.GetBytes(plainTextPassword);
            // Create an instances of the encryption algorithm (Rinjdael AES) for the encryption to perform,
            // a memory stream used to store the encrypted data temporarily, and
            // a crypto stream that performs the encryption algorithm.
            RijndaelManaged rmEncryption = new RijndaelManaged();
            MemoryStream myMemoryStream = new MemoryStream();
            CryptoStream myEncryptionStream = new CryptoStream(myMemoryStream, rmEncryption.CreateEncryptor(key, vector), CryptoStreamMode.Write);
            // Use the crypto stream to perform the encryption on the plain text byte array.
            myEncryptionStream.Write(textBytes, 0, textBytes.Length);
            myEncryptionStream.FlushFinalBlock();
            // Retrieve the encrypted data from the memory stream, and write it to a separate byte array.
            myMemoryStream.Position = 0;
            Byte[] encryptedBytes = new Byte[myMemoryStream.Length];
            myMemoryStream.Read(encryptedBytes, 0, encryptedBytes.Length);
            // Close all the streams.
            myEncryptionStream.Close();
            myMemoryStream.Close();
            // Convert the bytes to a string and display it.
            encryptedPassword = Convert.ToBase64String(encryptedBytes);
            // Write encrypted password to a cookie
            HttpCookie myCookie = new HttpCookie("LoginCookie");
            myCookie.Values["Email"] = Email;
            myCookie.Values["Password"] = encryptedPassword;
            myCookie.Expires = new DateTime(2020, 2, 1);
            Response.Cookies.Add(myCookie);
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            clearparameters();
            LoginUser.CommandType = CommandType.StoredProcedure;
            LoginUser.CommandText = "TP_FindUser";
            LoginUser.Parameters.AddWithValue("@Email", txtEmail.Text );
            LoginUser.Parameters.AddWithValue("@Password", txtPassword.Text);
            SqlParameter outputPar = new SqlParameter("@Out", SqlDbType.Int, 50);
            outputPar.Direction = ParameterDirection.Output;
            LoginUser.Parameters.Add(outputPar);
            objDB.GetDataSetUsingCmdObj(LoginUser);
            string a = LoginUser.Parameters["@Out"].Value.ToString();
            if (a.Equals("1"))
            {
                AddCookieEmailPassword(txtEmail.Text, txtPassword.Text);
                Response.Redirect("HomePage.aspx");
            }
            else
            {
                invalidLogin.Visible = true;
            }

            
        }
       
        protected void clearparameters()
        {
            RegisterUSER.Parameters.Clear();
            LoginUser.Parameters.Clear();
        }
        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            clearparameters();
            RegisterUSER.CommandType = CommandType.StoredProcedure;
            RegisterUSER.CommandText = "TP_CreateUser";
            RegisterUSER.Parameters.AddWithValue("@Email", txtRegEmail.Text);
            RegisterUSER.Parameters.AddWithValue("@FIRST", txtFirstName.Text);
            RegisterUSER.Parameters.AddWithValue("@Last", txtLastName.Text);
            RegisterUSER.Parameters.AddWithValue("@PHONE", txtPhone.Text);
            RegisterUSER.Parameters.AddWithValue("@Password", txtRegPassword.Text);
            RegisterUSER.Parameters.AddWithValue("@City", txtCity.Text);
            RegisterUSER.Parameters.AddWithValue("@State", txtState.Text);
            RegisterUSER.Parameters.AddWithValue("@Organization", txtOrganization.Text);
            LoginUser.CommandType = CommandType.StoredProcedure;
            LoginUser.CommandText = "TP_FindUser";
            LoginUser.Parameters.AddWithValue("@Email", txtRegEmail.Text);
            LoginUser.Parameters.AddWithValue("@Password", txtRegPassword.Text);
            SqlParameter outputPar = new SqlParameter("@Out", SqlDbType.Int, 50);
            outputPar.Direction = ParameterDirection.Output;
            LoginUser.Parameters.Add(outputPar);
            objDB.GetDataSetUsingCmdObj(LoginUser);
            string a = LoginUser.Parameters["@Out"].Value.ToString();
            if (txtFirstName.Text == "" || txtLastName.Text == "" || txtRegEmail.Text == "" || txtRegPassword.Text == "" || txtPhone.Text == "")
            {
                invalidRegister.Visible = true;
                SecurityQuestions.Visible = false;
                return;
            }
            if (a.Equals("0"))
            {
                AddCookieEmailPassword(txtRegEmail.Text, txtRegPassword.Text);
                if (objDB.DoUpdateUsingCmdObj(RegisterUSER) == 1)
                {
                    //lblReport.Text = "You Registered Succesfully";
                }
                SecurityQuestions.Visible = true;

                operand = "signup";
            }
            else
            {
                duplicateRecord.Visible = true;
            }

        }
        protected void lblForgot_Click(object sender, EventArgs e)
        {
            SecurityQuestions.Visible = true;
            operand = "forgot";

        }

        protected void btnAnswerQ_Click(object sender, EventArgs e)
        {
            string Q1 = txtQ1.Text;
            string Q2 = txtQ2.Text;
            string Q3 = txtQ3.Text;

            if (operand == "forgot")
            {
                FindQuestions.Parameters.Clear();
                FindQuestions.CommandType = CommandType.StoredProcedure;
                FindQuestions.CommandText = "TP_GetAnswers";
                FindQuestions.Parameters.AddWithValue("@Email", txtEmail.Text);
                SqlParameter outputPar = new SqlParameter("@Q1", SqlDbType.VarChar, 100);
                outputPar.Direction = ParameterDirection.Output;
                FindQuestions.Parameters.Add(outputPar);
                SqlParameter outputPar2 = new SqlParameter("@Q2", SqlDbType.VarChar, 100);
                outputPar2.Direction = ParameterDirection.Output;
                FindQuestions.Parameters.Add(outputPar2);
                SqlParameter outputPar3 = new SqlParameter("@Q3", SqlDbType.VarChar, 100);
                outputPar3.Direction = ParameterDirection.Output;
                FindQuestions.Parameters.Add(outputPar3);
                objDB.GetDataSetUsingCmdObj(FindQuestions);

                if (Q1 != FindQuestions.Parameters["@Q1"].Value.ToString())
                {
                    return;
                }
                if (Q2 != FindQuestions.Parameters["@Q2"].Value.ToString())
                {
                    return;
                }
                if (Q3 != FindQuestions.Parameters["@Q3"].Value.ToString())
                {
                    return;
                }
                //GetPassword.Parameters.Clear();
                GetPassword.CommandType = CommandType.StoredProcedure;
                GetPassword.CommandText = "TP_GetPassword";
                GetPassword.Parameters.AddWithValue("@Email", txtEmail.Text);
                SqlParameter outputPassword = new SqlParameter("@Password", SqlDbType.VarChar, 100);
                outputPassword.Direction = ParameterDirection.Output;
                GetPassword.Parameters.Add(outputPassword);
                objDB.GetDataSetUsingCmdObj(GetPassword);
                txtPassword.Text = GetPassword.Parameters["@Password"].Value.ToString();

            }
            if (operand == "signup")
            {
                if(Q1 == "" || Q2 == "" || Q3 == "")
                {
                    invalidRegister.Visible = true;
                    SecurityQuestions.Visible = true;
                    flag = true;
                    return;
                }

                InsertQuestions.CommandType = CommandType.StoredProcedure;
                InsertQuestions.CommandText = "TP_InsertQuestions";
                InsertQuestions.Parameters.AddWithValue("@Email", txtRegEmail.Text);
                InsertQuestions.Parameters.AddWithValue("@Q1", txtQ1.Text);
                InsertQuestions.Parameters.AddWithValue("@Q2", txtQ2.Text);
                InsertQuestions.Parameters.AddWithValue("@Q3", txtQ3.Text);
                objDB.DoUpdateUsingCmdObj(InsertQuestions);


                Response.Redirect("Settings.aspx");
            }
        }
    }
}