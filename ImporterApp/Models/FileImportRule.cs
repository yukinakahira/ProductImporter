namespace ImporterApp.Models
{
    // ファイル取込ルール
    public class FileImportRule
    {
        //ユースジId
        public string UsageId { get; set; } = string.Empty;
        //グループ会社ID
        public string GpCompanyId { get; set; } = string.Empty;
        //処理モード
        public string ProcessMode { get; set; } = string.Empty;
        //ファイル名
        public string FileName { get; set; } = string.Empty;
        //文字コード
        public string CharacterCode { get; set; } = string.Empty;
        //区切り文字
        public string Delimiter { get; set; } = string.Empty;
        //ヘーダ行インデックス
        public int HeaderRowIndex { get; set; }
        //スキップ行
        public int SkipRowIndex { get; set; }
        //有効フラグ
        public bool IsActive { get; set; }
    }
}