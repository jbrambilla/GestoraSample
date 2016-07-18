using System;
using System.Collections.Generic;

namespace GestoraSample.Dominio.Entidades
{
    public class Cliente
    {
        public Guid ClienteId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }

        public ICollection<Produto> Produtos { get; set; }
    }
}
