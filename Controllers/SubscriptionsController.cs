using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EzhikLoader.Server.Data;

namespace EzhikLoader.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly MyDbContext _dbContext;

        public SubscriptionsController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
