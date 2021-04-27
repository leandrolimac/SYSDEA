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

namespace SySDEAProject.Controllers
{
    public class AvaliacoesController : Controller
    {
        private SySDEAContext db = new SySDEAContext();

        // GET: Avaliacoes
        public ActionResult Index()
        {
            var avaliacao = db.Avaliacao.Include(a => a.Horario).Include(a => a.LocalEntidade).Include(a => a.VersaoProva);
            return View(avaliacao.ToList());
        }

        // GET: Avaliacoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = db.Avaliacao.Find(id);
            if (avaliacao == null)
            {
                return HttpNotFound();
            }
            return View(avaliacao);
        }

        // GET: Avaliacoes/Create
        public ActionResult Create()
        {
            ViewBag.idHorario = new SelectList(db.Horario, "idHorario", "idHorario");
            ViewBag.idLocalEntidade = new SelectList(db.LocalEntidade, "idLocalEntidade", "emailContato");
            //ViewBag.idProcesso = new SelectList(db.Processo, "idProcesso", "providenciasExtras");
            ViewBag.idVersaoProva = new SelectList(db.VersaoProva, "idVersaoProva", "idVersaoProva");
            return View();
        }

        // POST: Avaliacoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idAvaliacao,idLocalEntidade,idVersaoProva,observacoes,dthrProvaInicio,dthrProvaFim,dthrEnvioDados,possuiRecurso,sucesso")] Avaliacao avaliacao)
        {
            if (ModelState.IsValid)
            {
                db.Avaliacao.Add(avaliacao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.idLocalEntidade = new SelectList(db.LocalEntidade, "idLocalEntidade", "emailContato", avaliacao.idLocalEntidade);
            //ViewBag.idProcesso = new SelectList(db.Processo, "idProcesso", "providenciasExtras", avaliacao.idProcesso);
            ViewBag.idVersaoProva = new SelectList(db.VersaoProva, "idVersaoProva", "idVersaoProva", avaliacao.idVersaoProva);
            return View(avaliacao);
        }
        
       [Authorize(Roles ="Avaliador")]
        public ActionResult IniciarAvaliacao(int Id)
        {               
            
            int idAvaliador = User.Identity.GetUserId<int>();
            Avaliador avaliador = db.Avaliador.Find(idAvaliador);
            Horario horario = db.Horario.Find(Id);
            if(horario.status == 7)
            {
                Avaliacao avaliacao = new Avaliacao(horario);
                return View(avaliacao);
            }
            else
            {
                return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "Avaliação Indisponível" });
            }                        
        }


        [HttpPost]
        [Authorize(Roles = "Avaliador")]
        public ActionResult IniciarAvaliacao(Avaliacao avaliacao)
        {            
            avaliacao.dthrProvaInicio = DateTime.Now;
            avaliacao.status = 1;
            //avaliacao.idVersaoProva = 1;
            Historico HistoricoPiloto = db.Historico.Find(avaliacao.idPiloto);
            var versoesOriginaisAtivas = from vOA in db.VersaoOriginal where vOA.ativa == true select vOA;
            List < int > numerosVersoesFeitas = new List<int>();
            for (int i = 0; i == HistoricoPiloto.VersaoOriginal.Count(); i++) {
                numerosVersoesFeitas[i] = (HistoricoPiloto.VersaoOriginal.ElementAt(i).numero.Value); 
            }
            Random randomNumber = new Random();
            int versaoAFazer = versoesOriginaisAtivas.ElementAt(randomNumber.Next(0, versoesOriginaisAtivas.Count())).numero.Value;            
            while (numerosVersoesFeitas.Contains(versaoAFazer))
            {                
               versaoAFazer = versoesOriginaisAtivas.ElementAt(randomNumber.Next(0, versoesOriginaisAtivas.Count())).numero.Value;
            }
            avaliacao.dthrProvaInicio = DateTime.Now;
            db.Avaliacao.Add(avaliacao);            
            db.SaveChanges();
            return View("AvaliacaoIniciada");
        }

