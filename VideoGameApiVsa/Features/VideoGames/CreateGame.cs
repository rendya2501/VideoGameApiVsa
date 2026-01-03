using FluentValidation;
using MediatR;
using VideoGameApiVsa.Data;
using VideoGameApiVsa.Entities;

namespace VideoGameApiVsa.Features.VideoGames;

/// <summary>
/// 「ゲーム作成」機能の垂直スライス
/// </summary>
/// <remarks>
/// - Request / Command / Validator / Handler / Endpoint を1ファイルに集約
/// - このファイルだけ見れば機能の全体像が分かる
/// </remarks>
public static class CreateGame
{
    /// <summary>
    /// リクエスト用DTO（OpenAPI/Scalarで表示される）
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Genre"></param>
    /// <param name="ReleaseYear"></param>
    /// <remarks>
    /// - Minimal API / Carter のエンドポイントで直接受け取る
    /// - OpenAPI / Scalar / Swagger に表示されるモデル
    /// </remarks>
    public record CreateGameRequest(string Title, string Genre, int ReleaseYear);

    /// <summary>
    /// MediatRコマンド（内部使用のみ）
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Genre"></param>
    /// <param name="ReleaseYear"></param>
    /// <remarks>
    /// - アプリケーション内部でのみ使用される
    /// - ValidationBehavior により FluentValidation が自動適用される
    /// - IRequest<T> を実装することで Handler と1対1で結びつく
    /// </remarks>
    public record CreateGameCommand(string Title, string Genre, int ReleaseYear) : IRequest<CreateGameResponse>;

    /// <summary>
    /// コマンド処理結果のレスポンス
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Title"></param>
    /// <param name="Genre"></param>
    /// <param name="ReleaseYear"></param>
    /// <remarks>
    /// - Handler から返され、Endpoint がそのまま HTTP レスポンスに変換
    /// - Entity を直接返さず、API契約専用の型を使う
    /// </remarks>
    public record CreateGameResponse(int Id, string Title, string Genre, int ReleaseYear);

    /// <summary>
    /// FluentValidation によるコマンド検証
    /// </summary>
    /// <remarks>
    /// - MediatR Pipeline（ValidationBehavior）経由で自動実行される
    /// - 失敗時は ValidationException が投げられ、
    ///   ExceptionHandler → ProblemDetails に変換される
    /// </remarks>
    public class Validator : AbstractValidator<CreateGameCommand>
    {
        public Validator()
        {
            // タイトルは必須 & 最大100文字
            RuleFor(x => x.Title)
                .NotEmpty()// .WithMessage("Title is required.")
                .MaximumLength(100);// .WithMessage("Length is Max100.");

            // ジャンルは必須 & 最大50文字
            RuleFor(x => x.Genre)
                .NotEmpty()
                .MaximumLength(50);

            // リリース年は現実的な範囲に制限
            RuleFor(x => x.ReleaseYear)
                .InclusiveBetween(1950, DateTime.Now.Year);
        }
    }

    /// <summary>
    /// コマンドを実際に処理する Handler
    /// </summary>
    /// <param name="dbContext"></param>
    public class Handler(VideoGameDbContext dbContext) : IRequestHandler<CreateGameCommand, CreateGameResponse>
    {
        public async Task<CreateGameResponse> Handle(CreateGameCommand command, CancellationToken ct)
        {
            // Command → Entity への変換
            var videoGame = new VideoGame
            {
                Title = command.Title,
                Genre = command.Genre,
                ReleaseYear = command.ReleaseYear
            };

            // EF Core による永続化
            dbContext.VideoGames.Add(videoGame);
            await dbContext.SaveChangesAsync(ct);

            // Entity → Response への変換
            return new CreateGameResponse(
                videoGame.Id,
                videoGame.Title,
                videoGame.Genre,
                videoGame.ReleaseYear
            );
        }
    }

    /// <summary>
    /// HTTP エンドポイント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <remarks>
    /// - Minimal API / Carter から呼ばれる
    /// - リクエストを Command に変換して MediatR に委譲するだけ
    /// </remarks>
    public static async Task<IResult> Endpoint(ISender sender, CreateGameRequest request, CancellationToken ct)
    {
        // 外部入力 DTO → 内部 Command へ変換
        var command = new CreateGameCommand(
            request.Title,
            request.Genre,
            request.ReleaseYear
        );

        // MediatR 経由で処理を実行
        var result = await sender.Send(command, ct);

        // 201 Created + Location ヘッダ付きレスポンス
        return Results.CreatedAtRoute(
            routeName: VideoGameRouteNames.GetById,
            routeValues: new { id = result.Id },
            value: result);
    }
}
