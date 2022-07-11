using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

using Twilio.TwiML;
using Twilio.AspNet.Mvc;
using AceTuitionPaymentSystem.Models;

namespace AceTuitionPaymentSystem.Controllers
{
    public class SMSController : TwilioController
    {
        // GET: SMS
        public void SendSMS(string messageBody, string[] paymentList, string[] parentList, string messageType)
        {

            var accountSid = "AC4af61209ca0d55085914c89215390027";
            var authToken = "305f5872eb4f2f67fb8e87db498fad08";
            TwilioClient.Init(accountSid, authToken);

            
            var from = new PhoneNumber("+15033609335");
            var whatsappFrom = new PhoneNumber("whatsapp:+14155238886");


            using (var context = new AceTuitionEntities())
            {
                if (messageType == "whatsapp")
                {

                    List<string> parentPhoneList = new List<string>();

                    foreach (var parentIC in parentList)
                    {
                        var parent = context.tb_parent.Where(x => x.parent_ic == parentIC).SingleOrDefault();
                        parentPhoneList.Add(parent.parent_phone);

                    }

                    var phoneList = parentPhoneList.Select(x => x).Distinct();

                    foreach (var phone in phoneList)
                    {
                        var to = new PhoneNumber("whatsapp:+6" + phone);
                        var message = MessageResource.Create(
                            body: messageBody,
                            from: whatsappFrom,
                            to: to
                        );
                    }

                }
                else if (messageType == "sms")
                {
                    List<string> parentPhoneList = new List<string>();

                    foreach (var paymentIC in parentList)
                    {
                        var parent = context.tb_parent.Where(x => x.parent_ic == paymentIC).SingleOrDefault();
                        parentPhoneList.Add(parent.parent_phone);

                    }

                    var phoneList = parentPhoneList.Select(x => x).Distinct();

                    foreach (var phone in phoneList)
                    {
                        var to = new PhoneNumber("+6" + phone);
                        var message = MessageResource.Create(
                            to: to,
                            from: from,
                            body: messageBody
                        );
                    }
                }


            }




            //using (var context = new AceTuitionEntities())
            //{
            //    var outstanding = context.tb_payment.Where(x => x.payment_status == "outstanding").ToList();

            //    List<string> parentList = new List<string>();

            //    foreach(var payment in outstanding)
            //    {
            //        var parent = context.tb_parent.Where(x => x.parent_ic == payment.parent_ic).SingleOrDefault();
            //        parentList.Add(parent.parent_phone);

            //    }

            //    var phoneList = parentList.Select(x => x).Distinct();

            //    foreach(var phone in phoneList)
            //    {
            //        var to = new PhoneNumber("+6"+ phone);
            //        var message = MessageResource.Create(
            //            to: to,
            //            from: from,
            //            body: messageBody
            //        );
            //    }
            //}




        }
    }
}