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
    public class Department
    {
        public int DepartmentId { get; set; }
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
    }

    public class DepartmentValidator : AbstractValidator<Department>
    {
        private readonly AppDbContext _context;
        public DepartmentValidator(AppDbContext context)
        {
            _context = context;

            // validate DepartmentName
            RuleFor(t => t.DepartmentName).NotEmpty();

            // validate for existing department name
            RuleFor(t => t).Must(t => !IsDeparmentExists(t)).WithMessage("Department name already exists.");
        }

        private bool IsDeparmentExists(Department department)
        {
            return _context.Departments.Any(t => t.DepartmentName == department.DepartmentName);
        }
    }
}
