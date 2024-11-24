using COMS.Application.DTOs.Product;
using Shared.Services;

namespace COMS.Application.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<IEnumerable<ProductDetailedDTO>>> GetAll();
        Task<ServiceResult<ProductDetailedDTO>> GetById(int productId);
        Task<ServiceResult<ProductDetailedDTO>> Add(ProductDTO productDTO);
        Task<ServiceResult<ProductDetailedDTO>> Update(int productId, ProductDTO productDTO);
        Task<ServiceResult<ProductDetailedDTO>> Remove(int productId);
    }
}
