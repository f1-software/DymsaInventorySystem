using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using DymsaInventory.Services;
using Newtonsoft.Json;
using DymsaInventory.Models;
using DymsaInventory.Models.ViewModel;
using Sotsera.Blazor.Toaster;
using Microsoft.Extensions.Hosting;

namespace DymsaInventory.Pages.Inventory
{
    public partial class SaleTransactionPage : ComponentBase
    {
        [Inject] IToaster _toaster { get; set; } = null!;
        [Inject] IHelperService HelperService { get; set; } = null!;
        public List<ItemCode>? ItemCodes { get; set; } = new();
        public List<ItemGenre>? ItemGenres { get; set; } = new();
        public List<Item>? Items { get; set; } = new();
        public List<SaleTransaction>? SaleTransactions { get; set; } = new();
        public List<PurchaseViewModels>? PurchaseInventory { get; set; } = new();
        public List<IventoryStocksViewModels>? StocksQty { get; set; } = new();
        public SaleTransaction CurrentSaleTransaction { get; set; } = new();
        public decimal? Cost { get; set; } = new();
        public decimal? DiscountPrice { get; set; } = new();
        public int? ItemId { get; set; } = new();

//        price x qty = grosssales - discoutn = net sales



//profit: equal qty sa sold ug sa cost

//if 2pcs nahalin = 2pcs rasad nakabutang sa cost

//kung 20pcs akoa actual gipalit..ang 18 ana kay sa iventory 2 ang sa sold

        protected override async Task OnInitializedAsync()
        {
            CurrentSaleTransaction.TransactionDate = System.DateTime.UtcNow;
            CurrentSaleTransaction.DiscountOrCommission = 0;
            CurrentSaleTransaction.Qty = 0;
          
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

            var purchaseTransactions = HelperService.DbQuery($@"SELECT Item.ItemId,Item.ItemDescription,sum(cost) as Cost,sum(qty) as totalQty,PriceCost 
                                                                FROM PurchaseTransaction
                                                                INNER JOIN Item on PurchaseTransaction.ItemId =item.ItemId
                                                                WHERE
                                                                PurchaseTransaction.ItemId =Item.ItemId
                                                                GROUP BY Item.ItemId,item.ItemDescription,PriceCost");
            PurchaseInventory = JsonConvert.DeserializeObject<List<PurchaseViewModels>>(purchaseTransactions);

            var stocksQty = HelperService.DbQuery($@"SELECT PurchaseTransaction.ItemId,SUM(PurchaseTransaction.qty) as StockQty,(SUM(PurchaseTransaction.qty) - SUM(SaleTransaction.Qty)) as Qty
                                                    FROM PurchaseTransaction
                                                    FULL JOIN SaleTransaction on PurchaseTransaction.itemid = SaleTransaction.ItemId
                                                    GROUP BY  PurchaseTransaction.ItemId");
            StocksQty = JsonConvert.DeserializeObject<List<IventoryStocksViewModels>>(stocksQty);

            StateHasChanged();
            await Task.CompletedTask;
        }

        public async Task SaveToCart()
        {
            var remainingQty = StocksQty?.Where(s => s.ItemId == CurrentSaleTransaction.ItemId).Select(s => s.Qty).FirstOrDefault();
            if (CurrentSaleTransaction.Qty > 0 )
            {
                
            var result = CurrentSaleTransaction.Qty * Cost;
            if(CurrentSaleTransaction.DiscountOrCommission >0)
            {
                CurrentSaleTransaction.Net = result - CurrentSaleTransaction.DiscountOrCommission;
            }
            else
            {
                CurrentSaleTransaction.Net = result;
            }
            
            CurrentSaleTransaction.Description = Items?.Where(i => i.ItemId == CurrentSaleTransaction.ItemId).Select(i => i.ItemDescription).FirstOrDefault();
            CurrentSaleTransaction.Status = Items?.Where(i => i.ItemId == CurrentSaleTransaction.ItemId).Select(i => i.IsActive).FirstOrDefault();
           
            CurrentSaleTransaction.Cost = Cost;
 
                HelperService.DbQuery($@"INSERT INTO
                                                SaleTransaction
                                                VALUES('{System.DateTime.UtcNow}',
                                                {CurrentSaleTransaction.ItemId},
                                                {CurrentSaleTransaction.Qty},
                                                {CurrentSaleTransaction.DiscountOrCommission},
                                                '{CurrentSaleTransaction.Comment}',
                                                '{CurrentSaleTransaction.Description}',                                                
                                                {CurrentSaleTransaction.Net},                                            
                                                {CurrentSaleTransaction.Cost},1)");

                SaleTransactions?.Add(CurrentSaleTransaction);
                // Console.WriteLine("Result: {0}",System.Text.Json.JsonSerializer.Serialize(CurrentSaleTransaction));
                    _toaster.Success($"Item '{CurrentSaleTransaction.Description}' have been sold successfully.");
            }
            else
            {
                _toaster.Warning("Insuffecient stocks.");
            }
            await Task.CompletedTask;
        }
        public async Task InsertSales()
        {
            if(SaleTransactions?.Count > 0)
            {

            HelperService.DbQuery($@"INSERT INTO
                                                SaleTransaction
                                                VALUES('{System.DateTime.UtcNow}',
                                                {CurrentSaleTransaction.ItemId},
                                                {CurrentSaleTransaction.Qty},
                                                {CurrentSaleTransaction.DiscountOrCommission},
                                                {CurrentSaleTransaction.Net},
                                                '{CurrentSaleTransaction.Comment}',
                                                {CurrentSaleTransaction.Status},   
                                                '{CurrentSaleTransaction.Description}'
                                                {CurrentSaleTransaction.Cost},1)");
            }
            CurrentSaleTransaction = new();
            CurrentSaleTransaction.TransactionDate = System.DateTime.UtcNow;
            CurrentSaleTransaction.Qty =0;
            await Task.CompletedTask;
        }
        public async Task ShowPrice()
        {
            // && CurrentSaleTransaction.Qty > 0
            if (CurrentSaleTransaction.ItemId != null)
            {
                Cost =  PurchaseInventory?.Where(x => x.ItemId == CurrentSaleTransaction.ItemId).Select(x => x.PriceCost).FirstOrDefault();

                var result = CurrentSaleTransaction.Qty * Cost;
                if (CurrentSaleTransaction.DiscountOrCommission>0)
                {
                    DiscountPrice = result - CurrentSaleTransaction.DiscountOrCommission;
                }
                else
                    DiscountPrice = result;
            }
        }
    }
}