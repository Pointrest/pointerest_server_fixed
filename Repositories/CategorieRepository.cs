using DTO;
using Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategorieRepository
    {
        string mConnectionString;

        public CategorieRepository()
            : this("mConnectionString")
        {

        }

        public CategorieRepository(string connectionString)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionString];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionString));

            else mConnectionString = cs.ConnectionString;
        }

       
         public Categoria Get(int id){
             
            Categoria categoria = null;
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"SELECT * from Categorie "
                                + " WHERE Categorie.ID = " + id;

                SqlTransaction transaction;
                using (var command = new System.Data.SqlClient.SqlCommand(query, connection, transaction = connection.BeginTransaction()))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categoria = new Categoria();

                            categoria.ID = reader.GetValue<int>("ID");
                            categoria.CategoryName = reader.GetValue<string>("CategoryName");
                  
                        }
                    }
                }
                transaction.Commit();
                connection.Close();
            }
            return categoria;

        }


         public IEnumerable<Categoria> GetAll()
         {
             List<Categoria> categorie = new List<Categoria>();

             using (var connection = new SqlConnection(mConnectionString))
             {
                 connection.Open();

                 string query = @"SELECT * from Categorie";

                 using (var command = new SqlCommand(query, connection))
                 {
                     using (SqlDataReader reader = command.ExecuteReader())
                     {
                         Categoria tmp = null;

                         while (reader.Read()){

                                 tmp = new Categoria();
                                 tmp.ID = reader.GetValue<int>("ID");
                                 tmp.CategoryName = reader.GetValue<string>("CategoryName");
                                 
                                 categorie.Add(tmp);
                         }
                     }
                 }
             }
             return categorie;
         }
    }
}
