using System;
using System.Threading.Tasks;
using ChatBot.Admin.DomainStorage.Model.DocumentStorage;

namespace ChatBot.Admin.DomainStorage.Providers.Abstractions.DocumentStorage
{
    public interface IDocumentStorageProvider
    {
        Guid StoreFile(Guid userId, Guid catalogId, string fileName, byte[] body);

        FileDto GetFile(Guid fileId);

        void DeleteFile(Guid fileId);

        void CheckSbtLifeCatalog();
    }
}
