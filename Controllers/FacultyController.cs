using Microsoft.AspNetCore.Mvc;
using StudentAttendanceManagementSystem.Models.DAO;
using StudentAttendanceManagementSystem.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StudentAttendanceManagementSystem.Controllers
{
    [Route("[controller]")]
    public class FacultyController : Controller
    {
        private readonly IBranchRepository _branchRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IStudentSubjectRepository _studentSubjectRepository;
        public FacultyController(IBranchRepository branchRepository,
            ISubjectRepository subjectRepository,
            ILoginRepository loginRepository,
            IStudentRepository studentRepository,
            IFacultyRepository facultyRepository,
            IStudentSubjectRepository studentSubjectRepository)
        {
            this._branchRepository = branchRepository;
            this._subjectRepository = subjectRepository;
            this._loginRepository = loginRepository;
            this._studentRepository = studentRepository;
            this._facultyRepository = facultyRepository;
            this._studentSubjectRepository = studentSubjectRepository;
        }

        [Route("[action]")]
        public IActionResult Faculty()
        {
            string userType = HttpContext.Session.GetString("UserType");
            if ("Faculty" != userType)
                return RedirectToAction("Login", "Home");

            int fcid = Convert.ToInt32(HttpContext.Session.GetInt32("FacultyId"));
            Faculty faculty = _facultyRepository.GetById(fcid);
            if(null == faculty)
            {
                HttpContext.Session.SetString("error", "Something went wrong (SERVER ERROR).");
            }
            else
            {
                List<StudentSubject> studentSubjects = _studentSubjectRepository.GetBySubjectId(faculty.SubjectId).ToList();
                for(int i = 0; i < studentSubjects.Count; i++)
                {
                    studentSubjects[i].Student = _studentRepository.GetById(studentSubjects[i].StudentId);
                }
                Subject currentSubject = _subjectRepository.GetById(faculty.SubjectId);
                if(null == currentSubject)
                {
                    HttpContext.Session.SetString("error", "Something went wrong (SERVER ERROR).");
                }
                else
                {
                    ViewData["StudentSubjectList"] = studentSubjects;
                    ViewData["Subject"] = currentSubject;
                    ViewData["Faculty"] = faculty;
                }
            }
            SetMessages();

            return View();
        }

        [Route("[action]/{objId}")]
        public IActionResult UpdateAttendance(int objId)
        {
            SetMessages();
            StudentSubject stusub = _studentSubjectRepository.GetById(objId);
            if(null == stusub)
            {
                HttpContext.Session.SetString("error", "ERROR-404 (Page not found)");
                return RedirectToAction("Faculty", "Faculty");
            }
            Student stu = _studentRepository.GetById(stusub.StudentId);
            if(null == stu)
            {
                HttpContext.Session.SetString("error", "ERROR-404 (Page not found)");
                return RedirectToAction("Faculty", "Faculty");
            }
            ViewData["StudentObj"] = stu;
            ViewData["StuSubObj"] = stusub;
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Update(StudentSubject updateSS)
        {
            if (ModelState.IsValid)
            {
                _studentSubjectRepository.Update(updateSS);
                HttpContext.Session.SetString("success", "Attendance updated");
                return RedirectToAction("UpdateAttendance", "Faculty", new { objId = updateSS.Id });
            }
            HttpContext.Session.SetString("error", "ERROR-404 (Page not found)");
            return RedirectToAction("Faculty", "Faculty");
        }

        private int getOldAttendance(int id)
        {
            StudentSubject oldss = _studentSubjectRepository.GetById(id);
            if (null == oldss)
                return -1;
            return oldss.Attendance;
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
