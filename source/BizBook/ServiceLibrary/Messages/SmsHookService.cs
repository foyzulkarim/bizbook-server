using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLibrary.Messages
{
    using System.Data.Entity;

    using CommonLibrary.Repository;
    using CommonLibrary.Service;

    using Model;
    using Model.Message;

    using RequestModel.Message;

    using ViewModel.Message;
    public class SmsHookService : BaseService<SmsHook, SmsHookRequestModel, SmsHookViewModel>
    {
        public SmsHookService(BaseRepository<SmsHook> repository)
            : base(repository)
        {
        }

        public async Task<List<SmsHook>> SearchHooks(SmsHookRequestModel request)
        {
            var hookNames = Enum.GetNames(typeof(BizSmsHook)).ToList();
            List<SmsHook> smsHooks = await request.GetOrderedData(Repository.Get()).ToListAsync();

            foreach (var n in hookNames)
            {
                BizSmsHook bizSmsHook = (BizSmsHook)Enum.Parse(typeof(BizSmsHook), n, true);
                var exists = smsHooks.Exists(x => x.BizSmsHook == bizSmsHook);
                if (!exists)
                {
                    // insert
                    SmsHook hook = new SmsHook
                                       {
                                           BizSmsHook = bizSmsHook,
                                           IsEnabled = false,
                                           Name = bizSmsHook.ToString(),
                                           ShopId = request.ShopId,
                                           Created = DateTime.Now,
                                           Modified = DateTime.Now,
                                           CreatedBy = "System",
                                           ModifiedBy = "System",
                                           Id = Guid.NewGuid().ToString(),
                                           IsActive = true,
                                           CreatedFrom = "System",
                                           Description = string.Empty
                                       };

                    bool add = this.Add(hook);
                    smsHooks.Add(hook);
                }
            }
            
            return smsHooks;
        }
    }
}
