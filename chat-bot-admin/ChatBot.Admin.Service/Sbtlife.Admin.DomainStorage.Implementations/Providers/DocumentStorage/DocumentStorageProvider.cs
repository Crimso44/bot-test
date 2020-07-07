using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChatBot.Admin.DomainStorage.Const;
using ChatBot.Admin.DomainStorage.Contexts;
using ChatBot.Admin.DomainStorage.Contexts.Entities.DocumentStorage;
using ChatBot.Admin.DomainStorage.Model.DocumentStorage;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.DocumentStorage;

namespace ChatBot.Admin.DomainStorage.Providers.DocumentStorage
{
    public class DocumentStorageProvider : IDocumentStorageProvider
    {
        private readonly DocumentStorageContext _ctx;

        public DocumentStorageProvider(DocumentStorageContext ctx)
        {
            _ctx = ctx;
        }


        public  Guid StoreFile(Guid userId, Guid catalogId, string fileName, byte[] body)
        {
            var id = Guid.NewGuid();
            var ext = Path.GetExtension(fileName);
            if (!string.IsNullOrEmpty(ext) && ext[0] == '.') ext = ext.Substring(1);
            fileName = fileName.Substring(0, fileName.Length - ext.Length - 1);
            if (string.IsNullOrEmpty(ext)) ext = "dat";
            var entity = new Files
            {
                Id = id,
                CatalogId = catalogId,
                Data = body,
                CreateDate = DateTime.Now,
                CreateEmployeeId = userId,
                Name = fileName,
                Extension = ext
            };
             _ctx.Files.Add(entity);
             _ctx.SaveChanges();
            return id;
        }

        public  FileDto GetFile(Guid fileId)
        {
            return  _ctx.Files.Where(x => x.Id == fileId).Select(x => new FileDto { Body = x.Data, Name = x.Name + "." + x.Extension }).Single();
        }

        public  void DeleteFile(Guid fileId)
        {
            var entity =  _ctx.Files.FirstOrDefault(x => x.Id == fileId);
            if (entity != null)
            {
                _ctx.Files.Remove(entity);
                 _ctx.SaveChanges();
            }
        }

        public  void CheckSbtLifeCatalog()
        {
            var cat =  _ctx.FilesCatalog.SingleOrDefault(x => x.Id == CommonConst.DocumentStorage.CatalogId);
            if (cat == null)
            {
                var topCat =  _ctx.FilesCatalog.SingleOrDefault(x => x.Id == CommonConst.DocumentStorage.TopCatalogId);
                if (topCat == null)
                {
                    topCat = new FilesCatalog()
                    {
                        Id = CommonConst.DocumentStorage.TopCatalogId,
                        IsArchive = false,
                        IsHidden = true,
                        Name = CommonConst.DocumentStorage.TopCatalogName
                    };
                     _ctx.FilesCatalog.Add(topCat);
                }
                cat = new FilesCatalog()
                {
                    Id = CommonConst.DocumentStorage.CatalogId,
                    IsArchive = false,
                    IsHidden = true,
                    Name = CommonConst.DocumentStorage.CatalogName,
                    ParentId = CommonConst.DocumentStorage.TopCatalogId
                };
                 _ctx.FilesCatalog.Add(cat);
                 _ctx.SaveChanges();
            }
        }
    }
}
