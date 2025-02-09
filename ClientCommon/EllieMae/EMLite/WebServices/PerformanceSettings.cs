// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.PerformanceSettings
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://encompass.elliemae.com/jedservices/")]
  [Serializable]
  public class PerformanceSettings
  {
    private int submissionIntervalField;
    private int queueSizeField;

    public int SubmissionInterval
    {
      get => this.submissionIntervalField;
      set => this.submissionIntervalField = value;
    }

    public int QueueSize
    {
      get => this.queueSizeField;
      set => this.queueSizeField = value;
    }
  }
}
