// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.DataEngine.LoanDefaultFHAConsumerChoice
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.DataEngine
{
  public class LoanDefaultFHAConsumerChoice : FormDataBase
  {
    public static readonly string[] LoanFields = new string[23]
    {
      "FICC.X1",
      "FICC.X2",
      "FICC.X3",
      "FICC.X4",
      "FICC.X5",
      "FICC.X6",
      "FICC.X7",
      "FICC.X8",
      "FICC.X9",
      "FICC.X10",
      "FICC.X11",
      "FICC.X12",
      "FICC.X13",
      "FICC.X14",
      "FICC.X15",
      "FICC.X16",
      "FICC.X17",
      "FICC.X18",
      "FICC.X19",
      "FICC.X20",
      "FICC.X21",
      "FICC.X22",
      "FICC.X23"
    };
    private DefaultFieldsInfo fhaDefaultFieldsInfo;

    public LoanDefaultFHAConsumerChoice(SessionObjects sessionObjects)
      : this((IWin32Window) null, sessionObjects)
    {
    }

    public LoanDefaultFHAConsumerChoice(IWin32Window win, SessionObjects sessionObjects)
    {
      this.fhaDefaultFieldsInfo = sessionObjects.ConfigurationManager.GetDefaultFields("FHAConsumerChoiceFieldList");
      if (this.fhaDefaultFieldsInfo != null && this.fhaDefaultFieldsInfo.Map.Count == 0)
      {
        Hashtable hashtable = Utils.LoanDefaultFileFromDocumentFolder(win, AssemblyResolver.GetResourceFileFullPath(SystemSettings.DocDirRelPath + "FHAConsumerChoiceFieldList.xml", SystemSettings.LocalAppDir), "FHAConsumerChoiceFieldList");
        if (hashtable == null)
          return;
        this.fhaDefaultFieldsInfo.Map = hashtable;
        this.Reset();
        this.CommitChanges(sessionObjects);
      }
      else
      {
        if (this.fhaDefaultFieldsInfo == null)
          return;
        this.Reset();
      }
    }

    public LoanDefaultFHAConsumerChoice(DefaultFieldsInfo fhaDefaultFieldsInfo)
    {
      if (fhaDefaultFieldsInfo == null)
        return;
      this.fhaDefaultFieldsInfo = fhaDefaultFieldsInfo;
      this.Reset();
    }

    public void Reset()
    {
      foreach (string loanField in LoanDefaultFHAConsumerChoice.LoanFields)
      {
        string field = this.fhaDefaultFieldsInfo.GetField(loanField);
        this.SetCurrentField(loanField, field);
      }
    }

    public DefaultFieldsInfo CommitChanges()
    {
      foreach (string loanField in LoanDefaultFHAConsumerChoice.LoanFields)
      {
        string simpleField = this.GetSimpleField(loanField);
        this.fhaDefaultFieldsInfo.SetField(loanField, simpleField);
      }
      return this.fhaDefaultFieldsInfo;
    }

    public DefaultFieldsInfo CommitChanges(SessionObjects sessionObjects)
    {
      sessionObjects.ConfigurationManager.UpdateDefaultFields(this.CommitChanges(), "FHAConsumerChoiceFieldList");
      return this.fhaDefaultFieldsInfo;
    }
  }
}
