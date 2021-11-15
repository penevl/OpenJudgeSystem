namespace OJS.Services.Ui.Data
{
    using OJS.Data.Models.Participants;
    using OJS.Services.Common.Data.Infrastructure;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IParticipantsDataService : IDataService<Participant>
    {
        Task Delete(IEnumerable<Participant> participants);
    }
}