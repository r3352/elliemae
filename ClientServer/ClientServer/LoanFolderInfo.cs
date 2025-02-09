// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanFolderInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanFolderInfo : IComparable
  {
    private string name;
    private string displayName;
    private LoanFolderInfo.LoanFolderType type;
    private bool dupLoanCheck;

    public LoanFolderInfo(string name, LoanFolderInfo.LoanFolderType type, bool dupLoanCheck)
    {
      this.name = name;
      this.type = type;
      this.dupLoanCheck = dupLoanCheck;
      if (this.name.StartsWith("<") && this.name.EndsWith(">"))
        this.displayName = this.name;
      else
        this.displayName = LoanFolderInfo.ToDisplayName(name, type);
    }

    public LoanFolderInfo(string name)
      : this(name, LoanFolderInfo.LoanFolderType.Regular, true)
    {
    }

    public string Name => this.name;

    public string DisplayName => this.displayName;

    public LoanFolderInfo.LoanFolderType Type => this.type;

    public bool IncludeInDuplicateLoanCheck => this.dupLoanCheck;

    public override string ToString() => this.displayName;

    public int CompareTo(object loanFolderInfo)
    {
      LoanFolderInfo loanFolderInfo1 = (LoanFolderInfo) loanFolderInfo;
      return string.Compare(this.Name.ToUpper(), loanFolderInfo1.Name.ToUpper());
    }

    public static string ToDisplayName(string folderName, LoanFolderInfo.LoanFolderType folderType)
    {
      return folderType == LoanFolderInfo.LoanFolderType.Archive ? "<" + folderName + ">" : folderName;
    }

    public static string FromDisplayName(string folderName)
    {
      return folderName.StartsWith("<") && folderName.EndsWith(">") && folderName.Length > 2 ? folderName.Substring(1, folderName.Length - 2) : folderName;
    }

    public enum LoanFolderType
    {
      NotSpecified = -1, // 0xFFFFFFFF
      Regular = 0,
      Trash = 1,
      Archive = 2,
    }
  }
}
