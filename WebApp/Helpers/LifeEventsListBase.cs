using CrossComponentCommunication;
using Domain;
using Helper;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Repository;

namespace WebApp
{
    public abstract class LifeEventsListBase : ComponentBase
    {
        [Inject] CrossComponentCommunication.CrossComponentCommunication CrossComponentCommunication {get;set; }
        [Inject] LifeEventsRepository LifeEventsRepository { get; set; }
        [Inject] UsersRepository UsersRepository { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }        
        protected UserRDTO User { get; set; }
        public DateRange _DateRange { get; set; }
        [Parameter]
        public DateRange DateRange
        {
            get { 
                return _DateRange;
            }
            set
            {
                _DateRange = value;
                UpdateLifeEventsList();
            }
        }        
        public List<LifeEventRDTO> LifeEvents { get; set; } = new List<LifeEventRDTO>();
        protected override async Task OnInitializedAsync()
        {
            User = await UsersRepository.Get();
            UpdateLifeEventsList();
            CrossComponentCommunication.ListenTo(BroadcastTypeEnum.NewLifeEvent, OnNewLifeEvent);
        }
        public async void OnNewLifeEvent(IBroadcastMessage message)
        {
            if (message is NewLifeEventBroadcastMessage)
            {
                var newLifeEventMessage = message as NewLifeEventBroadcastMessage;
                var newRec = await LifeEventsRepository.Get((message as NewLifeEventBroadcastMessage).LifeEventId);
                LifeEvents.Add(newRec);
                StateHasChanged();
            }
        }
        protected string GetDurationText(int durationInMinutes)
        {
            if (durationInMinutes < 60)
            {
                if (durationInMinutes > 0)
                    return $"{durationInMinutes}'";
                else
                    return "";
            }
            else
            {
                var hours = durationInMinutes / 60;
                var minutes = durationInMinutes % 60;
                if (minutes > 0)
                    return $"{hours}h {minutes}'";
                else
                    return $"{hours}h";
            }
        }
        protected async void DeleteLifeEvent(LifeEventRDTO lifeEvent)
        {
            if (await LifeEventsRepository.Delete(lifeEvent.Id!.Value))
            {
                Snackbar.Add("Deleted", Severity.Success);
                LifeEvents = LifeEvents.Where(p => p.Id != lifeEvent.Id).ToList();
            }
            else
                Snackbar.Add("An error occured!", Severity.Error);
        }   
        protected DateTime GetEventStartDate(LifeEventRDTO lifeEvent)
        {
            return lifeEvent.From.HasValue ? lifeEvent.From.Value : lifeEvent.DateCreated!.Value;
        }        
        async void UpdateLifeEventsList()
        {
            if (User != null && DateRange!=null && DateRange.From.HasValue && DateRange.To.HasValue)
            {
                LifeEvents = await LifeEventsRepository.GetAll(DateRange.From.Value.FromIanaTimeZone(User.TimeZone), DateRange.To.Value.FromIanaTimeZone(User.TimeZone));
                StateHasChanged();
            }
            
        }
    }
}
