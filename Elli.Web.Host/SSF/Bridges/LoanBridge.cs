// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SSF.Bridges.LoanBridge
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using Elli.Web.Host.SSF.Context;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host.SSF.Bridges
{
  public class LoanBridge : Bridge
  {
    private const string className = "LoanBridge";
    private static readonly string sw = Tracing.SwThinThick;

    internal LoanBridge(SSFContext context)
      : base(context)
    {
      if (this.context.loanDataMgr == null)
        throw new NullReferenceException("No loan currently loaded.");
    }

    public string all()
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'all'");
      try
      {
        Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Creating EBSRestClient");
        EBSRestClient ebsRestClient = new EBSRestClient(this.context.loanDataMgr, this.context.getAccessToken(false));
        Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Calling v3 GetLoan");
        Task<string> loan3 = ebsRestClient.GetLoan3();
        Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Waiting for v3 GetLoan Task");
        Task.WaitAll((Task) loan3);
        return loan3.Result;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanBridge.sw, TraceLevel.Error, nameof (LoanBridge), "Error occured 'all': " + ex.Message);
        throw;
      }
    }

    public void apply(string loanObject)
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'apply'");
      throw new NotImplementedException();
    }

    public void commit()
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'commit'");
      throw new NotImplementedException();
    }

    public string getField(string fieldId)
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'getField': fieldId=" + fieldId);
      try
      {
        return this.context.loanDataMgr.LoanData.GetField(fieldId);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanBridge.sw, TraceLevel.Error, nameof (LoanBridge), "Error occured 'getField': " + ex.Message);
        throw;
      }
    }

    public string getFields(string fieldIds)
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'getFields': fieldIds=" + fieldIds);
      try
      {
        string[] strArray = JsonConvert.DeserializeObject<string[]>(fieldIds);
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (string str in strArray)
          dictionary[str] = this.context.loanDataMgr.LoanData.GetField(str);
        return JsonConvert.SerializeObject((object) dictionary);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanBridge.sw, TraceLevel.Error, nameof (LoanBridge), "Error occured 'getFields': " + ex.Message);
        throw;
      }
    }

    public string getFieldsOptions(string fieldIds)
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'getFieldsOptions': fieldIds=" + fieldIds);
      try
      {
        string[] strArray = JsonConvert.DeserializeObject<string[]>(fieldIds);
        Dictionary<string, List<LoanBridge.FieldsOptionsModel>> dictionary = new Dictionary<string, List<LoanBridge.FieldsOptionsModel>>();
        foreach (string str in strArray)
        {
          FieldDefinition fieldDefinition = this.context.loanDataMgr.LoanData.GetFieldDefinition(str);
          if (fieldDefinition == null)
            throw new ArgumentException("Invalid FieldId:" + str);
          List<LoanBridge.FieldsOptionsModel> fieldsOptionsModelList = new List<LoanBridge.FieldsOptionsModel>();
          foreach (FieldOption fieldOption in fieldDefinition.Options.ToArray())
            fieldsOptionsModelList.Add(new LoanBridge.FieldsOptionsModel()
            {
              value = fieldOption.Value,
              name = fieldOption.Text
            });
          dictionary.Add(str, fieldsOptionsModelList);
        }
        return JsonConvert.SerializeObject((object) dictionary);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanBridge.sw, TraceLevel.Error, nameof (LoanBridge), "Error occured 'getFieldsOptions': " + ex.Message);
        throw;
      }
    }

    public void setFields(string fieldMap)
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'setFields': fieldIds=" + fieldMap);
      try
      {
        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(fieldMap);
        foreach (string key in dictionary.Keys)
          this.context.loanDataMgr.LoanData.SetField(key, dictionary[key]);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanBridge.sw, TraceLevel.Error, nameof (LoanBridge), "Error occured 'setFields': " + ex.Message);
        throw;
      }
    }

    public void merge()
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'merge'");
      throw new NotImplementedException();
    }

    public bool isReadOnly()
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'isReadOnly'");
      try
      {
        return this.context.loanDataMgr.Writable;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanBridge.sw, TraceLevel.Error, nameof (LoanBridge), "Error occured 'isReadOnly': " + ex.Message);
        throw;
      }
    }

    public void applyLock(string fieldId, bool locked)
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'applyLock': fieldId=" + fieldId + " locked=" + locked.ToString());
      try
      {
        if (locked == this.context.loanDataMgr.LoanData.IsLocked(fieldId))
          return;
        if (locked)
          this.context.loanDataMgr.LoanData.AddLock(fieldId);
        else
          this.context.loanDataMgr.LoanData.RemoveLock(fieldId);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanBridge.sw, TraceLevel.Error, nameof (LoanBridge), "Error occured 'applyLock': " + ex.Message);
        throw;
      }
    }

    public void calculate()
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'calculate'");
      throw new NotImplementedException();
    }

    public string getCurrentApplication()
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'getCurrentApplication'");
      throw new NotImplementedException();
    }

    public string execAction(string type)
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'execAction': type=" + type);
      throw new NotImplementedException();
    }

    public string getCollection(string name)
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'getCollection': name=" + name);
      throw new NotImplementedException();
    }

    public void removeAt(int index, int applicationIndex)
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'removeAt': index=" + (object) index + " applicationIndex=" + (object) applicationIndex);
      throw new NotImplementedException();
    }

    public string getSnapshot()
    {
      Tracing.Log(LoanBridge.sw, TraceLevel.Verbose, nameof (LoanBridge), "Entering 'getSnapshot'");
      try
      {
        if (this.context.loanDataMgr.LoanData == null)
          throw new NullReferenceException("No loan currently loaded.");
        LockRequestLog currentLockRequest = this.context.loanDataMgr.LoanData.GetLogList().GetCurrentLockRequest();
        Hashtable lockRequestSnapshot = currentLockRequest == null ? (Hashtable) null : currentLockRequest.GetLockRequestSnapshot();
        return lockRequestSnapshot == null ? string.Empty : JsonConvert.SerializeObject((object) lockRequestSnapshot);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanBridge.sw, TraceLevel.Error, nameof (LoanBridge), "Error occured 'getSnapshot': " + ex.Message);
        throw;
      }
    }

    public bool openLoan(string loanGuid)
    {
      Tracing.Log(LoanBridge.sw, nameof (LoanBridge), TraceLevel.Info, "OpenLoan: " + loanGuid);
      try
      {
        return Session.Application.GetService<ILoanConsole>().OpenLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanBridge.sw, nameof (LoanBridge), TraceLevel.Error, "OpenLoan failed: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to open the loan:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    private class FieldsOptionsModel
    {
      public string value { get; set; }

      public string name { get; set; }
    }
  }
}
