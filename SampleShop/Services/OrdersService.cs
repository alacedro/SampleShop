using System;
using System.Collections.Generic;
using System.Linq;
using SampleShop.Interfaces;
using SampleShop.Model;
using SampleShop.Queries;

namespace SampleShop.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly GetAllOrdersQuery queryAll;
        private readonly GetAllItemsQuery queryAllItems;
        private readonly GetOrderByIdQuery queryById;
        private readonly AddOrderQuery queryAdd;
        private readonly DeleteOrderQuery queryDelete;


        public OrdersService(GetAllOrdersQuery queryAllOrders, GetAllItemsQuery queryAllItems,
                             GetOrderByIdQuery queryById, AddOrderQuery queryAdd,
                             DeleteOrderQuery queryDelete)
        {
            this.queryAll = queryAllOrders;
            this.queryAllItems = queryAllItems;
            this.queryById = queryById;
            this.queryAdd = queryAdd;
            this.queryDelete = queryDelete;
        }

        /// <summary>
        /// Lists all orders that exist in db
        /// </summary>
        public IEnumerable<Order> All()
        {
            return queryAll.Execute().ToList();
        }

        /// <summary>
        /// Gets single order by its id
        /// </summary>
        public Order GetById(int id)
        {
            return queryById.Execute(id);
        }

        /// <summary>
        /// Tries to add given order to db, after validating it
        /// </summary>
        public Order Add(Order newOrder)
        {
            if (newOrder == null)
            {
                throw new ArgumentNullException("newOrder");
            }

            var result = ValidateNewOrder(newOrder);
            if ((result & ValidationResult.Ok) == ValidationResult.Ok)
            {
                queryAdd.Execute(newOrder);
                return newOrder;
            }

            return null;
        }

        /// <summary>
        /// Checks whether given order can be added.
        /// Performs logical and business validation.
        /// </summary>
        public ValidationResult ValidateNewOrder(Order newOrder)
        {
            var result = ValidationResult.Default;

            if (newOrder == null)
            {
                throw new ArgumentNullException("newOrder");
            }

            var items = queryAllItems.Execute();

            foreach (var item in newOrder.OrderItems)
            {
                if (item.Value <= 0) result |= ValidationResult.NoItemQuantity;
                if (!items.Any(p => p.Id == item.Key))
                {
                    result |= ValidationResult.ItemDoesNotExist;
                }
            }

            if (result == ValidationResult.Default)
            {
                result = ValidationResult.Ok;
            }

            return result;
        }

        /// <summary>
        /// Deletes (if exists) order from db (by its id)
        /// </summary>
        public void Delete(int id)
        {
            queryDelete.Execute(id);
        }

        /// <summary>
        /// Returns all orders (listed chronologically) between a given start date and end date.
        /// Start and end dates must be from the past (not in the future or today).
        /// </summary>
        public IEnumerable<Order> GetByDates(DateTime start, DateTime end)
        {
            if (start.Date >= DateTime.Today.Date || end.Date >= DateTime.Today.Date)
            {
                throw new ArgumentException("Dates must be in the past");
            }

            var orders = queryAll.Execute();

            var filteredOrders = orders.Where(x => x.CreateDate.Date >= start.Date && x.CreateDate.Date <= end.Date);

            return filteredOrders.AsEnumerable<Order>();

        }

        /// <summary>
        /// Returns the list of items sold across all orders in a given day with
        /// the total revenue
        /// Day must be from the past (not in the future or today).
        /// </summary>
        public IEnumerable<ItemSoldStatistics> GetItemsSoldByDay(DateTime day)
        {
            if (day.Date >= DateTime.Today.Date)
            {
                throw new ArgumentException("Date must be in the past");
            }

            var itemsInOrders = queryAll.Execute()
                .Where(x => x.CreateDate == day.Date)
                .Select(x => x.OrderItems);


            var counts = from e in itemsInOrders
                         from i in e
                         group i by i.Key into g
                         select new { ItemID = g.Key, Count = g.Sum(x => x.Value) };


            var totals = from di in queryAllItems.Execute()
                         from c in counts
                         where di.Id == c.ItemID
                         select new ItemSoldStatistics()
                         {
                             ItemId = di.Id,
                             Total = c.Count * di.Price
                         };

            return totals;
        }
    }
}
