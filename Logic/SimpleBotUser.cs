using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleBot.Logic
{
    public class SimpleBotUser
    {
        public string Reply(SimpleMessage message)
        {
            return $"{message.User} disse '{message.Text}";
        }

        //Salvar Histórico
        public void SaveData(SimpleMessage message)
        {
            MongoClient client;
            IMongoDatabase db;
            IMongoCollection<BsonDocument> col;
            BsonDocument doc;
            UserProfile profile;

            client = new MongoClient();
            db = client.GetDatabase("SimpleBot");
            col = db.GetCollection<BsonDocument>("Conversation");

            doc = new BsonDocument()
            {
                {"User", message.User },
                {"Text", message.Text },
                {"UserFromId",  message.Id}
            };

            col.InsertOne(doc);
         
            profile = GetProfile(message.Id);
            profile.Contador += 1; 
            SetProfile(profile);

        }

        private void SetProfile(UserProfile profile)
        {
            MongoClient client;
            IMongoDatabase db;
            IMongoCollection<BsonDocument> col;      


            client = new MongoClient();
            db = client.GetDatabase("SimpleBot");
            col = db.GetCollection<BsonDocument>("UserProfile");

            //Update
        }

        private UserProfile GetProfile(string id)
        {
            MongoClient client;
            IMongoDatabase db;
            IMongoCollection<BsonDocument> col;         
            UserProfile profile = null;
            List<BsonDocument> bsonElements;

            client = new MongoClient();
            db = client.GetDatabase("SimpleBot");
            col = db.GetCollection<BsonDocument>("UserProfile");

            bsonElements = col.Find(Builders<BsonDocument>.Filter.Eq("id", id)).ToList();

            if (bsonElements.Count > 0)
            {
                profile = new UserProfile
                {
                    Id = bsonElements[0].ToString(),
                    Contador = int.Parse(bsonElements[1].ToString())
                };
            }
            else
            {
                profile = new UserProfile
                {
                    Id = id,
                    Contador = 0
                };
            }

            return profile;
        }
    }
}