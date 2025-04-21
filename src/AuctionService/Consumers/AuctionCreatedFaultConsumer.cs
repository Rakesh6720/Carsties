using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
    {
        public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
        {
            Console.WriteLine($"Auction creation failed: {context.Message.Exceptions.First().Message}");
            var exception = context.Message.Exceptions.First();

            if (exception.ExceptionType == "System.ArgumentException")
            {
                Console.WriteLine($"Argument error: {exception.Message}");
                context.Message.Message.Model = "FooBar";
                await context.Publish(context.Message.Message);
            }
            else {
                Console.WriteLine($"Not an argument exception -- update error dashboard somewhere: {exception.Message}");
            }
        }
    }
}