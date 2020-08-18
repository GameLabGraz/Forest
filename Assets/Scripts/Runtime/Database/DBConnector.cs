using System;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;

namespace GameLabGraz.DatabaseConnection
{
    public class DBConnector
    {
        private readonly SQLiteConnection _connection;

        public DBConnector(string databaseName)
        {
            _connection = new SQLiteConnection($"URI = file:{Application.persistentDataPath}/{databaseName}; datetimeformat={CultureInfo.CurrentCulture}");
        }

        public int InsertData(string tableName, params object[] data)
        {
            return InsertData(tableName, false, data);
        }

        public int ReplaceData(string tableName, params object[] data)
        {
            return InsertData(tableName, true, data);
        }

        private int InsertData(string tableName, bool replace, params object[] data)
        {

            var insertKey = replace ? "REPLACE" : "INSERT";

            var command = data.Aggregate($"{insertKey} INTO {tableName} VALUES (", 
                              (current, entry) =>
                              {
                                  switch (entry)
                                  {
                                      case null:
                                          return $"{current}NULL,";

                                      case float _:
                                          return FormattableString.Invariant($"{current}{entry},");
                                      
                                      case DateTime time:
                                          return $"{current}'{time:yyyy-MM-dd HH:mm:ss:ff}',";

                                      case string _:
                                          return $"{current}'{entry}',";

                                      default:
                                          return $"{current}{entry},";
                                  }
                              }).RemoveLast(",") + ");";

            return ExecuteNonQuery(command);
        }

        public int UpdateData(string tableName, string column, object value, string condition)
        {
            var command = $"UPDATE {tableName} SET {column} = ";

            switch (value)
            {
                case null:
                    command += "NULL";
                    break;
                case float _:
                    command += FormattableString.Invariant($"{value}");
                    break;
                case DateTime time:
                    command += $"'{time:yyyy-MM-dd HH:mm:ss:ff}'";
                    break;
                case string _:
                    command += $"'{value}'";
                    break;
                default:
                    command += $"{value}";
                    break;
            }
            command += $" WHERE {condition};";

            return ExecuteNonQuery(command);
        }


        public int ExecuteNonQuery(string command)
        {
            _connection.Open();

            var sqlCommand = _connection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = command;
            var result = sqlCommand.ExecuteNonQuery();

            _connection.Close();
            return result;
        }

        public object[][] ExecuteQuery(string query)
        {
            _connection.Open();

            var sqlCommand = _connection.CreateCommand();
            sqlCommand.CommandText = query;

            var reader = sqlCommand.ExecuteReader();
            var result = new List<object[]>();

            while (reader.Read())
            {
                var resultRow = new object[reader.FieldCount];
                reader.GetValues(resultRow);

                result.Add(resultRow);
            }

            _connection.Close();
            return result.ToArray();
        }
    }
}