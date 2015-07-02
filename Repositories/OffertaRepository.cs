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

        public IEnumerable<OffertaQuery> Get(int puntoInteresseID)
        {
            List<OffertaQuery> Offerte = new List<OffertaQuery>();

            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string getGestoreID = @"SELECT [IDOfferta]
                                              ,[Nome]
                                              ,[Descrizione]
                                              ,[DataInizio]
                                              ,[DataFine]
                                              ,[Immagine]
                                              ,[IsTombStoned]
                                          FROM [dbo].[Offerte]
                                          WHERE IDPuntoInteresse = " + puntoInteresseID +
                                          "AND IsTombStoned = 0";

                using (var command = new SqlCommand(getGestoreID, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    { 
                        while (reader.Read())
                        {
                            var offerta = new OffertaQuery();
                            offerta.IDOfferta = reader.GetValue<int>("IDOfferta");
                            offerta.Nome = reader.GetValue<string>("Nome");
                            offerta.Descrizione = reader.GetValue<string>("Descrizione");
                            offerta.DataInizio = reader.GetValue<DateTime>("DataInizio");
                            offerta.DataFine = reader.GetValue<DateTime>("DataFine");
                            offerta.ImmagineOfferta = reader.GetValue<string>("Immagine");
                            Offerte.Add(offerta);
                        }
                    }
                }
            }
            return Offerte;
        }

        public void Post(CreateOffertaCommand createCommand)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string insertOfferta = @"INSERT INTO [dbo].[Offerte]
                                ([IDPuntoInteresse]
                               ,[Nome]
                               ,[Descrizione]
                               ,[DataInizio]
                               ,[DataFine]
                               ,[Immagine]
                               ,[IsTombStoned])
                                VALUES
                               (@IDPuntoInteresse
                               ,@Nome
                               ,@Descrizione
                               ,@DataInizio
                               ,@DataFine
                               ,@Immagine
                               ,0)";

                SqlTransaction transaction;
                using (var offertaCommand = new SqlCommand(insertOfferta, connection, transaction = connection.BeginTransaction()))
                {
                    offertaCommand.Parameters.Add(new SqlParameter("@IDPuntoInteresse", createCommand.IDPuntoInteresse));
                    offertaCommand.Parameters.Add(new SqlParameter("@Nome", createCommand.Nome));
                    offertaCommand.Parameters.Add(new SqlParameter("@Descrizione", createCommand.Descrizione));
                    offertaCommand.Parameters.Add(new SqlParameter("@DataInizio", createCommand.DataInizio));
                    offertaCommand.Parameters.Add(new SqlParameter("@DataFine", createCommand.DataFine));
                    offertaCommand.Parameters.Add(new SqlParameter("@Immagine", createCommand.Immagine));
                    
                    int rowAffect = (int)offertaCommand.ExecuteNonQuery();
             
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

                string query = @"UPDATE [dbo].[Offerte]
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
