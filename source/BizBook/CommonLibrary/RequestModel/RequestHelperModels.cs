using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CommonLibrary.Model;
using CommonLibrary.ViewModel;

namespace CommonLibrary.RequestModel
{
    using System.Data.Entity;

    public class OrderByRequest
    {
        public string PropertyName { get; set; }
        public bool IsAscending { get; set; }
    }


    public abstract class RequestModel<TModel> where TModel : Entity
    {
        protected const string All = "All";

        protected Expression<Func<TModel, bool>> ExpressionObj = e => true;

        protected RequestModel(string keyword, string orderBy = "Modified", string isAscending = "False")
        {
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = "";
            }
            Page = 1;
            Keyword = keyword.ToLower();
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy;
            }
            if (!string.IsNullOrWhiteSpace(isAscending))
            {
                IsAscending = isAscending;
            }

            Request = new OrderByRequest
            {
                PropertyName = string.IsNullOrWhiteSpace(OrderBy) ? "Modified" : OrderBy,
                IsAscending = string.IsNullOrWhiteSpace(isAscending) ? false : bool.Parse(isAscending)
            };

        }

        public string OrderBy { get; set; }
        public string IsAscending { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DateSearchColumn { get; set; }
        public int PerPageCount { get; set; } = 10;
        public int Page { get; set; }
        public List<string> Tables { get; set; }
        public string Id { get; set; }
        public OrderByRequest Request { get; }
        public string Keyword { get; set; }
        public string ParentId { get; set; }
        public string ShopId { get; set; }

        public bool IsIncludeParents { get; set; }


        protected Func<IQueryable<TSource>, IOrderedQueryable<TSource>> OrderByFunc<TSource>()
        {
            string propertyName = Request.PropertyName;
            bool ascending = Request.IsAscending;
            var source = Expression.Parameter(typeof(IQueryable<TSource>), "source");
            var item = Expression.Parameter(typeof(TSource), "item");
            var member = Expression.Property(item, propertyName);
            var selector = Expression.Quote(Expression.Lambda(member, item));
            var body = Expression.Call(
                typeof(Queryable), @ascending ? "OrderBy" : "OrderByDescending",
                new[] { item.Type, member.Type },
                source, selector);
            var expr = Expression.Lambda<Func<IQueryable<TSource>, IOrderedQueryable<TSource>>>(body, source);
            var func = expr.Compile();
            return func;
        }
        protected abstract Expression<Func<TModel, bool>> GetExpression();
        //public abstract IQueryable<TModel> IncludeParents(IQueryable<TModel> queryable);

        public virtual IQueryable<TModel> IncludeParents(IQueryable<TModel> queryable)
        {
            return queryable;
        }

        public abstract Expression<Func<TModel, DropdownViewModel>> Dropdown();
        public IQueryable<TModel> GetOrderedData(IQueryable<TModel> queryable)
        {
            queryable = queryable.Where(GetExpression());
            queryable = OrderByFunc<TModel>()(queryable);
            return queryable;
        }

        public IQueryable<TModel> SkipAndTake(IQueryable<TModel> queryable)
        {
            if (Page != -1)
            {
                queryable = queryable.Skip((Page - 1) * PerPageCount).Take(PerPageCount);
            }

            return queryable;
        }

        public IQueryable<TModel> GetData(IQueryable<TModel> queryable)
        {
            return queryable.Where(GetExpression());
        }

        public TModel GetFirstData(IQueryable<TModel> queryable)
        {
            return queryable.First(GetExpression());
        }

        protected Expression<Func<TModel, bool>> GenerateBaseEntityExpression()
        {
            if (Id.IdIsOk())
            {
                ExpressionObj = ExpressionObj.And(x => x.Id == Id);
            }

            if (StartDate != new DateTime())
            {
                StartDate = StartDate.Date;
                if (EndDate != new DateTime())
                {
                    EndDate = EndDate.Date.AddDays(1).AddMinutes(-1);
                }
                else
                {
                    EndDate = DateTime.Today.Date.AddDays(1).AddMinutes(-1);
                }


                if (string.IsNullOrWhiteSpace(DateSearchColumn))
                {
                    ExpressionObj = ExpressionObj.And(x => DbFunctions.TruncateTime(x.Modified) >= StartDate && DbFunctions.TruncateTime(x.Modified) <= EndDate);
                }
            }
            return ExpressionObj;
        }
    }

    public static class ExpressionHelper
    {
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static bool IdIsOk(this string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }
            //Guid guid;
            //if (!Guid.TryParse(id, out guid))
            //{
            //    return false;
            //}
            //if (guid == new Guid())
            //{
            //    return false;
            //}
            return true;
        }

        public static bool IdIsNotDefaultGuid(this string id)
        {
            Guid guid;

            if (!Guid.TryParse(id,out guid))
            {
                return false;
            }

            if (guid==new Guid())
            {
                return false;
            }

            return true;
        }

    }

    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
            Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }

    public class ReplaceExpressionVisitor
        : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
                return _newValue;
            return base.Visit(node);
        }
    }
}