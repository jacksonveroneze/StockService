using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Purchase : Entity
    {
        public string Description { get; private set; }

        public PurchaseStateEnum State { get; private set; } = PurchaseStateEnum.Open;

        public decimal Value { get; set; }

        private readonly List<PurchaseItem> _purchaseItems = new List<PurchaseItem>();
        public IReadOnlyCollection<PurchaseItem> PedidoItems => _purchaseItems;

        public Purchase()
        {
        }

        public Purchase(string description)
            => Description = description;

        public void AddPurchaseItem(PurchaseItem item)
        {
            if (State == PurchaseStateEnum.Closed)
                throw new DomainException("Este compra já está fechada, não pode ser movimentada.");

            if (item.IsValid() is false)
                return;

            if (ExistsPurchaseItem(item))
            {
                PurchaseItem currentItem = _purchaseItems.First(x => x.Id == item.Id);

                currentItem.UpdateItemFromOtherPurchaseItem(item);

                RemovePurchaseItem(item);

                item = currentItem;
            }

            _purchaseItems.Add(item);
        }

        public void RemovePurchaseItem(PurchaseItem item)
        {
            if (State == PurchaseStateEnum.Closed)
                throw new DomainException("Este compra já está fechada, não pode ser movimentada.");

            if (ExistsPurchaseItem(item) is false)
                throw new DomainException("Este item não encontra-se na compra.");

            _purchaseItems.Remove(item);
        }

        public decimal CalculatePurchaseValue()
        {
            Value = _purchaseItems.Sum(x => x.CalculteValue());

            return Value;
        }

        public bool ExistsPurchaseItem(PurchaseItem item)
            => _purchaseItems.Any(x => x.Id == item.Id);

        public void FinishPurchase() => State = PurchaseStateEnum.Closed;
    }
}
