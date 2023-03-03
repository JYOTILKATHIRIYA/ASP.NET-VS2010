using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class Dashboard : System.Web.UI.Page
{
    static string constr = WebConfigurationManager.ConnectionStrings["AS4_constr"].ConnectionString;
    SqlConnection con = new SqlConnection(constr);
    SqlCommand cmd;
    SqlDataAdapter adp;
    DataTable dt;
    int userid;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Broken || con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        display_userinfo();
    }

    public void displayDoctors()
    {
        dt = new DataTable();

        adp = new SqlDataAdapter("select * from tbldoctor", con);
        adp.Fill(dt);

        Doctors_gview.DataSource = dt;
        Doctors_gview.DataBind();
        
    }

    public void display_userinfo()
    {
        try
        {
            string user_no = Request.Cookies["user"].Value;
            cmd = new SqlCommand("select * from tblpatient where contactno='" + user_no + "';", con);
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Read();
            
            userid = Convert.ToInt32(reader["patientid"].ToString());
            Response.Write(reader["patientname"].ToString() + "<br>");
            Response.Write("Age : " + reader["age"].ToString() + "<br>");
            Response.Write("Gender : " + reader["gender"].ToString() + "<br>");
            Response.Write("Address : " + reader["address"].ToString() + "<br>");

            reader.Close();
            try
            {
                displayDoctors();
            }
            catch (Exception exp)
            {
                Response.Write("\nNo Doctors Found : " + exp.Message);
            }
        }
        catch (Exception exp)
        {
            Response.Write("Not Logged In ");
        }

    }

    protected void Doctors_gview_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ind=Doctors_gview.SelectedIndex;
        int docid = Convert.ToInt32(Doctors_gview.Rows[ind].Cells[1].Text);
        
        Session["userid"]=userid;
        Session["docid"] = docid;
        Response.Redirect("appointment_booking.aspx");
        
    }

    protected void search_doctor_btn_Click(object sender, EventArgs e)
    {
        string search = search_doctor_box.Text;
        if (search == "")
        {
            search_result_lbl.Text = "";
            return;
        }
        adp = new SqlDataAdapter(string.Format("select * from tbldoctor where specialization='{0}'", search), con);
            dt=new DataTable();
            
            
            adp.Fill(dt);

        if(dt.Rows.Count>0)
        {
            Doctors_gview.DataSource=dt;
            Doctors_gview.DataBind();
        search_result_lbl.Text="";

        }else{
        search_result_lbl.Text="No Doctors found";
            displayDoctors();
        }
        

    }
   
}