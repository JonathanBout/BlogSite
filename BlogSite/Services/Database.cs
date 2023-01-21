using LiteDB;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;

namespace BlogSite.Data
{
    public class Database : IDisposable
    {
        private LiteDatabase DB { get; set; }
        public Database()
        {
            DB = new LiteDatabase((OperatingSystem.IsWindows() ?
                Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\" : "/srv/")
                + Assembly.GetExecutingAssembly().GetName().Name
                + "-Database"
#if DEBUG
                + ".debug"
#endif
                + ".db"
            );
        }

        public T GetItem<T>(int id, string collectionName = "") where T : IId
        {
            var collection = GetCollection<T>(collectionName);
            return collection.FindById(id);
        }

        public int Insert<T>(T item, string collectionName = "") where T : IId
        {
            var collection = GetCollection<T>(collectionName);
            return collection.Insert(item);

        }

        public bool Delete<T>(int id, string collectionName = "") where T : IId
        {
            var collection = GetCollection<T>(collectionName);
            return collection.Delete(id);

        }

        public int Upsert<T>(T item, string collectionName = "") where T : IId
        {
            var collection = GetCollection<T>(collectionName);
            if (collection.Upsert(item))
            {
                return item.Id;
            }
            return -1;
        }

        public ILiteCollection<T> GetCollection<T>(string collectionName = "") where T : IId
        {
            if (string.IsNullOrWhiteSpace(collectionName))
            {
                return DB.GetCollection<T>();
            }
            return DB.GetCollection<T>(collectionName);
        }

        public void Dispose()
        {
            DB.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}


