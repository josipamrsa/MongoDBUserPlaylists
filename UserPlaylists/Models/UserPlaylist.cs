using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading;

namespace UserPlaylists.Models
{
    public class User
    {
        public ObjectId _id { get; set; }
        public string Username { get; set; }
    }

    public class Playlist
    {
        public ObjectId _id { get; set; }
        public string PlaylistName { get; set; }
        public string PlaylistDuration { get; set; }
        public int PerformanceCount { get; set; }
        public int ShareCount { get; set; }
        public IEnumerable<string> Genres { get; set; }
    }

    public class Song
    {
        public ObjectId _id { get; set; }
        public string SongName { get; set; }
        public string SongDuration { get; set; }
    }

    public class Artist
    {
        public ObjectId _id { get; set; }
        public string ArtistName { get; set; }
        public IEnumerable<string> Genres { get; set; }
    }

    public class Genre
    {
        public ObjectId _id { get; set; }
        public string GenreName { get; set; }
    }
   
    public class UserPlaylistData
    {
        public List<Artist> Artists { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Playlist> Playlists { get; set; }
        public List<Song> Songs { get; set; }
        public List<User> Users { get; set; }
    }
}
