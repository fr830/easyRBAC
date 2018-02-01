﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyRbac.Application.Login;
using EasyRbac.Application.User;
using EasyRbac.Domain.Entity;
using EasyRbac.Dto.Application;
using EasyRbac.Dto.AppLogin;
using EasyRbac.Dto.AppResource;
using EasyRbac.Dto.User;
using EasyRbac.Web.WebExtentions;
using Microsoft.AspNetCore.Mvc;

namespace EasyRbac.Web.Controllers.SsoApi
{
    public class AppLoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;

        public AppLoginController(ILoginService loginService, IUserService userService)
        {
            this._loginService = loginService;
            this._userService = userService;
        }

        [HttpPost("appLogin")]
        public async Task<AppLoginResult> AppLogin([FromBody]AppLoginDto dto)
        {
            var result = await this._loginService.AppLoginAsync(dto);
            return result;
        }

        [HttpGet("app/user/{userToken}")]
        [ResourceTag("AppGetUserInfo")]
        public async Task<UserInfoDto> GetUserInfo(string userToken)
        {
            (var userId, var appId) = await this.GetBaseInfo(userToken);

            return await this._userService.GetUserInfo(userId);
        }

        [HttpGet("app/resource/{userToken}")]
        [ResourceTag("AppGetUserResources")]
        public async Task<List<AppResourceDto>> GetUserResources(string userToken)
        {
            (var userId, var appId) = await this.GetBaseInfo(userToken);
            var result = await this._loginService.GetUserAppResourcesAsync(userId, appId);
            return result;
        }

        private async Task<(long, long)> GetBaseInfo(string userToken)
        {
            LoginTokenEntity userTokenEntity = await this._loginService.GetTokenEntityByTokenAsync(userToken);
            var identity = this.User.Identity as ApplicationIdentity;
           
            return (userTokenEntity.UserId,identity.App.Id);
        }
    }
}
