namespace DeepSpaceSaga.Server.Services.CloseCombat;

public class CloseCombatService : ICloseCombatService
{
    public string LastStageResult { get; private set; }

    public ICharacter Pleer { get; private set; }

    public ICharacter Enemy { get; private set; }

    public void EnemyAttack(HandToHandCombatAttacks attack)
    {
        
    }

    public void Initialization(ICharacter pleer, ICharacter enemy)
    {
        Pleer = pleer;
        Enemy = enemy;
    }

    public void PleerAttack(HandToHandCombatAttacks attack)
    {
        
    }
}
