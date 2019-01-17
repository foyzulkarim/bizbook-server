using System;
using System.Web.Http;
using CommonLibrary.Repository;
using Model;
using Model.Sales;
using RequestModel.Sales;
using Server.Inventory.Filters;
using ServiceLibrary.Sales;
using ViewModel.Sales;

namespace Server.Inventory.Controllers.CommandControllers.Sales
{
    using System.Collections.Generic;
    using System.Configuration;

    using Model.Message;
    using Model.Shops;

    using ServiceLibrary.Messages;
    using ServiceLibrary.Shops;

    using ViewModel.Shops;

    [RoutePrefix("api/Sale")]
    public class SaleController : BaseCommandController<Sale, SaleRequestModel, SaleViewModel>
    {
        SaleService saleService;

        public SaleController() : base(new SaleService(new BaseRepository<Sale>(BusinessDbContext.Create())))
        {
            this.saleService = this.Service as SaleService;
        }

        [HttpPut]
        [Route("NextState")]
        [ActionName("NextState")]
        [EntityEditFilter]
        public IHttpActionResult NextState(Sale model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var edit = this.saleService.NextState(model);
                Logger.Debug("User {@UserName} ConnectionId {@ConnectionId} NextState entity {TypeName} with value {Id}", this.AppUser.Id, this.AppUser.ConnectionId, typeName, model.Id);
               
                return Ok(edit);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while moving Next State {TypeName}", typeName);
                return InternalServerError(exception);
            }
        }


        [HttpPut]
        [Route("NextStateAll")]
        [ActionName("NextStateAll")]
        //[EntityEditFilter]
        public IHttpActionResult NextState(List<Sale> models)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                foreach (Sale model in models)
                {
                    var edit = this.saleService.NextState(model);
                    Logger.Debug("User {@UserName} ConnectionId {@ConnectionId} NextState entity {TypeName} with value {Id}", this.AppUser.Id, this.AppUser.ConnectionId, typeName, model.Id);
                }
              
