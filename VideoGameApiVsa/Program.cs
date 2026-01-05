using Carter;
using Scalar.AspNetCore;
using Serilog;
using VideoGameApiVsa.Extensions;

// ===================================================================
// Serilog の初期設定（ブートストラップロガー）
// ===================================================================
// この時点では appsettings.json がまだ読み込まれていないため、
// 最低限のロガーを作成（アプリケーション起動時のエラーもキャッチ可能）
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting VideoGameApiVsa application...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // 拡張メソッドでSerilog設定
    builder.Host.ConfigureSerilog();

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    // 拡張メソッドでサービス登録を整理
    builder.Services.AddDatabaseServices(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddValidationServices();

    var app = builder.Build();

    // ミドルウェアパイプライン（順序が重要）
    app.UseSerilogRequestLogging();  // HTTPリクエストロギング
    app.UseGlobalExceptionHandler();    // グローバル例外ハンドリング

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    // エンドポイント登録
    app.MapCarter();


    // シードデータ（開発環境のみ）
    await app.SeedDatabaseAsync();

    Log.Information("Application started successfully");
    app.Run();
}
catch (Exception ex)
{
    // アプリケーション起動時の致命的エラー
    // appsettings.json 読み込み失敗などもここでキャッチ
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    // アプリケーション終了時に確実にログをフラッシュ
    // バッファに残っているログをすべて書き込んでからプロセスを終了
    Log.CloseAndFlush();
}
