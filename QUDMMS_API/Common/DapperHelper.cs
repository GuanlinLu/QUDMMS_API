using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QUDMMSAPI.Common
{
    public class DapperHelper
    {
        public string ConnectionString { get; }

        private static IDbConnection CreateConnection()
        {
            IDbConnection Connection = null;

            Connection = new MySqlConnection(ConfigHelper.GetConfigRoot().GetSection("SqlConnectionStrings:QUDMMS_DB").Value);

            return Connection;
        }

        /// <summary>
        /// 执行sql，返回影响行数--异步
        /// </summary>
        /// <param name="DataBaseName"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteSqlIntAsync(string Sql, object Param = null, IDbTransaction Transaction = null)
        {
            if (Transaction == null)
            {
                using (IDbConnection conn = CreateConnection())
                {
                    conn.Open();

                    return await conn.ExecuteAsync(Sql, Param, commandTimeout: 60, commandType: CommandType.Text);
                }
            }
            else
            {
                var conn = Transaction.Connection;

                return await conn.ExecuteAsync(Sql, Param, transaction: Transaction, commandTimeout: 60, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 执行sql，返回datatable
        /// </summary>
        public static async Task<DataTable> ExecuteSqlDataTableAsync(string Sql, object Param = null, IDbTransaction Transaction = null)
        {

            DataTable table = new DataTable();

            if (Transaction == null)
            {

                using (IDbConnection conn = CreateConnection())
                {
                    conn.Open();

                    IDataReader reader = await conn.ExecuteReaderAsync(Sql, Param, commandTimeout: 60, commandType: CommandType.Text);

                    table.Load(reader);
                }

                return table;

            }
            else
            {
                var conn = Transaction.Connection;

                IDataReader reader = await conn.ExecuteReaderAsync(Sql, Param, commandTimeout: 60, commandType: CommandType.Text);

                table.Load(reader);

                return table;
            }
        }
    }
}
