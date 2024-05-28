using Grpc;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HostedInDesktop.Data.Services
{
    public class MultimediaServiceImpl
    {

        public MultimediaServiceImpl() 
        { 
        }

        public async Task<byte[]> LoadMainImageAccommodation(string _id, int index)
        {

            try
            {
                using var channel = GrpcChannel.ForAddress("http://127.0.0.1:3002");
                global::MultimediaService.MultimediaServiceClient stub = new(channel);
                DownloadMultimediaRequest downloadMultimediaRequest = new()
                {
                    ModelId = _id,
                    MultimediaIndex = index
                };
                using var call = stub.downloadAccommodationMultimedia(downloadMultimediaRequest);

                var writeStream = new MemoryStream();
                await foreach (var message in call.ResponseStream.ReadAllAsync())
                {
                    if (message.Data != null)
                    {
                        var bytes = message.Data.Memory;
                        await writeStream.WriteAsync(bytes);
                    }
                }
                return await Task.FromResult(writeStream.ToArray());
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
