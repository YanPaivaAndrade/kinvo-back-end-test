﻿using Aliquota.Domain.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliquota.Domain.Entities.Entidades
{
    public class Cliente : IEntidade<int>
    {
        public int Id { get; set; }
        public string Descricao { get => @"Entidade responsável por manter os dados referentes ao cliente"; }
        public string NomeCliente { get; set; }
        public ICollection<Produto> ProdutoLista { get; set; }
    }
}
