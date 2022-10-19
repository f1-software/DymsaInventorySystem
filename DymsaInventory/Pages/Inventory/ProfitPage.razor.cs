using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using DymsaInventory.Services;
using DymsaInventory.Models;
using Newtonsoft.Json;
using Microsoft.JSInterop;

namespace DymsaInventory.Pages.Inventory
{
    public partial class ProfitPage : ComponentBase
    {
        [Inject] IHelperService HelperService {get;set;} = null!;
        [Inject] IJSRuntime js { get; set; } = null!;
        public List<Item>? Items { get; set; } = new();
        public List<PurchaseTransaction>? PurchaseTransactions { get; set; } = new();


        protected override async Task OnInitializedAsync()
        {
            var itemquery = HelperService.DbQuery($@"select * from Item");
            Items = JsonConvert.DeserializeObject<List<Item>>(itemquery);

            var purchasetransactionquery = HelperService.DbQuery($@"select * from PurchaseTransaction");
            PurchaseTransactions = JsonConvert.DeserializeObject<List<PurchaseTransaction>>(purchasetransactionquery);
            await Task.CompletedTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await js.InvokeVoidAsync("ProfitPage_CalculatePurchaseTransactionTotalAmount");
        }
    }
}