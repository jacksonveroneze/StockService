namespace JacksonVeroneze.StockService.Domain.Util
{
    public static class Messages
    {
        public const string ItemFound = "Este item encontra-se como filho do registro atual.";
        public const string ProductFound = "O produto informado já encontra-se como filho do registro atual.";
        public const string MovementItemNotFound = "O item de movimento não foi encontrado.";
        public const string ItemNotFound = "Este item não encontra-se como filho do registro atual.";
        public const string RegisterClosed = "Este registro já está fechado.";
        public const string RegisterClosedNotMoviment = "Este registro já está fechado, não pode ser movimentado.";
    }
}
