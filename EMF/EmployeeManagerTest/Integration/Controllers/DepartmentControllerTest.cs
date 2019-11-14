using CustomerManager.Controllers;
using CustomerManager.Data;
using CustomerManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagerTest.Integration.Controllers
{
    public class DepartmentControllerTest
    {
        [Fact]
        public async Task Index_Returns_ViewAndListOfDeparment()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("DepControllerIndexDb");
            using var context = new AppDbContext(options);

            //add some data
            await context.Departments.AddRangeAsync(new Department { DepartmentName = "IT" },
                new Department { DepartmentName = "IT" });
            await context.SaveChangesAsync();

            var controller = new DepartmentController(context);

            //Act
            var result = await controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Department>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIndexIsNull()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("DepControllerDetailsIndexNullDb");
            using var context = new AppDbContext(options);

            var controller = new DepartmentController(context);

            //Act
            var result = await controller.Details(null);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIndexDoesNotExist()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("DepControllerDetailsIndexNotExistDb");
            using var context = new AppDbContext(options);

            var controller = new DepartmentController(context);

            //Act
            var result = await controller.Details(0);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewAndResult_WhenIndexExist()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("DepControllerDetailsIndexExistDb");
            using var context = new AppDbContext(options);

            var department = new Department { DepartmentName = "Test" };
            await context.AddAsync(department);
            await context.SaveChangesAsync();

            var controller = new DepartmentController(context);

            //Act
            var result = await controller.Details(department.DepartmentId);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Department>(viewResult.ViewData.Model);
            Assert.Equal(model.DepartmentId, department.DepartmentId);
        }

        [Fact]
        public async Task CreatePost_SavesData_ReturnsRedirectActionResult_WhenModelIsValid()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("DepControllerDetailsCreateValidDb");
            using var context = new AppDbContext(options);
            var controller = new DepartmentController(context);

            //Act
            var result = await controller.Create(new Department { DepartmentName = "New Department" });

            //Assert
            Assert.IsType<RedirectToActionResult>(result);

            var departments = await context.Departments.ToListAsync();
            Assert.NotEmpty(departments);
        }

        [Fact]
        public async Task Edit_UpdatesData_ReturnsRedirectActionResult_WhenModelIsValid()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("DepControllerDetailsCreateValidDb");
            using var context = new AppDbContext(options);
            var controller = new DepartmentController(context);
            //init department
            var existingDepartment = new Department { DepartmentName = "Department" };
            await context.AddAsync(existingDepartment);
            await context.SaveChangesAsync();

            //update name
            existingDepartment.DepartmentName = "New Department";

            //Act
            var result = await controller.Edit(existingDepartment.DepartmentId, existingDepartment);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);

            var updatedDepartment = await context
                .Departments
                .Where(t => t.DepartmentId == existingDepartment.DepartmentId)
                .FirstOrDefaultAsync();

            Assert.Equal(existingDepartment.DepartmentName, updatedDepartment.DepartmentName);
        }

        [Fact]

        public async Task DeleteConfirmed_DeletesData_AndReturnsRedirectToAction_WhenIdIsValid()
        {
            //Arrange
            var options = TestFactory.CreateDbContextOptions("DepControllerDeleteConfirmedValidDb");
            using var context = new AppDbContext(options);
            var controller = new DepartmentController(context);
            //init department
            var existingDepartment = new Department { DepartmentName = "Department" };
            await context.AddAsync(existingDepartment);
            await context.SaveChangesAsync();

            //Act
            var result = await controller.DeleteConfirmed(existingDepartment.DepartmentId);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);

            Assert.Empty(await context.Departments.ToListAsync());
        }
    }
}
