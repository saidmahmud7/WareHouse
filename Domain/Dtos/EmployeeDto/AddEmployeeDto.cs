using Domain.Dtos.FixedAssetDto;
using Domain.Dtos.InventoryItem;
using Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.EmployeeDto;

public class AddEmployeeDto
{
    public required string FullName { get; set; }
    public RoleForEmployee RoleForEmployee { get; set; }

    public int PositionId { get; set; }
    public int? SubDepartmentId { get; set; }
}