namespace DeepSpaceSaga.Common.Extensions;

public static class CharacterExtensions
{
    public static void ApplyEnergyCost(this ICharacter character, int value)
    {
        character.Stats.Current.Energy -= value;
        if (character.Stats.Current.Energy < 0) character.Stats.Current.Energy = 0;
    }

    public static void ApplyDamage(this ICharacter character, int value)
    {
        character.Stats.Current.Health -= value;

        if (character.Stats.Current.Health < 0) character.Stats.Current.Health = 0;
    }

    public static void ApplyPain(this ICharacter character, int value)
    {
        character.Stats.Current.Pain += value;
        if (character.Stats.Current.Pain > 100) character.Stats.Current.Pain = 100;
    }

    public static void ApplyMoraleReduce(this ICharacter character, int value)
    {
        character.Stats.Current.Morale -= value;
        if (character.Stats.Current.Morale < 0) character.Stats.Current.Morale = 0;
    }
}
