using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProyectoAPI.Datos;
using ProyectoAPI.Modelos;
using ProyectoAPI.Modelos.Dto;
using ProyectoAPI.Repositorio.IRepositorio;
using System.Net;

namespace ProyectoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectoController : ControllerBase
    {
        private readonly ILogger<ProyectoController> _logger;
        private readonly IVehiculoRepositorio _vehiculoRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public ProyectoController(ILogger<ProyectoController> logger, IVehiculoRepositorio vehiculoRepo, IMapper mapper)
        {
            _logger = logger;
            _vehiculoRepo = vehiculoRepo;
            _mapper = mapper;
            _response = new();
        }
        
                
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVehiculos()
        {
            try
            {
                _logger.LogInformation("Obtener Registro de Vehiculos");
                IEnumerable<Vehiculo> vehiculoList = await _vehiculoRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<VehiculoDto>>(vehiculoList);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;
           

        }

        [HttpGet("id", Name = "GetVehiculo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> GetVehiculo(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer registro con Id " + id);
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                //var vehiculo = VehiculoStore.vehiculoList.FirstOrDefault(v => v.Id == id);
                var vehiculo = await _vehiculoRepo.Obtener(v => v.Id == id); ;

                if (vehiculo == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<VehiculoDto>(vehiculo);
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

                
            }
            return _response;
            
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearVehiculo([FromBody] VehiculoCrearDto crearDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _vehiculoRepo.Obtener(v => v.Patente.ToLower() == crearDto.Patente.ToLower()) != null)
                {
                    ModelState.AddModelError("PatenteExiste", "La Patente que quiere ingresar ya existe");
                    return BadRequest(ModelState);
                }

                if (crearDto == null)
                {
                    return BadRequest(crearDto);
                }

                if (!string.IsNullOrEmpty(crearDto.Color) && !char.IsUpper(crearDto.Color[0]))
                {
                    ModelState.AddModelError("PrimeraLetraColor", "La primera letra para color debe ser mayúscula.");
                    return BadRequest(ModelState);
                }

                Vehiculo modelo = _mapper.Map<Vehiculo>(crearDto);


                await _vehiculoRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVehiculo", new { id = modelo.Id }, _response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var vehiculo = await _vehiculoRepo.Obtener(v => v.Id == id);

                if (vehiculo == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _vehiculoRepo.Remover(vehiculo);
                _response.statusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
            
        }

        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVehiculo(int id, [FromBody] VehiculoUpdateDto updateDto)
        {
            if(updateDto == null || id!= updateDto.Id)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
  
            Vehiculo modelo = _mapper.Map<Vehiculo>(updateDto);

            await _vehiculoRepo.Actualizar(modelo);
            _response.statusCode = HttpStatusCode.NoContent;
            
            return Ok(_response);

            
        }

        [HttpPatch("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVehiculo(int id, JsonPatchDocument<VehiculoUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var vehiculo = await _vehiculoRepo.Obtener(v => v.Id == id, tracked:false);

            VehiculoUpdateDto vehiculoDto = _mapper.Map<VehiculoUpdateDto>(vehiculo);

           

            if (vehiculo ==null) return BadRequest();

            patchDto.ApplyTo(vehiculoDto, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Vehiculo modelo = _mapper.Map<Vehiculo>(vehiculoDto);
            
            await _vehiculoRepo.Actualizar(modelo);
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);

        }
    }

}
            
            
            
            
            
            
//        private readonly ILogger<ProyectoController> _logger;
//        public ProyectoController(ILogger<ProyectoController> logger)
//        {
//            _logger = logger;
//        }

//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public ActionResult<IEnumerable<VehiculoDto>> GetVehiculos()
//        {
//            _logger.LogInformation("Obtener Registros");
//            return Ok(VehiculoStore.vehiculoList);
//        }
           

//        [HttpPost]
//        public ActionResult<VehiculoDto> CrearVehiculo([FromBody] VehiculoDto vehiculoDto)
//        {
//            if(ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//            if(vehiculoDto == null)
//            {
//                return BadRequest(vehiculoDto);
//            }
//            if (vehiculoDto.Id > 0)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//            vehiculoDto.Id = VehiculoStore.vehiculoList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
//            VehiculoStore.vehiculoList.Add(vehiculoDto);
            
//            return Ok(vehiculoDto);
//        }
//    }
//}
