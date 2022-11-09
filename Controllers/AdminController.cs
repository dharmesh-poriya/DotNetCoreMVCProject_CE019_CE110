using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceManagementSystem.Models;
using StudentAttendanceManagementSystem.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentAttendanceManagementSystem.Controllers
{
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly IBranchRepository _branchRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IStudentSubjectRepository _studentSubjectRepository;
        public AdminController(IBranchRepository branchRepository,
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
        public IActionResult Admin()
        {
            string userType = HttpContext.Session.GetString("UserType");
            if ("Admin" != userType)
                return RedirectToAction("Login", "Home");

            SetMessages();

            List<Branch> branches = _branchRepository.GetAllBranch().ToList();
            ViewData["branchList"] = branches;
            return View();
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult AddNewBranch(Branch newBranch)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("success", newBranch.BranchName + " branch Added successfully!!");
                _branchRepository.Add(newBranch);
                return RedirectToAction("Admin", "Admin");
            }
            HttpContext.Session.SetString("error", "Please fill all details carefully");
            return RedirectToAction("Admin", "Admin");
        }

        [Route("[action]/{id}")]
        public IActionResult BranchDetails(int Id)
        {
            Branch currentBranch = _branchRepository.GetById(Id);
            if (null == currentBranch)
            {
                HttpContext.Session.SetString("error", "Branch Not Exist");
                return RedirectToAction("Admin", "Admin");
            }
            SetMessages();
            List<Faculty> faculties = _facultyRepository.GetAll(Id).ToList();
            List<Subject> subjects = _subjectRepository.GetAllByBranchId(Id).ToList();

            ViewData["Branch"] = currentBranch;
            ViewData["Subjects"] = subjects;
            ViewData["Faculties"] = faculties;
            return View();
        }

        [Route("[action]")]
        public IActionResult AddNewSubject(Subject newSubject)
        {
            Subject isExist = _subjectRepository.GetBySubjectCode(newSubject.SubjectCode);
            if (null != isExist)
            {
                HttpContext.Session.SetString("error", newSubject.SubjectCode + " Subject code is already used!!");
            }
            else
            {
                Subject subject = _subjectRepository.Add(newSubject);
                List<Student> allStudents = _studentRepository.GetByBranchId(newSubject.BranchId).ToList();
                Subject newsub = _subjectRepository.GetBySubjectCode(newSubject.SubjectCode);
                if (null != newsub && null != allStudents)
                {
                    for (int i = 0; i < allStudents.Count; i++)
                    {
                        StudentSubject stusub = new StudentSubject();
                        stusub.Attendance = 0;
                        stusub.SemesterNo = newSubject.SemesterNo;
                        stusub.StudentId = allStudents[i].Id;
                        stusub.SubjectId = newsub.Id;
                        _studentSubjectRepository.Add(stusub);
                    }
                }
                HttpContext.Session.SetString("success", newSubject.SubjectName + " Subject Added successfully!!");
            }
            return RedirectToAction("BranchDetails", "Admin", new { id = newSubject.BranchId });
        }

        [Route("[action]/{id}")]
        public IActionResult AddNewStudent(int Id)
        {
            SetMessages();
            Branch currentBranch = _branchRepository.GetById(Id);
            if (null == currentBranch)
            {
                HttpContext.Session.SetString("error", "Branch Not Exist!!");
                return RedirectToAction("BranchDetails", "Admin", new { id = Id });
            }
            ViewData["Branch"] = currentBranch;
            return View();
        }


        [HttpPost]
        [Route("[action]/{id}")]
        public IActionResult AddStudent(Student newStudent)
        {
            newStudent.ActualDOB = newStudent.DOB.ToShortDateString();
            newStudent.Password = newStudent.ActualDOB.ToString();
            if (ModelState.IsValid)
            {
                Login isExist = _loginRepository.GetByUserName(newStudent.UniversityId);
                if (null != isExist)
                {
                    HttpContext.Session.SetString("error", "Student already Exist!!");
                }
                else
                {
                    Login newStudentLogin = new Login();
                    newStudentLogin.Username = newStudent.UniversityId;
                    newStudentLogin.Password = newStudent.Password;
                    Login addNewLogin = _loginRepository.Add(newStudentLogin);
                    Login newLogin = _loginRepository.GetLogin(newStudent.UniversityId, newStudent.Password);
                    if (null == newLogin)
                    {
                        HttpContext.Session.SetString("error", "Something Went Wrong!!");
                    }
                    else
                    {
                        newStudent.LoginId = newLogin.Id;
                        Student newAddedStudent = _studentRepository.Add(newStudent);
                        Student newstu = _studentRepository.GetByLoginId(newStudent.LoginId);
                        Branch stubranch = _branchRepository.GetById(newStudent.BranchId);
                        if (null != newstu && null != stubranch)
                        {
                            for (int i = 1; i <= stubranch.TotalSemester; i++)
                            {
                                List<Subject> subjects = _subjectRepository.GetAllBySemesterBranchId(i, newStudent.BranchId).ToList();
                                for (int j = 0; j < subjects.Count; j++)
                                {
                                    StudentSubject stusub = new StudentSubject();
                                    stusub.Attendance = 0;
                                    stusub.SemesterNo = i;
                                    stusub.StudentId = newstu.Id;
                                    stusub.SubjectId = subjects[j].Id;
                                    _studentSubjectRepository.Add(stusub);
                                }
                            }
                            HttpContext.Session.SetString("success", newStudent.UniversityId + " added successfully!!");
                        }
                        else
                        {
                            // remove login details
                            HttpContext.Session.SetString("error", "Something went wrong (SERVER ERROR).");
                        }
                    }
                }
            }
            else
            {
                HttpContext.Session.SetString("error", "Fill all details carefully!!");
            }
            return RedirectToAction("AddNewStudent", "Admin", new { id = newStudent.BranchId });

        }


        // add faculty
        [Route("[action]/{id}")]
        public IActionResult AddNewFaculty(int Id)
        {
            SetMessages();
            Branch currentBranch = _branchRepository.GetById(Id);
            if (null == currentBranch)
            {
                HttpContext.Session.SetString("error", "Branch Not Exist!!");
                return RedirectToAction("BranchDetails", "Admin", new { id = Id });
            }
            List<Subject> subjects = _subjectRepository.GetAllByBranchId(Id).ToList<Subject>();
            ViewData["Branch"] = currentBranch;
            ViewData["Subjects"] = subjects;
            return View();
        }


        [HttpPost]
        [Route("[action]/{id}")]
        public IActionResult AddFaculty(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                if (-1 == faculty.SubjectId)
                {
                    HttpContext.Session.SetString("error", "Please select a subject!!");
                }
                else
                {
                    Faculty fsub = _facultyRepository.GetBySubjectId(faculty.SubjectId);
                    if (null != fsub)
                    {
                        HttpContext.Session.SetString("error", "Selected subject already assigned!!");
                    }
                    else
                    {
                        Faculty fem = _facultyRepository.GetByEmail(faculty.Email);
                        if (null != fem)
                        {
                            HttpContext.Session.SetString("error", "Entered Email already used!!");
                        }
                        else
                        {
                            faculty.Password = faculty.FirstName + "@DRU";
                            Login nLogin = new Login();
                            nLogin.Username = faculty.Email;
                            nLogin.Password = faculty.Password;
                            nLogin = _loginRepository.Add(nLogin);
                            if (null == nLogin)
                            {
                                HttpContext.Session.SetString("error", "Something went wrong!!");
                            }
                            else
                            {
                                faculty.LoginId = nLogin.Id;
                                if (null == _facultyRepository.Add(faculty))
                                {
                                    HttpContext.Session.SetString("error", "Something went wrong!!");
                                }
                                else
                                {
                                    HttpContext.Session.SetString("success", faculty.FirstName + " added successfully!!");
                                }
                            }
                        }
                    }
                }
            }
            return RedirectToAction("AddNewFaculty", "Admin", new { id = faculty.BranchId });

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
