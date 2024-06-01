using Google.Protobuf;
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
                using var channel = GrpcChannel.ForAddress(Utils.GrcpServerData.BASE_ADDRESS);
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

        public async Task<string> SaveImagesAccommodation(string _id, ByteString[] imageBytes)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(Utils.GrcpServerData.BASE_ADDRESS);
                global::MultimediaService.MultimediaServiceClient stub = new(channel);

                using var call = stub.uploadAccommodationMultimedia();

                foreach (var item in imageBytes)
                {
                    await call.RequestStream.WriteAsync(new UploadMultimediaRequest
                    {
                        ModelId = _id,
                        Data = item
                    });
                }
                await call.RequestStream.CompleteAsync();
                var response = await call.ResponseAsync;

                return response.Description;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<string> UpdateMultimediaAccommodation(string _id, int multimediaIndex, ByteString multimediaBytes)
        {

            try
            {
                using var channel = GrpcChannel.ForAddress(Utils.GrcpServerData.BASE_ADDRESS);
                global::MultimediaService.MultimediaServiceClient stub = new(channel);

                using var call = stub.updateAccommodationMultimedia();

                await call.RequestStream.WriteAsync(new UpdatedMultimediaRequest
                {
                    ModelId = _id,
                    MultimediaId = multimediaIndex,
                    Data = multimediaBytes
                });

                await call.RequestStream.CompleteAsync();
                var response = await call.ResponseAsync;

                return response.Description;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public async Task<string> UploadProfilePhoto(string userId, ByteString[] imageBytes)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(Utils.GrcpServerData.BASE_ADDRESS);
                global::MultimediaService.MultimediaServiceClient stub = new(channel);

                using var call = stub.uploadProfilePhoto();

                foreach (var item in imageBytes)
                {
                    await call.RequestStream.WriteAsync(new UploadMultimediaRequest
                    {
                        ModelId = userId,
                        Data = item
                    });
                }
                await call.RequestStream.CompleteAsync();
                var response = await call.ResponseAsync;

                return response.Description;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }
    
    
    }
}
