using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserJourney.Core.Contracts;
using UserJourney.Repositories.Contracts;
using UserJourney.Repositories.EF;

namespace UserJourney.Site.Controllers
{
    public class HomeController : BaseController
    {
        private IProjectUnitOfWork _unitOfWork { get; set; }
        private IHttpContextAccessor _contextAccessor;
        private IGenericRepository<Users> _userRepository;
        private IClaimsManager _claimsManager;


        #region Constructor
        public HomeController(IProjectUnitOfWork unitOfWork, IHelperService helperService, IEmailService emailService, IHttpContextAccessor contextAccessor, IClaimsManager claimsManager) : base(unitOfWork, helperService)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userRepository = _unitOfWork.GetRepository<Users>();
            _claimsManager = claimsManager;
        }
        #endregion
        public IActionResult Dashboard()
        {
            ViewBag.FullName = _claimsManager.GetCurrentUserName();
            return View();
        }
    }
}
