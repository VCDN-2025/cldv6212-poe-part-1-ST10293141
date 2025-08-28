using System.Collections.Generic;

namespace ABCRETAIL.Models
{

    public class TablesViewModel

    {

        public List<CustomerEntity> Customers { get; set; } = new();

        public List<ProductEntity> Products { get; set; } = new();

    }

}
