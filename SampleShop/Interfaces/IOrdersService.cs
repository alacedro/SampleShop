using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using SampleShop.Model;

namespace SampleShop.Interfaces
{
    /// <summary>
    /// Interface for order's service
    /// </summary>
    public interface IOrdersService
    {
        IEnumerable<Order> All();
        // TODO: Something is missing here
        Order GetById(int id);
        Order Add(Order newOrder);
        void Delete(int id);
        IEnumerable<Order> GetByDates(DateTime start, DateTime end);
        IEnumerable<ItemSoldStatistics> GetItemsSoldByDay(DateTime day);
    }
}
