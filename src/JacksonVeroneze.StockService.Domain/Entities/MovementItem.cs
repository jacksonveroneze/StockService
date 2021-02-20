using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class MovementItem : Entity
    {
        public int Amount { get; private set; }

        public Movement Movement { get; private set; }

        public MovementItem(int amount, Movement movement)
        {
            Amount = amount;
            Movement = movement;

            Validate();
        }

        private void Validate()
        {
            Validacoes.ValidarSeMenorQue(Amount, 1, "A quantidade deve ser maior que zero");
        }
    }
}
