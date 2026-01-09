using System.Net;
using Domain.Dtos.EmployeeDto;
using Domain.Dtos.FixedAssetDto;
using Domain.Dtos.InventoryItem;
using Domain.Entities;
using Domain.Exeptions;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.EmployeeRepositories;
using Infrastructure.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class EmployeeService(IEmployeeRepository repository, IWebHostEnvironment _environment) : IEmployeeService
{
    public async Task<PaginationResponse<List<GetEmployeeDto>>> GetAllEmployeeAsync(EmployeeFilter filter)
    {
        var employee = await repository.GetAll(filter);
        var totalRecords = employee.Count;
        var data = employee
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();
        var result = data.Select(e => new GetEmployeeDto()
        {
            Id = e.Id,
            FullName = e.FullName,
            RoleForEmployee = e.RoleForEmployee,
            PositionId = e.PositionId,
            SubDepartmentId = e.SubDepartmentId,
            FixedAssets = e.FixedAssets.Select(f => new GetFixedAssetDto()
            {
                Id = f.Id,
                Name = f.Name,
                InventoryNumber = f.InventoryNumber,
                AcquisitionDate = f.AcquisitionDate,
                EmployeeId = f.EmployeeId,
                SerialNumber = f.SerialNumber,
                UsefulLifeYears = f.UsefulLifeYears,
            }).ToList(),
            InventoryItems = e.InventoryItems.Select(i => new GetInventoryItemDto()
            {
                Id = i.Id,
                Name = i.Name,
                InventoryNumber = i.InventoryNumber,
                AcquisitionDate = i.AcquisitionDate,
                EmployeeId = i.EmployeeId,
                Unit = i.Unit,
            }).ToList()
        }).ToList();
        return new PaginationResponse<List<GetEmployeeDto>>(result, totalRecords, filter.PageNumber,
            filter.PageSize);
    }

    public async Task<ApiResponse<GetEmployeeDto>> GetByIdAsync(int id)
    {
        var employee = await repository.GetEmployee(q => q.Id == id);
        if (employee == null)
        {
            throw new ApiException($"No employee found with id: {id}");
        }

        var result = new GetEmployeeDto()
        {
            Id = employee.Id,
            FullName = employee.FullName,
            RoleForEmployee = employee.RoleForEmployee,
            PositionId = employee.PositionId,
            SubDepartmentId = employee.SubDepartmentId,
            FixedAssets = employee.FixedAssets.Select(f => new GetFixedAssetDto()
            {
                Id = f.Id,
                Name = f.Name,
                InventoryNumber = f.InventoryNumber,
                AcquisitionDate = f.AcquisitionDate,
                EmployeeId = f.EmployeeId,
                SerialNumber = f.SerialNumber,
                UsefulLifeYears = f.UsefulLifeYears,
            }).ToList(),
            InventoryItems = employee.InventoryItems.Select(i => new GetInventoryItemDto()
            {
                Id = i.Id,
                Name = i.Name,
                InventoryNumber = i.InventoryNumber,
                AcquisitionDate = i.AcquisitionDate,
                EmployeeId = i.EmployeeId,
                Unit = i.Unit,
            }).ToList()
        };
        return new ApiResponse<GetEmployeeDto>(result);
    }

    public async Task<ApiResponse<string>> CreateAsync(AddEmployeeDto request)
    {
        var employee = new Employee()
        {
            FullName = request.FullName,
            RoleForEmployee = request.RoleForEmployee,
            PositionId = request.PositionId,
            SubDepartmentId = request.SubDepartmentId,
        };

      

        var result = await repository.CreateEmployee(employee);
        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK, "Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, UpdateEmployeeDto request)
    {
        var employee = await repository.GetEmployee(q => q.Id == id);
        if (employee == null)
        {
            throw new ApiException($"No employee found with id: {id}");
        }

        employee.Id = request.Id;
        employee.FullName = request.FullName;
        employee.RoleForEmployee = request.RoleForEmployee;
        employee.PositionId = request.PositionId;
        employee.SubDepartmentId = request.SubDepartmentId;

       

        var result = await repository.UpdateEmployee(employee);
        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK, "Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> UpdateUserProfileImageAsync(int employeeId,
        IFormFile profileImage)
    {
        var user = await repository.GetEmployee(u => u.Id == employeeId);
        if (user == null)
            return new ApiResponse<string>(HttpStatusCode.NotFound, "User not found");
        const long maxFileSize = 5 * 1024 * 1024;
        if (profileImage.Length > maxFileSize)
            return new ApiResponse<string>(HttpStatusCode.BadRequest, "Image file size must be less than 5MB");

        var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(profileImage.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
            return new ApiResponse<string>(HttpStatusCode.BadRequest,
                "Invalid image format. Allowed: .jpg, .jpeg, .png, .gif");

        var uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads", "profiles");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

      
        await repository.UpdateEmployee(user);

        return new ApiResponse<string>(HttpStatusCode.OK,"Profile image updated successfully");
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        var employee = await repository.GetEmployee(q => q.Id == id);
        if (employee == null)
        {
            throw new ApiException($"No employee found with id: {id}");
        }

        var result = await repository.DeleteEmployee(employee);
        return result == 1
            ? new ApiResponse<string>("Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }
}