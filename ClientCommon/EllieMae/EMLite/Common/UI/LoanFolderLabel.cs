// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.LoanFolderLabel
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.UI;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class LoanFolderLabel(PipelineElementData data) : TextElement(LoanFolderLabel.getFolderDisplayName(data))
  {
    private static string getFolderDisplayName(PipelineElementData data)
    {
      string folderName = string.Concat(data.GetValue());
      LoanFolderInfo.LoanFolderType folderType = LoanFolderInfo.LoanFolderType.Regular;
      if (string.Concat(data.PipelineInfo.GetField("LoanFolder.Archive")) == "Y")
        folderType = LoanFolderInfo.LoanFolderType.Archive;
      return LoanFolderInfo.ToDisplayName(folderName, folderType);
    }
  }
}
