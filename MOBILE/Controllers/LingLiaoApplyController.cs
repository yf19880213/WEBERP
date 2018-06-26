using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOBILE.Controllers
{
    public class LingLiaoApplyController : Controller
    {
        ERPMSEntities1 efdb = new ERPMSEntities1();
        // GET: LingLiaoApply
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Machine()
        {
            try
            {
                var list = (from p in efdb.T_EMMS_生产设备编码表 select new { name = p.设备名称 }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult LingLiaoDetail()
        {
            return View();

        }

        public ActionResult GetDepartment()
        {

            //var list = (from p in efdb.Table_岗位编码表 select new { name = p.岗位名称 }).ToList();
            var list = (from p in efdb.Table_组织编码表 select new { name = p.名称 }).ToList();


            return Json(list, JsonRequestBehavior.AllowGet);
        }


    }
}