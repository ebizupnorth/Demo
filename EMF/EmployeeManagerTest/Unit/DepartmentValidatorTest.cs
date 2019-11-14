using CustomerManager.Data;
using CustomerManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EmployeeManagerTest.Unit
{
    public class DepartmentValidatorTest
    {
        [Fact]
        public void Empty_DepartmentName_Returns_Error()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("DepartmentEmptyDb");

            using var context = new AppDbContext(options);

            var validator = new DepartmentValidator(context);

            //Act
            var result = validator.Validate(new Department());

            //Assert
            Assert.Contains(result.Errors, e => e.PropertyName == "DepartmentName");
        }

        [Fact]
        public void NotEmpty_DepartmentName_Returns_Success()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("DepartmentNotEmptyDb");

            using var context = new AppDbContext(options);

            var validator = new DepartmentValidator(context);

            //Act
            var result = validator.Validate(new Department { DepartmentName = "IT" });

            //Assert
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Duplicate_Name_Returns_Error()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("DepartmentDuplicateDb");

            using var context = new AppDbContext(options);

            //add a department
            context.Departments.Add(new Department { DepartmentName = "HR" });
            context.SaveChanges();

            var validator = new DepartmentValidator(context);

            //Act
            var result = validator.Validate(new Department { DepartmentName = "HR" });

            //Assert
            Assert.NotEmpty(result.Errors);
        }

    }


}
