using BarberBossI.Application.AutoMapper;
using BarberBossI.Application.UseCases.Faturamento.Delete;
using BarberBossI.Application.UseCases.Faturamento.GetAll;
using BarberBossI.Application.UseCases.Faturamento.GetById;
using BarberBossI.Application.UseCases.Faturamento.Register;
using BarberBossI.Application.UseCases.Faturamento.Reports.Excel;
using BarberBossI.Application.UseCases.Faturamento.Reports.Pdf;
using BarberBossI.Application.UseCases.Faturamento.Update;
using BarberBossI.Application.UseCases.Login;
using BarberBossI.Application.UseCases.User.Register;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBossI.Application;

public static class DependencyInjectionExtension
{
	public static void AddApplication(this IServiceCollection services)
	{
		AddAutoMapper(services);
		AddUseCases(services);
	}

	private static void AddAutoMapper(IServiceCollection services)
	{
		services.AddAutoMapper(typeof(AutoMapping));
	}

	private static void AddUseCases(IServiceCollection services)
	{
		services.AddScoped<IRegisterFaturamentoUseCase, RegisterFaturamentoUseCase>();
		services.AddScoped<IGetAllFaturamentosUseCase, GetAllFaturamentosUseCase>();
		services.AddScoped<IGetFaturamentoByIdUseCase, GetFaturamentoByIdUseCase>();
		services.AddScoped<IDeleteFaturamentoUseCase, DeleteFaturamentoUseCase>();
		services.AddScoped<IUpdateFaturamentoUseCase, UpdateFaturamentoUseCase>();
		services.AddScoped<IGenerateFaturamentoReportExcelUseCase, GenerateFaturamentoReportExcelUseCase>();
		services.AddScoped<IGenerateFaturamentoReportPdfUseCase, GenerateFaturamentoReportPdfUseCase>();
		services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
		services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
	}
}
