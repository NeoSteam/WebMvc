using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel;
using System.Reflection;

namespace DBUtility
{
    public class MongoDBHelper
    {
        static string dbname = "logsdb";

        static string dburl = "mongodb://127.0.0.1:27017";
        public MongoServer server;
        public MongoDatabase db;
        public MongoDBHelper()
        {
            if (server != null && db != null) return;
            server = new MongoClient(dburl).GetServer();
            // server.Connect();
            db = server.GetDatabase(dbname);

        }

        public MongoDBHelper(string _dburl, string _dbname)
        {
            dbname = _dbname;
            dburl = _dburl;
            server = new MongoClient(dburl).GetServer();
            db = server.GetDatabase(dbname);
        }

        public void Insert<T>(T ent)
        {
            var collection = db.GetCollection(typeof(T).Name);
            collection.Insert<T>(ent);
            server.Disconnect();
        }

        public void Insert<T>(T ent, string table_name)
        {
            db.GetCollection(table_name).Insert<T>(ent);
            server.Disconnect();
        }

        public void Insert(BsonDocument ent, string table_name)
        {
            var collection = db.GetCollection(table_name);
            collection.Insert(ent);
            server.Disconnect();
        }

        public void InsertBatch(List<BsonDocument> ents, string table_name)
        {
            db.GetCollection(table_name).InsertBatch(ents);
            server.Disconnect();
        }

        public void InsertBatch<T>(List<T> ents, string table_name)
        {
            db.GetCollection(table_name).InsertBatch<T>(ents);
            server.Disconnect();
        }

        public void Save(BsonDocument ent, string table_name)
        {
            db.GetCollection(table_name).Save(ent);
            server.Disconnect();
        }

        public void Update<T>(T ent, QueryDocument query)
        {
            var collection = db.GetCollection(typeof(T).Name);
            var update = ent.SetUpdateDocument();
            collection.Update(query, update);
            server.Disconnect();
        }

        public void Delete<T>(QueryDocument query)
        {
            var collection = db.GetCollection(typeof(T).Name);
            collection.Remove(query);
            server.Disconnect();
        }

        public long Count<T>()
        {
            var collection = db.GetCollection(typeof(T).Name);
            var count = collection.Count();
            server.Disconnect();
            return count;
        }

        public long Count(string table_name, QueryDocument query)
        {
            var collection = db.GetCollection(table_name);
            return collection.Find(query).Count();
        }

        public T SelectOne<T>(QueryDocument query) where T : new()
        {
            var cols = db.GetCollection(typeof(T).Name);
            var ents = cols.FindOneAs<T>(query);
            server.Disconnect();
            return ents;
        }

        public List<T> List<T>(QueryDocument query)
        {
            var ents = db.GetCollection<T>(typeof(T).Name).Find(query).ToList();
            server.Disconnect();
            return ents;
        }

        public List<T> List<T>(QueryDocument query, string table_name)
        {
            var docs = db.GetCollection(table_name).Find(query);
            server.Disconnect();
            var _ls = (default(T)).SetDataList(docs);
            return _ls;
        }

        public List<T> List<T>(QueryDocument query, string table_name, int take_num)
        {
            var docs = db.GetCollection(table_name).Find(query).SetLimit(take_num);
            server.Disconnect();
            var _ls = (default(T)).SetDataList(docs);
            return _ls;
        }
    }

    public static class MongoExtend
    {
        public static UpdateDocument SetUpdateDocument<T>(this T ent)
        {
            var props = typeof(T).GetProperties();
            var query = new QueryDocument();
            foreach (PropertyInfo p in props)
            {
                query.Add(new BsonElement(p.Name, BsonValue.Create(p.GetValue(ent, null))));
            }
            return new UpdateDocument { { "$set", query } };
        }

        public static List<T> SetDataList<T>(this T ent, MongoCursor<BsonDocument> docs)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var arrs = new List<T>();
            foreach (var l in docs)
            {
                var newT = TypeDescriptor.CreateInstance(null, typeof(T), null, null);
                foreach (PropertyDescriptor p in properties)
                {
                    var value = l[p.Name].ToString();
                    if (value == null || string.IsNullOrEmpty(value.ToString())) continue;
                    if (!p.PropertyType.IsGenericType)
                    {
                        //非泛型
                        p.SetValue(newT, Convert.ChangeType(value, p.PropertyType));
                    }
                    else
                    {
                        //泛型Nullable<>
                        var genericTypeDefinition = p.PropertyType.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(Nullable<>))
                        {
                            p.SetValue(newT, Convert.ChangeType(value, Nullable.GetUnderlyingType(p.PropertyType)));
                        }
                    }
                }
                arrs.Add((T)newT);
            }
            return arrs;
        }
    }
}
