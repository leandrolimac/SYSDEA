using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SySDEAProject.Models;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Validation;
using System.Data.SqlClient;

namespace SySDEAProject.Controllers
{
    public class HorariosController : Controller
    {

        private SySDEAContext db = new SySDEAContext();



        [Authorize(Roles ="Entidade")]
        public ActionResult Agendamentos(bool? expirados)
        //********************************************************************************************//
        // EXIBE OS AGENDAMENTOS DA ENTIDADE, SEJAM DISPONÍVEIS, RESERVADOS, CONFIRMADOS OU           //
        // EM CANCELAMENTO                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            DateTime agoraMenos30 = DateTime.Now.AddMinutes(-30);
            List<Horario> indisponiveis = (from hi in db.Horario where hi.data.Value < agoraMenos30 && hi.status != 6 select hi).ToList();

            for (int j = 0; j < indisponiveis.Count(); j++)
            {
                Horario horarioExpirado = indisponiveis.ElementAt(j);
                horarioExpirado.status = 6;
                if (horarioExpirado.Piloto != null)
                {
                    horarioExpirado.Piloto.agendamentoAtivo = false;
                }
                db.Entry(horarioExpirado.Piloto).State = EntityState.Modified;
                db.Entry(horarioExpirado).State = EntityState.Modified;
                db.SaveChanges();
            }

            int entidadeId = User.Identity.GetUserId<int>();
            var agendamentos = from a in db.Horario select a;
            if (expirados != true)
            {
                agendamentos = from a in db.Horario
                                   where a.LocalEntidade.idEntidade == entidadeId && a.status != 6 && a.status != 8
                                   select a;
                ViewBag.expirados = false;

            }else
            {
                ViewBag.expirados = true;
                agendamentos = from a in db.Horario
                                   where a.LocalEntidade.idEntidade == entidadeId
                                   select a;
            }
            return View(agendamentos.ToList());
        }

        [Authorize(Roles = "Entidade")]
        public ActionResult AgendamentosDoDia()
        //********************************************************************************************//
        // EXIBE OS AGENDAMENTOS DA ENTIDADE PARA O DIA CORRENTE, SEJAM DISPONÍVEIS, RESERVADOS,      //
        // CONFIRMADOS OU EM CANCELAMENTO                                                             //
        //                                                                                            //
        //********************************************************************************************//
        {
            DateTime agoraMenos30 = DateTime.Now.AddMinutes(-30);
            List<Horario> indisponiveis = (from hi in db.Horario where  hi.data < agoraMenos30 && hi.status != 6 select hi).ToList();

            for (int j = 0; j < indisponiveis.Count(); j++)
            {
                Horario horarioExpirado = indisponiveis.ElementAt(j);
                horarioExpirado.status = 6;
                if (horarioExpirado.Piloto != null)
                {
                    horarioExpirado.Piloto.agendamentoAtivo = false;
                }
                db.Entry(horarioExpirado.Piloto).State = EntityState.Modified;
                db.Entry(horarioExpirado).State = EntityState.Modified;
                db.SaveChanges();
            }

            int entidadeId = User.Identity.GetUserId<int>();
            var agendamentos = from a in db.Horario where a.LocalEntidade.idEntidade == entidadeId select a ;
            List<Horario> agendamentosHoje = new List<Horario>();
            foreach(var item in agendamentos)
            {
                if (item.data >= DateTime.Now.AddMinutes(-30))
                    agendamentosHoje.Add(item);
            }
            return View(agendamentosHoje.ToList());
        }



