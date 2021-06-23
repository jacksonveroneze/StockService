using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Models;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Repositories
{
    public interface IOutputRepository : IRepository<Output>
    {
        Task<IList<OutputItemModel>> FindItems(Guid outputId);
    }
}
