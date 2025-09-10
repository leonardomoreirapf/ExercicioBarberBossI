using AutoMapper;
using BarberBossI.Application.AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace CommonTesteUtilities.Mapper;

public class MapperBuilder
{
	public static IMapper Build()
	{
		var mapper = new MapperConfiguration(configure => { configure.AddProfile(new AutoMapping()); });

		return mapper.CreateMapper();
	}
}
