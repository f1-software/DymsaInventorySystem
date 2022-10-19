using Microsoft.AspNetCore.Components;
using DymsaInventory.Services;
using DymsaInventory.Models;
using Newtonsoft.Json;

namespace DymsaInventory.Pages.Inventory
{
    public partial class ItemGenrePage : ComponentBase
    {
        [Inject] IHelperService HelperService { get; set; } = null!;
        public List<ItemGenre>? ItemGenres { get; set; } = new();
        public ItemGenre CurrentItemGenre { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        public async Task LoadData()
        {
            var itemcodesquery = HelperService.DbQuery($@"select * from ItemGenre");
            ItemGenres = JsonConvert.DeserializeObject<List<ItemGenre>>(itemcodesquery);
            StateHasChanged();
            await Task.CompletedTask;
        }

        public async Task Save()
        {
            var res = HelperService.DbQuery($@"INSERT INTO ItemGenre VALUES('{CurrentItemGenre.ItemGenreName}',1)");
            await LoadData();
            CurrentItemGenre = new();
        }
    }
}
