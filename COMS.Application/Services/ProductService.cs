using AutoMapper;
using COMS.Application.DTOs.Product;
using COMS.Application.Interfaces;
using COMS.Domain.Entities;
using COMS.Domain.Interfaces;
using Shared.Services;

namespace COMS.Application.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<IEnumerable<ProductDetailedDTO>>> GetAll()
        {
            var productsDb = await _productRepository.GetAll();

            if (productsDb != null)
            {
                var result = ServiceResult<IEnumerable<ProductDetailedDTO>>.SuccessResult(_mapper.Map<IEnumerable<ProductDetailedDTO>>(productsDb));

                return result;
            }

            return ServiceResult<IEnumerable<ProductDetailedDTO>>.ErrorResult("Ocorreu um erro ao carregar os dados dos produtos");
        }

        public async Task<ServiceResult<ProductDetailedDTO>> GetById(int productId)
        {
            var productDb = await _productRepository.GetById(productId);

            if (productDb != null)
            {
                var result = ServiceResult<ProductDetailedDTO>.SuccessResult(_mapper.Map<ProductDetailedDTO>(productDb));

                return result;
            }
            else
                return ServiceResult<ProductDetailedDTO>.ErrorResult("O produto não existe");

        }

        public async Task<ServiceResult<ProductDetailedDTO>> Add(ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);

            var productAdded = await _productRepository.Add(product);

            if (await _productRepository.SaveChangesAsync())
            {
                return ServiceResult<ProductDetailedDTO>.SuccessResult(_mapper.Map<ProductDetailedDTO>(productAdded));
            }

            return ServiceResult<ProductDetailedDTO>.ErrorResult("Ocorreu um erro ao criar o produto");
        }

        public async Task<ServiceResult<ProductDetailedDTO>> Update(int productId, ProductDTO productDTO)
        {
            var productDb = await _productRepository.GetById(productId);

            if (productDb == null)
                return ServiceResult<ProductDetailedDTO>.ErrorResult("O Produto não existe");

            productDb.Name = productDTO.Name;
            productDb.Description = productDTO.Description;
            productDb.Price = productDTO.Price;

            var productUpdated = await _productRepository.Update(productDb);

            if (await _productRepository.SaveChangesAsync())
            {
                return ServiceResult<ProductDetailedDTO>.SuccessResult(_mapper.Map<ProductDetailedDTO>(productUpdated));
            }

            return ServiceResult<ProductDetailedDTO>.ErrorResult("Ocorreu um erro ao atualizar o produto");
        }

        public async Task<ServiceResult<ProductDetailedDTO>> Remove(int productId)
        {
            var productDb = await _productRepository.GetById(productId);

            if (productDb == null)
                return ServiceResult<ProductDetailedDTO>.ErrorResult("O Produto não existe");

            await _productRepository.Remove(productId);

            if (await _productRepository.SaveChangesAsync())
            {
                return ServiceResult<ProductDetailedDTO>.SuccessResult(_mapper.Map<ProductDetailedDTO>(productDb));
            }

            return ServiceResult<ProductDetailedDTO>.ErrorResult("Ocorreu um erro ao remover o produto");
        }
    }
}
