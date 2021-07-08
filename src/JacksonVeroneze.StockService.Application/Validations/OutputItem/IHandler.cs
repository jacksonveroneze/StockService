namespace JacksonVeroneze.StockService.Application.Validations.OutputItem
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        object Handle(object request);
    }
}
