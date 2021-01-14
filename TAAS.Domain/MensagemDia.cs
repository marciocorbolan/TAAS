using System;

namespace TAAS.Domain
{
    public class MensagemDia
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public bool Exibida { get; set; }
        public DateTime DtCadastro { get; set; }
        public DateTime DtModificado { get; set; }
    }
}
