using System;
using MediatR;

namespace JacksonVeroneze.StockService.Core.Messages
{
    public abstract class Command : Message, IRequest<bool>
    {
        public DateTime Timestamp { get; } = DateTime.Now;

        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}
