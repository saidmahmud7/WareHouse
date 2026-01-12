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
    public async Task<ApiResponse<List<GetEmployeeDto>>> GetAllEmployeeAsync(EmployeeFilter filter)
    {
        var employee = await repository.GetAll(filter);
       
        var result = employee.Select(e => new GetEmployeeDto()
        {
            Id = e.Id,
            FullName = e.FullName,
            RoleForEmployee = e.RoleForEmployee,
            Position = e.Position,
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
        return new ApiResponse<List<GetEmployeeDto>>(result);
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
            Position = employee.Position,
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
            Position = request.Position,
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

        employee.FullName = request.FullName;
        employee.RoleForEmployee = request.RoleForEmployee;
        employee.Position = request.Position;
        employee.SubDepartmentId = request.SubDepartmentId;

       

        var result = await repository.UpdateEmployee(employee);
        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK, "Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
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