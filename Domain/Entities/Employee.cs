using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public RoleForEmployee RoleForEmployee { get; set; }   
    public int PositionId { get; set; }
    public Position Position { get; set; }

    public int? SubDepartmentId { get; set; }
    public SubDepartment? SubDepartment { get; set; }

    public List<FixedAsset> FixedAssets { get; set; } = new();
    public List<InventoryItem> InventoryItems { get; set; } = new();
}