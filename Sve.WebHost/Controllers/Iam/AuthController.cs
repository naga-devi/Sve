namespace JxNet.Extensions.WebHost.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    //using JxNet.Extensions.ApiBase.Jwt;
    //using JxNet.Extensions.ApiBase.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sve.Contract.Interface.Iam;
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase.Jwt.Models;
    using JxNet.Extensions.ApiBase;
    using JxNet.Extensions.ApiBase.Jwt.Handlers;
    using JxNet.Extensions.ApiBase.Models;

    [Route("api/iam")]
    public class IamAuthController : AuthenticationController
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly AppSettings _appSettings;
        private readonly IUsersService _usersService;
        private readonly JwtTokenConfig _jwtTokenConfig;

        public IamAuthController(
            ILogger<AuthenticationController> logger,
            IJwtAuthManager jwtAuthManager,
            //IOptions<AppSettings> appSettings,
            AppSettings appSettings,
            IUsersService usersService,
            JwtTokenConfig jwtTokenConfig
            ) : base(logger, (ApiBase.Jwt.Handlers.IJwtAuthManager)jwtAuthManager)
        {
            _appSettings = appSettings;
            _logger = logger;
            _jwtAuthManager = jwtAuthManager;
            _usersService = usersService;
            _jwtTokenConfig = jwtTokenConfig;
        }

        public override async Task<ActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if (request.UserName.IsNullOrEmpty())
            {
                return Ok(new ResponseResult(status: Status.Error, message: "Invalid username"));
            }

            var user = await _usersService.AuthenticateAsync(request.UserName, request.Password.Encrypt(_appSettings.EncryptionKey));

            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name,request.UserName),
                    new Claim(ApiBase.Constants.BaseClaimsConstants.DisplayName, user.FullName ?? request.UserName),
                    //new Claim(JxNet.Extensions.ApiBase.Constants.BaseClaimsConstants.UserName, user.UserName),
                    new Claim(ApiBase.Constants.BaseClaimsConstants.UserId, user.UserId.ToString()),
                    new Claim(ClaimConstants.UserTypeId, user.UserTypeId.ToString()),
                    new Claim(ClaimConstants.Role, user.UserTypeId.ToString())
                };

                var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
                _logger.LogInformation($"User [{request.UserName}] logged in the system.");

                //_cache.Set("Token-" + serverResponse.UserId, serverResponse.Token);
                //request.Token = _cache.Get("Token-" + _memoryCache.Get<string>("UserId"));

                return Ok(new
                {
                    jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString,
                    user.UserName,
                    DisplayName = user.FullName,
                    status= "success"
                });
            }
            else
            {
                return Ok(new ResponseResult(status: Status.Error, message: "Incorrect username or password"));
            }
        }

        ///// <summary>
        ///// For testing other approach
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[NonAction]
        //private async Task<IActionResult> Authenticate([FromBody] LoginRequest request, CancellationToken cancellationToken)
        //{
        //    if (!request.UserName.IsUserNameValid())
        //    {
        //        return BadRequest("Invalid username");
        //    }

        //    var password = request.Password.Encrypt(_appSettings.EncryptionKey);

        //    return (await _usersService.LoginAsync(request.UserName, password, cancellationToken)).ResponseHandler();
        //}

        [HttpGet("me")]
        [Authorize]
        public ActionResult GetCurrentUser()
        {
            return Ok(new LoginResult
            {
                UserName = User.Identity.Name,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                DisplayName = User.FindFirst("OriginalUserName")?.Value
            });
        }

        [HttpGet("heart-beat")]
        [Authorize]
        public ActionResult GetHeartBeat()
        {
            return Ok();
        }
    }
}
