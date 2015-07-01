using Data;
using DTO;
using DTO.Commands;
using Microsoft.SqlServer.Types;
using Repositories.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Spatial;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PuntiInteresseRep
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

        private void AddPIToList(List<PIQuery> puntiInteresse, SqlDataReader reader)
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
                    tmp.Indirizzo = reader.GetValue<string>("indirizzo");
                    tmp.CategoriaID = reader.GetValue<int>("CategoriaID");
                    tmp.Categoria = reader.GetValue<string>("CategoryName");
                    tmp.SottocategoriaID = reader.GetValue<int>("SottocategoriaID");
                    tmp.Sottocategoria = reader.GetValue<string>("SubCategoryName");
                    dynamic geoPoint = reader.GetValue(6);
                    tmp.Latitudine = (double)geoPoint.Lat;
                    tmp.Longitudine = (double)geoPoint.Long;
                    tmp.Images = new List<ImmaginePIQuery>();

                    ImmaginePIQuery image = CreateImage(reader);

                    if (image.ImageData != null)
                    {
                        tmp.Images.Add(image);

                    }
                    puntiInteresse.Add(tmp);
                    index++;
                    tmpID = ID;
                }
                else
                {
                    ImmaginePIQuery image = CreateImage(reader);
                    puntiInteresse[index - 1].Images.Add(image);
                }
            }
        }

        public IEnumerable<PIQuery> GetAll()
        {
            return GetAllPI();
        }

        private IEnumerable<PIQuery> GetAllPI()
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
                                    WHERE PuntiInteresse.IsTombStoned = 0
                                    AND (Immagini.isTombStone = 0 OR Immagini.isTombStone is NULL)";

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

        public IEnumerable<PIQuery> Get(string gestoreUsername)
        {
            List<PIQuery> puntiInteresse = new List<PIQuery>();
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

                var getPIGestore = @"SELECT * from PuntiInteresse 
                                    Left Outer Join Immagini 
                                    on PuntiInteresse.PuntoInteresseID = Immagini.PuntointeresseID
									Left Outer Join Sottocategorie
                                    on PuntiInteresse.SottocategoriaID = Sottocategorie.SottocategoriaID
                                    Left Outer Join Categorie
                                    on Sottocategorie.CategoriaID = Categorie.CategoriaID
								    where PuntiInteresse.GestoreID =" + gestoreID +
                                    @" AND PuntiInteresse.IsTombStoned = 0 
                                    AND (Immagini.isTombStone = 0 OR Immagini.isTombStone is NULL)";

                using (var command = new SqlCommand(getPIGestore, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        AddPIToList(puntiInteresse, reader);
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
                                on PuntiInteresse.SottocategoriaID = Sottocategorie.SottocategoriaID
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
                                dynamic geoPoint = reader.GetValue(6);
                                tmp.Latitudine = (double)geoPoint.Lat;
                                tmp.Longitudine = (double)geoPoint.Long;
                                ImmaginePIQuery image = CreateImage(reader);
                                tmp.Images = new List<ImmaginePIQuery>();
                                tmp.Images.Add(image);

                                tmpID = ID;
                            }
                            else
                            {
                                ImmaginePIQuery image = CreateImage(reader);
                                tmp.Images.Add(image);
                            }
                        }
                    }
                }
            }
            return tmp;
        }

        public void Post(string gestoreName, CreatePuntoInteresseCommand createCommand)
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

                string insertPuntoInteresse = @"INSERT INTO [dbo].[PuntiInteresse]
                                ([GestoreID]
                                ,[Nome]
                               ,[Descrizione]
                                ,[Indirizzo]
                                ,[GPSPoint]
                               ,[SottocategoriaID]
                                ,[IsTombStoned])
                                OUTPUT INSERTED.PuntoInteresseID
                                VALUES
                               (@GestoreID
                               ,@Nome
                               ,@Descrizione
                               ,@Indirizzo
                               , geography::STGeomFromText(@GPSPoint,4326) 
                               ,@SottocategoriaID
                               ,0)";

                SqlTransaction transaction;
                using (var puntoInteresseCommand = new SqlCommand(insertPuntoInteresse, connection, transaction = connection.BeginTransaction()))
                {
                    puntoInteresseCommand.Parameters.Add(new SqlParameter("@GestoreID", gestoreID));
                    puntoInteresseCommand.Parameters.Add(new SqlParameter("@Nome", createCommand.Nome));
                    puntoInteresseCommand.Parameters.Add(new SqlParameter("@Descrizione", createCommand.Descrizione));
                    puntoInteresseCommand.Parameters.Add(new SqlParameter("@Indirizzo", createCommand.Indirizzo));
                    //puntoInteresseCommand.Parameters.Add(new SqlParameter("@Latitudine", createCommand.Latitudine));
                    //puntoInteresseCommand.Parameters.Add(new SqlParameter("@Longitudine", createCommand.Longitudine));
                    var point = GetPointToInsert(createCommand.Latitudine, createCommand.Longitudine);
                    puntoInteresseCommand.Parameters.Add(new SqlParameter("@GPSPoint", point));
                    puntoInteresseCommand.Parameters.Add(new SqlParameter("@SottocategoriaID", createCommand.SottocategoriaID));

                    int lastID = (int)puntoInteresseCommand.ExecuteScalar();

                    if (createCommand.Images != null)
                    {
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
                    }
                    transaction.Commit();
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
                                                ,[GPSPoint] = geography::STGeomFromText(@GPSPoint,4326)
                                                ,[SottocategoriaID] = @SottocategoriaID
                                                WHERE PuntiInteresse.PuntoInteresseID = @IDPuntoInteresse";

                SqlTransaction transaction;
                using (var command = new SqlCommand(updatePuntoInteresse, connection, transaction = connection.BeginTransaction()))
                {
                    command.Parameters.Add(new SqlParameter("@IDPuntoInteresse", updateCommand.ID));
                    command.Parameters.Add(new SqlParameter("@Nome", updateCommand.Nome));
                    command.Parameters.Add(new SqlParameter("@Descrizione", updateCommand.Descrizione));
                    command.Parameters.Add(new SqlParameter("@Indirizzo", updateCommand.Indirizzo));
                    //command.Parameters.Add(new SqlParameter("@Point", GetGeoPoint(updateCommand.Latitudine, updateCommand.Longitudine)));
                    var point = GetPointToInsert(updateCommand.Latitudine, updateCommand.Longitudine);
                    command.Parameters.Add(new SqlParameter("@GPSPoint", point));
                    command.Parameters.Add(new SqlParameter("@SottocategoriaID", updateCommand.SottocategoriaID));

                    // Update Punto Interesse
                    var x = command.ExecuteNonQuery();
                    transaction.Commit();
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

                SqlTransaction transaction;
                using (var tombCommand = new SqlCommand(setEveryThingToTombed, connection, transaction = connection.BeginTransaction()))
                {
                    int rowAffected = tombCommand.ExecuteNonQuery();
                    transaction.Commit();
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

                        using (var insertNewImageCommand = new SqlCommand(addImageQuery, connection, transaction = connection.BeginTransaction()))
                        {
                            insertNewImageCommand.Parameters.Add(new SqlParameter("@PuntoInteresseID", updateCommand.IDPuntoInteresse));

                            var compressor = new ImageCompressor();
                            var compressedImage = compressor.CompressImage(image.ImageData);

                            insertNewImageCommand.Parameters.Add(new SqlParameter("@ImageData", compressedImage));
                            //insertNewImageCommand.Parameters.Add(new SqlParameter("@IsTombStone", 0));
                            int rowAffected = (int)insertNewImageCommand.ExecuteNonQuery();
                            transaction.Commit();
                        }


                    }
                    //if image id > -1 is already present in db so set tombed to 0
                    else if (image.ImageID > -1)
                    {
                        var unTombQuery = @"UPDATE [dbo].[Immagini]
                                                SET [isTombStone] = 0
                                                WHERE Immagini.ImmagineID = @ID";
                        using (var unTombCommand = new SqlCommand(unTombQuery, connection, transaction = connection.BeginTransaction()))
                        {
                            unTombCommand.Parameters.Add(new SqlParameter("@ID", image.ImageID));
                            int rowAffected = (int)unTombCommand.ExecuteNonQuery();
                            transaction.Commit();
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

                SqlTransaction transaction;
                using (var command = new SqlCommand(query, connection, transaction = connection.BeginTransaction()))
                {
                    int count = command.ExecuteNonQuery();
                    transaction.Commit();
                }
                connection.Close();
            }
        }

        private ImmaginePIQuery CreateImage(SqlDataReader reader)
        {
            ImmaginePIQuery image = new ImmaginePIQuery(reader.GetValue<int>("ImmagineID")
                                                                        , reader.GetValue<string>("Image"));
            return image;
        }

        private static DbGeography GetGeoPoint(double latitude, double longitude)
        {
            string wkt = string.Format(CultureInfo.InvariantCulture, "POINT({1} {0})", latitude, longitude);
            return DbGeography.FromText(wkt, 4326);
        }

        private static DbGeography GetGeoPointRE(double latitude, double longitude)
        {
            DbGeography point = DbGeography.FromText(String.Format("POINT({0} {1})", latitude, longitude), 4326);
            return point;
        }

        public IEnumerable<PIMobileQuery> GetPIInRadius(double latitudine, double longitudine, int raggio)
        {
            var userGeoPoint = GetGeoPoint(latitudine, longitudine);
            List<PIQuery> allPI = GetAllPI().ToList();
            List<PIMobileQuery> userWantedPI = new List<PIMobileQuery>();

            foreach (var puntoInteresse in allPI)
            {
                    var nextPointToCheck = GetGeoPoint(puntoInteresse.Latitudine, puntoInteresse.Longitudine);
                    if (nextPointToCheck != null)
                    {
                        double? distance = nextPointToCheck.Distance(userGeoPoint);

                        if (distance <= raggio * 1000)
                        {
                            var images = new List<int>();
                            foreach (var image in puntoInteresse.Images)
                            {
                                if (image.ImageData != null)
                                    images.Add(image.ImageID);
                            }
                            userWantedPI.Add(new PIMobileQuery(puntoInteresse.ID, puntoInteresse.Nome, puntoInteresse.CategoriaID
                                                            , puntoInteresse.Categoria, puntoInteresse.SottocategoriaID
                                                            , puntoInteresse.Sottocategoria, puntoInteresse.Descrizione, 
                                                            puntoInteresse.Indirizzo, puntoInteresse.Latitudine, puntoInteresse.Longitudine, images));
                        }
                    }
            }
            return userWantedPI;
        }

        private static string GetPointToInsert(double latitude, double longitude)
        {
            string lat = latitude.ToString(CultureInfo.InvariantCulture);
            string lon = longitude.ToString(CultureInfo.InvariantCulture);
            var point = "POINT(" + lon + " " + lat + ")";
            return point;
        }
    }
}
