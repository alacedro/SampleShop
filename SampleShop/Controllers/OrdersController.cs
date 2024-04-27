using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;

using System;
using System.Collections.Generic;
using SampleShop.Interfaces;
using SampleShop.Model;

namespace SampleShop.Controllers
{
    // TODO
    // The action methods that use the new orders service methods have to be implemented. 
    // The desired URIs are specificed over each of the action methods. */
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService service;

        public OrdersController(IOrdersService service)
        {
            this.service = service;
        }

        // GET api/orders
        [FunctionName(nameof(GetAllOrders))]
        public IActionResult GetAllOrders([HttpTrigger("get", Route = "orders")]HttpRequest request)
        {
            var orders = service.All();
            return Ok(orders);
        }

        // GET api/orders/5
        [FunctionName(nameof(Get))]
        public IActionResult Get([HttpTrigger("get", Route = "orders/{id:int}")]HttpRequest request, int id)
        {
            var orders = service.GetById(id);
            return Ok(orders);
        }

        // GET api/orders/dates/?start=2018-01-03&end=2018-02-03
        [FunctionName(nameof(GetDates))]
        public IActionResult GetDates([HttpTrigger("get", Route = "orders/dates")]HttpRequest request)
        {
            DateTime startDate, endDate;

            DateTime.TryParse(request.Query["start"].ToString(), out startDate);
            DateTime.TryParse(request.Query["end"].ToString(), out endDate);

            var orders = service.GetByDates(startDate, endDate);
            return Ok(orders);
        }

        // GET api/orders/items/?day=2018-01-15
        [FunctionName(nameof(Getitems))]
        public IActionResult Getitems([HttpTrigger("get", Route = "orders/items")]HttpRequest request)
        {
            DateTime day;
            DateTime.TryParse(request.Query["day"].ToString(), out day);

            var items = service.GetItemsSoldByDay(day);
            return Ok(items);
        }

        // POST api/orders
        [FunctionName(nameof(Post))]
        public IActionResult Post([HttpTrigger("post", Route = "orders")]HttpRequest request, [FromBody] Order value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = service.Add(value);
            if (order != null)
            {
                return Ok(order);;
            }

            return BadRequest("Failed Validation");
        }

        // DELETE api/items/5
        [FunctionName(nameof(Remove))]
        public IActionResult Remove([HttpTrigger("delete", Route = "orders/{id}")]HttpRequest request, int id)
        {
            service.Delete(id);

            return Ok();
        }
    }
}
