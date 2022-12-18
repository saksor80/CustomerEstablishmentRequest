namespace CustomerEstablishmentRequest.Models
{
    public class CustomerEstablishmentRequestForm   //Sets model for CustomerEstablishmentRequestForm
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
    }
}
