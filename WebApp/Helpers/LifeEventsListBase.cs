using CrossComponentCommunication;
using Domain;
using Helper;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Extensions;
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
                ReloadLifeEventsList();
            }
        }
        string _Filter { get; set; }
        [Parameter]
        public string Filter
        {
            get
            {
                return _Filter;
            }
            set
            {
                if (value == null)
                    _Filter = "";
                else
                    _Filter = value;
                ReloadLifeEventsList();
            }
        }
        public List<LifeEventRDTO> LifeEventsToDisplay { get; set; } = new List<LifeEventRDTO>();
        protected override async Task OnInitializedAsync()
        {
            User = await UsersRepository.Get();
            ReloadLifeEventsList();
            CrossComponentCommunication.ListenTo(BroadcastTypeEnum.NewLifeEvent, OnNewLifeEvent);
        }
        public async void OnNewLifeEvent(IBroadcastMessage message)
        {
            if (message is NewLifeEventBroadcastMessage)
            {
                var newLifeEventMessage = message as NewLifeEventBroadcastMessage;
                var newRec = await LifeEventsRepository.Get((message as NewLifeEventBroadcastMessage).LifeEventId);
                if(newRec.From.Value.ToIanaTimeZone(User.TimeZone).Day==DateTime.UtcNow.ToIanaTimeZone(User.TimeZone).Day)
                    LifeEventsToDisplay.Add(newRec);
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
                LifeEventsToDisplay = LifeEventsToDisplay.Where(p => p.Id != lifeEvent.Id).ToList();
            }
            else
                Snackbar.Add("An error occured!", Severity.Error);
        }   
        protected DateTime GetEventStartDate(LifeEventRDTO lifeEvent)
        {
            return lifeEvent.From.HasValue ? lifeEvent.From.Value : lifeEvent.DateCreated!.Value;
        }     
        async Task<List<LifeEventRDTO>> FilterLifeEventsList()
        {
            var filter = Filter.ToLower();
            return await LifeEventsRepository.GetAll(filter);
        }        
        async Task ReloadLifeEventsList()
        {
            if (User != null && DateRange != null && DateRange.From.HasValue)
            {
                if(string.IsNullOrEmpty(Filter))
                    LifeEventsToDisplay = await LifeEventsRepository.GetAllForDay(DateRange.From.Value.FromIanaTimeZone(User.TimeZone));
                else
                    LifeEventsToDisplay = await FilterLifeEventsList();
                StateHasChanged();
            }

        }
    }
}
