using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace QuestMaster
{
    class BDConnection
    {
        string serverUrl;//Хост
        string username;//Имя пользователя
        string port;// ОН нужен!?
        string password;//Пароль
        string DBName;//Имя Базы данных

        public BDConnection(string url, string username, string port, string password, string DBName)
        {
            this.serverUrl = url;
            this.username = username;
            this.port = port;
            this.password = password;
            this.DBName = DBName;

            MySqlConnectionStringBuilder link = new MySqlConnectionStringBuilder();
            link.Server = url;
            link.UserID = username;
            link.Password = password;
            link.Database = DBName;
        }
    }
}
