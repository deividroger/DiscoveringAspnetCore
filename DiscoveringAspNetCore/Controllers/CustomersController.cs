using AutoMapper;
using DiscoveringAsp.netCore.Dtos;
using DiscoveringAsp.netCore.Entities;
using DiscoveringAsp.netCore.QueryParameters;
using DiscoveringAsp.netCore.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscoveringAsp.netCore.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private ILogger<CustomersController> _logger;
        private ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository, ILogger<CustomersController> logger)
        {
            _customerRepository = customerRepository;

            _logger = logger;

            _logger.LogInformation("Customerscontroller started");
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Customer>), 200)]
        public IActionResult GetAllCustomers(CustomerQueyParameters customerQueyParameters)
        {
            _logger.LogInformation("-----> GetAllCustomers()");

            var allCustomer = _customerRepository.GetAll(customerQueyParameters).ToList();

            var allCustomerDto = allCustomer.Select(x => Mapper.Map<CustomerDto>(x));

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new { totalCount = _customerRepository.Count() }));

            return Ok(allCustomerDto);
        }

        [HttpGet("{id}", Name = "GetSingleCustomer")]
        public IActionResult GetSingleCustomer(Guid id)
        {
            _logger.LogInformation("-----> GetSingleCustomer()");

            var customer = _customerRepository.GetSingle(id);

            if (customer == null)
                return NotFound();

            return Ok(Mapper.Map<CustomerDto>(customer));

        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), 201)]
        [ProducesResponseType(typeof(CustomerCreateDto), 400)]
        public IActionResult AddCustomer([FromBody] CustomerCreateDto customerDto)
        {
            _logger.LogInformation("-----> AddCustomer()");

            if (customerDto == null) return BadRequest("customercreate object was null");

            if (!ModelState.IsValid) return BadRequest();

            var toadd = Mapper.Map<Customer>(customerDto);

            _customerRepository.Add(toadd);

            var result = _customerRepository.Save();

            if (!result) throw new Exception("something went wrong when add new customer");

            return CreatedAtRoute("GetSingleCustomer", new { id = toadd.Id }, Mapper.Map<CustomerDto>(toadd));

        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(CustomerDto), 200)]
        [ProducesResponseType(typeof(CustomerUpdateDto), 400)]
        public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerUpdateDto updateDto, [FromHeader] string algumaCoisa)
        {
            _logger.LogInformation("-----> UpdateCustomer()");

            if (updateDto == null) return BadRequest("customerupdate object was null");


            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
                return NotFound();

            if (!ModelState.IsValid) return BadRequest();

            Mapper.Map(updateDto, existingCustomer);

            _customerRepository.Update(existingCustomer);

            var result = _customerRepository.Save();

            if (!result) throw new Exception("something went wrong when add updating customer");


            return Ok(Mapper.Map<CustomerDto>(existingCustomer));

        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(typeof(CustomerDto), 200)]
        [ProducesResponseType(typeof(CustomerUpdateDto), 400)]
        public ActionResult PartiallyUpdate(Guid id, [FromBody] JsonPatchDocument<CustomerUpdateDto> customerPatchDoc)
        {
            _logger.LogInformation("-----> PartiallyUpdate()");

            if (customerPatchDoc == null)
                return BadRequest();

            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
                return NotFound();

            var customerToPatch = Mapper.Map<CustomerUpdateDto>(existingCustomer);

            customerPatchDoc.ApplyTo(customerToPatch, ModelState);

            TryValidateModel(customerToPatch);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            Mapper.Map(customerToPatch, existingCustomer);

            _customerRepository.Update(existingCustomer);


            var result = _customerRepository.Save();

            if (!result) throw new Exception("something went wrong when update customer");


            return Ok(Mapper.Map<CustomerDto>(existingCustomer));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Remove(Guid id)
        {
            _logger.LogInformation("-----> Remove()");

            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
                return NotFound();

            _customerRepository.Delete(id);

            var result = _customerRepository.Save();

            if (!result) throw new Exception("something went wrong when remove customer");

            return NoContent();

        }

    }
}
