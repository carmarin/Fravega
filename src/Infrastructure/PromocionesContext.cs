using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using Promociones.Domain.Entities;

namespace Promociones.Infrastructure
{
    class PromocionesContext
    {
        private readonly IMongoDatabase _database = null;

        public PromocionesContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Promocion> Promociones
        {
            get
            {
                return _database.GetCollection<Promocion>("Promocion");
            }
        }
    }
}
