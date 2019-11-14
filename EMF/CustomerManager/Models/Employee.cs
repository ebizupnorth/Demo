using CustomerManager.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManager.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }

    public class EmployeeValidator : AbstractValidator<Employee>
    {
        private readonly AppDbContext _context;
        public EmployeeValidator(AppDbContext context)
        {
            _context = context;

            //validate firstname
            RuleFor(t => t.FirstName).NotEmpty();

            //validate lastname
            RuleFor(t => t.LastName).NotEmpty();

            //validate contact number
            RuleFor(t => t.ContactNumber).NotEmpty();
            //contact number must be digit only
            RuleFor(t => t.ContactNumber).Matches(@"^\d+");

            //validate department id
            RuleFor(t => t.DepartmentId).NotEmpty().WithMessage("Please select department or create a new one.");

            //check for existing employee
            RuleFor(t => t).Must(t => !IsEmployeeExists(t)).WithMessage("Employee already exists.");

        }

        private bool IsEmployeeExists(Employee employee)
        {
            var savedEmployee = _context
                .Employees
                .AsNoTracking()
                .Where(t => t.FirstName == employee.FirstName
                && t.LastName == employee.LastName
                && t.ContactNumber == employee.ContactNumber)
                .FirstOrDefault();

            //not exist
            if (savedEmployee == null) return false;

            //check if updating
            if (savedEmployee.EmployeeId == employee.EmployeeId) return false;

            return true;
        }

    }
}
