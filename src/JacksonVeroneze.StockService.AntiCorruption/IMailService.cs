using System.Threading.Tasks;
using Refit;

namespace JacksonVeroneze.StockService.AntiCorruption
{
    [Headers(new[] {"Accept: application/json;charset=UTF-8"})]
    public interface IMailService
    {
        [Post("/api/v1/mail")]
        Task<MailResponse> SendAsync(MailRequest request);
    }
}
