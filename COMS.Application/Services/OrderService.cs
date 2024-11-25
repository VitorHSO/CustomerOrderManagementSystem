using AutoMapper;
using COMS.Application.DTOs.Order;
using COMS.Application.DTOs.Transaction;
using COMS.Application.Interfaces;
using COMS.Domain.Entities;
using COMS.Domain.Interfaces;
using Shared.Services;

namespace COMS.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IProductRepository productRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<IEnumerable<OrderDetailedDTO>>> GetAll()
        {
            var ordersDb = await _orderRepository.GetAll();

            if (ordersDb != null)
            {
                var result = ServiceResult<IEnumerable<OrderDetailedDTO>>.SuccessResult(_mapper.Map<IEnumerable<OrderDetailedDTO>>(ordersDb));

                return result;
            }

            return ServiceResult<IEnumerable<OrderDetailedDTO>>.ErrorResult("Ocorreu um erro ao carregar os dados dos pedidos");
        }

        public async Task<ServiceResult<OrderDetailedDTO>> GetById(int orderId)
        {
            var orderDb = await _orderRepository.GetById(orderId);

            if (orderDb != null)
            {
                var result = ServiceResult<OrderDetailedDTO>.SuccessResult(_mapper.Map<OrderDetailedDTO>(orderDb));

                return result;
            }
            else
                return ServiceResult<OrderDetailedDTO>.ErrorResult("O pedido não existe");
        }

        public async Task<TransactionDTO> Add(OrderDTO orderDTO)
        {
            var transaction = CreateTransaction("OrderAdd", orderDTO);

            if (!await _customerRepository.Exists(orderDTO.CustomerId))
            {
                transaction.Status = "Error";
                transaction.Description = "The order customer does not exist";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            foreach (var orderItem in orderDTO.OrderItems)
            {
                if (!await _productRepository.Exists(orderItem.ProductId))
                {
                    transaction.Status = "Error";
                    transaction.Description = "The product does not exist in the catalog";
                    await SaveTransactionAsync(transaction);

                    return _mapper.Map<TransactionDTO>(transaction);
                }
            }

            var order = _mapper.Map<Order>(orderDTO);

            await _orderRepository.Add(order);

            if (await _orderRepository.SaveChangesAsync())
            {
                transaction.Status = "Success";
                transaction.Description = "Ordem created successfully";
            }
            else
            {
                transaction.Status = "Error";
                transaction.Description = "The ordem was not created";
            }

            await SaveTransactionAsync(transaction);

            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<TransactionDTO> Update(int orderId, OrderDTO orderDTO)
        {
            var transaction = CreateTransaction("OrderUpdate", orderDTO);

            var orderDb = await _orderRepository.GetById(orderId);

            if (orderDb == null)
            {
                transaction.Status = "Error";
                transaction.Description = "The ordem does not exist";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            if (!await _customerRepository.Exists(orderDTO.CustomerId))
            {
                transaction.Status = "Error";
                transaction.Description = "The order customer does not exist";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            foreach (var orderItem in orderDTO.OrderItems)
            {
                if (!await _productRepository.Exists(orderItem.ProductId))
                {
                    transaction.Status = "Error";
                    transaction.Description = "The product does not exist in the catalog";
                    await SaveTransactionAsync(transaction);

                    return _mapper.Map<TransactionDTO>(transaction);
                }

                if (orderItem.Quantity < 1)
                {
                    transaction.Status = "Error";
                    transaction.Description = "The minimum order item quantity cannot be less than 1";
                    await SaveTransactionAsync(transaction);

                    return _mapper.Map<TransactionDTO>(transaction);
                }
            }

            var order = _mapper.Map<Order>(orderDTO);

            var ordemItemToAdd = order.OrderItems.Where(m => !orderDb.OrderItems.Select(b => b.ProductId).Contains(m.ProductId)).ToList();
            var ordemItemToRemove = orderDb.OrderItems.Where(b => !order.OrderItems.Select(m => m.ProductId).Contains(b.ProductId)).ToList();

            foreach (var remove in ordemItemToRemove)
            {
                orderDb.OrderItems.Remove(remove);
            }
            foreach (var memberAdd in ordemItemToAdd)
            {
                orderDb.OrderItems.Add(memberAdd);
            }

            await _orderRepository.Update(orderDb);

            if (await _orderRepository.SaveChangesAsync())
            {
                transaction.Status = "Success";
                transaction.Description = "Ordem updated successfully";
            }
            else
            {
                transaction.Status = "Error";
                transaction.Description = "The ordem was not updated";
            }

            await SaveTransactionAsync(transaction);

            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<TransactionDTO> Remove(int orderId)
        {
            var transaction = CreateTransaction("OrderRemove", orderId);

            var orderDb = await _orderRepository.GetById(orderId);

            if (orderDb == null)
            {
                transaction.Status = "Error";
                transaction.Description = "The ordem does not exist";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            await _orderRepository.Remove(orderId);

            if (await _orderRepository.SaveChangesAsync())
            {
                transaction.Status = "Success";
                transaction.Description = "Ordem removed successfully";
            }
            else
            {
                transaction.Status = "Error";
                transaction.Description = "The ordem was not removed";
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
