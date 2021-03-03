using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QUDMMSAPI
{
    public class ApiAuthorizeFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor != null)
            {
                bool isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType().Equals(typeof(NoPermissionRequiredAttribute)));

                if (isDefined) return;
            }

            string Userid = context.HttpContext.Request.Headers["Userid"].FirstOrDefault();

            string Token = context.HttpContext.Request.Headers["Token"].FirstOrDefault();

            //只要接口前面加了[ApiController],代表每次请求都会执行这里的代码，这里代码一般用来验证令牌合法性（通过查询数据库）

            // if(验证不通过)
            //{
            // context.Result = new UnauthorizedObjectResult(new
            // {
            //    Code = StatusCodes.Status401Unauthorized,
            //    Errors = "拒绝访问，登录令牌参数有误。"
            // }); 

            //}

            //如果验证通过就不用理会

            if (Userid != "123" && Token != "123")
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Code = StatusCodes.Status401Unauthorized,
                    Errors = "拒绝访问，登录令牌参数有误。"
                });

            }

            await base.OnActionExecutionAsync(context, next);

            return;
        }

        public class NoPermissionRequiredAttribute : ActionFilterAttribute
        {
            public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                await base.OnActionExecutionAsync(context, next);

            }

        }
    }
}
