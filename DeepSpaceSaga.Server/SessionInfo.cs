using DeepSpaceSaga.Common.Abstractions.Session.Entities;

namespace DeepSpaceSaga.Server;

public class SessionInfo
{
    public int Turn { get; set; }
    
    public SessionState State { get; set; } 
}