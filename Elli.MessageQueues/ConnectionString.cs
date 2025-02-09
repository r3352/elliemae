// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.ConnectionString
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.MessageQueues
{
  public class ConnectionString
  {
    private readonly IDictionary<string, string> _parametersDictionary = (IDictionary<string, string>) new Dictionary<string, string>();
    private string _password;

    public ConnectionString(string connectionStringValue)
    {
      string str1 = connectionStringValue != null ? connectionStringValue : throw new ArgumentNullException(nameof (connectionStringValue));
      char[] chArray = new char[1]{ ';' };
      foreach (string str2 in str1.Split(chArray))
      {
        if (!string.IsNullOrWhiteSpace(str2))
        {
          string[] strArray = str2.Split('=');
          if (strArray.Length != 2)
            throw new Exception("Invalid connection string element: '{keyValuePair}' should be 'key=value'");
          this._parametersDictionary.Add(strArray[0].ToLower(), strArray[1]);
        }
      }
      this.Port = int.Parse(this.GetValue("port", "5672"));
      this.Host = this.GetValue("host", "localhost");
      this.VirtualHost = this.GetValue("virtualHost", "/");
      this.UserName = this.GetValue("username", "guest");
      this.Password = this.GetValue("password", "guest");
      if (!this.Host.Contains(":"))
        return;
      int length = this.Host.IndexOf(":", StringComparison.Ordinal);
      this.Port = int.Parse(this.Host.Substring(length + 1));
      this.Host = this.Host.Substring(0, length);
    }

    public int Port { get; private set; }

    public string Host { get; private set; }

    public string VirtualHost { get; private set; }

    public string UserName { get; private set; }

    public string Password
    {
      get => this._password;
      private set => this._password = Global.PasswordPolicy.Decrypt(value);
    }

    public string GetValue(string key)
    {
      string key1 = key != null ? key.ToLower() : throw new ArgumentNullException("GetValue(key)");
      return this._parametersDictionary.ContainsKey(key1) ? this._parametersDictionary[key1] : throw new Exception("No value with key '{key}' exists");
    }

    public string GetValue(string key, string defaultValue)
    {
      string key1 = key != null ? key.ToLower() : throw new ArgumentNullException("ConnectionString.GetValue(key)");
      return !this._parametersDictionary.ContainsKey(key1) || string.IsNullOrEmpty(this._parametersDictionary[key1]) ? defaultValue : this._parametersDictionary[key1];
    }
  }
}
