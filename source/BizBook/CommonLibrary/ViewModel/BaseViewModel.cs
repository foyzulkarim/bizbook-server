using System;
using CommonLibrary.Model;

namespace CommonLibrary.ViewModel
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
            CreatedFrom = x.CreatedFrom;
            IsActive = x.IsActive;
        }


        public bool IsActive { get; set; }

        [IsViewable]
        public string Id { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedFrom { get; set; }
    
        public DateTime Modified { get; set; }

        public string ModifiedBy { get; set; }
    }
}