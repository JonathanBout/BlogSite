using BlogSite.Data;
using LiteDB;
using Microsoft.AspNetCore.Mvc.ModelBinding;

//var db = new LiteDatabase("C:\\Users\\Public\\Documents\\database.db");
//ILiteCollection<Data> data = db.GetCollection<Data>();
//data.Insert(new Data(0, "Hello, World!"));
//data.Insert(new Data(0, "Hello, World the 2nd!"));
//var dataArray = data.FindAll().ToArray();
//Console.ReadLine();
//class Data
//{
//	public int Id { get; set; }
//	public string Content { get; set; }

//	[BsonCtor]
//	public Data(int id, string content)
//	{
//		Id = id;
//		Content = content;
//	}
//}



namespace BlogSite.Services
{
    public class Database : IDisposable
    {
        private LiteDatabase DB { get; set; }
        public Database()
        {
            DB = new LiteDatabase((OperatingSystem.IsWindows() ?
                Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\" : "/srv/") + "blogdata"
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


