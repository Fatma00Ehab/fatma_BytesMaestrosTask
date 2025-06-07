using backend.DataAccess.IRepository;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DataAccess
{
    public class ProductRepository : DataRepository<Product>, IProductRepository
    {
        public ProductRepository(DataContext context) : base(context)
        {
        }
    }
}


