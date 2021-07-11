using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Models;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Repositories
{
    public interface IDevolutionRepository : IRepository<Devolution>
    {
        Task<IList<DevolutionItemModel>> FindItems(Guid devolutionId);
    }
}
