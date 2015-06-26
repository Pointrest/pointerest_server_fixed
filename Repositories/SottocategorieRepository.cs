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
            : this("connectionString")
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
                            tmp.ID = reader.GetValue<int>("SottocategoriaID");
                            tmp.SubCategoryName = reader.GetValue<string>("SubCategoryName");
                            tmp.CategoriaID = reader.GetValue<int>("CategoriaID");

                            sottocategorie.Add(tmp);
                        }
                    }
                }
            }
            return sottocategorie;
        }

        public List<Sottocategoria> Get(int id)
        {

            List<Sottocategoria> sottoCategorie = new List<Sottocategoria>();
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"SELECT [SottocategoriaID]
                                ,[SubCategoryName]
                                FROM [dbo].[Sottocategorie]
                                WHERE Sottocategorie.CategoriaID = " + id;

                using (var command = new System.Data.SqlClient.SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Sottocategoria tmp = new Sottocategoria();
                            tmp.ID = reader.GetValue<int>("SottocategoriaID");
                            tmp.SubCategoryName = reader.GetValue<string>("SubCategoryName");

                            sottoCategorie.Add(tmp);
                        }
                    }
                }
                connection.Close();
            }
            return sottoCategorie;

        }
    }
}
