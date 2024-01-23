using System.Reflection.Metadata.Ecma335;
using System.Data.SqlClient;
using System;



namespace Core
{
    public class DataConnection
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;

        public SqlDataReader Reader
        {
            get { return reader; }
        }

        public DataConnection()
        {
            connection = new SqlConnection("server = .\\SQLEXPRESS; database= SGCodeChallenge; integrated security=true");
            command = new SqlCommand();
        }

        public void setQuery(string query)
        {
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;
        }

        public void read()
        {
            command.Connection = connection;
            try
            {
                connection.Open();
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void execute()
        {
            command.Connection = connection;
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setParameter(string name, object value)
        {
            command.Parameters.AddWithValue(name, value ?? DBNull.Value);
        }

        public void closeConnection()
        {
            if (reader != null)
                reader.Close();
            connection.Close();
        }
    }

}