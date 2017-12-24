using System.Net;
using System.Net.Http;
using System.Web.Http;
using HomeCinema.Data.Infrastructure;
using HomeCinema.Data.Repositories;
using HomeCinema.Entities;
using HomeCinema.Services;
using HomeCinema.Services.Abstract;
using HomeCinema.Web.Infrastructure.Core;
using HomeCinema.Web.Models;

namespace HomeCinema.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiControllerBase
    {
        private readonly IMembershipService _membershipService;

        public AccountController(IMembershipService membershipService,
            IEntityBaseRepository<Error> errorsRepository, IUnitOfWork unitOfWork)
            : base(errorsRepository, unitOfWork)
        {
            _membershipService = membershipService;
        }

        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, LoginViewModel user)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var userContext = _membershipService.ValidateUser(user.Username, user.Password);

                    if (userContext.User != null)
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new {success = true});
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new {success = false});
                    }
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new {success = false});

                return response;
            });
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public HttpResponseMessage Register(HttpRequestMessage request, RegistrationViewModel user)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new {success = false});
                }
                else
                {
                    var _user = _membershipService.CreateUser(user.Username, user.Email, user.Password, new[] {1});

                    if (_user != null)
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new {success = true});
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new {success = false});
                    }
                }

                return response;
            });
        }
    }
}