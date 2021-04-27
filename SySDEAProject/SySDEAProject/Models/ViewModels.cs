using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SySDEAProject.Models
{
        public class CriarHorarioViewModel
        {
            public CriarHorarioViewModel()
        {
            
        }
        public CriarHorarioViewModel(Horario horario)
        {
            data = horario.data.Value.Date;
            hora = horario.data.Value.TimeOfDay;
            idHorario = horario.idHorario;
            preco = horario.preco;
            status = horario.status;
            sala = horario.sala;
            Piloto_id = horario.idPiloto;
            idLocalEntidade = horario.idLocalEntidade;
        }
        [Required]
            [Display(Name = "Data"), ValidateDataAgendamento]
            [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]            
            public DateTime? data { get; set; }
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
            public TimeSpan? hora { get; set; }

            [Display(Name = "Código do horário")]
            public int idHorario { get; set; }

            public double preco { get; set; }
            public int? status { get; set; }

            public int? sala { get; set; }
            
            public int? Piloto_id { get; set; }
            public int idLocalEntidade { get; set; }
        }
        public class SolicitacaoHorarioViewModel
    {
        public SolicitacaoHorarioViewModel()
        {            
        }
        public int idPiloto { get; set; }
       
        public int idLocalEntidade { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [ValidateDataAgendamento]
        public DateTime dia { get; set; }                
        public TimeSpan hora1 { get; set; }        
        public TimeSpan? hora2 { get; set; }        
        public TimeSpan? hora3 { get; set; }
        [DataType(DataType.MultilineText)]       
        public string observacoes { get; set; }
    }
    }
