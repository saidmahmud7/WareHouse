using System.Net;
using Domain.Dtos;
using Domain.Dtos.DepartmentDto;
using Domain.Dtos.SubDepartmentDto;
using Domain.Entities;
using Domain.Exeptions;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.DepartmentRepositories;
using Infrastructure.Response;

namespace Infrastructure.Services;

public class DepartmentService(IDepartmentRepository repository) : IDepartmentService
{
    public async Task<ApiResponse<List<GetDepartmentDto>>> GetAllDepartmentAsync(DepartmentFilter filter)
    {
        var department = await repository.GetAll(filter);
       
        var result = department.Select(d => new GetDepartmentDto()
        {
            Id = d.Id,
            Name = d.Name,
            SubDepartments = d.SubDepartments.Select(s => new GetSubDepartmentDto()
            {
                Id = s.Id,
                Name = s.Name,
                DepartmentId = s.DepartmentId,

            }).ToList()
        }).ToList();
        return new ApiResponse<List<GetDepartmentDto>>(result);
    }

    public async Task<ApiResponse<GetDepartmentDto>> GetByIdAsync(int id)
    {
        var department = await repository.GetDepartment(q => q.Id == id);
        if (department == null)
        {
            throw new ApiException($"No Department found with id: {id}");
        }

        var result = new GetDepartmentDto()
        {
            Id = department.Id,
            Name = department.Name,
           
            SubDepartments = department.SubDepartments.Select(s => new GetSubDepartmentDto()
            {
                Id = s.Id,
                Name = s.Name,
                DepartmentId = s.DepartmentId,
                
            }).ToList()
        };
        return new ApiResponse<GetDepartmentDto>(result);
    }

    public async Task<ApiResponse<string>> CreateAsync(AddDepartmentDto request)
    {
        var department = new Department()
        {
            Name = request.Name,
          
        };
        var result = await repository.CreateDepartment(department);

        return result == 1
            ? new ApiResponse<string>("Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, UpdateDepartmentDto request)
    {
        var department = await repository.GetDepartment(q => q.Id == id);
        if (department == null)
        {
            throw new ApiException($"No Department found with id: {id}");
        }

        department.Name = request.Name;
        
        var result = await repository.UpdateDepartment(department);
        return result == 1
            ? new ApiResponse<string>("Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        var department = await repository.GetDepartment(q => q.Id == id);
        if (department == null)
        {
            throw new ApiException($"No Department found with id: {id}");
        }

        var result = await repository.DeleteDepartment(department);
        return result == 1
            ? new ApiResponse<string>("Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }
}

