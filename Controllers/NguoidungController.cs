using bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bookstore.Controllers
{
    public class NguoidungController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        //DANG KÝ
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        // POST: Hàm DangKy
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            // gán giá trị nhập vào cho các biến
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/YYYY}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được trống";
            } else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = " không được trống";
            } else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = " không được trống";
            } else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = " không được trống";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = " không được trống";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = " không được trống";
            }
            else
            {
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
        }
        //DANG NHAP
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var Tendn = collection["TenDN"];
            var MatKhau = collection["Matkhau"];

            if (String.IsNullOrEmpty(Tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(MatKhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.FirstOrDefault(n => n.Taikhoan == Tendn && n.Matkhau == MatKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng bạn đã đăng nhập thành công!";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "BookStore");
                }
                else
                    ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không chính xác!";        
            }
            return View();

        }


    }
}