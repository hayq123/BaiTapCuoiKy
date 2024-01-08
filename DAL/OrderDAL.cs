﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data;
using System.Data.Common;
using System.Collections;

namespace DAL
{
    public class OrderDAL
    {
        private static SqlConnection conn = DBConfig.Instance.SQLConnect();
        private static OrderDAL instance = new();
        public static OrderDAL Instance
        {
            get { return instance; }
        }
        public List<Order> get()
        {
            List<Order> list = new List<Order>();
            conn.Open();
            using (var cmd = new SqlCommand("GetOrder", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Order order = new Order();
                    order.ID = reader.GetInt32("order_id");
                    order.Customer = new Customer();
                    order.Customer.ID = reader.GetInt32("customer_id");
                    order.Accountant = new Staff();
                    order.Accountant.ID = reader.GetInt32("accoutant_id");
                    order.Seller = new Staff();
                    order.Seller.ID = reader.GetInt32("seller_id");
                    order.Order_date = reader.GetDateTime("order_date");
                    order.StatusInt = reader.GetInt32("status");
                    order.PaymentInt = reader.GetInt32("payment");
                    try
                    {
                        if (reader.GetDateTime("update_status_time") != null)
                            order.UpdateStatusTime = reader.GetDateTime("update_status_time");
                    }
                    catch { }
                    list.Add(order);
                }
            }
            conn.Close();
            return list;
        }
        public bool insert(Order order)
        {
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("InsertOrder", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Customer_id", SqlDbType.Int).Value = order.Customer.ID;
                    cmd.Parameters.Add("@Seller_id", SqlDbType.Int).Value = order.Seller.ID;
                    cmd.Parameters.Add("@Order_date", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                }
                foreach (Laptop item in order.Laptop)
                {
                    insertdetail(item, order);
                }
                conn.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool update(Order order)
        {
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("UpdateOrder", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@order_id", SqlDbType.Int).Value = order.ID;
                    cmd.Parameters.Add("@payment", SqlDbType.Int).Value = order.PaymentInt;
                    cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = order.StatusInt;
                    cmd.Parameters.Add("@update_status_date", SqlDbType.DateTime).Value = order.UpdateStatusTime;
                    cmd.Parameters.Add("@accountant_id", SqlDbType.Int).Value = order.Accountant.ID;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool delete(Order order)
        {
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("DeleteOrder", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Order_id", SqlDbType.Int).Value = order.ID;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool insertdetail(Laptop laptop, Order order)
        {
            try
            {
                using (var cmd = new SqlCommand("InsertOrderDetail", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@order_id", SqlDbType.Int).Value = order.ID;
                    cmd.Parameters.Add("@laptop_id", SqlDbType.Int).Value = laptop.ID;
                    cmd.Parameters.Add("@quantity_bought", SqlDbType.Int).Value = laptop.QuantityBought;
                    cmd.Parameters.Add("@price", SqlDbType.VarChar).Value = laptop.Price;
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Order getdetail(Order order)
        {
            conn.Open();
            using (var cmd = new SqlCommand("GetOrderDetail", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@order_id", SqlDbType.Int).Value = order.ID;
                DbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    order.Laptop = new List<Laptop> { new Laptop() };
                    foreach (Laptop laptop in order.Laptop)
                    {
                        laptop.Price = reader.GetDecimal("price");
                        laptop.QuantityBought = reader.GetInt32("quantity");
                    }
                }
               // float totalprice =
            }
            conn.Close();
            return order;
        }
    }
}