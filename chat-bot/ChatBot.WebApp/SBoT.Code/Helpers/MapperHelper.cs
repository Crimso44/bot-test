using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace SBoT.Code.Helpers
{
    public static class MapperHelper
    {
        public static TDestination Map<TSource, TDestination>(
            this TDestination destination, TSource source)
        {
            if (source != null)
            {
                return Mapper.Map(source, destination);
            }
            return destination;
        }
    }
}
