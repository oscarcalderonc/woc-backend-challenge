using woc_backend_challenge.Handlers;
using woc_backend_challenge.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

string brivJson = File.ReadAllText(Path.Combine(builder.Environment.ContentRootPath, "briv.json"));
builder.Services.AddSingleton(new CharacterRepository(brivJson));

var app = builder.Build();
app.UseCors();

// Routes
app.MapGet("/api/healthz", HealthzHandler.Get);
app.MapGet("/api/character/{characterId}", CharacterHandler.Get);
app.MapPost("/api/character/{characterId}/damage", HitPointsHandler.PostDamage);
app.MapPost("/api/character/{characterId}/heal", HitPointsHandler.PostHeal);
app.MapPost("/api/character/{characterId}/temp_hit_points", HitPointsHandler.PostTemporaryHeal);

app.Run();
