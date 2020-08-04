using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CommandLine;

namespace Redminer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Arguments>(args)
                .WithParsedAsync(async arguments =>
                {
                    var entry = CreateMapper()
                        .Map<TimeEntry>(arguments);

                    if (!arguments.To.HasValue)
                    {
                        await LogTime(entry.IssueId, entry.Hours, entry.SpendOn, entry.ActivityId,
                            entry.Comments, arguments.ApiKey);
                    }
                    else
                    {
                        for (var i = 0; i < (arguments.To - entry.SpendOn).Value.Days + 1; i++)
                        {
                            await LogTime(entry.IssueId, entry.Hours, entry.SpendOn.AddDays(i), entry.ActivityId,
                                entry.Comments, arguments.ApiKey);
                        }
                    }
                });
        }

        private static IMapper CreateMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Arguments, TimeEntry>()
                    .ForMember(x => x.SpendOn, opt => opt.MapFrom(x => x.From));
            }).CreateMapper();
            return mapper;
        }

        private static async Task LogTime(int issueId, double hours, DateTime? spendTimeDate,
            ActivityType activityId, string comments, string apiKey)
        {
            var client = CreateHttpClient(apiKey);
            var resp = await client.PostAsync("time_entries.json",
                CreateTimeEntryBody(issueId, hours, spendTimeDate, activityId, comments));
            if (resp.StatusCode == HttpStatusCode.Created)
            {
                Console.WriteLine($"Списанно {hours} часов за {spendTimeDate:yyyy-MM-dd} на задачу {issueId}.");
            }
            else
            {
                Console.WriteLine($"Не удалось списать часы за {spendTimeDate:yyyy-MM-dd} на задачу {issueId}.");
            }
        }

        private static StringContent CreateTimeEntryBody(int issueId, double hours, DateTime? spendTimeDate,
            ActivityType activityId, string comments)
        {
            var data = new SpendTimeData
            {
                TimeEntry = new TimeEntry
                {
                    IssueId = issueId, Hours = hours, ActivityId = activityId,
                    Comments = comments,
                    SpendOn = spendTimeDate ?? DateTime.Today
                }
            };

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions()
            {
                Converters = {new DateTimeConverter()}
            });

            return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        private static HttpClient CreateHttpClient(string apiKey)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://rm.bimteam.ru"),
            };
            httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", apiKey);

            return httpClient;
        }
    }
}