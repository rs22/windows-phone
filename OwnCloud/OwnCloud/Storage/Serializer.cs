
﻿using OwnCloud.Common.Storage;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace OwnCloud.Storage {
    public class Serializer : ISerializer {
        public Task<T> Open<T>(string filePath) where T : class, new() {
            return Open<T>(filePath, false);
        }

        public Task<T> Open<T>(string filePath, bool useBinary) where T : class, new() {
            var loadedObject = default(T);

            using (var stream = PlatformFileAccess.GetOpenFileStream(filePath)) {
                using (var reader = (useBinary ? XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max) : XmlReader.Create(stream))) {
                    if (stream.Length > 0) {
                        var serializer = new DataContractSerializer(typeof(T));

                        loadedObject = (T)serializer.ReadObject(reader);
                    }
                }
            }

            return TaskEx.FromResult(loadedObject ?? new T());
        }

        public Task Save<T>(string filePath, T objectToSave) {
            return Save(filePath, objectToSave, false);
        }

        public Task Save<T>(string filePath, T objectToSave, bool useBinary) {
            using (var stream = PlatformFileAccess.GetSaveFileStream(filePath)) {
                using (var writer = (useBinary ? XmlDictionaryWriter.CreateBinaryWriter(stream) : XmlWriter.Create(stream))) {
                    var serializer = new DataContractSerializer(typeof(T));

                    serializer.WriteObject(writer, objectToSave);

                    writer.Flush();
                }
            }
            return TaskEx.FromResult(0);
        }
    }
}