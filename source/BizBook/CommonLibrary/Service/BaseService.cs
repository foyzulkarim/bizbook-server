using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CommonLibrary.Model;
using CommonLibrary.Repository;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;

namespace CommonLibrary.Service
{
    public class BaseService<T, TRm, TVm> where T : Entity where TRm : RequestModel<T> where TVm : BaseViewModel<T>
    {
        protected BaseRepository<T> Repository;

        public BaseService(BaseRepository<T> repository)
        {
            Repository = repository;
        }

        public virtual bool Add(T entity)
        {
            var add = Repository.Add(entity);
            var save = Repository.Save();
            return save;
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            return Repository.Filter(filter, orderBy, includeProperties);
        }

        public bool Delete(T entity)
        {
            bool deleted = Repository.Delete(entity);
            Repository.Save();
            return deleted;
        }

        public bool Delete(string id)
        {
            var entity = Repository.Filter(x => x.Id == id).FirstOrDefault();
            bool deleted = Repository.Delete(entity);
            Repository.Save();
            return deleted;
        }

        public virtual bool Edit(T entity)
        {
            bool edit = Repository.Edit(entity);
            Repository.Save();
            return edit;
        }

        public T GetById(string id)
        {
            return Repository.GetById(id);
        }

        public async Task<List<TVm>> GetAllAsync()
        {
            var queryable = await Repository.Get().ToListAsync();
            var vms = queryable.Select(x => (TVm)Activator.CreateInstance(typeof(TVm), new object[] { x }));
            return vms.ToList();
        }

        public List<DropdownViewModel> GetDropdownList(TRm request)
        {
            IQueryable<T> queryable = Repository.Get();
            queryable = request.GetOrderedData(queryable);
            List<DropdownViewModel> list = queryable.Select(request.Dropdown()).ToList();
            return list;
        }

        public async Task<Tuple<List<DropdownViewModel>, int>> GetDropdownListAsync(TRm request)
        {
            IQueryable<T> queryable = Repository.Get();
            queryable = request.GetOrderedData(queryable);
            List<DropdownViewModel> list = await queryable.Select(request.Dropdown()).ToListAsync();
            return new Tuple<List<DropdownViewModel>, int>(list, list.Count);
        }

        public async Task<Tuple<List<TVm>, int>> SearchAsync(TRm request)
        {
            var queryable = request.GetOrderedData(Repository.Get());
            int count = queryable.Count();
            queryable = request.SkipAndTake(queryable);
            if (request.IsIncludeParents)
            {
                queryable = request.IncludeParents(queryable);
            }
            
            var list = await queryable.ToListAsync();
            List<TVm> vms = list.ConvertAll(CreateVmInstance);
            return new Tuple<List<TVm>, int>(vms, count);
        }

        private static TVm CreateVmInstance(T x)
        {
            return (TVm)Activator.CreateInstance(typeof(TVm), x);
        }

        public virtual TVm GetDetail(string id)
        {
            var model = Repository.GetById(id);
            if (model == null)
            {
                return null;
            }
            return CreateVmInstance(model);
        }

        public virtual bool SyncList(List<T> entities)
        {
            foreach (T e in entities)
            {
                Upsert(e);
            }
            return true;
        }

        public virtual bool Upsert(T e)
        {
            T dbModel = GetById(e.Id);
            bool success;
            if (dbModel == null)
            {
                e.Modified = DateTime.UtcNow;
                success = Add(e);
            }
            else
            {
                Repository.Db.Entry(dbModel).State = EntityState.Detached;
                e.Modified = DateTime.UtcNow;
                success = Edit(e);
            }
            return success;
        }

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
    }

}
