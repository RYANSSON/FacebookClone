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
using System.Runtime.Serialization.Formatters.Binary;       //needed for BinaryFormatter
using System.IO;                                            //needed for the MemoryStream

namespace FakeFacebook
{
    public partial class Settings : System.Web.UI.Page
    {
        DBConnect objDB = new DBConnect();
        SqlCommand SaveSettings = new SqlCommand();
        SqlCommand GetAllSettings = new SqlCommand();
        SqlCommand FindSettings = new SqlCommand();
        SqlCommand UpdateSettings = new SqlCommand();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["LoginCookie"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {

            }

            if (!IsPostBack && Session["SettingsObject"] != null)
            {
                Byte[] byteArray = (Byte[])Session["SettingsObject"];

                BinaryFormatter deSerializer = new BinaryFormatter();

                MemoryStream memStream = new MemoryStream(byteArray);



                SettingsOBJ settingsOBJ = (SettingsOBJ)deSerializer.Deserialize(memStream);

                ddLoginSettings.SelectedValue = settingsOBJ.Login;
                ddPhotosSettings.SelectedValue = settingsOBJ.Photos;
                ddPProfileSettings0.SelectedValue = settingsOBJ.Profile;
                ddContactSettings.SelectedValue = settingsOBJ.Personal;
            }
            else if (!IsPostBack && Session["SettingsObject"] == null)
            {
                HttpCookie myCookie = Request.Cookies["LoginCookie"];
                GetAllSettings.CommandType = CommandType.StoredProcedure;
                GetAllSettings.CommandText = "TP_GetAllSettings";
                string a = myCookie.Values["Email"].ToString();
                GetAllSettings.Parameters.AddWithValue("@Email", myCookie.Values["Email"].ToString());
                SqlParameter outputPar1 = new SqlParameter("@LoginSetting", SqlDbType.VarChar, 100);
                outputPar1.Direction = ParameterDirection.Output;
                GetAllSettings.Parameters.Add(outputPar1);
                SqlParameter outputPar2 = new SqlParameter("@ProfileSetting", SqlDbType.VarChar, 100);
                outputPar2.Direction = ParameterDirection.Output;
                GetAllSettings.Parameters.Add(outputPar2);
                SqlParameter outputPar3 = new SqlParameter("@PhotoSetting", SqlDbType.VarChar, 100);
                outputPar3.Direction = ParameterDirection.Output;
                GetAllSettings.Parameters.Add(outputPar3);
                SqlParameter outputPar4 = new SqlParameter("@PersonalSetting", SqlDbType.VarChar, 100);
                outputPar4.Direction = ParameterDirection.Output;
                GetAllSettings.Parameters.Add(outputPar4);
                objDB.GetDataSetUsingCmdObj(GetAllSettings);

                // Serialize the CreditCard object
                SettingsOBJ settings = new SettingsOBJ();
                settings.Login = GetAllSettings.Parameters["@LoginSetting"].Value.ToString();
                settings.Photos = GetAllSettings.Parameters["@PhotoSetting"].Value.ToString();
                settings.Profile = GetAllSettings.Parameters["@ProfileSetting"].Value.ToString();
                settings.Personal = GetAllSettings.Parameters["@PersonalSetting"].Value.ToString();
                ddLoginSettings.SelectedValue = GetAllSettings.Parameters["@LoginSetting"].Value.ToString();
                ddPhotosSettings.SelectedValue = GetAllSettings.Parameters["@PhotoSetting"].Value.ToString();
                ddPProfileSettings0.SelectedValue = GetAllSettings.Parameters["@ProfileSetting"].Value.ToString();
                ddContactSettings.SelectedValue = GetAllSettings.Parameters["@PersonalSetting"].Value.ToString();
                BinaryFormatter serializer = new BinaryFormatter();
                MemoryStream memStream = new MemoryStream();
                Byte[] byteArray;
                serializer.Serialize(memStream, settings);
                byteArray = memStream.ToArray();
                Session["SettingsObject"] = byteArray;
            }
        }

        protected void btnSaveSettings_Click(object sender, EventArgs e)
        {
            HttpCookie myCookie = Request.Cookies["LoginCookie"];
            FindSettings.CommandType = CommandType.StoredProcedure;
            FindSettings.CommandText = "TP_IfEmailExists";
            FindSettings.Parameters.AddWithValue("@Email", myCookie.Values["Email"].ToString());
            SqlParameter outputPar = new SqlParameter("@Exists", SqlDbType.VarChar, 100);
            outputPar.Direction = ParameterDirection.Output;
            FindSettings.Parameters.Add(outputPar);
            objDB.GetDataSetUsingCmdObj(FindSettings);
            if (FindSettings.Parameters["@Exists"].Value.ToString() == "0")
            { //add new settings for a user
                SaveSettings.CommandType = CommandType.StoredProcedure;
                SaveSettings.CommandText = "TP_SaveSettings";
                SaveSettings.Parameters.AddWithValue("@Email", myCookie.Values["Email"].ToString());
                SaveSettings.Parameters.AddWithValue("@login", ddLoginSettings.SelectedValue);
                SaveSettings.Parameters.AddWithValue("@Photo", ddPhotosSettings.SelectedValue);
                SaveSettings.Parameters.AddWithValue("@ProfileInfo", ddPProfileSettings0.SelectedValue);
                SaveSettings.Parameters.AddWithValue("@PersonalContact", ddContactSettings.SelectedValue);

                objDB.DoUpdateUsingCmdObj(SaveSettings);
            }
            else
            { // update the settings for this user
                UpdateSettings.CommandType = CommandType.StoredProcedure;
                UpdateSettings.CommandText = "TP_UpdateSettings";
                UpdateSettings.Parameters.AddWithValue("@Email", myCookie.Values["Email"].ToString());
                UpdateSettings.Parameters.AddWithValue("@login", ddLoginSettings.SelectedValue);
                UpdateSettings.Parameters.AddWithValue("@Photo", ddPhotosSettings.SelectedValue);
                UpdateSettings.Parameters.AddWithValue("@ProfileInfo", ddPProfileSettings0.SelectedValue);
                UpdateSettings.Parameters.AddWithValue("@PersonalContact", ddContactSettings.SelectedValue);

                objDB.DoUpdateUsingCmdObj(UpdateSettings);
            }

            SettingsOBJ setting = new SettingsOBJ();
            setting.Login = ddLoginSettings.SelectedValue;
            setting.Photos = ddPhotosSettings.SelectedValue;
            setting.Profile = ddPProfileSettings0.SelectedValue;
            setting.Personal = ddContactSettings.SelectedValue;
            // Serialize the CreditCard object

            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            Byte[] byteArray;
            serializer.Serialize(memStream, setting);
            byteArray = memStream.ToArray();
            Session["SettingsObject"] = byteArray;

            Response.Redirect("HomePage.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //log out delete pass from cookie
            HttpCookie myCookie = Request.Cookies["LoginCookie"];
            myCookie.Values["Email"] = myCookie.Values["Email"];
            myCookie.Values["Password"] = "";
            myCookie.Expires = new DateTime(2020, 2, 1);
            Response.Cookies.Add(myCookie);
            Response.Redirect("Login.aspx");
        }
    }
}