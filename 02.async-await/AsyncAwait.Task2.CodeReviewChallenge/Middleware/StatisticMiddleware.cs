﻿using System;
using System.Threading.Tasks;
using AsyncAwait.Task2.CodeReviewChallenge.Headers;
using CloudServices.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AsyncAwait.Task2.CodeReviewChallenge.Middleware
{
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
            string path = context.Request.Path;
            await _statisticService.RegisterVisitAsync(path);

            await SetVisitsCountHeader(context, path);

            await _next(context);
        }

        private async Task SetVisitsCountHeader(HttpContext context, string path)
        {
            var visitsCount = await _statisticService.GetVisitsCountAsync(path);

            context.Response.Headers.Add(CustomHttpHeaders.TotalPageVisits, visitsCount.ToString());
        }
    }
}
