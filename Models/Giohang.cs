using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bookstore.Models
{
    public class Giohang
    {
        // khoi tao doi tuong db lay du lieu
        DataClasses1DataContext db = new DataClasses1DataContext();
        public int iMasach { set; get; }
        public string sTensach { set; get; }
        public string sAnhbia { set; get; }
        public double dDongia { set; get; }
        public int iSoluong { set; get; }
        public double dThanhtien
        
        {
            get { return iSoluong * dDongia; }
        }

        // khoi tao doi tuong theo ma sach dc truyen vao
        public Giohang(int Masach)
        {
            iMasach = Masach;
            SACH sach = db.SACHes.Single(n => n.Masach == iMasach);
            sTensach = sach.Tensach;
            sAnhbia = sach.Anhbia;
            dDongia = double.Parse(sach.Giaban.ToString());
            iSoluong = 1;
        }
    }
}