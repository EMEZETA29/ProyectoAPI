﻿

using System.ComponentModel.DataAnnotations;

namespace ProyectoAPI.Modelos.Dto
{
    public class VehiculoUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(6)]
        public string Patente { get; set; }

        [Required]
        public int ModeloId { get; set; }

        [Required]
        public int MarcaId { get; set; }

        [MaxLength(50)]
        public string? Color { get; set; }

        [Required]
        public int CarroceriaId { get; set; }
    }
}
