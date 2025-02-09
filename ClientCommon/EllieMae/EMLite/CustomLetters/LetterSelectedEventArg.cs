// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.LetterSelectedEventArg
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  public class LetterSelectedEventArg : EventArgs
  {
    private FileSystemEntry _LetterFile;
    private bool _IsPrintPreview;
    private string _Action = "";

    public LetterSelectedEventArg(FileSystemEntry letterFile)
      : this(letterFile, false, "MailMerge")
    {
    }

    public LetterSelectedEventArg(FileSystemEntry letterFile, bool bPrintPreview, string action)
    {
      this._LetterFile = letterFile;
      this._IsPrintPreview = bPrintPreview;
      this._Action = action;
    }

    public FileSystemEntry LetterFile => this._LetterFile;

    public bool IsPrintPreview => this._IsPrintPreview;

    public string Action => this._Action;
  }
}
