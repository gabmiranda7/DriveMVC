namespace Drive.Models
{
    public class ArquivoModel
    {
        public int Id { get; set; }
        public string NomeArquivo { get; set; }
        public string Extensao { get; set; }
        public string TipoMime { get; set; }
        public long Tamanho { get; set; }
        public DateTime DataUpload { get; set; }
        public byte[] ArquivoBytes { get; set; }
    }
}
