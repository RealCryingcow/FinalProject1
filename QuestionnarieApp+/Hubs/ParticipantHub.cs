using Microsoft.AspNetCore.SignalR;

namespace QuestionnarieApp_.Hubs
{
    public class ParticipantHub : Hub
    {
        public async Task NotifyParticipantCountChange(int questionnaireId, int count)
        {
            await Clients.All.SendAsync("UpdateParticipantCount", questionnaireId, count);
        }
    }
}
