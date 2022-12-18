using System.Data.SqlClient;

namespace CustomerEstablishmentRequest.Models
{
    public class CustomerEstablishmentRequestResultHandled  //Sets model for CustomerEstablishmentResultHandled
    {
        public string RequestID { get; set; }
        public string HandledDate { get; set; }
        public string RequestStatus { get; set; }

        public int UpdateRecord()   //Updates record in CustomerInformation-table
        {
            using(SqlConnection con = new SqlConnection(GetConString.ConString()))    //Sets new SqlConnection using GetConString class
            {
                string query = "UPDATE CustomerInformation SET HandledDate = @HandledDate, RequestStatus = @RequestStatus WHERE RequestID = @RequestID";   //Sets sql command string

                using (SqlCommand cmd = new SqlCommand(query, con))    //Sets new SqlCommand cmd
                {
                    cmd.Parameters.AddWithValue("@RequestID", RequestID);
                    cmd.Parameters.AddWithValue("@HandledDate", HandledDate);
                    cmd.Parameters.AddWithValue("@RequestStatus", RequestStatus);

                    con.Open(); //Opens SqlConnection
                    int i = cmd.ExecuteNonQuery();  //Executes SqlCommand cmd
                    con.Close();    //Closes SqlConnection
                    return i;   //Returns records updated
                }
            }
        }
    }
}