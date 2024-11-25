using COMS.Application.DTOs.Product;
using COMS.Application.DTOs.Transaction;
using Shared.Services;

namespace COMS.Application.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<IEnumerable<ProductDetailedDTO>>> GetAll();
        Task<ServiceResult<ProductDetailedDTO>> GetById(int productId);
        Task<TransactionDTO> Add(ProductDTO productDTO);
        Task<TransactionDTO> Update(int productId, ProductDTO productDTO);
        Task<TransactionDTO> Remove(int productId);
    }
}
