using System.Data.SqlClient;

namespace CustomerEstablishmentRequest.Models
{
    public class CustomerEstablishmentRequestResultUnhandled    //Sets model for CustomerEstablishmentResultUnhandled
    {
        public string RequestID { get; set; }
        public string RequestStatus { get; set; }
        public string Comment { get; set; }

        public int UpdateRecord()   //Updates record in CustomerInformation-table
        {
            using (SqlConnection con = new SqlConnection(GetConString.ConString()))    //Sets new SqlConnection using GetConString class
            {
                string query = "UPDATE CustomerInformation SET RequestStatus = @RequestStatus, Comment = @Comment WHERE RequestID = @RequestID";   //Sets sql command string

                using (SqlCommand cmd = new SqlCommand(query, con))    //Sets new SqlCommand cmd
                {
                    cmd.Parameters.AddWithValue("@RequestID", RequestID);
                    cmd.Parameters.AddWithValue("@RequestStatus", RequestStatus);
                    cmd.Parameters.AddWithValue("@Comment", Comment);

                    con.Open(); //Opens SqlConnection
                    int i = cmd.ExecuteNonQuery();  //Executes SqlCommand cmd
                    con.Close();    //Closes SqlConnection
                    return i;   //Returns records updated
                }
            }
        }
    }
}