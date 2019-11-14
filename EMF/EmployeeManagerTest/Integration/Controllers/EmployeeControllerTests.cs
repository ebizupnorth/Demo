using CustomerManager.Controllers;
using CustomerManager.Data;
using CustomerManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagerTest.Integration.Controllers
{
    public class EmployeeControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewAndListofEmployees()
        {

            //Arrange
            var options = TestFactory.CreateDbContextOptions("EmpControllerIndexDb");
            using var context = new AppDbContext(options);

            //add department
            var department = new Department { DepartmentName = "Emp" };
            await context.AddAsync(department);
            await context.SaveChangesAsync();

            //add some data
            await context.Employees.AddRangeAsync(new Employee
            {
                FirstName = "First",
                LastName = "Last",
                ContactNumber = "2222333",
                DepartmentId = department.DepartmentId
            }, new Employee
            {
                FirstName = "First2",
                LastName = "Last2",
                ContactNumber = "22223332",
                DepartmentId = department.DepartmentId
            }
            );
            await context.SaveChangesAsync();

            var controller = new EmployeeController(context);

            //Act
            var result = await controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Employee>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task CreatePost_SavesData_AndReturnsRedirectToAction_WhenModelIsValid()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("EmpControllerCreateDb");
            using var context = new AppDbContext(options);

            //add department
            var department = new Department { DepartmentName = "Emp" };
            await context.AddAsync(department);
            await context.SaveChangesAsync();

            var newEmployee = new Employee
            {
                FirstName = "First",
                LastName = "Last",
                ContactNumber = "2222333",
                DepartmentId = department.DepartmentId
            };
            var controller = new EmployeeController(context);

            //Act
            var result = await controller.Create(newEmployee);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.NotEmpty(await context.Employees.ToListAsync());
        }

        [Fact]
        public async Task DeleteConfirmed_DeletesData_AndReturnsRedirectToAction_WhenIdIsValid()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("EmpControllerDeleteValidDb");
            using var context = new AppDbContext(options);
            var controller = new DepartmentController(context);
            //add department
            var department = new Department { DepartmentName = "Department" };
            await context.AddAsync(department);
            await context.SaveChangesAsync();

            //add employee
            await context.AddAsync(new Employee
            {
                FirstName = "First",
                LastName = "Last",
                ContactNumber = "2222333",
                DepartmentId = department.DepartmentId
            });

            //Act
            var result = await controller.DeleteConfirmed(department.DepartmentId);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);

            Assert.Empty(await context.Employees.ToListAsync());
        }
    }
}