                return Ok(true);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while moving Next State {TypeName}", typeName);
                return InternalServerError(exception);
            }
        }

        [HttpPut]
        [Route("UpdateState")]
        [ActionName("UpdateState")]
        [EntityEditFilter]
        public IHttpActionResult UpdateState(Sale model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var edit = this.saleService.UpdateState(model, model.NextOrderState);
                Logger.Debug("User {@UserName} ConnectionId {@ConnectionId} NextState entity {TypeName} with value {Id}", this.AppUser.Id, this.AppUser.ConnectionId, typeName, model.Id);
                model.OrderState = model.NextOrderState;
                SendSms(model);
                return Ok(edit);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while moving Next State {TypeName}", typeName);
                return InternalServerError(exception);
            }
        }

        [HttpPut]
        [Route("UpdateStateAll")]
        [ActionName("UpdateStateAll")]
        public IHttpActionResult UpdateState(List<Sale> models)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var model in models)
                {
                    var edit = this.saleService.UpdateState(model, model.NextOrderState);                   
                    Logger.Debug("User {@UserName} ConnectionId {@ConnectionId} NextState entity {TypeName} with value {Id}", this.AppUser.Id, this.AppUser.ConnectionId, typeName, model.Id);
                }

                foreach (var model in models)
                {
                    model.OrderState = model.NextOrderState;
                }

                SendSmses(models);                             
                return Ok(true);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while moving Next State {TypeName}", typeName);
                return InternalServerError(exception);
            }
        }

        [HttpPut]
        [Route("Return")]
        [ActionName("Return")]
        [EntityEditFilter]
        public IHttpActionResult Return(Sale model)
        {
            var data = model;
            if (!ModelState.IsValid)
            {
                Logger.Warning("User {@UserName} ConnectionId {@ConnectionId} sent Invalid model state {@Data}", this.AppUser.UserName, this.AppUser.ConnectionId, data);
                return BadRequest(ModelState);
            }

            try
            {
                var service = Service as SaleService;
                bool returned = service.Edit(model);
                Logger.Debug("User {@UserName} ConnectionId {@ConnectionId} sale returned with Id {@Id}", this.AppUser.UserName, this.AppUser.ConnectionId, data.Id);
            
                return Ok(returned);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Logger.Fatal(exception, "Exception occurred while updating sale return");
                return InternalServerError(exception);
            }
        }


        [HttpPut]
        [Route("Return2")]
        [ActionName("Return2")]
        [EntityEditFilter]
        public IHttpActionResult Return2(Sale model)
        {
            var data = model;
            if (!ModelState.IsValid)
            {
                Logger.Warning("User {@UserName} ConnectionId {@ConnectionId} sent Invalid model state {@Data}", this.AppUser.UserName, this.AppUser.ConnectionId, data);
                return BadRequest(ModelState);
            }

            try
            {
                var service = Service as SaleService;
                bool returned = service.SaleReturn(model);
                Logger.Debug("User {@UserName} ConnectionId {@ConnectionId} sale returned with Id {@Id}", this.AppUser.UserName, this.AppUser.ConnectionId, data.Id);

                return Ok(returned);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Logger.Fatal(exception, "Exception occurred while updating sale return");
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("Add")]
        [ActionName("Add")]
        [EntitySaveFilter]
        public override IHttpActionResult Add(Sale model)
        {
            Sale data = model;
            if (!ModelState.IsValid)
            {
                Logger.Warning("User {@AppUser} sent Invalid model state {@Data}", this.AppUser, data);
                return BadRequest(ModelState);
            }

            try
            {
                var add = Service.Add(model);
                if (add)
                {
                    SendSms(model);
                }
                
                Logger.Debug("User {@UserName} ConnectionId {@ConnectionId} Added entity {TypeName} {@Id}", this.AppUser.UserName, this.AppUser.ConnectionId, typeName, data.Id);
                return Ok(new { model.Id, model.OrderNumber });
            }
            catch (Exception exception)
            {
                Logger.Fatal(
                    exception,
                    "Exception occurred while saving {@Data} by User {@AppUser}",
                    data,
                    this.AppUser);
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("SalesDuesUpdate")]
        [ActionName("SalesDuesUpdate")]
        public IHttpActionResult SalesDuesUpdate(SalesDuesUpdateModel model)
        {                        
            try
            {
                model.ShopId = this.AppUser.ShopId;
                model.Transaction.Id = Guid.NewGuid().ToString();
                model.Transaction.Created = DateTime.Now;
                model.Transaction.Modified = DateTime.Now;
                model.Transaction.CreatedBy = this.AppUser.UserName;
                model.Transaction.ModifiedBy = AppUser.UserName;
                model.Transaction.IsActive = true;
                model.Transaction.ShopId = this.AppUser.ShopId;

                bool resp = saleService.SalesDuesUpdate(model);
                return this.Ok(model);
            }
            catch (Exception exception)
            {
                Logger.Fatal(
                    exception,
                    "Exception occurred while SalesDuesUpdate {@Data} by User {@AppUser}",
                    model,
                    this.AppUser);
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("AddLocal")]
        [ActionName("AddLocal")]      
        public IHttpActionResult AddLocal(Sale model)
        {
            Sale data = model;
            if (!ModelState.IsValid)
            {
                Logger.Warning("User {@AppUser} sent Invalid model state {@Data}", this.AppUser, data);
                return BadRequest(ModelState);
            }

            try
            {
                
                var add = this.saleService.Add2(model);                
                Logger.Debug("User {@UserName} ConnectionId {@ConnectionId} Added entity {TypeName} {@Id}", this.AppUser.UserName, this.AppUser.ConnectionId, typeName, data.Id);
                return Ok(new { model.Id, model.OrderNumber });
            }
            catch (Exception exception)
            {
                Logger.Fatal(
                    exception,
                    "Exception occurred while saving {@Data} by User {@AppUser}",
                    data,
                    this.AppUser);
                return InternalServerError(exception);
            }
        }

        private void SendSms(Sale model)
        {
            try
            {
                string apiUrl = ConfigurationManager.AppSettings["SmsApi"];
                BusinessDbContext db = BusinessDbContext.Create();
                SmsService smsService = new SmsService(new BaseRepository<Sms>(db));
                ShopService shopService = new ShopService(new BaseRepository<Shop>(db));
                ShopViewModel shop = shopService.GetMyDetail(model.ShopId);
                bool smsSent = smsService.SendSms(model, shop, apiUrl);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while sending SMS " + model.Id);
            }
        }

        private void SendSmses(List<Sale> sales)
        {
            string apiUrl = ConfigurationManager.AppSettings["SmsApi"];
            BusinessDbContext db = BusinessDbContext.Create();
            SmsService smsService = new SmsService(new BaseRepository<Sms>(db));
            ShopService shopService = new ShopService(new BaseRepository<Shop>(db));
            ShopViewModel shop = shopService.GetMyDetail(this.AppUser.ShopId);
            foreach (Sale model in sales)
            {
                try
                {
                    bool smsSent = smsService.SendSms(model, shop, apiUrl);
                }
                catch (Exception exception)
                {
                    Logger.Fatal(exception, "Exception occurred while sending SMS " + model.Id);
                }
            }
        }        
    }

}