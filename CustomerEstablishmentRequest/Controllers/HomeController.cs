using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CustomerEstablishmentRequest.Models;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CustomerEstablishmentRequest.Controllers  //Sets space for Controllers
{
    public class HomeController : Controller    //Sets Homecontroller
    {
        public IActionResult Index()    //Sets IActionResult for Index
        {
            return View();  //Returns index view
        }

        string Baseurl = "http://localhost:53569/";

        public async Task<ActionResult> Input() //Gets Rest Api input data to a controller and passes it to a view
        {
            List<InputMVC> InputInfo = new List<InputMVC>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Input");
                if (Res.IsSuccessStatusCode)
                {
                    var InputResponse = Res.Content.ReadAsStringAsync().Result;
                    InputInfo = JsonConvert.DeserializeObject<List<InputMVC>>(InputResponse);
                }

                return View(InputInfo);
            }
        }

        public IActionResult SendCustomerInformation() //Sets IActionResult for SendCustomerInformation
        {
            return View(); //Returns SendCustomerInformation view
        }

        public IActionResult Message() //Sets IActionResult for Message
        {
            return View();  //Returns Message view
        }

        public IActionResult Error() //Sets IActionResult for Error
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });    //Returns ErrorViewModel into a view
        }

        DateTime date1 = DateTime.Now;  //Sets HandledDate

        [HttpPost]  //Posts CustomerEstablishmentRequest Form data into IActionResult CustomerEstablishmentRequestResult                                                                         
        [ValidateAntiForgeryToken]  //Validates AntiForgeryToken
        public IActionResult CustomerEstablishmentRequestResult(string ID, string Result)   //Sets IActionResult CustomerEstablishmentRequestResult, passes ID and Result
        {
            if (HttpContext.Request.Form["txtResult"].ToString() == "Käsitelty")    //Checks if request result is handled
            {
                CustomerEstablishmentRequestResultHandled Data = new CustomerEstablishmentRequestResultHandled  //Sets new CustomerEstablishmentRequestResultHandled model class and passes Form data to it
                {
                    RequestID = ID,
                    RequestStatus = HttpContext.Request.Form["txtResult"].ToString(),
                    HandledDate = date1.ToShortDateString(),
                };
                int result = Data.UpdateRecord();
                if (result > 0) //Checks if data was recorded
                {
                    ViewBag.Message = "Kiitos, vastaus lähetettiin vastuumyyjälle sähköpostilla!";  //Sets success message
                    SendMail(); //Calls for SendMail for sending handled result Email to vendor
                }
                else
                {
                    ViewBag.Message = "Tietojen lähettämisessä tapahtui virhe!";    //Sets error message
                }
            }
            if (HttpContext.Request.Form["txtResult"].ToString() == "Hylätty")  //Checks if request result is rejected
            {
                CustomerEstablishmentRequestResultRejected Data = new CustomerEstablishmentRequestResultRejected    //Sets new CustomerEstablishmentRequestResultRejected model class and passes Form data to it
                {
                    RequestID = ID,
                    RequestStatus = HttpContext.Request.Form["txtResult"].ToString(),
                    HandledDate = date1.ToShortDateString(),
                    Comment = HttpContext.Request.Form["txtComment"].ToString(),
                };
                int result = Data.UpdateRecord();
                if (result > 0) //Checks if data was recorded
                {
                    ViewBag.Message = "Kiitos, vastaus lähetettiin vastuumyyjälle sähköpostilla!";  //Sets success message
                    SendMail(); //Calls for SendMail for sending rejected result Email to vendor
                }
                else
                {
                    ViewBag.Message = "Tietojen lähettämisessä tapahtui virhe!";    //Sets error message
                }
            }
            if (HttpContext.Request.Form["txtResult"].ToString() == "Odottaa")  //Checks if request result is unhandled
            {
                CustomerEstablishmentRequestResultUnhandled Data = new CustomerEstablishmentRequestResultUnhandled  //Sets new CustomerEstablishmentRequestResultUnhandled model class and passes Form data to it
                {
                    RequestID = ID,
                    RequestStatus = HttpContext.Request.Form["txtResult"].ToString(),
                    Comment = HttpContext.Request.Form["txtComment"].ToString(),
                };
                int result = Data.UpdateRecord();
                if (result > 0) //Checks if data was recorded
                {
                    ViewBag.Message = "Kiitos, vastaus lähetettiin vastuumyyjälle sähköpostilla!";  //Sets success message
                    SendMail(); //Calls for SendMail for sending unhandled result Email to vendor
                }
                else
                {
                    ViewBag.Message = "Tietojen lähettämisessä tapahtui virhe!";    //Sets error message
                }
            }
            async void SendMail()   //Sends Email
            {
                var message = new MailMessage();    //Sets message variable for sending Email

                if (HttpContext.Request.Form["txtResult"].ToString() == "Käsitelty") {
                    Result = "Hyväksytty";  //Checks if request result is handled
                    var body = "Pyyntötunnus: {0}<br>Pyynnön Pvm: {1}<br>Asiakkaan nimi: {2}<br>Y-tunnus: {3}<br><hr><br>Vastaus: {4}<br>Asiakastunnus: {5}";   //Sets variable for message body
                    message.Body = string.Format(body, ID, HttpContext.Request.Form["txtRequestDate"].ToString(), HttpContext.Request.Form["txtName"].ToString(), HttpContext.Request.Form["txtBusinessID"].ToString(), Result, HttpContext.Request.Form["txtCustomerID"].ToString());  //Sets Message body 
                }
                if (HttpContext.Request.Form["txtResult"].ToString() == "Hylätty")
                {
                    Result = "Hylätty"; //Checks if request result is rejected
                    var body = "Pyyntötunnus: {0}<br>Pyynnön Pvm: {1}<br>Asiakkaan nimi: {2}<br>Y-tunnus: {3}<br><hr><br>Vastaus: {4}<br>Syy: {5}"; //Sets variable for message body
                    message.Body = string.Format(body, ID, HttpContext.Request.Form["txtRequestDate"].ToString(), HttpContext.Request.Form["txtName"].ToString(), HttpContext.Request.Form["txtBusinessID"].ToString(), Result, HttpContext.Request.Form["txtComment"].ToString()); //Sets Message body 
                }
                if (HttpContext.Request.Form["txtResult"].ToString() == "Odottaa")
                {
                    Result = "Lisätietoja tarvitaan";   //Checks if request result is unhandled
                    var body = "Pyyntötunnus: {0}<br>Pyynnön Pvm: {1}<br>Asiakkaan nimi: {2}<br>Y-tunnus: {3}<br><hr><br>Vastaus: {4}<br>Selite: {5}";  //Sets variable for message body
                    message.Body = string.Format(body, ID, HttpContext.Request.Form["txtRequestDate"].ToString(), HttpContext.Request.Form["txtName"].ToString(), HttpContext.Request.Form["txtBusinessID"].ToString(), Result, HttpContext.Request.Form["txtComment"].ToString()); //Sets Message body 
                }

                message.To.Add(new MailAddress(HttpContext.Request.Form["txtVendorEmail"].ToString())); //Sets Email address where Email will be sent to
                message.From = new MailAddress("");  //Input Sender Email address here
                message.Subject = "Vastaus asiakkaanperustamispyyntöön";    //Sets message subject
                message.IsBodyHtml = true;  //Sets message body to be Html formatted

                using (var smtp = new SmtpClient()) //Sets SmtpClient
                {
                    var credential = new NetworkCredential  //Sets Email credentials
                    {
                        //Input your Email Credentials
                        UserName = "",
                        Password = ""
                    };
                    smtp.Credentials = credential;  //Sets credentials for Smtp
                    smtp.Host = "smtp-mail.outlook.com";    //Sets host for Smtp
                    smtp.Port = 587;    //Sets port for Smtp
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);  //Sends message to smtp-server and waits for async
                }
            }
            return View("Message"); //returns message into a view
        }

        [HttpPost]  //Posts selected row ID into IActionResult CustomerEstablishmentRequestForm
        public IActionResult CustomerEstablishmentRequestForm(string ID)    //Sets IActionResult CustomerEstablishmentRequestForm and passes ID
        {
            using (SqlConnection con = new SqlConnection(GetConString.ConString()))    //Sets new SqlConnection using GetConString class
            {
                String sql = "SELECT * FROM CustomerInformation WHERE RequestID = @ID"; //Sets sql command string using passed ID
                SqlCommand cmd = new SqlCommand(sql, con);  //Sets new SqlCommand cmd

            var model = new List<CustomerEstablishmentRequestForm>();   //Sets new List model variable
                {
                    cmd.Parameters.AddWithValue("@ID", ID);

                    con.Open(); //Opens SqlConnection
                    SqlDataReader rdr = cmd.ExecuteReader();    //Sets SqlDataReader using SqlCommand cmd
                    while (rdr.Read())  //Reads while there are records to be red
                    {
                        var CustomerEstablishmentRequestForm = new CustomerEstablishmentRequestForm //Sets new CustomerEstablishmentRequestForm model class and passes SqlDataReader data to it
                        {
                            RequestID = (int)rdr["RequestID"],
                            RequestDate = (string)rdr["RequestDate"],
                            RequestStatus = (string)rdr["RequestStatus"],
                            Name = (string)rdr["Name"],
                            BusinessID = (string)rdr["BusinessID"],
                            Address = (string)rdr["Address"],
                            ZipCode = (string)rdr["ZipCode"],
                            City = (string)rdr["City"],
                            CountryCode = (string)rdr["CountryCode"],
                            Phone = (string)rdr["Phone"],
                            CustomerEmail = (string)rdr["CustomerEmail"],

                            Vendor = (string)rdr["Vendor"],
                            VendorEmail = (string)rdr["VendorEmail"],
                            PriceList = (string)rdr["PriceList"],
                            CustomerType = (string)rdr["CustomerType"],

                            BillingMethod = (string)rdr["BillingMethod"],
                            BillingCycle = (string)rdr["BillingCycle"],
                            PaymentCondition = (string)rdr["PaymentCondition"],
                            OvtID = (string)rdr["OvtID"],
                            EinvoiceAddress = (string)rdr["EinvoiceAddress"],
                            EinvoiceOperator = (string)rdr["EinvoiceOperator"],
                            EinvoiceOperatorID = (string)rdr["EinvoiceOperatorID"],

                        };

                        model.Add(CustomerEstablishmentRequestForm);    //Adds record to List model variable
                    }
                    con.Close();    //Closes SqlConnection
                    return View(model); //returns List model variable into a view
                }
            }
        }

        public ActionResult GetCustomerInformation()    //Sets IActionResult GetCustomerInformation
        {
            SqlConnection con = new SqlConnection(GetConString.ConString());    //Sets new SqlConnection using GetConString class
            String sql = "SELECT RequestID, RequestDate, RequestStatus, Vendor, Name FROM CustomerInformation WHERE RequestStatus = 'Avoin' OR RequestStatus = 'Odottaa'"; //Sets sql command string
            SqlCommand cmd = new SqlCommand(sql, con);  //Sets new SqlCommand cmd

            var model = new List<GetCustomerInformation>(); //Sets new List model variable
            {
                con.Open(); //Opens SqlConnection
                SqlDataReader rdr = cmd.ExecuteReader();    //Sets SqlDataReader using SqlCommand cmd
                while (rdr.Read())  //Reads while there are records to be red
                {
                    var GetCustomerInformation = new GetCustomerInformation //Sets new GetCustomerInformation model class and passes SqlDataReader data to it
                    {
                        RequestID = (int)rdr["RequestID"],
                        RequestDate = (string)rdr["RequestDate"],
                        RequestStatus = (string)rdr["RequestStatus"],
                        Vendor = (string)rdr["Vendor"],
                        Name = (string)rdr["Name"]
                    };

                model.Add(GetCustomerInformation);  //Adds record to List model variable
                }
                con.Close();    //Closes SqlConnection
            }
            return View(model); //returns List model variable into a view
        }

        DateTime date2 = DateTime.Now;  //Sets RequestDate

        [HttpPost]  //Posts CustomerInformation Form data into IActionResult SendCustomerInformationForm
        public IActionResult SendCustomerInformationForm()  //Sets controller SendCustomerInformationForm
        {
            CustomerInformation Data = new CustomerInformation  //Sets new CustomerInformation class for data and passes it into a model
            {
                RequestDate = date2.ToShortDateString(),
                RequestStatus = "Avoin",
                Name = HttpContext.Request.Form["txtName"].ToString(),
                BusinessID = HttpContext.Request.Form["txtBusinessID"].ToString(),
                Address = HttpContext.Request.Form["txtAddress"].ToString(),
                ZipCode = HttpContext.Request.Form["txtZipCode"].ToString(),
                City = HttpContext.Request.Form["txtCity"].ToString(),
                CountryCode = HttpContext.Request.Form["txtCountryCode"].ToString(),
                Phone = HttpContext.Request.Form["txtPhone"].ToString(),
                CustomerEmail = HttpContext.Request.Form["txtCustomerEmail"].ToString(),

                Vendor = HttpContext.Request.Form["txtVendor"].ToString(),
                VendorEmail = HttpContext.Request.Form["txtVendorEmail"].ToString(),
                PriceList = HttpContext.Request.Form["txtPriceList"].ToString(),
                CustomerType = HttpContext.Request.Form["slcCustomerType"].ToString(),

                BillingMethod = HttpContext.Request.Form["slcBillingMethod"].ToString(),
                BillingCycle = HttpContext.Request.Form["slcBillingCycle"].ToString(),
                PaymentCondition = HttpContext.Request.Form["slcPaymentCondition"].ToString(),
                OvtID = HttpContext.Request.Form["txtOvtID"].ToString(),
                EinvoiceAddress = HttpContext.Request.Form["txtEinvoiceAddress"].ToString(),
                EinvoiceOperator = HttpContext.Request.Form["txtEinvoiceOperator"].ToString(),
                EinvoiceOperatorID = HttpContext.Request.Form["txtEinvoiceOperatorID"].ToString(),
            };

            int result = Data.AddRecord();
            if (result > 0) //Checks if data was recorded
            {
                ViewBag.Message = "Kiitos, tiedot lähetettiin onnistuneesti!";  //Sets success message
            }
            else
            {
                ViewBag.Message = "Tietojen lähettämisessä tapahtui virhe!";    //Sets error message
            }
            return View("Message"); //returns message into a view
        }
    }
}
