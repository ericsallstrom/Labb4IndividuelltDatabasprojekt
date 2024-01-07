using Labb4IndividuelltDatabasprojekt.Data;
using Labb4IndividuelltDatabasprojekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Labb4IndividuelltDatabasprojekt.Navigation
{
    internal class HandleQueries
    {
        // Properties for handling user input, choices and database context.
        private UserInputHandler InputHandler { get; }
        private KrutångerHighSchoolDbContext Context { get; set; }
        private DataGenerator DataGenerator { get; set; }
        private byte GenderChoice { get; set; }
        private int ClassChoice { get; set; }
        private byte ProfessionChoice { get; set; }
        private int MenuChoice { get; set; }

        // Constructor initializes the class with a database context.
        public HandleQueries(KrutångerHighSchoolDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            InputHandler = new(context);
            DataGenerator = new();
        }

        // Method to display a menu and get the user's choice.
        private void GetMenu(string prompt, string[] menuOptions)
        {
            Menu menu = new()
            {
                Prompt = prompt,
                MenuOptions = menuOptions
            };

            MenuChoice = menu.GetMenuChoice();
        }

        // Method to assign a grade for a specific course to a student.
        public void AssignGradeToStudentByID()
        {
            Console.CursorVisible = true;

            int studentId;

            while (true)
            {
                Console.Clear();
                Console.Write("To assign a grade to a student for a specific course in the database, " +
                  "\nplease provide valid student ID followed by a valid course ID. " +
                  "\nEnter \"q\" to conclude the process." +
                  "\n\nASSIGN GRADE TO STUDENT" +
                  "\n=======================" +
                  "\nEnter Student ID: ");

                string userInput = Console.ReadLine()!.ToLower().Trim();

                if (userInput == "q")
                {
                    Console.Clear();
                    Console.Write("The process has been terminated.\n");
                    break;
                }
                else if (int.TryParse(userInput, out studentId))
                {
                    if (StudentIdExistsInDb(studentId))
                    {
                        var studentInfo = Context.GetStudentInformation(studentId);

                        while (true)
                        {
                            PrintCourseAndShortStudentInfoFromId(studentInfo);

                            Console.Write("\nEnter Course ID (enter \"q\" to conclude the process): ");

                            userInput = Console.ReadLine()!.ToLower().Trim();

                            if (userInput == "q")
                            {
                                Console.Clear();
                                Console.Write("The process has been terminated.\n");

                                Console.CursorVisible = false;
                                PressAnyKeyMessage();
                                return;
                            }
                            else if (int.TryParse(userInput, out int courseId))
                            {
                                if (CourseIdExistsInDb(studentId, courseId))
                                {
                                    SetGrade(studentId, courseId);
                                    return;
                                }
                                else
                                {
                                    Console.Write("\nCourse ID doesn't exists in the database. Please try again.");
                                    Thread.Sleep(3000);
                                }
                            }
                            else
                            {
                                Console.Write("\nInvalid ID. Please try again.");
                                Thread.Sleep(3000);
                            }
                        }
                    }
                    else
                    {
                        Console.Write("\nStudent ID doesn't exists in the database. Please try again.");
                        Thread.Sleep(3000);
                    }
                }
                else
                {
                    Console.Write("\nInvalid ID. Please try again.");
                    Thread.Sleep(3000);
                }
            }

            Console.CursorVisible = false;
            PressAnyKeyMessage();
        }

        // Method to assign the grade and store it in the db. 
        private void SetGrade(int studentId, int courseId)
        {

            while (true)
            {
                Console.Clear();

                var chosenStudent = Context.Students.FirstOrDefault(s => s.StudentId == studentId);
                var chosenCourse = Context.Courses.FirstOrDefault(c => c.CourseId == courseId);

                Console.WriteLine($"Student: {chosenStudent?.FirstName} {chosenStudent?.Surname}" +
                                $"\nCourse:  {chosenCourse?.CourseName}");
                Console.Write("\nGrades range from A to F, where A is the highest and F is the lowest grade." +
                    "\nEnter \"q\" to conclude the process." +
                    "\n\nPlease assign a grade for the selected course: ");

                string grade = Console.ReadLine()!.Trim().ToUpperInvariant();

                if (grade == "Q")
                {
                    Console.Clear();
                    Console.Write("\nYou have chosen to conclude the process.\n");
                    break;
                }
                else if (GradeExistsInDb(grade))
                {
                    using (var transaction = Context.Database.BeginTransaction())
                    {
                        try
                        {
                            int gradeId = GetGradeId(grade);

                            var gradeType = Context.GradeTypes.FirstOrDefault(gt => gt.GradeId == gradeId);

                            var courseRegistration = Context.CourseRegistrations
                                .FirstOrDefault(cr => cr.FkStudentId == studentId && cr.FkCourseId == courseId);

                            var classMentorId = GetClassMentorId(studentId);

                            var assignedGrade = new Grade
                            {
                                FkCourseRegId = courseRegistration?.CourseRegId,
                                FkGradeId = gradeType?.GradeId,
                                GradedDate = DateTime.Now,
                                FkCourseTeacherId = classMentorId
                            };

                            Context.Add(assignedGrade);

                            Context.SaveChanges();

                            transaction.Commit();

                            Console.WriteLine("\nGrade was successfully assigned.");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Grade could not be assigned: {ex.Message}");
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    Console.Write("\nThe entered grade is not valid or does not exist in the database." +
                        "\nPlease make sure to enter a valid grade (A, B, C, D, E, or F).");
                    Thread.Sleep(3500);
                }
            }

            Console.CursorVisible = false;
            PressAnyKeyMessage();
        }

        // Mehtod that returns the id for specific class mentor.
        private int? GetClassMentorId(int studentId)
        {
            var classMentorId = Context.Students
                .Where(s => s.StudentId == studentId)
                .Join(Context.ClassMentors,
                    student => student.FkClassId,
                    mentor => mentor.FkClassId,
                    (student, mentor) => mentor.FkPersonnelId)
                .FirstOrDefault();

            return classMentorId;
        }

        // Method that returns the id for a certain grade type.
        private int GetGradeId(string grade)
        {
            var gradeType = Context.GradeTypes.FirstOrDefault(gt => gt.Grade == grade);

            return gradeType.GradeId;
        }

        // Method that checks whether a grade typ exists in the db.
        private bool GradeExistsInDb(string grade)
        {
            return Context.GradeTypes.Any(gt => gt.Grade == grade);
        }

        // Method that displays the information about a specific student based on student id.
        public void GetStudentInformationByID()
        {
            string userInput;

            while (true)
            {
                Console.CursorVisible = true;
                Console.Clear();

                Console.Write("To access vital information about a specific student in the Krutånger " +
                    "\nHigh School database, please provide a valid student ID." +
                    "\nEnter \"q\" to conclude the process." +
                    "\n\nRETRIEVE STUDENT INFORMATION" +
                    "\n============================" +
                    "\nEnter Student ID: ");

                userInput = Console.ReadLine()!.ToLower().Trim();

                if (userInput == "q")
                {
                    Console.Clear();
                    Console.Write("\nYou have chosen to conclude the process.\n");
                    break;
                }
                else if (int.TryParse(userInput, out int studentId))
                {
                    if (StudentIdExistsInDb(studentId))
                    {
                        var studentInfo = Context.GetStudentInformation(studentId);

                        PrintStudentInfoFromId(studentInfo);

                        break;
                    }
                    else
                    {
                        Console.Write("\nStudent ID doesn't exists in the database. Please try again.");
                        Thread.Sleep(3000);
                    }
                }
                else
                {
                    Console.Write("\nInvalid ID. Please try again.");
                    Thread.Sleep(3000);
                }
            }

            Console.CursorVisible = false;
            PressAnyKeyMessage();
        }

        // Method that retrieves and displays the courses and info for a specific student.
        private void PrintCourseAndShortStudentInfoFromId(List<StudentInformation> studentInfo)
        {
            Console.Clear();

            foreach (var s in studentInfo)
            {
                Console.WriteLine($"ID:    {s.StudentId}" +
                               $"\nName:   {s.FirstName} {s.Surname}" +
                               $"\nClass:  {s.ClassName}" +
                               $"\nBranch: {s.Branch}\n");
                
                var coursesEnrolledIn = (from cr in Context.CourseRegistrations
                                         where cr.FkStudentId == s.StudentId
                                         join c in Context.Courses on cr.FkCourseId equals c.CourseId
                                         join g in Context.Grades on cr.CourseRegId equals g.FkCourseRegId into gradeGroup
                                         from gg in gradeGroup.DefaultIfEmpty()
                                         join gt in Context.GradeTypes on gg.FkGradeId equals gt.GradeId into gradeTypeGroup
                                         from ggt in gradeTypeGroup.DefaultIfEmpty()
                                         select new
                                         {
                                             CourseId = c.CourseId,
                                             CourseName = c.CourseName,
                                             Grade = ggt.Grade
                                         }).ToList();

                Console.WriteLine("ID\tCourse\t\t\tGrade");
                Console.WriteLine(new string('-', 37));

                foreach (var c in coursesEnrolledIn)
                {
                    string formattedCourse = c.CourseName?.Length > 16 ? $"{c.CourseName}\t"
                        : c.CourseName?.Length <= 7 ? $"{c.CourseName}\t\t\t" : $"{c.CourseName}\t\t";

                    if (c.Grade == null)
                    {
                        Console.WriteLine($"{c.CourseId}\t{formattedCourse}N/A");
                    }
                    else
                    {
                        Console.WriteLine($"{c.CourseId}\t{formattedCourse}{c.Grade}");
                    }
                }
            }
        }

        // Method that displays info about a student.
        private void PrintStudentInfoFromId(List<StudentInformation> studentInfo)
        {
            Console.Clear();

            foreach (var s in studentInfo)
            {
                Console.WriteLine($"ID:       {s.StudentId}" +
                                $"\nName:     {s.FirstName} {s.Surname}" +
                                $"\nSSN:      {s.SSN}" +
                                $"\nGender:   {s.Gender}" +
                                $"\nPhone Nr: {s.PhoneNr}" +
                                $"\nEmail:    {s.Email}" +
                                $"\nClass:    {s.ClassName}" +
                                $"\nBranch:   {s.Branch}" +
                                $"\nMentor:   {s.Mentor}");

                Console.WriteLine("\nCOURSES REGISTERED - Below are the courses the student is enrolled in.");

                var coursesEnrolledIn = (from cr in Context.CourseRegistrations
                                         where cr.FkStudentId == s.StudentId
                                         join c in Context.Courses on cr.FkCourseId equals c.CourseId
                                         select new { c.CourseName })
                                        .ToList();

                foreach (var c in coursesEnrolledIn)
                {
                    Console.WriteLine($"{c.CourseName}");
                }
            }
        }

        // Method that returns a list of course IDs in which the student is enrolled.
        private List<int?> GetEnrolledCourses(int studentId)
        {
            var enrolledCourses = Context.CourseRegistrations
                .Where(cr => cr.FkStudentId == studentId)
                .Select(cr => cr.FkCourseId)
                .ToList();

            return enrolledCourses;
        }

        // Method that checks whether a certain course exists in the db based on its id.
        private bool CourseIdExistsInDb(int studentId, int courseId)
        {
            var enrolledCourseIds = GetEnrolledCourses(studentId);

            return enrolledCourseIds.Contains(courseId);
        }

        // Method that checks whether a student exists in the db based on its id.
        private bool StudentIdExistsInDb(int studentId)
        {
            return Context.Students.Any(s => s.StudentId == studentId);
        }

        // Method that displays the total salary for every department.
        public void PrintTotalSalaryForSpecificDepartment(int departmentChoice)
        {
            var departmentInfo = (from d in Context.Departments
                                  where d.DepartmentId == departmentChoice
                                  join p in Context.Personnel on d.DepartmentId equals p.FkDepartmentId
                                    into specificDepartment
                                  select new
                                  {
                                      DepartmentId = d.DepartmentId,
                                      DepartmentName = d.DepartmentName,
                                      TotalSalary = specificDepartment.Sum(p => p.Salary),
                                      AvgSalary = specificDepartment.Average(p => p.Salary)
                                  }
                                 ).FirstOrDefault();

            Console.Clear();

            if (departmentInfo != null)
            {
                Console.WriteLine($"DEPARTMENT: {departmentInfo.DepartmentName.ToUpper()}");
                Console.WriteLine("===========");
                Console.WriteLine($"Total Salary: {departmentInfo.TotalSalary:n1} kr.");
                Console.WriteLine($"Total Average Salary: {departmentInfo.AvgSalary:n1} kr.");
            }
            else
            {
                Console.WriteLine($"No information available for the chosen department.");
            }

            PressAnyKeyMessage();
        }

        // Method to confirm personnel entries with the user.
        private void ConfirmPersonnelEntries(Personnel newPersonnel)
        {
            // Retrieve associated gender and job title from the database.
            var selectedGender = Context.Genders.SingleOrDefault(g => g.GenderId == newPersonnel.FkGenderId);
            var selectedJobTitle = Context.JobTitles.SingleOrDefault(j => j.JobTitleId == newPersonnel.FkJobTitleId);

            // Display the entered values and promptCourseId for confirmation.
            string prompt = $"Are you satisfied with the entered values:\n" +
                $"\nFirst Name: {newPersonnel.FirstName}" +
                $"\nSurname:    {newPersonnel.Surname}" +
                $"\nSSN:        {newPersonnel.Ssn}" +
                $"\nGender:     {selectedGender?.TypeOfGender}" +
                $"\nJob Title:  {selectedJobTitle?.JobTitle1}\n";

            string[] menuOptions = { "Yes", "No" };

            GetMenu(prompt, menuOptions);
        }

        // Method to confirm s entries with the user.
        private void ConfirmStudentEntries(Student newStudent)
        {
            // Retrieve associated gender and class from the database.
            var selectedGender = Context.Genders.SingleOrDefault(g => g.GenderId == newStudent.FkGenderId);
            var selectedClass = Context.ClassLists.SingleOrDefault(c => c.ClassId == newStudent.FkClassId);

            // Display the entered values and promptCourseId for confirmation.
            string prompt = $"Are you satisfied with the entered values:\n" +
                $"\nFirst Name: {newStudent.FirstName}" +
                $"\nSurname:    {newStudent.Surname}" +
                $"\nSSN:        {newStudent.Ssn}" +
                $"\nGender:     {selectedGender.TypeOfGender}" +
                $"\nClass:      {selectedClass.ClassName}" +
                $"\nBranch:     {selectedClass.Branch}\n";

            string[] menuOptions = { "Yes", "No" };

            GetMenu(prompt, menuOptions);
        }
        // Method to promptCourseId the user to choose a gender.
        private void ChooseGender()
        {
            string prompt = "Select the gender of the student:";

            string[] menuOptions = { "Male", "Female", "Non Binary" };

            GetMenu(prompt, menuOptions);
        }

        // Method to promptCourseId the user to choose a class.
        private void ChooseClass()
        {
            string prompt = "Select class for student enrollment:";

            string[] menuOptions =
                {
                    "Class 7A", "Class 7B", "Class 8A",
                    "Class 8B", "Class 9A", "Class 9B"
                };

            GetMenu(prompt, menuOptions);
        }

        // Method to promptCourseId the user to choose a profession.
        private void ChooseProfession()
        {
            string prompt = "Choose profession for the new staff member:";

            string[] menuOptions =
            {
                "Principal", "Administrator",
                "Teacher", "Janitor", "School Psychologist",
                "Special Need Teacher", "Chef"
            };

            GetMenu(prompt, menuOptions);
        }

        // Method to add a new personnel to the database.
        public void AddPersonnel()
        {
            // Get user input for personnel details.
            string firstName = InputHandler.GetNonEmptyName("Enter a first name: ");
            string validatedFirstName = InputHandler.CapitalizeFirstLetter(firstName);
            string surname = InputHandler.GetNonEmptyName($"First Name: {validatedFirstName}\n" +
              $"\nEnter a surname: ");
            string validatedSurname = InputHandler.CapitalizeFirstLetter(surname);
            string validatedSsn = InputHandler.ValidateSSN($"First Name: {validatedFirstName}" +
                $"\nSurname: {validatedSurname}\n" +
                $"\nEnter SSN (YYYYMMDDXXXX): ");

            // Prompt the user to choose gender and profession.
            ChooseGender();
            GenderChoice = (byte)++MenuChoice;
            ChooseProfession();
            ProfessionChoice = (byte)++MenuChoice;

            // Create a new personnel object.
            var newPersonnel = new Personnel
            {
                FirstName = validatedFirstName,
                Surname = validatedSurname,
                Ssn = validatedSsn,
                FkGenderId = GenderChoice,
                FkJobTitleId = ProfessionChoice
            };

            // Confirm entered values with the user.
            ConfirmPersonnelEntries(newPersonnel);

            // Add personnel to the database if confirmed.
            if (MenuChoice == 0)
            {
                Context.Add(newPersonnel);
                Context.SaveChanges();

                Console.Clear();
                Console.WriteLine("The new staff member has been successfully registered.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("The process of register a new staff member has been canceled.");
            }

            PressAnyKeyMessage();
        }

        // Method to add a new student to the database.
        public void AddStudent(string prompt)
        {
            // Get user input for student details.
            string firstName = InputHandler.GetNonEmptyName("Enter the student's first name: ");
            string validatedFirstName = InputHandler.CapitalizeFirstLetter(firstName);
            string surname = InputHandler.GetNonEmptyName($"First Name: {validatedFirstName}\n" +
                $"\nEnter the student's surname: ");
            string validatedSurname = InputHandler.CapitalizeFirstLetter(surname);
            string validatedSsn = InputHandler.ValidateSSN($"First Name: {validatedFirstName}" +
                $"\nSurname: {validatedSurname}\n" +
                $"\nEnter the student's SSN (YYYYMMDDXXXX): ");

            // Prompt user to choose gender and class.
            ChooseGender();
            GenderChoice = (byte)++MenuChoice;
            ChooseClass();
            ClassChoice = ++MenuChoice;

            string generatedPhoneNr = DataGenerator.GeneratePhoneNr();
            string generatedEmail = DataGenerator.GenerateEmail(validatedFirstName, validatedSurname);

            // Create a new student object.
            var newStudent = new Student
            {
                FirstName = validatedFirstName,
                Surname = validatedSurname,
                Ssn = validatedSsn,
                FkGenderId = GenderChoice,
                PhoneNr = generatedPhoneNr,
                Email = generatedEmail,
                FkClassId = ClassChoice,
                SchoolStart = DateTime.Now
            };

            // Confirm entered values with the user.
            ConfirmStudentEntries(newStudent);

            // Add student to the database if confirmed.            
            if (MenuChoice == 0)
            {
                Context.Add(newStudent);
                Context.SaveChanges();

                // Retrieve the student from the newly added student in the database.
                var addedStudent = Context.Students.OrderByDescending(s => s.StudentId).First();

                // Get the list of courses for the selected class.
                var classCourses = Context.ClassLists
                    .Include(cl => cl.Students)
                        .ThenInclude(s => s.CourseRegistrations)
                            .ThenInclude(cr => cr.FkCourse)
                    .Where(cl => cl.ClassId == addedStudent.FkClassId)
                    .SelectMany(cl => cl.Students)
                    .SelectMany(s => s.CourseRegistrations)
                    .Select(cr => cr.FkCourse)
                    .Distinct()
                    .ToList();

                // Register the student for each c in registered to the class.
                foreach (var course in classCourses)
                {
                    if (course != null)
                    {
                        var courseRegistration = new CourseRegistration
                        {
                            FkStudentId = addedStudent.StudentId,
                            FkCourseId = course.CourseId
                        };

                        Context.CourseRegistrations.Add(courseRegistration);
                    }
                }

                Context.SaveChanges();

                Console.Clear();
                Console.WriteLine("The new student has been successfully enrolled.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("The process of enrolling a new student has been canceled.");
            }

            PressAnyKeyMessage();
        }

        // Method to print all courses with average grades as well as the lowest and highest grade for each c.
        public void PrintAllCoursesWithAvgGrade()
        {
            // Retrieve and display courses with average grade as well as lowest and highest grade for each c.
            var courseWithAverageGrade = Context.CoursesWithAverageGradeViews
                .OrderBy(c => c.Course).ToList();

            // Display grade information in formatted table.
            Console.WriteLine("Course\t\t\tAverage Grade\tLowest Grade\tHighest Grade");
            Console.WriteLine($"{new string('-', 69)}");

            foreach (var c in courseWithAverageGrade)
            {
                // Format name of c for better alignment.
                string formattedCourse = c.Course?.Length > 16 ? $"{c.Course}\t"
                 : c.Course?.Length <= 7 ? $"{c.Course}\t\t\t" : $"{c.Course}\t\t";

                Console.Write($"{formattedCourse}{c.AverageGrade}\t\t{c.LowestGrade}\t\t{c.HighestGrade}");

                Console.WriteLine();
            }

            PressAnyKeyMessage();
        }

        // Method to print grades of students in the latest month.
        public void PrintGradesLatestMonth()
        {
            // Retrieve and display graded students in the latest month from the database.
            var latestGradedStudents = Context.LatestMonthGradesViews
                .OrderByDescending(s => s.GradedDate)
                .ThenBy(s => s.ClassName)
                .ThenBy(s => s.Surname).ToList();

            if (latestGradedStudents.Count == 0)
            {
                Console.WriteLine("No grades have been graded in the latest month.");
            }
            else
            {
                // Display information about s and their grade in each c in formatted table.
                Console.WriteLine("First Name\tLast Name\tClass\tCourse\t\t\tGrade\tDate Graded");
                Console.WriteLine($"{new string('-', 83)}");

                foreach (var s in latestGradedStudents)
                {
                    // Format first name, surname and name of c for better alignment.
                    string formattedFirstName = s.FirstName?.Length > 7 ? $"{s.FirstName}\t" : $"{s.FirstName}\t\t";
                    string formattedSurname = s.Surname?.Length > 7 ? $"{s.Surname}\t" : $"{s.Surname}\t\t";
                    string formattedCourse = s.Course?.Length > 16 ? $"{s.Course}\t"
                        : s.Course?.Length <= 7 ? $"{s.Course}\t\t\t" : $"{s.Course}\t\t";

                    Console.Write($"{formattedFirstName}{formattedSurname}{s.ClassName}\t{formattedCourse}{s.Grade}\t{s.GradedDate:dd-MM-yyyy}");

                    Console.WriteLine();
                }
            }

            PressAnyKeyMessage();
        }

        // Method to print students from a specific class.
        public void PrintStudentsFromSpecificClass(int sortByChoice, int orderByChoice, int classChoice)
        {
            // Order students based on user choices and retrieve students from a specific class.
            var orderedStudents = sortByChoice switch
            {
                0 => orderByChoice == 0 ? Context.Students.OrderBy(s => s.FirstName) : Context.Students.OrderByDescending(s => s.FirstName),
                1 => orderByChoice == 0 ? Context.Students.OrderBy(s => s.Surname) : Context.Students.OrderByDescending(s => s.Surname),
                2 => orderByChoice == 0 ? Context.Students.OrderBy(s => s.StudentId) : Context.Students.OrderByDescending(s => s.StudentId),
                _ => throw new ArgumentException("Invalid sortByChoice")
            };

            var studentsFromSpecificClass = (from s in orderedStudents
                                             join cl in Context.ClassLists
                                                on s.FkClassId equals cl.ClassId
                                             join g in Context.Genders
                                                on s.FkGenderId equals g.GenderId
                                             where s.FkClassId == classChoice
                                             select new
                                             {
                                                 s.StudentId,
                                                 s.FirstName,
                                                 s.Surname,
                                                 s.Ssn,
                                                 g.TypeOfGender,
                                                 cl.ClassName,
                                                 cl.Branch,
                                                 s.SchoolStart
                                             }).ToList();

            // Display student information in formatted table.
            Console.Clear();
            Console.WriteLine("ID\tFirst Name\tLast Name\tSSN\t\tGender\tClass\tBranch\t\t\tDate of Enrollment");
            Console.WriteLine($"{new string('-', 114)}");

            foreach (var s in studentsFromSpecificClass)
            {
                // Format first name and surname for better alignment.
                string formattedFirstName = s.FirstName.Length > 7 ? $"{s.FirstName}\t" : $"{s.FirstName}\t\t";
                string formattedSurname = s.Surname.Length > 7 ? $"{s.Surname}\t" : $"{s.Surname}\t\t";

                Console.Write($"{s.StudentId}\t{formattedFirstName}{formattedSurname}{s.Ssn}\t{s.TypeOfGender}\t{s.ClassName}\t{s.Branch}\t\t{s.SchoolStart:yyyy-MM-dd}");

                Console.WriteLine();
            }

            PressAnyKeyMessage();
        }

        // Method to retrieve info about each student and their grades in each c.
        public void GetGradesForEachStudent()
        {
            Console.Clear();

            // Retrieve students with class information and associated grades.
            var studentsWithClassInfo = Context.Students
                .Include(s => s.FkClass)
                .Include(s => s.CourseRegistrations)
                    .ThenInclude(cr => cr.FkCourse)
                .Include(s => s.CourseRegistrations)
                    .ThenInclude(cr => cr.Grades)
                        .ThenInclude(g => g.FkGrade)
                .Include(s => s.CourseRegistrations)
                    .ThenInclude(cr => cr.Grades)
                        .ThenInclude(g => g.FkCourseTeacher)
                            .ThenInclude(ct => ct.FkPersonnel)
                .Where(s => s.CourseRegistrations.Any(cr => cr.Grades.Count != 0))
                .OrderBy(s => s.FkClass.ClassName)
                .ThenBy(s => s.Surname)
                .ToList();

            // Print grades for each student.
            foreach (var student in studentsWithClassInfo)
            {
                PrintStudentGradesForEachCourse(student);
            }

            PressAnyKeyMessage();
        }

        // Method to print info about a student's grades, including their
        // name, class, courses, grades, teachers, and grading dates.
        private static void PrintStudentGradesForEachCourse(Student student)
        {
            Console.WriteLine($"Student: {student.FirstName} {student.Surname}");
            Console.WriteLine($"Class: {student.FkClass?.ClassName} ({student.FkClass?.Branch})" +
                $"\n------");

            // Iterate through each c the student is registered in.
            foreach (var cr in student.CourseRegistrations)
            {
                // Iterate through each grade that's been graded in every c the student participated in. 
                foreach (var g in cr.Grades)
                {
                    Console.WriteLine($"Course: {g.FkCourseReg?.FkCourse?.CourseName}");
                    Console.WriteLine($"Grade: {g.FkGrade?.Grade}");
                    Console.WriteLine($"Teacher: {g.FkCourseTeacher?.FkPersonnel?.FirstName} {g.FkCourseTeacher?.FkPersonnel?.Surname}");
                    Console.WriteLine($"Date graded: {g.GradedDate?.ToString("yyyy-MM-dd")}\n");
                }
            }

            Console.WriteLine("------------------------------\n");
        }

        // Method that displays every active course.
        public void PrintEveryActiveCourse()
        {
            var courses = Context.Courses
                .Include(cr => cr.CourseTeachers)
                .Where(c => c.EndDate >= DateTime.Now)
                .OrderBy(c => c.EndDate)
                .ThenBy(c => c.CourseName)
                .ToList();

            Console.WriteLine("Course\t\t\tStart Date\tEnd Date\tTeacher");
            Console.WriteLine($"{new string('-', 68)}");

            foreach (var c in courses)
            {
                string formattedCourseName = c.CourseName?.Length > 16 ? $"{c.CourseName}\t"
                    : c.CourseName?.Length <= 7 ? $"{c.CourseName}\t\t\t" : $"{c.CourseName}\t\t";

                var teacher = Context.CourseTeachers
                    .Where(ct => ct.FkCourseId == c.CourseId)
                    .Select(ct => ct.FkPersonnel)
                    .FirstOrDefault();

                string teacherFullName = teacher != null ? $"{teacher.FirstName} {teacher.Surname}" : "N/A";

                Console.WriteLine($"{formattedCourseName}{c.StartDate:yyyy-MM-dd}\t{c.EndDate:yyyy-MM-dd}\t{teacherFullName}");
            }

            PressAnyKeyMessage();
        }

        // Method to print all students with sorting and ordering options.
        public void PrintAllStudentsWithOptions(int sortByChoice, int orderByChoice)
        {
            // Order students based on user choices and retrieve all students.
            var orderedStudents = sortByChoice switch
            {
                0 => orderByChoice == 0 ? Context.Students.OrderBy(s => s.FirstName) : Context.Students.OrderByDescending(s => s.FirstName),
                1 => orderByChoice == 0 ? Context.Students.OrderBy(s => s.Surname) : Context.Students.OrderByDescending(s => s.Surname),
                2 => orderByChoice == 0 ? Context.Students.OrderBy(s => s.StudentId) : Context.Students.OrderByDescending(s => s.StudentId),
                _ => throw new ArgumentException("Invalid sortByChoice")
            };

            var students = (from s in orderedStudents
                            join cl in Context.ClassLists
                                on s.FkClassId equals cl.ClassId
                            join g in Context.Genders
                                on s.FkGenderId equals g.GenderId
                            select new
                            {
                                s.StudentId,
                                s.FirstName,
                                s.Surname,
                                s.Ssn,
                                g.TypeOfGender,
                                cl.ClassName,
                                cl.Branch,
                                s.SchoolStart
                            }).ToList();

            // Display student information in formatted table.
            Console.Clear();
            Console.WriteLine("ID\tFirst Name\tLast Name\tSSN\t\tGender\tClass\tBranch\t\t\tDate of Enrollment");
            Console.WriteLine($"{new string('-', 114)}");

            foreach (var s in students)
            {
                // Format first name and surname for better alignment.
                string formattedFirstName = s.FirstName.Length > 7 ? $"{s.FirstName}\t" : $"{s.FirstName}\t\t";
                string formattedSurname = s.Surname.Length > 7 ? $"{s.Surname}\t" : $"{s.Surname}\t\t";

                Console.Write($"{s.StudentId}\t{formattedFirstName}{formattedSurname}{s.Ssn}\t{s.TypeOfGender}\t{s.ClassName}\t{s.Branch}\t\t{s.SchoolStart:dd-MM-yyyy}");

                Console.WriteLine();
            }

            PressAnyKeyMessage();
        }

        // Method that displays number of teachers in each branch.
        public void PrintNumberOfTeachersInEachBranch()
        {
            var teachersPerBranch = Context.ClassMentors
                .GroupBy(cm => cm.FkClass.Branch)
                .Select(n => new
                {
                    BranchName = n.Key,
                    TeacherCount = n.Count()
                })
                .ToList();

            foreach (var result in teachersPerBranch)
            {
                Console.WriteLine($"Total Number of Teachers in Branch {result.BranchName}: {result.TeacherCount}");
                Console.WriteLine("---");
            }

            PressAnyKeyMessage();
        }

        // Method to print personnel with specific job titles.
        public void PrintPersonnelWithSpecificJobTitles(int menuChoice)
        {
            // Retrieve and display personnel with a specific job title from the database.
            var personnel = (from p in Context.Personnel
                             join jt in Context.JobTitles
                                 on p.FkJobTitleId equals jt.JobTitleId
                             where p.FkJobTitleId == menuChoice
                             orderby p.Surname
                             select new
                             {
                                 p.FirstName,
                                 p.Surname,
                                 JobTitle = jt.JobTitle1,
                                 p.EmploymentDate,
                                 p.FkDepartment
                             }).ToList();

            // Display personnel information in formatted table.
            Console.Clear();
            Console.WriteLine("First Name\tLast Name\tJob Title\t\tDepartment\t\tEmployed Since\tYears Employed");
            Console.WriteLine($"{new string('-', 110)}");

            foreach (var p in personnel)
            {
                // Format first name and surname for better alignment.
                string formattedFirstName = p.FirstName.Length > 7 ? $"{p.FirstName}\t" : $"{p.FirstName}\t\t";
                string formattedSurname = p.Surname.Length > 7 ? $"{p.Surname}\t" : $"{p.Surname}\t\t";
                string formattedJobTitle = p.JobTitle.Length > 16 ? $"{p.JobTitle}\t"
                    : p.JobTitle.Length <= 7 ? $"{p.JobTitle}\t\t\t" : $"{p.JobTitle}\t\t";
                string formattedDepartmentName = p.FkDepartment.DepartmentName?.Length > 16 ?
                    $"{p.FkDepartment.DepartmentName}\t" : $"{p.FkDepartment.DepartmentName}\t\t";

                Console.Write($"{formattedFirstName}{formattedSurname}{formattedJobTitle}" +
                    $"{formattedDepartmentName}{p.EmploymentDate:yyyy-MM-dd}\t{YearsOfEmployment(p.EmploymentDate)}");

                Console.WriteLine();
            }

            PressAnyKeyMessage();
        }

        // Method to print all personnel information.
        public void PrintAllPersonnel()
        {
            // Retrieve and display personnel information, including related job titles from the database.
            var personnel = (from p in Context.Personnel
                             join jt in Context.JobTitles
                                  on p.FkJobTitleId equals jt.JobTitleId
                             orderby p.PersonnelId
                             select new
                             {
                                 p.PersonnelId,
                                 p.FirstName,
                                 p.Surname,
                                 JobTitle = jt.JobTitle1,
                                 p.EmploymentDate,
                                 p.FkDepartment
                             }).ToList();

            // Display personnel information in formatted table.
            Console.Clear();
            Console.WriteLine("ID\tFirst Name\tLast Name\tJob Title\t\tDepartment\t\tEmployed Since\tYears Employed");
            Console.WriteLine($"{new string('-', 118)}");

            foreach (var p in personnel)
            {
                // Format first name and surname for better alignment.
                string formattedFirstName = p.FirstName.Length > 7 ? $"{p.FirstName}\t" : $"{p.FirstName}\t\t";
                string formattedSurname = p.Surname.Length > 7 ? $"{p.Surname}\t" : $"{p.Surname}\t\t";
                string formattedJobTitle = p.JobTitle.Length > 16 ? $"{p.JobTitle}\t"
                    : p.JobTitle.Length <= 7 ? $"{p.JobTitle}\t\t\t" : $"{p.JobTitle}\t\t";
                string formattedDepartmentName = p.FkDepartment.DepartmentName?.Length > 16 ?
                    $"{p.FkDepartment.DepartmentName}\t" : $"{p.FkDepartment.DepartmentName}\t\t";

                Console.Write($"{p.PersonnelId}\t{formattedFirstName}{formattedSurname}{formattedJobTitle}" +
                    $"{formattedDepartmentName}{p.EmploymentDate:yyyy-MM-dd}\t{YearsOfEmployment(p.EmploymentDate)}");

                Console.WriteLine();
            }

            PressAnyKeyMessage();
        }

        // Method that returns the number of years a employee have been working at Krutånger High School.
        private static int YearsOfEmployment(DateTime employmentDate)
        {
            int numberOfYears = DateTime.Now.Year - employmentDate.Year;

            if (DateTime.Now.Month < employmentDate.Month || (DateTime.Now.Month == employmentDate.Month && DateTime.Now.Day < employmentDate.Day))
            {
                numberOfYears--;
            }

            return numberOfYears;
        }

        // Method to display a message and wait for any key press.
        private static void PressAnyKeyMessage()
        {
            Console.WriteLine("\nPress any key to go back.");
            Console.ReadKey();
        }
    }
}
