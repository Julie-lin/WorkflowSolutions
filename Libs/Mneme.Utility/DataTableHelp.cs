using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;


namespace Mneme.Utility
{
    public class DataTableHelp<T>
    {
        public static System.Data.DataTable CreateDataTable(Type types, ref List<String> names, List<String> excludeNames)
        {
            DataTable dt = new DataTable();
            System.Reflection.PropertyInfo[] propertyInfos;
            propertyInfos = types.GetProperties(
                  BindingFlags.Public  // Get public and non-public
                | BindingFlags.Instance  // Get instance + static
                | BindingFlags.FlattenHierarchy); // Search up the hierarchy

            //List<String> names = new List<string>();
            dt.Columns.Add(new DataColumn("N", typeof(int)));
            try
            {
                foreach (PropertyInfo p in propertyInfos)
                {
                    Type t = p.PropertyType;
                    if (!t.IsPrimitive && !t.Equals(typeof(String)))
                    {
                        continue;
                    }
                    String n = p.Name;
                    if (!excludeNames.Contains(n))
                    {
                        dt.Columns.Add(new DataColumn(n, t));
                        names.Add(n);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        //calling example
        //dt = DataTableHelp<TruePositives>.CreateDataTable(typeof(TruePositives), ref names);
        //DataTableHelp<TruePositives>.PopulateDataTable(peaks, dt, names);
        public static System.Data.DataTable CreateDataTable(Type types, ref List<String> names)
        {
            DataTable dt = new DataTable();
            System.Reflection.PropertyInfo[] propertyInfos;
            propertyInfos = types.GetProperties(
                  BindingFlags.Public  // Get public and non-public
                | BindingFlags.Instance  // Get instance + static
                | BindingFlags.FlattenHierarchy); // Search up the hierarchy

            //List<String> names = new List<string>();
            dt.Columns.Add(new DataColumn("N", typeof(int)));
            try
            {
                foreach (PropertyInfo p in propertyInfos)
                {
                    Type t = p.PropertyType;
                    if (!t.IsPrimitive && !t.Equals(typeof(String)))
                    {
                        continue;
                    }
                    String n = p.Name;
                    dt.Columns.Add(new DataColumn(n, t));
                    names.Add(n);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public static void PopulateDataTable(List<T> list, DataTable dt, List<String> names)
        {
            int i = 1;
            foreach (var tt in list)
            {
                DataRow row = dt.NewRow();
                row["N"] = i++;
                foreach (String n in names)
                {
                    try
                    {
                        object x = tt.GetType().GetProperty(n).GetValue(tt, null);
                        row[n] = x;
                    }
                    catch
                    {

                    }
                }
                dt.Rows.Add(row);
            }

        }
    }
}
