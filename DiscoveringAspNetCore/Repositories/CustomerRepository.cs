using DiscoveringAsp.netCore.Entities;
using DiscoveringAsp.netCore.QueryParameters;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace DiscoveringAsp.netCore.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private DiscoveringAspnetCoreContext _context;

        public CustomerRepository(DiscoveringAspnetCoreContext context)
        {
            _context = context;
        }

        public IQueryable<Customer> GetAll(CustomerQueyParameters customerQueyParameters )
        {
            IQueryable<Customer> _allCustomers = _context.Customers.OrderBy(customerQueyParameters.OrderBy,customerQueyParameters.Descending);

            if (customerQueyParameters.HasQuery)
            {
                _allCustomers = _allCustomers.Where(x => x.FirstName.ToLowerInvariant().Contains( customerQueyParameters.Query.ToLowerInvariant()) ||
                x.LastName.ToLowerInvariant().Contains( customerQueyParameters.Query.ToLowerInvariant()));

            }

            return _allCustomers
                .Skip(customerQueyParameters.PageCount * (customerQueyParameters.Page - 1))
                .Take(customerQueyParameters.PageCount);
        }

        public Customer GetSingle(Guid id)
        {
            return _context.Customers.Where(x => x.Id == id).SingleOrDefault();
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public void Delete(Guid id)
        {
            var customer = GetSingle(id);

            _context.Customers.Remove(customer);
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public int Count()
        {
            return _context.Customers.Count();
        }

    }
}
