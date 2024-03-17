using Microsoft.AspNetCore.OData.Query;

namespace SearchAPI.ODataSettings
{
    public class EnableQueryWithCustomSettingsAttribute : EnableQueryAttribute
    {
        const int pageSizeDefault = 20;
        public override IQueryable ApplyQuery(IQueryable queryable, ODataQueryOptions queryOptions)
        {
            int? top = queryOptions?.Top?.Value;
            if (top == null)
            {
                return queryOptions.ApplyTo(queryable,
                    new ODataQuerySettings
                    {
                        PageSize = this.PageSize == 0 ? pageSizeDefault : this.PageSize
                    });

            }

            return queryOptions.ApplyTo(queryable); ;
        }
    }
}
