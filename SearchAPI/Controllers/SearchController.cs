using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using SearchAPI.Common.Classes.Identity;
using SearchAPI.Models;
using SearchAPI.Repository;
using SearchAPI.ODataSettings;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IRepository<Products> _repProduct;

        public SearchController(ILogger<SearchController> logger, IRepository<Products> repProduct)
        {
            _logger = logger;
            _repProduct = repProduct;
        }

        // GET: api/<SearchController>
        [HttpGet]
        [EnableQueryWithCustomSettings]
        [ResponseCache(Location = ResponseCacheLocation.Client, VaryByQueryKeys =new string[] { "$select","$filter","$top","$skip","$orderby" },Duration = 600)]
        public async Task<IActionResult> Get()
        {
            IQueryable<Products> pdtList = _repProduct.GetAll();
            return Ok(pdtList);
        }
    }
}
