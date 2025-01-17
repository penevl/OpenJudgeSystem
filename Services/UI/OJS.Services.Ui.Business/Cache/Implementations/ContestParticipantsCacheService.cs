﻿namespace OJS.Services.Ui.Business.Cache.Implementations;

using OJS.Services.Infrastructure.Constants;
using OJS.Services.Infrastructure.Cache;
using OJS.Services.Common.Models.Cache;
using System.Threading.Tasks;

public class ContestParticipantsCacheService : IContestParticipantsCacheService
{
    private readonly ICacheService cache;
    private readonly IParticipantsBusinessService participantsBusinessService;

    public ContestParticipantsCacheService(ICacheService cache, IParticipantsBusinessService participantsBusinessService)
    {
        this.cache = cache;
        this.participantsBusinessService = participantsBusinessService;
    }

    public async Task<int> GetCompeteContestParticipantsCount(int contestId, int cacheSeconds = CacheConstants.FiveMinutesInSeconds)
        => await this.cache.Get(
            string.Format(CacheConstants.CompeteContestParticipantsCount, contestId),
            () => this.participantsBusinessService.GetCompeteParticipantsCount(contestId),
            cacheSeconds);

    public async Task<int> GetPracticeContestParticipantsCount(int contestId, int cacheSeconds = CacheConstants.FiveMinutesInSeconds)
        => await this.cache.Get(
            string.Format(CacheConstants.PracticeContestParticipantsCount, contestId),
            () => this.participantsBusinessService.GetPracticeParticipantsCount(contestId),
            cacheSeconds);
}