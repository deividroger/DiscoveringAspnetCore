using System;
using System.Linq;
using DiscoveringAsp.netCore.Entities;
using DiscoveringAsp.netCore.QueryParameters;

namespace DiscoveringAsp.netCore.Repositories
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        void Delete(Guid id);
        IQueryable<Customer> GetAll(CustomerQueyParameters customerQueyParameters);
        Customer GetSingle(Guid id);
        bool Save();
        void Update(Customer customer);

        int Count();
    }
}