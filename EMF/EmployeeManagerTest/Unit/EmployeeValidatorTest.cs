using CustomerManager.Data;
using CustomerManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EmployeeManagerTest.Unit
{
    public class EmployeeValidatorTest
    {
        [Fact]
        public void Add_Valid_EmployeeModel_Returns_Success()
        {

            //Arrange
            var options = TestFactory.CreateDbContextOptions("EmployeeAddDb");
            using var context = new AppDbContext(options);

            var validator = new EmployeeValidator(context);


            //add a department
            var department = new Department { DepartmentName = "Tech" };
            context.Departments.Add(department);
            context.SaveChanges();

            //Act
            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                ContactNumber = "1234567",
                DepartmentId = department.DepartmentId
            };          
            var result = validator.Validate(employee);

            //Assert
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Empty_EmployeeModel_Returns_Error()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("EmployeeEmptyDb");
            using var context = new AppDbContext(options);

            var validator = new EmployeeValidator(context);
            //Act
            var result = validator.Validate(new Employee());

            //Assert
            Assert.Contains(result.Errors, e => e.PropertyName == "FirstName");
            Assert.Contains(result.Errors, e => e.PropertyName == "LastName");
            Assert.Contains(result.Errors, e => e.PropertyName == "ContactNumber");
        }

        [Fact]
        public void Duplicate_Employee_Returns_Error()
        {

            //Arrange
            var options = TestFactory.CreateDbContextOptions("EmployeeDuplicateDb");
            using var context = new AppDbContext(options);

            var validator = new EmployeeValidator(context);

            //add a department
            var department = new Department { DepartmentName = "Tech" };
            //add employee
            context.Employees.Add(new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                ContactNumber = "1234567",
                Department = department
            });
            context.SaveChanges();

            //Act
            var result = validator.Validate(new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                ContactNumber = "1234567",
                Department = department
            });

            //Assert
            Assert.NotEmpty(result.Errors);
        }


    }


}
