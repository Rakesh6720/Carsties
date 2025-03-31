using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        private readonly IMapper _mapper;

        public AuctionUpdatedConsumer(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            Console.WriteLine("---> Consuming auction updated: " + context.Message.Id);

            var item = _mapper.Map<Item>(context.Message);

            var resut = await DB.Update<Item>()
                .Match(x => x.ID == context.Message.Id)
                .ModifyOnly(x => new 
                {
                    x.Color,
                    x.Make,
                    x.Model,
                    x.Year,
                    x.Mileage
                }, item)
                .ExecuteAsync();
                                
            if (!resut.IsAcknowledged)
            {
                Console.WriteLine("---> Problem updating mongodb: " + context.Message.Id);
                throw new MessageException(typeof(AuctionUpdated), "Problem updating mongodb");
            }
            else
            {
                Console.WriteLine("---> Auction updated successfully: " + context.Message.Id);
            }
        }
    }
}