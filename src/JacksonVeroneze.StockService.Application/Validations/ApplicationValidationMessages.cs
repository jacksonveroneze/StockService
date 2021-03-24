namespace JacksonVeroneze.StockService.Application.Validations
{
    public class ApplicationValidationMessages
    {
        // Product
        public const string ProductFoundByDescription = "Produto já cadastrado com a descrição informada";
        public const string ProductNotFoundById = "Produto não encontrado com o código informado";
        public const string ProductHasItems = "Este produto tem dependencias, portanto não pode ser removido";

        // Purchase
        public const string PurchaseNotFoundById = "Compra näo encontrado com o código informado";
        public const string PurchaseHasItems = "Esta compra tem dependencias, portanto não pode ser removido";

        public const string PurchaseIsClosed =
            "A compra informada já encontra-se fechada, portanto não pode ser movimentada ou excluida";


        // PurchaseItem
        public const string PurchaseItemNotFoundById = "Item da compra näo encontrado com o código informado";

        // Adjustment
        public const string AdjustmentNotFoundById = "Compra näo encontrado com o código informado";
        public const string AdjustmentHasItems = "Esta compra tem dependencias, portanto não pode ser removido";

        public const string AdjustmentIsClosed =
            "A compra informada já encontra-se fechada, portanto não pode ser movimentada ou excluida";


        // AdjustmentItem
        public const string AdjustmentItemNotFoundById = "Item da compra näo encontrado com o código informado";

        // Output
        public const string OutputNotFoundById = "Compra näo encontrado com o código informado";
        public const string OutputHasItems = "Esta compra tem dependencias, portanto não pode ser removido";

        public const string OutputIsClosed =
            "A compra informada já encontra-se fechada, portanto não pode ser movimentada ou excluida";

        // OutputItem
        public const string OutputItemNotFoundById = "Item da compra näo encontrado com o código informado";
    }
}
