using System.Data;
using System.Data.SqlClient;

namespace Test
{
    class SqlHelper
    {
        private static string connStr;

        public static DataSet getList(string subject)
        {
            connStr = "server=202.118.26.80;database="+subject+";uid=sa;pwd=316227cafb@123";
            SqlConnection sqlCnt = new SqlConnection(connStr);
            sqlCnt.Open();

            SqlCommand mySqlCommand = new SqlCommand();
            mySqlCommand.CommandType = CommandType.Text;
            mySqlCommand.CommandText = "select * from Chapter";
            mySqlCommand.Connection = sqlCnt;

            SqlDataAdapter myDataAdapter = new SqlDataAdapter("select * from Chapter", sqlCnt);
            DataSet myDataSet = new DataSet();      // 创建DataSet
            myDataAdapter.Fill(myDataSet, "Chapter");	// 将返回的数据集作为“表”填入DataSet中，表名可以与数据库真实的表名不同，并不影响后续的增、删、改等操作
            return myDataSet;
        }

        //public static DataTable ExecuteDataTable(string sql, params SqlParameter[] parameter)
        //{
        //    DataTable table;
        //    DataSet dataSet = new DataSet();
        //    using (SqlConnection connection = new SqlConnection(connStr))
        //    {
        //        using (SqlCommand command = new SqlCommand(sql, connection))
        //        {
        //            command.Parameters.AddRange(parameter);
        //            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
        //            {
        //                adapter.Fill(dataSet);
        //                table = dataSet.Tables[0];
        //            }
        //        }
        //    }
        //    return table;
        //}

        //public static int ExecuteNonQuery(string sql, params SqlParameter[] parameter)
        //{
        //    int num;
        //    using (SqlConnection connection = new SqlConnection(connStr))
        //    {
        //        using (SqlCommand command = new SqlCommand())
        //        {
        //            command.Connection = connection;
        //            command.Parameters.AddRange(parameter);
        //            command.CommandText = sql;
        //            connection.Open();
        //            num = command.ExecuteNonQuery();
        //        }
        //    }
        //    return num;
        //}

        //public static int ExecuteSacalar(string sql, params SqlParameter[] parameter)
        //{
        //    int num2;
        //    using (SqlConnection connection = new SqlConnection(connStr))
        //    {
        //        using (SqlCommand command = connection.CreateCommand())
        //        {
        //            num2 = (int)command.ExecuteScalar();
        //        }
        //    }
        //    return num2;
        //}
    }
}
