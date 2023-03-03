using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class PatientRegistration : System.Web.UI.Page
{
    static string constr=WebConfigurationManager.ConnectionStrings["AS4_constr"].ConnectionString;
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

    protected void age_box_TextChanged(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        String name = name_box.Text;
        int age = Convert.ToInt32(age_box.Text);
        string gender = gender_rlist.SelectedItem.Text;
        string address=address_box.Text;
        string mo=contact_box.Text;
        string password=pass_box.Text;
        bool flag = false;
        try
        {
            cmd = new SqlCommand(
                string.Format(
                "INSERT INTO tblpatient VALUES('{0}',{1},'{2}','{3}','{4}',1,'{5}')", name, age, gender, address, mo, password)
                , con);

            cmd.ExecuteNonQuery();
            Response.Write("<h1 style='color:green'>Registered Successfully...</h1>");
            flag = true;
            
        }
        catch (Exception exp)
        {
            Response.Write("Not Registered : " + exp.Message);
        }
        if (flag)
        {
            Server.Transfer("login.aspx");
        }
    }
    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string name = args.Value;
        //Response.Write(name);
        args.IsValid = false;

        if (name.Length >2)
        {
            //args.IsValid = false;
            return;

        }
        args.IsValid = true;
    }
}