using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestoraSample.Apresentacao.ViewModels
{
    public class ProdutoViewModel
    {
        public ProdutoViewModel()
        {
            ProdutoId = Guid.NewGuid();
        }

        [Key]
        public Guid ProdutoId { get; set; }

        [Required(ErrorMessage = "O Nome do Produto é obrigatório.")]
        [MinLength(1, ErrorMessage = "Mínimo de {0} caracteres.")]
        [MaxLength(10, ErrorMessage = "Máximo de {0} caracteres.")]
        [Display(Name = "Nome do Produto")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O Valor é obrigatório.")]
        [DataType(DataType.Currency, ErrorMessage = "Formato inválido.")]
        [Display(Name = "Preço do Produto")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Seleciona um Cliente")]
        [Display(Name = "Cliente")]
        public Guid ClienteId { get; set; }

        [ScaffoldColumn(false)]
        public ClienteViewModel Cliente { get; set; }
    }
}