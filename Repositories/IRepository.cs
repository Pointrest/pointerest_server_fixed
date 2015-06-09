using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Post(int GestoreID, CreatePuntoInteresseCommand command);
        void Put(UpdatePuntoInteresseCommand udateCommand);
        void Delete(int id);
    }
}
