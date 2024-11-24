using AutoMapper;
using COMS.Application.DTOs.Order;
using COMS.Application.DTOs.OrderItem;
using COMS.Application.Interfaces;
using COMS.Domain.Entities;
using COMS.Domain.Interfaces;
using Shared.Services;

namespace COMS.Application.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
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

        public async Task<ServiceResult<OrderDetailedDTO>> Add(OrderDTO orderDTO)
        {
            if (!await _customerRepository.Exists(orderDTO.CustomerId))
                return ServiceResult<OrderDetailedDTO>.ErrorResult("O cliente do pedido não existe");

            foreach (var orderItem in orderDTO.OrderItems)
            {
                if (!await _productRepository.Exists(orderItem.ProductId))
                    return ServiceResult<OrderDetailedDTO>.ErrorResult("O pedido possui um produto que não existe no catálogo");
            }

            var order = _mapper.Map<Order>(orderDTO);

            await _orderRepository.Add(order);

            var orderAdded = await _orderRepository.GetById(order.Id);

            //if (await _orderRepository.SaveChangesAsync())
            //{
                return ServiceResult<OrderDetailedDTO>.SuccessResult(_mapper.Map<OrderDetailedDTO>(orderAdded));
            //}

            //return ServiceResult<OrderDetailedDTO>.ErrorResult("Ocorreu um erro ao criar o pedido");
        }

        public async Task<ServiceResult<OrderDetailedDTO>> Update(int orderId, OrderDTO orderDTO)
        {
            var orderDb = await _orderRepository.GetById(orderId);

            if (orderDb == null)
                return ServiceResult<OrderDetailedDTO>.ErrorResult("O pedido não existe");

            if (!await _customerRepository.Exists(orderDTO.CustomerId))
                return ServiceResult<OrderDetailedDTO>.ErrorResult("O cliente do pedido não existe");

            foreach (var orderItem in orderDTO.OrderItems)
            {
                if (!await _productRepository.Exists(orderItem.ProductId))
                    return ServiceResult<OrderDetailedDTO>.ErrorResult("O pedido possui um produto que não existe no catálogo");

                if (orderItem.Quantity < 1)
                    return ServiceResult<OrderDetailedDTO>.ErrorResult("A quantidade mínima do item do pedido não pode ser inferior há 1");
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

            var orderUpdated = await _orderRepository.GetById(orderId);

            //if (await _orderRepository.SaveChangesAsync())
            //{
                return ServiceResult<OrderDetailedDTO>.SuccessResult(_mapper.Map<OrderDetailedDTO>(orderUpdated));
            //}

            //return ServiceResult<OrderDetailedDTO>.ErrorResult("Ocorreu um erro ao atualizar o pedido");
        }

        public async Task<ServiceResult<OrderDetailedDTO>> Remove(int orderId)
        {
            var orderDb = await _orderRepository.GetById(orderId);

            if (orderDb == null)
                return ServiceResult<OrderDetailedDTO>.ErrorResult("O pedido não existe");

            await _orderRepository.Remove(orderId);

            if (await _orderRepository.SaveChangesAsync())
            {
                return ServiceResult<OrderDetailedDTO>.SuccessResult(_mapper.Map<OrderDetailedDTO>(orderDb));
            }

            return ServiceResult<OrderDetailedDTO>.ErrorResult("Ocorreu um erro ao remover o pedido");
        }
    }
}
