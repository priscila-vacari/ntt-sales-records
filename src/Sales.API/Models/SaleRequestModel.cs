using System.Text.Json.Serialization;

namespace Sales.API.Models
{
    /// <summary>
    /// Classe de requisição da venda
    /// </summary>
    public class SaleRequestModel
    {
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
        /// Filial
        /// </summary>
        [JsonPropertyName("branch")]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Itens
        /// </summary>
        [JsonPropertyName("itens")]
        public List<SaleItemRequestModel> Items { get; set; } = [];
    }
}
