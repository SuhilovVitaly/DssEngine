namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface ICloseCombatService
{
    string LastStageResult { get; }

    ICharacter Pleer { get; }

    ICharacter Enemy { get; }

    void Initialization(ICharacter pleer, ICharacter enemy);

    void PleerAttack(HandToHandCombatAttacks attack);

    void EnemyAttack(HandToHandCombatAttacks attack);
}
