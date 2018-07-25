using System;

namespace DiscoveringAsp.netCore.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

    }
}
