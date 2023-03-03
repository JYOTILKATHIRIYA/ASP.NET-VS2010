using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class appointment_booking : System.Web.UI.Page
{
    static string constr = WebConfigurationManager.ConnectionStrings["AS4_constr"].ConnectionString;
    SqlConnection con = new SqlConnection(constr);
    SqlCommand cmd;
    SqlDataAdapter adp;
    DataTable dt;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (con.State == ConnectionState.Broken || con.State == ConnectionState.Closed)
        {
            con.Open();
        }
    }
    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
       
        
    }
    public Boolean validate_date(DateTime date_time)
    {
        string dayname = Calendar1.SelectedDate.DayOfWeek.ToString();

        if (dayname == "Saturday" || dayname == "Sunday")
        {
            Response.Write("It is a Holiday... Please select another day");
            return false;
        }
        DateTime today = DateTime.Today;

        int date_validator = date_time.CompareTo(today);

        if (date_validator < 0)
        {
            Response.Write("Please Select a valid Date");
            return false;
        }
        
        return true;
    }

    public string format_time(string time)
    {
        if (time.Contains("am"))
        {
            time = time.Replace("am", "");

        }
        if (time.Contains("pm"))
        {
            time = time.Replace("pm", "");


            double time_converter = Convert.ToDouble(time);
            time_converter += 12;
            if (time_converter == 24) time_converter -= 0.1;

            time = time_converter.ToString("0.00");

        }

        time = time.Replace(".", ":");
        return time;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DateTime date_time = new DateTime();
        date_time = Calendar1.SelectedDate;
        string time = DropDownList1.SelectedItem.Text;
        
        Boolean result=validate_date(date_time);

        if (!result)
        {
            return;
        }

        
        time = format_time(time);

        string date = date_time.ToLongDateString();
        Boolean flag = false;
        try
        {
            string userid = Session["userid"].ToString();
            string docid = Session["docid"].ToString();
           
            cmd = new SqlCommand(string.Format("INSERT INTO tblappointment(appointment_date,appointment_time,doctorid,patientid) VALUES('{0}','{1}',{2},{3})", date, time, docid, userid), con);
            cmd.ExecuteNonQuery();
            flag = true;
        }
        catch (Exception exp)
        {
            Response.Write("<br>Not Booked :"+exp.Message);

        }
        if (flag)
        {
            Server.Transfer("appointment_booked.aspx");

        }

    }

   
    
}