using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService_controllers.DTOs;
using AuctionService_controllers.Entities;
using AutoMapper;

namespace AuctionService_controllers.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Auction, AuctionDto>()
                .IncludeMembers(x => x.Item);
            CreateMap<Item, AuctionDto>();
            CreateMap<CreateAuctionDto, Auction>()
                .ForMember(d => d.Item, o => o.MapFrom(s => s));
            CreateMap<CreateAuctionDto, Item>();
        }
    }
}