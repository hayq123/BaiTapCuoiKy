﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DAL;
using DTO;
using Microsoft.VisualBasic.ApplicationServices;

namespace GUI
{
    public partial class SellGUI_menu : Form
    {

        private Staff user = new();



        public SellGUI_menu(Staff user)
        {
            this.user = user;
            InitializeComponent();
        }
        private void SellGUI_menu_Load(object sender, EventArgs e)
        {
            tab1loading();
        }
        #region tab0
        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        #endregion
        #region tab1
        private Customer customer = new Customer();
        #region loadingEvent
        private void tab1loading()
        {
            dgvKhachHang.Enabled = true;
            txbCustomerAddress.Enabled = false;
            txbCustomerName.Enabled = false;
            txbCustomerPhone.Enabled = false;
            txbCustomerID.Enabled = false;
            btnCustomerThem.Enabled = true;
            btnCustomerGhi.Enabled = false;
            btnCustomerHuy.Enabled = false;
            btnCustomerSua.Enabled = true;
            btnCustomerXoa.Enabled = true;
            tabControl1.SelectedIndex = 1;
            dgvKhachHang.Rows.Clear();
            foreach (Customer item in CustomerBUS.Instance.get())
            {
                dgvKhachHang.Rows.Add(item.ID, item.Name, item.Phone, item.Address);
            }
            DataGridViewRow row = dgvKhachHang.Rows[0];
            if (Convert.ToString(row.Cells["colID"].Value) != "")
            {
                txbCustomerID.Text = Convert.ToString(row.Cells["colID"].Value);
                txbCustomerName.Text = Convert.ToString(row.Cells["colName"].Value);
                txbCustomerPhone.Text = Convert.ToString(row.Cells["colPhone"].Value);
                txbCustomerAddress.Text = Convert.ToString(row.Cells["colAddress"].Value);
            }
        }
        #endregion
        #region clickEvent
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            row = dgvKhachHang.Rows[e.RowIndex];
            if (Convert.ToString(row.Cells["colID"]) != null)
            {
                txbCustomerID.Text = Convert.ToString(row.Cells["colID"].Value);
                txbCustomerName.Text = Convert.ToString(row.Cells["colName"].Value);
                txbCustomerPhone.Text = Convert.ToString(row.Cells["colPhone"].Value);
                txbCustomerAddress.Text = Convert.ToString(row.Cells["colAddress"].Value);
            }

        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            txbCustomerAddress.Enabled = true;
            txbCustomerName.Enabled = true;
            txbCustomerPhone.Enabled = true;
            btnCustomerGhi.Enabled = true;
            btnCustomerHuy.Enabled = true;
            btnCustomerSua.Enabled = false;
            btnCustomerXoa.Enabled = false;
            txbCustomerID.Text = "";
            txbCustomerAddress.Text = "";
            txbCustomerName.Text = "";
            txbCustomerPhone.Text = "";
            dgvKhachHang.Enabled = false;
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            txbCustomerAddress.Enabled = true;
            txbCustomerName.Enabled = true;
            txbCustomerPhone.Enabled = true;
            btnCustomerGhi.Enabled = true;
            btnCustomerHuy.Enabled = true;
            btnCustomerSua.Enabled = true;
            btnCustomerXoa.Enabled = false;
            dgvKhachHang.Enabled = false;
            btnCustomerThem.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận xóa", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (CustomerBUS.Instance.delete(customer))
                {
                    tab1loading();
                    MessageBox.Show("Đã xóa thành công!");
                }
                else
                {
                    MessageBox.Show("Xóa không thành công!");
                }
            }
            else
            {
                MessageBox.Show("Xóa không thành công!");
            }
        }
        private void btnGhi_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận ghi", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (btnCustomerThem.Enabled)
                {
                    if (txbCustomerName.Text != "" && txbCustomerPhone.Text != "" && txbCustomerAddress.Text != "")
                    {
                        if (CustomerBUS.Instance.CheckPhone(customer) == true)
                        {
                            MessageBox.Show("Số điện thoại đã tồn tại");
                        }
                        else if (customer.Phone.Length != 10)
                        {
                            MessageBox.Show("Số điện thoại phải đúng đủ 10 số!");
                        }
                        else
                        {
                            if (CustomerBUS.Instance.insert(customer))
                            {
                                tab1loading();
                                MessageBox.Show("Đã ghi thành công!");
                            }
                            else
                            {
                                MessageBox.Show("Ghi không thành công!");
                            }
                        }
                    }
                }
                else
                {
                    if (CustomerBUS.Instance.update(customer))
                    {
                        MessageBox.Show("Đã ghi thành công!");
                        tab1loading();
                    }
                    else
                    {
                        MessageBox.Show("Ghi không thành công!");
                    }

                }
            }
            else
            {
                MessageBox.Show("Ghi không thành công!");
            }

        }
        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận hủy", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                tab1loading();
            }
        }
        #endregion
        #region textChangeEvent
        private void txbID_TextChanged(object sender, EventArgs e)
        {
            if (txbCustomerID.Text.Length > 0)
            {
                customer.ID = Convert.ToInt32(txbCustomerID.Text);
            }
        }

        private void txbName_TextChanged(object sender, EventArgs e)
        {
            customer.Name = txbCustomerName.Text;
        }

        private void txbPhone_TextChanged(object sender, EventArgs e)
        {

            customer.Phone = txbCustomerPhone.Text;
        }

        private void txbAddress_TextChanged(object sender, EventArgs e)
        {
            customer.Address = txbCustomerAddress.Text;
        }
        #endregion
        #endregion
        #region tab2
        private List<Laptop> laptops = new List<Laptop>();
        private List<Customer> customers = new List<Customer>();
        private Order order = new Order();
        private Laptop choosenlaptop = new Laptop();
        #region loadingEvent
        void tab2Loading1()
        {
            tabControl1.SelectedIndex = 2;
            cboOrderKhachHang.Enabled = true;
            cboOrderLapTop.Enabled = false;
            txbSoLuongOrder.Enabled = false;
            btnThemOrder.Enabled = false;
            btnSuaOrder.Enabled = false;
            btnTaoOrder.Enabled = true;
            btnGhiOrder.Enabled = false;
            txbSoLuong = new();
            cboOrderLapTop = new();
            cboOrderKhachHang.Items.Clear();
            cboOrderLapTop.Items.Clear();
            dgvOrder.Rows.Clear();
            order.Seller = user;
            customers = CustomerBUS.Instance.get();
            foreach (Customer item in customers)
            {
                cboOrderKhachHang.Items.Add(item.toString);
            }
            laptops.Clear();
            laptops = LaptopBUS.Instance.get();
            cboOrderLapTop.Items.Clear();
            foreach (Laptop item in laptops)
            {
                cboOrderLapTop.Items.Add(item.Name);
            }
        }
        void tab2loading2()
        {
            cboOrderKhachHang.Enabled = false;
            cboOrderLapTop.Enabled = false;
            txbSoLuongOrder.Enabled = false;
            btnThemOrder.Enabled = true;
            btnSuaOrder.Enabled = true;
            btnOrderXoa.Enabled = true;
            btnTaoOrder.Enabled = false;
            btnGhiOrder.Enabled = false;
            decimal sum = 0;
            foreach (DataGridViewRow row in dgvOrder.Rows)
            {
                sum += Convert.ToDecimal(row.Cells["Col5"].Value);
            }
            SUM.Text = Convert.ToString(sum);
        }
        #endregion
        #region clickEvent
        private void btnOrder_Click(object sender, EventArgs e)
        {
            tab2Loading1();
        }
        private void button13_Click(object sender, EventArgs e)
        {

            if (cboOrderKhachHang.Text != "")
            {
                tab2loading2();
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn khách hàng!");
            }
        }
        private void btnThemOrder_Click(object sender, EventArgs e)
        {

            txbSoLuong.Text = "";
            cboOrderLapTop.Text = "";
            btnSuaOrder.Enabled = false;
            btnOrderXoa.Enabled = false;
            cboOrderLapTop.Enabled = true;
            txbSoLuongOrder.Enabled = true;
            btnGhiOrder.Enabled = true;


        }
        private void btnSuaOrder_Click(object sender, EventArgs e)
        {
            btnSuaOrder.Enabled = true;
            btnThemOrder.Enabled = false;
            btnOrderXoa.Enabled = false;
            cboOrderLapTop.Enabled = true;
            txbSoLuongOrder.Enabled = true;
            btnGhiOrder.Enabled = true;
        }
        private void btnOrderXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận xóa", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dgvOrder.Rows)
                {
                    if (Convert.ToString(row.Cells["Col2"].Value) == cboOrderLapTop.Text)
                    {
                        dgvOrder.Rows.Remove(row);
                    }
                    tab2loading2();
                }
            }
        }
        private void btnHoanThanh_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvOrder.Rows)
            {
                Laptop laptop = new();
                laptop.ID = Convert.ToInt32(row.Cells["Col1"].Value);
                laptop.Name = Convert.ToString(row.Cells["Col2"].Value);
                laptop.QuantityImport = Convert.ToInt32(row.Cells["Col3"].Value);
                order.Laptop.Add(laptop);
            }
            if (OrderBUS.Instance.insert(order) == true)
            {
                tab2Loading1();
                MessageBox.Show("Ghi thành công!");
            }
            else
            {
                MessageBox.Show("Ghi không thành công!");
            }
        }
        private void btnGhiOrder_Click(object sender, EventArgs e)
        {
            if (cboOrderLapTop.Text != "" && txbSoLuongOrder.Text != "")
            {
                if (btnCustomerThem.Enabled == true)
                {
                    dgvOrder.Rows.Add(choosenlaptop.ID, choosenlaptop.Name, choosenlaptop.QuantityBought, choosenlaptop.Price, (decimal)choosenlaptop.Price * (decimal)choosenlaptop.QuantityBought);
                }
                else
                {
                    foreach (DataGridViewRow row in dgvOrder.Rows)
                    {
                        if (Convert.ToString(row.Cells["Col2"].Value) == cboOrderLapTop.Text)
                        {
                            dgvOrder.Rows.Remove(row);
                        }
                    }
                    dgvOrder.Rows.Add(choosenlaptop.ID, choosenlaptop.Name, choosenlaptop.QuantityBought, choosenlaptop.Price, (decimal)choosenlaptop.Price * (decimal)choosenlaptop.QuantityBought);
                }
                tab2loading2();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
            }
        }
        private void dgvOrder_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = new DataGridViewRow();
                row = dgvOrder.Rows[e.RowIndex];
                if (Convert.ToString(row.Cells["Col1"].Value) != "")
                {
                    cboOrderLapTop.Text = Convert.ToString(row.Cells["Col2"].Value);
                    txbSoLuongOrder.Text = Convert.ToString(row.Cells["Col4"].Value);
                }
            }
            catch
            {

            }
        }
        #endregion
        #region textChangeEvent
        private void txbSoLuongOrder_TextChanged(object sender, EventArgs e)
        {
            if (txbSoLuongOrder.Text.Length > 0)
            {
                choosenlaptop.QuantityBought = Convert.ToInt32(txbSoLuongOrder.Text);
            }
        }

        private void cboOrderLapTop_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Laptop item in laptops)
            {
                if (item.Name == cboOrderLapTop.Text)
                {
                    choosenlaptop = item;
                    break;
                }
            }
        }
        private void cboOrderKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var item in customers)
            {
                if (item.toString == cboOrderKhachHang.Text)
                {
                    order.Customer = item;
                    break;
                }
            }
        }
        #endregion
        #endregion

        
    }
}