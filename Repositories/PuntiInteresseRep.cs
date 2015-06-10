using Data;
using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PuntiInteresseRep : IRepository<PuntoInteresse>
    {
        string mConnectionString;

        public PuntiInteresseRep() : this("mConnectionString") {}

        public PuntiInteresseRep(string connectionString)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionString];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionString));

            else mConnectionString = cs.ConnectionString;
        }

        public IEnumerable<PuntoInteresse> GetAll()
        {
            List<PuntoInteresse> puntiInteresse = new List<PuntoInteresse>();

            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"SELECT * from PuntiInteresse 
                                Left Outer Join Immagini 
                                on PuntiInteresse.PuntoInteresseID = Immagini.PuntointeresseID
                                Left Outer Join Categorie
                                on PuntiInteresse.SottocategoriaID = Categorie.ID
                                Left Outer Join Sottocategorie
                                on PuntiInteresse.SottocategoriaID = Sottocategorie.ID";

                using (var command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        PuntoInteresse tmp = null;
                        var tmpID = -1;
                        var index = 0;

                        while (reader.Read())
                        {
                            var ID = 0;
                            if (tmpID != (ID = reader.GetValue<int>("PuntoInteresseID")))
                            {
                                tmp = new PuntoInteresse();
                                tmp.ID = ID;
                                tmp.IDGestore = reader.GetValue<int>("GestoreID");
                                tmp.Nome = reader.GetValue<string>("Nome");
                                tmp.Categoria = reader.GetValue<string>("CategoryName");
                                tmp.Sottocategoria = reader.GetValue<string>("SubCategoryName");
                                tmp.Latitudine = reader.GetValue<double>("Latitudine");
                                tmp.Longitudine = reader.GetValue<double>("Longitudine");

                                ImmaginePuntoInteresse image = createImage(reader);
                                tmp.Images.Add(image);

                                puntiInteresse.Add(tmp);
                                index++;

                                tmpID = ID;
                            }
                            else
                            {
                                ImmaginePuntoInteresse image = createImage(reader);
                                puntiInteresse[index - 1].Images.Add(image);
                            }
                        }
                    }
                }
            }
            return puntiInteresse;
        }

        public PuntoInteresse Get(int id)
        {
            PuntoInteresse tmp = null;
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"SELECT * from PuntiInteresse 
                                Left Outer Join Immagini 
                                on PuntiInteresse.PuntoInteresseID = Immagini.PuntointeresseID
                                Left Outer Join Categorie
                                on PuntiInteresse.SottocategoriaID = Categorie.ID
                                Left Outer Join Sottocategorie
                                on PuntiInteresse.SottocategoriaID = Sottocategorie.ID
                                where PuntiInteresse.PuntoInteresseID = " + id;

                using (var command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        var tmpID = -1;

                        while (reader.Read())
                        {
                            var ID = 0;
                            if (tmpID != (ID = reader.GetValue<int>("PuntoInteresseID")))
                            {
                                tmp = new PuntoInteresse();
                                tmp.ID = ID;
                                tmp.IDGestore = reader.GetValue<int>("GestoreID");
                                tmp.Nome = reader.GetValue<string>("Nome");
                                tmp.Categoria = reader.GetValue<string>("CategoryName");
                                tmp.Sottocategoria = reader.GetValue<string>("SubCategoryName");
                                tmp.Latitudine = reader.GetValue<double>("Latitudine");
                                tmp.Longitudine = reader.GetValue<double>("Longitudine");

                                ImmaginePuntoInteresse image = createImage(reader);
                                tmp.Images.Add(image);

                                tmpID = ID;
                            }
                            else
                            {
                                ImmaginePuntoInteresse image = createImage(reader);
                                tmp.Images.Add(image);
                            }
                        }
                    }
                }
            }
            return tmp;
        }

        public void Post(int GestoreID, CreatePuntoInteresseCommand createCommand)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string insertPuntoInteresse = @"INSERT INTO [dbo].[PuntiInteresse]
                                ([GestoreID]
                                ,[Nome]
                                ,[Categoria]
                               ,[Descrizione]
                               ,[Latitudine]
                               ,[Longitudine]
                               ,[SottocategoriaID])
                                OUTPUT INSERTED.ID
                                VALUES
                               (@GestoreID
                               ,@Nome
                               ,@Descrizione
                               ,@Latitudine
                               ,@Longitudine
                               ,@SottocategoriaID)";

                using (var puntoInteresseCommand = new SqlCommand(insertPuntoInteresse, connection))
                {
                    puntoInteresseCommand.Parameters.Add(new SqlParameter("@GestoreID", createCommand.IDGestore));
                    puntoInteresseCommand.Parameters.Add(new SqlParameter("@Nome", createCommand.Nome));
                    puntoInteresseCommand.Parameters.Add(new SqlParameter("@Descrizione", createCommand.Descrizione));
                    puntoInteresseCommand.Parameters.Add(new SqlParameter("@Latitudine", createCommand.Latitudine));
                    puntoInteresseCommand.Parameters.Add(new SqlParameter("@Longitudine", createCommand.Longitudine));
                    puntoInteresseCommand.Parameters.Add(new SqlParameter("@SottocategoriaID", createCommand.Sottocategoria));

                    int lastID = (int)puntoInteresseCommand.ExecuteScalar();

                    string insertImmaginePuntoInteresse = @"INSERT INTO [dbo].[Immagini]
                                                         ([PuntointeresseID]
                                                         ,[Image])
                                                         VALUES
                                                         @IDPUntoInteresse
                                                        ,@Immagine)";

                    List<string> immagini = null;
                    var index = 0;
                    foreach (string image in (immagini = createCommand.Images))
                    {

                        using (var imageCommand = new SqlCommand(insertImmaginePuntoInteresse, connection))
                        {
                            imageCommand.Parameters.Add(new SqlParameter("@IDPUntoInteresse", lastID));
                            imageCommand.Parameters.Add(new SqlParameter("Immagine", immagini[index]));
                        }
                        ++index;
                    }

                    connection.Close();
                }
            }
        }

        public void Put(UpdatePuntoInteresseCommand updateCommand)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string updatePuntoInteresse = @"UPDATE [dbo].[PuntiInteresse]
                                                SET [GestoreID] = @GestoreID
                                                ,[Nome] = @Nome
                                                ,[Descrizione] = @Descrizione
                                                ,[Latitudine] = @Latitudine
                                                ,[Longitudine] = @Longitudine
                                                ,[SottocategoriaID] = @Sottocategoria
                                                WHERE PuntiInteresse.ID = @IDPuntoInteresse";

                SqlTransaction tr;
                using (var command = new SqlCommand(updatePuntoInteresse, connection, tr = connection.BeginTransaction()))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        command.Parameters.Add(new SqlParameter("@IDPuntoInteresse", updateCommand.ID));
                        command.Parameters.Add(new SqlParameter("@GestoreID", updateCommand.IDGestore));
                        command.Parameters.Add(new SqlParameter("@Nome", updateCommand.Nome));
                        command.Parameters.Add(new SqlParameter("@Descrizione", updateCommand.Descrizione));
                        command.Parameters.Add(new SqlParameter("@Latitudine", updateCommand.Latitudine));
                        command.Parameters.Add(new SqlParameter("@Longitudine", updateCommand.Longitudine));
                        command.Parameters.Add(new SqlParameter("@SottocategoriaID", updateCommand.Sottocategoria));

                        // Update Punto Interesse
                        command.ExecuteNonQuery();

                        // set every image of the puntoInteresse to tombed
                        string setEveryThingToTombed = @"UPDATE [dbo].[Immagini]
                                                         SET [isTombStone] = 1
                                                         WHERE Immagini.PuntointeresseID = " + updateCommand.ID;

                        using (var tombCommand = new SqlCommand(setEveryThingToTombed, connection))
                        {
                            int rowAffected = tombCommand.ExecuteNonQuery();
                        }

                        foreach(DTO.UpdatePuntoInteresseCommand.UpdatedImage image in updateCommand.Images) {

                            //if image id == -1 is not present in db so add it
                            if (image.ImageID == -1)
                            {
                                var addImageQuery = @"INSERT INTO [dbo].[Immagini]
                                                    ([PuntointeresseID]
                                                   ,[Image]
                                                   ,[isTombStone])
                                                     VALUES
                                                   (@PuntoInteresseID
                                                   ,@ImageData
                                                   ,@IsTombStone";

                                using (var insertNewImageCommand = new SqlCommand(addImageQuery, connection))
                                {
                                    insertNewImageCommand.Parameters.Add(new SqlParameter("@PuntoInteresseID", updateCommand.ID));
                                    insertNewImageCommand.Parameters.Add(new SqlParameter("@ImageData", image.ImageData));
                                    insertNewImageCommand.Parameters.Add(new SqlParameter("@IsTombStone", 0));
                                    int rowAffected = (int)insertNewImageCommand.ExecuteNonQuery();
                                }


                            }
                            //if image id > -1 is already present in db so set tombed to 0
                            else if (image.ImageID > -1)
                            {
                                var unTombQuery = @"UPDATE [dbo].[Immagini]
                                                SET [isTombStone] = 0
                                                WHERE Immagini.ImmagineID = @ID";
                                using (var unTombCommand = new SqlCommand(unTombQuery, connection))
                                {
                                    unTombCommand.Parameters.Add(new SqlParameter("@ID", image.ImageID));
                                    int rowAffected = (int)unTombCommand.ExecuteNonQuery();
                                }
                            }
                        }

                       






                        // if image string == dbimagestring set this row in db to not tombed

                        /*foreach (string image in updateCommand.Images)
                        {
                            string updateImmaginePuntoInteresse = @"UPDATE [dbo].[Immagini]
                                                                 SET [isTombStone] = 0
                                                                 WHERE Immagini.Image = " + image;

                            using (var check = new SqlCommand(updateImmaginePuntoInteresse, connection))
                            {
                                int affected = check.ExecuteNonQuery();

                                // if image is not present in db add it
                                if (affected < 1)
                                {
                                    string insertNewImmaginePuntoInteresse = @"INSERT INTO [dbo].[Immagini]
                                                                           ([PuntointeresseID]
                                                                           ,[Image]
                                                                           ,[isTombStone])
                                                                            VALUES
                                                                           (@PuntoInteresseID
                                                                           ,@Image
                                                                           ,@IsTombStoned)";

                                    using (var insertNewImage = new SqlCommand(insertNewImmaginePuntoInteresse, connection))
                                    {
                                        insertNewImage.Parameters.Add(new SqlParameter("@IDPuntoInteresse", updateCommand.ID));
                                        insertNewImage.Parameters.Add(new SqlParameter("@Image", image));
                                        insertNewImage.Parameters.Add(new SqlParameter("@IsTombStoned", false));

                                        // Update Punto Interesse
                                        insertNewImage.ExecuteNonQuery();
                                    }
                                }
                            }
                        }*/

                        tr.Commit();
                        connection.Close();
                    }
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"UPDATE [dbo].[PuntiInteresse]
                            SET [IsTombStoned] = 0
                            WHERE PuntiInteresse.PuntoInteresseId = " + id;

                using (var command = new SqlCommand(query, connection))
                {
                    int count = command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private static ImmaginePuntoInteresse createImage(SqlDataReader reader)
        {
            ImmaginePuntoInteresse image = new ImmaginePuntoInteresse(reader.GetValue<int>("ImmagineID")
                                                                        , reader.GetValue<string>("Image")
                                                                        , false);
            return image;
        }
    }
}
