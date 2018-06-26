using DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOBILE.Controllers
{
    public class SupplierController : Controller
    {
        ERPMSEntities1 efdb = new ERPMSEntities1();
        // GET: Supplier
        public ActionResult Index()
        {
            return View();
        }
        //跳转到supperDetail 页面
        public ActionResult SupplierDetail(string masterId)
        {
            TempData["masterId"] = masterId;
            return View();
        }

        //得到所有的单号和申请人 
        public ActionResult GetAllDanhao()
        {
            try
            {
                var list = (from p in efdb.Purchase
                            select new
                            {
                                name = p.SaleMane,
                                danhao = p.MasterId
                            }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        //根据单号得到明细
        public ActionResult GetDetail()
        {
            try
            {
                string masterId = TempData["masterId"].ToString();
                var list = (from p in efdb.PurchaseDetail where p.MasterId == masterId select p).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        //根据名字 得到供应商报价
        public ActionResult GetUnitByNameFirst(string name)
        {
            try
            {
                var res = efdb.Database.SqlQuery<T_PHMS_统计_供应商明细表>("select ID, 供应商编码, 入库单号, 订单号, 制单日期, 材料名称, 规格型号, 计量单位, 数量, 金额, 开票金额, 付款金额,case 数量 when 0 then 金额 else 金额 / 数量 end as Unit from T_PHMS_统计_供应商明细表 where 材料名称 =   @name", new SqlParameter[] { new SqlParameter("@name", name) }).ToList();
                if (res == null) return null;
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
                return null;
                throw;
            }
        }
        //根据名字 得到供应商报价
        public ActionResult GetUnitByNameFirstOther(string name)
        {
            try
            {
                var res = efdb.Database.SqlQuery<T_FCMS_统计_供应商明细表>("select ID, 供应商编码, 入库单号, 订单号, 制单日期, 材料名称, 规格型号, 计量单位, 数量, 金额, 开票金额, 付款金额,case 数量 when 0 then 金额 else 金额 / 数量 end as Unit from T_FCMS_统计_供应商明细表 where 材料名称 =   @name", new SqlParameter[] { new SqlParameter("@name", name) }).ToList();
                if (res == null) return null;
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
                return null;
                throw;
            }
        }

        //根据名字 得到供应商报价
        public ActionResult GetUnitByName(string name)
        {
            try
            {
                var res = efdb.Database.SqlQuery<T_PHMS_统计_供应商明细表>("select ID, 供应商编码, 入库单号, 订单号, 制单日期, 材料名称, 规格型号, 计量单位, 数量, 金额, 开票金额, 付款金额,case 数量 when 0 then 金额 else 金额 / 数量 end as Unit from T_PHMS_统计_供应商明细表 where 材料名称 like   @name", new SqlParameter[] { new SqlParameter("@name",  "%" + name + "%") }).ToList();
                if (res == null) return null;
                 return Json(res, JsonRequestBehavior.AllowGet);            
                
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
                return null;
                throw;
            }
        }
        public ActionResult GetUnitByNameOther(string name)
        {
            try
            {
                var query = efdb.Database.SqlQuery<T_FCMS_统计_供应商明细表>("select ID, 供应商编码, 入库单号, 订单号, 制单日期, 材料名称, 规格型号, 计量单位, 数量, 金额, 开票金额, 付款金额 ,case 数量 when 0 then 金额 else 金额 / 数量 end as Unit from T_FCMS_统计_供应商明细表 where 材料名称 like  @name", new SqlParameter[] { new SqlParameter("@name", "%" + name + "%") }).ToList();
                if (query == null) return null;
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
                return null;
                throw;
            }
        }


    }
}