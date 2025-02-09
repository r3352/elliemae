// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.DataFile
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Common;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class DataFile : IDisposable
  {
    private const string className = "DataFile�";
    private const string LoanDataFilename = "loan.em�";
    public const string BackupExtension = "encbak�";
    private const int MaxBytesPerWrite = 2000000;
    private string path;
    private SafeMutex innerLock;
    private bool exists;

    public DataFile(string path, SafeMutex innerLock)
    {
      this.innerLock = innerLock;
      this.path = path;
      this.exists = File.Exists(path);
      if (this.exists)
        TraceLog.WriteVerbose(nameof (DataFile), "Checked out existing data file " + path);
      else
        TraceLog.WriteVerbose(nameof (DataFile), "Checked out non-existing data file " + path);
    }

    public DataFile(string path)
    {
      this.innerLock = (SafeMutex) null;
      this.path = path;
      this.exists = File.Exists(path);
      if (this.exists)
        TraceLog.WriteVerbose(nameof (DataFile), "Checked out existing data file " + path);
      else
        TraceLog.WriteVerbose(nameof (DataFile), "Checked out non-existing data file " + path);
    }

    public bool Exists => this.exists;

    public DateTime LastModified
    {
      get
      {
        this.validateExists();
        int num = 0;
        while (true)
        {
          try
          {
            return File.GetLastWriteTime(this.path);
          }
          catch (IOException ex)
          {
            if (++num > 2)
              throw;
            else
              TraceLog.WriteError(nameof (DataFile), "IO error reading last modified time for '" + this.path + "': " + (object) ex);
          }
        }
      }
    }

    public Version FileVersion
    {
      get
      {
        this.validateExists();
        int num = 0;
        while (true)
        {
          try
          {
            int[] versionInfo = ValidationUtil.GetVersionInfo(FileVersionInfo.GetVersionInfo(this.path).FileVersion);
            return new Version(versionInfo[0], versionInfo[1], versionInfo[2], versionInfo[3]);
          }
          catch (IOException ex)
          {
            if (++num > 2)
              throw;
            else
              TraceLog.WriteError(nameof (DataFile), "IO error reading file version for '" + this.path + "': " + (object) ex);
          }
        }
      }
    }

    public BinaryObject GetData() => this.GetData(string.Empty, 0L, 0L);

    public BinaryObject GetData(
      string fileName,
      long maxFileSizeForThrowingError,
      long maxFileSizeForLoggingWarning)
    {
      this.validateExists();
      return this.exists ? DataFile.threadSafeRead(this.path, fileName, maxFileSizeForThrowingError, maxFileSizeForLoggingWarning) : (BinaryObject) null;
    }

    public string GetText() => this.GetText(Encoding.Default);

    public string GetText(Encoding encoding)
    {
      using (BinaryObject data = this.GetData())
        return data?.ToString(encoding);
    }

    public string Path => this.path;

    public string Filename
    {
      get => System.IO.Path.GetFileName(this.path);
      set
      {
        if (string.Compare(this.Filename, value, true) != 0)
          throw new ArgumentException("The file name is not compatible with the current value.");
        this.path = this.path.Length < 256 ? System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.path), value) : throw new PathTooLongException();
      }
    }

    public void CreateNew(BinaryObject data)
    {
      if (this.exists)
        Err.Raise(TraceLevel.Warning, nameof (DataFile), new ServerException("Cannot call CreateNew for existing DataFile object"));
      this.CheckIn(data);
    }

    public void Delete()
    {
      TraceLog.WriteInfo(nameof (DataFile), "To delete data file " + this.path);
      this.validateInstance();
      File.Delete(this.path);
      this.exists = false;
      this.Dispose();
      TraceLog.WriteVerbose(nameof (DataFile), "Deleted data file " + this.path);
    }

    public void CheckIn(BinaryObject data, ILoanSettings loanSettings = null, bool ValidateIfFileExists = false)
    {
      this.CheckIn(data, false, loanSettings, ValidateIfFileExists);
    }

    public void CheckIn(
      BinaryObject data,
      bool keepCheckedOut,
      ILoanSettings loanSettings = null,
      bool ValidateIfFileExists = false)
    {
      this.validateInstance(false);
      if (data == null)
        Err.Raise(TraceLevel.Warning, nameof (DataFile), new ServerException("Data cannot be null"));
      this.saveFileToDisk(data, loanSettings, ValidateIfFileExists);
      this.exists = true;
      if (!keepCheckedOut)
        this.Dispose();
      TraceLog.WriteVerbose(nameof (DataFile), "Checked in " + (object) data.Length + " bytes to data file " + this.path);
    }

    public void Append(BinaryObject data, bool keepCheckedOut)
    {
      this.validateInstance(true);
      if (data == null)
        Err.Raise(TraceLevel.Warning, nameof (DataFile), new ServerException("Data cannot be null"));
      this.appendToDiskFile(data);
      if (!keepCheckedOut)
        this.Dispose();
      TraceLog.WriteVerbose(nameof (DataFile), "Appended " + (object) data.Length + " bytes to data file " + this.path);
    }

    public void UndoCheckout() => this.Dispose();

    public void Dispose()
    {
      if (this.innerLock == null)
        return;
      this.innerLock.ReleaseMutex();
      this.innerLock = (SafeMutex) null;
    }

    private void saveFileToDisk(
      BinaryObject data,
      ILoanSettings loanSettings = null,
      bool ValidateIfFileExists = false)
    {
      Directory.CreateDirectory(System.IO.Path.GetDirectoryName(this.path));
      string str = (string) null;
      if (this.exists)
        str = DataFile.threadSafeBackup(this);
      try
      {
        DataFile.threadSafeWrite(data, this.path, ValidateIfFileExists);
        try
        {
          if (str == null)
            return;
          File.Delete(str);
          TraceLog.WriteDebug(nameof (DataFile), "Deleted backup file \"" + str + "\"");
        }
        catch
        {
        }
      }
      catch (IOException ex)
      {
        TraceLog.WriteWarning(nameof (DataFile), "Error while writing file. Message: " + ex.Message);
        if (str != null)
        {
          File.Copy(str, this.path, true);
          TraceLog.WriteDebug(nameof (DataFile), "Restored backup file \"" + str + "\"");
          try
          {
            File.Delete(str);
          }
          catch
          {
          }
        }
        throw;
      }
      catch (Exception ex)
      {
        if (str != null)
        {
          File.Copy(str, this.path, true);
          TraceLog.WriteDebug(nameof (DataFile), "Restored backup file \"" + str + "\"");
          try
          {
            File.Delete(str);
          }
          catch
          {
          }
        }
        Err.Reraise(nameof (DataFile), ex);
      }
    }

    private void appendToDiskFile(BinaryObject data)
    {
      Directory.CreateDirectory(System.IO.Path.GetDirectoryName(this.path));
      DataFile.threadSafeAppend(data, this.path);
    }

    private void validateInstance() => this.validateInstance(true);

    private void validateInstance(bool requireExists)
    {
      if (this.innerLock == null)
        Err.Raise(TraceLevel.Error, nameof (DataFile), new ServerException("Attempt to access disposed DataFile object"));
      if (!requireExists)
        return;
      this.validateExists();
    }

    private void validateExists()
    {
      if (this.Exists)
        return;
      Err.Raise(TraceLevel.Error, nameof (DataFile), new ServerException("Object does not exist"));
    }

    public static bool IsValidFilename(string name)
    {
      switch (name)
      {
        case null:
          return false;
        case "":
          return false;
        default:
          byte[] bytes = Encoding.ASCII.GetBytes(name);
          for (int index = 0; index < bytes.Length; ++index)
          {
            if (bytes[index] < (byte) 32 || bytes[index] > (byte) 126)
              return false;
          }
          return "\\/:*?\"<>|".IndexOfAny(name.ToCharArray()) < 0;
      }
    }

    public static bool IsValidFilePath(string name)
    {
      if (name == null || name.Length == 0)
        return false;
      if (name.Length >= 2 && name[1] == ':')
        name = name.Substring(2);
      string[] strArray = name.Split('\\');
      if (name == null)
        return true;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index].Length != 0 && !DataFile.IsValidFilename(strArray[index]))
          return false;
      }
      return true;
    }

    public static bool IsValidSubobjectName(string name) => FileSystem.IsValidSubobjectName(name);

    private static BinaryObject threadSafeRead(
      string path,
      string fileName,
      long maxFileSizeForThrowingError,
      long maxFileSizeForLoggingWarning)
    {
      DateTime now = DateTime.Now;
      BinaryObject binaryObject = (BinaryObject) null;
      try
      {
        if (path.EndsWith("\\templates\\blankloan\\loan.em", StringComparison.CurrentCultureIgnoreCase))
        {
          binaryObject = new BinaryObject(Resources.BlankLoan, Encoding.Default);
        }
        else
        {
          using (FileStream inputStream = EllieMae.EMLite.DataAccess.Threading.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read))
          {
            long length = inputStream.Length;
            if (maxFileSizeForThrowingError > 0L && length > maxFileSizeForThrowingError)
            {
              TraceLog.WriteError(nameof (DataFile), string.Format("File cannot be opened as it has exceeded max threshold size. Path= '{0}', Size= {1} bytes, MaxThresholdLimit= {2} bytes.", (object) path, (object) length, (object) maxFileSizeForThrowingError));
              throw new FileSizeLimitExceededException("Cannot open file '" + fileName + "' because it is too large, Please contact your system administrator.", fileName, path, length, maxFileSizeForThrowingError);
            }
            if (maxFileSizeForLoggingWarning > 0L && length > maxFileSizeForLoggingWarning)
              TraceLog.WriteWarning(nameof (DataFile), "Exceeded warning threshold limit for file size. " + string.Format("Path= '{0}', Size/threshold= {1}/{2} bytes, MaxThresholdLimit= {3} bytes.", (object) path, (object) length, (object) maxFileSizeForLoggingWarning, (object) maxFileSizeForThrowingError));
            binaryObject = new BinaryObject((Stream) inputStream);
            inputStream.Close();
          }
        }
      }
      catch (FileSizeLimitExceededException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ex.GetType() == typeof (UnauthorizedAccessException))
          TraceLog.WriteError(nameof (DataFile), string.Format("ThreadSafeRead throws an UnauthorizedAccessException. File: {0}, thread: {1}, Message: {2}, StackTrace: {3}", (object) path, (object) Thread.CurrentThread.GetHashCode(), (object) ex.Message, (object) ex.StackTrace));
        else
          TraceLog.WriteError(nameof (DataFile), string.Format("ThreadSafeRead throws an exception. File: {0}, thread: {1}, Message: {2}, StackTrace: {3}", (object) path, (object) Thread.CurrentThread.GetHashCode(), (object) ex.Message, (object) ex.StackTrace));
      }
      TimeSpan timeSpan = DateTime.Now - now;
      if (timeSpan.TotalSeconds > 3.0)
        TraceLog.WriteWarning(nameof (DataFile), "Long duration file read occurred on thread " + (object) Thread.CurrentThread.GetHashCode() + " with elapsed time = " + timeSpan.TotalSeconds.ToString("0.00") + "s. File Path = " + path + ". Stack Trace follows:" + Environment.NewLine + (object) new StackTrace(1));
      else
        TraceLog.WriteVerbose(nameof (DataFile), "File '" + path + "' containing " + (object) binaryObject.Length + " bytes read in " + timeSpan.TotalMilliseconds.ToString("0") + " ms");
      return binaryObject;
    }

    private static void threadSafeWrite(BinaryObject o, string path, bool ValidateIfFileExists = false)
    {
      TraceLog.WriteVerbose(nameof (DataFile), "Writing file \"" + path + "\"");
      DateTime now = DateTime.Now;
      if (!ValidateIfFileExists)
      {
        using (FileStream fileStream = EllieMae.EMLite.DataAccess.Threading.OpenFile(path, FileMode.Create, FileAccess.Write, FileShare.None))
          o.Write((Stream) fileStream);
      }
      else
      {
        TraceLog.WriteVerbose(nameof (DataFile), string.Format("ValidateIfFileExists is True, trying with FileMode.CreateNew. - Path {0}", (object) path));
        try
        {
          using (FileStream fileStream = EllieMae.EMLite.DataAccess.Threading.OpenFile(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            o.Write((Stream) fileStream);
        }
        catch (IOException ex)
        {
          TraceLog.WriteWarning(nameof (DataFile), string.Format("Failed while creating new file as file already exists. - Path {0}.  Error: {1}", (object) path, (object) ex.Message));
          throw;
        }
      }
      TimeSpan timeSpan = DateTime.Now - now;
      if (timeSpan.TotalSeconds <= 3.0)
        return;
      TraceLog.WriteWarning(nameof (DataFile), "Long duration file write occurred on thread " + (object) Thread.CurrentThread.GetHashCode() + " with elapsed time = " + timeSpan.TotalSeconds.ToString("0.00") + "s. File Path = " + path + ". Stack Trace follows:" + Environment.NewLine + (object) new StackTrace(1));
    }

    private static void threadSafeAppend(BinaryObject o, string path)
    {
      TraceLog.WriteVerbose(nameof (DataFile), "Appending to file \"" + path + "\"");
      DateTime now = DateTime.Now;
      using (FileStream fileStream = EllieMae.EMLite.DataAccess.Threading.OpenFile(path, FileMode.Append, FileAccess.Write, FileShare.None))
        o.Write((Stream) fileStream);
      TimeSpan timeSpan = DateTime.Now - now;
      if (timeSpan.TotalSeconds <= 3.0)
        return;
      TraceLog.WriteWarning(nameof (DataFile), "Long duration file write occurred on thread " + (object) Thread.CurrentThread.GetHashCode() + " with elapsed time = " + timeSpan.TotalSeconds.ToString("0.00") + "s. File Path = " + path + ". Stack Trace follows:" + Environment.NewLine + (object) new StackTrace(1));
    }

    private static string threadSafeBackup(DataFile file)
    {
      int num = 1;
      BinaryObject data = file.GetData();
label_1:
      try
      {
        string path = file.Path + "." + (object) num++ + ".encbak";
        try
        {
          using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            data.Write((Stream) fileStream);
          TraceLog.WriteDebug(nameof (DataFile), "Created backup file \"" + path + "\"");
          return path;
        }
        catch (IOException ex)
        {
          if (num >= 100)
            throw new Exception("Unable to create backup file for " + file.Path, (Exception) ex);
          goto label_1;
        }
      }
      finally
      {
        data?.Dispose();
      }
    }

    private static void ensureDirectoryExists(string filePath)
    {
      string directoryName = System.IO.Path.GetDirectoryName(filePath);
      if (Directory.Exists(directoryName))
        return;
      Directory.CreateDirectory(directoryName);
    }

    private static string threadSafeVersionWrite(
      BinaryObject o,
      string path,
      int? loanVerionNumber = null)
    {
      TraceLog.WriteInfo(nameof (DataFile), string.Format("Writing loan version file - version : {0} , location : {1} ", (object) loanVerionNumber, (object) path));
      DateTime now = DateTime.Now;
      string str = string.Empty;
      try
      {
        str = path.Replace(new FileInfo(path).Name, "Versions") + "\\" + string.Format("{0:D5}_", (object) loanVerionNumber) + "loan.em";
        DataFile.ensureDirectoryExists(str);
        using (FileStream fileStream = EllieMae.EMLite.DataAccess.Threading.OpenFile(str, FileMode.Create, FileAccess.Write, FileShare.None))
          o.Write((Stream) fileStream);
        TraceLog.WriteDebug(nameof (DataFile), "Created version file \"" + str + "\"");
        TimeSpan timeSpan = DateTime.Now - now;
        if (timeSpan.TotalSeconds > 3.0)
          TraceLog.WriteWarning("VersionedDataFile", "Long duration file write occurred on thread " + (object) Thread.CurrentThread.GetHashCode() + " with elapsed time = " + timeSpan.TotalSeconds.ToString("0.00") + "s. File Path = " + str + ". Stack Trace follows:" + Environment.NewLine + (object) new StackTrace(1));
      }
      catch (Exception ex)
      {
        Err.Raise(TraceLevel.Error, nameof (DataFile), new ServerException("Error creating loan version file.", ex));
      }
      return str;
    }
  }
}
