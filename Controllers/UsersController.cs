using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SessionManagement.Models;

namespace SessionManagement.Controllers
{
    public class UsersController : Controller
    {
        List<SelectListItem> getCities()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AspMVC;Integrated Security=True;";
            //Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
            cn.Open();

            SqlCommand citiesCmd = new SqlCommand();
            citiesCmd.Connection = cn;
            citiesCmd.CommandType = System.Data.CommandType.Text;
            citiesCmd.CommandText = "select * from Cities";

            List<SelectListItem> cities = new List<SelectListItem>();
            try
            {
                SqlDataReader dr = citiesCmd.ExecuteReader();
                while (dr.Read())
                {
                   cities.Add( new SelectListItem { Text = (String)dr["Name"], Value = (String)dr["Name"] });
                }
                dr.Close();
                return cities;
            }
            catch(Exception e)
            {   
                ViewBag.Exception = e;
                return null;
            }
            finally
            {
                cn.Close(); 
            }

        }
        // GET: Users
        public ActionResult Home()
        {
            User user;
           
            HttpCookie loginCookie = Request.Cookies["loginCookie"];
            //null if not present

            if(Session["user"] == null && loginCookie != null)
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AspMVC;Integrated Security=True;";
                //Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
                cn.Open();

                SqlCommand login = new SqlCommand();
                login.Connection = cn;
                login.CommandType = System.Data.CommandType.Text;
                login.CommandText = "select * from Users where LoginName = @LoginName and Password = @Password";
                login.Parameters.AddWithValue("@LoginName", loginCookie.Values["LoginName"]);
                login.Parameters.AddWithValue("@Password", loginCookie.Values["Password"]);

                try
                {
                    SqlDataReader dr = login.ExecuteReader();
                    if (dr.Read())
                    {
                        User u = new User();
                        u.LoginName = dr["LoginName"].ToString();
                        u.FullName = dr["FullName"].ToString();
                        u.EmailId = dr["EmailId"].ToString();
                        u.City = dr["City"].ToString();
                        u.Phone = dr["Phone"].ToString();

                        Session["user"] = u;
                        dr.Close();
                        return RedirectToAction("Home");
                    }
                    else
                    {
                        ViewBag.error = "Invalid Login name or Password";
                        dr.Close();
                        return RedirectToAction("Login");
                    }
                }
                catch
                {
                    return View();
                }
                finally
                {
                    cn.Close();
                }

            }
            if ((user = (User)Session["user"]) != null){
                return View(user);

            }
            return RedirectToAction("Login");
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
           // delete a cookie
           HttpCookie loginCookie = new HttpCookie("loginCookie");

            loginCookie.Expires = DateTime.Now.AddDays(-1);

            Response.Cookies.Add(loginCookie);

            return RedirectToAction("Login");
        }

        // GET: Users/Register
        public ActionResult Register()
        {
            if (Session["user"] != null)
                return RedirectToAction("Home");
            ViewBag.Cities = getCities();
            return View();
        }

        // POST: Users/Regiter
        [HttpPost]
        public ActionResult Register(User user)
        {

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AspMVC;Integrated Security=True;";
            //Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
            cn.Open();

            SqlCommand regCmd = new SqlCommand();
            regCmd.Connection = cn;
            regCmd.CommandType = System.Data.CommandType.Text;
            regCmd.CommandText = "Insert into Users values(@LoginName, @Password, @FullName, @EmailId, @City, @Phone)";

            regCmd.Parameters.AddWithValue("@LoginName", user.LoginName);
            regCmd.Parameters.AddWithValue("@Password", user.Password);
            regCmd.Parameters.AddWithValue("@FullName", user.FullName);
            regCmd.Parameters.AddWithValue("@EmailId", user.EmailId);
            regCmd.Parameters.AddWithValue("@City", user.City);
            regCmd.Parameters.AddWithValue("@Phone", user.Phone);

            try
            {
                regCmd.ExecuteNonQuery();

                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
            finally
            {
                cn.Close();
            }
        }


        [HttpGet]
        public ActionResult Login()
        {
            if (Session["user"] != null)
                return RedirectToAction("Home");
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLogin user)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AspMVC;Integrated Security=True;";
            //Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
            cn.Open();

            SqlCommand login = new SqlCommand();
            login.Connection = cn;
            login.CommandType = System.Data.CommandType.Text;
            login.CommandText = "select * from Users where LoginName = @LoginName and Password = @Password";
            login.Parameters.AddWithValue("@LoginName", user.LoginName);
            login.Parameters.AddWithValue("@Password", user.Password);

            try
            {
                SqlDataReader dr = login.ExecuteReader();
                if (dr.Read())
                {
                    if (user.Remember)
                    {
                        HttpCookie loginCookie = new HttpCookie("loginCookie");
                        loginCookie.Expires = DateTime.Now.AddDays(1);

                        loginCookie.Values["LoginName"] = user.LoginName;
                        loginCookie.Values["Password"] = user.Password;

                        Response.Cookies.Add(loginCookie);
                    }
                    User u = new User();
                    u.LoginName = dr["LoginName"].ToString();
                    u.FullName = dr["FullName"].ToString();
                    u.EmailId = dr["EmailId"].ToString();
                    u.City = dr["City"].ToString();
                    u.Phone = dr["Phone"].ToString();

                    Session["user"] = u;
                    dr.Close();
                    return RedirectToAction("Home");
                }
                else
                {
                    ViewBag.error = "Invalid Login name or Password";
                    dr.Close();
                    return View();
                }
            }
            catch
            {
                return View();
            }
            finally
            {
                cn.Close();
            }
        }
        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
