using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HangOut.Domain.Payload.Request.Business
{
    public class CreateBusinessRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name {  get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public bool Active { get; set; } = true;

        public string? Vibe {  get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Latitude is required")]
        public string Latitude { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Longitude is required")]
        public string Longitude { get; set; }

        [Required(ErrorMessage ="Address is required")]
        public string Address {  get; set; }

        [Required(ErrorMessage = "Province is required")]
        public string Province {  get; set; }

        public string? Description {  get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]        
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public DateTime? LastModifiedDate { get; set; }

        public List<string> ImageUrl { get; set; }
    }
}
