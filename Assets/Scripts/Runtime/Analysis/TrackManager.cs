using System;
using System.Collections.Generic;
using System.Linq;
using GameLabGraz.DatabaseConnection;
using UnityEngine;

namespace GameLabGraz.ImmersiveAnalytics
{
    public struct TrackEntry
    {
        public DateTime Time;
        public Vector3 Position;
        public Vector3 Rotation;
    }

    public class TrackManager : MonoBehaviour
    {
        private const string CreatePlayerTable = "CREATE TABLE IF NOT EXISTS Players (" +
                                                 "PlayerID INTEGER PRIMARY KEY AUTOINCREMENT," +
                                                 "Name TEXT," +
                                                 "StartTime TEXT," +
                                                 "EndTime TEXT);";

        private const string CreateTrackableObjectTable = "CREATE TABLE IF NOT EXISTS TrackableObjects (" +
                                                          "ObjectID INTEGER NOT NULL UNIQUE," +
                                                          "Name TEXT," +
                                                          "PRIMARY KEY (ObjectID));";

        private const string CreateRecordTable = "CREATE TABLE IF NOT EXISTS Records (" +
                                                 "PlayerID INTEGER," +
                                                 "ObjectID INTEGER," +
                                                 "Time TEXT," +
                                                 "PositionX FLOAT," +
                                                 "PositionY FLOAT," +
                                                 "PositionZ FLOAT," +
                                                 "RotationX FLOAT," +
                                                 "RotationY FLOAT," +
                                                 "RotationZ FLOAT," +
                                                 "FOREIGN KEY (PlayerID) REFERENCES Players(PlayerID)," +
                                                 "FOREIGN KEY (ObjectID) REFERENCES TrackableObjects(ObjectID));";

        [SerializeField] private string databaseName = "MyDatabase";

        private DBConnector _databaseConnector;

        private DBConnector DatabaseConnector
        {
            get
            {
                if (_databaseConnector == null)
                {
                    _databaseConnector = new DBConnector(databaseName);
                    _databaseConnector.ExecuteNonQuery(CreatePlayerTable);
                    _databaseConnector.ExecuteNonQuery(CreateTrackableObjectTable);
                    _databaseConnector.ExecuteNonQuery(CreateRecordTable);
                }
                return _databaseConnector;
            }
        }


        private PlayerTracker _player;

        private readonly List<TrackableObject> _trackableObjects = new List<TrackableObject>();

        private float _time;

        private static TrackManager _instance;

        public static TrackManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<TrackManager>();
                return _instance;
            }
        }

        public void RegisterPlayer(PlayerTracker player)
        {
            _player = player;

            DatabaseConnector.InsertData("Players", 
                null, 
                player.name,
                DateTime.Now,
                null
            );

            _player.Id = (long)DatabaseConnector.ExecuteQuery(
                "SELECT * FROM Players").Last()[0];
        }

        public void UnregisterPlayer() 
        {
            DatabaseConnector.UpdateData("Players",
                "EndTime", 
                DateTime.Now,
                $"playerID={_player.Id}");
            _player = null;
        }

        public void RegisterTrackableObject(TrackableObject obj)
        {
            _trackableObjects.Add(obj);

            DatabaseConnector.ReplaceData("TrackableObjects", 
                obj.Id, 
                obj.name);
        }

        public void UnregisterTrackableObject(TrackableObject obj)
        {
            _trackableObjects.Remove(obj);
        }

        public DateTime GetStartTime(int playerId)
        {
            var dbResult = DatabaseConnector.ExecuteQuery(
                $"SELECT StartTime FROM Players WHERE PlayerID=={playerId}");
            return DateTime.ParseExact((string)dbResult[0][0], "yyyy-MM-dd HH:mm:ss:ff", null);
        }

        public DateTime GetEndTime(int playerId)
        {
            var dbResult = DatabaseConnector.ExecuteQuery(
                $"SELECT EndTime FROM Players WHERE PlayerID=={playerId}");
            return DateTime.ParseExact((string)dbResult[0][0], "yyyy-MM-dd HH:mm:ss:ff", null);
        }

        public List<TrackEntry> GetTrackRecords(int playerId, int objectId)
        {
            var dbResult = DatabaseConnector.ExecuteQuery(
                "SELECT Time, PositionX, PositionY, PositionZ, RotationX, RotationY, RotationZ " +
                $"FROM Records WHERE PlayerID=={playerId} AND ObjectID=={objectId}");

            return dbResult.Select(entry => new TrackEntry()
            {
                Time = DateTime.ParseExact((string)entry[0], "yyyy-MM-dd HH:mm:ss:ff", null),

                Position = new Vector3(
                    Convert.ToSingle(entry[1]), 
                    Convert.ToSingle(entry[2]), 
                    Convert.ToSingle(entry[3])), 

                Rotation = new Vector3(
                    Convert.ToSingle(entry[4]), 
                    Convert.ToSingle(entry[5]), 
                    Convert.ToSingle(entry[6]))
            }).ToList();
        }

        private void Update()
        {
            /*
            _time += Time.deltaTime;
            if (_time < 1.0f) return;
            _time = 0.0f;
            */

            if(_player == null) return;

            foreach (var trackableObject in _trackableObjects.Where(trackableObject => trackableObject.transform.hasChanged))
            {
                var objectPosition = trackableObject.transform.position;
                var objectRotation = trackableObject.transform.rotation.eulerAngles;

                DatabaseConnector.InsertData("Records",
                    _player.Id,
                    trackableObject.Id,
                    DateTime.Now,
                    objectPosition.x,
                    objectPosition.y,
                    objectPosition.z,
                    objectRotation.x,
                    objectRotation.y,
                    objectRotation.z
                );

                trackableObject.transform.hasChanged = false;
            }
        }
    }
}