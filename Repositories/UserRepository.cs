using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Repositories
{
    public class UserRepository : IUserRepository<HttpResponseMessage>
    {

        string mConnectionString;
       
        public UserRepository()
            : this("mConnectionString")
        {
            
        }

        public UserRepository(string connectionString)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionString];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionString));

            else mConnectionString = cs.ConnectionString;
        }

        public HttpResponseMessage Post(User user)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"INSERT INTO [dbo].[Users]
                                ([Username]
                                ,[Name]
                               ,[Surname]
                               ,[Password])
                                OUTPUT INSERTED.ID
                                VALUES
                               (@Username
                               ,@Name
                               ,@Surname
                               ,@Password)";

                SqlTransaction transaction;
                using (var command = new SqlCommand(query, connection, transaction = connection.BeginTransaction()))
                {
                    command.Parameters.Add(new SqlParameter("@Username", user.userName));
                    command.Parameters.Add(new SqlParameter("@Name", user.name));
                    command.Parameters.Add(new SqlParameter("@Surname", user.surName));
                    command.Parameters.Add(new SqlParameter("@Password", user.password));
                   
                    int lastID = (int)command.ExecuteNonQuery();

                    transaction.Commit();
                    connection.Close();

                    return getHttpStatusCode(lastID);       
                }
            }
        }

        public HttpResponseMessage Post(int id, int idPuntoInteresse)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"INSERT INTO [dbo].[Preferiti]
                                ([UserID]
                                ,[PuntointeresseID])
                                VALUES
                               (@ID
                               ,@PuntoInteresseID)";

                SqlTransaction transaction;
                using (var command = new SqlCommand(query, connection, transaction = connection.BeginTransaction()))
                {
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    command.Parameters.Add(new SqlParameter("@PuntoInteresseID",idPuntoInteresse));

                    int lastID = command.ExecuteNonQuery();

                    transaction.Commit();
                    connection.Close();

                    return getHttpStatusCode(lastID);            
                }  
            }
        }

        private HttpResponseMessage getHttpStatusCode(int lastID)
        {
            if (lastID != null && lastID > 0)
                return new HttpResponseMessage(HttpStatusCode.OK);
            else
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}
