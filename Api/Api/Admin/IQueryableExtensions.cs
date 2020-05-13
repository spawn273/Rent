using RentApi.Infrastructure.Database.Models;
using SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities;
using System.Collections.Generic;
using System.Linq;

namespace RentApi.Api.Admin
{
    public static class IQueryableExtensions
    {
        public static IQueryable<AccountDTO> ToDTO(this IQueryable<User> equipment)
        {
            return equipment.Select(x => new AccountDTO
            {
                Id = x.Id,
                Name = $"{x.FirstName} {x.MiddleName} {x.LastName}",
                UserName = x.UserName,
                ShopId = x.Employee.ShopId
            });
        }
    }
}
