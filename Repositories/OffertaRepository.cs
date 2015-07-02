using DTO.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using Data;
using DTO.Commands;

namespace Repositories
{
    public class OffertaRepository
    {
        string mConnectionString;

        public OffertaRepository() : this("connectionString") { }

        public OffertaRepository(string connectionString)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionString];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionString));

            else mConnectionString = cs.ConnectionString;
        }

        private void AddOffToList(List<OffertaQuery> Offerte, SqlDataReader reader)
        {
            OffertaQuery tmp = null;
            var tmpID = -1;
            var index = 0;

            while (reader.Read())
            {
                var ID = 0;
                if (tmpID != (ID = reader.GetValue<int>("IDOfferta")))
                {
                    tmp = new OffertaQuery();
                    tmp.IDOfferta = ID;
                    tmp.Nome = reader.GetValue<string>("Nome");
                    tmp.Descrizione = reader.GetValue<string>("Descrizione");
                    tmp.DataInizio = reader.GetValue<DateTime>("DataInizio");
                    tmp.DataFine = reader.GetValue<DateTime>("DataFine");

                    tmp.ImmagineOfferta = reader.GetValue<string>("Immagine");

                    Offerte.Add(tmp);
                    index++;
                    tmpID = ID;
                }
            }
        }

        public IEnumerable<OffertaQuery> GetAll()
        {
            return GetAllOff();
        }

        // ritorna tutte offerte
        private IEnumerable<OffertaQuery> GetAllOff()
        {
            List<OffertaQuery> Offerte = new List<OffertaQuery>();

            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"SELECT * from Offerte";

                using (var command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        AddOffToList(Offerte, reader);
                    }
                }
            }
            return Offerte;
        }

        // ritorna offerte per gestore
        public IEnumerable<OffertaQuery> Get(string gestoreUsername)
        {
            List<OffertaQuery> Offerte = new List<OffertaQuery>();
            int gestoreID = -1;


            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string getGestoreID = @"SELECT [ID]
                                        FROM [dbo].[Gestori]
                                        WHERE Gestori.Username = '" + gestoreUsername + "'";

                using (var command = new SqlCommand(getGestoreID, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gestoreID = reader.GetValue<int>("ID");
                        }
                    }
                }

                var getOffGestore = @"SELECT * from Offerte
								    where PuntiInteresse.GestoreID =" + gestoreID +
                                    @"";

                using (var command = new SqlCommand(getOffGestore, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        AddOffToList(Offerte, reader);
                    }
                }
            }
            return Offerte;
        }


        public OffertaQuery Get(int id)
        {
            OffertaQuery tmp = null;
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"SELECT * from Offerte 
                                where Offerte.IDOfferta = " + id;

                using (var command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var tmpID = -1;
                        while (reader.Read())
                        {
                            var ID = 0;
                            if (tmpID != (ID = reader.GetValue<int>("IDOfferta")))
                            {
                                tmp = new OffertaQuery();
                                
                                tmp.Nome = reader.GetValue<string>("Nome");
                                tmp.Descrizione = reader.GetValue<string>("Descrizione");
                                tmp.DataInizio = reader.GetValue<DateTime>("DataInizio");
                                tmp.DataFine = reader.GetValue<DateTime>("DataFine");
                                tmp.ImmagineOfferta = reader.GetValue<string>("Immagine");

                                //ImmagineOffertaQuery image = CreateImage(reader);

                                tmpID = ID;
                            }
                            else
                            {
                                tmp.ImmagineOfferta = reader.GetValue<string>("ImmagineOfferta");
                            }
                        }
                    }
                }
            }
            return tmp;
        }

        public void Post(string gestoreName, CreateOffertaCommand createCommand)
        {
            int gestoreID = -1;

            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string getGestoreID = @"SELECT [ID]
                                        FROM [dbo].[Gestori]
                                        WHERE Gestori.Username = '" + gestoreName + "'";


                using (var command = new SqlCommand(getGestoreID, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gestoreID = reader.GetValue<int>("ID");
                        }
                    }
                }

                string insertOfferta = @"INSERT INTO [dbo].[Offerte]
                                ([GestoreID]
                                ,[Nome]
                               ,[Descrizione]
                                ,[Indirizzo]
                                ,[Immagine]
                                ,[IsTombStoned])
                                OUTPUT INSERTED.IDOfferta
                                VALUES
                               (@GestoreID
                               ,@Nome
                               ,@Descrizione
                               ,0)";

                SqlTransaction transaction;
                using (var offertaCommand = new SqlCommand(insertOfferta, connection, transaction = connection.BeginTransaction()))
                {
                    offertaCommand.Parameters.Add(new SqlParameter("@IDOfferta", createCommand.IDOfferta));
                    offertaCommand.Parameters.Add(new SqlParameter("@Nome", createCommand.Nome));
                    offertaCommand.Parameters.Add(new SqlParameter("@Descrizione", createCommand.Descrizione));
                    offertaCommand.Parameters.Add(new SqlParameter("@DataInizio", createCommand.DataInizio));
                    offertaCommand.Parameters.Add(new SqlParameter("@DataFine", createCommand.DataFine));
                    offertaCommand.Parameters.Add(new SqlParameter("@Immagine", createCommand.Immagine));
                    
                    int lastID = (int)offertaCommand.ExecuteScalar();
             
                    transaction.Commit();
                    connection.Close();
                }
            }
        }

        public void Put(UpdateOffertaCommand updateCommand)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string updateOfferta = @"UPDATE [dbo].[PuntiInteresse]
                                                SET [Nome] = @Nome
                                                ,[Descrizione] = @Descrizione
                                                WHERE Offerta.IDOfferta = @IDOfferta";

                SqlTransaction transaction;
                using (var command = new SqlCommand(updateOfferta, connection, transaction = connection.BeginTransaction()))
                {
                    command.Parameters.Add(new SqlParameter("@IDOfferta", updateCommand.IDOfferta));
                    command.Parameters.Add(new SqlParameter("@Nome", updateCommand.Nome));
                    command.Parameters.Add(new SqlParameter("@Descrizione", updateCommand.Descrizione));
                    command.Parameters.Add(new SqlParameter("@DataInizio", updateCommand.DataInizio));
                    command.Parameters.Add(new SqlParameter("@DataFine", updateCommand.DataFine));
                    command.Parameters.Add(new SqlParameter("@Immagine", updateCommand.Immagine));

                    // Update Punto Interesse
                    var x = command.ExecuteNonQuery();
                    transaction.Commit();
                }
                connection.Close();
            }
        }



        public void Delete(int id)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"UPDATE [dbo].[Offerta]
                            SET [IsTombStoned] = 1
                            WHERE Offerte.IDOfferta = " + id;

                SqlTransaction transaction;
                using (var command = new SqlCommand(query, connection, transaction = connection.BeginTransaction()))
                {
                    int count = command.ExecuteNonQuery();
                    transaction.Commit();
                }
                connection.Close();
            }
        }
    }


}
