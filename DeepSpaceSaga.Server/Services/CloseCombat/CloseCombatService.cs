namespace DeepSpaceSaga.Server.Services.CloseCombat;

public class CloseCombatService : ICloseCombatService
{
    public string LastStageResult { get; private set; }

    public string LastStagePosition { get; private set; } = "0-0-0-0-0";    

    public ICharacter Pleer { get; private set; }

    public ICharacter Enemy { get; private set; }

    private HandToHandCombatAttacks _attack;
    private AttackResult _lastAttackResult;
    private IGenerationTool _generationTool;

    public CloseCombatService(IGenerationTool generationTool)
    {
        _generationTool = generationTool ?? throw new ArgumentNullException(nameof(generationTool));
    }

    public AttackResult EnemyAttack(HandToHandCombatAttacks attack)
    {
        return null;
    }

    public void Initialization(ICharacter pleer, ICharacter enemy)
    {
        Pleer = pleer;
        Enemy = enemy;
    }

    public AttackResult PleerAttack(HandToHandCombatAttacks attack)
    {
        _attack = attack;

        _lastAttackResult = AttackCalculation.Attack(_attack, Pleer, Enemy, _generationTool);

        LastStagePosition = $"0-{(int)_attack}-1-{(_lastAttackResult.IsAttackSuccessful ? "1" : "0")}-0";

        return _lastAttackResult;
    }    
}
