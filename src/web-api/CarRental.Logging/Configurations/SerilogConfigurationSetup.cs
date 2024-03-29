﻿using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;

namespace CarRental.Logging.Configurations
{
    public static class SerilogConfigurationSetup
    {
        public static void WithCarRentalConfiguration(this LoggerConfiguration loggerConfig,
            IServiceProvider serviceProvider, IConfiguration config)
        {
            var instrumentationKey = config["ApplicationInsights:InstrumentationKey"];

            var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name!;

            loggerConfig
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Assembly", assemblyName)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate)
                .WriteTo.Logger(lc => lc.Filter.ByExcluding(Matching.WithProperty<bool>("Security", p => p)))
                .WriteTo.ApplicationInsights(
                    new TelemetryConfiguration { InstrumentationKey = instrumentationKey },
                    new CustomApplicationInsightsTelemetryConverter());

        }

        private class CustomApplicationInsightsTelemetryConverter : TraceTelemetryConverter
        {
            public override IEnumerable<ITelemetry> Convert(LogEvent logEvent, IFormatProvider formatProvider)
            {
                foreach (ITelemetry telemetry in base.Convert(logEvent, formatProvider))
                {
                    if (logEvent.Properties.ContainsKey("ErrorId"))
                    {
                        telemetry.Context.Operation.Id = logEvent.Properties["ErrorId"].ToString();
                    }

                    ISupportProperties propTelemetry = (ISupportProperties)telemetry;

                    var removeProps = new[] { "MessageTemplate", "ErrorId", "ErrorMessage" };

                    removeProps = removeProps.Where(prop => propTelemetry.Properties.ContainsKey(prop)).ToArray();

                    foreach (var prop in removeProps)
                    {
                        propTelemetry.Properties.Remove(prop);
                    }

                    yield return telemetry;
                }
            }
        }
    }
}
