﻿using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace OwnCloud.Common.Storage {
    public interface ISerializer {
        Task<T> Open<T>(string filePath) where T : class, new();
        Task<T> Open<T>(string filePath, bool useBinary) where T : class, new();
        Task Save<T>(string filePath, T objectToSave);
        Task Save<T>(string filePath, T objectToSave, bool useBinary);
    }
}