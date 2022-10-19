using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using DymsaInventory.Services;
using Newtonsoft.Json;
using DymsaInventory.Models;

namespace DymsaInventory.Pages.Inventory
{
    public partial class SaleTransactionPage : ComponentBase
    {
        [Inject] IHelperService HelperService { get; set; } = null!;
        public List<ItemCode>? ItemCodes { get; set; } = new();
        public List<ItemGenre>? ItemGenres { get; set; } = new();
        public List<Item>? Items { get; set; } = new();
        public List<SaleTransaction>? SaleTransactions { get; set; } = new();
        public SaleTransaction CurrentSaleTransaction { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            CurrentSaleTransaction.TransactionDate = System.DateTime.UtcNow;
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

            var saletransactionquery = HelperService.DbQuery($@"select * from SaleTransaction");
            SaleTransactions = JsonConvert.DeserializeObject<List<SaleTransaction>>(saletransactionquery);

            StateHasChanged();
            await Task.CompletedTask;
        }

        public async Task Save()
        {
            HelperService.DbQuery($@"INSERT INTO
                                                SaleTransaction
                                                VALUES('{System.DateTime.UtcNow}',
                                                {CurrentSaleTransaction.ItemId},
                                                {CurrentSaleTransaction.Qty},
                                                {CurrentSaleTransaction.DiscountOrCommission},
                                                {CurrentSaleTransaction.Net},
                                                '{CurrentSaleTransaction.Comment}',1)");
            SaleTransactions?.Add(CurrentSaleTransaction);
            CurrentSaleTransaction = new();
            await Task.CompletedTask;
        }
    }
}