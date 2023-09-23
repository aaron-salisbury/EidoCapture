using System;

namespace EidoCapture.Data.Domains
{
    [Serializable]
    internal class InternalStorage
    {
        public string UserStorageDirectory { get; set; } = null!;
    }
}
