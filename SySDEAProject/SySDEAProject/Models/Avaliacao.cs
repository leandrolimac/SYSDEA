namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Avaliacao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Avaliacao()
        {
            AudioProva = new HashSet<AudioProva>();
            Fiscalizacao = new HashSet<Fiscalizacao>();
            NotaAvaliador = new HashSet<NotaAvaliador>();
            Recurso = new HashSet<Recurso>();
            VideoProva = new HashSet<VideoProva>();
        }

        public Avaliacao(Horario horario)
        {
            this.Piloto = horario.Piloto;
            this.Horario = horario;
            this.LocalEntidade = horario.LocalEntidade;
            this.status = 1;
            this.idPiloto = horario.Piloto.Id;

            AudioProva = new HashSet<AudioProva>();
            Fiscalizacao = new HashSet<Fiscalizacao>();
            NotaAvaliador = new HashSet<NotaAvaliador>();
            Recurso = new HashSet<Recurso>();
            VideoProva = new HashSet<VideoProva>();
        }
        public int idAvaliacao { get; set; }

        public int idLocalEntidade { get; set; }

        public int idPiloto { get; set; }

        public int idVersaoProva { get; set; }

        public int? idNotaFinalEntidade { get; set; }

        public int? idNotaFinal { get; set; }

        public int numeroNotaFinalEntidade { get; set; }
        public int notaFinal { get; set; }
        public DateTime? dthrProvaInicio { get; set; }

        public DateTime? dthrProvaFim { get; set; }

        public DateTime? dthrEnvioDados { get; set; }
        public bool? possuiRecurso { get; set; }

        public bool? sucesso { get; set; }

        public int? status { get; set; }
        /*
         1 - PRÉ-AVALIAÇÃO
         2 - GRAVAÇÃO INICIADA         
         3 - GRAVAÇÃO ENCERRADA, AGUARDANDO AVALIAÇÃO
         4 - PROCESSO AVALIADO POR UM EXAMINADOR
         5 - PROCESSO AVALIADO POR DOIS EXAMINADORES, PRONTO PARA ENVIO
         6 - PROCESSO NA CARGA AGUARDANDO AVALIAÇÃO
         7 - PROCESSO EM FISCALIZAÇÃO ADMINISTRATIVA
         8 - PROCESSO NA CARGA, FISCALIZAÇÃO ADMINISTRATIVA PRONTA, AGUARDANDO ANÁLISE DE ENTREVISTA
         9 - PROCESSO NA CARGA, FISCALIZAÇÃO ADMINISTRATIVA PRONTA, AGUARDANDO ANÁLISE DE PARECER
         10- PRONTO PARA DIVULGAÇÃO DE RESULTADO
         11- CONCLUÍDO
         12- RECURSO PENDENTE
         13- RECURSO EM PROGRESSO
         14- RECURSO CONCLUÍDO
         15- PRONTO PARA DIVULGAÇÃO DO RESULTADO DE RECURSO
         16- FALHA NA AVALIAÇÃO
         

        */

        public DateTime? dthrDivulgaRes { get; set; }

        public DateTime? dthrEncerra { get; set; }

        public string providenciasExtras { get; set; }

        public long cdProcesso { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AudioProva> AudioProva { get; set; }

        public virtual Horario Horario { get; set; }

        public virtual VersaoProva VersaoProva { get; set; }

        public virtual Piloto Piloto {get;set;}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual LocalEntidade LocalEntidade { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fiscalizacao> Fiscalizacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotaAvaliador> NotaAvaliador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recurso> Recurso { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VideoProva> VideoProva { get; set; }

        public virtual Nota NotaFinalEntidade { get; set; }

        public virtual Nota NotaFinal { get; set; }

    }
}
