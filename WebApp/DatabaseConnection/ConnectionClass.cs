using System;
using MySql.Data.MySqlClient;

namespace WebApp
{
    public class ConnectionClass
    {


        public ConnectionClass()
        {

        }
        public void Openconnection(MySqlConnection connectionToDb){
            try{
                connectionToDb.Open();
            }
            catch(MySqlException e){
                Console.WriteLine(e.Message);
            }
        }
        public void Dispose(MySqlConnection connectionToDb)
        {
            connectionToDb.Close();
        }
        public MySqlConnection CreateConnection(){
            MySqlConnection connectionToDb = new MySqlConnection("server = localhost; userid=root;password=P@$$w0rd;port=3306;database=PhotographShelf");
            return connectionToDb;
        }
    }
}
