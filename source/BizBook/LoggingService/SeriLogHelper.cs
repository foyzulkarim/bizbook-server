namespace LoggingService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Serilog;
    using Serilog.Sinks.Elasticsearch;

    public interface ISerilogConfigurator
    {
        LoggerConfiguration Configure(LoggerConfiguration configuration);
    }

    public class SerilogConfigRequest
    {
        public SerilogConfigRequest()
        {
            this.Configurators = new List<ISerilogConfigurator>();
        }

        public bool IsVerbose { get; set; } = true;
        public List<ISerilogConfigurator> Configurators { get; set; }
    }

    public class SerilogConfigParameters
    {
        public SerilogConfigParameters(Type type)
        {
            this.Type = type;
        }

        public SerilogConfigParameters(string systemName)
        {
            this.SystemName = systemName;
        }

        public string ElasticHosts { get; set; } = ConfigurationManager.AppSettings["ElasticHosts"];
        public string FolderPath { get; set; } = ConfigurationManager.AppSettings["FolderPath"];
        public bool AllowConsole { get; set; } = true;
        public Type Type { get; }
        public string SystemName { get; }
    }
    
    public class ElasticSinkOptions : ISerilogConfigurator
    {
        public bool AutoRegisterTemplate { get; set; } = true;
        public long BufferFileSizeLimitBytes { get; set; } = 10240;
        public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromSeconds(5);
        public TimeSpan? BufferLogShippingInterval { get; set; } = TimeSpan.FromSeconds(2);
        public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(2);
        public string IndexFormat { get; set; } = ConfigurationManager.AppSettings["ElasticSearchLogIndex"];
        public string Hosts { get; }
        public int BatchPostingLimit { get; set; } = 100;

        public ElasticSinkOptions(string hosts)
        {
            if (!string.IsNullOrWhiteSpace(hosts))
            {
                this.Hosts = hosts;
            }
            else
            {
                throw new ArgumentNullException(hosts, "At least one host entry is required.");
            }
        }

        public LoggerConfiguration Configure(LoggerConfiguration logger)
        {
            if (!string.IsNullOrWhiteSpace(this.Hosts))
            {
                IEnumerable<Uri> uris = this.Hosts.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => new Uri(x));
                ElasticsearchSinkOptions sinkOptions = new ElasticsearchSinkOptions(uris)
                {
                    AutoRegisterTemplate = this.AutoRegisterTemplate,
                    BufferFileSizeLimitBytes = this.BufferFileSizeLimitBytes,
                    BatchPostingLimit = this.BatchPostingLimit,
                    Period = this.Period,
                    BufferLogShippingInterval = this.BufferLogShippingInterval,
                    ConnectionTimeout = this.ConnectionTimeout,
                    IndexFormat = this.IndexFormat,
                };

                logger = logger.WriteTo.Elasticsearch(sinkOptions);

            }
            return logger;
        }
    }

    public class FileSinkOptions : ISerilogConfigurator
    {
        public string Path { get; }

        public FileSinkOptions(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                this.Path = path;
            }
            else
            {
                throw new ArgumentNullException(path, "At least one host entry is required.");
            }
        }

        public LoggerConfiguration Configure(LoggerConfiguration logger)
        {
            logger = logger.WriteTo.RollingFile(this.Path,fileSizeLimitBytes: 100000000);
            return logger;
        }
    }

    //public class ApplicationInsightOptions : ISerilogConfigurator
    //{
    //    public LoggerConfiguration Configure(LoggerConfiguration logger)
    //    {
    //        string key = ConfigurationManager.AppSettings["InstrumentationKey"];
    //        logger = logger.WriteTo.ApplicationInsightsEvents(key);
    //        return logger;
    //    }
    //}

    //public class AzureTableStorageOptions : ISerilogConfigurator
    //{
    //    public LoggerConfiguration Configure(LoggerConfiguration logger)
    //    {
    //        string connectionString=ConfigurationManager.ConnectionStrings["StorageAccount"].ConnectionString;
    //        string tableName = ConfigurationManager.AppSettings["TableName"];
    //        TimeSpan timeSpan = TimeSpan.FromSeconds(2);
    //        logger = logger.WriteTo.AzureTableStorageWithProperties(connectionString,storageTableName:tableName,period:timeSpan,batchPostingLimit:100);
    //        return logger;
    //    }
    //}

    public class SeriLogHelper
    {
        public static void ConfigureLogging(string systemName, SerilogConfigRequest request)
        {
            LoggerConfiguration logConfig = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("System", systemName);

            if (request.IsVerbose)
            {
                logConfig = logConfig.MinimumLevel.Verbose();
            }

            foreach (ISerilogConfigurator configurator in request.Configurators)
            {
                logConfig = configurator.Configure(logConfig);
            }

            Log.Logger = logConfig.CreateLogger();
        }

        public static void ConfigureLoggingDefaults(SerilogConfigParameters parameters)
        {
            var systemName = parameters.Type?.Name ?? parameters.SystemName;

            var serilogConfigRequest = new SerilogConfigRequest();
            serilogConfigRequest.Configurators.Add(new ElasticSinkOptions(parameters.ElasticHosts));
            serilogConfigRequest.Configurators.Add(new FileSinkOptions(parameters.FolderPath + $@"\{systemName}.log"));
            ConfigureLogging(systemName, serilogConfigRequest);
        }

        public static void ConfigureLoggingDefaults(Type type)
        {
            ConfigureLoggingDefaults(new SerilogConfigParameters(type));
        }

        public static void ConfigureLoggingDefaults(string systemName)
        {
            ConfigureLoggingDefaults(new SerilogConfigParameters(systemName));
        }
    }
}
