namespace CustomerEstablishmentRequest.Models
{
    public class Input
    {
        public int Id { get; internal set; }
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
    }
}
