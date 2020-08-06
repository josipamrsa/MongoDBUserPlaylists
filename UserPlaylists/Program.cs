using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;

using UserPlaylists.Models;
using System.Data;
using System.Threading;
using System.Runtime.CompilerServices;

namespace UserPlaylists
{
    class Program
    {
        //----Modeli----//
        public static List<Artist> Artists { get; set; }
        public static List<Genre> Genres { get; set; }
        public static List<Playlist> Playlists { get; set; }
        public static List<Song> Songs { get; set; }
        public static List<User> Users { get; set; }
        public static UserPlaylistData UserPlaylistData { get; set; }

        //----Veza na bazu podataka----//

        public static IMongoDatabase userPlaylistDatabase { get; set; }
      

        public static void GenerateData()
        {
            // Generiranje podataka iz JSON-a u objekt

            var master = File.ReadAllText("./JSON/Master.json");
            UserPlaylistData = JsonConvert.DeserializeObject<UserPlaylistData>(master);

            Artists = UserPlaylistData.Artists;
            Genres = UserPlaylistData.Genres;
            Playlists = UserPlaylistData.Playlists;
            Songs = UserPlaylistData.Songs;
            Users = UserPlaylistData.Users;                       
        }

        public static void DatabaseConnect()
        {
            // Povezivanje na bazu podataka

            MongoClient mgClient = new MongoClient("mongodb://localhost:27017");
            userPlaylistDatabase = mgClient.GetDatabase("UserPlaylist");                           
        }
        
        public static void CreateCollections()
        {
            // Stvaranje kolekcija

            IMongoCollection<Artist> artistCollection = userPlaylistDatabase.GetCollection<Artist>("Artist");
            artistCollection.InsertMany(Artists);

            IMongoCollection<Genre> genreCollection = userPlaylistDatabase.GetCollection<Genre>("Genre");
            genreCollection.InsertMany(Genres);

            IMongoCollection<Playlist> playlistCollection = userPlaylistDatabase.GetCollection<Playlist>("Playlist");
            playlistCollection.InsertMany(Playlists);

            IMongoCollection<Song> songCollection = userPlaylistDatabase.GetCollection<Song>("Song");
            songCollection.InsertMany(Songs);

            IMongoCollection<User> userCollection = userPlaylistDatabase.GetCollection<User>("User");
            userCollection.InsertMany(Users);
        }

        public static void UpdateRecords()
        {
            // Ažuriranje podataka

            IMongoCollection<User> userCollection = userPlaylistDatabase.GetCollection<User>("User");

            var update = Builders<User>.Update.Set(x => x.Username, "Josipa");       
            var rezultat = userCollection.FindOneAndUpdate<User>(x => x.Username == "User1", update);

            //update = Builders<User>.Update.Set(x => x.Username, "User1");
            //rezultat = userCollection.FindOneAndUpdate<User>(x => x.Username == "Josipa", update);

            update = Builders<User>.Update.Set(x => x.Username, "Student");
            rezultat = userCollection.FindOneAndUpdate<User>(x => x.Username == "User2", update);

            //update = Builders<User>.Update.Set(x => x.Username, "User2");
            //rezultat = userCollection.FindOneAndUpdate<User>(x => x.Username == "Student", update);
        }

        public static void FindRecords()
        {
            // Pronalazak zapisa

            //----IZVOĐAČI----//

            IMongoCollection<Artist> artistCollection = userPlaylistDatabase.GetCollection<Artist>("Artist");
           
            // Nađi izvođača po imenu
            Console.WriteLine("Svi izvođači imena \"Mark Markee\": ");

            artistCollection.Find(x => x.ArtistName == "Mark Markee")
                .ForEachAsync<Artist>(x => Console.WriteLine("{0} -> {1}", x.ArtistName, string.Join(", ", x.Genres)));
            Thread.Sleep(1000);

            Console.WriteLine();

            //----PLAYLISTE----//

            IMongoCollection<Playlist> playlistCollection = userPlaylistDatabase.GetCollection<Playlist>("Playlist");

            // Nađi playliste koje imaju određeni žanr
            Console.WriteLine("Sve playliste sa žanrom \"Rock\": ");

            playlistCollection.Find(x => x.Genres.Contains("Rock"))
                .ForEachAsync<Playlist>(x => Console.WriteLine("{0}", x.PlaylistName));
            Thread.Sleep(1000);

            Console.WriteLine();

            // Nađi playliste po određenom broju skladbi i broju dijeljenja
            Console.WriteLine("Sve playliste sa brojem skladbi većim od 3 i brojem dijeljenja manjim od 15: ");

            playlistCollection.Find(x => x.PerformanceCount > 3 && x.ShareCount < 15)
                .ForEachAsync<Playlist>(x => Console.WriteLine("{0} -> {1}", x.PlaylistName, x.PlaylistDuration));
            Thread.Sleep(1000);

            Console.WriteLine();

            //----SKLADBE----//

            IMongoCollection<Song> songCollection = userPlaylistDatabase.GetCollection<Song>("Song");

            // Nađi skladbe sa "Song" u imenu:
            Console.WriteLine("Sve skladbe sa \"Song\" u imenu: ");

            songCollection.Find(x => x.SongName.Contains("Song"))
                .ForEachAsync<Song>(x => Console.WriteLine("{0}", x.SongName));
            Thread.Sleep(1000);           
        }

        public void DeleteRecords()
        {
            // Brisanje zapisa

            IMongoCollection<Genre> genreCollection = userPlaylistDatabase.GetCollection<Genre>("Genre");
            genreCollection.DeleteOne<Genre>(x => x.GenreName == "Folk");
        }

        static void Main(string[] args)
        {
            GenerateData();
            //DatabaseConnect();
            //CreateCollections();

            //Console.WriteLine("Kolekcije stvorene!");

            //UpdateRecords();
            //FindRecords();
            //DeleteRecords();

            Console.ReadKey();
        }
    }
}
