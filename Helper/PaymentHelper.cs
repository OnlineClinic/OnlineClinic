using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;
using System.Net;
using MyOnlineClinic.Web.Models;
using CreditCardValidator;
using Newtonsoft.Json;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Entity;
namespace MyOnlineClinic.Web.Helper
{
    public class PaymentHelper : IPaymentHelper
    {
        IPaymentHistoryService _paymentHistoryService;
        IPaymentCaptureService _paymentCaptureService;
        IPaymentRefundService _paymentRefundService;
        public PaymentHelper(IPaymentHistoryService paymentHistoryService,
            IPaymentCaptureService paymentCaptureService, IPaymentRefundService paymentRefundService)
        {
            _paymentHistoryService = paymentHistoryService;
            _paymentCaptureService = paymentCaptureService;
            _paymentRefundService = paymentRefundService;
        }
        public Models.PaypalResponseModel PayWithCreditCard(CreditCardViewModel model)
        {
            PaypalResponseModel objModel = new PaypalResponseModel();
            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                var apiContext = MyOnlineClinic.Web.Models.Configuration.GetAPIContext();

                CreditCardValidator.CreditCardDetector obj = new CreditCardDetector(model.number);
                if (obj.IsValid())
                {

                    // A transaction defines the contract of a payment - what is the payment for and who is fulfilling it. 
                    CreditCard credtCard = new CreditCard();
                    credtCard.type = obj.BrandName.ToLower();
                    //credtCard.number = "4446283280247004";
                    credtCard.number = model.number;
                    credtCard.expire_month = model.expire_month;
                    credtCard.expire_year = model.expire_year;
                    credtCard.first_name = model.first_name;
                    credtCard.last_name = model.last_name;

                    FundingInstrument fundInstrument = new FundingInstrument();
                    fundInstrument.credit_card = credtCard;

                    List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
                    fundingInstrumentList.Add(fundInstrument);

                    PayPal.Api.Payer payr = new PayPal.Api.Payer { funding_instruments = fundingInstrumentList, payment_method = "credit_card" };

                    PayPal.Api.Amount amnt = new PayPal.Api.Amount { currency = model.Currency, total = model.ConsultFee.ToString() };

                    PayPal.Api.Transaction tran = new PayPal.Api.Transaction();
                    tran.description = "creating a direct payment with credit card";
                    tran.amount = amnt;

                    List<PayPal.Api.Transaction> transactions = new List<PayPal.Api.Transaction> { tran };

                    PayPal.Api.Payment pymnt = new PayPal.Api.Payment
                    {
                        intent = "authorize",
                        payer = payr,
                        transactions = transactions
                    };
                    var rep = pymnt.Create(apiContext);

                    if (rep.state == "approved")
                    {
                        objModel.Status = rep.state;
                        objModel.StatusCode = rep.state;
                        objModel.ResponseString = rep.id;
                        objModel.AuthorizationId = rep.transactions.FirstOrDefault().related_resources.FirstOrDefault().authorization.id;
                        var paymentHisotry = Common.MapPayPalResponseWithHistory(rep, model);
                        var t = rep.ConvertToJson();
                        _paymentHistoryService.Add(paymentHisotry);

                    }
                    return objModel;
                }
                return objModel;
            }
            catch (PayPal.PayPalException ex)
            {
                objModel.ResponseString = ex.Message;
                return objModel;
            }
        }


        public PaypalResponseModel CapturePayment(CaptureViewModel model)
        {
            PaypalResponseModel objModel = new PaypalResponseModel();
            try
            {
                
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                
                var apiContext = MyOnlineClinic.Web.Models.Configuration.GetAPIContext();
                
                PayPal.Api.Authorization authorization = new PayPal.Api.Authorization { id = model.AuthorizationId };
                
                var amnt = new PayPal.Api.Amount { currency = model.Currency, total = model.Amount };
                
                var capture = new Capture { amount = amnt, is_final_capture = true };
                
                var authCapture = authorization.Capture(apiContext, capture);

                if (authCapture.state == "complete")
                {
                    PaymentCapture objCapture = new PaymentCapture();
                    objCapture.CaptureId = authCapture.id;
                    objCapture.PaymentHistoryId = model.PaymentHistoryId;
                    _paymentCaptureService.Add(objCapture);                   
                    objModel.CaptureId = authCapture.id;
                    objModel.ResponseString = authCapture.parent_payment;
                    return objModel;
                }
                else {
                    return objModel;
                }
            }
            catch (Exception ex)
            {
                return objModel; 
            }
        }

        public PaypalResponseModel RefundPayment(RefundViewModel model)
        {
            PaypalResponseModel objModel = new PaypalResponseModel();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                var apiContext = MyOnlineClinic.Web.Models.Configuration.GetAPIContext();
                var capture = new Capture { id = model.CaptureId };
                var refundAmount = new PayPal.Api.Amount { currency = model.Currency, total = model.Amount };
                var refund = new Refund { amount = refundAmount };
                var captureRefundDetails = capture.Refund(apiContext, refund);
                return objModel;
            }
            catch (Exception ex)
            {
                return objModel;
            }
        }
    }
}