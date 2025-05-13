using DeepSpaceSaga.Common.Implementation.Services;

namespace DeepSpaceSaga.Tests.ServerTests;

public class LocalGameServerTests
{
    private readonly LocalGameServer _sut;

    public LocalGameServerTests()
    {
        // Initialize the logger
        TestLoggerRepository.Initialize();
        _sut = new LocalGameServer(new GameFlowService(new SessionInfo(), new Executor( new SessionInfo())));
    }
    
} 