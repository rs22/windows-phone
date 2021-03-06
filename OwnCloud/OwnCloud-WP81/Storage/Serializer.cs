﻿
using OwnCloud.Common.Storage;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace OwnCloud.Storage {
    public class Serializer : ISerializer {
        public async Task<T> Open<T>(string filePath) where T : class, new() {
            return await Open<T>(filePath, false);
        }

        public async Task<T> Open<T>(string filePath, bool useBinary) where T : class, new() {
            var loadedObject = default(T);

            using (var inputStream = await PlatformFileAccess.GetOpenFileSequentialStream(filePath)) {
                using (var stream = inputStream.AsStreamForRead()) {
                    using (var reader = (useBinary ? XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max) : XmlReader.Create(stream))) {
                        if (stream.Length > 0) {
                            var serializer = new DataContractSerializer(typeof(T));

                            loadedObject = (T)serializer.ReadObject(reader);
                        }
                    }
                }
            }

            return loadedObject ?? new T();
        }

        public async Task Save<T>(string filePath, T objectToSave) {
            await Save(filePath, objectToSave, false);
        }

        public async Task Save<T>(string filePath, T objectToSave, bool useBinary) {
            using (var outputStream = await PlatformFileAccess.GetSaveFileStream(filePath)) {
                using (var stream = outputStream.AsStreamForWrite()) {
                    using (var writer = (useBinary ? XmlDictionaryWriter.CreateBinaryWriter(stream) : XmlWriter.Create(stream))) {
                        var serializer = new DataContractSerializer(
                            typeof(T),
                            new DataContractSerializerSettings {
                                PreserveObjectReferences = true
                            });

                        serializer.WriteObject(writer, objectToSave);

                        await stream.FlushAsync();
                    }
                }
            }
        }
    }
}