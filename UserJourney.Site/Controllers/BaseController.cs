using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserJourney.Core.Contracts;
using UserJourney.Repositories.Contracts;

namespace UserJourney.Site.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        private IProjectUnitOfWork _unitOfWork { get; set; }
        private readonly IHelperService _helperService;

        #region Constructor
        public BaseController(IProjectUnitOfWork unitOfWork, IHelperService helperService)
        {
            _unitOfWork = unitOfWork;
            _helperService = helperService;
        }
        #endregion    
    }
}
