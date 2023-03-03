using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class login : System.Web.UI.Page
{
    static string constr = WebConfigurationManager.ConnectionStrings["AS4_constr"].ConnectionString;
    SqlConnection con = new SqlConnection(constr);
    SqlCommand cmd;
    SqlDataAdapter adp;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Broken || con.State == ConnectionState.Closed)
        {
            con.Open();
        }
    }
    protected void PatientLogin_Authenticate(object sender, AuthenticateEventArgs e)
    {
        string mo = PatientLogin.UserName;
        string pass=PatientLogin.Password;

        if (mo.Length < 10 || mo.Length>10)
        {
            PatientLogin.FailureText = "Please Enter Valid Mobile No";
            return;
        }


        cmd = new SqlCommand(string.Format("select contactno,password from tblpatient where contactno={0}", mo),con);
        SqlDataReader reader = cmd.ExecuteReader();

        if (!reader.HasRows)
        {
            PatientLogin.FailureText = "User Does not Exist";
            return;
        }

        reader.Read();

        if (reader.GetString(1) == pass)
        {
            HttpCookie user = new HttpCookie("user", reader.GetString(0));
            Response.SetCookie(user);

            Response.Redirect("Dashboard.aspx");
        }
        else
        {
            PatientLogin.FailureText = "Wrong Password Entered";
            return;
        }
    }
}