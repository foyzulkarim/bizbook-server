using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Transactions;

using RequestModel.Transactions;
using ViewModel.Transactions;

namespace Server.Inventory.Controllers.QueryControllers.Transactions
{
    [RoutePrefix("api/TransactionQuery")]
    public class TransactionQueryController : BaseQueryController<Transaction, TransactionRequestModel, TransactionViewModel>
    {
        public TransactionQueryController() : base(new BaseService<Transaction, TransactionRequestModel, TransactionViewModel>(new BaseRepository<Transaction>(BusinessDbContext.Create())))
        {

        }

        [ActionName("Dropdowns")]
        [HttpGet]
        [Route("Dropdowns")]
        public IHttpActionResult GetDropdowns()
        {
            List<string> transactionFors = Enum.GetNames(typeof(TransactionFor)).ToList();
            List<string> transactionWiths = Enum.GetNames(typeof(TransactionWith)).ToList();
            List<string> transactionMediums = Enum.GetNames(typeof(TransactionMedium)).ToList();
            List<string> transactionFlowTypes = Enum.GetNames(typeof(TransactionFlowType)).ToList();
            List<string> paymentGatewayServices = Enum.GetNames(typeof(PaymentGatewayService)).ToList();
            List<string> accountTypes = Enum.GetNames(typeof (AccountHeadType)).ToList();
            List<string> accountInfoTypes = Enum.GetNames(typeof(WalletType)).ToList();

            var dropdowns = new
            {
                transactionFors,
                transactionWiths,
                transactionMediums,
                transactionFlowTypes,
                paymentGatewayServices,
                accountTypes,
                accountInfoTypes
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dropdowns);
            return ResponseMessage(response);
        }        
    }
}
