using System.Text.Json.Serialization;

namespace Sales.API.Models
{
    /// <summary>
    /// Classe de requisição do item da venda
    /// </summary>
    public class SaleItemRequestModel
    {
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
    }
}
