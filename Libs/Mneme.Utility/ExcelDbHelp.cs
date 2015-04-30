using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mneme.Utility
{
    public class ExcelDbHelp
    {
        public static string CreateExcelConnectionString(string file)
        {
            string connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;" + 
                "Data Source={0};" + "Extended Properties=" + (char)34 + 
                "Excel 12.0 Xml;HDR=YES;" + (char)34, file);


            //string connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;" +
            //        "Data Source={0};Extended Properties='Excel 12.0;HDR=YES;IMEX=0'", file);

            return connectionString;
        }

        public static void ExecuteCommand(string file, OleDbCommand command, int colCount, List<object> dataList)
        {
            System.Data.OleDb.OleDbConnection MyConnection;

            //System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();

            string connectionString = ExcelDbHelp.CreateExcelConnectionString(file);

            MyConnection = new OleDbConnection(connectionString);
            MyConnection.Open();
            DbTransaction transaction = MyConnection.BeginTransaction();

            command.Connection = MyConnection;
            command.Transaction = (OleDbTransaction)transaction;

            int count = 0;
            while (count < dataList.Count)
            {
                for (int i = 0; i < colCount; i++)
                {
                    command.Parameters[i].Value = dataList[count];
                    count++;
                }
                command.ExecuteNonQuery();
            }

            transaction.Commit();

            MyConnection.Close();
        }
        public static void ExecuteCommand(string file, string commandText, List<string> headers, List<object> dataList)
        {
            System.Data.OleDb.OleDbConnection MyConnection;

            //System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();

            string connectionString = ExcelDbHelp.CreateExcelConnectionString(file);

            MyConnection = new OleDbConnection(connectionString);
            MyConnection.Open();
            DbTransaction transaction = MyConnection.BeginTransaction();

            OleDbCommand command = new OleDbCommand();
            command.Connection = MyConnection;
            command.CommandText = commandText;
            command.Transaction = (OleDbTransaction)transaction;

            foreach (string header in headers)
            {
                string s = "" + "";
                command.Parameters.Add(new OleDbParameter() { ParameterName = "@" + header });
            }
            int count = 0;
            while (count < dataList.Count)
            {
                for (int i = 0; i < headers.Count; i++)
                {
                    command.Parameters[i].Value = dataList[count];
                    count++;
                }
                command.ExecuteNonQuery();
            }

            transaction.Commit();

            MyConnection.Close();
        }
    }
}
