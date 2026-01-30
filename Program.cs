using DotNetAiChat.Memory;
using DotNetAiChat.Rag;
using DotNetAiChat.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IAiChatService, OpenAiChatService>();
builder.Services.AddSingleton<IConversationStore, InMemoryConversationStore>();
builder.Services.AddSingleton<ContextWindowManager>();
builder.Services.AddScoped<IAiDecisionService, OpenAiDecisionService>();
builder.Services.AddSingleton<DocumentIngestionService>();
builder.Services.AddSingleton<TextChunker>();
builder.Services.AddSingleton<DocumentTextExtractor>();
builder.Services.AddSingleton<EmbeddingService>();
builder.Services.AddSingleton<InMemoryVectorStore>();
builder.Services.AddSingleton<RagAnswerService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();

