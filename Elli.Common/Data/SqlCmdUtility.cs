// Decompiled with JetBrains decompiler
// Type: Elli.Common.Data.SqlCmdUtility
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

#nullable disable
namespace Elli.Common.Data
{
  public class SqlCmdUtility
  {
    private Dictionary<string, string> variables;
    private List<string> lockedVariables;
    private ErrorMode errorMode;
    private SqlConnection sqlConn;
    private List<string> ignoredCommands;
    private bool allowVariableOverwrites;

    public SqlCmdUtility(SqlConnection sqlConn)
    {
      this.sqlConn = sqlConn != null ? sqlConn : throw new Exception("sqlConn cannot be null");
      this.variables = new Dictionary<string, string>();
      this.variables.Add("SQLCMDUSER", "");
      this.variables.Add("SQLCMDPASSWORD", "");
      this.variables.Add("SQLCMDSERVER", sqlConn.DataSource);
      this.variables.Add("SQLCMDWORKSTATION", sqlConn.WorkstationId);
      this.variables.Add("SQLCMDDBNAME", sqlConn.Database);
      this.variables.Add("SQLCMDLOGINTIMEOUT", sqlConn.ConnectionTimeout.ToString());
      this.variables.Add("SQLCMDSTATTIMEOUT", "0");
      this.variables.Add("SQLCMDHEADERS", "0");
      this.variables.Add("SQLCMDCOLSEP", "");
      this.variables.Add("SQLCMDCOLWIDTH", "0");
      this.variables.Add("SQLCMDPACKETSIZE", "4096");
      this.variables.Add("SQLCMDERRORLEVEL", "0");
      this.variables.Add("SQLCMDMAXVARTYPEWIDTH", "256");
      this.variables.Add("SQLCMDMAXFIXEDTYPEWIDTH", "0");
      this.variables.Add("SQLCMDEDITOR", "edit.com");
      this.variables.Add("SQLCMDINI", "");
      this.variables.Add("SQLCMDREAL", "0");
      this.lockedVariables = new List<string>();
      this.lockedVariables.Add("SQLCMDREAL");
      this.ignoredCommands = new List<string>();
      this.ignoredCommands.Add(":ED");
      this.ignoredCommands.Add(":ERROR");
      this.ignoredCommands.Add(":!!");
      this.ignoredCommands.Add(":PERFTRACE");
      this.ignoredCommands.Add(":QUIT");
      this.ignoredCommands.Add(":EXIT");
      this.ignoredCommands.Add(":HELP");
      this.ignoredCommands.Add(":XML");
      this.ignoredCommands.Add(":R");
      this.ignoredCommands.Add(":SERVERLIST");
      this.ignoredCommands.Add(":LISTVAR");
      this.errorMode = ErrorMode.Ignore;
      this.allowVariableOverwrites = false;
    }

    public bool AllowVariableOverwrites
    {
      get => this.allowVariableOverwrites;
      set => this.allowVariableOverwrites = value;
    }

    public void SetVariable(string variableName, string variableValue)
    {
      variableName = variableName.Trim().ToUpper();
      if (string.IsNullOrEmpty(variableName) || variableName.Trim().Length == 0)
        throw new Exception("Variable name cannot be blank or contain spaces");
      if (this.variables.ContainsKey(variableName))
      {
        this.variables[variableName] = variableValue;
      }
      else
      {
        this.variables.Add(variableName, variableValue);
        if (this.allowVariableOverwrites)
          return;
        this.lockedVariables.Add(variableName);
      }
    }

    public List<Exception> Execute(string scriptToExecute)
    {
      List<Exception> exceptionList = new List<Exception>();
      StringBuilder stringBuilder = new StringBuilder();
      SqlTransaction transaction = (SqlTransaction) null;
      this.sqlConn.Open();
      try
      {
        string[] strArray = (scriptToExecute.Replace(Environment.NewLine, "\n") + "\nGO\n").Split('\n');
        for (int index = 0; index < strArray.GetUpperBound(0); ++index)
        {
          string str1 = strArray[index].Replace("\t", " ").Trim().ToUpper() + " ";
          if (!this.ignoredCommands.Contains(str1.Split(' ')[0]))
          {
            if (str1.StartsWith("GO "))
            {
              try
              {
                if (stringBuilder.Length > 0)
                  (transaction == null ? (DbCommand) new SqlCommand(stringBuilder.ToString(), this.sqlConn) : (DbCommand) new SqlCommand(stringBuilder.ToString(), this.sqlConn, transaction)).ExecuteNonQuery();
              }
              catch (Exception ex)
              {
                if (this.errorMode != ErrorMode.Ignore)
                {
                  SqlException sqlException = ex is SqlException ? (SqlException) ex : throw new Exception("Error executing " + stringBuilder.ToString(), ex);
                  if (sqlException.Class == (byte) 16 && sqlException.Number == 50000 && sqlException.State == (byte) 1)
                  {
                    if (sqlException.Message.StartsWith("The database schema already has version "))
                      break;
                  }
                }
                else
                  exceptionList.Add(new Exception("Error executing " + stringBuilder.ToString(), ex));
              }
              stringBuilder = new StringBuilder();
            }
            else if (str1.StartsWith(":SETVAR "))
            {
              string str2 = strArray[index].Trim().Substring(8, strArray[index].Trim().Length - 8);
              int length = str2.IndexOf(" ");
              string upper = str2.Substring(0, length).Trim().ToUpper();
              string str3 = str2.Substring(length + 1, str2.Length - length - 1).Trim();
              string str4 = str3.StartsWith("\"") && str3.EndsWith("\"") ? str3.Substring(1, str3.Length - 2) : throw new Exception(string.Format("Improperly formatted :SetVar on line {0}.", (object) index));
              if (this.variables.ContainsKey(upper))
              {
                if (!this.lockedVariables.Contains(upper))
                  this.variables[upper] = str4;
              }
              else
                this.variables.Add(upper, str4);
            }
            else if (str1.StartsWith(":ON ERROR "))
            {
              string str5 = str1.Substring(10, str1.Length - 10).Trim();
              switch (str5)
              {
                case "EXIT":
                  this.errorMode = ErrorMode.Exit;
                  transaction = this.sqlConn.BeginTransaction(IsolationLevel.ReadCommitted);
                  continue;
                case "RESUME":
                case "IGNORE":
                  this.errorMode = ErrorMode.Ignore;
                  continue;
                default:
                  throw new Exception(string.Format("Unknown On Error mode '{0}' on line {1}", (object) str5, (object) index));
              }
            }
            else
            {
              string str6 = strArray[index];
              if (str6.Length > 4 && str6.Contains("$("))
              {
                foreach (KeyValuePair<string, string> variable in this.variables)
                {
                  string format = string.Format("$({0})", (object) variable.Key);
                  for (int length = str6.ToUpper().IndexOf(format); length >= 0; length = str6.ToUpper().IndexOf(string.Format(format)))
                  {
                    int startIndex = length + format.Length;
                    str6 = str6.Substring(0, length) + variable.Value + str6.Substring(startIndex, str6.Length - startIndex);
                  }
                }
              }
              stringBuilder.AppendLine(str6);
            }
          }
        }
        transaction?.Commit();
      }
      catch (Exception ex)
      {
        transaction?.Rollback();
        throw ex;
      }
      return exceptionList;
    }
  }
}
