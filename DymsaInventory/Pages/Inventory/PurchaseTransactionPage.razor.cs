using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using DymsaInventory.Services;
using Newtonsoft.Json;
using DymsaInventory.Models;
using Microsoft.JSInterop;

namespace DymsaInventory.Pages.Inventory
{
    public partial class PurchaseTransactionPage : ComponentBase
    {
        [Inject] IHelperService HelperService { get; set; } = null!;
        public List<ItemCode>? ItemCodes { get; set; } = new();
        public List<ItemGenre>? ItemGenres { get; set; } = new();
        public List<Item>? Items { get; set; } = new();
        public List<PurchaseTransaction>? PurchaseTransactions { get; set; } = new();
        public PurchaseTransaction CurrentPurchaseTransaction { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            CurrentPurchaseTransaction.TransactionDate = System.DateTime.UtcNow;
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

            var purchasetransactionquery = HelperService.DbQuery($@"select * from PurchaseTransaction");
            PurchaseTransactions = JsonConvert.DeserializeObject<List<PurchaseTransaction>>(purchasetransactionquery);



            StateHasChanged();
            await Task.CompletedTask;
        }

        public async Task Save()
        {
            if (CurrentPurchaseTransaction.PurchaseTransactionId > 0)
            {
                HelperService.DbQuery($@"UPDATE PurchaseTransaction
                SET TransactionDate = '{CurrentPurchaseTransaction.TransactionDate}',
                ItemId = {CurrentPurchaseTransaction.ItemId},
                Cost = {CurrentPurchaseTransaction.Cost},
                Qty = {CurrentPurchaseTransaction.Qty},
                AdditionalFee = {CurrentPurchaseTransaction.AdditionalFee},
                Comment = '{CurrentPurchaseTransaction.Comment}',
                IsActive = 1
                WHERE PurchaseTransactionId = {CurrentPurchaseTransaction.PurchaseTransactionId}");
            }
            else
            {
                HelperService.DbQuery($@"INSERT INTO
                                                PurchaseTransaction
                                                VALUES('{System.DateTime.UtcNow}',
                                                {CurrentPurchaseTransaction.ItemId},
                                                {CurrentPurchaseTransaction.Cost},
                                                {CurrentPurchaseTransaction.Qty},
                                                {CurrentPurchaseTransaction.AdditionalFee},
                                                '{CurrentPurchaseTransaction.Comment}',1)");
            }

            await LoadData();
            CurrentPurchaseTransaction = new();
        }

        public async Task Edit(PurchaseTransaction model)
        {
            CurrentPurchaseTransaction = model;
            await Task.CompletedTask;
        }
    }
}