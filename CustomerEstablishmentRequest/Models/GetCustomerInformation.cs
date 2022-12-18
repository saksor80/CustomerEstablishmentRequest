namespace CustomerEstablishmentRequest.Models
{
    public class GetCustomerInformation //Sets model for GetCustomerInformation
    {
        public int RequestID { get; set; }
        public string RequestDate { get; set; }
        public string RequestStatus { get; set; }
        public string Vendor { get; set; }
        public string Name { get; set; }
    }
}
