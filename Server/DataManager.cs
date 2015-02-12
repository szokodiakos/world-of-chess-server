using CommonUtils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Server
{
    class DataManager
    {
        public static object locker = new object();

        private bool connectionStringLoaded = false;
        private static String connectionString = "server=localhost;user=root;password=root;database=World;port=3306;";
        private static MySqlConnection conn = null;

        private void InitConnectionString()
        {
            if (!connectionStringLoaded)
            {
                try
                {
                    connectionString = System.IO.File.ReadAllText("connection.cfg");
                    Log.Verbose("Loaded connection string from file.");
                }
                catch (Exception exc)
                {
                    Log.Warning("Failed to load connectionstring form 'connection.cfg'", exc);
                }
            }
        }

        private static void InitLog()
        {
            if (!Log.IsLoggerInitialized())
            { Log.Init("ServerLog.log", Log.LogLevel.Verbose); }
        }

        private static bool IsConnectionUp()
        {
            if (conn == null) return false;

            return conn.State == System.Data.ConnectionState.Open;
        }

        private static void Connect()
        {
            InitLog();

            

            if (conn == null)
            {
                conn = new MySqlConnection(connectionString);
            }

            Log.Verbose("Connecting to DB server. Connection string: " + connectionString);

            if (conn.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            else
            {
                Log.Verbose(String.Format("DB Connection was {0}! Connection cancelled.", conn.State));
            }
        }

        private static void Disconnect()
        {
            InitLog();

            if (conn == null)
            {
                Log.Verbose("DB Connection was not open! Disconnection cancelled.");
                return;
            }

            if (conn.State != System.Data.ConnectionState.Closed)
            {
                try
                {
                    conn.Close();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            else
            {
                Log.Verbose(String.Format("DB Connection was {0}! Disconnect cancelled.", conn.State));
            }
        }

        public static bool IsPlayerInDb(String username)
        {
            bool ret = false;

            lock (locker)
            {
                Connect();

                try
                {
                    string sql = "SELECT COUNT(*) FROM Players WHERE Name = @Username";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Username", username);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        int r = Convert.ToInt32(result);
                        ret = r != 0;

                        if (r > 1) { Log.Warning(String.Format("More than 1 records of {0} in table Players!", username)); }
                    }
                    else
                    {
                        throw new NullReferenceException("Result from DB was null!");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Disconnect();
                }
            }

            return ret;
        }

        public static int GetUserRating(String username)
        {
            int ret = Rules.DefaultRanking;
            lock (locker)
            {
                Connect();

                try
                {
                    string sql = "SELECT rating FROM Players WHERE Name = @Username";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Username", username);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        int r = Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new NullReferenceException("Result from DB was null!");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Disconnect();
                }
            }
            return ret;
        }
        /// <summary>
        /// Sets User Rating
        /// </summary>
        /// <param name="username">name of the player</param>
        /// <param name="rating">rating of the player</param>
        public static void SetUserRating(String username, int rating)
        {
            lock (locker)
            {
                Connect();

                try
                {
                    string sql = "UPDATE Players SET rating = @Rating WHERE name = @Username";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    int result = cmd.ExecuteNonQuery();
                    Log.Verbose("Executed 'SetUserRating'. Return code was: " + result);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Disconnect();
                }
            }
        }
        /// <summary>
        /// Adds a new 
        /// </summary>
        /// <param name="username"></param>
        public static void AddUser(String username)
        {
            lock (locker)
            {
                Connect();

                try
                {
                    string sql = "INSERT INTO Players (name, rating) VALUES (@Username, @DefaultRanking)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@DefaultRanking", Rules.DefaultRanking);
                    cmd.Parameters.AddWithValue("@Username", username);
                    int result = cmd.ExecuteNonQuery();
                    Log.Verbose("Executed 'AddUser'. Return code was: " + result);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Disconnect();
                }
            }
        }

        public static List<String> GetHighscore(int limit = 7)
        {
            List<String> ret = null;

            lock (locker)
            {
                Connect();

                try
                {
                    string sql = "SELECT name, rating FROM Players ORDER BY rating DESC LIMIT @Limit";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Limit", limit);

                    var rdr = cmd.ExecuteReader();
                    List<string> highscoreTable = new List<string>();
                    while (rdr.Read())
                    {
                        highscoreTable.Add(rdr.GetString(0));
                        highscoreTable.Add(rdr.GetInt32(1).ToString());
                    }
                    ret = highscoreTable;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Disconnect();
                }
            }

            return ret;
        }

        public static void ResetDB()
        {
            lock (locker)
            {
                Connect();

                try
                {
                    string sql = @"DROP DATABASE World;

CREATE DATABASE World;

USE World;

CREATE TABLE Players (
         id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
         name VARCHAR(100),
		 rating INT DEFAULT @DefaultRanking
       );";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@DefaultRanking", Rules.DefaultRanking);
                    int result = cmd.ExecuteNonQuery();
                    Log.Verbose("Executed 'AddUser'. Return code was: " + result);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Disconnect();
                }
            }
        }

    }
}
