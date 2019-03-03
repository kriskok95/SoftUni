using System;
using System.Collections.Generic;
using System.Linq;

public class Olympics : IOlympics
{
    private Dictionary<int, Competitor> competitorById;
    private Dictionary<int, Competition> competitionById;

    public Olympics()
    {
        this.competitorById = new Dictionary<int, Competitor>();
        this.competitionById = new Dictionary<int, Competition>();
    }

    public void AddCompetition(int id, string name, int participantsLimit)
    {
        if (this.competitionById.ContainsKey(id))
        {
            throw new ArgumentException();
        }
        this.competitionById.Add(id, new Competition(name, id, participantsLimit));
        this.competitionById[id].Competitors = new HashSet<Competitor>();
    }

    public void AddCompetitor(int id, string name)
    {
        if (this.competitorById.ContainsKey(id))
        {
            throw new ArgumentException();
        }
        this.competitorById.Add(id, new Competitor(id, name));
    }

    public void Compete(int competitorId, int competitionId)
    {
        if (!this.competitionById.ContainsKey(competitionId) || !this.competitorById.ContainsKey(competitorId))
        {
            throw new ArgumentException();
        }

        Competition competition = this.competitionById[competitionId];
        Competitor competitor = this.competitorById[competitorId];
        competition.Competitors.Add(competitor);
        competitor.TotalScore += competition.Score;
    }

    public int CompetitionsCount()
    {
        return this.competitionById.Count;
    }

    public int CompetitorsCount()
    {
        return this.competitorById.Count;
    }

    public bool Contains(int competitionId, Competitor comp)
    {
        if (!this.competitionById.ContainsKey(competitionId))
        {
            throw new ArgumentException();
        }
        return this.competitionById[competitionId].Competitors.Contains(comp);
    }

    public void Disqualify(int competitionId, int competitorId)
    {
        if (!this.Contains(competitionId, new Competitor(competitorId, "")))
        {
            throw new ArgumentException();
        }
        if (!this.competitionById.ContainsKey(competitionId) || !this.competitorById.ContainsKey(competitorId))
        {
            throw new ArgumentException();
        }

        Competition competition = this.competitionById[competitionId];
        Competitor competitor = this.competitorById[competitorId];
        if (competition.Competitors.Contains(competitor))
        {
            competition.Competitors.Remove(competitor);
            competitor.TotalScore -= competition.Score;
        }
    }

    public IEnumerable<Competitor> FindCompetitorsInRange(long min, long max)
    {
        return this.competitorById.Values
            .Where(x => x.TotalScore > min && x.TotalScore <= max)
            .OrderBy(x => x.Id);
    }

    public IEnumerable<Competitor> GetByName(string name)
    {
        IEnumerable<Competitor> result = this.competitorById.Values.Where(x => x.Name == name)
            .OrderBy(x => x.Id);

        if (!result.Any())
        {
            throw new ArgumentException();
        }

        return result;
    }

    public Competition GetCompetition(int id)
    {
        if (!this.competitionById.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        return this.competitionById[id];
    }

    public IEnumerable<Competitor> SearchWithNameLength(int min, int max)
    {
        return this.competitorById.Values
            .Where(x => x.Name.Length >= min && x.Name.Length <= max)
            .OrderBy(x => x.Id);
    }
}