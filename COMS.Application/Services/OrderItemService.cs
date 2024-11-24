using AutoMapper;
using COMS.Application.DTOs.Order;
using COMS.Application.DTOs.OrderItem;
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
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
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

        public async Task<ServiceResult<OrderItemDetailedDTO>> Add(OrderItemDTO orderitemDTO)
        {
            var orderitem = _mapper.Map<OrderItem>(orderitemDTO);

            await _orderItemRepository.Add(orderitem);

            var orderitemAdded = await _orderItemRepository.GetById(orderitem.Id);
            //if (await _orderItemRepository.SaveChangesAsync())
            //{
                return ServiceResult<OrderItemDetailedDTO>.SuccessResult(_mapper.Map<OrderItemDetailedDTO>(orderitemAdded));
            //}

            //return ServiceResult<OrderItemDetailedDTO>.ErrorResult("Ocorreu um erro ao criar o item do pedido");
        }

        public async Task<ServiceResult<OrderItemDetailedDTO>> Update(int orderItemId, OrderItemDTO orderItemDTO)
        {
            var orderItemDb = await _orderItemRepository.GetById(orderItemId);

            if (orderItemDb == null)
                return ServiceResult<OrderItemDetailedDTO>.ErrorResult("O item do pedido não existe");

            if (!await _orderRepository.Exists(orderItemDTO.OrderId))
                return ServiceResult<OrderItemDetailedDTO>.ErrorResult("O pedido não existe");

            if (!await _productRepository.Exists(orderItemDTO.ProductId))
                return ServiceResult<OrderItemDetailedDTO>.ErrorResult("O produto não existe no catálogo");

            if (orderItemDTO.Quantity < 1)
                return ServiceResult<OrderItemDetailedDTO>.ErrorResult("A quantidade mínima do item do pedido não pode ser inferior há 1");

            orderItemDb.OrderId = orderItemDTO.OrderId;
            orderItemDb.ProductId = orderItemDTO.ProductId;
            orderItemDb.Quantity = orderItemDTO.Quantity;

            await _orderItemRepository.Update(orderItemDb);
            var orderitemUpdated = await _orderItemRepository.GetById(orderItemId);

            return ServiceResult<OrderItemDetailedDTO>.SuccessResult(_mapper.Map<OrderItemDetailedDTO>(orderitemUpdated));
        }

        public async Task<ServiceResult<OrderItemDetailedDTO>> Remove(int orderItemId)
        {
            var orderItemDb = await _orderItemRepository.GetById(orderItemId);

            if (orderItemDb == null)
                return ServiceResult<OrderItemDetailedDTO>.ErrorResult("O item do pedido não existe");

            await _orderItemRepository.Remove(orderItemId);

            if (await _orderItemRepository.SaveChangesAsync())
            {
                return ServiceResult<OrderItemDetailedDTO>.SuccessResult(_mapper.Map<OrderItemDetailedDTO>(orderItemDb));
            }

            return ServiceResult<OrderItemDetailedDTO>.ErrorResult("Ocorreu um erro ao remover o item do pedido");
        }
    }
}
