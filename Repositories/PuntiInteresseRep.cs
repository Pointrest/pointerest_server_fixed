using Data;
using DTO;
using DTO.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PuntiInteresseRep// : IRepository<PuntoInteresse>
    {
        string mConnectionString;

        public PuntiInteresseRep() : this("connectionString") { }

        public PuntiInteresseRep(string connectionString)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionString];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionString));

            else mConnectionString = cs.ConnectionString;
        }

        private static void AddPIToList(List<PIQuery> puntiInteresse, SqlDataReader reader)
        {
            PIQuery tmp = null;
            var tmpID = -1;
            var index = 0;

            while (reader.Read())
            {
                var ID = 0;
                if (tmpID != (ID = reader.GetValue<int>("PuntoInteresseID")))
                {
                    tmp = new PIQuery();
                    tmp.ID = ID;
                    //tmp.IDGestore = reader.GetValue<int>("GestoreID");
                    tmp.Nome = reader.GetValue<string>("Nome");
                    tmp.Descrizione = reader.GetValue<string>("descrizione");
                    tmp.CategoriaID = reader.GetValue<int>("CategoriaID");
                    tmp.Categoria = reader.GetValue<string>("CategoryName");
                    tmp.SottocategoriaID = reader.GetValue<int>("SottocategoriaID");
                    tmp.Sottocategoria = reader.GetValue<string>("SubCategoryName");
                    tmp.Latitudine = reader.GetValue<double>("Latitudine");
                    tmp.Longitudine = reader.GetValue<double>("Longitudine");

                    ImmaginePIQuery image = createImage(reader);
                    tmp.Images = new List<ImmaginePIQuery>();
                    tmp.Images.Add(image);

                    puntiInteresse.Add(tmp);
                    index++;

                    tmpID = ID;
                }
                else
                {
                    ImmaginePIQuery image = createImage(reader);
                    puntiInteresse[index - 1].Images.Add(image);
                }
            }
        }

        public IEnumerable<PIQuery> GetAll()
        {
            List<PIQuery> puntiInteresse = new List<PIQuery>();

            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"SELECT * from PuntiInteresse 
                                    Left Outer Join Immagini 
                                    on PuntiInteresse.PuntoInteresseID = Immagini.PuntointeresseID
									Left Outer Join Sottocategorie
                                    on PuntiInteresse.SottocategoriaID = Sottocategorie.SottocategoriaID
                                    Left Outer Join Categorie
                                    on Sottocategorie.CategoriaID = Categorie.CategoriaID
                                    WHERE PuntiInteresse.IsTombStoned = 0";

                using (var command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        AddPIToList(puntiInteresse, reader);
                    }
                }
            }
            return puntiInteresse;
        }

        public IEnumerable<PIQuery> Get(string username)
        {
            List<PIQuery> puntiInteresse = new List<PIQuery>();
            int gestoreID = -1;


            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string getGestoreID = @"SELECT [ID]
                                        FROM [dbo].[Gestori]
                                        WHERE Gestori.Username = '" + username + "'";

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

                var getPIGestore = @"SELECT * from PuntiInteresse 
                                    Left Outer Join Immagini 
                                    on PuntiInteresse.PuntoInteresseID = Immagini.PuntointeresseID
									Left Outer Join Sottocategorie
                                    on PuntiInteresse.SottocategoriaID = Sottocategorie.SottocategoriaID
                                    Left Outer Join Categorie
                                    on Sottocategorie.CategoriaID = Categorie.CategoriaID
								    where PuntiInteresse.GestoreID =" + gestoreID +
                                    @" AND PuntiInteresse.IsTombStoned = 0 
                                    AND Immagini.isTombStone = 0";

                using (var command = new SqlCommand(getPIGestore, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AddPIToList(puntiInteresse, reader);
                        }
                    }
                }
            }
            return puntiInteresse;
        }

        public PIQuery Get(int id)
        {
            PIQuery tmp = null;
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"SELECT * from PuntiInteresse 
                                Left Outer Join Immagini 
                                on PuntiInteresse.PuntoInteresseID = Immagini.PuntointeresseID
                                Left Outer Join Categorie
                                on PuntiInteresse.SottocategoriaID = Categorie.CategoriaID
                                Left Outer Join Sottocategorie
                                on PuntiInteresse.SottocategoriaID = Sottocategorie.SottocategorieID
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
                                tmp = new PIQuery();
                                tmp.ID = ID;
                                //tmp.IDGestore = reader.GetValue<int>("GestoreID");
                                tmp.Nome = reader.GetValue<string>("Nome");
                                tmp.Categoria = reader.GetValue<string>("CategoryName");
                                tmp.SottocategoriaID = reader.GetValue<int>("SottocategoriaID");
                                tmp.Latitudine = reader.GetValue<double>("Latitudine");
                                tmp.Longitudine = reader.GetValue<double>("Longitudine");

                                ImmaginePIQuery image = createImage(reader);
                                tmp.Images = new List<ImmaginePIQuery>();
                                tmp.Images.Add(image);

                                tmpID = ID;
                            }
                            else
                            {
                                ImmaginePIQuery image = createImage(reader);
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

        public void Put(UpdatePIDataCommand updateCommand)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string updatePuntoInteresse = @"UPDATE [dbo].[PuntiInteresse]
                                                SET [Nome] = @Nome
                                                ,[Descrizione] = @Descrizione
                                                ,[Latitudine] = @Latitudine
                                                ,[Longitudine] = @Longitudine
                                                ,[SottocategoriaID] = @SottocategoriaID
                                                WHERE PuntiInteresse.PuntoInteresseID = @IDPuntoInteresse";

                using (var command = new SqlCommand(updatePuntoInteresse, connection))
                {
                        command.Parameters.Add(new SqlParameter("@IDPuntoInteresse", updateCommand.ID));
                        command.Parameters.Add(new SqlParameter("@Nome", updateCommand.Nome));
                        command.Parameters.Add(new SqlParameter("@Descrizione", updateCommand.Descrizione));
                        command.Parameters.Add(new SqlParameter("@Latitudine", updateCommand.Latitudine));
                        command.Parameters.Add(new SqlParameter("@Longitudine", updateCommand.Longitudine));
                        command.Parameters.Add(new SqlParameter("@SottocategoriaID", updateCommand.SottocategoriaID));

                        // Update Punto Interesse
                        var x = command.ExecuteNonQuery();
                    
                }
                connection.Close();
            }
        }

        public void Put(UpdatePIImagesCommand updateCommand)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                // set every image of the puntoInteresse to tombed
                string setEveryThingToTombed = @"UPDATE [dbo].[Immagini]
                                                         SET [isTombStone] = 1
                                                         WHERE Immagini.PuntointeresseID = " + updateCommand.IDPuntoInteresse;

                using (var tombCommand = new SqlCommand(setEveryThingToTombed, connection))
                {
                    int rowAffected = tombCommand.ExecuteNonQuery();
                }

                foreach (var image in updateCommand.Images)
                {

                    //if image id == -1, image is not present in db so add it
                    if (image.ImageID == -1)
                    {
                        var addImageQuery = @"INSERT INTO [dbo].[Immagini]
                                                    ([PuntointeresseID]
                                                   ,[Image]
                                                   ,[isTombStone])
                                                     VALUES
                                                   (@PuntoInteresseID
                                                   ,@ImageData
                                                   ,0)";

                        using (var insertNewImageCommand = new SqlCommand(addImageQuery, connection))
                        {
                            insertNewImageCommand.Parameters.Add(new SqlParameter("@PuntoInteresseID", updateCommand.IDPuntoInteresse));
                            insertNewImageCommand.Parameters.Add(new SqlParameter("@ImageData", image.ImageData));
                            //insertNewImageCommand.Parameters.Add(new SqlParameter("@IsTombStone", 0));
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
                
                connection.Close();

            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(mConnectionString))
            {
                connection.Open();

                string query = @"UPDATE [dbo].[PuntiInteresse]
                            SET [IsTombStoned] = 1
                            WHERE PuntiInteresse.PuntoInteresseId = " + id;

                using (var command = new SqlCommand(query, connection))
                {
                    int count = command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private static ImmaginePIQuery createImage(SqlDataReader reader)
        {
            ImmaginePIQuery image = new ImmaginePIQuery(reader.GetValue<int>("ImmagineID")
                                                                        , reader.GetValue<string>("Image"));
            return image;
        }
    }
}
