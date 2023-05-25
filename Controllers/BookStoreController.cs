using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bookstore.Models;

using PagedList;
using PagedList.Mvc;

namespace bookstore.Controllers
{
    public class BookStoreController : Controller
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        private List<SACH> Laysachmoi(int count) 
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        
        private List<SACH> Laysach()
        {
            return data.SACHes.ToList();
        }
        // GET: BookStore
        public ActionResult Index(int ? page)
        {
            // tao bien danh so san pham tren moi trang
            int pageSize = 5;
            // tao bien so trang 
            int pageNum = (page ?? 1);

            var sachmoi = Laysachmoi(4);
            return View(sachmoi.ToPagedList(pageNum,pageSize));           
        }
        // lay tat ca sach
        public ActionResult tatcasach()
        {           
            var laysach = Laysach();
            return View(laysach);

        }
        // CHUDE
        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        // NXB
        public ActionResult Nhaxuatban()
        {
            var nxb = from xb in data.NHAXUATBANs select xb;
            return PartialView(nxb);
        }
        // CHUDE
        public ActionResult SPTheochude(int ?id=1)
        {
            var sach = from s in data.SACHes where s.MaCD== id select s;
            return View(sach);
        }
        // NHAXUATBAN
        public ActionResult SPTheoNXB(int ?id=1)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(sach);
        }
        //DETAIL
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());

        }       

    }
}