// {}(function () {
//     window.ProfitPageCalculatePurchaseTransactionTotalAmount = function () {
//         var sum = 0;
//         $(".total_transaction_amount").each(function(){
//             sum += parseFloat($(this).text());
//         });
//         $('#total_amount_purchase_transaction').text(sum);
//     }
// });

function ProfitPage_CalculatePurchaseTransactionTotalAmount(params) {
    var sum = 0;
    $(".total_transaction_amount").each(function(){
        sum += parseFloat($(this).text());
    });
    $('#total_amount_purchase_transaction').text(sum);
}