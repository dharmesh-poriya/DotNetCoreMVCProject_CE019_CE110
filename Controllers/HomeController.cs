using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using StudentAttendanceManagementSystem.Models;
using StudentAttendanceManagementSystem.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentAttendanceManagementSystem.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IStudentSubjectRepository _studentSubjectRepository;
        private readonly ISubjectRepository _subjectRepository;
        public HomeController(ILoginRepository loginRepository,
            IAdminRepository adminRepository,
            IFacultyRepository facultyRepository,
            IStudentRepository studentRepository,
            IBranchRepository branchRepository,
            IStudentSubjectRepository studentSubjectRepository,
            ISubjectRepository subjectRepository)
        {
            this._loginRepository = loginRepository;
            this._adminRepository = adminRepository;
            this._facultyRepository = facultyRepository;
            this._studentRepository = studentRepository;
            this._branchRepository = branchRepository;
            this._studentSubjectRepository = studentSubjectRepository;
            this._subjectRepository = subjectRepository;
        }

        [Route("[action]")]
        [Route("~/")]
        [Route("")]
        public IActionResult Index()
        {
            SetMessages();
            string userType = HttpContext.Session.GetString("UserType");
            if(null == userType)
                return RedirectToAction("Login");

            if ("Admin" == userType)
                return RedirectToAction("Admin", "Admin");

            if ("Faculty" == userType)
                return RedirectToAction("Faculty", "Faculty");
            int stuid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentId"));
            Student student = _studentRepository.GetById(stuid);
            if(null != student)
            {
                int branchid = student.BranchId;
                Branch branch = _branchRepository.GetById(branchid);
                if(null != branch)
                {
                    ViewData["StudentObj"] = student;
                    ViewData["BranchObj"] = branch;
                    return View();
                }
            }

            HttpContext.Session.SetString("error", "Something went wrong (SERVER ERROR)");
            SetMessages();
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Login()
        {
            SetMessages();
            if(null != HttpContext.Request.Cookies["UserType"])
            {
                string utype = HttpContext.Request.Cookies["UserType"];
                int uid = 0;
                if ("Admin" == utype)
                {
                    uid = Convert.ToInt32(HttpContext.Session.GetInt32("AdminId"));
                    HttpContext.Session.SetInt32("AdminId",uid);
                }
                else if ("Faculty" == utype)
                {
                    uid = Convert.ToInt32(HttpContext.Session.GetInt32("FacultyId"));
                    HttpContext.Session.SetInt32("FacultyId", uid);
                }
                else
                {
                    uid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentId"));
                    HttpContext.Session.SetInt32("StudentId", uid);
                }
                HttpContext.Session.SetString("UserType", utype);
                
            }

            string userType = HttpContext.Session.GetString("UserType");
            if ("Admin" == userType)
                return RedirectToAction("Admin", "Admin");

            if ("Faculty" == userType)
                return RedirectToAction("Faculty", "Faculty");

            if ("Student" == userType)
                return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Login(Login _login)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddMonths(1));
            if (ModelState.IsValid)
            {
                Login login = _loginRepository.GetLogin(_login.Username, _login.Password);
                if(null != login)
                {
                    Admin _admin = _adminRepository.GetAdminByLoginId(login.Id);
                    if(null != _admin)
                    {
                        HttpContext.Session.SetString("UserType", "Admin");
                        HttpContext.Session.SetInt32("AdminId", _admin.Id);
                        HttpContext.Session.SetString("success", _admin.Name + " logged in successfull!!");
                        if (_login.remember)
                        {
                            HttpContext.Response.Cookies.Append("UserType", "Admin", cookieOptions);
                            HttpContext.Response.Cookies.Append("AdminId", _admin.Id.ToString(), cookieOptions);
                        }
                        return RedirectToAction("Admin", "Admin");
                    }
                    Faculty _faculty = _facultyRepository.GetByLoginId(login.Id);
                    if (null != _faculty)
                    {
                        HttpContext.Session.SetString("UserType", "Faculty");
                        HttpContext.Session.SetInt32("FacultyId", _faculty.Id);
                        HttpContext.Session.SetString("success", _faculty.FirstName + " logged in successfull!!");
                        if (_login.remember)
                        {
                            HttpContext.Response.Cookies.Append("UserType", "Faculty", cookieOptions);
                            HttpContext.Response.Cookies.Append("FacultyId", _faculty.Id.ToString(), cookieOptions);
                        }
                        return RedirectToAction("Faculty", "Faculty");
                    }
                    Student _student = _studentRepository.GetByLoginId(login.Id);
                    if (null != _student)
                    {
                        HttpContext.Session.SetString("UserType", "Student");
                        HttpContext.Session.SetInt32("StudentId", _student.Id);
                        HttpContext.Session.SetString("success", _student.UniversityId + " logged in successfull!!");
                        if (_login.remember)
                        {
                            HttpContext.Response.Cookies.Append("UserType", "Student", cookieOptions);
                            HttpContext.Response.Cookies.Append("StudentId", _student.Id.ToString(), cookieOptions);
                        }
                        return RedirectToAction("Index","Home");
                    }
                    HttpContext.Session.SetString("error", "Something went wrong");
                }
                else
                {
                    HttpContext.Session.SetString("error", "Invalid Credential");
                }
            }
            return RedirectToAction("Login","Home");
        }
        [Route("[action]/{branch}/{semester}")]
        public IActionResult Attendance(int semester,int branch)
        {
            int stuid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentId"));
            Branch stuBranch = _branchRepository.GetById(branch);
            if((null != stuBranch) && (1 <= semester) && (semester <= stuBranch.TotalSemester))
            {
                List<StudentSubject> studentSubjects = _studentSubjectRepository.getBySemesterNoStudentId(semester,stuid).ToList();
                for(int i = 0; i < studentSubjects.Count; i++)
                {
                    Subject sub = _subjectRepository.GetById(studentSubjects[i].SubjectId);
                    studentSubjects[i].Subject = sub;
                }
                Student cstu = _studentRepository.GetById(stuid);
                ViewData["Student"] = cstu;
                ViewData["SubjectWiseAttendance"] = studentSubjects;
                return View();
            }
            HttpContext.Session.SetString("error", "ERROR-404 : Page Note Found");
            return RedirectToAction("Index","Home");
        }

        [Route("[action]")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserType");
            HttpContext.Session.Remove("AdminId");
            HttpContext.Session.Remove("FacultyId");
            HttpContext.Session.Remove("StudentId");

            Response.Cookies.Delete("UserType");
            Response.Cookies.Delete("AdminId");
            Response.Cookies.Delete("FacultyId");
            Response.Cookies.Delete("StudentId");
            return RedirectToAction("Login", "Home");
        }

        private void SetMessages()
        {
            ViewData["ErrorMessage"] = HttpContext.Session.GetString("error");
            ViewData["SuccessMessage"] = HttpContext.Session.GetString("success");
            HttpContext.Session.Remove("error");
            HttpContext.Session.Remove("success");
        }
    }
}

// session
//Set value in Session object.
//HttpContext.Session.SetString("Person", "Mudassar");
//Get value from Session object.
//ViewData["Message"] = HttpContext.Session.GetString("Person");

//@RenderPage("~/Views/Shared/SampleView.cshtml")
//return RedirectToAction("actionName", "controllerName", new { area = "Admin" });