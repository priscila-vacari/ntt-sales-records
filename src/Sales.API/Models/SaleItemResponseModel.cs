using System.Text.Json.Serialization;

namespace Sales.API.Models
{
    /// <summary>
    /// Classe de resposta do item da venda
    /// </summary>
    public class SaleItemResponseModel
    {
        /// <summary>
        /// Id do produto
        /// </summary>
        [JsonPropertyName("saleId")]
        public int SaleId { get; set; }

        /// <summary>
        /// Id do produto
        /// </summary>
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        /// <summary>
        /// Quantidade
        /// </summary>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [JsonPropertyName("totalValue")]
        public decimal TotalValue { get; set; }
    }
}
