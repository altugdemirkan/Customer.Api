using Customer.Domain.Requests.Customer;
using Customer.Facade.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerFacade customerFacade;

        public CustomerController(ICustomerFacade customerFacade)
        {
            this.customerFacade = customerFacade;
        }

        [HttpPost]
        public async Task<IActionResult> Insert(CustomerInsertRequest customerInsertRequest)
        {
            var response = await customerFacade.Insert(customerInsertRequest);
            return Ok(response);
        }
            
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await customerFacade.GetAll(new CustomerGetAllRequest());
            if (!response.isSuccess)
            {
                return NotFound(response.Messages);
            }
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request = new CustomerGetRequest { id = id };
            var response = await customerFacade.Get(request);
            if (!response.isSuccess)
            {
                return NotFound(response.Messages);
            }
            return Ok(response);
        }
        [HttpGet("/nationality/{nationality}")]
        public async Task<IActionResult> GetByNationality(string nationality)
        {
            var request = new CustomerGetByNationalityRequest { nationality = nationality };
            var response = await customerFacade.GetByNationality(request);
            if (!response.isSuccess)
            {
                return NotFound(response.Messages);
            }
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new CustomerDeleteRequest { id = id };
            var response = await customerFacade.Delete(request);
            if (!response.isSuccess)
            {
                return NotFound(response.Messages);
            }
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> Update(CustomerUpdateRequest customerUpdateRequest)
        {
            var response = await customerFacade.Update(customerUpdateRequest);
            if (!response.isSuccess)
            {
                return NotFound(response.Messages);
            }
            return Ok(response);
        }
    }
}
