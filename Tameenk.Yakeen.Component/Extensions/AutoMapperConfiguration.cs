using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Tameenk.Yakeen.DAL;
using Tameenk.Yakeen.Service.Models;

namespace Tameenk.Yakeen.Component.Extensions
{
    public static class AutoMapperConfiguration
    {
        public static void Init()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<DriverLicense, DriverLicenseYakeenInfoModel>()
                    .ForMember(a => a.TypeCode, b =>
                    {
                        b.MapFrom(d => d.TypeDesc);
                    })
                    .ForMember(a => a.ExpiryDateH, b =>
                    {
                        b.MapFrom(c => c.ExpiryDateH);
                    });

            });
            Mapper = MapperConfiguration.CreateMapper();
        }


        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// Mapper configuration
        /// </summary>
        public static MapperConfiguration MapperConfiguration { get; private set; }
    }
}
