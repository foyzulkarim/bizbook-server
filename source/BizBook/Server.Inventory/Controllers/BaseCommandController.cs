using System;
using System.Web.Http;
using CommonLibrary.Model;
using CommonLibrary.RequestModel;
using CommonLibrary.Service;
using CommonLibrary.ViewModel;
using Serilog;
using Server.Inventory.Attributes;
using Server.Inventory.Filters;
using Server.Inventory.Models;

namespace Server.Inventory.Controllers
{
    using Model;

    [BizBookAuthorization]
    public abstract class BaseCommandController<T, TRm, TVm> : ApiController where T : Entity where TRm : RequestModel<T> where TVm : BaseViewModel<T>
    {
        public static ILogger Logger = Log.ForContext(typeof(BaseCommandController<T, TRm, TVm>));
        protected BaseService<T, TRm, TVm> Service;
        protected string typeName = string.Empty;
        public ApplicationUser AppUser;

        protected BaseCommandController(BaseService<T, TRm, TVm> service)
        {
            Service = service;
            typeName = typeof(T).Name;
        }     

        [HttpPost]
        [Route("Add")]
        [ActionName("Add")]
        [EntitySaveFilter]
        public virtual IHttpActionResult Add(T model)
        {
            T data = model;
            if (!ModelState.IsValid)
            {
                Logger.Warning("User {@AppUser} sent Invalid model state {@Data}", this.AppUser, data);
                return BadRequest(ModelState);
            }

            try
            {
                var add = Service.Add(model);
                Logger.Debug("User {@UserName} ConnectionId {@ConnectionId} Added entity {TypeName} {@Id}", this.AppUser.UserName, this.AppUser.ConnectionId, typeName, data.Id);
                return Ok(model.Id);
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

        [HttpPut]
        [Route("Edit")]
        [ActionName("Edit")]
        [EntityEditFilter]
        public virtual IHttpActionResult Put(T model)
        {
            T data = model;
            if (!ModelState.IsValid)
            {
                Logger.Warning("User {@AppUser} sent Invalid model state {@Data}", this.AppUser, data);
                return BadRequest(ModelState);
            }

            try
            {
                var edit = Service.Edit(model);
                Logger.Debug("User {@UserName} ConnectionId {@ConnectionId} edited entity {TypeName} {@Id}", this.AppUser.UserName, this.AppUser.ConnectionId, typeName, data.Id);                
                return Ok(edit);
            }
            catch (Exception exception)
            {
                Logger.Error(
                    exception,
                    "Exception occurred while editing {TypeName} {@Data} by User {@AppUser}",
                    typeName,
                    data,
                    this.AppUser);
                return InternalServerError(exception);
            }
        }

        [Route("Delete")]
        [ActionName("Delete")]
        [HttpDelete]
        public virtual IHttpActionResult Delete(string id)
        {
            try
            {
                var delete = Service.Delete(id);
                Logger.Debug("User {@AppUser} Deleted entity {TypeName} {@Id} ", this.AppUser, typeName, id);
                return Ok(delete);
            }
            catch (Exception exception)
            {
                Logger.Fatal(
                    exception,
                    "Exception occurred while editing {TypeName} {@Id} by User {@AppUser}",
                    typeName,
                    id,
                    this.AppUser);
                return InternalServerError(exception);
            }
        }

        //[Route("Sync")]
        //[ActionName("Sync")]
        //[HttpPost]
        //public virtual IHttpActionResult Sync(List<T> models)
        //{
        //    var m = models.Select(x => new { x.Id, x.Modified, x.ModifiedBy }).ToList();
        //    string s = JsonConvert.SerializeObject(m);
        //    Logger.Information(this.typeName + " Sync models: {@S}", s);
        //    try
        //    {
        //        bool add = this.Service.SyncList(models);
        //        return this.Ok(add);
        //    }
        //    catch (Exception exception)
        //    {
        //        Logger.Fatal(exception, "Exception occurred while saving ProductCategory Groups");
        //        return this.InternalServerError(exception);
        //    }
        //}

        protected Entity AddCommonValues(Entity fromEntity, Entity toEntity)
        {
            toEntity.Id = Guid.NewGuid().ToString();
            toEntity.Created = fromEntity.Created;
            toEntity.CreatedFrom = fromEntity.CreatedFrom;
            toEntity.CreatedBy = fromEntity.CreatedBy;
            toEntity.Modified = fromEntity.Modified;
            toEntity.ModifiedBy = fromEntity.ModifiedBy;
            toEntity.IsActive = true;
            return toEntity;
        }

        protected ShopChild AddCommonValues(ShopChild from, ShopChild to)
        {
            AddCommonValues((Entity)from, (Entity)to);
            to.ShopId = from.ShopId;
            return to;
        }
    }
}
