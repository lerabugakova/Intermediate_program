using AsyncAwait.Task2.CodeReviewChallenge.Headers;
using CloudServices.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace AsyncAwait.Task2.CodeReviewChallenge.Middleware;

public class StatisticMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IStatisticService _statisticService;

    public StatisticMiddleware(RequestDelegate next, IStatisticService statisticService)
    {
        _next = next;
        _statisticService = statisticService ?? throw new ArgumentNullException(nameof(statisticService));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path;
        var token = context.RequestAborted;

        await _statisticService.RegisterVisitAsync(path, token);

        var count = await _statisticService.GetVisitsCountAsync(path, token);
        context.Response.Headers.Add(CustomHttpHeaders.TotalPageVisits, count.ToString());

        await Task.Delay(3000, token);
        await _next(context);
    }
}
