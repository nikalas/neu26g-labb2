namespace neu26g_labb2.Models
{
    public interface IGameContext
    {
        Player Player { get; }
        IReadOnlyList<LevelElement> Elements { get; }

        LevelElement? ElementAt(int x, int y);
        bool TryMove(Actor actor, int x, int y);
        void ResolveAttack(Actor attacker, Actor defender);
        void Remove(LevelElement element);
    }
}
