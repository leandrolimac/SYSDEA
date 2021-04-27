using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Utilities;
using System.Linq;
using System.Web;

namespace SySDEAProject.Models
{

    public partial class SySDEAContext : IdentityDbContext<Usuario, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public SySDEAContext() : base("DefaultConnection")
        {
        }
        public static SySDEAContext Create()
        {
            return new SySDEAContext();
        }
        public virtual DbSet<CustomUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AudioProva> AudioProva { get; set; }
        public virtual DbSet<AudioQuestao> AudioQuestao { get; set; }
        public virtual DbSet<Avaliacao> Avaliacao { get; set; }
        public virtual DbSet<Avaliador> Avaliador { get; set; }
        public virtual DbSet<AvaliadorXEntidade> Avaliadores_Entidades { get; set; }
        public virtual DbSet<ConfigSySDEA> ConfigSySDEA { get; set; }
        //public virtual DbSet<Discriminator> Discriminator { get; set; }
        public virtual DbSet<EmailSySDEA> EmailSySDEA { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        //public virtual DbSet<EnderecoPessoa> EnderecoPessoa { get; set; }
        //public virtual DbSet<EnderecoLocalEntidade> EnderecoLocalEntidade { get; set; }
        public virtual DbSet<Endereco> Endereco { get; set; }
        public virtual DbSet<Entidade> Entidade { get; set; }
        //public virtual DbSet<EntidadeLogin> EntidadeLogin { get; set; }
        public virtual DbSet<FiscADM> FiscADM { get; set; }
        public virtual DbSet<Fiscal> Fiscal { get; set; }
        public virtual DbSet<Fiscalizacao> Fiscalizacao { get; set; }
        public virtual DbSet<FiscEntrevista> FiscEntrevista { get; set; }
        public virtual DbSet<FiscParecer> FiscParecer { get; set; }
        public virtual DbSet<Historico> Historico { get; set; }
        public virtual DbSet<Horario> Horario { get; set; }
        public virtual DbSet<ImagemPiloto> ImagemPiloto { get; set; }
        public virtual DbSet<ImagemQuestao> ImagemQuestao { get; set; }
        public virtual DbSet<Interacao> Interacao { get; set; }
        public virtual DbSet<InfoAeronauta> InfoAeronauta { get; set; }
        public virtual DbSet<LocalEntidade> LocalEntidade { get; set; }
        public virtual DbSet<Locutor> Locutor { get; set; }
        public virtual DbSet<LocutorXSotaque> LocutorXSotaque { get; set; }
        public virtual DbSet<Nota> Nota { get; set; }
        public virtual DbSet<NotaAvaliador> NotaAvaliador { get; set; }
        public virtual DbSet<NotaFisc> NotaFisc { get; set; }
        public virtual DbSet<ParteOriginal> ParteOriginal { get; set; }
        //public virtual DbSet<ParteProva> ParteProva { get; set; }
        public virtual DbSet<PedacoQuestao> PedacoQuestao { get; set; }
        public virtual DbSet<Piloto> Piloto { get; set; }
        //public virtual DbSet<Processo> Processo { get; set; }
        public virtual DbSet<Questao> Questao { get; set; }
        public virtual DbSet<Recurso> Recurso { get; set; }
        public virtual DbSet<RegistroAtividade> RegistroAtividade { get; set; }
        public virtual DbSet<SolicitacaoHorario> SolicitacaoHorario { get; set; }
        public virtual DbSet<Sotaque> Sotaque { get; set; }
        public virtual DbSet<Tema> Tema { get; set; }
        public virtual DbSet<TextoQuestao> TextoQuestao { get; set; }
        public virtual DbSet<UserPessoa> UserPessoa { get; set; }
        public virtual DbSet<VersaoOriginal> VersaoOriginal { get; set; }
        public virtual DbSet<VersaoProva> VersaoProva { get; set; }
        public virtual DbSet<VideoProva> VideoProva { get; set; }

        //public System.Data.Entity.DbSet<SySDEAProject.Models.CustomRole> CustomRoles { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {



            modelBuilder.Entity<AudioQuestao>()
                .HasMany(e => e.Interacao)
                .WithRequired(e => e.AudioQuestao)
                .WillCascadeOnDelete(false);

            /*modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Usuario)
                .Map(m => m.ToTable("CustomUserRoles").MapLeftKey("UserId").MapRightKey("RoleId"));*/

            modelBuilder.Entity<Avaliacao>()
                .HasMany(e => e.AudioProva)
                .WithRequired(e => e.Avaliacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Avaliacao>()
                .HasMany(e => e.Fiscalizacao)
                .WithRequired(e => e.Avaliacao)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Avaliacao>()
                .HasMany(e => e.NotaAvaliador)
                .WithRequired(e => e.Avaliacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Avaliacao>()
                .HasMany(e => e.Recurso)
                .WithRequired(e => e.Avaliacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Avaliacao>()
                .HasMany(e => e.VideoProva)
                .WithRequired(e => e.Avaliacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Avaliador>()
                .HasMany(e => e.NotaAvaliador)
                .WithRequired(e => e.Avaliador)
                .WillCascadeOnDelete(false);


            /* modelBuilder.Entity<Avaliador>()
                .HasMany(e => e.Horario)
                .WithMany(e => e.Avaliador)
                .Map(m => m.ToTable("Avaliadores_Horarios").MapLeftKey("idAvaliador").MapRightKey("idHorario")); */

            modelBuilder.Entity<Empresa>()
                .HasMany(e => e.Piloto)
                .WithRequired(e => e.Empresa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Entidade>()
                .HasMany(e => e.LocalEntidade)
                .WithRequired(e => e.Entidade)
                .WillCascadeOnDelete(true);



            modelBuilder.Entity<Fiscal>()
                .HasMany(e => e.Fiscalizacao)
                .WithRequired(e => e.Fiscal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Fiscal>()
                .HasMany(e => e.Recurso)
                .WithRequired(e => e.Fiscal)
                .WillCascadeOnDelete(false);

            /*  modelBuilder.Entity<Fiscalizacao>()
                  .HasOptional(e => e.FiscADM)
                  .WithRequired(e => e.Fiscalizacao);

              modelBuilder.Entity<Fiscalizacao>()
                  .HasOptional(e => e.FiscEntrevista)
                  .WithRequired(e => e.Fiscalizacao);

              modelBuilder.Entity<Fiscalizacao>()
                  .HasOptional(e => e.FiscParecer)
                  .WithRequired(e => e.Fiscalizacao); */

            modelBuilder.Entity<FiscParecer>()
                .HasMany(e => e.NotaFisc)
                .WithRequired(e => e.FiscParecer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Historico>()
                .HasMany(e => e.Questao)
                .WithMany(e => e.Historico)
                .Map(m => m.ToTable("HISTORICOS_QUESTOES").MapLeftKey("ID_PILOTO").MapRightKey("ID_QUESTAO"));

            modelBuilder.Entity<Historico>()
                .HasMany(e => e.VersaoOriginal)
                .WithMany(e => e.Historico)
                .Map(m => m.ToTable("HISTORICOS_VERSOES_ORIGINAIS").MapLeftKey("ID_PILOTO").MapRightKey("ID_VERSAO_ORIGINAL"));

            /*modelBuilder.Entity<Horario>()
                .HasMany(e => e.Avaliacao)
                .WithRequired(e => e.Horario)
                .WillCascadeOnDelete(false);
                */
            modelBuilder.Entity<LocalEntidade>()
                .HasMany(e => e.Horario)
                .WithRequired(e => e.LocalEntidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Locutor>()
                .HasMany(e => e.LocutorXSotaque)
                .WithRequired(e => e.Locutor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LocutorXSotaque>()
                .HasMany(e => e.AudioQuestao)
                .WithRequired(e => e.LocutorXSotaque)
                .HasForeignKey(e => new { e.idLocutor, e.idSotaque })
                .WillCascadeOnDelete(false);

            /*modelBuilder.Entity<Nota>()
                .HasOptional(e => e.NotaAvaliador)
                .WithRequired(e => e.Nota);

            modelBuilder.Entity<Nota>()
                .HasOptional(e => e.NotaFisc)
                .WithRequired(e => e.Nota);*/

            modelBuilder.Entity<VersaoProva>()
                .HasMany(e => e.Questao)
                .WithMany(e => e.VersaoProva)
                .Map(m => m.ToTable("VERSOES_PROVA_QUESTOES").MapLeftKey("ID_VERSAO_PROVA").MapRightKey("ID_QUESTAO"));

            modelBuilder.Entity<VersaoProva>()
                .HasMany(e => e.ParteProva)
                .WithMany(e => e.VersaoProva)
                .Map(m => m.ToTable("VERSOES_PROVA_PARTES_PROVAS").MapLeftKey("ID_VERSAO_PROVA").MapRightKey("ID_PARTE_PROVA"));

            modelBuilder.Entity<Questao>()
    .HasMany(e => e.ParteProva)
    .WithMany(e => e.Questao)
    .Map(m => m.ToTable("QUESTOES_PARTES_PROVAS").MapLeftKey("ID_QUESTAO").MapRightKey("ID_PARTE_PROVA"));

            modelBuilder.Entity<Piloto>()
                .HasOptional(e => e.Historico)
                .WithRequired(e => e.Piloto);

            modelBuilder.Entity<Piloto>()
                .HasOptional(e => e.ImagemPiloto)
                .WithRequired(e => e.Piloto);


            modelBuilder.Entity<Questao>()
                .Property(e => e.peso)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Questao>()
                .HasMany(e => e.PedacoQuestao)
                .WithRequired(e => e.Questao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Recurso>()
                .HasMany(e => e.Fiscalizacao)
                .WithRequired(e => e.Recurso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sotaque>()
                .HasMany(e => e.LocutorXSotaque)
                .WithRequired(e => e.Sotaque)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tema>()
                .Property(e => e.descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Tema>()
                .HasMany(e => e.Questao)
                .WithRequired(e => e.Tema)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<VersaoOriginal>()
                .HasMany(e => e.ParteOriginal)
                .WithRequired(e => e.VersaoOriginal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VersaoProva>()
                .HasMany(e => e.Avaliacao)
                .WithRequired(e => e.VersaoProva)
                .WillCascadeOnDelete(false);

        }

        public System.Data.Entity.DbSet<SySDEAProject.Models.EmailModelo> EmailModeloes { get; set; }
    }

    class MyCodeGenerator : CSharpMigrationCodeGenerator
    {
        protected override void Generate(
            DropIndexOperation dropIndexOperation, IndentedTextWriter writer)
        {
            dropIndexOperation.Table = StripDbo(dropIndexOperation.Table);

            base.Generate(dropIndexOperation, writer);
        }

        // TODO: Override other Generate overloads that involve table names

        private string StripDbo(string table)
        {
            if (table.StartsWith("dbo."))
            {
                return table.Substring(4);
            }

            return table;
        }
    }
}