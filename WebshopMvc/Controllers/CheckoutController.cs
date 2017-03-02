using Rotativa;
using Rotativa.Options;
using ShopDAL;
using ShopDAL.DomainModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebshopMvc.Controllers
{
    public class CheckoutController : Controller
    {
        string orderEmail = "";
        FacadeDAL facade = new FacadeDAL();
        const string PromoCode = "FREE";
        // GET: Checkout
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                //if (string.Equals(values["PromoCode"], PromoCode,
                //    StringComparison.OrdinalIgnoreCase) == false)
                //{
                //    return View(order);
                //}
                //else
                //{
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;
                orderEmail = order.Email;
                //orderId = order.OrderId;
                //Save Order
                facade.GetCheckoutRepository().AddOrder(order);
                //Process the order
                var cart = facade.GetShoppingCartRepository().GetCart(this.HttpContext);
                cart.CreateOrder(order);

                //return RedirectToAction("Receipt", new { id = order.OrderId });
                return RedirectToAction("Receipt", new { id = order.OrderId });
                //}
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }




        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            string userName = User.Identity.Name;
            bool isValid = facade.GetCheckoutRepository().CheckIfComplete(id, userName);
            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Receipt(int id)
        {
            string userName = User.Identity.Name;
            bool isValid = facade.GetCheckoutRepository().CheckIfComplete(id, userName);
            if (isValid)
            {
                var order = facade.GetCheckoutRepository().GetCurrentOrder(id);
                var model = new Order();
                foreach (var item in order)
                {

                    model.FirstName = item.FirstName;
                    model.LastName = item.LastName;
                    model.Address = item.Address;
                    model.City = item.City;
                    model.Country = item.Country;
                    model.Email = item.Email;
                    model.OrderDetails = item.OrderDetails;
                    model.OrderDate = item.OrderDate;
                    model.Total = item.Total;
                    model.OrderId = item.OrderId;

                }
                return new ViewAsPdf(model);
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult GetPdfreceipt()
        {
            var actionPDF = new ActionAsPdf("Receipt")
            {
                FileName = "TestView.pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Landscape,
                PageMargins = { Left = 1, Right = 1 }
            };
            Stream applicationPDFData = new MemoryStream(actionPDF.BuildPdf(ControllerContext));
            return RedirectToAction("SendOrder", applicationPDFData);
        }
        public async Task<ActionResult> SendOrder(Stream stream)
        {
            //Stream stream = new MemoryStream(applicationPDFData);
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(orderEmail);  // replace with valid value 
                message.From = new MailAddress("sender@mail.com");  // replace with valid value
                message.Subject = "Receipt";
                message.Body = string.Format(body, "Thank you for choosing us!");
                message.Attachments.Add(new Attachment(stream, "Receipt", "application/pdf"));
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "yourmail@mail.com",  // replace with valid value
                        Password = "yourCode"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    //return RedirectToAction("Sent");
                }
            }
            return RedirectToAction("Receipt");
        }

    }
}