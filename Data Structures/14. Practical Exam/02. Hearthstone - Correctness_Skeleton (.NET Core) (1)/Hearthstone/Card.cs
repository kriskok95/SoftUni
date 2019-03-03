using System;

public class Card : IComparable<Card>
{

    public Card(string name, int damage, int score, int level)
    {
        this.Name = name;
        this.Damage = damage;
        this.Score = score;
        this.Level = level;
        this.Health = 20;
    }
    public string Name { get; set; }

    public int Damage { get; set; }

    public int Score { get; set; }

    public int Health { get; set; }

    public int Level { get; set; }

    public int CompareTo(Card other)
    {
        return this.Name.CompareTo(other.Name);
    }

    public override bool Equals(object obj)
    {
        Card card = obj as Card;
        return this.Name.Equals(card.Name);
    }

    public override int GetHashCode()
    {
        var hasCode = 352033288;
        hasCode = hasCode * -1521134295 + this.Name.GetHashCode();
        return hasCode;
    }
}