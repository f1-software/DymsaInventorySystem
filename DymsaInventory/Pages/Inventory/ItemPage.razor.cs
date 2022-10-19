using Microsoft.AspNetCore.Components;
using DymsaInventory.Services;
using DymsaInventory.Models;
using Newtonsoft.Json;

namespace DymsaInventory.Pages.Inventory
{
    public partial class ItemPage : ComponentBase
    {
        [Inject] IHelperService HelperService { get; set; } = null!;
        public List<ItemCode>? ItemCodes { get; set; } = new();
        public List<ItemGenre>? ItemGenres { get; set; } = new();
        public List<Item>? Items { get; set; } = new();
        public Item CurrentItem { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        public async Task LoadData()
        {
            var itemcodesquery = HelperService.DbQuery($@"select * from ItemCode");
            ItemCodes = JsonConvert.DeserializeObject<List<ItemCode>>(itemcodesquery);

            var itemgenresquery = HelperService.DbQuery($@"select * from ItemGenre");
            ItemGenres = JsonConvert.DeserializeObject<List<ItemGenre>>(itemgenresquery);

            var itemquery = HelperService.DbQuery($@"select * from Item");
            Items = JsonConvert.DeserializeObject<List<Item>>(itemquery);

            StateHasChanged();
            await Task.CompletedTask;
        }

        public async Task Save()
        {
            HelperService.DbQuery($@"INSERT INTO
                                                Item
                                                VALUES({CurrentItem.ItemGenreId},
                                                {CurrentItem.ItemCodeId},
                                                '{CurrentItem.ItemName}',
                                                '{CurrentItem.ItemDescription}',1)");
            await LoadData();
            CurrentItem = new();
        }
    }
}
