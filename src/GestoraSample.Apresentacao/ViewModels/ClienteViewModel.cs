using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestoraSample.Apresentacao.ViewModels
{
    public class ClienteViewModel
    {
        public ClienteViewModel()
        {
            ClienteId = Guid.NewGuid();
            Produtos = new List<ProdutoViewModel>();
        }

        [Key]
        public Guid ClienteId { get; set; }

        [Required(ErrorMessage = "O Nome do Cliente é obrigatório.")]
        [MinLength(1, ErrorMessage = "Mínimo de {0} caracteres.")]
        [MaxLength(10, ErrorMessage = "Máximo de {0} caracteres.")]
        [Display(Name = "Nome do Cliente")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O Cpf é obrigatório.")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O Telefone é obrigatório.")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<ProdutoViewModel> Produtos { get; set; }
    }
}