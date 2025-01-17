﻿namespace OJS.Services.Administration.Business.Implementations;

using Microsoft.EntityFrameworkCore;
using OJS.Services.Administration.Data;
using System.Threading.Tasks;
using System.Linq;

public class ParticipantsBusinessService : IParticipantsBusinessService
{
    private readonly IParticipantsDataService participantsData;
    private readonly IParticipantScoresDataService scoresDataService;

    public ParticipantsBusinessService(
        IParticipantsDataService participantsData,
        IParticipantScoresDataService scoresDataService)
    {
        this.participantsData = participantsData;
        this.scoresDataService = scoresDataService;
    }

    public async Task UpdateTotalScoreSnapshotOfParticipants()
        => await this.participantsData.UpdateTotalScoreSnapshot();

    public async Task RemoveDuplicateParticipantScores()
    {
        var duplicateGroups = await this.scoresDataService
            .GetAll()
            .GroupBy(ps => new { ps.IsOfficial, ps.ProblemId, ps.ParticipantId })
            .Where(psGroup => psGroup.Count() > 1)
            .Select(psGroup => new
            {
                GroupKey = psGroup.Key,
                ScoresToRemove = psGroup
                    .OrderByDescending(ps => ps.Points)
                    .Skip(1),
            })
            .ToListAsync();

        var participantScoresToRemove = duplicateGroups
            .SelectMany(group => group.ScoresToRemove)
            .ToList();

        await this.scoresDataService.Delete(participantScoresToRemove);
    }
}