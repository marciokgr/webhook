using Microsoft.AspNetCore.HttpLogging;

const string server = "http://localhost:5003";
const string callback = "http://localhost:5004/webhook/item/new";
const string topic = "item.new";

var client = new HttpClient();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

//Criando um log de acesso a API
builder.Services.AddW3CLogging(logging =>
{
    logging.LoggingFields = W3CLoggingFields.All;
    logging.FlushInterval = TimeSpan.FromSeconds(2);
});

await client.PostAsJsonAsync(server + "/subscribe", new { topic, callback });

Console.WriteLine($"subscrito no tópico {topic} com o callback {callback}");

var app = builder.Build();
app.UseW3CLogging(); //ativando log
app.MapPost("/webhook/item/new", (object payload, ILogger<Program> logger) =>
{
    logger.LogInformation("recebido payload: {payload}", payload);
});

app.Run();