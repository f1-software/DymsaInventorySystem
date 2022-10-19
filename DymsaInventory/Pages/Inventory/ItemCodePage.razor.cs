using Microsoft.AspNetCore.Components;
using DymsaInventory.Services;
using DymsaInventory.Models;
using Newtonsoft.Json;

namespace DymsaInventory.Pages.Inventory
{
    public partial class ItemCodePage : ComponentBase
    {
        [Inject] IHelperService HelperService { get; set; } = null!;
        public List<ItemCode>? ItemCodes { get; set; } = new();
        public ItemCode CurrentItemCode { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        public async Task LoadData()
        {
            var itemcodesquery = HelperService.DbQuery($@"select * from ItemCode");
            ItemCodes = JsonConvert.DeserializeObject<List<ItemCode>>(itemcodesquery);
            StateHasChanged();
            await Task.CompletedTask;
        }

        public async Task SaveItemCode()
        {
            var res = HelperService.DbQuery($@"INSERT INTO ItemCode VALUES('{CurrentItemCode.ItemCodeName}',1)");
            await LoadData();
            CurrentItemCode = new();
        }
    }
}
