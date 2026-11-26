using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlycuahangbanxemay
{
    public partial class frmHoaDon : Form
    {
        public frmHoaDon()
        {
            InitializeComponent();
        }

        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            txtDaTra.Text = "0";
            txtDaTra.Text = "0";
            lblNhanVien.Text = "Nhân viên: " + PhienDangNhap.HoTen;   // người đang đăng nhập

            try
            {
                NapKhachHang();
                NapDanhSachXe();
                NapHoaDon("");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được CSDL. Kiểm tra lại chuỗi kết nối trong Db.cs\n\n" + ex.Message);
            }
        }
        private void NapKhachHang()
        {
            DataTable dt = Db.LayBang(
                "SELECT MaKH, HoTen + N' - ' + SDT AS HienThi FROM KhachHang ORDER BY HoTen");

            cboKhachHang.DisplayMember = "HienThi";
            cboKhachHang.ValueMember = "MaKH";
            cboKhachHang.DataSource = dt;
            cboKhachHang.SelectedIndex = -1;   // chưa chọn khách nào
        }

        private void NapDanhSachXe()
        {
            DataTable dt = Db.LayBang(
                "SELECT MaXe, TenXe, GiaBan, SoLuongTon FROM Xe WHERE SoLuongTon > 0 ORDER BY TenXe");

            // Chỉ được Clear khi ComboBox chưa gắn DataSource (lần nạp đầu tiên)
            if (cboXe.DataSource == null)
            {
                cboXe.Items.Clear();
            }

            cboXe.DisplayMember = "TenXe";
            cboXe.ValueMember = "MaXe";
            cboXe.DataSource = dt;
            cboXe.SelectedIndex = -1;
            txtDonGia.Clear();
        }

        private void NapHoaDon(string tuKhoa)
        {
            DataTable dt = Db.LayBang(
                "SELECT h.MaHD, k.HoTen AS TenKhach, h.TongTien, h.DaTra, h.TongTien - h.DaTra AS ConNo, " +
                "h.NgayLap, n.HoTen AS TenNV " +
                "FROM HoaDon h " +
                "JOIN KhachHang k ON h.MaKH = k.MaKH " +
                "JOIN NhanVien n ON h.MaNV = n.MaNV " +
                "WHERE h.MaHD LIKE @kw OR k.HoTen LIKE @kw " +
                "ORDER BY h.NgayLap DESC, h.MaHD DESC",
                Db.Param("@kw", "%" + tuKhoa + "%"));

            dgvHoaDon.Rows.Clear();
            foreach (DataRow r in dt.Rows)
            {
                dgvHoaDon.Rows.Add(
                    r["MaHD"].ToString(),
                    r["TenKhach"].ToString(),
                    r["TongTien"],
                    r["DaTra"],
                    r["ConNo"],
                    Convert.ToDateTime(r["NgayLap"]).ToString("dd/MM/yyyy"),
                    r["TenNV"].ToString());
            }
        }

        private void cboXe_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView dong = cboXe.SelectedItem as DataRowView;
            if (dong == null)
            {
                txtDonGia.Clear();
                return;
            }
            decimal gia = Convert.ToDecimal(dong["GiaBan"]);
            txtDonGia.Text = gia.ToString("N0");
        }

        private void btnThemXe_Click(object sender, EventArgs e)
        {

        }

        private void txtDaTra_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {

        }

        private void btnTim_Click(object sender, EventArgs e)
        {

        }

        private void btnHienTatCa_Click(object sender, EventArgs e)
        {

        }

        private void btnKhachMoi_Click(object sender, EventArgs e)
        {
            frmKhachHang f = new frmKhachHang();
            f.ShowDialog();      // thêm khách xong đóng form lại
            NapKhachHang();      // nạp lại danh sách khách
        }
    }
}
