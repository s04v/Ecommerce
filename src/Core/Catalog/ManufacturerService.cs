using Common;
using Common.Data;
using Common.Exceptions;
using Common.Services;
using Core.Activities;
using Core.AdminActivities.Domain;
using Core.Catalog.Domain;
using Core.Catalog.Dtos;
using Core.Catalog.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Catalog
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IStorageService _storageService;
        private readonly IAdminActivityService _activityService;

        public ManufacturerService(IApplicationDbContext dbContext, IStorageService storageService, IAdminActivityService activityService)
        {
            _dbContext = dbContext;
            _storageService = storageService;
            _activityService = activityService;
        }

        public async Task CreateManufacturer(ManufacturerDto manufacturerDto)
        {
            string fileExt = MimeTypeHelper.MimeToExtension(manufacturerDto.MimeType);
            string fileName = $@"{Guid.NewGuid()}.{fileExt}";

            await _storageService.SaveFileAsync(manufacturerDto.Picture, fileName);

            var manufacturer = new Manufacturer
            {
                Name = manufacturerDto.Name,
                Picture = fileName,
            };

            await _dbContext.AddAsync(manufacturer);
            await _dbContext.SaveChangesAsync();

            await _activityService.InsertActivity(AdminActivityAreaEnum.Manufacturer,
                $"Created \"{manufacturer.Name}\" manufacturer");
        }

        public async Task<Manufacturer> Get(int id)
        {
            return await _dbContext.Manufacturer
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Manufacturer>> GetAll()
        {
            return await _dbContext.Manufacturer
                .ToListAsync();
        }

        public async Task UpdateManufacturer(ManufacturerDto manufacturerDto)
        {
            var manufacturer = await _dbContext.Manufacturer
                .Where(o => o.Id == manufacturerDto.Id)
                .FirstOrDefaultAsync();

            if (manufacturer == null)
            {
                throw new DomainException("Manufacturer not found");
            }

            manufacturer.Name = manufacturerDto.Name;

            if (manufacturerDto.Picture != null)
            {
                string fileExt = MimeTypeHelper.MimeToExtension(manufacturerDto.MimeType);
                string fileName = $@"{Guid.NewGuid()}.{fileExt}";

                await _storageService.SaveFileAsync(manufacturerDto.Picture, fileName);

                await _storageService.DeleteFileAsync(manufacturer.Picture);

                manufacturer.Picture = fileName;
            }

            await _dbContext.SaveChangesAsync();

            await _activityService.InsertActivity(AdminActivityAreaEnum.Manufacturer,
                $"Updated \"{manufacturer.Name}\" manufacturer.");
        }

        public async Task Remove(int id)
        {
            var manufacturer = new Manufacturer() { Id = id };

            _dbContext.Manufacturer.Attach(manufacturer);
            _dbContext.Manufacturer.Remove(manufacturer);
            await _dbContext.SaveChangesAsync();

            await _activityService.InsertActivity(AdminActivityAreaEnum.Manufacturer,
               $"Removed \"{manufacturer.Name}\" manufacturer.");
        }
    }
}
