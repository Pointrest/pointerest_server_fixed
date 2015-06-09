using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    interface IGestoreRepository<T> where T : class
    {
        T Get(int id);
        void Post(Gestore gestore);
        void Delete(int id);
    }
}
