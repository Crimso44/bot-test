using System;

namespace ChatBot.Admin.Worker.Code
{
    internal class GlobalServiceProvider
    {
        private static readonly object SyncRoot = new object();

        private static volatile IServiceProvider _instance;

        public static IServiceProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            var factory = new ServiceProviderFactory();
                            _instance = factory.Build();
                        }
                    }
                }

                return _instance;
            }
        }
    }
}