        [ChildActionOnly]
        [Authorize(Roles = "Avaliador")]
        [HttpPost]
        public ActionResult EncerrarAvaliacao(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = db.Avaliacao.Find(id);
            if (avaliacao == null)
            {
                return HttpNotFound();
            }
            return View(avaliacao);
        }

        [HttpPost]
        public ActionResult EncerrarAvaliacao([Bind(Include = "idAvaliacao")]Avaliacao avaliacao, bool sucesso, string observacoes)
        {
            avaliacao = db.Avaliacao.Find(avaliacao.idAvaliacao);
            if (sucesso == true)
            {
                
                avaliacao.status = 3;
                avaliacao.dthrProvaFim = DateTime.Now;
                avaliacao.sucesso = true;
                //avaliacao.observacoes = avaliacao.observacoes +"\r"+ observacoes;
                avaliacao.Horario.status = 8;
                db.Entry(avaliacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AvaliacaoEncerrada", avaliacao);
            }
            else
            {
                //avaliacao.observacoes = avaliacao.observacoes + "\r" + observacoes;
                avaliacao.status = 16;
                avaliacao.Horario.status = 8;
                db.Entry(avaliacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AvaliacaoEncerrada",avaliacao);
            }
        }


        public ActionResult AtribuirNota(int? id)
        {
            int idUsuario = User.Identity.GetUserId<int>();
            Avaliador avaliador = db.Avaliador.Find(idUsuario);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = db.Avaliacao.Find(id);
            if (avaliacao == null)
            {
                return HttpNotFound();
            }
            AtribuirNotaViewModel nota = new AtribuirNotaViewModel();
            nota.canacpiloto = avaliacao.Piloto.CANACPiloto;
            nota.empresaPiloto = avaliacao.Piloto.Empresa.nome;
            nota.entidade = avaliacao.LocalEntidade.Entidade.nome;
            if (avaliador.Id == avaliacao.Horario.idEle)
            {
                nota.ele = avaliador.UserPessoa.nome;
            }
            if (avaliador.Id == avaliacao.Horario.idSme)
            {
                nota.sme = avaliador.UserPessoa.nome;
            }
            nota.dataProva = avaliacao.dthrProvaInicio.Value.Date;

            List<NivelSDEA> niveis = new List<NivelSDEA>()
            {
                new NivelSDEA() {nivelAtual = 0 ,descricao ="Sem nível" },
                new NivelSDEA() {nivelAtual = 1 ,descricao ="Nível 1" },
                new NivelSDEA() {nivelAtual = 2 ,descricao ="Nível 2" },
                new NivelSDEA() {nivelAtual = 3 ,descricao ="Nível 3" },
                new NivelSDEA() {nivelAtual = 4 ,descricao ="Nível 4" },
                new NivelSDEA() {nivelAtual = 5 ,descricao ="Nível 5" },
                new NivelSDEA() {nivelAtual = 7 ,descricao ="Nível 5+" }
            };
            ViewBag.nivel = new SelectList(niveis, "nivelAtual", "descricao");

            return View(nota);
            
        }

        [HttpPost]
        public ActionResult AtribuirNota(AtribuirNotaViewModel nota)
        {

            return View();
        }


        public ActionResult AvaliacoesEmProgresso()
        {
            int idEntidade = User.Identity.GetUserId<int>();
            var avaliacoes = from a in db.Avaliacao where a.LocalEntidade.idEntidade == idEntidade && a.status <= 5 select a;
            
            return View(avaliacoes.ToList());        
        }



        public ActionResult DetalhesAvaliacao(int? id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = db.Avaliacao.Find(id);
            if (avaliacao == null)
            {
                return HttpNotFound();
            }
            if (avaliacao.status > 2)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(avaliacao);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
