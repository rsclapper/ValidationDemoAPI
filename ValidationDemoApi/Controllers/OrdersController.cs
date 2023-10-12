﻿using Microsoft.AspNetCore.Mvc;
using ValidationDemoApi.CORE.Interfaces;
using ValidationDemoApi.CORE.Models;
using ValidationDemoApi.Mappers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ValidationDemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRepository<Order> _orderRepo;

        public OrdersController(IRepository<Order> orderRepo)
        {
            _orderRepo = orderRepo;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public IActionResult Get()
        {
            var orders = _orderRepo.GetAll();

            return Ok(orders.Select(o => o.MapToDto()).ToList());
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}", Name="GetOrder")]
        public IActionResult GetById(int id)
        {
            var order = _orderRepo.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order.MapToDto());
            
        }
        // GET api/Contacts/{contactId}/Orders
        [HttpGet, Route("/api/Contacts/{contactId}/Orders")]
        public IActionResult GetByContactId(int contactId)
        {
            var orders = _orderRepo.GetAll(o => o.ContactId == contactId);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders.Select(o => o.MapToDto()).ToList());
        }

        // POST api/<OrdersController>
        [HttpPost]
        public IActionResult Post([FromBody] OrderDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = value.MapToEntity();
            _orderRepo.Add(order);
            return CreatedAtRoute(nameof(GetById), new { id = order.Id }, order.MapToDto());
            
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] OrderDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = _orderRepo.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            order.ContactId = value.ContactId;
            order.OrderDate = value.OrderDate;
            order.Total = value.Total;
            _orderRepo.Update(order);
            return NoContent();
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = _orderRepo.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            _orderRepo.Delete(order);
            return NoContent();
        }


    }
}
