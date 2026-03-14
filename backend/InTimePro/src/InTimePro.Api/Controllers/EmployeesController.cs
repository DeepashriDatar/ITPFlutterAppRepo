using InTimePro.Api.Services;
using System.Linq;
using System.Web.Http;

namespace InTimePro.Api.Controllers
{
    [RoutePrefix("api/v1/employees")]
    public class EmployeesController : ApiController
    {
        private readonly IAuthService _authService;

        public EmployeesController()
        {
            _authService = new AuthService();
        }

        public EmployeesController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetEmployees()
        {
            var employees = _authService.GetEmployees()
                .Select(e => new
                {
                    e.Id,
                    e.Email,
                    e.Name,
                    e.Department,
                    e.Role,
                    e.IsActive
                })
                .ToList();

            return Ok(new
            {
                success = true,
                data = new { employees }
            });
        }
    }
}
