using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAAS.Domain
{
    [Table("mensagem_dia")]
    [Display(Name = "Mensagem do Dia")]
    public class MensagemDia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        
        [Column("texto", TypeName = "varchar(256)")]
        [Display(Name = "Texto")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [MaxLength(256, ErrorMessage = "O campo {0} deve conter no máximo 256 caracteres!")]
        [MinLength(10, ErrorMessage = "O campo {0} deve conter no mínimo 10 caracteres!")]
        public string Texto { get; set; }
        
        [Column("exibida")]
        [Display(Name = "Exibida")]
        public bool Exibida { get; set; }
        
        [Column("dt_cadastro")]
        [Display(Name = "Data de cadastro")]
        public DateTime DtCadastro { get; set; }
        
        [Column("dt_modificado")]
        [Display(Name = "Data de modificação")]
        public DateTime DtModificado { get; set; }
    }
}
