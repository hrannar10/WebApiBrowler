using System;
using System.Collections.Generic;
using WebApiBrowler.Entities;
using WebApiBrowler.Helpers;
using WebApiBrowler.Models;

namespace WebApiBrowler.Services
{
    public interface IAssetService
    {
        IEnumerable<Asset> GetAll();
        Asset GetById(Guid id);
        Asset Create(Asset asset);
        void Update(Asset assetParam);
        void Delete(Guid id);
        int GetStatusId(string status);
        IEnumerable<AssetType> GetTypes();
        AssetType CreateAssetType(AssetType assetType);
    }

    public class AssetService : IAssetService
    {
        private readonly ApplicationDbContext _context;

        public AssetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Asset> GetAll()
        {
            return _context.Assets;
        }

        public Asset GetById(Guid id)
        {
            return _context.Assets.Find(id);
        }

        public Asset Create(Asset asset)
        {
            // validation
            if (asset == null)
            {
                throw new AppException("Asset not found");
            }
            _context.Assets.Add(asset);
            _context.SaveChanges();

            return asset;
        }

        public void Update(Asset assetParam)
        {
            var asset = _context.Assets.Find(assetParam.Id);

            if (asset == null)
            {
                throw new AppException("Asset not found");
            }

            // update asset properties
            asset.Description = assetParam.Description;
            asset.Name = assetParam.Name;
            asset.StatusId = assetParam.StatusId;
            asset.TypeId = assetParam.TypeId;

            _context.Assets.Update(asset);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var asset = _context.Assets.Find(id);
            if (asset == null) return;

            _context.Assets.Remove(asset);
            _context.SaveChanges();
        }

        public int GetStatusId(string status)
        {
            switch ((Enums.AssetStatus)Enum.Parse(typeof(Enums.AssetStatus), status))
            {
                case Enums.AssetStatus.Available:
                    return (int)Enums.AssetStatus.Available;
                case Enums.AssetStatus.Unavailable:
                    return (int)Enums.AssetStatus.Unavailable;
                case Enums.AssetStatus.Damaged:
                    return (int)Enums.AssetStatus.Damaged;
                case Enums.AssetStatus.Lost:
                    return (int)Enums.AssetStatus.Lost;
                default:
                    return (int)Enums.AssetStatus.Unknown;
            }
        }

        public IEnumerable<AssetType> GetTypes()
        {
            return _context.AssetTypes;
        }

        public AssetType CreateAssetType(AssetType assetType)
        {
            // validation
            if (assetType == null)
            {
                throw new AppException("AssetType not found");
            }
            _context.AssetTypes.Add(assetType);
            _context.SaveChanges();

            return assetType;
        }
    }
}
