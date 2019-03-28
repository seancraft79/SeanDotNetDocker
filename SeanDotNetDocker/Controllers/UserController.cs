﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeanDotNetDocker.Models;
using SeanDotNetDocker.Models.User;

namespace SeanDotNetDocker.Controllers
{
    public class UserController : Controller
    {
        //[User][6][1]
        private IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        //[User][6][2]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        //[User][6][3] : 회원 가입 폼
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //[User][6][4] : 회원 가입 처리
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _repository.GetUserByUserIdAsync(model.Email);
                if (user != null)
                {
                    ModelState.AddModelError("", "이미 가입된 사용자입니다.");
                    return View(model);
                }
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "잘못된 가입 시도!!!");
                return View(model);
            }
            else
            {
                //_repository.AddUser(model.UserId, model.Password);
                model.Password = Common.CryptorEngine.EncryptPassword(model.Password);
                await _repository.AddUserAsync(model);
                return RedirectToAction("Index");
            }
        }


        //[User][6][5] : 로그인 폼
        [HttpGet]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }





        //[User][6][6] : 로그인 처리
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            UserViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                //if (_repository.IsCorrectUser(model.UserId, model.Password))
                if (await _repository.IsCorrectUserAsync(model.UserId,
                    Common.CryptorEngine.EncryptPassword(model.Password)))
                {
                    //[!] 인증 부여: 인증된 사용자의 주요 정보(Name, Role, ...)를 기록
                    var claims = new List<Claim>()
                    {
                        // 로그인 아이디 지정
                        new Claim("UserId", model.UserId),

                        // 기본 역할 지정, "Role" 기능에 "Users" 값 부여
                        new Claim(ClaimTypes.Role, "Users") // 추가 정보 기록
                    };

                    var ci = new ClaimsIdentity(claims,
                        Common.CryptorEngine.EncryptPassword(model.Password));



                    //[1] 로그인 처리: Authorize 특성 사용해서 로그인 체크 가능 
                    //[1][1] ASP.NET Core 1.X 사용: 버전업되면서 아래 메서드 사용 중지 
                    //await HttpContext.Authentication.SignInAsync(
                    //    "Cookies", new ClaimsPrincipal(ci));
                    //[1][2] ASP.NET Core 2.X 사용
                    await HttpContext.SignInAsync(
                        "Cookies",
                        new ClaimsPrincipal(ci),
                        new AuthenticationProperties { IsPersistent = true });



                    ////[참고] ASP.NET Core Identity에서 로그인하는 모양
                    //var identity = new ClaimsIdentity("Cookies");
                    //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    //identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                    //await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));



                    // 추가: 세션에 로그인 사용자 정보 저장
                    HttpContext.Session.SetString("Username", model.UserId);


                    return LocalRedirect("/User/Index");
                }
            }

            return View(model);
        }


        //[User][6][7] : 로그아웃 처리
        public async Task<IActionResult> Logout()
        {
            // Startup.cs의 미들웨어에서 지정한 "Cookies" 이름 사용
            // ASP.NET Core 1.X
            //await HttpContext.Authentication.SignOutAsync("Cookies");
            // ASP.NET Core 2.X
            await HttpContext.SignOutAsync("Cookies");

            return Redirect("/User/Index");
        }


        //[User][6][8] : 회원 정보 보기 및 수정
        [Authorize]
        public IActionResult UserInfor()
        {
            return View();
        }

        //[User][6][9] : 인사말 페이지
        public IActionResult Greetings()
        {
            //[Authorize] 특성의 또 다른 표현 방법
            // 인증되지 않은 사용자는
            if (User.Identity.IsAuthenticated == false)
            {
                // 로그인 페이지로 리디렉션
                return new ChallengeResult();
            }

            return View();
        }

        //[User][6][10] : 접근 거부 페이지
        public IActionResult Forbidden()
        {
            return View();
        }

        //[!] 추가: 사용자 상세 보기(GetUsers 저장 프로시저 사용)
        [Authorize]
        public async Task<IActionResult> UserDetail()
        {
            string userId = User.FindFirst("UserId").Value;
            var user = await _repository.GetUserByUserIdAsync(userId);

            return View(user);
        }

        /// <summary>
        /// 아이디 중복 확인 Web API 테스트
        /// </summary>
        public IActionResult CheckUsername()
        {
            return View();
        }
    }
}