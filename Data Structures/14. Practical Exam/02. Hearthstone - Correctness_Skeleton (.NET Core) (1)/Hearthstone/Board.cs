using System;
using System.Collections.Generic;
using System.Linq;

public class Board : IBoard
{
    private Dictionary<string, Card> byName;
    private Dictionary<int, HashSet<Card>> byLevel;

    public Board()
    {
        this.byName = new Dictionary<string, Card>();
        this.byLevel = new Dictionary<int, HashSet<Card>>();

    }

    public bool Contains(string name)
    {
        return this.byName.ContainsKey(name);
    }

    public int Count()
    {
        return this.byName.Count;
    }

    public void Draw(Card card)
    {
        if (this.Contains(card.Name))
        {
            throw new ArgumentException();
        }

        this.byName.Add(card.Name, card);
        if (!this.byLevel.ContainsKey(card.Level))
        {
            this.byLevel.Add(card.Level, new HashSet<Card>());
        }

        this.byLevel[card.Level].Add(card);
    }

    public IEnumerable<Card> GetBestInRange(int start, int end)
    {
        return this.byName.Values
            .Where(x => x.Score >= start && x.Score <= end)
            .OrderByDescending(x => x.Level);
    }

    public void Heal(int health)
    {
        Card card = this.byName.Values.OrderBy(x => x.Health).First();
        card.Health += health;
    }

    public IEnumerable<Card> ListCardsByPrefix(string prefix)
    {
        IEnumerable<Card> cards = this.byName.Values
            .Where(x => x.Name.StartsWith(prefix))
            .OrderBy(x => x.Name.Substring(x.Name.Length - 1))
            .ThenBy(x => x.Level);

        return cards;
    }

    public void Play(string attackerCardName, string attackedCardName)
    {
        if (!this.Contains(attackerCardName) || !this.Contains(attackedCardName))
        {
            throw new ArgumentException();
        }

        Card attackerCard = this.byName[attackerCardName];
        Card attackedCard = this.byName[attackedCardName];

        if (attackerCard.Level != attackedCard.Level)
        {
            throw new ArgumentException();
        }

        if (attackedCard.Health <= 0)
        {
            return;
        }

        attackedCard.Health -= attackerCard.Damage;
        if (attackedCard.Health <= 0)
        {
            attackerCard.Score += attackedCard.Level;
        }
    }

    public void Remove(string name)
    {
        if (!this.Contains(name))
        {
            throw new ArgumentException();
        }

        Card card = this.byName[name];
        this.byName.Remove(name);
        this.byLevel[card.Level].Remove(card);
    }

    public void RemoveDeath()
    {
        List<Card> cardsForRemove = this.byName.Values
            .Where(x => x.Health <= 0)
            .ToList();

        foreach (var card in cardsForRemove)
        {
            this.byName.Remove(card.Name);
            this.byLevel[card.Level].Remove(card);
        }
    }

    public IEnumerable<Card> SearchByLevel(int level)
    {
        return this.byLevel[level]
            .OrderByDescending(x => x.Score);
    }
}