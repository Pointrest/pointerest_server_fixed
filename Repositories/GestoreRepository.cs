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
using Data;

namespace Repositories
{
    public class GestoreRepository// : IGestoreRepository<HttpResponseMessage>
    {
        string mConnectionString;
        private string p;

        public GestoreRepository()
            : this("mConnectionString")
        {

        }

        public GestoreRepository(string connectionString)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionString];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionString));

            else mConnectionString = cs.ConnectionString;
        }

        public Gestore Get(int id)
        {
            Gestore gestore = null;
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"SELECT * from Gestori "
                                + " WHERE Gestori.ID = " + id;

                SqlTransaction transaction;
                using (var command = new System.Data.SqlClient.SqlCommand(query, connection, transaction = connection.BeginTransaction()))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gestore = new Gestore();

                            gestore.ID = reader.GetValue<int>("ID");
                            gestore.nome = reader.GetValue<string>("Nome");
                            gestore.cognome = reader.GetValue<string>("Cognome");
                            gestore.username = reader.GetValue<string>("Username");
                            gestore.password = reader.GetValue<string>("Password");
                            gestore.isTombStone = reader.GetValue<bool>("isTombStone");
                        }
                    }
                }
                transaction.Commit();
                connection.Close();
            }
            return gestore;
        }

        public HttpResponseMessage Post(Gestore gestore)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"INSERT INTO [dbo].[Gestori]
                                ([Nome]
                                ,[Cognome]
                               ,[Username]
                               ,[Password]
                               ,[isTombStone])
                                OUTPUT INSERTED.ID
                                VALUES
                               (@Nome
                               ,@Cognome
                               ,@Username
                               ,@Password
                               ,@isTombStone)";

                SqlTransaction transaction;
                using (var command = new SqlCommand(query, connection,transaction = connection.BeginTransaction()))
                {
                    command.Parameters.Add(new SqlParameter("@Nome", gestore.nome));
                    command.Parameters.Add(new SqlParameter("@Cognome", gestore.cognome));
                    command.Parameters.Add(new SqlParameter("@Username", gestore.username));
                    command.Parameters.Add(new SqlParameter("@Password", gestore.password));
                    command.Parameters.Add(new SqlParameter("@isTombStone", gestore.isTombStone));

                    int lastID = (int)command.ExecuteNonQuery();


                    transaction.Commit();
                    connection.Close();

                    return getHttpStatusCode(lastID);
                }
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"UPDATE [dbo].[Gestori]
                               SET [isTombStone] = 1
                               WHERE Gestori.ID = " + id;

                SqlTransaction transaction;
                using (var command = new SqlCommand(query, connection, transaction = connection.BeginTransaction()))
                {                   
                    int lastID = (int)command.ExecuteNonQuery();

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
