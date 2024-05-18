using Application.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IUnitOfWork
    {
        //public IProductRepo ProductRepo { get;}
        public IUserRepo UserRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
