using AutoMapper;
using COMS.Application.DTOs.Customer;
using COMS.Application.Interfaces;
using COMS.Domain.Entities;
using COMS.Domain.Interfaces;
using Shared.Services;

namespace COMS.Application.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }       

        public async Task<ServiceResult<IEnumerable<CustomerDetailedDTO>>> GetAll()
        {
            var customersDb = await _customerRepository.GetAll();

            if (customersDb != null)
            {
                var result = ServiceResult<IEnumerable<CustomerDetailedDTO>>.SuccessResult(_mapper.Map<IEnumerable<CustomerDetailedDTO>>(customersDb));

                return result;
            }

            return ServiceResult<IEnumerable<CustomerDetailedDTO>>.ErrorResult("Ocorreu um erro ao carregar os dados dos clientes");
        }

        public async Task<ServiceResult<CustomerDetailedDTO>> GetById(int customerId)
        {
            var customerDb = await _customerRepository.GetById(customerId);

            if (customerDb != null)
            {
                var result = ServiceResult<CustomerDetailedDTO>.SuccessResult(_mapper.Map<CustomerDetailedDTO>(customerDb));

                return result;
            }
            else
                return ServiceResult<CustomerDetailedDTO>.ErrorResult("O cliente não existe");

        }

        public async Task<ServiceResult<CustomerDetailedDTO>> Add(CustomerDTO customerDTO)
        {
            var customer = _mapper.Map<Customer>(customerDTO);

            var customerAdded = await _customerRepository.Add(customer);

            if (await _customerRepository.SaveChangesAsync())
            {
                return ServiceResult<CustomerDetailedDTO>.SuccessResult(_mapper.Map<CustomerDetailedDTO>(customerAdded));
            }

            return ServiceResult<CustomerDetailedDTO>.ErrorResult("Ocorreu um erro ao criar o cliente");
        }

        public async Task<ServiceResult<CustomerDetailedDTO>> Update(int customerId, CustomerDTO customerDTO)
        {
            // Vai dar erro porque o Encript4 vai ser diferente na hora de decriptografar, tem que ver como corrigir
            var customerDb = await _customerRepository.GetById(customerId);

            if (customerDb == null)
                return ServiceResult<CustomerDetailedDTO>.ErrorResult("O cliente não existe");

            var customer = _mapper.Map<Customer>(customerDTO);
            customerDb.Name = customer.Name;
            customerDb.Email = customer.Email;
            customerDb.Phone = customer.Phone;

            var customerUpdated = await _customerRepository.Update(customerDb);

            if (await _customerRepository.SaveChangesAsync())
            {
                return ServiceResult<CustomerDetailedDTO>.SuccessResult(_mapper.Map<CustomerDetailedDTO>(customerUpdated));
            }

            return ServiceResult<CustomerDetailedDTO>.ErrorResult("Ocorreu um erro ao atualizar o cliente");
        }

        public async Task<ServiceResult<CustomerDetailedDTO>> Remove(int customerId)
        {
            var customerDb = await _customerRepository.GetById(customerId);

            if (customerDb == null)
                return ServiceResult<CustomerDetailedDTO>.ErrorResult("O cliente não existe");

            await _customerRepository.Remove(customerId);

            if (await _customerRepository.SaveChangesAsync())
            {
                return ServiceResult<CustomerDetailedDTO>.SuccessResult(_mapper.Map<CustomerDetailedDTO>(customerDb));
            }

            return ServiceResult<CustomerDetailedDTO>.ErrorResult("Ocorreu um erro ao remover o cliente");
        }
    }
}
