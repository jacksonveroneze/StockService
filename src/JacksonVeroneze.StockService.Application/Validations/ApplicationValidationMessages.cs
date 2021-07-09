namespace JacksonVeroneze.StockService.Application.Validations
{
    public static class ApplicationValidationMessages
    {
        // Product
        public const string ProductFoundByDescription = "Produto já cadastrado com a descrição informada";
        public const string ProductNotFoundById = "Produto não encontrado com o código informado";
        public const string ProductHasItems = "Este produto tem dependencias, portanto não pode ser removido";

        // Purchase
        public const string PurchaseNotFoundById = "Compra não encontrado com o código informado";
        public const string PurchaseHasItems = "Esta compra tem dependencias, portanto não pode ser removido";

        public const string PurchaseIsClosed =
            "A compra informada já encontra-se fechada, portanto não pode ser movimentada ou excluida";


        // PurchaseItem
        public const string PurchaseItemNotFoundById = "Item da compra não encontrado com o código informado";

        // Adjustment
        public const string AdjustmentNotFoundById = "Acerto não encontrado com o código informado";
        public const string AdjustmentHasItems = "Este acerto tem dependencias, portanto não pode ser removido";

        public const string AdjustmentIsClosed =
            "O acerto informada já encontra-se fechada, portanto não pode ser movimentada ou excluida";


        // AdjustmentItem
        public const string AdjustmentItemNotFoundById = "Item do acerto não encontrado com o código informado";

        // Output
        public const string OutputNotFoundById = "Saída não encontrada com o código informado";
        public const string OutputHasItems = "Esta saída tem dependencias, portanto não pode ser removido";

        public const string OutputIsClosed =
            "A saída informada já encontra-se fechada, portanto não pode ser movimentada ou excluida";

        public const string OutputIsNotClosed =
            "A saída informada não está fechada, portanto não pode ser movimentada ou excluida";

        public const string OutputIsOpened =
            "A saída informada está aberta, portanto não pode ser reaberta";

        // OutputItem
        public const string OutputItemNotFoundById = "Item da saída não encontrado com o código informado";
        public const string OutputItemProductNotMovement = "Item da saída (Produto) não tem movimentação de estoque";
        public const string OutputItemProductNotSufficientStock= "Item da saída (Produto) não tem estoque suficiente para esta saída";
        public const string OutputItemWithoutMovementItem = "Item da saída sem movimentação de estoque";
    }
}
