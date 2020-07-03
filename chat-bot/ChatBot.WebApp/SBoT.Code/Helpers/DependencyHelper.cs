using SBoT.Code.Repository.Interfaces;
using SBoT.Code.Repository;
using System;
using System.Collections.Generic;
using SBoT.Code.Entity.Interfaces;
using SBoT.Code.Entity;
using SBoT.Domain.DataModel.SBoT.Interfaces;
using SBoT.Domain.DataModel.SBoT;

namespace SBoT.Code.Helpers
{
    public static class DependencyHelper
    {
        public static Dictionary<Type, Type> GetDependencies()
        {
            return new Dictionary<Type, Type>
            {
                {typeof (IWordFormer), typeof (WordFormer)},
                {typeof (IChatter), typeof (Chatter)},
                {typeof (IFileTransformer), typeof (FileTransformer)},
                {typeof (IRoleChecker), typeof (RoleChecker)},

                {typeof (ISboTRepository), typeof (SBoTRepository)},
                {typeof (ISBoTDataModel), typeof (SBoTDataModel)},
            };
        }

    }
}
