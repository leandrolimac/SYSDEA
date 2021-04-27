using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SySDEAProject.Models
{
    public partial class AvaliadorXEntidade
    {

        public AvaliadorXEntidade()
        {
            
        }
        public AvaliadorXEntidade(int idEntidade, int idAvaliador, DateTime dataAdmissao)
        {
            this.idEntidade = idEntidade;
            this.idAvaliador = idAvaliador;
            this.dataAdmissao = dataAdmissao;
            
        }


        public int idAvaliador { get; set; }


        public int idEntidade { get; set; }


        public int idAvaliadorXEntidade { get; set; }


        public Nullable<DateTime> dataAdmissao { get; set; }


        public Nullable<DateTime> dataEncerramento { get; set; }


        public virtual Avaliador Avaliador { get; set; }

        public virtual Entidade Entidade { get; set; }


    }
}