using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.Core.Services;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IPaymentService _service;

        public PaymentsController(IMapper mapper, IPaymentService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _service.GetAllAsync();
            var paymentDtos = _mapper.Map<List<PaymentDto>>(payments.ToList());
            return CreateActionResult(ResponseDto<List<PaymentDto>>.Success(200, paymentDtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment= await _service.GetByIdAsync(id);
            var paymentDto = _mapper.Map<PaymentDto>(payment);
            return CreateActionResult(ResponseDto<PaymentDto>.Success(200, paymentDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(PaymentDto paymentDto)
        {
            var payment = await _service.AddAsync(_mapper.Map<Payment>(paymentDto));
            var paymentDtos = _mapper.Map<PaymentDto>(payment);
            return CreateActionResult(ResponseDto<PaymentDto>.Success(201, paymentDtos));
        }

        [HttpPut]
        public async Task<IActionResult> Update(PaymentDto paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);
            await _service.UpdateAsync(payment);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var payment = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(payment);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
    }
}
