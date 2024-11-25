using AutoMapper;
using COMS.Application.DTOs.Product;
using COMS.Application.DTOs.Transaction;
using COMS.Application.Interfaces;
using COMS.Domain.Entities;
using COMS.Domain.Interfaces;
using Shared.Services;

namespace COMS.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _transactionRepository = transactionRepository;
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

        public async Task<TransactionDTO> Add(ProductDTO productDTO)
        {
            var transaction = CreateTransaction("ProductAdd", productDTO);

            var product = _mapper.Map<Product>(productDTO);
            var productAdded = await _productRepository.Add(product);

            if (await _productRepository.SaveChangesAsync())
            {
                transaction.Status = "Success";
                transaction.Description = "Product created successfully";
            }
            else
            {
                transaction.Status = "Error";
                transaction.Description = "The product was not created";
            }

            await SaveTransactionAsync(transaction);

            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<TransactionDTO> Update(int productId, ProductDTO productDTO)
        {
            var transaction = CreateTransaction("ProductUpdate", productDTO);

            var productDb = await _productRepository.GetById(productId);

            if (productDb == null)
            {
                transaction.Status = "Error";
                transaction.Description = "The product does not exist";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            productDb.Name = productDTO.Name;
            productDb.Description = productDTO.Description;
            productDb.Price = productDTO.Price;

            var productUpdated = await _productRepository.Update(productDb);

            if (await _productRepository.SaveChangesAsync())
            {
                transaction.Status = "Success";
                transaction.Description = "Product updated successfully";
            }
            else
            {
                transaction.Status = "Error";
                transaction.Description = "The product was not updated";
            }

            await SaveTransactionAsync(transaction);

            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<TransactionDTO> Remove(int productId)
        {
            var transaction = CreateTransaction("ProductRemove", productId);

            var productDb = await _productRepository.GetById(productId);

            if (productDb == null)
            {
                transaction.Status = "Error";
                transaction.Description = "The product does not exist";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            await _productRepository.Remove(productId);

            if (await _productRepository.SaveChangesAsync())
            {
                transaction.Status = "Success";
                transaction.Description = "Product removed successfully";
            }
            else
            {
                transaction.Status = "Error";
                transaction.Description = "The product was not removed";
            }

            await SaveTransactionAsync(transaction);

            return _mapper.Map<TransactionDTO>(transaction);
        }

        private Transaction CreateTransaction(string actionName, object requestData)
        {
            var transaction = new Transaction
            {
                Name = actionName
            };
            transaction.SetModelRequest(requestData);
            return transaction;
        }

        private async Task SaveTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.Add(transaction);
        }
    }
}
