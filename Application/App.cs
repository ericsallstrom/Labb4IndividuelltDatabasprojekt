using Labb4IndividuelltDatabasprojekt.Data;
using Labb4IndividuelltDatabasprojekt.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb4IndividuelltDatabasprojekt.Application
{
    internal class App
    {
        // Properties for sorting and checking menu choices.
        private int SortByChoice { get; set; }
        private int SortByOrder { get; set; }
        private int MenuChoice { get; set; }
        private HandleQueries PrintQuery { get; set; }

        // Constructor to initialize the database context and PrintQueries instance.
        public App()
        {
            var context = new KrutångerHighSchoolDbContext();
            PrintQuery = new HandleQueries(context);
        }

        // Main method to run the application.
        public void RunApp()
        {
            Console.CursorVisible = false;            

            while (true)
            {
                // Display the main menu.
                RunMainMenu();

                // Execute the selected option.
                switch (MenuChoice)
                {
                    case 0:
                        RunPersonnelManagementMenu();
                        break;
                    case 1:
                        RunAcademicInformationMenu();
                        break;
                    case 2:
                        RunRegistrationMenu();
                        break;
                    case 3:
                        ExitApp();
                        return;
                }
            }
        }

        // Method to runt the department salary retrieval.
        private void RetrieveDepartmentSalary()
        {
            while (true)
            {
                string prompt = "Review the total salary expenditure and average salary for each department." +
               "\n\nDEPARTMENT SALARY OVERVIEW" +
               "\n==========================";

                string[] menuOptions =
                {
                    "Administration", "Teaching", "Special Education", "Support Staff",
                    "Finance & HR", "Cafeteria", "Health Services", "Back"
                };

                GetMenu(prompt, menuOptions);

                if (MenuChoice != 7)
                {
                    int departmentChoice = ++MenuChoice;
                    PrintQuery.PrintTotalSalaryForSpecificDepartment(departmentChoice);
                }
                else
                {
                    return;
                }
            }
        }

        // Method to register new personnel.
        private void RegisterNewPersonnel()
        {
            string prompt = ("To register a new staff member, please provide the following details: " +
               "\nfirst name, surname, social security number (SSN)" +
               "\nand select appropriate profession from the available options." +
               "\n\nREGISTER NEW PERSONNEL" +
               "\n======================");

            string[] menuOptions = { "Register Personnel", "Back" };

            GetMenu(prompt, menuOptions);

            // Process the user's choice.
            if (MenuChoice == 0)
            {
                PrintQuery.AddPersonnel();
            }
        }

        // Method to enroll a new student.
        private void EnrollNewStudent()
        {
            string prompt = ("To enroll a new student, please provide the following details: " +
                "\nfirst name, surname, social security number (SSN)" +
                "\nand select a class from the available options." +
                "\n\nENROLL NEW STUDENT" +
                "\n====================");

            string[] menuOptions = { "Enroll Student", "Back" };

            GetMenu(prompt, menuOptions);

            if (MenuChoice == 0)
            {
                PrintQuery.AddStudent(prompt);
            }
        }

        // Method to display how many teachers there are in each branch.
        private void RetrieveNumberOfTeachersInEachBranch()
        {
            Console.Clear();
            Console.WriteLine("Overview of the distribution of teachers across the different branches." +
                "\n\nNUMBER OF TEACHERS IN EACH BRANCH" +
                  "\n=================================\n");

            PrintQuery.PrintNumberOfTeachersInEachBranch();
        }

        // Displays the grade for each course for every student.
        private void RetrieveGradesForEachStudent()
        {
            Console.Clear();
            Console.WriteLine("Review each student's performance, examining grades for" +
                "\nevery course, assessed by their respective teachers," +
                "\nalong with the date of the assessment." +
                "\n\nSTUDENT GRADE OVERVIEW" +
                  "\n======================\n");

            PrintQuery.GetGradesForEachStudent();
        }

        // Displays every active course.
        private void RetrieveEveryActiveCourse()
        {
            Console.Clear();
            Console.WriteLine("Examine the details of each course." +
                "\n\nCOURSE OVERVIEW" +
                  "\n===============\n");

            PrintQuery.PrintEveryActiveCourse();
        }

        // Method to retrieve information about all courses with average
        // grades, as well as the highest and lowest grade for each course.
        private void RetrieveCoursesWithGrades()
        {
            Console.Clear();
            Console.WriteLine("Review every course, featuring the average grade, as well" +
                "\nas the highest and lowest grades achieved in every class." +
                "\n\nCOURSE PERFORMANCE OVERVIEW" +
                "\n===========================\n");

            PrintQuery.PrintAllCoursesWithAvgGrade();
        }

        // Method to retrieve grades from latest month.
        private void RetrieveGradesFromLatestMonth()
        {
            Console.Clear();
            Console.WriteLine("Review the grades of each student for every course from the latest month." +
                "\n\nLATEST MONTH GRADE REPORT" +
                "\n=========================\n");

            PrintQuery.PrintGradesLatestMonth();
        }

        // Method to exit the app.
        private static void ExitApp()
        {
            Console.Clear();
            Console.WriteLine("The program is shutting down...");
            Thread.Sleep(2000);
            Console.WriteLine("\nHave a great day and welcome back.");
            Thread.Sleep(3000);
        }

        // Method to display the main menu.
        private void RunMainMenu()
        {
            string prompt = "Welcome to the Krutånger High School Database." +
                "\nExplore and retrieve a wide range of information about students and personnel." +
                "\n\nNavigate using the arrow keys and press Enter to make your selection." +
                "\n\nKRUTÅNGER HIGH SCHOOL - DATABASE" +
                "\n================================";

            string[] menuOptions =
            {
                "Personnel Management", "Academic Information", "Registration", "Exit"
            };

            GetMenu(prompt, menuOptions);
        }

        // Method to run the registration menu.
        private void RunRegistrationMenu()
        {
            while (true)
            {
                string prompt = "Seamlessly register new students and personnel, ensuring smooth onboarding into the school system." +
                    "\n\nREGISTRATION" +
                      "\n============";

                string[] menuOptions =
                {
                    "Register New Student", "Register New Personnel", "Back"
                };

                GetMenu(prompt, menuOptions);

                if (MenuChoice == 0)
                {
                    EnrollNewStudent();
                }
                else if (MenuChoice == 1)
                {
                    RegisterNewPersonnel();
                }
                else
                {
                    return;
                }
            }
        }

        // Method to run the academic information menu.
        private void RunAcademicInformationMenu()
        {
            while (true)
            {
                string prompt = "Handle student and grade management efficently. Stay updated on the latest " +
                       "\nmonth's grades, course details, and student performance in various subjects. " +
                       "\nAssign grades to students. View information about a specific student by ID." +                       
                       "\n\nACADEMIC INFORMATION" +
                       "\n====================";

                string[] menuOptions =
                {
                    "View Student Details", "View Specific Student Information by ID", "View Grades Assigned in the Latest Month", 
                    "Explore Every Active Course", "Analyze Courses with Average, Highest, and Lowest Grades", 
                    "Examine Grades of Every Student in Each Course", "Assign Grade to Student by ID", "Back"
                };

                GetMenu(prompt, menuOptions);

                switch (MenuChoice)
                {
                    case 0:
                        RetrieveStudentsWithOptions();
                        break;
                    case 1:
                        PrintQuery.GetStudentInformationByID();
                        break;
                    case 2:
                        RetrieveGradesFromLatestMonth();
                        break;
                    case 3:
                        RetrieveEveryActiveCourse();
                        break;
                    case 4:
                        RetrieveCoursesWithGrades();
                        break;
                    case 5:
                        RetrieveGradesForEachStudent();
                        break;
                    case 6:
                        PrintQuery.AssignGradeToStudentByID();
                        break;
                    case 7:
                        return;
                }
            }
        }

        // Method to run the personnel management menu.
        private void RunPersonnelManagementMenu()
        {
            while (true)
            {
                string prompt = "Access detailed personnel information, calculate salary statistics, and " +
                        "\nunderstand the distribution of teachers across different branches." +
                        "\n\nPERSONNEL MANAGEMENT" +
                        "\n====================";

                string[] menuOptions =
                {
                    "View Personnel Details", "Calculate Total & Average Salary by Department", "Explore Number of Teachers in Each Branch", "Back"
                };

                GetMenu(prompt, menuOptions);

                if (MenuChoice == 0)
                {
                    RetrievePersonnelWithOptions();
                }
                else if (MenuChoice == 1)
                {
                    RetrieveDepartmentSalary();
                }
                else if (MenuChoice == 2)
                {
                    RetrieveNumberOfTeachersInEachBranch();
                }
                else
                {
                    return;
                }
            }
        }

        // Method to display the menu for seeing students from a specific class.
        private void SeeStudentsFromSpecificClassMenu()
        {
            while (true)
            {
                string prompt = "Retrieve information about students from a specific class." +
                    "\nYou can choose sorting by first name or surname and specify" +
                    "\nin which order they should be presented." +
                    "\n\nRETRIEVE STUDENTS FROM A SPECIFIC CLASS" +
                    "\n=======================================";

                string[] menuOptions =
                {
                    "Class 7A", "Class 7B", "Class 8A",
                    "Class 8B", "Class 9A", "Class 9B",
                    "Back"
                };

                GetMenu(prompt, menuOptions);

                string prompt2 = "Please select your sorting preference for the students' names." +
                    "\n\nSORT BY FIRST NAME OR SURNAME" +
                    "\n=============================";

                if (MenuChoice != 6)
                {
                    int classChoice = ++MenuChoice;
                    SortByChoice = GetSortingByFirstNameLastNameOrID(prompt2);
                    SortByOrder = GetOrderBy();

                    PrintQuery.PrintStudentsFromSpecificClass(SortByChoice, SortByOrder, classChoice);
                }
                else
                {
                    return;
                }
            }
        }

        // Method to display the menu for sorting by first name or surname.
        private int GetSortingByFirstNameLastNameOrID(string prompt)
        {
            string[] menuOptions = { "First Name", "Surname", "Student ID" };

            GetMenu(prompt, menuOptions);

            return MenuChoice;
        }

        // Method to display the menu for sorting order.
        private int GetOrderBy()
        {
            string prompt = "Please specify whether the list of students " +
                "\nshould be sorted in ascending or descending order." +
                "\n\nPRESENTED ORDER" +
                "\n===============";

            string[] menuOptions = { "Ascending Order", "Descending Order" };

            GetMenu(prompt, menuOptions);

            return MenuChoice;
        }

        // Method to retrieve students with various sorting and filtering options.
        private void RetrieveStudentsWithOptions()
        {
            while (true)
            {
                string prompt = "Retrieve information about every student at Krutånger High School, sorted" +
                    "\nby either first name, surname or student id. Choose to see the information" +
                    "\nof all students or students from a specific class. Additionally, you can " +
                    "\nspecify whether the sorting should be in ascending or descending order." +
                    "\n\nSTUDENT OVERVIEW" +
                    "\n================";

                string prompt2 = "Please select your sorting preference." +
                       "\n\nSORT BY FIRST NAME, SURNAME OR ID" +
                       "\n=================================";

                string[] menuOptions = { "All Students", "Students From a Specific Class", "Back" };

                GetMenu(prompt, menuOptions);


                if (MenuChoice == 0)
                {
                    SortByChoice = GetSortingByFirstNameLastNameOrID(prompt2); // Let the user choose first name, surname or student id.
                    SortByOrder = GetOrderBy(); // Let the user choose ascending or descending order.

                    PrintQuery.PrintAllStudentsWithOptions(SortByChoice, SortByOrder);
                }
                else if (MenuChoice == 1)
                {
                    SeeStudentsFromSpecificClassMenu();
                }
                else
                {
                    return;
                }
            }
        }

        // Method to retrieve personnel with sorting and filtering options.
        private void RetrievePersonnelWithOptions()
        {
            while (true)
            {
                string prompt = "Retrieve information of every personnel at Krutånger High School" +
                    "\nor filter personnel based on their specific job titles." +
                    "\n\nPERSONNEL OVERVIEW" +
                    "\n==================";

                string[] menuOptions =
                {
                    "All Employees", "Principal", "Administrators",
                    "Teachers", "Janitors", "School Psychologist",
                    "Special Need Teachers", "Chef", "Accountant", "Back"
                };

                GetMenu(prompt, menuOptions);

                if (MenuChoice == 0)
                {
                    PrintQuery.PrintAllPersonnel();
                }
                else if (MenuChoice == 9)
                {
                    return;
                }
                else
                {
                    PrintQuery.PrintPersonnelWithSpecificJobTitles(MenuChoice);
                }
            }
        }

        // Method to display the menu and get user's choice.
        public void GetMenu(string prompt, string[] menuOptions)
        {
            Menu menu = new()
            {
                Prompt = prompt,
                MenuOptions = menuOptions
            };

            MenuChoice = menu.GetMenuChoice();
        }
    }
}
