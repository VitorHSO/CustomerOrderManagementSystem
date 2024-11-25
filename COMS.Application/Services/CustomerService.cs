using AutoMapper;
using COMS.Application.DTOs.Customer;
using COMS.Application.DTOs.Transaction;
using COMS.Application.Interfaces;
using COMS.Domain.Entities;
using COMS.Domain.Interfaces;
using Shared.Services;

namespace COMS.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _transactionRepository = transactionRepository;
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

        public async Task<TransactionDTO> Add(CustomerDTO customerDTO)
        {
            var transaction = CreateTransaction("CustomerAdd", customerDTO);

            var customer = _mapper.Map<Customer>(customerDTO);
            await _customerRepository.Add(customer);

            if (await _customerRepository.SaveChangesAsync())
            {
                transaction.Status = "Success";
                transaction.Description = "Customer created successfully";
            }
            else
            {
                transaction.Status = "Error";
                transaction.Description = "The client was not created";
            }

            await SaveTransactionAsync(transaction);

            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<TransactionDTO> Update(int customerId, CustomerDTO customerDTO)
        {
            var transaction = CreateTransaction("CustomerUpdate", customerDTO);

            var customerDb = await _customerRepository.GetById(customerId);

            if (customerDb == null)
            {
                transaction.Status = "Error";
                transaction.Description = "The client does not exist";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            var customer = _mapper.Map<Customer>(customerDTO);
            customerDb.Name = customer.Name;
            customerDb.Email = customer.Email;
            customerDb.Phone = customer.Phone;

            var customerUpdated = await _customerRepository.Update(customerDb);

            if (await _customerRepository.SaveChangesAsync())
            {
                transaction.Status = "Success";
                transaction.Description = "Customer updated successfully";
            }
            else
            {
                transaction.Status = "Error";
                transaction.Description = "The customer was not updated";
            }

            await SaveTransactionAsync(transaction);

            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<TransactionDTO> Remove(int customerId)
        {
            var transaction = CreateTransaction("CustomerRemove", customerId);

            var customerDb = await _customerRepository.GetById(customerId);

            if (customerDb == null)
            {
                transaction.Status = "Error";
                transaction.Description = "The customer does not exist";
                await SaveTransactionAsync(transaction);

                return _mapper.Map<TransactionDTO>(transaction);
            }

            await _customerRepository.Remove(customerId);

            if (await _customerRepository.SaveChangesAsync())
            {
                transaction.Status = "Success";
                transaction.Description = "Customer removed successfully";
            }
            else
            {
                transaction.Status = "Error";
                transaction.Description = "The customer was not removed";
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
