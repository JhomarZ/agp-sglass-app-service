using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace AGP.Snowden.DataAccessLayer.SAP
{
    [Table("PP_TRACKNG_IMPUT", Schema = "SAP")]
    public partial class PP_TRACKNG_IMPUT
    {

        public long Id { get; set; }

        [StringLength(30)]
        public string DocumentoCompra { get; set; }

        [StringLength(30)]
        public string NroPosicionDC { get; set; }
        [StringLength(30)]
        public string TipoImputacion { get; set; }

        [StringLength(30)]
        public string Centro { get; set; }
        [StringLength(30)]
        public string NumeroMaterial { get; set; }
        [StringLength(300)]
        public string DescripcionMaterial { get; set; }
        [StringLength(30)]
        public string CntPedido { get; set; }
        [StringLength(30)]
        public string UMB { get; set; }
        [StringLength(150)]
        public string Solicitante { get; set; }
        [StringLength(150)]
        public string Responsable { get; set; }
        [StringLength(30)]
        public string FechaCreacion { get; set; }
        [StringLength(30)]
        public string HoraRegistrada { get; set; }
        [StringLength(30)]
        public string NroCuentaProveedor { get; set; }
        [StringLength(150)]
        public string Responsable_ERNAM_EKKO { get; set; }
        [StringLength(30)]
        public string NroDocumentoComercial { get; set; }
        [StringLength(30)]
        public string GrupoArticulos { get; set; }
        [StringLength(30)]
        public string FechaInicio { get; set; }
        [StringLength(30)]
        public string FechaFin { get; set; }

        [StringLength(30)]
        public string MBLNR { get; set; }

        [StringLength(200)]
        public string NOM_COMPLETO { get; set; }


        [StringLength(200)]
        public string NOM_COMPLETO_EBAN { get; set; }

        [StringLength(150)]
        public string ResponsableIngreso { get; set; }


        [StringLength(300)]
        public string ResponsableIngresoFullName { get; set; }


        [NotMapped]
        public TrackingImputadosExtension Extension { get; set; }
    }
}
