namespace JacksonVeroneze.StockService.Core.Data
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
