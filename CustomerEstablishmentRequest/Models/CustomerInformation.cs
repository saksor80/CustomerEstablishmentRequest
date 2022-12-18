using System.Data.SqlClient;

namespace CustomerEstablishmentRequest.Models
{
    public class CustomerInformation    //Sets model for CustomerInformation
    {
        public int RequestID { get; set; }
        public string RequestDate { get; set; }
        public string RequestStatus { get; set; }
        public string Name { get; set; }
        public string BusinessID { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string Phone { get; set; }
        public string CustomerEmail { get; set; }

        public string Vendor { get; set; }
        public string VendorEmail { get; set; }
        public string PriceList { get; set; }
        public string CustomerType { get; set; }

        public string BillingMethod { get; set; }
        public string BillingCycle { get; set; }
        public string PaymentCondition { get; set; }
        public string OvtID { get; set; }
        public string EinvoiceAddress { get; set; }
        public string EinvoiceOperator { get; set; }
        public string EinvoiceOperatorID { get; set; }

        public int AddRecord()  //Adds new record into CustomerInformation-table
        {
            using (SqlConnection con = new SqlConnection(GetConString.ConString()))  //Sets new SqlConnection using GetConString class
            {
                string query = "INSERT INTO CustomerInformation (RequestDate, RequestStatus, Name, BusinessID, Address, ZipCode, City, CountryCode, Phone, CustomerEmail, Vendor, VendorEmail, " +
                "PriceList, CustomerType, BillingMethod, BillingCycle, PaymentCondition, OvtID, EinvoiceAddress, EinvoiceOperator, EinvoiceOperatorID) " +
                           "VALUES (@RequestDate, @RequestStatus, @Name, @BusinessID, @Address, @ZipCode, @City, @CountryCode, @Phone, @CustomerEmail, @Vendor, @VendorEmail, @PriceList, @CustomerType, " +
                                   "@BillingMethod, @BillingCycle, @PaymentCondition, @OvtID, @EinvoiceAddress, @EinvoiceOperator, @EinvoiceOperatorID)";    //Sets sql command string

                using (SqlCommand cmd = new SqlCommand(query, con)) //Sets new SqlCommand cmd
                {
                    cmd.Parameters.AddWithValue("@RequestDate", RequestDate);
                    cmd.Parameters.AddWithValue("@RequestStatus", RequestStatus);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@BusinessID", BusinessID);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@ZipCode", ZipCode);
                    cmd.Parameters.AddWithValue("@City", City.ToUpper());
                    cmd.Parameters.AddWithValue("@CountryCode", CountryCode.ToUpper());
                    cmd.Parameters.AddWithValue("@Phone", Phone);
                    cmd.Parameters.AddWithValue("@CustomerEmail", CustomerEmail);
                    cmd.Parameters.AddWithValue("@Vendor", Vendor);
                    cmd.Parameters.AddWithValue("@VendorEmail", VendorEmail);
                    cmd.Parameters.AddWithValue("@PriceList", PriceList);
                    cmd.Parameters.AddWithValue("@CustomerType", CustomerType);
                    cmd.Parameters.AddWithValue("@BillingMethod", BillingMethod);
                    cmd.Parameters.AddWithValue("@BillingCycle", BillingCycle);
                    cmd.Parameters.AddWithValue("@PaymentCondition", PaymentCondition);
                    cmd.Parameters.AddWithValue("@OvtID", OvtID);
                    cmd.Parameters.AddWithValue("@EinvoiceAddress", EinvoiceAddress);
                    cmd.Parameters.AddWithValue("@EinvoiceOperator", EinvoiceOperator);
                    cmd.Parameters.AddWithValue("@EinvoiceOperatorID", EinvoiceOperatorID);

                    con.Open(); //Opens SqlConnection
                    int i = cmd.ExecuteNonQuery();  //Executes SqlCommand cmd
                    con.Close();    //Closes SqlConnection
                    return i;   //Returns records added
                }
            }
        }
    }
}