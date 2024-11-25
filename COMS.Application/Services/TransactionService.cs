using AutoMapper;
using COMS.Application.DTOs.Transaction;
using COMS.Application.Interfaces;
using COMS.Domain.Interfaces;
using Shared.Services;

namespace COMS.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<IEnumerable<TransactionDTO>>> GetAll()
        {
            var transactionsDb = await _transactionRepository.GetAll();

            if (transactionsDb != null)
            {
                var result = ServiceResult<IEnumerable<TransactionDTO>>.SuccessResult(_mapper.Map<IEnumerable<TransactionDTO>>(transactionsDb));

                return result;
            }

            return ServiceResult<IEnumerable<TransactionDTO>>.ErrorResult("Ocorreu um erro ao carregar os dados das transações");
        }

        public async Task<ServiceResult<TransactionDetailedDTO>> GetById(int transactionId)
        {
            var transactionDb = await _transactionRepository.GetById(transactionId);

            if (transactionDb != null)
            {
                var result = ServiceResult<TransactionDetailedDTO>.SuccessResult(_mapper.Map<TransactionDetailedDTO>(transactionDb));

                return result;
            }
            else
                return ServiceResult<TransactionDetailedDTO>.ErrorResult("A transação não existe");
        }
    }
}
