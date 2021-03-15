namespace JacksonVeroneze.StockService.Application.Validations
{
    public class ApplicationValidationMessages
    {
        public const string ProductFoundByDescription = "Produto já cadastrado com a descrição informada";
        public const string ProductNotFoundById = "Produto näo encontrado com o código informado";
        public const string ProductHasItems = "Este produto tem dependencias, portanto não pode ser removido";

        //
        public const string PurchaseFoundByDescription = "Compra já cadastrado com a descrição informada";
        public const string PurchaseNotFoundById = "Compra näo encontrado com o código informado";
        public const string PurchaseHasItems = "Esta compra tem dependencias, portanto não pode ser removido";

        public const string PurchaseIsClosed =
            "A compra informada já encontra-se fechada, portanto não pode ser movimentada ou excluida";
    }
}
