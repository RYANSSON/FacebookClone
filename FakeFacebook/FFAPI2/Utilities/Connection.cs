using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class DBConnect
    {
        String SqlConnectString = "server=cis-mssql1.temple.edu;Database=FA18_3342_tuf97565;User id=tuf97565;Password=eegieYu7";
        //String SqlConnectString = "server=127.0.0.1,5555;Database=FA18_3342_tuf97565;User id=tuf97565;Password=eegieYu7";
        SqlConnection myConnectionSql;
        SqlCommand objCmd;
        SqlDataReader objDataReader;
        DataSet ds;

        public DBConnect()
        {
            myConnectionSql = new SqlConnection(SqlConnectString);
        }

        // This method is used to execute a SELECT SQL statement and retrieve a record set containing the results.
        // Inputs: a string containing a SELECT SQL statement. 
        // Returns: a DataSet containing the records found by the query. 
        // Note: The DataSet is also stored as a class variable for use in the GetField method
        public DataSet GetDataSet(String SqlSelect)
        {
            SqlDataAdapter myDataAdapter = new SqlDataAdapter(SqlSelect, myConnectionSql);
            DataSet myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet);
            ds = myDataSet;
            return myDataSet;
        }

        // This method is used to execute a SELECT SQL statement and retrieve a record set containing the results.
        // Inputs: (1) a string containing a SELECT SQL statement. 
        //         (2) an int variable output parameter used to store the number of records found by the query. 
        // Returns: a DataSet containing the records found by the query. 
        // Note: The DataSet is also stored as a class variable for use in the GetField method
        public DataSet GetDataSet(String SqlSelect, out int theRecordCount)
        {
            SqlDataAdapter myDataAdapter = new SqlDataAdapter(SqlSelect, myConnectionSql);
            DataSet myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet);
            ds = myDataSet;
            theRecordCount = ds.Tables[0].Rows.Count;
            return myDataSet;
        }

        // This method is used to execute a SELECT SQL statement and retrieve a record set containing the results.
        // Inputs: (1) a string containing a SELECT SQL statement. 
        //         (2) an int variable output parameter used to store the number of records found by the query.
        //         (3) a string variable output parameter used to store an error message when an exception occurs. 
        // Returns: a DataSet containing the records found by the query. 
        // Note: The DataSet is also stored as a class variable for use in the GetField method
        public DataSet GetDataSet(String SqlSelect, out int theRecordCount, out String theErrorMessage)
        {
            try
            {

                SqlDataAdapter myDataAdapter = new SqlDataAdapter(SqlSelect, myConnectionSql);
                DataSet myDataSet = new DataSet();
                myDataAdapter.Fill(myDataSet);
                ds = myDataSet;
                theRecordCount = ds.Tables[0].Rows.Count;
                theErrorMessage = "";
                return myDataSet;
            }
            catch (Exception ex)
            {
                theRecordCount = 0;
                theErrorMessage = ex.Message;
                return null;
            }
        }

        // This method is used to execute an INSERT, UPDATE, or DELETE SQL statement.
        // Inputs: a string containing a SQL statement. 
        // Returns: the number of rows affected by the SQL statement, or -1 when an exception occurs. 
        public int DoUpdate(String SqlManipulate)
        {
            objCmd = new SqlCommand(SqlManipulate, myConnectionSql);
            try
            {
                myConnectionSql.Open();
                int ret = objCmd.ExecuteNonQuery();
                myConnectionSql.Close();
                return ret;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        // This method is used to execute Stored Procedures that use a INSERT, UPDATE, or DELETE SQL statement
        // Inputs: a Command object setup to use a Stored Procedure (requires the use of CommandText, CommandType, and Parameters properties). 
        // Returns: the number of rows affected by the update, or -1 when an exception occurs.
        public int DoUpdateUsingCmdObj(SqlCommand theCommandObject)
        {
            try
            {
                theCommandObject.Connection = myConnectionSql;
                theCommandObject.Connection.Open();
                int ret = theCommandObject.ExecuteNonQuery();
                theCommandObject.Connection.Close();
                return ret;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        // This method is used to execute Stored Procedures that use a SELECT SQL statement with or without SQL Parameters.
        // Inputs: a Command object setup to use a Stored Procedure (requires the use of CommandText, CommandType, and Parameters properties). 
        // Returns: a DataSet containing the results of the query executed by a Stored Procedure.
        public DataSet GetDataSetUsingCmdObj(SqlCommand theCommand)
        {
            theCommand.Connection = myConnectionSql;
            SqlDataAdapter myDataAdapter = new SqlDataAdapter(theCommand);
            DataSet myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet);
            ds = myDataSet;

            return myDataSet;
        }

        // This method is used to retrieve a row from a DataSet.
        // Inputs: (1) the DataSet used for row retrieval.
        //         (2) the zero-based index of the row to retrieve from the DataSet.
        // Returns: a DataRow from the input DataSet
        public DataRow GetRow(DataSet theDataSet, int theRow)
        {
            DataTable objTable = ds.Tables[0];
            DataRow objRow = objTable.Rows[theRow];
            return objRow;
        }

        // This method is used to find rows in the local DataSet based on some condition.
        // Inputs: a string representing the filter criteria (DataView RowFilter Syntax required).
        // Returns: an array of DataRows that match the filter criteria.
        public Array GetRows(String theCondition)
        {
            DataRow[] objRow;
            DataTable objTable = ds.Tables[0];
            objRow = objTable.Select(theCondition);
            return objRow;
        }


        // This method is used to retrieve a single field's value for a record in the local DataSet.
        // Inputs: (1) the field name.
        //         (2) the zero-based index of the row (record) in the DataSet from which the field is to be extracted.
        // Returns: an object representing the field's data value.
        // Note: this method assumes that one of the GetDataSet methods were previously called; 
        //       otherwise, the local DataSet doesn't contain anything.
        public Object GetField(String theFieldName, int theRow)
        {
            DataTable objTable = ds.Tables[0];
            DataRow objRow = objTable.Rows[theRow];
            return objRow[theFieldName];
        }

        // This method is used to update the database based on the changes made to the data in a DataSet.
        // Inputs: the DataSet used to update the database.
        public void CommitDataSet(DataSet theDataSet)
        {
            SqlDataAdapter myDataAdapter = new SqlDataAdapter();
            myDataAdapter.Update(theDataSet);
        }

        // This method is used to execute a query and retrieve a single value.
        // Inputs: the Command object setup to execute a scalar function.
        // Returns: a single scalar value (the first column of the first row of a query) as an object.
        public Object ExecuteScalarFunction(SqlCommand theCommand)
        {
            theCommand.Connection = myConnectionSql;
            return theCommand.ExecuteScalar();
        }

        // This method is used to retrieve the Connection object, which can be used for live connection operations like
        // getting a DataReader. The methods in this class properly open and close the connection as needed. 
        // If the Connection object is used outside of this class' method, you must properly open and close the connection manually.
        public SqlConnection GetConnection()
        {
            return myConnectionSql;
        }

        // This method is used to close an open connection to the database. The methods in this class properly open and close the connection as needed.
        // Therefore, this method was included as a utility for closing the connection when the Connection object is worked with directly
        // upon calling the GetConnection method.
        public void CloseConnection()
        {
            try
            {
                myConnectionSql.Close();
            }
            catch (Exception ex)
            {
                // Catch exception created when trying to close a closed connection.
            }
        }

        // The Deconstructor
        ~DBConnect()
        {
            // Close any open connections to the database before the objects of this class
            // are garbage collected.
            try
            {
                myConnectionSql.Close();
            }
            catch (Exception ex)
            {
                // Catch exception created when trying to close a closed connection.
            }
        }

    }   // end class
}   // end namespace
