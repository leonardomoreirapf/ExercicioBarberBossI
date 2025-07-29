using AutoMapper;
using BarberBossI.Communication.Requests;
using BarberBossI.Communication.Responses;
using BarberBossI.Domain.Entities;

namespace BarberBossI.Application.AutoMapper;

public class AutoMapping : Profile
{
	public AutoMapping()
	{
		RequestToEntity();
		EntityToResponse();
	}

	private void RequestToEntity()
	{
		CreateMap<RequestFaturamentoJson, Faturamento>();
		CreateMap<RequestRegisterUserJson, User>()
			.ForMember(dest => dest.Password, config => config.Ignore());
	}
	private void EntityToResponse()
	{
		CreateMap<Faturamento, ResponseRegisteredFaturamentoJson>();
		CreateMap<Faturamento, ResponseShortFaturamentoJson>();
		CreateMap<Faturamento, ResponseFaturamentoJson>();
	}
}