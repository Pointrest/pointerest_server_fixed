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
    public class SottocategorieRepository
    {
        string mConnectionString;

        public SottocategorieRepository()
            : this("mConnectionString")
        {

        }

        public SottocategorieRepository(string connectionString)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionString];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionString));

            else mConnectionString = cs.ConnectionString;
        }

        public IEnumerable<Sottocategoria> GetAll()
        {
            List<Sottocategoria> sottocategorie = new List<Sottocategoria>();

            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"SELECT * from Sottocategorie";

                using (var command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Sottocategoria tmp = null;

                        while (reader.Read())
                        {

                            tmp = new Sottocategoria();
                            tmp.ID = reader.GetValue<int>("ID");
                            tmp.SubCategoryName = reader.GetValue<string>("SubCategoryName");
                            tmp.CategoriaID = reader.GetValue<int>("CategoriaID");

                            sottocategorie.Add(tmp);
                        }
                    }
                }
            }
            return sottocategorie;

        }
    
    }
}
