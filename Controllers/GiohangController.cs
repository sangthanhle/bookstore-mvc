using bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bookstore.Controllers
{
    public class GiohangController : Controller
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        // lay gio hang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                // kiem tra ton tai gio hang va khoi tao 
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        // them gio hang
        public ActionResult Themgiohang(int iMasach, String strURL)
        {
            // lay secsion gio hang 
            List<Giohang> lstGiohang = Laygiohang();
            // kiemtra san pham da tontai hay chwa
            Giohang sanpham = lstGiohang.Find(n => n.iMasach == iMasach);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMasach);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }

        }
        // tinh tong so luong
        private int Tongsoluong()
        {
            int iTongsoluong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongsoluong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongsoluong;
        }
        // tinh tong tien
        private double Tongtien()
        {
            double iTongtien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongtien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongtien;
        }
        //hiện thị Giỏ hàng
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Bookstore");
            }
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = Tongtien();
            return View(lstGiohang);
        }
        // tao partail view  hien thong tin gio hàng
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = Tongtien();
            return PartialView();
        }
        // XOA GIO HANG
        public ActionResult Xoagiohang( int iMaSP)
        {
            // lay gio hang tu SS
            List<Giohang> lstGiohang = Laygiohang();
            // kiem tra san pham da co trong gio hang
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            // neu ton tai thi cho xua so luong
            if(sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMasach == iMaSP);
                return RedirectToAction("Giohang");
            }
            if(lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Bookstore");
            }
            return RedirectToAction("Giohang");
        }
        // cap nhat gio hang
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            // lay gio hang tu SS
            List<Giohang> lstGiohang = Laygiohang();
            // kiem tra san pham da co trong gio hang
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            // neu ton tai thi cho xua so luong
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        // hien thi view dat hang  de cap nhat thong tin vao don hang
        [HttpGet]
        public ActionResult DatHang()
        {
            // kiem tra dang nhap
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"]== null)
            {
                return RedirectToAction("Index", "Bookstore");
            }
            // lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = Tongtien();
            return View(lstGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            // them don hang
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var Ngaygiao = string.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(Ngaygiao);
            ddh.Tinhtranggiaohang = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            // them chi tiet don hang
           foreach (var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.Masach = item.iMasach;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }

    }
}