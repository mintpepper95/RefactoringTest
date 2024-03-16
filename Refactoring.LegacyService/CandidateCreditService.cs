using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Refactoring.LegacyService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "LegacyApp.ICandidateTestService")]
public interface ICandidateCreditService {
    [OperationContract(Action = "http://xxx.com/ICandidateTestService/GetCredit")]
    Task<int> GetCreditAsync(string firstname, string surname, DateTime dateOfBirth);
}

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface ICandidateCreditServiceChannel : ICandidateCreditService, IClientChannel {
}

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public partial class CandidateCreditServiceClient : ClientBase<ICandidateCreditService>, ICandidateCreditService {
    private ICandidateCreditServiceChannel _candidateTestServiceChannelImplementation;
    public CandidateCreditServiceClient() { }

    public CandidateCreditServiceClient(string endpointConfigurationName) :
        base(endpointConfigurationName) { }

    public CandidateCreditServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) :
        base(endpointConfigurationName, remoteAddress) { }

    public CandidateCreditServiceClient(Binding binding, EndpointAddress remoteAddress) :
        base(binding, remoteAddress) { }

    // GetCredit is I/O bound. Made it async so thread doesn't just sit idling.
    public Task<int> GetCreditAsync(string firstname, string surname, DateTime dateOfBirth) {
        return base.Channel.GetCreditAsync(firstname, surname, dateOfBirth);
    }
}
