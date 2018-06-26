using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
namespace MOBILE.Controllers
{
    public class ExamineController : Controller
    {
        ERPMSEntities1 efdb = new ERPMSEntities1();
        // GET: Examine
        public ActionResult Index()
        {
            return View("Examinelist");
        }

        public ActionResult GetLoadingDetail()
        {
            var name = User.Identity.Name;
            try
            {
                var count = from p in efdb.Purchase where p.Auditoring == 0 select new { masterId=p.MasterId,saleMan=p.SaleMane};
                return Json(count.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
            

        }
        public ActionResult ExaminRecord(string masterId)
        {
            Session["masterId"] = masterId;
            return View();
        }
        public ActionResult ExaminRecordDetail(string masterId)
        {
            try
            {                  
                    var list = (from p in efdb.PurchaseDetail where p.MasterId == masterId select new {  cailiaoName = p.CailiaoName, num = p.Num }).ToList();
                    return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                 return null;
                throw;
            }
           
        }

        public ActionResult ExaminRecordShow(string masterId)
        {
            try
            {
                             var query = (from p in efdb.Purchase
                                 where p.MasterId == masterId
                                 select new
                                 {
                                     deparment = p.Department,
                                     calDate = p.CaigouDate,
                                     applyCommen = p.ApplyCommen,
                                     respons=p.Responsibity

                                 }).SingleOrDefault();
                    return Json(query, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return null;
                throw;
            }
            
        }

        //
        public ActionResult GetAllUnExamine()
        {
            try
            {
                var list = (from p in efdb.Purchase
                            where p.Auditoring == 0
                            select new
                            {
                                name = p.SaleMane,
                                caigouLate = p.CaigouDate,
                                masterId = p.MasterId,
                                department = p.Department,
                                response = p.Responsibity
                            }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
           
        }
        //通过审核
        public ActionResult PassById(string Id,string text)
        {
            if (text.Trim().Length == 0) return null;
            var query  = efdb.Purchase.SingleOrDefault<Purchase>(s => s.MasterId == Id);
            query.Auditoring = 1;
            query.ExaminCommen = text;
            query.Exmaine = Session["name"].ToString();
            try
            {
                
                efdb.SaveChanges();
                return Content("success");
            }
            catch (Exception)
            {
                return Content("fail");
                throw;
            }
           
        }
        // 不通过审核
        public ActionResult UnPassById(string id,string text)
        {
            if (id.Trim().Length == 0) return null;            
            var query = efdb.Purchase.SingleOrDefault<Purchase>(s => s.MasterId == id);
            query.Auditoring = -1;
            query.Exmaine = User.Identity.Name;
            if (text.Length!=0&&text!="")
            {
                query.ExaminCommen = text;
            }           
            try
            {
                efdb.SaveChanges();
                return Content("success");
            }
            catch (Exception)
            {
                return Content("fail");
                throw;
            }
        }


        //已审核
        public ActionResult Conclusion()
        {
            try
            {
                string name = User.Identity.Name;
                var query = (from p in efdb.Purchase
                             where p.Auditoring != 0&&p.Exmaine==name
                             select new
                             {
                                 auditoring = p.Auditoring,
                                 masterId = p.MasterId
                             }
                        ).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
           
        }

        public ActionResult GetDetailById(string Id)
        {
            if (Id.Trim() == "") return null;
            try
            {
                var query = (from p in efdb.PurchaseDetail where p.MasterId == Id select new {
                    name=p.CailiaoName,
                    num=p.Num
                }).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public ActionResult Examine()
        {
            return View();
        }
        public ActionResult ShengHe()
        {
            try
            {
                var query = (from p in efdb.Purchase
                             where p.SaleMane == User.Identity.Name && p.Auditoring != 0
                             select new
                             {
                                 master=p.MasterId,
                                 authoring = p.Auditoring,
                                 examinePerson = p.Exmaine,
                                 examingCommen=p.ExaminCommen
                             }).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
                throw;
               
            }
        }
        public ActionResult ExamineDetail(string masterId)
        {
            TempData["masterId"] = masterId;
            return View();
        }
        public  ActionResult GetExaminDetail()
        {
            try
            {
                string Id = TempData["masterId"].ToString();
                var query = (from p in efdb.PurchaseDetail
                             where p.MasterId == Id
                             select new
                             {
                                 name = p.CailiaoName,
                                 num = p.Num
                             }).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }
}