using Core.Catalog.Domain;
using Core.Catalog.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Catalog.Interfaces
{
    public interface IManufacturerService
    {
        Task CreateManufacturer(ManufacturerDto manufacturerDto);
        Task UpdateManufacturer(ManufacturerDto manufacturerDto);
        Task<Manufacturer> Get(int id);
        Task<IEnumerable<Manufacturer>> GetAll();
        Task Remove(int id);
    }
}
