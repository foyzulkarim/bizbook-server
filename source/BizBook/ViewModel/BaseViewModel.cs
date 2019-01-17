using System;
using Model;

namespace ViewModel
{
    public abstract class BaseViewModel<T> where T : Entity
    {
        protected BaseViewModel(Entity x)
        {
            Id = x.Id;
            Created = x.Created;
            CreatedBy = x.CreatedBy;
            Modified = x.Modified;
            ModifiedBy = x.ModifiedBy;
        }

        protected BaseViewModel()
        {
        }

        public string Id { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTime Modified { get; set; }

        public string ModifiedBy { get; set; }
    }
}