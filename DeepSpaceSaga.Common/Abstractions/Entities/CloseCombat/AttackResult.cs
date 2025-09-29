namespace DeepSpaceSaga.Server.Services.CloseCombat;

public class AttackResult
{
    public int HealthPoints { get; set; } 
    public int MoralePoints { get; set; }
    public int PainPoints { get; set; }
    public int EnergyPoints { get; set; }
    public bool Stability { get; set; }
    public bool IsAttackSuccessful { get; set; }
}
