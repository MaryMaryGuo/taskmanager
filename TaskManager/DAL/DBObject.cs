using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//for this project
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TaskManager.DAL
{
    public class DBObject
    {
        //private class fields
        private String _connectionString;
        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlTransaction _transaction;

        //constructor

        public DBObject()
        {
            
            try
            {
                _connectionString = "Data Source=HOME;Initial Catalog=TaskManager;Integrated Security = True";
                _connection = new SqlConnection(_connectionString);

                _connection.Open();

                
                _command = _connection.CreateCommand();
                //will open connection then use try catch
          
                _transaction = _connection.BeginTransaction();
                _command.Transaction = _transaction;

            }
            catch (Exception ex)
            {
                //log error
                throw new Exception("Connection data error occured! Please contact Application Support.", ex);
            }
        }

        //execute transaction nonquery
        public void ExecuteTransaction(string sql, List<IDataParameter> pars, CommandType commType, bool commitTransaction)
        {
            try
            {
                _command.CommandText = sql;
                _command.CommandType = commType;
                //clear the parameters
                _command.Parameters.Clear();
                if (pars != null)
                {
                    _command.Parameters.AddRange(pars.ToArray());

                }
                _command.ExecuteNonQuery();
                //commit the transaction
                if (commitTransaction)
                {
                    _transaction.Commit();
                    _connection.Close();
                }

            }
            catch (Exception ex)
            {
                //rollback the transaction
                _transaction.Rollback();
                //log error
                throw new Exception("Execute Nonquery error occured! Please contact Application Support.", ex);
            }
        }

        //execute a query to the datareader
        public IDataReader ExecuteQuery(string sql, List<IDataParameter> pars, CommandType commType, bool commitTranscation)
        {
            try
            {
                _transaction = null;

                _command.CommandText = sql;
                _command.CommandType = commType;
                _command.Parameters.Clear();

                if (pars != null)
                {
                    _command.Parameters.AddRange(pars.ToArray());
                }

                var reader = _command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;

            }
            catch (Exception ex)
            {
                //log error
                throw new Exception("Execute Query data error occured! Please contact Application Support.", ex);
            }
        }

        //execute a scalar
        public object ExecuteScalar(string sql, List<IDataParameter> pars, CommandType commType, bool commitTranscation)
        {
            object result = null;
            try
            {
                _command.CommandText = sql;
                _command.CommandType = commType;
                _command.Parameters.Clear();

                if (pars != null)
                {
                    _command.Parameters.AddRange(pars.ToArray());
                }

                //executescalar return singler values first field of the result
                result = _command.ExecuteScalar();
                if (commitTranscation)
                {
                    _transaction.Commit();
                    _connection.Close();
                }

            }
            catch (Exception ex)
            {
                //log error
                throw new Exception("Execute scalar error occured! Please contact Application Support.", ex);
            }

            return result;
        }

        public IDataParameter CreateParameter
        {
            get
            {
                return new SqlParameter();
            }
        }
    }
}
