﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Negocio.Dominio;

namespace login_v6.Controllers
{
    [Authorize]
    public class InsumoController : Controller
    {
        public insumo_dominio insu = new insumo_dominio();
        public detalle_insumo_impresora_dominio deta = new detalle_insumo_impresora_dominio();
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult AgregarInsumo(int id)
        {
            ViewBag.id_ubicacion_impresora = id;
            ViewBag.insumos = insu.ListarSinCantidad();
            ViewBag.insu = insu.Listar();

            return View(new detalle_insumo_impresora());
        }
        [HttpPost]
        public ActionResult Guardar_detalle_insumo_impresora(detalle_insumo_impresora model)
        {
            deta.Guardar(model);

            int insu = Convert.ToInt32(model.insumo_id);

            deta.Restar(insu);

            return View();
        }


        public ActionResult AgregarInsumoPC(int id)
        {
            insumo_pc_dominio insu = new insumo_pc_dominio();

            ViewBag.id_detalle = id;
            ViewBag.insumos = insu.ListarSinCantidad();


            return View(new pc_insumo_pc());
        }


        public ActionResult Guardar_insumo_pc(pc_insumo_pc model)
        {
            pc_insumo_pc_dominio insu = new pc_insumo_pc_dominio();
            insumo_pc_dominio insumo = new insumo_pc_dominio();

            insu.Guardar(model);
            insumo.Restar(Convert.ToInt32(model.insumo_id));
            return View();
        }
    }
}