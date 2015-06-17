using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace Repositories
{
    public class ImagesRepository
    {
        string mConnectionString;
        private string p;

        public ImagesRepository()
            : this("connectionString")
        {

        }

        public ImagesRepository(string connectionString)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionString];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionString));

            else mConnectionString = cs.ConnectionString;
        }
        public string Get(int id)
        {
            string image = null;
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"SELECT [Image]
                                  FROM [dbo].[Immagini]
                                  WHERE Immagini.ImmagineID = " + id;

                using (var command = new System.Data.SqlClient.SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetValue<string>("Image");
                        }
                    }
                }
                connection.Close();
            }
            return null;
        }
    }
}
