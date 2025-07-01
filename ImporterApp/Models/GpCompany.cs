namespace ImporterApp.Models
{
    // This model represents the 'GP会社マスタ' (GP Company Master) table [cite: 1]
    public class GpCompany
    {
        // GP会社ID (GP Company ID) [cite: 1]
        public string GpCompanyId { get; set; } = string.Empty;

        // GP会社名 (GP Company Name) [cite: 1]
        public string GpCompanyName { get; set; } = string.Empty;

        // 表示順 (Display Order) [cite: 1]
        public int DisplayOrder { get; set; }

        // 備考 (Remarks) [cite: 1]
        public string Remarks { get; set; } = string.Empty;
    }
}