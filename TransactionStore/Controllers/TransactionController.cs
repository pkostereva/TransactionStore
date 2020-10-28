using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TransactionStore.API.Models.InputModels;
using TransactionStore.API.Models.OutputModels;
using TransactionStore.DB.Models;
using TransactionStore.Repository;

namespace TransactionStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        public TransactionController(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        [HttpPost("deposit")]
        public async ValueTask<ActionResult<TransactionEntityOutputModel>> Deposit(TransactionEntityInputModel inputModel)
        {
            if (inputModel.TimeStamp == null) return BadRequest("TimeStamp should not be null!");
            if (inputModel.Amount < 0) { return BadRequest("Deposit amount must be more than zero!"); }
            var result = await _transactionRepository.DepositOrWithdraw(_mapper.Map<TransactionEntity>(inputModel));
            if (result.IsOkay)
            {
                return Ok(_mapper.Map<TransactionEntityOutputModel>(result.RequestData));
            }
            if (result.exMessage == "A timestamp constraint violation occurred.")
                return Conflict($"Transaction failed: {result.exMessage}");
            return Problem($"Transaction failed: {result.exMessage}", statusCode: 520);
        }

        [HttpPost("withdraw")]
        public async ValueTask<ActionResult<TransactionEntityOutputModel>> Withdraw(TransactionEntityInputModel inputModel)
        {
            if (inputModel.TimeStamp == null) return BadRequest("TimeStamp should not be null!");
            if (inputModel.Amount < 0) { return BadRequest("Withdraw amount must be more than zero!"); }
            inputModel.Amount = -inputModel.Amount;
            var result = await _transactionRepository.DepositOrWithdraw(_mapper.Map<TransactionEntity>(inputModel));
            if (result.IsOkay)
            {
                return Ok(_mapper.Map<TransactionEntityOutputModel>(result.RequestData));
            }
            if (result.exMessage == "A timestamp constraint violation occurred.")
                return Conflict($"Transaction failed: {result.exMessage}");
            return Problem($"Transaction failed {result.exMessage}", statusCode: 520);
        }

        [HttpPost("transfer")]
        public async ValueTask<ActionResult<TransactionEntityOutputModel>> Transfer(TransferInputModel inputModel)
        {
            if (inputModel.SenderTimeStamp==null&&inputModel.RecipientTimeStamp==null) return BadRequest("TimeStamp should not be null!");
            if (inputModel.RecipientAccount.Id < 1 || inputModel.SenderAccount.Id < 1) { return BadRequest("Incorrect AccountId"); }
            if (inputModel.Amount < 0) { return BadRequest("Deposit amount must be more than zero!"); }
            var dataModels = _mapper.Map<TransferEntities>(inputModel);
            var result = await _transactionRepository.Transfer(dataModels);
            if (result.IsOkay)
            {
                return Ok(_mapper.Map<List<TransactionEntityOutputModel>>(result.RequestData));
            }
            if (result.exMessage == "A timestamp constraint violation occurred.")
                return Conflict($"Transaction failed: {result.exMessage}");
            return Problem($"Transaction failed {result.exMessage}", statusCode: 520);
        }

        [HttpGet("{id}")]
        public async ValueTask<ActionResult<TransactionEntityOutputModel>> GetTransactionById(int id)
        {
            if (id < 1) { return BadRequest("Incorrect id"); }
            var result = await _transactionRepository.GetTransactionById(id);
            if (result.IsOkay)
            {
                if (result.RequestData == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<TransactionEntityOutputModel>(result.RequestData));
            }
            return Problem($"Request failed {result.exMessage}", statusCode: 520);
        }

        [HttpGet]
        public async ValueTask<ActionResult<List<TransactionEntityOutputModel>>> GetAllTransactions()
        {
            var result = await _transactionRepository.GetAllTransactions();
            if (result.IsOkay)
            {
                if (result.RequestData == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<List<TransactionEntityOutputModel>>(result.RequestData));
            }
            return Problem($"Request failed {result.exMessage}", statusCode: 520);
        }

        [HttpGet("balance/{accountId}")]
        public async ValueTask<ActionResult<AccountBalanceOutputModel>> GetAccountBalance(int accountId)
        {
            var result = await _transactionRepository.GetAccountBalance(accountId);
            if (result.IsOkay)
            {
                return Ok(_mapper.Map<AccountBalanceOutputModel>(result.RequestData));
            }
            return Problem($"Request failed {result.exMessage}", statusCode: 520);
        }

        [HttpPost("initialize")]
        public async ValueTask<ActionResult<TransactionEntityOutputModel>> Initialize(TransactionEntityInputModel inputModel)
        {
            var result = await _transactionRepository.InitializeTransaction(_mapper.Map<TransactionEntity>(inputModel));
            if (result.IsOkay)
            {
                return Ok(_mapper.Map<TransactionEntityOutputModel>(result.RequestData));
            }
            return Problem($"Transaction failed: {result.exMessage}", statusCode: 520);
        }
    }
}