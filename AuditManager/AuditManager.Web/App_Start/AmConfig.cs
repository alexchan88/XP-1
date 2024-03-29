﻿//using Microsoft.Owin;
//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Security.Principal;
//using System.Threading.Tasks;

//namespace AuditManager.Web
//{
//    using AppFunc = Func<IDictionary<string, object>, Task>;

//    public class WindowsPrincipalHandler
//    {
//        private readonly AppFunc _next;

//        public WindowsPrincipalHandler(AppFunc next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(IDictionary<string, object> env)
//        {
//            var context = new OwinContext(env);

//            var windowsPrincipal = context.Request.User as WindowsPrincipal;
//            if (windowsPrincipal != null && windowsPrincipal.Identity.IsAuthenticated)
//            {
//                await _next(env);

//                if (context.Response.StatusCode == 401)
//                {
//                    // We're going no add the identifier claim
//                    var nameClaim = windowsPrincipal.FindFirst(ClaimTypes.Name);

//                    // This is the domain name
//                    string name = nameClaim.Value;

//                    // If the name is something like DOMAIN\username then
//                    // grab the name part
//                    var parts = name.Split(new[] { '\\' }, 2);

//                    string shortName = parts.Length == 1 ? parts[0] : parts[parts.Length - 1];

//                    // REVIEW: Do we want to preserve the other claims?

//                    // Normalize the claims here
//                    var claims = new List<Claim>();
//                    claims.Add(new Claim(ClaimTypes.NameIdentifier, name));
//                    claims.Add(new Claim(ClaimTypes.Name, shortName));
//                    claims.Add(new Claim(ClaimTypes.AuthenticationMethod, "Windows"));
//                    var identity = new ClaimsIdentity(claims, "");//Constants.JabbRAuthType);

//                    context.Authentication.SignIn(identity);

//                    context.Response.Redirect((context.Request.PathBase + context.Request.Path).Value);
//                }

//                return;
//            }

//            await _next(env);
//        }
//    }

//}