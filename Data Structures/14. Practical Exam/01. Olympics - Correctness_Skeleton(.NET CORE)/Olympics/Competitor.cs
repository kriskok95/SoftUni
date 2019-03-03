using System;

public class Competitor : IComparable<Competitor>
{
    public Competitor(int id, string name)
    {
        this.Id = id;
        this.Name = name;
        this.TotalScore = 0;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public long TotalScore { get; set; }
    public int CompareTo(Competitor other)
    {
        return this.Id.CompareTo(other.Id);
    }

    public override bool Equals(object obj)
    {
        Competitor comp = obj as Competitor;

        return this.Id.Equals(comp.Id);
    }

    public override int GetHashCode()
    {
        var hasCode = 352033288;
        hasCode = hasCode * -1521134295 + Id.GetHashCode();
        return hasCode;
    }
}
