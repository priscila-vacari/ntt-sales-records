using System.Text.Json.Serialization;

namespace Sales.API.Models
{
    /// <summary>
    /// Classe de resposta da requisição do pedido
    /// </summary>
    public class SaleCreateResponseModel
    {
        /// <summary>
        /// Id 
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
