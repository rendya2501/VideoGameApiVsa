namespace VideoGameApiVsa.Entities;

/// <summary>
/// VideoGameに関連する定数定義
/// </summary>
/// <remarks>
/// ビジネスルールに関する定数を一元管理し、
/// マジックナンバーの発生を防ぐ。
/// </remarks>
public static class VideoGameConstants
{
    /// <summary>
    /// リリース年の最小値（1950年）
    /// </summary>
    /// <remarks>
    /// 1950年は商業的なビデオゲームが登場した時期の近似値。
    /// - 1952年: OXO (Noughts and Crosses) - 最初期のビデオゲーム
    /// - 1958年: Tennis for Two
    /// - 1962年: Spacewar!
    /// 
    /// 1950年以前のゲームは学術的・実験的なものが多く、
    /// 商業データベースでの管理対象外とする。
    /// </remarks>
    public const int MinReleaseYear = 1950;

    /// <summary>
    /// タイトルの最大文字数
    /// </summary>
    public const int TitleMaxLength = 100;

    /// <summary>
    /// ジャンルの最大文字数
    /// </summary>
    public const int GenreMaxLength = 50;

    /// <summary>
    /// デフォルトのリリース年
    /// </summary>
    /// <remarks>
    /// リリース年が未指定の場合のデフォルト値。
    /// 最小値と同じ1950年を使用。
    /// </remarks>
    public const int DefaultReleaseYear = MinReleaseYear;
}
