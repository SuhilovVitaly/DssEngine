namespace DeepSpaceSaga.Server.Services.CloseCombat;

public class AttackCalculation
{
    public static AttackResult Attack(HandToHandCombatAttacks attack, ICharacter attacker, ICharacter defender, IGenerationTool generationTool)
    {
        var resultDice = generationTool.GetInteger(1, 100);
        var IsAttackSuccessful = resultDice > 50 ? true : false;

        if(IsAttackSuccessful == false)
        {
            return new AttackResult
            {
                EnergyPoints = 0,
                HealthPoints = 0,
                IsAttackSuccessful = false,
                MoralePoints = 0,
                PainPoints = 0
            };
        }

        return new AttackResult
        {
            EnergyPoints = generationTool.GetInteger(1, 10),
            HealthPoints = generationTool.GetInteger(1, 10),
            IsAttackSuccessful = true,
            MoralePoints = generationTool.GetInteger(1, 10),
            PainPoints = generationTool.GetInteger(1, 10),
            Stability = true
        };
    }
}
