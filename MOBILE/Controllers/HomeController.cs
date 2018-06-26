using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Converters;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using DAL;
using System.IO;
using System.Security.Cryptography;
using System.Web.Security;
namespace MOBILE.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ERPMSEntities1 efdb = new ERPMSEntities1();
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        /// 

        public ActionResult ChaiGou()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ChaiGouApply()
        {
            return View();
        }
        #region 跳转
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 采购登录
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        public ActionResult ApplyLogin(string name, string password)
        {
            JsonResult jr = null;
            var pwd = password.Sha1();
            var user = efdb.SYS_用户信息表.Where(p => p.登录名 == name && p.密码 == pwd).FirstOrDefault();
            if (user != null)
            {
                WebHelper.SetFormsAuthentication(user.登录名, user);
                //ViewBag.name = User.Identity.Name;
                Session["name"] = user.登录名;
                Session["date"] = DateTime.Now.ToString("yyyy/MM/dd hhmmssms");

                jr = Json(new { success = 0, msg = "/Home/Index" });
            }
            else
            {
                jr = Json(new { success = -1, msg = "账号或密码错误" });
            }

            return jr;
        }

  
#endregion

    }
}