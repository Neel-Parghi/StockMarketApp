using Services;
using StockMarket;
using ServiceContracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddSingleton<IStocksService, StocksService>();
builder.Services.AddSingleton<IFinnhubService, FinnhubService>();
builder.Services.AddHttpClient();

var app = builder.Build();
app.UseStaticFiles();
app.MapControllers();
app.UseRouting();

app.Run();
