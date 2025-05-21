using System;
using FlightManager.Business.Interfaces;
using FlightManager.Data;
using FlightManager.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FlightManager.Business
{
	public static class DIServiceExtension
	{
		public static IServiceCollection AddDIServices(this IServiceCollection services)
		{
			services.AddScoped<IFlightService, FlightService>();
			services.AddScoped<IFlightRepository, FlightRepository>();

			return services;
		}
	}
}

