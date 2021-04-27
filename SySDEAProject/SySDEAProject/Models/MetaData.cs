using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SySDEAProject.Models
{


    [Table("AUDIO_PROVA")]
    public partial class AudioProvaMetaData
    {
        [Key]
        [Column(name: "ID_AUDIO_PROVA")]
        public int idAudioProva ;
        [Column(name: "ID_AVALIACAO")]
        public int idAvaliacao ;
        [Column(name: "DT_GRAVACAO", TypeName = "datetime")]
        public DateTime? dthrGravacao;

    }
    [Table("AUDIO_QUESTAO")]
    public partial class AudioQuestaoMetaData
    {
        [Column(name: "AR_AUDIO_QUESTAO")]
        [Display(Name = "Arquivo"), DataType(DataType.Upload)]
        public byte[] audio;

        [Column(name: "DT_GRAVACAO", TypeName = "DateTime")]
        [Display(Name = "Data de gravação")]
        public DateTime? dataGravacao;

        [Column(name: "ID_LOCUTOR")]
        [Display(Name = "Código do locutor")]
        public int idLocutor;

        [Column(name: "ID_SOTAQUE")]
        [Display(Name = "Código do sotaque")]
        public int idSotaque;

        [Column(name: "NR_QTD_INTERACOES")]
        [Display(Name = "Número de interações")]
        public int? numeroInteracoes;

    }
    [Table("AVALIACAO")]
    public partial class AvaliacaoMetaData
    {
        [Key, ForeignKey("Horario")]
        [Column(name: "ID_AVALIACAO")]
        public int idAvaliacao;

        [ForeignKey("LocalEntidade")]
        [Column(name: "ID_LOCAL_ENTIDADE")]
        public int idLocalEntidade;

        [ForeignKey("Piloto")]
        [Column(name: "ID_USUARIO")]
        public int idPiloto;

        [ForeignKey("VersaoProva")]
        [Column(name: "ID_VERSAO_PROVA")]
        public int idVersaoProva;

        [ForeignKey("NotaFinalEntidade")]
        [Column(name: "ID_NOTA_FINAL_ENTIDADE")]
        public int? idNotaFinalEntidade;

        [ForeignKey("NotaFinal")]
        [Column(name: "ID_NOTA_FINAL")]
        public int? idNotaFinal;

        [Column(name: "NR_NOTA_FINAL_ENTIDADE")]
        public int numeroNotaFinalEntidade;

        [Column(name: "NR_NOTA_FINAL_AVALIACAO")]
        public int notaFinal;

        [Column(name: "DT_INICIO_AVALIACAO", TypeName = "datetime")]
        public DateTime? dthrProvaInicio;

        [Column(name: "DT_FIM_AVALIACAO", TypeName = "datetime")]
        public DateTime? dthrProvaFim;

        [Column(name: "DT_ENVIO_DADOS_AVALIACAO", TypeName = "datetime")]
        public DateTime? dthrEnvioDados;

        [Column(name: "SN_POSSUI_RECURSO_AVALIACAO")]
        public bool? possuiRecurso;

        [Column(name: "SN_SUCESSO_AVALIACAO")]
        public bool? sucesso;

        [Column(name: "ID_STATUS_AVALIACAO")]
        public int? status;

        [Column(name: "DT_DIVULGACAO_RESULTADO_AVALIACAO", TypeName = "datetime")]
        public DateTime? dthrDivulgaRes;

        [Column(name: "DT_ENCERRA_PROCESSO_AVALIACAO", TypeName = "datetime")]
        public DateTime? dthrEncerra;

        [Column(name: "DS_MEDIDA_EXTRA_AVALIACAO")]
        public string providenciasExtras;

        [Column(name: "NR_PROCESSO")]
        public string cdProcesso;

    }

    [Table("AVALIADOR")]
    public partial class AvaliadorMetaData
    {
        [Key, ForeignKey("UserPessoa")]
        [Column(name: "ID_AVALIADOR")]
        public int Id;

        [Required]
        [Column(name: "DT_INICIO_ATIVIDADE_AVALIADOR", TypeName = "date")]
        [Display(Name ="Início das atividades")]
        public DateTime dtInicioAtividade;

        [Column(name: "SN_EM_ATIVIDADE_AVALIADOR")]
        [Display(Name ="Em atividade?")]
        public bool emAtividade;

        [Column(name: "QT_MAXIMO_PROVAS_AVALIADOR")]
        public int maxProvasDiarias;

        [Column(name: "DT_VALIDADE_CERTIFICACAO_AVALIADOR", TypeName = "date")]
        [Display(Name ="Data de validade")]
        public DateTime dataDeValidade;

        [Column(name: "TP_QUALIFICACAO_AVALIADOR")]
        [Display(Name ="Tipo de avaliador")]
        public int? tipoAvaliador;

        [Column(name: "DT_CONCLUSAO_CURSO", TypeName = "date")]
        [Display(Name = "Conclusão do curso")]
        public DateTime dtConclusaoCurso;

        [Column(name: "SN_SUSPENSO_AVALIADOR")]
        [Display(Name = "Suspenso?")]
        public bool suspenso;

        [Column(name: "SN_ATIVO_AVALIADOR")]
        public bool ativo;
    }

    [Table("AVALIADOR_ENTIDADE")]
    public partial class AvaliadorXEntidadeMetaData
    {
        [Key, ForeignKey("Avaliador")]
        [Column(name: "ID_USUARIO", Order = 0)]
        public int idAvaliador;

        [Key, ForeignKey("Entidade")]
        [Column(name: "ID_ENTIDADE", Order = 1)]
        public int idEntidade;

        [Key, Column(name: "ID_AVALIADOR_ENTIDADE", Order = 2)]
        public int idAvaliadorXEntidade;

        [Column(name: "DT_ADMISSAO_AVALIADOR", TypeName = "datetime")]
        public DateTime dataAdmissao;

        [Column(name: "DT_ENCERRAMENTO_AVALIADOR", TypeName = "datetime")]
        public DateTime dataEncerramento;
    }
    [Table("CONFIG_SYSDEA")]
    public partial class ConfigSySDEAMetaData
    {
        [Key]
        [Column(name: "ID_CONFIG_SYSDEA")]
        public int Id;
        [Column(name: "DT_LAST_MODIFIED")]
        public DateTime lastModified;
    }
    [Table("CLAIM")]
    public partial class CustomUserClaimMetaData
    {
        [Key]
        [Column(name: "ID_CLAIM")]
        public int Id;

        [Column(name: "ID_USUARIO")]
        [ForeignKey("Usuario")]
        public int UserId;

        [Column(name: "TX_URI_CLAIM")]
        public string ClaimType;

        [Column(name: "TX_VALOR_CLAIM")]
        public string ClaimValue;

    }
    public partial class ContextoNotaMetaData
    {
        [Column(name: "ID_CONTEXTO_NOTA")]
        public int idContextoNota;

        [Column(name: "TP_CONTEXTO_NOTA", TypeName ="char")]
        public string contextoNota;

        [Column(name: "DS_CONTEXTO_NOTA")]
        public string descricao;
    }
    [Table("EMAIL_MODELO")]
    public partial class EmailModeloMetaData
    {
        [Key, ForeignKey("LocalEntidade"), Column("ID_EMAIL_MODELO")]
        public int Id;
        [Column(name: "TX_TEXTO_EMAIL")]
        public string texto;

        [Column(name: "TX_ASSUNTO_EMAIL")]
        public string assunto;

        [Column(name: "DS_DESCRICAO_EMAIL")]
        public string descricao;
    }
    [Table("EMAIL_SYSDEA")]
    public partial class EmailSySDEAMetaData
    {
        [Key]
        [Column(name: "ID_EMAIL_SYSDEA")]
        public int idEmail ;

        [ForeignKey("Horario")]
        [Column(name: "ID_HORARIO")]
        public int? idHorario ;

        [Column(name: "TX_TEXTO_EMAIL")]
        public string textoEmail ;

        [StringLength(80)]
        [Column(name: "TX_ASSUNTO_EMAIL")]
        public string assunto ;

        [Column(name: "TX_DESTINO_EMAIL")]
        [StringLength(80)]
        public string destino ;

        [Column(name: "TX_REMETENTE_EMAIL")]
        [StringLength(80)]
        public string remetente ;

        [Column(name: "TX_ORIGEM_EMAIL")]
        [StringLength(80)]
        public string tipoOrigem ;

        [Column(name: "DT_HORA_ENVIO")]
        public DateTime horaEnvio ;
    }
    [Table("EMPRESA")]
    public partial class EmpresaMetaData
    {
        [Key]
        [Column(name: "ID_EMPRESA")]
        [Display(Name = "Código da empresa")]
        public int idEmpresa;
        [StringLength(255)]
        [Column(name: "NM_EMPRESA")]
        [Display(Name = "Nome"), Required]
        public string nome;
        [Column(name: "NR_TELEFONE_EMPRESA")]
        [Display(Name = "Telefone")]
        public Nullable<long> tel;
        [StringLength(255)]
        [Column(name: "TX_EMAIL_EMPRESA")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email;
    }
    [Table("ENTIDADE")]
    public partial class EntidadeMetaData
    {
        [Column(name: "ID_USUARIO")]
        public int Id;

        [StringLength(70)]
        [Column(name: "NM_ENTIDADE")]
        public string nome { get; set; }

        [StringLength(255)]
        [Column(name: "TX_EMAIL_ENTIDADE")]
        public string EmailEntidade { get; set; }

        [Column(name: "TX_EMAIL_ELE_ENTIDADE")]
        public string emailELEs { get; set; }

        [Column(name: "NR_CNPJ_ENTIDADE")]
        public long? cnpj { get; set; }

        [Column(name: "SN_SUSPENSA_ENTIDADE")]
        public bool suspensa { get; set; }

        [Column(name: "SN_ATIVA_ENTIDADE")]
        public bool ativa { get; set; } // campo para exclusão lógica

        [Column(name: "TP_TIPO_ENTIDADE")]
        public int tipoEntidade { get; set; } //Diferencia se é uma empresa aérea, escola de inglês ou centro de treinamento
    }

    [Table("FISCALIZACAO_ADMINISTRATIVA")]
    public partial class FiscADMMetaData
    {

        [Column(name: "TP_STATUS_DOCUMENTACAO")]
        public int? documentacao { get; set; }

        [Column(name: "TP_STATUS_GRAVACAO")]
        public int? gravacao { get; set; }

        [Column(name: "TP_STATUS_RELATORIO")]
        public int? relatorios { get; set; }

        
    }
    [Table("FISCAL")]
    public partial class FiscalMetaData
    {
        [Key, ForeignKey("UserPessoa")]
        [Column(name: "ID_FISCAL")]
        public int Id;

        [Column(name: "NR_SIAPE")]
        [Display(Name ="SIAPE")]
        public int? siape;

        [Column(name: "SN_ADMINISTRADOR")]
        [Display(Name = "Administrador?")]
        public bool administrador;

        [Column(name: "SN_ATIVO")]
        public bool ativo;

    }
    [Table("FISCALIZACAO")]
    public partial class FiscalizacaoMetaData
    {
        [Key]
        [Column(name: "ID_FISCALIZACAO")]
        public int idFiscalizacao { get; set; }

        [Column(name: "ID_AVALIACAO")]
        [ForeignKey("Avaliacao")]
        public int idAvaliacao { get; set; }

        [Column(name: "ID_FISCAL")]
        [ForeignKey("Fiscal")]
        public int idFiscal { get; set; }

        [Column(name: "ID_RECURSO")]
        [ForeignKey("Recurso")]
        public int idRecurso { get; set; }

        [Column(name: "TP_TIPO_FISCALIZACAO")]
        public int? codigoTipoFisc { get; set; }

        [Column(name: "DT_INICIO_FISCALIZACAO", TypeName = "datetime")]
        public DateTime? dthrInicio { get; set; }

        [Column(name: "DT_FIM_FISCALIZACAO", TypeName = "datetime")]
        public DateTime? dthrFim { get; set; }

        [Column(name: "DS_OBSERVACOES")]
        public string observacoes { get; set; }

        [Column(name: "SN_PARTE_RECURSO")]
        public bool? parteDeRecurso { get; set; }
    }
    [Table("FISCALIZACAO_ENTREVISTA")]
    public partial class FiscEntrevistaMetaData
    {

        [Column(name: "TP_GRAU_USO_LINGUAGEM")]
        public int? usoDaLinguagem { get; set; }

        [Column(name: "TP_GRAU_CONSTRUCAO_CONVERSA")]
        public int? construcaoRapport { get; set; }

        [Column(name: "TP_GRAU_CONDUCAO_ENTREVISTA")]
        public int? conducaoEntrevista { get; set; }

        [Column(name: "SN_NIVEL_FALA_OK")]
        public bool? speechRate { get; set; }

        [Column(name: "SN_INGLES_OK")]
        public bool? ingles { get; set; }

        [Column(name: "SN_PARAFRASES_OK")]
        public bool? parafrases { get; set; }

        [Column(name: "SN_ENERGIA_OK")]
        public bool? energia { get; set; }

        [Column(name: "SN_ETIQUETA_OK")]
        public bool? etiqueta { get; set; }

        [Column(name: "SN_ATITUDE_OK")]
        public bool? atitude { get; set; }

        [Column(name: "SN_CLAREZA_OK")]
        public bool? clarification { get; set; }

        [Column(name: "SN_PROCEDIMENTOS_OK")]
        public bool? procedimentosBasicos { get; set; }

        [Column(name: "SN_TEMPO_OK")]
        public bool? controleTempo { get; set; }

        [Column(name: "SN_INTERPRETACAO_OK")]
        public bool? interpretacaoScript { get; set; }

    }
    [Table("FISCALIZACAO_PARECER")]
    public partial class FiscParecerMetaData
    {
        
        [Column(name: "SN_NOTA_ALTERADA")]
        public bool? notaAlterada;
    }
    [Table("HISTORICO")]
    public partial class HistoricoMetaData
    {
        [Display(Name = "Observações de horário")]
        [DataType(DataType.MultilineText)]
        [Column(name: "DS_OBSERVACAO_HISTORICO")]
        public string observacoes;
    }
    [Table("HORARIO")]
    public partial class HorarioMetaData
    {
        [Key]
        [Column(name: "ID_HORARIO")]
        public int idHorario { get; set; }


        [Column(name: "VL_PRECO_PROVA", TypeName ="money")]
        public double preco { get; set; }

        [Column(name: "ID_SME")]
        [ForeignKey("sme")]
        public int? idSme { get; set; }


        [Column(name: "ID_ELE")]
        [ForeignKey("ele")]
        public int? idEle { get; set; }

        [Column(name: "DT_HORARIO_AGENDADO", TypeName = "datetime2")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? data { get; set; }

        [Column(name: "TP_STATUS_AGENDAMENTO")]
        public int? status { get; set; }

        [Column(name: "NR_SALA")]
        public int? sala { get; set; }

        [ForeignKey("Piloto")]
        [Column(name: "ID_PILOTO")]
        public int? idPiloto { get; set; }

        [Column(name: "ID_LOCAL_ENTIDADE")]
        [ForeignKey("LocalEntidade")]
        public int idLocalEntidade { get; set; }
    }

    [Table("IMAGEM_PILOTO")]
    public partial class ImagemPilotoMetaData
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(name: "ID_PILOTO")]
        public int idPiloto { get; set; }

        [Display(Name = "Foto do piloto")]
        [DataType(DataType.Upload)]
        [Column(name: "IM_IMAGEM_PILOTO")]
        public byte[] arquivo { get; set; }
    }
    [Table("IMAGEM_QUESTAO")]
    public partial class ImagemQuestaoMetaData
    {
        [Display(Name = "Arquivo"), DataType(DataType.Upload)]
        public byte[] imagem;
    }

    [Table("INSTRUCAO")]
    public partial class InstrucaoQuestaoMetaData
    {

        [StringLength(511)]
        [Column(name: "TX_TEXTO_INSTRUCAO")]
        public string texto { get; set; }

        [Column(name: "NR_NIVEL_VOCABULARIO")]
        public int? nivelVocab { get; set; }

        [Column(name: "NR_NIVEL_ESTRUTURA")]
        public int? nivelStruct { get; set; }

    }
    [Table("INTERACAO")]
    public partial class InteracaoMetaData
    {
        [Key]
        [Column(name: "ID_INTERACAO")]
        public int idInteracao { get; set; }

        [StringLength(255)]
        [Column(name: "NM_TITULO")]
        public string titulo { get; set; }

        [Column(name: "DS_DESCRICAO")]
        public string descricao { get; set; }

        [Column(name: "ID_PEDACO_QUESTAO")]
        [ForeignKey("AudioQuestao")]
        public int idPedacoQuestao { get; set; }

    }

    [Table("LOCAL_ENTIDADE")]
    public partial class LocalEntidadeMetaData
    {
        [Key]
        [Column(name: "ID_LOCAL_ENTIDADE")]        
        [Display(Name = "Código do local da entidade")]
        public int idLocalEntidade;
        
        [Column(name: "ID_ENTIDADE")]
        [ForeignKey("Entidade")]
        [Display(Name = "Código da Entidade")]
        public int idEntidade;

        [StringLength(255), Required]
        [Column(name: "TX_EMAILCONTATO")]
        public string emailContato;

        [Column(name: "SN_ACEITA_SOLICITACOES")]
        [Display(Name = "Aceita solicitações de horários?")]
        public bool aceitaSolicitacoes;

        [Column(name: "VL_PRECO_PROVA")]
        public double precoAvaliacao;

        [Column(name: "SN_ATIVA")]
        [Display(Name = "Filial existe?")]
        public bool ativa;

        [Column(name: "NR_SALAS")]
        [Display(Name = "Número de salas")]
        public Nullable<int> numeroSalas;

        [StringLength(30)]
        [Column(name: "TX_TITULO")]
        [Display(Name = "Filial da entidade")]
        public string titulo;

        [Column(name: "SN_SUSPENSA")]
        [Display(Name = "Filial suspensa?")]
        public bool suspensa;
    }

    public partial class PilotoMetaData 
    {

        [Display(Name = "Telefone de contato")]
        public long? telContato;
        [Display(Name = "CANAC do piloto")]
        public int CANACPiloto;
        [Display(Name = "Código da empresa")]
        public int idEmpresa;
        [Range(0, 5, ErrorMessage = "Nível inválido, escolha um nível entre 'Sem nível' e 5")]
        [Display(Name = "Nível Atual")]
        public int nivelAtual;
        [Display(Name = "Observações de piloto")]
        [DataType(DataType.MultilineText)]
        public string observacoes;

    }


   
    public partial class QuestaoMetaData
    {
     
        [Display(Name = "Código da questão")]
        public int idQuestao;
        [Display(Name = "Dificuldade")]
        public Nullable<byte> dificuldade;
        [Display(Name = "Peso de escolha")]
        public Nullable<double> peso;
        [Display(Name = "Questão suspensa?")]
        public bool suspensa;
        [Display(Name = "Número de vezes usada")]
        public Nullable<int> vezesEscolhida;
        [Display(Name = "Tipo de conteúdo")]
        public string conteudo;
        [Display(Name = "Diretriz")]
        public string diretriz;
        [Display(Name = "Código de tema")]
        public int idTema;
        [Display(Name = "Código de parte original")]
        public Nullable<int> idParteOriginal;
        [Display(Name = "Número de parte")]
        public Nullable<int> numeroParte;
    }

    public partial class PedacoQuestaoMetaData
    {
        [Display(Name ="Código do pedaço de questão")]
        public int idPedacoQuestao;

        [Display(Name ="Código da questão")]
        public int idQuestao;

        [Display(Name ="Número de ordenação")]
        public int? numeroOrdem;
    }





        

    public class EnderecoMetaData
    {
        [Display(Name = "CEP")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:99999-999}")]
        public long cep;
        [Display(Name = "Rua ou avenida")]
        public string rua;
        [Display(Name = "Número")]
        public string numero;
        [Display(Name = "Complemento")]
        public string complemento;
        [Display(Name = "Bairro")]
        public string bairro;
        [Display(Name = "Município")]
        public string municipio;
        [Display(Name = "UF")]
        public string uf;
    }

    public class TextoQuestaoMetaData
    {
        [Display(Name ="Texto")]
        [StringLength(511)]
        public string texto ;
        [Display(Name ="Tipo do texto")]
        public int? tipoTexto ;
    }
    public partial class UserPessoaMetaData
    {

        [Display(Name = "Nome completo"), Required]
        public string nome;
        [Display(Name = "E-mail"), Required]
        [DataType(DataType.EmailAddress)]
        public string Email;
        [Display(Name = "Telefone"), Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:(##) ####-#####}")]
        public string PhoneNumber ;
        [Display(Name = "CPF"), Required]
        public long cpf;

        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de nascimento")]
        public DateTime dataNascimento;

    }
    public class VersaoProvaMetaData
    {
        [Display(Name ="Código da versão")]
        public int idVersaoProva ;
        [Display(Name = "Vezes aplicada")]
        public int? vezesAplicada ;
        [Display(Name = "Data de geração")]
       
        public DateTime? dtGeracao ;
    }
}
