using AutoMapper;
using COMS.Application.DTOs.Customer;
using COMS.Application.DTOs.Order;
using COMS.Application.DTOs.OrderItem;
using COMS.Application.DTOs.Transaction;
using COMS.Application.Interfaces;
using COMS.Domain.Entities;
using COMS.Domain.Interfaces;
using Shared.Services;

namespace COMS.Application.Services
{
    public class OrderItemService : BaseService, IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, IProductRepository productRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<IEnumerable<OrderItemDetailedDTO>>> GetAll()
        {
            var orderitemsDb = await _orderItemRepository.GetAll();

            if (orderitemsDb != null)
            {
                var result = ServiceResult<IEnumerable<OrderItemDetailedDTO>>.SuccessResult(_mapper.Map<IEnumerable<OrderItemDetailedDTO>>(orderitemsDb));

                return result;
            }

            return ServiceResult<IEnumerable<OrderItemDetailedDTO>>.ErrorResult("Ocorreu um erro ao carregar os dados dos items de pedido");
        }

        public async Task<ServiceResult<OrderItemDetailedDTO>> GetById(int orderItemId)
        {
            var orderItemDb = await _orderItemRepository.GetById(orderItemId);

            if (orderItemDb != null)
            {
                var result = ServiceResult<OrderItemDetailedDTO>.SuccessResult(_mapper.Map<OrderItemDetailedDTO>(orderItemDb));

                return result;
            }
            else
                return ServiceResult<OrderItemDetailedDTO>.ErrorResult("O item do pedido não existe");
        }

        public async Task<TransactionDTO> Add(OrderItemDTO orderItemDTO)
        {
            var transaction = CreateTransaction("OrdemItemAdd", orderItemDTO);

            var orderItem = _mapper.Map<OrderItem>(orderItemDTO);

            await _orderItemRepository.Add(orderItem);

            if (await _orderItemRepository.SaveChangesAsync())
            {
                transaction.Status = "Success";
                transaction.Description = "Ordem item created successfully";
            }
            else
            {
                transaction.Status = "Error";
                transaction.Description = "The ordem item was not created";
            }

            await SaveTransactionAsync(transaction);

            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<TransactionDTO> Update(int orderItemId, OrderItemDTO orderItemDTO)
        {
            var transaction = CreateTransaction("OrderItemUpdate", orderItemDTO);

            var orderItemDb = await _orderItemRepository.GetById(orderItemId);

            if (orderItemDb == null)
            {
                transaction.Status = "Error";
                transaction.Description = "The ordem item does not exist";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            if (!await _orderRepository.Exists(orderItemDTO.OrderId))
            {
                transaction.Status = "Error";
                transaction.Description = "The ordem does not exist";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            if (!await _productRepository.Exists(orderItemDTO.ProductId))
            {
                transaction.Status = "Error";
                transaction.Description = "The product does not exist in the catalog";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            if (orderItemDTO.Quantity < 1)
            {
                transaction.Status = "Error";
                transaction.Description = "The minimum order item quantity cannot be less than 1";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            orderItemDb.OrderId = orderItemDTO.OrderId;
            orderItemDb.ProductId = orderItemDTO.ProductId;
            orderItemDb.Quantity = orderItemDTO.Quantity;

            await _orderItemRepository.Update(orderItemDb);

            if (await _orderItemRepository.SaveChangesAsync())
            {
                transaction.Status = "Success";
                transaction.Description = "Ordem item updated successfully";
            }
            else
            {
                transaction.Status = "Error";
                transaction.Description = "The ordem item was not updated";
            }

            await SaveTransactionAsync(transaction);

            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<TransactionDTO> Remove(int orderItemId)
        {
            var transaction = CreateTransaction("OrderItemRemove", orderItemId);

            var orderItemDb = await _orderItemRepository.GetById(orderItemId);

            if (orderItemDb == null)
            {
                transaction.Status = "Error";
                transaction.Description = "The ordem item does not exist";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            await _orderItemRepository.Remove(orderItemId);

            if (await _orderItemRepository.SaveChangesAsync())
            {
                transaction.Status = "Success";
                transaction.Description = "Ordem item removed successfully";
            }
            else
            {
                transaction.Status = "Error";
                transaction.Description = "The ordem item was not removed";
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
