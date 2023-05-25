using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bookstore.Models;
using System.IO;


using PagedList;
using PagedList.Mvc;
namespace bookstore.Controllers
{
    public class AdminController : Controller
    {
        //Tao 1 doi tuong chua toan bo CSDL tu dbBookstore
        DataClasses1DataContext data = new DataClasses1DataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sach()
        {
            //int pageNumber = (page ?? 1);
            //int pageSize = 7;
            return View(data.SACHes.ToList() );
            //return View(data.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập Tên tài khoản";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (ad)

                Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    //ViewBag.Thongbap = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tài khoản hoặc mật khẩu không đúng !";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiSach()
        {
            // dua du lieu vao droplist
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "Tenchude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSach(SACH sach, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "Tenchude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file, luu y bo sung thu vien using System.IO
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/img1"), fileName);
                    //Kiem tra hinh anh ton tai chua?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sach.Anhbia = fileName;
                    //Luu vao CSDL
                    data.SACHes.InsertOnSubmit(sach);
                    data.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }

        }
    }
}
