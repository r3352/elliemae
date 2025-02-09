// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.MetricsFactory
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using Elli.Metrics.Disabled;

#nullable disable
namespace Elli.Metrics
{
  public class MetricsFactory : IMetricsFactory
  {
    private static readonly object LockObject = new object();
    private const bool Enabled = false;

    public bool MetricsCollectionEnabled => false;

    public IFormBuilderMetricsRecorder CreateFormBuilderMetricsRecorder(
      string client,
      string instance)
    {
      return (IFormBuilderMetricsRecorder) new FormBuilderMetricsRecorder();
    }

    public ILoanMetricsRecorder CreateLoanMetricsRecorder(string client, string instance)
    {
      return (ILoanMetricsRecorder) new LoanMetricsRecorder();
    }

    public IMongoUtilityMetricsRecorder CreateMongoUtilityMetricsRecorder(
      string client,
      string instance)
    {
      return (IMongoUtilityMetricsRecorder) new MongoUtilityMetricsRecorder();
    }

    public ILoanPipelineMetricsRecorder CreateLoanPipelineMetricsRecorder(
      string client,
      string instance)
    {
      return (ILoanPipelineMetricsRecorder) new LoanPipelineMetricsRecorder();
    }

    public ILoanSerializationMatrixRecorder CreateLoanSerializationMetricsRecorder(
      string client,
      string instance)
    {
      return (ILoanSerializationMatrixRecorder) new LoanSerializationMetricsRecorder();
    }

    public ILoanAccessorMetricsRecorder CreateLoanAccessorMetricsRecorder(
      string client,
      string instance)
    {
      return (ILoanAccessorMetricsRecorder) new LoanAccessorMetricsRecorder();
    }

    public IServerEventMetricsRecorder CreateServerEventMetricsRecorder()
    {
      return (IServerEventMetricsRecorder) new ServerEventMetricsRecorder();
    }

    public IContactsPaginationMetricsRecorder CreateContactsPaginationMetricsRecorder(
      string client,
      string instance)
    {
      return (IContactsPaginationMetricsRecorder) new ContactsPaginationMetricsRecorder();
    }

    public IHazelCastMetricsRecorder CreateHazelCastMetricsRecorder()
    {
      return (IHazelCastMetricsRecorder) new HazelCastMetricsRecorder();
    }
  }
}
