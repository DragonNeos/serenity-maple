using MySql.Data.MySqlClient;
using Serenity.Other;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Database
{
    public class MySQL_Connection
    {
        private MySqlConnection Connection { get; set; }
        private MySqlCommand Command { get; set; }
        public MySqlDataReader Reader { get; set; }
        private string mConnectString { get; set; }
        public bool mShuttingDown { get; set; }

        public MySQL_Connection(string pUsername, string pPassword, string pDatabase, string pServer, ushort pPort = 3306)
        {
            mShuttingDown = false;
            mConnectString = "Server=" + pServer + "; Port=" + pPort + "; Database=" + pDatabase + "; Uid=" + pUsername + "; Pwd=" + pPassword;
            Connect();
        }

        public void Connect()
        {
            try
            {
                Connection = new MySqlConnection(mConnectString);
                Connection.StateChange += new System.Data.StateChangeEventHandler(Connection_StateChange);
                Connection.Open();
            }
            catch (MySqlException ex)
            {
                //
            }
            catch (Exception ex)
            {
                //
            }
        }

        void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            if (e.CurrentState == System.Data.ConnectionState.Closed && !mShuttingDown)
            {
                Connection.StateChange -= Connection_StateChange;
                Logger.WriteLog(Logger.LogTypes.Error, "Unable to connect to MySql, attempting to reconnect.");
                Connect();
            }
            else if (e.CurrentState == System.Data.ConnectionState.Open)
            {
                //
            }
        }

        public int RunQuery(string query)
        {
            try
            {

                if (Reader != null && !Reader.IsClosed)
                {
                    Reader.Close();
                    Reader.Dispose();
                    Reader = null;
                }
                Command = new MySqlCommand(query, Connection);
                if (query.StartsWith("SELECT"))
                {
                    Reader = Command.ExecuteReader();
                    return Reader.HasRows ? 1 : 0;
                }
                else if (query.StartsWith("DELETE") || query.StartsWith("UPDATE") || query.StartsWith("INSERT"))
                    return Command.ExecuteNonQuery();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Lost connection to DB... Trying to reconnect and wait a second before retrying to run query.");
                Connect();
                System.Threading.Thread.Sleep(1000);
                RunQuery(query);
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 2055)
                {
                    Console.WriteLine("Lost connection to DB... Trying to reconnect and wait a second before retrying to run query.");
                    Connect();
                    System.Threading.Thread.Sleep(1000);
                    RunQuery(query);
                }
                else
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine(query);
                    throw new Exception(string.Format("[{0}][DB LIB] Got exception @ MySQL_Connection::RunQuery({1}) : {2}", DateTime.Now.ToString(), query, ex.ToString()));
                }
            }
            return 0;
        }

        public int GetLastInsertId()
        {
            return (int)Command.LastInsertedId;
        }

        public bool Ping()
        {
            if (Reader != null && !Reader.IsClosed)
                return false;
            return Connection.Ping();
        }

        public bool NameTaken(string name)
        {
            this.RunQuery("SELECT * FROM characters WHERE name = '" + MySqlHelper.EscapeString(name) + "'");
            MySqlDataReader reader = this.Reader;
            reader.Read();

            if (!reader.HasRows)
            {
                reader.Close();
                return false;
            }

            reader.Close();
            return true;
        }
    }
}
