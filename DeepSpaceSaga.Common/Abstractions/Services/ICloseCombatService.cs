namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface ICloseCombatService
{
    string LastStagePosition { get; }
    string LastStageResult { get; }

    ICharacter Pleer { get; }

    ICharacter Enemy { get; }

    void Initialization(ICharacter pleer, ICharacter enemy);

    AttackResult PleerAttack(HandToHandCombatAttacks attack);

    AttackResult EnemyAttack(HandToHandCombatAttacks attack);
}