        [Authorize(Roles = "Entidade")]
        public ActionResult agendaSolicitacao(int id, int nh)
        //********************************************************************************************//
        // CRIA UM HORÁRIO RESERVADO NO DIA E HORÁRIO ESPECIFICADO PELO PILOTO NO FORMULÁRIO DE       //
        // SOLICITAÇÃO DE AGENDAMENTO                                                                 //
        //                                                                                            //
        //********************************************************************************************//
        {
            SolicitacaoHorario solicitacao = db.SolicitacaoHorario.Find(id);
            Horario horario = new Horario();
            if (solicitacao.Piloto.agendamentoAtivo == true)
            {
                return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "Piloto já agendado em outro horário" });
            }
            else            
            {
                horario.Piloto = solicitacao.Piloto;
                horario.LocalEntidade = solicitacao.LocalEntidade;
                switch (nh)
                {
                    case 1:
                        horario.data = solicitacao.hora1;                        
                        break;
                    case 2:
                        horario.data = solicitacao.hora2.Value;
                        
                        break;
                    case 3:
                        horario.data = solicitacao.hora3.Value;
                        
                        break;
                }                
                
                if (horario.data.Value.Date < DateTime.Today) {
                    ViewBag.StatusMessage = "Horário expirado";
                    return RedirectToAction("VerificaSolicitacoes");
                }
                else
                if (horario.data.Value.Date == DateTime.Today)
                {
                    if (horario.data < DateTime.Now)
                    {
                        ViewBag.StatusMessage = "Horário expirado";
                        return RedirectToAction("VerificaSolicitacoes");
                    }
                }
                horario.preco = solicitacao.LocalEntidade.precoAvaliacao;
                horario.status = 2;
                horario.sala = 1;
                horario.Piloto.agendamentoAtivo = true;
                solicitacao.aberta = false;
                solicitacao.aceita = true;
                db.Entry(solicitacao).State = EntityState.Modified;
                db.Entry(horario.Piloto).State = EntityState.Modified;
                db.Horario.Add(horario);
                db.SaveChanges();
                return RedirectToAction("Agendamentos", "Horarios", new { StatusMessage = "Piloto agendado com sucesso" });
            }
        }
        [Authorize(Roles = "Entidade")]
        public ActionResult RecusaSolicitacao(int? id)
        //********************************************************************************************//
        // RECUSA UMA SOLICITAÇÃO DE HORÁRIO FEITA POR UM PILOTO E ENVIA UM EMAIL O AVISANDO SOBRE    //
        // O ESTADO DE SEU AGENDAMENTO                                                                //
        //                                                                                            //
        //********************************************************************************************//
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SolicitacaoHorario solicitacao = db.SolicitacaoHorario.Find(id);
            if (solicitacao == null)
            {
                return HttpNotFound();
            }

            return View(solicitacao);            
        }
        [HttpPost, ActionName("RecusaSolicitacao")]
        public async System.Threading.Tasks.Task<ActionResult> ConfirmaRecusaSolicitacao(int idSolicitacao, string justificativa)
        {
            SolicitacaoHorario solicitacao = db.SolicitacaoHorario.Find(idSolicitacao);
            solicitacao.aberta = false;
            solicitacao.aceita = false;
            db.Entry(solicitacao).State = EntityState.Modified;
            MailMessage message = new MailMessage();
            message.Subject = "Solicitação de horário recusada";
            message.To.Add(solicitacao.Piloto.UserPessoa.Email);        
            message.Body = "Prezado" + solicitacao.Piloto.UserPessoa.nome + "," + "<br/>" + "Sua solicitação de horário foi recusada pela " + solicitacao.LocalEntidade.titulo + "<br/>";
            EmailSySDEA email = new EmailSySDEA(message);            
            email.tipoOrigem = "Resposta de Solicitação";
           
            if (justificativa != "")
            {
                email.textoEmail = email.textoEmail + " Justificativa: " + justificativa;
            }

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "sistema.sysdea@anac.gov.br",  // Email usado para envio da mensagem
                    Password = "qiuEUvZBW7vGAZm9PseK"  // senha para autenticação no servidor de emails
                };
                smtp.Credentials = credential;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                await smtp.SendMailAsync(message);
                db.EmailSySDEA.Add(email);
                db.SaveChanges();
                return RedirectToAction("Agendamentos");
            }
            
        }





        // GET: Horarios
        [Authorize(Roles ="Fiscal, Admin")]
        public ActionResult Index()
        {
            //********************************************************************************************//
            //                                                                                            //
            //    LISTA OS HORÁRIOS CADASTRADOS NO SISTEMA                                                //
            //                                                                                            //
            //********************************************************************************************//
            var horario = db.Horario.Include(h => h.LocalEntidade).Include(h => h.Piloto);
            return View(horario.ToList());
        }


        //HTTP GET
        [Authorize(Roles ="Entidade")]
        public ActionResult CancelarHorarioDisponivel(int? id)
        //********************************************************************************************//
        //                                                                                            //
        //     ENTIDADE DELETA UM HORÁRIO SEM PILOTO AGENDADO DO SISTEMA                              //
        //                                                                                            //
        //********************************************************************************************//
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horario.Find(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            else
            {
                if(horario.status != 1)
                {
                    return View("PainelUsuario","Manage", new { StatusMessage = "Este horário possui um piloto agendado. Utilize a funcionalidade Cancelar agendamento reservado " });
                }
            }
            return View(horario);
        }
       [HttpPost]
        public ActionResult ConfirmaCancelarHorario(int idHorario)
        {
            Horario horario = db.Horario.Find(idHorario);
            db.Horario.Remove(horario);
            db.SaveChanges();
            return RedirectToAction("Agendamentos","Horarios", new { StatusMessage = "Horário excluido com sucesso" });
        }



        [Authorize(Roles = "Entidade")]
        public ActionResult CancelarAgendamentoReservado(int? id)
        //********************************************************************************************//
        // REMOVE UM PILOTO DE UM HORÁRIO RESERVADO E O HORÁRIO VOLTA A ESTAR DISPONÍVEL              //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horario.Find(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            return View(horario);
        }
        [HttpPost]
        
        public ActionResult ConfirmaCancelarAgendamento(int idHorario)
        {
            Horario horario = db.Horario.Find(idHorario);
            horario.Piloto.agendamentoAtivo = false;
            db.Entry(horario.Piloto).State = EntityState.Modified;
            horario.Piloto = null;
            horario.status = 1;
            horario.idPiloto = null;          
            db.Entry(horario).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Manage", new { StatusMessage = "Agendamento cancelado com sucesso" });
        }



        [HttpPost]
        [Authorize(Roles = "Entidade, Fiscal")]
        public ActionResult ConsultarPiloto(Piloto piloto)
        //********************************************************************************************//
        //                                                                                            //
        //ENTIDADE CONSULTA UM DE SEUS PILOTOS AGENDADOS                                              //
        //                                                                                            //
        //********************************************************************************************//
        {
            int codigoEntidade = piloto.Horario.Last().LocalEntidade.idEntidade;
            if (codigoEntidade == User.Identity.GetUserId<int>())
            {
                return View(piloto);
            }
            else
                return RedirectToAction("AcessoNegado", "Account");
        }
        [Authorize(Roles ="Entidade,Fiscal")]
        public ActionResult ConsultarPiloto(int id)
        {
            Piloto piloto = db.Piloto.Find(id);
            if (piloto != null)
            {
                return ConsultarPiloto(piloto);
            }
            return View("Error");
        }


        [Authorize(Roles = "Entidade")]
        public ActionResult ContatarPiloto(int? Id)
        //********************************************************************************************//
        //  ENTIDADE ENVIA UM EMAIL PARA O PILOTO SELECIONADO                                         //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {

            Piloto piloto = db.Piloto.Find(Id);
            if (piloto != null)
            {
                return View(piloto);
            }
            return View("Error");
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ContatarPiloto([Bind(Include="Id, nome, Email")] Piloto piloto,string assunto, string mensagem)
        {
            if (ModelState.IsValid)
            {
                Entidade entidade = db.Entidade.Find(User.Identity.GetUserId<int>());
                
                var body = "<p>Email SySDEA: {0}</p><p>Mensagem:</p><p>{1}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("leandro.campos@anac.gov.br"));  // trocar por email do piloto

                message.Subject = assunto;
                message.Body = string.Format(body, entidade.nome , mensagem);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "sistema.sysdea@anac.gov.br",  
                        Password = "qiuEUvZBW7vGAZm9PseK" 
                     };
                    smtp.Credentials = credential;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    await smtp.SendMailAsync(message);
                    EmailSySDEA emailSySDEA = new EmailSySDEA();
                    emailSySDEA.textoEmail = message.Body;
                    emailSySDEA.assunto = assunto;
                    emailSySDEA.tipoOrigem = "Contato de entidade para piloto";
                    db.EmailSySDEA.Add(emailSySDEA);
                    db.SaveChanges();
                    return RedirectToAction("Sent","Home");
                }
            }
            return RedirectToAction("PainelUsuario", "Manage");
        }





        //********************************************************************************************//
        //                                                                                            //
        //PILOTO ENVIA UM EMAIL PARA A ENTIDADE SELECIONADA                                           //
        //                                                                                            //
        //********************************************************************************************//
        [Authorize(Roles = "Piloto")]
        public ActionResult ContatarEntidade()
            
        {

            int idPiloto = User.Identity.GetUserId<int>();
            Piloto piloto = db.Piloto.Find(idPiloto);
            if (piloto.agendamentoAtivo)
            {

                LocalEntidade LocalEntidade = piloto.Horario.Last().LocalEntidade;
                    ViewBag.LocalEntidade = LocalEntidade;
                    return View(LocalEntidade);                
            }
            return View("Error");
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ContatarEntidade(int idLocalEntidade, string assunto, string mensagem)
        {
            if (ModelState.IsValid)
            {
                Piloto piloto = db.Piloto.Find(User.Identity.GetUserId<int>());
               
                LocalEntidade localEntidade = db.LocalEntidade.Find(idLocalEntidade);
                var body = "<p>Email SySDEA: {0}</p><p>Mensagem:</p><p>{1}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("leandro.campos@anac.gov.br"));  // trocar por email da entidade
                message.To.Add(new MailAddress(localEntidade.emailContato));
                message.Subject = assunto;
                message.Body = string.Format(body, piloto.UserPessoa.nome, mensagem);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "sistema.sysdea@anac.gov.br",
                        Password = "qiuEUvZBW7vGAZm9PseK"
                    };
                    smtp.Credentials = credential;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    await smtp.SendMailAsync(message);
                    EmailSySDEA emailSySDEA = new EmailSySDEA(message);
                    
                    emailSySDEA.tipoOrigem = "Contato de entidade para piloto";
                    db.EmailSySDEA.Add(emailSySDEA);
                    db.SaveChanges();
                    return RedirectToAction("Sent", "Home");
                }
            }
            return RedirectToAction("PainelUsuario", "Account");
        }

        //********************************************************************************************//
        //                                                                                            //
        // INICIA UMA AVALIAÇÃO AGENDADA, O HORÁRIO DEVE TER NO MÁXIMO 30 MINUTOS DE DIFERENÇA DO     //
        // AGENDADO                                                                                   //
        //********************************************************************************************//
        [Authorize(Roles = "Entidade")]
        public ActionResult IniciarAvaliacao(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horario.Find(id);
            if (horario == null) //CASO O HORÁRIO NÃO EXISTA
            {
                return HttpNotFound();
            }

            if (horario.LocalEntidade.Entidade.Id == User.Identity.GetUserId<int>()) //Verifica se o horário pertence à entidade em sessão
            {
                if(horario.status != 3) //Se o horário não estiver confirmado, retornar à agenda
                {
                    return RedirectToAction("Agendamentos", "Horarios", new { StatusMessage = "O horário escolhido não está confirmado, favor verificar" });
                }

                TimeSpan TrintaMinutos = new TimeSpan(0, 0, 30, 0);
                if (horario.data.Value.Add(TrintaMinutos) >= DateTime.Now && DateTime.Now.Add(TrintaMinutos) >= horario.data.Value // Confere se há menos de 30 minutos de diferença entre a hora atual e a hora agendada
                    && horario.data.Value.Date == DateTime.Now.Date //Confere se o dia está certo
                    ) 
                {
                    //Prepara os "avaliadores" para as drop down lists da view
                    int idEntidadeSQL;
                    idEntidadeSQL = User.Identity.GetUserId<int>();
                    Entidade entidade = db.Entidade.Find(idEntidadeSQL);
                    var locaisEntidadeAtual = from l in db.LocalEntidade where l.idEntidade == idEntidadeSQL select l; //"SELECT * FROM localEntidade WHERE idEntidade =" + idEntidadeSQL

                   
                    ViewBag.idLocalEntidade = new SelectList(locaisEntidadeAtual, "idLocalEntidade", "titulo");
                    var avaliadoresEntidadeELE = from a in db.Avaliador where a.Avaliador_Entidade.Any(x => x.idEntidade == idEntidadeSQL) && (a.tipoAvaliador == "01" || a.tipoAvaliador == "03") select a;
                    var avaliadoresEntidadeSME = from a in db.Avaliador where a.Avaliador_Entidade.Any(x => x.idEntidade == idEntidadeSQL) && (a.tipoAvaliador == "02" || a.tipoAvaliador == "03") select a;
                    ViewBag.idAvaliador1 = new SelectList(avaliadoresEntidadeELE, "id", "UserPessoa.Nome");
                    ViewBag.idAvaliador2 = new SelectList(avaliadoresEntidadeSME, "id", "UserPessoa.Nome");
                    return View(horario);
                }
                else //CASO A DIFERENÇA DE HORÁRIO SEJA MAIOR QUE 30 MINUTOS
                {
                    return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "Não é possível começar uma prova com diferença maior que 30 minutos do horário agendado" });
                }
            }
            else { //CASO O HORÁRIO NÃO PERTENÇA À INSTITUIÇÃO OU NÃO ESTEJA CONFIRMADO COM UM PILOTO
                                
                    return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "Horário inválido" });
                }

            }

        [HttpPost]
        public ActionResult IniciarAvaliacao([Bind(Include = "idHorario")]Horario horario, int? idAvaliador1, int? idAvaliador2)
        {
            horario = db.Horario.Find(horario.idHorario);
            int idEntidadeSQL;
            idEntidadeSQL = User.Identity.GetUserId<int>();
            Entidade entidade = db.Entidade.Find(idEntidadeSQL);
            var locaisEntidadeAtual = from l in db.LocalEntidade where l.idEntidade == idEntidadeSQL select l; //"SELECT * FROM localEntidade WHERE idEntidade =" + idEntidadeSQL
            ViewBag.idLocalEntidade = new SelectList(locaisEntidadeAtual, "idLocalEntidade", "titulo");
            var avaliadoresEntidadeELE = from a in db.Avaliador where a.Avaliador_Entidade.Any(x => x.idEntidade == idEntidadeSQL) && (a.tipoAvaliador == "01" || a.tipoAvaliador == "03") select a;
            var avaliadoresEntidadeSME = from a in db.Avaliador where a.Avaliador_Entidade.Any(x => x.idEntidade == idEntidadeSQL) && (a.tipoAvaliador == "02" || a.tipoAvaliador == "03") select a;
            ViewBag.idAvaliador1 = new SelectList(avaliadoresEntidadeELE, "id", "UserPessoa.Nome");
            ViewBag.idAvaliador2 = new SelectList(avaliadoresEntidadeSME, "id", "UserPessoa.Nome");
            Avaliador avaliadorELE = db.Avaliador.Find(idAvaliador1);
            Avaliador avaliadorSME = db.Avaliador.Find(idAvaliador2);
            if (avaliadorELE == null || avaliadorSME == null)
            {
                ViewBag.StatusMessage = "Por favor, selecione os dois avaliadores";
                return View(horario);
            }
            if (idAvaliador1 == idAvaliador2)
            {
                ViewBag.StatusMessage = "Um mesmo avaliador não pode ocupar as duas posições";
                return View(horario);
            }
            TimeSpan TrintaMinutos = new TimeSpan(0, 0, 30, 0);
            if (horario.data.Value.Add(TrintaMinutos) >= DateTime.Now && DateTime.Now.Add(TrintaMinutos) >= horario.data // Confere se há menos de 30 minutos de diferença entre a hora atual e a hora agendada
                && horario.data.Value.Date == DateTime.Now.Date //Confere se o dia está certo
                )
            {

                horario.ele = avaliadorELE;
                horario.sme = avaliadorSME;
                horario.status = 5;
                horario.Piloto.agendamentoAtivo = false;
                db.Entry(horario).State = EntityState.Modified;
                db.SaveChanges();              
                
                return RedirectToAction("AvaliacaoPronta", new { id = horario.idHorario });
            }else
            {
                
                return RedirectToAction("Agendamentos","Horarios", new { StatusMessage = "Tempo limite excedido, por favor crie um novo agendamento, a tolerância é de 30 minutos" });
            }
        }
        [Authorize(Roles ="Piloto")]
        public ActionResult Painel(int? idEntidade, bool? disponiveis)
        //********************************************************************************************//
        // EXIBE O PAINEL DE AGENDAMENTO PARA OS PILOTOS                                              //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            var entidadesArray = from e in db.Entidade select e;
            ViewBag.entidades = entidadesArray.ToList();
            ViewBag.numeroEntidades = entidadesArray.Count();
            var horario = db.Horario.Include(h => h.LocalEntidade).Include(h => h.Piloto);
            DateTime dataHoje = DateTime.Now;
            horario = from h in horario where h.data >= dataHoje orderby h.data select h;

            //MUDA A PROPRIEDADE DO ULTIMO AGENDAMENTO CRIADO PARA A PÁGINA "PAINEL"

            ConfigSySDEA config = db.ConfigSySDEA.Find(1);
            if (config != null)
            {
                ViewBag.LastModified = config.lastModified.ToShortDateString() + " " + config.lastModified.ToShortTimeString();
            }
            DateTime agoraMenos30 = DateTime.Now.AddMinutes(-30);
            List<Horario> indisponiveis = (from hi in db.Horario where hi.data < agoraMenos30 && hi.status != 6 select hi).ToList();

            for (int j = 0; j < indisponiveis.Count(); j++)
            {
                Horario horarioExpirado = indisponiveis.ElementAt(j);
                horarioExpirado.status = 6;
                db.Entry(horarioExpirado).State = EntityState.Modified;                
                    db.SaveChanges();                               
            }
            //horario.OrderBy(h => h.data);
            if (disponiveis == true)
            {
                horario = from h in horario where h.status == 1 select h;
            }
            if (idEntidade != null)
            {
                
               
                horario = from h in horario orderby h.data select h ;
                horario = horario.Where(h => h.LocalEntidade.idEntidade == idEntidade);
                if (disponiveis == true)
                {
                    horario = from h in horario where h.status == 1 select h;
                }
                return View(horario.ToList());
            }

             
            
            return View(horario.ToList());
        }

        [HttpPost]
        
        public ActionResult Painel2([Bind(Include = "data")] Horario horario)
        //********************************************************************************************//
        //   PAINEL GRANDE COM TODAS INSTITUIÇÕES E HORARIOS DE TODA A SEMANA                         //
        //                                 [EM CONSTRUÇÃO]                                            //
        //                                                                                            //
        //********************************************************************************************//

        {
            DateTime dataBusca = DateTime.Parse(horario.data.ToString());
            DayOfWeek diaSemana = dataBusca.DayOfWeek;
            
            int diasMais = 0, diasMenos = 0;
            if (diaSemana == DayOfWeek.Monday)
            {
                diasMais = 4;
                diasMenos = 0;
                //horarios = from h in db.Horario where h.data <= dataBusca.AddDays(4) && h.data >= dataBusca select h;
            }else if (diaSemana == DayOfWeek.Tuesday)
            {
                diasMais = 3;
                diasMenos = 1;
                //horarios = from h in db.Horario where h.data <= dataBusca.AddDays(3) && h.data >= dataBusca.AddDays(-1) select h;
            }else if(diaSemana == DayOfWeek.Wednesday)
            {
                diasMais = 2;
                diasMenos = 2;
               // horarios = from h in db.Horario where h.data <= dataBusca.AddDays(2) && h.data >= dataBusca.AddDays(-2) select h;
            }else if (diaSemana == DayOfWeek.Thursday)
            {
                diasMais = 1;
                diasMenos = 3;
                //horarios = from h in db.Horario where h.data <= dataBusca.AddDays(1) && h.data >= dataBusca.AddDays(-3) select h;
            }else if(diaSemana == DayOfWeek.Friday)
            {
                diasMais = 0;
                diasMenos = 4;
                //horarios = from h in db.Horario where h.data <= dataBusca && h.data >= dataBusca.AddDays(-4) select h;
            }
            List<List<List<Horario>>> horariosPorLocal = new List<List<List<Horario>>>();
            var locaisEntidades = from e in db.LocalEntidade select e;
            for (int i=0; i < db.Entidade.Count(); i++)
            {
                LocalEntidade localEntidade = locaisEntidades.ToList().ElementAt(i);
                var horarioEntidade = from h in db.Horario where h.LocalEntidade.idLocalEntidade ==  localEntidade.idLocalEntidade select h;
               // horariosPorLocal.Add(horarioEntidade.ToList());
                //var hor = from h in db.Horario where Entidade = 
                for (int j = 0; j < 5; j++) {
                    DateTime databuscaMais = dataBusca.AddDays(diasMais - j);
                    DateTime databuscaMenos = dataBusca.AddDays(-(diasMenos - j));
                    //var horLocal = from h in horarioEntidade where h.data >= databuscaMenos && h.data <= databuscaMais select h ;
                    var horLocalDia = from h in horarioEntidade where h.data == dataBusca select h;
                    if (horLocalDia.Count() > 0)
                    {
                        horariosPorLocal[i] = new List<List<Horario>>();
                        horariosPorLocal[i][j] = (horLocalDia.ToList());
                    }
                        }
            }
            ViewBag.locaisEntidades = locaisEntidades.ToList();
            return View(horariosPorLocal.ToList());
        }



        // GET: Horarios/Details/5
        [Authorize(Roles ="Fiscal,Entidade, Admin")]
        public ActionResult Detalhes(int? id)
        //********************************************************************************************//
        //   EXIBE DETALHES DO HORÁRIO SELECIONADO                                                    //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horario.Find(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            return View(horario);
        }
        [Authorize(Roles = "Entidade")]
        public ActionResult DetalheSolicitacao(int? id)
        //********************************************************************************************//
        //   EXIBE A SOLICITAÇÃO SELECIONADA                                                          //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SolicitacaoHorario solicitacao = db.SolicitacaoHorario.Find(id);
            if (solicitacao == null)
            {
                return HttpNotFound();
            }
            return View(solicitacao);
        }
        // GET: Entidades/DetalheHorario/5
        [Authorize(Roles ="Piloto")]
        public ActionResult DetalheHorario(int? id)
        //********************************************************************************************//
        //   EXIBE OS DETALHES DO HORÁRIO SELECIONADO(PARA PILOTOS)                                   //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horario.Find(id);
            /*if(horario.Piloto != null)
            {
                return RedirectToAction("HorarioOcupado");
            }*/
            if (horario == null)
            {
                return HttpNotFound();
            }
            return View(horario);
        }
        public ActionResult HorarioOcupado()
        //********************************************************************************************//
        //EXIBE UMA TELA QUE INFORMA QUE O HORÁRIO SELECIONADO JÁ ESTÁ OCUPADO                        //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {

            return View();
        }
        [Authorize(Roles = "Piloto")]
        public async System.Threading.Tasks.Task<ActionResult> Agendar(int? id)
        //********************************************************************************************//
        //  PILOTO RESERVA O HORÁRIO DISPONÍVEL DESEJADO                                              //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            int pilotoID = User.Identity.GetUserId<int>();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horario.Find(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            if (horario.Piloto != null)
            {
                return RedirectToAction("HorarioOcupado");
            }
            Piloto piloto = db.Piloto.Find(pilotoID);
            ViewBag.resultado = "Piloto já agendado em outro horário";
            if (piloto == null)
            {
                return HttpNotFound();
            }
            if (piloto.agendamentoAtivo == false)
            {
                TimeSpan hora = TimeSpan.Parse(horario.data.ToString());
                horario.status = 2;
                horario.Piloto = piloto;
                horario.idPiloto = piloto.Id;
                db.Entry(horario).State = EntityState.Modified;
                piloto.agendamentoAtivo = true;
                db.Entry(piloto).State = EntityState.Modified;
                

                var body = "<p> {0} </p><p>{1}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(piloto.UserPessoa.Email)); 
                string corpoEmail = "Horario Agendado <br>" + "Entidade:" + horario.LocalEntidade.Entidade.nome + "<br> Filial:" + horario.LocalEntidade.titulo + "<br> Data:" + horario.data.Value.ToShortDateString()+"<br>Hora:"+hora.ToString(@"hh\:mm");
                message.Subject = "Agendamento SDEA";
                message.Body = string.Format(body, "Sistema SySDEA", corpoEmail);
                message.IsBodyHtml = true;
                EmailSySDEA emailSySDEA = new EmailSySDEA(message);
                db.EmailSySDEA.Add(emailSySDEA);
                db.SaveChanges();
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "sistema.sysdea@anac.gov.br",  // 
                        Password = "qiuEUvZBW7vGAZm9PseK"  // 
                    };
                    smtp.Credentials = credential;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    await smtp.SendMailAsync(message);

                    ViewBag.resultado = "Agendamento concluído";
                }

                return View();

            }
            return View();
        }
        [Authorize(Roles = "Entidade")]
        public ActionResult CriarHorarioFechado()
        //********************************************************************************************//
        //    ENTIDADE CRIA UM HORÁRIO JÁ COM UM PILOTO RESERVADO                                     //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            int idEntidadeSQL;
            idEntidadeSQL = User.Identity.GetUserId<int>();
            Entidade entidade = db.Entidade.Find(idEntidadeSQL);
            var locaisEntidadeAtual = from l in db.LocalEntidade where l.idEntidade == idEntidadeSQL select l; //"SELECT * FROM localEntidade WHERE idEntidade =" + idEntidadeSQL

            var maxSalas = db.LocalEntidade.Max(x => x.numeroSalas);
            var numSalas = db.LocalEntidade.First(x => x.numeroSalas == maxSalas).numeroSalas;
            ViewBag.idLocalEntidade = new SelectList(locaisEntidadeAtual, "idLocalEntidade", "titulo");

            var salas = new List<int>();
            for (int i = 1; i <= numSalas; i++)
                salas.Add(i);
            ViewBag.sala = new SelectList(salas, "sala");
            


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarHorarioFechado([Bind(Include = "idHorario,preco,data,hora,idLocalEntidade")] Horario horario, string sala, string canacPiloto)
        {
            if (ModelState.IsValid)
            {
                int canacPilotoInt = int.Parse(canacPiloto);
                try
                {
                    var pilotoBuscado = from p in db.Piloto where p.CANACPiloto == canacPilotoInt select p;
                    Piloto piloto = pilotoBuscado.First();
                    if (piloto.agendamentoAtivo == false)
                    {
                        horario.Piloto = piloto;
                    }
                    else
                    {
                        ViewBag.erro = "Piloto já agendado em outro horário";
                        return CriarHorarioFechado();
                    }
                }
                catch (Exception)
                {
                    ViewBag.erro = "Canac inválido";
                    return CriarHorarioFechado();
                }
                
                horario.sala = int.Parse(sala);
                horario.status = 2;
                db.Horario.Add(horario);
                db.SaveChanges();
                return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "Horario Criado com sucesso" });
            }
            ViewBag.idLocalEntidade = new SelectList(db.LocalEntidade, "idLocalEntidade", "titulo", horario.idLocalEntidade);

            return View(horario);
        }

        public ActionResult ConfirmarAgendamento(int? id)
        //********************************************************************************************//
        //  ENTIDADE ALTERA O STATUS DE UM AGENDAMENTO DE RESERVADO PARA CONFIRMADO                   //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horario.Find(id);
            if (horario == null)
            {
                return HttpNotFound();
            } else if (horario.Piloto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(horario);
        }

        [HttpPost, ActionName("ConfirmarAgendamento")]
        public ActionResult ConfirmarAgendamentoPost(int id)
        {
            Horario horario = db.Horario.Find(id);
            //CONNECTION STRING
            //Provider =Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\leandro.campos\Documents\access\Dataling.mdb 
            /*string connectionString = @"Microsoft.Jet.OLEDB.4.0; Data Source = C:\Users\leandro.campos\Documents\access\Dataling.mdb";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                command.CommandText = "select versao_prova from Avaliacao where id_canac_piloto="+horario.Piloto.CANACPiloto.ToString();
                
                SqlDataReader reader = command.ExecuteReader();               
                var versao_prova = reader["versao_prova"].ToString();
            }

            */
            
            horario.status = 3;
            db.Entry(horario).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "Horário confirmado com sucesso" });
        }

        [Authorize(Roles ="Entidade")]
        public ActionResult CancelarAgendamentoConfirmado(int? id)
        //********************************************************************************************//
        //  INICIA O CANCELAMENTO DE UM AGENDAMENTO JÁ CONFIRMADO PELA ENTIDADE E AVISA O PILOTO      //
        //  PILOTO PRECISA CONFIRMAR O CANCELAMENTO AO ENTRAR NO SISTEMA SYSDEA                       //
        //                                                                                            //
        //********************************************************************************************//
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horario.Find(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            return View(horario);
        }

        [HttpPost]
        [ActionName("CancelarAgendamentoConfirmado")]
        public async System.Threading.Tasks.Task<ActionResult> CancelarAgendamentoConfirmadoPost(int id, string justificativa)

        {
            int entidadeId = User.Identity.GetUserId<int>();
            Entidade entidade = db.Entidade.Find(entidadeId);
            Horario horario = db.Horario.Find(id);
            if (entidade.tipoEntidade == 2)
            {


                horario.status = 1;
                horario.Piloto.agendamentoAtivo = false;
                db.Entry(horario.Piloto).State = EntityState.Modified;
                horario.Piloto = null;
                horario.idPiloto = null;
                db.Entry(horario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "Agendamento cancelado com sucesso" });
            }
            horario.status = 4;
            db.Entry(horario).State = EntityState.Modified;

            MailMessage message = new MailMessage();
            message.Body = "Prezado, " + horario.Piloto.UserPessoa.nome + "," + "<br/>" + "A instituição " + horario.LocalEntidade.Entidade.nome + " solicitou o cancelamento de seu agendamento, por favor entre no SySDEA e confirme o cancelamento ou contate a instituição para esclarecimentos"+"<br/>"+"Justificativa:"+justificativa;
            message.To.Add(horario.Piloto.UserPessoa.Email);
            message.Subject = "Cancelamento de horário confirmado";
            message.IsBodyHtml = true;
            EmailSySDEA emailSySDEA = new EmailSySDEA(message);
            emailSySDEA.tipoOrigem = "Cancelamento de horário confirmado";

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "sistema.sysdea@anac.gov.br",  // 
                    Password = "qiuEUvZBW7vGAZm9PseK"  // 
                };
                smtp.Credentials = credential;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);

            }
                db.EmailSySDEA.Add(emailSySDEA);
                db.SaveChanges();
                return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "O piloto precisa confirmar o cancelamento" });
            }


        // GET: Horarios/Create
        [Authorize(Roles = "Entidade")]
        public ActionResult Criar()
        //********************************************************************************************//
        //  CRIA UM HORÁRIO DISPONÍVEL PARA AGENDAMENTO                                               //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            int idEntidadeSQL;
            idEntidadeSQL = User.Identity.GetUserId<int>();
            Entidade entidade = db.Entidade.Find(idEntidadeSQL);
            var locaisEntidadeAtual = from l in db.LocalEntidade where l.idEntidade == idEntidadeSQL select l; //"SELECT * FROM localEntidade WHERE idEntidade =" + idEntidadeSQL

            var maxSalas = locaisEntidadeAtual.Max(x => x.numeroSalas);
            var numSalas = locaisEntidadeAtual.First(x => x.numeroSalas == maxSalas).numeroSalas;
            ViewBag.idLocalEntidade = new SelectList(locaisEntidadeAtual, "idLocalEntidade", "titulo");
            var avaliadoresEntidadeELE = from a in db.Avaliador where a.Avaliador_Entidade.Any(x => x.idEntidade == idEntidadeSQL) && (a.tipoAvaliador == "01" || a.tipoAvaliador =="03") select a;
            var avaliadoresEntidadeSME = from a in db.Avaliador where a.Avaliador_Entidade.Any(x => x.idEntidade == idEntidadeSQL) && (a.tipoAvaliador == "02" || a.tipoAvaliador == "03") select a;
            ViewBag.idAvaliador1 = new SelectList(avaliadoresEntidadeELE, "id", "UserPessoa.Nome");
            ViewBag.idAvaliador2 = new SelectList(avaliadoresEntidadeSME, "id", "UserPessoa.Nome");
            //ViewBag.idAvaliador.Add(semAvaliador); 

            //ViewBag.idAvaliador.Insert(0,"-Sem avaliador-");
            var salas = new List<int>();
            for (int i = 1; i <= numSalas; i++)
                salas.Add(i);
            ViewBag.sala = new SelectList(salas, "sala");
            
            return View();
        }

        // POST: Horarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Criar([Bind(Include = "idHorario,preco,data,hora,idLocalEntidade")]string idAvaliador1, string idAvaliador2, CriarHorarioViewModel horario2, string sala)
        {
           
            int idEntidadeSQL;
            idEntidadeSQL = User.Identity.GetUserId<int>();
            Entidade entidade = db.Entidade.Find(idEntidadeSQL);
            var locaisEntidadeAtual = from l in db.LocalEntidade where l.idEntidade == idEntidadeSQL select l; //"SELECT * FROM localEntidade WHERE idEntidade =" + idEntidadeSQL

            var maxSalas = locaisEntidadeAtual.Max(x => x.numeroSalas);
            var numSalas = locaisEntidadeAtual.First(x => x.numeroSalas == maxSalas).numeroSalas;
            ViewBag.idLocalEntidade = new SelectList(locaisEntidadeAtual, "idLocalEntidade", "titulo");
            //ViewBag.idLocalEntidade = new SelectList(db.LocalEntidade, "idLocalEntidade", "titulo", horario.idLocalEntidade);
            var salas = new List<int>();
            for (int i = 1; i <= numSalas; i++)
                salas.Add(i);
            var avaliadoresEntidade = from a in db.Avaliador where a.Avaliador_Entidade.Any(x => x.idEntidade == idEntidadeSQL) select a;
            
            ViewBag.idAvaliador1 = new SelectList(avaliadoresEntidade, "id", "UserPessoa.Nome");
            ViewBag.idAvaliador2 = new SelectList(avaliadoresEntidade, "id", "UserPessoa.Nome");
            ViewBag.sala = new SelectList(salas, "sala");
            if (horario2.data == DateTime.Now.Date && horario2.hora <= DateTime.Now.TimeOfDay)
            {
                ViewBag.StatusMessage = "Horários não podem ser criados antes da data e hora atual";
                return View();
            }
            if (idAvaliador1 == idAvaliador2 && idAvaliador1 != "")
            {

                ViewBag.MsgAvaliadoresIguais = "Avaliadores devem ser diferentes";
                return View(horario2);
            }
            Horario horario = new Horario(horario2);
            
            if (ModelState.IsValid)
            {
                
                if (idAvaliador1 != "")
                {
                    Avaliador avaliador1 = db.Avaliador.Find(int.Parse(idAvaliador1));
                    horario.ele = avaliador1;
                }
                if (idAvaliador2 != "")
                {
                    Avaliador avaliador2 = db.Avaliador.Find(int.Parse(idAvaliador2));
                    horario.sme = avaliador2;
                }
                horario.sala = int.Parse(sala);
                horario.status = 1;
                ConfigSySDEA config = db.ConfigSySDEA.Find(1);
                if(config == null)
                {
                    config = new ConfigSySDEA() { Id = 1 };
                }
                config.lastModified = DateTime.Now;
                db.Entry(config).State = EntityState.Modified;
                db.Horario.Add(horario);
                db.SaveChanges();
                return RedirectToAction("PainelUsuario","Manage", new { StatusMessage = "Horario Criado com sucesso" });
            }                       
            return View(horario2);
        }

        [Authorize(Roles ="Piloto")]
        public ActionResult ConsultarAgendamentoAtivo()
        //********************************************************************************************//
        //  VERIFICA O ULTIMO AGENDAMENTO DO PILOTO CASO ESTE AINDA ESTEJA ATIVO                      //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            int idPiloto = User.Identity.GetUserId<int>();
            Piloto piloto = db.Piloto.Find(idPiloto);
            if (piloto.agendamentoAtivo == false)
            {
                return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "Você não possui um agendamento ativo" });
            }
            Horario horario = piloto.Horario.Last();
            if(horario.status == 1) { 
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }else if(horario.status == 2 || horario.status == 3)
            {
                return View(horario);
            }else if(horario.status == 4)
            {
                return RedirectToAction("PilotoConfirmaCancelamento");
            }                                   
            return View(horario);
        }

        public ActionResult PilotoCancelaAgendamento(int? id)
        //********************************************************************************************//
        //  PILOTO CANCELA SEU AGENDAMENTO RESERVADO OU SOLICITA O CANCELAMENTO CASO ESTEJA           //
        //  CONFIRMADO                                                                                //
        //                                                                                            //
        //********************************************************************************************//
        {
            Horario horario = db.Horario.Find(id);
            if (horario.status == 2)
            {
                return View("CancelarAgendamentoReservado", horario);
            }
            else if (horario.status == 3)
            {
                return RedirectToAction("PilotoSolicitaCancelamento");
            }
            else return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [Authorize(Roles ="Piloto")]
        public ActionResult PilotoConfirmaCancelamento()
        //********************************************************************************************//
        //  PILOTO ENTRA NO SISTEMA E CONFIRMA CIÊNCIA DO CANCELAMENTO COMEÇADO PELA ENTIDADE         //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            int idPiloto = User.Identity.GetUserId<int>();
            Piloto piloto = db.Piloto.Find(idPiloto);
            Horario horario = piloto.Horario.Last();
            if (horario.status == 4)
            {
                return View(horario);
            }
            
            return RedirectToAction("PainelUsuario", "Manage");        }
        [HttpPost]
        public ActionResult PilotoConfirmaCancelamento(int idHorario)
        {
            Horario horario = db.Horario.Find(idHorario);
            horario.Piloto.agendamentoAtivo = false;
            db.Entry(horario.Piloto).State = EntityState.Modified;
            horario.Piloto = null;
            horario.idPiloto = null;
            horario.status = 1;
            db.Entry(horario).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "Horário cancelado com sucesso" });
        }


        [Authorize(Roles = "Piloto, Entidade, Fiscal, Admin, Avaliador")]
        public ActionResult BuscaHorario()
        //********************************************************************************************//
        //   BUSCA UM HORÁRIO POR DATA OU LOCAL DE PROVA                                              //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {

            ViewBag.idLocalEntidade = from l in db.LocalEntidade select l;
            return View();
        }



        [HttpPost, ActionName("ConsultaPorData")]
        
        public ActionResult ConsultaPorData([Bind(Include = "data")] Horario horario)
        //********************************************************************************************//
        //    EXIBE TODOS OS HORÁRIOS PARA UMA DATA ESCOLHIDA                                         //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            if (ModelState.IsValid)
            {
                DateTime? dataBusca = horario.data.Value.Date;
                var horariosQuery = from h in db.Horario where h.data.Value.Date == dataBusca select h;
                List<Horario> horariosFiltrados = new List<Horario>();
                horariosFiltrados = horariosQuery.ToList();
                if (horariosFiltrados.Count() > 0)
                {
                    return View("HorariosData", horariosFiltrados.ToList());
                }
            }
            return RedirectToAction("BuscaHorario");
        }
        [HttpPost, ActionName("ConsultaPorEntidade")]

        public ActionResult ConsultaPorEntidade(int idLocalEntidade)
        //********************************************************************************************//
        //    EXIBE TODOS OS HORÁRIOS DE UM DETERMINADO LOCAL DE PROVA                                //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {

            if (ModelState.IsValid)
            {
                var horariosFiltrados = from h in db.Horario where h.idLocalEntidade == idLocalEntidade  select h;
                if (horariosFiltrados.Count() > 0)
                {
                    return View("HorariosEntidade", horariosFiltrados.ToList());
                }
            }
            return View("BuscaHorario");
        }
        // GET: Horarios/Edit/5
        [Authorize(Roles ="Entidade")]
        public ActionResult Alterar(int? id)
        //********************************************************************************************//
        //  ALTERA INFORMAÇÕES DE UM HORÁRIO                                                          //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horario.Find(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            ViewBag.idLocalEntidade = new SelectList(db.LocalEntidade, "idLocalEntidade", "Titulo", horario.idLocalEntidade);
            ViewBag.Piloto_id = new SelectList(db.Piloto, "Id", "nome", horario.idPiloto);
            CriarHorarioViewModel horario2 = new CriarHorarioViewModel(horario);
            return View(horario2);
        }

        // POST: Horarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alterar([Bind(Include = "idHorario,Piloto_id,preco,data,hora,status,sala,idLocalEntidade")] CriarHorarioViewModel horario2)
        {
            Horario horario = new Horario(horario2);
            if (ModelState.IsValid)
            {                
                db.Entry(horario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { StatusMessage = "Horário alterado com sucesso" });
            }
            ViewBag.idLocalEntidade = new SelectList(db.LocalEntidade, "idLocalEntidade", "rua", horario.idLocalEntidade);
            ViewBag.Piloto_id = new SelectList(db.Piloto, "Id", "nome", horario.idPiloto);
            return View(horario2);
        }

        // GET: Horarios/Delete/5
        [Authorize(Roles ="Entidade")]
        public ActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horario.Find(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            return View(horario);
        }



        // POST: Horarios/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirConfirmado(int id)
        {
            Horario horario = db.Horario.Find(id);
            db.Horario.Remove(horario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult AvaliacaoPronta(int? id)
        {
            Horario horario = db.Horario.Find(id);
            return View(horario);
        }

        //GET: Horario/solicitarHorario
        //Piloto solicita um horário diferente dos agendados
        [Authorize(Roles = "Piloto")]
        public ActionResult solicitaHorario()
        {
            int idPiloto = User.Identity.GetUserId<int>();
            Piloto piloto = db.Piloto.Find(idPiloto);
            if(piloto.agendamentoAtivo == true)
            {
                return RedirectToAction("PainelUsuario","Manage", new { StatusMessage = "Piloto já agendado em outro horário, favor cancelar o agendamento antes de solicitar um horário" });
            }
            var locaisEntidades = from l in db.LocalEntidade where l.aceitaSolicitacoes == true select l;
            ViewBag.idLocalEntidade = new SelectList(locaisEntidades, "idLocalEntidade", "titulo");
            SolicitacaoHorarioViewModel solicita = new SolicitacaoHorarioViewModel();
            return View();
        }
        [Authorize(Roles ="Piloto")]
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> solicitaHorario(SolicitacaoHorarioViewModel solicita)
        {

            DateTime dataHora1 = solicita.dia + solicita.hora1;

            if (dataHora1 < DateTime.Now)
            {
                var locaisEntidades = from l in db.LocalEntidade where l.aceitaSolicitacoes == true select l;
                ViewBag.idLocalEntidade = new SelectList(locaisEntidades, "idLocalEntidade", "titulo");
                ViewBag.StatusMessage = "A Data/Hora escolhida não deve ser anterior à Data/Hora atual";
                return View(solicita);
            }
            if (solicita.hora2 != null)
            {
                DateTime dataHora2 = solicita.dia + solicita.hora2.Value;
                if(dataHora2 < DateTime.Now)
                {
                    var locaisEntidades = from l in db.LocalEntidade where l.aceitaSolicitacoes == true select l;
                    ViewBag.idLocalEntidade = new SelectList(locaisEntidades, "idLocalEntidade", "titulo");
                    ViewBag.StatusMessage = "A Data/Hora escolhida não deve ser anterior à Data/Hora atual";
                    return View(solicita);
                }
            }
            if (solicita.hora3 != null)
            {
                DateTime dataHora3 = solicita.dia + solicita.hora3.Value;
                if (dataHora3 < DateTime.Now)
                {
                    var locaisEntidades = from l in db.LocalEntidade where l.aceitaSolicitacoes == true select l;
                    ViewBag.idLocalEntidade = new SelectList(locaisEntidades, "idLocalEntidade", "titulo");
                    ViewBag.StatusMessage = "A Data/Hora escolhida não deve ser anterior à Data/Hora atual";
                    return View(solicita);
                }
            }
            
                if (ModelState.IsValid)
            {
                int idPiloto = User.Identity.GetUserId<int>();
                EmailSySDEA emailSySDEA = new EmailSySDEA();
                emailSySDEA.horaEnvio = DateTime.Now;
                emailSySDEA.assunto = "SySDEA: Solicitação de horário";
                LocalEntidade localEntidade = db.LocalEntidade.Find(solicita.idLocalEntidade);
                Piloto piloto = db.Piloto.Find(idPiloto);
                emailSySDEA.destino = localEntidade.Entidade.EmailEntidade;
                emailSySDEA.tipoOrigem = "Solicitacao de Horario";
                SolicitacaoHorario solicitacao = new SolicitacaoHorario();
                solicitacao.idLocalEntidade = solicita.idLocalEntidade;
                solicitacao.Piloto = piloto;
                solicitacao.aberta = true;
                solicitacao.hora1 = solicita.dia.Add(solicita.hora1);
                if (solicita.hora2 != null)
                {
                    solicitacao.hora2 = solicita.dia.Add(solicita.hora2.Value);
                }
                if (solicita.hora3 != null)
                {
                    solicitacao.hora3 = solicita.dia.Add(solicita.hora2.Value);
                }
                db.SolicitacaoHorario.Add(solicitacao);

                var body = "Nova solicitação de horário do piloto: " + piloto.UserPessoa.nome + "<br/>" + " Dia desejado:" + solicita.dia.ToShortDateString() + "<br/>" + " Horários desejados: " + solicita.hora1 + "  " + solicita.hora2 + "  " + solicita.hora3 + "<br/>" + solicita.observacoes;
                var message = new MailMessage();
                message.To.Add(new MailAddress(localEntidade.emailContato));

                message.Subject = emailSySDEA.assunto;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "sistema.sysdea@anac.gov.br",
                        Password = "qiuEUvZBW7vGAZm9PseK"
                    };
                    smtp.Credentials = credential;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    await smtp.SendMailAsync(message);

                    emailSySDEA.textoEmail = message.Body;
                    

                    db.EmailSySDEA.Add(emailSySDEA);
                    db.SaveChanges();
                    return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "Solicitação enviada com sucesso" });
                }
            }
            else
            {
                var locaisEntidades = from l in db.LocalEntidade where l.aceitaSolicitacoes == true select l;
                ViewBag.idLocalEntidade = new SelectList(locaisEntidades, "idLocalEntidade", "titulo");
                return View();
            }
       }
        [Authorize(Roles = "Entidade")]
        public async System.Threading.Tasks.Task<ActionResult> verificaSolicitacoes()
        {

            int idEntidade = User.Identity.GetUserId<int>();

            var solicitacoes = (from s in db.SolicitacaoHorario where s.LocalEntidade.idEntidade == idEntidade && s.aberta == true select s).ToList();
            for (int i = 0; i < solicitacoes.Count(); i++)
            {
                SolicitacaoHorario solicitacao = solicitacoes.ElementAt(i);
                if (solicitacao.hora1 < DateTime.Now && solicitacao.hora2 < DateTime.Now && solicitacao.hora3.Value < DateTime.Now)
                {
                    solicitacao.aberta = false;
                    solicitacao.aceita = false;

                    MailMessage message = new MailMessage();
                    message.Subject = "Aviso de solicitação expirada";
                    message.To.Add(solicitacao.Piloto.UserPessoa.Email);
                    message.Body = "Prezado" + solicitacao.Piloto.UserPessoa.nome + "," + "<br/>" + "< p > Sua solicitação de abertura de horário expirou sem nenhuma ação tomada pela entidade " + solicitacao.LocalEntidade.titulo + ", volte ao sistema para realizar uma nova solicitação ou escolher um horário disponível no painel </ p > ";
                    message.IsBodyHtml = true;
                    EmailSySDEA email = new EmailSySDEA(message);
                    email.destino = solicitacao.Piloto.UserPessoa.Email;
                    email.tipoOrigem = "Solicitação Expirada";
                    using (var smtp = new SmtpClient())
                    {
                        smtp.UseDefaultCredentials = false;
                        var credential = new NetworkCredential
                        {
                            UserName = "sistema.sysdea@anac.gov.br",  // Email usado para envio da mensagem
                            Password = "qiuEUvZBW7vGAZm9PseK"  // senha para autenticação no servidor de emails
                        };
                        smtp.Credentials = credential;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        await smtp.SendMailAsync(message);

                    }
                    db.Entry(solicitacao).State = EntityState.Modified;
                    db.EmailSySDEA.Add(email);
                    db.SaveChanges();


                }


            }
            solicitacoes = (from s in db.SolicitacaoHorario where s.LocalEntidade.idEntidade == idEntidade && s.aberta == true select s).ToList();
            return View(solicitacoes.ToList());
        }

    }
}
