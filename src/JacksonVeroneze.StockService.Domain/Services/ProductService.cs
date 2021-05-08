using System.Threading.Tasks;
using AutoMapper;
using JacksonVeroneze.StockService.AntiCorruption;
using JacksonVeroneze.StockService.Infra.Bus;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Product;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    /// <summary>
    ///
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        //private readonly IBusExternal _bus;


        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Method responsible for add item.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task AddAsync(Product product)
        {
            await _repository.AddAsync(product);

            if (await _repository.UnitOfWork.CommitAsync())
            {
                // await _bus.PublishEvent(_mapper.Map<ProductAddedEvent>(product));

                // await _mailService.SendAsync(new MailRequest()
                // {
                //     From = "jackson@jacksonveroneze.com",
                //     To = "jackson@jacksonveroneze.com",
                //     Subject = "Novo produto cadastrado",
                //     Text = product.Description
                // });
            }
        }
    }
}
