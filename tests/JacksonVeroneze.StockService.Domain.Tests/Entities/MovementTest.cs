using System.Collections.Generic;
using FluentAssertions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Domain.Entities;
using Xunit;
using UtilCommon = JacksonVeroneze.StockService.Common.Fakers.Util;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class MovementTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("Movement", "Movement")]
        public void Movement_Movement_DeveAdionarOItemCorretamente()
        {
            // Arange
            int totalItens = 10;

            Movement movement = MovementFaker.Generate().Generate();
            IList<MovementItem> itemsMock = MovementItemFaker.Generate(movement).Generate(totalItens);

            // Act
            foreach (MovementItem itemMock in itemsMock)
                movement.AddItem(itemMock);

            // Assert
            movement.Items.Should().HaveCount(totalItens);
        }
    }
}
