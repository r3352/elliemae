// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.DbTableEncryption.DbTableEncryptor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.DbTableEncryption
{
  public abstract class DbTableEncryptor
  {
    protected const string TargetSuffix = "_new�";
    protected const string BackupSuffix = "_backup�";
    private readonly string InstanceName;
    private InstanceEncryptor Encryptor;
    private const string tempColNameSuffix = "_target�";

    public abstract string TableName { get; }

    public abstract IList<string> PkColumnNames { get; }

    public abstract IList<string> EncryptColNames { get; }

    public abstract IList<string> OtherColNames { get; }

    public EnGlobalSettings GlobalSettings { get; set; }

    public DbTableEncryptor.FlagGetter EncryptionFlagGetter { get; set; }

    protected DbTableEncryptor(string instanceName)
    {
      instanceName = instanceName.ToUpper();
      this.InstanceName = instanceName;
      this.GlobalSettings = new EnGlobalSettings(instanceName);
      this.Encryptor = new InstanceEncryptor(instanceName);
    }

    public string EncryptToHex(string clearText)
    {
      return HexEncoding.Instance.GetString(this.Encryptor.Encrypt(clearText));
    }

    public string DecryptFromHex(string hexString)
    {
      return this.Encryptor.Decrypt(HexEncoding.Instance.GetBytes(hexString));
    }

    public object EncryptValue(object clearText)
    {
      return !(clearText is string clearText1) ? (object) null : (object) this.Encryptor.Encrypt(clearText1);
    }

    public object DecryptValue(object encrypted)
    {
      return !(encrypted is byte[] bytes) || bytes.Length == 0 ? (object) null : (object) this.Encryptor.Decrypt(bytes);
    }

    public void Toggle() => this.Migrate(Mode.Toggle, (string) null);

    public void Migrate(Mode mode = Mode.Encrypt) => this.Migrate(mode, (string) null);

    public void Migrate(string fromInstanceName) => this.Migrate(Mode.Migrate, fromInstanceName);

    private void Migrate(Mode requestedMode, string fromInstanceName)
    {
      try
      {
        using (SqlConnection dbConn = new SqlConnection(this.GlobalSettings.DatabaseConnectionString))
        {
          if (dbConn.State == ConnectionState.Closed)
            dbConn.Open();
          CompanySettingsFlag companySettingsFlag = this.EncryptionFlagGetter(dbConn);
          bool flag1 = !companySettingsFlag.Timestamp.HasValue;
          bool flag2 = requestedMode == Mode.Toggle;
          bool flag3 = requestedMode == Mode.Migrate;
          bool flag4 = requestedMode == Mode.Decrypt | flag3 || flag2 && !flag1;
          if (flag2 || flag4 ^ flag1)
          {
            if (flag3 && companySettingsFlag.InstanceName != fromInstanceName)
              fromInstanceName = companySettingsFlag.InstanceName;
            if (flag4 && companySettingsFlag.InstanceName != this.InstanceName)
              this.Encryptor = new InstanceEncryptor(companySettingsFlag.InstanceName);
            string str1 = flag4 ? "Decrypting" : "Encrypting";
            Console.WriteLine("\n\n=== " + (flag3 ? "Migrating" : str1) + " " + this.TableName + " ===");
            Mode mode1 = flag1 ? Mode.Encrypt : Mode.Decrypt;
            Mode mode2 = flag2 ? mode1 : requestedMode;
            DbTableEncryptor.ObjectMapper newValueFn = mode2 == Mode.Decrypt ? new DbTableEncryptor.ObjectMapper(this.DecryptValue) : new DbTableEncryptor.ObjectMapper(this.EncryptValue);
            if (flag3)
            {
              InstanceEncryptor sourceEncryptor = new InstanceEncryptor(fromInstanceName);
              newValueFn = (DbTableEncryptor.ObjectMapper) (v => this.EncryptValue((object) sourceEncryptor.Decrypt(v as byte[])));
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            this.UpdateValues(dbConn, mode2, newValueFn);
            this.ClearFlagCache();
            companySettingsFlag.Timestamp = !flag4 || flag3 ? new DateTime?(DateTime.UtcNow) : new DateTime?();
            string str2 = flag4 ? "decrypted" : "encrypted";
            Console.WriteLine(this.TableName + " has been " + (flag3 ? "migrated" : str2) + ".");
            Console.WriteLine("Total elapsed = " + this.fmtElapsed(stopwatch.Elapsed));
          }
          else
            Console.WriteLine(this.TableName + " is " + (flag1 ? "not" : "already") + " encrypted.");
        }
      }
      catch (AggregateException ex)
      {
        System.Collections.ObjectModel.ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
        int count1 = innerExceptions.Count;
        int count2 = 3;
        int num = 0;
        Console.WriteLine(string.Format("\n\n{0:N0} error{1} during table update{2}:", (object) count1, count1 == 1 ? (object) "" : (object) "s", count1 > count2 ? (object) string.Format(", first {0} shown", (object) count2) : (object) ""));
        foreach (Exception exception in innerExceptions.Take<Exception>(count2))
          Console.WriteLine(string.Format("\n\n{0} ==> {1}\n\nStack trace:\n{2}", (object) ++num, (object) exception.Message, (object) exception.StackTrace));
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error during table update:\n" + ex.Message + "\n\nStack trace:\n" + ex.StackTrace);
      }
    }

    private void UpdateValues(
      SqlConnection dbConn,
      Mode mode,
      DbTableEncryptor.ObjectMapper newValueFn)
    {
      this.EnsureMigrationTable(dbConn, mode);
      DbTableEncryptor.SqlBatcher.BatchConfig batchConfig = new DbTableEncryptor.SqlBatcher.BatchConfig()
      {
        MinBatchSize = 50000,
        TableName = this.TableName,
        PkColumnNames = this.PkColumnNames
      };
      IEnumerable<string> source = this.PkColumnNames.Union<string>((IEnumerable<string>) this.EncryptColNames).Union<string>((IEnumerable<string>) this.OtherColNames);
      string sql = "SELECT " + string.Join(", ", source.Select<string, string>((System.Func<string, string>) (n => "[" + n + "]"))) + " FROM [batch]";
      DbTableEncryptor.SqlBatcher batcher = new DbTableEncryptor.SqlBatcher((DbConnection) dbConn, batchConfig);
      CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
      ParallelOptions parallelOptions = new ParallelOptions()
      {
        CancellationToken = cancellationTokenSource.Token
      };
      Console.WriteLine(string.Format("Total rows to process: {0:N0}", (object) batcher.TotalCount));
      while (!batcher.Done)
      {
        using (new DbTableEncryptor.BatchLogger(batcher))
        {
          DataTable batch = batcher.NextBatch(sql);
          if (batch.Columns[this.EncryptColNames[0]].DataType == typeof (string) ^ mode == Mode.Encrypt)
            throw new ArgumentException("Encryption settings flag inconsistent with source column type");
          foreach (string encryptColName in (IEnumerable<string>) this.EncryptColNames)
            batch.Columns.Add(encryptColName + "_target", mode == Mode.Decrypt ? typeof (string) : typeof (byte[]));
          Parallel.ForEach<DataRow>(batch.Rows.OfType<DataRow>(), parallelOptions, (Action<DataRow>) (row =>
          {
            try
            {
              foreach (string encryptColName in (IEnumerable<string>) this.EncryptColNames)
              {
                object obj = newValueFn(row[encryptColName]);
                lock (batch)
                  row[encryptColName + "_target"] = obj;
              }
            }
            catch
            {
              throw;
            }
          }));
          foreach (string encryptColName in (IEnumerable<string>) this.EncryptColNames)
          {
            batch.Columns.Remove(encryptColName);
            batch.Columns[encryptColName + "_target"].ColumnName = encryptColName;
          }
          SqlTransaction externalTransaction = (SqlTransaction) null;
          using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(dbConn, SqlBulkCopyOptions.KeepIdentity, externalTransaction))
          {
            sqlBulkCopy.DestinationTableName = this.TableName + "_new";
            foreach (string str in source)
              sqlBulkCopy.ColumnMappings.Add(str, str);
            sqlBulkCopy.WriteToServer(batch);
          }
        }
      }
      this.PromoteMigrationTable(dbConn);
    }

    protected abstract void EnsureMigrationTable(SqlConnection dbConn, Mode mode);

    protected abstract void PromoteMigrationTable(SqlConnection dbConn);

    private void ClearFlagCache()
    {
      string machineName = Environment.MachineName;
      string cacheKey = Company.GetCacheKey("MIGRATION");
      string str = string.IsNullOrEmpty(this.InstanceName) ? "" : "&i=" + this.InstanceName;
      Uri requestUri = new Uri("http://" + machineName + "/Encompass/rmi.aspx?p=B316@r10n&c=removecachekey&k=" + cacheKey + str);
      HttpClient httpClient = new HttpClient();
      try
      {
        string result = httpClient.GetStringAsync(requestUri).Result;
        Console.WriteLine("Flushed company settings cache key '" + cacheKey + "'");
      }
      catch (Exception ex)
      {
        Exception exception = ex is AggregateException aggregateException ? aggregateException.InnerExceptions.First<Exception>() : ex;
        Console.WriteLine("Error during flush of company settings cache key '" + cacheKey + "':\n" + exception.Message);
        if (string.IsNullOrEmpty(exception.StackTrace))
          return;
        Console.WriteLine("\n\nStack trace:\n{ex.StackTrace}");
      }
    }

    private string fmtElapsed(TimeSpan elapsed)
    {
      return elapsed.ToString(elapsed.TotalDays >= 1.0 ? "d'd, 'h'h:'mm'm '" : (elapsed.Hours > 0 ? "h'h:'mm'm '" : "m'm '") + "ss'.'fff's'");
    }

    public delegate CompanySettingsFlag FlagGetter(SqlConnection dbConn);

    protected delegate object ObjectMapper(object value);

    protected class SqlBatcher
    {
      public SqlConnection DbConn { get; set; }

      public int TotalCount { get; protected set; }

      public int BatchSize { get; protected set; }

      public int NextStart { get; set; } = 1;

      public int NextEnd => Math.Min(this.TotalCount, this.NextStart + this.BatchSize - 1);

      public bool Done => this.NextStart > this.TotalCount;

      protected string FromClause { get; set; }

      protected string WithClause { get; set; }

      protected int GetTotalCount()
      {
        return (int) new SqlCommand("SELECT COUNT(*) " + this.FromClause, this.DbConn).ExecuteScalar();
      }

      protected string BatchWhere
      {
        get => string.Format("WHERE [row#] BETWEEN 1 AND {0}", (object) this.NextEnd);
      }

      public SqlBatcher(
        DbConnection dbConn,
        DbTableEncryptor.SqlBatcher.BatchConfig batchConfig)
      {
        string tableName = batchConfig.TableName;
        string str1 = string.Join(", ", batchConfig.PkColumnNames.Select<string, string>((System.Func<string, string>) (n => "o.[" + n + "]")));
        string str2 = string.Join(" AND ", batchConfig.PkColumnNames.Select<string, string>((System.Func<string, string>) (n => "o.[" + n + "] = n.[" + n + "]")));
        int minBatchSize = batchConfig.MinBatchSize;
        int? maxBatchSize = batchConfig.MaxBatchSize;
        this.DbConn = (SqlConnection) dbConn;
        this.FromClause = "FROM [" + tableName + "] o LEFT JOIN [" + tableName + "_new] n ON " + str2 + " WHERE n.[" + batchConfig.PkColumnNames[0] + "] IS NULL";
        this.WithClause = "WITH [batch] AS (SELECT o.*, ROW_NUMBER() OVER (ORDER BY " + str1 + ") AS [row#] " + this.FromClause + ")\n";
        this.BatchSize = (int) Math.Max((double) minBatchSize, Math.Min((double) (maxBatchSize ?? minBatchSize), Math.Ceiling((double) (this.TotalCount = this.GetTotalCount()) * 0.1)));
      }

      public DataTable NextBatch(string sql)
      {
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand(this.WithClause + " " + sql + " " + this.BatchWhere, this.DbConn));
        DataSet dataSet = new DataSet();
        sqlDataAdapter.Fill(dataSet);
        this.NextStart += this.BatchSize;
        return dataSet.Tables[0];
      }

      public class BatchConfig
      {
        public string TableName { get; set; }

        public IList<string> PkColumnNames { get; set; }

        public int MinBatchSize { get; set; } = 10;

        public int? MaxBatchSize { get; set; }
      }
    }

    protected class BatchLogger : IDisposable
    {
      private readonly Stopwatch stopwatch;

      private string ToNumber(int value) => value.ToString("N0");

      public BatchLogger(DbTableEncryptor.SqlBatcher batcher)
      {
        int digits = this.ToNumber(batcher.TotalCount).Length;
        Console.Write("Updating rows " + paddedInt(batcher.NextStart) + " TO " + paddedInt(batcher.NextEnd) + "...");
        this.stopwatch = Stopwatch.StartNew();

        string paddedInt(int value) => this.ToNumber(value).PadLeft(digits, ' ');
      }

      void IDisposable.Dispose()
      {
        Console.WriteLine(string.Format("done ({0,6} msec)", (object) this.ToNumber((int) this.stopwatch.ElapsedMilliseconds)));
      }
    }
  }
}
