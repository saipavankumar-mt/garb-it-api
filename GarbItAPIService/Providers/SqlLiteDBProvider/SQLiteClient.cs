using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SqLiteDBProvider
{
    public class SQLiteClient : IDisposable
    {
        private ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim();
        private SQLiteConnection sqlite_conn;

        public SQLiteClient()
        {
            sqlite_conn = new SQLiteConnection("Data Source=E:\\garb-it.db; Version = 3; New = True; Compress = True; ");
        }


        public SQLiteDataReader RunSelectSQL(string sqlCmd)
        {
            SQLiteDataReader sqlite_datareader = null;

            try
            {
                _readerWriterLock.EnterReadLock();
                sqlite_conn.Open();
                
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = sqlCmd;

                sqlite_datareader = sqlite_cmd.ExecuteReader();
            }
            catch (Exception ex)
            {

            }
            finally
            {                
                _readerWriterLock.ExitReadLock();
            }

            return sqlite_datareader;
        }

        public bool RunInsertOrUpdateOrDeleteSQL(string sqlCmd)
        {
            int result = -1;
            bool isbreaked = false;
            try
            {
                _readerWriterLock.EnterWriteLock();
                if (_readerWriterLock.WaitingReadCount > 0)
                {
                    isbreaked = true;
                }
                else
                {
                    sqlite_conn.Open();

                    SQLiteCommand sqlite_cmd;
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = sqlCmd;

                    result = sqlite_cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sqlite_conn.Close();
                _readerWriterLock.ExitWriteLock();
            }

            if (isbreaked)
            {
                Thread.Sleep(10);
                return RunInsertOrUpdateOrDeleteSQL(sqlCmd);
            }
            return result >= 0 ? true : false;
        }

        public int RunCountSQL(string sqlCmd)
        {
            int count = 0;
            try
            {
                _readerWriterLock.EnterReadLock();
                sqlite_conn.Open();

                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = sqlCmd;

                count = Convert.ToInt32(sqlite_cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sqlite_conn.Close();
                _readerWriterLock.ExitReadLock();
            }

            return count;
        }

        public void Dispose()
        {
            sqlite_conn.Close();
        }
    }
}
