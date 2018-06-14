using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Promociones.Domain.Entities
{
    public class Promocion
    {
        [BsonId(IdGenerator = typeof(AscendingGuidGenerator))]
        public Guid IdPromocion { get; set; }
        public int[] TipoMedioPagoId { get; set; } 
        public int[] EntidadFinancieraId { get; set; }
        public int[] MedioPagoId { get; set; }
        public int? MaxCantidadDeCuotas { get; set; } 
        public int[] ProductoCategoriaIds { get; set; }
        public decimal? PorcentajeDecuento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaModificacion { get; set; }
    }
}
