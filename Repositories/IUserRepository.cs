using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    interface IUserRepository<T> where T : class
    {
        T Post(User user);
        T Post(int id,int idPuntoInteresse);
    }
}
