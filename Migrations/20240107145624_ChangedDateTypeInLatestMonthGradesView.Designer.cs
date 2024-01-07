﻿// <auto-generated />
using System;
using Labb4IndividuelltDatabasprojekt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Labb4IndividuelltDatabasprojekt.Migrations
{
    [DbContext(typeof(KrutångerHighSchoolDbContext))]
    [Migration("20240107145624_ChangedDateTypeInLatestMonthGradesView")]
    partial class ChangedDateTypeInLatestMonthGradesView
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.ClassList", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassId"));

                    b.Property<string>("Branch")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ClassName")
                        .HasMaxLength(2)
                        .IsUnicode(false)
                        .HasColumnType("varchar(2)");

                    b.HasKey("ClassId");

                    b.ToTable("ClassList", (string)null);
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.ClassMentor", b =>
                {
                    b.Property<int?>("FkClassId")
                        .HasColumnType("int")
                        .HasColumnName("FK_ClassId");

                    b.Property<int?>("FkPersonnelId")
                        .HasColumnType("int")
                        .HasColumnName("FK_PersonnelId");

                    b.HasIndex("FkClassId");

                    b.HasIndex("FkPersonnelId");

                    b.ToTable("ClassMentor", (string)null);
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<string>("CourseName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Course");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("CourseId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.CourseRegistration", b =>
                {
                    b.Property<int>("CourseRegId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseRegId"));

                    b.Property<int?>("FkCourseId")
                        .HasColumnType("int")
                        .HasColumnName("FK_CourseId");

                    b.Property<int?>("FkStudentId")
                        .HasColumnType("int")
                        .HasColumnName("FK_StudentId");

                    b.HasKey("CourseRegId");

                    b.HasIndex("FkCourseId");

                    b.HasIndex("FkStudentId");

                    b.ToTable("CourseRegistration", (string)null);
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.CourseTeacher", b =>
                {
                    b.Property<int>("CourseTeacherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseTeacherId"));

                    b.Property<int?>("FkCourseId")
                        .HasColumnType("int")
                        .HasColumnName("FK_CourseId");

                    b.Property<int?>("FkPersonnelId")
                        .HasColumnType("int")
                        .HasColumnName("FK_PersonnelId");

                    b.HasKey("CourseTeacherId");

                    b.HasIndex("FkCourseId");

                    b.HasIndex("FkPersonnelId");

                    b.ToTable("CourseTeacher", (string)null);
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentId"));

                    b.Property<string>("DepartmentName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Gender", b =>
                {
                    b.Property<byte>("GenderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("GenderId"));

                    b.Property<string>("TypeOfGender")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("Gender");

                    b.HasKey("GenderId")
                        .HasName("PK__Genders__4E24E9F7FF3B3015");

                    b.ToTable("Genders");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Grade", b =>
                {
                    b.Property<int>("GradedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GradedId"));

                    b.Property<int?>("FkCourseRegId")
                        .HasColumnType("int")
                        .HasColumnName("FK_CourseRegId");

                    b.Property<int?>("FkCourseTeacherId")
                        .HasColumnType("int")
                        .HasColumnName("FK_CourseTeacherId");

                    b.Property<byte?>("FkGradeId")
                        .HasColumnType("tinyint")
                        .HasColumnName("FK_GradeId");

                    b.Property<DateTime?>("GradedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("GradedId");

                    b.HasIndex("FkCourseRegId");

                    b.HasIndex("FkCourseTeacherId");

                    b.HasIndex("FkGradeId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.GradeType", b =>
                {
                    b.Property<byte>("GradeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("GradeId"));

                    b.Property<string>("Grade")
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("char(1)")
                        .IsFixedLength();

                    b.HasKey("GradeId");

                    b.ToTable("GradeType", (string)null);
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.JobTitle", b =>
                {
                    b.Property<byte>("JobTitleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("JobTitleId"));

                    b.Property<string>("JobTitle1")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("JobTitle");

                    b.HasKey("JobTitleId");

                    b.ToTable("JobTitles");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Personnel", b =>
                {
                    b.Property<int>("PersonnelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonnelId"));

                    b.Property<string>("Email")
                        .HasMaxLength(75)
                        .IsUnicode(false)
                        .HasColumnType("varchar(75)");

                    b.Property<DateTime>("EmploymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<int?>("FkDepartmentId")
                        .HasColumnType("int");

                    b.Property<byte?>("FkGenderId")
                        .HasColumnType("tinyint")
                        .HasColumnName("FK_GenderId");

                    b.Property<byte?>("FkJobTitleId")
                        .HasColumnType("tinyint")
                        .HasColumnName("FK_JobTitleId");

                    b.Property<string>("PhoneNr")
                        .HasMaxLength(16)
                        .IsUnicode(false)
                        .HasColumnType("varchar(16)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Ssn")
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnType("varchar(13)")
                        .HasColumnName("SSN");

                    b.Property<string>("Surname")
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.HasKey("PersonnelId");

                    b.HasIndex("FkDepartmentId");

                    b.HasIndex("FkGenderId");

                    b.HasIndex("FkJobTitleId");

                    b.HasIndex(new[] { "Ssn" }, "UQ_PersonnelSSN")
                        .IsUnique()
                        .HasFilter("[SSN] IS NOT NULL");

                    b.HasIndex(new[] { "Ssn" }, "UQ_Personnel_SSN")
                        .IsUnique()
                        .HasFilter("[SSN] IS NOT NULL");

                    b.ToTable("Personnel");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"));

                    b.Property<string>("Email")
                        .HasMaxLength(75)
                        .IsUnicode(false)
                        .HasColumnType("varchar(75)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<int?>("FkClassId")
                        .HasColumnType("int")
                        .HasColumnName("FK_ClassId");

                    b.Property<byte?>("FkGenderId")
                        .HasColumnType("tinyint")
                        .HasColumnName("FK_GenderId");

                    b.Property<string>("PhoneNr")
                        .HasMaxLength(16)
                        .IsUnicode(false)
                        .HasColumnType("varchar(16)");

                    b.Property<DateTime?>("SchoolStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("Ssn")
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnType("varchar(13)")
                        .HasColumnName("SSN");

                    b.Property<string>("Surname")
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.HasKey("StudentId");

                    b.HasIndex("FkClassId");

                    b.HasIndex("FkGenderId");

                    b.HasIndex(new[] { "Ssn" }, "UQ_StudentSSN")
                        .IsUnique()
                        .HasFilter("[SSN] IS NOT NULL");

                    b.HasIndex(new[] { "Ssn" }, "UQ_Students_SSN")
                        .IsUnique()
                        .HasFilter("[SSN] IS NOT NULL");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Views.CoursesWithAverageGradeView", b =>
                {
                    b.Property<string>("AverageGrade")
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1)");

                    b.Property<string>("Course")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("HighestGrade")
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1)");

                    b.Property<string>("LowestGrade")
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1)");

                    b.ToTable((string)null);

                    b.ToView("CoursesWithAverageGradeView", (string)null);
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Views.LatestMonthGradesView", b =>
                {
                    b.Property<string>("ClassName")
                        .HasMaxLength(2)
                        .IsUnicode(false)
                        .HasColumnType("varchar(2)");

                    b.Property<string>("Course")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<string>("Grade")
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("char(1)")
                        .IsFixedLength();

                    b.Property<DateTime?>("GradedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Surname")
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.ToTable((string)null);

                    b.ToView("LatestMonthGradesView", (string)null);
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.ClassMentor", b =>
                {
                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.ClassList", "FkClass")
                        .WithMany()
                        .HasForeignKey("FkClassId")
                        .HasConstraintName("FK_ClassMentor_ClassList");

                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.Personnel", "FkPersonnel")
                        .WithMany()
                        .HasForeignKey("FkPersonnelId")
                        .HasConstraintName("FK_ClassMentor_Personnel");

                    b.Navigation("FkClass");

                    b.Navigation("FkPersonnel");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.CourseRegistration", b =>
                {
                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.Course", "FkCourse")
                        .WithMany("CourseRegistrations")
                        .HasForeignKey("FkCourseId")
                        .HasConstraintName("FK_CourseRegistration_Courses");

                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.Student", "FkStudent")
                        .WithMany("CourseRegistrations")
                        .HasForeignKey("FkStudentId")
                        .HasConstraintName("FK_CourseRegistration_Students");

                    b.Navigation("FkCourse");

                    b.Navigation("FkStudent");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.CourseTeacher", b =>
                {
                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.Course", "FkCourse")
                        .WithMany("CourseTeachers")
                        .HasForeignKey("FkCourseId")
                        .HasConstraintName("FK_CourseTeacher_Courses");

                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.Personnel", "FkPersonnel")
                        .WithMany("CourseTeachers")
                        .HasForeignKey("FkPersonnelId")
                        .HasConstraintName("FK_CourseTeacher_Personnel");

                    b.Navigation("FkCourse");

                    b.Navigation("FkPersonnel");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Grade", b =>
                {
                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.CourseRegistration", "FkCourseReg")
                        .WithMany("Grades")
                        .HasForeignKey("FkCourseRegId")
                        .HasConstraintName("FK_Grades_CourseRegistration");

                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.CourseTeacher", "FkCourseTeacher")
                        .WithMany("Grades")
                        .HasForeignKey("FkCourseTeacherId")
                        .HasConstraintName("FK_Grades_CourseTeacher");

                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.GradeType", "FkGrade")
                        .WithMany("Grades")
                        .HasForeignKey("FkGradeId")
                        .HasConstraintName("FK_Grades_GradeType");

                    b.Navigation("FkCourseReg");

                    b.Navigation("FkCourseTeacher");

                    b.Navigation("FkGrade");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Personnel", b =>
                {
                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.Department", "FkDepartment")
                        .WithMany()
                        .HasForeignKey("FkDepartmentId");

                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.Gender", "FkGender")
                        .WithMany("Personnel")
                        .HasForeignKey("FkGenderId")
                        .HasConstraintName("FK_Personnel_Genders");

                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.JobTitle", "FkJobTitle")
                        .WithMany("Personnel")
                        .HasForeignKey("FkJobTitleId")
                        .HasConstraintName("FK_Personnel_JobTitles");

                    b.Navigation("FkDepartment");

                    b.Navigation("FkGender");

                    b.Navigation("FkJobTitle");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Student", b =>
                {
                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.ClassList", "FkClass")
                        .WithMany("Students")
                        .HasForeignKey("FkClassId")
                        .HasConstraintName("FK_Students_ClassList");

                    b.HasOne("Labb4IndividuelltDatabasprojekt.Models.Gender", "FkGender")
                        .WithMany("Students")
                        .HasForeignKey("FkGenderId")
                        .HasConstraintName("FK_Students_Genders");

                    b.Navigation("FkClass");

                    b.Navigation("FkGender");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.ClassList", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Course", b =>
                {
                    b.Navigation("CourseRegistrations");

                    b.Navigation("CourseTeachers");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.CourseRegistration", b =>
                {
                    b.Navigation("Grades");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.CourseTeacher", b =>
                {
                    b.Navigation("Grades");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Gender", b =>
                {
                    b.Navigation("Personnel");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.GradeType", b =>
                {
                    b.Navigation("Grades");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.JobTitle", b =>
                {
                    b.Navigation("Personnel");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Personnel", b =>
                {
                    b.Navigation("CourseTeachers");
                });

            modelBuilder.Entity("Labb4IndividuelltDatabasprojekt.Models.Student", b =>
                {
                    b.Navigation("CourseRegistrations");
                });
#pragma warning restore 612, 618
        }
    }
}
