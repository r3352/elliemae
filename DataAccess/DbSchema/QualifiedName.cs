// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.QualifiedName
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.DataAccess.DbSchema
{
  public class QualifiedName
  {
    private static readonly Regex nameRegex = new Regex("\\[.*?\\]");
    private string[] nameParts;

    public QualifiedName(string qualifiedName)
    {
      this.nameParts = QualifiedName.splitName(qualifiedName);
      this.FullName = qualifiedName;
      this.ObjectName = this.nameParts[this.nameParts.Length - 1];
      if (this.nameParts.Length >= 2)
      {
        this.SchemaName = this.nameParts[0];
        this.UnqualifiedName = string.Join(".", this.nameParts, 1, this.nameParts.Length - 1);
      }
      else
      {
        this.SchemaName = "";
        this.UnqualifiedName = qualifiedName;
      }
      if (this.nameParts.Length >= 3)
        this.ParentName = string.Join(".", this.nameParts, 0, this.nameParts.Length - 1);
      else
        this.ParentName = "";
    }

    public string SchemaName { get; private set; }

    public string ObjectName { get; private set; }

    public string ParentName { get; private set; }

    public string UnqualifiedName { get; private set; }

    public string FullName { get; private set; }

    public string GetUnbracketedObjectName() => this.ObjectName.Replace("[", "").Replace("]", "");

    public static implicit operator string(QualifiedName qn) => qn?.ToString();

    public override string ToString() => this.ObjectName;

    public override bool Equals(object obj)
    {
      return obj is QualifiedName qualifiedName && string.Compare(qualifiedName.FullName, this.FullName, true) == 0;
    }

    public override int GetHashCode()
    {
      return StringComparer.CurrentCultureIgnoreCase.GetHashCode(this.FullName);
    }

    public static string[] splitName(string dbName)
    {
      List<string> stringList = new List<string>();
      Match match = QualifiedName.nameRegex.Match(dbName);
      if (!match.Success)
        return new string[1]{ dbName };
      for (; match != null && match.Success; match = match.NextMatch())
        stringList.Add(match.Value);
      return stringList.ToArray();
    }
  }
}
