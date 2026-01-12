using Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.EmployeeDto;

public class UpdateEmployeeDto
{
    public required string FullName { get; set; }
    public RoleForEmployee RoleForEmployee { get; set; }
    public string Position { get; set; }
    public int SubDepartmentId { get; set; }
}