using System.Text.Json.Serialization;

namespace Sales.API.Models
{
    /// <summary>
    /// Classe de resposta da venda
    /// </summary>
    public class SaleResponseModel
    {
        /// <summary>
        /// Id da venda
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Número da venda
        /// </summary>
        [JsonPropertyName("number")]
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Data da venda
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Cliente
        /// </summary>
        [JsonPropertyName("customer")]
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        /// Valor total
        /// </summary>
        [JsonPropertyName("totalValue")]
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Filial
        /// </summary>
        [JsonPropertyName("branch")]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a venda foi cancelada
        /// </summary>
        [JsonPropertyName("isCancelled")]
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Itens
        /// </summary>
        [JsonPropertyName("itens")]
        public List<SaleItemResponseModel> Items { get; set; } = [];
    }
}
