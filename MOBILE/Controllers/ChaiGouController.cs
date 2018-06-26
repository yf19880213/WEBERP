using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using System.Data.Entity;

namespace MOBILE.Controllers
{
    public class ChaiGouController : Controller
    {
        #region 

        ERPMSEntities1 efdb = new ERPMSEntities1();
       
        // GET: ChaiGou
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetUnAudition()
        {
            //var count = efdb.Purchase.Where(p => p.Auditoring == 0 || p.Auditoring == null).Count();
            //return Content(count.ToString());
            return null;
        }

        /// <summary>
        /// 采购申请
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ChaiGouApply()
        {
            return View();
        }
        public ActionResult ChaiGouDetail()
        {
            return View();
        }
        //确定
        public ActionResult ChaiGouInsert(Purchase data)
        {
            if (data == null) return null;           
            data.CaigouDate = DateTime.Now;
            efdb.Entry<Purchase>(data).State = EntityState.Added;
            try
            {
                efdb.SaveChanges();
                return Content("success");
        
            }
            catch (Exception ex)
            {
                return Content("fail"+ex.Message);
            }
           
        }
        //取消
        public ActionResult ChaiGouCancel()
        {          
            try
            {
                string masterId = Session["masterId"].ToString().Trim();
                Purchase objPur = new Purchase() { MasterId = masterId };
                var list = efdb.Purchase.Where(u => u.MasterId == masterId);
                if (list != null && list.Any())
                {
                    foreach (Purchase item in list)
                    {
                        efdb.Purchase.Remove(item);
                    }
                }
                efdb.SaveChanges();
                Session.Remove("masterId");
                return Content("success");
            }
            catch (Exception)
            {
                return Content("fail");
                throw;
            }
        }
        public ActionResult ChaiGouUnAuDetail(string masterId)
        {
            TempData["_masterId"] = masterId;
            return View();
        }
        public ActionResult ShowDetail()
        {

            if (TempData["_masterId"].ToString()!=null&& TempData["_masterId"].ToString().Trim().Length!=0)
            {
                string masterId = TempData["_masterId"].ToString();
                var list = (from p in efdb.PurchaseDetail where p.MasterId == masterId select new { cailiaoid = p.CailiaoId, cailiaoname = p.CailiaoName, num = p.Num }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        /// <summary>
        /// 采购审核
        /// </summary>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        public ActionResult ChaiGouExamine()
        {
            return View();
        }
#endregion
        #region 前后台交互
        //得到所有类型
        public ActionResult GetAllType()
        {
            try
            {
                var list = from p in efdb.T_MIMS_材料类别编码表 select new { text = p.材料类别名称, Id = p.材料类别编码 };
                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                new Exception(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 根据类别得到一种类型
        /// </summary>
        /// <param name="data">类别</param>
        /// <returns></returns>
        public ActionResult GetStuffByCode(string data)
        {
            try
            {
                var list = (from p in efdb.T_MIMS_材料编码表 where p.材料类别编码 == data select new { Id = p.材料编码, name = p.材料名称 }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
                return null;
            }
        }
        //所有部门
        public ActionResult GetDepartment(string danghao)
        {
            try
            {
                Session["masterId"] = danghao.Trim();
                var list = (from p in efdb.Table_组织编码表 select new { name = p.名称 }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //插入采购数据
        public ActionResult GetRequest(List<PurchaseDetail> data)
        {
            JsonResult jr = null;
            if (data != null && data.Count > 0)
            {
                foreach (var i in data)
                {
                    
                    i.MasterId = Session["masterId"].ToString().Trim();
                    efdb.Entry<PurchaseDetail>(i).State = EntityState.Added;
                }
                try
                {
                    efdb.SaveChanges();
                    Session.Remove("masterId");
                    jr = Json(new { success = 0, msg = "采购成功" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                   {
                    new Exception(ex.Message);
                      jr = Json(new { success = -1, msg = "采购失败",error=ex.Message }, JsonRequestBehavior.AllowGet);
                   }
            }
            else
            {
                    jr = Json(new { success = -1, msg = "没有选择任何数据" }, JsonRequestBehavior.AllowGet);
             }
                return jr;

        }
        #endregion

        #region 审核采购数目
        public ActionResult GetLoadingCount()
        {
            var name = User.Identity.Name;
            var count = efdb.Purchase.Where(p => p.Auditoring == 0&&p.SaleMane== name).Count();
            return Content(count.ToString());

        }
        //审核细节
        public ActionResult GetLoadingDetail()
        {
            var name = User.Identity.Name;
            try
            {
                var count = from p in efdb.Purchase where p.Auditoring == 0 && p.SaleMane == name select p.MasterId;
                return Json(count.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
           

        }
        //已审核 审核人员
        public ActionResult GetFinishCount()
        {
            var name = User.Identity.Name;
            var count = efdb.Purchase.Where(p => p.Auditoring != 0&&p.Exmaine==name).Count();
            return Content(count.ToString());
        }

        //已审核 采购人员
        public ActionResult GetFinishCountPurduce()
        {
            var name = User.Identity.Name;
            var count = efdb.Purchase.Where(p => p.Auditoring != 0 && p.SaleMane == name).Count();
            return Content(count.ToString());
        }
        //审核时查询
        public ActionResult GetLoadingCountEximine()
        {
            var name = User.Identity.Name;
            var count = efdb.Purchase.Where(p => p.Auditoring == 0).Count();
            return Content(count.ToString());

        }

        #endregion

        #region 审核，未审核
        public ActionResult ChaiGouLoading()
        {
            return View();
        }
        //单一结果
        public ActionResult ChaiGouLoadingView()
        {
            
            try
            {
                string name = User.Identity.Name;
                var dingdang = (from p in efdb.Purchase
                                where p.Auditoring == 0&&p.SaleMane==name
                                select new
                                {
                                    name = p.SaleMane,
                                    caigouData = p.CaigouDate,
                                    masterId = p.MasterId
                                }).SingleOrDefault();
                efdb.SaveChanges();
                return Json(dingdang, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
            
        }

        public ActionResult BianMa()
        {
            return View();
        }





        #endregion
    }

}
